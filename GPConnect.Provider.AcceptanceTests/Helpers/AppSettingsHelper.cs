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
        public static bool TraceOutputConsoleLog => Get<bool>("traceOutputConsoleLog");

        // Data Settings
        public static string DataDirectory => Get<string>("dataDirectory");

        // FHIR Settings
        public static string GPConnectSpecVersionFoundationsAndAppmts => Get<string>("gpConnectSpecVersionFoundationsAndAppmts");
        public static string GPConnectSpecVersionStructured => Get<string>("gpConnectSpecVersionStructured");
        public static string GPConnectSpecVersionDocuments => Get<string>("gpConnectSpecVersionDocuments");

        public static string FhirDirectory => Get<string>("fhirDirectory");
        public static string FhirWebDirectory => Get<string>("fhirWebDirectory");
        public static bool FhirCheckWeb => Get<bool>("fhirCheckWeb");
        public static bool FhirCheckDisk => Get<bool>("fhirCheckDisk");
        public static bool FhirCheckWebFirst => Get<bool>("fhirCheckWebFirst");

        // Security Settings
        public static bool UseTLSFoundationsAndAppmts => Get<bool>("useTLSFoundationsAndAppmts");
        public static bool UseTLSStructured => Get<bool>("useTLSStructured");
        public static bool UseTLSDocuments => Get<bool>("useTlsDocuments");

        // Server Settings
        public static string ServerUrlFoundationsAndAppmts => Get<string>("serverUrlFoundationsAndAppmts");
        public static string ServerHttpsPortFoundationsAndAppmts => Get<string>("serverHttpsPortFoundationsAndAppmts");
        public static string ServerHttpPortFoundationsAndAppmts => Get<string>("serverHttpPortFoundationsAndAppmts");
        public static string ServerBaseFoundationsAndAppmts => Get<string>("serverBaseFoundationsAndAppmts");

        public static string ServerUrlStructured => Get<string>("serverUrlStructured");
        public static string ServerHttpsPortStructured => Get<string>("serverHttpsPortStructured");
        public static string ServerHttpPortStructured => Get<string>("serverHttpPortStructured");
        public static string ServerBaseStructured => Get<string>("serverBaseStructured");

        public static string ServerUrlDocuments => Get<string>("serverUrlDocuments");
        public static string ServerHttpsPortDocuments => Get<string>("serverHttpsPortDocuments");
        public static string ServerHttpPortDocuments => Get<string>("serverHttpPortDocuments");
        public static string ServerBaseDocuments => Get<string>("serverBaseDocuments");


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

        //public static string JwtAudValue => Get<string>("jwtAudValue");
        public static string JwtAudValueFoundationsAndAppmts => Get<string>("jwtAudValueFoundationsAndAppmts");
        public static string JwtAudValueStructured => Get<string>("jwtAudValueStructured");
        public static string JwtAudValueDocuments => Get<string>("jwtAudValueDocuments");


        public static T Get<T>(string key)
        {
            // Force ConfigurationManager to reload from the assembly config file
            var configFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location + ".config";

            // Open the config file explicitly
            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = configFilePath };
            var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            var appSetting = configuration.AppSettings.Settings[key]?.Value;

            if (!(key.Equals("serverHttpsPortFoundationsAndAppmts") || key.Equals("serverHttpPortFoundationsAndAppmts") || key.Equals("serverHttpsPortStructured") || key.Equals("serverHttpPortStructured") || key.Equals("serverHttpsPortDocuments") || key.Equals("serverHttpPortDocuments")) && string.IsNullOrWhiteSpace(appSetting))
            {
                throw new ConfigurationErrorsException($"AppSettings Key='{key}' Not Found.");
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromInvariantString(appSetting);
        }
    }
}
