namespace GPConnect.Provider.AcceptanceTests.Context
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Xml.Linq;
    using Constants;
    using Helpers;
    using Hl7.Fhir.Model;
    using Logger;
    using Newtonsoft.Json.Linq;
    using TechTalk.SpecFlow;

    // ReSharper disable once ClassNeverInstantiated.Global
    public interface IHttpContext
    {
        // Headers Helper
        HttpHeaderHelper RequestHeaders { get; }
        // Http Helper
        HttpParameterHelper RequestParameters { get; }
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
        long ResponseTimeInMilliseconds { get; set; }
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
        Dictionary<string, Hl7.Fhir.Model.Resource> StoredFhirResources { get;}
        Dictionary<string, List<Hl7.Fhir.Model.Resource>> StoredSlots { get; }
        Dictionary<string, Appointment> StoredAppointment { get; }
        Dictionary<string, string> StoredDate { get; }
        Dictionary<string, Patient> registerPatient { get; }
        Dictionary<string, string> resourceNameStored { get; }
    }

    public class HttpContext : IHttpContext
    {
        public readonly ScenarioContext ScenarioContext;

        // Headers Helper
        public HttpHeaderHelper RequestHeaders { get; }

        // Http Helper
        public HttpParameterHelper RequestParameters { get; set; }

        // JWT Helper
        public JwtHelper Jwt { get; }

        // Security Context
        public SecurityContext SecurityContext { get; }

        public HttpContext(ScenarioContext scenarioContext, HttpHeaderHelper requestHeaderHelper, JwtHelper jwtHelper, SecurityContext securityContext, HttpParameterHelper requestParametersHelper)
        {
            ScenarioContext = scenarioContext;
            RequestHeaders = requestHeaderHelper;
            Jwt = jwtHelper;
            SecurityContext = securityContext;
            RequestParameters = requestParametersHelper;
            BodyParameters = new Parameters();
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
            // Parsed Response
            public const string kResponseJSON = "responseJSON";
            public const string kResponseXML = "responseXML";
            // Consumer
            public const string kConsumerASID = "consumerASID";
            // Producer
            public const string kProviderASID = "providerASID";

            public const string kStoredFhirResources = "storedFhirResources";
            public const string kStoredSlots = "storedSlots";
            public const string kStoredAppointment = "storedAppointment";
            public const string kStoredDate = "storedDate";
            public const string kRegisterPatient = "registerPatient";
            public const string kResourceNameStored = "resourceNameStored";
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
            get {
                try
                {
                    return ScenarioContext.Get<string>(Context.kRequestMethod);
                }
                catch (Exception) {
                    return "";
                }
            }
            set
            {
                Log.WriteLine("{0}={1}", Context.kRequestMethod, value);
                ScenarioContext.Set(value, Context.kRequestMethod);
            }
        }

        public string RequestUrl
        {
            get {
                try
                {
                    return ScenarioContext.Get<string>(Context.kRequestUrl);
                }
                catch (Exception) {
                    return "";
                }
            }
            set
            {
                Log.WriteLine("{0}={1}", Context.kRequestUrl, value);
                ScenarioContext.Set(value, Context.kRequestUrl);
            }
        }

        public string RequestContentType
        {
            get {
                try
                {
                    return ScenarioContext.Get<string>(Context.kRequestContentType);
                }
                catch (Exception) {
                    return "";
                }
            }
            set
            {
                Log.WriteLine("{0}={1}", Context.kRequestContentType, value);
                ScenarioContext.Set(value, Context.kRequestContentType);
            }
        }

        public string RequestBody
        {
            get
            {
                try
                {
                    return ScenarioContext.Get<string>(Context.kRequestBody);
                } catch (Exception)
                {
                    return "";
                }
            }
            set
            {
                Log.WriteLine("{0}={1}", Context.kRequestBody, value);
                ScenarioContext.Set(value, Context.kRequestBody);
            }
        }

        // Raw Response
        public string ResponseContentType
        {
            get {
                try
                {
                    return ScenarioContext.Get<string>(Context.kResponseContentType);
                } catch (Exception)
                {
                    return "";
                }
            }
            set
            {
                Log.WriteLine("{0}={1}", Context.kResponseContentType, value);
                ScenarioContext.Set(value, Context.kResponseContentType);
            }
        }

        public HttpStatusCode ResponseStatusCode
        {
            get {
                try
                {
                    return ScenarioContext.Get<HttpStatusCode>(Context.kResponseStatusCode);
                } catch (Exception)
                {
                    return HttpStatusCode.OK;
                }
            }
            set
            {
                Log.WriteLine("{0}={1}", Context.kResponseStatusCode, value);
                ScenarioContext.Set(value, Context.kResponseStatusCode);
            }
        }

        public string ResponseBody
        {
            get {
                try
                {
                    return ScenarioContext.Get<string>(Context.kResponseBody);
                } catch (Exception)
                {
                    return "";
                }
            }
            set
            {
                Log.WriteLine("{0}={1}", Context.kResponseBody, value);
                ScenarioContext.Set(value, Context.kResponseBody);
            }
        }

        public long ResponseTimeInMilliseconds
        {
            get
            {
                try
                {
                    return ScenarioContext.Get<long>(Context.kResponseTimeInMilliseconds);
                }
                catch (Exception) {
                    return -1;
                }
            }
            set
            {
                Log.WriteLine("{0}={1}", Context.kResponseTimeInMilliseconds, value);
                ScenarioContext.Set(value, Context.kResponseTimeInMilliseconds);
                // Is The Response Time Acceptable Based On The 'P' Limit
                ResponseTimeAcceptable = value <= 1000;
            }
        }

        private bool ResponseTimeAcceptable
        {
            get { return ScenarioContext.Get<bool>(Context.kResponseTimeAcceptable); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kResponseTimeAcceptable, value);
                ScenarioContext.Set(value, Context.kResponseTimeAcceptable);
            }
        }

        public Dictionary<string, string> ResponseHeaders
        {
            get
            {
                try
                {
                    var responseHeaders = ScenarioContext.Get<Dictionary<string, string>>(Context.kResponseHeaders);
                    if (responseHeaders == null)
                    {
                        // If a dictionary does not exist create it and return
                        ScenarioContext.Set(new Dictionary<string, string>(), Context.kResponseHeaders);
                        responseHeaders = ScenarioContext.Get<Dictionary<string, string>>(Context.kResponseHeaders);
                    }
                    return responseHeaders;
                }
                catch (Exception) {
                    ScenarioContext.Set(new Dictionary<string, string>(), Context.kResponseHeaders);
                    return ScenarioContext.Get<Dictionary<string, string>>(Context.kResponseHeaders);
                }
            }
            set { ScenarioContext.Set(value, Context.kResponseHeaders); }
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

        public Dictionary<string, Resource> StoredFhirResources {
            get {
                try
                {
                    return ScenarioContext.Get<Dictionary<string, Resource>>(Context.kStoredFhirResources);
                }
                catch (Exception) {
                }
                try
                {
                    ScenarioContext.Set(new Dictionary<string, Resource>(), Context.kStoredFhirResources);
                    return ScenarioContext.Get<Dictionary<string, Resource>>(Context.kStoredFhirResources);
                }
                catch (Exception) {
                }
                return null;
            }
        }


        public Dictionary<string, List<Resource>> StoredSlots
        {
            get
            {
                try
                {
                   
                    return ScenarioContext.Get<Dictionary<string, List<Resource>>>(Context.kStoredSlots);
                }
                catch (Exception)
                {
                }
                try
                {
                  
                    ScenarioContext.Set(new Dictionary<string, List<Resource>>(), Context.kStoredSlots);
                    return ScenarioContext.Get<Dictionary<string, List<Resource>>>(Context.kStoredSlots);
                }
                catch (Exception)
                {
                }
                return null;
            }
        }


        public Dictionary<string, Appointment> StoredAppointment
        {
            get
            {
                try
                {

                    return ScenarioContext.Get<Dictionary<string, Appointment>>(Context.kStoredAppointment);
                }
                catch (Exception)
                {
                }
                try
                {

                    ScenarioContext.Set(new Dictionary<string, Appointment>(), Context.kStoredAppointment);
                    return ScenarioContext.Get<Dictionary<string, Appointment>>(Context.kStoredAppointment);
                }
                catch (Exception)
                {
                }
                return null;
            }
        }


        public Dictionary<string, string> StoredDate
        {
            get
            {
                try
                {

                    return ScenarioContext.Get<Dictionary<string, string>>(Context.kStoredDate);
                }
                catch (Exception)
                {
                }
                try
                {

                    ScenarioContext.Set(new Dictionary<string, string>(), Context.kStoredDate);
                    return ScenarioContext.Get<Dictionary<string, string>>(Context.kStoredDate);
                }
                catch (Exception)
                {
                }
                return null;
            }
        }

        public Dictionary<string, string> resourceNameStored
        {
            get
            {
                try
                {

                    return ScenarioContext.Get<Dictionary<string, string>>(Context.kResourceNameStored);
                }
                catch (Exception)
                {
                }
                try
                {

                    ScenarioContext.Set(new Dictionary<string, string>(), Context.kResourceNameStored);
                    return ScenarioContext.Get<Dictionary<string, string>>(Context.kResourceNameStored);
                }
                catch (Exception)
                {
                }
                return null;
            }
        }

        public Dictionary<string, Patient> registerPatient
        {
            get
            {
                try
                {

                    return ScenarioContext.Get<Dictionary<string, Patient>>(Context.kRegisterPatient);
                }
                catch (Exception)
                {
                }
                try
                {

                    ScenarioContext.Set(new Dictionary<string, Patient>(), Context.kRegisterPatient);
                    return ScenarioContext.Get<Dictionary<string, Patient>>(Context.kRegisterPatient);
                }
                catch (Exception)
                {
                }
                return null;
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

        public void SaveToDisk(string filename)
        {
            var requestHeaders = new XElement(Context.kRequestHeaders);
            foreach (var entry in RequestHeaders.GetRequestHeaders())
            {
                requestHeaders.Add(new XElement("requestHeader", new XAttribute("name", entry.Key), new XAttribute("value", entry.Value)));
            }
            var requestParameters = new XElement(Context.kRequestParameters);
            foreach (var entry in RequestParameters.GetRequestParameters())
            {
                requestParameters.Add(new XElement("requestParameter", new XAttribute("name", entry.Key), new XAttribute("value", entry.Value)));
            }
            var responseHeaders = new XElement(Context.kResponseHeaders);
            foreach (var entry in ResponseHeaders)
            {
                responseHeaders.Add(new XElement("responseHeader", new XAttribute("name", entry.Key), new XAttribute("value", entry.Value)));
            }

            var doc = new XDocument(
                new XElement("httpContext",
                    new XAttribute(Context.kUseWebProxy, UseWebProxy),
                    new XAttribute(Context.kWebProxyUrl, WebProxyAddress),
                    new XAttribute(Context.kUseSpineProxy, UseSpineProxy),
                    new XAttribute(Context.kSpineProxyUrl, SpineProxyAddress),
                    new XAttribute("providerUrl", ProviderAddress),
                    new XElement("request",
                        new XAttribute("endpointUrl", EndpointAddress),
                        requestHeaders,
                        new XElement(Context.kRequestUrl, RequestUrl),
                        requestParameters,
                        new XElement(Context.kRequestMethod, RequestMethod),
                        new XElement(Context.kRequestContentType, RequestContentType),
                        new XElement(Context.kRequestBody, System.Security.SecurityElement.Escape(RequestBody))),
                    new XElement("response",
                        new XElement(Context.kResponseContentType, ResponseContentType),
                        new XElement(Context.kResponseStatusCode, (int)ResponseStatusCode),
                        new XElement(Context.kResponseTimeInMilliseconds, ResponseTimeInMilliseconds),
                        new XElement(Context.kResponseTimeAcceptable, ResponseTimeAcceptable),
                        responseHeaders,
                        new XElement(Context.kResponseBody, System.Security.SecurityElement.Escape(ResponseBody)))
                ));
            doc.Save(filename);
        }

        public void SetDefaults()
        {
            RequestHeaders.Clear();
            RequestUrl = "";
            RequestParameters.ClearParameters();
            RequestBody = null;
            ResponseStatusCode = default(HttpStatusCode);
            ResponseTimeInMilliseconds = -1;
            ResponseContentType = null;
            ResponseBody = null;
            ResponseHeaders.Clear();
        }

        public void SetDefaultHeaders()
        {
            RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspTraceId, Guid.NewGuid().ToString());
            RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspFrom, ConsumerASID);
            RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspTo, ProviderASID);

            RequestHeaders.ReplaceHeader(HttpConst.Headers.kAccept, FhirConst.ContentTypes.kJsonFhir);
            RequestContentType = FhirConst.ContentTypes.kJsonFhir;
        }

        private string GetBaseUrl()
        {
            var sspAddress = UseSpineProxy ? SpineProxyAddress + "/" : string.Empty;

            var baseUrl = sspAddress + Protocol + FhirServerUrl + ":" + FhirServerPort + FhirServerFhirBase;

            if (baseUrl[baseUrl.Length - 1] != '/')
            {
                baseUrl = baseUrl + "/";
            }

            return baseUrl;
        }

        public string BaseUrl => GetBaseUrl();

        public Parameters BodyParameters { get; set; }

        public HttpMethod HttpMethod { get; set; }
    }
}