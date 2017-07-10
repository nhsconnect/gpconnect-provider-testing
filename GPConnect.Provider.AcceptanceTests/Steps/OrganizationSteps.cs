namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Context;
    using Enum;
    using Hl7.Fhir.Model;
    using Shouldly;
    using TechTalk.SpecFlow;
    using static Hl7.Fhir.Model.Bundle;

    [Binding]
    public sealed class OrganizationSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private readonly BundleSteps _bundleSteps;

        public OrganizationSteps(FhirContext fhirContext, HttpSteps httpSteps, HttpContext httpContext, BundleSteps bundleSteps) : base(fhirContext, httpSteps)
        {
            _httpContext = httpContext;
            _bundleSteps = bundleSteps;
        }

        [Given(@"I get organization ""(.*)"" id and save it as ""(.*)""")]
        public void GivenIGetOrganizationIdAndSaveItAs(string orgName, string savedOrg)
        {

            string value = GlobalContext.OdsCodeMap[orgName];

            Given("I am using the default server");
            Given($@"I am performing the ""urn:nhs:names:services:gpconnect:fhir:rest:search:organization"" interaction");
            Given($@"I add an Organization Identifier parameter with Organization Code System and value ""{orgName}""");
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
            _httpContext.StoredFhirResources.Add(savedOrg, organization);
        }
        
        [When(@"I perform a vread for organizaiton ""([^""]*)""")]
        public void WhenIPerformAVreadForOrganizaiton(string storedOrganizationKey)
        {
            Organization organization = (Organization)_httpContext.StoredFhirResources[storedOrganizationKey];
            When($@"I make a GET request to ""/Organization/{organization.Id}/_history/{organization.Meta.VersionId}""");
        }

        [When(@"I perform an organization vread with version ""([^""]*)"" for organization stored against key ""([^""]*)""")]
        public void WhenIPerformAnOrganizationVreadWithVersionForOrganizationStoredAgainstKey(string version, string storedOrganizationKey)
        {
            Organization organizationValue = (Organization)_httpContext.StoredFhirResources[storedOrganizationKey];

            When($@"I make a GET request to ""/Organization/{organizationValue.Id}/_history/{version}""");
        }

        [Then(@"the response should be an Organization resource")]
        public void TheResponseShouldBeAnOrganizationResource()
        {
            _fhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.Organization);
        }

        //Hayden
        private List<Organization> Organizations => _fhirContext.Organizations;

        [Then(@"the Organization Identifiers should be valid")]
        public void TheOrganizationIdentifiersShouldBeValid()
        {
            OrganizationIdentifiersShouldBeValid();
        }


        [Then(@"the Organization Identifiers should be valid for Organization ""([^""]*)""")]
        public void TheOrganizationIdentifiersShouldBeValidForOrganization(string organizationName)
        {
            OrganizationIdentifiersShouldBeValid(organizationName);
        }
        
        private void OrganizationIdentifiersShouldBeValid(string organizationName = null)
        {
            Organizations.ForEach(organization =>
            {
                if (organization.Identifier != null)
                {
                    var odsOrganizationCodeIdentifiers = organization.Identifier
                        .Where(identifier => identifier.System.Equals("http://fhir.nhs.net/Id/ods-organization-code"))
                        .ToList();

                    odsOrganizationCodeIdentifiers.Count.ShouldBeLessThanOrEqualTo(1, "There may only be one Organization Identifier within the returned Organization.");

                    organization.Identifier.ForEach(identifier =>
                    {
                        identifier.System.ShouldNotBeNullOrEmpty("The Identifier System should not be null or empty");
                        identifier.System.ShouldBeOneOf("http://fhir.nhs.net/Id/ods-organization-code", "http://fhir.nhs.net/Id/ods-site-code", "The Identifier System should be one of the two valid System values.");
                        identifier.Value.ShouldNotBeNull("The included identifier should have a value.");
                    });

                    var odsOrganizationCodeIdentifier = odsOrganizationCodeIdentifiers.FirstOrDefault();

                    if (organizationName != null)
                    {
                        var odsCode = GlobalContext.OdsCodeMap[organizationName];
                        odsOrganizationCodeIdentifier?.Value.ShouldBe(odsCode, $"The Organization Identifier (Organization Code) Value should match the expected value {odsCode}.");
                    }
                }
            });
        }

        [Then(@"the Organization Metadata should be valid")]
        public void TheOrganizationMetadataShouldBeValid()
        {
            Organizations.ForEach(organization =>
            {
                CheckForValidMetaDataInResource(organization, "http://fhir.nhs.net/StructureDefinition/gpconnect-organization-1");
            });
        }

        [Then(@"the Organization Type should be valid")]
        public void TheOrganizationTypeShouldBeValid()
        {
            Organizations.ForEach(organization =>
            {
                if (organization.Type?.Coding != null)
                {
                    var codingCount = organization.Type.Coding.Count;

                    codingCount.ShouldBeLessThanOrEqualTo(1, $"There should only be 1 Coding within the Type, but found {codingCount}.");

                    var coding = organization.Type.Coding.FirstOrDefault();

                    if (coding != null)
                    {
                        coding.System.ShouldNotBeNullOrEmpty("The Coding System value should not be null or empty");
                        coding.System.ShouldBe("http://fhir.nhs.net/ValueSet/organisation-type-1", "The Coding should contain a valid System value.");
                        coding.Code.ShouldNotBeNullOrEmpty("The Coding Code value should not be null or empty");
                        coding.Display.ShouldNotBeNullOrEmpty("The Coding Display value should not be null or empty");
                    }
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


        [Then(@"the Organization PartOf Organization should be resolvable")]
        public void TheOrganizationPartOfOrganizationResolvable()
        {
            Organizations.ForEach(organization =>
            {
                if (organization.PartOf != null)
                {
                    _httpSteps.ConfigureRequest(GpConnectInteraction.OrganizationRead);

                    _httpContext.RequestUrl = organization.PartOf.Reference;

                    _httpSteps.MakeRequest(GpConnectInteraction.OrganizationRead);

                    _httpSteps.ThenTheResponseStatusCodeShouldIndicateSuccess();

                    StoreTheOrganization();

                    var returnedReference = _httpContext.StoredOrganization.ResourceIdentity().ToString();

                    returnedReference.ShouldStartWith(organization.PartOf.Reference);
                }
            });
        }


        [Given(@"I add an Organization Identifier parameter with System ""([^""]*)"" and Value ""([^""]*)""")]
        public void AddAnIdentifierParameterWithSystemAndValue(string system, string value)
        {
            _httpContext.RequestParameters.AddParameter("identifier", system + '|' + GlobalContext.OdsCodeMap[value]);
        }

        [Given(@"I add an Organization Identifier parameter with Organization Code System and Value ""([^""]*)""")]
        public void AddAnIdentifierParameterWithOrganizationsCodeSystemAndValue(string value)
        {
            _httpContext.RequestParameters.AddParameter("identifier", "http://fhir.nhs.net/Id/ods-organization-code" + '|' + GlobalContext.OdsCodeMap[value]);
        }

        [Given(@"I add an Organization Identifier parameter with Site Code System and Value ""([^""]*)""")]
        public void AddAnIdentifierParameterWithSiteCodeSystemAndValue(string value)
        {
            _httpContext.RequestParameters.AddParameter("identifier", "http://fhir.nhs.net/Id/ods-site-code" + '|' + GlobalContext.OdsCodeMap[value]);
        }

        [Given(@"I get the Organization for Organization Code ""([^""]*)""")]
        public void GetTheOrganizationForOrganizationCode(string code)
        {
            _httpSteps.ConfigureRequest(GpConnectInteraction.OrganizationSearch);

            AddAnIdentifierParameterWithOrganizationsCodeSystemAndValue(code);

            _httpSteps.MakeRequest(GpConnectInteraction.OrganizationSearch);
        }

        [Given(@"I store the Organization Id")]
        public void StoreTheOrganizationId()
        {
            var organization = _fhirContext.Organizations.FirstOrDefault();
            if (organization != null)
                _httpContext.GetRequestId = organization.Id;
        }

        [Given(@"I store the Organization")]
        public void StoreTheOrganization()
        {
            var organization = _fhirContext.Organizations.FirstOrDefault();
            if (organization != null)
                _httpContext.StoredOrganization = organization;
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


        [Then(@"the Organization Identifiers are correct for Organization Code ""([^""]*)""")]
        public void OrganizationIdentifiersAreCorrectForOrganizationCode(string organizationCode)
        {
            foreach (var organization in Organizations)
            {
                //Check if Organization Code Identifiers are valid.
                OrganizationCodeIdentifiersAreValid(organizationCode, organization);

                //Check if Site Code Identifiers are valid
                SiteCodeIdentifiersAreValid(organizationCode, organization);
            }
        }

        [Then(@"the Organization Identifiers are correct for Site Code ""([^""]*)""")]
        public void OrganizationIdentifiersAreCorrectForSiteCode(string siteCode)
        {
            //Get Organization Codes for Site Code
            var organizationSiteCodeMap = GlobalContext.OrganizationSiteCodeMap[siteCode];

            foreach (var organization in Organizations)
            {
                foreach (var organizationCode in organizationSiteCodeMap)
                {
                    //Check if Organization in Bundle contains Identifier for Organization Code
                    var oganizationContainsIdentifier = organization.Identifier
                        .Select(identifier => identifier.Value)
                        .Contains(GlobalContext.OdsCodeMap[organizationCode]);

                    if (oganizationContainsIdentifier)
                    {
                        //Check if Organization Code Identifiers are valid.
                        OrganizationCodeIdentifiersAreValid(organizationCode, organization);

                        //Check if Site Code Identifiers are valid
                        SiteCodeIdentifiersAreValid(organizationCode, organization);
                    }
                }
            }

        }

        private static void SiteCodeIdentifiersAreValid(string organizationCode, Organization organization)
        {
            //Get Site Codes for Organization Code
            var siteCodesForOrganization = GlobalContext.OrganizationSiteCodeMap[organizationCode]
                .Select(x => GlobalContext.OdsCodeMap[x])
                .ToList();

            //Get Site Code Identifier Values for Organization
            var siteCodeIdentifierValues = organization.Identifier
                .Where(identifier => identifier.System == "http://fhir.nhs.net/Id/ods-site-code")
                .Select(identifier => identifier.Value)
                .ToList();

            //Check there are the correct amount of Site Code Identifiers
            siteCodeIdentifierValues.Count.ShouldBe(siteCodesForOrganization.Count, $"There should be a total of {siteCodesForOrganization.Count} Site Code Identifiers for {organizationCode}");

            //Check there are no duplicate Site Code Identifier Values
            siteCodeIdentifierValues.Count.ShouldBe(siteCodeIdentifierValues.Distinct().Count(), $"There are duplicate Site Code Identifiers for {organizationCode}");

            foreach (var siteCodeIdentifierValue in siteCodeIdentifierValues)
            {
                //Check each Site Code Identifier Value is in the expected Site Codes for Organization
                siteCodesForOrganization.ShouldContain(siteCodeIdentifierValue, $"The Site Code {siteCodeIdentifierValue} was not expected for {organizationCode}");
            }
        }

        private static void OrganizationCodeIdentifiersAreValid(string organizationCode, Organization organization)
        {
            //Get Organization Codes for Organization
            var organizationCodes = new List<string> { GlobalContext.OdsCodeMap[organizationCode]};

            //Get Organization Code Identifier values
            var organizationCodeIdentifierValues = organization.Identifier
                .Where(identifier => identifier.System == "http://fhir.nhs.net/Id/ods-organization-code")
                .Select(identifier => identifier.Value)
                .ToList();

            //Check there are the correct amount of Organization Code Identifiers
            organizationCodeIdentifierValues.Count.ShouldBe(organizationCodes.Count, $"There should be a total of {organizationCodes.Count} Organization Code Identifiers for {organizationCode}");

            //Check there are no duplicate Organization Code Identifier Values
            organizationCodeIdentifierValues.Count.ShouldBe(organizationCodeIdentifierValues.Distinct().Count(), $"There are duplicate Organization Code Identifiers for {organizationCode}");

            foreach (var organizationCodeIdentifierValue in organizationCodeIdentifierValues)
            {
                //Check each Organization Code Identifier Value is in the expected Organization Codes for Organization
                organizationCodes.ShouldContain(organizationCodeIdentifierValue, $"The Organization Code {organizationCodeIdentifierValue} was not expected for {organizationCode}");
            }
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


