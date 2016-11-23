using TechTalk.SpecFlow;
using GPConnect.Provider.AcceptanceTests.tools;
using System.Net;
using System;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using Shouldly;

namespace GPConnect.Provider.AcceptanceTests.Steps
{

    [Binding]
    public class Security : TechTalk.SpecFlow.Steps
    {
        private readonly ScenarioContext _scenarioContext;
        private HeaderController _headerController;
        private JwtHelper _jwtHelper;

        public Security(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _headerController = HeaderController.Instance;
            _jwtHelper = JwtHelper.Instance;
        }


        // JWT configuration steps

        [Given(@"I set the default JWT")]
        public void ISetTheDefaultJWT()
        {
            _jwtHelper.setJwtDefaultValues();
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenOrgResource());
        }

        [Given(@"I set the default JWT without base64 encoding")]
        public void ISetTheJWTWithoutBase64Encoding()
        {
            _jwtHelper.setJwtDefaultValues();
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenOrgResourceWithoutEncoding());
        }

        [Given(@"I set the JWT expiry time to ""(.*)"" seconds after creation time")]
        public void ISetTheJWTExpiryTimeToSecondsAfterCreationTime(double expirySeconds)
        {
            _jwtHelper.setJWTExpiryTimeInSeconds(expirySeconds);
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenOrgResource());
        }
        
        [Given(@"I set the JWT creation time to ""(.*)"" seconds after the current time")]
        public void ISetTheJWTCreationTimeToSecondsAfterTheCurrentTime(double secondsInFuture)
        {
            _jwtHelper.setJWTCreationTimeSeconds(secondsInFuture);
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenOrgResource());
        }
        
        [Given(@"I set the JWT reason for request to ""(.*)""")]
        public void ISetTheJWTReasonForRequestTo(string reasonForRequest)
        {
            _jwtHelper.setJWTReasonForRequest(reasonForRequest);
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenOrgResource());
        }


        // Certificate configruation steps

        [Given(@"I do not want to verify the server certificate")]
        public void IDoNotWantToVerifyTheServerCertificate()
        {
            _scenarioContext.Set(false, "validateServerCert");
        }

        [Given(@"I do want to verify the server certificate")]
        public void IDoWantToVerifyTheServerCertificate()
        {
            _scenarioContext.Set(true, "validateServerCert");
        }

        [Given(@"I am not using a client certificate")]
        public void IAmNotUsingAClientCertificate()
        {
            _scenarioContext.Set(false, "sendClientCert");
        }

        [Given(@"I am using client certificate with thumbprint ""(.*)""")]
        public void IAmUsingClientCertificateWithThumbprint(string thumbPrint)
        {
            _scenarioContext.Set(thumbPrint, "clientCertThumbPrint");
            _scenarioContext.Set(true, "sendClientCert");
            Console.WriteLine("client certificate thumb print");
            Given(@"I configure server certificate and ssl");
        }

        [Given(@"I am using TLS Connection")]
        public void IAmUsingTLSConnection()
        {
            _scenarioContext.Set(true, "useTLS");
        }

        [Given(@"I am not using TLS Connection")]
        public void IAmNotUsingTLSConnection()
        {
            _scenarioContext.Set(false, "useTLS");
            Given(@"I do not want to verify the server certificate");
            And(@"I am not using a client certificate");
        }

        [Given(@"I configure server certificate and ssl")]
        public void IConfigureServerCertificatesAndSsl() {

            // Client Certificate
            if (_scenarioContext.Get<bool>("sendClientCert")) {
                var thumbPrint = _scenarioContext.Get<string>("clientCertThumbPrint");
                var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                try
                {
                    store.Open(OpenFlags.ReadOnly);
                    var signingCert = store.Certificates.Find(X509FindType.FindByThumbprint, thumbPrint, false);
                    if (signingCert.Count == 0)
                    {
                        throw new FileNotFoundException(string.Format("Cert with thumbprint: '{0}' not found in local machine cert store.", thumbPrint));
                    }
                    Console.WriteLine("Certificate Found = " + signingCert[0]);
                    _scenarioContext.Set(signingCert[0], "clientCertificate");
                }
                finally
                {
                    store.Close();
                }
            }

            // Server Certificate
            if (_scenarioContext.Get<bool>("validateServerCert"))
            {
                // Validate Server Certificate
                ServicePointManager.ServerCertificateValidationCallback =
                    (sender, cert, chain, error) =>
                    {
                        var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                        var returnValue = false;
                        try
                        {
                            store.Open(OpenFlags.ReadOnly);
                            returnValue = store.Certificates.Contains(cert);
                        }
                        finally
                        {
                            store.Close();
                        }
                        Console.WriteLine(returnValue);
                        return returnValue;
                    };
                ServicePointManager.MaxServicePointIdleTime = 0;
            }
            else {
                // Do NOT Validate Server Certificate
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.MaxServicePointIdleTime = 0;
            }

        }


        [Then(@"the response status code should indicate authentication failure")]
        public void ThenTheResponseStatusCodeShouldIndicateAuthenticationFailure()
        {
            _scenarioContext.Get<HttpStatusCode>("responseStatusCode").ShouldBe(HttpStatusCode.Forbidden);
            Console.Out.WriteLine("Response HttpStatusCode={0}", _scenarioContext.Get<HttpStatusCode>("responseStatusCode"));
        }

        [Then(@"the response status code should be ""(.*)""")]
        public void ThenTheResponseStatusCodeShouldBe(string statusCode)
        {
            _scenarioContext.Get<HttpStatusCode>("responseStatusCode").ToString().ShouldBe(statusCode);
            Console.Out.WriteLine("Response HttpStatusCode should be {0} but was {1}", statusCode, _scenarioContext.Get<HttpStatusCode>("responseStatusCode"));
        }

    }
}
