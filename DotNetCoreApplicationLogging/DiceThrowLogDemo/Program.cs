using System;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Tracing;
using DiceThrowLibrary.Logging;

namespace DataVisualizationDotNetCore
{
    /// <summary>
    /// As created by Visual Studio, the Program class is static but for the Microsoft.Extensions.Logging
    /// logger to work correctly, the Program class must not be static. Changing Program to non-static is perfectly fine.
    /// </summary>
    class Program
    {
        // the Microsoft.Extensions.Logging (MEL) logger for this particular Program element.
        // Each Form will have one of these and will create it 
        // in its constructor.
        //static private ILogger iMELLogger;

        // the MEL loggerFactory for all Forms, Program, etc. Each
        // Form creates its own logger using this static factory
        //static public ILoggerFactory iMELLoggerFactory;

        // the Event Tracing for Windows ETW Listener that receives
        // ETW event messages from the DiceThrowLibrary
        //private static EventListenerStub _etwListener;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // to support logging of unhandled exceptions from application and libraries
            Application.ThreadException += Application_ThreadException;

            // System.Diagnostics.Tracing.Eventlistener cannot be created directly,
            // instead a derived class must be created.
            //_etwListener = new EventListenerStub();

            // enable listening to the event source in the library.
           // _etwListener.EnableEvents(LogEventSource.Instance, EventLevel.LogAlways);

            // Create the logger factory. This is where we add new MEL logging providers
            //iMELLoggerFactory = LoggerFactory.Create(builder =>
            //{
            //    builder
            //        .AddDebug()
            //        .AddEventLog();
            //});

            // create the local logger instance for the Program class.  One of these
            // will be created for each Form or other class that does MEL logging
            //iMELLogger = iMELLoggerFactory.CreateLogger<Program>();

            // assign the iMELLogger for the Program class to the ETW listener so
            // we can log ETW event messages as well as application events
            //_etwListener.iMELLogger = iMELLogger;

            // log our first MEL message to all the logging providers
            //iMELLogger.LogInformation("DiceThrowLogDemo started, loggerFactory set up.");


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        /// <summary>
        /// Unhandled exceptions from the application and any linked .Net library bubble up to this handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            // This will contain the exception message and stack trace. If
            // .pdb files are available, it will also have the line number.
            string logMessage = e.Exception.ToString();

            // write to the Microsoft.Extensions.Logging Program logger;
            //if (iMELLogger != null)
            //{
            //    iMELLogger.LogError(logMessage);
            //}

            DiceThrowLibrary.Logging.Logger.Error(logMessage);
        }

        /// <summary>
        /// We must dispose of the ETW listener when Program ends because the ETW EventSource in DiceThrowLibrary
        /// uses unsafe code as ETW is a Win32 framework.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            // we dispose in all cases
            //_etwListener.Dispose();
        }

    }

    // Receives ETW logging messages from the library. You cannot use EventListener directly,
    // you must use a derived class, even if it is just a stub.
    sealed public class EventListenerStub : EventListener
    {
        // the Microsoft.Extensions.Logging (MEL) ILogger instance to use when
        // logging events to the MEL logging providers
        public ILogger iMELLogger
        {
            get;
            set;
        }

        /// <summary>
        /// This receives data from the LogEventSource present in DiceThrowLibrary
        /// </summary>
        /// <param name="eventData"></param>
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            var outputMessage = $@"{eventData.TimeStamp:u} | {eventData.OSThreadId} | {eventData.Level}";

            outputMessage
                = $"{outputMessage} | {(eventData.Payload != null ? eventData.Payload[0] : "Missing message payload.")}";

            if (eventData.Keywords.HasFlag(Keywords.ExceptionKeyword))
            {
                outputMessage = $@"{outputMessage}{Environment.NewLine}    {
                    (eventData.Payload != null ? eventData.Payload[4] : "Missing exception payload.")}";
            }

            // write the ETW event message to the Microsoft.Extensions.Logging framework
            if (iMELLogger != null)
            {
                switch (eventData.Level)
                {
                    case EventLevel.Critical:
                        {
                            iMELLogger.LogCritical(outputMessage);
                            break;
                        }
                    case EventLevel.Error:
                        {
                            iMELLogger.LogError(outputMessage);
                            break;
                        }
                    case EventLevel.Warning:
                        {
                            iMELLogger.LogWarning(outputMessage);
                            break;
                        }
                    case EventLevel.LogAlways:
                    case EventLevel.Informational:
                    case EventLevel.Verbose:
                        {
                            iMELLogger.LogInformation(outputMessage);
                            break;
                        }
                }

            }

            base.OnEventWritten(eventData);
        }
    }
}
