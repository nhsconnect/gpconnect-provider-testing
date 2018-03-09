namespace GPConnect.Provider.AcceptanceTests.Factories
{
    using Constants;
    using Enum;
    using Helpers;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;
    using Hl7.Fhir.Serialization;
    using Http;
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

        public void ConfigureBody(HttpRequestConfiguration httpRequestConfiguration)
        {
            ConfigureSerializer(httpRequestConfiguration);

            switch (_gpConnectInteraction)
            {
                case GpConnectInteraction.GpcGetCareRecord:
                    ConfigureGpcGetCareRecordBody(httpRequestConfiguration);
                    break;
                case GpConnectInteraction.GpcGetStructuredRecord:
                    ConfigureGpcGetStructuredRecordBody(httpRequestConfiguration);
                    break;
                case GpConnectInteraction.RegisterPatient:
                    ConfigureRegisterPatientBody(httpRequestConfiguration);
                    break;
                case GpConnectInteraction.AppointmentCreate:
                    ConfigureAppointmentCreateBody(httpRequestConfiguration);
                    break;
                case GpConnectInteraction.AppointmentAmend:
                    ConfigureAppointmentAmendBody(httpRequestConfiguration);
                    break;
                case GpConnectInteraction.AppointmentCancel:
                    ConfigureAppointmentCancelBody(httpRequestConfiguration);
                    break;
            }
        }

        private static void ConfigureSerializer(HttpRequestConfiguration httpRequestConfiguration)
        {
            _serializer = httpRequestConfiguration.RequestContentType.Contains("xml")
                ? new Serializer(FhirSerializer.SerializeToXml)
                : FhirSerializer.SerializeToJson;
        }

        private void ConfigureAppointmentCreateBody(HttpRequestConfiguration httpRequestConfiguration)
        {
            if (_fhirResourceRepository.Appointment != null)
            {
                httpRequestConfiguration.RequestBody = _serializer(_fhirResourceRepository.Appointment);
            }
        }

        private static void ConfigureGpcGetCareRecordBody(HttpRequestConfiguration httpRequestConfiguration)
        {
            httpRequestConfiguration.RequestBody = _serializer(httpRequestConfiguration.BodyParameters);
        }

        private static void ConfigureGpcGetStructuredRecordBody(HttpRequestConfiguration httpRequestConfiguration)
        {
            httpRequestConfiguration.RequestBody = _serializer(httpRequestConfiguration.BodyParameters);
        }

        private static void ConfigureRegisterPatientBody(HttpRequestConfiguration httpRequestConfiguration)
        {
            httpRequestConfiguration.RequestBody = _serializer(httpRequestConfiguration.BodyParameters);
        }


        private void ConfigureAppointmentAmendBody(HttpRequestConfiguration httpRequestConfiguration)
        {
            if (_fhirResourceRepository.Appointment != null)
            {
                httpRequestConfiguration.RequestBody = _serializer(_fhirResourceRepository.Appointment);
            }
        }

        private void ConfigureAppointmentCancelBody(HttpRequestConfiguration httpRequestConfiguration)
        {
            if (_fhirResourceRepository.Appointment != null)
            {
                httpRequestConfiguration.RequestBody = _serializer(_fhirResourceRepository.Appointment);
            }
        }

        public void ConfigureInvalidResourceType(HttpRequestConfiguration httpRequestConfiguration)
        {
            httpRequestConfiguration.RequestBody = FhirHelper.ChangeResourceTypeString(httpRequestConfiguration.RequestBody, FhirConst.Resources.kInvalidResourceType);
        }

        public void ConfigureInvalidParameterResourceType(HttpRequestConfiguration httpRequestConfiguration)
        {
            httpRequestConfiguration.RequestBody = FhirHelper.ChangeParameterResourceTypeString(httpRequestConfiguration.RequestBody, FhirConst.Resources.kInvalidResourceType);
        }

        public void ConfigureParameterResourceWithAdditionalField(HttpRequestConfiguration httpRequestConfiguration)
        {
            httpRequestConfiguration.RequestBody = FhirHelper.AddFieldToParameterResource(httpRequestConfiguration.RequestBody, FhirConst.Resources.kInvalidResourceType);
        }

        public void ConfigureAdditionalInvalidFieldInResource(HttpRequestConfiguration httpRequestConfiguration)
        {
            httpRequestConfiguration.RequestBody = FhirHelper.AddInvalidFieldToResourceJson(httpRequestConfiguration.RequestBody);
        }
    }
}
