namespace GPConnect.Provider.AcceptanceTests.Context
{
    using System.IdentityModel.Tokens.Jwt;
    using System.IO;
    using System.Xml.Linq;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Serialization;
    using Http;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class HttpContext : IHttpContext
    {
        public HttpContext()
        {
            HttpResponse = new HttpResponse();
            HttpRequestConfiguration = new HttpRequestConfiguration();
            FhirResponse = new FhirResponse();
        }

        public FhirResponse FhirResponse { get; set; }

        public HttpResponse HttpResponse { get; set; }

        public HttpRequestConfiguration HttpRequestConfiguration { get; set; }

        public void SetDefaults()
        {
            HttpResponse = new HttpResponse();
            HttpRequestConfiguration.RequestHeaders.Clear();
            HttpRequestConfiguration.RequestUrl = "";
            HttpRequestConfiguration.RequestParameters.ClearParameters();
            HttpRequestConfiguration.RequestBody = null;
            HttpRequestConfiguration.BodyParameters = new Parameters();
        }

        public void SaveToDisk(string filename)
        {
            var requestHeaders = new XElement(Context.kRequestHeaders);
            foreach (var entry in HttpRequestConfiguration.RequestHeaders.GetRequestHeaders())
            {
                requestHeaders.Add(new XElement("requestHeader", new XAttribute("name", entry.Key), new XAttribute("value", entry.Value)));
            }
            var requestParameters = new XElement(Context.kRequestParameters);
            foreach (var entry in HttpRequestConfiguration.RequestParameters.GetRequestParameters())
            {
                requestParameters.Add(new XElement("requestParameter", new XAttribute("name", entry.Key), new XAttribute("value", entry.Value)));
            }
            var responseHeaders = new XElement(Context.kResponseHeaders);
            foreach (var entry in HttpResponse.Headers)
            {
                responseHeaders.Add(new XElement("responseHeader", new XAttribute("name", entry.Key), new XAttribute("value", entry.Value)));
            }

            var doc = new XDocument(
                new XElement("httpContext",
                    new XAttribute(Context.kUseWebProxy, HttpRequestConfiguration.UseWebProxy),
                    new XAttribute(Context.kWebProxyUrl, HttpRequestConfiguration.WebProxyAddress),
                    new XAttribute(Context.kUseSpineProxy, HttpRequestConfiguration.UseSpineProxy),
                    new XAttribute(Context.kSpineProxyUrl, HttpRequestConfiguration.SpineProxyAddress),
                    new XAttribute("providerUrl", HttpRequestConfiguration.ProviderAddress),
                    new XElement("request",
                        new XAttribute("endpointUrl", HttpRequestConfiguration.EndpointAddress),
                        requestHeaders,
                        new XElement(Context.kRequestUrl, HttpRequestConfiguration.RequestUrl),
                        requestParameters,
                        new XElement(Context.kRequestMethod, HttpRequestConfiguration.HttpMethod.ToString()),
                        new XElement(Context.kRequestContentType, HttpRequestConfiguration.RequestContentType),
                        new XElement(Context.kRequestBody, System.Security.SecurityElement.Escape(HttpRequestConfiguration.RequestBody))),
                    new XElement("response",
                        new XElement(Context.kResponseContentType, HttpResponse.ContentType),
                        new XElement(Context.kResponseStatusCode, (int)HttpResponse.StatusCode),
                        new XElement(Context.kResponseTimeInMilliseconds, HttpResponse.ResponseTimeInMilliseconds),
                        new XElement(Context.kResponseTimeAcceptable, HttpResponse.ResponseTimeAcceptable),
                        responseHeaders,
                        new XElement(Context.kResponseBody, System.Security.SecurityElement.Escape(HttpResponse.Body)))
                ));
            doc.Save(filename);
        }

        public void SaveToFhirContextToDisk(string filename)
        {
            var doc = new XDocument(
                new XElement("fhirContext",
                    new XElement("request",
                        new XElement("fhirRequestParameters", FhirSerializer.SerializeResourceToJson(HttpRequestConfiguration.BodyParameters))
                    ),
                    new XElement("response",
                        new XElement("fhirResponseResource", FhirSerializer.SerializeResourceToJson(FhirResponse.Resource))
                    )
                )
            );
            doc.Save(filename);
        }

        public void SaveJSONResponseToDisk(string filename)
        {
            var JSONResponse = JToken.Parse(FhirSerializer.SerializeResourceToJson(FhirResponse.Resource));

            string jsonPrettyPrinted = JsonConvert.SerializeObject(JSONResponse, Formatting.Indented);
            File.WriteAllText(@filename, jsonPrettyPrinted);


        }

        public void SaveJWTToDisk(string filename)
        {
            string JWTToken;

            foreach (var entry in HttpRequestConfiguration.RequestHeaders.GetRequestHeaders())
            {
                //find JWT
                if (entry.Key == "Authorization")
                {
                    JWTToken = entry.Value.Replace("Bearer ", "");

                    //Convert Token to JSON
                    var jwtHandler = new JwtSecurityTokenHandler();
                    var token = jwtHandler.ReadJwtToken(JWTToken);

                    string jwtHeaderPrettyPrinted = JsonConvert.SerializeObject(token.Header, Formatting.Indented);
                    string jwtPayloadPrettyPrinted = JsonConvert.SerializeObject(token.Payload, Formatting.Indented);
                    File.WriteAllText(@filename, "Header:\n" + jwtHeaderPrettyPrinted + "\nPayload:\n" + jwtPayloadPrettyPrinted);
                }

            }

        }


        public void SaveJSONRequestBodyToDisk(string filename)
        {
            var JSONRquestBody = JsonConvert.DeserializeObject(HttpRequestConfiguration.RequestBody);
            string jsonPrettyPrinted = JsonConvert.SerializeObject(JSONRquestBody, Formatting.Indented);
            File.WriteAllText(@filename, jsonPrettyPrinted);
        }


        private static class Context
        {
            // Provider
            public const string kFhirServerUrl = "fhirServerUrl";
            public const string kFhirServerPort = "fhirServerPort";
            public const string kFhirServerFhirBase = "fhirServerFhirBase";
            // Web Proxy
            public const string kUseWebProxy = "useWebProxy";
            public const string kWebProxyUrl = "webProxyUrl";
            public const string kWebProxyPort = "webProxyPort";
            // Spine Proxy
            public const string kUseSpineProxy = "useSpineProxy";
            public const string kSpineProxyUrl = "spineProxyUrl";
            public const string kSpineProxyPort = "spineProxyPort";
            // Request
            public const string kRequestHeaders = "requestHeaders";
            public const string kRequestUrl = "requestUrl";
            public const string kRequestParameters = "requestParameters";
            public const string kRequestMethod = "requestMethod";
            public const string kRequestContentType = "requestContentType";
            public const string kRequestBody = "requestBody";
            // Raw Response
            public const string kResponseHeaders = "responseHeaders";
            public const string kResponseContentType = "responseContentType";
            public const string kResponseStatusCode = "responseStatusCode";
            public const string kResponseBody = "responseBody";
            public const string kResponseTimeInMilliseconds = "responseTimeInMilliseconds";
            public const string kResponseTimeAcceptable = "responseTimeAcceptable";


        }
    }
}