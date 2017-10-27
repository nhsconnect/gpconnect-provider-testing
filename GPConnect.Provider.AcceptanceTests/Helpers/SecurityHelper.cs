using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using GPConnect.Provider.AcceptanceTests.Logger;

namespace GPConnect.Provider.AcceptanceTests.Helpers
{
    static class SecurityHelper
    {
        public static X509Certificate2 GetCertificateByClientThumbPrint(string clientCertThumbPrint)
        {
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                var signingCert = store.Certificates.Find(X509FindType.FindByThumbprint, clientCertThumbPrint, false);
                if (signingCert.Count != 1)
                {
                    throw new FileNotFoundException($"Cert with thumbprint: '{clientCertThumbPrint}' not found in local machine cert store.");
                }
                Log.WriteLine("Client Certificate Found = " + signingCert[0]);
                return signingCert[0];
            }
            finally
            {
                store.Close();
            }
        }

        public static void ValidateServerCertificate()
        {
            ServicePointManager.ServerCertificateValidationCallback =
                (sender, cert, chain, error) =>
                {
                    var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                    bool returnValue;
                    try
                    {
                        Log.WriteLine("Server Certificate recieved = " + cert);
                        Log.WriteLine("Store Certificate Size = " + store.Certificates.Count);
                        foreach (var storedCert in store.Certificates)
                        {
                            Log.WriteLine("Store Certificate = " + storedCert);
                        }

                        store.Open(OpenFlags.ReadOnly);
                        // TODO Fix The Validation Of The Server Certificate
                        returnValue = store.Certificates.Contains(cert);
                    }
                    finally
                    {
                        store.Close();
                    }
                    Log.WriteLine(returnValue.ToString());
                    return returnValue;
                };
            ServicePointManager.MaxServicePointIdleTime = 0;
            Log.WriteLine("Setup The Server Certificate Callback To Validate The Server Certificate.");
        }

        public static void DoNotValidateServerCertificate()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.MaxServicePointIdleTime = 0;
            Log.WriteLine("Force The Server Certificate Callback To Always Be True.");
        }

        public static void ForceServerCertificateToValidate()
        {
            ServicePointManager.ServerCertificateValidationCallback = null;
            ServicePointManager.MaxServicePointIdleTime = 0;
            Log.WriteLine("Reset The Server Certificate Callback.");
        }
    }
}
