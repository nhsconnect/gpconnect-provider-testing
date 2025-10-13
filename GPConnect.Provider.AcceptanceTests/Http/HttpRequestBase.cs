namespace GPConnect.Provider.AcceptanceTests.Http
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using Constants;
    using Context;
    using Logger;
    using static System.Net.WebUtility;

    public class HttpRequestBase
    {
        protected readonly SecurityContext _securityContext;
        protected readonly HttpRequestConfiguration _httpRequestConfiguration;

        public HttpRequestBase(HttpRequestConfiguration httpRequestConfiguration, SecurityContext securityContext)
        {
            _httpRequestConfiguration = httpRequestConfiguration;
            _securityContext = securityContext;
        }

        protected HttpResponse GetHttpResponse(HttpResponseMessage result)
        {
            var httpResponse = new HttpResponse
            {
                StatusCode = result.StatusCode
            };

            // Some HTTP responses will have no content e.g. 304
            if (result.Content.Headers.ContentType != null)
            {
                httpResponse.ContentType = result.Content.Headers.ContentType.MediaType;
            }

            var stream = result.Content.ReadAsStreamAsync().Result;

            if (IsGZipRequestAndResponse(result))
            {
                stream = new GZipStream(stream, CompressionMode.Decompress);
            }

            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                httpResponse.Body = reader.ReadToEnd();
            }

            // Add headers
            foreach (var headerKey in result.Headers)
            {
                foreach (var headerKeyValues in headerKey.Value)
                {
                    httpResponse.Headers.Add(headerKey.Key, headerKeyValues);
                    Log.WriteLine("Header - " + headerKey.Key + " : " + headerKeyValues);
                }
            }

            foreach (var header in result.Content.Headers)
            {
                foreach (var headerValues in header.Value)
                {
                    httpResponse.Headers.Add(header.Key, headerValues);
                    Log.WriteLine("Header - " + header.Key + " : " + headerValues);
                }
            }

            httpResponse.Redirected = _httpRequestConfiguration.BaseUrl + _httpRequestConfiguration.RequestUrl != result.RequestMessage.RequestUri.ToString();

            return httpResponse;
        }

        protected HttpRequestMessage GetHttpRequestMessage()
        {
            var queryStringParameters = GetQueryStringParameters();

            var requestMessage = new HttpRequestMessage(_httpRequestConfiguration.HttpMethod, _httpRequestConfiguration.RequestUrl + queryStringParameters);

            if (_httpRequestConfiguration.RequestBody != null)
            {
                requestMessage.Content = new StringContent(_httpRequestConfiguration.RequestBody, Encoding.UTF8, _httpRequestConfiguration.RequestContentType);;
            }

            foreach (var header in _httpRequestConfiguration.RequestHeaders.GetRequestHeaders())
            {
                try
                {
                    Log.WriteLine("Header - {0} -> {1}", header.Key, header.Value);
                    requestMessage.Headers.Add(header.Key, header.Value);
                }
                catch (Exception e)
                {
                    Log.WriteLine("Could not add header: " + header.Key + e);
                }
            }

            return requestMessage;
        }

        protected string GetQueryStringParameters()
        {
            if (!_httpRequestConfiguration.RequestParameters.GetRequestParameters().Any())
            {
                return string.Empty;
            }

            var requestParamString = "?";

            foreach (var parameter in _httpRequestConfiguration.RequestParameters.GetRequestParameters())
            {
                Log.WriteLine("Parameter - {0} -> {1}", parameter.Key, parameter.Value);
                requestParamString = requestParamString + UrlEncode(parameter.Key) + "=" + UrlEncode(parameter.Value) + "&";
            }

            return requestParamString.Substring(0, requestParamString.Length - 1);
        }

        protected HttpClientHandler ConfigureHandler()
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true
            };
            
            if (_securityContext.SendClientCert)
            {
                var clientCert = _securityContext.ClientCert;
                handler.ClientCertificates.Add(clientCert);
            }

            if (_httpRequestConfiguration.UseWebProxy)
            {
                handler.Proxy = new WebProxy(new Uri(_httpRequestConfiguration.WebProxyAddress, UriKind.Absolute));
            }

            return handler;
        }

        protected HttpClient GetHttpClient()
        {
            var handler = ConfigureHandler();

            return new HttpClient(handler)
            { 
                BaseAddress = new Uri(_httpRequestConfiguration.BaseUrl),
                Timeout = new TimeSpan(0, 0, 10, 0)
            };
        }

        private bool IsGZipRequestAndResponse(HttpResponseMessage result)
        {
            return result.Content.Headers.ContentEncoding.Contains("gzip") && 
                _httpRequestConfiguration.RequestHeaders.GetHeaderValue(HttpConst.Headers.kAcceptEncoding).Contains("gzip");
        }
    }
}
