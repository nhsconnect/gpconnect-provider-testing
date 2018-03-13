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
    public sealed class AccessStructuredAllergySteps : BaseSteps
    {
        private readonly HttpContext _httpContext;

        private List<AllergyIntolerance> AllergyIntolerances => _httpContext.FhirResponse.AllergyIntolerances;
        private List<List> Lists => _httpContext.FhirResponse.Lists;

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

        #region Allergy Intolerance Validity Checks

        [Then(@"the AllergyIntolerance should be valid")]
        public void TheAllergyIntoleranceShouldBeValid()
        {
            TheAllergyIntoleranceCategoryShouldbeValid();
            TheAllergyIntoleranceAssertedDateShouldBeValid();
            TheAllergyIntoleranceClinicalStatusShouldbeValid();
            TheAllergyIntoleranceCodeShouldbeValid();
            TheAllergyIntoleranceIdShouldBeValid();
            TheAllergyIntoleranceMetadataShouldBeValid();
            TheAllergyIntolerancePatientShouldBeValidAndResolvable();
            TheAllergyIntoleranceVerificationStatusShouldbeValid();
            TheAllergyIntoleranceReactionShouldBeValid();
            TheAllergyIntoleranceEndDateShouldBeValid();
        }

        [Then(@"the AllergyIntolerance Metadata should be valid")]
        public void TheAllergyIntoleranceMetadataShouldBeValid()
        {
            AllergyIntolerances.ForEach(allergyIntolerance =>
            {
                CheckForValidMetaDataInResource(allergyIntolerance, FhirConst.StructureDefinitionSystems.kAllergyIntolerance);
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
        public void TheBundleShouldContainAListWithTheTitleAndEntries(string title, int entries)
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
                if (allergy.ClinicalStatus != null)
                {
                    allergy.ClinicalStatus.ShouldNotBeNull("AllergyIntolerance ClinicalStatus cannot be null");
                    allergy.ClinicalStatus.ShouldBeOfType<AllergyIntolerance.AllergyIntoleranceClinicalStatus>($"AllergyIntolerance ClinicalStatus is not a valid value within the value set {FhirConst.ValueSetSystems.kAllergyIntoleranceClinicalStatus}");
                }
            });
        }

        [Then(@"the AllergyIntolerance verificationStatus should be valid")]
        public void TheAllergyIntoleranceVerificationStatusShouldbeValid()
        {
            AllergyIntolerances.ForEach(allergy =>
            {
                if (allergy.VerificationStatus != null)
                {
                    allergy.VerificationStatus.ShouldNotBeNull("AllergyIntolerance VerificationStatus cannot be null");
                    allergy.VerificationStatus.ShouldBe(AllergyIntolerance.AllergyIntoleranceVerificationStatus.Unconfirmed);
                }
            });
        }

        [Then(@"the AllergyIntolerance category should be valid")]
        public void TheAllergyIntoleranceCategoryShouldbeValid()
        {
            AllergyIntolerances.ForEach(allergy =>
            {
                if (allergy.Category != null)
                {
                    allergy.Category.ShouldNotBeNull("AllergyIntolerance Category cannot be null");
                    allergy.Category.ShouldBeOfType<AllergyIntolerance.AllergyIntoleranceCategory>($"AllergyIntolerance Category is not a valid value within the value set {FhirConst.ValueSetSystems.kAllergyIntoleranceCategory}");
                    allergy.Category.ShouldNotBeSameAs(AllergyIntolerance.AllergyIntoleranceCategory.Biologic);
                    allergy.Category.ShouldNotBeSameAs(AllergyIntolerance.AllergyIntoleranceCategory.Food);
                }
            });
        }

        [Then(@"the AllergyIntolerance Code should be valid")]
        public void TheAllergyIntoleranceCodeShouldbeValid()
        {
            AllergyIntolerances.ForEach(allergy =>
            {
                if (allergy.Code != null)
                {
                    allergy.Code.Coding.ShouldNotBeNull("AllergyIntolerance Code coding cannot be null");

                    var codeList = ValueSetCache.Get(FhirConst.ValueSetSystems.kAllergyIntoleranceCode).WithComposeIncludes().ToArray();
                    allergy.Code.Coding.ForEach(coding =>
                    {
                        coding.System.ShouldNotBeNull("Code should not be null");
                        coding.Code.ShouldBeOneOf(codeList.Select(c => c.Code).ToArray());
                        coding.Display.ShouldBeOneOf(codeList.Select(c => c.Display).ToArray());

                    });
                }
            });
        }

        [Then(@"the AllergyIntolerance assertedDate should be valid")]
        public void TheAllergyIntoleranceAssertedDateShouldBeValid()
        {
            AllergyIntolerances.ForEach(allergy =>
            {
                allergy.AssertedDate.ShouldBeOfType<FhirDateTime>();
            });
        }

        [Then(@"the AllergyIntolerance endDate should be valid")]
        public void TheAllergyIntoleranceEndDateShouldBeValid()
        {
            AllergyIntolerances.ForEach(allergy =>
            {
                if (allergy.ClinicalStatus.Equals(AllergyIntolerance.AllergyIntoleranceClinicalStatus.Resolved))
                {
                    var endAllergyList = allergy.Extension.Where(e => e.Url.Equals(FhirConst.StructureDefinitionSystems.kAllergyEndExt)).ToList();
                    endAllergyList.Count.ShouldBe(1);
                    endAllergyList.First().Extension.Where(e => e.Url.Equals("endDate")).First().Value.ShouldNotBeNull();
                }
            });
        }

        [Then(@"the AllergyIntolerance Patient should be valid and resolvable")]
        public void TheAllergyIntolerancePatientShouldBeValidAndResolvable()
        {
            AllergyIntolerances.ForEach(allergy =>
            {
                if (allergy.Patient != null)
                {
                    var reference = allergy.Patient.Reference;

                    reference.ShouldStartWith("Patient/");

                    var resource = _httpSteps.GetResourceForRelativeUrl(GpConnectInteraction.PatientRead, reference);

                    resource.GetType().ShouldBe(typeof(Patient));
                }
            });
        }

        [Then(@"the emptyReason code is correct for 'NoKnownAllergies'")]
        public void TheEmptyReasoncodeIsCorrectForNoKnownAllergies()
        {
            Lists.ShouldHaveSingleItem();
            Lists.First().EmptyReason.Text.ShouldBe("No Known Allergies");
            Lists.First().EmptyReason.Coding.First().Code.ShouldBe("nil-known");
        }

        [Then(@"the emptyReason code is correct for no allergies recorded")]
        public void TheEmptyReasoncodeIsCorrectForNoAllergiesRecorded()
        {
            Lists.ShouldHaveSingleItem();
            Lists.First().EmptyReason.Text.ShouldNotBe("No Known Allergies");
            Lists.First().EmptyReason.Coding.First().Code.ShouldNotBe("nil-known");
            Lists.First().Note.First().Text.ShouldBe("There are no allergies in the patient record but it has not been confirmed with the patient that they have no allergies (that is, a ‘no known allergies’ code has not been recorded).");
        }

        [Then(@"the allergyIntolerance reaction should be valid")]
        public void TheAllergyIntoleranceReactionShouldBeValid()
        {
            AllergyIntolerances.ForEach(allergy =>
            {
                allergy.Reaction.Count.ShouldBeLessThanOrEqualTo(1);
                if (allergy.Reaction != null)
                {
                    allergy.Reaction[0].Manifestation.Count.ShouldBeLessThanOrEqualTo(1);
                }
            });
        }

        #endregion
    }
}
