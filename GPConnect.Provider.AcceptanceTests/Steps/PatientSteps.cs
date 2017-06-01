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

    [Binding]
    public class PatientSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private readonly SecurityContext SecurityContext;
        private readonly HttpContext HttpContext;
        private readonly HttpSteps HttpSteps;
        
        // Constructor

        public PatientSteps(SecurityContext securityContext, HttpContext httpContext, FhirContext fhirContext, HttpSteps httpSteps)
        {
            Log.WriteLine("PatientSteps() Constructor");
            SecurityContext = securityContext;
            HttpContext = httpContext;            
            FhirContext = fhirContext;
            HttpSteps = httpSteps;
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

            var returnedFirstResource = (Patient)((Bundle)FhirContext.FhirResponseResource).Entry[0].Resource;
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
            HttpContext.StoredFhirResources.Add(patientSearchResponseBundleKey, (Bundle)FhirContext.FhirResponseResource);
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

        [When(@"I perform a patient vread for patient ""([^""]*)"" with ETag ""([^""]*)""")]
        public void IPerformAPatientVReadForPatientWithETag(string patient, string etag)
        {
            var patientResource = HttpContext.StoredFhirResources[patient];

            string versionId = HttpContext.resourceNameStored[etag];
            string[] elements = versionId.Split(new char[] { '"' });

            When($@"I make a GET request to ""/Patient/{patientResource.Id}/_history/{elements[1]}""");
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
            
            id.ShouldBe(((Patient)FhirContext.FhirResponseResource).Id);
        }

        [When(@"I search for Patient ""([^""]*)"" with system ""([^""]*)""")]
        public void ISearchForPatientWithSystem(string patient, string identifierSystem)
        {
            var parameterString = identifierSystem + "|" + FhirContext.FhirPatients[patient];
            ISearchForAPatientWithParameterNameAndParameterString("identifier", parameterString);
        }

        [When(@"I search for Patient ""([^""]*)"" without system in identifier parameter")]
        public void ISearchForPatientWithoutSystemInIdentifierParameter(string patient)
        {
            ISearchForAPatientWithParameterNameAndParameterString("identifier", FhirContext.FhirPatients[patient]);
        }

        [When(@"I search for Patient ""([^""]*)"" with parameter name ""([^""]*)"" and system ""([^""]*)""")]
        public void ISearchForPatientWithParameterNameAndSystem(string patient, string parameterName, string parameterSystem)
        {
            var parameterString = parameterSystem + "|" + FhirContext.FhirPatients[patient];
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
            Bundle bundle = (Bundle)FhirContext.FhirResponseResource;
            foreach (var entry in bundle.Entry) {
                var patientResource = (Patient)entry.Resource;
                int nhsNumberIdentifierCount = 0;
                foreach (var identifier in patientResource.Identifier) {
                    if (identifier.System == FhirConst.IdentifierSystems.kNHSNumber) {
                        nhsNumberIdentifierCount++;
                        identifier.Value.ShouldBe(FhirContext.FhirPatients[patient],"NHS Number does not match expected NHS Number.");
                    }
                }
                nhsNumberIdentifierCount.ShouldBe(1, "There was more or less than 1 NHS Number identifier.");
            }
        }

        [Then(@"if Patient resource contains careProvider the reference must be valid")]
        public void ThenIfPatientResourceContainsCareProviderTheReferenceMustBeValid()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
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
                                var returnedResource = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner", careProvider.Reference);
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
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    if (patient.ManagingOrganization != null && patient.ManagingOrganization.Reference != null)
                    {
                        patient.ManagingOrganization.Reference.ShouldStartWith("Organization/");
                        var returnedResource = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:organization", patient.ManagingOrganization.Reference);
                        returnedResource.GetType().ShouldBe(typeof(Organization));
                    }
                }
            }
        }

        [Then(@"if patient resource contains telecom element the system and value must be populated")]
        public void ThenIfPatientResourceContainsTelecomElementTheSystemAndValueMustBePopulated ()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
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
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
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
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
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
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
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
                            var returnedResource = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:organization", contact.Organization.Reference);
                            returnedResource.GetType().ShouldBe(typeof(Organization));
                        }
                    }
                }
            }
        }

        [Then(@"if careProvider is present in patient resource the reference should be valid")]
        public void ThenIfCareProviderIsPresentInPatientResourceTheReferenceShouldBeValid()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    if (patient.CareProvider != null)
                    {
                        foreach (var careProvider in patient.CareProvider)
                        {
                            careProvider.Reference.ShouldStartWith("Practitioner/");
                            var returnedResource = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner", careProvider.Reference);
                            returnedResource.GetType().ShouldBe(typeof(Practitioner));
                        }
                    }
                }
            }
        }

        [Then(@"if managingOrganization is present in patient resource the reference should be valid")]
        public void ThenIfManagingOrganizationIsPresentInPatientResourceTheReferenceShouldBeValid()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    if (patient.ManagingOrganization != null)
                    {
                        patient.ManagingOrganization.Reference.ShouldStartWith("Organization/");
                        var returnedResource = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:organization", patient.ManagingOrganization.Reference);
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
            FhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.Patient);
        }

        [Then(@"the patient resource should contain an id")]
        public void ThenThePatientResourceShouldContainAnId()
        {
            ((Patient)FhirContext.FhirResponseResource).Id.ShouldNotBeNullOrWhiteSpace("Id must be set");
        }

        [Then(@"the patient resource should contain valid meta data")]
        public void ThenThePatientResourceShouldContainValidMetaData()
        {
            ((Patient)FhirContext.FhirResponseResource).Meta.ShouldNotBeNull("Null Meta");
            ((Patient)FhirContext.FhirResponseResource).Meta.VersionId.ShouldNotBeNull("Null Meta VersionId");
            ((Patient)FhirContext.FhirResponseResource).Meta.LastUpdated.ShouldNotBeNull("Null Meta LastUpdated");
            ((Patient)FhirContext.FhirResponseResource).Meta.Profile.ShouldHaveSingleItem("Empty Meta Profile");
            ((Patient)FhirContext.FhirResponseResource).Meta.ProfileElement[0].Value.ShouldBe("http://fhir.nhs.net/StructureDefinition/gpconnect-patient-1");
        }

        [Then(@"the patient resource should contain a single NHS Number identifier for patient ""([^""]*)""")]
        public void ThenThePatientResourceShouldContainASingleNHSNumberIdentifierForPatient(string patient)
        {
            ((Patient)FhirContext.FhirResponseResource).Identifier.ShouldNotBeNull("Null Identifier");
            ((Patient)FhirContext.FhirResponseResource).Identifier.ShouldHaveSingleItem("Empty Identifier");
            ((Patient)FhirContext.FhirResponseResource).Identifier[0].System.ShouldBe("http://fhir.nhs.net/Id/nhs-number");
            ((Patient)FhirContext.FhirResponseResource).Identifier[0].Value.ShouldBe(FhirContext.FhirPatients[patient]);
        }

        [Then(@"the patient resource should contain a valid careProvider Practitioner reference")]
        public void ThenThePatientResourceShouldContainAValidCareProviderPractitionerReference()
        {
            var CareProvider = ((Patient)FhirContext.FhirResponseResource).CareProvider;

            CareProvider.Count.ShouldBeLessThanOrEqualTo(1);

            if (CareProvider.Count > 0)
            {
                CareProvider[0].Reference.ShouldNotBeNull();
                CareProvider[0].Reference.ShouldStartWith("Practitioner/");
                var returnedResource = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner", CareProvider[0].Reference);
                returnedResource.GetType().ShouldBe(typeof(Practitioner));
            }
        }

        [Then(@"the patient resource should contain a valid managingOrganization Organization reference")]
        public void ThenThePatientResourceShouldContainAValidManagingOrganizationOrganizationReference()
        {
            var ManagingOrganization = ((Patient)FhirContext.FhirResponseResource).ManagingOrganization;

            if (ManagingOrganization != null)
            {
                ManagingOrganization.Reference.ShouldNotBeNull();
                ManagingOrganization.Reference.ShouldStartWith("Organization/");
                var returnedResource = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:organization", ManagingOrganization.Reference);
                returnedResource.GetType().ShouldBe(typeof(Organization));
            }
        }

        [Then(@"the patient resource should contain a valid deceased dateTime field")]
        public void ThenThePatientResourceShouldContainAValidDeceasedDateTimeField()
        {
            var Deceased = ((Patient)FhirContext.FhirResponseResource).Deceased;

            if (Deceased != null)
            {
                Deceased.ShouldBeOfType<FhirDateTime>();
            }
        }

        [Then(@"the patient resource should contain a valid multipleBirth boolean field")]
        public void ThenThePatientResourceShouldContainAValidMultipleBirthBooleanField()
        {
            var MultipleBirth = ((Patient)FhirContext.FhirResponseResource).MultipleBirth;

            if (MultipleBirth != null)
            {
                MultipleBirth.ShouldBeOfType<FhirBoolean>();
            }
        }

        [Then(@"the patient resource should contain valid telecom fields")]
        public void ThenThePatientResourceShouldContainValidTelecomFields()
        {
            foreach (var contactPoint in ((Patient)FhirContext.FhirResponseResource).Telecom)
            {
                contactPoint.System.ShouldNotBeNull("ContactPoint system was null.");
                contactPoint.Value.ShouldNotBeNullOrWhiteSpace("ContactPoint value was null/blank.");
            }
        }

        [Then(@"the patient resource should contain valid relationship coding fields")]
        public void ThenThePatientResourceShouldContainValidRelationshipCodingFields()
        {
            foreach (var contact in ((Patient)FhirContext.FhirResponseResource).Contact)
            {
                foreach (var relationship in contact.Relationship)
                {
                    shouldBeSingleCodingWhichIsInValuest(GlobalContext.FhirRelationshipValueSet, relationship.Coding);
                }
            }
        }

        [Then(@"the patient resource should contain valid maritalStatus coding fields")]
        public void ThenThePatientResourceShouldContainValidMaritalStatusCodingFields()
        {
            var maritalStatus = ((Patient)FhirContext.FhirResponseResource).MaritalStatus;

            if (null != maritalStatus)
            {
                shouldBeSingleCodingWhichIsInValuest(GlobalContext.FhirMaritalStatusValueSet, maritalStatus.Coding);
            }
        }

        [Then(@"the patient resource should contain valid language coding fields for each communication")]
        public void ThenThePatientResourceShouldContainValidLanguageCodingFieldsForEachCommunication()
        {
            foreach (var communication in ((Patient)FhirContext.FhirResponseResource).Communication)
            {
                shouldBeSingleCodingWhichIsInValuest(GlobalContext.FhirHumanLanguageValueSet, communication.Language.Coding);
            }
        }

        [Then(@"the patient resource should contain no more than one family or given name")]
        public void ThenThePatientResourceShouldContainNoMoreThanOneFamilyOrGivenName()
        {
            foreach (var name in ((Patient)FhirContext.FhirResponseResource).Name)
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
            foreach (var contact in ((Patient)FhirContext.FhirResponseResource).Contact)
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
        
        [Then(@"the patient resource should not contain the fhir fields photo animal or link")]
        public void ThenThePatientResourceShouldNotContainTheFhirFieldsPhotoAnimalOrLink()
        {
            Patient patient = (Patient)FhirContext.FhirResponseResource;
            (patient.Photo == null || patient.Photo.Count == 0).ShouldBeTrue("There should be no photo element in Patient Resource"); // C# API creates an empty list if no element is present
            patient.Animal.ShouldBeNull("There should be no Animal element in Patient Resource");
            (patient.Link == null || patient.Link.Count == 0).ShouldBeTrue("There should be no link element in Patient Resource");
        }
    }
}
