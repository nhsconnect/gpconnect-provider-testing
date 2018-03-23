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

        [Then(@"the Bundle should be valid")]
        public void TheBundleShouldBeValid()
        {
            Bundle.Meta.ShouldNotBeNull();
            CheckForValidMetaDataInResource(Bundle, FhirConst.StructureDefinitionSystems.kGpcStructuredRecordBundle);
            Bundle.Type.HasValue.ShouldBeTrue();
            Bundle.Type.Value.ShouldBe(Bundle.BundleType.Collection);
            Bundle.Entry.ShouldNotBeEmpty();
            Bundle.Entry.ForEach(entry =>
            {
                entry.Resource.ShouldNotBeNull();
            });
        }

        [Then(@"the List of AllergyIntolerances should be valid")]
        public void TheListOfAllergyIntolerancesShouldBeValid()
        {
            Lists.ForEach(list =>
            {
                list.Id.ShouldNotBeNull();
                //CheckForValidMetaDataInResource(list, FhirConst.StructureDefinitionSystems.kList);
                list.Status.ShouldBeOfType<List.ListStatus>("Status of allergies list is of wrong type.");
                list.Status.ShouldBe(List.ListStatus.Current);
                list.Mode.ShouldBeOfType<ListMode>("Mode of allergies list is of wrong type.");
                list.Mode.ShouldBe(ListMode.Snapshot);
                list.Code.ShouldNotBeNull();
                list.Subject.ShouldNotBeNull();
                isTheListSubjectValid(list.Subject).ShouldBeTrue();
                if (list.Entry.Count.Equals(0))
                {
                    list.EmptyReason.ShouldNotBeNull();
                    list.EmptyReason.Text.Equals("noContent");
                };
                list.Entry.ForEach(entry =>
                {
                    entry.Item.ShouldNotBeNull();
                    entry.Item.Display.ShouldStartWith("AllergyIntolerance");
                });
            });
        }

        private Boolean isTheListSubjectValid(ResourceReference subject)
        { 
            return !(subject.Reference.Equals(null) && subject.Identifier.Equals(null));
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

        [Then(@"the Lists are valid for a patient with no allergies but no explicit recording of No Known Allergies")]
        public void TheListsAreValidForAPatientWithNoAllergies()
        {
            Lists.ForEach(list =>
           {
               list.EmptyReason.ShouldBeNull();
               list.Note.ShouldNotBeNull();
               list.Note.ShouldHaveSingleItem();
               list.Note.First().Text.ShouldMatch("There are no allergies in the patient record but it has not been confirmed with the patient that they have no allergies (that is, a ‘no known allergies’ code has not been recorded).");
               list.Entry.ShouldBeEmpty();
           });
        }

        #endregion

        #region Allergy Intolerance Validity Checks

        [Then(@"the AllergyIntolerance should be valid")]
        public void TheAllergyIntoleranceShouldBeValid()
        {
            TheAllergyIntoleranceCategoryShouldbeValid();
            TheAllergyIntoleranceAssertedDateShouldBeValid();
            TheAllergyIntoleranceClinicalStatusShouldbeValid();
            TheAllergyIntoleranceIdShouldBeValid();
            TheAllergyIntoleranceMetadataShouldBeValid();
            TheAllergyIntolerancePatientShouldBeValidAndResolvable();
            TheAllergyIntoleranceVerificationStatusShouldbeValid();
            TheAllergyIntoleranceReactionShouldBeValid();
            TheAllergyIntoleranceEndDateShouldBeValid();
            TheAllergyIntoleranceCodeShouldbeValid();
            TheAllergyIntoleranceOnsetDateTimeShouldBeValid();
            TheListOfAllergyIntolerancesShouldBeValid();
        }

        [Then(@"the AllergyIntolerance onsetDateTime should be valid")]
        public void TheAllergyIntoleranceOnsetDateTimeShouldBeValid()
        {
            AllergyIntolerances.ForEach(allergyIntolerance =>
            {
                allergyIntolerance.Onset.ShouldNotBeNull();
                allergyIntolerance.Onset.TypeName.ShouldBe("dateTime");
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
                allergy.ClinicalStatus.ShouldBeOfType<AllergyIntolerance.AllergyIntoleranceClinicalStatus>($"AllergyIntolerance ClinicalStatus is not a valid value within the value set {FhirConst.ValueSetSystems.kAllergyIntoleranceClinicalStatus}");
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
                allergy.Category.ShouldBeOfType<AllergyIntolerance.AllergyIntoleranceCategory>($"AllergyIntolerance Category is not a valid value within the value set {FhirConst.ValueSetSystems.kAllergyIntoleranceCategory}");
                allergy.Category.ShouldNotBeSameAs(AllergyIntolerance.AllergyIntoleranceCategory.Biologic);
                allergy.Category.ShouldNotBeSameAs(AllergyIntolerance.AllergyIntoleranceCategory.Food);
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
                   coding.System.Equals(FhirConst.ValueSetSystems.kAllergyIntoleranceCode);
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
                    var endAllergyList = allergy.Extension.Where(e => e.Url.Equals(FhirConst.StructureDefinitionSystems.kAllergyEndExt)).ToList();
                    endAllergyList.ShouldHaveSingleItem();
                    endAllergyList.First().Extension.Where(e => e.Url.Equals("endDate")).First().Value.ShouldNotBeNull();
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
                allergy.Reaction.Count.ShouldBeLessThanOrEqualTo(1);
                if (allergy.Reaction != null)
                {
                    allergy.Reaction[0].Manifestation.Count.ShouldBeLessThanOrEqualTo(1);
                    if (allergy.Reaction[0].Severity != null)
                    {
                        allergy.Reaction[0].Severity.ShouldBeOfType<AllergyIntolerance.AllergyIntoleranceSeverity>($"AllergyIntolerance Severity is not a valid value within the value set {FhirConst.ValueSetSystems.kAllergyIntoleranceSeverity}");
                    }
                    if(allergy.Reaction[0].ExposureRoute != null)
                    {
                        allergy.Reaction[0].ExposureRoute.Coding.First().System.Equals(FhirConst.ValueSetSystems.kAllergyIntoleranceExposure); 
                    }
                }
            });
        }

        #endregion
    }
}
