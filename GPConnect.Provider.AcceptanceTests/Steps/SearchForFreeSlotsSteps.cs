using System.Linq;
using GPConnect.Provider.AcceptanceTests.Constants;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System;
    using System.Collections.Generic;
    using Context;
    using Enum;
    using Hl7.Fhir.Model;
    using Repository;
    using Shouldly;
    using TechTalk.SpecFlow;
    using static Hl7.Fhir.Model.Slot;

    [Binding]
    public class SearchForFreeSlotsSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private readonly BundleSteps _bundleSteps;
        private readonly JwtSteps _jwtSteps;
        private readonly HttpRequestConfigurationSteps _httpRequestConfigurationSteps;
        private readonly IFhirResourceRepository _fhirResourceRepository;

        private List<Slot> Slots => _httpContext.FhirResponse.Slots;
        private List<Schedule> Schedules => _httpContext.FhirResponse.Schedules;

        public SearchForFreeSlotsSteps(HttpContext httpContext, HttpSteps httpSteps, BundleSteps bundleSteps, JwtSteps jwtSteps, HttpRequestConfigurationSteps httpRequestConfigurationSteps, IFhirResourceRepository fhirResourceRepository)
            : base(httpSteps)
        {
            _httpContext = httpContext;
            _bundleSteps = bundleSteps;
            _jwtSteps = jwtSteps;
            _httpRequestConfigurationSteps = httpRequestConfigurationSteps;
            _fhirResourceRepository = fhirResourceRepository;
        }

        [Given(@"I get Available Free Slots")]
        public void GetAvailableFreeSlots()
        {
            _httpSteps.ConfigureRequest(GpConnectInteraction.SearchForFreeSlots);

            _jwtSteps.SetTheJwtRequestedScopeToOrganizationRead();
            SetRequiredParametersWithOrgTypeAndATimePeriod(14, true);
            
            _httpSteps.MakeRequest(GpConnectInteraction.SearchForFreeSlots);
        }

        [Given(@"I get Available Free Slots for Today")]
        public void GetAvailableFreeSlotsForToday()
        {
            _httpSteps.ConfigureRequest(GpConnectInteraction.SearchForFreeSlots);

            _jwtSteps.SetTheJwtRequestedScopeToOrganizationRead();
            SetRequiredParametersWithOrgTypeAndATimePeriod(0, false);

            _httpSteps.MakeRequest(GpConnectInteraction.SearchForFreeSlots);
        }

        [Given(@"I get Available Free Slots without organisation type")]
        public void GetAvailableFreeSlotsWithoutOrgType()
        {
            _httpSteps.ConfigureRequest(GpConnectInteraction.SearchForFreeSlots);

            _jwtSteps.SetTheJwtRequestedScopeToOrganizationRead();
            SetRequiredParametersWithOrgTypeAndATimePeriod(14, false);
            
            _httpSteps.MakeRequest(GpConnectInteraction.SearchForFreeSlots);
        }

        [Given(@"I store the Free Slots Bundle")]
        public void StoreTheFreeSlotsBundle()
        {
            var bundle = _httpContext.FhirResponse.Bundle;
            if (bundle != null)
            {
                _fhirResourceRepository.Bundle = bundle;
            } else
            {
                bundle.ShouldNotBeNull("The bundle returned by the free slots search was null.");
            }
        }

        [Given(@"I set the required parameters with a time period of ""(.*)"" days")]
        public void SetRequiredParametersWithTimePeriod(int days)
        {
            _httpRequestConfigurationSteps.GivenIAddTheTimePeriodParametersforDaysStartingTomorrowWithStartEndPrefix(days,"ge","le");
            _httpRequestConfigurationSteps.GivenIAddTheParameterWithTheValue("status", "free");
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("searchFilter", "https://fhir.nhs.uk/Id/ods-organization-code" + '|' + GlobalContext.OdsCodeMap["ORG1"]);
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("searchFilter", "https://fhir.nhs.uk/STU3/CodeSystem/GPConnect-OrganisationType-1" + '|' + "urgent-care");
            _httpRequestConfigurationSteps.GivenIAddTheParameterWithTheValue("_include", "Slot:schedule");
        }
// Added 1.2.1 RMB 5/10/2018
        [Given(@"I set the required parameters with org type and a time period of ""(.*)"" days")]
        public void SetRequiredParametersWithOrgTypeAndATimePeriod(int days, Boolean orgType)
        {
            _httpRequestConfigurationSteps.GivenIAddTheTimePeriodParametersforDaysStartingTomorrowWithStartEndPrefix(days, "ge", "le");
            _httpRequestConfigurationSteps.GivenIAddTheParameterWithTheValue("status", "free");
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("searchFilter", "https://fhir.nhs.uk/Id/ods-organization-code" + '|' + GlobalContext.OdsCodeMap["ORG1"]);
            if (orgType)
            {
                _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("searchFilter", "https://fhir.nhs.uk/STU3/CodeSystem/GPConnect-OrganisationType-1" + '|' + "urgent-care");
            }
            
            _httpRequestConfigurationSteps.GivenIAddTheParameterWithTheValue("_include", "Slot:schedule");
        }
        [Given(@"I add a single searchFilter paramater with value equal to ""(.*)""")]
        public void ISetASingleTheSearchFilterParameterTo(string invalidValue)
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("searchFilter", "https://fhir.nhs.uk/Id/ods-organization-code" + '|' + GlobalContext.OdsCodeMap["ORG1"]);

        }

        [Given(@"I add two searchFilter paramaters with values equal to ""(.*)"" and ""(.*)""")]
        public void IAddTwoSearchFilterParametersWithValuesEqualToAnd(string firstInvalidValue, string secondInvalidValue)
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("searchFilter", "https://fhir.nhs.uk/Id/ods-organization-code" + '|' + GlobalContext.OdsCodeMap["OG1"]);
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("searchFilter", "https://fhir.nhs.uk/STU3/CodeSystem/GPConnect-OrganisationType-1" + '|' + "");

        }

        [Given(@"I add two valid searchFilter paramaters")]
        public void IAddTwoValidSearchFilterParamaters()
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("searchFilter", "https://fhir.nhs.uk/Id/ods-organization-code" + '|' + GlobalContext.OdsCodeMap["ORG1"]);
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("searchFilter", "https://fhir.nhs.uk/STU3/CodeSystem/GPConnect-OrganisationType-1" + '|' + "urgent-care");
            
        }

        [Given(@"I add org code searchFilter paramaters")]
        public void IAddOrgCodeSearchFilterParamaters()
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("searchFilter", "https://fhir.nhs.uk/Id/ods-organization-code" + '|' + GlobalContext.OdsCodeMap["ORG1"]);

        }

        [Given(@"I add org type searchFilter paramaters")]
        public void IAddOrgTypeSearchFilterParamaters()
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("searchFilter", "https://fhir.nhs.uk/STU3/CodeSystem/GPConnect-OrganisationType-1" + '|' + "urgent-care");

        }

        [Given(@"I add two additional non GP Connect specific searchFilter paramaters")]
        public void IAddTwoAdditionalNonGPConnectSpecificSearchFilterParamaters()
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("searchFilter", "FilterCategory-7" + '|' + "OtherConsumerCategory");
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("searchFilter", "https://fhir.nhs.uk/Id/otherIdentifier" + '|' + "otherID");
        }

        [Given(@"I add one additional non GP Connect specific searchFilter paramaters")]
        public void IAddOneAdditionalNonGPConnectSpecificSearchFilterParamaters()
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("searchFilter", "TypeCategory" + '|' + "OtherTypeCategory");
        }

        [Given(@"I add three valid searchFilter paramaters")]
        public void IAddThreeValidSearchFilterParamaters()
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("searchFilter", "https://fhir.nhs.uk/Id/ods-organization-code" + '|' + GlobalContext.OdsCodeMap["ORG1"]);
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("searchFilter", "https://fhir.nhs.uk/STU3/CodeSystem/GPConnect-OrganisationType-1" + '|' + "urgent-care");
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("searchFilter", "https://fhir.nhs.uk/STU3/CodeSystem/GPConnect-OrganisationType-1" + '|' + "Other");
        }

        [Given(@"I add a invalid searchFilter paramater with system equal to ""(.*)""")]
        public void IAddInvalidSystemSearchFilterParamaters(string invalidSystem)
        {
           
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("searchFilter", invalidSystem + '|' + "urgent-care");
          
        }


        [Then(@"the Bundle should not contain resources")]
        public void TheBundleShouldNotContainResources()
        {
            Slots.Count.ShouldBe(0, "I require no slots to be returned so as to test the exclusion of other resources.");

            _httpContext.FhirResponse.Entries.Count(entry => entry.Resource != null).ShouldBe(0, "No resources should be present in the bundle when no slots are returned.");
        }

        [Then(@"the Bundle should contain Slots")]
        public void TheBundleShouldContainSlots()
        {
            Slots.Count.ShouldBeGreaterThanOrEqualTo(1, "There should should be at least 1 Slot in the Bundle but found 0.");
        }

        [Then(@"the Slot Status should be Free")]
        public void TheSlotFreeBusyTypeShouldBeFree()
        {
            Slots.ForEach(slot =>
            {
                slot.Status.ShouldBe(SlotStatus.Free, $"The Slot Status should be {SlotStatus.Free.ToString()}, but was {slot.Status?.ToString()}");
            });
        }

        [Then(@"the Slot Metadata should be valid")]
        public void TheSlotMetadataShouldBeValid()
        {
            Slots.ForEach(slot =>
            {
                CheckForValidMetaDataInResource(slot, FhirConst.StructureDefinitionSystems.kSlot);
            });
        }

        [Then("the Slot Id should be valid")]
        public void TheSlotIdShouldBeValid()
        {
            Slots.ForEach(slot =>
            {
                slot.Id.ShouldNotBeNullOrEmpty($"The Slot Id should not be null or empty.");
            });
        }

        // github ref 120
        // RMB 25/10/2018        
        [Then(@"the Slot Not In Use should be valid")]
        public void TheSlotNotInUseShouldBeValid()
        {
            Slots.ForEach(slot =>
            {
                slot.Specialty.Count.ShouldBe(0);

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
        // Added 1.1.1 RMB 4/9/2018 Delivery Channel removed from Schedule and added to Slot resource
        [Then(@"the Slot Extensions should be valid")]
        public void ThenTheSlotExtensionsshouldbevalid()
        {
            Slots.ForEach(slot =>
            {
                var deliveryChannelExtensions = slot.Extension.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kDeliveryChannel2Ext)).ToList();
                deliveryChannelExtensions.Count.ShouldBeGreaterThanOrEqualTo(0, "Incorrect number of Slot delivery Channel Extensions have been returned. This should be 1 or more.");
            });
        }

        [Then(@"the Slot Schedule should be referenced in the Bundle")]
        public void TheSlotScheduleShouldBeReferencedInTheBundle()
        {
            Slots.ForEach(slot =>
            {
                slot.Schedule.ShouldNotBeNull("The Slot Schedule should not be null");
                slot.Schedule.Reference.ShouldNotBeNullOrEmpty($"The Slot Schedule Reference should not be null or empty but was {slot.Schedule.Reference}");

                _bundleSteps.ResponseBundleContainsReferenceOfType(slot.Schedule.Reference, ResourceType.Schedule);
            });
        }

        [Then(@"the Schedule Metadata should be valid")]
        public void TheScheduleMetadataShouldBeValid()
        {
            Schedules.ForEach(schedule =>
            {
                CheckForValidMetaDataInResource(schedule, FhirConst.StructureDefinitionSystems.kSchedule);
            });
        }


        [Then(@"the Schedule Location should be referenced in the Bundle")]
        public void TheScheduleLocationShouldBeReferencedInTheBundle()
        {
            Schedules.ForEach(schedule =>
            {
                schedule.Actor.ShouldNotBeNull();
                
                var locationReference = schedule.Actor.FirstOrDefault(actor => actor.Reference.StartsWith("Location/"))?.Reference;

                locationReference.ShouldNotBeNullOrEmpty("The Schedule Actors should contain a Location Reference, but did not.");

                _bundleSteps.ResponseBundleContainsReferenceOfType(locationReference, ResourceType.Location);
            });
        }

        [Then("the Schedule Id should be valid")]
        public void TheScheduleIdShouldBeValid()
        {
            Schedules.ForEach(schedule =>
            {
                schedule.Id.ShouldNotBeNullOrEmpty($"The Schedule Id should not be null or empty.");
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
        // github ref 120
        // RMB 25/10/2018        
        [Then(@"the Schedule Not In Use should be valid")]
        public void TheScheduleNotInUseShouldBeValid()
        {
            Schedules.ForEach(schedule =>
            {
                schedule.Specialty.Count.ShouldBe(0);

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

        [Then("the Schedule ServiceType should be valid")]
        public void TheScheduleServiceTypeShouldBeValid()
        {
            Schedules.ForEach(schedule =>
            {
                schedule.ServiceType?.Count.ShouldBeLessThanOrEqualTo(1, $"The Schedule should have a maximum of 1 ServiceType but found {schedule.ServiceType?.Count}.");
            });
        }

        [Then("the Schedule Extensions should be populated and valid")]
        public void TheScheduleExtensionsShouldBePopulatedAndValid()
        {
            Schedules.ForEach(schedule =>
            {
                var practitionerRoleExtensions = schedule.Extension.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kPractitionerRoleExt)).ToList();
                practitionerRoleExtensions.Count.ShouldBe(1, "Incorrect number of practitionerRole Extensions have been returned. This should be 1.");

                // Added 1.1.1 RMB 4/9/2018 Delivery Channel removed from Schedule and added to Slot resource
                var deliveryChannelExtensions = schedule.Extension.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kDeliveryChannelExt)).ToList();
                deliveryChannelExtensions.Count.ShouldBe(0, "Incorrect number of delivery Channel Extensions have been returned. This should be 0.");

            });
        }

        [Then(@"the Bundle Metadata should be valid")]
        public void TheScheduleBundleMetadataShouldBeValid()
        {
            CheckForValidMetaDataInResource(_httpContext.FhirResponse.Bundle, FhirConst.StructureDefinitionSystems.kGpcSearchSet);
        }

        [Then(@"the excluded actor ""(.*)"" should not be present in the Bundle")]
        public void TheExcludedActorShouldNotBePresentInTheBundle(ResourceType excludedActor)
        {
            _bundleSteps.ResponseBundleDoesNotContainReferenceOfType(excludedActor);
        }
    }
}
