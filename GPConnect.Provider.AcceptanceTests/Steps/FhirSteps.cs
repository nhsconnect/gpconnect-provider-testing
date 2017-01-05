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
using Newtonsoft.Json;

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

        [Given(@"I set the JWT header for getcarerecord with config patient ""([^""]*)""")]
        public void ISetTheJWTHeaderForGetcarerecordWithConfigPatient(string patient) {
            Given($@"I set the JWT requested scope to ""{JwtConst.Scope.kPatientRead}""");
            And($@"I set the JWT requested record patient NHS number to ""{FhirContext.FhirPatients[patient]}""");
        }

        [Given(@"I author a request for the ""(.*)"" care record section for config patient ""(.*)""")]
        public void IAuthorARequestForTheCareRecordSectionForPatient(string recordSectionCode, string patient)
        {
            Given($@"I author a request for the ""{recordSectionCode}"" care record section for patient with NHS Number ""{FhirContext.FhirPatients[patient]}""");
        }

        [Given(@"I author a request for the ""(.*)"" care record section for patient with NHS Number ""(.*)""")]
        public void IAuthorARequestForTheCareRecordSection(string recordSectionCode, string nhsNumber)
        {
            Given($@"I set the JWT requested scope to ""{JwtConst.Scope.kPatientRead}""");
            And($@"I set the JWT requested record patient NHS number to ""{nhsNumber}""");
            And($@"I am requesting the record for patient with NHS Number ""{nhsNumber}""");
            And($@"I am requesting the ""{recordSectionCode}"" care record section");
        }

        [Given(@"I am requesting the record for patient with NHS Number ""(.*)""")]
        public void GivenIAmRequestingTheRecordForPatientWithNHSNumber(string nhsNumber)
        {
            Given($@"I set the JWT requested scope to ""{JwtConst.Scope.kPatientRead}""");
            And($@"I set the JWT requested record patient NHS number to ""{nhsNumber}""");
            FhirContext.FhirRequestParameters.Add(FhirConst.GetCareRecordParams.kPatientNHSNumber, FhirHelper.GetNHSNumberIdentifier(nhsNumber));
        }

        [Given(@"I am requesting the record for config patient ""([^""]*)"" using a fhir string parameter")]
        public void GivenIAmRequestingTheRecordForConfigPatientUsingAFHirStringParameter(string patient)
        {
            Given($@"I set the JWT requested scope to ""{JwtConst.Scope.kPatientRead}""");
            And($@"I set the JWT requested record patient NHS number to ""{FhirContext.FhirPatients[patient]}""");
            FhirContext.FhirRequestParameters.Add(FhirConst.GetCareRecordParams.kPatientNHSNumber, new FhirString(FhirContext.FhirPatients[patient]));
        }


        [Given(@"I am requesting the record for config patient ""([^""]*)"" of system ""([^""]*)""")]
        public void GivenIAmRequestingTheRecordForConfigPatientOfSystem(string patient, string system)
        {
            Given($@"I set the JWT requested scope to ""{JwtConst.Scope.kPatientRead}""");
            And($@"I set the JWT requested record patient NHS number to ""{FhirContext.FhirPatients[patient]}""");
            FhirContext.FhirRequestParameters.Add(FhirConst.GetCareRecordParams.kPatientNHSNumber, FhirHelper.GetIdentifier(system, FhirContext.FhirPatients[patient]));
        }

        [Given(@"I am requesting the ""([^""]*)"" care record section")]
        public void GivenIAmRequestingTheCareRecordSection(string careRecordSection)
        {
            FhirContext.FhirRequestParameters.Add(FhirConst.GetCareRecordParams.kRecordSection, FhirHelper.GetRecordSectionCodeableConcept(careRecordSection));
        }

        [Given(@"I am requesting the ""([^""]*)"" care record section with a string parameter")]
        public void GivenIAmRequestingTheCareRecordSectionWithAStringParameter(string careRecordSection)
        {
            FhirContext.FhirRequestParameters.Add(FhirConst.GetCareRecordParams.kRecordSection, new FhirString(careRecordSection));
        }

        [Given(@"I am requesting the ""([^""]*)"" care record section with system ""([^""]*)""")]
        public void GivenIAmRequestingTheCareRecordSectionWithSystem(string careRecordSection, string system)
        {
            FhirContext.FhirRequestParameters.Add(FhirConst.GetCareRecordParams.kRecordSection, FhirHelper.GetRecordSectionCodeableConcept(system, careRecordSection));
        }

        [Given(@"I set a valid time period start and end date")]
        public void GivenISetAValidTimePeriodStartAndEndDate()
        {
            FhirContext.FhirRequestParameters.Add(FhirConst.GetCareRecordParams.kTimePeriod, FhirHelper.GetDefaultTimePeriodForGetCareRecord());
        }

        [Given(@"I set a time period parameter start date to ""([^""]*)"" and end date to ""([^""]*)""")]
        public void GivenISetATimePeriodParameterStartDateToAndEndDateTo(string startDate, string endDate)
        {
            FhirContext.FhirRequestParameters.Add(FhirConst.GetCareRecordParams.kTimePeriod, FhirHelper.GetTimePeriod(startDate, endDate));
        }

        [Given(@"I set a time period parameter with start date ""([^""]*)""")]
        public void GivenISetATimePeriodParameterWithStartDate(string startDate)
        {
            FhirContext.FhirRequestParameters.Add(FhirConst.GetCareRecordParams.kTimePeriod, FhirHelper.GetTimePeriod(startDate, null));
        }

        [Given(@"I set a time period parameter with end date ""([^""]*)""")]
        public void GivenISetATimePeriodParameterWithEndDate(string endDate)
        {
            FhirContext.FhirRequestParameters.Add(FhirConst.GetCareRecordParams.kTimePeriod, FhirHelper.GetTimePeriod(null, endDate));
        }

        [When(@"I request the FHIR ""(.*)"" Patient Type operation")]
        public void IRequestTheFHIROperation(string operation)
        {
            var preferredFormat = ResourceFormat.Json;
            if (!HttpContext.RequestHeaders.GetHeaderValue(HttpConst.Headers.kAccept).Equals(FhirConst.ContentTypes.kJsonFhir))
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
            };

            // Make The Request And Save The Returned Resource
            try
            {
                FhirContext.FhirResponseResource = fhirClient.TypeOperation<Patient>(operation, FhirContext.FhirRequestParameters);
            }
            catch (Exception e) {
                Log.WriteLine(e.StackTrace);
            }

            // Grab The Response Body
            HttpContext.ResponseBody = fhirClient.LastBodyAsText;
            FhirContext.FhirResponseResource = fhirClient.LastBodyAsResource;

            // TODO Parse The XML or JSON For Easier Processing
        }

        // Response Validation Steps

        [Then(@"the response body should be FHIR JSON")]
        public void ThenTheResponseBodyShouldBeFHIRJSON()
        {
            HttpContext.ResponseContentType.ShouldStartWith(FhirConst.ContentTypes.kJsonFhir);
            Log.WriteLine("Response ContentType={0}", HttpContext.ResponseContentType);
            // TODO Move JSON Parsing Out Of Here
            HttpContext.ResponseJSON = JObject.Parse(HttpContext.ResponseBody);
        }

        [Then(@"the response body should be FHIR XML")]
        public void ThenTheResponseBodyShouldBeFHIRXML()
        {
            HttpContext.ResponseContentType.ShouldStartWith(FhirConst.ContentTypes.kXmlFhir);
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
            var passed = HttpContext.ResponseJSON[key].Any(entry => entry.Value<string>().Equals(value));
            passed.ShouldBeTrue();
        }

        [Then(@"the JSON array ""([^""]*)"" should contain ""([^""]*)"" or ""([^""]*)""")]
        public void ThenTheJSONArrayShouldContain(string key, string value1, string value2)
        {
            Log.WriteLine("Array " + HttpContext.ResponseJSON[key] + "should contain " + value1 + " or " + value2);
            var passed = HttpContext.ResponseJSON[key].Any(entry => entry.Value<string>().Equals(value1) || entry.Value<string>().Equals(value2));
            passed.ShouldBeTrue();
        }

        [Then(@"the JSON element ""(.*)"" should be present")]
        public void ThenTheJSONElementShouldBePresent(string jsonPath)
        {
            Log.WriteLine("Json KeyPath={0} should be present", jsonPath);
            HttpContext.ResponseJSON.SelectToken(jsonPath).ShouldNotBeNull();
        }

        [Then(@"response bundle entry ""([^""]*)"" should contain element ""([^""]*)""")]
        public void ThenResponseBundleEntryShouldContainElement(string entryResourceType, string jsonPath) {
            var resourceEntry = HttpContext.ResponseJSON.SelectToken($"$.entry[?(@.resource.resourceType == '{entryResourceType}')]");
            resourceEntry.SelectToken(jsonPath).ShouldNotBeNull();
        }

        [Then(@"response bundle entry ""([^""]*)"" should contain element ""([^""]*)"" with value ""([^""]*)""")]
        public void ThenResponseBundleEntryShouldContainElementWithValue(string entryResourceType, string jsonPath, string elementValue)
        {
            var resourceEntry = HttpContext.ResponseJSON.SelectToken($"$.entry[?(@.resource.resourceType == '{entryResourceType}')]");
            resourceEntry.SelectToken(jsonPath).Value<string>().ShouldBe(elementValue);
        }
        
        [Then(@"response bundle entry ""([^""]*)"" should contain element ""([^""]*)"" and that element should reference a resource in the bundle")]
        public void ThenResponseBundleEntryShouldContainElementAndThatElementShouldReferenceAResourceInTheBundle(string entryResourceType, string jsonPath)
        {
            var resourceEntry = HttpContext.ResponseJSON.SelectToken($"$.entry[?(@.resource.resourceType == '{entryResourceType}')]");
            var internalReference = resourceEntry.SelectToken(jsonPath).Value<string>();
            HttpContext.ResponseJSON.SelectToken("$.entry[?(@.fullUrl == '" + internalReference + "')]").ShouldNotBeNull();
        }

        [Then(@"the conformance profile should contain the ""([^""]*)"" operation")]
        public void ThenTheConformanceProfileShouldContainTheOperation(string operationName) {
            Log.WriteLine("Conformance profile should contain operation = {0}", operationName);
            var passed = false;
            foreach (var rest in HttpContext.ResponseJSON.SelectToken("rest")) {
                foreach (var operation in rest.SelectToken("operation"))
                {
                    if (operationName.Equals(operation["name"].Value<string>()) && operation.SelectToken("definition.reference") != null) {
                        passed = true;
                        break;
                    }
                }
                if (passed) { break; }
            }
            passed.ShouldBeTrue();
        }

        [Then(@"if response bundle entry ""([^""]*)"" contains element ""([^""]*)""")]
        public void ThenIfResponseBundleEntryContainsElement(string entryResourceType, string jsonPath)
        {
            var resourceEntry = HttpContext.ResponseJSON.SelectToken($"$.entry[?(@.resource.resourceType == '{entryResourceType}')]");
            if (resourceEntry.SelectToken(jsonPath) == null) {
                Log.WriteLine("No Reference in response bundle so skipping rest of test but giving Pass to scenario.");
                Assert.Pass(); // If element is not present pass and ignore other steps
            }
        }

    }
}
