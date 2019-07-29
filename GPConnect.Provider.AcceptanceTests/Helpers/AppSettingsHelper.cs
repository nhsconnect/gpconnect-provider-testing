using System.ComponentModel;
using System.Configuration;

namespace GPConnect.Provider.AcceptanceTests.Helpers
{
    using TechTalk.SpecFlow;

    public static class AppSettingsHelper
    {
        // Trace Log Settings
        public static bool TraceAllScenarios => Get<bool>("traceAllScenarios");
        public static string TraceBaseDirectory => Get<string>("traceBaseDirectory");
        public static bool TraceOutputJSONResponse => Get<bool>("traceOutputJSONResponse");
        public static bool TraceOutputJWT => Get<bool>("traceOutputJWT");
        public static bool TraceOutputJSONRequestBody => Get<bool>("traceOutputJSONRequestBody");

        // Data Settings
        public static string DataDirectory => Get<string>("dataDirectory");

        // FHIR Settings
        public static string GPConnectSpecVersion => Get<string>("gpConnectSpecVersion");
        public static string FhirDirectory => Get<string>("fhirDirectory");
        public static string FhirWebDirectory => Get<string>("fhirWebDirectory");
        public static bool FhirCheckWeb => Get<bool>("fhirCheckWeb");
        public static bool FhirCheckDisk => Get<bool>("fhirCheckDisk");
        public static bool FhirCheckWebFirst => Get<bool>("fhirCheckWebFirst");

        // Security Settings
        public static bool UseTLS => Get<bool>("useTLS");

        // Server Settings
        public static string ServerUrl => Get<string>("serverUrl");
        public static string ServerHttpsPort => Get<string>("serverHttpsPort");
        public static string ServerHttpPort => Get<string>("serverHttpPort");
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
        public static bool SendClientCert => Get<bool>("sendClientCert");
        public static bool ValidateServerCert => Get<bool>("validateServerCert");

        //Certificates to imitate the Consumer calling the SSP
        public static string ThumbprintConsumerValid => Get<string>("Thumbprint:Consumer:Valid");
        public static string ThumbprintConsumerInvalidFqdn => Get<string>("Thumbprint:Consumer:Invalid:Fqdn");
        public static string ThumbprintConsumerInvalidAuthority => Get<string>("Thumbprint:Consumer:Invalid:Authority");
        public static string ThumbprintConsumerInvalidRevoked => Get<string>("Thumbprint:Consumer:Invalid:Revoked");
        public static string ThumbprintConsumerInvalidExpired => Get<string>("Thumbprint:Consumer:Invalid:Expired");

        //Certificates to imitate the SSP calling the Provider
        public static string ThumbprintSspValid => Get<string>("Thumbprint:Ssp:Valid");

        public static string ThumbprintSspInvalidExpired => Get<string>("Thumbprint:Ssp:Invalid:Expired");

        public static string ThumbprintSspInvalidFqdn => Get<string>("Thumbprint:Ssp:Invalid:Fqdn");

        public static string ThumbprintSspInvalidAuthority => Get<string>("Thumbprint:Ssp:Invalid:Authority");

        public static string ThumbprintSspInvalidRevoked => Get<string>("Thumbprint:Ssp:Invalid:Revoked");

        public static string CurlClientCertificate => Get<string>($"Curl:{CurlClientCertificateLocator}:Certificate");
        public static string CurlClientKey => Get<string>($"Curl:{CurlClientCertificateLocator}:Key");
        public static string CurlClientPassword => Get<string>($"Curl:{CurlClientCertificateLocator}:Password");

        private static string CurlClientCertificateLocator => FeatureContext.Current.FeatureInfo.Title == "ssp" ? "Consumer" : "Ssp";

        public static bool TeardownEnabled => Get<bool>("Teardown:Enabled");

        public static bool RandomPatientEnabled => Get<bool>("RandomPatient:Enabled");

        // Consumer Settings
        public static string ConsumerASID => Get<string>("consumerASID");

        // Provider Settings
        public static string ProviderASID => Get<string>("providerASID");

        public static string JwtAudValue => Get<string>("jwtAudValue");

        public static T Get<T>(string key)
        {
            var appSetting = ConfigurationManager.AppSettings[key];


            if (!(key.Equals("serverHttpPort") || key.Equals("serverHttpsPort")) && string.IsNullOrWhiteSpace(appSetting))
            {
                throw new ConfigurationErrorsException($"AppSettings Key='{key}' Not Found.");
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));

            return (T)converter.ConvertFromInvariantString(appSetting);
        }
    }
}
