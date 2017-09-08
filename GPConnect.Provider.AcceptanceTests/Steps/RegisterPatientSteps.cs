using GPConnect.Provider.AcceptanceTests.Extensions;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Context;
    using Hl7.Fhir.Model;
    using Repository;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    public class RegisterPatientSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private readonly PatientSteps _patientSteps;
        private readonly IFhirResourceRepository _fhirResourceRepository;
        private List<Patient> Patients => _httpContext.FhirResponse.Patients;

        public RegisterPatientSteps(HttpSteps httpSteps, HttpContext httpContext, PatientSteps patientSteps, IFhirResourceRepository fhirResourceRepository)
            : base(httpSteps)
        {
            _httpContext = httpContext;
            _patientSteps = patientSteps;
            _fhirResourceRepository = fhirResourceRepository;
        }

        [Given(@"I create a Patient which does not exist on PDS and store it")]
        public void CreateAPatientWhichDoesNoteExistOnPDSAndStoreIt()
        {
            var returnPatient = new Patient
            {
                Name = new List<HumanName>
                {
                    CreateUsualName("GPConnectGivenName", "GPConnectFamilyName")
                },
                Gender = AdministrativeGender.Other,
                BirthDateElement = new Date("2017-05-05")
            };

            returnPatient.Identifier.Add(new Identifier(FhirConst.IdentifierSystems.kNHSNumber, "9019546082"));

            _fhirResourceRepository.Patient = returnPatient;
        }

        [Given(@"I set the Stored Patient Demographics to not match the NHS number")]
        public void SetTheStoredPatientDemographicsToNotMatchTheNhsNumber()
        {

            _fhirResourceRepository.Patient.Name = new List<HumanName>
            {
                CreateUsualName("GPConnectGivenName", "GPConnectFamilyName")
            };

            _fhirResourceRepository.Patient.Gender = AdministrativeGender.Other;

            _fhirResourceRepository.Patient.BirthDateElement = new Date("2017-05-05");
        }

        [Given(@"I add a Usual Name to the Stored Patient")]
        public void AddUsualNameToTheStoredPatient()
        {

            var name = CreateUsualName("AdditionalGivenName", "AdditionalFamilyName");

            _fhirResourceRepository.Patient.Name.Add(name);
        }

        [Given(@"I remove the Identifiers from the Stored Patient")]
        public void RemoveTheIdentifiersFromTheStoredPatient()
        {
            _fhirResourceRepository.Patient.Identifier = null;
        }


        [Given(@"I remove the Usual Name from the Stored Patient")]
        public void RemoveTheUsualNameFromTheStoredPatient()
        {
            var unIndex = FindUsualNameIndex(_fhirResourceRepository.Patient.Name);
            if (unIndex >= 0)
            {
                _fhirResourceRepository.Patient.Name[unIndex].Use = HumanName.NameUse.Anonymous;
            }
        }

        [Given(@"I remove the Family Name from the Stored Patient")]
        public void RemoveTheFamilyNameFromTheStoredPatient()
        {
            var unIndex = FindUsualNameIndex(_fhirResourceRepository.Patient.Name);
            if (unIndex >= 0)
            {
                _fhirResourceRepository.Patient.Name[unIndex].Family = null;
            }
        }

        private int FindUsualNameIndex(List<HumanName> names)
        {
            return names.FindIndex(n => n.Use.HasValue && n.Use.Value.Equals(HumanName.NameUse.Usual));
        }


        [Given(@"I remove the Gender from the Stored Patient")]
        public void RemoveTheGenderFromTheStoredPatient()
        {
            _fhirResourceRepository.Patient.Gender = null;
        }

        [Given(@"I remove the Birth Date from the Stored Patient")]
        public void RemoveTheBirthDateFromTheStoredPatient()
        {
            _fhirResourceRepository.Patient.BirthDate = null;
        }

        [Given(@"I add an Identifier with Value ""([^""]*)"" to the Stored Patient")]
        public void AddAnIdentifierWithValueToTheStoredPatient(string nhsNumber)
        {
            _fhirResourceRepository.Patient.Identifier.Add(new Identifier(FhirConst.IdentifierSystems.kNHSNumber, nhsNumber));
        }

       
        [Given(@"I add the Stored Patient as a parameter")]
        public void AddTheStoredPatientAsAParameter()
        {
            _httpContext.HttpRequestConfiguration.BodyParameters.Add("registerPatient", _fhirResourceRepository.Patient);
        }

        [Given(@"I add the Stored Appointment as a parameter")]
        public void AddTheStoredAppointmentAsAParameter()
        {
            _httpContext.HttpRequestConfiguration.BodyParameters.Add("appointment", _fhirResourceRepository.Appointment);
        }

        [Given(@"I add the Stored Patient as a parameter with name ""([^""]*)""")]
        public void AddTheStoredPatientAsAParameterWithName(string parameterName)
        {
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(parameterName, _fhirResourceRepository.Patient);
        }

        [Given(@"I add the Family Name ""([^""]*)"" to the Stored Patient")]
        public void AddTheFamilyNameToTheStoredPatient(string familyName)
        {
            foreach (var name in _fhirResourceRepository.Patient.Name)
            {
                name.FamilyElement.Add(new FhirString(familyName));
            }
        }

        [Given(@"I add an Identifier with missing System to the Stored Patient")]
        public void AddAnIdentifierWithMissingSystemToTheStoredPatient()
        {
            var identifier = new Identifier
            {
                Value = "NewIdentifierNoSystem"
            };

            _fhirResourceRepository.Patient.Identifier.Add(identifier);
        }
        
        [Given(@"I add a generic Identifier to the Stored Patient")]
        public void AddAGenericIdentifierToTheStoredPatient()
        {
            var identifier = new Identifier
            {
                Value = "GenericIdentifierValue",
                System = "GenericIdentifierSystem"
            };

            _fhirResourceRepository.Patient.Identifier.Add(identifier);
        }

        [Given(@"I add a Active element to the Stored Patient")]
        public void AddAActiveElementToStoredPatient()
        {
            _fhirResourceRepository.Patient.Active = true;
        }

        [Given(@"I add a Address element to the Stored Patient")]
        public void AddAAddressElementToStoredPatient()
        {
            var address = new Address
            {
                CityElement = new FhirString("Leeds"),
                PostalCode = "LS1 6AE"
            };

            address.LineElement.Add(new FhirString("1 Trevelyan Square"));
            address.LineElement.Add(new FhirString("Boar Lane"));

            _fhirResourceRepository.Patient.Address.Add(address);
        }

        [Given(@"I add a Animal element to the Stored Patient")]
        public void AddAAnimalElementToStoredPatient()
        {
            _fhirResourceRepository.Patient.Animal = new Patient.AnimalComponent
            {
                Species = new CodeableConcept("AllSpecies", "Human")
            };
        }

        [Given(@"I add a Births element to the Stored Patient")]
        public void AddABirthsElementToStoredPatient()
        {
            _fhirResourceRepository.Patient.MultipleBirth = new FhirBoolean(true);
        }

        [Given(@"I add a CareProvider element to the Stored Patient")]
        public void AddACareProviderElementToStoredPatient()
        {
            var reference = new ResourceReference
            {
                Display = "Test Care Provider"
            };

            _fhirResourceRepository.Patient.CareProvider.Add(reference);
        }

        [Given(@"I add a Communication element to the Stored Patient")]
        public void AddACommunicationElementToStoredPatient()
        {
            var com = new Patient.CommunicationComponent
            {
                Language = new CodeableConcept("https://tools.ietf.org/html/bcp47", "en")
            };

            _fhirResourceRepository.Patient.Communication.Add(com);
        }


        [Given(@"I add a Contact element to the Stored Patient")]
        public void AddAContactElementToStoredPatient()
        {
            var contact = new Patient.ContactComponent
            {
                Name = CreateName(HumanName.NameUse.Anonymous, "TestGiven", "TestFamily")
            };

            _fhirResourceRepository.Patient.Contact.Add(contact);
        }

        [Given(@"I add a Deceased element to the Stored Patient")]
        public void AddADeceasedElementToStoredPatient()
        {
            _fhirResourceRepository.Patient.Deceased = new FhirDateTime("2017-09-01 10:00:00");
        }

        [Given(@"I add a Link element to the Stored Patient")]
        public void AddALinkElementToStoredPatient()
        {
            var reference = new ResourceReference
            {
                Display = "Test Care Provider"
            };

            var link = new Patient.LinkComponent
            {
                Other = reference,
                Type = Patient.LinkType.Refer
            };

            _fhirResourceRepository.Patient.Link.Add(link);
        }

        [Given(@"I add a ManagingOrg element to the Stored Patient")]
        public void AddAManagingOrgElementToStoredPatient()
        {
            var reference = new ResourceReference
            {
                Display = "Test Managing Org"
            };

            _fhirResourceRepository.Patient.ManagingOrganization = reference;
        }

        [Given(@"I add a Marital element to the Stored Patient")]
        public void AddAMaritalElementToStoredPatient()
        {
            _fhirResourceRepository.Patient.MaritalStatus = new CodeableConcept(FhirConst.ValueSetSystems.kMaritalStatus, "M");
        }

        [Given(@"I add a Photo element to the Stored Patient")]
        public void AddAPhotoElementToStoredPatient()
        {
            var attachment = new Attachment
            {
                Url = "Test Photo Element"
            };

            _fhirResourceRepository.Patient.Photo.Add(attachment);
        }

        [Given(@"I add a Telecom element to the Stored Patient")]
        public void AddATelecomElementToStoredPatient()
        {
            _fhirResourceRepository.Patient.Telecom.Add(new ContactPoint(ContactPoint.ContactPointSystem.Phone, ContactPoint.ContactPointUse.Home, "01234567891"));
        }

        [Then(@"the Patient Registration Details Extension should be valid")]
        public void ThePatientRegistrationDetailsExtensioShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                var registrationDetailsExtensions = patient.Extension.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kExtCcGpcRegDetails)).ToList();

                registrationDetailsExtensions.Count.ShouldBe(1, "Incorrect number of registration details extension have been returned. This should be 1.");

                var regDetailsExtension = registrationDetailsExtensions.First();
                var regExtensions = regDetailsExtension.Extension;

                ValidatePatientRegistrationType(regExtensions);
                ValidatePatientRegistrationStatus(regExtensions);
                ValidatePatientRegistrationPeriod(regExtensions);

            });
        }

        private void ValidatePatientRegistrationType(List<Extension> extList)
        {

            var extensions = extList.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kCCExtRegistrationType)).ToList();

            extensions.Count.ShouldBe(1, "The patient resource should contain a registration type extension.");

            var codeList = GlobalContext.GetExtensibleValueSet(FhirConst.ValueSetSystems.kCcGpcRegistrationType).WithComposeIncludes().ToList();

            extensions.ForEach(registrationTypeExtension =>
            {
                registrationTypeExtension.Value.ShouldNotBeNull("The registration type extension should have a value element.");
                registrationTypeExtension.Value.ShouldBeOfType<CodeableConcept>("The registration type extension should be a CodeableConcept.");

                var concept = (CodeableConcept)registrationTypeExtension.Value;

                concept.Coding.ForEach(code =>
                {
                    ShouldBeSingleCodingWhichIsInCodeList(code, codeList);
                });
            });

        }

        private void ValidatePatientRegistrationStatus(List<Extension> extList)
        {
            var extensions = extList.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kCCExtRegistrationStatus)).ToList();

            extensions.Count.ShouldBe(1,"The patient resource should contain a registration status extension.");

            var codeList = GlobalContext.GetExtensibleValueSet(FhirConst.ValueSetSystems.kCcGpcRegistrationStatus).WithComposeIncludes().ToList();

            extensions.ForEach(registrationStatusExtension =>
            {
                registrationStatusExtension.Value.ShouldNotBeNull("The registration status extension should have a value element.");
                registrationStatusExtension.Value.ShouldBeOfType<CodeableConcept>("The registration status extension should be a CodeableConcept.");

                var concept = (CodeableConcept)registrationStatusExtension.Value;

                concept.Coding.ForEach(code =>
                {
                    ShouldBeSingleCodingWhichIsInCodeList(code, codeList);
                });
            });
        }

        private void ValidatePatientRegistrationPeriod(List<Extension> extList)
        {
            var extensions = extList.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kCCExtRegistrationPeriod)).ToList();

            extensions.Count.ShouldBeLessThanOrEqualTo(1, "The patient resource should contain a maximum of 1 registration period extension.");

            extensions.ForEach(registrationPeriodExtension =>
            {
                registrationPeriodExtension.Value.ShouldNotBeNull("The registration period extension should have a value element.");
                registrationPeriodExtension.Value.ShouldBeOfType<Period>("The registration status extension should be a Period.");
            });
        }
        
        [Then(@"the Patient Demographics should match the Stored Patient")]
        public void ThePatientDemographicsShouldMatchTheStoredPatient()
        {
            var storedPatientNhsNumber = _fhirResourceRepository.Patient
                .Identifier
                .First(identifier => identifier.System == FhirConst.IdentifierSystems.kNHSNumber)
                .Value;

            Patients.ForEach(patient =>
            {
                patient.BirthDate.ShouldNotBeNull("The returned patient resource should contain a birthDate element.");
                patient.BirthDate.ShouldBe(_fhirResourceRepository.Patient.BirthDate, "The returned patient DOB does not match the creted patient DOB");

                patient.Gender.ShouldNotBeNull("The patient resource should contain a gender element");
                patient.Gender.ShouldBe(_fhirResourceRepository.Patient.Gender, "The returned patient gender does not match the creted patient gender");

                var patientNames = _fhirResourceRepository.Patient.Name;

                patientNames.Count.ShouldBeGreaterThanOrEqualTo(1, "There should be at least one name element within the returned patient resource");
                
                var unIndex = FindUsualNameIndex(patientNames);

                unIndex.ShouldBeGreaterThanOrEqualTo(0, "Could not find the required Patient name with a Use value of Usual.");

                var usualName = _fhirResourceRepository.Patient.Name[unIndex];
                var storedFamilyName = usualName.Family.First();

                patientNames.ForEach(name =>
                {
                    name.Family.ShouldNotBeNull("There should be a family name in the returned patient resource.");
                    name.Family.Count().ShouldBe(1, "The returned Patient Resource should contain a single family name");
                    name.Family.First().ShouldBe(storedFamilyName, "Returned patient family name does not match created patient family name", StringCompareShould.IgnoreCase);
                });

                var nhsNumberIdentifiers = patient
                    .Identifier
                    .Where(identifier => identifier.System == FhirConst.IdentifierSystems.kNHSNumber)
                    .ToList();

                nhsNumberIdentifiers.Count.ShouldBe(1, "The returned Patient Resource should contain a single NHS Number identifier");

                var nhsNumberIdentifier = nhsNumberIdentifiers.First();

                nhsNumberIdentifier.Value.ShouldNotBeNullOrEmpty("The NHS Number identifier must have a value element.");
                nhsNumberIdentifier.Value.ShouldBe(storedPatientNhsNumber, "The returned NHS Number does not match the sent NHS Number");
            });

        }

        [Given(@"I get the next Patient to register and store it")]
        public void GetTheNextPatientToRegisterAndStoreIt()
        {
            //Mimic PDS Trace
            var registerPatients = GlobalContext.RegisterPatients;

            foreach (var registerPatient in registerPatients)
            {
                _patientSteps.GetThePatientForPatientNhsNumber(registerPatient.SPINE_NHS_NUMBER);

                var entries = _httpContext.FhirResponse.Entries;

                if (!entries.Any())
                {
                    var patientToRegister = new Patient
                    {
                        BirthDateElement = new Date(registerPatient.DOB),
                        Name = new List<HumanName>
                        {
                            CreateUsualName(registerPatient.NAME_GIVEN, registerPatient.NAME_FAMILY)
                        },
                        Identifier = new List<Identifier>
                        {
                            new Identifier(FhirConst.IdentifierSystems.kNHSNumber, registerPatient.SPINE_NHS_NUMBER)
                        }
                    };

                    switch (registerPatient.GENDER)
                    {
                        case "MALE":
                            patientToRegister.Gender = AdministrativeGender.Male;
                            break;
                        case "FEMALE":
                            patientToRegister.Gender = AdministrativeGender.Female;
                            break;
                        case "OTHER":
                            patientToRegister.Gender = AdministrativeGender.Other;
                            break;
                        case "UNKNOWN":
                            patientToRegister.Gender = AdministrativeGender.Unknown;
                            break;
                    }


                    _fhirResourceRepository.Patient = patientToRegister;

                    return;
                }
            }
        }

        [Given(@"I store the patient in the register patient resource format")]
        public void GivenIStoreThePatientInTheRegisterPatientResourceFormat()
        {
            Patients.Count.ShouldBeGreaterThanOrEqualTo(1, "No patients were returned for the patient search.");

            var patient = Patients.First();
            
            var registerPatient = new Patient();

            var identifier = patient.Identifier.FirstOrDefault(x => x.System == FhirConst.IdentifierSystems.kNHSNumber);

            if (identifier != null)
            {
                registerPatient.Identifier.Add(new Identifier(identifier.System, identifier.Value));
            }

            var name = patient.Name.First();

            if (name != null)
            {
                registerPatient.Name.Add(CreateName(HumanName.NameUse.Usual, "GPConnectGivenName", "GPConnectFamilyName"));
            }

            registerPatient.Gender = patient.Gender ?? AdministrativeGender.Unknown;
            registerPatient.BirthDateElement = patient.BirthDateElement ?? new Date();

            _fhirResourceRepository.Patient = registerPatient;
        }

        [Given(@"I Set the Stored Patient Registration Details Extension")]
        public void SetTheStoredPatientRegistrationDetailsExtension(string startDate)
        {
            var extList = new List<Extension>
            {
                GetCodingExtension(FhirConst.StructureDefinitionSystems.kCCExtRegistrationStatus, FhirConst.ValueSetSystems.kCcGpcRegistrationStatus, "I", "Inactive")
            };

            var registrationDetails = new Extension
            {
                Url = FhirConst.StructureDefinitionSystems.kExtCcGpcRegDetails,
                Extension = extList
            };

            _fhirResourceRepository.Patient.Extension.Add(registrationDetails);
        }

        private HumanName CreateUsualName(string givenName, string familyName)
        {

            return CreateName(HumanName.NameUse.Usual, givenName, familyName);
        }

        private HumanName CreateName(HumanName.NameUse use, string givenName, string familyName)
        {
            var humanName = new HumanName()
            {
                FamilyElement = new List<FhirString> { new FhirString(familyName) },
                GivenElement = new List<FhirString> { new FhirString(givenName) },
                Use = use
            };

            return humanName;
        }

        private static Extension GetCodingExtension(string extensionUrl, string codingUrl, string code, string display)
        {
            var coding = new Coding
            {
                Code = code,
                Display = display,
                System = codingUrl
            };

            var reason = new CodeableConcept();
            reason.Coding.Add(coding);

            return new Extension
            {
                Url = extensionUrl,
                Value = reason
            };
        }
        
    }
}
