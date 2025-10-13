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
    using GPConnect.Provider.AcceptanceTests.Steps;
    using GPConnect.Provider.AcceptanceTests.Logger;
    using NUnit.Framework;

    [Binding]
    public sealed class StructuredMedicationSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;

        private List<Medication> Medications => _httpContext.FhirResponse.Medications;
        private List<MedicationStatement> MedicationStatements => _httpContext.FhirResponse.MedicationStatements;
        private List<MedicationRequest> MedicationRequests => _httpContext.FhirResponse.MedicationRequests;
        private List<Hl7.Fhir.Model.List> Lists => _httpContext.FhirResponse.Lists;
        private Bundle Bundle => _httpContext.FhirResponse.Bundle;

        public StructuredMedicationSteps(HttpSteps httpSteps, HttpContext httpContext)
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

        [Given(@"I add the medications parameter without mandatory partParameter")]
        public void GivenIAddTheMedicationsParameterWithoutMandatoryParameter()
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

        [Given(@"I add an incorrectly named medication parameter")]
        public void GivenIAddAnIncorrectlyNamedMedicationParameter()
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kPrescriptionIssues, (Base)new FhirBoolean(false)),
            };

            _httpContext.HttpRequestConfiguration.BodyParameters.Add("includeInvalidMedications", tuples);
        }

        [Given(@"I add the medications parameter with a timePeriod")]
        public void GivenIAddTheMedicationsParameterWithATimePeriod()
        {
            var tempDate = DateTime.UtcNow.AddYears(-2);
            var startDate = tempDate.ToString("yyyy-MM-dd");

            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kPrescriptionIssues, (Base)new FhirBoolean(false)),
                Tuple.Create(FhirConst.GetStructuredRecordParams.kMedicationDatePeriod, (Base)FhirHelper.GetStartDate(startDate))
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kMedication, tuples);
        }

        [Given(@"I add the medications parameter to search from ""(.*)"" years back")]
        public void GivenIAddTheMedicationsParameterToSearchFrom(int yearsToSearchBack)
        {
            var tempDate = DateTime.UtcNow.AddYears(-yearsToSearchBack);
            var startDate = tempDate.ToString("yyyy-MM-dd");

            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kPrescriptionIssues, (Base)new FhirBoolean(false)),
                Tuple.Create(FhirConst.GetStructuredRecordParams.kMedicationDatePeriod, (Base)FhirHelper.GetStartDate(startDate))
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kMedication, tuples);
        }

        [Given(@"I add the medications parameter with a start date equal to current date")]
        public void GivenIAddTheMedicationsParameterWithAStartDate()
        {
            var tempDate = DateTime.UtcNow;
            var startDate = tempDate.ToString("yyyy-MM-dd");

            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                        Tuple.Create(FhirConst.GetStructuredRecordParams.kPrescriptionIssues, (Base)new FhirBoolean(false)),
//                Tuple.Create(FhirConst.GetStructuredRecordParams.kMedicationDatePeriod, (Base)TimePeriodHelper.GetTimePeriodStartDateOnlyFormatted("yyyy-MM-dd"))               
            Tuple.Create(FhirConst.GetStructuredRecordParams.kMedicationDatePeriod, (Base)FhirHelper.GetStartDate(startDate))
        };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kMedication, tuples);
        }

        [Given(@"I add the medications parameter with a start date greater than current date")]
        public void GivenIAddTheMedicationsParameterWithAStartDateGreaterThanCurrentDate()
        {
            var tempDate = DateTime.UtcNow.AddDays(+1);
            var startDate = tempDate.ToString("yyyy-MM-dd");

            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                        Tuple.Create(FhirConst.GetStructuredRecordParams.kPrescriptionIssues, (Base)new FhirBoolean(false)),
//                Tuple.Create(FhirConst.GetStructuredRecordParams.kMedicationDatePeriod, (Base)TimePeriodHelper.GetTimePeriodStartDateOnlyFormatted("yyyy-MM-dd"))               
            Tuple.Create(FhirConst.GetStructuredRecordParams.kMedicationDatePeriod, (Base)FhirHelper.GetStartDate(startDate))
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
                Tuple.Create(FhirConst.GetStructuredRecordParams.kMedicationDatePeriod, (Base)FhirHelper.GetTimePeriod(startDate , endDate))
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kMedication, tuples);
        }
        // github ref 127
        // RMB 5/11/2018
        [Given(@"I set a medications period parameter start date to ""([^ ""]*)""")]
        public void GivenISetAMedicationsTimeAParameterStartDateTo(string startDate)
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kPrescriptionIssues, (Base)new FhirBoolean(false)),
                Tuple.Create(FhirConst.GetStructuredRecordParams.kMedicationDatePeriod, (Base)FhirHelper.GetStartDate(startDate))
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kMedication, tuples);
        }

        [Given(@"I add a duplicate medication part parameter")]
        public void GivenIaddaduplicatemedicationpartparameter()
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kPrescriptionIssues, (Base)new FhirBoolean(Boolean.Parse("true"))),
                Tuple.Create(FhirConst.GetStructuredRecordParams.kPrescriptionIssues, (Base)new FhirBoolean(Boolean.Parse("true")))
            };

            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kMedication, tuples);

        }

        #endregion

        #region List and Bundle Checks

        [Then(@"the List of MedicationStatements should be valid")]
        public void TheListOfMedicationStatementsShouldBeValid()
        {

            Lists.ForEach(list =>
            {
                if (list.Code.Coding.First().Code.Equals(FhirConst.GetSnoMedParams.kMeds))
                {

                    AccessRecordSteps.BaseListParametersAreValid(list);
                    // Added 1.2.1 RMB 1/10/2018				
                    list.Meta.VersionId.ShouldBeNull("List Meta VersionID should be Null");
                    list.Meta.LastUpdated.ShouldBeNull("List Meta LastUpdated should be Null");

                    //Medication specific checks
                    CheckForValidMetaDataInResource(list, FhirConst.StructureDefinitionSystems.kList);
                    MedicationStatements.Count().Equals(list.Entry.Count());
                    list.Title.Equals(FhirConst.ListTitles.kMedications);
                    list.Code.Equals("933361000000108");

                    if (list.Entry.Count.Equals(0))
                    {
                        list.EmptyReason.ShouldNotBeNull("The List's empty reason field must be populated if the list is empty.");
                        //                    list.EmptyReason.Text.Equals("noContent");
                        // Added github ref 87
                        // RMB 9/10/2018
                        //
                        if (list.EmptyReason.Coding.Count.Equals(1))
                        {
                            list.EmptyReason.Coding.First().Code.ShouldBe("no-content-recorded");
                            // Amended for github ref 172
                            // RMB 24/1/19							
                            list.EmptyReason.Coding.First().Display.ShouldBe("No Content Recorded");
                        }
                        list.Note.ShouldNotBeNull("The List's note field must be populated if the list is empty.");
                        // Added git hub ref 88
                        // RMB 9/10/2018
                        //#289 PG 6/9/2019 - changed as more notes added
                        //list.Note.First().Text.ShouldBe("Information not available");

                        var found = false;
                        foreach (var note in list.Note)
                        {
                            if (note.Text.Contains("Information not available"))
                                found = true;
                        }

                        if (!found)
                        {
                            Log.WriteLine("Warning not Found : Information not available");
                            found.ShouldBeTrue("Warning not Found : Information not available");
                        }
                        else
                        {
                            Log.WriteLine("Warning Found : Information not available");
                        }

                    }
                    else
                    {
                        list.Entry.ForEach(entry =>
                        {
                            entry.Item.ShouldNotBeNull("The item field must be populated for each list entry.");
                            entry.Item.Reference.ShouldStartWith("MedicationStatement");
                        });
                    }
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
            // Added 1.2.1 RMB 1/10/2018
            TheMedicationNotInUseShouldBeValid();
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
                        // Added github ref 85
                        // RMB 15/10/2018
                        coding.System.ShouldBeOneOf("http://snomed.info/sct", "http://read.info/readv2", "http://read.info/ctv3", "https://fhir.hl7.org.uk/Id/emis-drug-codes", "https://fhir.hl7.org.uk/Id/egton-codes", "https://fhir.hl7.org.uk/Id/multilex-drug-codes", "https://fhir.hl7.org.uk/Id/resipuk-gemscript-drug-codes");
                        Extension extension = coding.GetExtension("https://fhir.hl7.org.uk/STU3/StructureDefinition/Extension-coding-sctdescid");
                        //                        extension.ShouldNotBeNull();
                        //                        extension.GetExtension("descriptionId").ShouldNotBeNull();
                        //                        extension.GetExtension("descriptionDisplay").ShouldNotBeNull();
                        //
                        //                       if (extension.GetExtension("descriptionId").Value.Equals("196421000000109"))
                        //                      {
                        //                           medication.Code.Text.ShouldNotBeNullOrEmpty();
                        //                       }
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

        // Added 1.2.1 RMB 1/10/2018        
        [Then(@"the Medication Not In Use should be valid")]
        public void TheMedicationNotInUseShouldBeValid()
        {
            Medications.ForEach(medication =>
            {
                medication.Meta.VersionId.ShouldBeNull("Medication Meta VersionId should be Null");
                medication.Meta.LastUpdated.ShouldBeNull("Medication Meta lastUpdated should be Null");
                medication.Package.ShouldBeNull("Medication Package should be Null");
            });
        }

        #endregion

        #region Medication Statement Checks

        [Then(@"the Medication Statements should be valid")]
        public void TheMedicationStatementsShouldBeValid()
        {
            TheMedicationStatementIdShouldBeValid();
            TheMedicationStatementMetadataShouldBeValid();

            // Added check for Medication Statement PrescribingAgency is Mandatory RMB 08-08-2016		
            TheMedicationStatementPrescribingAgencyShouldBeValid();

            // Added check for Medication Statement System should be set and be a GUID RMB 08-08-2016		
            TheMedicationStatementIdentifierShouldBeValid();

            TheMedicationStatementBasedOnShouldNotBeNullAndShouldReferToMedicationRequestWithIntentPlan();
            TheMedicationStatementContextShouldBeValid();
            TheMedicationStatementStatusShouldbeValid();
            TheMedicationStatementEffectiveShouldbeValid();
            TheMedicationStatementMedicationReferenceShouldbeValid();
            TheMedicationStatementSubjectShouldbeValid();
            TheMedicationStatementTakenShouldbeValid();
            TheMedicationStatementDosageTextShouldbeValid();
            TheSpecifiedMedicationStatementFieldsShouldBeNull();
            // Added 1.2.1 RMB 1/10/2018
            TheMedicationStatementNotInUseShouldBeValid();
        }

        [Then(@"the specified MedicationStatement fields should be null")]
        public void TheSpecifiedMedicationStatementFieldsShouldBeNull()
        {
            MedicationStatements.ForEach(medStatement =>
            {
                medStatement.PartOf.ShouldBeEmpty();
                medStatement.Category.ShouldBeNull("MedicationStatement Category should be Null");
                medStatement.InformationSource.ShouldBeNull("MedicationStatement InformationSource should be Null");
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

        // Added check for MedicationStatement PrescribingAgency is Mandatory RMB 08-08-2016		
        [Then(@"the MedicationStatement PrescribingAgency should be valid")]
        public void TheMedicationStatementPrescribingAgencyShouldBeValid()
        {
            MedicationStatements.ForEach(medStatement =>
            {
                medStatement.GetExtension("https://fhir.nhs.uk/STU3/StructureDefinition/Extension-CareConnect-GPC-PrescribingAgency-1").ShouldNotBeNull();
            });
        }

        //PG 10-4-2019 #190 - Updated Code to check that GUID is valid
        [Then(@"the MedicationStatement Identifier should be valid")]
        public void TheMedicationStatementIdentifierShouldBeValid()
        {
            MedicationStatements.ForEach(medStatement =>
            {
                medStatement.Identifier.Count.ShouldBeGreaterThan(0, "There should be at least 1 Identifier system/value pair");
                if (medStatement.Identifier.Count == 1)
                {
                    var identifier = medStatement.Identifier.First();
                    identifier.System.ShouldNotBeNullOrWhiteSpace("MedicationStatement Identifier System Cannot be null or Empty");
                    identifier.Value.ShouldNotBeNullOrEmpty("MedicationStatement Identifier Value Cannot be null or Empty Value");
                }
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
                // Commented out code added back in 1.2.0 RMB 8/8/2018
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

                    // Added for 1.2.0 RMB 8/8/2018
                    dosage.Text.Equals("No information available");
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

        [Then(@"the MedicationStatement EffectiveDate is Greater Than Search Date of ""(.*)"" years ago")]
        private void TheMedicationStatementEffectiveDateIsGreaterThanSearchDate(string searchDateString)
        {
            DateTime searchDate = DateTime.Parse(searchDateString);

            MedicationStatements.ForEach(medStatement =>
            {
                if (medStatement.Effective != null)
                {
                    if (medStatement.Effective.TypeName.Contains("Period"))
                    {
                        Period effectivePeriod = (Period)medStatement.Effective;
                        var foundValidDateFlag = false;

                        //check start and end date and ensure atleast one is greater than search date
                        if (DateTime.Parse(effectivePeriod.Start) >= searchDate || DateTime.Parse(effectivePeriod.End) >= searchDate)
                        {
                            foundValidDateFlag = true;
                        }

                        if (!foundValidDateFlag)
                            Assert.Fail("Effective Date start or end should be greater than search date");
                    }
                    else
                    {
                        var effectiveDateTime = medStatement.Effective;
                        effectiveDateTime.ShouldBeOfType<FhirDateTime>();
                        DateTime effectiveDateToCheck = DateTime.Parse(effectiveDateTime.ToString());
                        effectiveDateToCheck.ShouldBeGreaterThanOrEqualTo(searchDate);
                        effectiveDateToCheck.ShouldBeLessThanOrEqualTo(DateTime.UtcNow);
                    }

                }
                else
                {
                    medStatement.DateAsserted.ShouldNotBeNull();
                    DateTime dateAsserted = DateTime.Parse(medStatement.DateAsserted);
                    dateAsserted.ShouldBeGreaterThanOrEqualTo(searchDate);
                    dateAsserted.ShouldBeLessThanOrEqualTo(DateTime.UtcNow);
                }

            });
        }

        private void checkDateIsInRange(Boolean useStart, Boolean useEnd, DateTime toCheck)
        {
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

            // git hub ref 184
            // RMB 19/2/19			
            //DateTime endPeriod = DateTime.Parse(period.End);
            DateTime endPeriod = DateTime.UtcNow;

            if (period.End != null)
            {
                endPeriod = DateTime.Parse(period.End);
            }

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

        // Added 1.2.1 RMB 1/10/2018        
        [Then(@"the MedicationStatement Not In Use should be valid")]
        public void TheMedicationStatementNotInUseShouldBeValid()
        {
            MedicationStatements.ForEach(medStatement =>
            {
                medStatement.Meta.VersionId.ShouldBeNull("MedicationStatement Meta VersionID should be Null");
                medStatement.Meta.LastUpdated.ShouldBeNull("MedicationStatement Meta LastUpdated should be Null");
                medStatement.PartOf.Count().ShouldBe(0);
                medStatement.Category.ShouldBeNull("MedicationStatement Category should be Null");
                medStatement.InformationSource.ShouldBeNull("MedicationStatement InformationSource should be Null");
                medStatement.DerivedFrom.Count().ShouldBe(0);
                //                medStatement.Taken.ShouldBeNull();
                medStatement.ReasonNotTaken.Count().ShouldBe(0);

            });
        }
        /* SJD 19/07/2019 removed as not relevant to 'Retrieve the medication structured record section for a patient including prescription issues' test
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
						// git hub ref 183
						// RMB 14/2/19
						CodeableConcept prescriptionType = (CodeableConcept)relatedRequest.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationPrescriptionType).Value;
						if (prescriptionType.Coding.First().Display.Contains("Repeat"))
						{
							relatedRequest.AuthoredOn.ShouldContain(plan.AuthoredOn);
						}
					});
				});
			} */

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

            // Added RMB 8/8/2018
            TheMedicationRequestIdentifierShouldBeValid();

            //TheMedicationRequestGroupIdentiferShouldBeValid();
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
            // Added 1.2.1 RMB 1/10/2018
            TheMedicationRequestNotInUseShouldBeValid();
        }

        [Then(@"the specified Medication Requests fields should be null")]
        public void TheSpecifiedMedicationRequestsFieldsShouldBeNull()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                medRequest.Definition.ShouldBeEmpty();
                medRequest.Category.ShouldBeNull("MedicationRequest Category should be Null");
                medRequest.Priority.ShouldBeNull("MedicationRequest Priority should be Null");
                medRequest.SupportingInformation.ShouldBeEmpty();

                // Removed 1.2.0 RMB 8/8/2018
                //medRequest.DispenseRequest.ExpectedSupplyDuration.ShouldBeNull();

                medRequest.Substitution.ShouldBeNull("MedicationRequest Substitution should be Null");

                // Added RMB 8/8/2018
                medRequest.DetectedIssue.ShouldBeEmpty();

                medRequest.EventHistory.ShouldBeEmpty();

                CodeableConcept prescriptionType = (CodeableConcept)medRequest.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationPrescriptionType).Value;
                if (!prescriptionType.Coding.First().Display.Contains("Repeat"))
                {
                    // git hub ref 167
                    //  RMB 22/1/19
                    //medRequest.GroupIdentifier.ShouldBeNull();
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

        // Added RMB 8/8/2018
        [Then(@"the MedicationRequest Identifier should be valid")]
        public void TheMedicationRequestIdentifierShouldBeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                medRequest.Identifier.Count.ShouldBeGreaterThan(0, "There should be at least 1 Identifier system/value pair");
                if (medRequest.Identifier.Count == 1)
                {
                    var identifier = medRequest.Identifier.First();
                    identifier.System.ShouldNotBeNullOrWhiteSpace("MedicationRequest Identifier System Cannot be null or Empty");
                    identifier.Value.ShouldNotBeNullOrEmpty("MedicationRequest Identifier Value Cannot be null or Empty Value");
                }
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
                // git hub ref 170
                // RMB 23/1/19
                //                medRequest.Status.ShouldNotBeNull("MedicationRequest Status cannot be null");
                if (medRequest.Status != null)
                {
                    medRequest.Status.ShouldBeOfType<MedicationRequest.MedicationRequestStatus>($"MedicationRequest Status is not a valid value within the value set {FhirConst.ValueSetSystems.kVsMedicationRequestStatus}");
                    medRequest.Status.ShouldBeOneOf(MedicationRequest.MedicationRequestStatus.Active, MedicationRequest.MedicationRequestStatus.Completed, MedicationRequest.MedicationRequestStatus.Stopped);
                }

                CodeableConcept prescriptionType = (CodeableConcept)medRequest.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationPrescriptionType).Value;
                // git hub ref 170
                // RMB 23/1/19
                //                if (prescriptionType.Coding.First().Display.Contains("Acute"))
                //                { 
                //
                //                    medRequest.Status.ShouldBe(MedicationRequest.MedicationRequestStatus.Completed);
                //               }
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

                // Added for 1.2.0 RMB 8/8/2018
                medRequest.DosageInstruction.First().Text.Equals("No information available");
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
                    // git hub ref 160
                    // RMB 14/1/19
                    //medRequest.DispenseRequest.ValidityPeriod.End.ShouldBeNull();
                }
            });
        }

        //#310 - PG 4/10/2019 - Changed logic to look for extension when repeat or repeat dispensing with intent=plan
        [Then(@"the MedicationRequest repeat information should be valid")]
        public void TheMedicationRequestRepeatInformationShouldbeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                CodeableConcept prescriptionType = (CodeableConcept)medRequest.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationPrescriptionType).Value;

                //Repeats Plans - Check two Extensions are sent
                if ((prescriptionType.Coding.First().Display.Equals("Repeat") || prescriptionType.Coding.First().Display.Equals("Repeat dispensing")) && medRequest.Intent.Equals(MedicationRequest.MedicationRequestIntent.Plan))
                {
                    Extension repeatInformation = medRequest.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationRepeatInformation);

                    if (repeatInformation != null)
                    {
                        repeatInformation.GetExtension("numberOfRepeatPrescriptionsIssued").ShouldNotBeNull();
                        repeatInformation.GetExtension("numberOfRepeatPrescriptionsAllowed").ShouldNotBeNull();
                    }
                    else
                    {
                        Assert.Fail("Repeat or Repeat dispensing Medication Request Should have Repeat Information Extension. Failing MedRequest ID: " + medRequest.Id.ToString());
                    }
                }
                //Acute etc - Check No Repeat extensions sent
                else
                {
                    Extension repeatInformation = medRequest.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationRepeatInformation);
                    if (repeatInformation != null)
                    {
                        repeatInformation.GetExtension("numberOfRepeatPrescriptionsIssued").ShouldBeNull();
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

                // Added for 1.2.0 RMB 8/8/2018
                prescriptionType.Equals("No information available");

                CodeableConcept prescriptionTypeValue = (CodeableConcept)prescriptionType.Value;

                prescriptionTypeValue.Coding.First().System.Equals(FhirConst.CodeSystems.kCcPresriptionType);
            });
        }

        [Then(@"the MedicationRequest StatusReason should be valid")]
        public void TheMedicationRequestStatusReasonShouldbeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                if ((medRequest.Intent.Equals(MedicationRequest.MedicationRequestIntent.Plan)) && (medRequest.Status.Equals(MedicationRequest.MedicationRequestStatus.Stopped)))
                {
                    Extension endReason = medRequest.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationRequestEndReason);
                    endReason.ShouldNotBeNull();
                    endReason.GetExtension("statusChangeDate").ShouldNotBeNull();
                    endReason.GetExtension("statusReason").ShouldNotBeNull();
                    endReason.GetExtension("statusReason").Equals("No information available");
                }
                else if ((medRequest.Intent.Equals(MedicationRequest.MedicationRequestIntent.Order)) && (medRequest.Status.Equals(MedicationRequest.MedicationRequestStatus.Stopped)))
                {
                    Extension endReason = medRequest.GetExtension(FhirConst.StructureDefinitionSystems.kMedicationRequestEndReason);
                    endReason.ShouldBeNull();
                }

            });
        }

        // Added 1.2.1 RMB 1/10/2018        
        [Then(@"the MedicationRequest Not In Use should be valid")]
        public void TheMedicationRequestNotInUseShouldBeValid()
        {
            MedicationRequests.ForEach(medRequest =>
            {
                medRequest.Meta.VersionId.ShouldBeNull("MedicationRequest Meta VersionID should be Null");
                medRequest.Meta.LastUpdated.ShouldBeNull("MedicationRequest MetaLastUpdated should be Null");
                medRequest.Definition.Count().ShouldBe(0);
                medRequest.Category.ShouldBeNull("MedicationRequest Category should be Null");
                medRequest.Priority.ShouldBeNull("MedicationRequest Priority should be Null");
                medRequest.SupportingInformation.Count().ShouldBe(0);
                medRequest.Substitution.ShouldBeNull("MedicationRequest Substitution should be Null");
                medRequest.DetectedIssue.Count().ShouldBe(0);
                medRequest.EventHistory.Count().ShouldBe(0);

            });
        }
        #endregion
    }
}
