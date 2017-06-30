﻿using System.Linq;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using Constants;
    using Context;
    using Helpers;
    using Hl7.Fhir.Model;
    using NUnit.Framework;
    using System;
    using Shouldly;
    using System.Collections.Generic;
    using Enum;
    using TechTalk.SpecFlow;
    using static Hl7.Fhir.Model.Bundle;

    [Binding]
    public class PractitionerSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private readonly BundleSteps _bundleSteps;
        // Headers Helper
        public HttpHeaderHelper Headers { get; }

        public PractitionerSteps(HttpHeaderHelper headerHelper, FhirContext fhirContext, HttpContext httpContext,
            HttpSteps httpSteps, BundleSteps bundleSteps) : base(fhirContext, httpSteps)
        {
            _httpContext = httpContext;
            Headers = headerHelper;
            _bundleSteps = bundleSteps;
        }

        [Given(@"I add the practitioner identifier parameter with system ""(.*)"" and value ""(.*)""")]
        public void GivenIAddThePractitionerIdentifierParameterWithTheSystemAndValue(string systemParameter,
            string valueParameter)
        {
            Given(
                $@"I add the parameter ""identifier"" with the value ""{systemParameter + '|' +
                                                                        GlobalContext.PractionerCodeMap[valueParameter]}""");
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
            foreach (EntryComponent entry in ((Bundle) _fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    Practitioner practitioner = (Practitioner) entry.Resource;
                    practitioner.Communication.ShouldNotBeNull("Communication should not be null");
                }
            }
        }

        [Then(@"the practitioner resource should not contain unwanted fields")]
        public void ThenThePractitionerResourceShouldNotContainFhirFieldsPhotoOrQualification()
        {
            foreach (EntryComponent entry in ((Bundle) _fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    Practitioner practitioner = (Practitioner) entry.Resource;

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
            foreach (EntryComponent entry in ((Bundle) _fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    Practitioner practitioner = (Practitioner) entry.Resource;
                    int userIdCount = 0;
                    foreach (var identifier in practitioner.Identifier)
                    {
                        if (String.Equals(identifier.System, "http://fhir.nhs.net/Id/sds-user-id"))
                        {
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
            foreach (EntryComponent entry in ((Bundle) _fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    Practitioner practitioner = (Practitioner) entry.Resource;
                    foreach (var identifier in practitioner.Identifier)
                    {
                        var validSystems = new String[2]
                            {"http://fhir.nhs.net/Id/sds-user-id", "http://fhir.nhs.net/Id/sds-role-profile-id"};
                        identifier.System.ShouldBeOneOf(validSystems);
                    }
                }
            }
        }

        [When(@"I get practitioner ""(.*)"" and use the id to make a get request to the url ""(.*)""")]
        public void WhenIGetPractitionerAndUseTheIdToMakeAGetRequestToTheURL(string practitioner, string URLpassed)
        {
            Practitioner pracValue = (Practitioner) _httpContext.StoredFhirResources[practitioner];
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
        public void GivenIFindPractitionerAndSaveItWithTheKey(string practitionerName, string practitionerKey)
        {
            string interactionId = "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner";
            string system = "http://fhir.nhs.net/Id/sds-user-id";
            string URL = "/Practitioner";

            Given("I am using the default server");
            Given($@"I am performing the ""{interactionId}"" interaction");
            Given(
                $@"I add the practitioner identifier parameter with system ""{system}"" and value ""{practitionerName}""");
            When($@"I make a GET request to ""{URL}""");
            Then("the response status code should indicate success");
            Then("the response body should be FHIR JSON");
            Then($@"the response should be a Bundle resource of type ""searchset""");
            Then("the response bundle should contain at least One Practitioner resource");

            int practitionerCount = 0;
            Practitioner practitioner = null;
            foreach (EntryComponent entry in ((Bundle) _fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    practitionerCount++;
                    practitioner = (Practitioner) entry.Resource;
                    break;
                }
            }
            practitionerCount.ShouldBeGreaterThanOrEqualTo(1, "No Practitioner found for given practitioner ODS Code.");
            practitioner.ShouldNotBeNull("No practitioner was taken from the practitioner search response.");
            if (practitioner != null)
            {
                if (_httpContext.StoredFhirResources.ContainsKey(practitionerKey))
                    _httpContext.StoredFhirResources.Remove(practitionerKey);
                _httpContext.StoredFhirResources.Add(practitionerKey, practitioner);
            }
        }

        [Then(@"the response should be an Practitioner resource")]
        public void theResponseShouldBeAnPractitionerResource()
        {
            _fhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.Practitioner);
        }

        [Then(@"the practitioner resource it should contain meta data profile and version id")]
        public void thePractitionerResourceItShouldContainMetaDataProfileAndVersionId()
        {

            Practitioner practitioner = (Practitioner) _fhirContext.FhirResponseResource;
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
            Practitioner practitioner = (Practitioner) _fhirContext.FhirResponseResource;
            practitioner.Name.ShouldNotBeNull();
            int count = 0;
            foreach (string family in practitioner.Name.Family)
            {
                count++;
            }
            count.ShouldBeLessThanOrEqualTo(1);

        }

        [Then(
            @"the practitioner resource should contain a role id equal to role id ""(.*)"" or role id ""(.*)"" or role id ""(.*)"""
        )]
        public void ThenThePractitionerResourceShouldContainARoleIdEqualTo(string roleId, string roleId2, string roleId3)
        {
            List<String> roleIds = new List<string>();
            roleIds = addRoleIdsToList(roleIds, roleId, roleId2, roleId3);

            Practitioner practitioner = (Practitioner) _fhirContext.FhirResponseResource;
            foreach (Identifier identifier in practitioner.Identifier)
            {
                if (identifier.System.Equals("http://fhir.nhs.net/Id/sds-role-profile-id"))
                {
                    roleIds.ShouldContain(identifier.Value.ToString());
                }

            }
        }

        private List<string> addRoleIdsToList(List<string> roleIdList, string roleId, string roleId2, string roleId3)
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
            Practitioner practitioner = (Practitioner) _fhirContext.FhirResponseResource;
            foreach (Identifier identifier in practitioner.Identifier)
            {
                identifier.System.ShouldNotBeNullOrEmpty();
                var validSystems = new string[2]
                    {"http://fhir.nhs.net/Id/sds-role-profile-id", "http://fhir.nhs.net/Id/sds-user-id"};
                identifier.System.ShouldBeOneOf(validSystems, "The identifier System can only be one of the valid value");
                identifier.Value.ShouldNotBeNullOrEmpty();
            }
        }

        [Then(@"the identifier used to search for ""(.*)"" is the same as the identifier returned in practitioner read")
        ]
        public void ThenTheIdentifierUsedToSearchForIsTheSameAsTheIdentifierReturnedInPractitionerRead(
            string practitionerKey)
        {
            string practitionerIdentifier = GlobalContext.PractionerCodeMap[practitionerKey];
            Practitioner practitioner = (Practitioner) _fhirContext.FhirResponseResource;
            Boolean identifierFound = false;
            foreach (Identifier identifier in practitioner.Identifier)
            {
                if (string.Equals(identifier.Value, practitionerIdentifier))
                {
                    identifierFound = true;
                    break;
                }
            }
            identifierFound.ShouldBeTrue(
                "The practitioner identifier used to search for practitioner is not present in the practitioner read response");
        }

        [Then(@"if the practitioner resource contains a practitioner role it has a valid coding and system")]
        public void ThenIfThePractitionerResourceContainsAPractitionerRoleItHasAValidCodingAndSystem()
        {

            Practitioner practitioner = (Practitioner) _fhirContext.FhirResponseResource;
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

            Practitioner practitioner = (Practitioner) _fhirContext.FhirResponseResource;

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
            Practitioner practitioner = (Practitioner) _fhirContext.FhirResponseResource;

            foreach (CodeableConcept codeableConcept in practitioner.Communication)
            {
                shouldBeSingleCodingWhichIsInValuest(GlobalContext.FhirHumanLanguageValueSet, codeableConcept.Coding);
            }
        }

        [When(@"I perform a vread for practitioner ""([^""]*)""")]
        public void WhenIPerformAVreadForPractitioner(string practitionerKey)
        {
            Practitioner practioner = (Practitioner) _httpContext.StoredFhirResources[practitionerKey];
            When($@"I make a GET request to ""/Practitioner/{practioner.Id}/_history/{practioner.Meta.VersionId}""");

        }

        [When(
            @"I perform an practitioner vread with version id ""([^""]*)"" for practitioner stored against key ""([^""]*)"""
        )]
        public void WhenIPerformAnPractitionerVReadWithVersionForPractitionerStoredAgainstKey(string version,
            string practitionerKey)
        {
            Practitioner practioner = (Practitioner) _httpContext.StoredFhirResources[practitionerKey];
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


        //Hayden
        private List<Practitioner> GetPractitioners()
        {
            return _fhirContext.Practitioners;
        }

        [Then(@"the Practitioner Metadata should be valid")]
        public void ThePractitionerMetadataShouldBeValid()
        {
            var practitioners = GetPractitioners();

            practitioners.ForEach(practitioner =>
            {
                CheckForValidMetaDataInResource(practitioner, "http://fhir.nhs.net/StructureDefinition/gpconnect-practitioner-1");
            });
        }

        [Then(@"the Practitioner should be valid")]
        public void ThePractitionerShouldBeValid()
        {
            ThePractitionerIdentifiersShouldBeValid();
            ThePractitionerNameShouldBeValid();
            ThePractitionerPhotoAndQualificationShouldBeExcluded();
            ThePractitionerPractitionerRolesShouldBeValid();
            ThenThePractitionerCommunicationShouldBeValid();
            ThePractitionerPractitionerRolesManagingOrganizationShouldBeReferencedInTheBundle();
            //ThePractitionerNameFamilyNameShouldBeValid();
        }

        [Then(@"the Practitioner Identifiers should be valid")]
        public void ThePractitionerIdentifiersShouldBeValid()
        {
            ThePractitionerIdentifiersShouldBeFixedValues();
            ThePractitionerSdsUserIdentifierShouldBeValid();
            ThePractitionerSdsRoleProfileIdentifierShouldBeValid();
        }

        [Then(@"the Practitioner Identifiers should be fixed values")]
        public void ThePractitionerIdentifiersShouldBeFixedValues()
        {
            var practitioners = GetPractitioners();

            practitioners.ForEach(practitioner =>
            {
                practitioner.Identifier.ForEach(identifier =>
                {
                    identifier.System.ShouldBeOneOf("http://fhir.nhs.net/Id/sds-user-id", "http://fhir.nhs.net/Id/sds-role-profile-id");
                });
            });
        }

        [Then(@"the Practitioner SDS Role Profile Identifier should be valid")]
        public void ThePractitionerSdsRoleProfileIdentifierShouldBeValid()
        {
            ThePractitionerSdsRoleProfileIdentifierShouldBeValid(null);
        }

        [Then(@"the Practitioner SDS Role Profile Identifier should be valid for ""([^""]*)"" total Role Profile Identifiers")]
        public void ThePractitionerSdsRoleProfileIdentifierShouldBeValidForRoleProfileIdentifiers(int totalRoleProfileCount)
        {
            ThePractitionerSdsRoleProfileIdentifierShouldBeValid(totalRoleProfileCount);
        }

        private void ThePractitionerSdsRoleProfileIdentifierShouldBeValid(int? expectedTotalRoleProfileCount)
        {
            var practitioners = GetPractitioners();
            var actualTotalRoleProfileCount = 0;

            practitioners.ForEach(practitioner =>
            {
                var sdsRoleProfileIdentifiers = practitioner.Identifier.Where(identifier => identifier.System.Equals("http://fhir.nhs.net/Id/sds-role-profile-id")).ToList();

                if (expectedTotalRoleProfileCount != null)
                {
                    actualTotalRoleProfileCount = actualTotalRoleProfileCount + sdsRoleProfileIdentifiers.Count;
                }

                sdsRoleProfileIdentifiers.ForEach(identifier =>
                {
                    identifier.Value.ShouldNotBeNull();
                });
            });

            actualTotalRoleProfileCount.ShouldBe(expectedTotalRoleProfileCount.GetValueOrDefault());
        }
        
        [Then(@"the Practitioner SDS User Identifier should be valid for single User Identifier")]
        public void ThePractitionerSdsUserIdentifierShouldBeValidForSingleUserIdentifier()
        {
            ThePractitionerSdsUserIdentifierShouldBeValid(true);
        }

        [Then(@"the Practitioner SDS User Identifier should be valid")]
        public void ThePractitionerSdsUserIdentifierShouldBeValid()
        {
            ThePractitionerSdsUserIdentifierShouldBeValid(false);
        }

        private void ThePractitionerSdsUserIdentifierShouldBeValid(bool shouldBeSingle)
        {
            var practitioners = GetPractitioners();

            practitioners.ForEach(practitioner =>
            {
                var sdsUserIdentifiers = practitioner.Identifier.Where(identifier => identifier.System.Equals("http://fhir.nhs.net/Id/sds-user-id")).ToList();

                if (shouldBeSingle)
                {
                    sdsUserIdentifiers.Count.ShouldBe(1);
                }
                else
                {
                    sdsUserIdentifiers.Count.ShouldBeLessThanOrEqualTo(1);
                }

                sdsUserIdentifiers.ForEach(identifier =>
                {
                    identifier.Value.ShouldNotBeNull();
                });
            });
        }

        [Then(@"the Practitioner Name should be valid")]
        public void ThePractitionerNameShouldBeValid()
        {
            var practitioners = GetPractitioners();

            practitioners.ForEach(practitioner =>
            { 
                practitioner.Name.ShouldNotBeNull();
            });

            //ThePractitionerNameFamilyNameShouldBeValid();
        }

        [Then(@"the Practitioner Name FamilyName should be valid")]
        public void ThePractitionerNameFamilyNameShouldBeValid()
        {
            var practitioners = GetPractitioners();

            practitioners.ForEach(practitioner =>
            {
                practitioner.Name.Family?.Count().ShouldBeLessThanOrEqualTo(1);
            });
        }

        [Then(@"the Practitioner Photo and Qualification should be excluded")]
        public void ThePractitionerPhotoAndQualificationShouldBeExcluded()
        {
            var practitioners = GetPractitioners();

            practitioners.ForEach(practitioner =>
            {
                practitioner.Photo?.Count.ShouldBe(0);
                practitioner.Qualification?.Count.ShouldBe(0);
            });
        }

        [Then(@"the Practitioner PractitionerRoles should be valid")]
        public void ThePractitionerPractitionerRolesShouldBeValid()
        {
            ThePractitionerPractionerRolesRolesShouldBeValid();
        }

        [Then(@"the Practitioner PractitionerRoles Roles should be valid")]
        public void ThePractitionerPractionerRolesRolesShouldBeValid()
        {
            var practitioners = GetPractitioners();

            practitioners.ForEach(practitioner =>
            {

                practitioner.PractitionerRole.ForEach(practitionerRole =>
                {
                    if (practitionerRole.Role?.Coding != null)
                    {
                        practitionerRole.Role.Coding.Count.ShouldBeLessThanOrEqualTo(1);
                        practitionerRole.Role.Coding.ForEach(coding =>
                        {
                            coding.System.ShouldBe("http://fhir.nhs.net/ValueSet/sds-job-role-name-1");
                            coding.Code.ShouldNotBeNull();
                            coding.Display.ShouldNotBeNull();
                        });
                    }
                });
            });
        }

        [Then(@"the Practitioner Communication should be valid")]
        public void ThenThePractitionerCommunicationShouldBeValid()
        {
            var practitioners = GetPractitioners();

            practitioners.ForEach(practitioner =>
            {
                practitioner.Communication.ForEach(codeableConcept =>
                {
                    ShouldBeSingleCodingWhichIsInValueSet(GlobalContext.FhirHumanLanguageValueSet, codeableConcept.Coding);
                });
            });
        }

        [Then(@"the Practitioner PractitionerRoles ManagingOrganization should be referenced in the Bundle")]
        public void ThePractitionerPractitionerRolesManagingOrganizationShouldBeReferencedInTheBundle()
        {
            var practitioners = GetPractitioners();

            practitioners.ForEach(practitioner =>
            {
                practitioner.PractitionerRole.ForEach(practitionerRole =>
                {
                    if (practitionerRole.ManagingOrganization != null)
                    {
                        _bundleSteps.ResponseBundleContainsReferenceOfType(practitionerRole.ManagingOrganization.Reference, ResourceType.Organization);
                    }
                });
            });
        }

        [Then(@"the Practitioner PractitionerRoles ManagingOrganization should exist")]
        public void ThePractitionerPractitionerRolesManagingOrganizationShouldExist()
        {
            var practitioners = GetPractitioners();

            practitioners.ForEach(practitioner =>
            {
                practitioner.PractitionerRole.ForEach(practitionerRole =>
                {
                    practitionerRole.ManagingOrganization.Reference.ShouldNotBeNull();
                    practitionerRole.ManagingOrganization.Reference.ShouldStartWith("Organization/");

                    var returnedResource = _httpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:organization", practitionerRole.ManagingOrganization.Reference);

                    returnedResource.GetType().ShouldBe(typeof(Organization));
                });
            });
        }

        [Given(@"I add an Practitioner Identifier parameter with System ""([^""]*)"" and Value ""([^""]*)""")]
        public void AddAnIdentifierParameterWithSystemAndValue(string system, string value)
        {
            _httpContext.RequestParameters.AddParameter("identifier", system + '|' + GlobalContext.PractionerCodeMap[value]);
        }

        [Given(@"I get the Practitioner for Practitioner Code ""([^""]*)""")]
        public void GetTheOrganizationIdForOrganizationCode(string code)
        {
            _httpSteps.ConfigureRequest(GpConnectInteraction.PractitionerSearch);

            AddAnIdentifierParameterWithSystemAndValue("http://fhir.nhs.net/Id/sds-user-id", code);

            _httpSteps.MakeRequest(GpConnectInteraction.PractitionerSearch);
        }

        [Given(@"I store the Practitioner Id")]
        public void StoreTheOrganizationId()
        {
            var practitioner = _fhirContext.Practitioners.FirstOrDefault();
            if (practitioner != null)
                _httpContext.GetRequestId = practitioner.Id;
        }
    }
}


