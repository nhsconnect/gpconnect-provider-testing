namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cache;
    using Cache.ValueSet;
    using Constants;
    using Context;
    using Enum;
    using Extensions;
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
        private readonly JwtSteps _jwtSteps;

        private List<Organization> Organizations => _httpContext.FhirResponse.Organizations;

        public OrganizationSteps(HttpSteps httpSteps, HttpContext httpContext, BundleSteps bundleSteps, HttpResponseSteps httpResponseSteps, IFhirResourceRepository fhirResourceRepository, JwtSteps jwtSteps) : base(httpSteps)
        {
            _httpContext = httpContext;
            _bundleSteps = bundleSteps;
            _httpResponseSteps = httpResponseSteps;
            _fhirResourceRepository = fhirResourceRepository;
            _jwtSteps = jwtSteps;
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

        [Then("the Organization Id should be valid")]
        public void TheOrganizationIdShouldBeValid()
        {
            Organizations.ForEach(organization =>
            {
                organization.Id.ShouldNotBeNullOrEmpty($"The Organization Id should not be null or empty.");
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

                }
            });
        }

        [Then(@"the Organization should be valid")]
        public void TheOrganizationShouldBeValid()
        {
            TheOrganizationMetadataShouldBeValid();
            TheOrganizationIdentifiersShouldBeValid();
            TheLocationPartOfLocationShouldBeValid();
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

        // github ref 120
        // RMB 25/10/2018        
        [Then(@"the Organization Not In Use should be valid")]
        public void TheOrganizationNotInUseShouldBeValid()
        {
            Organizations.ForEach(organization =>
            {
                organization.Contact.Count.ShouldBe(0);
                organization.Endpoint.Count.ShouldBe(0); ;

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
                organization.Extension.ForEach(ValidateExtension);
            });
        }

        private static void ValidateExtension(Extension extension)
        {
            if (extension != null)
            {
                var validExtensions = new List<string> { FhirConst.StructureDefinitionSystems.kExtCcGpcMainLoc, FhirConst.StructureDefinitionSystems.kOrgzPeriod };

                validExtensions.ShouldContain(extension.Url, $"The Organisation Extension is invalid. Extensions must be one of {validExtensions}.");
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
                    var valueSet = ValueSetCache.Get(FhirConst.ValueSetSystems.kVsContactEntityType);
                    var contactEntityTypes = valueSet.WithComposeImports().ToArray();

                    contactPurpose.Coding.ForEach(coding =>
                    {
                        if (coding.System.Equals(FhirConst.CodeSystems.kContactEntityType) && contactEntityTypes.Any() && !string.IsNullOrEmpty(coding.Code))
                        {
                            coding.Code.ShouldBeOneOf(contactEntityTypes, $"Organisation Contact Purpose System is {FhirConst.CodeSystems.kContactEntityType}, but the code provided is not valid for this ValueSet.");
                        }
                    });
                }

                var contactName = contact.Name;

                if (contactName != null)
                {
                    contactName.Use?.ShouldBeOfType<HumanName.NameUse>($"Organisation Contact Name Use is not a valid value within the value set {FhirConst.CodeSystems.kNameUse}");

                    contactName.Family.Count().ShouldBeLessThanOrEqualTo(1, "Organisation Contact Name Family Element should contain a maximum of 1.");
                }

                ValidateTelecom(contact.Telecom, "Organisation Contact Telecom");

                ValidateAddress(contact.Address, "Organisation Contact Address");

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

        [Then(@"the Organization PartOf Organization should be valid")]
        public void TheLocationPartOfLocationShouldBeValid()
        {
            Organizations.ForEach(organization =>
            {
                organization.PartOf?.Reference?.ShouldStartWith("Organization/", Case.Sensitive, "The reference element within the PartOf element of the Organization resource should contain a relative Organization reference.");
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




        [Then(@"the Organization Identifiers are correct for Organization Code ""([^""]*)""")]
        public void OrganizationIdentifiersAreCorrectForOrganizationCode(string organizationCode)
        {
            Organizations.ForEach(organization =>
            {
                //Check if Organization Code Identifiers are valid.
                OrganizationCodeIdentifiersAreValid(organizationCode, organization);

            });
        }

        private static void OrganizationCodeIdentifiersAreValid(string organizationCode, Organization organization)
        {
            //Get Organization Codes for Organization
            var organizationCodes = new List<string> { GlobalContext.OdsCodeMap[organizationCode] };

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

        [Then(@"an organization returned in the bundle has ""([^""]*)"" ""([^""]*)"" system identifier with ""([^""]*)""")]
        public void ThenAnOrganizationReturnedInTheBundleHasSystemIdentifier(int orgCount, string orgSystem, string orgCode)
        {
            var totalValidOrganisations = 0;

            Organizations.ForEach(organization =>
            {
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

                }

                if ((orgLoopCounter == orgCount))
                {
                    totalValidOrganisations++;
                }
            });

            totalValidOrganisations.ShouldBe(Organizations.Count, "The number of organizations codes are invalid");
        }

        private static List<string> getIdentifiersInList(string code)
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

        public void StoreTheOrganization(string siteCode)
        {
            var organization = Organizations.FirstOrDefault(orgz => string.IsNullOrEmpty(siteCode) || (!string.IsNullOrEmpty(siteCode) && orgz.Identifier.Select(zi => zi.Value).ToList().Contains(GlobalContext.OdsCodeMap[siteCode])));
            if (organization != null)
            {
                _httpContext.HttpRequestConfiguration.GetRequestId = organization.Id;
                _fhirResourceRepository.Organization = organization;
            }
            else
            {
                throw new Exception("Organization cannot be stored. Organization equal to null");
            }
        }
    }
}


