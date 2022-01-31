using Microsoft.Diagnostics.Tracing.Session;
using System.Threading.Tasks;

namespace DiceThrowService
{
    internal class MonitorTask
    {
        private TraceEventSession? Session;
        private readonly DiceThrowMonitoringService? Parent;

        public MonitorTask(DiceThrowMonitoringService parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Start the task, it is async so the service doesn't hang in OnStart.
        /// </summary>
        /// <returns>Task</returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task StartTask()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            Session = new TraceEventSession(CommonUtil.GetTraceEventSessionName());

            // subscribe to all events in the source through
            // the default DynamicTraceEventParser
            Session.Source.Dynamic.All += Source_AllEvents;

            // turn on the ETW Provider
            Session?.EnableProvider(Parent?.EtwProviderName);

            // process ETW events forever until cancelled in CancelTask
            Session?.Source.Process();
        }

        /// <summary>
        /// Event handler to write the event data to the parent class.
        /// </summary>
        /// <param name="obj">The TraceEvent object from ETW</param>
        private void Source_AllEvents(Microsoft.Diagnostics.Tracing.TraceEvent obj)
        {

            Parent?.WriteEvent(obj.ToString());
        }

        /// <summary>
        /// Disable the ETW provider, stop the session and destroy it.
        /// </summary>
        public void CancelTask()
        {
            // turn off the ETW provider
            Session?.DisableProvider(Parent?.EtwProviderName);

            // kill the session
            Session?.Stop();
            Session?.Dispose();
            Session = null;
        }
    }
}
