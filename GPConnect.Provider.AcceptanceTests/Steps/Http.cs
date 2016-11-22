using System;
using System.Net;
using RestSharp;
using Shouldly;
using TechTalk.SpecFlow;
using System.Collections.Generic;
using GPConnect.Provider.AcceptanceTests.tools;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;

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

        [Given(@"I am using the default server")]
        public void GivenIAmUsingTheDefaultServer()
        {
            // Default to HTTP or HTTPS
            if ("true".Equals(ConfigurationManager.AppSettings["useTLS"]))
            {
                _scenarioContext.Set(true, "useTLS");
            }
            else {
                _scenarioContext.Set(false, "useTLS");
            }

            // Fhir Server Details from config file
            _scenarioContext.Set(ConfigurationManager.AppSettings["fhirServerUrl"], "serverUrl");
            _scenarioContext.Set(ConfigurationManager.AppSettings["fhirServerPort"], "serverPort");
            _scenarioContext.Set(ConfigurationManager.AppSettings["fhirServerFhirBase"], "fhirServerFhirBase");

            // Spine Proxy Details from config file
            _scenarioContext.Set(false, "useSpineProxy");
            _scenarioContext.Set(ConfigurationManager.AppSettings["spineProxyUrl"], "spineProxyUrl");
            _scenarioContext.Set(ConfigurationManager.AppSettings["spineProxyPort"], "spineProxyPort");

            // Certificates
            _scenarioContext.Set(ConfigurationManager.AppSettings["clientCertThumbPrint"], "clientCertThumbPrint");
            _scenarioContext.Set(true, "sendClientCert");
            _scenarioContext.Set(true, "validateServerCert");

            Given(@"I configure server certificate and ssl");
            And(@"I am using ""application/json+fhir"" to communicate with the server");
            And(@"I set base URL to ""/fhir""");
            And(@"I am accredited system ""200000000359""");
            And(@"I am connecting to accredited system ""200000000360""");
            And(@"I am generating a random message trace identifier");
            And(@"I am generating an organization JWT header");
        }


        [Given(@"I am using server ""([^\s]*)""")]
        public void GivenIAmUsingServer(string serverUrl)
        {
            _scenarioContext.Set(serverUrl, "serverUrl");
            _scenarioContext.Set(ConfigurationManager.AppSettings["fhirServerPort"], "serverPort");
            _scenarioContext.Set(ConfigurationManager.AppSettings["fhirServerFhirBase"], "fhirServerFhirBase");
        }

        [Given(@"I am using server ""([^\s]*)"" on port ""([^\s]*)""")]
        public void GivenIAmUsingServerOnPort(string serverUrl, string serverPort)
        {
            _scenarioContext.Set(serverUrl, "serverUrl");
            _scenarioContext.Set(serverPort, "serverPort");
            _scenarioContext.Set(ConfigurationManager.AppSettings["fhirServerFhirBase"], "fhirServerFhirBase");
        }

        [Given(@"I am connecting to server on port ""([^\s]*)""")]
        public void GivenIAmConnectingToServerOnPort(string serverPort)
        {
            _scenarioContext.Set(serverPort, "serverPort");
        }

        [Given(@"I set base URL to ""([^\s]*)""")]
        public void GivenISetBaseURLTo(string baseUrl)
        {
            _scenarioContext.Set(baseUrl, "fhirServerFhirBase");
        }


        // Spine Proxy Configuration Steps

        [Given(@"I am not using the spine proxy server")]
        public void GivenIAmNotUsingTheSpineProxyServer()
        {
            _scenarioContext.Set(false, "useSpineProxy");
        }

        [Given(@"I am using the spine proxy server ""([^\s]*)""")]
        public void GivenIAmUsingTheSpineProxyServer(string proxyServerUrl)
        {
            _scenarioContext.Set(true, "useSpineProxy");
            _scenarioContext.Set(proxyServerUrl, "spineProxyUrl");
            _scenarioContext.Set(ConfigurationManager.AppSettings["spineProxyPort"], "spineProxyPort");
        }

        [Given(@"I am using the spine proxy server ""([^\s]*)"" on port ""([^\s]*)""")]
        public void GivenIAmUsingTheSpineProxyServerOnPort(string proxyServerUrl, string proxyServerPort)
        {
            _scenarioContext.Set(true, "useSpineProxy");
            _scenarioContext.Set(proxyServerUrl, "spineProxyUrl");
            _scenarioContext.Set(proxyServerPort, "spineProxyPort");
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

        [Given(@"I am generating an organization JWT header")]
        public void GivenIAmGeneratingAnOrganizationAuthorizationHeader()
        {
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenOrgResource());
        }

        [Given(@"I am generating a patient JWT header with nhs number ""(.*)""")]
        public void GivenIAmGeneratingAPatientAuthorizationHeader(string nhsNumber)
        {
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenPatientResource(nhsNumber));
        }


        // Generic Request Steps

        [When(@"I make a GET request to ""(.*)""")]
        public void WhenIMakeAGETRequestTo(string relativeUrl)
        {
            // Build The Request

            string httpProtocol = _scenarioContext.Get<bool>("useTLS") ? "https://" : "http://";
            string spineProxyUrl = _scenarioContext.Get<bool>("useSpineProxy") ? spineProxyUrl = httpProtocol + _scenarioContext.Get<string>("spineProxyUrl") + ":" + _scenarioContext.Get<string>("spineProxyPort") + "/" : "";
            string serverUrl = httpProtocol + _scenarioContext.Get<string>("serverUrl") + ":" + _scenarioContext.Get<string>("serverPort") + _scenarioContext.Get<string>("fhirServerFhirBase");

            Console.WriteLine("SpineProxyURL = " + spineProxyUrl);
            Console.WriteLine("ServerURL = " + serverUrl);

            var restClient = new RestClient(spineProxyUrl + serverUrl);

            Console.Out.WriteLine("GET relative Fhir URL = {0}", relativeUrl);
            var restRequest = new RestRequest(relativeUrl, Method.GET);

            if (_scenarioContext.Get<bool>("sendClientCert")) {
                try
                {
                    X509Certificate2 clientCertificate = _scenarioContext.Get<X509Certificate2>("clientCertificate");
                    if (restClient.ClientCertificates == null) {
                        restClient.ClientCertificates = new X509CertificateCollection();
                    }
                    restClient.ClientCertificates.Add(clientCertificate);
                }
                catch (KeyNotFoundException e) {
                    // No client certificate found in scenario context
                    Console.WriteLine("No client certificate found in scenario context");
                }
            }

            // Add Headers
            foreach (KeyValuePair<string, string> header in _headerController.getRequestHeaders())
            {
                Console.WriteLine("Header - {0} -> {1}", header.Key, header.Value);
                restRequest.AddHeader(header.Key, header.Value);
            }
            
            // Execute The Request
            var restResponse = restClient.Execute(restRequest);
            Console.WriteLine("Error Message = " + restResponse.ErrorMessage);
            Console.WriteLine("Error Exception = " + restResponse.ErrorException);
            
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
            Console.Out.WriteLine("Response HttpStatusCode={0}", _scenarioContext.Get<HttpStatusCode>("responseStatusCode"));
        }

        [Then(@"the response status code should indicate failure")]
        public void ThenTheResponseStatusCodeShouldIndicateFailure()
        {
            Console.Out.WriteLine("Response HttpStatusCode should not be '{0}' but was '{1}'", HttpStatusCode.OK, _scenarioContext.Get<HttpStatusCode>("responseStatusCode"));
            _scenarioContext.Get<HttpStatusCode>("responseStatusCode").ShouldNotBe(HttpStatusCode.OK);
        }

    }
}
