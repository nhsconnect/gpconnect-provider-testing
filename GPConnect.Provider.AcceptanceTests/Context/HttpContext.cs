namespace GPConnect.Provider.AcceptanceTests.Context
{
    using System.Collections.Generic;
    using System.Net;
    using System.Xml.Linq;
    using Helpers;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Serialization;
    using Http;
    using TechTalk.SpecFlow;

    public class HttpContext : IHttpContext
    {
        public HttpContext(ScenarioContext scenarioContext, HttpHeaderHelper requestHeaderHelper, JwtHelper jwtHelper, SecurityContext securityContext, HttpParameterHelper requestParametersHelper)
        {
            ScenarioContext = scenarioContext;
            Jwt = jwtHelper;
            SecurityContext = securityContext;
            HttpResponse = new HttpResponse();
            HttpRequestConfiguration = new HttpRequestConfiguration(securityContext)
            {
                RequestHeaders = requestHeaderHelper,
                RequestParameters = requestParametersHelper,
                BodyParameters = new Parameters()
            };
        }

        public readonly ScenarioContext ScenarioContext;

        // Http Helper
        public HttpParameterHelper RequestParameters { get; set; }

        // JWT Helper
        public JwtHelper Jwt { get; }

        // Security Context
        public SecurityContext SecurityContext { get; }

        public Patient StoredPatient { get; set; }

        public Organization StoredOrganization { get; set; }

        public Bundle StoredBundle { get; set; }

        public Appointment CreatedAppointment { get; set; }

        public Location StoredLocation { get; set; }

        public FhirResponse FhirResponse { get; set; }

        public HttpResponse HttpResponse { get; set; }

        public HttpRequestConfiguration HttpRequestConfiguration { get; set; }

        public void SetDefaults()
        {
            HttpRequestConfiguration.RequestHeaders.Clear();
            HttpRequestConfiguration.RequestUrl = "";
            HttpRequestConfiguration.RequestParameters.ClearParameters();
            HttpRequestConfiguration.RequestBody = null;
            HttpRequestConfiguration.BodyParameters = new Parameters();
            HttpRequestConfiguration.DecompressionMethod = DecompressionMethods.None;

            HttpResponse = new HttpResponse
            {
                StatusCode = default(HttpStatusCode),
                ResponseTimeInMilliseconds = -1,
                ContentType = null,
                Body = null,
                Headers = new Dictionary<string, string>()
            };


            FhirResponse = new FhirResponse();
        }

        public void SaveToDisk(string filename)
        {
            var requestHeaders = new XElement(Context.kRequestHeaders);
            foreach (var entry in HttpRequestConfiguration.RequestHeaders.GetRequestHeaders())
            {
                requestHeaders.Add(new XElement("requestHeader", new XAttribute("name", entry.Key), new XAttribute("value", entry.Value)));
            }
            var requestParameters = new XElement(Context.kRequestParameters);
            foreach (var entry in RequestParameters.GetRequestParameters())
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
                        new XElement(Context.kRequestMethod, HttpRequestConfiguration.RequestMethod),
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