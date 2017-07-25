﻿namespace GPConnect.Provider.AcceptanceTests.Factories
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

            if (!string.IsNullOrEmpty(httpContext.GetRequestVersionId))
            {
                httpContext.RequestUrl = httpContext.RequestUrl + "/_history/" + httpContext.GetRequestVersionId;
            }

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

            if (!string.IsNullOrEmpty(httpContext.GetRequestVersionId))
            {
                httpContext.RequestUrl = httpContext.RequestUrl + "/_history/" + httpContext.GetRequestVersionId;
            }

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
            httpContext.RequestUrl = "Patient/" + httpContext.GetRequestId;

            if (!string.IsNullOrEmpty(httpContext.GetRequestVersionId))
            {
                httpContext.RequestUrl = httpContext.RequestUrl + "/_history/" + httpContext.GetRequestVersionId;
            }

            httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.PatientRead);
        }

        private static void ConfigureLocationSearchContext(HttpContext httpContext)
        {
            httpContext.HttpMethod = HttpMethod.Get;
            httpContext.RequestUrl = "Location";
            httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.LocationSearch);
        }

        private static void ConfigureLocationReadContext(HttpContext httpContext)
        {
            httpContext.HttpMethod = HttpMethod.Get;
            httpContext.RequestUrl = "Location/" + httpContext.GetRequestId; 
            httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.LocationRead);
        }

        private static void ConfigureRegisterPatientContext(HttpContext httpContext)
        {
            httpContext.HttpMethod = HttpMethod.Post;
            httpContext.RequestUrl = "Patient/$gpc.registerpatient";
            httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.RegisterPatient);
        }

        private static void ConfigureGpcGetScheduleContext(HttpContext httpContext)
        {
            httpContext.HttpMethod = HttpMethod.Post;
            httpContext.RequestUrl = "Organization/" + httpContext.StoredOrganization.Id + "/$gpc.getschedule";
            httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.GpcGetSchedule);
        }

        private static void ConfigureAppointmentCreate(HttpContext httpContext)
        {
            httpContext.HttpMethod = HttpMethod.Post;
            httpContext.RequestUrl = "Appointment";
            httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.AppointmentCreate);
        }

        private static void ConfigureAppointmentSearchContext(HttpContext httpContext)
        {
            httpContext.HttpMethod = HttpMethod.Get;;
            httpContext.RequestUrl = "Patient/" + httpContext.StoredPatient.Id + "/Appointment";
            httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.AppointmentSearch);
        }
        private static void ConfigureAppointmentAmendContext(HttpContext httpContext)
        {
            httpContext.HttpMethod = HttpMethod.Put;
            httpContext.RequestUrl = "Appointment/" + httpContext.CreatedAppointment?.Id;
            httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.AppointmentAmend);
        }

        private static void ConfigureAppointmentCancelContext(HttpContext httpContext)
        {
            httpContext.HttpMethod = HttpMethod.Put; 
            httpContext.RequestUrl = "Appointment/" + httpContext.CreatedAppointment.Id;
            httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.AppointmentCancel);
        }

        private static void ConfigureAppointmentReadContext(HttpContext httpContext)
        {
            httpContext.HttpMethod = HttpMethod.Get;
            httpContext.RequestUrl = "Appointment/" + httpContext.CreatedAppointment.Id;
            httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.AppointmentRead);
        }

        private static void ConfigureMetadataReadContext(HttpContext httpContext)
        {
            httpContext.HttpMethod = HttpMethod.Get;
            httpContext.RequestUrl = "metadata";
            httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.MetadataRead);
        }

        //TODO: Remove fhirContext dependencies
        public void ConfigureFhirContext(FhirContext fhirContext)
        {
            fhirContext.FhirResponseResource = null;
            fhirContext.FhirRequestParameters = new Parameters();
        }
    }
}
