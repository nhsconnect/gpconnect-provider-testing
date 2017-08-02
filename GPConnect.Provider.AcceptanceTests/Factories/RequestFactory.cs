namespace GPConnect.Provider.AcceptanceTests.Factories
{
    using Constants;
    using Context;
    using Enum;
    using Helpers;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;
    using Hl7.Fhir.Serialization;
    using Repository;

    public class RequestFactory
    {
        private readonly GpConnectInteraction _gpConnectInteraction;
        private readonly IFhirResourceRepository _fhirResourceRepository;

        private delegate string Serializer(Base data, SummaryType summaryType = SummaryType.False, string root = null);
        private static Serializer _serializer;

        public RequestFactory(GpConnectInteraction gpConnectInteraction, IFhirResourceRepository fhirResourceRepository)
        {
            _gpConnectInteraction = gpConnectInteraction;
            _fhirResourceRepository = fhirResourceRepository;
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
            _serializer = httpContext.HttpRequestConfiguration.RequestContentType.Contains("xml")
                ? new Serializer(FhirSerializer.SerializeToXml)
                : FhirSerializer.SerializeToJson;
        }

        private void ConfigureAppointmentCreateBody(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.RequestBody = _serializer(_fhirResourceRepository.Appointment);
        }

        private static void ConfigureGpcGetCareRecordBody(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.RequestBody = _serializer(httpContext.HttpRequestConfiguration.BodyParameters);
        }

        private static void ConfigureRegisterPatientBody(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.RequestBody = _serializer(httpContext.HttpRequestConfiguration.BodyParameters);
        }

        private static void ConfigureGpcGetSchedule(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.RequestBody = _serializer(httpContext.HttpRequestConfiguration.BodyParameters);
        }

        private void ConfigureAppointmentAmendBody(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.RequestBody = _serializer(_fhirResourceRepository.Appointment);
        }

        private void ConfigureAppointmentCancelBody(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.RequestBody = _serializer(_fhirResourceRepository.Appointment);
        }

        public void ConfigureInvalidResourceType(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.RequestBody = FhirHelper.ChangeResourceTypeString(httpContext.HttpRequestConfiguration.RequestBody, FhirConst.Resources.kInvalidResourceType);
        }

        public void ConfigureInvalidParameterResourceType(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.RequestBody = FhirHelper.ChangeParameterResourceTypeString(httpContext.HttpRequestConfiguration.RequestBody, FhirConst.Resources.kInvalidResourceType);
        }

        public void ConfigureParameterResourceWithAdditionalField(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.RequestBody = FhirHelper.AddFieldToParameterResource(httpContext.HttpRequestConfiguration.RequestBody, FhirConst.Resources.kInvalidResourceType);
        }

        public void ConfigureAdditionalInvalidFieldInResource(HttpContext httpContext)
        {
            httpContext.HttpRequestConfiguration.RequestBody = FhirHelper.AddInvalidFieldToResourceJson(httpContext.HttpRequestConfiguration.RequestBody);
        }
    }
}
