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
    }
}
