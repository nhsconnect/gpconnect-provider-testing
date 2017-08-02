namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using Context;
    using Enum;
    using Hl7.Fhir.Model;
    using Repository;
    using Shouldly;
    using TechTalk.SpecFlow;
    using static Hl7.Fhir.Model.Slot;

    [Binding]
    public class GetScheduleSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private readonly AccessRecordSteps _accessRecordSteps;
        private readonly BundleSteps _bundleSteps;
        private readonly OrganizationSteps _organizationSteps;
        private readonly IFhirResourceRepository _fhirResourceRepository;

        private List<Slot> Slots => _httpContext.FhirResponse.Slots;
        private List<Schedule> Schedules => _httpContext.FhirResponse.Schedules;

        public GetScheduleSteps(HttpContext httpContext, HttpSteps httpSteps, AccessRecordSteps accessRecordSteps, BundleSteps bundleSteps, OrganizationSteps organizationSteps, IFhirResourceRepository fhirResourceRepository)
            : base(httpSteps)
        {
            _httpContext = httpContext;
            _accessRecordSteps = accessRecordSteps;
            _bundleSteps = bundleSteps;
            _organizationSteps = organizationSteps;
            _fhirResourceRepository = fhirResourceRepository;
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
            var schedule = _httpContext.FhirResponse.Bundle;

            if (schedule != null)
            {
                _fhirResourceRepository.Bundle = schedule;
            }
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
                    identifier.Value.ShouldNotBeNullOrEmpty($"The Slot Identifier Value should not be null or empty but was {identifier.Value}.");
                });
            });
        }

        [Then(@"the Slot Schedule should be referenced in the Bundle")]
        public void TheSlotScheduleShouldBeReferencedInTheBundle()
        {
            Slots.ForEach(slot =>
            {
                slot.Schedule?.Reference.ShouldNotBeNullOrEmpty($"The Slot Schedule Reference should not be null or empty but was {slot.Schedule?.Reference}");

                _bundleSteps.ResponseBundleContainsReferenceOfType(slot.Schedule?.Reference, ResourceType.Schedule);
            });
        }

        [Then(@"the Schedule Metadata should be valid")]
        public void TheScheduleMetadataShouldBeValid()
        {
            Schedules.ForEach(schedule =>
            {
                CheckForValidMetaDataInResource(schedule, "http://fhir.nhs.net/StructureDefinition/gpconnect-schedule-1");
            });
        }


        [Then(@"the Schedule Location should be referenced in the Bundle")]
        public void TheScheduleLocationShouldBeReferencedInTheBundle()
        {
            Schedules.ForEach(schedule =>
            {
                schedule.Actor.ShouldNotBeNull();
                schedule.Actor.Reference.ShouldNotBeNull();
                schedule.Actor.Reference.ShouldStartWith("Location/");
                _bundleSteps.ResponseBundleContainsReferenceOfType(schedule.Actor.Reference, ResourceType.Location);
            });
        }

        [Then(@"the Schedule Identifiers should be valid")]
        public void TheScheduleIdentifiersShouldBeValid()
        {
            Schedules.ForEach(schedule =>
            {
                schedule.Identifier.Count.ShouldBeLessThanOrEqualTo(1, $"The Schedule shoud have a maximum of 1 Identifier but found {schedule.Identifier.Count}.");

                schedule.Identifier.ForEach(identifier =>
                {
                    identifier.Value.ShouldNotBeNullOrEmpty($"The Schedule Identifier should not be null or empty but was {identifier.Value}.");
                });
            });
        }

        [Then("the Schedule PlanningHorizon should be valid")]
        public void TheSchedulePlanningHorizonShouldBeValid()
        {
            Schedules.ForEach(schedule =>
            {
                if (schedule.PlanningHorizon != null)
                {
                    schedule.PlanningHorizon.Start.ShouldNotBeNullOrEmpty($"The Schedule PlanningHorizon Start should not be null or empty but was {schedule.PlanningHorizon?.Start}.");
                }
            });
        }

        [Then("the Schedule Type should be valid")]
        public void TheScheduleTypeShouldBeValid()
        {
            Schedules.ForEach(schedule =>
            {
                schedule.Type?.Count.ShouldBeLessThanOrEqualTo(1, $"The Schedule should have a maximum of 1 Type but found {schedule.Type?.Count}.");
            });
        }

        [Then("the Schedule Practitioner Extensions should be valid and referenced in the Bundle")]
        public void TheSchedulePractitionerExtensionsShouldBeValidAndReferencedInTheBundle()
        {
            Schedules.ForEach(schedule =>
            {
                schedule.Extension.ForEach(extension =>
                {
                    const string url = "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-practitioner-1";
                    extension.Url.ShouldBe(url, $"The Practitioner Extension Url should be {url} but was {extension.Url}.");
                    extension.Value.ShouldNotBeNull("The Practitioner Extension Value should not be null.");

                    var reference = ((ResourceReference)extension.Value).Reference;
                    reference.ShouldNotBeNullOrEmpty($"The Practitioner Reference should not be null or empty but was {reference}.");

                    const string shouldStartWith = "Practitioner/";
                    reference.ShouldStartWith(shouldStartWith, $"The Practitioner Reference should start with {shouldStartWith} but was {reference}.");

                    _bundleSteps.ResponseBundleContainsReferenceOfType(reference, ResourceType.Practitioner);
                });
            });
        }

        [Then(@"the Schedule Bundle Metadata should be valid")]
        public void TheScheduleBundleMetadataShouldBeValid()
        {
            CheckForValidMetaDataInResource(_httpContext.FhirResponse.Bundle, "http://fhir.nhs.net/StructureDefinition/gpconnect-getschedule-bundle-1");
        }
    }
}
