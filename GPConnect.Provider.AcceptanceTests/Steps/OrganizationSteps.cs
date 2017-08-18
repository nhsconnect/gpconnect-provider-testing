using GPConnect.Provider.AcceptanceTests.Constants;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Context;
    using Enum;
    using Hl7.Fhir.Model;
    using Repository;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    public sealed class OrganizationSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private readonly BundleSteps _bundleSteps;
        private readonly HttpResponseSteps _httpResponseSteps;
        private readonly IFhirResourceRepository _fhirResourceRepository;

        private List<Organization> Organizations => _httpContext.FhirResponse.Organizations;

        public OrganizationSteps(HttpSteps httpSteps, HttpContext httpContext, BundleSteps bundleSteps, HttpResponseSteps httpResponseSteps, IFhirResourceRepository fhirResourceRepository) : base(httpSteps)
        {
            _httpContext = httpContext;
            _bundleSteps = bundleSteps;
            _httpResponseSteps = httpResponseSteps;
            _fhirResourceRepository = fhirResourceRepository;
        }

        [Then(@"the Response Resource should be an Organization")]
        public void TheResponseResourceShouldBeAnOrganization()
        {
            _httpContext.FhirResponse.Resource.ResourceType.ShouldBe(ResourceType.Organization);
        }

        [Then("the Organization Id should equal the Request Id")]
        public void TheOrganizationIdShouldEqualTheRequestId()
        {
            Organizations.ForEach(organization =>
            {
                organization.Id.ShouldBe(_httpContext.HttpRequestConfiguration.GetRequestId,
                    $"The Organization Id should be equal to {_httpContext.HttpRequestConfiguration.GetRequestId} but was {organization.Id}.");
            });
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
                        .Where(identifier => identifier.System.Equals(FhirConst.IdentifierSystems.kOdsOrgzCode))
                        .ToList();

                    odsOrganizationCodeIdentifiers.Count.ShouldBeLessThanOrEqualTo(1, "There may only be one Organization Identifier within the returned Organization.");

                    organization.Identifier.ForEach(identifier =>
                    {
                        identifier.System.ShouldNotBeNullOrEmpty("The Identifier System should not be null or empty");
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
                CheckForValidMetaDataInResource(organization, FhirConst.StructureDefinitionSystems.kOrganisation);
            });
        }

        [Then("the Organization Full Url should be valid")]
        public void TheOrganizationFullUrlShouldBeValid()
        {
            var organizationEntries = _httpContext
                .FhirResponse
                .Entries
                .Where(entry => entry.Resource.ResourceType == ResourceType.Organization)
                .ToList();

            organizationEntries.ForEach(organizationEntry =>
            {
                organizationEntry.FullUrl.ShouldNotBeNull();
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
                        coding.Code.ShouldNotBeNullOrEmpty("The Coding Code value should not be null or empty");
                        coding.Display.ShouldNotBeNullOrEmpty("The Coding Display value should not be null or empty");
                    }
                }
            });
        }

        [Then(@"the Organization Name should be valid")]
        public void TheOrganizationNameShouldBeValid()
        {
            Organizations.ForEach(organization =>
            {
                organization.Name.ShouldNotBeNullOrEmpty("The organisations name is missing but must be provided.");
            });
        }

        [Then(@"the Organization Contact should be valid")]
        public void TheOrganizationContactShouldBeValid()
        {
            Organizations.ForEach(organization =>
            {
                organization.Contact.ForEach(ValidateContact);

                organization.ModifierExtension.ForEach(ValidateExtensions);
            });
        }

        private void ValidateExtensions(Extension extensions)
        {
            if (extensions != null)
            {
                var validExtensions = new [] { FhirConst.StructureDefinitionSystems.kExtCcGpcMainLoc, FhirConst.StructureDefinitionSystems.kOrgzPeriod };
                extensions.Url.ShouldBeOneOf(validExtensions);
            }
        }

        private void ValidateContact(Organization.ContactComponent contact)
        {
            if (contact != null)
            {
                var contactName = contact.Name;

                if (contactName != null)
                {
                    contactName.Use?.ShouldBeOfType<HumanName.NameUse>(string.Format("Organisation Contact Name Use is not a valid value within the value set {0}", FhirConst.ValueSetSystems.kNameUse));

                    contactName.Family.Count().ShouldBeLessThanOrEqualTo(1, "Organisation Contact Name Family Element should contain a maximum of 1.");
                }

                var contactTeleCom = contact.Telecom;

                contactTeleCom.ForEach(teleCom =>
                {
                    teleCom.System?.ShouldBeOfType<ContactPoint.ContactPointSystem>(string.Format("Organisation Contact Telecom System is not a valid value within the value set {0}", FhirConst.ValueSetSystems.kContactPointSystem));
                    teleCom.Use?.ShouldBeOfType<ContactPoint.ContactPointUse>(string.Format("Organisation Contact Telecom Use is not a valid value within the value set {0}", FhirConst.ValueSetSystems.kNContactPointUse));
                });

                var contactAddress = contact.Address;

                if (contactAddress != null)
                {
                    contactAddress.Type?.ShouldBeOfType<Address.AddressType>(string.Format("Organisation Contact Address Type is not a valid value within the value set {0}", FhirConst.ValueSetSystems.kAddressType));
                    contactAddress.Use?.ShouldBeOfType<Address.AddressUse>(string.Format("Organisation Contact Address Use is not a valid value within the value set {0}", FhirConst.ValueSetSystems.kAddressUse));
                }

            }
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

                    _httpContext.HttpRequestConfiguration.RequestUrl = organization.PartOf.Reference;

                    _httpSteps.MakeRequest(GpConnectInteraction.OrganizationRead);

                    _httpResponseSteps.ThenTheResponseStatusCodeShouldIndicateSuccess();

                    StoreTheOrganization();

                    var returnedReference = _fhirResourceRepository.Organization.ResourceIdentity().ToString();

                    returnedReference.ShouldStartWith(organization.PartOf.Reference);
                }
            });
        }

        [Given(@"I add an Organization Identifier parameter with System ""([^""]*)"" and Value ""([^""]*)""")]
        public void AddAnIdentifierParameterWithSystemAndValue(string system, string value)
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("identifier", system + '|' + GlobalContext.OdsCodeMap[value]);
        }

        [Given(@"I add an Organization Identifier parameter with Organization Code System and Value ""([^""]*)""")]
        public void AddAnIdentifierParameterWithOrganizationsCodeSystemAndValue(string value)
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("identifier", string.Format("{0}|{1}", FhirConst.IdentifierSystems.kOdsOrgzCode, GlobalContext.OdsCodeMap[value]));
        }

        [Given(@"I add an Organization Identifier parameter with Site Code System and Value ""([^""]*)""")]
        public void AddAnIdentifierParameterWithSiteCodeSystemAndValue(string value)
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("identifier", string.Format("{0}|{1}", FhirConst.IdentifierSystems.kOdsSiteCode, GlobalContext.OdsCodeMap[value]));
        }

        [Given(@"I add an Identifier parameter with the Value ""([^""]*)""")]
        public void AddAnIdentifierParameterWithTheValue(string value)
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("identifier", value);
        }

        [Given(@"I add a ""([^""]*)"" parameter with the Value ""([^""]*)""")]
        public void AddAnIdentifierParameterWithTheValue(string parameterName, string value)
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter(parameterName, value);
        }

        [Given(@"I add a Format parameter with the Value ""([^""]*)""")]
        public void AddAFormatParameterWithTheValue(string value)
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("_format", value);
        }

        [Given(@"I get the Organization for Organization Code ""([^""]*)""")]
        public void GetTheOrganizationForOrganizationCode(string code)
        {
            _httpSteps.ConfigureRequest(GpConnectInteraction.OrganizationSearch);

            AddAnIdentifierParameterWithOrganizationsCodeSystemAndValue(code);

            _httpSteps.MakeRequest(GpConnectInteraction.OrganizationSearch);
        }

        [Given(@"I store the Organization Version Id")]
        public void StoreTheOrganizationVersionId()
        {
            var organization = Organizations.FirstOrDefault();
            if (organization != null)
                _httpContext.HttpRequestConfiguration.GetRequestVersionId = organization.VersionId;
        }

        [Given(@"I store the Organization")]
        public void StoreTheOrganization()
        {
            var organization = Organizations.FirstOrDefault();
            if (organization != null)
            {
                _httpContext.HttpRequestConfiguration.GetRequestId = organization.Id;
                _fhirResourceRepository.Organization = organization;
            }
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
                .Where(identifier => identifier.System == FhirConst.IdentifierSystems.kOdsSiteCode)
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
                .Where(identifier => identifier.System == FhirConst.IdentifierSystems.kOdsOrgzCode)
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

        [Then(@"the returned organization contains an identifier of type ""([^""]*)"" with a value of ""([^""]*)""")]
        public void ThenTheReturnedOrgainzationContainsIdentifiersOfTypeWithValues(string identifierSystem, string identifierValue)
        {
            var organization = (Organization)_httpContext.FhirResponse.Resource;
            organization.Identifier.ShouldNotBeNull("The organization should contain an organization identifier as the business identifier was used to find the organization for this test.");

            var identifierFound = organization.Identifier.Find(identifier => identifier.System.Equals(identifierSystem) && identifier.Value.Equals(GlobalContext.OdsCodeMap[identifierValue]));
            identifierFound.ShouldNotBeNull($"The expected identifier was not found in the returned organization resource, expected value {GlobalContext.OdsCodeMap[identifierValue]}");
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

        [Then(@"the response bundle Organization entries should contain a maximum of 1 ""([^""]*)"" system identifier")]
        public void ThenResponseBundleOrganizationEntriesShouldContainAMaximumOf1OrgCodeSystemIdentifier(string systemIdentifier)
        {
            foreach (var entry in _httpContext.FhirResponse.Entries)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Organization))
                {

                    Organization organization = (Organization)entry.Resource;

                    var odsOrganizationCodeIdentifiers = organization.Identifier
                        .Where(identifier => identifier.System.Equals(FhirConst.IdentifierSystems.kOdsOrgzCode))
                        .ToList();

                    var odsSiteCodeIdentifiers = organization.Identifier
                        .Where(identifier => identifier.System.Equals(FhirConst.IdentifierSystems.kOdsSiteCode))
                        .ToList();

                    odsOrganizationCodeIdentifiers.Count.ShouldBeLessThanOrEqualTo(1, string.Format("There may only be one Identifier with system of {0} within the returned Organization.", FhirConst.IdentifierSystems.kOdsOrgzCode));
                    odsSiteCodeIdentifiers.Count.ShouldBeLessThanOrEqualTo(1, string.Format("There may only be one Identifier with system of {0} within the returned Organization.", FhirConst.IdentifierSystems.kOdsSiteCode));
                }
            }
        }
    }
}


