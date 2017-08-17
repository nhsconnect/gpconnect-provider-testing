namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using Context;
    using Hl7.Fhir.Model;
    using Shouldly;
    using System.Collections.Generic;
    using Enum;
    using TechTalk.SpecFlow;
    using System.Linq;
    using Repository;

    [Binding]
    public class PractitionerSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private readonly BundleSteps _bundleSteps;
        private readonly OrganizationSteps _organizationSteps;
        private readonly HttpResponseSteps _httpResponseSteps;
        private readonly IFhirResourceRepository _fhirResourceRepository;

        private List<Practitioner> Practitioners => _httpContext.FhirResponse.Practitioners;

        public PractitionerSteps(HttpContext httpContext, HttpSteps httpSteps, BundleSteps bundleSteps, OrganizationSteps organizationSteps, HttpResponseSteps httpResponseSteps, IFhirResourceRepository fhirResourceRepository) 
            : base(httpSteps)
        {
            _httpContext = httpContext;
            _bundleSteps = bundleSteps;
            _organizationSteps = organizationSteps;
            _httpResponseSteps = httpResponseSteps;
            _fhirResourceRepository = fhirResourceRepository;
        }

        [Given(@"I add a Practitioner Identifier parameter with System ""([^""]*)"" and Value ""([^""]*)""")]
        public void AddAPractitionerIdentifierParameterWithSystemAndValue(string system, string value)
        {
            string practitionerCode;

            GlobalContext.PractionerCodeMap.TryGetValue(value, out practitionerCode);

            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("identifier", system + '|' + practitionerCode);
        }

        [Given(@"I add a Practitioner Identifier parameter with SDS User Id System and Value ""([^""]*)""")]
        public void AddAPractitionerIdentifierParameterWithSdsUserIdSystemAndValue(string value)
        {
            string practitionerCode;

            GlobalContext.PractionerCodeMap.TryGetValue(value, out practitionerCode);

            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("identifier", "https://fhir.nhs.uk/Id/sds-user-id" + '|' + GlobalContext.PractionerCodeMap[value]);
        }

        [Given(@"I add a Practitioner ""([^""]*)"" parameter with System ""([^""]*)"" and Value ""([^""]*)""")]
        public void AddAPractitionerParameterWithSystemAndValue(string identifier, string system, string value)
        {
            string practitionerCode;

            GlobalContext.PractionerCodeMap.TryGetValue(value, out practitionerCode);

            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter(identifier, system + '|' + practitionerCode);
        }

        [Given(@"I get the Practitioner for Practitioner Code ""([^""]*)""")]
        public void GetThePractitionerForPractitionerCode(string code)
        {
            _httpSteps.ConfigureRequest(GpConnectInteraction.PractitionerSearch);

            AddAPractitionerIdentifierParameterWithSystemAndValue("https://fhir.nhs.uk/Id/sds-user-id", code);

            _httpSteps.MakeRequest(GpConnectInteraction.PractitionerSearch);
        }

        [Given(@"I store the Practitioner")]
        public void StoreThePractitioner()
        {
            var practitioner = Practitioners.FirstOrDefault();
            if (practitioner != null)
            {
                _httpContext.HttpRequestConfiguration.GetRequestId = practitioner.Id;
                _fhirResourceRepository.Practitioner = practitioner;
            }
        }

        [Given(@"I store the Practitioner Version Id")]
        public void StoreThePractitionerVersionId()
        {
            var practitioner = Practitioners.FirstOrDefault();
            if (practitioner != null)
                _httpContext.HttpRequestConfiguration.GetRequestVersionId = practitioner.VersionId;
        }

        [Then(@"the Response Resource should be a Practitioner")]
        public void ResponseResourceShouldBeAPractitioner()
        {
            _httpContext.FhirResponse.Resource.ResourceType.ShouldBe(ResourceType.Practitioner);
        }

        [Then("the Practitioner Id should be valid")]
        public void ThePractitionerIdShouldBeValid()
        {
            Practitioners.ForEach(practitioner =>
            {
                practitioner.Id.ShouldNotBeNullOrEmpty($"The Practitioner Id should not be null or empty but was {practitioner.Id}.");
            });
        }

        [Then("the Practitioner Id should equal the Request Id")]
        public void ThePractitionerIdShouldEqualTheRequestId()
        {
            Practitioners.ForEach(practitioner =>
            {
                practitioner.Id.ShouldBe(_httpContext.HttpRequestConfiguration.GetRequestId,
                    $"The Practitioner Id should be equal to {_httpContext.HttpRequestConfiguration.GetRequestId} but was {practitioner.Id}.");
            });
        }

        [Then(@"the Practitioner should be valid")]
        public void ThePractitionerShouldBeValid()
        {
            ThePractitionerIdentifiersShouldBeValid();
            ThePractitionerNameShouldBeValid();
            ThePractitionerExcludeDisallowedElements();
            ThePractitionerPractitionerRolesShouldBeValid();
            ThePractitionerCommunicationShouldBeValid();
            ThePractitionerPractitionerRolesManagingOrganizationShouldBeReferencedInTheBundle();
            ThePractitionerNameShouldBeValid();
        }

        [Then(@"the Practitioner Metadata should be valid")]
        public void ThePractitionerMetadataShouldBeValid()
        {
            Practitioners.ForEach(practitioner =>
            {
                CheckForValidMetaDataInResource(practitioner, "http://fhir.nhs.net/StructureDefinition/gpconnect-practitioner-1");
            });
        }

        [Then(@"the Practitioner Identifiers should be valid")]
        public void ThePractitionerIdentifiersShouldBeValid()
        {
            ThePractitionerIdentifiersShouldBeFixedValues();
            ThePractitionerSdsUserIdentifierShouldBeValid();
            ThePractitionerSdsRoleProfileIdentifierShouldBeValid();
        }

        [Then(@"the Practitioner Identifiers should be valid for Practitioner ""([^""]*)""")]
        public void ThePractitionerIdentifiersShouldBeValid(string practitionerName)
        {
            ThePractitionerIdentifiersShouldBeFixedValues();
            ThePractitionerSdsUserIdentifierShouldBeValid(practitionerName, false);
            ThePractitionerSdsRoleProfileIdentifierShouldBeValid();
        }

        [Then(@"the Practitioner Identifiers should be valid fixed values")]
        public void ThePractitionerIdentifiersShouldBeFixedValues()
        {
            Practitioners.ForEach(practitioner =>
            {
                practitioner.Identifier.ForEach(identifier =>
                {
                    identifier.System.ShouldBeOneOf("https://fhir.nhs.uk/Id/sds-user-id", "http://fhir.nhs.net/Id/sds-role-profile-id");
                });
            });
        }

        [Then(@"the Practitioner SDS Role Profile Identifier should be valid")]
        public void ThePractitionerSdsRoleProfileIdentifierShouldBeValid()
        {
            ThePractitionerSdsRoleProfileIdentifierShouldBeValid(null);
        }

        [Then(@"the Practitioner SDS Role Profile Identifier should be valid for ""([^""]*)"" Role Profile Identifiers")]
        public void ThePractitionerSdsRoleProfileIdentifierShouldBeValidForRoleProfileIdentifiers(int roleProfileCount)
        {
            ThePractitionerSdsRoleProfileIdentifierShouldBeValid(roleProfileCount);
        }

        private void ThePractitionerSdsRoleProfileIdentifierShouldBeValid(int? expectedTotalRoleProfileCount)
        {
            var actualTotalRoleProfileCount = 0;

            Practitioners.ForEach(practitioner =>
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

        [Then(@"the Practitioner SDS User Identifier should be valid for Value ""([^""]*)""")]
        public void ThePractitionerSdsUserIdentifierShouldBeValidForValue(string value)
        {
            ThePractitionerSdsUserIdentifierShouldBeValid(value, true);
        }

        [Then(@"the Practitioner SDS User Identifier should be valid")]
        public void ThePractitionerSdsUserIdentifierShouldBeValid()
        {
            ThePractitionerSdsUserIdentifierShouldBeValid(null, false);
        }

        private void ThePractitionerSdsUserIdentifierShouldBeValid(string practitionerName, bool shouldBeSingle)
        {
            Practitioners.ForEach(practitioner =>
            {
                var sdsUserIdentifiers = practitioner.Identifier.Where(identifier => identifier.System.Equals("https://fhir.nhs.uk/Id/sds-user-id")).ToList();

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

                    if (!string.IsNullOrEmpty(practitionerName))
                    {
                        identifier.Value.ShouldBe(GlobalContext.PractionerCodeMap[practitionerName]);
                    }
                });
            });
        }

        [Then(@"the Practitioner Name should be valid")]
        public void ThePractitionerNameShouldBeValid()
        {
            Practitioners.ForEach(practitioner =>
            { 
                practitioner.Name.ShouldNotBeNull("Practitioner resources must contain a name element");
                practitioner.Name.Family?.Count().ShouldBeLessThanOrEqualTo(1, "There must be a maximum of 1 family name in the practitioner name.");
            });
        }

        [Then(@"the Practitioner should exclude disallowed elements")]
        public void ThePractitionerExcludeDisallowedElements()
        {
            Practitioners.ForEach(practitioner =>
            {
                practitioner.Photo?.Count.ShouldBe(0, "Practitioner should not contain a Photo");
                practitioner.Qualification?.Count.ShouldBe(0, "Practitioner should not contain a Qualification");
                practitioner.BirthDate.ShouldBeNull("Practitioner should not contain a Birth Date");
                practitioner.BirthDateElement.ShouldBeNull("Practitioner should not contain a Birth Date Element");

                practitioner.PractitionerRole.ForEach(practitionerRole =>
                {
                    practitionerRole.HealthcareService?.Count.ShouldBe(0, "Practitioner Role should not contain a Healthcare Service");
                    practitionerRole.Location?.Count.ShouldBe(0, "Practitioner Role should not contain a Location");
                });
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
            Practitioners.ForEach(practitioner =>
            {
                practitioner.PractitionerRole.ForEach(practitionerRole =>
                {
                    if (practitionerRole.Role?.Coding != null)
                    {
                        practitionerRole.Role.Coding.Count.ShouldBeLessThanOrEqualTo(1, "There should be a maximum of one practitioner role coding in each practitioner role.");
                        practitionerRole.Role.Coding.ForEach(coding =>
                        {
                            coding.System.ShouldBe("http://fhir.nhs.net/ValueSet/sds-job-role-name-1");
                            coding.Code.ShouldNotBeNull("The practitioner role code element should not be null");
                            coding.Display.ShouldNotBeNull("The practitioner role display elemenet should not be null");
                        });
                    }
                });
            });
        }

        [Then(@"the Practitioner Communication should be valid")]
        public void ThePractitionerCommunicationShouldBeValid()
        {
            Practitioners.ForEach(practitioner =>
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
            Practitioners.ForEach(practitioner =>
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

        [Then(@"the Practitioner PractitionerRoles ManagingOrganization should be valid and resolvable")]
        public void ThePractitionerPractitionerRolesManagingOrganizationShouldBeValidAndResolvable()
        {
            Practitioners.ForEach(practitioner =>
            {
                practitioner.PractitionerRole.ForEach(practitionerRole =>
                {
                    if (practitionerRole.ManagingOrganization != null)
                    {
                        practitionerRole.ManagingOrganization.Reference.ShouldNotBeNull("If a Practitioner has a Managing Organization it must have a reference");
                        practitionerRole.ManagingOrganization.Reference.ShouldStartWith("Organization/");

                        _httpSteps.ConfigureRequest(GpConnectInteraction.OrganizationRead);

                        _httpContext.HttpRequestConfiguration.RequestUrl = practitionerRole.ManagingOrganization.Reference;

                        _httpSteps.MakeRequest(GpConnectInteraction.OrganizationRead);

                        _httpResponseSteps.ThenTheResponseStatusCodeShouldIndicateSuccess();

                        _organizationSteps.StoreTheOrganization();

                        var returnedReference = _fhirResourceRepository.Organization.ResourceIdentity().ToString();

                        returnedReference.ShouldStartWith(practitionerRole.ManagingOrganization.Reference);
                    }
                });
            });
        }
    }
}


