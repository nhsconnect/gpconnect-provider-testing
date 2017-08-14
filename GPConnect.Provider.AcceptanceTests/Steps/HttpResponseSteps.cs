namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System;
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

        [Then(@"the required cacheing headers should be present in the response")]
        public void ThenTheRequiredCacheingHeadersShouldBePresentInTheResponse()
        {
            string cacheControl;
            string expires;
            string pragma;
            _httpContext.HttpResponse.Headers.TryGetValue("Cache-Control", out cacheControl);
            _httpContext.HttpResponse.Headers.TryGetValue("Expires", out expires);
            _httpContext.HttpResponse.Headers.TryGetValue("Pragma", out pragma);
            cacheControl.ShouldBe("no-store, no-cache", "The response payload should contain a resource.");
            expires.ShouldBe("0", "The response payload should contain a resource.");
            pragma.ShouldBe("no-cache", "The response payload should contain a resource.");
        }
    }
}
