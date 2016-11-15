using System;
using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;
using Shouldly;
using TechTalk.SpecFlow;
using System.Collections.Generic;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public class HttpSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private Dictionary<string, string> requestHeaders;

        public HttpSteps(ScenarioContext scenarioContext)
        {
            this._scenarioContext = scenarioContext;
            requestHeaders = new Dictionary<string, string>();
        }

        [BeforeScenario("stdHeaders")]
        public void addStdHeaders()
        {
            Console.WriteLine("Adding standard headers...");
            GivenIAmGeneratingARandomMessageTraceIdentifier();
            GivenIAmAccreditedSystem("200000000359");
            GivenIConnectingToAccreditedSystem("200000000360");
            GivenIAmPerformingTheInteraction("urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord");
            requestHeaders.Add("Authorization", "Bearer eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJpc3MiOiJodHRwOi8vZ3Bjb25uZWN0LXVhdC5hbnN3ZXJhcHBjbG91ZC5jb20vIy9wYXRpZW50cy85MDAwMDAwMDMzL3BhdGllbnRzLXN1bW1hcnkiLCJzdWIiOiIxIiwiYXVkIjoiaHR0cHM6Ly9hdXRob3JpemUuZmhpci5uaHMubmV0L3Rva2VuIiwiZXhwIjoxNDcyMjM5ODUyLCJpYXQiOjE0NzE5Mzk4NTIsInJlYXNvbl9mb3JfcmVxdWVzdCI6ImRpcmVjdGNhcmUiLCJyZXF1ZXN0ZWRfcmVjb3JkIjp7InJlc291cmNlVHlwZSI6IlBhdGllbnQiLCJpZGVudGlmaWVyIjpbeyJzeXN0ZW0iOiJodHRwOi8vZmhpci5uaHMubmV0L0lkL25ocy1udW1iZXIiLCJ2YWx1ZSI6IjkwMDAwMDAwMzMifV19LCJyZXF1ZXN0ZWRfc2NvcGUiOiJwYXRpZW50LyoucmVhZCIsInJlcXVlc3RpbmdfZGV2aWNlIjp7InJlc291cmNlVHlwZSI6IkRldmljZSIsImlkIjoiMSIsImlkZW50aWZpZXIiOlt7InN5c3RlbSI6IldlYiBJbnRlcmZhY2UiLCJ2YWx1ZSI6IkdQIENvbm5lY3QgRGVtb25zdHJhdG9yIn1dLCJtb2RlbCI6IkRlbW9uc3RyYXRvciIsInZlcnNpb24iOiIxLjAifSwicmVxdWVzdGluZ19vcmdhbml6YXRpb24iOnsicmVzb3VyY2VUeXBlIjoiT3JnYW5pemF0aW9uIiwiaWQiOiIxIiwiaWRlbnRpZmllciI6W3sic3lzdGVtIjoiaHR0cDovL2ZoaXIubmhzLm5ldC9JZC9vZHMtb3JnYW5pemF0aW9uLWNvZGUiLCJ2YWx1ZSI6IltPRFNDb2RlXSJ9XSwibmFtZSI6IkdQIENvbm5lY3QgRGVtb25zdHJhdG9yIn0sInJlcXVlc3RpbmdfcHJhY3RpdGlvbmVyIjp7InJlc291cmNlVHlwZSI6IlByYWN0aXRpb25lciIsImlkIjoiMSIsImlkZW50aWZpZXIiOlt7InN5c3RlbSI6Imh0dHA6Ly9maGlyLm5ocy5uZXQvc2RzLXVzZXItaWQiLCJ2YWx1ZSI6IkcxMzU3OTEzNSJ9LHsic3lzdGVtIjoibG9jYWxTeXN0ZW0iLCJ2YWx1ZSI6IjEifV0sIm5hbWUiOnsiZmFtaWx5IjpbIkRlbW9uc3RyYXRvciJdLCJnaXZlbiI6WyJHUENvbm5lY3QiXSwicHJlZml4IjpbIk1yIl19fX0.");
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
            requestHeaders.Remove("Content-Type");
            requestHeaders.Add("Content-Type", requestContentType);
        }

        [Given(@"I set ""(.*)"" request header to ""(.*)""")]
        public void GivenISetRequestHeaderTo(string headerKey, string headerValue)
        {
            Console.Out.WriteLine("{0}={1}", headerKey, headerValue);
            requestHeaders.Remove(headerKey);
            requestHeaders.Add(headerKey, headerValue);
        }

        [Given(@"I am accredited system ""(.*)""")]
        public void GivenIAmAccreditedSystem(string fromASID)
        {
            Console.Out.WriteLine("Ssp-From={0}", fromASID);
            requestHeaders.Remove("Ssp-From");
            requestHeaders.Add("Ssp-From", fromASID);
        }

        [Given(@"I am performing the ""(.*)"" interaction")]
        public void GivenIAmPerformingTheInteraction(string interactionId)
        {
            Console.Out.WriteLine("Ssp-InteractionId={0}", interactionId);
            requestHeaders.Remove("Ssp-InteractionId");
            requestHeaders.Add("Ssp-InteractionId", interactionId);
        }

        [Given(@"I am connecting to accredited system ""(.*)""")]
        public void GivenIConnectingToAccreditedSystem(string toASID)
        {
            Console.Out.WriteLine("Ssp-To={0}", toASID);
            requestHeaders.Remove("Ssp-To");
            requestHeaders.Add("Ssp-To", toASID);
        }

        [Given(@"I am generating a random message trace identifier")]
        public void GivenIAmGeneratingARandomMessageTraceIdentifier()
        {
            String traceIdentifier = Guid.NewGuid().ToString("");
            Console.Out.WriteLine("TraceIdentifier={0}", traceIdentifier);
            requestHeaders.Remove("Ssp-TraceID");
            requestHeaders.Add("Ssp-TraceID", traceIdentifier);
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
