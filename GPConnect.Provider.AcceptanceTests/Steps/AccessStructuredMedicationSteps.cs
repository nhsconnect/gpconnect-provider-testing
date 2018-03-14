namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using Cache.ValueSet;
    using Constants;
    using Context;
    using TechTalk.SpecFlow;
    using Shouldly;
    using Hl7.Fhir.Model;
    using System.Collections.Generic;
    using System;
    using Extensions;
    using System.Linq;
    using GPConnect.Provider.AcceptanceTests.Enum;
    using static Hl7.Fhir.Model.Parameters;
    using GPConnect.Provider.AcceptanceTests.Helpers;

    [Binding]
    public sealed class AccessStructuredMedicationSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;

        private List<Medication> Medications => _httpContext.FhirResponse.Medications;
        private List<MedicationStatement> MedicationStatements => _httpContext.FhirResponse.MedicationStatements;
        private List<MedicationRequest> MedicationRequests => _httpContext.FhirResponse.MedicationRequests;
        private List<List> Lists => _httpContext.FhirResponse.Lists;

        public AccessStructuredMedicationSteps(HttpSteps httpSteps, HttpContext httpContext) 
            : base(httpSteps)
        {
            _httpContext = httpContext;
        }

        #region Medication Parameters

        [Given(@"I add the medication parameter with includePrescriptionIssues set to ""(.*)""")]
        public void GivenIAddTheMedicationsParameterWithIncludePrescriptionIssuesSetTo(string partValue)
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] { Tuple.Create(FhirConst.GetStructuredRecordParams.kPrescriptionIssues, (Base)new FhirBoolean(Boolean.Parse(partValue))) };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kMedication, tuples);
        }

        [Given(@"I add the medications parameter")]
        public void GivenIAddTheMedicationsParameter()
        {
            ParameterComponent param = new ParameterComponent();
            param.Name = FhirConst.GetStructuredRecordParams.kMedication;
            _httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(param);
        }

        [Given(@"I add an invalid medications parameter")]
        public void GivenIAddAnInvalidMedicationsParameter()
        {
            ParameterComponent param = new ParameterComponent();
            param.Name = "inlcudeInvalidMedications";
            _httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(param);
        }

        [Given(@"I add the medications parameter with a timePeriod")]
        public void GivenIAddTheMedicationsParameterWithATimePeriod()
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] { Tuple.Create(FhirConst.GetStructuredRecordParams.kTimePeriod, (Base)TimePeriodHelper.GetDefaultTimePeriod())};
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kMedication, tuples);
        }

        [Given(@"I add the medications parameter with a start date")]
        public void GivenIAddTheMedicationsParameterWithAStartPeriod()
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] { Tuple.Create(FhirConst.GetStructuredRecordParams.kTimePeriod, (Base)TimePeriodHelper.GetTimePeriodStartDateOnly()) };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kMedication, tuples);
        }

        [Given(@"I add the medications parameter with an end date")]
        public void GivenIAddTheMedicationsParameterWithAnEndPeriod()
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] { Tuple.Create(FhirConst.GetStructuredRecordParams.kTimePeriod, (Base)TimePeriodHelper.GetTimePeriodEndDateOnly()) };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kMedication, tuples);
        }

        #endregion

        #region Medication Checks

        [Then(@"the Medications should be valid")]
        public void TheMedicationsShouldBeValid()
        {
            TheMedicationIdShouldBeValid();
            TheMedicationCodeShouldBeValid();
            TheMedicationMetadataShouldBeValid();
        }

        [Then(@"the Medication Id should be valid")]
        public void TheMedicationIdShouldBeValid()
        {
            Medications.ForEach(medication =>
            {
                medication.Id.ShouldNotBeNullOrEmpty();
            });
        }

        [Then(@"the Medication Code should be valid")]
        public void TheMedicationCodeShouldBeValid()
        {
            Medications.ForEach(medication =>
            {
                if (medication.Code != null)
                {
                    medication.Code.Coding.ShouldNotBeNull("Medication Code coding cannot be null");
                    medication.Code.Coding.ForEach(coding =>
                    {
                        coding.System.ShouldNotBeNull("Code should not be null");
                        coding.System.ShouldBe("http://snomed.info/sct");
                    });
                }
            });
        }

        [Then(@"the Medication Metadata should be valid")]
        public void TheMedicationMetadataShouldBeValid()
        {
            Medications.ForEach(medication =>
            {
                CheckForValidMetaDataInResource(medication, FhirConst.StructureDefinitionSystems.kMedication);
            });
        }

        #endregion

        #region Medication Statement Checks

        [Then(@"the Medication Statements should be valid")]
        public void TheMedicationStatementsShouldBeValid()
        {
            TheMedicationStatementIdShouldBeValid();
            TheMedicationStatementMetadataShouldBeValid();
            TheMedicationStatementBasedOnShouldNotBeNullAndShouldReferToMedicationRequestWithIntentPlan();
            TheMedicationStatementStatusShouldbeValid();
            TheMedicationStatementMedicationReferenceShouldbeValid();
            TheMedicationStatementSubjectShouldbeValid();
            TheMedicationStatementTakenShouldbeValid();
            TheMedicationStatementDosageTextShouldbeValid();
        }

        [Then(@"the MedicationStatement Id should be valid")]
        public void TheMedicationStatementIdShouldBeValid()
        {
            MedicationStatements.ForEach(medStatement =>
            {
                medStatement.Id.ShouldNotBeNullOrEmpty();
            });
        }

        [Then(@"the Medication Statement Metadata should be valid")]
        public void TheMedicationStatementMetadataShouldBeValid()
        {
            MedicationStatements.ForEach(medicationStatement =>
            {
                CheckForValidMetaDataInResource(medicationStatement, FhirConst.StructureDefinitionSystems.kMedicationStatement);
            });
        }

        [Then(@"the MedicationStatement BasedOn should not be null and should refer to MedicationRequest with intent Plan")]
        public void TheMedicationStatementBasedOnShouldNotBeNullAndShouldReferToMedicationRequestWithIntentPlan()
        {
            MedicationStatements.ForEach(medStatement =>
            {
                medStatement.BasedOn.ShouldNotBeNull("MedicationStatements must refer to a MedicationRequest");
                medStatement.BasedOn.ShouldHaveSingleItem();

                medStatement.BasedOn.First().Reference.StartsWith("MedicationRequest");
                var requestId = medStatement.BasedOn.First().Reference.Substring(17);

                var requests = MedicationRequests.Where(req => req.Id.Equals(requestId)).ToList();
                requests.ShouldHaveSingleItem();
                requests.First().Intent.ShouldBeOfType<MedicationRequest.MedicationRequestIntent>("MedicationStatement links to MedicationRequest of incorrect type");
                requests.First().Intent.ShouldBe(MedicationRequest.MedicationRequestIntent.Plan);
            });
        }

        [Then(@"the MedicationStatement Status should be valid")]
        public void TheMedicationStatementStatusShouldbeValid()
        {
            MedicationStatements.ForEach(medStatement =>
            {
                if (medStatement.Status != null)
                {
                    medStatement.Status.ShouldNotBeNull("MedicationStatement Status cannot be null");
                    medStatement.Status.ShouldBeOfType<MedicationStatement.MedicationStatementStatus>($"MedicationStatements Status is not a valid value within the value set {FhirConst.ValueSetSystems.kMedicationStatementStatus}");
                    medStatement.Status.ShouldNotBe(MedicationStatement.MedicationStatementStatus.EnteredInError);
                    medStatement.Status.ShouldNotBe(MedicationStatement.MedicationStatementStatus.Intended);
                    medStatement.Status.ShouldNotBe(MedicationStatement.MedicationStatementStatus.Stopped);
                }
            });
        }

        [Then(@"the MedicationStatement MedicationReference should be valid")]
        public void TheMedicationStatementMedicationReferenceShouldbeValid()
        {
            MedicationStatements.ForEach(medStatement =>
            {
                if (medStatement.Medication != null)
                {
                    medStatement.Medication.ShouldNotBeNull("MedicationStatement MedicationReference cannot be null");
                }
            });
        }

        [Then(@"the MedicationStatement date asserted should be valid")]
        public void TheMedicationStatementDateAssertedShouldbeValid()
        {
            MedicationStatements.ForEach(medStatement =>
            {
                medStatement.DateAsserted.ShouldNotBeNull();
            });
        }

        [Then(@"the MedicationStatement subject should be valid")]
        public void TheMedicationStatementSubjectShouldbeValid()
        {
            MedicationStatements.ForEach(medStatement =>
            {
                medStatement.Subject.ShouldNotBeNull();
                medStatement.Subject.Reference.StartsWith("Patient");
            });
        }

        [Then(@"the MedicationStatement taken should be valid")]
        public void TheMedicationStatementTakenShouldbeValid()
        {
            MedicationStatements.ForEach(medStatement =>
            {
                medStatement.Taken.ShouldNotBeNull();
                medStatement.Taken.ShouldBeOfType<MedicationStatement.MedicationStatementTaken>("Medication Taken is of the wrong type");
            });
        }

        [Then(@"the MedicationStatement dosage text should be valid")]
        public void TheMedicationStatementDosageTextShouldbeValid()
        {
            MedicationStatements.ForEach(medStatement =>
            {
                medStatement.Dosage.ForEach(dosage =>
                {
                    dosage.Text.ShouldNotBeNullOrEmpty();
                });
            });
        }

        [Then(@"the List of MedicationStatements should be valid")]
        public void TheListOfMedicationStatementsShouldBeValid()
        {
            Lists.ShouldHaveSingleItem();
            Lists.ForEach(list =>
            {
                MedicationStatements.Count().Equals(list.Entry.Count());
                list.Id.ShouldNotBeNull();
                CheckForValidMetaDataInResource(list, FhirConst.StructureDefinitionSystems.kList);
                list.Status.ShouldBeOfType<List.ListStatus>("Status of medications list is of wrong type.");
                list.Status.ShouldBe(List.ListStatus.Current);
                list.Mode.ShouldBeOfType<ListMode>("Mode of medications list is of wrong type.");
                list.Mode.ShouldBe(ListMode.Snapshot);

            });
            
        }

        #endregion

        #region Medication Request Checks

        [Then(@"the Medication Requests should be valid")]
        public void TheMedicationRequestsShouldBeValid()
        {
            TheMedicationRequestIdShouldBeValid();
            TheMedicationRequestMetadataShouldBeValid();
            TheMedicationRequestGroupIdentiferShouldBeValid();
            TheMedicationRequestBasedOnShouldBeValid();
            TheMedicationRequestStatusShouldbeValid();
            TheMedicationRequestIntentShouldbeValid();
            TheMedicationRequestMedicationShouldbeValid();
            TheMedicationRequestSubjectShouldbeValid();
            TheMedicationRequestStatusReasonShouldbeValid();
            TheMedicationRequestPrescriptionTypeShouldbeValid();
            TheMedicationRequestRepeatInformationShouldbeValid();
            TheMedicationRequestDispenseRequestValidityPeriodShouldbeValid();
            TheMedicationRequestDosageInstructionsTextShouldbeValid();
            TheMedicationRequestRecorderShouldbeValid();
            TheMedicationRequestAuthoredOnShouldbeValid();
        }

        [Then(@"the MedicationRequest Id should be valid")]
        public void TheMedicationRequestIdShouldBeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                medRequest.Id.ShouldNotBeNullOrEmpty();
            });
        }

        [Then(@"the MedicationRequest Metadata should be valid")]
        public void TheMedicationRequestMetadataShouldBeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                CheckForValidMetaDataInResource(medRequest, FhirConst.StructureDefinitionSystems.kMedicationRequest);
            });
        }

        [Then(@"the MedicationRequest BasedOn should be valid")]
        public void TheMedicationRequestBasedOnShouldBeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                CodeableConcept prescriptionType = (CodeableConcept)medRequest.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationPrescriptionType).Value;
                if (prescriptionType.Coding.First().Display.Contains("Repeat"))
                {
                    medRequest.BasedOn.ShouldNotBeNull();
                }
                else 
                {
                    medRequest.BasedOn.ShouldBeNull();
                }
                
            });
        }

        [Then(@"the MedicationRequest GroupIdentifier should be valid")]
        public void TheMedicationRequestGroupIdentiferShouldBeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                CodeableConcept prescriptionType = (CodeableConcept)medRequest.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationPrescriptionType).Value;
                if (prescriptionType.Coding.First().Display.Contains("Repeat"))
                {
                    medRequest.GroupIdentifier.ShouldNotBeNull();
                }
            });
        }

        [Then(@"the MedicationRequest Status should be valid")]
        public void TheMedicationRequestStatusShouldbeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                medRequest.Status.ShouldNotBeNull("MedicationStatement Status cannot be null");
                medRequest.Status.ShouldBeOfType<MedicationRequest.MedicationRequestStatus>($"MedicationRequest Status is not a valid value within the value set {FhirConst.ValueSetSystems.kMedicationRequestStatus}");
                medRequest.Status.ShouldBeOneOf(MedicationRequest.MedicationRequestStatus.Active, MedicationRequest.MedicationRequestStatus.Completed, MedicationRequest.MedicationRequestStatus.Stopped);
                   
                CodeableConcept prescriptionType = (CodeableConcept)medRequest.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationPrescriptionType).Value;
                if (prescriptionType.Coding.First().Display.Contains("Acute"))
                { 
                    medRequest.Status.ShouldBe(MedicationRequest.MedicationRequestStatus.Completed);
                }
            });
        }

        [Then(@"the MedicationRequest Intent should be valid")]
        public void TheMedicationRequestIntentShouldbeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                medRequest.Intent.ShouldNotBeNull();
                medRequest.Intent.ShouldBeOfType<MedicationRequest.MedicationRequestIntent>($"MedicationRequest Intent is not a valid value within the value set {FhirConst.ValueSetSystems.kMedicationRequestIntent}");
                medRequest.Intent.ShouldBeOneOf(MedicationRequest.MedicationRequestIntent.Plan, MedicationRequest.MedicationRequestIntent.Order);
            });
        }

        [Then(@"the MedicationRequest Medication should be valid")]
        public void TheMedicationRequestMedicationShouldbeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                medRequest.Medication.ShouldNotBeNull();
            });
        }
        
        [Then(@"the MedicationRequest subject should be valid")]
        public void TheMedicationRequestSubjectShouldbeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                medRequest.Subject.ShouldNotBeNull();
                medRequest.Subject.Reference.StartsWith("Patient");
            });
        }

        [Then(@"the MedicationRequest authoredOn should be valid")]
        public void TheMedicationRequestAuthoredOnShouldbeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                medRequest.AuthoredOn.ShouldNotBeNull();
            });
        }

        [Then(@"the MedicationRequest recorder should be valid")]
        public void TheMedicationRequestRecorderShouldbeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                medRequest.Recorder.ShouldNotBeNull();
                medRequest.Recorder.Reference.StartsWith("Practitioner");
            });
        }

        [Then(@"the MedicationRequest dosageInstructions text should be valid")]
        public void TheMedicationRequestDosageInstructionsTextShouldbeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                medRequest.DosageInstruction.ShouldHaveSingleItem();
                medRequest.DosageInstruction.First().Text.ShouldNotBeNullOrEmpty();
            });
        }

        [Then(@"the MedicationRequest dispenseRequest validityPeriod should be valid")]
        public void TheMedicationRequestDispenseRequestValidityPeriodShouldbeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                medRequest.DispenseRequest.ValidityPeriod.ShouldNotBeNull();
                medRequest.DispenseRequest.ValidityPeriod.Start.ShouldNotBeNull();

                CodeableConcept prescriptionType = (CodeableConcept)medRequest.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationPrescriptionType).Value;
                if (prescriptionType.Coding.First().Display.Contains("Acute"))
                {
                    medRequest.DispenseRequest.ValidityPeriod.End.ShouldBeNull();
                }
            });
        }

        [Then(@"the MedicationRequest repeat information should be valid")]
        public void TheMedicationRequestRepeatInformationShouldbeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                Extension repeatInformation = medRequest.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationRepeatInformation);
                repeatInformation.ShouldNotBeNull();
                repeatInformation.GetExtension("numberOfRepeatPrescriptionsIssued").ShouldNotBeNull();
                CodeableConcept prescriptionType = (CodeableConcept)medRequest.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationPrescriptionType).Value;
                if (prescriptionType.Coding.First().Display.Contains("Acute"))
                {
                   repeatInformation.GetExtension("numberOfRepeatPrescriptionsAllowed").ShouldBeNull();
                }
            });
        }

        [Then(@"the MedicationRequest PrescriptionType should be valid")]
        public void TheMedicationRequestPrescriptionTypeShouldbeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                medRequest.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationPrescriptionType).ShouldNotBeNull();
                CodeableConcept prescriptionType = (CodeableConcept)medRequest.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationPrescriptionType).Value;

                prescriptionType.Coding.First().System.Equals(FhirConst.ValueSetSystems.kMedicationPrescriptionType);
            });
        }

        [Then(@"the MedicationRequest StatusReason should be valid")]
        public void TheMedicationRequestStatusReasonShouldbeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                if(medRequest.Status.Equals(MedicationRequest.MedicationRequestStatus.Stopped))
                {
                    Extension endReason = medRequest.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationRequestEndReason);
                    endReason.ShouldNotBeNull();
                    endReason.GetExtension("statusChangeDate").ShouldNotBeNull();
                    endReason.GetExtension("statusReason").ShouldNotBeNull();
                }
            });
        }

        #endregion
    }
}
