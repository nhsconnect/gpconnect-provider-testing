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
    }
}
