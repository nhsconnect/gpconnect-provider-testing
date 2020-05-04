namespace GPConnect.Provider.AcceptanceTests.Constants
{
    internal static class SpineConst
    {
        internal static class InteractionIds
        {
            private const string BaseInteraction = "urn:nhs:names:services:gpconnect:fhir:";
            private const string StructuredBaseInteraction = "urn:nhs:names:services:gpconnect:structured:fhir:";

            public const string GpcGetCareRecord = BaseInteraction + "operation:gpc.getcarerecord-1";
            public const string GpcGetStructuredRecord = BaseInteraction + "operation:gpc.getstructuredrecord-1";

            public const string OrganizationSearch = BaseInteraction + "rest:search:organization-1";
            public const string OrganizationRead = BaseInteraction + "rest:read:organization-1";

            public const string PractitionerSearch = BaseInteraction + "rest:search:practitioner-1";
            public const string PractitionerRead = BaseInteraction + "rest:read:practitioner-1";

            public const string PatientSearch = BaseInteraction + "rest:search:patient-1";
            public const string PatientRead = BaseInteraction + "rest:read:patient-1";

            public const string LocationSearch = BaseInteraction + "rest:search:location-1";
            public const string LocationRead = BaseInteraction + "rest:read:location-1";

            public static string RegisterPatient = BaseInteraction + "operation:gpc.registerpatient-1";

            public static string AppointmentCreate => BaseInteraction + "rest:create:appointment-1";
            public static string AppointmentSearch => BaseInteraction + "rest:search:patient_appointments-1";
            public static string AppointmentAmend => BaseInteraction + "rest:update:appointment-1";
            public static string AppointmentCancel => BaseInteraction + "rest:cancel:appointment-1";
            public static string AppointmentRead => BaseInteraction + "rest:read:appointment-1";

            public static string SlotSearch => BaseInteraction + "rest:search:slot-1";

            public static string MetadataRead => BaseInteraction + "rest:read:metadata-1";

            public const string kFhirPractitioner = "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner-1";
			
            public static string StructuredMetaDataRead => StructuredBaseInteraction + "rest:read:metadata-1";

        }
    }
}
