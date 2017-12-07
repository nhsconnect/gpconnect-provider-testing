namespace GPConnect.Provider.AcceptanceTests.Helpers
{
    using Hl7.Fhir.Model;

    public static class RecordSectionHelper
    {
        private const string DefaultSystem = "http://fhir.nhs.uk/ValueSet/gpconnect-record-section-1";

        private const string InvalidSystem = "Invalid-System";

        private const string InvalidCode = "ZZZ";

        public static CodeableConcept GetRecordSection(string recordSectionCode)
        {
            return new CodeableConcept(DefaultSystem, recordSectionCode);
        }

        public static CodeableConcept GetRecordSectionWithInvalidSystem(string recordSectionCode)
        {
            return new CodeableConcept(InvalidSystem, recordSectionCode);
        }

        public static CodeableConcept GetRecordSectionWithInvalidCode()
        {
            return new CodeableConcept(DefaultSystem, InvalidCode);
        }

        public static CodeableConcept GetRecordSectionWithEmptySystem(string recordSectionCode)
        {
            return new CodeableConcept(string.Empty, recordSectionCode);
        }

        public static CodeableConcept GetRecordSystemWithEmptyCode()
        {
            return new CodeableConcept(DefaultSystem, string.Empty);
        }
    }
}
