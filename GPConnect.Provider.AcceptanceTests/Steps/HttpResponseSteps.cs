namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Constants;
    using Context;
    using Logger;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    public class HttpResponseSteps : Steps
    {
        private readonly HttpContext _httpContext;

        public HttpResponseSteps(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

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

        [Then(@"the Response should contain the ETag header matching the Resource Version Id")]
        public void TheResponseShouldContainTheETagHeaderMatchingTheResourceVersionId()
        {
            var versionId = _httpContext.FhirResponse.Resource.VersionId;

            string eTag;
            _httpContext.HttpResponse.Headers.TryGetValue("ETag", out eTag);

            eTag.ShouldStartWith("W/\"", Case.Sensitive, "The ETag header should start with W/\"");

            eTag.ShouldEndWith(versionId + "\"", Case.Sensitive, "The ETag header should contain the resource version enclosed within speech marks");

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

        [Then(@"the required cacheing headers should be present in the response")]
        public void ThenTheRequiredCacheingHeadersShouldBePresentInTheResponse()
        {
            string cacheControl;
            _httpContext.HttpResponse.Headers.TryGetValue("Cache-Control", out cacheControl);

            // Amended git hub ref 82 from Shouldbe to ShouldContain 
            // RMB 9/10/2018			
            cacheControl.ShouldContain("no-store", Case.Sensitive, "The response payload should contain a resource.");

        }

        [Then("if redirected the Response Headers should contain a Strict-Transport-Security header")]
        public void IfRedirectedTheResponseHeadersShouldContainAStrictTransportSecurityHeader()
        {
            if (_httpContext.HttpResponse.Redirected)
            {
                _httpContext.HttpResponse.Headers.ShouldContainKey("Strict-Transport-Security", "The Response Headers should contain a Strict-Transport-Security header, but it wasn't found.");
            }
        }

        [Then("the Response should indicate the connection was closed by the server")]
        public void TheResponseShouldIndicateTheConnectionWasClosedByTheServer()
        {
            if (_httpContext.HttpResponse.StatusCode == 0)
            {
                _httpContext.HttpResponse.ConnectionClosed.ShouldBe(true, "The connection should have been closed by the server");
            }
        }

        [Then(@"the Response Status Code should be one of ""(.*)""")]
        public void TheResponseStatusCodeShouldBeOf(List<HttpStatusCode> statusCodes)
        {
            if (_httpContext.HttpResponse.StatusCode != 0)
            {
                var statusCode = _httpContext.HttpResponse.StatusCode;
                var statusCodeList = string.Join(", ", statusCodes.Select(sc => (int)sc));

                statusCodes.ShouldContain(statusCode, $"The Response Status Code should be one of {statusCodeList}, but it was {(int)statusCode}.");
            }
        }

        [Then("the Response should indicate the connection was closed by the server or the Request was redirected")]
        public void TheResponseShouldIndicateTheConnectionWasClosedByTheServerOrTheRequestWasRedirected()
        {
            var connectionClosedOrRedirected = _httpContext.HttpResponse.Redirected || _httpContext.HttpResponse.ConnectionClosed;

            connectionClosedOrRedirected.ShouldBe(true, "The connection should have been closed by the server or the Request should have been redirected but neither occured.");
        }

        [Then(@"the Response Headers should contain an Access-Control-Request-Method header")]
        public void TheResponseHeadersShouldContainAnAccessControlRequestMethodHeader()
        {
            const string header = "Access-Control-Request-Method";
            _httpContext.HttpResponse.Headers.ShouldContainKey(header, $"The Response Headers should have contained an {header} header, but did not.");
        }

        [Then(@"the Access-Control-Request-Method header should contain the ""(.*)"" request methods")]
        public void TheAccessControlRequestMethodHeaderShouldContainTheRequestMethods(List<string> methods)
        {
            const string headerName = "Access-Control-Request-Method";

            var headerValue = _httpContext.HttpResponse.Headers[headerName];

            methods.ForEach(method =>
            {
                headerValue.ShouldContain(method, Case.Sensitive, $"The {headerName} header should contain the {method} HTTP method, but did not.");
            });
        }

        [Then(@"the response should contain the recorder reference")]
        public void TheResponseShouldContainTheRecorderReference()
        {
            var values = _httpContext.FhirResponse.AllergyIntolerances;
            foreach (var allergy in values)
            {
                allergy.Recorder.ShouldNotBeNull("Recorder has a value");
            }

        }

        [StepArgumentTransformation]
        public List<string> CommaSeperatedValuesTransform(string csv)
        {
            return csv.Split(',').Select(x => x.Trim()).ToList();
        }

        [StepArgumentTransformation]
        public List<HttpStatusCode> CommaSeperatedHttpStatusCodeTransform(string httpStatusCodes)
        {
            return httpStatusCodes
                .Split(',')
                .Select(sc => (HttpStatusCode)int.Parse(sc.Trim()))
                .ToList();
        }
    }
}
