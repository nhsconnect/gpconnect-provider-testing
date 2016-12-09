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

        // JWT Helper
        public JwtHelper Jwt { get; }

        // Security Context
        public SecurityContext SecurityContext { get; }

        public HttpContext(ScenarioContext scenarioContext, HttpHeaderHelper headerHelper, JwtHelper jwtHelper, SecurityContext securityContext)
        {
            ScenarioContext = scenarioContext;
            Headers = headerHelper;
            Jwt = jwtHelper;
            SecurityContext = securityContext;
        }

        private static class Context
        {
            // Provider
            public const string FhirServerUrl = "fhirServerUrl";
            public const string FhirServerPort = "fhirServerPort";
            public const string FhirServerFhirBase = "fhirServerFhirBase";
            // Web Proxy
            public const string UseWebProxy = "useWebProxy";
            public const string WebProxyUrl = "webProxyUrl";
            public const string WebProxyPort = "webProxyPort";
            // Spine Proxy
            public const string UseSpineProxy = "useSpineProxy";
            public const string SpineProxyUrl = "spineProxyUrl";
            public const string SpineProxyPort = "spineProxyPort";
            // Request
            public const string RequestUrl = "requestUrl";
            public const string RequestMethod = "requestMethod";
            public const string RequestContentType = "requestContentType";
            public const string RequestBody = "requestBody";
            // Raw Response
            public const string ResponseContentType = "responseContentType";
            public const string ResponseStatusCode = "responseStatusCode";
            public const string ResponseBody = "responseBody";
            // Parsed Response
            public const string ResponseJSON = "responseJSON";
            public const string ResponseXML = "responseXML";
            // Consumer
            public const string ConsumerASID = "consumerASID";
            // Producer
            public const string ProviderASID = "providerASID";
        }

        // Protocol

        public string Protocol => SecurityContext.UseTLS ? "https://" : "http://";

        // Web Proxy
        public bool UseWebProxy
        {
            get { return ScenarioContext.Get<bool>(Context.UseWebProxy); }
            set
            {
                Log.WriteLine("{0}={1}", Context.UseWebProxy, value);
                ScenarioContext.Set(value, Context.UseWebProxy);
            }
        }

        public string WebProxyUrl
        {
            get { return ScenarioContext.Get<string>(Context.WebProxyUrl); }
            set
            {
                Log.WriteLine("{0}={1}", Context.WebProxyUrl, value);
                ScenarioContext.Set(value, Context.WebProxyUrl);
            }
        }

        public string WebProxyPort
        {
            get { return ScenarioContext.Get<string>(Context.WebProxyPort); }
            set
            {
                Log.WriteLine("{0}={1}", Context.WebProxyPort, value);
                ScenarioContext.Set(value, Context.WebProxyPort);
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
            get { return ScenarioContext.Get<bool>(Context.UseSpineProxy); }
            set { ScenarioContext.Set(value, Context.UseSpineProxy); }
        }

        public string SpineProxyUrl
        {
            get { return ScenarioContext.Get<string>(Context.SpineProxyUrl); }
            set
            {
                Log.WriteLine("{0}={1}", Context.SpineProxyUrl, value);
                ScenarioContext.Set(value, Context.SpineProxyUrl);
            }
        }

        public string SpineProxyPort
        {
            get { return ScenarioContext.Get<string>(Context.SpineProxyPort); }
            set
            {
                Log.WriteLine("{0}={1}", Context.SpineProxyPort, value);
                ScenarioContext.Set(value, Context.SpineProxyPort);
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
            get { return ScenarioContext.Get<string>(Context.RequestMethod); }
            set
            {
                Log.WriteLine("{0}={1}", Context.RequestMethod, value);
                ScenarioContext.Set(value, Context.RequestMethod);
            }
        }

        public string RequestUrl
        {
            get { return ScenarioContext.Get<string>(Context.RequestUrl); }
            set
            {
                Log.WriteLine("{0}={1}", Context.RequestUrl, value);
                ScenarioContext.Set(value, Context.RequestUrl);
            }
        }

        public string RequestContentType
        {
            get { return ScenarioContext.Get<string>(Context.RequestContentType); }
            set
            {
                Log.WriteLine("{0}={1}", Context.RequestContentType, value);
                ScenarioContext.Set(value, Context.RequestContentType);
            }
        }

        public string RequestBody
        {
            get { return ScenarioContext.Get<string>(Context.RequestBody); }
            set
            {
                Log.WriteLine("{0}={1}", Context.RequestBody, value);
                ScenarioContext.Set(value, Context.RequestBody);
            }
        }

        // Raw Response
        public string ResponseContentType
        {
            get { return ScenarioContext.Get<string>(Context.ResponseContentType); }
            set
            {
                Log.WriteLine("{0}={1}", Context.ResponseContentType, value);
                ScenarioContext.Set(value, Context.ResponseContentType);
            }
        }

        public HttpStatusCode ResponseStatusCode
        {
            get { return ScenarioContext.Get<HttpStatusCode>(Context.ResponseStatusCode); }
            set
            {
                Log.WriteLine("{0}={1}", Context.ResponseStatusCode, value);
                ScenarioContext.Set(value, Context.ResponseStatusCode);
            }
        }

        public string ResponseBody
        {
            get { return ScenarioContext.Get<string>(Context.ResponseBody); }
            set
            {
                Log.WriteLine("{0}={1}", Context.ResponseBody, value);
                ScenarioContext.Set(value, Context.ResponseBody);
            }
        }

        // Parsed Response
        public JObject ResponseJSON
        {
            get { return ScenarioContext.Get<JObject>(Context.ResponseJSON); }
            set { ScenarioContext.Set(value, Context.ResponseJSON); }
        }

        public XDocument ResponseXML
        {
            get { return ScenarioContext.Get<XDocument>(Context.ResponseXML); }
            set
            {
                Log.WriteLine("{0}={1}", Context.ResponseXML, value);
                ScenarioContext.Set(value, Context.ResponseXML);
            }
        }

        // Consumer
        public string ConsumerASID
        {
            get { return ScenarioContext.Get<string>(Context.ConsumerASID); }
            set
            {
                Log.WriteLine("{0}={1}", Context.ConsumerASID, value);
                ScenarioContext.Set(value, Context.ConsumerASID);
            }
        }

        // Provider
        public string ProviderASID
        {
            get { return ScenarioContext.Get<string>(Context.ProviderASID); }
            set
            {
                Log.WriteLine("{0}={1}", Context.ProviderASID, value);
                ScenarioContext.Set(value, Context.ProviderASID);
            }
        }

        public string FhirServerUrl
        {
            get { return ScenarioContext.Get<string>(Context.FhirServerUrl); }
            set
            {
                Log.WriteLine("{0}={1}", Context.FhirServerUrl, value);
                ScenarioContext.Set(value, Context.FhirServerUrl);
            }
        }

        public string FhirServerPort
        {
            get { return ScenarioContext.Get<string>(Context.FhirServerPort); }
            set
            {
                Log.WriteLine("{0}={1}", Context.FhirServerPort, value);
                ScenarioContext.Set(value, Context.FhirServerPort);
            }
        }

        public string FhirServerFhirBase
        {
            get { return ScenarioContext.Get<string>(Context.FhirServerFhirBase); }
            set
            {
                Log.WriteLine("{0}={1}", Context.FhirServerFhirBase, value);
                ScenarioContext.Set(value, Context.FhirServerFhirBase);
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
