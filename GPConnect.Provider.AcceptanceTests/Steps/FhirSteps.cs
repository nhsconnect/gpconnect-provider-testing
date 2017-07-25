namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Linq;
    using System.Xml.Linq;
    using Constants;
    using Context;
    using Helpers;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Serialization;
    using Logger;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    public class FhirSteps : Steps
    {
        private readonly FhirContext _fhirContext;
        private readonly HttpContext _httpContext;

        // Constructor
        public FhirSteps(HttpContext httpContext, FhirContext fhirContext)
        {
            Log.WriteLine("FhirSteps() Constructor");
            _httpContext = httpContext;            
            _fhirContext = fhirContext;
        }

        // Before Scenario
        [BeforeScenario(Order = 4)]
        public void ClearFhirOperationParameters()
        {
            Log.WriteLine("ClearFhirOperationParameters()");
            _fhirContext.FhirRequestParameters = new Parameters();
        }

        [Given(@"I am requesting the ""([^""]*)"" care record section")]
        public void GivenIAmRequestingTheCareRecordSection(string careRecordSection)
        {
            _fhirContext.FhirRequestParameters.Add(FhirConst.GetCareRecordParams.kRecordSection, FhirHelper.GetRecordSectionCodeableConcept(careRecordSection));
        }

        [Then(@"the response body should be FHIR JSON")]
        public void ThenTheResponseBodyShouldBeFHIRJSON()
        {
            _httpContext.ResponseContentType.ShouldStartWith(FhirConst.ContentTypes.kJsonFhir);
            Log.WriteLine("Response ContentType={0}", _httpContext.ResponseContentType);
            _httpContext.ResponseJSON = JObject.Parse(_httpContext.ResponseBody);
            FhirJsonParser fhirJsonParser = new FhirJsonParser();
            _fhirContext.FhirResponseResource = fhirJsonParser.Parse<Resource>(_httpContext.ResponseBody);
        }

        [Then(@"the response should be the format FHIR JSON")]
        public void TheResponseShouldBeTheFormatFHIRJSON()
        {
            _httpContext.ResponseContentType.ShouldStartWith(FhirConst.ContentTypes.kJsonFhir);
        }

        [Then(@"the response should be the format FHIR XML")]
        public void TheResponseShouldBeTheFormatXMLJSON()
        {
            _httpContext.ResponseContentType.ShouldStartWith(FhirConst.ContentTypes.kXmlFhir);
        }

        [Then(@"the response body should be empty")]
        public void ThenTheResponseBodyShouldBeEmpty()
        {
            _httpContext.ResponseBody.ShouldBeNullOrEmpty("The response should not contain a payload but the response was not null or empty");
        }

        [Then(@"the response body should be FHIR XML")]
        public void ThenTheResponseBodyShouldBeFHIRXML()
        {
            _httpContext.ResponseContentType.ShouldStartWith(FhirConst.ContentTypes.kXmlFhir);
            Log.WriteLine("Response ContentType={0}", _httpContext.ResponseContentType);
            // TODO Move XML Parsing Out Of Here
            _httpContext.ResponseXML = XDocument.Parse(_httpContext.ResponseBody);
            FhirXmlParser fhirXmlParser = new FhirXmlParser();
            _fhirContext.FhirResponseResource = fhirXmlParser.Parse<Resource>(_httpContext.ResponseBody);
        }

        [Then(@"the response bundle should contain ""([^""]*)"" entries")]
        public void ThenResponseBundleEntryShouldNotBeEmpty(int expectedSize)
        {
            Bundle bundle = (Bundle)_fhirContext.FhirResponseResource;
            bundle.Entry.Count.ShouldBe(expectedSize, "The response bundle does not contain the expected number of entries");
        }

        [Then(@"the response bundle entry ""([^""]*)"" should contain element ""([^""]*)""")]
        public void ThenResponseBundleEntryShouldContainElement(string entryResourceType, string jsonPath)
        {
            var resourceEntry = _httpContext.ResponseJSON.SelectToken($"$.entry[?(@.resource.resourceType == '{entryResourceType}')]");
            resourceEntry.SelectToken(jsonPath).ShouldNotBeNull();
        }

        [Then(@"the response bundle ""([^""]*)"" entries should contain element ""([^""]*)""")]
        public void ThenResponseBundleEntriesShouldContainElement(string entryResourceType, string jsonPath)
        {
            var resourceEntries = _httpContext.ResponseJSON.SelectTokens($"$.entry[?(@.resource.resourceType == '{entryResourceType}')]");

            foreach (var resourceEntry in resourceEntries)
            {
                resourceEntry.SelectToken(jsonPath).ShouldNotBeNull();
            }
        }

        [Then(@"the response bundle entry ""([^""]*)"" should optionally contain element ""([^""]*)"" and that element should reference a resource in the bundle")]
        public void ThenResponseBundleEntryShouldContainElementAndThatElementShouldReferenceAResourceInTheBundle(string entryResourceType, string jsonPath)
        {
            var resourceEntry = _httpContext.ResponseJSON.SelectToken($"$.entry[?(@.resource.resourceType == '{entryResourceType}')]");

            if (resourceEntry.SelectToken(jsonPath) != null)
            {
                var internalReference = resourceEntry.SelectToken(jsonPath).Value<string>();
                _httpContext.ResponseJSON.SelectToken("$.entry[?(@.fullUrl == '" + internalReference + "')]").ShouldNotBeNull();
            }
        }

        [Then(@"the conformance profile should contain the ""([^""]*)"" operation")]
        public void ThenTheConformanceProfileShouldContainTheOperation(string operationName)
        {
            Log.WriteLine("Conformance profile should contain operation = {0}", operationName);
            var passed = false;
            foreach (var rest in _httpContext.ResponseJSON.SelectToken("rest"))
            {
                foreach (var operation in rest.SelectToken("operation"))
                {
                    if (operationName.Equals(operation["name"].Value<string>()) && operation.SelectToken("definition.reference") != null)
                    {
                        passed = true;
                        break;
                    }
                }
                if (passed) { break; }
            }
            passed.ShouldBeTrue();
        }

       [Then(@"the conformance profile should contain the ""([^""]*)"" resource with a ""([^""]*)"" interaction")]
        public void ThenTheConformanceProfileShouldContainTheResourceWithInteraction(string resourceName, string interaction)
        {
            Log.WriteLine("Conformance profile should contain resource = {0} with interaction = {1}", resourceName, interaction);

            foreach (var rest in _httpContext.ResponseJSON.SelectToken("rest"))
            {
                foreach (var resource in rest.SelectToken("resource"))
                {
                    if (resourceName.Equals(resource["type"].Value<string>()) && null != resource.SelectToken("interaction[?(@.code == '" + interaction + "')]"))
                    {
                        return;
                    }
                }
            }

            Assert.Fail("No interaction " + interaction + " for " + resourceName + " resource found.");
        }
        
        [Then(@"all search response entities in bundle should contain a logical identifier")]
        public void AllSearchResponseEntitiesShouldContainALogicalIdentifier()
        {
            ((Bundle)_fhirContext.FhirResponseResource)
                .Entry
                .Where(x => string.IsNullOrWhiteSpace(x.Resource.Id))
                .ShouldBeEmpty("Found an empty (or non-existant) logical id");
        }

        [Then(@"the returned resource shall contains a logical id")]
        public void ThenTheReturnedResourceShallContainALogicalId()
        {
            _fhirContext.FhirResponseResource.Id.ShouldNotBeNullOrEmpty("The returned resource should contain a logical Id but does not.");
        }

        [Then(@"the returned resource shall contain a logical id matching the requested read logical identifier")]
        public void ThenTheReturnedResourceShallContainALogicalIdMatchingTheRequestedReadLogicalIdentifier()
        {
            _fhirContext.FhirResponseResource.Id.ShouldNotBeNullOrEmpty("The returned resource should contain a logical Id but does not.");
            _fhirContext.FhirResponseResource.Id.ShouldBe(_httpContext.GetRequestId, "The returned resource logical id did not match the requested id");
        }

        [Then(@"the response resource logical identifier should match that of stored resource ""([^""]*)""")]
        public void TheResponseResourceLogicalIdentifierShouldMatchThatOfStoredResource(string resource)
        {
            var id = _httpContext.StoredFhirResources[resource].Id;

            _fhirContext.FhirResponseResource.Id.ShouldBe(id);
        }
    }
}
