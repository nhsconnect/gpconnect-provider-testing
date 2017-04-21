using System;
using GPConnect.Provider.AcceptanceTests.Context;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using RestSharp;
using Shouldly;
using TechTalk.SpecFlow;
using static Hl7.Fhir.Model.Bundle;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    class GetScheduleSteps : TechTalk.SpecFlow.Steps
    {

        private readonly FhirContext FhirContext;
        private readonly HttpContext HttpContext;
        private readonly HttpSteps HttpSteps;

        public GetScheduleSteps(FhirContext fhirContext, HttpContext httpContext, HttpSteps httpSteps)
        {
            FhirContext = fhirContext;
            HttpContext = httpContext;
            HttpSteps = httpSteps;
        }

        [Given(@"I search for the organization ""([^""]*)"" on the providers system and save the first response to ""([^""]*)""")]
        public void GivenISearchForTheOrganizationOnTheProviderSystemAndSaveTheFirstResponseTo(string organizaitonName, string storeKey)
        {
            var relativeUrl = "Organization?identifier=http://fhir.nhs.net/Id/ods-organization-code|" + FhirContext.FhirOrganizations[organizaitonName];
            var returnedResourceBundle = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:search:organization", relativeUrl);
            returnedResourceBundle.GetType().ShouldBe(typeof(Bundle));
            ((Bundle)returnedResourceBundle).Entry.Count.ShouldBeGreaterThan(0);
            var returnedFirstResource = (Organization)((Bundle)returnedResourceBundle).Entry[0].Resource;
            returnedFirstResource.GetType().ShouldBe(typeof(Organization));
            if (HttpContext.StoredFhirResources.ContainsKey(storeKey)) HttpContext.StoredFhirResources.Remove(storeKey);
            HttpContext.StoredFhirResources.Add(storeKey, returnedFirstResource);
        }

        [Given(@"I add period request parameter with a start date of todays and an end date ""([^""]*)"" days later")]
        public void GivenIAddPeriodRequestParameterWithAStartDateOfTodayAndAnEndDateDaysLater(double numberOfDaysRange) {
            DateTime currentDateTime = DateTime.Now;
            Period period = new Period(new FhirDateTime(currentDateTime), new FhirDateTime(currentDateTime.AddDays(numberOfDaysRange)));
            FhirContext.FhirRequestParameters.Add("timePeriod", period);
        }

        [Given(@"I add period request parameter with only a start date")]
        public void GivenIAddPeriodRequestParameterWithOnlyAStartDate()
        {
            Period period = new Period();
            period.StartElement = FhirDateTime.Now();
            FhirContext.FhirRequestParameters.Add("timePeriod", period);
        }

        [Given(@"I add period request parameter with only an end date")]
        public void GivenIAddPeriodRequestParameterWithOnlyAnEndDate()
        {
            Period period = new Period();
            period.EndElement = new FhirDateTime(DateTime.Now.AddDays(2));
            FhirContext.FhirRequestParameters.Add("timePeriod", period);
        }

        [Given(@"I add period request parameter with start date ""([^""]*)"" and end date ""([^""]*)""")]
        public void GivenIAddPeriodRequestParameterWithStartDateAndEndDate(string startDate, string endDate)
        {
            Period period = new Period();
            period.Start = startDate;
            period.End = endDate;
            FhirContext.FhirRequestParameters.Add("timePeriod", period);
        }

        [When(@"I send a gpc.getschedule operation for the organization stored as ""([^""]*)""")]
        public void ISendAGpcGetScheduleOperationForTheOrganizationStoredAs(string storeKey)
        {
            Organization organization = (Organization)HttpContext.StoredFhirResources[storeKey];
            ISendAGpcGetScheduleOperationForTheOrganizationWithLogicalId(organization.Id);
        }

        [When(@"I send a gpc.getschedule operation for the organization with locical id ""([^""]*)""")]
        public void ISendAGpcGetScheduleOperationForTheOrganizationWithLogicalId(string logicalId)
        {
            HttpSteps.RestRequest(Method.POST, "/Organization/" + logicalId + "/$gpc.getschedule", FhirSerializer.SerializeToJson(FhirContext.FhirRequestParameters));
        }

        [Then(@"the response bundle should include slot resources")]
        public void ThenTheResponseBundlleShouldIncludeSlotResources()
        {
            bool slotResourceFoundInResponse = false;
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Slot))
                {
                    slotResourceFoundInResponse = true;
                    break;
                }
            }
            slotResourceFoundInResponse.ShouldBeTrue("No Slots Resources were found in the response bundle.");
        }
    }
}
