namespace GPConnect.Provider.AcceptanceTests.Factories
{
    using System;
    using System.Net.Http;
    using Constants;
    using Context;
    using Enum;
    using Hl7.Fhir.Model;

    public class HttpContextFactory
    {
        private readonly GpConnectInteraction _gpConnectInteraction;

        public HttpContextFactory(GpConnectInteraction gpConnectInteraction)
        {
            _gpConnectInteraction = gpConnectInteraction;
        }

        public void ConfigureHttpContext(HttpContext httpContext)
        {
            SetHttpContextDefaults(httpContext);

            switch (_gpConnectInteraction)
            {
                case GpConnectInteraction.GpcGetCareRecord:
                    ConfigureGpcGetCareRecordContext(httpContext);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_gpConnectInteraction), _gpConnectInteraction, null);
            }
        }

        private static void SetHttpContextDefaults(HttpContext httpContext)
        {
            httpContext.SetDefaults();
            httpContext.SetDefaultHeaders();
            httpContext.LoadAppConfig();
        }

        private static void ConfigureGpcGetCareRecordContext(HttpContext httpContext)
        {
            httpContext.HttpMethod = HttpMethod.Post;
            httpContext.RequestUrl = "Patient/$gpc.getcarerecord";
            httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.kFhirOperationGetCareRecord);
        }

        //TODO: Remove fhirContext dependencies
        public void ConfigureFhirContext(FhirContext fhirContext)
        {
            fhirContext.FhirResponseResource = null;
            fhirContext.FhirRequestParameters = new Parameters();
        }
    }
}
