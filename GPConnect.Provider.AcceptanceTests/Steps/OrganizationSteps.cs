using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Extensions;

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

                    odsOrganizationCodeIdentifiers.Count.ShouldBeLessThanOrEqualTo(1, "There may only be a maximum one Organization Identifier within the returned Organization.");

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

                    var odsSiteCodeIdentifiers = organization.Identifier
                        .Where(identifier => identifier.System.Equals(FhirConst.IdentifierSystems.kOdsSiteCode))
                        .ToList();

                    odsSiteCodeIdentifiers.Count.ShouldBeLessThanOrEqualTo(1, "There may only be a maximum one Site Identifier within the returned Organization.");

                    var localOrgzCodeIdentifiers = organization.Identifier
                        .Where(identifier => identifier.System.Equals(FhirConst.IdentifierSystems.kLocalOrgzCode))
                        .ToList();

                    localOrgzCodeIdentifiers.ForEach(lOrgz =>
                    {
                        lOrgz.Extension.ForEach(ext => ext.Url.ShouldNotBeNullOrEmpty("Local Identifier has an invalid extension. Extensions must have a URL element."));
                        lOrgz.Use?.ShouldBeOfType<Identifier.IdentifierUse>(string.Format("Local Identifier use is Invalid. But be from the value set: {0}", FhirConst.ValueSetSystems.kIdentifierUse));

                        var localOrgzType = lOrgz.Type;

                        if (localOrgzType != null)
                        {
                            localOrgzType.Extension.ForEach(ext => ext.Url.ShouldNotBeNullOrEmpty("Local Identifier Type has an invalid extension. Extensions must have a URL element."));

                            var localOrgzTypeValues = GlobalContext.FhirIdentifierTypeValueSet.WithComposeImports();
                            var localOrgzTypeCodes = localOrgzType.Coding.Where(lzc => lzc.System.Equals(FhirConst.ValueSetSystems.kIdentifierType)).ToList();
                            localOrgzTypeCodes.ForEach(lztc =>
                            {
                                lztc.Extension.ForEach(ext => ext.Url.ShouldNotBeNullOrEmpty("Local Identifier Type Coding has an invalid extension. Extensions must have a URL element."));
                                lztc.Code.ShouldBeOneOf(localOrgzTypeValues.ToArray());
                            });
                        }

                        if (lOrgz.Assigner != null)
                        {
                            _httpSteps.ConfigureRequest(GpConnectInteraction.OrganizationRead);

                            _httpContext.HttpRequestConfiguration.RequestUrl = organization.PartOf.Reference;

                            _httpSteps.MakeRequest(GpConnectInteraction.OrganizationRead);

                            _httpResponseSteps.ThenTheResponseStatusCodeShouldIndicateSuccess();

                            StoreTheOrganization();

                            var returnedReference = _fhirResourceRepository.Organization.ResourceIdentity().ToString();

                            returnedReference.ShouldBe(FhirConst.StructureDefinitionSystems.kOrganisation);

                            lOrgz.Assigner.Extension.ForEach(ext => ext.Url.ShouldNotBeNullOrEmpty("Local Identifier Assigner has an invalid extension. Extensions must have a URL element."));

                        }
                    });

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
            });
        }

        [Then(@"the Organization Address should be valid")]
        public void TheOrganizationAddressShouldBeValid()
        {
            Organizations.ForEach(organization =>
            {
                organization.Address.ForEach(add => ValidateAddress(add, "Organisation Address"));
            });
        }

        [Then(@"the Organization Telecom should be valid")]
        public void TheOrganizationTelecomShouldBeValid()
        {
            Organizations.ForEach(organization =>
            {
                ValidateTelecom(organization.Telecom, "Organisation Telecom");
            });
        }

        [Then(@"the Organization Extensions should be valid")]
        public void TheOrganizationExtensionsShouldBeValid()
        {
            Organizations.ForEach(organization =>
            {
                organization.Extension.ForEach(ValidateExtensions);
            });
        }

        private void ValidateExtensions(Extension extensions)
        {
            if (extensions != null)
            {
                var validExtensions = new [] { FhirConst.StructureDefinitionSystems.kExtCcGpcMainLoc, FhirConst.StructureDefinitionSystems.kOrgzPeriod };
                extensions.Url.ShouldBeOneOf(validExtensions, string.Format("Organisation Extension is invalid. Extensions must be one of {0}", validExtensions.ToString()));
            }
        }

        private void ValidateContact(Organization.ContactComponent contact)
        {
            if (contact != null)
            {
                contact.Extension.ForEach(ext => ext.Url.ShouldNotBeNullOrEmpty("Organisation Contact has an invalid extension. Extensions must have a URL element."));

                var contactPurpose = contact.Purpose;

                if (contactPurpose != null)
                {
                    contactPurpose.Extension.ForEach(ext => ext.Url.ShouldNotBeNullOrEmpty("Organisation Contact Purpose Code has an invalid extension. Extensions must have a URL element."));
                    //Valueset is not available so cannot check this yet
                    //contactPurpose.Coding.ForEach(cd =>
                    //{
                    //    cd.Extension.ForEach(ext => ext.Url.ShouldNotBeNullOrEmpty("Organisation Contact Purpose has an invalid extension. Extensions must have a URL element."));
                    //    if (cd.System.Equals(FhirConst.ValueSetSystems.kContactEntityType))
                    //    {
                    //        if (!string.IsNullOrEmpty(cd.Code))
                    //        {
                    //            cd.Code.ShouldBeOfType<>()
                    //        }
                    //    }
                    //});

                }

                var contactName = contact.Name;

                if (contactName != null)
                {
                    contactName.Use?.ShouldBeOfType<HumanName.NameUse>(string.Format("Organisation Contact Name Use is not a valid value within the value set {0}", FhirConst.ValueSetSystems.kNameUse));

                    contactName.Family.Count().ShouldBeLessThanOrEqualTo(1, "Organisation Contact Name Family Element should contain a maximum of 1.");
                }

                ValidateTelecom(contact.Telecom, "Organisation Contact Telecom");

                ValidateAddress(contact.Address, "Organisation Contact Address");

            }
        }

        private void ValidateAddress(Address address, string from)
        {
            if (address != null)
            {
                address.Extension.ForEach(ext => ext.Url.ShouldNotBeNullOrEmpty(string.Format("{0} has an invalid extension. Extensions must have a URL element.", from)));
                address.Type?.ShouldBeOfType<Address.AddressType>(string.Format("{0} Type is not a valid value within the value set {1}", from, FhirConst.ValueSetSystems.kAddressType));
                address.Use?.ShouldBeOfType<Address.AddressUse>(string.Format("{0} Use is not a valid value within the value set {1}", from, FhirConst.ValueSetSystems.kAddressUse));
            }
        }

        private void ValidateTelecom(List<ContactPoint> telecoms, string from)
        {
            telecoms.ForEach(teleCom =>
            {
                teleCom.Extension.ForEach(ext => ext.Url.ShouldNotBeNullOrEmpty(string.Format("{0} has an invalid extension. Extensions must have a URL element.", from)));
                teleCom.System?.ShouldBeOfType<ContactPoint.ContactPointSystem>(string.Format("{0} System is not a valid value within the value set {1}", from, FhirConst.ValueSetSystems.kContactPointSystem));
                teleCom.Use?.ShouldBeOfType<ContactPoint.ContactPointUse>(string.Format("{0} Use is not a valid value within the value set {1}", from, FhirConst.ValueSetSystems.kNContactPointUse));
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
            StoreTheOrganization(null);
        }

        [Given(@"I store the Organization with site code ""([^""]*)""")]
        public void StoreTheOrganization(string siteCode)
        {
            var organization = Organizations.FirstOrDefault(orgz => string.IsNullOrEmpty(siteCode) || (!string.IsNullOrEmpty(siteCode) && orgz.Identifier.Select(zi => zi.Value).ToList().Contains(GlobalContext.OdsCodeMap[siteCode])));
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

            var referenceValueLists = getIdentifiersInList(identifierValue);
            var identifierFound = organization.Identifier.Find(identifier => identifier.System.Equals(identifierSystem) && referenceValueLists.Contains(identifier.Value));
            identifierFound.ShouldNotBeNull($"The expected identifier was not found in the returned organization resource, expected value {referenceValueLists}");
        }

        [Then(@"an organization returned in the bundle has ""([^""]*)"" ""([^""]*)"" system identifier with ""([^""]*)"" and ""([^""]*)"" ""([^""]*)"" system identifier with site code ""([^""]*)""")]
        public void ThenAnOrganizationReturnedInTheBundleHasSystemIdentifierWithAndSystemIdentifierWithSiteCode(int orgCount, string orgSystem, string orgCode, int siteCount, string siteSystem, string siteCode)
        {
            var totalValidOrganisations = 0;

            Organizations.ForEach(organization =>
            {
                var siteLoopCounter = 0;
                var orgLoopCounter = 0;

                foreach (var identifier in organization.Identifier)
                {
                    if (identifier.System == orgSystem)
                    {
                        var referenceValueLists = getIdentifiersInList(orgCode);
                        referenceValueLists.ShouldContain(identifier.Value);
                        orgLoopCounter++;
                        continue;
                    }
                    if (identifier.System == siteSystem)
                    {
                        var referenceValueLists = getIdentifiersInList(siteCode);
                        referenceValueLists.ShouldContain(identifier.Value);
                        siteLoopCounter++;
                        continue;
                    }
                }

                if ((orgLoopCounter == orgCount) && (siteLoopCounter == siteCount))
                {
                    totalValidOrganisations++;
                }
            });

            totalValidOrganisations.ShouldBe(Organizations.Count, "The number of organizations or site codes are invalid");
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

                    var filteredIdentifiers = organization.Identifier
                        .Where(identifier => identifier.System.Equals(systemIdentifier))
                        .ToList();

                    filteredIdentifiers.Count.ShouldBeLessThanOrEqualTo(1, string.Format("There may only be one Identifier with system of {0} within the returned Organization.", systemIdentifier));
                }
            }
        }
    }
}


