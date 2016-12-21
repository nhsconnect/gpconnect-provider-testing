using System.Net;
using System.Xml.Linq;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Logger;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;
// ReSharper disable ClassNeverInstantiated.Global

namespace GPConnect.Provider.AcceptanceTests.Context
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public interface IHttpContext
    {
        // Headers Helper
        HttpHeaderHelper Headers { get; }
        // Http Helper
        HttpHelper Http { get; }
        // JWT Helper
        JwtHelper Jwt { get; }
        // Security Context
        SecurityContext SecurityContext { get; }
        // Protocol
        string Protocol { get; }
        // Web Proxy
        bool UseWebProxy { get; set; }
        string WebProxyUrl { get; set; }
        string WebProxyPort { get; set; }
        string WebProxyAddress { get; }
        // Spine Proxy
        bool UseSpineProxy { get; set; }
        string SpineProxyUrl { get; set; }
        string SpineProxyPort { get; set; }
        string SpineProxyAddress { get; }
        // Raw Request
        string RequestMethod { get; set; }
        string RequestUrl { get; set; }
        string RequestContentType { get; set; }
        string RequestBody { get; set; }
        // Raw Response
        string ResponseContentType { get; set; }
        HttpStatusCode ResponseStatusCode { get; set; }
        string ResponseBody { get; set; }
        // Parsed Response
        JObject ResponseJSON { get; set; }
        XDocument ResponseXML { get; set; }
        // Consumer
        string ConsumerASID { get; set; }
        // Provider
        string ProviderASID { get; set; }
        string FhirServerUrl { get; set; }
        string FhirServerPort { get; set; }
        string FhirServerFhirBase { get; set; }
        string ProviderAddress { get; }
        string EndpointAddress { get; }
    }

    public class HttpContext : IHttpContext
    {
        public readonly ScenarioContext ScenarioContext;

        // Headers Helper
        public HttpHeaderHelper Headers { get; }

        // Http Helper
        public HttpHelper Http { get; }

        // JWT Helper
        public JwtHelper Jwt { get; }

        // Security Context
        public SecurityContext SecurityContext { get; }

        public HttpContext(ScenarioContext scenarioContext, HttpHeaderHelper headerHelper, JwtHelper jwtHelper, SecurityContext securityContext, HttpHelper httpHelper)
        {
            ScenarioContext = scenarioContext;
            Headers = headerHelper;
            Jwt = jwtHelper;
            SecurityContext = securityContext;
            Http = httpHelper;
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
            public const string kRequestUrl = "requestUrl";
            public const string kRequestMethod = "requestMethod";
            public const string kRequestContentType = "requestContentType";
            public const string kRequestBody = "requestBody";
            // Raw Response
            public const string kResponseContentType = "responseContentType";
            public const string kResponseStatusCode = "responseStatusCode";
            public const string kResponseBody = "responseBody";
            // Parsed Response
            public const string kResponseJSON = "responseJSON";
            public const string kResponseXML = "responseXML";
            // Consumer
            public const string kConsumerASID = "consumerASID";
            // Producer
            public const string kProviderASID = "providerASID";
        }

        // Protocol

        public string Protocol => SecurityContext.UseTLS ? "https://" : "http://";

        // Web Proxy
        public bool UseWebProxy
        {
            get { return ScenarioContext.Get<bool>(Context.kUseWebProxy); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kUseWebProxy, value);
                ScenarioContext.Set(value, Context.kUseWebProxy);
            }
        }

        public string WebProxyUrl
        {
            get { return ScenarioContext.Get<string>(Context.kWebProxyUrl); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kWebProxyUrl, value);
                ScenarioContext.Set(value, Context.kWebProxyUrl);
            }
        }

        public string WebProxyPort
        {
            get { return ScenarioContext.Get<string>(Context.kWebProxyPort); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kWebProxyPort, value);
                ScenarioContext.Set(value, Context.kWebProxyPort);
            }
        }

        public string WebProxyAddress
        {
            get
            {
                var webProxyAddress = Protocol + WebProxyUrl + ":" + WebProxyPort;
                Log.WriteLine("webProxyAddress=" + webProxyAddress);
                return webProxyAddress;
            }
        }

        // Spine Proxy
        public bool UseSpineProxy
        {
            get { return ScenarioContext.Get<bool>(Context.kUseSpineProxy); }
            set { ScenarioContext.Set(value, Context.kUseSpineProxy); }
        }

        public string SpineProxyUrl
        {
            get { return ScenarioContext.Get<string>(Context.kSpineProxyUrl); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kSpineProxyUrl, value);
                ScenarioContext.Set(value, Context.kSpineProxyUrl);
            }
        }

        public string SpineProxyPort
        {
            get { return ScenarioContext.Get<string>(Context.kSpineProxyPort); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kSpineProxyPort, value);
                ScenarioContext.Set(value, Context.kSpineProxyPort);
            }
        }

        public string SpineProxyAddress
        {
            get
            {
                var spineProxyAddress = Protocol + SpineProxyUrl + ":" + SpineProxyPort;
                Log.WriteLine("spineProxyAddress=" + spineProxyAddress);
                return spineProxyAddress;
            }
        }

        // Raw Request
        public string RequestMethod
        {
            get { return ScenarioContext.Get<string>(Context.kRequestMethod); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kRequestMethod, value);
                ScenarioContext.Set(value, Context.kRequestMethod);
            }
        }

        public string RequestUrl
        {
            get { return ScenarioContext.Get<string>(Context.kRequestUrl); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kRequestUrl, value);
                ScenarioContext.Set(value, Context.kRequestUrl);
            }
        }

        public string RequestContentType
        {
            get { return ScenarioContext.Get<string>(Context.kRequestContentType); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kRequestContentType, value);
                ScenarioContext.Set(value, Context.kRequestContentType);
            }
        }

        public string RequestBody
        {
            get { return ScenarioContext.Get<string>(Context.kRequestBody); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kRequestBody, value);
                ScenarioContext.Set(value, Context.kRequestBody);
            }
        }

        // Raw Response
        public string ResponseContentType
        {
            get { return ScenarioContext.Get<string>(Context.kResponseContentType); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kResponseContentType, value);
                ScenarioContext.Set(value, Context.kResponseContentType);
            }
        }

        public HttpStatusCode ResponseStatusCode
        {
            get { return ScenarioContext.Get<HttpStatusCode>(Context.kResponseStatusCode); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kResponseStatusCode, value);
                ScenarioContext.Set(value, Context.kResponseStatusCode);
            }
        }

        public string ResponseBody
        {
            get { return ScenarioContext.Get<string>(Context.kResponseBody); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kResponseBody, value);
                ScenarioContext.Set(value, Context.kResponseBody);
            }
        }

        // Parsed Response
        public JObject ResponseJSON
        {
            get { return ScenarioContext.Get<JObject>(Context.kResponseJSON); }
            set { ScenarioContext.Set(value, Context.kResponseJSON); }
        }

        public XDocument ResponseXML
        {
            get { return ScenarioContext.Get<XDocument>(Context.kResponseXML); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kResponseXML, value);
                ScenarioContext.Set(value, Context.kResponseXML);
            }
        }

        // Consumer
        public string ConsumerASID
        {
            get { return ScenarioContext.Get<string>(Context.kConsumerASID); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kConsumerASID, value);
                ScenarioContext.Set(value, Context.kConsumerASID);
            }
        }

        // Provider
        public string ProviderASID
        {
            get { return ScenarioContext.Get<string>(Context.kProviderASID); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kProviderASID, value);
                ScenarioContext.Set(value, Context.kProviderASID);
            }
        }

        public string FhirServerUrl
        {
            get { return ScenarioContext.Get<string>(Context.kFhirServerUrl); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kFhirServerUrl, value);
                ScenarioContext.Set(value, Context.kFhirServerUrl);
            }
        }

        public string FhirServerPort
        {
            get { return ScenarioContext.Get<string>(Context.kFhirServerPort); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kFhirServerPort, value);
                ScenarioContext.Set(value, Context.kFhirServerPort);
            }
        }

        public string FhirServerFhirBase
        {
            get { return ScenarioContext.Get<string>(Context.kFhirServerFhirBase); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kFhirServerFhirBase, value);
                ScenarioContext.Set(value, Context.kFhirServerFhirBase);
            }
        }

        public string ProviderAddress => Protocol + FhirServerUrl + ":" + FhirServerPort + FhirServerFhirBase;

        public string EndpointAddress
        {
            get
            {
                var sspAddress = UseSpineProxy ? SpineProxyAddress + "/" : string.Empty;
                var endpointAddress = sspAddress + ProviderAddress;
                Log.WriteLine("endpointAddress=" + endpointAddress);
                return endpointAddress;
            }
        }

        // Load App.Config

        public void LoadAppConfig()
        {
            Log.WriteLine("HttpContext->LoadAppConfig()");
            // Server
            FhirServerUrl = AppSettingsHelper.ServerUrl;
            FhirServerPort = AppSettingsHelper.ServerPort;
            FhirServerFhirBase = AppSettingsHelper.ServerBase;
            // Web Proxy
            UseWebProxy = AppSettingsHelper.UseWebProxy;
            WebProxyUrl = AppSettingsHelper.WebProxyUrl;
            WebProxyPort = AppSettingsHelper.WebProxyPort;
            // Spine Proxy
            UseSpineProxy = AppSettingsHelper.UseSpineProxy;
            SpineProxyUrl = AppSettingsHelper.SpineProxyUrl;
            SpineProxyPort = AppSettingsHelper.SpineProxyPort;
            // Provider
            ProviderASID = AppSettingsHelper.ProviderASID;
            // Consumer
            ConsumerASID = AppSettingsHelper.ConsumerASID;
        }
    }
}
