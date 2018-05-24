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
    using GPConnect.Provider.AcceptanceTests.Enum;
    using static Hl7.Fhir.Model.Parameters;
    using GPConnect.Provider.AcceptanceTests.Helpers;

    [Binding]
    public sealed class AccessStructuredAllergySteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private List<AllergyIntolerance> AllAllergyIntolerances = new List<AllergyIntolerance>();
        private List<AllergyIntolerance> ActiveAllergyIntolerances => _httpContext.FhirResponse.AllergyIntolerances;
        private List<List> Lists => _httpContext.FhirResponse.Lists;
        private Bundle Bundle => _httpContext.FhirResponse.Bundle;

        public AccessStructuredAllergySteps(HttpSteps httpSteps, HttpContext httpContext) 
            : base(httpSteps)
        {
            _httpContext = httpContext;
        }

        #region Allergy Parameters

        [Given(@"I add the allergies parameter with resolvedAllergies set to ""(.*)""")]
        public void GivenIAddTheAllergiesParameterWithResolvedAllergiesSetTo(string partValue)
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] { Tuple.Create(FhirConst.GetStructuredRecordParams.kResolvedAllergies, (Base)new FhirBoolean(Boolean.Parse(partValue))) };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kAllergies, tuples);
        }

        [Given(@"I add the allergies parameter")]
        public void GivenIAddTheAllergiesParameter()
        {
            ParameterComponent param = new ParameterComponent();
            param.Name = FhirConst.GetStructuredRecordParams.kAllergies;
            _httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(param);
        }

        [Given(@"I add an invalid allergies parameter")]
        public void GivenIAddAnInvalidAllergiesParameter()
        {
           IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kResolvedAllergies, (Base)new FhirBoolean(false))
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add("inlcudeInvalidAllergies", tuples);
        }

        [Given(@"I add the allergies parameter with includePrescriptionIssues")]
        public void GivenIAddTheAllergiesParameterWithIncludePrescriptionIssues()
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kPrescriptionIssues, (Base)new FhirBoolean(false)),
                Tuple.Create(FhirConst.GetStructuredRecordParams.kResolvedAllergies, (Base)new FhirBoolean(false))
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kAllergies, tuples);
        }

        [Given(@"I add the allergies parameter with a timePeriod")]
        public void GivenIAddTheAllergiesParameterWithATimePeriod()
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kTimePeriod, (Base)TimePeriodHelper.GetDefaultTimePeriod()),
                Tuple.Create(FhirConst.GetStructuredRecordParams.kResolvedAllergies, (Base)new FhirBoolean(false))
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kAllergies, tuples);
        }

        [Given(@"I add the allergies parameter with a start date")]
        public void GivenIAddTheAllergiesParameterWithAStartPeriod()
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kTimePeriod, (Base)TimePeriodHelper.GetTimePeriodStartDateOnly()),
                Tuple.Create(FhirConst.GetStructuredRecordParams.kResolvedAllergies, (Base)new FhirBoolean(false))
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kAllergies, tuples);
        }

        [Given(@"I add the allergies parameter with an end date")]
        public void GivenIAddTheAllergiesParameterWithAnEndPeriod()
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kTimePeriod, (Base)TimePeriodHelper.GetTimePeriodEndDateOnly()),
                Tuple.Create(FhirConst.GetStructuredRecordParams.kResolvedAllergies, (Base)new FhirBoolean(false))
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kAllergies, tuples);
        }

        #endregion

        #region List and Bundle Validity Checks

        [Then(@"the List of AllergyIntolerances should be valid")]
        public void TheListOfAllergyIntolerancesShouldBeValid()
        {
            Lists.ForEach(list =>
            {
                AccessRecordSteps.BaseListParametersAreValid(list);

                //Alergy specific checks
                CheckForValidMetaDataInResource(list, FhirConst.StructureDefinitionSystems.kList);

                if(list.Title.Equals(FhirConst.ListTitles.kActiveAllergies))
                {
                    list.Code.Equals("886921000000105");

                } else if (list.Title.Equals(FhirConst.ListTitles.kResolvedAllergies))
                {
                    list.Code.Equals("TBD");
                }

                if (list.Entry.Count > 0)
                {
                    list.Entry.ForEach(entry =>
                    {
                        entry.Item.ShouldNotBeNull("The item field must be populated for each list entry.");
                        entry.Item.Reference.ShouldMatch("AllergyIntolerance/.+|#.+");
                        if (entry.Item.IsContainedReference)
                        {
                            string id = entry.Item.Reference.Substring(1);
                            List<Resource> contained = list.Contained.Where(allergy => allergy.Id.Equals(id)).ToList();
                            contained.Count.ShouldBe(1);
                            contained.ForEach(allergy =>
                            {
                                AllergyIntolerance allergyIntolerance = (AllergyIntolerance)allergy;
                                allergyIntolerance.ClinicalStatus.Equals(AllergyIntolerance.AllergyIntoleranceClinicalStatus.Resolved);
                            });
                        }
                    });
                }

                if (list.Entry.Count == 0)
                {
                    list.EmptyReason.ShouldNotBeNull("The List's empty reason field must be populated if the list is empty.");
                    list.Note.ShouldNotBeNull("The List's note field must be populated if the list is empty.");
                }
            });
        }


        [Then(@"the Bundle should contain ""(.*)"" allergies")]
        public void TheBundleShouldContainAllergies(int number)
        {
            ActiveAllergyIntolerances.Count.ShouldBe(number, "An incorrect number of allergies was returned for the patient.");
        }

        [Then(@"the Bundle should contain ""(.*)"" lists")]
        public void TheBundleShouldContainLists(int number)
        {
            Lists.Count.ShouldBe(number, "An incorrect number of lists was returned with the bundle.");
        }

        [Then(@"the Bundle should contain a list with the title ""(.*)""")]
        public void TheBundleShouldContainAListWithTheTitleAndEntries(string title)
        {
            getListsWithTitle(title).ShouldHaveSingleItem();
        }

        [Then(@"the Bundle should contain the correct number of allergies")]
        public void TheBundleShouldContainTheCorrectNumberOfAllergies()
        {
            List active = Lists.Where(list => list.Title.Equals(FhirConst.ListTitles.kActiveAllergies)).ToList().First();

            ActiveAllergyIntolerances.Count.ShouldBe(active.Entry.Count);
        }

        [Then(@"the Bundle should not contain a list with the title ""(.*)""")]
        public void TheBundleShouldNotContainAListWithTheTitle(string title)
        {
            getListsWithTitle(title).ShouldBeEmpty();
        }

        private List<List> getListsWithTitle(string title)
        {
            return Lists
                    .Where(list => list.Title.Equals(title))
                    .ToList();
        }

        [Then(@"the Lists are valid for a patient with no allergies")]
        public void TheListsAreValidForAPatientWithNoAllergies()
        {
            Lists.ForEach(list =>
           {
               list.Entry.ShouldBeEmpty();
               list.Note.ShouldNotBeNull();
               list.Note.ShouldHaveSingleItem();
               list.Note.First().Text.ShouldBe("Information not available");
               list.EmptyReason.ShouldNotBeNull();
               list.EmptyReason.Coding.Count.ShouldBe(1);
               list.EmptyReason.Coding.First().System.ShouldBe(FhirConst.StructureDefinitionSystems.kSpecial);
               list.EmptyReason.Coding.First().Code.ShouldBe("nocontentrecorded");
               list.EmptyReason.Coding.First().Display.ShouldBe("No content recorded");
           });
        }

        [Then(@"the Lists are valid for a patient with explicit no allergies coding")]
        public void TheListsAreValidForAPatientWithExplicitNoAllergiesCoding()
        {
            Lists.ForEach(list =>
            {
                if (list.Title.Equals(FhirConst.ListTitles.kActiveAllergies))
                {
                    ActiveAllergyIntolerances.Count.Equals(1);
                    ActiveAllergyIntolerances.ForEach(allergy =>
                    {
                        allergy.Code.Coding.First().Code.ShouldNotBeNull();
                    });
                }
            });
        }

        [Then(@"the Lists are valid for a patient with allergies")]
        public void TheListsAreValidForAPatientWithAllergies()
        {
            Lists.ForEach(list =>
            {
                list.EmptyReason.ShouldBeNull();
                list.Note.ForEach(note =>
                {
                    note.Text.ShouldNotContain("no known allergies");
                });
            });
        }

        #endregion

        #region Allergy Intolerance Validity Checks

        [Then(@"the AllergyIntolerance should be valid")]
        public void TheAllergyIntoleranceShouldBeValid()
        {
            AllAllergyIntolerances.AddRange(ActiveAllergyIntolerances);

            //Get the 'contained' resolved allergies from the resolved list
            List<List> resolved = Lists.Where(list => list.Title.Equals(FhirConst.ListTitles.kResolvedAllergies)).ToList();
            if (resolved.Count > 0)
            {
                List<Resource> resolvedAllergies = resolved.First().Contained.Where(resource => resource.ResourceType.Equals(ResourceType.AllergyIntolerance)).ToList();
                resolvedAllergies.ForEach(resource =>
                {
                    AllergyIntolerance allergy = (AllergyIntolerance)resource;
                    AllAllergyIntolerances.Add(allergy);
                });
            }
            
            TheAllergyIntoleranceRecorderShouldbeValid();
            TheAllergyIntoleranceClinicalStatusShouldbeValid();
            TheAllergyIntoleranceAssertedDateShouldBeValid();
            TheAllergyIntoleranceIdShouldBeValid();
            TheAllergyIntoleranceMetadataShouldBeValid();
            TheAllergyIntolerancePatientShouldBeValidAndResolvable();
            TheAllergyIntoleranceVerificationStatusShouldbeValid();
            TheAllergyIntoleranceReactionShouldBeValid();
            TheAllergyIntoleranceEndDateShouldBeValid();
            TheAllergyIntoleranceCodeShouldbeValid();
            TheListOfAllergyIntolerancesShouldBeValid();
            TheAllergyIntoleranceCategoryShouldbeValid();
            TheAllergyIntoleranceEncounterShouldBeValid();
        }

        private void TheAllergyIntoleranceEncounterShouldBeValid()
        {
            AllAllergyIntolerances.ForEach(allergy =>
            {
                Extension encounter = allergy.GetExtension(FhirConst.StructureDefinitionSystems.kExtEncounter);
                if (encounter != null)
                {
                    ResourceReference encRef = (ResourceReference)encounter.Value;
                    encRef.Reference.StartsWith("Encounter/");
                }
            });
        }

        private void TheAllergyIntoleranceRecorderShouldbeValid()
        {
            AllAllergyIntolerances.ForEach(allergy =>
            {
                allergy.Recorder.ShouldNotBeNull("AllergyIntolerance Recorder cannot be null");
            });
        }

        private void TheAllergyIntoleranceMetadataShouldBeValid()
        {
            AllAllergyIntolerances.ForEach(allergyIntolerance =>
            {
                CheckForValidMetaDataInResource(allergyIntolerance, FhirConst.StructureDefinitionSystems.kAllergyIntolerance);
            });
        }

        private void TheAllergyIntoleranceIdShouldBeValid()
        {
            AllAllergyIntolerances.ForEach(allergy =>
            {
                allergy.Id.ShouldNotBeNullOrWhiteSpace("Id must be set");
            });
        }

        private void TheAllergyIntoleranceClinicalStatusShouldbeValid()
        {
            ActiveAllergyIntolerances.ForEach(allergy =>
            {
                allergy.ClinicalStatus.Equals(AllergyIntolerance.AllergyIntoleranceClinicalStatus.Active);
            });

            AllAllergyIntolerances.ForEach(allergy =>
            {
                allergy.ClinicalStatus.ShouldNotBeNull("AllergyIntolerance ClinicalStatus cannot be null");
                allergy.ClinicalStatus.ShouldBeOfType<AllergyIntolerance.AllergyIntoleranceClinicalStatus>($"AllergyIntolerance ClinicalStatus is not a valid value within the value set {FhirConst.ValueSetSystems.kVsAllergyIntoleranceClinicalStatus}");
                allergy.ClinicalStatus.ShouldNotBeSameAs(AllergyIntolerance.AllergyIntoleranceClinicalStatus.Inactive, "The clinical status should never be set to inactive");        
            });
        }

        private void TheAllergyIntoleranceVerificationStatusShouldbeValid()
        {
            AllAllergyIntolerances.ForEach(allergy =>
            {
                allergy.VerificationStatus.ShouldNotBeNull("AllergyIntolerance VerificationStatus cannot be null");
                allergy.VerificationStatus.ShouldBe(AllergyIntolerance.AllergyIntoleranceVerificationStatus.Unconfirmed);
            });
        }

        private void TheAllergyIntoleranceCategoryShouldbeValid()
        {
            AllAllergyIntolerances.ForEach(allergy =>
            {

                allergy.Category.ShouldNotBeNull("AllergyIntolerance Category cannot be null");
                allergy.Category.ToList().ForEach(category =>
               {
                   category.ShouldBeOfType<AllergyIntolerance.AllergyIntoleranceCategory>($"AllergyIntolerance Category is not a valid value within the value set {FhirConst.ValueSetSystems.kVsAllergyIntoleranceCategory}");
                   category.ShouldNotBeSameAs(AllergyIntolerance.AllergyIntoleranceCategory.Biologic);
                   category.ShouldNotBeSameAs(AllergyIntolerance.AllergyIntoleranceCategory.Food);
               });                
            });
        }

        private void TheAllergyIntoleranceCodeShouldbeValid()
        {
            AllAllergyIntolerances.ForEach(allergy =>
            {
               allergy.Code.Coding.ShouldNotBeNull("AllergyIntolerance Code coding cannot be null");

               allergy.Code.Coding.ForEach(coding =>
               {
                   coding.System.ShouldNotBeNull("Code should not be null");
               });
            });
        }

        private void TheAllergyIntoleranceAssertedDateShouldBeValid()
        {
            AllAllergyIntolerances.ForEach(allergy =>
            {
                allergy.AssertedDate.ShouldNotBeNullOrEmpty();
            });
        }

        private void TheAllergyIntoleranceEndDateShouldBeValid()
        {
            AllAllergyIntolerances.ForEach(allergy =>
            {
                if (allergy.ClinicalStatus.Equals(AllergyIntolerance.AllergyIntoleranceClinicalStatus.Resolved))
                {
                    Extension endAllergy = allergy.GetExtension(FhirConst.StructureDefinitionSystems.kAllergyEndExt);
                    endAllergy.ShouldNotBeNull();
                    Extension endDate = endAllergy.GetExtension("endDate");
                    endDate.ShouldNotBeNull();
                    endDate.Value.ShouldNotBeNull();
                    Extension endReason = endAllergy.GetExtension("reasonEnded");
                    endReason.ShouldNotBeNull();
                    endReason.Value.ShouldNotBeNull();
                }
            });
        }

        private void TheAllergyIntolerancePatientShouldBeValidAndResolvable()
        {
            AllAllergyIntolerances.ForEach(allergy =>
            {
               var reference = allergy.Patient.Reference;
               reference.ShouldStartWith("Patient/");

               var resource = _httpSteps.GetResourceForRelativeUrl(GpConnectInteraction.PatientRead, reference);
               resource.GetType().ShouldBe(typeof(Patient));
            });
        }

        private void TheAllergyIntoleranceReactionShouldBeValid()
        {
            AllAllergyIntolerances.ForEach(allergy =>
            {
                if (allergy.Reaction != null)
                {
                    allergy.Reaction.Count.ShouldBeLessThanOrEqualTo(1);
                    AllergyIntolerance.ReactionComponent reaction = allergy.Reaction[0];
                    if (reaction.Manifestation != null)
                    {
                        reaction.Manifestation.Count.ShouldBeLessThanOrEqualTo(1);

                        if (reaction.Manifestation.Count == 1) {
                            if (reaction.Severity != null)
                            {
                                reaction.Severity.ShouldBeOfType<AllergyIntolerance.AllergyIntoleranceSeverity>($"AllergyIntolerance Severity is not a valid value within the value set {FhirConst.ValueSetSystems.kVsAllergyIntoleranceSeverity}");

                                var codableConcept = reaction.Manifestation.First();

                                var codingDisplay = codableConcept.Coding.First().Display;

                                codingDisplay.ShouldBe("nullFlavor NI", "AllergyIntolerance.reaction.manifestation SHOULD be coded as the nullFlavor NI");

                            }
                        }     
                    }
                 
                    if (reaction.ExposureRoute != null)
                    {
                        reaction.ExposureRoute.Coding.First().System.Equals(FhirConst.CodeSystems.kCCSnomed);
                    }


                    reaction.Note.ShouldBeEmpty();
                    reaction.Onset.ShouldBeNull();
                    reaction.Substance.ShouldBeNull();
                }
            });
        }

        #endregion
    }
}
