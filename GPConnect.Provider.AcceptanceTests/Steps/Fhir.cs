using TechTalk.SpecFlow;
using GPConnect.Provider.AcceptanceTests.tools;
using Shouldly;
using System;
using System.Collections.Generic;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json.Linq;

namespace GPConnect.Provider.AcceptanceTests.Steps
{

    [Binding]
    public class Fhir : TechTalk.SpecFlow.Steps
    {
        private readonly ScenarioContext _scenarioContext;
        private HeaderController _headerController;
        private JwtHelper _jwtHelper;

        public Fhir(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _headerController = HeaderController.Instance;
            _jwtHelper = JwtHelper.Instance;
        }

        // FHIR Demonstartor Steps

        [Given(@"I am using the gpconnect FHIR demonstator")]
        public void GivenIAmUsingTheGPConnectDemonstrator()
        {
            Given(@"I am using server ""http://gpconnect-uat.answerappcloud.com""");
            And(@"I am not using the spine proxy server");
            And(@"I am using ""application/json+fhir"" to communicate with the server");
            And(@"I set base URL to ""/fhir""");
            And(@"I am accredited system ""200000000359""");
            And(@"I am connecting to accredited system ""200000000360""");
            And(@"I am generating a random message trace identifier");
            And(@"I am generating an organization authorization header");
        }

        // FHIR Operation Steps

        [Given(@"I author a request for the ""(.*)"" care record section for patient with NHS Number ""(.*)""")]
        public void IAuthorARequestForTheCareRecordSection(string recordSectionCode, string nhsNumber)
        {
            var parameters = new Parameters();
            parameters.Add("nhsNumber", new Identifier("http://fhir.nhs.net/Id/nhs-number", nhsNumber));
            parameters.Add("recordSection", new CodeableConcept("http://fhir.nhs.net/ValueSet/gpconnect-record-section-1-0", recordSectionCode));
            _scenarioContext.Set(parameters, "fhirRequestParameters");
        }

        [When(@"I request the FHIR ""(.*)"" Patient Type operation")]
        public void IRequestTheFHIROperation(string operation)
        {
            var fhirClient = new FhirClient(_scenarioContext.Get<string>("spineProxyUrl") + _scenarioContext.Get<string>("serverUrl") + _scenarioContext.Get<string>("baseUrl"))
            {
                PreferredFormat = ResourceFormat.Json
            };
            fhirClient.OnBeforeRequest += (sender, args) =>
            {
                // Add Headers
                foreach (var header in _headerController.getRequestHeaders())
                {
                    Console.WriteLine("Header - {0} -> {1}", header.Key, header.Value);
                    if (header.Key != "Accept")
                        args.RawRequest.Headers.Add(header.Key, header.Value);
                }
            };
            fhirClient.OnAfterResponse += (sender, args) =>
            {
                _scenarioContext.Set(fhirClient.LastResponse.StatusCode, "responseStatusCode");
                Console.Out.WriteLine("Response StatusCode={0}", fhirClient.LastResponse.StatusCode);
                _scenarioContext.Set(fhirClient.LastResponse.ContentType, "responseContentType");
                Console.Out.WriteLine("Response ContentType={0}", fhirClient.LastResponse.ContentType);
            };
            _scenarioContext.Set(fhirClient, "fhirClient");
            var fhirResource = fhirClient.TypeOperation<Patient>(operation, _scenarioContext.Get<Parameters>("fhirRequestParameters"));
            _scenarioContext.Set(fhirResource, "fhirResource");
            var fhirResponse = FhirSerializer.SerializeResourceToJson(fhirResource);
            _scenarioContext.Set(fhirResponse, "responseBody");
            Console.Out.WriteLine("Response Content={0}", fhirResponse);
        }

        // Response Validation Steps

        [Then(@"the response body should be FHIR JSON")]
        public void ThenTheResponseBodyShouldBeFHIRJSON()
        {
            _scenarioContext.Get<string>("responseContentType").ShouldStartWith("application/json+fhir");
            Console.Out.WriteLine("Response ContentType={0}", "application/json+fhir");
            _scenarioContext.Set(JObject.Parse(_scenarioContext.Get<string>("responseBody")), "responseJSON");
        }

        [Then(@"the JSON value ""(.*)"" should be ""(.*)""")]
        public void ThenTheJSONValueShouldBe(string key, string value)
        {
            var json = _scenarioContext.Get<JObject>("responseJSON");
            Console.Out.WriteLine("Json Key={0} Value={1} Expect={2}", key, json[key], value);
            json[key].ShouldBe(value);
        }

    }
}
