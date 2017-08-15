using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Logger;
using TechTalk.SpecFlow;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Global

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public class SecuritySteps : TechTalk.SpecFlow.Steps
    {
        private readonly SecurityContext SecurityContext;

        // Constructor

        public SecuritySteps(SecurityContext securityContext)
        {
            Log.WriteLine("SecuritySteps() Constructor");
            SecurityContext = securityContext;
        }

        // Before Scenario

        [BeforeScenario(Order = 2)]
        public void LoadAppConfig()
        {
            SecurityContext.LoadAppConfig();
        }

        [BeforeScenario(Order = 4)]
        public void DoNotValidateServerCertificate()
        {
            SecurityHelper.DoNotValidateServerCertificate();
        }

        // Security Configuration Steps

        [Given(@"I do not want to verify the server certificate")]
        public void IDoNotWantToVerifyTheServerCertificate()
        {
            SecurityContext.ValidateServerCert = false;
        }

        [Given(@"I do want to verify the server certificate")]
        public void IDoWantToVerifyTheServerCertificate()
        {
            SecurityContext.ValidateServerCert = true;
        }

        [Given(@"I am not using a client certificate")]
        public void IAmNotUsingAClientCertificate()
        {
            SecurityContext.SendClientCert = false;
        }

        [Given(@"I am using client certificate with thumbprint ""(.*)""")]
        public void IAmUsingClientCertificateWithThumbprint(string thumbPrint)
        {
            SecurityContext.ClientCertThumbPrint = thumbPrint;
            SecurityContext.SendClientCert = true;
            Given(@"I configure server certificate and ssl");
        }

        [Given(@"I am using the SSP client certificate which is out of date")]
        public void IAmUsingTheSSPClientCertificateWhichIsOutOfDate()
        {
            SecurityContext.ClientCertThumbPrint = AppSettingsHelper.sspAsClientCertThumbPrintOutOfDate;
            SecurityContext.SendClientCert = true;
            Given(@"I configure server certificate and ssl");
        }

        [Given(@"I am using the client certificate which is out of date")]
        public void IAmUsingTheClientCertificateWhichIsOutOfDate()
        {
            SecurityContext.ClientCertThumbPrint = AppSettingsHelper.clientCertThumbPrintOutOfDate;
            SecurityContext.SendClientCert = true;
            Given(@"I configure server certificate and ssl");
        }


        [Given(@"I am using the SSP client certificate")]
        public void GivenIAmUsingTheSSPClientCertificate()
        {
            SecurityContext.ClientCertThumbPrint = AppSettingsHelper.sspAsClientCertThumbPrintValid;
            SecurityContext.SendClientCert = true;
            Given(@"I configure server certificate and ssl");
        }

        [Given(@"I am using the client certificate")]
        public void GivenIAmUsingTheClientCertificate()
        {
            SecurityContext.ClientCertThumbPrint = AppSettingsHelper.ClientCertThumbPrintValid;
            SecurityContext.SendClientCert = true;
            Given(@"I configure server certificate and ssl");
        }


        [Given(@"I am using the SSP client certificate with invalid FQDN")]
        public void GivenIAmUsingTheSSPClientCertificateWithInvalidFQDN()
        {
            SecurityContext.ClientCertThumbPrint = AppSettingsHelper.sspAsClientCertThumbPrintFQDNNotSSPFQDN;
            SecurityContext.SendClientCert = true;
            Given(@"I configure server certificate and ssl");
        }

        [Given(@"I am using the client certificate with invalid FQDN")]
        public void GivenIAmUsingThePClientCertificateWithInvalidFQDN()
        {
            SecurityContext.ClientCertThumbPrint = AppSettingsHelper.clientCertThumbPrintFQDNDoesNotMatchSendingSystem;
            SecurityContext.SendClientCert = true;
            Given(@"I configure server certificate and ssl");
        }

        [Given(@"I am using the SSP client certificate not signed by Spine CA")]
        public void GivenIAmUsingTheSSPClientCertificateNoteSignedBySpineCA()
        {
            SecurityContext.ClientCertThumbPrint = AppSettingsHelper.sspAsClientCertThumbPrintNotIssuedBySpineCA;
            SecurityContext.SendClientCert = true;
            Given(@"I configure server certificate and ssl");
        }

        [Given(@"I am using the client certificate not signed by Spine CA")]
        public void GivenIAmUsingTheClientCertificateNoteSignedBySpineCA()
        {
            SecurityContext.ClientCertThumbPrint = AppSettingsHelper.clientCertThumbPrintNotIssuedBySpineCA;
            SecurityContext.SendClientCert = true;
            Given(@"I configure server certificate and ssl");
        }

        [Given(@"I am using the SSP client certificate which has been revoked")]
        public void GivenIAmUsingTheSSPClientCertificateWhichHasBeenRevoked()
        {
            SecurityContext.ClientCertThumbPrint = AppSettingsHelper.sspAsClientCertThumbPrintRevoked;
            SecurityContext.SendClientCert = true;
            Given(@"I configure server certificate and ssl");
        }

        [Given(@"I am using the client certificate which has been revoked")]
        public void GivenIAmUsingTheClientCertificateWhichHasBeenRevoked()
        {
            SecurityContext.ClientCertThumbPrint = AppSettingsHelper.clientCertThumbPrintRevoked;
            SecurityContext.SendClientCert = true;
            Given(@"I configure server certificate and ssl");
        }

        [Given(@"I am using TLS Connection")]
        public void IAmUsingTLSConnection()
        {
            SecurityContext.UseTLS = true;
        }

        [Given(@"I am not using TLS Connection")]
        public void IAmNotUsingTLSConnection()
        {
            SecurityContext.UseTLS = false;
            Given(@"I do not want to verify the server certificate");
            And(@"I am not using a client certificate");
        }

        [Given(@"I set the Cipher to ""(.*)""")]
        public void SetTheCipherTo(string cipher)
        {
            SecurityContext.Cipher = cipher;
        }

        [Given(@"I configure server certificate and ssl")]
        public void ConfigureServerCertificatesAndSsl()
        {
            // Setup The Client Certificate
            if (SecurityContext.SendClientCert)
            {
                SecurityContext.ClientCert = SecurityHelper.GetCertificateByClientThumbPrint(SecurityContext.ClientCertThumbPrint);
            }

            // Setup The Server Certificate Validation (If Required)
            if (SecurityContext.ValidateServerCert)
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
