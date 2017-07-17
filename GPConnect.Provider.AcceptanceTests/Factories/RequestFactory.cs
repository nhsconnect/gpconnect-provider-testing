namespace GPConnect.Provider.AcceptanceTests.Factories
{
    using Constants;
    using Context;
    using Enum;
    using Helpers;
    using Hl7.Fhir.Serialization;

    //TODO: Consider XML requests and whether we need each interaction to have its own "configure" method.
    //There will be 2 types of serialization (JSON & XML) and 2 body types (Parameters & Resource)
    //Request body could be computed field as all info will be in HttpConext.
    public class RequestFactory
    {
        private readonly GpConnectInteraction _gpConnectInteraction;

        public RequestFactory(GpConnectInteraction gpConnectInteraction)
        {
            _gpConnectInteraction = gpConnectInteraction;
        }

        public void ConfigureBody(HttpContext httpContext)
        {
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

 
        private static void ConfigureAppointmentCreateBody(HttpContext httpContext)
        {
            httpContext.RequestBody = FhirSerializer.SerializeToJson(httpContext.CreatedAppointment);
        }

        private static void ConfigureGpcGetCareRecordBody(HttpContext httpContext)
        {
            httpContext.RequestBody = FhirSerializer.SerializeToJson(httpContext.BodyParameters);
        }

        private static void ConfigureRegisterPatientBody(HttpContext httpContext)
        {
            httpContext.RequestBody = httpContext.RequestContentType.Contains("xml") ? FhirSerializer.SerializeToXml(httpContext.BodyParameters) : FhirSerializer.SerializeToJson(httpContext.BodyParameters);
        }

        private static void ConfigureGpcGetSchedule(HttpContext httpContext)
        {
            httpContext.RequestBody = FhirSerializer.SerializeToJson(httpContext.BodyParameters);
        }

        private static void ConfigureAppointmentAmendBody(HttpContext httpContext)
        {
            httpContext.RequestBody = FhirSerializer.SerializeToJson(httpContext.CreatedAppointment);
        }

        private static void ConfigureAppointmentCancelBody(HttpContext httpContext)
        {
            httpContext.RequestBody = FhirSerializer.SerializeToJson(httpContext.CreatedAppointment);
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
    }
}
