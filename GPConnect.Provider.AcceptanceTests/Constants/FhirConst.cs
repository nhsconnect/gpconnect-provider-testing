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
            public const string kStartDate = "startDate";
            public const string kEndDate = "endDate";
        }

        internal static class Resources
        {
            public const string kInvalidResourceType = "InvalidResourceType";
        }

        internal static class IdentifierSystems
        {
            public const string kNHSNumber = "https://fhir.nhs.uk/Id/nhs-number";
            public const string kOdsOrgzCode = "https://fhir.nhs.uk/Id/ods-organization-code";
            public const string kOdsSiteCode = "https://fhir.nhs.uk/Id/ods-site-code";
            public const string kLocalOrgzCode = "https://fhir.nhs.uk/Id/local-organization-code";
            public const string kLocalLocationCode = "https://fhir.nhs.uk/Id/local-location-identifier";
            public const string kLocalPatientCode = "https://fhir.nhs.uk/Id/local-patient-identifier";
            public const string kPracSDSUserId = "https://fhir.nhs.uk/Id/sds-user-id";
            public const string kPracRoleProfile = "https://fhir.nhs.uk/Id/sds-role-profile-id";
        }

        internal static class ValueSetSystems
        {
            public const string kCcGpcRegistrationType = "https://fhir.nhs.uk/ValueSet/CareConnect-RegistrationType-1";
            public const string kCcGpcRegistrationStatus = "https://fhir.nhs.uk/ValueSet/CareConnect-RegistrationStatus-1";
            public const string kCcEthnicCategory = "https://fhir.nhs.uk/ValueSet/CareConnect-EthnicCategory-1";
            public const string kCcResidentialStatus = "https://fhir.nhs.uk/ValueSet/CareConnect-ResidentialStatus-1";
            public const string kCcTreatmentCategory = "https://fhir.nhs.uk/ValueSet/CareConnect-TreatmentCategory-1";
            public const string kCcHumanLanguage = "https://fhir.nhs.uk/ValueSet/CareConnect-HumanLanguage-1";
            public const string kCcLanguageAbilityMode = "https://fhir.nhs.uk/ValueSet/CareConnect-LanguageAbilityMode-1";
            public const string kCcLanguageAbilityProficiency = "https://fhir.nhs.uk/ValueSet/CareConnect-LanguageAbilityProficiency-1";
            public const string kCcReligiousAffiliation = "https://fhir.nhs.uk/ValueSet/CareConnect-ReligiousAffiliation-1";
            public const string kCcNhsNumVerification = "https://fhir.nhs.uk/STU3/ValueSet/CareConnect-NHSNumberVerificationStatus-1";
            public const string kCcNhsNumVerificationSys = "https://fhir.nhs.uk/CareConnect-NHSNumberVerificationStatus-1";

            //STU3
            public const string kNameUse = "http://hl7.org/fhir/stu3/valueset-name-use.html";
            public const string kContactPointSystem = "http://hl7.org/fhir/stu3/valueset-contact-point-system.html";
            public const string kNContactPointUse = "http://hl7.org/fhir/stu3/valueset-contact-point-use.html";
            public const string kAddressUse = "http://hl7.org/fhir/stu3/valueset-address-use.html";
            public const string kAddressType = "http://hl7.org/fhir/stu3/valueset-address-type.html";
            public const string kAdministrativeGender = "https://fhir.nhs.uk/STU3/ValueSet/CareConnect-AdministrativeGender-1";
            public const string kMaritalStatus = "https://fhir.nhs.uk/STU3/ValueSet/CareConnect-MaritalStatus-1";
            public const string kContactEntityType = "http://hl7.org/fhir/ValueSet/contactentity-type";
            public const string kServDelLocationRoleType = "http://hl7.org/fhir/ValueSet/v3-ServiceDeliveryLocationRoleType";
            public const string kEncounterParticipantType = "http://hl7.org/fhir/ValueSet/encounter-participant-type";
            public const string kSpineErrorOrWarningCode = "https://fhir.nhs.uk/STU3/ValueSet/Spine-ErrorOrWarningCode-1";
            public const string kRelationshipStatus = "http://hl7.org/fhir/ValueSet/v2-0131";
            public const string kLanguage = "http://hl7.org/fhir/ValueSet/languages";
        }

        internal static class StructureDefinitionSystems
        {
            public const string kGpcSearchSet = "http://fhir.nhs.net/StructureDefinition/gpconnect-searchset-bundle-1";


            //EXTENSIONS
            public const string kExtGpcPractitioner = "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-practitioner-1";
            public const string kExtRegistrationStatus = "https://fhir.nhs.uk/StructureDefinition/extension-registration-status-1";
            public const string kExtRegistrationType = "https://fhir.nhs.uk/StructureDefinition/extension-registration-type-1";
            public const string kExtRegistrationPeriod = "https://fhir.nhs.uk/StructureDefinition/extension-registration-period-1";
            public const string kCCExtRegistrationStatus = "registrationStatus";
            public const string kCCExtRegistrationType = "registrationType";
            public const string kCCExtRegistrationPeriod = "registrationPeriod";
            public const string kCCExtPreferredBranchSurgery = "preferredBranchSurgery";
            public const string kCCExtCommLanguage = "Language";
            public const string kCCExtCommPreferred = "Preferred";
            public const string kCCExtCommModeOfCommunication = "modeOfCommunication";
            public const string kCCExtCommCommProficiency = "communicationProficiency";
            public const string kCCExtCommInterpreterRequired = "interpreterRequired";

            //STU3 Uplift

            //Structures
            public const string kPractitioner = "https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Practitioner-1";
            public const string kPatient = "https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Patient-1";
            public const string kOrganisation = "https://fhir.nhs.net/STU3/StructureDefinition/CareConnect-GPC-Organization-1";
            public const string kLocation = "https://fhir.nhs.net/STU3/StructureDefinition/CareConnect-GPC-Location-1";
            public const string kOperationOutcome = "https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-OperationOutcome-1";

            //Extensions
            public const string kCCExtNhsCommunication =        "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-NHSCommunication-1";
            public const string kExtCcGpcRegDetails =           "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-RegistrationDetails-1";
            public const string kCCExtEthnicCategory =          "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-EthnicCategory-1";
            public const string kCcExtReligiousAffiliation =    "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-ReligiousAffiliation-1";
            public const string kCCExtPatientCadaver =          "http://hl7.org/fhir/StructureDefinition/patient-cadavericDonor";
            public const string kCCExtResidentialStatus =       "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-ResidentialStatus-1";
            public const string kCCExtTreatmentCategory =       "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-TreatmentCategory-1";
            public const string kExtCcGpcNhsNumVerification =   "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-NHSNumberVerificationStatus-1";
            public const string kExtCcGpcMainLoc =              "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-MainLocation-1";
            public const string kOrgzPeriod =                   "http://hl7.org/fhir/StructureDefinition/organization-period";

            public const string kAppointmentCancellationReason = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-GPConnect-AppointmentCancellationReason-1";
            public const string kAppointmentBookingOrganization = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-GPConnect-BookingOrganisation-1";
        }

        internal static class Prefixs
        {
            public const string kGreaterThanOrEqualTo = "ge";
            public const string kLessThanOrEqualTo = "le";
        }
    }
}
