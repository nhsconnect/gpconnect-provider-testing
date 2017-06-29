// ReSharper disable ClassNeverInstantiated.Global
namespace GPConnect.Provider.AcceptanceTests.Constants
{
    internal static class SpineConst
    {
        internal static class InteractionIds
        {
            public const string GpcGetCareRecord = "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord";

            public const string OrganizationSearch = "urn:nhs:names:services:gpconnect:fhir:rest:search:organization";

            public const string kFhirRestReadMetadata = "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata";
            public const string kFhirPractitioner = "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner";
        }        
    }
}
