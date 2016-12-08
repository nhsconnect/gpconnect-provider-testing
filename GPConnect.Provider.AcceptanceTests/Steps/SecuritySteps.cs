using System;
using System.Configuration;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using GPConnect.Provider.AcceptanceTests.Helpers;
using Shouldly;
using TechTalk.SpecFlow;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Global

namespace GPConnect.Provider.AcceptanceTests.Steps
{

    [Binding]
    public class SecuritySteps : TechTalk.SpecFlow.Steps
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

        private bool SendClientCert => _scenarioContext.Get<bool>(Context.SendClientCert);
        private bool ValidateServerCert => _scenarioContext.Get<bool>(Context.ValidateServerCert);
        private string ClientCertThumbPrint => _scenarioContext.Get<string>(Context.ClientCertThumbPrint);
        private X509Certificate2 ClientCert => _scenarioContext.Get<X509Certificate2>(Context.ClientCertificate);
        private HttpStatusCode ResponseStatusCode => _scenarioContext.Get<HttpStatusCode>(HttpSteps.Context.ResponseStatusCode);

        public SecuritySteps(ScenarioContext scenarioContext)
        {
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

        // Security Validation Steps

        [Then(@"the response status code should indicate authentication failure")]
        public void ThenTheResponseStatusCodeShouldIndicateAuthenticationFailure()
        {
            ResponseStatusCode.ShouldBe(HttpStatusCode.Forbidden);
            Console.WriteLine("Response HttpStatusCode={0}", ResponseStatusCode);
        }

        [Then(@"the response status code should be ""(.*)""")]
        public void ThenTheResponseStatusCodeShouldBe(string statusCode)
        {
            ResponseStatusCode.ToString().ShouldBe(statusCode);
            Console.WriteLine("Response HttpStatusCode should be {0} but was {1}", statusCode, ResponseStatusCode);
        }
    }
}
