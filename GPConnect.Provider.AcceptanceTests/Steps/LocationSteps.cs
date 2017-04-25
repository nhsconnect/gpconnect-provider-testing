using System;
using GPConnect.Provider.AcceptanceTests.Context;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using RestSharp;
using Shouldly;
using TechTalk.SpecFlow;
using static Hl7.Fhir.Model.Bundle;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    class LocationSteps : TechTalk.SpecFlow.Steps
    {

        private readonly FhirContext FhirContext;
        private readonly HttpContext HttpContext;
        private readonly HttpSteps HttpSteps;
        private readonly AccessRecordSteps AccessRecordSteps;

        public LocationSteps(FhirContext fhirContext, HttpContext httpContext, HttpSteps httpSteps, AccessRecordSteps accessRecordSteps)
        {
            FhirContext = fhirContext;
            HttpContext = httpContext;
            HttpSteps = httpSteps;
            AccessRecordSteps = accessRecordSteps;
        }

        [Then(@"the response bundle Location entries should contain a maximum of one ODS Site Code and one other identifier")]
        public void ThenTheResponseBundleLocationEntriesShouldContainAMaximumOfOneODSSiteCodeAndOneOtherIdentifier()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Location))
                {
                    Location location = (Location)entry.Resource;
                    int odsSiteCodeIdentifierCount = 0;
                    foreach (var identifier in location.Identifier) {
                        if (string.Equals(identifier.System, "http://fhir.nhs.net/Id/ods-site-code")) {
                            odsSiteCodeIdentifierCount++;
                        }
                    }
                    odsSiteCodeIdentifierCount.ShouldBeLessThanOrEqualTo(1, "There should be a maximum of one ODS Site Code within the Location resource.");
                    location.Identifier.Count.ShouldBeLessThanOrEqualTo(2, "There should be no more than one ODS Site Code and One other identifier, there is more than 2 identifiers in the Location resource.");
                }
            }
        }

        [Then(@"the response bundle Location entries should contain a name element")]
        public void ThenTheResponseBundleLocationEntriesShouldContainANameElement()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Location))
                {
                    Location location = (Location)entry.Resource;
                    location.Name.ShouldNotBeNullOrEmpty();
                }
            }
        }

        [Then(@"the response bundle location entries should contain system code and display if the Type coding is included in the resource")]
        public void ThenTheResponseBundleLocationEntriesShouldContainSystemCodeAndDisplayIfTheTypeCodingIsIncludedInTheResource()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Location))
                {
                    Location location = (Location)entry.Resource;
                    if (location.Type != null && location.Type.Coding != null)
                    {
                        location.Type.Coding.Count.ShouldBeLessThanOrEqualTo(1);
                        foreach (var coding in location.Type.Coding) {
                            // Need to pull in valueset from URL and validate against that
                            coding.System.ShouldBe("http://hl7.org/fhir/ValueSet/v3-ServiceDeliveryLocationRoleType");
                            coding.Code.ShouldNotBeNullOrEmpty();
                            coding.Display.ShouldNotBeNullOrEmpty();
                        }
                        
                    }
                }
            }
        }

        [Then(@"the response bundle location entries should contain valid  system code and display if the PhysicalType coding is included in the resource")]
        public void ThenTheResponseBundleLocationEntriesShouldContainValidSystemCodeAndDisplayIfThePhysicalTypeCodingIsIncludedInTheResource()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Location))
                {
                    Location location = (Location)entry.Resource;
                    if (location.PhysicalType != null && location.PhysicalType.Coding != null)
                    {
                        location.PhysicalType.Coding.Count.ShouldBeLessThanOrEqualTo(1);
                        foreach (var coding in location.PhysicalType.Coding)
                        {
                            var validSystems = new String[] { "http://snomed.info/sct", "http://read.info/readv2", "http://read.info/ctv3" };
                            coding.System.ShouldBeOneOf(validSystems);
                            coding.Code.ShouldNotBeNullOrEmpty();
                            coding.Display.ShouldNotBeNullOrEmpty();
                        }
                    }
                }
            }
        }
        
        [Then(@"if the response bundle location entries contain partOf element the reference should reference a resource in the response bundle")]
        public void ThenIfTheResponseBundleLocationEntriesContainPartOfElementTheReferenceShouldReferenceAResourceInTheResponseBundle()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Location))
                {
                    Location location = (Location)entry.Resource;
                    if (location.PartOf != null){
                        location.PartOf.Reference.ShouldNotBeNullOrEmpty();
                        AccessRecordSteps.responseBundleContainsReferenceOfType(location.PartOf.Reference, ResourceType.Location);
                    }
                }
            }
        }

        [Then(@"if the response bundle location entries contain managingOrganization element the reference should reference a resource in the response bundle")]
        public void ThenIfTheResponseBundleLocationEntriesContainManagingOrganizationElementTheReferenceShouldReferenceAResourceInTheResponseBundle()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Location))
                {
                    Location location = (Location)entry.Resource;
                    if (location.ManagingOrganization != null)
                    {
                        location.ManagingOrganization.Reference.ShouldNotBeNullOrEmpty();
                        AccessRecordSteps.responseBundleContainsReferenceOfType(location.ManagingOrganization.Reference, ResourceType.Organization);
                    }
                }
            }
        }
    }
}
