using GPConnect.Provider.AcceptanceTests.Context;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json;
using Shouldly;
using System;
using TechTalk.SpecFlow;

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
            FhirJsonParser fhirJsonParser = new FhirJsonParser();
            var fhirBundle = fhirJsonParser.Parse<Bundle>(JsonConvert.SerializeObject(HttpContext.ResponseJSON));
            fhirBundle.ResourceType.ShouldBe(ResourceType.Bundle);
            HttpContext.ResponseFhirBundle = fhirBundle; // Store the bundle for use by other validation
        }
    }
}
