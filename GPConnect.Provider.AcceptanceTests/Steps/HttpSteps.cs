using System;
using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;
using Shouldly;
using TechTalk.SpecFlow;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public class HttpSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public HttpSteps(ScenarioContext scenarioContext)
        {
            this._scenarioContext = scenarioContext;
        }

        [Given(@"I am using server ""(.*)""")]
        public void GivenIAmUsingServer(string serverUrl)
        {
            Console.Out.WriteLine("serverUrl={0}", serverUrl);
            _scenarioContext.Set(serverUrl, "serverUrl");
        }
        
        [Given(@"I am not using a proxy server")]
        public void GivenIAmNotUsingAProxyServer()
        {
            Console.Out.WriteLine("useProxy={0}", false);
            _scenarioContext.Set(false, "useProxy");
        }
        
        [Given(@"I set base URL to ""(.*)""")]
        public void GivenISetBaseURLTo(string baseUrl)
        {
            Console.Out.WriteLine("baseUrl={0}", baseUrl);
            _scenarioContext.Set(baseUrl, "baseUrl");
        }
        
        [Given(@"I am using ""(.*)"" to communicate with the server")]
        public void GivenIAmUsingToCommunicateWithTheServer(string requestContentType)
        {
            Console.Out.WriteLine("requestContentType={0}", requestContentType);
            _scenarioContext.Set(requestContentType, "requestContentType");
        }
        
        [When(@"I make a GET request to ""(.*)""")]
        public void WhenIMakeAGETRequestTo(string relativeUrl)
        {
            Console.Out.WriteLine("relativeUrl={0}", relativeUrl);
            _scenarioContext.Set(relativeUrl, "relativeUrl");
            // Build The Request
            var restClient = new RestClient(_scenarioContext.Get<string>("serverUrl"));
            _scenarioContext.Set(restClient, "restClient");
            var fullUrl = _scenarioContext.Get<string>("baseUrl") + _scenarioContext.Get<string>("relativeUrl");
            Console.Out.WriteLine("GET fullUrl={0}", fullUrl);
            var restRequest = new RestRequest(fullUrl, Method.GET);
            // Add Headers
            restRequest.AddHeader("Content-Type", _scenarioContext.Get<string>("requestContentType"));
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
            _scenarioContext.Set(JObject.Parse(_scenarioContext.Get<string>("responseBody")),"responseJSON");
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
