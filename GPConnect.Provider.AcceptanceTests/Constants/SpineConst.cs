namespace GPConnect.Provider.AcceptanceTests.Constants
{
    internal static class SpineConst
    {
        internal static class InteractionIds
        {
            private const string BaseInteraction = "urn:nhs:names:services:gpconnect:fhir:";

            public const string GpcGetCareRecord = BaseInteraction + "operation:gpc.getcarerecord";

            public const string OrganizationSearch = BaseInteraction + "rest:search:organization";
            public const string OrganizationRead = BaseInteraction + "rest:read:organization";

            public const string PractitionerSearch = BaseInteraction + "rest:search:practitioner";
            public const string PractitionerRead = BaseInteraction + "rest:read:practitioner";

            public const string PatientSearch = BaseInteraction + "rest:search:patient";
            public const string PatientRead = BaseInteraction + "rest:read:patient";

            public const string LocationSearch = BaseInteraction + "rest:search:location";
            public const string LocationRead = BaseInteraction + "rest:read:location";

            public static string RegisterPatient = BaseInteraction + "operation:gpc.registerpatient";

            public static string GpcGetSchedule = BaseInteraction + "operation:gpc.getschedule";
            public static string AppointmentCreate => BaseInteraction + "rest:create:appointment";


            public const string kFhirRestReadMetadata = "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata";
            public const string kFhirPractitioner = "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner";

        }
    }
}
