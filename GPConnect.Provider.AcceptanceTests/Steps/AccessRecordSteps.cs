namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using Constants;
    using Context;
    using Helpers;
    using TechTalk.SpecFlow;
    using Hl7.Fhir.Model;
    using Shouldly;
    using System;
    using System.Linq;

    [Binding]
    public sealed class AccessRecordSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private Bundle Bundle => _httpContext.FhirResponse.Bundle;

        public AccessRecordSteps(HttpSteps httpSteps, HttpContext httpContext) 
            : base(httpSteps)
        {
            _httpContext = httpContext;
        }

        [Given(@"I set the Parameter name ""(.*)"" to ""(.*)""")]
        public void SetTheParameterNameTo(string parameterName, string invalidParameterName)
        {
            _httpContext.HttpRequestConfiguration.BodyParameters.GetSingle(parameterName).Name = invalidParameterName;
        }

        #region NHS Number Parameter

        [Given(@"I add an NHS Number parameter for ""(.*)""")]
        public void AddAnNhsNumberParameterFor(string patient)
        {
            var nhsNumber = GlobalContext.PatientNhsNumberMap[patient];
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetCareRecordParams.kPatientNHSNumber, NhsNumberHelper.GetDefaultIdentifier(nhsNumber));
        }

        [Given(@"I add an NHS Number parameter for an invalid NHS Number")]
        public void AddAnNhsNumberParameterForInvalidNhsNumber()
        {
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetCareRecordParams.kPatientNHSNumber, NhsNumberHelper.GetDefaultIdentifierWithInvalidNhsNumber());
        }

        [Given(@"I add an NHS Number parameter with an empty NHS Number")]
        public void AddAnNhsNumberParameterWithAnEmptyNhsNumber()
        {
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetCareRecordParams.kPatientNHSNumber, NhsNumberHelper.GetIdentifierWithEmptyNhsNumber());
        }

        [Given(@"I add an NHS Number parameter for ""(.*)"" with an invalid Identifier System")]
        public void AddAnNhsNumberParameterForWithAnInvalidIdentifierSystem(string patient)
        {
            var nhsNumber = GlobalContext.PatientNhsNumberMap[patient];
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetCareRecordParams.kPatientNHSNumber, NhsNumberHelper.GetDefaultIdentifierWithInvalidSystem(nhsNumber));
        }

        [Given(@"I add an NHS Number parameter for ""(.*)"" with an empty Identifier System")]
        public void AddAnNhsNumberParameterForWithAnEmptyIdentifierSystem(string patient)
        {
            var nhsNumber = GlobalContext.PatientNhsNumberMap[patient];
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetCareRecordParams.kPatientNHSNumber, NhsNumberHelper.GetIdentifierWithEmptySystem(nhsNumber));
        }

        [Given(@"I add an NHS Number parameter for ""(.*)"" using an invalid parameter type")]
        public void AddANhsNumberParameterForUsingAnInvalidParameterType(string patient)
        {
            var nhsNumber = GlobalContext.PatientNhsNumberMap[patient];
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetCareRecordParams.kPatientNHSNumber, new FhirString(nhsNumber));
        }

        #endregion

        #region Bundle Checks

        [Then(@"the Bundle should be valid for patient ""(.*)""")]
        public void TheBundleShouldBeValid(string patient)
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
            CheckBundleResources(patient);

        }

        private void CheckBundleResources(string patient)
        {
            Boolean hasPatient = false;
            Boolean hasOrganization = false;
            Boolean hasPractitioner = false;
            Boolean hasPractitionerRole = false;
            Bundle.GetResources().ToList().ForEach(resource =>
            {
                if (resource.ResourceType.Equals(ResourceType.Patient))
                {
                    hasPatient = true;

                    //Check the returned patient has the correct NHS number
                    Patient patientResource = (Patient)resource;
                    patientResource.Identifier.ForEach(identifier =>
                    {
                        if (identifier.System.Equals(FhirConst.IdentifierSystems.kNHSNumber))
                        {
                            identifier.Value.ShouldBe(GlobalContext.PatientNhsNumberMap[patient], "NHS number returned in patient should be the same as the one requested");
                        }
                    });
                }
                else if (resource.ResourceType.Equals(ResourceType.Organization))
                {
                    hasOrganization = true;
                }
                else if (resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    hasPractitioner = true;
                }
                else if (resource.ResourceType.Equals(ResourceType.PractitionerRole))
                {
                    hasPractitionerRole = true;
                }
            });

            hasPatient.ShouldBe(true);
            hasOrganization.ShouldBe(true);
            hasPractitioner.ShouldBe(true);
            hasPractitionerRole.ShouldBe(true);
        }

        public static void BaseListParametersAreValid(List list)
        {
            list.Id.ShouldNotBeNull("The list must have an id.");

            list.Status.ShouldNotBeNull("The List status is a mandatory field.");
            list.Status.ShouldBeOfType<List.ListStatus>("Status of allergies list is of wrong type.");
            list.Status.ShouldBe(List.ListStatus.Current, "The list's status must be set to Current.");

            list.Mode.ShouldNotBeNull("The List mode is a mandatory field.");
            list.Mode.ShouldBeOfType<ListMode>("Mode of allergies list is of wrong type.");
            list.Mode.ShouldBe(ListMode.Snapshot, "The list's mode must be set to Snapshot.");

            list.Code.ShouldNotBeNull("The List code is a mandatory field.");

            list.Subject.ShouldNotBeNull("The List subject is a mandatory field.");
            isTheListSubjectValid(list.Subject).ShouldBeTrue();

            list.Title.ShouldNotBeNull("The List title is a mandatory field.");
        }


        private static Boolean isTheListSubjectValid(ResourceReference subject)
        {
            return !(null == subject.Reference && null == subject.Identifier);
        }

        #endregion

        #region Record Section Parameter

        [Given(@"I add a Record Section parameter for ""(.*)""")]
        public void AddARecordSectionParameterFor(string recordSection)
        {
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetCareRecordParams.kRecordSection, RecordSectionHelper.GetRecordSection(recordSection));
        }

        [Given(@"I add a Record Section parameter with invalid Code")]
        public void AddARecordSectionParameterWithInvalidCode()
        {
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetCareRecordParams.kRecordSection, RecordSectionHelper.GetRecordSectionWithInvalidCode());
        }

        [Given(@"I add a Record Section parameter for ""(.*)"" with invalid System")]
        public void AddARecordSectionParameterForWithInvalidSystem(string recordSection)
        {
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetCareRecordParams.kRecordSection, RecordSectionHelper.GetRecordSectionWithInvalidSystem(recordSection));
        }

        [Given(@"I add a Record Section parameter for ""(.*)"" with empty System")]
        public void AddARecordSectionParameterForWithEmptySystem(string recordSection)
        {
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetCareRecordParams.kRecordSection, RecordSectionHelper.GetRecordSectionWithEmptySystem(recordSection));
        }

        [Given(@"I add a Record Section parameter with empty Code")]
        public void AddARecordSectionParameterWithEmptyCode()
        {
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetCareRecordParams.kRecordSection, RecordSectionHelper.GetRecordSystemWithEmptyCode());
        }

        [Given(@"I add a Record Section parameter for ""(.*)"" using an invalid parameter type")]
        public void AddARecordSectionParameterForUsingAnInvalidParameterType(string recordSection)
        {
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetCareRecordParams.kRecordSection, new FhirString(recordSection));
        }

        #endregion

        #region Time Period Parameter

        [Given(@"I add a valid Time Period parameter")]
        public void AddAValidTimePeriodParameter()
        {
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetCareRecordParams.kTimePeriod, TimePeriodHelper.GetDefaultTimePeriod());
        }

        [Given(@"I add a Time Period parameter with invalid Start Date")]
        public void AddATimePeriodParameterWithInvalidStartDate()
        {
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetCareRecordParams.kTimePeriod, TimePeriodHelper.GetTimePeriodInvalidStartDate());
        }

        [Given(@"I add a Time Period parameter with invalid End Date")]
        public void AddATimePeriodParameterWithInvalidEndDate()
        {
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetCareRecordParams.kTimePeriod, TimePeriodHelper.GetTimePeriodInvalidEndDate());
        }

        [Given(@"I add a Time Period parameter with Start Date after End Date")]
        public void AddATimePeriodParameterWithStartDateAfterEndDate()
        {
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetCareRecordParams.kTimePeriod, TimePeriodHelper.GetTimePeriodStartDateAfterEndDate());
        }

        [Given(@"I add a Time Period parameter with Start Date only")]
        public void AddATimePeriodParameterWithStartDateOnly()
        {
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetCareRecordParams.kTimePeriod, TimePeriodHelper.GetTimePeriodStartDateOnly());
        }

        [Given(@"I add a Time Period parameter with End Date only")]
        public void AddATimePeriodParameterWithEndDateOnly()
        {
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetCareRecordParams.kTimePeriod, TimePeriodHelper.GetTimePeriodEndDateOnly());
        }

        [Given(@"I add a Time Period parameter with ""(.*)"" and ""(.*)""")]
        public void AddATimePeriodParameterWithStartDateAndEndDate(string startDate, string endDate)
        {
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetCareRecordParams.kTimePeriod, TimePeriodHelper.GetTimePeriod(startDate, endDate));
        }

        [Given(@"I add a Time Period parameter with Start Date today and End Date in ""(.*)"" days")]
        public void AddATimePeriodParameterWithStartDateTodayAndEndDateInDays(int days)
        {
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetCareRecordParams.kStartDate, TimePeriodHelper.GetTimePeriodStartDateTodayEndDateDays(days));
        }

        [Given(@"I add a Time Period parameter with Start Date format ""(.*)"" and End Date format ""(.*)""")]
        public void AddATimePeriodParameterWithstartDateFormatAndEndDateFormat(string startDateFormat, string endDateFormat)
        {
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetCareRecordParams.kTimePeriod, TimePeriodHelper.GetTimePeriodStartDateFormatEndDateFormat(startDateFormat, endDateFormat, -10));
        }

        #endregion
    }
}
