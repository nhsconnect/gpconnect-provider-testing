using System;
using GPConnect.Provider.AcceptanceTests.Context;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using RestSharp;
using Shouldly;
using TechTalk.SpecFlow;

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
    }
}
