using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Extensions;
using GPConnect.Provider.AcceptanceTests.Helpers;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json.Linq;
using Shouldly;
using TechTalk.SpecFlow;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace GPConnect.Provider.AcceptanceTests.Steps
{

    [Binding]
    public class FhirSteps : TechTalk.SpecFlow.Steps
    {

        private static class Context
        {
            public const string FhirClient = "fhirClient";
            public const string FhirRequestParameters = "fhirRequestParameters";
            public const string FhirResource = "fhirResource";
        }

        // TODO Once We Can Inject Other Steps Into This One We Can Reuse The HttpSteps and SecuritySteps Accessors So They Don't Need To Be Duplicated Here
        private Parameters FhirRequestParameters => _scenarioContext.Get<Parameters>(Context.FhirRequestParameters);
        private string ResponseContentType => _scenarioContext.Get<string>(HttpSteps.Context.ResponseContentType);
        private JObject ResponseJSON => _scenarioContext.Get<JObject>(HttpSteps.Context.ResponseJSON);
        private bool SendClientCert => _scenarioContext.Get<bool>(SecuritySteps.Context.SendClientCert);
        private X509Certificate2 ClientCertificate => _scenarioContext.Get<X509Certificate2>(SecuritySteps.Context.ClientCertificate);
        private string ResponseBody => _scenarioContext.Get<string>(HttpSteps.Context.ResponseBody);
        
        private readonly ScenarioContext _scenarioContext;
        private readonly HttpHeaderHelper _headerController;

        public FhirSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            // TODO Move To Injecting In These Helpers And Make Them Not Singletons
            _headerController = HttpHeaderHelper.Instance;
        }

        // FHIR Operation Steps

        [Given(@"I author a request for the ""(.*)"" care record section for patient with NHS Number ""(.*)""")]
        public void IAuthorARequestForTheCareRecordSection(string recordSectionCode, string nhsNumber)
        {
            // TODO Make A Helper For Building Operation Parameter Sets
            var parameters = new Parameters();
            parameters.Add(FhirConst.GetCareRecordParams.NHSNumber, FhirHelper.GetNHSNumberIdentifier(nhsNumber));
            parameters.Add(FhirConst.GetCareRecordParams.RecordSection, FhirHelper.GetRecordSectionCodeableConcept(recordSectionCode));
            _scenarioContext.Set(parameters, Context.FhirRequestParameters);

            Given(@"I set the JWT requested scope to ""patient/*.read""");
            And(@"I set the JWT requested record patient NHS number to """ + nhsNumber + "\"");
        }

        [When(@"I request the FHIR ""(.*)"" Patient Type operation")]
        public void IRequestTheFHIROperation(string operation)
        {
            var preferredFormat = ResourceFormat.Json;
            if (!_headerController.GetHeaderValue(HttpConst.Headers.Accept).Equals(FhirConst.ContentTypes.JsonFhir))
            {
                preferredFormat = ResourceFormat.Xml;
            }
            var fhirClient = new FhirClient(EndpointHelper.GetProviderURL(_scenarioContext))
            {
                PreferredFormat = preferredFormat
            };
            // TODO Setup The FHIR Client To Be Able To Use A WebProxy
            fhirClient.OnBeforeRequest += (sender, args) =>
            {
                // Add The Request Headers Apart From The Accept Header
                foreach (var header in _headerController.GetRequestHeaders().Where(header => header.Key != HttpConst.Headers.Accept))
                {
                    args.RawRequest.Headers.Add(header.Key, header.Value);
                    Console.WriteLine("Added Header Key='{0}' Value='{1}'", header.Key, header.Value);
                }

                // Setup The Client Certificate
                if (SendClientCert)
                {
                    try
                    {
                        args.RawRequest.ClientCertificates.Add(ClientCertificate);
                    }
                    catch (KeyNotFoundException)
                    {
                        // TODO Ensure That The Test Fails If We Get Here And The Exception Isn't Silently Swallowed
                        Console.WriteLine("No client certificate found in scenario context");
                    }
                }
            };
            fhirClient.OnAfterResponse += (sender, args) =>
            {
                _scenarioContext.Set(fhirClient.LastResponse.StatusCode, HttpSteps.Context.ResponseStatusCode);
                Console.WriteLine("Response StatusCode={0}", fhirClient.LastResponse.StatusCode);
                _scenarioContext.Set(fhirClient.LastResponse.ContentType, HttpSteps.Context.ResponseContentType);
                Console.WriteLine("Response ContentType={0}", fhirClient.LastResponse.ContentType);
            };
            _scenarioContext.Set(fhirClient, Context.FhirClient);

            try
            {
                var fhirResource = fhirClient.TypeOperation<Patient>(operation, FhirRequestParameters);
                _scenarioContext.Set(fhirResource, Context.FhirResource);
                var fhirResponse = fhirResource.ToJson();
                _scenarioContext.Set(fhirResponse, HttpSteps.Context.ResponseBody);
                Console.WriteLine("Response Content={0}", fhirResponse);
            }
            catch (Exception e)
            {
                // TODO Ensure That The Test Fails If We Get Here And The Exception Isn't Silently Swallowed
                Console.WriteLine("Operation Error = " + e.StackTrace);
            }
        }

        // Response Validation Steps

        [Then(@"the response body should be FHIR JSON")]
        public void ThenTheResponseBodyShouldBeFHIRJSON()
        {
            ResponseContentType.ShouldStartWith(FhirConst.ContentTypes.JsonFhir);
            Console.WriteLine("Response ContentType={0}", ResponseContentType);
            _scenarioContext.Set(JObject.Parse(ResponseBody), HttpSteps.Context.ResponseJSON);
        }

        [Then(@"the response body should be FHIR XML")]
        public void ThenTheResponseBodyShouldBeFHIRXML()
        {
            ResponseContentType.ShouldStartWith(FhirConst.ContentTypes.XmlFhir);
            Console.WriteLine("Response ContentType={0}", ResponseContentType);
            _scenarioContext.Set(XDocument.Parse(ResponseBody), HttpSteps.Context.ResponseXML);
        }

        [Then(@"the JSON value ""(.*)"" should be ""(.*)""")]
        public void ThenTheJSONValueShouldBe(string key, string value)
        {
            Console.WriteLine("Json Key={0} Value={1} Expect={2}", key, ResponseJSON[key], value);
            ResponseJSON[key].ShouldBe(value);
        }

    }
}
