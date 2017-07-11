namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using Context;
    using Enum;
    using Hl7.Fhir.Model;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    public sealed class OrganizationSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private readonly BundleSteps _bundleSteps;
        private List<Organization> Organizations => _fhirContext.Organizations;

        public OrganizationSteps(FhirContext fhirContext, HttpSteps httpSteps, HttpContext httpContext, BundleSteps bundleSteps) : base(fhirContext, httpSteps)
        {
            _httpContext = httpContext;
            _bundleSteps = bundleSteps;
        }

        [Then(@"the Response Resource should be an Organization")]
        public void TheResponseResourceShouldBeAnOrganization()
        {
            _fhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.Organization);
        }

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
            Organizations.ForEach(organization =>
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

        [Given(@"I add an Identifier parameter with the Value ""([^""]*)""")]
        public void AddAnIdentifierParameterWithTheValue(string value)
        {
            _httpContext.RequestParameters.AddParameter("identifier", value);
        }

        [Given(@"I add a ""([^""]*)"" parameter with the Value ""([^""]*)""")]
        public void AddAnIdentifierParameterWithTheValue(string parameterName, string value)
        {
            _httpContext.RequestParameters.AddParameter(parameterName, value);
        }

        [Given(@"I add a Format parameter with the Value ""([^""]*)""")]
        public void AddAFormatParameterWithTheValue(string value)
        {
            _httpContext.RequestParameters.AddParameter("_format", value);
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
            var organization = Organizations.FirstOrDefault();
            if (organization != null)
                _httpContext.GetRequestId = organization.Id;
        }

        [Given(@"I store the Organization Version Id")]
        public void StoreTheOrganizationVersionId()
        {
            var organization = Organizations.FirstOrDefault();
            if (organization != null)
                _httpContext.GetRequestVersionId = organization.VersionId;
        }

        [Given(@"I store the Organization")]
        public void StoreTheOrganization()
        {
            var organization = Organizations.FirstOrDefault();
            if (organization != null)
                _httpContext.StoredOrganization = organization;
        }

        [Then(@"the Organization Identifiers are correct for Organization Code ""([^""]*)""")]
        public void OrganizationIdentifiersAreCorrectForOrganizationCode(string organizationCode)
        {
            Organizations.ForEach(organization =>
            {
                //Check if Organization Code Identifiers are valid.
                OrganizationCodeIdentifiersAreValid(organizationCode, organization);

                //Check if Site Code Identifiers are valid
                SiteCodeIdentifiersAreValid(organizationCode, organization);
            });
        }

        [Then(@"the Organization Identifiers are correct for Site Code ""([^""]*)""")]
        public void OrganizationIdentifiersAreCorrectForSiteCode(string siteCode)
        {
            //Get Organization Codes for Site Code
            var organizationSiteCodeMap = GlobalContext.OrganizationSiteCodeMap[siteCode];

            Organizations.ForEach(organization =>
            {
                organizationSiteCodeMap.ForEach(organizationCode =>
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
                });
            });
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
            var organization = (Organization)_fhirContext.FhirResponseResource;
            organization.Identifier.ShouldNotBeNull("The organization should contain an organization identifier as the business identifier was used to find the organization for this test.");

            foreach (var identifierValue in identifierValueCSV.Split(','))
            {
                var identifierFound = organization.Identifier.Find(identifier => identifier.System.Equals(identifierSystem) && identifier.Value.Equals(GlobalContext.OdsCodeMap[identifierValue]));
                identifierFound.ShouldNotBeNull($"The expected identifier was not found in the returned organization resource, expected value {GlobalContext.OdsCodeMap[identifierValue]}");
            }

        }

        [Then(@"an organization returned in the bundle has ""([^""]*)"" ""([^""]*)"" system identifier with ""([^""]*)"" and ""([^""]*)"" ""([^""]*)"" system identifier with site code ""([^""]*)""")]
        public void ThenAnOrganizationReturnedInTheBundleHasSystemIdentifierWithAndSystemIdentifierWithSiteCode(int orgCount, string orgSystem, string orgCode, int siteCount, string siteSystem, string siteCode)
        {
            var allOrganizationsFound = false;

            Organizations.ForEach(organization =>
            {
                var siteLoopCounter = 0;
                var orgLoopCounter = 0;
                var organizationIdentifierCountCorrect = (organization.Identifier.Count == (orgCount + siteCount));

                foreach (var identifier in organization.Identifier)
                {
                    if (identifier.System == orgSystem)
                    {
                        var referenceValueLists = getIdentifiersInList(orgCode);
                        referenceValueLists.ShouldContain(identifier.Value);
                        orgLoopCounter++;
                    }
                    if (identifier.System == siteSystem)
                    {
                        var referenceValueLists = getIdentifiersInList(siteCode);
                        referenceValueLists.ShouldContain(identifier.Value);
                        siteLoopCounter++;
                    }
                }

                if ((orgLoopCounter == orgCount) && (siteLoopCounter == siteCount) && (organizationIdentifierCountCorrect))
                {
                    allOrganizationsFound = true;
                }
            });

            allOrganizationsFound.ShouldBe(true, "The number of organizations or site codes are invalid");
        }

        private List<string> getIdentifiersInList(string code)
        {
            return code.Split('|').Select(element => GlobalContext.OdsCodeMap[element]).ToList();
        }
    }
}


