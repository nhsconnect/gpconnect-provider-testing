namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using Context;
    using Hl7.Fhir.Model;
    using Shouldly;
    using System.Collections.Generic;
    using Enum;
    using TechTalk.SpecFlow;
    using System.Linq;
    using Cache;
    using Cache.ValueSet;
    using Repository;
    using Constants;

    [Binding]
    public class PractitionerSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private readonly IFhirResourceRepository _fhirResourceRepository;

        private List<Practitioner> Practitioners => _httpContext.FhirResponse.Practitioners;

        public PractitionerSteps(HttpContext httpContext, HttpSteps httpSteps, IFhirResourceRepository fhirResourceRepository)
            : base(httpSteps)
        {
            _httpContext = httpContext;
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

            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("identifier", FhirConst.IdentifierSystems.kPracSDSUserId + '|' + GlobalContext.PractionerCodeMap[value]);
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

            AddAPractitionerIdentifierParameterWithSystemAndValue(FhirConst.IdentifierSystems.kPracSDSUserId, code);

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
            ThePractitionerCommunicationShouldBeValid();
        }

        [Then(@"the Practitioner Entry should be valid")]
        public void ThePractitionerShouldBeFullyValid()
        {
            ThePractitionerMetadataShouldBeValid();
            ThePractitionerSdsUserIdentifierShouldBeValid();
            ThePractitionerIdentifiersShouldBeFixedValues();
            ThePractitionerNameShouldBeValid();
            ThePractitionerExcludeDisallowedElements();
            ThePractitionerCommunicationShouldBeValid();
        }
        
        [Then(@"the Practitioner Metadata should be valid")]
        public void ThePractitionerMetadataShouldBeValid()
        {
            Practitioners.ForEach(practitioner =>
            {
                CheckForValidMetaDataInResource(practitioner, FhirConst.StructureDefinitionSystems.kPractitioner);
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
                    identifier.System.ShouldBeOneOf(FhirConst.IdentifierSystems.kPracSDSUserId, FhirConst.IdentifierSystems.kPracRoleProfile, FhirConst.IdentifierSystems.kPracGMP);
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
                var sdsRoleProfileIdentifiers = practitioner.Identifier.Where(identifier => identifier.System.Equals(FhirConst.IdentifierSystems.kPracRoleProfile)).ToList();

                if (expectedTotalRoleProfileCount != null)
                {
                    actualTotalRoleProfileCount = actualTotalRoleProfileCount + sdsRoleProfileIdentifiers.Count;
                }

                sdsRoleProfileIdentifiers.ForEach(identifier =>
                {
                    identifier.Value.ShouldNotBeNull("SDS Role Identifier Value should not be null");
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
                var sdsUserIdentifiers = practitioner.Identifier.Where(identifier => identifier.System.Equals(FhirConst.IdentifierSystems.kPracSDSUserId)).ToList();

                if (shouldBeSingle)
                {
                    sdsUserIdentifiers.Count.ShouldBe(1, "There should be 1 SDS User Identifier");
                }
                else
                {
                    sdsUserIdentifiers.Count.ShouldBeLessThanOrEqualTo(1);
                }

                sdsUserIdentifiers.ForEach(identifier =>
                {
                    identifier.Value.ShouldNotBeNull("Identifier value should not be null");

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
                practitioner.Name.Count.ShouldBe(1, $"There should be 1 Practitioner Name, but found {practitioner.Name}.");
                var practionerName = practitioner.Name.First();

                practionerName.Family.ShouldNotBeNullOrEmpty("There should be 1 Family Name in the Practitioner name.");

                practionerName.Use?.ShouldBeOfType<HumanName.NameUse>($"Practitioner Name Use is not a valid value within the value set {FhirConst.CodeSystems.kNameUse}");
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
            });
        }


        [Then(@"the Practitioner nhsCommunication should be valid")]
        public void ThePractitionerCommunicationShouldBeValid()
        {
            Practitioners.ForEach(practitioner =>
            {
                practitioner.Communication.ForEach(codeableConcept =>
                {
                    var valueSet = ValueSetCache.Get(FhirConst.ValueSetSystems.kVsHumanLanguage);

                    ShouldBeSingleCodingWhichIsInValueSet(valueSet, codeableConcept.Coding);
                });
            });
        }

        [Then(@"the practitioner Telecom should be valid")]
        public void ThenThePractitionerTelecomShouldBeValid()
        {
            Practitioners.ForEach(practitioner =>
            {
                ValidateTelecom(practitioner.Telecom, "Practitioner Telecom");
            });

        }

        [Then(@"the practitioner Address should be valid")]
        public void ThenThePractitionerAddressShouldBeValid()
        {
            Practitioners.ForEach(practitioner =>
            {
                ValidateAddress(practitioner.Address, "Practitioner Address");
            });

        }

        [Then(@"the practitioner Gender should be valid")]
        public void ThenThePractitionerGenderShouldBeValid()
        {
            Practitioners.ForEach(practitioner =>
            {
                practitioner.Gender?.ShouldBeOfType<AdministrativeGender>(string.Format($"Type is not a valid value within the value set {FhirConst.CodeSystems.kAdministrativeGender}"));
            });
        }

        // github ref 120
        // RMB 25/10/2018        
        [Then(@"the Practitioner Not In Use should be valid")]
        public void ThePractitionerNotInUseShouldBeValid()
        {
            Practitioners.ForEach(practitioner =>
            {
                practitioner.Telecom.Count.ShouldBe(0);
                practitioner.Address.Count.ShouldBe(0);
                practitioner.BirthDate.ShouldBeNull();
                practitioner.Photo?.Count.ShouldBe(0);
                practitioner.Qualification.Count.ShouldBe(0);

            });
        }

        private static void ValidateAddress(List<Address> addressList, string from)
        {
            addressList.ForEach(address =>
            {
                address.Extension.ForEach(ext => ext.Url.ShouldNotBeNullOrEmpty($"{from} has an invalid extension. Extensions must have a URL element."));
                address.Type?.ShouldBeOfType<Address.AddressType>($"{from} Type is not a valid value within the value set {FhirConst.CodeSystems.kAddressType}");
                address.Use?.ShouldBeOfType<Address.AddressUse>($"{from} Use is not a valid value within the value set {FhirConst.CodeSystems.kAddressUse}");
            });
        }


        private static void ValidateTelecom(List<ContactPoint> telecoms, string from)
        {
            telecoms.ForEach(teleCom =>
            {
                teleCom.Extension.ForEach(ext => ext.Url.ShouldNotBeNullOrEmpty($"{from} has an invalid extension. Extensions must have a URL element."));
                teleCom.System?.ShouldBeOfType<ContactPoint.ContactPointSystem>($"{from} System is not a valid value within the value set {FhirConst.CodeSystems.kContactPointSystem}");
                teleCom.Use?.ShouldBeOfType<ContactPoint.ContactPointUse>($"{from} Use is not a valid value within the value set {FhirConst.CodeSystems.kNContactPointUse}");
            });
        }

		//#320 SJD 04/12/2019 
		[Given(@"I add a Practitioner Identifier parameter with unknown value")]
		public void AddAPractitionerIdentifierParameterWithUnknownValue()
		{
			_httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("identifier", FhirConst.IdentifierSystems.kPracSDSUserId + '|' + "unknownSDSUserID");
		}

	}
}


