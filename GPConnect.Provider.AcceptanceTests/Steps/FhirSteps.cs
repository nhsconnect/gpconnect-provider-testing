using System;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Logger;
using GPConnect.Provider.AcceptanceTests.Tables;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Newtonsoft.Json.Linq;
using Shouldly;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace GPConnect.Provider.AcceptanceTests.Steps
{

    [Binding]
    public class FhirSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private readonly SecurityContext SecurityContext;
        private readonly HttpContext HttpContext;
        
        // Constructor

        public FhirSteps(SecurityContext securityContext, HttpContext httpContext, FhirContext fhirContext)
        {
            Log.WriteLine("FhirSteps() Constructor");
            SecurityContext = securityContext;
            HttpContext = httpContext;            
            FhirContext = fhirContext;
        }

        // Before Scenario

        [BeforeScenario(Order = 4)]
        public void ClearFhirOperationParameters()
        {
            Log.WriteLine("ClearFhirOperationParameters()");
            FhirContext.FhirRequestParameters = new Parameters();
        }

        // FHIR Operation Steps

        [Given(@"I author a request for the ""(.*)"" care record section for patient with NHS Number ""(.*)""")]
        public void IAuthorARequestForTheCareRecordSection(string recordSectionCode, string nhsNumber)
        {
            Given($@"I set the JWT requested scope to ""{JwtConst.Scope.PatientRead}""");
            And($@"I set the JWT requested record patient NHS number to ""{nhsNumber}""");
            And($@"I am requesting the record for patient with NHS Number ""{nhsNumber}""");
            And($@"I am requesting the ""{recordSectionCode}"" care record section");
        }

        [Given(@"I am requesting the record for patient with NHS Number ""(.*)""")]
        public void GivenIAmRequestingTheRecordForPatientWithNHSNumber(string nhsNumber)
        {
            Given($@"I set the JWT requested scope to ""{JwtConst.Scope.PatientRead}""");
            And($@"I set the JWT requested record patient NHS number to ""{nhsNumber}""");
            FhirContext.FhirRequestParameters.Add(FhirConst.GetCareRecordParams.PatientNHSNumber, FhirHelper.GetNHSNumberIdentifier(nhsNumber));
        }

        [Given(@"I am requesting the ""(.*)"" care record section")]
        public void GivenIAmRequestingTheCareRecordSection(string careRecordSection)
        {
            // TODO Their Is A Bug In The Demonstrator -> It's Not Using A CodeableConcept For The RecordSection
            FhirContext.FhirRequestParameters.Add(FhirConst.GetCareRecordParams.RecordSection, FhirHelper.GetRecordSectionCodeableConcept(careRecordSection));
            //FhirContext.FhirRequestParameters.Add(FhirConst.GetCareRecordParams.RecordSection, new FhirString(careRecordSection));
        }

        [When(@"I request the FHIR ""(.*)"" Patient Type operation")]
        public void IRequestTheFHIROperation(string operation)
        {
            var preferredFormat = ResourceFormat.Json;
            if (!HttpContext.Headers.GetHeaderValue(HttpConst.Headers.Accept).Equals(FhirConst.ContentTypes.JsonFhir))
            {
                preferredFormat = ResourceFormat.Xml;
            }
            var fhirClient = new FhirClient(HttpContext.ProviderAddress)
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
                foreach (var header in HttpContext.Headers.GetRequestHeaders().Where(header => header.Key != HttpConst.Headers.Accept))
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
            };

            // Make The Request And Save The Returned Resource
            FhirContext.FhirResponseResource = fhirClient.TypeOperation<Patient>(operation, FhirContext.FhirRequestParameters);

            // Grab The Response Body
            HttpContext.ResponseBody = fhirClient.LastBodyAsText;
            
            // TODO Parse The XML or JSON For Easier Processing
        }

        // Response Validation Steps

        [Then(@"the response body should be FHIR JSON")]
        public void ThenTheResponseBodyShouldBeFHIRJSON()
        {
            HttpContext.ResponseContentType.ShouldStartWith(FhirConst.ContentTypes.JsonFhir);
            Log.WriteLine("Response ContentType={0}", HttpContext.ResponseContentType);
            // TODO Move JSON Parsing Out Of Here
            HttpContext.ResponseJSON = JObject.Parse(HttpContext.ResponseBody);
        }

        [Then(@"the response body should be FHIR XML")]
        public void ThenTheResponseBodyShouldBeFHIRXML()
        {
            HttpContext.ResponseContentType.ShouldStartWith(FhirConst.ContentTypes.XmlFhir);
            Log.WriteLine("Response ContentType={0}", HttpContext.ResponseContentType);
            // TODO Move XML Parsing Out Of Here
            HttpContext.ResponseXML = XDocument.Parse(HttpContext.ResponseBody);
        }

        [Then(@"the JSON value ""(.*)"" should be ""(.*)""")]
        public void ThenTheJSONValueShouldBe(string key, string value)
        {
            Log.WriteLine("Json Key={0} Value={1} Expect={2}", key, HttpContext.ResponseJSON[key], value);
            HttpContext.ResponseJSON[key].ShouldBe(value);
        }

        [Then(@"the JSON array ""([^""]*)"" should contain ""([^""]*)""")]
        public void ThenTheJSONArrayShouldContain(string key, string value)
        {
            Log.WriteLine("Array " + HttpContext.ResponseJSON[key] + "should contain " + value);
            var passed = HttpContext.ResponseJSON[key].Any(entry => string.Equals(entry.Value<string>(), value));
            passed.ShouldBeTrue();
        }

        [Then(@"the JSON array ""([^""]*)"" should contain ""([^""]*)"" or ""([^""]*)""")]
        public void ThenTheJSONArrayShouldContain(string key, string value1, string value2)
        {
            Log.WriteLine("Array " + HttpContext.ResponseJSON[key] + "should contain " + value1 + " or " + value2);
            var passed = HttpContext.ResponseJSON[key].Any(entry => string.Equals(entry.Value<string>(), value1) || string.Equals(entry.Value<string>(), value2));
            passed.ShouldBeTrue();
        }

        [Then(@"the JSON element ""(.*)"" should be present")]
        public void ThenTheJSONElementShouldBePresent(string jsonPath)
        {
            Log.WriteLine("Json KeyPath={0} should be present", jsonPath);
            HttpContext.ResponseJSON.SelectToken(jsonPath).ShouldNotBeNull();
        }

        [Then(@"the conformance profile should contain the ""([^""]*)"" operation")]
        public void ThenTheConformanceProfileShouldContainTheOperation(string operationName) {
            Log.WriteLine("Conformance profile should contain operation = {0}", operationName);
            var passed = false;
            foreach (var rest in HttpContext.ResponseJSON.SelectToken("rest")) {
                foreach (var operation in rest.SelectToken("operation"))
                {
                    if (string.Equals(operationName, operation["name"].Value<string>()) && operation.SelectToken("definition.reference") != null) {
                        passed = true;
                        break;
                    }
                }
                if (passed) { break; }
            }
            passed.ShouldBeTrue();
        }

    }
}
