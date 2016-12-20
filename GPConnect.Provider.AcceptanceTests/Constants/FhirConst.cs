// ReSharper disable ClassNeverInstantiated.Global
namespace GPConnect.Provider.AcceptanceTests.Constants
{
    internal static class FhirConst
    {
        internal static class ContentTypes
        {
            public const string JsonFhir = "application/json+fhir";
            public const string XmlFhir = "application/xml+fhir";
        }

        internal static class GetCareRecordParams
        {
            public const string PatientNHSNumber = "patientNHSNumber";
            public const string RecordSection = "recordSection";
            public const string TimePeriod = "timePeriod";
        }

        internal static class Resources
        {
            public const string InvalidResourceType = "InvalidResourceType";
        }
    }
}
