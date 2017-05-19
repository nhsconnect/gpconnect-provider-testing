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

        public OrganizationSteps(FhirContext fhirContext, AccessRecordSteps accessRecordSteps, HttpContext httpContext)
        {
            FhirContext = fhirContext;
            AccessRecordSteps = accessRecordSteps;
            HttpContext = httpContext;
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
        public void GivenIGetPracIdAndSaveItAsIdName(string orgName, string savedOrg)
        {
            string identifier = "urn:nhs:names:services:gpconnect:fhir:rest:search:organization";
            string system = "http://fhir.nhs.net/Id/ods-organization-code";
            string value = FhirContext.FhirOrganizations[orgName];
            string URL = "/Organization";

            Given("I am using the default server");
            Given($@"I am performing the ""{identifier}"" interaction");
            Given($@"I add the organization identifier parameter with system ""{system}"" and value ""{orgName}""");
            When($@"I make a GET request to ""{URL}""");
            Then("the response status code should indicate success");
            Then("the response body should be FHIR JSON");

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

    }
}
