using System.ComponentModel;
using System.Configuration;
using GPConnect.Provider.AcceptanceTests.Logger;
using GPConnect.Provider.AcceptanceTests.Steps;
using TechTalk.SpecFlow;

namespace GPConnect.Provider.AcceptanceTests.Helpers
{
    public static class AppSettingsHelper
    {
        // Trace Log Settings
        public static bool TraceAllScenarios => Get<bool>("traceAllScenarios");
        public static string TraceBaseDirectory => Get<string>("traceBaseDirectory");

        // Security Settings
        public static bool UseTLS => Get<bool>("useTLS");

        // FHIR Server Settings
        public static string FhirServerUrl => Get<string>("fhirServerUrl");
        public static string FhirServerPort => Get<string>("fhirServerPort");
        public static string FhirServerFhirBase => Get<string>("fhirServerFhirBase");

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
        public static string TestSuitASID => Get<string>("consumerASID");

        // Provider Settings
        public static string ProviderSystemASID => Get<string>("providerASID");

        public static T Get<T>(string key)
        {
            var appSetting = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(appSetting)) throw new ConfigurationErrorsException($"AppSettings Key='{key}' Not Found.");
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)(converter.ConvertFromInvariantString(appSetting));
        }

        public static void LoadAppSettings(ScenarioContext scenarioContext)
        {
            Log.WriteLine("LoadAppSettings From App.Config");

            // Security Details from config file
            scenarioContext.Set(UseTLS, SecuritySteps.Context.UseTLS);
            Log.WriteLine("UseTLS=" + UseTLS);

            // Fhir Server Details from config file
            scenarioContext.Set(FhirServerUrl, HttpSteps.Context.FhirServerUrl);
            Log.WriteLine("FhirServerUrl=" + FhirServerUrl);
            scenarioContext.Set(FhirServerPort, HttpSteps.Context.FhirServerPort);
            Log.WriteLine("FhirServerPort=" + FhirServerPort);
            scenarioContext.Set(FhirServerFhirBase, HttpSteps.Context.FhirServerFhirBase);
            Log.WriteLine("FhirServerFhirBase=" + FhirServerFhirBase);

            // Web Proxy Details from config file
            scenarioContext.Set(UseWebProxy, HttpSteps.Context.UseWebProxy);
            Log.WriteLine("UseWebProxy=" + UseWebProxy);
            scenarioContext.Set(WebProxyUrl, HttpSteps.Context.WebProxyUrl);
            Log.WriteLine("WebProxyUrl=" + WebProxyUrl);
            scenarioContext.Set(WebProxyPort, HttpSteps.Context.WebProxyPort);
            Log.WriteLine("WebProxyPort=" + WebProxyPort);

            // Spine Proxy Details from config file
            scenarioContext.Set(UseSpineProxy, HttpSteps.Context.UseSpineProxy);
            Log.WriteLine("UseSpineProxy=" + UseSpineProxy);
            scenarioContext.Set(SpineProxyUrl, HttpSteps.Context.SpineProxyUrl);
            Log.WriteLine("SpineProxyUrl=" + SpineProxyUrl);
            scenarioContext.Set(SpineProxyPort, HttpSteps.Context.SpineProxyPort);
            Log.WriteLine("SpineProxyPort=" + SpineProxyPort);

            // Certificates from config file
            scenarioContext.Set(ClientCertThumbPrint, SecuritySteps.Context.ClientCertThumbPrint);
            Log.WriteLine("ClientCertThumbPrint=" + ClientCertThumbPrint);
            scenarioContext.Set(SendClientCert, SecuritySteps.Context.SendClientCert);
            Log.WriteLine("SendClientCert=" + SendClientCert);
            scenarioContext.Set(ValidateServerCert, SecuritySteps.Context.ValidateServerCert);
            Log.WriteLine("ValidateServerCert=" + ValidateServerCert);

            // Provider from config file
            scenarioContext.Set(ProviderSystemASID, HttpSteps.Context.ProviderASID);
            Log.WriteLine("ProviderSystemASID=" + ProviderSystemASID);

            // Consumer from config file
            scenarioContext.Set(TestSuitASID, HttpSteps.Context.ConsumerASID);
            Log.WriteLine("TestSuitASID=" + TestSuitASID);
        }
    }
}
