using System;
using System.Net;
using RestSharp;
using Shouldly;
using TechTalk.SpecFlow;
using System.Collections.Generic;
using GPConnect.Provider.AcceptanceTests.tools;

namespace GPConnect.Provider.AcceptanceTests.Steps
{

    [Binding]
    public class Http : TechTalk.SpecFlow.Steps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly HeaderController _headerController;
        private readonly JwtHelper _jwtHelper;

        public Http(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _headerController = HeaderController.Instance;
            _jwtHelper = JwtHelper.Instance;
        }

        // Server Endpoint Configuration Steps

        [Given(@"I am using server ""([^\s]*)""")]
        public void GivenIAmUsingServer(string serverUrl)
        {
            _scenarioContext.Set(serverUrl, "serverUrl");
        }

        [Given(@"I am using server ""([^\s]*)"" on port ""([^\s]*)""")]
        public void GivenIAmUsingServerOnPort(string serverUrl, string serverPort)
        {
            _scenarioContext.Set(serverUrl + ":" + serverPort, "serverUrl");
        }
        
        [Given(@"I set base URL to ""([^\s]*)""")]
        public void GivenISetBaseURLTo(string baseUrl)
        {
            _scenarioContext.Set(baseUrl, "baseUrl");
        }


        // Spine Proxy Configuration Steps

        [Given(@"I am not using the spine proxy server")]
        public void GivenIAmNotUsingTheSpineProxyServer()
        {
            _scenarioContext.Set("", "spineProxyUrl");
        }

        [Given(@"I am using the spine proxy server ""([^\s]*)""")]
        public void GivenIAmUsingTheSpineProxyServer(string proxyServerUrl)
        {
            _scenarioContext.Set(proxyServerUrl + "/", "spineProxyUrl");
        }

        [Given(@"I am using the spine proxy server ""([^\s]*)"" on port ""([^\s]*)""")]
        public void GivenIAmUsingTheSpineProxyServerOnPort(string proxyServerUrl, string proxyServerPort)
        {
            _scenarioContext.Set(proxyServerUrl + ":" + proxyServerPort + "/", "spineProxyUrl");
        }


        // HTTP Header Configuration Steps

        [Given(@"I am using ""(.*)"" to communicate with the server")]
        public void GivenIAmUsingToCommunicateWithTheServer(string requestContentType)
        {
            _headerController.removeHeader("Accept");
            _headerController.addHeader("Accept", requestContentType);
        }

        [Given(@"I set ""(.*)"" request header to ""(.*)""")]
        public void GivenISetRequestHeaderTo(string headerKey, string headerValue)
        {
            _headerController.removeHeader(headerKey);
            _headerController.addHeader(headerKey, headerValue);
        }

        [Given(@"I am accredited system ""(.*)""")]
        public void GivenIAmAccreditedSystem(string fromASID)
        {
            _headerController.removeHeader("Ssp-From");
            _headerController.addHeader("Ssp-From", fromASID);
        }

        [Given(@"I am performing the ""(.*)"" interaction")]
        public void GivenIAmPerformingTheInteraction(string interactionId)
        {
            _headerController.removeHeader("Ssp-InteractionId");
            _headerController.addHeader("Ssp-InteractionId", interactionId);
        }

        [Given(@"I am connecting to accredited system ""(.*)""")]
        public void GivenIConnectingToAccreditedSystem(string toASID)
        {
            _headerController.removeHeader("Ssp-To");
            _headerController.addHeader("Ssp-To", toASID);
        }

        [Given(@"I am generating a random message trace identifier")]
        public void GivenIAmGeneratingARandomMessageTraceIdentifier()
        {
            _headerController.removeHeader("Ssp-TraceID");
            _headerController.addHeader("Ssp-TraceID", Guid.NewGuid().ToString(""));
        }

        [Given(@"I am generating an organization authorization header")]
        public void GivenIAmGeneratingAnOrganizationAuthorizationHeader()
        {
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenOrgResource());
        }

        [Given(@"I am generating a patient authorization header with nhs number ""(.*)""")]
        public void GivenIAmGeneratingAPatientAuthorizationHeader(string nhsNumber)
        {
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenPatientResource(nhsNumber));
        }


        // Generic Request Steps

        [When(@"I make a GET request to ""(.*)""")]
        public void WhenIMakeAGETRequestTo(string relativeUrl)
        {
            _scenarioContext.Set(relativeUrl, "relativeUrl");
            // Build The Request
            var restClient = new RestClient(_scenarioContext.Get<string>("spineProxyUrl") + _scenarioContext.Get<string>("serverUrl"));
            _scenarioContext.Set(restClient, "restClient");
            var fullUrl = _scenarioContext.Get<string>("baseUrl") + _scenarioContext.Get<string>("relativeUrl");
            Console.Out.WriteLine("GET fullUrl={0}", fullUrl);
            var restRequest = new RestRequest(fullUrl, Method.GET);

            // Add Headers
            foreach (KeyValuePair<string, string> header in _headerController.getRequestHeaders())
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


        // Response Validation Steps

        [Then(@"the response status code should indicate success")]
        public void ThenTheResponseStatusCodeShouldIndicateSuccess()
        {
            _scenarioContext.Get<HttpStatusCode>("responseStatusCode").ShouldBe(HttpStatusCode.OK);
            Console.Out.WriteLine("Response HttpStatusCode={0}", HttpStatusCode.OK);
        }
        
    }
}
