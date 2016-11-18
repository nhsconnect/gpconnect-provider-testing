using TechTalk.SpecFlow;
using GPConnect.Provider.AcceptanceTests.tools;
using System.Net;
using System;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using System.IO;

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

        [Given(@"I set the JWT expiry time to ""(.*)"" seconds after creation time")]
        public void ISetTheJWTExpiryTimeToSecondsAfterCreationTime(double expirySeconds)
        {
            _jwtHelper.setJWTExpiryTimeInSeconds(expirySeconds);
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenOrgResource());
        }


        // Certificate configruation steps

        [Given(@"I do not want to verify the server certificate")]
        public void IDoNotWantToVerifyTheServerCertificate()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.MaxServicePointIdleTime = 0;
        }

        [Given(@"I do want to verify the server certificate")]
        public void IDoWantToVerifyTheServerCertificate()
        {
            ServicePointManager.ServerCertificateValidationCallback =
                (sender, cert, chain, error) =>
                {
                    Console.Write("Check if server cert is in certificate store : ");
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

        [Given(@"I am using client certificate with thumbprint ""(.*)""")]
        public void IAmUsingClientCertificateWithThumbprint(string thumbPrint)
        {
            thumbPrint = Regex.Replace(thumbPrint, @"[^\da-zA-z]", string.Empty).ToUpper();
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

    }
}
