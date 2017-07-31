namespace GPConnect.Provider.AcceptanceTests.Http
{
    using System;
    using System.Net;
    using System.Net.Http;
    using Constants;
    using Helpers;
    using Hl7.Fhir.Model;
    using Logger;
    using TechTalk.SpecFlow;

    public class HttpRequestConfiguration
    {
        public HttpRequestConfiguration()
        {
            RequestHeaders = new HttpHeaderHelper();
            RequestParameters = new HttpParameterHelper();
        }

        public DecompressionMethods DecompressionMethod { get; set; }

        public bool UseTls => ScenarioContext.Current.Get<bool>("useTLS");

        public string Protocol => UseTls ? "https://" : "http://";

        // Web Proxy
        public bool UseWebProxy { get; set; }

        public string WebProxyUrl { get; set; }

        public string WebProxyPort { get; set; }

        public string WebProxyAddress => Protocol + WebProxyUrl + ":" + WebProxyPort;

        // Spine Proxy
        public bool UseSpineProxy { get; set; }

        public string SpineProxyUrl { get; set; }

        public string SpineProxyPort { get; set; }

        public string SpineProxyAddress => Protocol + SpineProxyUrl + ":" + SpineProxyPort;

        // Raw Request
        public string RequestMethod { get; set; }
        
        public string RequestUrl { get; set; }

        public string RequestContentType { get; set; }

        public string RequestBody { get; set; }

        // Consumer
        public string ConsumerASID { get; set; }

        // Provider
        public string ProviderASID { get; set; }

        public string FhirServerUrl { get; set; }

        public string FhirServerPort { get; set; }

        public string FhirServerFhirBase { get; set; }

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
        public HttpHeaderHelper RequestHeaders { get; set; }
        public HttpParameterHelper RequestParameters { get; set; }

        public void LoadAppConfig()
        {
            Log.WriteLine("HttpContext->LoadAppConfig()");
           
            FhirServerUrl = AppSettingsHelper.ServerUrl;
            FhirServerPort = AppSettingsHelper.ServerPort;
            FhirServerFhirBase = AppSettingsHelper.ServerBase;
            UseWebProxy = AppSettingsHelper.UseWebProxy;
            WebProxyUrl = AppSettingsHelper.WebProxyUrl;
            WebProxyPort = AppSettingsHelper.WebProxyPort;
            UseSpineProxy = AppSettingsHelper.UseSpineProxy;
            SpineProxyUrl = AppSettingsHelper.SpineProxyUrl;
            SpineProxyPort = AppSettingsHelper.SpineProxyPort;
            ProviderASID = AppSettingsHelper.ProviderASID;
            ConsumerASID = AppSettingsHelper.ConsumerASID;
        }

        public Parameters BodyParameters { get; set; }

        public HttpMethod HttpMethod { get; set; }

        public string GetRequestId { get; set; }

        public string GetRequestVersionId { get; set; }

        public void SetDefaultHeaders()
        {
            RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspTraceId, Guid.NewGuid().ToString());
            RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspFrom, ConsumerASID);
            RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspTo, ProviderASID);

            RequestHeaders.ReplaceHeader(HttpConst.Headers.kAccept, FhirConst.ContentTypes.kJsonFhir);
            RequestContentType = FhirConst.ContentTypes.kJsonFhir;
        }

    }
}
