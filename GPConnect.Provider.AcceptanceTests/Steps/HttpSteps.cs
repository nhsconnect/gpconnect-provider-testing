using System;
using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;
using Shouldly;
using TechTalk.SpecFlow;
using System.Collections.Generic;
using GPConnect.Provider.AcceptanceTests.tools;

namespace GPConnect.Provider.AcceptanceTests.Steps
{

    [Binding]
    public class HttpSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private Dictionary<string, string> requestHeaders;
        private JwtHelper jwtHelper = new JwtHelper();

        public HttpSteps(ScenarioContext scenarioContext)
        {
            this._scenarioContext = scenarioContext;
            requestHeaders = new Dictionary<string, string>();
        }
        
        [Given(@"I am using server ""(.*)""")]
        public void GivenIAmUsingServer(string serverUrl)
        {
            _scenarioContext.Set(serverUrl, "serverUrl");
        }

        [Given(@"I am not using a proxy server")]
        public void GivenIAmNotUsingAProxyServer()
        {
            _scenarioContext.Set(false, "useProxy");
        }

        [Given(@"I set base URL to ""(.*)""")]
        public void GivenISetBaseURLTo(string baseUrl)
        {
            _scenarioContext.Set(baseUrl, "baseUrl");
        }

        [Given(@"I am using ""(.*)"" to communicate with the server")]
        public void GivenIAmUsingToCommunicateWithTheServer(string requestContentType)
        {
            requestHeaders.Remove("Content-Type");
            requestHeaders.Add("Content-Type", requestContentType);
        }

        [Given(@"I set ""(.*)"" request header to ""(.*)""")]
        public void GivenISetRequestHeaderTo(string headerKey, string headerValue)
        {
            requestHeaders.Remove(headerKey);
            requestHeaders.Add(headerKey, headerValue);
        }

        [Given(@"I am accredited system ""(.*)""")]
        public void GivenIAmAccreditedSystem(string fromASID)
        {
            requestHeaders.Remove("Ssp-From");
            requestHeaders.Add("Ssp-From", fromASID);
        }

        [Given(@"I am performing the ""(.*)"" interaction")]
        public void GivenIAmPerformingTheInteraction(string interactionId)
        {
            requestHeaders.Remove("Ssp-InteractionId");
            requestHeaders.Add("Ssp-InteractionId", interactionId);
        }

        [Given(@"I am connecting to accredited system ""(.*)""")]
        public void GivenIConnectingToAccreditedSystem(string toASID)
        {
            requestHeaders.Remove("Ssp-To");
            requestHeaders.Add("Ssp-To", toASID);
        }

        [Given(@"I am generating a random message trace identifier")]
        public void GivenIAmGeneratingARandomMessageTraceIdentifier()
        {
            requestHeaders.Remove("Ssp-TraceID");
            requestHeaders.Add("Ssp-TraceID", Guid.NewGuid().ToString(""));
        }

        [Given(@"I am generating an organization authorization header")]
        public void GivenIAmGeneratingAnOrganizationAuthorizationHeader()
        {
            requestHeaders.Add("Authorization", "Bearer " + jwtHelper.buildBearerTokenOrgResource());
        }

        [Given(@"I am generating a patient authorization header with nhs number ""(.*)""")]
        public void GivenIAmGeneratingAPatientAuthorizationHeader(string nhsNumber)
        {
            requestHeaders.Add("Authorization", "Bearer " + jwtHelper.buildBearerTokenPatientResource(nhsNumber));
        }

        [When(@"I make a GET request to ""(.*)""")]
        public void WhenIMakeAGETRequestTo(string relativeUrl)
        {
            _scenarioContext.Set(relativeUrl, "relativeUrl");
            // Build The Request
            var restClient = new RestClient(_scenarioContext.Get<string>("serverUrl"));
            _scenarioContext.Set(restClient, "restClient");
            var fullUrl = _scenarioContext.Get<string>("baseUrl") + _scenarioContext.Get<string>("relativeUrl");
            Console.Out.WriteLine("GET fullUrl={0}", fullUrl);
            var restRequest = new RestRequest(fullUrl, Method.GET);

            // Add Headers
            foreach (KeyValuePair<string, string> header in requestHeaders)
            {
                Console.WriteLine("Header - {0} -> {1}", header.Key, header.Value);
                restRequest.AddHeader(header.Key, header.Value);
            }

            // Execute The Request
            var restResponse = restClient.Execute(restRequest);
            // Pull Apart The Response
            _scenarioContext.Set(restResponse, "restResponse");
            _scenarioContext.Set(restResponse.StatusCode, "responseStatusCode");
            Console.Out.WriteLine("Response StatusCode={0}", restResponse.StatusCode);
            _scenarioContext.Set(restResponse.ContentType, "responseContentType");
            Console.Out.WriteLine("Response ContentType={0}", restResponse.ContentType);
            _scenarioContext.Set(restResponse.Content, "responseBody");
            Console.Out.WriteLine("Response Content={0}", restResponse.Content);
        }

        [Then(@"the response status code should indicate success")]
        public void ThenTheResponseStatusCodeShouldIndicateSuccess()
        {
            _scenarioContext.Get<HttpStatusCode>("responseStatusCode").ShouldBe(HttpStatusCode.OK);
            Console.Out.WriteLine("Response HttpStatusCode={0}", HttpStatusCode.OK);
        }

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
