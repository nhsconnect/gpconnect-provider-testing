namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using Context;
    using Helpers;
    using TechTalk.SpecFlow;

    [Binding]
    public class SecuritySteps : Steps
    {
        private readonly SecurityContext _securityContext;

        public SecuritySteps(SecurityContext securityContext)
        {
            _securityContext = securityContext;
        }
        
        [BeforeScenario(Order = 2)]
        public void LoadAppConfig()
        {
            _securityContext.LoadAppConfig();
        }

        [BeforeScenario(Order = 4)]
        public void DoNotValidateServerCertificate()
        {
            SecurityHelper.DoNotValidateServerCertificate();
        }

        [Given(@"I am not using a client certificate")]
        public void IAmNotUsingAClientCertificate()
        {
            _securityContext.SendClientCert = false;
        }

        //SSP Client Certificate Methods
        [Given(@"I am using the SSP client certificate which has expired")]
        public void IAmUsingTheSSPClientCertificateWhichHasExpired()
        {
            _securityContext.ClientCertThumbPrint = AppSettingsHelper.ThumbprintSspInvalidExpired;
            _securityContext.SendClientCert = true;

            ConfigureServerCertificatesAndSsl();
        }

        [Given(@"I am using the valid SSP client certificate")]
        public void GivenIAmUsingTheSSPClientCertificate()
        {
            _securityContext.ClientCertThumbPrint = AppSettingsHelper.ThumbprintSspValid;
            _securityContext.SendClientCert = true;

            ConfigureServerCertificatesAndSsl();
        }

        [Given(@"I am using the SSP client certificate with invalid FQDN")]
        public void GivenIAmUsingTheSSPClientCertificateWithInvalidFQDN()
        {
            _securityContext.ClientCertThumbPrint = AppSettingsHelper.ThumbprintSspInvalidFqdn;
            _securityContext.SendClientCert = true;

            ConfigureServerCertificatesAndSsl();
        }

        [Given(@"I am using the SSP client certificate not signed by Spine CA")]
        public void GivenIAmUsingTheSSPClientCertificateNoteSignedBySpineCA()
        {
            _securityContext.ClientCertThumbPrint = AppSettingsHelper.ThumbprintSspInvalidAuthority;
            _securityContext.SendClientCert = true;

            ConfigureServerCertificatesAndSsl();
        }

        [Given(@"I am using the SSP client certificate which has been revoked")]
        public void GivenIAmUsingTheSSPClientCertificateWhichHasBeenRevoked()
        {
            _securityContext.ClientCertThumbPrint = AppSettingsHelper.ThumbprintSspInvalidRevoked;
            _securityContext.SendClientCert = true;

            ConfigureServerCertificatesAndSsl();
        }

        //Consumer Client Certificate Methods
        [Given(@"I am using the valid Consumer client certificate")]
        public void GivenIAmUsingTheClientCertificate()
        {
            _securityContext.ClientCertThumbPrint = AppSettingsHelper.ThumbprintConsumerValid;
            _securityContext.SendClientCert = true;

            ConfigureServerCertificatesAndSsl();
        }

        [Given(@"I am using the Consumer client certificate which is out of date")]
        public void IAmUsingTheClientCertificateWhichIsOutOfDate()
        {
            _securityContext.ClientCertThumbPrint = AppSettingsHelper.ThumbprintConsumerInvalidExpired;
            _securityContext.SendClientCert = true;

            ConfigureServerCertificatesAndSsl();
        }

        [Given(@"I am using the Consumer client certificate with invalid FQDN")]
        public void GivenIAmUsingThePClientCertificateWithInvalidFQDN()
        {
            _securityContext.ClientCertThumbPrint = AppSettingsHelper.ThumbprintConsumerInvalidFqdn;
            _securityContext.SendClientCert = true;

            ConfigureServerCertificatesAndSsl();
        }
        
        [Given(@"I am using the Consumer client certificate not signed by Spine CA")]
        public void GivenIAmUsingTheClientCertificateNoteSignedBySpineCA()
        {
            _securityContext.ClientCertThumbPrint = AppSettingsHelper.ThumbprintConsumerInvalidAuthority;
            _securityContext.SendClientCert = true;

            ConfigureServerCertificatesAndSsl();
        }

        [Given(@"I am using the Consumer client certificate which has been revoked")]
        public void GivenIAmUsingTheClientCertificateWhichHasBeenRevoked()
        {
            _securityContext.ClientCertThumbPrint = AppSettingsHelper.ThumbprintConsumerInvalidRevoked;
            _securityContext.SendClientCert = true;

            ConfigureServerCertificatesAndSsl();
        }

        [Given(@"I am using a TLS Connection")]
        public void IAmUsingTLSConnection()
        {
            _securityContext.UseTLS = true;
        }

        [Given(@"I am not using TLS Connection")]
        public void IAmNotUsingTLSConnection()
        {
            _securityContext.UseTLS = false;
        }

        [Given(@"I set the Cipher to ""(.*)""")]
        public void SetTheCipherTo(string cipher)
        {
            _securityContext.Cipher = cipher;
        }

        [Given(@"I configure server certificate and ssl")]
        public void ConfigureServerCertificatesAndSsl()
        {
            // Setup The Client Certificate
            if (_securityContext.SendClientCert)
            {
                _securityContext.ClientCert = SecurityHelper.GetCertificateByClientThumbPrint(_securityContext.ClientCertThumbPrint);
            }

            // Setup The Server Certificate Validation (If Required)
            if (_securityContext.ValidateServerCert)
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
