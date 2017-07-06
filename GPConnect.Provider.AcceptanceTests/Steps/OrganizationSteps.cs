namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using Context;
    using Hl7.Fhir.Model;
    using Shouldly;
    using System.Linq;
    using TechTalk.SpecFlow;
    using System;
    using System.Collections.Generic;
    using Constants;
    using Enum;
    using static Hl7.Fhir.Model.Bundle;

    [Binding]
    public sealed class OrganizationSteps : BaseSteps
    {
        private readonly HttpContext HttpContext;
        private readonly BundleSteps _bundleSteps;

        public OrganizationSteps(FhirContext fhirContext, HttpSteps httpSteps, HttpContext httpContext, BundleSteps bundleSteps) : base(fhirContext, httpSteps)
        {
            HttpContext = httpContext;
            _bundleSteps = bundleSteps;
        }

        [Given(@"I add the organization identifier parameter with system ""(.*)"" and value ""(.*)""")]
        public void GivenIAddTheOrganizationIdentifierParameterWithTheSystemAndValue(string systemParameter, string valueParameter)
        {
            Given($@"I add the parameter ""identifier"" with the value ""{systemParameter + '|' + GlobalContext.OdsCodeMap[valueParameter]}""");
        }

        [Then(@"the response should contain ods-([^""]*)-codes ""([^""]*)""")]
        public void ThenTheResponseShouldContainODSOrganizationCodesWithValues(string system, string elementValues)
        {
            List<string> referenceValueList = new List<string>();

            foreach (var element in elementValues.Split(new char[] { '|' }))
            {
                referenceValueList.Add(GlobalContext.OdsCodeMap[element]);
            }

            string referenceValues = String.Join("|", referenceValueList);

            Then($@"the response bundle ""Organization"" entries should contain element ""resource.identifier[?(@.system == 'http://fhir.nhs.net/Id/ods-{system}-code')].value"" with values ""{referenceValues}""");
        }

        [Then(@"the response bundle Organization entries should only contain an ODS organization codes and ODS Site Codes")]
        public void TheResponseBundleOrganizationEntriesShouldOnlyContainAnOdsOrganizationCodesAndOdsSiteCodes()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Organization))
                {
                    Organization organization = (Organization)entry.Resource;
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
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Organization))
                {
                    Organization organization = (Organization)entry.Resource;
                    if (organization.Type != null && organization.Type.Coding != null)
                    {
                        organization.Type.Coding.Count.ShouldBeLessThanOrEqualTo(1);
                        ShouldBeSingleCodingWhichIsInValueSet(GlobalContext.FhirMaritalStatusValueSet, organization.Type.Coding);
                    }
                }
            }
        }


        [Given(@"I get organization ""(.*)"" id and save it as ""(.*)""")]
        public void GivenIGetOrganizationIdAndSaveItAs(string orgName, string savedOrg)
        {

            string system = "http://fhir.nhs.net/Id/ods-organization-code";
            string value = GlobalContext.OdsCodeMap[orgName];

            Given("I am using the default server");
            Given($@"I am performing the ""urn:nhs:names:services:gpconnect:fhir:rest:search:organization"" interaction");
            Given($@"I add the organization identifier parameter with system ""{system}"" and value ""{orgName}""");
            When($@"I make a GET request to ""/Organization""");
            Then($@"the response status code should indicate success");
            Then($@"the response body should be FHIR JSON");
            Then($@"the response should be a Bundle resource of type ""searchset""");
            Then($@"the response bundle should contain ""1"" entries");

            Organization organization = new Organization();
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Organization))
                {
                    organization = (Organization)entry.Resource;

                }
            }
            HttpContext.StoredFhirResources.Add(savedOrg, organization);
        }

        [When(@"I get organization ""(.*)"" and use the id to make a get request to the url ""(.*)""")]
        public void WhenIGetOrganizationAndUseTheIdToMakeAGetRequestToTheURL(string organizationName, string URL)
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
        public void ThenTheResponseShouldBeAnOrganizationResource()
        {
            _fhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.Organization);
        }

        [When(@"I make a GET request for a organization using an invalid id of ""(.*)""")]
        public void ThenIMakeAGetRequestForAOrganizationUsingAnInvalidIdOf(string invalidId)
        {
            string URL = "/Organization/" + invalidId;
            When($@"I make a GET request to ""{URL}""");
        }

        [Then(@"the organization resource shall contain meta data profile and version id")]
        public void ThenTheOrganizationResourceShallContainMetaDataProfileAndVersionId()
        {

            Organization organization = (Organization)_fhirContext.FhirResponseResource;
            organization.Meta.VersionId.ShouldNotBeNull();
            organization.Meta.Profile.ShouldNotBeNull();

            foreach (string profile in organization.Meta.Profile)
            {
                profile.ShouldBe("http://fhir.nhs.net/StructureDefinition/gpconnect-organization-1");
            }

        }
        [Then(@"the returned organization resource identifiers should be of a valid type and cardinality")]
        public void ThenIfTheOrganizationResourcesContainAnIdentifierItIsValid()
        {
            Organization organization = (Organization)_fhirContext.FhirResponseResource;
            int organizationIdentiferCount = 0;
            foreach (Identifier identifier in organization.Identifier)
            {
                identifier.System.ShouldNotBeNullOrEmpty("The included identifier should have a system in it is included in the response.");
                var validSystems = new string[2] { "http://fhir.nhs.net/Id/ods-organization-code", "http://fhir.nhs.net/Id/ods-site-code" };
                identifier.System.ShouldBeOneOf(validSystems, "The identifier System can only be one of the two valid systems");
                identifier.Value.ShouldNotBeNullOrEmpty("The included identifier should have a value.");
                if (identifier.System.Equals("http://fhir.nhs.net/Id/ods-organization-code"))
                {
                    organizationIdentiferCount++;
                }
            }
            organizationIdentiferCount.ShouldBeLessThanOrEqualTo(1, "There may only be one organization identifier within the returned Organization resource.");
        }

        [Then(@"if the organization resource contains type element it shall contain the system code and display elements")]
        public void ThenIfTheOrganizationResourceContainsTypeElemetnItShallContainTheSystemCodeAndDisplayElements()
        {
            Organization organization = (Organization)_fhirContext.FhirResponseResource;
            if (organization.Type != null && organization.Type.Coding != null)
            {
                organization.Type.Coding.Count.ShouldBeLessThanOrEqualTo(1, "The should only be one coding element within the type element.");
                foreach (var coding in organization.Type.Coding)
                {
                    coding.System.ShouldBe("http://fhir.nhs.net/ValueSet/organisation-type-1", "Returned Organization Type Coding should contain a valid system element.");
                    coding.Code.ShouldNotBeNullOrEmpty("Returned Organization Type Coding should contain a Code element");
                    coding.Display.ShouldNotBeNullOrEmpty("Returned Organization Type Coding should contain a Display element");
                }
            }
        }

        [Then(@"if the organization resource contains a partOf reference it is valid and resolvable")]
        public void ThenIfTheOrganizationResourceContainsAPartOfReferenceItIsValidAndResolvable()
        {
            Organization organization = (Organization)_fhirContext.FhirResponseResource;
            if (organization.PartOf != null)
            {
                organization.PartOf.Reference.ShouldNotBeNull();
                string reference = organization.PartOf.Reference;
                reference.ShouldStartWith("Organization/");
                var returnedResource = _httpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:organization", reference);
                returnedResource.GetType().ShouldBe(typeof(Organization));

            }
        }

   

        [Then(@"if the organization resource contains type it is valid")]
        public void ThenIfTheOrganizationResourceContainsTypeItIsValid()
        {
            Organization organization = (Organization)_fhirContext.FhirResponseResource;
            if (organization.Type != null && organization.Type.Coding != null)
            {
                organization.Type.Coding.Count.ShouldBeLessThanOrEqualTo(1);
                foreach (var coding in organization.Type.Coding)
                {
                    coding.System.ShouldBe("http://fhir.nhs.net/ValueSet/organisation-type-1", "Returned Organization coding should contain a valid system element.");
                    coding.Code.ShouldNotBeNullOrEmpty("Returned Organization coding should contain a code element");
                    coding.Display.ShouldNotBeNullOrEmpty("Returned Organization coding should contain a display element");
                }
            }
        }

        [Then(@"if the organization resource contains a partOf reference it is valid")]
        public void ThenIfTheOrganizationResourceContainsAPartOfReferenceItIsValid()
        {
            Organization organization = (Organization)_fhirContext.FhirResponseResource;
            if (organization.PartOf != null)
            {
                organization.PartOf.Reference.ShouldNotBeNull();
                string reference = organization.PartOf.Reference;
                reference.ShouldStartWith("Organization/");
                var returnedResource = _httpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:organization", reference);
                returnedResource.GetType().ShouldBe(typeof(Organization));

            }
        }

        //Hayden
        private List<Organization> Organizations => _fhirContext.Organizations;

        [Then(@"the Organization Identifiers should be valid")]
        public void TheOrganizationIdentifiersShouldBeValid()
        {
            Organizations.ForEach(organization =>
            {
                if (organization.Identifier != null)
                {
                    var odsOrganizationCodeCount = organization.Identifier.Count(identifier => identifier.System.Equals("http://fhir.nhs.net/Id/ods-organization-code"));
                    odsOrganizationCodeCount.ShouldBeLessThanOrEqualTo(1);

                    organization.Identifier.ForEach(identifier =>
                    {
                        identifier.System.ShouldBeOneOf("http://fhir.nhs.net/Id/ods-organization-code", "http://fhir.nhs.net/Id/ods-site-code");
                        identifier.Value.ShouldNotBeNull();
                    });
                }
            });
        }


        [Then(@"the returned resource shall contain the business identifier for Organization ""([^""]*)""")]
        public void ThenTheReturnedResourceShallContainTheBusinessIdentifierForOrganization(string organizationName)
        {
            Organization organization = (Organization)_fhirContext.FhirResponseResource;
            organization.Identifier.ShouldNotBeNull("The organization should contain an organization identifier as the business identifier was used to find the organization for this test.");
            organization.Identifier.Find(identifier => identifier.System.Equals("http://fhir.nhs.net/Id/ods-organization-code")).Value.ShouldBe(GlobalContext.OdsCodeMap[organizationName], "Organization business identifier does not match the expected business identifier.");
        }

        [Then(@"the Organization Type should be valid")]
        public void TheOrganizationTypeShouldBeValid()
        {
            Organizations.ForEach(organization =>
            {
                if (organization.Type?.Coding != null)
                {
                    organization.Type.Coding.Count.ShouldBeLessThanOrEqualTo(1);

                    var coding = organization.Type.Coding.First();

                    coding.System.ShouldNotBeNull();
                    coding.Code.ShouldNotBeNull();
                    coding.Display.ShouldNotBeNull();
                }
            });
        }

        [Then(@"the Organization PartOf Organization should be referenced in the Bundle")]
        public void TheOrganizationPartOfOrganizationShouldBeReferencedInTheBundle()
        {
            _fhirContext.Organizations.ForEach(organization =>
            {
                if (organization.PartOf != null)
                {
                    _bundleSteps.ResponseBundleContainsReferenceOfType(organization.PartOf.Reference, ResourceType.Organization);
                }
            });
        }

        [Given(@"I add an Organization Identifier parameter with System ""([^""]*)"" and Value ""([^""]*)""")]
        public void AddAnIdentifierParameterWithSystemAndValue(string system, string value)
        {
            HttpContext.RequestParameters.AddParameter("identifier", system + '|' + GlobalContext.OdsCodeMap[value]);
        }

        [Given(@"I get the Organization for Organization Code ""([^""]*)""")]
        public void GetTheOrganizationForOrganizationCode(string code)
        {
            _httpSteps.ConfigureRequest(GpConnectInteraction.OrganizationSearch);

            AddAnIdentifierParameterWithSystemAndValue("http://fhir.nhs.net/Id/ods-organization-code", code);

            _httpSteps.MakeRequest(GpConnectInteraction.OrganizationSearch);
        }

        [Given(@"I store the Organization Id")]
        public void StoreTheOrganizationId()
        {
            var organization = _fhirContext.Organizations.FirstOrDefault();
            if (organization != null)
                HttpContext.GetRequestId = organization.Id;
        }

        [Given(@"I store the Organization")]
        public void StoreTheOrganization()
        {
            var organization = _fhirContext.Organizations.FirstOrDefault();
            if (organization != null)
                HttpContext.StoredOrganization = organization;
        }

        [Then(@"the ""([^""]*)"" organization returned in the bundle is saved and given the name ""([^""]*)""")]
        public void ThenTheOrganizationReturnedInTheBundleIsSavedAndGivenTheName(string organizationPosition, string name)
        {
            Organization organization = new Organization();
            int count = 0;
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Organization))
                {

                    if (count == 0 && organizationPosition == "First")
                    {
                        organization = (Organization)entry.Resource;
                        HttpContext.StoredFhirResources.Add(name, organization);
                        break;
                    }


                    if (count == 1 && organizationPosition == "Second")
                    {
                        organization = (Organization)entry.Resource;
                        HttpContext.StoredFhirResources.Add(name, organization);
                        break;
                    }
                    else
                    {
                        count++;
                        continue;
                    }
                }
            }
        }


        [Then(@"the stored organization ""([^""]*)"" contains ""([^""]*)"" ""([^""]*)"" system identifier with ""([^""]*)"" code ""([^""]*)""")]
        public void ThenTheStoredOrganizationContainsSystemIdentifierWithSiteCode(string organizationName, int Number, string System, string searchMethod, string Code)
        {
            int count = 0;
            Organization organization = (Organization)HttpContext.StoredFhirResources[organizationName];
            foreach (var identifier in organization.Identifier)
            {

                if (identifier.System == System)
                {
                    List<string> referenceValueLists = getIdentifiersInList(Code);
                    count++;
                    referenceValueLists.ShouldContain(identifier.Value);

                }
            }
            count.ShouldBe(Number);
        }

        [Then(@"an organization returned in the bundle has ""([^""]*)"" ""([^""]*)"" system identifier with ""([^""]*)"" and ""([^""]*)"" ""([^""]*)"" system identifier with site code ""([^""]*)""")]
        public void ThenAnOrganizationReturnedInTheBundleHasSystemIdentifierWithAndSystemIdentifierWithSiteCode(int orgCount, string orgSystem, string orgCode, int siteCount, string siteSystem, string siteCode)
        {
            int orgLoopCounter, siteLoopCounter;
            bool organizationIdentifierCountCorrect = false;
            bool allOrganizationsFound = false;
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Organization))
                {
             
                    Organization organization = (Organization)(Organization)entry.Resource;
                    siteLoopCounter = 0;
                    orgLoopCounter = 0;          
                    organizationIdentifierCountCorrect = (organization.Identifier.Count == (orgCount + siteCount));

                    foreach (var identifier in organization.Identifier)
                    {
                        if (identifier.System == orgSystem)
                        {
                            List<string> referenceValueLists = getIdentifiersInList(orgCode);
                            referenceValueLists.ShouldContain(identifier.Value);
                            orgLoopCounter++;
                        }
                        if (identifier.System == siteSystem)
                        {
                            List<string> referenceValueLists = getIdentifiersInList(siteCode);
                            referenceValueLists.ShouldContain(identifier.Value);
                            siteLoopCounter++;
                        }
                    }

                    if ((orgLoopCounter == orgCount) && (siteLoopCounter == siteCount) && (organizationIdentifierCountCorrect))
                    {
                        allOrganizationsFound = true;
                        break;
                    }
                }
            }
            allOrganizationsFound.ShouldBe(true, "The number of organizations or site codes are invalid");
        }

        [Then(@"the returned organization contains identifiers of type ""([^""]*)"" with values ""([^""]*)""")]
        public void ThenTheReturnedOrgainzationContainsIdentifiersOfTypeWithValues(string identifierSystem, string identifierValueCSV)
        {
            Organization organization = (Organization)_fhirContext.FhirResponseResource;
            organization.Identifier.ShouldNotBeNull("The organization should contain an organization identifier as the business identifier was used to find the organization for this test.");

            foreach (var identifierValue in identifierValueCSV.Split(','))
            {
                Identifier identifierFound = organization.Identifier.Find(identifier => identifier.System.Equals(identifierSystem) && identifier.Value.Equals(GlobalContext.OdsCodeMap[identifierValue]));
                identifierFound.ShouldNotBeNull($"The expected identifier was not found in the returned organization resource, expected value {GlobalContext.OdsCodeMap[identifierValue]}");
            }

        }

        private List<string> getIdentifiersInList(string code)
        {
            List<string> referenceValueLists = new List<string>();
            foreach (var element in code.Split(new char[] { '|' }))
            {
                referenceValueLists.Add(GlobalContext.OdsCodeMap[element]);
            }

            return referenceValueLists;
        }

    }
}


