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

        private List<AllergyIntolerance> AllergyIntolerances => _httpContext.FhirResponse.AllergyIntolerances;
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
            ParameterComponent param = new ParameterComponent();
            param.Name = "inlcudeInvalidAllergies";
            _httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(param);
        }

        [Given(@"I add the allergies parameter with a timePeriod")]
        public void GivenIAddTheAllergiesParameterWithATimePeriod()
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] { Tuple.Create(FhirConst.GetStructuredRecordParams.kTimePeriod, (Base)TimePeriodHelper.GetDefaultTimePeriod())};
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kAllergies, tuples);
        }

        [Given(@"I add the allergies parameter with a start date")]
        public void GivenIAddTheAllergiesParameterWithAStartPeriod()
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] { Tuple.Create(FhirConst.GetStructuredRecordParams.kTimePeriod, (Base)TimePeriodHelper.GetTimePeriodStartDateOnly()) };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kAllergies, tuples);
        }

        [Given(@"I add the allergies parameter with an end date")]
        public void GivenIAddTheAllergiesParameterWithAnEndPeriod()
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] { Tuple.Create(FhirConst.GetStructuredRecordParams.kTimePeriod, (Base)TimePeriodHelper.GetTimePeriodEndDateOnly()) };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kAllergies, tuples);
        }

        #endregion

        #region List and Bundle Validity Checks

        [Then(@"the List of AllergyIntolerances should be valid")]
        public void TheListOfAllergyIntolerancesShouldBeValid()
        {
            Lists.ForEach(list =>
            {
                StructuredRecordBaseSteps.BaseListParametersAreValid(list);

                //Alergy specific checks
                CheckForValidMetaDataInResource(list, FhirConst.StructureDefinitionSystems.kList);

                if(list.Title.Equals("Active Allergies"))
                {
                    list.Code.Equals("886921000000105");

                } else if (list.Title.Equals("Resolved Allergies"))
                {
                    list.Code.Equals("TBD");
                }

                if (list.Entry.Count > 0)
                {
                    list.Entry.ForEach(entry =>
                    {
                        entry.Item.ShouldNotBeNull("The item field must be populated for eac list entry.");
                        entry.Item.Reference.ShouldStartWith("AllergyIntolerance");
                    });
                }

                if (list.Entry.Count == 0)
                {
                    list.EmptyReason.ShouldNotBeNull("The List's empty reason field must be populated if the list is empty.");
                    list.EmptyReason.Text.Equals("Information not available");
                    list.Note.ShouldNotBeNull("The List's note field must be populated if the list is empty.");
                }
            });
        }


        [Then(@"the Bundle should contain ""(.*)"" allergies")]
        public void TheBundleShouldContainAllergies(int number)
        {
            AllergyIntolerances.Count.ShouldBe(number, "An incorrect number of allergies was returned for the patient.");
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
            int numberOfEntries = 0;
            Lists.ForEach(list =>
            {
                numberOfEntries = numberOfEntries + list.Entry.Count();
            });

            AllergyIntolerances.Count.ShouldBe(numberOfEntries);
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
               if (null == list.EmptyReason || list.EmptyReason.Coding.Count == 0)
               {
                   list.Note.ShouldNotBeNull();
                   list.Note.ShouldHaveSingleItem();
                   list.Note.First().Text.ShouldBe("There are no allergies in the patient record but it has not been confirmed with the patient that they have no allergies (that is, a ‘no known allergies’ code has not been recorded).");
                   list.Entry.ShouldBeEmpty();
                   list.EmptyReason.Text.ShouldBe("Information not available");
               }
               else
               {
                   list.EmptyReason.Coding.First().System.ShouldBe(FhirConst.StructureDefinitionSystems.kSpecial);
                   list.EmptyReason.Coding.First().Code.ShouldBe("nil-known");
                   list.EmptyReason.Text.ShouldBe("No Known Allergies");
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
        }

        

        [Then(@"the AllergyIntolerance Recorder should be valid")]
        public void TheAllergyIntoleranceRecorderShouldbeValid()
        {
            AllergyIntolerances.ForEach(allergy =>
            {
                allergy.Recorder.ShouldNotBeNull("AllergyIntolerance Recorder cannot be null");
            });
        }

        [Then(@"the AllergyIntolerance Metadata should be valid")]
        public void TheAllergyIntoleranceMetadataShouldBeValid()
        {
            AllergyIntolerances.ForEach(allergyIntolerance =>
            {
                CheckForValidMetaDataInResource(allergyIntolerance, FhirConst.StructureDefinitionSystems.kAllergyIntolerance);
            });
        }

        [Then(@"the AllergyIntolerance Id should be valid")]
        public void TheAllergyIntoleranceIdShouldBeValid()
        {
            AllergyIntolerances.ForEach(allergy =>
            {
                allergy.Id.ShouldNotBeNullOrWhiteSpace("Id must be set");
            });
        }

        [Then(@"the AllergyIntolerance clinicalStatus should be valid")]
        public void TheAllergyIntoleranceClinicalStatusShouldbeValid()
        {
            AllergyIntolerances.ForEach(allergy =>
            {
                allergy.ClinicalStatus.ShouldNotBeNull("AllergyIntolerance ClinicalStatus cannot be null");
                allergy.ClinicalStatus.ShouldBeOfType<AllergyIntolerance.AllergyIntoleranceClinicalStatus>($"AllergyIntolerance ClinicalStatus is not a valid value within the value set {FhirConst.ValueSetSystems.kVsAllergyIntoleranceClinicalStatus}");
                allergy.ClinicalStatus.ShouldNotBeSameAs(AllergyIntolerance.AllergyIntoleranceClinicalStatus.Inactive, "The clinical status should never be set to inactive");
                       
            });
        }

        [Then(@"the AllergyIntolerance verificationStatus should be valid")]
        public void TheAllergyIntoleranceVerificationStatusShouldbeValid()
        {
            AllergyIntolerances.ForEach(allergy =>
            {
                allergy.VerificationStatus.ShouldNotBeNull("AllergyIntolerance VerificationStatus cannot be null");
                allergy.VerificationStatus.ShouldBe(AllergyIntolerance.AllergyIntoleranceVerificationStatus.Unconfirmed);
            });
        }

        [Then(@"the AllergyIntolerance category should be valid")]
        public void TheAllergyIntoleranceCategoryShouldbeValid()
        {
            AllergyIntolerances.ForEach(allergy =>
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

        [Then(@"the AllergyIntolerance Code should be valid")]
        public void TheAllergyIntoleranceCodeShouldbeValid()
        {
            AllergyIntolerances.ForEach(allergy =>
            {
               allergy.Code.Coding.ShouldNotBeNull("AllergyIntolerance Code coding cannot be null");

               allergy.Code.Coding.ForEach(coding =>
               {
                   coding.System.ShouldNotBeNull("Code should not be null");
               });
            });
        }

        [Then(@"the AllergyIntolerance assertedDate should be valid")]
        public void TheAllergyIntoleranceAssertedDateShouldBeValid()
        {
            AllergyIntolerances.ForEach(allergy =>
            {
                allergy.AssertedDate.ShouldNotBeNullOrEmpty();
            });
        }

        [Then(@"the AllergyIntolerance endDate should be valid")]
        public void TheAllergyIntoleranceEndDateShouldBeValid()
        {
            AllergyIntolerances.ForEach(allergy =>
            {
                if (allergy.ClinicalStatus.Equals(AllergyIntolerance.AllergyIntoleranceClinicalStatus.Resolved))
                {
                    Extension endAllergy = allergy.GetExtension(FhirConst.StructureDefinitionSystems.kAllergyEndExt);
                    endAllergy.ShouldNotBeNull();
                    Extension endDate = endAllergy.GetExtension("endDate");
                    endDate.ShouldNotBeNull();
                    endDate.Value.ShouldNotBeNull();
                }
            });
        }

        [Then(@"the AllergyIntolerance Patient should be valid and resolvable")]
        public void TheAllergyIntolerancePatientShouldBeValidAndResolvable()
        {
            AllergyIntolerances.ForEach(allergy =>
            {
               var reference = allergy.Patient.Reference;
               reference.ShouldStartWith("Patient/");

               var resource = _httpSteps.GetResourceForRelativeUrl(GpConnectInteraction.PatientRead, reference);
               resource.GetType().ShouldBe(typeof(Patient));
            });
        }

        [Then(@"the allergyIntolerance reaction should be valid")]
        public void TheAllergyIntoleranceReactionShouldBeValid()
        {
            AllergyIntolerances.ForEach(allergy =>
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
