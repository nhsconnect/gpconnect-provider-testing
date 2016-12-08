using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Extensions;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
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
        private readonly ScenarioContext _scenarioContext;
        private readonly HttpSteps _httpSteps;
        private readonly SecuritySteps _securitySteps;

        private static class Context
        {
            public const string FhirRequestParameters = "fhirRequestParameters";
            public const string FhirResponseResource = "fhirResponseResource";
        }

        // FHIR Details

        private Parameters FhirRequestParameters => _scenarioContext.Get<Parameters>(Context.FhirRequestParameters);
        private Resource FhirResponseResource => _scenarioContext.Get<Resource>(Context.FhirResponseResource);
        
        // Constructor

        public FhirSteps(ScenarioContext scenarioContext, SecuritySteps securitySteps, HttpSteps httpSteps)
        {
            Log.WriteLine("FhirSteps() Constructor");
            _scenarioContext = scenarioContext;
            _httpSteps = httpSteps;
            _securitySteps = securitySteps;
        }

        // FHIR Operation Steps

        [Given(@"I author a request for the ""(.*)"" care record section for patient with NHS Number ""(.*)""")]
        public void IAuthorARequestForTheCareRecordSection(string recordSectionCode, string nhsNumber)
        {            
            Given($@"I set the JWT requested scope to ""{JwtConst.Scope.PatientRead}""");
            And($@"I set the JWT requested record patient NHS number to ""{nhsNumber}""");
            
            // TODO Make A Helper For Building Operation Parameter Sets
            var parameters = new Parameters();
            parameters.Add(FhirConst.GetCareRecordParams.NHSNumber, FhirHelper.GetNHSNumberIdentifier(nhsNumber));
            parameters.Add(FhirConst.GetCareRecordParams.RecordSection, FhirHelper.GetRecordSectionCodeableConcept(recordSectionCode));
            _scenarioContext.Set(parameters, Context.FhirRequestParameters);
        }

        [When(@"I request the FHIR ""(.*)"" Patient Type operation")]
        public void IRequestTheFHIROperation(string operation)
        {
            var preferredFormat = ResourceFormat.Json;
            if (!_httpSteps.Headers.GetHeaderValue(HttpConst.Headers.Accept).Equals(FhirConst.ContentTypes.JsonFhir))
            {
                preferredFormat = ResourceFormat.Xml;
            }
            var fhirClient = new FhirClient(EndpointHelper.GetProviderURL(_scenarioContext))
            {
                PreferredFormat = preferredFormat
            };

            // On Before Request
            fhirClient.OnBeforeRequest += (sender, args) =>
            {
                Log.WriteLine("*** OnBeforeRequest ***");
                var client = (FhirClient)sender;
                // Setup The Web Proxy
                if (_httpSteps.UseWebProxy)
                {
                    args.RawRequest.Proxy = new WebProxy(new Uri(EndpointHelper.GetWebProxyURL(_scenarioContext), UriKind.Absolute));
                }
                // Add The Request Headers Apart From The Accept Header
                foreach (var header in _httpSteps.Headers.GetRequestHeaders().Where(header => header.Key != HttpConst.Headers.Accept))
                {
                    args.RawRequest.Headers.Add(header.Key, header.Value);
                    Log.WriteLine("Added Header Key='{0}' Value='{1}'", header.Key, header.Value);
                }
                // Add The Client Certificate
                if (_securitySteps.SendClientCert)
                {
                    args.RawRequest.ClientCertificates.Add(_securitySteps.ClientCert);
                    Log.WriteLine("Added ClientCertificate Thumbprint='{0}'", _securitySteps.ClientCertThumbPrint);
                }
            };

            // On After Request
            fhirClient.OnAfterResponse += (sender, args) =>
            {
                Log.WriteLine("*** OnAfterResponse ***");
                var client = (FhirClient)sender;
                _scenarioContext.Set(client.LastResponse.StatusCode, HttpSteps.Context.ResponseStatusCode);
                Log.WriteLine("Response StatusCode={0}", client.LastResponse.StatusCode);
                _scenarioContext.Set(client.LastResponse.ContentType, HttpSteps.Context.ResponseContentType);
                Log.WriteLine("Response ContentType={0}", client.LastResponse.ContentType);
            };

            // Make The Request
            var fhirResource = fhirClient.TypeOperation<Patient>(operation, FhirRequestParameters);
            _scenarioContext.Set(fhirResource, Context.FhirResponseResource);

            // Grab The Response Body
            var responseBody = fhirClient.LastBodyAsText;
            _scenarioContext.Set(responseBody, HttpSteps.Context.ResponseBody);
            Log.WriteLine("Response Body={0}", responseBody);

            // TODO Parse The XML or JSON For Easier Processing
        }

        // Response Validation Steps

        [Then(@"the response body should be FHIR JSON")]
        public void ThenTheResponseBodyShouldBeFHIRJSON()
        {
            _httpSteps.ResponseContentType.ShouldStartWith(FhirConst.ContentTypes.JsonFhir);
            Log.WriteLine("Response ContentType={0}", _httpSteps.ResponseContentType);
            // TODO Move JSON Parsing Out Of Here
            _scenarioContext.Set(JObject.Parse(_httpSteps.ResponseBody), HttpSteps.Context.ResponseJSON);
        }

        [Then(@"the response body should be FHIR XML")]
        public void ThenTheResponseBodyShouldBeFHIRXML()
        {
            _httpSteps.ResponseContentType.ShouldStartWith(FhirConst.ContentTypes.XmlFhir);
            Log.WriteLine("Response ContentType={0}", _httpSteps.ResponseContentType);
            // TODO Move XML Parsing Out Of Here
            _scenarioContext.Set(XDocument.Parse(_httpSteps.ResponseBody), HttpSteps.Context.ResponseXML);
        }

        [Then(@"the JSON value ""(.*)"" should be ""(.*)""")]
        public void ThenTheJSONValueShouldBe(string key, string value)
        {
            Log.WriteLine("Json Key={0} Value={1} Expect={2}", key, _httpSteps.ResponseJSON[key], value);
            _httpSteps.ResponseJSON[key].ShouldBe(value);
        }

        [Then(@"the JSON array ""([^""]*)"" should contain ""([^""]*)""")]
        public void ThenTheJSONArrayShouldContain(string key, string value)
        {
            Log.WriteLine("Array " + _httpSteps.ResponseJSON[key] + "should contain " + value);
            var passed = _httpSteps.ResponseJSON[key].Any(entry => string.Equals(entry.Value<string>(), value));
            passed.ShouldBeTrue();
        }

        [Then(@"the JSON array ""([^""]*)"" should contain ""([^""]*)"" or ""([^""]*)""")]
        public void ThenTheJSONArrayShouldContain(string key, string value1, string value2)
        {
            Log.WriteLine("Array " + _httpSteps.ResponseJSON[key] + "should contain " + value1 + " or " + value2);
            var passed = _httpSteps.ResponseJSON[key].Any(entry => string.Equals(entry.Value<string>(), value1) || string.Equals(entry.Value<string>(), value2));
            passed.ShouldBeTrue();
        }

        [Then(@"the JSON element ""(.*)"" should be present")]
        public void ThenTheJSONElementShouldBePresent(string jsonPath)
        {
            Log.WriteLine("Json KeyPath={0} should be present", jsonPath);
            _httpSteps.ResponseJSON.SelectToken(jsonPath).ShouldNotBeNull();
        }

    }
}
