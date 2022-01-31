using System.Diagnostics;
using System.ServiceProcess;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace DiceThrowService
{
    internal class DiceThrowMonitoringService : ServiceBase
    {
        // the background task that monitors an ETW provider
        private MonitorTask? MonTask;

        // the name or GUID of the ETW Provider you wish to monitor
        public readonly string EtwProviderName;

        // An Azure ApplicationInsights Telemetry Client for sending events
        // to Azure ApplicationInsights, created in constructor.
        public TelemetryClient ServiceTelemetryClient;


        public DiceThrowMonitoringService()
        {
            // get the ApplicationInsights instrumentation key from App.config
            string key = CommonUtil.GetAppInsightsKey();

            // set up ApplicationInsights
            TelemetryConfiguration configuration = TelemetryConfiguration.CreateDefault();
            configuration.InstrumentationKey = key;

            ServiceTelemetryClient = new TelemetryClient(configuration);

            // This ServiceName is used in the EventLog
            this.ServiceName = CommonUtil.GetServiceName();

            // Get the ETW Provider Name for monitoring
            EtwProviderName = CommonUtil.GetEtwProviderName();

            // Tell ApplicationInsights the service is created.
            ServiceTelemetryClient.TrackTrace(string.Format("{0} created for {1}.", ServiceName, EtwProviderName));

            // Enable auto-logging of state changes
            this.AutoLog = true;

            // put logs in the Application category in EventViewer
            this.EventLog.Log = "Application";
        }

        /// Called when the service is started.
        /// </summary>
        /// <param name="args"></param>
        protected override async void OnStart(string[] args)
        {
            base.OnStart(args);

            // start the MonitorTask
            try
            {
                MonTask = new MonitorTask(this);

                // start monitoring
                await Task.Run(() => MonTask.StartTask());
            }
            catch
            {
                Debug.WriteLine("A MonitorTask error occured in the DiceThrowService.");
            }
        }

        /// <summary>
        /// Write an entry to the event log.  Called from a MonitorTask.
        /// </summary>
        /// <param name="text"></param>
        public void WriteEvent(string text)
        {

            // Write all the events from the provider to the log. You could gate this
            // with the AllEventsFlag if desired.
            if (text.ToLower().Contains("error"))
            {
                EventLog.WriteEntry(text, EventLogEntryType.Error);
            }
            else
            {
                EventLog.WriteEntry(text);
            }           

            // send event to ApplicationInsights
            ServiceTelemetryClient.TrackTrace(text);
        }

        /// <summary>
        /// Stop the service.
        /// </summary>
        protected override void OnStop()
        {
            base.OnStop();

            // Cancel monitoring, destroy the TraceEventSession
            MonTask?.CancelTask();
            MonTask = null;

            // before exit, flush the remaining telemetry data
            ServiceTelemetryClient.Flush();

            // flush is not blocking when not using InMemoryChannel so wait a bit. There is an active issue regarding the need for `Sleep`/`Delay`
            // which is tracked here: https://github.com/microsoft/ApplicationInsights-dotnet/issues/407
            Task.Delay(5000).Wait();
        }
    }
}
