using System.ComponentModel;
using System.Text.RegularExpressions;
using GPConnect.Provider.AcceptanceTests.Enum;

namespace GPConnect.Provider.AcceptanceTests.Helpers
{
    public static class ResourceReferenceHelper
    {
        private static string _relAbsRegex = @"^((http|https)://([A-Za-z0-9\\\/\.\:\%\$])*)?(Account|AllergyIntolerance|Appointment|AppointmentResponse|AuditEvent|Basic|Binary|BodySite|Bundle|CarePlan|Claim|ClaimResponse|ClinicalImpression|Communication|CommunicationRequest|Composition|ConceptMap|Condition|Conformance|Contract|Coverage|DataElement|DetectedIssue|Device|DeviceComponent|DeviceMetric|DeviceUseRequest|DeviceUseStatement|DiagnosticOrder|DiagnosticReport|DocumentManifest|DocumentReference|EligibilityRequest|EligibilityResponse|Encounter|EnrollmentRequest|EnrollmentResponse|EpisodeOfCare|ExplanationOfBenefit|FamilyMemberHistory|Flag|Goal|Group|HealthcareService|ImagingObjectSelection|ImagingStudy|Immunization|ImmunizationRecommendation|ImplementationGuide|List|Location|Media|Medication|MedicationAdministration|MedicationDispense|MedicationOrder|MedicationStatement|MessageHeader|NamingSystem|NutritionOrder|Observation|OperationDefinition|OperationOutcome|Order|OrderResponse|Organization|Patient|PaymentNotice|PaymentReconciliation|Person|Practitioner|Procedure|ProcedureRequest|ProcessRequest|ProcessResponse|Provenance|Questionnaire|QuestionnaireResponse|ReferralRequest|RelatedPerson|RiskAssessment|Schedule|SearchParameter|Slot|Specimen|StructureDefinition|Subscription|Substance|SupplyDelivery|SupplyRequest|TestScript|ValueSet|VisionPrescription)\/[A-Za-z0-9\-\.]{1,64}(\/_history\/[A-Za-z0-9\-\.]{1,64})?$";

        public static bool IsRelOrAbsReference(string reference)
        {
            if (string.IsNullOrEmpty(reference))
            {
                return false;
            }

            return Regex.IsMatch(reference, _relAbsRegex);

        }

        public static GpConnectInteraction GetReadInteractionType(string reference)
        {
            if (!string.IsNullOrEmpty(reference))
            {
                var matches = Regex.Match(reference, _relAbsRegex);

                if (matches.Success && matches.Groups.Count == 6)
                {
                    switch (matches.Groups[4].Value)
                    {
                        case "Organization":
                            return GpConnectInteraction.OrganizationRead;
                        case "Practitioner":
                            return GpConnectInteraction.PractitionerRead;
                    }
                }

            }

            throw new InvalidEnumArgumentException("No matching Interaction Enum found in reference.");

        }
    }
}
