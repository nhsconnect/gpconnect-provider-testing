﻿namespace GPConnect.Provider.AcceptanceTests.Constants
{
    internal static class FhirConst
    {
        internal static class GetSnoMedParams
        {
            public const string kConsultations = "1149501000000101";
            public const string kConsultation = "325851000000107";
            public const string kTopics = "25851000000105";
            public const string kHeadings = "24781000000107";
            public const string kProblems = "717711000000103";
            public const string kImmunizations = "1102181000000102";
            public const string kUncategorised = "826501000000100";
            public const string kMeds = "933361000000108";
            public const string kActiveAllergies = "886921000000105";
            public const string kResolvedAllergies = "1103671000000101";
            public const string kInvestigations = "887191000000108";
            public const string kOutboundReferrals = "792931000000107";
            public const string kObservationCommentNoteCode = "37331000000100";
            public const string kDiary = "714311000000108";
        }

        internal static class GetCareRecordParams
        {
            public const string kPatientNHSNumber = "patientNHSNumber";
            public const string kRecordSection = "recordSection";
            public const string kTimePeriod = "timePeriod";
            public const string kStartDate = "startDate";
            public const string kEndDate = "endDate";
        }

        internal static class GetStructuredRecordParams
        {
            public const string kPatientNHSNumber = "patientNHSNumber";
            public const string kAllergies = "includeAllergies";
            public const string kMedication = "includeMedication";

            //sara testing notes 29/08/19
            public const string kImmunizations = "includeImmunisations";

            public const string kUncategorised = "includeUncategorisedData";
            public const string kUncategorisedData = "uncategorisedDataSearchPeriod";
            public const string kConsultations = "includeConsultations";
            public const string kConsultationSearch = "consultationSearchPeriod";
            public const string kConsultationsMostRecent = "includeNumberOfMostRecent";
            public const string kProblems = "includeProblems";
            public const string kProblemsStatus = "filterStatus";
            public const string kProblemsSignificance = "filterSignificance";
            public const string kInvestigations = "includeInvestigations";
            public const string kInvestigationsSearch = "investigationSearchPeriod";
            public const string kReferrals = "includeReferrals";
            public const string kReferralSearch = "referralSearchPeriod";
            public const string kDiary = "includeDiaryEntries";
            public const string kDiarySearch = "diaryEntriesSearchDate";
            public const string kImmsNotGiven = "includeNotGiven";
            public const string kImmsDissent = "includeDissentConsent";

            public const string kStartDate = "start";
            public const string kEndDate = "end";
            public const string kResolvedAllergies = "includeResolvedAllergies";
            public const string kPrescriptionIssues = "includePrescriptionIssues";

            // github ref 127 RMB 5/11/2018            public const string kMedicationDatePeriod = "medicationDatePeriod";
            public const string kMedicationDatePeriod = "medicationSearchFromDate";

            public const string kTimePeriod = "timePeriod";
        }

        internal static class Resources
        {
            public const string kInvalidResourceType = "InvalidResourceType";
        }

        internal static class IdentifierSystems
        {
            public const string kNHSNumber = "https://fhir.nhs.uk/Id/nhs-number";
            public const string kOdsOrgzCode = "https://fhir.nhs.uk/Id/ods-organization-code";
            public const string kOdsOrgzCodeBackwardCom = "https://fhir.nhs.uk/Id/ods-organization-code";
            public const string kOdsSiteCode = "https://fhir.nhs.uk/Id/ods-site-code";
            public const string kLocalOrgzCode = "https://fhir.nhs.uk/Id/local-organization-code";
            public const string kLocalLocationCode = "https://fhir.nhs.uk/Id/local-location-identifier";
            public const string kLocalPatientCode = "https://fhir.nhs.uk/Id/local-patient-identifier";
            public const string kPracSDSUserId = "https://fhir.nhs.uk/Id/sds-user-id";
            public const string kPracRoleProfile = "https://fhir.nhs.uk/Id/sds-role-profile-id";
            public const string kAppointment = "https://fhir.nhs.uk/Id/gpconnect-appointment-identifier";
            public const string kGuid = "https://consumersupplier.com/Id/user-guid";
            public const string kPracGMP = "https://fhir.hl7.org.uk/Id/gmp-number";
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
            public const string kCcPresriptionType = "https://fhir.nhs.uk/STU3/CodeSystem/CareConnect-PrescriptionType-1";
            public const string kCcPartipationType = "https://fhir.nhs.uk/STU3/CodeSystem/GPConnect-ParticipantType-1";

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
            public const string kLocationType = "http://hl7.org/fhir/v3/RoleCode";
            public const string kLocationPhysicalType = "http://hl7.org/fhir/stu3/location-physical-type.html";
            public const string kCCSnomed = "http://snomed.info/sct";
            public const string kListOrder = "http://hl7.org/fhir/list-order";
        }

        internal static class ValueSetSystems
        {
            public const string kVsGpcRegistrationType = "https://fhir.nhs.uk/STU3/ValueSet/CareConnect-RegistrationType-1";
            public const string kVsGpcRegistrationStatus = "https://fhir.nhs.uk/STU3/ValueSet/CareConnect-RegistrationStatus-1";
            public const string kVsEthnicCategory = "https://fhir.nhs.uk/STU3/ValueSet/CareConnect-EthnicCategory-1";
            public const string kVsResidentialStatus = "https://fhir.nhs.uk/STU3/ValueSet/CareConnect-ResidentialStatus-1";
            public const string kVsTreatmentCategory = "https://fhir.nhs.uk/STU3/ValueSet/CareConnect-TreatmentCategory-1";
            public const string kVsHumanLanguage = "https://fhir.nhs.uk/STU3/ValueSet/CareConnect-HumanLanguage-1";
            public const string kVsLanguageAbilityMode = "https://fhir.nhs.uk/STU3/ValueSet/CareConnect-LanguageAbilityMode-1";
            public const string kVsLanguageAbilityProficiency = "https://fhir.nhs.uk/STU3/ValueSet/CareConnect-LanguageAbilityProficiency-1";
            public const string kVsNhsNumVerification = "https://fhir.nhs.uk/STU3/ValueSet/CareConnect-NHSNumberVerificationStatus-1";
            public const string kVsAdministrativeGender = "https://fhir.nhs.uk/STU3/ValueSet/CareConnect-AdministrativeGender-1";
            public const string kVsMaritalStatus = "https://fhir.nhs.uk/STU3/ValueSet/CareConnect-MaritalStatus-1";
            public const string kVsWarningCode = "https://fhir.nhs.uk/STU3/ValueSet/CareConnect-ListWarningCode-1";
            public const string kVsCareSetting = "https://fhir.nhs.uk/STU3/ValueSet/CareConnect-CareSettingType-1";

            //HL7
            public const string kVsNameUse = "http://hl7.org/fhir/stu3/valueset-name-use.html";

            public const string kVsContactPointSystem = "http://hl7.org/fhir/stu3/valueset-contact-point-system.html";
            public const string kVsNContactPointUse = "http://hl7.org/fhir/stu3/valueset-contact-point-use.html";
            public const string kVsAddressUse = "http://hl7.org/fhir/stu3/valueset-address-use.html";
            public const string kVsAddressType = "http://hl7.org/fhir/stu3/valueset-address-type.html";
            public const string kVsContactEntityType = "http://hl7.org/fhir/ValueSet/contactentity-type";
            public const string kVsEncounterParticipantType = "http://hl7.org/fhir/ValueSet/encounter-participant-type";
            public const string kVsRelationshipStatus = "http://hl7.org/fhir/ValueSet/v2-0131";
            public const string kVsNullFlavour = "http://hl7.org/fhir/ValueSet/v3-NullFlavor";

            // Added for 1.2.0 RMB 7/8/2018
            public const string kVsAllergyIntoleranceIdentifierSystem = "https://fhir.nhs.uk/Id/cross-care-setting-identifier";

            public const string kCrossCareIdentifier = "https://fhir.nhs.uk/Id/cross-care-setting-identifier";
            public const string kVsAllergyIntoleranceClinicalStatus = "http://hl7.org/fhir/stu3/valueset-allergy-clinical-status.html";
            public const string kVsAllergyIntoleranceCategory = "http://hl7.org/fhir/stu3/valueset-allergy-intolerance-category.html";
            public const string kVsAllergyIntoleranceCode = "https://fhir.nhs.uk/STU3/ValueSet/CareConnect-AllergyCode-1";
            public const string kVsAllergyIntoleranceSeverity = "http://hl7.org/fhir/stu3/valueset-reaction-event-severity.html";
            public const string kVsAllergyIntoleranceExposure = "https://fhir.nhs.uk/STU3/ValueSet/CareConnect-AllergyExposureRoute-1";
            public const string kVsMedicationStatementStatus = "http://hl7.org/fhir/stu3/valueset-medication-statement-status.html";
            public const string kVsMedicationRequestStatus = "http://hl7.org/fhir/stu3/valueset-medication-statement-status.html";
            public const string kVsMedicationStatementTaken = "http://hl7.org/fhir/stu3/valueset-medication-statement-taken.html";
            public const string kVsMedicationRequestIntent = "http://hl7.org/fhir/stu3/valueset-medication-request-intent.html";
            public const string kVsMedicationPrescriptionType = "https://fhir.nhs.uk/STU3/ValueSet/CareConnect-PrescriptionType-1";
            public const string kVsListStatus = "http://hl7.org/fhir/stu3/valueset-list-status.html";
            public const string kVsListMode = "http://hl7.org/fhir/stu3/valueset-list-mode.html";
            public const string kVsLanguage = "http://hl7.org/fhir/ValueSet/languages";
        }

        internal static class StructureDefinitionSystems
        {
            //EXTENSIONS
            public const string kExtGpcPractitioner = "https://fhir.nhs.uk/STU3/StructureDefinition/extension-gpconnect-practitioner-1";

            public const string kExtRegistrationStatus = "https://fhir.nhs.uk/StructureDefinition/extension-registration-status-1";
            public const string kExtRegistrationType = "https://fhir.nhs.uk/StructureDefinition/extension-registration-type-1";
            public const string kExtRegistrationPeriod = "https://fhir.nhs.uk/StructureDefinition/extension-registration-period-1";
            public const string kExtListWarningCode = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-ListWarningCode-1";
            public const string kExtListClinicalSetting = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-ClinicalSetting-1";
            public const string kExtEncounter = "http://hl7.org/fhir/StructureDefinition/encounter-associatedEncounter";
            public const string kExtPrescriptionAgency = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-PrescribingAgency-1";
            public const string kExtProblem = "https://fhir.hl7.org.uk/STU3/StructureDefinition/Extension-CareConnect-RelatedProblemHeader-1";
            public const string kExtProblemSignificance = "https://fhir.hl7.org.uk/STU3/StructureDefinition/Extension-CareConnect-ProblemSignificance-1";
            public const string kExtProblemRelatedContent = "https://fhir.hl7.org.uk/STU3/StructureDefinition/Extension-CareConnect-RelatedClinicalContent-1";
            public const string kCCExtRegistrationStatus = "registrationStatus";
            public const string kCCExtRegistrationType = "registrationType";
            public const string kCCExtRegistrationPeriod = "registrationPeriod";
            public const string kCCExtPreferredBranchSurgery = "preferredBranchSurgery";
            public const string kCCExtCommLanguage = "language";
            public const string kCCExtCommPreferred = "preferred";
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

            // Amended to https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-StructuredRecord-Bundle-1  RMB 14/8/2018
            public const string kGpcStructuredRecordBundle = "https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-StructuredRecord-Bundle-1";

            public const string kAllergyIntolerance = "https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-AllergyIntolerance-1";
            public const string kMedication = "https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Medication-1";
            public const string kMedicationStatement = "https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-MedicationStatement-1";
            public const string kMedicationRequest = "https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-MedicationRequest-1";
            public const string kList = "https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-List-1";
            public const string kSpecial = "http://hl7.org/fhir/special-values";
            public const string kImmunization = "https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Immunization-1";
            public const string kObservation = "https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Observation-1";
            public const string kDateRecorded = "https://fhir.hl7.org.uk/STU3/StructureDefinition/Extension-CareConnect-DateRecorded-1";
            public const string kVaccinationProcedure = "https://fhir.hl7.org.uk/STU3/StructureDefinition/Extension-CareConnect-VaccinationProcedure-1";
            public const string kEncounter = "https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Encounter-1";
            public const string kProblems = "https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-ProblemHeader-Condition-1";
            public const string kDiagnosticReport = "https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-DiagnosticReport-1";
            public const string kProcedureRequest = "https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-ProcedureRequest-1";
            public const string kSpecimen = "https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Specimen-1";
            public const string kReferrals = "https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-ReferralRequest-1";
            public const string kDocumentReference = "https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-DocumentReference-1";

            // git hub ref 158
            // RMB 9/1/19
            public const string kListEmptyReason = "https://fhir.nhs.uk/STU3/CodeSystem/CareConnect-ListEmptyReasonCode-1";

            //Extensions
            public const string kCCExtNhsCommunication = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-NHSCommunication-1";

            public const string kExtCcGpcRegDetails = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-RegistrationDetails-1";
            public const string kCCExtEthnicCategory = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-EthnicCategory-1";
            public const string kCcExtReligiousAffiliation = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-ReligiousAffiliation-1";
            public const string kCCExtPatientCadaver = "http://hl7.org/fhir/StructureDefinition/patient-cadavericDonor";
            public const string kCCExtResidentialStatus = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-ResidentialStatus-1";
            public const string kCCExtTreatmentCategory = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-TreatmentCategory-1";
            public const string kExtCcGpcNhsNumVerification = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-NHSNumberVerificationStatus-1";
            public const string kExtCcGpcMainLoc = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-MainLocation-1";
            public const string kOrgzPeriod = "http://hl7.org/fhir/StructureDefinition/organization-period";

            public const string kAppointmentCancellationReason = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-GPConnect-AppointmentCancellationReason-1";
            public const string kAppointmentBookingOrganization = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-GPConnect-BookingOrganisation-1";

            public const string kPractitionerRoleExt = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-GPConnect-PractitionerRole-1";
            public const string kDeliveryChannelExt = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-GPConnect-DeliveryChannel-1";

            // added 1.2.1 RMB 1/10/2018
            public const string kDeliveryChannel2Ext = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-GPConnect-DeliveryChannel-2";

            public const string kAllergyEndExt = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-AllergyIntoleranceEnd-1";

            public const string kMedicationPrescriptionType = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-PrescriptionType-1";
            public const string kMedicationRepeatInformation = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-MedicationRepeatInformation-1";
            public const string kMedicationRequestEndReason = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-MedicationStatusReason-1";
            public const string kMedicationStatementLastIssueDate = "https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-MedicationStatementLastIssueDate-1";
        }

        internal static class Prefixs
        {
            public const string kGreaterThanOrEqualTo = "ge";
            public const string kLessThanOrEqualTo = "le";
        }

        internal static class ListTitles
        {
            public const string kActiveAllergies = "Allergies and adverse reactions";
            public const string kResolvedAllergies = "Ended allergies";
            public const string kEndedAllergies = "Ended allergies (record artifact)";
            public const string kMedications = "Medications and medical devices";
            public const string kConsultations = "List of consultations";
            public const string kConsultation = "Surgery Consultation";
            public const string kInvestigations = "Investigations and results";
            public const string kOutboundReferrals = "Outbound referral";
            public const string kDiary = "Patient recall administration";

            //Secondary Lists
            public const string kSecConsultMedications = "Consultations - medications contained in consultations";

            public const string kSecConsultProblems = "Consultations - problems contained in consultations";
            public const string kSecConsultUncat = "Consultations - uncategorised data contained in consultations";
            public const string kSecProblemsMedications = "Problems - medications related to problems";
            public const string kSecProblemsUncat = "Problems - uncategorised data related to problems";
        }

        internal static class SecondaryListCodeAndDisplayValues
        {
            public const string kSecConsultMedicationsCode = "consultations-medications-contained-in-consultations";
            public const string kSecConsultProblemsCode = "consultations-problems-contained-in-consultations";
            public const string kSecConsultUncatCode = "consultations-uncategorised-data-contained-in-consultations";
            public const string kSecProblemsMedicationsCode = "problems-medications-related-to-problems";
            public const string kSecProblemsUncatCode = "problems-uncategorised-data-related-to-problems";

            public const string kSecConsultMedicationsDisplay = "Consultations - medications contained in consultations";
            public const string kSecConsultProblemsDisplay = "Consultations - problems contained in consultations";
            public const string kSecConsultUncatDisplay = "Consultations - uncategorised data contained in consultations";
            public const string kSecProblemsMedicationsDisplay = "Problems - medications related to problems";
            public const string kSecProblemsUncatDisplay = "Problems - uncategorised data related to problems";
        }
    }
}