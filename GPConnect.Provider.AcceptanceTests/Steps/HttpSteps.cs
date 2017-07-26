namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Security.Cryptography.X509Certificates;
    using System.Xml.Linq;
    using Constants;
    using Context;
    using Helpers;
    using Logger;
    using Newtonsoft.Json.Linq;
    using RestSharp;
    using Shouldly;
    using TechTalk.SpecFlow;
    using Hl7.Fhir.Model;
    using System.Text;
    using RestSharp.Extensions.MonoHttp;
    using Enum;
    using Factories;
    using Http;

    [Binding]
    public class HttpSteps : Steps
    {
        private readonly HttpContext _httpContext;
        private readonly JwtHelper _jwtHelper;
        private readonly SecuritySteps _securitySteps;

        public HttpSteps(HttpContext httpContext, JwtHelper jwtHelper, SecuritySteps securitySteps)
        {
            Log.WriteLine("HttpSteps() Constructor");
            _httpContext = httpContext;
            _jwtHelper = jwtHelper;
            _securitySteps = securitySteps;
        }

        // Before Scenarios

        [BeforeScenario(Order = 3)]
        public void LoadAppConfig()
        {
            _httpContext.LoadAppConfig();
        }

        [BeforeScenario(Order = 3)]
        public void ClearHeaders()
        {
            _httpContext.RequestHeaders.Clear();
        }

        [BeforeScenario(Order = 3)]
        public void ClearParameters()
        {
            _httpContext.RequestParameters.ClearParameters();
        }

        // Security Validation Steps

        [Then(@"the response status code should indicate authentication failure")]
        public void ThenTheResponseStatusCodeShouldIndicateAuthenticationFailure()
        {
            _httpContext.ResponseStatusCode.ShouldBe(HttpStatusCode.Forbidden);
            Log.WriteLine("Response HttpStatusCode={0}", _httpContext.ResponseStatusCode);
        }

        [Then(@"the response status code should be ""(.*)""")]
        public void ThenTheResponseStatusCodeShouldBe(string statusCode)
        {
            ((int)_httpContext.ResponseStatusCode).ToString().ShouldBe(statusCode);
            Log.WriteLine("Response HttpStatusCode should be {0} but was {1}", statusCode, _httpContext.ResponseStatusCode);
        }

        // Provider Configuration Steps

        [Given(@"I am using the default server")]
        public void GivenIAmUsingTheDefaultServer()
        {
            // Clear down headers for pre-steps which get resources for use within the test scenario
            _httpContext.RequestHeaders.Clear();
            _httpContext.RequestUrl = "";
            _httpContext.RequestParameters.ClearParameters();
            _httpContext.RequestBody = null;

            _httpContext.RequestParameters.ClearParameters();

            _httpContext.ResponseTimeInMilliseconds = -1;
            //HttpContext.ResponseStatusCode = null;
            _httpContext.ResponseContentType = null;
            _httpContext.ResponseBody = null;
            _httpContext.ResponseHeaders.Clear();
            _httpContext.HttpResponse.Resource = null;

            // Load The Default Settings From The App.config File
            _httpContext.LoadAppConfig();

            Given(@"I configure server certificate and ssl");
            And($@"I am using ""{FhirConst.ContentTypes.kJsonFhir}"" to communicate with the server");
            And(@"I am generating a random message trace identifier");
            And($@"I am accredited system ""{_httpContext.ConsumerASID}""");
            And($@"I am connecting to accredited system ""{_httpContext.ProviderASID}""");
            And(@"I set the default JWT");
        }

        [Given(@"I am connecting to server on port ""([^\s]*)""")]
        public void GivenIAmConnectingToServerOnPort(string serverPort)
        {
            _httpContext.FhirServerPort = serverPort;
        }


        [Given(@"I am not using the SSP")]
        public void GivenIAmNotUsingTheSSP()
        {
            _httpContext.UseSpineProxy = false;
        }

        // Http Header Configuration Steps

        [Given(@"I am using ""(.*)"" to communicate with the server")]
        public void GivenIAmUsingToCommunicateWithTheServer(string requestContentType)
        {
            _httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAccept, requestContentType);
            _httpContext.RequestContentType = requestContentType;
        }

        [Given(@"I set ""(.*)"" request header to ""(.*)""")]
        public void GivenISetRequestHeaderTo(string headerName, string headerValue)
        {
            _httpContext.RequestHeaders.ReplaceHeader(headerName, headerValue);
        }

        [Given(@"I set If-Match request header to ""(.*)""")]
        public void GivenISetRequestHeaderToNotStored(string headerValue)
        {
            _httpContext.RequestHeaders.ReplaceHeader("If-Match", "W/\"" + headerValue + "\"");
        }

        [Given(@"I am accredited system ""(.*)""")]
        public void GivenIAmAccreditedSystem(string fromASID)
        {
            _httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspFrom, fromASID);
        }

        [Given(@"I am performing the ""(.*)"" interaction")]
        public void GivenIAmPerformingTheInteraction(string interactionId)
        {
            _httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, interactionId);
        }

        [Given(@"I am connecting to accredited system ""(.*)""")]
        public void GivenIConnectingToAccreditedSystem(string toASID)
        {
            _httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspTo, toASID);
        }

        [Given(@"I am generating a random message trace identifier")]
        public void GivenIAmGeneratingARandomMessageTraceIdentifier()
        {
            _httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspTraceId, Guid.NewGuid().ToString(""));
        }

        [Given(@"I do not send header ""(.*)""")]
        public void GivenIDoNotSendHeader(string headerKey)
        {
            _httpContext.RequestHeaders.RemoveHeader(headerKey);
        }

        // Http Request Steps
        [Given(@"I set the Accept-Encoding header to gzip")]
        public void SetTheAcceptEncodingHeaderToGzip()
        {
            _httpContext.RequestHeaders.AddHeader(HttpConst.Headers.kAcceptEncoding, "gzip");
        }

        [Given(@"I set the request content type to ""(.*)""")]
        public void GivenISetTheRequestTypeTo(string requestContentType)
        {
            _httpContext.RequestContentType = requestContentType;
        }

        [Given(@"I set the Accept header to ""(.*)""")]
        public void GivenISetTheAcceptHeaderTo(string acceptContentType)
        {
            _httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAccept, acceptContentType);
        }

        [Given(@"I set the Prefer header to ""(.*)""")]
        public void GivenISetThePreferHeaderTo(string preferHeaderContent)
        {
            _httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kPrefer, preferHeaderContent);
        }

        [Given(@"I set the If-None-Match header to ""(.*)""")]
        public void GivenISetTheIfNoneMatchheaderHeaderTo(string ifNoneMatchHeaderContent)
        {
            _httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kIfNoneMatch, ifNoneMatchHeaderContent);
        }

        [Given(@"I set the If-Match header to ""([^""]*)""")]
        public void SetTheIfMatchHeaderTo(string value)
        {
            _httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kIfMatch, value);
        }

        [Given(@"I add the parameter ""(.*)"" with the value ""(.*)""")]
        public void GivenIAddTheParameterWithTheValue(string parameterName, string parameterValue)
        {
            _httpContext.RequestParameters.AddParameter(parameterName, parameterValue);
        }

        [Given(@"I add the parameter ""(.*)"" with the value or sitecode ""(.*)""")]
        public void GivenIAddTheParameterWithTheSiteCode(string parameterName, string parameterValue)
        {
            if (parameterValue.Contains("http://fhir.nhs.net/Id/ods-site-code"))
            {
                var result = parameterValue.LastIndexOf('|');
                var siteCode = parameterValue.Substring(parameterValue.LastIndexOf('|') + 1);
                string mappedSiteValue = GlobalContext.OdsCodeMap[siteCode];
                _httpContext.RequestParameters.AddParameter(parameterName, "http://fhir.nhs.net/Id/ods-site-code|" + mappedSiteValue);
                return;
            }

            _httpContext.RequestParameters.AddParameter(parameterName, parameterValue);
        }

        public Resource getReturnedResourceForRelativeURL(string interactionID, string relativeUrl)
        {
            // Store current state
            var preRequestHeaders = _httpContext.RequestHeaders.GetRequestHeaders();
            _httpContext.RequestHeaders.Clear();
            var preRequestUrl = _httpContext.RequestUrl;
            _httpContext.RequestUrl = "";
            var preRequestParameters = _httpContext.RequestParameters;
            _httpContext.RequestParameters.ClearParameters();
            var preRequestMethod = _httpContext.RequestMethod;
            var preRequestContentType = _httpContext.RequestContentType;
            var preRequestBody = _httpContext.RequestBody;
            _httpContext.RequestBody = null;

            var preResponseTimeInMilliseconds = _httpContext.ResponseTimeInMilliseconds;
            var preResponseStatusCode = _httpContext.ResponseStatusCode;
            var preResponseContentType = _httpContext.ResponseContentType;
            var preResponseBody = _httpContext.ResponseBody;
            var preResponseHeaders = _httpContext.ResponseHeaders;
            _httpContext.ResponseHeaders.Clear();

            JObject preResponseJSON = null;
            try
            {
                preResponseJSON = _httpContext.ResponseJSON;
            }
            catch (Exception) { }
            XDocument preResponseXML = null;
            try
            {
                preResponseXML = _httpContext.ResponseXML;
            }
            catch (Exception) { }

            var preFhirResponseResource = _httpContext.HttpResponse.Resource;

            // Setup configuration
            Given($@"I am using the default server");

            And($@"I set the default JWT");
            And($@"I am performing the ""{interactionID}"" interaction");
            if (relativeUrl.Contains("Patient"))
            {
               string removedSlash = relativeUrl.Replace(@"/", "");
                string patientName = removedSlash.ToLower();
                And($@"I set the JWT requested record NHS number to config patient ""{patientName}""");
                And($@"I set the JWT requested scope to ""patient/*.read""");
            }
            // Make Call
            RestRequest(Method.GET, relativeUrl);

            // Check the response
            _httpContext.ResponseStatusCode.ShouldBe(HttpStatusCode.OK);
            Then($@"the response body should be FHIR JSON"); // Create resource object from returned JSON
            var returnResource = _httpContext.HttpResponse.Resource; // Store the found resource for use in the calling system

            // Restore state
            _httpContext.RequestHeaders.SetRequestHeaders(preRequestHeaders);
            _httpContext.RequestUrl = preRequestUrl;
            _httpContext.RequestParameters = preRequestParameters;
            _httpContext.RequestMethod = preRequestMethod;
            _httpContext.RequestContentType = preRequestContentType;
            _httpContext.RequestBody = preRequestBody;

            _httpContext.ResponseTimeInMilliseconds = preResponseTimeInMilliseconds;
            _httpContext.ResponseStatusCode = preResponseStatusCode;
            _httpContext.ResponseContentType = preResponseContentType;
            _httpContext.ResponseBody = preResponseBody;
            _httpContext.ResponseHeaders = preResponseHeaders;
            _httpContext.ResponseJSON = preResponseJSON;
            _httpContext.ResponseXML = preResponseXML;
            _httpContext.HttpResponse.Resource = preFhirResponseResource;

            return returnResource;
        }

        // Rest Request Helper

        public void RestRequest(Method method, string relativeUrl, string body = null)
        {
            var timer = new System.Diagnostics.Stopwatch();

            Log.WriteLine("{0} relative URL = {1}", method, relativeUrl);

            // Save The Request Details
            _httpContext.RequestMethod = method.ToString();
            _httpContext.RequestUrl = relativeUrl;
            _httpContext.RequestBody = body;

            // Build The Rest Request
            var restClient = new RestClient(_httpContext.EndpointAddress);

            // Setup The Web Proxy
            if (_httpContext.UseWebProxy)
            {
                restClient.Proxy = new WebProxy(new Uri(_httpContext.WebProxyAddress, UriKind.Absolute));
            }

            // Setup The Client Certificate
            if (_httpContext.SecurityContext.SendClientCert)
            {
                var clientCert = _httpContext.SecurityContext.ClientCert;
                if (restClient.ClientCertificates == null)
                {
                    restClient.ClientCertificates = new X509CertificateCollection();
                }
                restClient.ClientCertificates.Clear();
                restClient.ClientCertificates.Add(clientCert);
            }

            // Remove default handlers to stop it sending default Accept header
            restClient.ClearHandlers();

            // Add Parameters
            String requestParamString = "?";
            foreach (var parameter in _httpContext.RequestParameters.GetRequestParameters())
            {
                Log.WriteLine("Parameter - {0} -> {1}", parameter.Key, parameter.Value);
                requestParamString = requestParamString + HttpUtility.UrlEncode(parameter.Key, Encoding.UTF8) + "=" + HttpUtility.UrlEncode(parameter.Value, Encoding.UTF8) + "&";
            }
            requestParamString = requestParamString.Substring(0, requestParamString.Length - 1);

            var restRequest = new RestRequest(relativeUrl + requestParamString, method);

            // Set the Content-Type header
            restRequest.AddParameter(_httpContext.RequestContentType, body, ParameterType.RequestBody);
            _httpContext.RequestHeaders.AddHeader(HttpConst.Headers.kContentType, _httpContext.RequestContentType);

            // Add The Headers
            foreach (var header in _httpContext.RequestHeaders.GetRequestHeaders())
            {
                Log.WriteLine("Header - {0} -> {1}", header.Key, header.Value);
                restRequest.AddHeader(header.Key, header.Value);
            }

            // Execute The Request
            IRestResponse restResponse = null;
            try
            {
                // Start The Performance Timer Running
                timer.Start();

                // Perform The Rest Request
                restResponse = restClient.Execute(restRequest);
            }
            catch (Exception e)
            {
                Log.WriteLine(e.StackTrace);
            }
            finally
            {
                // Always Stop The Performance Timer Running
                timer.Stop();
            }

            // Save The Time Taken To Perform The Request
            _httpContext.ResponseTimeInMilliseconds = timer.ElapsedMilliseconds;

            // TODO Save The Error Message And Exception Details
            Log.WriteLine("Error Message = " + restResponse.ErrorMessage);
            Log.WriteLine("Error Exception = " + restResponse.ErrorException);

            // Save The Response Details
            _httpContext.ResponseStatusCode = restResponse.StatusCode;
            _httpContext.ResponseContentType = restResponse.ContentType;
            _httpContext.ResponseBody = restResponse.Content;

            _httpContext.ResponseHeaders.Clear();
            foreach (var parameter in restResponse.Headers)
            {
                _httpContext.ResponseHeaders.Add(parameter.Name, (string)parameter.Value);
            }
            
        }

        // Response Validation Steps

        [Then(@"the response status code should indicate success")]
        public void ThenTheResponseStatusCodeShouldIndicateSuccess()
        {
            _httpContext.ResponseStatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Then(@"the response status code should indicate created")]
        public void ThenTheResponseStatusCodeShouldIndicateCreated()
        {
            _httpContext.ResponseStatusCode.ShouldBe(HttpStatusCode.Created);
        }

        [Then(@"the response status code should indicate failure")]
        public void ThenTheResponseStatusCodeShouldIndicateFailure()
        {
            _httpContext.ResponseStatusCode.ShouldNotBe(HttpStatusCode.OK);
        }

        [Then(@"the response status code should indicate unsupported media type error")]
        public void ThenTheResponseShouldIndicateUnsupportedMediaTypeError()
        {
            _httpContext.ResponseStatusCode.ShouldBe(HttpStatusCode.UnsupportedMediaType);
            Log.WriteLine("Response HttpStatusCode should be {0} but was {1}", HttpStatusCode.UnsupportedMediaType, _httpContext.ResponseStatusCode);
        }

        [Then(@"the response should be gzip encoded")]
        public void ThenTheResponseShouldBeGZipEncoded()
        {
            bool gZipHeaderFound = false;
            foreach (var header in _httpContext.ResponseHeaders)
            {
                if (header.Key.Equals(HttpConst.Headers.kContentEncoding, StringComparison.CurrentCultureIgnoreCase) && header.Value.Equals("gzip", StringComparison.CurrentCultureIgnoreCase))
                {
                    gZipHeaderFound = true;
                }
            }
            gZipHeaderFound.ShouldBeTrue();
        }

        [Then(@"the response should be chunked")]
        public void ThenReesponseShouldBeChunked()
        {
            bool chunkedHeaderFound = false;
            foreach (var header in _httpContext.ResponseHeaders)
            {
                if (header.Key.Equals(HttpConst.Headers.kTransferEncoding, StringComparison.CurrentCultureIgnoreCase) && header.Value.Equals("chunked", StringComparison.CurrentCultureIgnoreCase))
                {
                    chunkedHeaderFound = true;
                }
            }
            chunkedHeaderFound.ShouldBeTrue();
        }

        //Hayden
        [Given(@"I configure the default ""(.*)"" request")]
        public void ConfigureRequest(GpConnectInteraction interaction)
        {
            var httpContextFactory = new HttpContextFactory(interaction);

            httpContextFactory.ConfigureHttpContext(_httpContext);

            var jwtFactory = new JwtFactory(interaction);

            jwtFactory.ConfigureJwt(_jwtHelper, _httpContext);

            _securitySteps.ConfigureServerCertificatesAndSsl();
        }



        [Given(@"I set the GET request Id to ""([^""]*)""")]
        public void SetTheGetRequestIdTo(string id)
        {
            _httpContext.RequestUrl = "/fhir/Patient/" + id;
        }

        [Given(@"I set the GET request Version Id to ""([^""]*)""")]
        public void SetTheGetRequestVersionIdTo(string versionId)
        {
            _httpContext.CreatedAppointment.VersionId = versionId;
        }

        [Given(@"I set the request Http Method to ""([^""]*)""")]
        public void SetTheRequestHttpMethodTo(string method)
        {
            _httpContext.HttpMethod = new HttpMethod(method);
        }

        [Given(@"I set the request URL to ""([^""]*)""")]
        public void SetTheRequestUrlTo(string url)
        {
            _httpContext.RequestUrl = url;
        }

        [Given(@"I set the Interaction Id header to ""([^""]*)""")]
        public void SetTheInteractionIdHeaderTo(string interactionId)
        {
            _httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, interactionId);
        }

        [Given(@"I set the Read Operation logical identifier used in the request to ""([^""]*)""")]
        public void SetTheReadOperationLogicalIdentifierUsedInTheRequestTo(string logicalId)
        {
            _httpContext.GetRequestId = logicalId;

            var lastIndex = _httpContext.RequestUrl.LastIndexOf('/');

            if (_httpContext.RequestUrl.Contains("$"))
            {
                var action = _httpContext.RequestUrl.Substring(lastIndex);

                var firstIndex = _httpContext.RequestUrl.IndexOf('/');
                var url = _httpContext.RequestUrl.Substring(0, firstIndex + 1);

                _httpContext.RequestUrl = url + _httpContext.GetRequestId + action;
            }
            else
            {
               _httpContext.RequestUrl = _httpContext.RequestUrl.Substring(0, lastIndex + 1) + _httpContext.GetRequestId; 
            }
        }
        
        [Given(@"I set the Read Operation relative path to ""([^""]*)"" and append the resource logical identifier")]
        public void SetTheReadOperationRelativePathToAndAppendTheResourceLogicalIdentifier(string relativePath)
        {
            _httpContext.RequestUrl = relativePath + "/" + _httpContext.GetRequestId;
        }

        [When(@"I make the ""(.*)"" request")]
        public void MakeRequest(GpConnectInteraction interaction)
        {
            var requestFactory = new RequestFactory(interaction);

            requestFactory.ConfigureBody(_httpContext);

            if (!string.IsNullOrEmpty(_httpContext.RequestHeaders.GetHeaderValue(HttpConst.Headers.kAuthorization)))
            {
                _httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerToken());
            }

            var httpRequest = new HttpRequest(_httpContext);

            httpRequest.MakeHttpRequest();
        }

        [When(@"I make the ""(.*)"" request with an unencoded JWT Bearer Token")]
        public void MakeRequestWithAnUnencodedJwtBearerToken(GpConnectInteraction interaction)
        {
            var requestFactory = new RequestFactory(interaction);

            requestFactory.ConfigureBody(_httpContext);

            _httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerTokenWithoutEncoding());

            var httpRequest = new HttpRequest(_httpContext);

            httpRequest.MakeHttpRequest();
        }

        [When(@"I make the ""(.*)"" request with invalid Resource type")]
        public void MakeRequestWithInvalidResourceType(GpConnectInteraction interaction)
        {
            var requestFactory = new RequestFactory(interaction);

            requestFactory.ConfigureBody(_httpContext);
            requestFactory.ConfigureInvalidResourceType(_httpContext);

            _httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerToken());

            var httpRequest = new HttpRequest(_httpContext);

            httpRequest.MakeHttpRequest();
        }

        [When(@"I make the ""(.*)"" request with Invalid Additional Field in the Resource")]
        public void MakeRequestWithInvalidAdditionalFieldInTheResource(GpConnectInteraction interaction)
        {
            var requestFactory = new RequestFactory(interaction);

            requestFactory.ConfigureBody(_httpContext);
            requestFactory.ConfigureAdditionalInvalidFieldInResource(_httpContext);

            _httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerToken());

            var httpRequest = new HttpRequest(_httpContext);

            httpRequest.MakeHttpRequest();
        }

        [When(@"I make the ""(.*)"" request with invalid parameter Resource type")]
        public void MakeRequestWithInvalidParameterResourceType(GpConnectInteraction interaction)
        {
            var requestFactory = new RequestFactory(interaction);
            requestFactory.ConfigureBody(_httpContext);
            requestFactory.ConfigureInvalidParameterResourceType(_httpContext);
            _httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerToken());

            var httpRequest = new HttpRequest(_httpContext);

            httpRequest.MakeHttpRequest();
        }

        [When(@"I make the ""(.*)"" request with additional field in parameter Resource")]
        public void MakeRequestWithAdditionalFieldInParameterResource(GpConnectInteraction interaction)
        {
            var requestFactory = new RequestFactory(interaction);
            requestFactory.ConfigureBody(_httpContext);
            requestFactory.ConfigureParameterResourceWithAdditionalField(_httpContext);
            _httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerToken());

            var httpRequest = new HttpRequest(_httpContext);

            httpRequest.MakeHttpRequest();
        }

        [Given("I set the Decompression Method to gzip")]
        public void SetTheDecompressionMethodToGzip()
        {
            _httpContext.DecompressionMethod = DecompressionMethods.GZip;
        }

        [Then(@"the Response should contain the ETag header matching the Resource Version Id")]
        public void TheResponseShouldContainTheETagHeaderMatchingTheResourceVersionId()
        {
            var versionId = _httpContext.HttpResponse.Resource.VersionId;

            string eTag;
            _httpContext.ResponseHeaders.TryGetValue("ETag", out eTag);

            eTag.ShouldStartWith("W/\"", "The ETag header should start with W/\"");

            eTag.ShouldEndWith(versionId + "\"", "The ETag header should contain the resource version enclosed within speech marks");

            eTag.ShouldBe("W/\"" + versionId + "\"", "The ETag header contains invalid characters");
        }

        [Then(@"the content-type should not be equal to null")]
        public void ThenTheContentTypeShouldNotBeEqualToNull()
        {
            string contentType = null;
            _httpContext.ResponseHeaders.TryGetValue("Content-Type", out contentType);
            contentType.ShouldNotBeNullOrEmpty("The response should contain a Content-Type header.");
        }

        [Then(@"the content-type should be equal to null")]
        public void ThenTheContentTypeShouldBeEqualToZero()
        {
            string contentType = null;
            _httpContext.ResponseHeaders.TryGetValue("Content-Type", out contentType);
            contentType.ShouldBe(null, "There should not be a content-type header on the response");
        }

        [Then(@"the content-length should not be equal to zero")]
        public void ThenTheContentLengthShouldNotBeEqualToZero()
        {
            string contentLength = "";
            _httpContext.ResponseHeaders.TryGetValue("Content-Length", out contentLength);
            contentLength.ShouldNotBe("0", "The response payload should contain a resource.");
        }
    }
}
