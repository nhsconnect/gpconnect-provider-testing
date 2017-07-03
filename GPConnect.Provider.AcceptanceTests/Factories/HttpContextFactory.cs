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
                case GpConnectInteraction.OrganizationSearch:
                    ConfigureOrganizationSearchContext(httpContext);
                    break;
                case GpConnectInteraction.OrganizationRead:
                    ConfigureOrganizationReadContext(httpContext);
                    break;
                case GpConnectInteraction.PractitionerSearch:
                    ConfigurePractitionerSearchContext(httpContext);
                    break;
                case GpConnectInteraction.PractitionerRead:
                    ConfigurePractitionerReadContext(httpContext);
                    break;
                case GpConnectInteraction.PatientSearch:
                    ConfigurePatientSearchContext(httpContext);
                    break;
                case GpConnectInteraction.PatientRead:
                    ConfigurePatientReadContext(httpContext);
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
            httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.GpcGetCareRecord);
        }

        private static void ConfigureOrganizationSearchContext(HttpContext httpContext)
        {
            httpContext.HttpMethod = HttpMethod.Get;
            httpContext.RequestUrl = "Organization";
            httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.OrganizationSearch);
        }

        private static void ConfigureOrganizationReadContext(HttpContext httpContext)
        {
            httpContext.HttpMethod = HttpMethod.Get;
            httpContext.RequestUrl = "Organization/" + httpContext.GetRequestId;
            httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.OrganizationRead);
        }

        private static void ConfigurePractitionerSearchContext(HttpContext httpContext)
        {
            httpContext.HttpMethod = HttpMethod.Get;
            httpContext.RequestUrl = "Practitioner";
            httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.PractitionerSearch);
        }

        private static void ConfigurePractitionerReadContext(HttpContext httpContext)
        {
            httpContext.HttpMethod = HttpMethod.Get;
            httpContext.RequestUrl = "Practitioner/" + httpContext.GetRequestId;
            httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.PractitionerRead);
        }

        private static void ConfigurePatientSearchContext(HttpContext httpContext)
        {
            httpContext.HttpMethod = HttpMethod.Get;
            httpContext.RequestUrl = "Patient";
            httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.PatientSearch);
        }

        private static void ConfigurePatientReadContext(HttpContext httpContext)
        {
            httpContext.HttpMethod = HttpMethod.Get;
            httpContext.RequestUrl = "Patient/";
            httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.PatientRead);
        }

        //TODO: Remove fhirContext dependencies
        public void ConfigureFhirContext(FhirContext fhirContext)
        {
            fhirContext.FhirResponseResource = null;
            fhirContext.FhirRequestParameters = new Parameters();
        }
    }
}
