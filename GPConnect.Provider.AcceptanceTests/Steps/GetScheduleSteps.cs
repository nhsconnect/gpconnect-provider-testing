namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Context;
    using Enum;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Serialization;
    using RestSharp;
    using Shouldly;
    using TechTalk.SpecFlow;
    using static Hl7.Fhir.Model.Bundle;
    using static Hl7.Fhir.Model.Slot;

    [Binding]
    public class GetScheduleSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private readonly AccessRecordSteps _accessRecordSteps;
        private readonly BundleSteps _bundleSteps;
        private readonly OrganizationSteps _organizationSteps;
        private List<Slot> Slots => _fhirContext.Slots;

        public GetScheduleSteps(FhirContext fhirContext, HttpContext httpContext, HttpSteps httpSteps, AccessRecordSteps accessRecordSteps, BundleSteps bundleSteps, OrganizationSteps organizationSteps)
            : base(fhirContext, httpSteps)
        {
            _httpContext = httpContext;
            _accessRecordSteps = accessRecordSteps;
            _bundleSteps = bundleSteps;
            _organizationSteps = organizationSteps;
        }

        [Given(@"I search for the organization ""([^""]*)"" on the providers system and save the first response to ""([^""]*)""")]
        public void GivenISearchForTheOrganizationOnTheProviderSystemAndSaveTheFirstResponseTo(string organizaitonName, string storeKey)
        {
            var relativeUrl = "Organization?identifier=http://fhir.nhs.net/Id/ods-organization-code|" + GlobalContext.OdsCodeMap[organizaitonName];
            var returnedResourceBundle = _httpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:search:organization", relativeUrl);
            returnedResourceBundle.GetType().ShouldBe(typeof(Bundle));
            ((Bundle)returnedResourceBundle).Entry.Count.ShouldBeGreaterThan(0);
            var returnedFirstResource = (Organization)((Bundle)returnedResourceBundle).Entry[0].Resource;
            returnedFirstResource.GetType().ShouldBe(typeof(Organization));
            if (_httpContext.StoredFhirResources.ContainsKey(storeKey)) _httpContext.StoredFhirResources.Remove(storeKey);
            _httpContext.StoredFhirResources.Add(storeKey, returnedFirstResource);
         
        }

        [Given(@"I add period request parameter with a start date of today and an end date ""([^""]*)"" days later")]
        public void GivenIAddPeriodRequestParameterWithAStartDateOfTodayAndAnEndDateDaysLater(double numberOfDaysRange) {
            DateTime currentDateTime = DateTime.Now;
            Period period = new Period(new FhirDateTime(currentDateTime), new FhirDateTime(currentDateTime.AddDays(numberOfDaysRange)));
            _fhirContext.FhirRequestParameters.Add("timePeriod", period);
        }

        [Given(@"I add period request parameter with only a start date")]
        public void GivenIAddPeriodRequestParameterWithOnlyAStartDate()
        {
            Period period = new Period();
            period.StartElement = FhirDateTime.Now();
            _fhirContext.FhirRequestParameters.Add("timePeriod", period);
        }

        [Given(@"I add period request parameter with only an end date")]
        public void GivenIAddPeriodRequestParameterWithOnlyAnEndDate()
        {
            Period period = new Period();
            period.EndElement = new FhirDateTime(DateTime.Now.AddDays(2));
            _fhirContext.FhirRequestParameters.Add("timePeriod", period);
        }

        [Given(@"I add period request parameter with start date ""([^""]*)"" and end date ""([^""]*)""")]
        public void GivenIAddPeriodRequestParameterWithStartDateAndEndDate(string startDate, string endDate)
        {
            Period period = new Period();
            period.Start = startDate;
            period.End = endDate;
            _fhirContext.FhirRequestParameters.Add("timePeriod", period);
        }

        [When(@"I send a gpc.getschedule operation for the organization stored as ""([^""]*)""")]
        public void ISendAGpcGetScheduleOperationForTheOrganizationStoredAs(string storeKey)
        {
            Organization organization = (Organization)_httpContext.StoredFhirResources[storeKey];
            ISendAGpcGetScheduleOperationForTheOrganizationWithLogicalId(organization.Id);
        }

        [When(@"I send a gpc.getschedule operation for the organization stored as ""([^""]*)"" to the wrong endpoint")]
        public void ISendAGpcGetScheduleOperationForTheOrganizationStoredAsToTheWrongEndpoint(string storeKey)
        {
            Organization organization = (Organization)_httpContext.StoredFhirResources[storeKey];
            string body = null;
            if (_httpContext.RequestContentType.Contains("xml"))
            {
                body = FhirSerializer.SerializeToXml(_fhirContext.FhirRequestParameters);
            }
            else
            {
                body = FhirSerializer.SerializeToJson(_fhirContext.FhirRequestParameters);
            }
            _httpSteps.RestRequest(Method.POST, "/" + organization.Id + "/$gpc.getschedule", body);
        }

        [When(@"I send a gpc.getschedule operation for the organization with locical id ""([^""]*)""")]
        public void ISendAGpcGetScheduleOperationForTheOrganizationWithLogicalId(string logicalId)
        {
            string body = null;
            if (_httpContext.RequestContentType.Contains("xml"))
            {
                body = FhirSerializer.SerializeToXml(_fhirContext.FhirRequestParameters);
            } else
            {
                body = FhirSerializer.SerializeToJson(_fhirContext.FhirRequestParameters);
            }
            _httpSteps.RestRequest(Method.POST, "/Organization/" + logicalId + "/$gpc.getschedule", body);
        }

        [Then(@"the Bundle should contain Slots")]
        public void TheBundleShouldContainSlots()
        {
            Slots.Count.ShouldBeGreaterThanOrEqualTo(1, "There should should be at least 1 Slot in the Bundle but found 0.");
        }

        [Then(@"the Slot FreeBusyType should be Free")]
        public void TheSlotFreeBusyTypeShouldBeFree()
        {
            Slots.ForEach(slot =>
            {
                slot.FreeBusyType.ShouldBe(SlotStatus.Free, $"The Slot FreeBusyType should be {SlotStatus.Free.ToString()}, but was {slot.FreeBusyType?.ToString()}");
            });
        }

        [Then(@"the Slot Metadata should be valid")]
        public void TheSlotMetadataShouldBeValid()
        {
            Slots.ForEach(slot =>
            {
                CheckForValidMetaDataInResource(slot, "http://fhir.nhs.net/StructureDefinition/gpconnect-slot-1");
            });
        }

        [Then(@"the Slot Identifiers should be valid")]
        public void ThenTheSlotResourcesCanContainAMaximumOfOneIdentifierWithAPopulatedValue()
        {
            Slots.ForEach(slot =>
            {
                slot.Identifier.Count.ShouldBeLessThanOrEqualTo(1, $"There Slot Identifiers should contain no more than 1 Identifier but found {slot.Identifier.Count}.");

                slot.Identifier.ForEach(identifier =>
                {
                    identifier.Value.ShouldNotBeNullOrEmpty($"The Slot Identifier Value should not be null or empty but was {identifier.Value}");
                });
            });
        }

        [Then(@"the schedule reference within the slots resources should be resolvable in the response bundle")]
        public void ThenTheScheduleReferenceWithinTheSlotsResourcesShouldBeResolvableInTheResponseBundle()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Slot))
                {
                    Slot slot = (Slot)entry.Resource;
                    slot.Schedule.Reference.ShouldNotBeNull("There must be a Schedule reference within all slots.");
                    slot.Schedule.Reference.ShouldNotBeEmpty("There must be a Schedule reference within all slots.");
                    _bundleSteps.ResponseBundleContainsReferenceOfType(slot.Schedule.Reference, ResourceType.Schedule);
                }
            }
        }

        [Then(@"the schedule resources in the response bundle should contain meta data information")]
        public void ThenTheScheduleResourcesInTheResponseBundleShouldContainMetaDataInformation()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Schedule))
                {
                    Schedule schedule = (Schedule)entry.Resource;
                    schedule.Meta.ShouldNotBeNull("All Schedule resources should contain MetaDate elements");
                    schedule.Meta.VersionId.ShouldNotBeNull("All Schedule resource MetaData versionId should be populated");
                    schedule.Meta.VersionId.ShouldNotBeEmpty("All Schedule resource MetaData versionId should be populated");
                    int scheduleMetaProfileCount = 0;
                    foreach (string profile in schedule.Meta.Profile)
                    {
                        profile.ShouldBe("http://fhir.nhs.net/StructureDefinition/gpconnect-schedule-1", "The Schedule MetaData profile is invalid");
                        scheduleMetaProfileCount++;
                    }
                    scheduleMetaProfileCount.ShouldBeGreaterThanOrEqualTo(1, "A Schedule Resource is missing the profile within the MetaData element.");
                }
            }
        }

        [Then(@"the schedule resources in the response bundle should contain an actor which references a location within the response bundle")]
        public void ThenTheScheduleResourcesInTheResponseBundleShouldContainAnActorWhichReferencesALocationWithinTheResponseBundle()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Schedule))
                {
                    Schedule schedule = (Schedule)entry.Resource;
                    schedule.Actor.ShouldNotBeNull();
                    schedule.Actor.Reference.ShouldNotBeNull();
                    schedule.Actor.Reference.ShouldStartWith("Location/");
                    _bundleSteps.ResponseBundleContainsReferenceOfType(schedule.Actor.Reference, ResourceType.Location);
                }
            }
        }

        [Then(@"the schedule resources can contain a single identifier but must have a value if present")]
        public void ThenTheScheduleResourcesCanContainASingleIdentifierButMustHaveAValueIfPresent()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Schedule))
                {
                    Schedule schedule = (Schedule)entry.Resource;
                    int identifierCount = 0;
                    foreach (var identifier in schedule.Identifier) {
                        identifier.Value.ShouldNotBeNull("If a schedule identifier is included it should have a value");
                        identifier.Value.ShouldNotBeEmpty();
                        identifierCount++;
                    }
                    identifierCount.ShouldBeLessThanOrEqualTo(1, "There should be a maximum of one identifier in a schedule resource.");
                }
            }
        }
        
        [Then(@"the schedule resources can contain a planningHorizon but it must contain a valid start date")]
        public void ThenTheScheduleResourcesCanContainAPlanningHorizonButItMustContainAValidStartDate()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Schedule))
                {
                    Schedule schedule = (Schedule)entry.Resource;
                    if (schedule.PlanningHorizon != null)
                    {
                        schedule.PlanningHorizon.Start.ShouldNotBeNull();
                    }
                }
            }
        }

        [Then(@"the schedule resources can contain a single type element")]
        public void ThenTheScheduleResourcesCanContainASingleTypeElement()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Schedule))
                {
                    Schedule schedule = (Schedule)entry.Resource;
                    if (schedule.Type != null)
                    {
                        schedule.Type.Count.ShouldBeLessThanOrEqualTo(1);
                    }
                }
            }
        }

        [Then(@"the schedule resources can contain extensions which references practitioner resources within the bundle")]
        public void ThenTheScheduleResourcesCanContainExtensionsWhichReferencesPractitionerResourcesWithinTheBundle()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Schedule))
                {
                    Schedule schedule = (Schedule)entry.Resource;
                    foreach (var extension in schedule.Extension) {
                        extension.Url.ShouldBe("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-practitioner-1");
                        extension.Value.ShouldNotBeNull("Practitioner reference extension must have a reference.");
                        var practitionerReference = ((ResourceReference)extension.Value).Reference;
                        practitionerReference.ShouldNotBeNull();
                        practitionerReference.ShouldStartWith("Practitioner/");
                        _bundleSteps.ResponseBundleContainsReferenceOfType(practitionerReference, ResourceType.Practitioner);
                    }
                }
            }
        }

        [Given(@"I get the Schedule for Organization Code ""([^""]*)""")]
        public void GetTheScheduleForOrganizationCode(string code)
        {
            _organizationSteps.GetTheOrganizationForOrganizationCode(code);
            _organizationSteps.StoreTheOrganization();

            _httpSteps.ConfigureRequest(GpConnectInteraction.GpcGetSchedule);

            _accessRecordSteps.AddATimePeriodParameterWithStartDateTodayAndEndDateInDays(13);

            _httpSteps.MakeRequest(GpConnectInteraction.GpcGetSchedule);
        }

        [Given(@"I store the Schedule")]
        public void StoreTheSchedule()
        {
            var schedule = _fhirContext.Bundle;

            if (schedule != null)
            {
                _httpContext.StoredBundle = schedule;
            }
        }

        [Then(@"the Schedule Bundle Metadata should be valid")]
        public void TheScheduleBundleMetadataShouldBeValid()
        {
            CheckForValidMetaDataInResource(_fhirContext.Bundle, "http://fhir.nhs.net/StructureDefinition/gpconnect-getschedule-bundle-1");
        }
    }
}
