using System.Security.Cryptography.X509Certificates;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Logger;
using TechTalk.SpecFlow;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Global

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    public interface ISecuritySteps
    {
        bool SendClientCert { get; }
        bool ValidateServerCert { get; }
        string ClientCertThumbPrint { get; }
        X509Certificate2 ClientCert { get; }
    }

    [Binding]
    public class SecuritySteps : TechTalk.SpecFlow.Steps, ISecuritySteps
    {
        private readonly ScenarioContext _scenarioContext;

        internal static class Context
        {
            public const string UseTLS = "useTLS";
            public const string ValidateServerCert = "validateServerCert";
            public const string SendClientCert = "sendClientCert";
            public const string ClientCertThumbPrint = "clientCertThumbPrint";
            public const string ClientCertificate = "clientCertificate";
        }

        // Security Details

        public bool UseTLS => _scenarioContext.Get<bool>(Context.UseTLS);
        public bool ValidateServerCert => _scenarioContext.Get<bool>(Context.ValidateServerCert);
        public bool SendClientCert => _scenarioContext.Get<bool>(Context.SendClientCert);
        public string ClientCertThumbPrint => _scenarioContext.Get<string>(Context.ClientCertThumbPrint);
        public X509Certificate2 ClientCert => _scenarioContext.Get<X509Certificate2>(Context.ClientCertificate);

        // Constructor

        public SecuritySteps(ScenarioContext scenarioContext)
        {
            Log.WriteLine("SecuritySteps() Constructor");
            _scenarioContext = scenarioContext;
        }

        // Security Configuration Steps

        [Given(@"I do not want to verify the server certificate")]
        public void IDoNotWantToVerifyTheServerCertificate()
        {
            _scenarioContext.Set(false, Context.ValidateServerCert);
        }

        [Given(@"I do want to verify the server certificate")]
        public void IDoWantToVerifyTheServerCertificate()
        {
            _scenarioContext.Set(true, Context.ValidateServerCert);
        }

        [Given(@"I am not using a client certificate")]
        public void IAmNotUsingAClientCertificate()
        {
            _scenarioContext.Set(false, Context.SendClientCert);
        }

        [Given(@"I am using client certificate with thumbprint ""(.*)""")]
        public void IAmUsingClientCertificateWithThumbprint(string thumbPrint)
        {
            _scenarioContext.Set(thumbPrint, Context.ClientCertThumbPrint);
            _scenarioContext.Set(true, Context.SendClientCert);
            Given(@"I configure server certificate and ssl");
        }

        [Given(@"I am using an invalid client certificate")]
        public void IAmUsingAnInvalidClientCertificate()
        {
            _scenarioContext.Set(AppSettingsHelper.ClientInvalidCertThumbPrint, Context.ClientCertThumbPrint);
            _scenarioContext.Set(true, Context.SendClientCert);
            Given(@"I configure server certificate and ssl");
        }

        [Given(@"I am using an expired client certificate")]
        public void IAmUsingAnExpiredClientCertificate()
        {
            _scenarioContext.Set(AppSettingsHelper.ClientExpiredCertThumbPrint, Context.ClientCertThumbPrint);
            _scenarioContext.Set(true, Context.SendClientCert);
            Given(@"I configure server certificate and ssl");
        }

        [Given(@"I am using TLS Connection")]
        public void IAmUsingTLSConnection()
        {
            _scenarioContext.Set(true, Context.UseTLS);
        }

        [Given(@"I am not using TLS Connection")]
        public void IAmNotUsingTLSConnection()
        {
            _scenarioContext.Set(false, Context.UseTLS);
            Given(@"I do not want to verify the server certificate");
            And(@"I am not using a client certificate");
        }

        [Given(@"I configure server certificate and ssl")]
        public void IConfigureServerCertificatesAndSsl()
        {
            // Setup The Client Certificate
            if (SendClientCert)
            {
                _scenarioContext.Set(SecurityHelper.GetCertificateByClientThumbPrint(ClientCertThumbPrint), Context.ClientCertificate);
            }

            // Setup The Server Certificate Validation (If Required)
            if (ValidateServerCert)
            {
                SecurityHelper.ValidateServerCertificate();
            }
            else
            {
                SecurityHelper.DoNotValidateServerCertificate();
            }
        }
    }
}
