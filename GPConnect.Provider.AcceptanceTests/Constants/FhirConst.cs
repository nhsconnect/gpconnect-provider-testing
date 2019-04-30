// ReSharper disable ClassNeverInstantiated.Global
namespace GPConnect.Provider.AcceptanceTests.Constants
{
    internal static class FhirConst
    {
        internal static class ContentTypes
        {
            public const string kJsonFhir = "application/json+fhir";
            public const string kXmlFhir = "application/xml+fhir";
        }

        internal static class GetCareRecordParams
        {
            public const string kPatientNHSNumber = "patientNHSNumber";
            public const string kRecordSection = "recordSection";
            public const string kTimePeriod = "timePeriod";
        }

        internal static class Resources
        {
            public const string kInvalidResourceType = "InvalidResourceType";
        }

        internal static class IdentifierSystems
        {
            public const string kNHSNumber = "http://fhir.nhs.net/Id/nhs-number";
            public const string kGuid = "https://consumersupplier.com/Id/user-guid"; //added for #235
            public const string kPracSDSUserId = "http://fhir.nhs.net/sds-user-id";  //added for #235
            public const string kPracRoleProfile = "https://fhir.nhs.net/Id/sds-role-profile-id";  //added for #235
        }
    }
}
