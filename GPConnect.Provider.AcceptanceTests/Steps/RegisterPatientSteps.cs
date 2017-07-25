namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Context;
    using Hl7.Fhir.Model;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    public class RegisterPatientSteps : Steps
    {
        private readonly FhirContext _fhirContext;
        private readonly HttpContext _httpContext;
        private readonly PatientSteps _patientSteps;
        private List<Patient> Patients => _fhirContext.Patients;

        public RegisterPatientSteps(FhirContext fhirContext, HttpContext httpContext, PatientSteps patientSteps)
        {
            _fhirContext = fhirContext;
            _httpContext = httpContext;
            _patientSteps = patientSteps;
        }

        [Given(@"I create a Patient which does not exist on PDS and store it")]
        public void CreateAPatientWhichDoesNoteExistOnPDSAndStoreIt()
        {
            var name = new HumanName();

            name.FamilyElement.Add(new FhirString("GPConnectFamilyName"));
            name.GivenElement.Add(new FhirString("GPConnectGivenName"));

            var returnPatient = new Patient
            {
                Name = new List<HumanName> {name},
                Gender = AdministrativeGender.Other,
                BirthDateElement = new Date("2017-05-05")
            };

            returnPatient.Identifier.Add(new Identifier(FhirConst.IdentifierSystems.kNHSNumber, "9019546082"));

            _httpContext.StoredPatient = returnPatient;
        }

        [Given(@"I set the Stored Patient Demographics to not match the NHS number")]
        public void SetTheStoredPatientDemographicsToNotMatchTheNhsNumber()
        {
            var name = new HumanName();

            name.FamilyElement.Add(new FhirString("GPConnectFamilyName"));
            name.GivenElement.Add(new FhirString("GPConnectGivenName"));

            _httpContext.StoredPatient.Name = new List<HumanName>
            {
                name
            };

            _httpContext.StoredPatient.Gender = AdministrativeGender.Other;

            _httpContext.StoredPatient.BirthDateElement = new Date("2017-05-05");
        }
        
        [Given(@"I remove the Identifiers from the Stored Patient")]
        public void RemoveTheIdentifiersFromTheStoredPatient()
        {
            _httpContext.StoredPatient.Identifier = null;
        }

        [Given(@"I remove the Name from the Stored Patient")]
        public void RemoveTheNameFromTheStoredPatient()
        {
            _httpContext.StoredPatient.Name = null;
        }

        [Given(@"I remove the Family Name from the Stored Patient")]
        public void RemoveTheFamilyNameFromTheStoredPatient()
        {
            _httpContext.StoredPatient.Name[0].Family = null;
        }

        [Given(@"I remove the Given Name from the Stored Patient")]
        public void RemoveTheGivenNameFromTheStoredPatient()
        {
            _httpContext.StoredPatient.Name[0].Given = null;
        }

        [Given(@"I remove the Gender from the Stored Patient")]
        public void RemoveTheGenderFromTheStoredPatient()
        {
            _httpContext.StoredPatient.Gender = null;
        }

        [Given(@"I remove the Birth Date from the Stored Patient")]
        public void RemoveTheBirthDateFromTheStoredPatient()
        {
            _httpContext.StoredPatient.BirthDate = null;
        }

        [Given(@"I add an Identifier with Value ""([^""]*)"" to the Stored Patient")]
        public void AddAnIdentifierWithValueToTheStoredPatient(string nhsNumber)
        {
            _httpContext.StoredPatient.Identifier.Add(new Identifier(FhirConst.IdentifierSystems.kNHSNumber, nhsNumber));
        }

        [Given(@"I set the Stored Patient Registration Period with Start Date ""([^""]*)"" and End Date ""([^""]*)""")]
        public void SetTheStoredPatientRegistrationPeriodWithStartDateAndEndDate(string startDate, string endDate)
        {
            var period = new Period
            {
                Start = startDate,
                End = endDate
            };

            var registrationPeriod = new Extension
            {
                Url = "http://fhir.nhs.net/StructureDefinition/extension-registration-period-1",
                Value = period
            };

            _httpContext.StoredPatient.Extension.Add(registrationPeriod);
        }
        
        [Given(@"I add the Stored Patient as a parameter")]
        public void AddTheStoredPatientAsAParameter()
        {
            _httpContext.BodyParameters.Add("registerPatient", _httpContext.StoredPatient);
        }

        [Given(@"I add the Stored Appointment as a parameter")]
        public void AddTheStoredAppointmentAsAParameter()
        {
            _httpContext.BodyParameters.Add("appointment", _httpContext.CreatedAppointment);
        }

        [Given(@"I add the Stored Patient as a parameter with name ""([^""]*)""")]
        public void AddTheStoredPatientAsAParameterWithName(string parameterName)
        {
            _httpContext.BodyParameters.Add(parameterName, _httpContext.StoredPatient);
        }

        [Given(@"I add the Family Name ""([^""]*)"" to the Stored Patient")]
        public void AddTheFamilyNameToTheStoredPatient(string familyName)
        {
            foreach (var name in _httpContext.StoredPatient.Name)
            {
                name.FamilyElement.Add(new FhirString(familyName));
            }
        }

        [Given(@"I add the Given Name ""([^""]*)"" to the Stored Patient")]
        public void AddTheGivenNameToTheStoredPatient(string givenName)
        {
            foreach (var name in _httpContext.StoredPatient.Name)
            {
                name.GivenElement.Add(new FhirString(givenName));
            }
        }

        [Given(@"I add a Name with Given Name ""([^""]*)"" and Family Name ""([^""]*)"" to the Stored Patient")]
        public void AddANameWithGivenNameAndFamilyNameToTheStoredPatient(string givenName, string familyName)
        {
            var name = new HumanName();

            name.GivenElement.Add(new FhirString(givenName));
            name.FamilyElement.Add(new FhirString(familyName));

            _httpContext.StoredPatient.Name.Add(name);
        }

        [Given(@"I add an Identifier with missing System to the Stored Patient")]
        public void AddAnIdentifierWithMissingSystemToTheStoredPatient()
        {
            var identifier = new Identifier
            {
                Value = "NewIdentifierNoSystem"
            };

            _httpContext.StoredPatient.Identifier.Add(identifier);
        }
        
        [Given(@"I add a generic Identifier to the Stored Patient")]
        public void AddAGenericIdentifierToTheStoredPatient()
        {
            var identifier = new Identifier
            {
                Value = "GenericIdentifierValue",
                System = "GenericIdentifierSystem"
            };

            _httpContext.StoredPatient.Identifier.Add(identifier);
        }

        [Given(@"I add a Active element to the Stored Patient")]
        public void AddAActiveElementToStoredPatient()
        {
            _httpContext.StoredPatient.Active = true;
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
           
            _httpContext.StoredPatient.Address.Add(address);
        }

        [Given(@"I add a Animal element to the Stored Patient")]
        public void AddAAnimalElementToStoredPatient()
        {
            _httpContext.StoredPatient.Animal = new Patient.AnimalComponent
            {
                Species = new CodeableConcept("AllSpecies", "Human")
            };
        }

        [Given(@"I add a Births element to the Stored Patient")]
        public void AddABirthsElementToStoredPatient()
        {
            _httpContext.StoredPatient.MultipleBirth = new FhirBoolean(true);
        }

        [Given(@"I add a CareProvider element to the Stored Patient")]
        public void AddACareProviderElementToStoredPatient()
        {
            var reference = new ResourceReference
            {
                Display = "Test Care Provider"
            };

            _httpContext.StoredPatient.CareProvider.Add(reference);
        }

        [Given(@"I add a Communication element to the Stored Patient")]
        public void AddACommunicationElementToStoredPatient()
        {
            var com = new Patient.CommunicationComponent
            {
                Language = new CodeableConcept("https://tools.ietf.org/html/bcp47", "en")
            };

            _httpContext.StoredPatient.Communication.Add(com);
        }


        [Given(@"I add a Contact element to the Stored Patient")]
        public void AddAContactElementToStoredPatient()
        {
            var contact = new Patient.ContactComponent
            {
                Name = new HumanName()
            };

            contact.Name.GivenElement.Add(new FhirString("TestGiven"));
            contact.Name.FamilyElement.Add(new FhirString("TestFamily"));

            _httpContext.StoredPatient.Contact.Add(contact);
        }

        [Given(@"I add a Deceased element to the Stored Patient")]
        public void AddADeceasedElementToStoredPatient()
        {
            _httpContext.StoredPatient.Deceased = new FhirBoolean(false);
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

            _httpContext.StoredPatient.Link.Add(link);
        }

        [Given(@"I add a ManagingOrg element to the Stored Patient")]
        public void AddAManagingOrgElementToStoredPatient()
        {
            var reference = new ResourceReference
            {
                Display = "Test Managing Org"
            };

            _httpContext.StoredPatient.ManagingOrganization = reference;
        }

        [Given(@"I add a Marital element to the Stored Patient")]
        public void AddAMaritalElementToStoredPatient()
        {
            _httpContext.StoredPatient.MaritalStatus = new CodeableConcept("http://hl7.org/fhir/v3/MaritalStatus", "M");
        }
        
        [Given(@"I add a Photo element to the Stored Patient")]
        public void AddAPhotoElementToStoredPatient()
        {
            var attachment = new Attachment
            {
                Url = "Test Photo Element"
            };

            _httpContext.StoredPatient.Photo.Add(attachment);
        }
    
        [Given(@"I add a Telecom element to the Stored Patient")]
        public void AddATelecomElementToStoredPatient()
        {
            _httpContext.StoredPatient.Telecom.Add(new ContactPoint(ContactPoint.ContactPointSystem.Phone, ContactPoint.ContactPointUse.Home, "01234567891"));
        }

        [Then(@"the Patient Registration Type should be valid")]
        public void ThePatientRegistrationTypeShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                var registrationTypeExtensions = patient.Extension.Where(extension => extension.Url == "http://fhir.nhs.net/StructureDefinition/extension-registration-type-1").ToList();

                registrationTypeExtensions.ShouldNotBeEmpty("The patient resource should contain a registration type extension.");

                registrationTypeExtensions.ForEach(registrationTypeExtension =>
                {
                    registrationTypeExtension.Value.ShouldNotBeNull("The registration type extension should have a value element.");
                });
            });
        }

        [Then(@"the Patient Registration Status should be valid")]
        public void ThePatientRegistrationStatusShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                var registrationStatusExtensions = patient.Extension.Where(extension => extension.Url == "http://fhir.nhs.net/StructureDefinition/extension-registration-status-1").ToList();

                registrationStatusExtensions.ShouldNotBeEmpty("The patient resource should contain a registration status extension.");

                registrationStatusExtensions.ForEach(registrationStatusExtension =>
                {
                    registrationStatusExtension.Value.ShouldNotBeNull("The registration status extension should have a value element.");
                });
            });
        }


        [Then(@"the Patient Registration Period should be valid")]
        public void ThePatientRegistrationPeriodShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                var registrationPeriodExtensions = patient.Extension.Where(extension => extension.Url == "http://fhir.nhs.net/StructureDefinition/extension-registration-period-1").ToList();

                registrationPeriodExtensions.ShouldNotBeEmpty("The expected registration extension was not present in the returned patient resource.");

                registrationPeriodExtensions.ForEach(registrationPeriodExtension =>
                {
                    registrationPeriodExtension.Value.ShouldNotBeNull("If an extension is included in the patient resource it must contain a value.");
                });
            });
        }
        
        [Then(@"the Patient Demographics should match the Stored Patient")]
        public void ThePatientDemographicsShouldMatchTheStoredPatient()
        {
            var storedPatientNhsNumber = _httpContext.StoredPatient
                .Identifier
                .First(identifier => identifier.System == FhirConst.IdentifierSystems.kNHSNumber)
                .Value;

            Patients.ForEach(patient =>
            {
                patient.BirthDate.ShouldNotBeNull("The returned patient resource should contain a birthDate element.");
                patient.BirthDate.ShouldBe(_httpContext.StoredPatient.BirthDate, "The returned patient DOB does not match the creted patient DOB");

                patient.Gender.ShouldNotBeNull("The patient resource should contain a gender element");
                patient.Gender.ShouldBe(_httpContext.StoredPatient.Gender, "The returned patient gender does not match the creted patient gender");

                patient.Name.Count.ShouldBe(1, "There should be a single name element within the returned patient resource");

                var storedGivenName = _httpContext.StoredPatient.Name.First().Given.First();
                var storedFamilyName = _httpContext.StoredPatient.Name.First().Family.First();

                patient.Name.ForEach(name =>
                {
                    name.Family.ShouldNotBeNull("There should be a family name in the returned patient resource.");
                    name.Family.Count().ShouldBe(1, "The returned Patient Resource should contain a single family name");
                    name.Family.First().ShouldBe(storedFamilyName, "Returned patient family name does not match created patient family name", StringCompareShould.IgnoreCase);


                    name.Given.ShouldNotBeNull("There should be a given name in the returned patient resource.");
                    name.Given.Count().ShouldBe(1, "The returned Patient Resource should contain a single given name");
                    name.Given.First().ShouldBe(storedGivenName, "Returned patient given name does not match created patient family name", StringCompareShould.IgnoreCase);
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
            var registerPatients = GlobalContext.RegisterPatients;

            foreach (var registerPatient in registerPatients)
            {
                _patientSteps.GetThePatientForPatientNhsNumber(registerPatient.SPINE_NHS_NUMBER);

                var entries = _fhirContext.Entries;

                if (!entries.Any())
                {
                    var name = new HumanName
                    {
                        FamilyElement = new List<FhirString> {new FhirString(registerPatient.NAME_FAMILY)},
                        GivenElement = new List<FhirString> {new FhirString(registerPatient.NAME_GIVEN)}
                    };

                    var patientToRegister = new Patient
                    {
                        BirthDateElement = new Date(registerPatient.DOB),
                        Name = new List<HumanName>
                        {
                            name
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


                    _httpContext.StoredPatient = patientToRegister;

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
                const string familyName = "GPConnectFamilyName";
                const string givenName = "GPConnectGivenName";

                var humanName = new HumanName();

                humanName.FamilyElement.Add(new FhirString(familyName));
                humanName.GivenElement.Add(new FhirString(givenName));

                registerPatient.Name.Add(humanName);
            }

            registerPatient.Gender = patient.Gender ?? AdministrativeGender.Unknown;
            registerPatient.BirthDateElement = patient.BirthDateElement ?? new Date();

            _httpContext.StoredPatient = registerPatient;
        }

        [Given(@"I set the Stored Patient Registration Period with Start ""([^""]*)""")]
        public void SetTheStoredPatientRegistrationPeriodWithStartDate(string startDate)
        {
            var registrationPeriod = new Extension
            {
                Url = "http://fhir.nhs.net/StructureDefinition/extension-registration-period-1",
                Value = new Period {Start = startDate }
                
            };

            _httpContext.StoredPatient.Extension.Add(registrationPeriod);
        }

        [Given(@"I set the Stored Patient Registration Status with Value ""([^""]*)""")]
        public void SetTheStoredPatientRegistrationStatusTo(string code)
        {
            var codableConcept = new CodeableConcept
            {
                Coding = new List<Coding>
                {
                    new Coding { Code = code }
                }
            };

            var registrationStatus = new Extension
            {
                Url = "http://fhir.nhs.net/StructureDefinition/extension-registration-status-1",
                Value = codableConcept
            };

            _httpContext.StoredPatient.Extension.Add(registrationStatus);
        }

        [Given(@"I set the Stored Patient Registration Type with Value ""([^""]*)""")]
        public void SetTheStoredPatientRegistrationTypeTo(string code)
        {
            var codableConcept = new CodeableConcept
            {
                Coding = new List<Coding>
                {
                    new Coding { Code = code }
                }
            };

            var registrationType = new Extension
            {
                Url = "http://fhir.nhs.net/StructureDefinition/extension-registration-type-1",
                Value = codableConcept
            };

            _httpContext.StoredPatient.Extension.Add(registrationType);
        }
    }
}
