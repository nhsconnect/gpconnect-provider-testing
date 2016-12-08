using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Helpers;
using RestSharp;
using Shouldly;
using TechTalk.SpecFlow;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable InconsistentNaming

namespace GPConnect.Provider.AcceptanceTests.Steps
{

    [Binding]
    public class HttpSteps : TechTalk.SpecFlow.Steps
    {
        private readonly ScenarioContext _scenarioContext;

        private readonly HttpHeaderHelper _headerController;
        private readonly JwtHelper _jwtHelper;

        internal static class Context
        {
            // Provider
            public const string FhirServerUrl = "fhirServerUrl";
            public const string FhirServerPort = "fhirServerPort";
            public const string FhirServerFhirBase = "fhirServerFhirBase";
            // Web Proxy
            public const string UseWebProxy = "useWebProxy";
            public const string WebProxyUrl = "webProxyUrl";
            public const string WebProxyPort = "webProxyPort";
            // Spine Proxy
            public const string UseSpineProxy = "useSpineProxy";
            public const string SpineProxyUrl = "spineProxyUrl";
            public const string SpineProxyPort = "spineProxyPort";
            // Response
            public const string ResponseStatusCode = "responseStatusCode";
            public const string ResponseContentType = "responseContentType";
            public const string RestResponse = "restResponse";
            public const string ResponseBody = "responseBody";
            public const string ResponseJSON= "responseJSON";
            public const string ResponseXML = "responseXML";
            // Consumer
            public const string ConsumerASID = "consumerASID";
            // Producer
            public const string ProviderASID = "providerASID";
        }

        private bool UseSpineProxy => _scenarioContext.Get<bool>(Context.UseSpineProxy);
        private string TestSuitASID => _scenarioContext.Get<string>(Context.ConsumerASID);
        private string ProviderSystemASID => _scenarioContext.Get<string>(Context.ProviderASID);
        private bool UseWebProxy => _scenarioContext.Get<bool>(Context.UseWebProxy);
        private bool SendClientCert => _scenarioContext.Get<bool>(SecuritySteps.Context.SendClientCert);
        private X509Certificate2 ClientCertificate => _scenarioContext.Get<X509Certificate2>(SecuritySteps.Context.ClientCertificate);
        private HttpStatusCode ResponseStatusCode => _scenarioContext.Get<HttpStatusCode>(Context.ResponseStatusCode);

        public HttpSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _headerController = HttpHeaderHelper.Instance;
            _jwtHelper = JwtHelper.Instance;
        }

        // Server Endpoint Configuration Steps

        [Given(@"I am using the default server")]
        public void GivenIAmUsingTheDefaultServer()
        {
            // Load The Default Settings From The App.config File
            AppSettingsHelper.LoadAppSettings(_scenarioContext);

            Given(@"I configure server certificate and ssl");
            And(@"I am using ""application/json+fhir"" to communicate with the server");
            And(@"I am generating a random message trace identifier");
            And(string.Format(@"I am accredited system ""{0}""", TestSuitASID));
            And(string.Format(@"I am connecting to accredited system ""{0}""", ProviderSystemASID)); 
            And(@"I am generating an organization JWT header");
        }

        [Given(@"I am using server ""([^\s]*)""")]
        public void GivenIAmUsingServer(string serverUrl)
        {
            _scenarioContext.Set(serverUrl, Context.FhirServerUrl);
        }

        [Given(@"I am using server ""([^\s]*)"" on port ""([^\s]*)""")]
        public void GivenIAmUsingServerOnPort(string serverUrl, string serverPort)
        {
            _scenarioContext.Set(serverUrl, Context.FhirServerUrl);
            _scenarioContext.Set(serverPort, Context.FhirServerPort);
        }

        [Given(@"I am connecting to server on port ""([^\s]*)""")]
        public void GivenIAmConnectingToServerOnPort(string serverPort)
        {
            _scenarioContext.Set(serverPort, Context.FhirServerPort);
        }

        [Given(@"I set base URL to ""([^\s]*)""")]
        public void GivenISetBaseURLTo(string baseUrl)
        {
            _scenarioContext.Set(baseUrl, Context.FhirServerFhirBase);
        }

        // Spine Proxy Configuration Steps

        [Given(@"I am not using the spine proxy server")]
        public void GivenIAmNotUsingTheSpineProxyServer()
        {
            _scenarioContext.Set(false, Context.UseSpineProxy);
        }

        [Given(@"I am using the spine proxy server ""([^\s]*)""")]
        public void GivenIAmUsingTheSpineProxyServer(string proxyServerUrl)
        {
            _scenarioContext.Set(true, Context.UseSpineProxy);
            _scenarioContext.Set(proxyServerUrl, Context.SpineProxyUrl);
            _scenarioContext.Set(ConfigurationManager.AppSettings[Context.SpineProxyPort], Context.SpineProxyPort);
        }

        [Given(@"I am using the spine proxy server ""([^\s]*)"" on port ""([^\s]*)""")]
        public void GivenIAmUsingTheSpineProxyServerOnPort(string proxyServerUrl, string proxyServerPort)
        {
            _scenarioContext.Set(true, Context.UseSpineProxy);
            _scenarioContext.Set(proxyServerUrl, Context.SpineProxyUrl);
            _scenarioContext.Set(proxyServerPort, Context.SpineProxyPort);
        }

        // HTTP Header Configuration Steps

        [Given(@"I am using ""(.*)"" to communicate with the server")]
        public void GivenIAmUsingToCommunicateWithTheServer(string requestContentType)
        {
            _headerController.ReplaceHeader(HttpConst.Headers.Accept, requestContentType);
        }

        [Given(@"I set ""(.*)"" request header to ""(.*)""")]
        public void GivenISetRequestHeaderTo(string headerKey, string headerValue)
        {
            _headerController.ReplaceHeader(headerKey, headerValue);
        }

        [Given(@"I am accredited system ""(.*)""")]
        public void GivenIAmAccreditedSystem(string fromASID)
        {
            _headerController.ReplaceHeader(HttpConst.Headers.SspFrom, fromASID);
        }

        [Given(@"I am performing the ""(.*)"" interaction")]
        public void GivenIAmPerformingTheInteraction(string interactionId)
        {
            _headerController.ReplaceHeader(HttpConst.Headers.SspInteractionId, interactionId);
        }

        [Given(@"I am connecting to accredited system ""(.*)""")]
        public void GivenIConnectingToAccreditedSystem(string toASID)
        {
            _headerController.ReplaceHeader(HttpConst.Headers.SspTo, toASID);
        }

        [Given(@"I am generating a random message trace identifier")]
        public void GivenIAmGeneratingARandomMessageTraceIdentifier()
        {
            _headerController.ReplaceHeader(HttpConst.Headers.SspTraceID, Guid.NewGuid().ToString(""));
        }

        [Given(@"I am generating an organization JWT header")]
        public void GivenIAmGeneratingAnOrganizationAuthorizationHeader()
        {
            _jwtHelper.SetJwtDefaultValues();
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I am generating a patient JWT header with nhs number ""(.*)""")]
        public void GivenIAmGeneratingAPatientAuthorizationHeader(string nhsNumber)
        {
            _jwtHelper.SetJwtDefaultValues();
            _jwtHelper.RequestedPatientNHSNumber = nhsNumber;
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I do not send header ""(.*)""")]
        public void GivenIDoNotSendHeader(string headerKey)
        {
            _headerController.RemoveHeader(headerKey);
        }


        // Generic Request Steps

        [When(@"I make a GET request to ""(.*)""")]
        public void WhenIMakeAGETRequestTo(string relativeUrl)
        {
            RestRequest(Method.GET, relativeUrl);
        }

        [When(@"I make a POST request to ""(.*)""")]
        public void WhenIMakeAPOSTRequestTo(string relativeUrl)
        {
            RestRequest(Method.POST, relativeUrl);
        }

        [When(@"I make a PUT request to ""(.*)""")]
        public void WhenIMakeAPUTRequestTo(string relativeUrl)
        {
            RestRequest(Method.PUT, relativeUrl);
        }

        [When(@"I make a PATCH request to ""(.*)""")]
        public void WhenIMakeAPATCHRequestTo(string relativeUrl)
        {
            RestRequest(Method.PATCH, relativeUrl);
        }

        [When(@"I make a DELETE request to ""(.*)""")]
        public void WhenIMakeADELETERequestTo(string relativeUrl)
        {
            RestRequest(Method.DELETE, relativeUrl);
        }

        [When(@"I make a OPTIONS request to ""(.*)""")]
        public void WhenIMakeAOPTIONSRequestTo(string relativeUrl)
        {
            RestRequest(Method.OPTIONS, relativeUrl);
        }

        public void RestRequest(Method method, string relativeUrl)
        {
            Console.WriteLine("{0} relative Fhir URL = {1}", method, relativeUrl);

            // Build The Rest Request
            var restClient = new RestClient(EndpointHelper.GetProviderURL(_scenarioContext));
            var restRequest = new RestRequest(relativeUrl, method);

            // Setup The Web Proxy
            if (UseWebProxy)
            {
                restClient.Proxy = new WebProxy(new Uri(EndpointHelper.GetWebProxyURL(_scenarioContext), UriKind.Absolute));
            }

            // Setup The Client Certificate
            if (SendClientCert)
            {
                try
                {
                    var clientCertificate = ClientCertificate;
                    if (restClient.ClientCertificates == null)
                    {
                        restClient.ClientCertificates = new X509CertificateCollection();
                    }
                    restClient.ClientCertificates.Clear();
                    restClient.ClientCertificates.Add(clientCertificate);
                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine("No client certificate found in scenario context");
                }
            }

            // Add The Headers
            foreach (var header in _headerController.GetRequestHeaders())
            {
                Console.WriteLine("Header - {0} -> {1}", header.Key, header.Value);
                restRequest.AddHeader(header.Key, header.Value);
            }

            // Execute The Request
            var restResponse = restClient.Execute(restRequest);
            Console.WriteLine("Error Message = " + restResponse.ErrorMessage);
            Console.WriteLine("Error Exception = " + restResponse.ErrorException);

            // Pull Apart The Response
            _scenarioContext.Set(restResponse, Context.RestResponse);
            _scenarioContext.Set(restResponse.StatusCode, Context.ResponseStatusCode);
            Console.WriteLine("Response StatusCode={0}", restResponse.StatusCode);
            _scenarioContext.Set(restResponse.ContentType, Context.ResponseContentType);
            Console.WriteLine("Response ContentType={0}", restResponse.ContentType);
            _scenarioContext.Set(restResponse.Content, Context.ResponseBody);
            Console.WriteLine("Response Content={0}", restResponse.Content);
        }

        // Response Validation Steps

        [Then(@"the response status code should indicate success")]
        public void ThenTheResponseStatusCodeShouldIndicateSuccess()
        {
            ResponseStatusCode.ShouldBe(HttpStatusCode.OK);
            Console.WriteLine("Response HttpStatusCode={0}", ResponseStatusCode);
        }

        [Then(@"the response status code should indicate failure")]
        public void ThenTheResponseStatusCodeShouldIndicateFailure()
        {
            ResponseStatusCode.ShouldNotBe(HttpStatusCode.OK);
            Console.WriteLine("Response HttpStatusCode should not be '{0}' and was '{1}'", HttpStatusCode.OK, ResponseStatusCode);
        }

    }
}
