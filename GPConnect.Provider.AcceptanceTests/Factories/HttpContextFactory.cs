namespace GPConnect.Provider.AcceptanceTests.Factories
{
    using System;
    using System.Net.Http;
    using Constants;
    using Context;
    using Enum;

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
                case GpConnectInteraction.LocationSearch:
                    ConfigureLocationSearchContext(httpContext);
                    break;
                case GpConnectInteraction.LocationRead:
                    ConfigureLocationReadContext(httpContext);
                    break;
                case GpConnectInteraction.RegisterPatient:
                    ConfigureRegisterPatientContext(httpContext);
                    break;
                case GpConnectInteraction.GpcGetSchedule:
                    ConfigureGpcGetScheduleContext(httpContext);
                    break;
                case GpConnectInteraction.AppointmentCreate:
                    ConfigureAppointmentCreate(httpContext);
                    break;
                case GpConnectInteraction.AppointmentSearch:
                    ConfigureAppointmentSearchContext(httpContext);
                    break;
                case GpConnectInteraction.AppointmentAmend:
                    ConfigureAppointmentAmendContext(httpContext);
                    break;
                case GpConnectInteraction.AppointmentCancel:
                    ConfigureAppointmentCancelContext(httpContext);
                    break;
                case GpConnectInteraction.AppointmentRead:
                    ConfigureAppointmentReadContext(httpContext);
                    break;
                case GpConnectInteraction.MetadataRead:
                    ConfigureMetadataReadContext(httpContext);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_gpConnectInteraction), _gpConnectInteraction, null);
            }
        }

        private static void SetHttpContextDefaults(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.SetDefaultHeaders();
            httpContext.HttpRequestConfiguration.LoadAppConfig();
        }

        private static void ConfigureGpcGetCareRecordContext(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.HttpMethod = HttpMethod.Post;
            httpContext.HttpRequestConfiguration.RequestUrl = "Patient/$gpc.getcarerecord";
            httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.GpcGetCareRecord);
        }

        private static void ConfigureOrganizationSearchContext(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.HttpMethod = HttpMethod.Get;
            httpContext.HttpRequestConfiguration.RequestUrl = "Organization";
            httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.OrganizationSearch);
        }

        private static void ConfigureOrganizationReadContext(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.HttpMethod = HttpMethod.Get;
            httpContext.HttpRequestConfiguration.RequestUrl = "Organization/" + httpContext.HttpRequestConfiguration.GetRequestId;

            if (!string.IsNullOrEmpty(httpContext.HttpRequestConfiguration.GetRequestVersionId))
            {
                httpContext.HttpRequestConfiguration.RequestUrl = httpContext.HttpRequestConfiguration.RequestUrl + "/_history/" + httpContext.HttpRequestConfiguration.GetRequestVersionId;
            }

            httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.OrganizationRead);
        }

        private static void ConfigurePractitionerSearchContext(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.HttpMethod = HttpMethod.Get;
            httpContext.HttpRequestConfiguration.RequestUrl = "Practitioner";
            httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.PractitionerSearch);
        }

        private static void ConfigurePractitionerReadContext(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.HttpMethod = HttpMethod.Get;
            httpContext.HttpRequestConfiguration.RequestUrl = "Practitioner/" + httpContext.HttpRequestConfiguration.GetRequestId;

            if (!string.IsNullOrEmpty(httpContext.HttpRequestConfiguration.GetRequestVersionId))
            {
                httpContext.HttpRequestConfiguration.RequestUrl = httpContext.HttpRequestConfiguration.RequestUrl + "/_history/" + httpContext.HttpRequestConfiguration.GetRequestVersionId;
            }

            httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.PractitionerRead);
        }

        private static void ConfigurePatientSearchContext(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.HttpMethod = HttpMethod.Get;
            httpContext.HttpRequestConfiguration.RequestUrl = "Patient";
            httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.PatientSearch);
        }

        private static void ConfigurePatientReadContext(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.HttpMethod = HttpMethod.Get;
            httpContext.HttpRequestConfiguration.RequestUrl = "Patient/" + httpContext.HttpRequestConfiguration.GetRequestId;

            if (!string.IsNullOrEmpty(httpContext.HttpRequestConfiguration.GetRequestVersionId))
            {
                httpContext.HttpRequestConfiguration.RequestUrl = httpContext.HttpRequestConfiguration.RequestUrl + "/_history/" + httpContext.HttpRequestConfiguration.GetRequestVersionId;
            }

            httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.PatientRead);
        }

        private static void ConfigureLocationSearchContext(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.HttpMethod = HttpMethod.Get;
            httpContext.HttpRequestConfiguration.RequestUrl = "Location";
            httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.LocationSearch);
        }

        private static void ConfigureLocationReadContext(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.HttpMethod = HttpMethod.Get;
            httpContext.HttpRequestConfiguration.RequestUrl = "Location/" + httpContext.HttpRequestConfiguration.GetRequestId; 
            httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.LocationRead);
        }

        private static void ConfigureRegisterPatientContext(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.HttpMethod = HttpMethod.Post;
            httpContext.HttpRequestConfiguration.RequestUrl = "Patient/$gpc.registerpatient";
            httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.RegisterPatient);
        }

        private static void ConfigureGpcGetScheduleContext(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.HttpMethod = HttpMethod.Post;
            httpContext.HttpRequestConfiguration.RequestUrl = "Organization/" + httpContext.StoredOrganization.Id + "/$gpc.getschedule";
            httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.GpcGetSchedule);
        }

        private static void ConfigureAppointmentCreate(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.HttpMethod = HttpMethod.Post;
            httpContext.HttpRequestConfiguration.RequestUrl = "Appointment";
            httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.AppointmentCreate);
        }

        private static void ConfigureAppointmentSearchContext(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.HttpMethod = HttpMethod.Get;
            httpContext.HttpRequestConfiguration.RequestUrl = "Patient/" + httpContext.StoredPatient.Id + "/Appointment";
            httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.AppointmentSearch);
        }
        private static void ConfigureAppointmentAmendContext(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.HttpMethod = HttpMethod.Put;
            httpContext.HttpRequestConfiguration.RequestUrl = "Appointment/" + httpContext.CreatedAppointment?.Id;
            httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.AppointmentAmend);
        }

        private static void ConfigureAppointmentCancelContext(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.HttpMethod = HttpMethod.Put; 
            httpContext.HttpRequestConfiguration.RequestUrl = "Appointment/" + httpContext.CreatedAppointment.Id;
            httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.AppointmentCancel);
        }

        private static void ConfigureAppointmentReadContext(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.HttpMethod = HttpMethod.Get;
            httpContext.HttpRequestConfiguration.RequestUrl = "Appointment/" + httpContext.CreatedAppointment.Id;
            httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.AppointmentRead);
        }

        private static void ConfigureMetadataReadContext(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.HttpMethod = HttpMethod.Get;
            httpContext.HttpRequestConfiguration.RequestUrl = "metadata";
            httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.MetadataRead);
        }
    }
}
