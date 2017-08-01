namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System;
    using System.Net;
    using System.Net.Http;
    using Constants;
    using Context;
    using Helpers;
    using Logger;
    using Shouldly;
    using TechTalk.SpecFlow;
    using Hl7.Fhir.Model;
    using Enum;
    using Factories;
    using Http;

    [Binding]
    public class HttpSteps : Steps
    {
        private readonly HttpContext _httpContext;
        private readonly JwtHelper _jwtHelper;
        private readonly SecuritySteps _securitySteps;
        private readonly SecurityContext _securityContext;

        public HttpSteps(HttpContext httpContext, JwtHelper jwtHelper, SecuritySteps securitySteps, SecurityContext securityContext)
        {
            Log.WriteLine("HttpSteps() Constructor");
            _httpContext = httpContext;
            _jwtHelper = jwtHelper;
            _securitySteps = securitySteps;
            _securityContext = securityContext;
        }

        // Before Scenarios

        [BeforeScenario(Order = 3)]
        public void LoadAppConfig()
        {
            _httpContext.HttpRequestConfiguration.LoadAppConfig();
        }

        [BeforeScenario(Order = 3)]
        public void ClearHeaders()
        {
            _httpContext.HttpRequestConfiguration.RequestHeaders.Clear();
        }

        [BeforeScenario(Order = 3)]
        public void ClearParameters()
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.ClearParameters();
        }

        // Security Validation Steps

        [Then(@"the response status code should indicate authentication failure")]
        public void ThenTheResponseStatusCodeShouldIndicateAuthenticationFailure()
        {
            _httpContext.HttpResponse.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
            Log.WriteLine("Response HttpStatusCode={0}", _httpContext.HttpResponse.StatusCode);
        }

        [Then(@"the response status code should be ""(.*)""")]
        public void ThenTheResponseStatusCodeShouldBe(string statusCode)
        {
            ((int)_httpContext.HttpResponse.StatusCode).ToString().ShouldBe(statusCode);
            Log.WriteLine("Response HttpStatusCode should be {0} but was {1}", statusCode, _httpContext.HttpResponse.StatusCode);
        }

        [Given(@"I am connecting to server on port ""([^\s]*)""")]
        public void GivenIAmConnectingToServerOnPort(string serverPort)
        {
            _httpContext.HttpRequestConfiguration.FhirServerPort = serverPort;
        }


        [Given(@"I am not using the SSP")]
        public void GivenIAmNotUsingTheSsp()
        {
            _httpContext.HttpRequestConfiguration.UseSpineProxy = false;
        }

        // Http Header Configuration Steps

        [Given(@"I set ""(.*)"" request header to ""(.*)""")]
        public void GivenISetRequestHeaderTo(string headerName, string headerValue)
        {
            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(headerName, headerValue);
        }

        [Given(@"I set If-Match request header to ""(.*)""")]
        public void GivenISetRequestHeaderToNotStored(string headerValue)
        {
            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader("If-Match", "W/\"" + headerValue + "\"");
        }

        [Given(@"I am performing the ""(.*)"" interaction")]
        public void GivenIAmPerformingTheInteraction(string interactionId)
        {
            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, interactionId);
        }

        [Given(@"I am connecting to accredited system ""(.*)""")]
        public void GivenIConnectingToAccreditedSystem(string toASID)
        {
            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspTo, toASID);
        }

        [Given(@"I am generating a random message trace identifier")]
        public void GivenIAmGeneratingARandomMessageTraceIdentifier()
        {
            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspTraceId, Guid.NewGuid().ToString(""));
        }

        [Given(@"I do not send header ""(.*)""")]
        public void GivenIDoNotSendHeader(string headerKey)
        {
            _httpContext.HttpRequestConfiguration.RequestHeaders.RemoveHeader(headerKey);
        }

        // Http Request Steps
        [Given(@"I set the Accept-Encoding header to gzip")]
        public void SetTheAcceptEncodingHeaderToGzip()
        {
            _httpContext.HttpRequestConfiguration.RequestHeaders.AddHeader(HttpConst.Headers.kAcceptEncoding, "gzip");
        }

        [Given(@"I set the request content type to ""(.*)""")]
        public void GivenISetTheRequestTypeTo(string requestContentType)
        {
            _httpContext.HttpRequestConfiguration.RequestContentType = requestContentType;
        }

        [Given(@"I set the Accept header to ""(.*)""")]
        public void GivenISetTheAcceptHeaderTo(string acceptContentType)
        {
            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAccept, acceptContentType);
        }

        [Given(@"I set the Prefer header to ""(.*)""")]
        public void GivenISetThePreferHeaderTo(string preferHeaderContent)
        {
            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kPrefer, preferHeaderContent);
        }

        [Given(@"I set the If-None-Match header to ""(.*)""")]
        public void GivenISetTheIfNoneMatchheaderHeaderTo(string ifNoneMatchHeaderContent)
        {
            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kIfNoneMatch, ifNoneMatchHeaderContent);
        }

        [Given(@"I set the If-Match header to ""([^""]*)""")]
        public void SetTheIfMatchHeaderTo(string value)
        {
            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kIfMatch, value);
        }

        [Given(@"I add the parameter ""(.*)"" with the value ""(.*)""")]
        public void GivenIAddTheParameterWithTheValue(string parameterName, string parameterValue)
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter(parameterName, parameterValue);
        }

        [Given(@"I add the parameter ""(.*)"" with the value or sitecode ""(.*)""")]
        public void GivenIAddTheParameterWithTheSiteCode(string parameterName, string parameterValue)
        {
            if (parameterValue.Contains("http://fhir.nhs.net/Id/ods-site-code"))
            {
                var siteCode = parameterValue.Substring(parameterValue.LastIndexOf('|') + 1);
                string mappedSiteValue = GlobalContext.OdsCodeMap[siteCode];
                _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter(parameterName, "http://fhir.nhs.net/Id/ods-site-code|" + mappedSiteValue);
                return;
            }

            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter(parameterName, parameterValue);
        }

        public Resource GetResourceForRelativeUrl(GpConnectInteraction gpConnectInteraction, string relativeUrl)
        {
            var httpContext = new HttpContext();
            httpContext.SetDefaults();
            httpContext.HttpRequestConfiguration.SetDefaultHeaders();

            var httpContextFactory = new HttpContextFactory(gpConnectInteraction);
            httpContextFactory.ConfigureHttpContext(httpContext);
            
            var jwtHelper = new JwtHelper();
            var jwtFactory = new JwtFactory(gpConnectInteraction);

            jwtFactory.ConfigureJwt(jwtHelper, httpContext);

            if (relativeUrl.Contains("Patient"))
            {
                var patient = relativeUrl.ToLower().Replace("/", string.Empty);
                jwtHelper.RequestedPatientNHSNumber = GlobalContext.PatientNhsNumberMap[patient];
            }

            _securitySteps.ConfigureServerCertificatesAndSsl();

            httpContext.HttpRequestConfiguration.RequestUrl = relativeUrl;

            var requestFactory = new RequestFactory(gpConnectInteraction);
            requestFactory.ConfigureBody(httpContext);

            httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, jwtHelper.GetBearerToken());

            var httpRequest = new HttpRequest(httpContext, _securityContext);

            httpRequest.MakeHttpRequest();

            return httpContext.FhirResponse.Resource;
        }

        [Then(@"the response status code should indicate success")]
        public void ThenTheResponseStatusCodeShouldIndicateSuccess()
        {
            _httpContext.HttpResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Then(@"the response status code should indicate created")]
        public void ThenTheResponseStatusCodeShouldIndicateCreated()
        {
            _httpContext.HttpResponse.StatusCode.ShouldBe(HttpStatusCode.Created);
        }

        [Then(@"the response status code should indicate failure")]
        public void ThenTheResponseStatusCodeShouldIndicateFailure()
        {
            _httpContext.HttpResponse.StatusCode.ShouldNotBe(HttpStatusCode.OK);
        }

        [Then(@"the response status code should indicate unsupported media type error")]
        public void ThenTheResponseShouldIndicateUnsupportedMediaTypeError()
        {
            _httpContext.HttpResponse.StatusCode.ShouldBe(HttpStatusCode.UnsupportedMediaType);
            Log.WriteLine("Response HttpStatusCode should be {0} but was {1}", HttpStatusCode.UnsupportedMediaType, _httpContext.HttpResponse.StatusCode);
        }

        [Then(@"the response should be gzip encoded")]
        public void ThenTheResponseShouldBeGZipEncoded()
        {
            bool gZipHeaderFound = false;
            foreach (var header in _httpContext.HttpResponse.Headers)
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
            foreach (var header in _httpContext.HttpResponse.Headers)
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

            _httpContext.SetDefaults();

            httpContextFactory.ConfigureHttpContext(_httpContext);

            var jwtFactory = new JwtFactory(interaction);

            jwtFactory.ConfigureJwt(_jwtHelper, _httpContext);

            _securitySteps.ConfigureServerCertificatesAndSsl();
        }


        [Given(@"I set the GET request Id to ""([^""]*)""")]
        public void SetTheGetRequestIdTo(string id)
        {
            _httpContext.HttpRequestConfiguration.RequestUrl = "/fhir/Patient/" + id;
        }

        [Given(@"I set the GET request Version Id to ""([^""]*)""")]
        public void SetTheGetRequestVersionIdTo(string versionId)
        {
            _httpContext.CreatedAppointment.VersionId = versionId;
        }

        [Given(@"I set the request Http Method to ""([^""]*)""")]
        public void SetTheRequestHttpMethodTo(string method)
        {
            _httpContext.HttpRequestConfiguration.HttpMethod = new HttpMethod(method);
        }

        [Given(@"I set the request URL to ""([^""]*)""")]
        public void SetTheRequestUrlTo(string url)
        {
            _httpContext.HttpRequestConfiguration.RequestUrl = url;
        }

        [Given(@"I set the Interaction Id header to ""([^""]*)""")]
        public void SetTheInteractionIdHeaderTo(string interactionId)
        {
            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, interactionId);
        }

        [Given(@"I set the Read Operation logical identifier used in the request to ""([^""]*)""")]
        public void SetTheReadOperationLogicalIdentifierUsedInTheRequestTo(string logicalId)
        {
            _httpContext.HttpRequestConfiguration.GetRequestId = logicalId;

            var lastIndex = _httpContext.HttpRequestConfiguration.RequestUrl.LastIndexOf('/');

            if (_httpContext.HttpRequestConfiguration.RequestUrl.Contains("$"))
            {
                var action = _httpContext.HttpRequestConfiguration.RequestUrl.Substring(lastIndex);

                var firstIndex = _httpContext.HttpRequestConfiguration.RequestUrl.IndexOf('/');
                var url = _httpContext.HttpRequestConfiguration.RequestUrl.Substring(0, firstIndex + 1);

                _httpContext.HttpRequestConfiguration.RequestUrl = url + _httpContext.HttpRequestConfiguration.GetRequestId + action;
            }
            else
            {
               _httpContext.HttpRequestConfiguration.RequestUrl = _httpContext.HttpRequestConfiguration.RequestUrl.Substring(0, lastIndex + 1) + _httpContext.HttpRequestConfiguration.GetRequestId; 
            }
        }
        
        [Given(@"I set the Read Operation relative path to ""([^""]*)"" and append the resource logical identifier")]
        public void SetTheReadOperationRelativePathToAndAppendTheResourceLogicalIdentifier(string relativePath)
        {
            _httpContext.HttpRequestConfiguration.RequestUrl = relativePath + "/" + _httpContext.HttpRequestConfiguration.GetRequestId;
        }

        [When(@"I make the ""(.*)"" request")]
        public void MakeRequest(GpConnectInteraction interaction)
        {
            var requestFactory = new RequestFactory(interaction);

            requestFactory.ConfigureBody(_httpContext);

            if (!string.IsNullOrEmpty(_httpContext.HttpRequestConfiguration.RequestHeaders.GetHeaderValue(HttpConst.Headers.kAuthorization)))
            {
                _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerToken());
            }

            var httpRequest = new HttpRequest(_httpContext, _securityContext);

            httpRequest.MakeHttpRequest();
        }

        [When(@"I make the ""(.*)"" request with an unencoded JWT Bearer Token")]
        public void MakeRequestWithAnUnencodedJwtBearerToken(GpConnectInteraction interaction)
        {
            var requestFactory = new RequestFactory(interaction);

            requestFactory.ConfigureBody(_httpContext);

            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerTokenWithoutEncoding());

            var httpRequest = new HttpRequest(_httpContext, _securityContext);

            httpRequest.MakeHttpRequest();
        }

        [When(@"I make the ""(.*)"" request with invalid Resource type")]
        public void MakeRequestWithInvalidResourceType(GpConnectInteraction interaction)
        {
            var requestFactory = new RequestFactory(interaction);

            requestFactory.ConfigureBody(_httpContext);
            requestFactory.ConfigureInvalidResourceType(_httpContext);

            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerToken());

            var httpRequest = new HttpRequest(_httpContext, _securityContext);

            httpRequest.MakeHttpRequest();
        }

        [When(@"I make the ""(.*)"" request with Invalid Additional Field in the Resource")]
        public void MakeRequestWithInvalidAdditionalFieldInTheResource(GpConnectInteraction interaction)
        {
            var requestFactory = new RequestFactory(interaction);

            requestFactory.ConfigureBody(_httpContext);
            requestFactory.ConfigureAdditionalInvalidFieldInResource(_httpContext);

            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerToken());

            var httpRequest = new HttpRequest(_httpContext, _securityContext);

            httpRequest.MakeHttpRequest();
        }

        [When(@"I make the ""(.*)"" request with invalid parameter Resource type")]
        public void MakeRequestWithInvalidParameterResourceType(GpConnectInteraction interaction)
        {
            var requestFactory = new RequestFactory(interaction);
            requestFactory.ConfigureBody(_httpContext);
            requestFactory.ConfigureInvalidParameterResourceType(_httpContext);
            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerToken());

            var httpRequest = new HttpRequest(_httpContext, _securityContext);

            httpRequest.MakeHttpRequest();
        }

        [When(@"I make the ""(.*)"" request with additional field in parameter Resource")]
        public void MakeRequestWithAdditionalFieldInParameterResource(GpConnectInteraction interaction)
        {
            var requestFactory = new RequestFactory(interaction);
            requestFactory.ConfigureBody(_httpContext);
            requestFactory.ConfigureParameterResourceWithAdditionalField(_httpContext);
            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerToken());

            var httpRequest = new HttpRequest(_httpContext, _securityContext);

            httpRequest.MakeHttpRequest();
        }

        [Given("I set the Decompression Method to gzip")]
        public void SetTheDecompressionMethodToGzip()
        {
            _httpContext.HttpRequestConfiguration.DecompressionMethod = DecompressionMethods.GZip;
        }

        [Then(@"the Response should contain the ETag header matching the Resource Version Id")]
        public void TheResponseShouldContainTheETagHeaderMatchingTheResourceVersionId()
        {
            var versionId = _httpContext.FhirResponse.Resource.VersionId;

            string eTag;
            _httpContext.HttpResponse.Headers.TryGetValue("ETag", out eTag);

            eTag.ShouldStartWith("W/\"", "The ETag header should start with W/\"");

            eTag.ShouldEndWith(versionId + "\"", "The ETag header should contain the resource version enclosed within speech marks");

            eTag.ShouldBe("W/\"" + versionId + "\"", "The ETag header contains invalid characters");
        }

        [Then(@"the content-type should not be equal to null")]
        public void ThenTheContentTypeShouldNotBeEqualToNull()
        {
            string contentType;
            _httpContext.HttpResponse.Headers.TryGetValue("Content-Type", out contentType);
            contentType.ShouldNotBeNullOrEmpty("The response should contain a Content-Type header.");
        }

        [Then(@"the content-type should be equal to null")]
        public void ThenTheContentTypeShouldBeEqualToZero()
        {
            string contentType;
            _httpContext.HttpResponse.Headers.TryGetValue("Content-Type", out contentType);
            contentType.ShouldBe(null, "There should not be a content-type header on the response");
        }

        [Then(@"the content-length should not be equal to zero")]
        public void ThenTheContentLengthShouldNotBeEqualToZero()
        {
            string contentLength;
            _httpContext.HttpResponse.Headers.TryGetValue("Content-Length", out contentLength);
            contentLength.ShouldNotBe("0", "The response payload should contain a resource.");
        }
    }
}
