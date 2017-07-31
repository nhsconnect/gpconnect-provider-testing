namespace GPConnect.Provider.AcceptanceTests.Http
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Xml.Linq;
    using Constants;
    using Context;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Serialization;
    using Logger;
    using Newtonsoft.Json.Linq;
    using RestSharp.Extensions.MonoHttp;

    public class HttpRequest
    {
        private readonly HttpContext _httpContext;
        private readonly SecurityContext _securityContext;

        public HttpRequest(HttpContext httpContext, SecurityContext securityContext)
        {
            _httpContext = httpContext;
            _securityContext = securityContext;
        }

        public void MakeHttpRequest()
        {
            var httpClient = GetHttpClient();

            var requestMessage = GetHttpRequestMessage();

            var timer = new System.Diagnostics.Stopwatch();

            // Start The Performance Timer Running
            timer.Start();

            // Perform The Http Request
            var result = httpClient.SendAsync(requestMessage).Result;

            // Always Stop The Performance Timer Running
            timer.Stop();

            // Save The Time Taken To Perform The Request
            _httpContext.HttpResponse.ResponseTimeInMilliseconds = timer.ElapsedMilliseconds;

            // Save The Response Details
            _httpContext.HttpResponse.StatusCode = result.StatusCode;

            // Some HTTP responses will have no content e.g. 304
            if (result.Content.Headers.ContentType != null)
            {
                _httpContext.HttpResponse.ContentType = result.Content.Headers.ContentType.MediaType;
            }

            using (var reader = new StreamReader(result.Content.ReadAsStreamAsync().Result))
            {
                _httpContext.HttpResponse.Body = reader.ReadToEnd();
            }

            // Add headers
            foreach (var headerKey in result.Headers)
            {
                foreach (var headerKeyValues in headerKey.Value)
                {
                    _httpContext.HttpResponse.Headers.Add(headerKey.Key, headerKeyValues);
                    Log.WriteLine("Header - " + headerKey.Key + " : " + headerKeyValues);
                }
            }

            foreach (var header in result.Content.Headers)
            {
                foreach (var headerValues in header.Value)
                {
                    _httpContext.HttpResponse.Headers.Add(header.Key, headerValues);
                    Log.WriteLine("Header - " + header.Key + " : " + headerValues);
                }
            }

            ParseResponse();
        }

        private HttpRequestMessage GetHttpRequestMessage()
        {
            var queryStringParameters = GetQueryStringParameters();

            var requestMessage = new HttpRequestMessage(_httpContext.HttpRequestConfiguration.HttpMethod, _httpContext.HttpRequestConfiguration.RequestUrl + queryStringParameters);

            if (_httpContext.HttpRequestConfiguration.RequestBody != null)
            {
                requestMessage.Content = new StringContent(_httpContext.HttpRequestConfiguration.RequestBody, Encoding.UTF8, _httpContext.HttpRequestConfiguration.RequestContentType);
            }

            // Add The Headers
            _httpContext.HttpRequestConfiguration.RequestHeaders.AddHeader(HttpConst.Headers.kContentType, _httpContext.HttpRequestConfiguration.RequestContentType);
            foreach (var header in _httpContext.HttpRequestConfiguration.RequestHeaders.GetRequestHeaders())
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

        private string GetQueryStringParameters()
        {
            if (!_httpContext.HttpRequestConfiguration.RequestParameters.GetRequestParameters().Any())
            {
                return string.Empty;
            }

            var requestParamString = "?";

            foreach (var parameter in _httpContext.HttpRequestConfiguration.RequestParameters.GetRequestParameters())
            {
                Log.WriteLine("Parameter - {0} -> {1}", parameter.Key, parameter.Value);
                requestParamString = requestParamString + HttpUtility.UrlEncode(parameter.Key, Encoding.UTF8) + "=" + HttpUtility.UrlEncode(parameter.Value, Encoding.UTF8) + "&";
            }

            return requestParamString.Substring(0, requestParamString.Length - 1);
        }

        private void ParseResponse()
        {
            var responseIsCompressed = _httpContext.HttpResponse.Headers.Contains(new KeyValuePair<string, string>(HttpConst.Headers.kContentEncoding, "gzip"));
            var decompressResponse = _httpContext.HttpRequestConfiguration.DecompressionMethod != DecompressionMethods.None;

            if (responseIsCompressed && !decompressResponse)
                return;

            switch (_httpContext.HttpResponse.ContentType)
            {
                case FhirConst.ContentTypes.kJsonFhir:
                    _httpContext.HttpResponse.ResponseJSON = JObject.Parse(_httpContext.HttpResponse.Body);
                    var jsonParser = new FhirJsonParser();
                    _httpContext.FhirResponse.Resource = jsonParser.Parse<Resource>(_httpContext.HttpResponse.Body);
                    break;
                case FhirConst.ContentTypes.kXmlFhir:
                    _httpContext.HttpResponse.ResponseXML = XDocument.Parse(_httpContext.HttpResponse.Body);
                    var xmlParser = new FhirXmlParser();
                    _httpContext.FhirResponse.Resource = xmlParser.Parse<Resource>(_httpContext.HttpResponse.Body);
                    break;
            }
        }

        private WebRequestHandler ConfigureHandler()
        {
            var handler = new WebRequestHandler
            {
                AutomaticDecompression = _httpContext.HttpRequestConfiguration.DecompressionMethod
            };

            if (_securityContext.SendClientCert)
            {
                var clientCert = _securityContext.ClientCert;
                handler.ClientCertificates.Add(clientCert);
            }

            if (_httpContext.HttpRequestConfiguration.UseWebProxy)
            {
                handler.Proxy = new WebProxy(new Uri(_httpContext.HttpRequestConfiguration.WebProxyAddress, UriKind.Absolute));
            }

            return handler;
        }

        private HttpClient GetHttpClient()
        {
            var handler = ConfigureHandler();

            return new HttpClient(handler)
            {
                BaseAddress = new Uri(_httpContext.HttpRequestConfiguration.BaseUrl),
                Timeout = new TimeSpan(0, 0, 10, 0)
            };
        }
    }
}
