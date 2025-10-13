namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using Cache;
    using Cache.ValueSet;
    using Constants;
    using Context;
    using Enum;
    using Helpers;
    using Hl7.Fhir.Model;
    using Repository;
    using Shouldly;
    using TechTalk.SpecFlow;
    using Extensions;
    using System;

    [Binding]
    public class PatientSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private readonly BundleSteps _bundleSteps;
        private readonly JwtSteps _jwtSteps;
        private readonly HttpRequestConfigurationSteps _httpRequestConfigurationSteps;
        private readonly IFhirResourceRepository _fhirResourceRepository;
        private List<Patient> Patients => _httpContext.FhirResponse.Patients;

        public PatientSteps(HttpSteps httpSteps, HttpContext httpContext, BundleSteps bundleSteps, JwtSteps jwtSteps, HttpRequestConfigurationSteps httpRequestConfigurationSteps, IFhirResourceRepository fhirResourceRepository) : base(httpSteps)
        {
            _httpContext = httpContext;
            _bundleSteps = bundleSteps;
            _jwtSteps = jwtSteps;
            _httpRequestConfigurationSteps = httpRequestConfigurationSteps;
            _fhirResourceRepository = fhirResourceRepository;
        }


        [Then(@"the Response Resource should be a Patient")]
        public void TheResponseResourceShouldBeAPatient()
        {
            _httpContext.FhirResponse.Resource.ResourceType.ShouldBe(ResourceType.Patient);
        }

        [Then(@"the Patient Id should be valid")]
        public void ThePatienIdShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                patient.Id.ShouldNotBeNullOrWhiteSpace("Id must be set");
            });
        }

        [Then(@"the Patient Id should equal the Request Id")]
        public void ThePatienIdShouldEqualTheRequestId()
        {
            Patients.ForEach(patient =>
            {
                patient.Id.ShouldBe(_httpContext.HttpRequestConfiguration.GetRequestId, $"The Patient Id should be equal to {_httpContext.HttpRequestConfiguration.GetRequestId} but was {patient.Id}.");
            });
        }

        [Then(@"the Patient Metadata should be valid")]
        public void ThePatientMetadataShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                CheckForValidMetaDataInResource(patient, FhirConst.StructureDefinitionSystems.kPatient);
            });
        }

        [Then(@"the Patient Deceased should be valid")]
        public void ThePatientDeceasedShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                patient.Deceased?.ShouldBeOfType<FhirDateTime>();
            });
        }

        [Then(@"the Patient MultipleBirth should be valid")]
        public void ThePatientMultipleBirthShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                patient.MultipleBirth?.ShouldBeOfType<FhirBoolean>("Multiple Birth must be of type FhirBoolean");



            });
        }

        [Then(@"the Patient Contact Relationship should be valid")]
        public void ThePatientContactRelationshipShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                patient.Contact.ForEach(contact =>
                {
                    contact.Relationship.ForEach(relationship =>
                    {
                        var valueSet = ValueSetCache.Get(FhirConst.ValueSetSystems.kVsRelationshipStatus);

                        ShouldBeSingleCodingWhichIsInValueSet(valueSet, relationship.Coding);
                    });
                });
            });
        }

        [Then(@"the Patient Contact Name should be valid")]
        public void ThePatientContactNameShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                patient.Contact.ForEach(contact =>
                {
                    contact.Name.Family.Count().ShouldBe(1, "There should be 1 family name");

                    contact.Name.Use.ShouldNotBeNull("Contact Name Use should not be null");
                    contact.Name.Use.ShouldBeOfType<HumanName.NameUse>($"Patient Contact Name Use is not a valid value within the value set {FhirConst.CodeSystems.kNameUse}");
                    contact.Gender?.ShouldBeOfType<AdministrativeGender>($"Type is not a valid value within the value set {FhirConst.CodeSystems.kAdministrativeGender}");
                });
            });
        }

        [Then(@"the Patient Name should be valid")]
        public void ThePatientNameShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                var officialNameCount = patient.Name.Count(name => name.Use == HumanName.NameUse.Official);

                officialNameCount.ShouldBe(1, $"The Patient Name should contain 1 Official name but found {officialNameCount}.");

                patient.Name.ShouldNotBeNull("Patient name should not be null");
                patient.Name.ForEach(name =>
                {
                    name.Family.ShouldNotBeNullOrEmpty("Patient Family Name cannot be null");
                });
            });
        }
        // git hub ref 118
        // RMB 25/10/2018
        [Then(@"the Patient Contact Telecom use should be valid")]
        public void ThePatientContactTelecomUseShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                patient.Contact.ForEach(contact =>
                {
                    contact.Telecom.ForEach(telecom =>
                    {
                        telecom.Use.ShouldNotBeNull();
                    });

                });
            });
        }

        [Then(@"the Patient Identifiers should be valid")]
        public void ThePatientIdentifiersShouldBeValid()
        {
            ThePatientIdentifiersShouldBeValid(null);
        }

        [Then(@"the Patient Identifiers should be valid for Patient ""([^""]*)""")]
        public void ThePatientIdentifiersShouldBeValidForPatient(string patient)
        {
            ThePatientIdentifiersShouldBeValid(patient);
        }

        private void ThePatientIdentifiersShouldBeValid(string patientName)
        {
            Patients.ForEach(patient =>
            {
                patient.Identifier.Count.ShouldBeLessThanOrEqualTo(1);
                if (patient.Identifier.Count == 1)
                {
                    var identifier = patient.Identifier.First();

                    FhirConst.IdentifierSystems.kNHSNumber.Equals(identifier.System).ShouldBeTrue();
                    NhsNumberHelper.IsNhsNumberValid(identifier.Value).ShouldBeTrue();

                    if (!string.IsNullOrEmpty(patientName))
                    {
                        identifier.Value.ShouldBe(GlobalContext.PatientNhsNumberMap[patientName]);
                    }

                    var extension = identifier.Extension.First();
                    ValidateCodeConceptExtension(extension, FhirConst.ValueSetSystems.kVsNhsNumVerification);

                }
            });
        }

        [Then(@"the Patient Telecom should be valid")]
        public void ThePatientTelecomShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                patient.Telecom.ForEach(telecom =>
                {
                    telecom.System.ShouldNotBeNull("The telecom system should not be null");
                    telecom.Value.ShouldNotBeNull("The telecom value element should not be null");
                    //telecom.System.ShouldBeOfType<ContactPoint.ContactPointSystem>(string.Format("{0} System is not a valid value within the value set {1}", FhirConst.ValueSetSystems.kContactPointSystem));
                    // git hub ref 121
                    // RMB 25/10/2018
                    telecom.Use.ShouldNotBeNull("The telecom use element should not be null");

                });
            });
        }

        [Then(@"the Patient RegistrationDetails should include preferredBranchSurgery")]
        public void ThePatientRegistrationDetailsShouldIncludePreferredBranchSurgery()
        {
            Patients.ForEach(patient =>
            {
                var registrationDetailsExtensions = patient.Extension.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kExtCcGpcRegDetails)).ToList();

                registrationDetailsExtensions.Count.ShouldBe(1, "Incorrect number of registration details extension have been returned. This should be 1.");

                var regDetailsExtension = registrationDetailsExtensions.First();
                var regExtensions = regDetailsExtension.Extension;

                var extensions = regExtensions.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kCCExtPreferredBranchSurgery)).ToList();

                extensions.Count.ShouldBeLessThanOrEqualTo(1, "The patient resource should contain a maximum of 1 Preferred Branch Surgery extension.");

                extensions.ForEach(preferredBranchSurgeryExtension =>
                {
                    preferredBranchSurgeryExtension.Value.ShouldNotBeNull("The Preferred Branch Surgery extension should have a value element.");
                    preferredBranchSurgeryExtension.Value.ShouldBeOfType<ResourceReference>("The Preferred Branch Surgery extension should be a Period.");

                    preferredBranchSurgeryExtension.Value.ShouldNotBeNull();
                });

            });
        }

        [Then(@"the Patient MaritalStatus should be valid")]
        public void ThePatientMaritalStatusShouldbeValid()
        {
            Patients.ForEach(patient =>
            {
                if (patient.MaritalStatus != null)
                {
                    patient.MaritalStatus.Coding.ShouldNotBeNull("Patient MaritalStatus coding cannot be null");


                    var maritalStatusList = ValueSetCache.Get(FhirConst.ValueSetSystems.kVsMaritalStatus).WithComposeIncludes().ToArray();
                    maritalStatusList.Concat(ValueSetCache.Get(FhirConst.ValueSetSystems.kVsNullFlavour).WithComposeIncludes().ToArray());
                    patient.MaritalStatus.Coding.ForEach(coding =>
                    {
                        coding.System.ShouldNotBeNull("MaritalStatus System should not be null");
                        coding.Code.ShouldBeOneOf(maritalStatusList.Select(c => c.Code).ToArray());
                        coding.Display.ShouldBeOneOf(maritalStatusList.Select(c => c.Display).ToArray());
                    });
                }
            });
        }

        [Then(@"the Patient Communication should be valid")]
        public void ThePatientCommunicationShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                patient.Communication?.ForEach(communication =>
                {
                    communication.Language.ShouldNotBeNull("The communication language element should not be null");

                    var valueSet = ValueSetCache.Get(FhirConst.ValueSetSystems.kVsHumanLanguage);

                    ShouldBeSingleCodingWhichIsInValueSet(valueSet, communication.Language.Coding);
                });
            });
        }

        [Then(@"the Patient GeneralPractitioner Practitioner should be valid and resolvable")]
        public void ThePatientGeneralPractitionerPractitionerShouldBeValidAndResolvable()
        {
            Patients.ForEach(patient =>
            {
                if (patient.GeneralPractitioner != null)
                {
                    patient.GeneralPractitioner.Count.ShouldBeLessThanOrEqualTo(1);

                    if (patient.GeneralPractitioner.Count.Equals(1))
                    {
                        var reference = patient.GeneralPractitioner.First().Reference;

                        reference.ShouldStartWith("Practitioner/");

                        var resource = _httpSteps.GetResourceForRelativeUrl(GpConnectInteraction.PractitionerRead, reference);

                        resource.GetType().ShouldBe(typeof(Practitioner));
                    }
                }
            });
        }

        [Then(@"the Patient GeneralPractitioner Practitioner should be referenced in the Bundle")]
        public void ThePatientGeneralPractitionerShouldBeReferencedInTheBundle()
        {
            Patients.ForEach(patient =>
            {
                if (patient.GeneralPractitioner != null)
                {
                    patient.GeneralPractitioner.Count.ShouldBeLessThanOrEqualTo(1);

                    if (patient.GeneralPractitioner.Count.Equals(1))
                    {
                        _bundleSteps.ResponseBundleContainsReferenceOfType(patient.GeneralPractitioner.First().Reference, ResourceType.Practitioner);
                    }
                }
            });
        }

        [Then(@"the Patient ManagingOrganization Organization should be valid and resolvable")]
        public void ThePatientManagingOrganizationOrganizationShouldBeValidAndResolvable()
        {
            Patients.ForEach(patient =>
            {
                if (patient.ManagingOrganization != null)
                {
                    var reference = patient.ManagingOrganization.Reference;

                    reference.ShouldStartWith("Organization/");

                    var resource = _httpSteps.GetResourceForRelativeUrl(GpConnectInteraction.OrganizationRead, reference);

                    resource.GetType().ShouldBe(typeof(Organization));
                }
            });
        }

        [Then(@"the Patient ManagingOrganization Organization should be referenced in the Bundle")]
        public void ThenPatientManagingOrganizationOrganizationShouldBeRefrencedInTheBundle()
        {
            Patients.ForEach(patient =>
            {
                if (patient.ManagingOrganization != null)
                {
                    _bundleSteps.ResponseBundleContainsReferenceOfType(patient.ManagingOrganization.Reference, ResourceType.Organization);

                }
            });
        }

        [Then(@"the Patient ManagingOrganization Should be Valid")]
        public void ThenthePatientManagingOrganizationShouldbeValidandincludedintheBundle()
        {
            Patients.ForEach(patient =>
            {
                patient.ManagingOrganization.ShouldNotBeNull("Fail : Patient ManagingOrganization Should not be null - mandatory element");

                if (patient.ManagingOrganization != null)
                {
                    var reference = patient.ManagingOrganization.Reference;

                    reference.ShouldStartWith("Organization/");

                    var resource = _httpSteps.GetResourceForRelativeUrl(GpConnectInteraction.OrganizationRead, reference);

                    resource.GetType().ShouldBe(typeof(Organization));

                }
            });
        }

        [Then(@"the Patient should exclude disallowed fields")]
        public void ThePatientShouldExcludeFields()
        {
            Patients.ForEach(patient =>
            {
                // C# API creates an empty list if no element is present
                patient.Photo?.Count.ShouldBe(0, "There should be no photo element in Patient Resource");
                patient.Link?.Count.ShouldBe(0, "There should be no link element in Patient Resource");
                patient.Animal.ShouldBeNull("There should be no Animal element in Patient Resource");
            });
        }

        [Then(@"the Patient Contact should be valid")]
        public void ThePatientContactShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                patient.Contact.ForEach(contact =>
                {
                    // Contact Relationship Checks
                    contact.Relationship.ForEach(relationship =>
                    {
                        var valueSet = ValueSetCache.Get(FhirConst.ValueSetSystems.kVsRelationshipStatus);

                        ShouldBeSingleCodingWhichIsInValueSet(valueSet, relationship.Coding);
                        // github ref 126
                        // RMB 29/10/2018
                        relationship.Text.ShouldNotBeNull();
                        relationship.Coding.Count.ShouldBe(0);
                    });

                    //contact.Name.ShouldBeNull();
                    contact.Name.Use.ShouldNotBeNull("Patient Name Use cannot be null");
                    contact.Name.Use.ShouldBeOfType<HumanName.NameUse>($"Patient Name Use is not a valid value within the value set {FhirConst.CodeSystems.kNameUse}");
                    //contact.Name.Family.Count().ShouldBeLessThanOrEqualTo(1);
                    // Contact Name Checks
                    // Contact Telecom Checks
                    // Contact Address Checks
                    // Contact Gender Checks
                    // No mandatory fields and value sets are standard so will be validated by parse of response to fhir resource

                    // Contact Organization Checks
                    if (contact.Organization?.Reference != null)
                    {
                        _httpContext.FhirResponse.Entries.ShouldContain(
                            entry => entry.Resource.ResourceType.Equals(ResourceType.Organization) &&
                            entry.FullUrl.Equals(contact.Organization.Reference)
                        );
                    }
                });
            });
        }

        [Given(@"I add a Patient Identifier parameter with System ""([^""]*)"" and Value ""([^""]*)""")]
        public void AddAPatientIdentifierParameterWithSystemAndValue(string system, string value)
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("identifier", system + '|' + GlobalContext.PatientNhsNumberMap[value]);
        }

        [Given(@"I add a Patient Identifier parameter with default System and Value ""([^""]*)""")]
        public void AddAPatientIdentifierParameterWithDefaultSystemAndValue(string value)
        {
            AddAPatientIdentifierParameterWithSystemAndValue(FhirConst.IdentifierSystems.kNHSNumber, value);
        }

        [Given(@"I add a Patient Identifier parameter with identifier name ""([^""]*)"" default System and Value ""([^""]*)""")]
        public void AddAPatientIdentifierParameterWithIdentifierNameDefaultSystemAndValue(string identifierName, string value)
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter(identifierName, FhirConst.IdentifierSystems.kNHSNumber + '|' + GlobalContext.PatientNhsNumberMap[value]);
        }

        [Given(@"I add a Patient Identifier parameter with default System and NHS number ""([^""]*)""")]
        public void AddAPatientIdentifierParameterWithDefaultSystemAndNhsNumber(string nhsNumber)
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("identifier", FhirConst.IdentifierSystems.kNHSNumber + '|' + nhsNumber);
        }

        [Given(@"I add a Patient Identifier parameter with no System and Value ""([^""]*)""")]
        public void AddAPatientIdentifierParameterWithNoSystemAndValue(string value)
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("identifier", GlobalContext.PatientNhsNumberMap[value]);
        }

        [Given(@"I get an existing patients nshNumber")]
        public void GetTheRandomPatientValue()
        {
            var value = "patient1";

            if (AppSettingsHelper.RandomPatientEnabled == true)
            {
                value = RandomPatientSteps.ReturnRandomPatient();
            }

            _httpSteps.ConfigureRequest(GpConnectInteraction.PatientSearch);

            AddAPatientIdentifierParameterWithDefaultSystemAndValue(value);

            _httpSteps.MakeRequest(GpConnectInteraction.PatientSearch);
        }


        [Given(@"I get the Patient for Patient Value ""([^""]*)""")]
        public void GetThePatientForPatientValue(string value)
        {
            _httpSteps.ConfigureRequest(GpConnectInteraction.PatientSearch);

            AddAPatientIdentifierParameterWithDefaultSystemAndValue(value);

            _httpSteps.MakeRequest(GpConnectInteraction.PatientSearch);
        }

        [Given(@"I get the Patient for Patient NHS number ""([^""]*)""")]
        public void GetThePatientForPatientNhsNumber(string nhsNumber)
        {
            _httpSteps.ConfigureRequest(GpConnectInteraction.PatientSearch);

            AddAPatientIdentifierParameterWithDefaultSystemAndNhsNumber(nhsNumber);

            _httpSteps.MakeRequest(GpConnectInteraction.PatientSearch);
        }

        [Given(@"I store the Patient")]
        public void StoreThePatient()
        {
            var patient = Patients.FirstOrDefault();

            if (patient != null)
            {
                _httpContext.HttpRequestConfiguration.GetRequestId = patient.Id;
                _fhirResourceRepository.Patient = patient;
            }
            else
            {
                patient.ShouldNotBeNull("Stored patient is null.");
            }
        }

        [Given(@"I store the Patient Version Id")]
        public void StoreThePatientVersionId()
        {
            var patient = Patients.FirstOrDefault();

            if (patient != null)
            {
                _httpContext.HttpRequestConfiguration.GetRequestVersionId = patient.VersionId;
            }
        }

        [Given(@"I set an invalid Patient Version Id")]
        public void SetAnInvlalidPatientVersionId()
        {
            _httpContext.HttpRequestConfiguration.GetRequestVersionId = "1234567890";
        }

        [Given(@"I set the If-None-Match header to the stored Patient Version Id")]
        public void SetTheIfNoneMatchHeaderToTheStoredPatientVersionId()
        {
            var patient = _fhirResourceRepository.Patient;

            if (patient != null)
                _httpRequestConfigurationSteps.GivenISetTheIfNoneMatchheaderHeaderTo("W/\"" + patient.VersionId + "\"");
        }

        [Then(@"the Patient Use should be valid")]
        public void ThePatientUseShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                patient.Name.ForEach(name =>
                {
                    // Contact Relationship Checks

                    name.Use.ShouldNotBeNull("Patient Name Use cannot be null");
                    name.Use.ShouldBeOfType<HumanName.NameUse>(string.Format("Patient Name Use is not a valid value within the value set {0}", FhirConst.CodeSystems.kNameUse));
                });
            });
        }

        [Then(@"the Patient Family Name should be valid")]
        public void ThePatientFamilyShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                patient.Name.ForEach(name =>
                {
                    // Contact Relationship Checks

                    name.Family.ShouldNotBeNull("Patient Name Family cannot be null");
                    name.Family.Count().ShouldBe(1, "Patient family must be populated");
                });
            });
        }

        [Then(@"the Patient Gender should be valid")]
        public void ThePatientGenderShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                patient.Gender.ShouldBeOfType<AdministrativeGender>($"Patient Gender is not a valid value within the value set {FhirConst.CodeSystems.kAdministrativeGender}");
            });
        }

        [Then(@"the Patient DOB should be valid")]
        public void ThePatientDOBShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                patient.BirthDate.ShouldNotBeNullOrEmpty("Fail Patient DOB should not be null or empty");
            });
        }

        [Then(@"the Patient Link should be valid and resolvable")]
        public void ThePatientLinkShouldBeValidAndResolvable()
        {
            Patients.ForEach(patient =>
            {
                patient.Link.ForEach(link =>
                {
                    link.Type.ShouldNotBeNull("The Patient Link Type should not be null, but was.");
                    link.Type.ShouldBeDefinedIn(typeof(Patient.LinkType), $"The Patient Link Type ({link.Type.ToString()}) was an invalid value.");

                    var reference = link.Other.Reference;

                    //Can't check RelatedPerson as endpoint doesn't exist
                    if (reference.StartsWith("Patient"))
                    {
                        var resource = _httpSteps.GetResourceForRelativeUrl(GpConnectInteraction.PatientRead, reference);

                        resource.GetType().ShouldBe(typeof(Patient));
                    }
                });
            });
        }

        // github ref 120
        // RMB 25/10/2018        
        [Then(@"the Patient Not In Use should be valid")]
        public void ThePatientNotInUseShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                patient.MaritalStatus.ShouldBeNull();
                patient.MultipleBirth.ShouldBeNull();
                // EXTENSIONS
                var extensions = patient.Extension;
                ValidateNoExtension(extensions, FhirConst.StructureDefinitionSystems.kCCExtEthnicCategory);
                ValidateNoExtension(extensions, FhirConst.StructureDefinitionSystems.kCcExtReligiousAffiliation);
                ValidateNoExtension(extensions, FhirConst.StructureDefinitionSystems.kCCExtPatientCadaver);
                ValidateNoExtension(extensions, FhirConst.StructureDefinitionSystems.kCCExtResidentialStatus);
                ValidateNoExtension(extensions, FhirConst.StructureDefinitionSystems.kCCExtTreatmentCategory);

            });
        }

        private void ValidateNoExtension(List<Extension> extensions, string defUri)
        {
            var exts = extensions.Where(
                ece => ece.Url.Equals(defUri)).ToList();
            exts.Count.ShouldBeLessThanOrEqualTo(0);

        }

        [Then(@"the Patient Registration Details should be valid")]
        public void ThePatientRegistrationDetailsShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                var registrationDetailsExtensions = patient.Extension.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kExtCcGpcRegDetails)).ToList();
                if (registrationDetailsExtensions.Count() > 0)
                {
                    var regDetailsExtension = registrationDetailsExtensions.First();
                    var regExtensions = regDetailsExtension.Extension;

                    //Check the registrationPeriod is valid
                    var extensions = regExtensions.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kCCExtRegistrationPeriod)).ToList();
                    extensions.Count.ShouldBeLessThanOrEqualTo(1, "The patient resource should contain a maximum of 1 registration period extension.");
                    extensions.ForEach(registrationPeriodExtension =>
                    {
                        registrationPeriodExtension.Value.ShouldNotBeNull("The registration period extension should have a value element.");
                        registrationPeriodExtension.Value.ShouldBeOfType<Period>("The registration period extension should be a Period.");
                    });

                    //Check the registrationPeriod is valid
                    extensions = regExtensions.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kCCExtRegistrationType)).ToList();
                    extensions.Count.ShouldBeLessThanOrEqualTo(1, "The patient resource should contain a maximum of 1 registration type extension.");
                    extensions.ForEach(registrationTypeExtension =>
                    {
                        registrationTypeExtension.Value.ShouldNotBeNull("The registration type extension should have a value element.");
                    });

                    //Check the preferredBranchSurgery is valid
                    extensions = regExtensions.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kCCExtPreferredBranchSurgery)).ToList();
                    extensions.Count.ShouldBeLessThanOrEqualTo(1, "The patient resource should contain a maximum of 1 preferred branch surgery extension.");
                    extensions.ForEach(registrationTypeExtension =>
                    {
                        registrationTypeExtension.Value.ShouldNotBeNull("The preferred branch surgery extension should have a value element.");
                    });
                }

            });
        }

    }
}
