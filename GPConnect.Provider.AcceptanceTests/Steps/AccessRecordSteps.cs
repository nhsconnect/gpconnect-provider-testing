using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Data;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using static Hl7.Fhir.Model.Bundle;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public sealed class AccessRecordSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private readonly HttpContext HttpContext;

        public AccessRecordSteps(FhirContext fhirContext, HttpContext httpContext)
        {
            FhirContext = fhirContext;
            HttpContext = httpContext;
        }

        [Given(@"I have the following patient records")]
        public void GivenIHaveTheFollowingPatientRecords(Table table)
        {
            FhirContext.FhirPatients.Clear();
            foreach (var row in table.Rows)
            {
                string mappedNHSNumber = row["NHSNumber"];
                // Map the native NHS Number to provider equivalent from CSV file
                foreach (NHSNoMap nhsNoMap in GlobalContext.NHSNoMapData)
                {
                    if (String.Equals(nhsNoMap.NativeNHSNumber, row["NHSNumber"])) {
                        mappedNHSNumber = nhsNoMap.ProviderNHSNumber;
                        Log.WriteLine("Mapped test NHS number {0} to NHS Number {1}", row["NHSNumber"], nhsNoMap.ProviderNHSNumber);
                        break;
                    }
                }
                FhirContext.FhirPatients.Add(row["Id"], mappedNHSNumber);
            }
        }

        [Given(@"I am requesting the record for config patient ""([^""]*)""")]
        public void GivenIAmRequestingTheRecordForConfigPatient(string patient)
        {
            Given($@"I am requesting the record for patient with NHS Number ""{FhirContext.FhirPatients[patient]}""");
        }

        [Given(@"I replace the parameter name ""([^""]*)"" with ""([^""]*)""")]
        public void GivenIReplaceTheParameterNameWith(string parameterName, string newParameterName)
        {
            FhirContext.FhirRequestParameters.GetSingle(parameterName).Name = newParameterName;
        }

        [Given(@"I set the parameter patientNHSNumber with an empty value")]
        public void GivenISetThePatientNHSNumberParameterWithAnEmptyValue()
        {
            var parameter = FhirContext.FhirRequestParameters.GetSingle("patientNHSNumber");
            ((Identifier)parameter.Value).Value = "";
        }

        [Given(@"I set the parameter patientNHSNumber with an empty system")]
        public void GivenISetThePatientNHSNumberParameterWithAnEmptySystem()
        {
            var parameter = FhirContext.FhirRequestParameters.GetSingle("patientNHSNumber");
            ((Identifier)parameter.Value).System = "";
        }

        [Given(@"I set the parameter recordSection with an empty system")]
        public void GivenISetTheRecordSectionParameterWithAnEmptySystem()
        {
            var parameter = FhirContext.FhirRequestParameters.GetSingle("recordSection");
            ((CodeableConcept)parameter.Value).Coding[0].System = "";
        }

        [Then(@"the JSON response should be a Bundle resource")]
        public void ThenTheJSONResponseShouldBeABundleResource()
        {
            FhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.Bundle);
        }

        [Then(@"the JSON response should be a OperationOutcome resource")]
        public void ThenTheJSONResponseShouldBeAOperationOutcomeResource()
        {
            FhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.OperationOutcome);
            OperationOutcome operationOutcome = (OperationOutcome)FhirContext.FhirResponseResource;
            if (operationOutcome.Meta != null)
            {
                foreach (string profile in operationOutcome.Meta.Profile)
                {
                    profile.ShouldBe("http://fhir.nhs.net/StructureDefinition/gpconnect-operationoutcome-1");
                }
            }
            else
            {
                operationOutcome.Meta.ShouldNotBeNull();
            }

            foreach (OperationOutcome.IssueComponent issue in operationOutcome.Issue)
            {
                {
                    if (issue.Details != null)
                    {
                        foreach (Coding coding in issue.Details.Coding)
                        {
                            coding.System.ShouldBe("http://fhir.nhs.net/ValueSet/gpconnect-error-or-warning-code-1");
                            coding.Code.ShouldNotBeNull();
                            coding.Display.ShouldNotBeNull();
                        }
                    }
                    else
                    {
                        issue.Details.ShouldNotBeNull();
                    }
                }
            }
        }

        [Then(@"the JSON response should be a OperationOutcome resource with error code ""([^""]*)""")]
        public void ThenTheJSONResponseShouldBeAOperationOutcomeResourceWithErrorCode(string errorCode)
        {
            FhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.OperationOutcome);
            List<string> errorCodes = new List<string>();
            OperationOutcome operationOutcome = (OperationOutcome)FhirContext.FhirResponseResource;

            if (operationOutcome.Meta != null)
            {
                foreach (string profile in operationOutcome.Meta.Profile)
                {
                    profile.ShouldBe("http://fhir.nhs.net/StructureDefinition/gpconnect-operationoutcome-1");
                }
            }
            else
            {
                operationOutcome.Meta.ShouldNotBeNull();
            }
            foreach (OperationOutcome.IssueComponent issue in operationOutcome.Issue)
            {
                {
                    if (issue.Details != null)
                    {
                        foreach (Coding coding in issue.Details.Coding)
                        {
                            coding.System.ShouldBe("http://fhir.nhs.net/ValueSet/gpconnect-error-or-warning-code-1");
                            errorCodes.Add(coding.Code);
                            coding.Display.ShouldNotBeNull();
                        }
                    }
                    else
                    {
                        issue.Details.ShouldNotBeNull();
                    }
                }

                errorCodes.ShouldContain(errorCode);
            }
        }

        [Then(@"the JSON response bundle should contain a single Patient resource")]
        public void ThenTheJSONResponseBundleShouldContainASinglePatientResource()
        {
            int count = 0;
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient)) count++;
            }
            count.ShouldBe(1);
        }

        [Then(@"the JSON response bundle should contain a single Composition resource")]
        public void ThenTheJSONResponseBundleShouldContainASingleCompositionResource()
        {
            int count = 0;
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition)) count++;
            }
            count.ShouldBe(1);
        }

        [Then(@"the JSON response bundle should be type document")]
        public void ThenTheJSONResponseBundleShouldBeTypeDocument()
        {
            ((Bundle)FhirContext.FhirResponseResource).Type.ShouldBe(BundleType.Document);
        }
        [Then(@"the JSON response bundle should be type searchset")]
        public void ThenTheJSONResponseBundleShouldBeTypeSearchSet()
        {
            ((Bundle)FhirContext.FhirResponseResource).Type.ShouldBe(BundleType.Searchset);
        }


        [Then(@"the JSON response bundle should contain the composition resource as the first entry")]
        public void ThenTheJSONResponseBundleShouldContainTheCompositionResourceAsTheFirstEntry()
        {
            ((Bundle)FhirContext.FhirResponseResource).Entry[0].Resource.ResourceType.ShouldBe(ResourceType.Composition);
        }

        [Then(@"response bundle Patient entry should be a valid Patient resource")]
        public void ThenResponseBundlePatientEntryShouldBeAValidPatientResource()
        {
            var fhirResource = HttpContext.ResponseJSON.SelectToken($"$.entry[?(@.resource.resourceType == 'Patient')].resource");
            FhirJsonParser fhirJsonParser = new FhirJsonParser();
            var patientResource = fhirJsonParser.Parse<Patient>(JsonConvert.SerializeObject(fhirResource));
            patientResource.ResourceType.ShouldBe(ResourceType.Patient);
        }

        [Then(@"response bundle Patient entry should contain a valid NHS number identifier")]
        public void ThenResponseBundlePatientEntryShouldContainAValidNHSNumberIdentifier()
        {
            var passed = false;
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    foreach (var identifier in patient.Identifier)
                    {
                        if (FhirConst.IdentifierSystems.kNHSNumber.Equals(identifier.System) && FhirHelper.isValidNHSNumber(identifier.Value))
                        {
                            passed = true;
                            break;
                        }
                    }
                    passed.ShouldBeTrue();
                }
            }
        }

        [Then(@"response bundle Patient resource should contain valid telecom information")]
        public void ThenResponseBundlePatientResourceShouldContainValidTelecomInfromation()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    foreach (var telecom in patient.Telecom)
                    {
                        telecom.System.ShouldNotBeNull();
                        telecom.Value.ShouldNotBeNull();
                    }
                }
            }
        }

        [Then(@"if composition contains the resource type element the fields should match the fixed values of the specification")]
        public void ThenIfCompositionContainsTheResourceTypeElementTheFieldsShouldMatchTheFixedValuesOfTheSpecification()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
                {
                    Composition composition = (Composition)entry.Resource;

                    if (composition.Type == null)
                    {
                        Assert.Pass();
                    }
                    else
                    {
                        if (composition.Type.Coding != null)
                        {
                            int codingCount = 0;
                            foreach (Coding coding in composition.Type.Coding)
                            {
                                codingCount++;
                                coding.System.ShouldBe("http://snomed.info/sct");
                                coding.Code.ShouldBe("425173008");
                                coding.Display.ShouldBe("record extract (record artifact)");
                            }
                            codingCount.ShouldBeLessThanOrEqualTo(1);
                        }

                        if (composition.Type.Text != null)
                        {
                            composition.Type.Text.ShouldBe("record extract (record artifact)");
                        }
                    }
                }
            }
        }

        [Then(@"if composition contains the resource class element the fields should match the fixed values of the specification")]
        public void ThenIfCompositionContainsTheResourceClassElementTheFieldsShouldMatchTheFixedValuesOfTheSpecification()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
                {
                    Composition composition = (Composition)entry.Resource;

                    if (composition.Class == null)
                    {
                        Assert.Pass();
                    }
                    else
                    {
                        if (composition.Class.Coding != null)
                        {
                            int codingCount = 0;
                            foreach (Coding coding in composition.Class.Coding)
                            {
                                codingCount++;
                                coding.System.ShouldBe("http://snomed.info/sct");
                                coding.Code.ShouldBe("700232004");
                                coding.Display.ShouldBe("general medical service (qualifier value)");
                            }
                            codingCount.ShouldBeLessThanOrEqualTo(1);
                        }

                        if (composition.Class.Text != null)
                        {
                            composition.Class.Text.ShouldBe("general medical service (qualifier value)");
                        }
                    }
                }
            }
        }

        [Then(@"if composition contains the patient resource maritalStatus fields matching the specification")]
        public void ThenIfCompositionContainsThePatientResourceMaritalStatusFieldsMatchingTheSpecification()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    if (patient.MaritalStatus == null || patient.MaritalStatus.Coding == null)
                    {
                        Assert.Pass();
                    }
                    else
                    {
                        shouldBeSingleCodingWhichIsInValuest(GlobalContext.FhirMaritalStatusValueSet, patient.MaritalStatus.Coding);
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
                    foreach (Patient.ContactComponent contact in patient.Contact) {
                        // Contact Relationship Checks
                        foreach (CodeableConcept relationship in contact.Relationship) {
                            shouldBeSingleCodingWhichIsInValuest(GlobalContext.FhirRelationshipValueSet, relationship.Coding);
                        }

                        // Contact Name Checks
                        // Contact Telecom Checks
                        // Contact Address Checks
                        // Contact Gender Checks
                        // No mandatory fields and value sets are standard so will be validated by parse of response to fhir resource

                        // Contact Organization Checks
                        if (contact.Organization != null && contact.Organization.Reference != null)
                        {
                            var referenceExistsInBundle = false;
                            foreach (EntryComponent entryForOrganization in ((Bundle)FhirContext.FhirResponseResource).Entry)
                            {
                                if (entry.Resource.ResourceType.Equals(ResourceType.Organization) && entry.FullUrl.Equals(contact.Organization.Reference))
                                {
                                    referenceExistsInBundle = true;
                                    break;
                                }
                            }
                            referenceExistsInBundle.ShouldBeTrue();
                        }
                    }
                }
            }
        }
        
        [Then(@"if composition contains the patient resource communication the mandatory fields should matching the specification")]
        public void ThenIfCompositionContainsThePatientResourceCommunicationTheMandatoryFieldsShouldMatchingTheSpecification()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    if (patient.Communication == null)
                    {
                        Assert.Pass();
                    }
                    else
                    {
                        foreach (Patient.CommunicationComponent communicaiton in patient.Communication)
                        {
                            shouldBeSingleCodingWhichIsInValuest(GlobalContext.FhirHumanLanguageValueSet, communicaiton.Language.Coding);
                        }
                    }
                }
            }
        }

        [Then(@"if Patient careProvider is included in the response the reference should reference a Practitioner within the bundle")]
        public void ThenIfPatientCareProviderIsIncludedInTheResponseTheReferenceShouldReferenceAPractitionerWithinTheBundle()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    if (patient.CareProvider != null) {
                        var count = 0;
                        foreach (ResourceReference careProvider in patient.CareProvider)
                        {
                            count++;
                            responseBundleContainsReferenceOfType(careProvider.Reference, ResourceType.Practitioner);
                        }
                        count.ShouldBeLessThanOrEqualTo(1);
                    }
                }
            }
        }

        [Then(@"if Patient managingOrganization is included in the response the reference should reference an Organization within the bundle")]
        public void ThenIfPatientManagingOrganizationIsIncludedInTheResponseTheReferenceShouldReferenceAnOrganizationWithinTheBundle()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    if (patient.ManagingOrganization != null)
                    {
                        responseBundleContainsReferenceOfType(patient.ManagingOrganization.Reference, ResourceType.Organization);
                    }
                }
            }
        }

        [Then(@"patient resource should not contain the fhir fields photo animal or link")]
        public void ThenPatientResourceShouldNotContainTheFhirFieldsPhotoAnimalOrLink()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    (patient.Photo == null || patient.Photo.Count == 0).ShouldBeTrue(); // C# API creates an empty list if no element is present
                    patient.Animal.ShouldBeNull();
                    (patient.Link == null || patient.Link.Count == 0).ShouldBeTrue();
                }
            }
        }

        [Then(@"if the response bundle contains a ""([^""]*)"" resource")]
        public void ThenIfTheResponseBundleContainsAResource(string resourceType)
        {
            var resourceFound = false;
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.ToString().Equals(resourceType))
                {
                    resourceFound = true;
                    break;
                }
            }
            if (resourceFound == false)
            {
                // If no resource is found then the test scenario passes
                Assert.Pass();
            }
        }

        [Then(@"practitioner resources should contain a single name element")]
        public void ThenPractitionerResourcesShouldContainASingleNameElement()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    Practitioner practitioner = (Practitioner)entry.Resource;
                    practitioner.Name.ShouldNotBeNull();
                }
            }
        }

      

        [Then(@"practitioner family name should equal ""(.*)")]
        public void FamilyNameShouldEqualVariable(String familyName)
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    Practitioner practitioner = (Practitioner)entry.Resource;
                    String name = practitioner.Name.Family.ToString();
                    if (name != familyName)
                    {
                        Assert.Fail();
                    }
                    else {
                        Assert.Pass();
                    }
                }
            }
        }


        [Then(@"practitioner should only have one family name")]
        public void PractitionerShouldNotHaveMoreThenOneFamilyName()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    Practitioner practitioner = (Practitioner)entry.Resource;
                    var familyNameCount = 0;


                    foreach (String name in practitioner.Name.Family)
                    {
                      familyNameCount++;
                    }

                    familyNameCount.ShouldBeLessThanOrEqualTo(1);
                



                }

            }

        }

        [Then(@"practitioner resources should not contain the disallowed elements")]
        public void ThenPractitionerResourcesShouldNotContainTheDisallowedElements()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    Practitioner practitioner = (Practitioner)entry.Resource;
                    (practitioner.Photo == null || practitioner.Photo.Count == 0).ShouldBeTrue(); // C# API creates an empty list if no element is present
                    (practitioner.Qualification == null || practitioner.Qualification.Count == 0).ShouldBeTrue();
                }
            }
        }

        [Then(@"practitioner resources must only contain one user id and one profile id")]
        public void ThenPractitionerResourcesMustOnlyContainOneUserIdAndOneProfileId()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    Practitioner practitioner = (Practitioner)entry.Resource;

                    var sdsUserIdCount = 0;
                    var sdsRoleProfileIdCount = 0;

                    foreach (Identifier identifier in practitioner.Identifier) {
                        if (identifier.System.Equals("http://fhir.nhs.net/Id/sds-user-id")) {
                            sdsUserIdCount++;
                            identifier.Value.ShouldNotBeNull();
                        } else if (identifier.System.Equals("http://fhir.nhs.net/Id/sds-role-profile-id"))
                        {
                            sdsRoleProfileIdCount++;
                            identifier.Value.ShouldNotBeNull();
                        }
                    }
                    sdsUserIdCount.ShouldBeLessThanOrEqualTo(1);
                    sdsRoleProfileIdCount.ShouldBeLessThanOrEqualTo(1);
                }
            }
        }



        [Then(@"practitioner contains SDS identifier ""(.*)")]
        public void PractitionIdentifierEqualToPassedResource(String id)
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    Practitioner practitioner = (Practitioner)entry.Resource;

                    foreach (Identifier identifier in practitioner.Identifier)
                    {
                                      
                        if (identifier.System.Equals(id))
                        {
                                                       Assert.Pass();
                        }
                        else if (identifier.System.Equals("http://fhir.nhs.net/Id/sds-role-profile-id"))
                        {

                            Assert.Pass();
                        }
                    }
                    Assert.Fail();
                }
            }
        }

        [Then(@"if practitionerRole has managingOrganization element then reference must exist")]
        public void ThenIfPractitionerRoleHasMangingOrganizationElementThenReferenceMustExist()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    Practitioner practitioner = (Practitioner)entry.Resource;
                    foreach (Practitioner.PractitionerRoleComponent practitionerRole in practitioner.PractitionerRole)
                    {

                        if (practitionerRole.ManagingOrganization != null)
                        {
                           
                            practitionerRole.ManagingOrganization.Reference.ShouldNotBeNull();

                        }
                    }
                }
            }
        }

        [Then(@"if practitionerRole has role element which contains a coding then the system, code and display must exist")]
        public void ThenIfPractitionerRoleHasRoleElementWhichContainsACodingThenTheSystemCodeAndDisplayMustExist()
            {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    Practitioner practitioner = (Practitioner)entry.Resource;
                    foreach (Practitioner.PractitionerRoleComponent practitionerRole in practitioner.PractitionerRole) {
                        if (practitionerRole.Role != null && practitionerRole.Role.Coding != null) {
                            var codingCount = 0;
                            foreach (Coding coding in practitionerRole.Role.Coding) {
                                codingCount++;
                                coding.System.ShouldBe("http://fhir.nhs.net/ValueSet/human-language-11");
                                coding.Code.ShouldNotBeNull();
                                coding.Display.ShouldNotBeNull();
                            }
                            codingCount.ShouldBeLessThanOrEqualTo(1);
                        }
                    }
                }
            }
        }

        [Then(@"If the practitioner has communicaiton elemenets containing a coding then there must be a system, code and display element")]
        public void ThenIfThePractitionerHasCommunicationElementsContainingACodingThenThereMustBeASystemCodeAndDisplayElement()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    Practitioner practitioner = (Practitioner)entry.Resource;
                    //If the practitioner has a communicaiton elemenets containing a coding then there must be a system, code and display element. There must only be one coding per communication element.
                    foreach (CodeableConcept codeableConcept in practitioner.Communication) {
                        shouldBeSingleCodingWhichIsInValuest(GlobalContext.FhirHumanLanguageValueSet, codeableConcept.Coding);
                    }
                }
            }
        }

        [Then(@"if practitioner contains a managingOrganization the reference relates to an Organization within the response bundle")]
        public void ThenPractitionerContainsAManagingOrganizationTheReferenceRelatesToAnOrganizationWithinTheResponseBundle()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    Practitioner practitioner = (Practitioner)entry.Resource;
                    foreach (Practitioner.PractitionerRoleComponent practitionerRole in practitioner.PractitionerRole)
                    {
                        if (practitionerRole.ManagingOrganization != null) {
                            responseBundleContainsReferenceOfType(practitionerRole.ManagingOrganization.Reference, ResourceType.Organization);
                        }
                    }
                }
            }
        }

        [Then(@"Organization resources identifiers must comply with specification identifier restricitions")]
        public void ThenOrganizationResourceIdentifiersMustComplyWithSpecificationIdentifierRestrictions()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Organization))
                {
                    Organization organization  = (Organization)entry.Resource;

                    var odsOrganizationCodeCount = 0;

                    foreach (Identifier identifier in organization.Identifier)
                    {
                        if (identifier.System.Equals("http://fhir.nhs.net/Id/ods-organization-code"))
                        {
                            odsOrganizationCodeCount++;
                            identifier.Value.ShouldNotBeNull();
                        }
                        else if (identifier.System.Equals("http://fhir.nhs.net/Id/ods-site-code"))
                        {
                            identifier.Value.ShouldNotBeNull();
                        }
                    }
                    odsOrganizationCodeCount.ShouldBeLessThanOrEqualTo(1);
                }
            }
        }

        [Then(@"if Organization includes type coding the elements are mandatory")]
        public void ThenIfOrganizationIncludesTypeCodingTheElementsAreMandatory()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Organization))
                {
                    Organization organization = (Organization)entry.Resource;
                    if (organization.Type != null && organization.Type.Coding != null) {
                        var codingCount = 0;
                        foreach (Coding coding in organization.Type.Coding) {
                            codingCount++;
                            coding.System.ShouldNotBeNull();
                            coding.Code.ShouldNotBeNull();
                            coding.Display.ShouldNotBeNull();
                        }
                        codingCount.ShouldBeLessThanOrEqualTo(1);
                    }
                }
            }
        }

        [Then(@"if Organization includes partOf it should reference a resource in the response bundle")]
        public void ThenIfOrganizationIncludesPartOfItShouldReferenceAResourceInTheResponseBundle()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Organization))
                {
                    Organization organization = (Organization)entry.Resource;
                    if (organization.PartOf != null && organization.PartOf.Reference != null)
                    {
                        responseBundleContainsReferenceOfType(organization.PartOf.Reference, ResourceType.Organization);
                    }
                }
            }
        }

        [Then(@"the Device resource should conform to cardinality set out in specificaiton")]
        public void ThenTheDeviceResourceShouldConformToCardinalitySetOutInSpecification()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Device))
                {
                    Device device = (Device)entry.Resource;

                    device.Status.ShouldBeNull();
                    device.ManufactureDate.ShouldBeNull();
                    device.Expiry.ShouldBeNull();
                    device.Udi.ShouldBeNull();
                    device.LotNumber.ShouldBeNull();
                    device.Patient.ShouldBeNull();
                    (device.Contact == null || device.Contact.Count == 0).ShouldBeTrue();
                    device.Url.ShouldBeNull();

                    var identifierCount = 0;
                    foreach (Identifier identifier in device.Identifier) {
                        identifierCount++;
                        identifier.Value.ShouldNotBeNull();
                    }
                    identifierCount.ShouldBeLessThanOrEqualTo(1);

                    device.Note.Count.ShouldBeLessThanOrEqualTo(1);
                }
            }
        }

        [Then(@"the Device resource type should match the fixed values from the specfication")]
        public void ThenTheDeviceResourceTypeShouldMatchTheFixedValuesFromTheSpecification()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Device))
                {
                    Device device = (Device)entry.Resource;
                    var codingCount = 0;
                    foreach (Coding coding in device.Type.Coding) {
                        codingCount++;
                        coding.System.ShouldBe("http://snomed.info/sct");
                        coding.Code.ShouldBe("462240000");
                        coding.Display.ShouldBe("Patient health record information system (physical object)");
                    }
                    codingCount.ShouldBeLessThanOrEqualTo(1);
                }
            }
        }

        [Then(@"the HTML in the response matches the Regex check ""([^""]*)""")]
        public void ThenTheHTMLInTheResponseMatchesTheRegexCheck(string regexPattern)
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
                {
                    Composition composition = (Composition)entry.Resource;
                    var sectionCount = 0;
                    foreach (Composition.SectionComponent section in composition.Section) {
                        sectionCount++;
                        var HTML = section.Text.Div;
                        HTML.ShouldMatch(regexPattern);
                    }
                    sectionCount.ShouldBe(1);
                }
            }
        }

        [Then(@"the composition resource in the bundle should contain meta data profile")]
        public void ThenTheCompositionResourceInTheBundleShouldContainMetaDataProfile()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
                {
                    Composition composition = (Composition)entry.Resource;
                    composition.Meta.ShouldNotBeNull();
                    foreach (string profile in composition.Meta.Profile) {
                        profile.ShouldBe("http://fhir.nhs.net/StructureDefinition/gpconnect-carerecord-composition-1");
                    }
                }
            }
        }

        [Then(@"the patient resource in the bundle should contain meta data profile and version id")]
        public void ThenThePatientResourceInTheBundleShouldContainMetaDataProfileAndVersionId()
        {
            checkForValidMetaDataInResource(ResourceType.Patient, "http://fhir.nhs.net/StructureDefinition/gpconnect-patient-1");
        }

        [Then(@"if the response bundle contains an organization resource it should contain meta data profile and version id")]
        public void ThenIfTheResponseBundleContainsAnOrganizationResourceItShouldContainMetaDataProfileAndVersionId()
        {
            checkForValidMetaDataInResource(ResourceType.Organization, "http://fhir.nhs.net/StructureDefinition/gpconnect-organization-1");
        }

        [Then(@"if the response bundle contains a practitioner resource it should contain meta data profile and version id")]
        public void ThenIfTheResponseBundleContainsAPractitionerResourceItShouldContainMetaDataProfileAndVersionId()
        {
            checkForValidMetaDataInResource(ResourceType.Practitioner, "http://fhir.nhs.net/StructureDefinition/gpconnect-practitioner-1");
        }

        [Then(@"if the response bundle contains a device resource it should contain meta data profile and version id")]
        public void ThenIfTheResponseBundleContainsADeviceResourceItShouldContainMetaDataProfileAndVersionId()
        {
            checkForValidMetaDataInResource(ResourceType.Device, "http://fhir.nhs.net/StructureDefinition/gpconnect-device-1");
        }

        [Then(@"if the response bundle contains a location resource it should contain meta data profile and version id")]
        public void ThenIfTheResponseBundleContainsALocationResourceItShouldContainMetaDataProfileAndVersionId()
        {
            checkForValidMetaDataInResource(ResourceType.Location, "http://fhir.nhs.net/StructureDefinition/gpconnect-location-1");
        }

        public void checkForValidMetaDataInResource(ResourceType resourceType, string profileId) {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(resourceType))
                {
                    var resource = entry.Resource;
                    resource.Meta.ShouldNotBeNull();
                    int metaProfileCount = 0;
                    foreach (string profile in resource.Meta.Profile)
                    {
                        metaProfileCount++;
                        profile.ShouldBe(profileId);
                    }
                    metaProfileCount.ShouldBe(1);
                    resource.Meta.VersionId.ShouldNotBeNull();
                }
            }
        }

        public void shouldBeSingleCodingWhichIsInValuest(ValueSet valueSet, List<Coding> codingList) {
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
            var pass = false;
            foreach (ValueSet.ConceptDefinitionComponent valueSetConcept in valueset.CodeSystem.Concept)
            {
                if (valueSetConcept.Code.Equals(coding.Code) && valueSetConcept.Display.Equals(coding.Display))
                {
                    pass = true;
                }
            }
            pass.ShouldBeTrue();
        }

        public void responseBundleContainsReferenceOfType(string reference, ResourceType resourceType) {
            var pass = false;
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (reference.Equals(entry.FullUrl) && entry.Resource.ResourceType == resourceType){
                    pass = true;
                }
            }
            pass.ShouldBeTrue();
        }

    }
}
