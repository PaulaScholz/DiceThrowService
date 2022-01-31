/* CommonUtil.cs
 * Description: A static class for retrieving configuration information.
 * Author: Paula Scholz
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiceThrowService
{
    /// <summary>
    /// These methods return information from App.Config and are only executed once per session.
    /// </summary>
    static class CommonUtil
    {
        /// <summary>
        /// Return the name of the service from the App.Config file.
        /// </summary>
        /// <returns>String with the Service Name</returns>
        public static string GetServiceName()
        {
            try
            {
                Assembly? executingAssembly = Assembly.GetAssembly(typeof(DiceThrowMonitoringService));

                string? targetDir = executingAssembly?.Location;

                Configuration? config = ConfigurationManager.OpenExeConfiguration(targetDir);

                string? serviceName = config?.AppSettings.Settings["ServiceName"].Value.ToString();

                if (serviceName is not null) return serviceName;

                return String.Empty;
            }
            catch (Exception)
            {
                return "DiceThrow Monitoring Service";
            }
        }

        /// <summary>
        /// Return the InstrumentationKey of ApplicationInsights from the App.Config file.
        /// </summary>
        /// <returns>String with the AppInsights Key</returns>
        public static string GetAppInsightsKey()
        {
            try
            {
                Configuration? config = GetConfiguration();

                string? appKey = config?.AppSettings.Settings["AppInsightsInstrumentationKey"].Value.ToString();

                if (appKey is not null) return appKey;

                return String.Empty;
            }
            catch (Exception)
            {
                return "2b8ce440-680d-48b8-8d65-a9ffbfeacadd";
            }
        }

        /// <summary>
        /// Return the name of the ETW Provider from the App.Config file.
        /// </summary>
        /// <returns>String with the ETW Provider Name</returns>
        public static string GetEtwProviderName()
        {
            try
            {
                Configuration? config = GetConfiguration();

                string? providerName = config?.AppSettings.Settings["EtwProviderName"].Value.ToString();

                if (providerName is not null) return providerName;

                return String.Empty;
            }
            catch (Exception)
            {
                return "DiceThrowLog";
            }
        }

        /// <summary>
        /// Return the name of the Trace Event Session from the App.Config file.
        /// </summary>
        /// <returns>String with the Trace Event Session Name</returns>
        public static string GetTraceEventSessionName()
        {
            try
            {
                Configuration? config = GetConfiguration();

                string? traceEventSessionName = config?.AppSettings.Settings["TraceEventSessionName"].Value.ToString();

                if(traceEventSessionName is not null) return traceEventSessionName;

                return string.Empty;    
            }
            catch (Exception)
            {
                return "DiceThrow Monitor";
            }
        }

        /// <summary>
        /// Return the value of the AllEventsFlag from the App.Config file. If True,
        /// the service will log all ETW events from the provider specified by EtwProviderName
        /// </summary>
        /// <returns>True or False boolean value</returns>
        public static bool GetAllEventsFlag()
        {
            try
            {
                Configuration? config = GetConfiguration();

                string? sFlag = config?.AppSettings.Settings["AllEventsFlag"].Value.ToString();

                if(sFlag is not null) return bool.Parse(sFlag);

                return false;

            }
            catch (Exception)
            {
                return false;
            }
        }

        private static Configuration GetConfiguration()
        {
            Assembly? executingAssembly = Assembly.GetAssembly(typeof(DiceThrowMonitoringService));

            string? targetDir = executingAssembly?.Location;

            Configuration? config = ConfigurationManager.OpenExeConfiguration(targetDir);

            return config;
        }
    }
}
