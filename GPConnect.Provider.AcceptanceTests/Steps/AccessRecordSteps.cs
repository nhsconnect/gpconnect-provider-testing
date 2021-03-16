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
    using System.Collections.Generic;
    using GPConnect.Provider.AcceptanceTests.Cache.ValueSet;
    using GPConnect.Provider.AcceptanceTests.Logger;

    [Binding]
    public sealed class AccessRecordSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private Bundle Bundle => _httpContext.FhirResponse.Bundle;
        private List<List> Lists => _httpContext.FhirResponse.Lists;

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

        //SJD 03/10/2019 changed method so now creates an invalid parameter
        [Given(@"I add an NHS Number parameter for ""(.*)"" using an invalid parameter type")]
        public void AddANhsNumberParameterForUsingAnInvalidParameterType(string patient)
        {
            var nhsNumber = GlobalContext.PatientNhsNumberMap[patient];
            _httpContext.HttpRequestConfiguration.BodyParameters.Add("invalidNHSNumberParam", NhsNumberHelper.GetDefaultIdentifier(nhsNumber));
        }

        #endregion NHS Number Parameter

        #region Bundle Checks

        [Then(@"the Bundle should be valid for patient ""(.*)""")]
        public void TheBundleShouldBeValid(string patient)
        {
            Bundle.Id.ShouldBe(GlobalContext.SSPTraceID, "Fail : Bundle ID should match the Request Header Ssp-TraceID");
            Logger.Log.WriteLine("Info : Bundle ID matched Request SSPTraceID");

            Bundle.Meta.ShouldNotBeNull();
            Bundle.Meta.VersionId.ShouldBeNull();
            Bundle.Meta.LastUpdated.ShouldBeNull();

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

        [Then(@"The Bundle id should match the SSPTraceID")]
        public void TheBundleidshouldmatchtheSSPTraceID()
        {
            Bundle.Id.ShouldBe(GlobalContext.SSPTraceID, "Fail : Bundle ID should match the Request Header Ssp-TraceID");
            Logger.Log.WriteLine("Info : Bundle ID matched Request SSPTraceID");
        }

        private void CheckBundleResources(string patient)
        {
            List<Resource> patientResources = Bundle.GetResources().ToList()
                        .Where(resource => resource.ResourceType.Equals(ResourceType.Patient))
                        .ToList();

            patientResources.Count.ShouldBeGreaterThan(0, "Bundle must contain a Patient resource");

            patientResources.ForEach(resource =>
            {
                //Check the returned patient has the correct NHS number
                Patient patientResource = (Patient)resource;
                patientResource.Identifier.ForEach(identifier =>
                {
                    if (identifier.System.Equals(FhirConst.IdentifierSystems.kNHSNumber))
                    {
                        identifier.Value.ShouldBe(GlobalContext.PatientNhsNumberMap[patient], "NHS number returned in patient should be the same as the one requested");
                    }
                });

                checkPractitionerOrganisationResourcesAgainstPatientGp(patientResource);
            });
        }

        /*
         * Check that a practitioner resource exists for the patient's usual GP (also check role resources exist)
         */

        private void checkPractitionerOrganisationResourcesAgainstPatientGp(Patient patient)
        {
            List<Resource> practitionerResources = Bundle.GetResources().ToList()
                        .Where(resource => resource.ResourceType.Equals(ResourceType.Practitioner))
                        .ToList();

            List<Resource> organisationResources = Bundle.GetResources().ToList()
                        .Where(resource => resource.ResourceType.Equals(ResourceType.Organization))
                        .ToList();

            patient.GeneralPractitioner.ForEach(gp =>
            {
                string identifier = gp.Reference.Substring(13);
                // Added code for PractitionerRole RMB 14/08/2018
                if (gp.Reference.StartsWith("PractitionerRole"))
                {
                    //if the patient's general practitioner is a practitioner role reference then a matching practitionerRole should be returned in the bundle
                    //practitionerResources.Where(prac => prac.Id.Equals(identifier)).ToList().Count.ShouldBeGreaterThan(0);
                }
                if (gp.Reference.StartsWith("Practitioner"))
                {
                    //if the patient's general practitioner is a practitioner reference then a matching practitioner should be returned in the bundle
                    practitionerResources.Where(prac => prac.Id.Equals(identifier)).ToList().Count.ShouldBeGreaterThan(0);
                }
                else if (gp.Reference.StartsWith("Organization"))
                {
                    //if the patient's general practitioner is an organisation reference then a matching organisation should be returned in the bundle
                    checkForOrganisationResource(identifier, organisationResources);
                }
            });

            if (patient.ManagingOrganization != null)
            {
                //if the patient's managing practitioner is not null then a matching organisation should be returned in the bundle
                string manOrgId = patient.ManagingOrganization.Reference.Substring(13);
                checkForOrganisationResource(manOrgId, organisationResources);
            }
        }

        private void checkForOrganisationResource(string orgId, List<Resource> organisationResources)
        {
            organisationResources.Where(org => org.Id.Equals(orgId)).ToList().Count.ShouldBeGreaterThan(0);
        }

        /*
         * Check that a PractitionerRole resource exists for the patient's usual GP's roles
         */

        private void checkPractitionerRoleResourcesAgainstPatientGpRole(Resource practitionerResource)
        {
            Practitioner practitioner = (Practitioner)practitionerResource;

            var pracRoleIdentifiers = practitioner.Identifier
                        .Where(identifier => identifier.System.Equals(FhirConst.IdentifierSystems.kPracRoleProfile))
                        .ToList();

            List<Resource> roleResources = Bundle.GetResources().ToList()
                        .Where(resource => resource.ResourceType.Equals(ResourceType.PractitionerRole))
                        .ToList();

            pracRoleIdentifiers.ForEach(roleIdentifier =>
            {
                roleResources.Where(role => role.Id.Equals(roleIdentifier.Value)).ToList().Count.ShouldBeGreaterThan(0);
            });
        }

        public static void BaseListParametersAreValid(List list)
        {
            // git hub ref 161
            // RMB 14/1/19
            //          list.Id.ShouldNotBeNull("The list must have an id.");

            // git hub ref 166
            // RMB 16/1/19
            list.Id.ShouldBeNull("The list must NOT have an id.");
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
            TheListClinicalSettingShouldBeValid(list);

            TheListWarningCodeShouldBeValid(list);
        }

        private static void TheListClinicalSettingShouldBeValid(List list)
        {
            List<Extension> clinicalSettings = list.Extension.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kExtListClinicalSetting)).ToList();
            clinicalSettings.Count.ShouldBeLessThanOrEqualTo(1);
            if (clinicalSettings.Count == 1)
            {
                CodeableConcept clinicalSetting = (CodeableConcept)clinicalSettings.First().Value;
                clinicalSetting.Coding.Count.Equals(1);
                clinicalSetting.Coding.First().System.Equals(FhirConst.CodeSystems.kCCSnomed);
                clinicalSetting.Coding.First().Code.Equals("1060971000000108");
                clinicalSetting.Coding.First().Display.Equals("General practice service");
            }
        }

        private static void TheListWarningCodeShouldBeValid(List list)
        {
            List<Extension> warningCodes = list.Extension.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kExtListWarningCode)).ToList();
            warningCodes.Count.ShouldBeLessThanOrEqualTo(4);
            if (warningCodes.Count == 1)
            {
                // git hub ref #173
                // RMB 24/1/19
                //Coding warningCode = (Coding)warningCodes.First().Value;
                Code warningCode = (Code)warningCodes.First().Value;
                var valueSet = ValueSetCache.Get(FhirConst.ValueSetSystems.kVsWarningCode);
                ValueSetContainsCode(valueSet, warningCode);
            }
        }

        private static Boolean isTheListSubjectValid(ResourceReference subject)
        {
            return !(null == subject.Reference && null == subject.Identifier);
        }

        [Then(@"The Structured List Does Not Include Not In Use Fields")]
        public void GivenTheStructuredListDoesNotIncludeMustNotFields()
        {
            Lists.ForEach(list =>
            {
                list.Id.ShouldBeNull("List Id is Not Supposed to be Sent - Not In Use Field");
                list.Meta.VersionId.ShouldBeNull("List Meta.VersionId is Not Supposed to be Sent - Not In Use Field");
                list.Meta.LastUpdated.ShouldBeNull("List Meta.LastUpdated is Not Supposed to be Sent - Not In Use Field");
                list.Source.ShouldBeNull("List Source is Not Supposed to be Sent - Not In Use Field");
            });
        }

        [Then(@"check structured list contains a note and emptyReason when no data in section")]
        public void GivenTheStructuredListContainsANoteAndEmptyReasonWhenNoDataInSection()
        {
            Lists.ForEach(list =>
          {
              //Check Note Element
              var noteFoundFlag = false;
              var noteMatch = list.Note.Where(note => note.Text.Contains("Information not available"));

              if (noteMatch.Count() >= 1)
              {
                  noteMatch.Count().ShouldBeGreaterThanOrEqualTo(1, "Unable to Find Note : Information not available");
                  noteFoundFlag = true;
              }

              if (noteFoundFlag)
              {
                  Log.WriteLine("Found Note : Information not available");
              }
              else
              {
                  Log.WriteLine("Note with message Information not available Not Found");
                  noteFoundFlag.ShouldBeTrue("Note with message Information not available Not Found");
              }

              //Check EmptyReason
              list.EmptyReason.ShouldNotBeNull("EmptyReason should not be null for List: ");
              list.EmptyReason.Coding.Count.ShouldBe(1);
              list.EmptyReason.Coding.First().System.ShouldBe(FhirConst.StructureDefinitionSystems.kListEmptyReason);
              list.EmptyReason.Coding.First().Code.ShouldBe("no-content-recorded");
              list.EmptyReason.Coding.First().Display.ShouldBe("No Content Recorded");
          });
        }

        #endregion Bundle Checks

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

        #endregion Record Section Parameter

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

        #endregion Time Period Parameter
    }
}