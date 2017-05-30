using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Data;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using static Hl7.Fhir.Model.Bundle;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public sealed class OrganizationSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private readonly AccessRecordSteps AccessRecordSteps;
        private readonly HttpContext HttpContext;
        private readonly HttpSteps HttpSteps;

        public OrganizationSteps(FhirContext fhirContext, AccessRecordSteps accessRecordSteps, HttpContext httpContext, HttpSteps httpSteps)
        {
            FhirContext = fhirContext;
            AccessRecordSteps = accessRecordSteps;
            HttpContext = httpContext;
            HttpSteps = httpSteps;
        }

        [Given(@"I have the test ods codes")]
        public void GivenIHaveTheTestODSCodes()
        {
            FhirContext.FhirOrganizations.Clear();

            foreach (ODSCodeMap odsMap in GlobalContext.ODSCodeMapData)
            {
                Log.WriteLine("Mapped test ODS code {0} to {1}", odsMap.NativeODSCode, odsMap.ProviderODSCode);
                FhirContext.FhirOrganizations.Add(odsMap.NativeODSCode, odsMap.ProviderODSCode);
            }
        }
        
        [Given(@"I add the organization identifier parameter with system ""(.*)"" and value ""(.*)""")]
        public void GivenIAddTheIdentifierParameterWithTheSystemAndValue(string systemParameter, string valueParameter)
        {
            Given($@"I add the parameter ""identifier"" with the value ""{systemParameter + '|' + FhirContext.FhirOrganizations[valueParameter]}""");
        }

        [Then(@"the response should contain ods-([^""]*)-codes ""([^""]*)""")]
        public void ThenResponseShouldContainODSOrganizationCodesWithValues(string system, string elementValues)
        {
            List<string> referenceValueList = new List<string>();

            foreach (var element in elementValues.Split(new char[] { '|' }))
            {
                referenceValueList.Add(FhirContext.FhirOrganizations[element]);
            }

            string referenceValues = String.Join("|", referenceValueList);

            Then($@"the response bundle ""Organization"" entries should contain element ""resource.identifier[?(@.system == 'http://fhir.nhs.net/Id/ods-{system}-code')].value"" with values ""{referenceValues}""");
        }

        [Then(@"the response bundle Organization entries should only contain an ODS organization codes and ODS Site Codes")]
        public void ThenThePractitionerResourcesInTheResponseBundleShouldOnlyContainAnSDSUserIdOrSDSRoleIds()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Organization))
                {
                    Organization organization= (Organization)entry.Resource;
                    foreach (var identifier in organization.Identifier)
                    {
                        var validSystems = new String[2] { "http://fhir.nhs.net/Id/ods-organization-code", "http://fhir.nhs.net/Id/ods-site-code" };
                        identifier.System.ShouldBeOneOf(validSystems);
                    }
                }
            }
        }

        [Then(@"the response bundle Organization entries should contain system code and display if the type coding is included in the resource")]
        public void ThenTheResponseBundleOrganizationEntriesShouldContainSystemCodeAndDisplayIfTheTypeCodingIsIncludedInTheResource()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Organization))
                {
                    Organization organization = (Organization)entry.Resource;
                    if (organization.Type != null && organization.Type.Coding != null) {
                        organization.Type.Coding.Count.ShouldBeLessThanOrEqualTo(1);
                        AccessRecordSteps.shouldBeSingleCodingWhichIsInValuest(GlobalContext.FhirMaritalStatusValueSet, organization.Type.Coding);
                        }
                }
            }
        }


        [Given(@"I get organization ""(.*)"" id and save it as ""(.*)""")]
        public void GivenIGetOrganizationIdAndSaveItAs(string orgName, string savedOrg)
        {
         
            string system = "http://fhir.nhs.net/Id/ods-organization-code";
            string value = FhirContext.FhirOrganizations[orgName];
 
            Given("I am using the default server");
            Given($@"I am performing the ""urn:nhs:names:services:gpconnect:fhir:rest:search:organization"" interaction");
            Given($@"I add the organization identifier parameter with system ""{system}"" and value ""{orgName}""");
            When($@"I make a GET request to ""/Organization""");
            Then($@"the response status code should indicate success");
            Then($@"the response body should be FHIR JSON");
            Then($@"the response should be a Bundle resource of type ""searchset""");
            Then($@"the response bundle should contain ""1"" entries");

            Organization organization = new Organization();
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Organization))
                {
                    organization = (Organization)entry.Resource;

                }
            }
            HttpContext.StoredFhirResources.Add(savedOrg, organization);
        }

        [When(@"I get ""(.*)"" id then make a GET request to organization url ""(.*)""")]
        public void ThenIGetIdThenMakeAGETRequestToOrganizationUrl(string organizationName, string URL)
        {
            Organization organizationValue = (Organization)HttpContext.StoredFhirResources[organizationName];
            string fullUrl = "";

            if (organizationValue.Id == null)
            {
                fullUrl = "/" + URL + "/" + null;
                When($@"I make a GET request to ""{fullUrl}""");
            }
            else
            {
                string id = organizationValue.Id.ToString();
                fullUrl = "/" + URL + "/" + id;
                When($@"I make a GET request to ""{fullUrl}""");
            }
        }

        [When(@"I perform a vread for organizaiton ""([^""]*)""")]
        public void WhenIPerformAVreadForOrganizaiton(string storedOrganizationKey)
        {
            Organization organization = (Organization)HttpContext.StoredFhirResources[storedOrganizationKey];
            When($@"I make a GET request to ""/Organization/{organization.Id}/_history/{organization.Meta.VersionId}""");
        }

        [When(@"I perform an organization vread with version ""([^""]*)"" for organization stored against key ""([^""]*)""")]
        public void WhenIPerformAnOrganizationVreadWithVersionForOrganizationStoredAgainstKey(string version, string storedOrganizationKey)
        {
            Organization organizationValue = (Organization)HttpContext.StoredFhirResources[storedOrganizationKey];
            
            When($@"I make a GET request to ""/Organization/{organizationValue.Id}/_history/{version}""");
        }

        [Then(@"the response should be an Organization resource")]
        public void theResponseShouldBeAnOrganizationResource()
        {
            FhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.Organization);
        }

        [When(@"I make a GET request for a organization using an invalid id of ""(.*)""")]
        public void ThenIMakeAGetRequestForAOrganizationUsingAnInvalidIdOf(string invalidId)
        {
            string URL = "/Organization/" + invalidId;
            When($@"I make a GET request to ""{URL}""");
        }

        [Then(@"the organization resource it should contain meta data profile and version id")]
        public void theOrganizationResourceItShouldContainMetaDataProfileAndVersionId()
        {

            Organization organization = (Organization)FhirContext.FhirResponseResource;
            organization.Meta.VersionId.ShouldNotBeNull();
            organization.Meta.Profile.ShouldNotBeNull();

            foreach (string profile in organization.Meta.Profile)
            {
                profile.ShouldBe("http://fhir.nhs.net/StructureDefinition/gpconnect-organization-1");
            }

        }

        [Then(@"if the organization resource contains an identifier it is valid")]
        public void ThenIfTheOrganizationResourcesContainAnIdentifierItIsValid()
        {
            Organization organization = (Organization)FhirContext.FhirResponseResource;
            foreach (Identifier identifier in organization.Identifier)
            {
                identifier.System.ShouldNotBeNullOrEmpty();
                var validSystems = new string[2] { "http://fhir.nhs.net/Id/ods-organization-code", "http://fhir.nhs.net/Id/ods-site-code" };
                identifier.System.ShouldBeOneOf(validSystems, "The identifier System can only be one of the valid value");
                identifier.Value.ShouldNotBeNullOrEmpty();
            }
        }

        [Then(@"if the organization resource contains type it is valid")]
        public void ThenIfTheOrganizationResourceContainsTypeItIsValis()
        {
            Organization organization = (Organization)FhirContext.FhirResponseResource;
            if (organization.Type != null && organization.Type.Coding != null)
            {
                organization.Type.Coding.Count.ShouldBeLessThanOrEqualTo(1);
                foreach (var coding in organization.Type.Coding) {
                    coding.System.ShouldBe("http://fhir.nhs.net/ValueSet/organisation-type-1", "Returned Organization coding should contain a valid system element.");
                    coding.Code.ShouldNotBeNullOrEmpty("Returned Organization coding should contain a code element");
                    coding.Display.ShouldNotBeNullOrEmpty("Returned Organization coding should contain a display element");
                }
            }
        }

        [Then(@"if the organization resource contains a partOf reference it is valid")]
        public void ThenIfTheOrganizationResourceContainsAPartOfReferenceItIsValid()
        {
            Organization organization = (Organization)FhirContext.FhirResponseResource;
            if (organization.PartOf != null)
            {
                organization.PartOf.Reference.ShouldNotBeNull();
                string reference = organization.PartOf.Reference;
                reference.ShouldStartWith("Organization/");
                var returnedResource = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:organization", reference);
                returnedResource.GetType().ShouldBe(typeof(Organization));

            }
        }
     }
}
