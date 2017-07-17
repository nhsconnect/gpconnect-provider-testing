namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Context;
    using Enum;
    using Helpers;
    using Hl7.Fhir.Model;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    public class PatientSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private readonly BundleSteps _bundleSteps;
        private readonly JwtSteps _jwtSteps;
        private List<Patient> Patients => _fhirContext.Patients;

        public PatientSteps(FhirContext fhirContext, HttpSteps httpSteps, HttpContext httpContext, BundleSteps bundleSteps, JwtSteps jwtSteps) : base(fhirContext, httpSteps)
        {
            _httpContext = httpContext;
            _bundleSteps = bundleSteps;
            _jwtSteps = jwtSteps;
        }


        [Then(@"the Response Resource should be a Patient")]
        public void TheResponseResourceShouldBeAPatient()
        {
            _fhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.Patient);
        }

        [Then(@"the Patient Id should be valid")]
        public void ThePatienIdShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                patient.Id.ShouldNotBeNullOrWhiteSpace("Id must be set");
            });
        }

        [Then(@"the Patient Metadata should be valid")]
        public void ThePatientMetadataShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                CheckForValidMetaDataInResource(patient, "http://fhir.nhs.net/StructureDefinition/gpconnect-patient-1");
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
                patient.MultipleBirth?.ShouldBeOfType<FhirBoolean>();
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
                        ShouldBeSingleCodingWhichIsInValueSet(GlobalContext.FhirRelationshipValueSet, relationship.Coding);
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
                    contact.Name.Family.Count().ShouldBeLessThanOrEqualTo(1);
                });
            });
        }

        [Then(@"the Patient Name should be valid")]
        public void ThePatientNameShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                patient.Name.ForEach(name =>
                {
                    name.Family.Count().ShouldBeLessThanOrEqualTo(1);
                    name.Given.Count().ShouldBeLessThanOrEqualTo(1);
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
                patient.Identifier.ShouldNotBeNull("The patient identifier should not be null");
                patient.Identifier.Count.ShouldBe(1);

                var identifier = patient.Identifier.First();

                FhirConst.IdentifierSystems.kNHSNumber.Equals(identifier.System).ShouldBeTrue();
                NhsNumberHelper.IsNhsNumberValid(identifier.Value).ShouldBeTrue();

                if (!string.IsNullOrEmpty(patientName))
                {
                    identifier.Value.ShouldBe(GlobalContext.PatientNhsNumberMap[patientName]);
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
                });
            });
        }

        [Then(@"the Patient MaritalStatus should be valid")]
        public void ThePatientMaritalStatusShouldbeValid()
        {
            Patients.ForEach(patient =>
            {
                if (patient.MaritalStatus?.Coding != null)
                {
                    ShouldBeSingleCodingWhichIsInValueSet(GlobalContext.FhirMaritalStatusValueSet, patient.MaritalStatus.Coding);
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
                    ShouldBeSingleCodingWhichIsInValueSet(GlobalContext.FhirHumanLanguageValueSet, communication.Language.Coding);
                });
            });
        }

        [Then(@"the Patient CareProvider Practitioner should be valid and resolvable")]
        public void ThePatientCareProviderPractitionerShouldBeValidAndResolvable()
        {
            Patients.ForEach(patient =>
            {
                if (patient.CareProvider != null)
                {
                    patient.CareProvider.Count.ShouldBeLessThanOrEqualTo(1);

                    if (patient.CareProvider.Count.Equals(1))
                    {
                        var reference = patient.CareProvider.First().Reference;

                        reference.ShouldStartWith("Practitioner/");

                        var returnedResource = _httpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner", reference);
                        returnedResource.GetType().ShouldBe(typeof(Practitioner));
                    }
                }
            });
        }

        [Then(@"the Patient CareProvider Practitioner should be referenced in the Bundle")]
        public void ThePatientCareProviderShouldBeReferencedInTheBundle()
        {
            Patients.ForEach(patient =>
            {
                if (patient.CareProvider != null)
                {
                    patient.CareProvider.Count.ShouldBeLessThanOrEqualTo(1);

                    if (patient.CareProvider.Count.Equals(1))
                    {
                        _bundleSteps.ResponseBundleContainsReferenceOfType(patient.CareProvider.First().Reference, ResourceType.Practitioner);
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
                    patient.ManagingOrganization.Reference.ShouldStartWith("Organization/");
                    var returnedResource = _httpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:organization", patient.ManagingOrganization.Reference);
                    returnedResource.GetType().ShouldBe(typeof(Organization));
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
                        ShouldBeSingleCodingWhichIsInValueSet(GlobalContext.FhirRelationshipValueSet, relationship.Coding);
                    });

                    contact.Name.Family.Count().ShouldBeLessThanOrEqualTo(1);
                    // Contact Name Checks
                    // Contact Telecom Checks
                    // Contact Address Checks
                    // Contact Gender Checks
                    // No mandatory fields and value sets are standard so will be validated by parse of response to fhir resource

                    // Contact Organization Checks
                    if (contact.Organization?.Reference != null)
                    {
                        _fhirContext.Entries.ShouldContain(
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
            _httpContext.RequestParameters.AddParameter("identifier", system + '|' + GlobalContext.PatientNhsNumberMap[value]);
        }

        [Given(@"I add a Patient Identifier parameter with default System and Value ""([^""]*)""")]
        public void AddAPatientIdentifierParameterWithDefaultSystemAndValue(string value)
        {
            AddAPatientIdentifierParameterWithSystemAndValue(FhirConst.IdentifierSystems.kNHSNumber, value);
        }

        [Given(@"I add a Patient Identifier parameter with identifier name ""([^""]*)"" default System and Value ""([^""]*)""")]
        public void AddAPatientIdentifierParameterWithIdentifierNameDefaultSystemAndValue(string identifierName, string value)
        {
            _httpContext.RequestParameters.AddParameter(identifierName, FhirConst.IdentifierSystems.kNHSNumber + '|' + GlobalContext.PatientNhsNumberMap[value]);
        }

        [Given(@"I add a Patient Identifier parameter with default System and NHS number ""([^""]*)""")]
        public void AddAPatientIdentifierParameterWithDefaultSystemAndNhsNumber(string nhsNumber)
        {
            _httpContext.RequestParameters.AddParameter("identifier", FhirConst.IdentifierSystems.kNHSNumber + '|' + nhsNumber);
        }

        [Given(@"I add a Patient Identifier parameter with no System and Value ""([^""]*)""")]
        public void AddAPatientIdentifierParameterWithNoSystemAndValue(string value)
        {
            _httpContext.RequestParameters.AddParameter("identifier", GlobalContext.PatientNhsNumberMap[value]);
        }

        [Given(@"I get the Patient for Patient Value ""([^""]*)""")]
        public void GetThePatientForPatientValue(string value)
        {
            _httpSteps.ConfigureRequest(GpConnectInteraction.PatientSearch);

            AddAPatientIdentifierParameterWithDefaultSystemAndValue(value);

            _jwtSteps.SetTheJwtRequestedRecordToTheNhsNumberFor(value);

            _httpSteps.MakeRequest(GpConnectInteraction.PatientSearch);
        }

        [Given(@"I get the Patient for Patient NHS number ""([^""]*)""")]
        public void GetThePatientForPatientNhsNumber(string nhsNumber)
        {
            _httpSteps.ConfigureRequest(GpConnectInteraction.PatientSearch);

            AddAPatientIdentifierParameterWithDefaultSystemAndNhsNumber(nhsNumber);

            _jwtSteps.SetTheJwtRequestedRecordToTheNhsNumber(nhsNumber);

            _httpSteps.MakeRequest(GpConnectInteraction.PatientSearch);
        }

        [Given(@"I store the Patient")]
        public void StoreThePatient()
        {
            var patient = _fhirContext.Patients.FirstOrDefault();

            if (patient != null)
            {
                _httpContext.StoredPatient = patient;
            }
        }

        [Given(@"I store the Patient Id")]
        public void StoreThePatientId()
        {
            var patient = _fhirContext.Patients.FirstOrDefault();

            if (patient != null)
            {
                _httpContext.GetRequestId = patient.Id;
            }
        }

        [Given(@"I store the Patient Version Id")]
        public void StoreThePatientVersionId()
        {
            var patient = _fhirContext.Patients.FirstOrDefault();

            if (patient != null)
            {
                _httpContext.GetRequestVersionId = patient.VersionId;
            }
        }

        [Given(@"I set an invalid Patient Version Id")]
        public void SetAnInvlalidPatientVersionId()
        {
            _httpContext.GetRequestVersionId = "1234567890";
        }

        [Given(@"I set the If-None-Match header to the stored Patient Version Id")]
        public void SetTheIfNoneMatchHeaderToTheStoredPatientVersionId()
        {
            var patient = _httpContext.StoredPatient;

            if (patient != null)
                _httpSteps.GivenISetTheIfNoneMatchheaderHeaderTo("W/\"" + patient.VersionId + "\"");
        }
    }
}
