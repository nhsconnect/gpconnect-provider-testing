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
        private readonly FhirContext _fhirContext;

        public HttpRequest(HttpContext httpContext, FhirContext fhirContext)
        {
            _httpContext = httpContext;
            _fhirContext = fhirContext;
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
            _httpContext.ResponseTimeInMilliseconds = timer.ElapsedMilliseconds;

            // Save The Response Details
            _httpContext.ResponseStatusCode = result.StatusCode;

            // Some HTTP responses will have no content e.g. 304
            if (result.Content.Headers.ContentType != null)
            {
                _httpContext.ResponseContentType = result.Content.Headers.ContentType.MediaType;
            }

            using (var reader = new StreamReader(result.Content.ReadAsStreamAsync().Result))
            {
                _httpContext.ResponseBody = reader.ReadToEnd();
            }

            // Add headers
            foreach (var headerKey in result.Headers)
            {
                foreach (var headerKeyValues in headerKey.Value)
                {
                    _httpContext.ResponseHeaders.Add(headerKey.Key, headerKeyValues);
                    Log.WriteLine("Header - " + headerKey.Key + " : " + headerKeyValues);
                }
            }

            foreach (var header in result.Content.Headers)
            {
                foreach (var headerValues in header.Value)
                {
                    _httpContext.ResponseHeaders.Add(header.Key, headerValues);
                    Log.WriteLine("Header - " + header.Key + " : " + headerValues);
                }
            }

            ParseResponse();
        }

        private HttpRequestMessage GetHttpRequestMessage()
        {
            var queryStringParameters = GetQueryStringParameters();

            var requestMessage = new HttpRequestMessage(_httpContext.HttpMethod, _httpContext.RequestUrl + queryStringParameters);

            if (_httpContext.RequestBody != null)
            {
                requestMessage.Content = new StringContent(_httpContext.RequestBody, Encoding.UTF8, _httpContext.RequestContentType);
            }

            // Add The Headers
            _httpContext.RequestHeaders.AddHeader(HttpConst.Headers.kContentType, _httpContext.RequestContentType);
            foreach (var header in _httpContext.RequestHeaders.GetRequestHeaders())
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
            if (!_httpContext.RequestParameters.GetRequestParameters().Any())
            {
                return string.Empty;
            }

            var requestParamString = "?";

            foreach (var parameter in _httpContext.RequestParameters.GetRequestParameters())
            {
                Log.WriteLine("Parameter - {0} -> {1}", parameter.Key, parameter.Value);
                requestParamString = requestParamString + HttpUtility.UrlEncode(parameter.Key, Encoding.UTF8) + "=" + HttpUtility.UrlEncode(parameter.Value, Encoding.UTF8) + "&";
            }

            return requestParamString.Substring(0, requestParamString.Length - 1);
        }

        private void ParseResponse()
        {
            var responseIsCompressed = _httpContext.ResponseHeaders.Contains(new KeyValuePair<string, string>(HttpConst.Headers.kContentEncoding, "gzip"));
            var decompressResponse = _httpContext.DecompressionMethod != DecompressionMethods.None;

            if (responseIsCompressed && !decompressResponse)
                return;

            switch (_httpContext.ResponseContentType)
            {
                case FhirConst.ContentTypes.kJsonFhir:
                    _httpContext.ResponseJSON = JObject.Parse(_httpContext.ResponseBody);
                    var jsonParser = new FhirJsonParser();
                    _fhirContext.FhirResponseResource = jsonParser.Parse<Resource>(_httpContext.ResponseBody);
                    break;
                case FhirConst.ContentTypes.kXmlFhir:
                    _httpContext.ResponseXML = XDocument.Parse(_httpContext.ResponseBody);
                    var xmlParser = new FhirXmlParser();
                    _fhirContext.FhirResponseResource = xmlParser.Parse<Resource>(_httpContext.ResponseBody);
                    break;
            }
        }

        private WebRequestHandler ConfigureHandler()
        {
            var handler = new WebRequestHandler
            {
                AutomaticDecompression = _httpContext.DecompressionMethod
            };

            if (_httpContext.SecurityContext.SendClientCert)
            {
                var clientCert = _httpContext.SecurityContext.ClientCert;
                handler.ClientCertificates.Add(clientCert);
            }

            if (_httpContext.UseWebProxy)
            {
                handler.Proxy = new WebProxy(new Uri(_httpContext.WebProxyAddress, UriKind.Absolute));
            }

            return handler;
        }

        private HttpClient GetHttpClient()
        {
            var handler = ConfigureHandler();

            return new HttpClient(handler)
            {
                BaseAddress = new Uri(_httpContext.BaseUrl)
            };
        }
    }
}
