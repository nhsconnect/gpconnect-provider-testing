using System.Security.Cryptography.X509Certificates;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Logger;
using TechTalk.SpecFlow;

// ReSharper disable ClassNeverInstantiated.Global

namespace GPConnect.Provider.AcceptanceTests.Context
{
    public interface ISecurityContext
    {
        bool UseTLS { get; set; }
        bool UseTLSFoundationsAndAppmts { get; set; }
        bool UseTLSStructured { get; set; }
        bool ValidateServerCert { get; set; }
        bool SendClientCert { get; set; }
        string ClientCertThumbPrint { get; set; }
        X509Certificate2 ClientCert { get; set; }
    }

    public class SecurityContext : ISecurityContext
    {
        private readonly ScenarioContext _scenarioContext;

        // Constructor
        public SecurityContext(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        private static class Context
        {
            public const string kUseTLS = "useTLS";
            public const string kUeTLSFoundationsAndAppmts = "useTLSFoundationsAndAppmts";
            public const string kUseTLSStructured = "useTLSStructured";
            public const string kValidateServerCert = "validateServerCert";
            public const string kSendClientCert = "sendClientCert";
            public const string kClientCertThumbPrint = "clientCertThumbPrint";
            public const string kClientCertificate = "clientCertificate";
        }

        // Security Details

        public bool UseTLS
        {
            get { return _scenarioContext.Get<bool>(Context.kUseTLS); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kUseTLS, value);
                _scenarioContext.Set(value, Context.kUseTLS);
            }
        }

        public bool UseTLSFoundationsAndAppmts
        {
            get { return _scenarioContext.Get<bool>(Context.kUeTLSFoundationsAndAppmts); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kUeTLSFoundationsAndAppmts, value);
                _scenarioContext.Set(value, Context.kUeTLSFoundationsAndAppmts);
            }
        }


        public bool UseTLSStructured
        {
            get { return _scenarioContext.Get<bool>(Context.kUseTLSStructured); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kUseTLSStructured, value);
                _scenarioContext.Set(value, Context.kUseTLSStructured);
            }
        }

        public bool ValidateServerCert
        {
            get { return _scenarioContext.Get<bool>(Context.kValidateServerCert); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kValidateServerCert, value);
                _scenarioContext.Set(value, Context.kValidateServerCert);
            }
        }

        public bool SendClientCert
        {
            get { return _scenarioContext.Get<bool>(Context.kSendClientCert); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kSendClientCert, value);
                _scenarioContext.Set(value, Context.kSendClientCert);
            }
        }

        public string ClientCertThumbPrint
        {
            get { return _scenarioContext.Get<string>(Context.kClientCertThumbPrint); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kClientCertThumbPrint, value);
                _scenarioContext.Set(value, Context.kClientCertThumbPrint);
            }
        }

        public X509Certificate2 ClientCert
        {
            get { return _scenarioContext.Get<X509Certificate2>(Context.kClientCertificate); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kClientCertificate, value);
                _scenarioContext.Set(value, Context.kClientCertificate);
            }
        }

        public string Cipher { get; set; }

        public void LoadAppConfig()
        {
            Log.WriteLine("SecurityContext->LoadAppConfig()");
            //UseTLS = AppSettingsHelper.UseTLS;
            UseTLS = true;
            UseTLSFoundationsAndAppmts = AppSettingsHelper.UseTLSFoundationsAndAppmts;
            UseTLSStructured = AppSettingsHelper.UseTLSStructured;
            ClientCertThumbPrint = AppSettingsHelper.ThumbprintSspValid;
            SendClientCert = AppSettingsHelper.SendClientCert;
            ValidateServerCert = AppSettingsHelper.ValidateServerCert;
        }
    }
}
