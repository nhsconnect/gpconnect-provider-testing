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
                    foreach (var identifier in practitioner.Identifier) {
                        if (String.Equals(identifier.System, "http://fhir.nhs.net/Id/sds-user-id")) {
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
                    foreach (var identifier in practitioner.Identifier) {
                        var validSystems = new String[2] { "http://fhir.nhs.net/Id/sds-user-id", "http://fhir.nhs.net/Id/sds-role-profile-id" };
                        identifier.System.ShouldBeOneOf(validSystems);
                    }
                }
            }
        }

        [When(@"I get practitioner ""(.*)"" and use the id to make a get request to the url ""(.*)""")]
        public void WhenIGetPractitionerAndUseTheIdToMakeAGetRequestToTheURL(string practitioner, string URLpassed)
        {
            Practitioner pracValue = (Practitioner)HttpContext.StoredFhirResources[practitioner];
            string id = pracValue.Id.ToString();
            string URL = "/" + URLpassed + "/" + id;
            When($@"I make a GET request to ""{URL}""");
        }

        [When(@"I make a GET request for a practitioner using an invalid id of ""(.*)"" and url ""(.*)""")]
        public void WhenIMakeAGetRequestForAPractitionerUsingAnInvalidIdOfAndUrl(string invalidId, string url)
        {
            string URL = "/" + url + "/" + invalidId;
            When($@"I make a GET request to ""{URL}""");
        }


        [Given(@"I find practitioner ""(.*)"" and save it with the key ""(.*)""")]
        public void GivenIFindPractitionerAndSaveItWithTheKey(string practitionerName, string practitionerId)
        {
            string interactionId = "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner";
            string system = "http://fhir.nhs.net/Id/sds-user-id";
            string URL = "/Practitioner";

            Given("I am using the default server");
            Given($@"I am performing the ""{interactionId}"" interaction");
            Given($@"I add the practitioner identifier parameter with system ""{system}"" and value ""{practitionerName}""");
            When($@"I make a GET request to ""{URL}""");
            Then("the response status code should indicate success");
            Then("the response body should be FHIR JSON");
            Then($@"the response should be a Bundle resource of type ""searchset""");
            Then("the response bundle should contain at least One Practitioner resource");


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

        [Then(@"the practitioner resource it should contain meta data profile and version id")]
        public void thePractitionerResourceItShouldContainMetaDataProfileAndVersionId()
        {

            Practitioner practitioner = (Practitioner)FhirContext.FhirResponseResource;
            practitioner.Meta.VersionId.ShouldNotBeNull();
            practitioner.Meta.Profile.ShouldNotBeNull();

            foreach (string profile in practitioner.Meta.Profile)
            {
                profile.ShouldBe("http://fhir.nhs.net/StructureDefinition/gpconnect-practitioner-1");
            }

        }


        [Then(@"the practitioner resource should contain a single name element")]
        public void ThenPractitionerResourcesShouldContainASingleNameElement()
        {
            Practitioner practitioner = (Practitioner)FhirContext.FhirResponseResource;
            practitioner.Name.ShouldNotBeNull();
            int count = 0;
            foreach (string family in practitioner.Name.Family)
            {
                count++;
            }
            count.ShouldBeLessThanOrEqualTo(1);

        }

        [Then(@"the practitioner resource should contain a role id equal to role id ""(.*)"" or role id ""(.*)"" or role id ""(.*)""")]
        public void ThenThePractitionerResourceShouldContainARoleIdEqualTo(string roleId, string roleId2, string roleId3)
        {
            List<String> roleIds = new List<string>();
            roleIds = addRoleIdsToList(roleIds, roleId, roleId2,roleId3);
    
            Practitioner practitioner = (Practitioner)FhirContext.FhirResponseResource;
            foreach (Identifier identifier in practitioner.Identifier)
            {
                if (identifier.System.Equals("http://fhir.nhs.net/Id/sds-role-profile-id"))
                {
                    roleIds.ShouldContain(identifier.Value.ToString());
                }
                    
            }
        }

        private List<string> addRoleIdsToList(List<string> roleIdList , string roleId, string roleId2, string roleId3)
        {
            if (roleId.StartsWith("PT"))
                {
                roleIdList.Add(roleId);
                }
            if (roleId2.StartsWith("PT"))
            {
                roleIdList.Add(roleId2);
            }
            if (roleId3.StartsWith("PT"))
            {
                roleIdList.Add(roleId3);
            }

            return roleIdList;
        }

        [Then(@"if the practitioner resource contains an identifier it is valid")]
        public void ThenIfThePractitionerResourcesContainAnIdentifierItIsValid()
        {
            Practitioner practitioner = (Practitioner)FhirContext.FhirResponseResource;
            foreach (Identifier identifier in practitioner.Identifier)
            {
                identifier.System.ShouldNotBeNullOrEmpty();
                var validSystems = new string[2] { "http://fhir.nhs.net/Id/sds-role-profile-id", "http://fhir.nhs.net/Id/sds-user-id" };
                identifier.System.ShouldBeOneOf(validSystems, "The identifier System can only be one of the valid value");
                identifier.Value.ShouldNotBeNullOrEmpty();
            }
        }

        [Then(@"the identifier used to search for ""(.*)"" is the same as the identifier returned in practitioner read")]
        public void ThenTheIdentifierUsedToSearchForIsTheSameAsTheIdentifierReturnedInPractitionerRead(string practitionerKey)
        {
            string practitionerIdentifier = FhirContext.FhirPractitioners[practitionerKey];
            Practitioner practitioner = (Practitioner)FhirContext.FhirResponseResource;
            foreach (Identifier identifier in practitioner.Identifier)
            { 
                identifier.Value.ShouldBe(practitionerIdentifier);
            }
        }
        
        [Then(@"if the practitioner resource contains a practitioner role it has a valid coding and system")]
        public void ThenIfThePractitionerResourceContainsAPractitionerRoleItHasAValidCodingAndSystem()
        {

            Practitioner practitioner = (Practitioner)FhirContext.FhirResponseResource;
            foreach (Practitioner.PractitionerRoleComponent practitionerRole in practitioner.PractitionerRole)
            {
                if (practitionerRole.Role != null && practitionerRole.Role.Coding != null)
                {
                    var codingCount = 0;
                    foreach (Coding coding in practitionerRole.Role.Coding)
                    {
                        codingCount++;
                        coding.System.ShouldBe("http://fhir.nhs.net/ValueSet/sds-job-role-name-1");
                        coding.Code.ShouldNotBeNull();
                        coding.Display.ShouldNotBeNull();
                    }
                    codingCount.ShouldBeLessThanOrEqualTo(1);
                }
            }
        }


        [Then(@"the single practitioner resource should not contain unwanted fields")]
        public void ThenTheSinglePractitionerResourceShouldNotContainFhirFieldsPhotoOrQualification()
        {

            Practitioner practitioner = (Practitioner)FhirContext.FhirResponseResource;

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

        [Then(@"the returned practitioner resource contains a communication element")]
        public void ThenTheReturnedPractitionerResourceContainsACommunicationElement()
        {
            Practitioner practitioner = (Practitioner)FhirContext.FhirResponseResource;


            foreach (CodeableConcept codeableConcept in practitioner.Communication)
            {
                shouldBeSingleCodingWhichIsInValuest(GlobalContext.FhirHumanLanguageValueSet, codeableConcept.Coding);
            }
        }

        [When(@"I perform a vread for practitioner ""([^""]*)""")]
        public void WhenIPerformAVreadForPractitioner(string practitionerKey)
        {
            Practitioner practioner = (Practitioner)HttpContext.StoredFhirResources[practitionerKey];
            When($@"I make a GET request to ""/Practitioner/{practioner.Id}/_history/{practioner.Meta.VersionId}""");

        }

        [When(@"I perform an practitioner vread with version id ""([^""]*)"" for practitioner stored against key ""([^""]*)""")]
        public void WhenIPerformAnPractitionerVReadWithVersionForPractitionerStoredAgainstKey(string version, string practitionerKey)
        {
            Practitioner practioner = (Practitioner)HttpContext.StoredFhirResources[practitionerKey];
            When($@"I make a GET request to ""/Practitioner/{practioner.Id}/_history/{version}""");
        } 


        public void shouldBeSingleCodingWhichIsInValuest(ValueSet valueSet, List<Coding> codingList)
        {
            var codingCount = 0;
            foreach (Coding coding in codingList)
            {
                codingCount++;
                valueSetContainsCodeAndDisplay(valueSet, coding);
            }
            codingCount.ShouldBeLessThanOrEqualTo(1);
        }
        public void valueSetContainsCodeAndDisplay(ValueSet valueset, Coding coding)
        {
            coding.System.ShouldBe(valueset.CodeSystem.System);
            // Loop through valid codes to find if the one in the resource is valid
            var pass = false;
            foreach (ValueSet.ConceptDefinitionComponent valueSetConcept in valueset.CodeSystem.Concept)
            {
                if (valueSetConcept.Code.Equals(coding.Code) && valueSetConcept.Display.Equals(coding.Display))
                {
                    pass = true;
                }
            }
            pass.ShouldBeTrue();
        }
    }
}
