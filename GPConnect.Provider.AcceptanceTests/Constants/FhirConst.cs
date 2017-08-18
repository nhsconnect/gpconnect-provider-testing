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
            public const string kOdsOrgzCode = "https://fhir.nhs.uk/Id/ods-organization-code";
            public const string kOdsSiteCode = "https://fhir.nhs.uk/Id/ods-site-code";
        }

        internal static class ValueSetSystems
        {
            public const string kOrgzType = "http://hl7.org/fhir/organization-type";
            public const string kNameUse = "http://hl7.org/fhir/ValueSet/name-use";
            public const string kContactPointSystem = "http://hl7.org/fhir/ValueSet/contact-point-system";
            public const string kNContactPointUse = "http://hl7.org/fhir/ValueSet/contact-point-use";
            public const string kAddressUse = "http://hl7.org/fhir/ValueSet/address-use";
            public const string kAddressType = "http://hl7.org/fhir/ValueSet/address-type";
        }

        internal static class StructureDefinitionSystems
        {
            public const string kOrganisation = "http://fhir.nhs.net/StructureDefinition/CareConnect-GPC-Organization-1";
            public const string kExtCcGpcMainLoc = "https://fhir.hl7.org.uk/StructureDefinition/Extension-CareConnect-GPC-MainLocation-1";
            public const string kOrgzPeriod = "http://hl7.org/fhir/StructureDefinition/organization-period";  
        }
    }
}
