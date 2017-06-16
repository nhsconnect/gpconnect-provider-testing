using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using Hl7.Fhir.Model;
using Shouldly;
using System.Linq;
using TechTalk.SpecFlow;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public sealed class AccessRecordSteps : BaseSteps
    {
        private readonly BundleSteps _bundleSteps;

        public AccessRecordSteps(FhirContext fhirContext, HttpSteps httpSteps, BundleSteps bundleSteps) 
            : base(fhirContext, httpSteps)
        {
            _bundleSteps = bundleSteps;
        }

        #region Given

        //Used elsewhere
        [Given(@"I am requesting the record for config patient ""([^""]*)""")]
        public void GivenIAmRequestingTheRecordForConfigPatient(string patient)
        {
            Given($@"I am requesting the record for patient with NHS Number ""{GlobalContext.PatientNhsNumberMap[patient]}""");
        }

        [Given(@"I replace the parameter name ""([^""]*)"" with ""([^""]*)""")]
        public void GivenIReplaceTheParameterNameWith(string parameterName, string newParameterName)
        {
            _fhirContext.FhirRequestParameters.GetSingle(parameterName).Name = newParameterName;
        }

        [Given(@"I set the parameter patientNHSNumber with an empty value")]
        public void GivenISetThePatientNHSNumberParameterWithAnEmptyValue()
        {
            var parameter = _fhirContext.FhirRequestParameters.GetSingle("patientNHSNumber");
            ((Identifier)parameter.Value).Value = "";
        }

        [Given(@"I set the parameter patientNHSNumber with an empty system")]
        public void GivenISetThePatientNHSNumberParameterWithAnEmptySystem()
        {
            var parameter = _fhirContext.FhirRequestParameters.GetSingle("patientNHSNumber");
            ((Identifier)parameter.Value).System = "";
        }

        [Given(@"I set the parameter recordSection with an empty system")]
        public void GivenISetTheRecordSectionParameterWithAnEmptySystem()
        {
            var parameter = _fhirContext.FhirRequestParameters.GetSingle("recordSection");
            ((CodeableConcept)parameter.Value).Coding[0].System = "";
        }

        #endregion

        [Then(@"the response bundle Patient entry should contain a valid NHS number identifier")]
        public void ThenResponseBundlePatientEntryShouldContainAValidNHSNumberIdentifier()
        {
            _fhirContext.Patients.ForEach(patient =>
                {
                    patient.Identifier.ShouldAllBe(identifier => 
                        FhirConst.IdentifierSystems.kNHSNumber.Equals(identifier.System) && 
                        FhirHelper.isValidNHSNumber(identifier.Value));
                });
        }

        [Then(@"the response bundle Patient resource should optionally contain valid telecom information")]
        public void ThenResponseBundlePatientResourceShouldContainValidTelecomInfromation()
        {
            _fhirContext.Patients.ForEach(patient => 
                {
                    patient.Telecom.ForEach(telecom =>
                    {
                        telecom.System.ShouldNotBeNull();
                        telecom.Value.ShouldNotBeNull();
                    });
                });
        }

        [Then(@"if composition contains the patient resource maritalStatus fields matching the specification")]
        public void ThenIfCompositionContainsThePatientResourceMaritalStatusFieldsMatchingTheSpecification()
        {
            _fhirContext.Patients.ForEach(patient =>
            {
                if (patient.MaritalStatus?.Coding != null)
                {
                    ShouldBeSingleCodingWhichIsInValueSet(GlobalContext.FhirMaritalStatusValueSet, patient.MaritalStatus.Coding);
                }
            });
        }

        [Then(@"if composition contains the patient resource communication the mandatory fields should matching the specification")]
        public void ThenIfCompositionContainsThePatientResourceCommunicationTheMandatoryFieldsShouldMatchingTheSpecification()
        {
            _fhirContext.Patients.ForEach(patient =>
            {
                patient.Communication?.ForEach(communication =>
                {
                    ShouldBeSingleCodingWhichIsInValueSet(GlobalContext.FhirHumanLanguageValueSet, communication.Language.Coding);
                });
            });
        }

        [Then(@"if Patient careProvider is included in the response the reference should reference a Practitioner within the bundle")]
        public void ThenIfPatientCareProviderIsIncludedInTheResponseTheReferenceShouldReferenceAPractitionerWithinTheBundle()
        {
            _fhirContext.Patients.ForEach(patient =>
            {
                if (patient.CareProvider != null)
                {
                    patient.CareProvider.Count.ShouldBeLessThanOrEqualTo(1);
                    patient.CareProvider.ForEach(careProvider =>
                    {
                        _bundleSteps.ResponseBundleContainsReferenceOfType(careProvider.Reference, ResourceType.Practitioner);
                    });
                }
            });
        }

        [Then(@"if Patient managingOrganization is included in the response the reference should reference an Organization within the bundle")]
        public void ThenIfPatientManagingOrganizationIsIncludedInTheResponseTheReferenceShouldReferenceAnOrganizationWithinTheBundle()
        {
            _fhirContext.Patients.ForEach(patient =>
            {
                if (patient.ManagingOrganization != null)
                {
                    _bundleSteps.ResponseBundleContainsReferenceOfType(patient.ManagingOrganization.Reference, ResourceType.Organization);
                }
            });
        }

        [Then(@"patient resource should not contain the fhir fields photo animal or link")]
        public void ThenPatientResourceShouldNotContainTheFhirFieldsPhotoAnimalOrLink()
        {
            _fhirContext.Patients.ForEach(patient =>
            {
                // C# API creates an empty list if no element is present
                patient.Photo?.Count.ShouldBe(0, "There should be no photo element in Patient Resource");
                patient.Link?.Count.ShouldBe(0, "There should be no link element in Patient Resource");
                patient.Animal.ShouldBeNull("There should be no Animal element in Patient Resource");
            });
        }

        [Then(@"if care record composition contains the patient resource contact the mandatory fields should matching the specification")]
        public void ThenIfCompositionContainsThePatientResourceContactTheMandatoryFieldsShouldMatchingTheSpecification()
        {
            _fhirContext.Patients.ForEach(patient =>
            {
                patient.Contact.ForEach(contact =>
                {
                    // Contact Relationship Checks
                    contact.Relationship.ForEach(relationship =>
                    {
                        ShouldBeSingleCodingWhichIsInValueSet(GlobalContext.FhirRelationshipValueSet, relationship.Coding);
                    });

                    // Contact Name Checks
                    // Contact Telecom Checks
                    // Contact Address Checks
                    // Contact Gender Checks
                    // No mandatory fields and value sets are standard so will be validated by parse of response to fhir resource

                    // Contact Organization Checks
                    if (contact.Organization?.Reference != null)
                    {
                        _fhirContext.Entries.ShouldContain(entry =>
                            entry.Resource.ResourceType.Equals(ResourceType.Organization) &&
                            entry.FullUrl.Equals(contact.Organization.Reference)
                        );
                    }
                });
            });
        }

       

        [Then(@"practitioner resources should contain a single name element")]
        public void ThenPractitionerResourcesShouldContainASingleNameElement()
        {
            _fhirContext.Practitioners.ForEach(practitioner =>
            {
                practitioner.Name.ShouldNotBeNull();
            });
        }

        //Hayden: Not referenced in any test... should this be removed?
        [Then(@"practitioner family name should equal ""(.*)")]
        public void FamilyNameShouldEqualVariable(string familyName)
        {
            _fhirContext.Practitioners.ForEach(practitioner =>
            {
                practitioner.Name.Family.ToList()[0].ShouldBe(familyName, "Family name doesn't match");
            });
        }

        [Then(@"practitioner should only have one family name")]
        public void PractitionerShouldNotHaveMoreThenOneFamilyName()
        {
            _fhirContext.Practitioners.ForEach(practitioner =>
            {
                practitioner.Name.Family.Count().ShouldBeLessThanOrEqualTo(1);
            });
        }

        [Then(@"practitioner resources should not contain the disallowed elements")]
        public void ThenPractitionerResourcesShouldNotContainTheDisallowedElements()
        {
            _fhirContext.Practitioners.ForEach(practitioner =>
            {
                // C# API creates an empty list if no element is present
                (practitioner.Photo == null || practitioner.Photo.Count == 0).ShouldBeTrue(); 
                (practitioner.Qualification == null || practitioner.Qualification.Count == 0).ShouldBeTrue();
            });
        }

        [Then(@"practitioner resources must contain one user id and optional profile ids")]
        public int ThenPractitionerResourcesMustContainOneUserIdAndOptionalProfileIdsReturnSdsRoleProfileCount()
        {
            var sdsRoleProfileTotal = 0;

            _fhirContext.Practitioners.ForEach(practitioner =>
            {
                var sdsUserIdCount = practitioner.Identifier.Count(identifier => identifier.System.Equals("http://fhir.nhs.net/Id/sds-user-id"));
                sdsUserIdCount.ShouldBe(1, "entry contains invalid http://fhir.nhs.net/Id/sds-user-id system quantity");

                practitioner.Identifier.ForEach(identifier =>
                {
                    identifier.Value.ShouldNotBeNull();
                });

                var roleProfileCount = practitioner.Identifier.Count(identifier => identifier.System.Equals("http://fhir.nhs.net/Id/sds-role-profile-id"));

                sdsRoleProfileTotal = sdsRoleProfileTotal + roleProfileCount;
            });

            return sdsRoleProfileTotal;
        }

        [Then(@"practitioner resources must contain one user id and a total of ""([^""]*)"" profile ids")]
        public void ThenPractitionerResourcesMustContainOneUserIdAndATotalXProfileIds(int profileIdCount)
        {
            ThenPractitionerResourcesMustContainOneUserIdAndOptionalProfileIdsReturnSdsRoleProfileCount()
                .ShouldBe(profileIdCount, "Unexpected http://fhir.nhs.net/Id/sds-role-profile-id system quantity");
        }
        
        [Then(@"all practitioners contain SDS identifier for practitioner ""([^""]*)""")]
        public void AllPractitionersContainSdsIdentifierForPractitioner(string practitionerId)
        {
            var customMessage = "No identifier with system http://fhir.nhs.net/Id/sds-user-id found";
            _fhirContext.Practitioners.ForEach(practitioner =>
            {
                practitioner.Identifier.ShouldAllBe(identifier => identifier.System.Equals("http://fhir.nhs.net/Id/sds-user-id"), customMessage);
            });
        }

        [Then(@"if practitionerRole has managingOrganization element then reference must exist")]
        public void ThenIfPractitionerRoleHasMangingOrganizationElementThenReferenceMustExist()
        {
            _fhirContext.Practitioners.ForEach(practitioner =>
            {
                practitioner.PractitionerRole.ForEach(practitionerRole =>
                {
                    practitionerRole.ManagingOrganization.Reference.ShouldNotBeNull();
                    practitionerRole.ManagingOrganization.Reference.ShouldStartWith("Organization/");
                    var returnedResource = _httpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:organization", practitionerRole.ManagingOrganization.Reference);
                    returnedResource.GetType().ShouldBe(typeof(Organization));
                });
            });
        }

        [Then(@"if practitionerRole has role element which contains a coding then the system, code and display must exist")]
        public void ThenIfPractitionerRoleHasRoleElementWhichContainsACodingThenTheSystemCodeAndDisplayMustExist()
        {
            _fhirContext.Practitioners.ForEach(practitioner =>
            {
                practitioner.PractitionerRole.ForEach(practitionerRole =>
                {
                    if (practitionerRole.Role?.Coding != null)
                    {
                        practitionerRole.Role.Coding.Count.ShouldBeLessThanOrEqualTo(1);
                        practitionerRole.Role.Coding.ForEach(coding =>
                        {
                            coding.System.ShouldBe("http://fhir.nhs.net/ValueSet/sds-job-role-name-1");
                            coding.Code.ShouldNotBeNull();
                            coding.Display.ShouldNotBeNull();
                        });
                    }
                });
            });
        }

        [Then(@"if the practitioner has communicaiton elemenets containing a coding then there must be a system, code and display element")]
        public void ThenIfThePractitionerHasCommunicationElementsContainingACodingThenThereMustBeASystemCodeAndDisplayElement()
        {
            _fhirContext.Practitioners.ForEach(practitioner =>
            {
                practitioner.Communication.ForEach(codeableConcept =>
                {
                    ShouldBeSingleCodingWhichIsInValueSet(GlobalContext.FhirHumanLanguageValueSet, codeableConcept.Coding);
                });
            });
        }

        [Then(@"if practitioner contains a managingOrganization the reference relates to an Organization within the response bundle")]
        public void ThenPractitionerContainsAManagingOrganizationTheReferenceRelatesToAnOrganizationWithinTheResponseBundle()
        {
            _fhirContext.Practitioners.ForEach(practitioner =>
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

        [Then(@"Organization resources identifiers must comply with specification identifier restricitions")]
        public void ThenOrganizationResourceIdentifiersMustComplyWithSpecificationIdentifierRestrictions()
        {
            _fhirContext.Organizations.ForEach(organization =>
            {
                var odsOrganizationCodeCount = organization.Identifier.Count(identifier => identifier.System.Equals("http://fhir.nhs.net/Id/ods-organization-code"));
                odsOrganizationCodeCount.ShouldBeLessThanOrEqualTo(1);

                organization.Identifier.ForEach(identifier =>
                {
                    if (identifier.System.Equals("http://fhir.nhs.net/Id/ods-organization-code") || identifier.System.Equals("http://fhir.nhs.net/Id/ods-site-code"))
                    {
                        identifier.Value.ShouldNotBeNull();
                    }
                }); 
            });
        }

        [Then(@"if Organization includes type coding the elements are mandatory")]
        public void ThenIfOrganizationIncludesTypeCodingTheElementsAreMandatory()
        {
            _fhirContext.Organizations.ForEach(organization =>
            {
                if (organization.Type?.Coding != null)
                {
                    organization.Type.Coding.Count.ShouldBeLessThanOrEqualTo(1);
                    organization.Type.Coding.ForEach(coding =>
                    {
                        coding.System.ShouldNotBeNull();
                        coding.Code.ShouldNotBeNull();
                        coding.Display.ShouldNotBeNull();
                    });
                }
            });
        }

        [Then(@"if Organization includes partOf it should reference a resource in the response bundle")]
        public void ThenIfOrganizationIncludesPartOfItShouldReferenceAResourceInTheResponseBundle()
        {
            _fhirContext.Organizations.ForEach(organization =>
            {
                if (organization.PartOf != null)
                {
                    _bundleSteps.ResponseBundleContainsReferenceOfType(organization.PartOf?.Reference, ResourceType.Organization);
                }
            });
        }

        [Then(@"the Device resource should conform to cardinality set out in specificaiton")]
        public void ThenTheDeviceResourceShouldConformToCardinalitySetOutInSpecification()
        {
            _fhirContext.Devices.ForEach(device =>
            {
                device.Status.ShouldBeNull();
                device.ManufactureDate.ShouldBeNull();
                device.Expiry.ShouldBeNull();
                device.Udi.ShouldBeNull();
                device.LotNumber.ShouldBeNull();
                device.Patient.ShouldBeNull();
                (device.Contact == null || device.Contact.Count == 0).ShouldBeTrue();
                device.Url.ShouldBeNull();

                device.Identifier.Count.ShouldBeLessThanOrEqualTo(1);
                device.Note.Count.ShouldBeLessThanOrEqualTo(1);

                device.Identifier.ForEach(identifier =>
                {
                    identifier.Value.ShouldNotBeNull();
                });
            });
        }

        [Then(@"the Device resource type should match the fixed values from the specfication")]
        public void ThenTheDeviceResourceTypeShouldMatchTheFixedValuesFromTheSpecification()
        {
            _fhirContext.Devices.ForEach(device =>
            {
                device.Type.Coding.Count.ShouldBeLessThanOrEqualTo(1);
                device.Type.Coding.ForEach(coding =>
                {
                    coding.System.ShouldBe("http://snomed.info/sct");
                    coding.Code.ShouldBe("462240000");
                    coding.Display.ShouldBe("Patient health record information system (physical object)");
                });
            });
        }
    }
}
