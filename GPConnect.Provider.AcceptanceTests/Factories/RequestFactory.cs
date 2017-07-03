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
            }
        }

        private static void ConfigureGpcGetCareRecordBody(HttpContext httpContext)
        {
            httpContext.RequestBody = FhirSerializer.SerializeToJson(httpContext.BodyParameters);
        }

        private static void ConfigureRegisterPatientBody(HttpContext httpContext)
        {
            httpContext.BodyParameters.Add("registerPatient", httpContext.SavedPatient);
            httpContext.RequestBody = FhirSerializer.SerializeToJson(httpContext.BodyParameters);
        }

        public void ConfigureInvalidResourceType(HttpContext httpContext)
        {
            httpContext.RequestBody = FhirHelper.ChangeResourceTypeString(httpContext.RequestBody, FhirConst.Resources.kInvalidResourceType);
        }
    }
}
