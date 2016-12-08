using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Xml;
using System.Xml.Linq;
using GPConnect.Provider.AcceptanceTests.Constants;
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
    public interface IHttpSteps
    {
        // Web Proxy
        bool UseWebProxy { get; }
        string WebProxyUrl { get; }
        string WebProxyPort { get; }
        // Spine Proxy
        bool UseSpineProxy { get; }
        string SpineProxyUrl { get; }
        string SpineProxyPort { get; }
        // Jwt
        JwtHelper Jwt { get; }
        // Headers
        HttpHeaderHelper Headers { get; }
        // Raw Response
        string ResponseContentType { get; }
        HttpStatusCode ResponseStatusCode { get; }
        string ResponseBody { get; }
        // Parsed Response
        JObject ResponseJSON { get; }
        XmlDocument ResponseXML { get; }
        // Consumer
        string ConsumerASID { get; }
        // Producer
        string ProviderASID { get; }
    }

    [Binding]
    public class HttpSteps : TechTalk.SpecFlow.Steps, IHttpSteps
    {
        private readonly SecuritySteps _securitySteps;
        private readonly ScenarioContext _scenarioContext;
        
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
            // Raw Response
            public const string ResponseContentType = "responseContentType";
            public const string ResponseStatusCode = "responseStatusCode";
            public const string ResponseBody = "responseBody";
            // Parsed Response
            public const string ResponseJSON = "responseJSON";
            public const string ResponseXML = "responseXML";
            // Consumer
            public const string ConsumerASID = "consumerASID";
            // Producer
            public const string ProviderASID = "providerASID";
        }

        // Web Proxy
        public bool UseWebProxy => _scenarioContext.Get<bool>(HttpSteps.Context.UseWebProxy);
        public string WebProxyUrl => _scenarioContext.Get<string>(Context.WebProxyUrl);
        public string WebProxyPort => _scenarioContext.Get<string>(Context.WebProxyPort);

        // Spine Proxy
        public bool UseSpineProxy => _scenarioContext.Get<bool>(Context.UseSpineProxy);
        public string SpineProxyUrl => _scenarioContext.Get<string>(Context.SpineProxyUrl);
        public string SpineProxyPort => _scenarioContext.Get<string>(Context.SpineProxyPort);

        // Headers Helper
        public HttpHeaderHelper Headers { get; }

        // JWT Helper
        public JwtHelper Jwt { get; }

        // Raw Response
        public string ResponseContentType => _scenarioContext.Get<string>(Context.ResponseContentType);
        public HttpStatusCode ResponseStatusCode => _scenarioContext.Get<HttpStatusCode>(Context.ResponseStatusCode);
        public string ResponseBody => _scenarioContext.Get<string>(Context.ResponseBody);

        // Parsed Response
        public JObject ResponseJSON => _scenarioContext.Get<JObject>(Context.ResponseJSON);
        public XmlDocument ResponseXML => _scenarioContext.Get<XmlDocument>(Context.ResponseXML);

        // Consumer
        public string ConsumerASID => _scenarioContext.Get<string>(Context.ConsumerASID);

        // Provider
        public string ProviderASID => _scenarioContext.Get<string>(Context.ProviderASID);

        // Constructor

        public HttpSteps(ScenarioContext scenarioContext, SecuritySteps securitySteps, HttpHeaderHelper headerHelper, JwtHelper jwtHelper)
        {
            Log.WriteLine("HttpSteps() Constructor");
            _scenarioContext = scenarioContext;
            _securitySteps = securitySteps;
            Headers = headerHelper;
            Jwt = jwtHelper;
        }

        // Security Validation Steps

        [Then(@"the response status code should indicate authentication failure")]
        public void ThenTheResponseStatusCodeShouldIndicateAuthenticationFailure()
        {
            ResponseStatusCode.ShouldBe(HttpStatusCode.Forbidden);
            Log.WriteLine("Response HttpStatusCode={0}", ResponseStatusCode);
        }

        [Then(@"the response status code should be ""(.*)""")]
        public void ThenTheResponseStatusCodeShouldBe(string statusCode)
        {
            ResponseStatusCode.ToString().ShouldBe(statusCode);
            Log.WriteLine("Response HttpStatusCode should be {0} but was {1}", statusCode, ResponseStatusCode);
        }

        // Provider Configuration Steps

        [Given(@"I am using the default server")]
        public void GivenIAmUsingTheDefaultServer()
        {
            // Load The Default Settings From The App.config File
            AppSettingsHelper.LoadAppSettings(_scenarioContext);

            Given(@"I configure server certificate and ssl");
            And($@"I am using ""{FhirConst.ContentTypes.JsonFhir}"" to communicate with the server");
            And(@"I am generating a random message trace identifier");
            And($@"I am accredited system ""{ConsumerASID}""");
            And($@"I am connecting to accredited system ""{ProviderASID}""");
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

        // Http Header Configuration Steps

        [Given(@"I am using ""(.*)"" to communicate with the server")]
        public void GivenIAmUsingToCommunicateWithTheServer(string requestContentType)
        {
            Headers.ReplaceHeader(HttpConst.Headers.Accept, requestContentType);
        }

        [Given(@"I set ""(.*)"" request header to ""(.*)""")]
        public void GivenISetRequestHeaderTo(string headerKey, string headerValue)
        {
            Headers.ReplaceHeader(headerKey, headerValue);
        }

        [Given(@"I am accredited system ""(.*)""")]
        public void GivenIAmAccreditedSystem(string fromASID)
        {
            Headers.ReplaceHeader(HttpConst.Headers.SspFrom, fromASID);
        }

        [Given(@"I am performing the ""(.*)"" interaction")]
        public void GivenIAmPerformingTheInteraction(string interactionId)
        {
            Headers.ReplaceHeader(HttpConst.Headers.SspInteractionId, interactionId);
        }

        [Given(@"I am connecting to accredited system ""(.*)""")]
        public void GivenIConnectingToAccreditedSystem(string toASID)
        {
            Headers.ReplaceHeader(HttpConst.Headers.SspTo, toASID);
        }

        [Given(@"I am generating a random message trace identifier")]
        public void GivenIAmGeneratingARandomMessageTraceIdentifier()
        {
            Headers.ReplaceHeader(HttpConst.Headers.SspTraceID, Guid.NewGuid().ToString(""));
        }

        [Given(@"I am generating an organization JWT header")]
        public void GivenIAmGeneratingAnOrganizationAuthorizationHeader()
        {
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I am generating a patient JWT header with nhs number ""(.*)""")]
        public void GivenIAmGeneratingAPatientAuthorizationHeader(string nhsNumber)
        {
            Jwt.RequestedPatientNHSNumber = nhsNumber;
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I do not send header ""(.*)""")]
        public void GivenIDoNotSendHeader(string headerKey)
        {
            Headers.RemoveHeader(headerKey);
        }

        // Http Request Steps

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

        private void RestRequest(Method method, string relativeUrl)
        {
            Log.WriteLine("{0} relative Fhir URL = {1}", method, relativeUrl);

            // Build The Rest Request
            var restClient = new RestClient(EndpointHelper.GetProviderURL(_scenarioContext));
            var restRequest = new RestRequest(relativeUrl, method);

            // Setup The Web Proxy
            if (UseWebProxy)
            {
                restClient.Proxy = new WebProxy(new Uri(EndpointHelper.GetWebProxyURL(_scenarioContext), UriKind.Absolute));
            }

            // Setup The Client Certificate
            if (_securitySteps.SendClientCert)
            {
                try
                {
                    var clientCert = _securitySteps.ClientCert;
                    if (restClient.ClientCertificates == null)
                    {
                        restClient.ClientCertificates = new X509CertificateCollection();
                    }
                    restClient.ClientCertificates.Clear();
                    restClient.ClientCertificates.Add(clientCert);
                }
                catch (KeyNotFoundException)
                {
                    Log.WriteLine("No client certificate found in scenario context");
                }
            }

            // Add The Headers
            foreach (var header in Headers.GetRequestHeaders())
            {
                Log.WriteLine("Header - {0} -> {1}", header.Key, header.Value);
                restRequest.AddHeader(header.Key, header.Value);
            }

            // Execute The Request
            var restResponse = restClient.Execute(restRequest);
            Log.WriteLine("Error Message = " + restResponse.ErrorMessage);
            Log.WriteLine("Error Exception = " + restResponse.ErrorException);

            // Pull Apart The Response
            _scenarioContext.Set(restResponse.StatusCode, Context.ResponseStatusCode);
            Log.WriteLine("Response StatusCode={0}", restResponse.StatusCode);
            _scenarioContext.Set(restResponse.ContentType, Context.ResponseContentType);
            Log.WriteLine("Response ContentType={0}", restResponse.ContentType);
            _scenarioContext.Set(restResponse.Content, Context.ResponseBody);
            Log.WriteLine("Response Body={0}", restResponse.Content);

            // TODO Parse The XML or JSON For Easier Processing
        }

        // Response Validation Steps

        [Then(@"the response status code should indicate success")]
        public void ThenTheResponseStatusCodeShouldIndicateSuccess()
        {
            ResponseStatusCode.ShouldBe(HttpStatusCode.OK);
            Log.WriteLine("Response HttpStatusCode={0}", ResponseStatusCode);
        }

        [Then(@"the response status code should indicate failure")]
        public void ThenTheResponseStatusCodeShouldIndicateFailure()
        {
            ResponseStatusCode.ShouldNotBe(HttpStatusCode.OK);
            Log.WriteLine("Response HttpStatusCode should not be '{0}' and was '{1}'", HttpStatusCode.OK, ResponseStatusCode);
        }

        [Then(@"the response body should be JSON")]
        public void ThenTheResponseBodyShouldBeJSON()
        {
            ResponseContentType.ShouldStartWith(HttpConst.ContentTypes.Json);
            Log.WriteLine("Response ContentType={0}", ResponseContentType);
            _scenarioContext.Set(JObject.Parse(ResponseBody), Context.ResponseJSON);
        }

        [Then(@"the response body should be XML")]
        public void ThenTheResponseBodyShouldBeXML()
        {
            ResponseContentType.ShouldStartWith(HttpConst.ContentTypes.Xml);
            Log.WriteLine("Response ContentType={0}", ResponseContentType);
            _scenarioContext.Set(XDocument.Parse(ResponseBody), Context.ResponseXML);
        }
    }
}
