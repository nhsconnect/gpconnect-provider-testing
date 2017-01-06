using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Shouldly;
using System;
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
                FhirContext.FhirPatients.Add(row["Id"], row["NHSNumber"]);
        }

        [Given(@"I am requesting the record for config patient ""([^""]*)""")]
        public void GivenIAmRequestingTheRecordForConfigPatient(string patient)
        {
            Given($@"I am requesting the record for patient with NHS Number ""{FhirContext.FhirPatients[patient]}""");
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
                        int codingCount = 0;
                        foreach (Coding coding in patient.MaritalStatus.Coding)
                        {
                            codingCount++;
                            coding.System.ShouldBe(GlobalContext.FhirMaritalStatusValueSet.CodeSystem.System);

                            // Loop through valid codes to find if the one in the resource is valid
                            var pass = false;
                            foreach (ValueSet.ConceptDefinitionComponent valueSetConcept in GlobalContext.FhirMaritalStatusValueSet.CodeSystem.Concept) {
                                if (valueSetConcept.Code.Equals(coding.Code) && valueSetConcept.Display.Equals(coding.Display)) {
                                    pass = true;
                                    break;
                                }
                            }
                            pass.ShouldBeTrue();
                        }
                        codingCount.ShouldBeLessThanOrEqualTo(1);
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
                            var codingCount = 0;
                            foreach (Coding coding in relationship.Coding)
                            {
                                codingCount++;
                                coding.System.ShouldBe(GlobalContext.FhirRelationshipValueSet.CodeSystem.System);

                                // Loop through valid codes to find if the one in the resource is valid
                                var pass = false;
                                foreach (ValueSet.ConceptDefinitionComponent valueSetConcept in GlobalContext.FhirMaritalStatusValueSet.CodeSystem.Concept)
                                {
                                    if (valueSetConcept.Code.Equals(coding.Code) && valueSetConcept.Display.Equals(coding.Display))
                                    {
                                        pass = true;
                                        break;
                                    }
                                }
                                pass.ShouldBeTrue();
                            }
                            codingCount.ShouldBeLessThanOrEqualTo(1);
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

    }
}
