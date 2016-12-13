using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Logger;
using Newtonsoft.Json.Linq;
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
        private readonly HttpContext HttpContext;
        
        // Constructor

        public HttpSteps(HttpContext httpContext)
        {
            Log.WriteLine("HttpSteps() Constructor");
            HttpContext = httpContext;
        }

        // Before Scenarios

        [BeforeScenario(Order = 3)]
        public void LoadAppConfig()
        {
            HttpContext.LoadAppConfig();
        }
        
        [BeforeScenario(Order = 3)]
        public void ClearHeaders()
        {
            HttpContext.Headers.Clear();
        }

        // Security Validation Steps

        [Then(@"the response status code should indicate authentication failure")]
        public void ThenTheResponseStatusCodeShouldIndicateAuthenticationFailure()
        {
            HttpContext.ResponseStatusCode.ShouldBe(HttpStatusCode.Forbidden);
            Log.WriteLine("Response HttpStatusCode={0}", HttpContext.ResponseStatusCode);
        }

        [Then(@"the response status code should be ""(.*)""")]
        public void ThenTheResponseStatusCodeShouldBe(string statusCode)
        {
            ((int)HttpContext.ResponseStatusCode).ToString().ShouldBe(statusCode);
            Log.WriteLine("Response HttpStatusCode should be {0} but was {1}", statusCode, HttpContext.ResponseStatusCode);
        }

        // Provider Configuration Steps

        [Given(@"I am using the default server")]
        public void GivenIAmUsingTheDefaultServer()
        {
            // Load The Default Settings From The App.config File
            HttpContext.LoadAppConfig();            

            Given(@"I configure server certificate and ssl");
            And($@"I am using ""{FhirConst.ContentTypes.JsonFhir}"" to communicate with the server");
            And(@"I am generating a random message trace identifier");
            And($@"I am accredited system ""{HttpContext.ConsumerASID}""");
            And($@"I am connecting to accredited system ""{HttpContext.ProviderASID}""");
            And(@"I am generating an organization JWT header");
        }

        [Given(@"I am using server ""([^\s]*)""")]
        public void GivenIAmUsingServer(string serverUrl)
        {
            HttpContext.FhirServerUrl = serverUrl;
        }

        [Given(@"I am using server ""([^\s]*)"" on port ""([^\s]*)""")]
        public void GivenIAmUsingServerOnPort(string serverUrl, string serverPort)
        {
            HttpContext.FhirServerUrl = serverUrl;
            HttpContext.FhirServerPort = serverPort;
        }

        [Given(@"I am connecting to server on port ""([^\s]*)""")]
        public void GivenIAmConnectingToServerOnPort(string serverPort)
        {
            HttpContext.FhirServerPort = serverPort;
        }

        [Given(@"I set base URL to ""([^\s]*)""")]
        public void GivenISetBaseURLTo(string baseUrl)
        {
            HttpContext.FhirServerFhirBase = baseUrl;
        }

        // Web Proxy Configuration Steps

        [Given(@"I am not using the web proxy server")]
        public void GivenIAmNotUsingTheWebProxyServer()
        {
            HttpContext.UseWebProxy = false;
        }

        [Given(@"I am using the web proxy server ""([^\s]*)""")]
        public void GivenIAmUsingTheWebProxyServer(string webServerUrl)
        {
            HttpContext.UseWebProxy = true;
            HttpContext.WebProxyUrl = webServerUrl;
        }

        [Given(@"I am using the web proxy server ""([^\s]*)"" on port ""([^\s]*)""")]
        public void GivenIAmUsingTheWebProxyServerOnPort(string proxyServerUrl, string proxyServerPort)
        {
            HttpContext.UseSpineProxy = true;
            HttpContext.SpineProxyUrl = proxyServerUrl;
            HttpContext.SpineProxyPort = proxyServerPort;
        }

        // Spine Proxy Configuration Steps

        [Given(@"I am not using the spine proxy server")]
        public void GivenIAmNotUsingTheSpineProxyServer()
        {
            HttpContext.UseSpineProxy = false;
        }

        [Given(@"I am using the spine proxy server ""([^\s]*)""")]
        public void GivenIAmUsingTheSpineProxyServer(string proxyServerUrl)
        {
            HttpContext.UseSpineProxy = true;
            HttpContext.SpineProxyUrl = proxyServerUrl;
        }

        [Given(@"I am using the spine proxy server ""([^\s]*)"" on port ""([^\s]*)""")]
        public void GivenIAmUsingTheSpineProxyServerOnPort(string proxyServerUrl, string proxyServerPort)
        {
            HttpContext.UseSpineProxy = true;
            HttpContext.SpineProxyUrl = proxyServerUrl;
            HttpContext.SpineProxyPort = proxyServerPort;
        }

        // Http Header Configuration Steps

        [Given(@"I am using ""(.*)"" to communicate with the server")]
        public void GivenIAmUsingToCommunicateWithTheServer(string requestContentType)
        {
            HttpContext.Headers.ReplaceHeader(HttpConst.Headers.Accept, requestContentType);
            HttpContext.RequestContentType = requestContentType;
        }

        [Given(@"I set ""(.*)"" request header to ""(.*)""")]
        public void GivenISetRequestHeaderTo(string headerKey, string headerValue)
        {
            HttpContext.Headers.ReplaceHeader(headerKey, headerValue);
        }

        [Given(@"I am accredited system ""(.*)""")]
        public void GivenIAmAccreditedSystem(string fromASID)
        {
            HttpContext.Headers.ReplaceHeader(HttpConst.Headers.SspFrom, fromASID);
        }

        [Given(@"I am performing the ""(.*)"" interaction")]
        public void GivenIAmPerformingTheInteraction(string interactionId)
        {
            HttpContext.Headers.ReplaceHeader(HttpConst.Headers.SspInteractionId, interactionId);
        }

        [Given(@"I am connecting to accredited system ""(.*)""")]
        public void GivenIConnectingToAccreditedSystem(string toASID)
        {
            HttpContext.Headers.ReplaceHeader(HttpConst.Headers.SspTo, toASID);
        }

        [Given(@"I am generating a random message trace identifier")]
        public void GivenIAmGeneratingARandomMessageTraceIdentifier()
        {
            HttpContext.Headers.ReplaceHeader(HttpConst.Headers.SspTraceID, Guid.NewGuid().ToString(""));
        }

        [Given(@"I am generating an organization JWT header")]
        public void GivenIAmGeneratingAnOrganizationAuthorizationHeader()
        {
            HttpContext.Headers.ReplaceHeader(HttpConst.Headers.Authorization, HttpContext.Jwt.GetBearerToken());
        }

        [Given(@"I am generating a patient JWT header with nhs number ""(.*)""")]
        public void GivenIAmGeneratingAPatientAuthorizationHeader(string nhsNumber)
        {
            HttpContext.Jwt.RequestedPatientNHSNumber = nhsNumber;
            HttpContext.Headers.ReplaceHeader(HttpConst.Headers.Authorization, HttpContext.Jwt.GetBearerToken());
        }

        [Given(@"I do not send header ""(.*)""")]
        public void GivenIDoNotSendHeader(string headerKey)
        {
            HttpContext.Headers.RemoveHeader(headerKey);
        }

        // Http Request Steps

        [Given(@"I set a JSON request body ""(.*)""")]
        public void GivenISetAJSONRequestBody(string body)
        {
            HttpContext.RequestContentType = HttpConst.ContentTypes.Json;
            HttpContext.RequestBody = body;
        }

        [Given(@"I set an XML request body ""(.*)""")]
        public void GivenISetAnXMLRequestBody(string body)
        {
            HttpContext.RequestContentType = HttpConst.ContentTypes.Xml;
            HttpContext.RequestBody = body;
        }

        [Given(@"I set the request content type to ""(.*)""")]
        public void GivenISetTheRequestTypeTo(string requestContentType)
        {
            HttpContext.RequestContentType = requestContentType;
        }

        [Given(@"I set the Accept header to ""(.*)""")]
        public void GivenISetTheAcceptHeaderTo(string acceptContentType)
        {
            HttpContext.Headers.ReplaceHeader(HttpConst.Headers.Accept, acceptContentType);
        }

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

        // Rest Request Helper

        private void RestRequest(Method method, string relativeUrl, string body = null)
        {
            Log.WriteLine("{0} relative URL = {1}", method, relativeUrl);

            // Save The Request Details
            HttpContext.RequestMethod = method.ToString();
            HttpContext.RequestUrl = relativeUrl;
            HttpContext.RequestBody = body;

            // Build The Rest Request
            var restClient = new RestClient(HttpContext.EndpointAddress);
            var restRequest = new RestRequest(relativeUrl, method);
            restRequest.AddParameter(HttpConst.ContentTypes.Json, body, ParameterType.RequestBody);

            // Setup The Web Proxy
            if (HttpContext.UseWebProxy)
            {
                restClient.Proxy = new WebProxy(new Uri(HttpContext.WebProxyAddress, UriKind.Absolute));
            }

            // Setup The Client Certificate
            if (HttpContext.SecurityContext.SendClientCert)
            {
                var clientCert = HttpContext.SecurityContext.ClientCert;
                if (restClient.ClientCertificates == null)
                {
                    restClient.ClientCertificates = new X509CertificateCollection();
                }
                restClient.ClientCertificates.Clear();
                restClient.ClientCertificates.Add(clientCert);
            }

            // Add The Headers
            foreach (var header in HttpContext.Headers.GetRequestHeaders())
            {
                Log.WriteLine("Header - {0} -> {1}", header.Key, header.Value);
                restRequest.AddHeader(header.Key, header.Value);
            }

            // Execute The Request
            var restResponse = restClient.Execute(restRequest);

            // TODO Save The Error Message And Exception Details
            Log.WriteLine("Error Message = " + restResponse.ErrorMessage);
            Log.WriteLine("Error Exception = " + restResponse.ErrorException);

            // Save The Response Details
            HttpContext.ResponseStatusCode = restResponse.StatusCode;
            HttpContext.ResponseContentType = restResponse.ContentType;
            HttpContext.ResponseBody = restResponse.Content;

            // TODO Parse The XML or JSON For Easier Processing

            LogResponseToDisk();
        }

        // Response Validation Steps

        [Then(@"the response status code should indicate success")]
        public void ThenTheResponseStatusCodeShouldIndicateSuccess()
        {
            HttpContext.ResponseStatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Then(@"the response status code should indicate failure")]
        public void ThenTheResponseStatusCodeShouldIndicateFailure()
        {
            HttpContext.ResponseStatusCode.ShouldNotBe(HttpStatusCode.OK);
        }

        [Then(@"the response status code should indicate unsupported media type error")]
        public void ThenTheResponseShouldIndicateUnsupportedMediaTypeError()
        {
            HttpContext.ResponseStatusCode.ShouldBe(HttpStatusCode.UnsupportedMediaType);
            Log.WriteLine("Response HttpStatusCode should be {0} but was {1}", HttpStatusCode.UnsupportedMediaType, HttpContext.ResponseStatusCode);
        }

        [Then(@"the response body should be JSON")]
        public void ThenTheResponseBodyShouldBeJSON()
        {
            HttpContext.ResponseContentType.ShouldStartWith(HttpConst.ContentTypes.Json);
            HttpContext.ResponseJSON = JObject.Parse(HttpContext.ResponseBody);
        }

        [Then(@"the response body should be XML")]
        public void ThenTheResponseBodyShouldBeXML()
        {
            HttpContext.ResponseContentType.ShouldStartWith(HttpConst.ContentTypes.Xml);
            HttpContext.ResponseXML = XDocument.Parse(HttpContext.ResponseBody);
        }

        // Logger

        private void LogResponseToDisk()
        {
            var traceDirectory = GlobalContext.GetValue<string>(GlobalConst.Trace.TraceDirectory);
            if (!Directory.Exists(traceDirectory)) return;
            var scenarioDirectory = Path.Combine(traceDirectory, HttpContext.ScenarioContext.ScenarioInfo.Title);
            Directory.CreateDirectory(scenarioDirectory);
            Log.WriteLine(scenarioDirectory);
        }
    }
}
