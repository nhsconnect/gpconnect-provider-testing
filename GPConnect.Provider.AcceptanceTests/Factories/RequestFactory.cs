namespace GPConnect.Provider.AcceptanceTests.Factories
{
    using System;
    using Context;
    using Enum;
    using Hl7.Fhir.Serialization;

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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void ConfigureGpcGetCareRecordBody(HttpContext httpContext)
        {
            httpContext.RequestBody = FhirSerializer.SerializeToJson(httpContext.BodyParameters);
        }
    }
}
