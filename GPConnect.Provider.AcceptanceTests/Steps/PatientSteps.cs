using System.Collections.Generic;
using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Model;
using Shouldly;
using TechTalk.SpecFlow;
using static Hl7.Fhir.Model.Bundle;
using NUnit.Framework;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Linq;
    using Data;
    using Enum;
    using Helpers;

    [Binding]
    public class PatientSteps : BaseSteps
    {
        private readonly HttpContext HttpContext;
        private readonly BundleSteps _bundleSteps;
        private readonly JwtSteps _jwtSteps;

        public PatientSteps(FhirContext fhirContext, HttpSteps httpSteps, HttpContext httpContext, BundleSteps bundleSteps, JwtSteps jwtSteps) : base(fhirContext, httpSteps)
        {
            Log.WriteLine("PatientSteps() Constructor");
            HttpContext = httpContext;
            _bundleSteps = bundleSteps;
            _jwtSteps = jwtSteps;
        }

        // Patient Steps
        [Given(@"I perform a patient search for patient ""([^""]*)"" and store the first returned resources against key ""([^""]*)""")]
        public void IPerformAPatientSearchForPatientAndStoreTheFirstReturnedResourceAgainstKey(string patient, string patientResourceKey)
        {
            Given($@"I am using the default server");
            And($@"I am performing the ""urn:nhs:names:services:gpconnect:fhir:rest:search:patient"" interaction");
            And($@"I set the JWT requested record NHS number to config patient ""{patient}""");
            And($@"I set the JWT requested scope to ""patient/*.read""");
            When($@"I search for Patient ""{patient}""");
            Then($@"the response status code should indicate success");
            And($@"the response body should be FHIR JSON");
            And($@"the response should be a Bundle resource of type ""searchset""");
            And($@"the response bundle should contain ""1"" entries");

            var returnedFirstResource = (Patient)((Bundle)_fhirContext.FhirResponseResource).Entry[0].Resource;
            returnedFirstResource.GetType().ShouldBe(typeof(Patient));
            if (HttpContext.StoredFhirResources.ContainsKey(patientResourceKey)) HttpContext.StoredFhirResources.Remove(patientResourceKey);
            HttpContext.StoredFhirResources.Add(patientResourceKey, returnedFirstResource);
        }

        [Given(@"I perform a patient search for patient with NHSNumber ""([^""]*)"" and store the response bundle against key ""([^""]*)""")]
        public void IPerformAPatientSearchForPatientWithNHSNumberAndStoreTheResponseBundleAgainstKey(string nhsNumber, string patientSearchResponseBundleKey)
        {
            Given($@"I am using the default server");
            And($@"I am performing the ""urn:nhs:names:services:gpconnect:fhir:rest:search:patient"" interaction");
            And($@"I set the JWT requested record patient NHS number to ""{nhsNumber}""");
            And($@"I set the JWT requested scope to ""patient/*.read""");
            When($@"I search for Patient with NHS Number ""{nhsNumber}""");
            Then($@"the response status code should indicate success");
            And($@"the response body should be FHIR JSON");
            And($@"the response should be a Bundle resource of type ""searchset""");
            if (HttpContext.StoredFhirResources.ContainsKey(patientSearchResponseBundleKey)) HttpContext.StoredFhirResources.Remove(patientSearchResponseBundleKey);
            HttpContext.StoredFhirResources.Add(patientSearchResponseBundleKey, (Bundle)_fhirContext.FhirResponseResource);
        }

        [When(@"I search for Patient ""([^""]*)""")]
        public void ISearchForPatient(string patient)
        {
            ISearchForPatientWithSystem(patient, FhirConst.IdentifierSystems.kNHSNumber);
        }

        [When(@"I search for Patient with NHS Number ""([^""]*)""")]
        public void ISearchForPatientWithNHSNumber(string nhsNumber)
        {
            var parameterString = FhirConst.IdentifierSystems.kNHSNumber + "|" + nhsNumber;
            ISearchForAPatientWithParameterNameAndParameterString("identifier", parameterString);
        }

        [When(@"I make a GET request for patient ""([^""]*)""")]
        public void IMakeAGETRequestForPatient(string patient)
        {
            var patientResource = HttpContext.StoredFhirResources[patient];
            When($@"I make a GET request to ""/Patient/{patientResource.Id}""");
        }

        [When(@"I make a GET request for patient ""([^""]*)"" with If-None-Match header")]
        public void IMakeAGETRequestForPatientWithIf_None_MatchHeader(string patient)
        {
            var etag = "W/\"" + HttpContext.StoredFhirResources[patient].Meta.VersionId + "\"";

            HttpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kIfNoneMatch, etag);
            IMakeAGETRequestForPatient(patient);
        }

        [When(@"I perform a patient vread for patient ""([^""]*)""")]
        public void IPerformAPatientVReadForPatient(string patient)
        {
            var patientResource = HttpContext.StoredFhirResources[patient];

            var versionId = HttpContext.StoredFhirResources[patient].Meta.VersionId;
            
            When($@"I make a GET request to ""/Patient/{patientResource.Id}/_history/{versionId}""");
        }

        [When(@"I perform a patient vread for patient ""([^""]*)"" with invalid ETag")]
        public void IPerformAPatientVReadForPatientWithInvalidETag(string patient)
        {
            var patientResource = HttpContext.StoredFhirResources[patient];
            When($@"I make a GET request to ""/Patient/{patientResource.Id}/_history/badETag""");
        }

        [Then(@"the response patient logical identifier should match that of stored patient ""([^""]*)""")]
        public void TheResponsePatientLogicalIdentifierShouldMatchThatOfStoredPatient(string patient)
        {
            var id = HttpContext.StoredFhirResources[patient].Id;
            
            id.ShouldBe(((Patient)_fhirContext.FhirResponseResource).Id);
        }

        [When(@"I search for Patient ""([^""]*)"" with system ""([^""]*)""")]
        public void ISearchForPatientWithSystem(string patient, string identifierSystem)
        {
            var parameterString = identifierSystem + "|" + GlobalContext.PatientNhsNumberMap[patient];
            ISearchForAPatientWithParameterNameAndParameterString("identifier", parameterString);
        }

        [When(@"I search for Patient ""([^""]*)"" without system in identifier parameter")]
        public void ISearchForPatientWithoutSystemInIdentifierParameter(string patient)
        {
            ISearchForAPatientWithParameterNameAndParameterString("identifier", GlobalContext.PatientNhsNumberMap[patient]);
        }

        [When(@"I search for Patient ""([^""]*)"" with parameter name ""([^""]*)"" and system ""([^""]*)""")]
        public void ISearchForPatientWithParameterNameAndSystem(string patient, string parameterName, string parameterSystem)
        {
            var parameterString = parameterSystem + "|" + GlobalContext.PatientNhsNumberMap[patient];
            ISearchForAPatientWithParameterNameAndParameterString(parameterName, parameterString);
        }

        [When(@"I search for a Patient with patameter name ""([^""]*)"" and parameter string ""([^""]*)""")]
        public void ISearchForAPatientWithParameterNameAndParameterString(string parameterName, string parameterString)
        {
            Given($@"I add the parameter ""{parameterName}"" with the value ""{parameterString}""");
            When($@"I make a GET request to ""/Patient""");
        }
        
        [Then(@"the response bundle Patient entries should contain a single NHS Number identifier for patient ""([^""]*)""")]
        public void ThenTheResponseBundlePatientEntriesShouldContainASingleNHSNumberIdentifierForPatient(string patient)
        {
            Bundle bundle = (Bundle)_fhirContext.FhirResponseResource;
            foreach (var entry in bundle.Entry) {
                var patientResource = (Patient)entry.Resource;
                int nhsNumberIdentifierCount = 0;
                foreach (var identifier in patientResource.Identifier) {
                    if (identifier.System == FhirConst.IdentifierSystems.kNHSNumber) {
                        nhsNumberIdentifierCount++;
                        identifier.Value.ShouldBe(GlobalContext.PatientNhsNumberMap[patient],"NHS Number does not match expected NHS Number.");
                    }
                }
                nhsNumberIdentifierCount.ShouldBe(1, "There was more or less than 1 NHS Number identifier.");
            }
        }

        [Then(@"if Patient resource contains careProvider the reference must be valid")]
        public void ThenIfPatientResourceContainsCareProviderTheReferenceMustBeValid()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    if (patient.CareProvider != null) {
                        patient.CareProvider.Count.ShouldBeLessThanOrEqualTo(1, "There should be a maximum of one care provider elements included in Patient Resource.");
                        foreach (var careProvider in patient.CareProvider)
                        {
                            if (careProvider.Reference != null)
                            {
                                careProvider.Reference.ShouldStartWith("Practitioner/");
                                var returnedResource = _httpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner", careProvider.Reference);
                                returnedResource.GetType().ShouldBe(typeof(Practitioner));
                            }
                        }
                    }
                }
            }
        }

        [Then(@"if Patient resource contains a managing organization the reference must be valid")]
        public void ThenIfPatientResourceContainsAManagingOrganizationTheReferenceMustBeValid()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    if (patient.ManagingOrganization != null && patient.ManagingOrganization.Reference != null)
                    {
                        patient.ManagingOrganization.Reference.ShouldStartWith("Organization/");
                        var returnedResource = _httpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:organization", patient.ManagingOrganization.Reference);
                        returnedResource.GetType().ShouldBe(typeof(Organization));
                    }
                }
            }
        }

        [Then(@"if patient resource contains telecom element the system and value must be populated")]
        public void ThenIfPatientResourceContainsTelecomElementTheSystemAndValueMustBePopulated ()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    if (patient.Telecom != null)
                    {
                        foreach (ContactPoint contactPoint in patient.Telecom) {
                            contactPoint.System.ShouldNotBeNull("The contactPoint system should be populated");
                            contactPoint.Value.ShouldNotBeNull("The contactPoint value should be populated");
                        }
                    }
                }
            }
        }

        [Then(@"if patient resource contains deceased element it should be dateTime and not boolean")]
        public void ThenIfPatientResourceContainsDeceasedElementItShouldBeDateTimeAndNotBoolean()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    if (patient.Deceased != null)
                    {
                        patient.Deceased.GetType().ShouldBe(typeof(FhirDateTime));
                    }
                }
            }
        }

        [Then(@"if composition contains the patient resource and it contains the multiple birth field it should be a boolean value")]
        public void ThenIfCompositionContainsThePatientResourceAndItContainsTheMultipleBirthFieldItShouldBeABooleanValue()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    if (patient.MultipleBirth != null)
                    {
                        patient.MultipleBirth.GetType().ShouldBe(typeof(FhirBoolean));
                    }
                }
            }
        }

        [Then(@"if composition contains the patient resource contact the mandatory fields should matching the specification")]
        public void ThenIfCompositionContainsThePatientResourceContactTheMandatoryFieldsShouldMatchingTheSpecification()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    foreach (Patient.ContactComponent contact in patient.Contact)
                    {
                        // Contact Relationship Checks
                        foreach (CodeableConcept relationship in contact.Relationship)
                        {
                            shouldBeSingleCodingWhichIsInValuest(GlobalContext.FhirRelationshipValueSet, relationship.Coding);
                        }

                        // Contact Name Checks
                        foreach (var name in patient.Name)
                        {
                            name.FamilyElement.Count.ShouldBeLessThanOrEqualTo(1,"There are too many family names within the contact element.");
                        }
                        
                        // Contact Organization Checks
                        if (contact.Organization != null && contact.Organization.Reference != null)
                        {
                            contact.Organization.Reference.ShouldStartWith("Organization/");
                            var returnedResource = _httpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:organization", contact.Organization.Reference);
                            returnedResource.GetType().ShouldBe(typeof(Organization));
                        }
                    }
                }
            }
        }

        [Then(@"if careProvider is present in patient resource the reference should be valid")]
        public void ThenIfCareProviderIsPresentInPatientResourceTheReferenceShouldBeValid()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    if (patient.CareProvider != null)
                    {
                        foreach (var careProvider in patient.CareProvider)
                        {
                            careProvider.Reference.ShouldStartWith("Practitioner/");
                            var returnedResource = _httpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner", careProvider.Reference);
                            returnedResource.GetType().ShouldBe(typeof(Practitioner));
                        }
                    }
                }
            }
        }

        [Then(@"if managingOrganization is present in patient resource the reference should be valid")]
        public void ThenIfManagingOrganizationIsPresentInPatientResourceTheReferenceShouldBeValid()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    if (patient.ManagingOrganization != null)
                    {
                        patient.ManagingOrganization.Reference.ShouldStartWith("Organization/");
                        var returnedResource = _httpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:organization", patient.ManagingOrganization.Reference);
                        returnedResource.GetType().ShouldBe(typeof(Organization));
                    }
                }
            }
        }

        public void shouldBeSingleCodingWhichIsInValuest(ValueSet valueSet, List<Coding> codingList)
        {
            var codingCount = 0;
            foreach (Coding coding in codingList)
            {
                codingCount++;
                valueSetContainsCodeAndDisplay(valueSet, coding);
            }
            codingCount.ShouldBeLessThanOrEqualTo(1);
        }

        public void valueSetContainsCodeAndDisplay(ValueSet valueset, Coding coding)
        {
            coding.System.ShouldBe(valueset.CodeSystem.System);
            // Loop through valid codes to find if the one in the resource is valid

            foreach (ValueSet.ConceptDefinitionComponent valueSetConcept in valueset.CodeSystem.Concept)
            {
                coding.Code.ShouldBe(valueSetConcept.Code);
                coding.Display.ShouldBe(valueSetConcept.Display);
            }
        }

        [Then(@"the response should be a Patient resource")]
        public void theResponseShouldBeAPatientResource()
        {
            _fhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.Patient);
        }

        [Then(@"the patient resource should contain an id")]
        public void ThenThePatientResourceShouldContainAnId()
        {
            ((Patient)_fhirContext.FhirResponseResource).Id.ShouldNotBeNullOrWhiteSpace("Id must be set");
        }

        [Then(@"the patient resource should contain valid meta data")]
        public void ThenThePatientResourceShouldContainValidMetaData()
        {
            ((Patient)_fhirContext.FhirResponseResource).Meta.ShouldNotBeNull("Null Meta");
            ((Patient)_fhirContext.FhirResponseResource).Meta.VersionId.ShouldNotBeNull("Null Meta VersionId");
            ((Patient)_fhirContext.FhirResponseResource).Meta.LastUpdated.ShouldNotBeNull("Null Meta LastUpdated");
            ((Patient)_fhirContext.FhirResponseResource).Meta.Profile.ShouldHaveSingleItem("Empty Meta Profile");
            ((Patient)_fhirContext.FhirResponseResource).Meta.ProfileElement[0].Value.ShouldBe("http://fhir.nhs.net/StructureDefinition/gpconnect-patient-1");
        }

        [Then(@"the patient resource should contain a single NHS Number identifier for patient ""([^""]*)""")]
        public void ThenThePatientResourceShouldContainASingleNHSNumberIdentifierForPatient(string patient)
        {
            ((Patient)_fhirContext.FhirResponseResource).Identifier.ShouldNotBeNull("Null Identifier");
            ((Patient)_fhirContext.FhirResponseResource).Identifier.ShouldHaveSingleItem("Empty Identifier");
            ((Patient)_fhirContext.FhirResponseResource).Identifier[0].System.ShouldBe("http://fhir.nhs.net/Id/nhs-number");
            ((Patient)_fhirContext.FhirResponseResource).Identifier[0].Value.ShouldBe(GlobalContext.PatientNhsNumberMap[patient]);
        }

        [Then(@"if the patient resource contains a careProvider Practitioner reference it is valid")]
        public void ThenIfThePatientResourceContainsACareProviderPractitionerReferenceItIsValid()
        {
            var CareProvider = ((Patient)_fhirContext.FhirResponseResource).CareProvider;

            if (null != CareProvider)
            {
                CareProvider.Count.ShouldBeLessThanOrEqualTo(1);

                foreach (var careProvider in CareProvider)
                {
                    if (null != careProvider.Reference)
                    {
                        careProvider.Reference.ShouldStartWith("Practitioner/");
                        var returnedResource = _httpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner", careProvider.Reference);
                        returnedResource.GetType().ShouldBe(typeof(Practitioner));
                    }
                }
            }
        }

        [Then(@"if the patient resource contains a managingOrganization Organization reference it is valid")]
        public void ThenIfThePatientResourceContainsAManagingOrganizationReferenceItIsValid()
        {
            var ManagingOrganization = ((Patient)_fhirContext.FhirResponseResource).ManagingOrganization;

            if (ManagingOrganization != null)
            {
                if (null != ManagingOrganization.Reference)
                {
                    ManagingOrganization.Reference.ShouldStartWith("Organization/");
                    var returnedResource = _httpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:organization", ManagingOrganization.Reference);
                    returnedResource.GetType().ShouldBe(typeof(Organization));
                }
            }
        }

        [Then(@"if the patient resource contains a deceased dateTime field it is valid")]
        public void ThenIfThePatientResourceContainsADeceasedDateTimeFieldItIsValid()
        {
            var Deceased = ((Patient)_fhirContext.FhirResponseResource).Deceased;

            if (Deceased != null)
            {
                Deceased.ShouldBeOfType<FhirDateTime>();
            }
        }

        [Then(@"if the patient resource contains a multipleBirth boolean field it is valid")]
        public void ThenThePatientResourceShouldContainAValidMultipleBirthBooleanField()
        {
            var MultipleBirth = ((Patient)_fhirContext.FhirResponseResource).MultipleBirth;

            if (MultipleBirth != null)
            {
                MultipleBirth.ShouldBeOfType<FhirBoolean>();
            }
        }

        [Then(@"if the patient resource contains telecom fields they are valid")]
        public void ThenThePatientResourceShouldContainValidTelecomFields()
        {
            foreach (var contactPoint in ((Patient)_fhirContext.FhirResponseResource).Telecom)
            {
                contactPoint.System.ShouldNotBeNull("ContactPoint system was null.");
                contactPoint.Value.ShouldNotBeNullOrWhiteSpace("ContactPoint value was null/blank.");
            }
        }

        [Then(@"if the patient resource contains relationship coding fields they are valid")]
        public void ThenThePatientResourceShouldContainValidRelationshipCodingFields()
        {
            foreach (var contact in ((Patient)_fhirContext.FhirResponseResource).Contact)
            {
                foreach (var relationship in contact.Relationship)
                {
                    shouldBeSingleCodingWhichIsInValuest(GlobalContext.FhirRelationshipValueSet, relationship.Coding);
                }
            }
        }

        [Then(@"if the patient resource contains maritalStatus coding fields they are valid")]
        public void ThenThePatientResourceShouldContainValidMaritalStatusCodingFields()
        {
            var maritalStatus = ((Patient)_fhirContext.FhirResponseResource).MaritalStatus;

            if (null != maritalStatus)
            {
                shouldBeSingleCodingWhichIsInValuest(GlobalContext.FhirMaritalStatusValueSet, maritalStatus.Coding);
            }
        }

        [Then(@"if the patient resource contains language coding fields for each communication they are valid")]
        public void ThenThePatientResourceShouldContainValidLanguageCodingFieldsForEachCommunication()
        {
            foreach (var communication in ((Patient)_fhirContext.FhirResponseResource).Communication)
            {
                shouldBeSingleCodingWhichIsInValuest(GlobalContext.FhirHumanLanguageValueSet, communication.Language.Coding);
            }
        }

        [Then(@"the patient resource should contain no more than one family or given name")]
        public void ThenThePatientResourceShouldContainNoMoreThanOneFamilyOrGivenName()
        {
            foreach (var name in ((Patient)_fhirContext.FhirResponseResource).Name)
            {
                int familynamecount = 0;

                foreach (var familyname in name.Family)
                {
                    familynamecount++;
                }

                familynamecount.ShouldBeLessThanOrEqualTo(1);

                int givennamecount = 0;

                foreach (var givenname in name.Given)
                {
                    givennamecount++;
                }

                givennamecount.ShouldBeLessThanOrEqualTo(1);
            }
        }

        [Then(@"the patient resource should contain no more than one family name field for each contact")]
        public void ThenThePatientResourceShouldContainNoMoreThanOneFamilyNameFieldForEachContact()
        {
            foreach (var contact in ((Patient)_fhirContext.FhirResponseResource).Contact)
            {
                var name = contact.Name;

                if (null != name)
                {
                    int familynamecount = 0;

                    foreach (var familyname in name.Family)
                    {
                        familynamecount++;
                    }

                    familynamecount.ShouldBeLessThanOrEqualTo(1);
                }
            }
        }

        #region Hayden

        private List<Patient> Patients => _fhirContext.Patients;

        [Then(@"the Patient Identifiers should be valid")]
        public void ThePatientIdentifiersShouldBeValid()
        {
            Patients.ForEach(patient =>
            {
                patient.Identifier.ShouldNotBeNull("The patient identifier should not be null");
                patient.Identifier.Count.ShouldBe(1);

                var identifier = patient.Identifier.First();

                FhirConst.IdentifierSystems.kNHSNumber.Equals(identifier.System).ShouldBeTrue();
                NhsNumberHelper.IsNhsNumberValid(identifier.Value).ShouldBeTrue();
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

        [Then(@"if composition contains the patient resource communication the mandatory fields should match the specification")]
        public void ThenIfCompositionContainsThePatientResourceCommunicationTheMandatoryFieldsShouldMatchTheSpecification()
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

        [Then(@"the Patient should exclude photo and link and animal fields")]
        public void ThePatientShouldExcludePhotoAndLinkAndAnimalFields()
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

        #endregion

        [Given(@"I add a Patient Identifier parameter with System ""([^""]*)"" and Value ""([^""]*)""")]
        public void AddAPatientIdentifierParameterWithSystemAndValue(string system, string value)
        {
            HttpContext.RequestParameters.AddParameter("identifier", system + '|' + GlobalContext.PatientNhsNumberMap[value]);
        }

        [Given(@"I add a Patient Identifier parameter with default System and Value ""([^""]*)""")]
        public void AddAPatientIdentifierParameterWithDefaultSystemAndValue(string value)
        {
            AddAPatientIdentifierParameterWithSystemAndValue(FhirConst.IdentifierSystems.kNHSNumber, value);
        }

        [Given(@"I add a Patient Identifier parameter with default System and NHS number ""([^""]*)""")]
        public void AddAPatientIdentifierParameterWithDefaultSystemAndNhsNumber(string nhsNumber)
        {
            HttpContext.RequestParameters.AddParameter("identifier", FhirConst.IdentifierSystems.kNHSNumber + '|' + nhsNumber);
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
                HttpContext.StoredPatient = patient;
            }
        }

        [Given(@"I store the Patient Id")]
        public void StoreThePatientId()
        {
            var patient = _fhirContext.Patients.FirstOrDefault();

            if (patient != null)
            {
                HttpContext.GetRequestId = patient.Id;
            }
        }
    }
}
