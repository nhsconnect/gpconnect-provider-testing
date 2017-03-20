using System.ComponentModel;
using System.Configuration;

namespace GPConnect.Provider.AcceptanceTests.Helpers
{
    public static class AppSettingsHelper
    {
        // Trace Log Settings
        public static bool TraceAllScenarios => Get<bool>("traceAllScenarios");
        public static string TraceBaseDirectory => Get<string>("traceBaseDirectory");

        // Data Settings
        public static string DataDirectory => Get<string>("dataDirectory");

        // FHIR Settings
        public static string FhirDirectory => Get<string>("fhirDirectory");
        
        // Security Settings
        public static bool UseTLS => Get<bool>("useTLS");

        // Server Settings
        public static string ServerUrl => Get<string>("serverUrl");
        public static string ServerPort => Get<string>("serverPort");
        public static string ServerBase => Get<string>("serverBase");

        // Web Proxy Settings
        public static bool UseWebProxy => Get<bool>("useWebProxy");
        public static string WebProxyUrl => Get<string>("webProxyUrl");
        public static string WebProxyPort => Get<string>("webProxyPort");

        // Spine Proxy Settings
        public static bool UseSpineProxy => Get<bool>("useSpineProxy");
        public static string SpineProxyUrl => Get<string>("spineProxyUrl");
        public static string SpineProxyPort => Get<string>("spineProxyPort");

        // Certificate Settings
        public static string ClientCertThumbPrint => Get<string>("clientCertThumbPrint");
        public static string ClientInvalidCertThumbPrint => Get<string>("clientInvalidCertThumbPrint");
        public static string ClientExpiredCertThumbPrint => Get<string>("clientExpiredCertThumbPrint");
        public static bool SendClientCert => Get<bool>("sendClientCert");
        public static bool ValidateServerCert => Get<bool>("validateServerCert");

        // Consumer Settings
        public static string ConsumerASID => Get<string>("consumerASID");

        // Provider Settings
        public static string ProviderASID => Get<string>("providerASID");

        private static T Get<T>(string key)
        {
            var appSetting = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(appSetting)) throw new ConfigurationErrorsException($"AppSettings Key='{key}' Not Found.");
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)(converter.ConvertFromInvariantString(appSetting));
        }
    }
}
