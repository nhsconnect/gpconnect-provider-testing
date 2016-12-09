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
            public const string UseTLS = "useTLS";
            public const string ValidateServerCert = "validateServerCert";
            public const string SendClientCert = "sendClientCert";
            public const string ClientCertThumbPrint = "clientCertThumbPrint";
            public const string ClientCertificate = "clientCertificate";
        }

        // Security Details

        public bool UseTLS
        {
            get { return _scenarioContext.Get<bool>(Context.UseTLS); }
            set
            {
                Log.WriteLine("{0}={1}", Context.UseTLS, value);
                _scenarioContext.Set(value, Context.UseTLS);
            }
        }

        public bool ValidateServerCert
        {
            get { return _scenarioContext.Get<bool>(Context.ValidateServerCert); }
            set
            {
                Log.WriteLine("{0}={1}", Context.ValidateServerCert, value);
                _scenarioContext.Set(value, Context.ValidateServerCert);
            }
        }

        public bool SendClientCert
        {
            get { return _scenarioContext.Get<bool>(Context.SendClientCert); }
            set
            {
                Log.WriteLine("{0}={1}", Context.SendClientCert, value);
                _scenarioContext.Set(value, Context.SendClientCert);
            }
        }

        public string ClientCertThumbPrint
        {
            get { return _scenarioContext.Get<string>(Context.ClientCertThumbPrint); }
            set
            {
                Log.WriteLine("{0}={1}", Context.ClientCertThumbPrint, value);
                _scenarioContext.Set(value, Context.ClientCertThumbPrint);
            }
        }

        public X509Certificate2 ClientCert
        {
            get { return _scenarioContext.Get<X509Certificate2>(Context.ClientCertificate); }
            set
            {
                Log.WriteLine("{0}={1}", Context.ClientCertificate, value);
                _scenarioContext.Set(value, Context.ClientCertificate);
            }
        }

        public void LoadAppConfig()
        {
            Log.WriteLine("SecurityContext->LoadAppConfig()");
            UseTLS = AppSettingsHelper.UseTLS;
            ClientCertThumbPrint = AppSettingsHelper.ClientCertThumbPrint;
            SendClientCert = AppSettingsHelper.SendClientCert;
            ValidateServerCert = AppSettingsHelper.ValidateServerCert;
        }
    }
}
