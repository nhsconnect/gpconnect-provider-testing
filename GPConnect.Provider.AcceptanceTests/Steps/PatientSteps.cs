using System;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Newtonsoft.Json.Linq;
using Shouldly;
using TechTalk.SpecFlow;
using Hl7.Fhir.Serialization;
using NUnit.Framework;
using System.IO;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace GPConnect.Provider.AcceptanceTests.Steps
{

    [Binding]
    public class PatientSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private readonly SecurityContext SecurityContext;
        private readonly HttpContext HttpContext;
        
        // Constructor

        public PatientSteps(SecurityContext securityContext, HttpContext httpContext, FhirContext fhirContext)
        {
            Log.WriteLine("PatientSteps() Constructor");
            SecurityContext = securityContext;
            HttpContext = httpContext;            
            FhirContext = fhirContext;
        }
        
        // Patient Steps

        [When(@"I search for Patient ""([^""]*)""")]
        public void ISearchForPatient(string patient)
        {
            ISearchForPatientWithSystem(patient, FhirConst.IdentifierSystems.kNHSNumber);
        }

        [When(@"I search for Patient ""([^""]*)"" with system ""([^""]*)""")]
        public void ISearchForPatientWithSystem(string patient, string identifierSystem)
        {
            var parameterString = "identifier=" + identifierSystem + "|" + FhirContext.FhirPatients[patient];
            ISearchForAPatientWithParameterString(patient, parameterString);
        }

        [When(@"I search for Patient ""([^""]*)"" without system in identifier parameter")]
        public void ISearchForPatientWithoutSystemInIdentifierParameter(string patient)
        {
            var parameterString = "identifier=" + FhirContext.FhirPatients[patient];
            ISearchForAPatientWithParameterString(patient, parameterString);
        }

        [When(@"I search for a Patient ""([^""]*)"" with parameter string ""([^""]*)""")]
        public void ISearchForAPatientWithParameterString(string patient, string parameterString)
        {
            Given($@"I set the JWT requested scope to ""{JwtConst.Scope.kPatientRead}""");
            And($@"I set the JWT requested record patient NHS number to ""{FhirContext.FhirPatients[patient]}""");

            var timer = new System.Diagnostics.Stopwatch();

            var preferredFormat = ResourceFormat.Json;
            if (!HttpContext.RequestHeaders.GetHeaderValue(HttpConst.Headers.kAccept).Equals(FhirConst.ContentTypes.kJsonFhir))
            {
                preferredFormat = ResourceFormat.Xml;
            }

            var fhirClient = new FhirClient(HttpContext.EndpointAddress)
            {
                PreferredFormat = preferredFormat
            };

            // On Before Request
            fhirClient.OnBeforeRequest += (sender, args) =>
            {
                Log.WriteLine("*** OnBeforeRequest ***");
                var client = (FhirClient)sender;

                // Setup The Web Proxy
                if (HttpContext.UseWebProxy)
                {
                    args.RawRequest.Proxy = new WebProxy(new Uri(HttpContext.WebProxyAddress, UriKind.Absolute));
                }
                // Add The Request Headers Apart From The Accept Header
                foreach (var header in HttpContext.RequestHeaders.GetRequestHeaders().Where(header => header.Key != HttpConst.Headers.kAccept))
                {
                    args.RawRequest.Headers.Add(header.Key, header.Value);
                    Log.WriteLine("Added Header Key='{0}' Value='{1}'", header.Key, header.Value);
                }
                // Add The Client Certificate
                if (SecurityContext.SendClientCert)
                {
                    args.RawRequest.ClientCertificates.Add(SecurityContext.ClientCert);
                    Log.WriteLine("Added ClientCertificate Thumbprint='{0}'", SecurityContext.ClientCertThumbPrint);
                }
            };

            // On After Request
            fhirClient.OnAfterResponse += (sender, args) =>
            {
                Log.WriteLine("*** OnAfterResponse ***");
                var client = (FhirClient)sender;
                HttpContext.ResponseStatusCode = client.LastResponse.StatusCode;
                Log.WriteLine("Response StatusCode={0}", client.LastResponse.StatusCode);
                HttpContext.ResponseContentType = client.LastResponse.ContentType;
                Log.WriteLine("Response ContentType={0}", client.LastResponse.ContentType);

                foreach (string headerKey in client.LastResponse.Headers.Keys)
                {
                    HttpContext.ResponseHeaders.Add(headerKey, client.LastResponse.Headers.Get(headerKey));
                }
            };

            // Make The Request And Save The Returned Resource
            try
            {
                // Set HttpContext variables for Logging purposes
                HttpContext.RequestUrl = "/Patient?" + parameterString;
                HttpContext.RequestMethod = "GET";
                HttpContext.RequestBody = "";

                // Start The Performance Timer Running
                timer.Start();

                // Perform The FHIR Request
                var query = new string[] { parameterString };

                FhirContext.FhirResponseResource = fhirClient.Search("Patient", query);
            }
            catch (Exception e)
            {
                Log.WriteLine(e.StackTrace);
            }
            finally
            {
                // Always Stop The Performance Timer Running
                timer.Stop();
            }

            // Save The Time Taken To Perform The Request
            HttpContext.ResponseTimeInMilliseconds = timer.ElapsedMilliseconds;

            // Grab The Response Body
            HttpContext.ResponseBody = fhirClient.LastBodyAsText;
            FhirContext.FhirResponseResource = fhirClient.LastBodyAsResource;
            LogToDisk();
        }

        private void LogToDisk()
        {
            var traceDirectory = GlobalContext.TraceDirectory;
            if (!Directory.Exists(traceDirectory)) return;
            var scenarioDirectory = Path.Combine(traceDirectory, HttpContext.ScenarioContext.ScenarioInfo.Title);
            int fileIndex = 1;
            while (Directory.Exists(scenarioDirectory + "-" + fileIndex)) fileIndex++;
            scenarioDirectory = scenarioDirectory + "-" + fileIndex;
            Directory.CreateDirectory(scenarioDirectory);
            Log.WriteLine(scenarioDirectory);
            HttpContext.SaveToDisk(Path.Combine(scenarioDirectory, "HttpContext.xml"));
            FhirContext.SaveToDisk(Path.Combine(scenarioDirectory, "FhirContext.xml"));
        }
    }
}
