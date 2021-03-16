namespace GPConnect.Provider.AcceptanceTests.Http
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using Constants;
    using GPConnect.Provider.AcceptanceTests.Context;
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
            RequestHeaders.Clear();
            RequestUrl = "";
            RequestParameters.ClearParameters();
            RequestBody = null;
            BodyParameters = new Parameters();
            LoadAppConfig();
            SetDefaultHeaders();
        }

        public string SspProtocol = "https://";

        public bool UseTlsFoundationsAndAppmts => ScenarioContext.Current.Get<bool>("useTLSFoundationsAndAppmts");
        public string ProtocolFoundationsAndAppmts => UseTlsFoundationsAndAppmts ? "https://" : "http://";

        public bool UseTlsStructured => ScenarioContext.Current.Get<bool>("useTLSStructured");
        public string ProtocolStructured => UseTlsStructured ? "https://" : "http://";

        public bool UseTlsDocuments => ScenarioContext.Current.Get<bool>("useTLSDocuments");
        public string ProtocolDocuments => UseTlsDocuments ? "https://" : "http://";

        // Web Proxy
        public bool UseWebProxy { get; set; }

        public string WebProxyUrl { get; set; }

        public string WebProxyPort { get; set; }

        public string WebProxyAddress => "https://" + WebProxyUrl + ":" + WebProxyPort;

        // Spine Proxy
        public bool UseSpineProxy { get; set; }

        public string SpineProxyUrl { get; set; }

        public string SpineProxyPort { get; set; }

        public string SpineProxyAddress => SspProtocol + SpineProxyUrl + ":" + SpineProxyPort;

        // Raw Request
        public string RequestMethod { get; set; }

        public string RequestUrl { get; set; }

        public string RequestUrlParameters { get; set; }

        public string RequestContentType { get; set; }

        public string RequestBody { get; set; }

        // Consumer
        public string ConsumerASID { get; set; }

        // Provider
        public string ProviderASID { get; set; }

        public string FhirServerUrlFoundationsAndAppmts { get; set; }
        public string FhirServerPortFoundationsAndAppmts => UseTlsFoundationsAndAppmts ? FhirServerHttpsPortFoundationsAndAppmts : FhirServerHttpPortFoundationsAndAppmts;
        public string FhirServerFhirBaseFoundationsAndAppmts { get; set; }

        public string FhirServerUrlStructured { get; set; }
        public string FhirServerPortStructured => UseTlsStructured ? FhirServerHttpsPortStructured : FhirServerHttpPortStructured;
        public string FhirServerFhirBaseStructured { get; set; }

        public string FhirServerUrlDocuments { get; set; }
        public string FhirServerPortDocuments => UseTlsDocuments ? FhirServerHttpsPortDocuments : FhirServerHttpPortDocuments;
        public string FhirServerFhirBaseDocuments { get; set; }

        public string ProviderAddress
        {
            get
            {
                var currentInteraction = this.RequestHeaders.GetHeaderValue("Ssp-InteractionId");

                //If Structured Request
                if (currentInteraction == SpineConst.InteractionIds.GpcGetStructuredRecord || currentInteraction == SpineConst.InteractionIds.StructuredMetaDataRead)
                {
                    if (UseTlsStructured)
                    {
                        return ProtocolStructured + ((FhirServerPortStructured != "") ? FhirServerUrlStructured + FhirServerFhirBaseStructured : FhirServerUrlStructured + FhirServerFhirBaseStructured);
                    }
                    else
                    {
                        return ProtocolStructured + ((FhirServerPortStructured != "") ? FhirServerUrlStructured + ":" + FhirServerPortStructured + FhirServerFhirBaseStructured : FhirServerUrlStructured + FhirServerFhirBaseStructured);
                    }
                }
                //Documents
                else if (currentInteraction == SpineConst.InteractionIds.DocumentsMetaDataRead || currentInteraction == SpineConst.InteractionIds.DocumentsPatientSearch || currentInteraction == SpineConst.InteractionIds.DocumentsSearch)
                {
                    if (UseTlsDocuments)
                    {
                        return ProtocolDocuments + ((FhirServerPortDocuments != "") ? FhirServerUrlDocuments + FhirServerFhirBaseDocuments : FhirServerUrlDocuments + FhirServerFhirBaseDocuments);
                    }
                    else
                    {
                        return ProtocolDocuments + ((FhirServerPortDocuments != "") ? FhirServerUrlDocuments + ":" + FhirServerPortDocuments + FhirServerFhirBaseDocuments : FhirServerUrlStructured + FhirServerFhirBaseDocuments);
                    }
                }
                //Documents Retrieve
                else if (currentInteraction == SpineConst.InteractionIds.DocumentsRetrieve)
                {
                    return GlobalContext.DocumentURL;
                }
                //Foundations and Appointments
                else
                {
                    if (UseTlsFoundationsAndAppmts)
                    {
                        return ProtocolFoundationsAndAppmts + ((FhirServerPortFoundationsAndAppmts != "") ? FhirServerUrlFoundationsAndAppmts + FhirServerFhirBaseFoundationsAndAppmts : FhirServerUrlFoundationsAndAppmts + FhirServerFhirBaseFoundationsAndAppmts);
                    }
                    else
                    {
                        return ProtocolFoundationsAndAppmts + ((FhirServerPortFoundationsAndAppmts != "") ? FhirServerUrlFoundationsAndAppmts + ":" + FhirServerPortFoundationsAndAppmts + FhirServerFhirBaseFoundationsAndAppmts : FhirServerUrlFoundationsAndAppmts + FhirServerFhirBaseFoundationsAndAppmts);
                    }
                }
            }
        }

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
            string baseUrl;

            var currentInteraction = this.RequestHeaders.GetHeaderValue("Ssp-InteractionId");

            //If Structured Request
            if (currentInteraction == SpineConst.InteractionIds.GpcGetStructuredRecord || currentInteraction == SpineConst.InteractionIds.StructuredMetaDataRead)
            {
                if (UseTlsStructured)
                {
                    baseUrl = sspAddress + ProtocolStructured + ((FhirServerPortStructured != "") ? FhirServerUrlStructured + FhirServerFhirBaseStructured : FhirServerUrlStructured + FhirServerFhirBaseStructured);
                }
                else
                {
                    baseUrl = sspAddress + ProtocolStructured + ((FhirServerPortStructured != "") ? FhirServerUrlStructured + ":" + FhirServerPortStructured + FhirServerFhirBaseStructured : FhirServerUrlStructured + FhirServerFhirBaseStructured);
                }
            }
            //Documents
            else if (currentInteraction == SpineConst.InteractionIds.DocumentsMetaDataRead || currentInteraction == SpineConst.InteractionIds.DocumentsPatientSearch || currentInteraction == SpineConst.InteractionIds.DocumentsSearch)
            {
                if (UseTlsDocuments)
                {
                    baseUrl = sspAddress + ProtocolDocuments + ((FhirServerPortDocuments != "") ? FhirServerUrlDocuments + FhirServerFhirBaseDocuments : FhirServerUrlDocuments + FhirServerFhirBaseDocuments);
                }
                else
                {
                    baseUrl = sspAddress + ProtocolDocuments + ((FhirServerPortDocuments != "") ? FhirServerUrlDocuments + ":" + FhirServerPortDocuments + FhirServerFhirBaseDocuments : FhirServerUrlDocuments + FhirServerFhirBaseDocuments);
                }
            }
            //Documents Retrieve
            else if (currentInteraction == SpineConst.InteractionIds.DocumentsRetrieve)
            {
                baseUrl = sspAddress + GlobalContext.DocumentURL;
            }
            //Foundations and Appointments
            else
            {
                if (UseTlsFoundationsAndAppmts)
                {
                    baseUrl = sspAddress + ProtocolFoundationsAndAppmts + ((FhirServerPortFoundationsAndAppmts != "") ? FhirServerUrlFoundationsAndAppmts + FhirServerFhirBaseFoundationsAndAppmts : FhirServerUrlFoundationsAndAppmts + FhirServerFhirBaseFoundationsAndAppmts);
                }
                else
                {
                    baseUrl = sspAddress + ProtocolFoundationsAndAppmts + ((FhirServerPortFoundationsAndAppmts != "") ? FhirServerUrlFoundationsAndAppmts + ":" + FhirServerPortFoundationsAndAppmts + FhirServerFhirBaseFoundationsAndAppmts : FhirServerUrlFoundationsAndAppmts + FhirServerFhirBaseFoundationsAndAppmts);
                }
            }

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

            FhirServerUrlFoundationsAndAppmts = AppSettingsHelper.ServerUrlFoundationsAndAppmts;
            FhirServerHttpPortFoundationsAndAppmts = AppSettingsHelper.ServerHttpPortFoundationsAndAppmts;
            FhirServerHttpsPortFoundationsAndAppmts = AppSettingsHelper.ServerHttpsPortFoundationsAndAppmts;
            FhirServerFhirBaseFoundationsAndAppmts = AppSettingsHelper.ServerBaseFoundationsAndAppmts;

            FhirServerUrlStructured = AppSettingsHelper.ServerUrlStructured;
            FhirServerHttpPortStructured = AppSettingsHelper.ServerHttpPortStructured;
            FhirServerHttpsPortStructured = AppSettingsHelper.ServerHttpsPortStructured;
            FhirServerFhirBaseStructured = AppSettingsHelper.ServerBaseStructured;

            FhirServerUrlDocuments = AppSettingsHelper.ServerUrlDocuments;
            FhirServerHttpPortDocuments = AppSettingsHelper.ServerHttpPortDocuments;
            FhirServerHttpsPortDocuments = AppSettingsHelper.ServerHttpsPortDocuments;
            FhirServerFhirBaseDocuments = AppSettingsHelper.ServerBaseDocuments;

            UseWebProxy = AppSettingsHelper.UseWebProxy;
            WebProxyUrl = AppSettingsHelper.WebProxyUrl;
            WebProxyPort = AppSettingsHelper.WebProxyPort;
            UseSpineProxy = AppSettingsHelper.UseSpineProxy;
            SpineProxyUrl = AppSettingsHelper.SpineProxyUrl;
            SpineProxyPort = AppSettingsHelper.SpineProxyPort;
            ProviderASID = AppSettingsHelper.ProviderASID;
            ConsumerASID = AppSettingsHelper.ConsumerASID;
        }

        public string FhirServerHttpPort { get; set; }
        public string FhirServerHttpsPort { get; set; }

        public string FhirServerHttpPortFoundationsAndAppmts { get; set; }
        public string FhirServerHttpsPortFoundationsAndAppmts { get; set; }

        public string FhirServerHttpPortStructured { get; set; }
        public string FhirServerHttpsPortStructured { get; set; }

        public string FhirServerHttpPortDocuments { get; set; }
        public string FhirServerHttpsPortDocuments { get; set; }

        public Parameters BodyParameters { get; set; }

        public HttpMethod HttpMethod { get; set; }

        public string GetRequestId { get; set; }

        public string GetRequestVersionId { get; set; }

        public void SetDefaultHeaders()
        {
            //PG 4/3/2021 - Generate new SSP-TraceID and Store in GlobalContext To Check that Bundles returned This in id element
            var sspTraceID = Guid.NewGuid().ToString();
            GlobalContext.SSPTraceID = sspTraceID;

            RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspTraceId, GlobalContext.SSPTraceID);
            RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspFrom, ConsumerASID);
            RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspTo, ProviderASID);

            RequestHeaders.ReplaceHeader(HttpConst.Headers.kAccept, ContentType.Application.FhirJson);
            RequestContentType = ContentType.Application.FhirJson;
        }
    }
}