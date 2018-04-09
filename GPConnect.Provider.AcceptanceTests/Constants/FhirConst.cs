namespace GPConnect.Provider.AcceptanceTests.Constants
{
    internal static class FhirConst
    {
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
            public const string kOdsOrgzCodeBackwardCom = "http://fhir.nhs.net/Id/ods-organization-code";
            public const string kOdsSiteCode = "https://fhir.nhs.uk/Id/ods-site-code";
            public const string kLocalOrgzCode = "https://fhir.nhs.uk/Id/local-organization-code";
            public const string kLocalLocationCode = "https://fhir.nhs.uk/Id/local-location-identifier";
            public const string kLocalPatientCode = "https://fhir.nhs.uk/Id/local-patient-identifier";
            public const string kPracSDSUserId = "https://fhir.nhs.uk/Id/sds-user-id";
            public const string kPracRoleProfile = "https://fhir.nhs.uk/Id/sds-role-profile-id";
            public const string kAppointment = "https://fhir.nhs.uk/Id/gpconnect-appointment-identifier";
        }

        internal static class CodeSystems
        {
            public const string kCcGpcRegistrationType = "https://fhir.nhs.uk/STU3/CodeSystem/CareConnect-RegistrationType-1";
            public const string kCcGpcRegistrationStatus = "https://fhir.nhs.uk/STU3/CodeSystem/CareConnect-RegistrationStatus-1";
            public const string kCcEthnicCategory = "https://fhir.nhs.uk/STU3/CodeSystem/CareConnect-EthnicCategory-1";
            public const string kCcResidentialStatus = "https://fhir.nhs.uk/STU3/CodeSystem/CareConnect-ResidentialStatus-1";
            public const string kCcTreatmentCategory = "https://fhir.nhs.uk/STU3/CodeSystem/CareConnect-TreatmentCategory-1";
            public const string kCcHumanLanguage = "https://fhir.nhs.uk/STU3/CodeSystem/CareConnect-HumanLanguage-1";
            public const string kCcLanguageAbilityMode = "https://fhir.nhs.uk/STU3/CodeSystem/CareConnect-LanguageAbilityMode-1";
            public const string kCcLanguageAbilityProficiency = "https://fhir.nhs.uk/STU3/CodeSystem/CareConnect-LanguageAbilityProficiency-1";
            public const string kCcNhsNumVerification = "https://fhir.nhs.uk/STU3/CodeSystem/CareConnect-NHSNumberVerificationStatus-1";

            //STU3
            public const string kNameUse = "http://hl7.org/fhir/name-use";
            public const string kContactPointSystem = "http://hl7.org/fhir/stu3/contact-point-system";
            public const string kNContactPointUse = "http://hl7.org/fhir/stu3/contact-point-use";
            public const string kAddressUse = "http://hl7.org/fhir/stu3/address-use";
            public const string kAddressType = "http://hl7.org/fhir/stu3/address-type";
            public const string kAdministrativeGender = "http://hl7.org/fhir/administrative-gender";
            public const string kMaritalStatus = "http://hl7.org/fhir/marital-status";
            public const string kNullFlavour = "http://hl7.org/fhir/v3/NullFlavor";
            public const string kContactEntityType = "http://hl7.org/fhir/contactentity-type";
            public const string kEncounterParticipantType = "http://hl7.org/fhir/encounter-participant-type";
            public const string kRelationshipStatus = "http://hl7.org/fhir/v2/0131";
        }

        internal static class StructureDefinitionSystems
        {
            //EXTENSIONS
            public const string kExtGpcPractitioner = "https://fhir.nhs.uk/STU3/StructureDefinition/extension-gpconnect-practitioner-1";
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
            public const string kOrganisation = "https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Organization-1";
            public const string kLocation = "https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Location-1";
            public const string kOperationOutcome = "https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-OperationOutcome-1";
            public const string kAppointment = "https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-Appointment-1";
            public const string kSlot = "https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-Slot-1";
            public const string kSchedule = "https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-Schedule-1";
            public const string kGpcSearchSet = "https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-Searchset-Bundle-1";

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

            public const string kPractitionerRoleExt = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-GPConnect-PractitionerRole-1";
            public const string kDeliveryChannelExt ="https://fhir.nhs.uk/STU3/StructureDefinition/Extension-GPConnect-DeliveryChannel-1";
        }

        internal static class Prefixs
        {
            public const string kGreaterThanOrEqualTo = "ge";
            public const string kLessThanOrEqualTo = "le";
        }
    }
}
