namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using Constants;
    using Context;
    using TechTalk.SpecFlow;
    using Shouldly;
    using Hl7.Fhir.Model;
    using System.Collections.Generic;
    using System;
    using System.Linq;
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
        private Bundle Bundle => _httpContext.FhirResponse.Bundle;

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

        [Given(@"I add the medicationDatePeriod partParameter")]
        public void IAddTheMedicationsDatePeriodPartParameter()
        {
            ParameterComponent param = new ParameterComponent();
            param.Name = FhirConst.GetStructuredRecordParams.kMedicationDatePeriod;
            param.Value = TimePeriodHelper.GetTimePeriodFormatted("yyyy-MM-dd");
            _httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(param);
        }

        [Given(@"I add the prescriptions partParameter")]
        public void IAddThePrescriptionsPartParameter()
        {
            ParameterComponent param = new ParameterComponent();
            param.Name = FhirConst.GetStructuredRecordParams.kPrescriptionIssues;
            param.Value = new FhirBoolean(true);
            _httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(param);
        }

        [Given(@"I add the medications parameter incorrectly")]
        public void GivenIAddTheMedicationsParameterIncorrectly()
        {
            ParameterComponent param = new ParameterComponent();
            param.Name = FhirConst.GetStructuredRecordParams.kMedication;
            _httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(param);

            ParameterComponent partParam = new ParameterComponent();
            partParam.Name = FhirConst.GetStructuredRecordParams.kPrescriptionIssues;
            _httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(partParam);
        }

        [Given(@"I add an invalid medications parameter")]
        public void GivenIAddAnInvalidMedicationsParameter()
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kPrescriptionIssues, (Base)new FhirBoolean(false)),
            };

            _httpContext.HttpRequestConfiguration.BodyParameters.Add("includeInvalidMedications", tuples);
        }

        [Given(@"I add the medications parameter with a timePeriod")]
        public void GivenIAddTheMedicationsParameterWithATimePeriod()
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kPrescriptionIssues, (Base)new FhirBoolean(false)),
                Tuple.Create(FhirConst.GetStructuredRecordParams.kMedicationDatePeriod, (Base)TimePeriodHelper.GetTimePeriodFormatted("yyyy-MM-dd"))
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kMedication, tuples);
        }

        [Given(@"I add the medications parameter with a start date")]
        public void GivenIAddTheMedicationsParameterWithAStartPeriod()
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kPrescriptionIssues, (Base)new FhirBoolean(false)),
                Tuple.Create(FhirConst.GetStructuredRecordParams.kMedicationDatePeriod, (Base)TimePeriodHelper.GetTimePeriodStartDateOnlyFormatted("yyyy-MM-dd"))
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kMedication, tuples);
        }

        [Given(@"I add the medications parameter with an end date")]
        public void GivenIAddTheMedicationsParameterWithAnEndPeriod()
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kPrescriptionIssues, (Base)new FhirBoolean(false)),
                Tuple.Create(FhirConst.GetStructuredRecordParams.kMedicationDatePeriod, (Base)TimePeriodHelper.GetTimePeriodEndDateOnlyFormatted("yyyy-MM-dd"))
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kMedication, tuples);
        }

        [Given(@"I set a medications period parameter start date to ""([^ ""]*)"" and end date to ""([^ ""]*)""")]
        public void GivenISetAMedicationsTimeAParameterStartDateToAndEndDateTo(string startDate, string endDate)
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kPrescriptionIssues, (Base)new FhirBoolean(false)),
                Tuple.Create(FhirConst.GetStructuredRecordParams.kMedicationDatePeriod, (Base)FhirHelper.GetTimePeriod(startDate, endDate))
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kMedication, tuples);
        }

        #endregion
        
        #region List and Bundle Checks

        [Then(@"the List of MedicationStatements should be valid")]
        public void TheListOfMedicationStatementsShouldBeValid()
        {
            Lists.ShouldHaveSingleItem("The medications data must contain a single list.");
            Lists.ForEach(list =>
            {
                AccessRecordSteps.BaseListParametersAreValid(list);

                //Medication specific checks
                CheckForValidMetaDataInResource(list, FhirConst.StructureDefinitionSystems.kList);
                MedicationStatements.Count().Equals(list.Entry.Count());
                list.Code.Equals("933361000000108");

                if (list.Entry.Count.Equals(0))
                {
                    list.EmptyReason.ShouldNotBeNull("The List's empty reason field must be populated if the list is empty.");
                    list.EmptyReason.Text.Equals("noContent");
                    list.Note.ShouldNotBeNull("The List's note field must be populated if the list is empty.");
                }
                else
                {
                    list.Entry.ForEach(entry =>
                    {
                        entry.Item.ShouldNotBeNull("The item field must be populated for eac list entry.");
                        entry.Item.Reference.ShouldStartWith("MedicationStatement");
                    });
                }
            });
        }

        [Then(@"the response bundle should not contain any medications data")]
        public void TheResponseBundleShouldNotContainAnyMedicationsData()
        {
            Medications.ShouldBeEmpty();
            MedicationStatements.ShouldBeEmpty();
            MedicationRequests.ShouldBeEmpty();
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
                        Extension extension = coding.GetExtension("https://fhir.hl7.org.uk/STU3/StructureDefinition/Extension-coding-sctdescid");
                        extension.ShouldNotBeNull();
                        extension.GetExtension("descriptionId").ShouldNotBeNull();
                        extension.GetExtension("descriptionDisplay").ShouldNotBeNull();

                        if (extension.GetExtension("descriptionId").Value.Equals("196421000000109"))
                        {
                            medication.Code.Text.ShouldNotBeNullOrEmpty();
                        }
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
            TheMedicationStatementContextShouldBeValid();
            TheMedicationStatementStatusShouldbeValid();
            TheMedicationStatementEffectiveShouldbeValid();
            TheMedicationStatementMedicationReferenceShouldbeValid();
            TheMedicationStatementSubjectShouldbeValid();
            TheMedicationStatementTakenShouldbeValid();
            TheMedicationStatementDosageTextShouldbeValid();
            TheSpecifiedMedicationStatementFieldsShouldBeNull();
        }

        [Then(@"the specified MedicationStatement fields should be null")]
        public void TheSpecifiedMedicationStatementFieldsShouldBeNull()
        {
            MedicationStatements.ForEach(medStatement =>
            {
                medStatement.PartOf.ShouldBeEmpty();
                medStatement.Category.ShouldBeNull();
                medStatement.InformationSource.ShouldBeNull();
                medStatement.DerivedFrom.ShouldBeEmpty();
                //medStatement.Taken.ShouldBeNull();
                medStatement.ReasonNotTaken.ShouldBeEmpty();
                medStatement.GetExtension("https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-MedicationChangeSummary-1").ShouldBeNull();
            });
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
                var requestId = medStatement.BasedOn.First().Reference.Substring(18);

                var requests = MedicationRequests.Where(req => req.Id.Equals(requestId) && req.Intent.Equals(MedicationRequest.MedicationRequestIntent.Plan)).ToList();
                requests.ShouldHaveSingleItem();
                requests.First().Intent.ShouldBeOfType<MedicationRequest.MedicationRequestIntent>("MedicationStatement links to MedicationRequest of incorrect type");
            });
        }

        [Then(@"the Medication Statement Context should be valid")]
        public void TheMedicationStatementContextShouldBeValid()
        {
            MedicationStatements.ForEach(medicationStatement =>
            {
                if (medicationStatement.Context != null)
                {
                    medicationStatement.Context.Reference.StartsWith("Encounter");
                }
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
                    medStatement.Status.ShouldBeOfType<MedicationStatement.MedicationStatementStatus>($"MedicationStatements Status is not a valid value within the value set {FhirConst.ValueSetSystems.kVsMedicationStatementStatus}");
                    medStatement.Status.ShouldBeOneOf(MedicationStatement.MedicationStatementStatus.Active, MedicationStatement.MedicationStatementStatus.Completed, MedicationStatement.MedicationStatementStatus.Stopped);
                    medStatement.Status.ShouldNotBeOneOf(MedicationStatement.MedicationStatementStatus.EnteredInError, MedicationStatement.MedicationStatementStatus.Intended, MedicationStatement.MedicationStatementStatus.OnHold);
                }
            });
        }

        [Then(@"the MedicationStatement MedicationReference should be valid")]
        public void TheMedicationStatementMedicationReferenceShouldbeValid()
        {
            MedicationStatements.ForEach(medStatement =>
            {
                medStatement.Medication.ShouldNotBeNull("MedicationStatement MedicationReference cannot be null");
                medStatement.Medication.TypeName.ShouldContain("Reference");

                ResourceReference medReference = (ResourceReference)medStatement.Medication;
                medReference.Reference.StartsWith("Encounter");
            });
        }

        [Then(@"the MedicationStatement effective should be valid")]
        public void TheMedicationStatementEffectiveShouldbeValid()
        {
            MedicationStatements.ForEach(medStatement =>
            {
                if (medStatement.Effective != null)
                {
                    if (medStatement.Effective.TypeName.Contains("Period"))
                    {
                        Period effectivePeriod = (Period)medStatement.Effective;
                        effectivePeriod.Start.ShouldNotBeNull();
                    }
                    else
                    {
                        medStatement.Effective.TypeName.ShouldContain("DateTime");
                    }
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
                //medStatement.Taken.ShouldNotBeNull();
                //medStatement.Taken.ShouldBeOfType<MedicationStatement.MedicationStatementTaken>("Medication Taken is of the wrong type");
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

        [Then(@"the MedicationStatement for prescriptions prescribed elsewhere should be valid")]
        public void TheMedicationStatementForPrescriptionsPrescribedElsewhereShouldBeValid()
        {
            List<MedicationStatement> prescribedElsewhere = MedicationStatements.Where(medStatement => 
                medStatement.GetExtension(FhirConst.StructureDefinitionSystems.kExtPrescriptionAgency) != null).ToList();

            prescribedElsewhere.ForEach(medStatement =>
            {
                var requestId = medStatement.BasedOn.First().Reference.Substring(18);
                List<MedicationRequest> requestsFromElsewhere = MedicationRequests.Where(medRequest => medRequest.Id.Equals(requestId)).ToList();
                requestsFromElsewhere.ShouldHaveSingleItem();
                requestsFromElsewhere.First().Intent.Equals(MedicationRequest.MedicationRequestIntent.Plan);
            });
        }

        [Then(@"the MedicationStatement dates are with the default period with start ""(.*)"" and end ""(.*)""")]
        private void TheMedicationStatementDatesAreWithinTheDefaultPeriod(Boolean useStart, Boolean useEnd)
        {
            MedicationStatements.ForEach(medStatement =>
            {
                Extension lastIssueDateExt = medStatement.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationStatementLastIssueDate);
                if (lastIssueDateExt != null)
                {
                    var lastIssueDateFhir = lastIssueDateExt.Value;
                    lastIssueDateFhir.ShouldBeOfType<FhirDateTime>();
                    checkDateIsInRange(useStart, useEnd, DateTime.Parse(lastIssueDateFhir.ToString()));
                }
                else if (medStatement.Effective != null)
                {
                    if (medStatement.Effective.TypeName.Contains("Period"))
                    {
                        Period effectivePeriod = (Period)medStatement.Effective;
                        checkPeriodIsInRange(useStart, useEnd, effectivePeriod);
                    }
                    else
                    {
                        var effectiveDateTime = medStatement.Effective;
                        effectiveDateTime.ShouldBeOfType<FhirDateTime>();
                        checkDateIsInRange(useStart, useEnd, DateTime.Parse(effectiveDateTime.ToString()));
                    }

                }
                else
                {
                    medStatement.DateAsserted.ShouldNotBeNull();
                    DateTime dateAsserted = DateTime.Parse(medStatement.DateAsserted);
                    checkDateIsInRange(useStart, useEnd, dateAsserted);
                }
            });
        }

        private void checkDateIsInRange(Boolean useStart, Boolean useEnd, DateTime toCheck) {
            DateTime start = DateTime.UtcNow.AddYears(-2);
            DateTime end = DateTime.UtcNow;

            if (useStart)
            {
                toCheck.ShouldBeGreaterThanOrEqualTo(start);
            }
            if (useEnd)
            {
                toCheck.ShouldBeLessThanOrEqualTo(end);
            }
        }

        private void checkPeriodIsInRange(Boolean useStart, Boolean useEnd, Period period)
        {
            DateTime startPeriod = DateTime.Parse(period.Start);
            DateTime endPeriod = DateTime.Parse(period.End);
            DateTime start = DateTime.UtcNow.AddYears(-2);
            DateTime end = DateTime.UtcNow;

            if (useStart)
            {
                if (startPeriod < start)
                {
                    endPeriod.ShouldBeGreaterThanOrEqualTo(start);
                }
                else
                {
                    startPeriod.ShouldBeGreaterThanOrEqualTo(start);
                }
            }
            if (useEnd)
            {
                if (endPeriod > end)
                {
                    startPeriod.ShouldBeLessThanOrEqualTo(end);
                }
                else
                {
                    endPeriod.ShouldBeLessThanOrEqualTo(end);
                }
            }
        }

        #endregion

        #region Medication Request Checks

        [Then(@"order requests should have the same authoredOn date as their plan")]
        public void TheRelatedMedicationRequestsShouldHaveTheSameAuthoredOnDates()
        {
            var plans = MedicationRequests.Where(req => req.Intent.Equals(MedicationRequest.MedicationRequestIntent.Plan)).ToList();
            plans.ForEach(plan =>
            {
                var allRelatedRequests = MedicationRequests.Where(
                     relatedReq => relatedReq.BasedOn.Count > 0 && relatedReq.BasedOn.First().Reference.Equals("MedicationRequest/" + plan.Id)
                );

                allRelatedRequests.ToList().ForEach(relatedRequest =>
                {
                    relatedRequest.AuthoredOn.ShouldBe(plan.AuthoredOn);
                });
            });
        }

        [Then(@"there should only be one order request for acute prescriptions")]
        public void ThereShouldOnlyBeOneOrderRequestForAcutePrescriptions()
        {
            List<MedicationRequest> acuteRequests = MedicationRequests.Where(req => isRequestAnAcutePlan(req).Equals(true)).ToList();
            acuteRequests.ForEach(acuteRequest =>
            {
                List<MedicationRequest> acuteRelatedRequests = MedicationRequests.Where(
                     relatedReq => relatedReq.BasedOn.Count > 0 && relatedReq.BasedOn.First().Reference.Equals("MedicationRequest/" + acuteRequest.Id)
                ).ToList();
                acuteRelatedRequests.Count.ShouldBeLessThanOrEqualTo(1); //Acute should be one plan and one order at most and plans should not have a BasedOn
            });
        }

        private Boolean isRequestAnAcutePlan(MedicationRequest request)
        {
            CodeableConcept prescriptionType = (CodeableConcept)request.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationPrescriptionType).Value;
            if (prescriptionType.Coding.First().Display.Contains("Acute"))
            {
                return request.Intent.Equals(MedicationRequest.MedicationRequestIntent.Plan);
            }
            return false;
        }

        [Then(@"the Medication Requests should not contain any issues")]
        public void TheMedicationRequestsShouldNotContainAnyIssues()
        {
            MedicationRequests.Where(req => req.Intent.Equals(MedicationRequest.MedicationRequestIntent.Order)).ToList().ShouldBeEmpty();  
        }

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
            TheMedicationRequestContextShouldbeValid();
            TheMedicationRequestStatusReasonShouldbeValid();
            TheMedicationRequestPrescriptionTypeShouldbeValid();
            TheMedicationRequestRepeatInformationShouldbeValid();
            TheMedicationRequestDispenseRequestValidityPeriodShouldbeValid();
            TheMedicationRequestDosageInstructionsTextShouldbeValid();
            TheMedicationRequestRecorderShouldbeValid();
            TheMedicationRequestAuthoredOnShouldbeValid();
            ThereShouldBeAtLeastOneMedicationRequestWithIntentToPlan();
            TheSpecifiedMedicationRequestsFieldsShouldBeNull();
        }

        [Then(@"the specified Medication Requests fields should be null")]
        public void TheSpecifiedMedicationRequestsFieldsShouldBeNull()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                medRequest.Definition.ShouldBeEmpty();
                medRequest.Category.ShouldBeNull();
                medRequest.Priority.ShouldBeNull();
                medRequest.SupportingInformation.ShouldBeEmpty();
                medRequest.DispenseRequest.ExpectedSupplyDuration.ShouldBeNull();
                medRequest.Substitution.ShouldBeNull();
                medRequest.EventHistory.ShouldBeEmpty();
                
                CodeableConcept prescriptionType = (CodeableConcept)medRequest.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationPrescriptionType).Value;
                if (!prescriptionType.Coding.First().Display.Contains("Repeat"))
                {
                    medRequest.GroupIdentifier.ShouldBeNull();
                }
            });
        }

        [Then(@"the MedicationRequest Id should be valid")]
        public void TheMedicationRequestIdShouldBeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                medRequest.Id.ShouldNotBeNullOrEmpty();
            });
        }

        [Then(@"there should be at least one medication request with intent to plan")]
        public void ThereShouldBeAtLeastOneMedicationRequestWithIntentToPlan()
        {
            if (MedicationStatements.Count > 0)
            {
                var planRequests = MedicationRequests.Where(req => req.Intent.Equals(MedicationRequest.MedicationRequestIntent.Plan)).ToList();
                planRequests.ShouldNotBeNull();
                planRequests.Count.ShouldBeGreaterThan(0);
            }
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
                if (medRequest.Intent.Equals(MedicationRequest.MedicationRequestIntent.Order))
                {
                    medRequest.BasedOn.ShouldNotBeEmpty();
                    medRequest.BasedOn.First().Reference.StartsWith("MedicationRequest");
                }
                else 
                {
                    medRequest.BasedOn.ShouldBeEmpty();
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
                    medRequest.GroupIdentifier.Value.ShouldNotBeNull();
                }
            });
        }

        [Then(@"the MedicationRequest Status should be valid")]
        public void TheMedicationRequestStatusShouldbeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                medRequest.Status.ShouldNotBeNull("MedicationStatement Status cannot be null");
                medRequest.Status.ShouldBeOfType<MedicationRequest.MedicationRequestStatus>($"MedicationRequest Status is not a valid value within the value set {FhirConst.ValueSetSystems.kVsMedicationRequestStatus}");
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
                medRequest.Intent.ShouldBeOfType<MedicationRequest.MedicationRequestIntent>($"MedicationRequest Intent is not a valid value within the value set {FhirConst.ValueSetSystems.kVsMedicationRequestIntent}");
                medRequest.Intent.ShouldBeOneOf(MedicationRequest.MedicationRequestIntent.Plan, MedicationRequest.MedicationRequestIntent.Order);
            });
        }

        [Then(@"the MedicationRequest Medication should be valid")]
        public void TheMedicationRequestMedicationShouldbeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                medRequest.Medication.ShouldNotBeNull();

                medRequest.Medication.TypeName.ShouldContain("Reference");

                ResourceReference medReference = (ResourceReference)medRequest.Medication;
                medReference.Reference.StartsWith("Medication");
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

        [Then(@"the MedicationRequest context should be valid")]
        public void TheMedicationRequestContextShouldbeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                if (medRequest.Context != null)
                {
                    medRequest.Context.Reference.StartsWith("Encounter");
                }
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
                CodeableConcept prescriptionType = (CodeableConcept)medRequest.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationPrescriptionType).Value;
                if (prescriptionType.Coding.First().Display.Contains("Repeat"))
                {
                    Extension repeatInformation = medRequest.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationRepeatInformation);
                    repeatInformation.ShouldNotBeNull();

                    repeatInformation.GetExtension("numberOfRepeatPrescriptionsIssued").ShouldNotBeNull();

                }
                else
                { 
                    Extension repeatInformation = medRequest.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationRepeatInformation);
                    if (repeatInformation != null)
                    {
                        repeatInformation.GetExtension("numberOfRepeatPrescriptionsAllowed").ShouldBeNull();
                    }
                }
            });
        }

        [Then(@"the MedicationRequest PrescriptionType should be valid")]
        public void TheMedicationRequestPrescriptionTypeShouldbeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                Extension prescriptionType = medRequest.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationPrescriptionType);
                prescriptionType.ShouldNotBeNull();
                CodeableConcept prescriptionTypeValue = (CodeableConcept)prescriptionType.Value;

                prescriptionTypeValue.Coding.First().System.Equals(FhirConst.CodeSystems.kCcPresriptionType);
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
