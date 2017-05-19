using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Data;
using GPConnect.Provider.AcceptanceTests.Extensions;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Model;
using NUnit.Framework;
using System;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using static Hl7.Fhir.Model.Bundle;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public class PractitionerSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private readonly HttpContext HttpContext;

        // Headers Helper
        public HttpHeaderHelper Headers { get; }

        public PractitionerSteps(HttpHeaderHelper headerHelper, FhirContext fhirContext, HttpContext httpContext)
        {
            FhirContext = fhirContext;
            Headers = headerHelper;
            HttpContext = httpContext;
        }

        [Given(@"I have the test practitioner codes")]
        public void GivenIHaveTheTestPractitionerCodes()
        {
            FhirContext.FhirPractitioners.Clear();

            foreach (PractitionerCodeMap practitionerCodeMap in GlobalContext.PractitionerMapData)
            {
                Log.WriteLine("Mapped test Practitioner code {0} to {1}", practitionerCodeMap.NativePractitionerCode, practitionerCodeMap.ProviderPractitionerCode);
                FhirContext.FhirPractitioners.Add(practitionerCodeMap.NativePractitionerCode, practitionerCodeMap.ProviderPractitionerCode);
            }
        }

        [Given(@"I add the practitioner identifier parameter with system ""(.*)"" and value ""(.*)""")]
        public void GivenIAddThePractitionerIdentifierParameterWithTheSystemAndValue(string systemParameter, string valueParameter)
        {
            Given($@"I add the parameter ""identifier"" with the value ""{systemParameter + '|' + FhirContext.FhirPractitioners[valueParameter]}""");
        }
        
        [Then(@"the interactionId ""(.*)"" should be valid")]
        public void ValidInteractionId(String interactionId)
        {
            var id = SpineConst.InteractionIds.kFhirPractitioner;
            interactionId.ShouldBeSameAs(id);
        }

        [Then(@"the interactionId ""(.*)"" should be Invalid")]
        public void InValidInteractionId(String interactionId)
        {
            var id = SpineConst.InteractionIds.kFhirPractitioner;
            interactionId.ShouldNotBeSameAs(id);
        }

        [Then(@"there is a communication element")]
        public void ThenPractitionerResourcesShouldContainCommunicationElement()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    Practitioner practitioner = (Practitioner)entry.Resource;
                    practitioner.Communication.ShouldNotBeNull("Communication should not be null");
                }
            }
        }

        [Then(@"the practitioner resource should not contain unwanted fields")]
        public void ThenThePractitionerResourceShouldNotContainFhirFieldsPhotoOrQualification()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    Practitioner practitioner = (Practitioner)entry.Resource;

                    if (null != practitioner.Photo && practitioner.Photo.Count > 0)
                    {
                        Assert.Fail("Practitioner should not contain a Photo");
                    }

                    if (null != practitioner.Qualification && practitioner.Qualification.Count > 0)
                    {
                        Assert.Fail("Practitioner should not contain a Qualification");
                    }

                    if (null != practitioner.BirthDate)
                    {
                        Assert.Fail("Practitioner should not contain a BirthDate");
                    }

                    if (null != practitioner.BirthDateElement)
                    {
                        Assert.Fail("Practitioner should not contain a BirthDateElement");
                    }

                    foreach (Practitioner.PractitionerRoleComponent practitionerRole in practitioner.PractitionerRole)
                    {
                        if (null != practitionerRole.HealthcareService && practitionerRole.HealthcareService.Count > 0)
                        {
                            Assert.Fail("Practitioner role should not contain a HealthcareService");
                        }

                        if (null != practitionerRole.Location && practitionerRole.Location.Count > 0)
                        {
                            Assert.Fail("Practitioner role should not contain a Location");
                        }
                    }
                }
            }
        }

        [Then(@"the practitioner resources in the response bundle should contain no more than a single SDS User Id")]
        public void ThenThePractitionerResourceInTheResponseBundleShouldContainNoMoreThanASingleSDSUserId()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    Practitioner practitioner = (Practitioner)entry.Resource;
                    int userIdCount = 0;
                    foreach (var identifier in practitioner.Identifier){
                        if (String.Equals(identifier.System, "http://fhir.nhs.net/Id/sds-user-id")){
                            userIdCount++;
                        }
                    }
                    userIdCount.ShouldBeLessThanOrEqualTo(1);
                }
            }
        }

        [Then(@"the practitioner resources in the response bundle should only contain an SDS user id or SDS role ids")]
        public void ThenThePractitionerResourcesInTheResponseBundleShouldOnlyContainAnSDSUserIdOrSDSRoleIds()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    Practitioner practitioner = (Practitioner)entry.Resource;
                    foreach (var identifier in practitioner.Identifier){
                        var validSystems = new String[2] { "http://fhir.nhs.net/Id/sds-user-id", "http://fhir.nhs.net/Id/sds-role-profile-id" };
                        identifier.System.ShouldBeOneOf(validSystems);
                    }
                }
            }
        }

        [When(@"I get ""(.*)"" id then make a GET request to ""(.*)""")]
        public void ThenIMakeAGetRequestToURLAndSearchForPractitioner(string practitioner, string URLpassed)
        {
            Practitioner pracValue = (Practitioner)HttpContext.StoredFhirResources[practitioner];
            string id = pracValue.Id.ToString();
            string URL = URLpassed + id;
           When($@"I make a GET request to ""{URL}""");
        }

        [When(@"I make a GET request for a practitioner using an invalid id of ""(.*)""")]
        public void ThenIMakeAGetRequestForAPractitionerUsingAnInvalidIdOf(string invalidId)
        {
            string URL = "/Practitioner/" + invalidId;
            When($@"I make a GET request to ""{URL}""");
        }

        
        [Given(@"I get ""(.*)"" id and save it as ""(.*)""")]
        public void GivenIGetPracIdAndSaveItAsIdName(string practitionerName, string practitionerId)
        {
            string identifier = "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner";
            string system = "http://fhir.nhs.net/Id/sds-user-id";
            string value = FhirContext.FhirPractitioners[practitionerName];
            string URL = "/Practitioner";

            Given("I am using the default server");
            Given($@"I am performing the ""{identifier}"" interaction");
            Given($@"I add the practitioner identifier parameter with system ""{system}"" and value ""{practitionerName}""");
            When($@"I make a GET request to ""{URL}""");
            Then("the response status code should indicate success");
            Then("the response body should be FHIR JSON");

            Practitioner practitioner = new Practitioner();
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    practitioner = (Practitioner)entry.Resource;
                    
                }
            }
            HttpContext.StoredFhirResources.Add(practitionerId, practitioner);
        }

        [Then(@"the response should be an Practitioner resource")]
        public void theResponseShouldBeAnPractitionerResource()
        {
            FhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.Practitioner);
        }


    }
}
