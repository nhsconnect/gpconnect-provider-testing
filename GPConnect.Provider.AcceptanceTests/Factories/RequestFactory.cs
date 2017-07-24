namespace GPConnect.Provider.AcceptanceTests.Factories
{
    using Constants;
    using Context;
    using Enum;
    using Helpers;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;
    using Hl7.Fhir.Serialization;

    public class RequestFactory
    {
        private readonly GpConnectInteraction _gpConnectInteraction;
        private delegate string Serializer(Base data, SummaryType summaryType = SummaryType.False, string root = null);
        private static Serializer _serializer;

        public RequestFactory(GpConnectInteraction gpConnectInteraction)
        {
            _gpConnectInteraction = gpConnectInteraction;
        }

        public void ConfigureBody(HttpContext httpContext)
        {
            ConfigureSerializer(httpContext);

            switch (_gpConnectInteraction)
            {
                case GpConnectInteraction.GpcGetCareRecord:
                    ConfigureGpcGetCareRecordBody(httpContext);
                    break;
                case GpConnectInteraction.RegisterPatient:
                    ConfigureRegisterPatientBody(httpContext);
                    break;
                case GpConnectInteraction.GpcGetSchedule:
                    ConfigureGpcGetSchedule(httpContext);
                    break;
                case GpConnectInteraction.AppointmentCreate:
                    ConfigureAppointmentCreateBody(httpContext);
                    break;
                case GpConnectInteraction.AppointmentAmend:
                    ConfigureAppointmentAmendBody(httpContext);
                    break;
                case GpConnectInteraction.AppointmentCancel:
                    ConfigureAppointmentCancelBody(httpContext);
                    break;
            }
        }

        private static void ConfigureSerializer(HttpContext httpContext)
        {
            _serializer = httpContext.RequestContentType.Contains("xml")
                ? new Serializer(FhirSerializer.SerializeToXml)
                : FhirSerializer.SerializeToJson;
        }

        private static void ConfigureAppointmentCreateBody(HttpContext httpContext)
        {
            httpContext.RequestBody = _serializer(httpContext.CreatedAppointment);
        }

        private static void ConfigureGpcGetCareRecordBody(HttpContext httpContext)
        {
            httpContext.RequestBody = _serializer(httpContext.BodyParameters);
        }

        private static void ConfigureRegisterPatientBody(HttpContext httpContext)
        {
            httpContext.RequestBody = _serializer(httpContext.BodyParameters);
        }

        private static void ConfigureGpcGetSchedule(HttpContext httpContext)
        {
            httpContext.RequestBody = _serializer(httpContext.BodyParameters);
        }

        private static void ConfigureAppointmentAmendBody(HttpContext httpContext)
        {
            httpContext.RequestBody = _serializer(httpContext.CreatedAppointment);
        }

        private static void ConfigureAppointmentCancelBody(HttpContext httpContext)
        {
            httpContext.RequestBody = _serializer(httpContext.CreatedAppointment);
        }

        public void ConfigureInvalidResourceType(HttpContext httpContext)
        {
            httpContext.RequestBody = FhirHelper.ChangeResourceTypeString(httpContext.RequestBody, FhirConst.Resources.kInvalidResourceType);
        }

        public void ConfigureInvalidParameterResourceType(HttpContext httpContext)
        {
            httpContext.RequestBody = FhirHelper.ChangeParameterResourceTypeString(httpContext.RequestBody, FhirConst.Resources.kInvalidResourceType);
        }

        public void ConfigureParameterResourceWithAdditionalField(HttpContext httpContext)
        {
            httpContext.RequestBody = FhirHelper.AddFieldToParameterResource(httpContext.RequestBody, FhirConst.Resources.kInvalidResourceType);
        }

        public void ConfigureAdditionalInvalidFieldInResource(HttpContext httpContext)
        {
            httpContext.RequestBody = FhirHelper.AddInvalidFieldToResourceJson(httpContext.RequestBody);
        }
    }
}
