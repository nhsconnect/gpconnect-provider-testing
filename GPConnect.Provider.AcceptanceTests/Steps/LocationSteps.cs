namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Context;
    using Enum;
    using Hl7.Fhir.Model;
    using Shouldly;
    using TechTalk.SpecFlow;
    using static Hl7.Fhir.Model.Bundle;

    [Binding]
    class LocationSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private readonly HttpContext HttpContext;
        private readonly HttpSteps _httpSteps;
        private readonly BundleSteps _bundleSteps;

        public LocationSteps(FhirContext fhirContext, HttpContext httpContext, HttpSteps httpSteps, AccessRecordSteps accessRecordSteps, BundleSteps bundleSteps)
        {
            FhirContext = fhirContext;
            HttpContext = httpContext;
            _httpSteps = httpSteps;
            _bundleSteps = bundleSteps;
        }

        [Given(@"I get location ""(.*)"" id and save it as ""(.*)""")]
        public void GivenIGetLocationIdAndSaveItAs(string orgName, string savedOrg)
        {
            string system = "http://fhir.nhs.net/Id/ods-site-code";
            string value = GlobalContext.OdsCodeMap[orgName];

            Given("I am using the default server");
            Given($@"I am performing the ""urn:nhs:names:services:gpconnect:fhir:rest:search:location"" interaction");
            Given($@"I add the location identifier parameter with system ""{system}"" and value ""{orgName}""");
            When($@"I make a GET request to ""/Location""");
            Then($@"the response status code should indicate success");
            Then($@"the response body should be FHIR JSON");
            Then($@"the response should be a Bundle resource of type ""searchset""");

            Location location = new Location();
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Location))
                {
                    location = (Location)entry.Resource;

                }
            }
            HttpContext.StoredFhirResources.Add(savedOrg, location);
        }

        [When(@"I get location ""(.*)"" and use the id to make a get request to the url ""(.*)""")]
        public void WhenIGetLocationAndUseTheIdToMakeAGetRequestToTheUrl(string locationName, string URL)
        {
            Location locationValue = (Location)HttpContext.StoredFhirResources[locationName];
            string fullUrl = locationValue.Id == null
                ? $"/{URL}/"
                : $"/{URL}/{locationValue.Id.ToString()}";

            When($@"I make a GET request to ""{fullUrl}""");
        }

        [When(@"I make a GET request for a location with id ""(.*)""")]
        public void WhenIMakeAGetRequestForALocationWithId(string id)
        {
            string URL = "/Location/" + id;
            When($@"I make a GET request to ""{URL}""");
        }

        [When(@"I make a GET request for location ""([^""]*)"" with If-None-Match header")]
        public void IMakeAGETRequestForLocationWithIf_None_MatchHeader(string location)
        {
            var locationResource = HttpContext.StoredFhirResources[location];
            var etag = "W/\"" + locationResource.Meta.VersionId + "\"";

            HttpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kIfNoneMatch, etag);
            WhenIMakeAGetRequestForALocationWithId(locationResource.Id);
        }

        [When(@"I perform a location vread for location ""([^""]*)""")]
        public void IPerformALocationVReadForLocation(string location)
        {
            var locationResource = HttpContext.StoredFhirResources[location];

            var versionId = HttpContext.StoredFhirResources[location].Meta.VersionId;

            When($@"I make a GET request to ""/Location/{locationResource.Id}/_history/{versionId}""");
        }

        [When(@"I perform a location vread for location ""([^""]*)"" with invalid ETag")]
        public void IPerformALocationVReadForLocationWithInvalidETag(string location)
        {
            var locationResource = HttpContext.StoredFhirResources[location];
            When($@"I make a GET request to ""/Location/{locationResource.Id}/_history/badETag""");
        }

        [Given(@"I add the location identifier parameter with system ""(.*)"" and value ""(.*)""")]
        public void GivenIAddTheLocationIdentifierParameterWithTheSystemAndValue(string systemParameter, string valueParameter)
        {
            Given($@"I add the parameter ""identifier"" with the value ""{systemParameter + '|' + GlobalContext.OdsCodeMap[valueParameter]}""");
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
                    foreach (var identifier in location.Identifier)
                    {
                        if (string.Equals(identifier.System, "http://fhir.nhs.net/Id/ods-site-code"))
                        {
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
                        foreach (var coding in location.Type.Coding)
                        {
                            // Need to pull in valueset from URL and validate against that
                            coding.System.ShouldBe("http://hl7.org/fhir/ValueSet/v3-ServiceDeliveryLocationRoleType");
                            coding.Code.ShouldNotBeNullOrEmpty();
                            coding.Display.ShouldNotBeNullOrEmpty();
                        }
                    }
                }
            }
        }

        [Then(@"the response bundle location entries should contain valid system code and display if the PhysicalType coding is included in the resource")]
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
                    if (location.PartOf != null)
                    {
                        location.PartOf.Reference.ShouldNotBeNullOrEmpty();
                        _bundleSteps.ResponseBundleContainsReferenceOfType(location.PartOf.Reference, ResourceType.Location);
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
                        _bundleSteps.ResponseBundleContainsReferenceOfType(location.ManagingOrganization.Reference, ResourceType.Organization);
                    }
                }
            }
        }

        [Then(@"the location resource should contain meta data profile and version id")]
        public void ThenTheLocationResourceShouldContainMetaDataProfileAndVersionId()
        {
            Location location = (Location)FhirContext.FhirResponseResource;
            location.Meta.VersionId.ShouldNotBeNull("The meta data element should contain a version id element");
            location.Meta.Profile.ShouldNotBeNull("The meta data element should contain a profile element");

            var count = 0;

            foreach (string profile in location.Meta.Profile)
            {
                profile.ShouldBe("http://fhir.nhs.net/StructureDefinition/gpconnect-location-1", "The meta data profile element is not valid");
                count++;
            }
            count.ShouldBe(1, "There should only be one meta data profile within the meta data element.");
        }
        
        [Then(@"the response Location entry should contain a name element")]
        public void ThenTheResponseLocationEntryShouldContainANameElement()
        {
            Location location = (Location)FhirContext.FhirResponseResource;
            location.Name.ShouldNotBeNullOrEmpty();
        }
        
        [Then(@"the location response should contain valid system code and display if the PhysicalType coding is included in the resource")]
        public void ThenTheLocationResponseShouldContainValidSystemCodeAndDisplayIfThePhysicalTypecodingIsIncludedInTheResource()
        {
            Location location = (Location)FhirContext.FhirResponseResource;
            if (location.PhysicalType != null && location.PhysicalType.Coding != null)
            {
                var validSystems = new List<string>() { "http://snomed.info/sct", "http://read.info/readv2", "http://read.info/ctv3" };
                
                foreach (var coding in location.PhysicalType.Coding)
                {
                    validSystems.Remove(coding.System).ShouldBeTrue($"Invalid or duplicate system: {coding.System}");
                    coding.Code.ShouldNotBeNullOrEmpty();
                    coding.Display.ShouldNotBeNullOrEmpty();
                }
            }
        }

        [Then(@"if the location response contains a managing organization it contains a valid reference")]
        public void ThenIfTheLocationResponseContainsAManagingOrganizationItContainsAValidReference()
        {
            Location location = (Location)FhirContext.FhirResponseResource;
            if (location.ManagingOrganization != null)
            {
                location.ManagingOrganization.Reference.ShouldNotBeNullOrEmpty();
                _bundleSteps.ResponseBundleContainsReferenceOfType(location.ManagingOrganization.Reference, ResourceType.Organization);
            }
        }

        [Then(@"if the location response contains a partOf element its reference is valid")]
        public void ThenIfTheLocationResponseContainsAPartOfElementItsReferenceIsValid()
        {
            Location location = (Location)FhirContext.FhirResponseResource;
            if (location.PartOf != null)
            {
                location.PartOf.Reference.ShouldNotBeNullOrEmpty();
                _bundleSteps.ResponseBundleContainsReferenceOfType(location.PartOf.Reference, ResourceType.Location);
            }
        }

        [Then(@"if the location response contains a type element it is valid")]
        public void ThenIfTheLocationResponseContainsATypeElementItIsValid()
        {
            Location location = (Location)FhirContext.FhirResponseResource;

            if (location.Type != null && location.Type.Coding != null)
            {
                location.Type.Coding.Count.ShouldBeLessThanOrEqualTo(1);
                foreach (var coding in location.Type.Coding)
                {
                    coding.System.ShouldBe("http://hl7.org/fhir/ValueSet/v3-ServiceDeliveryLocationRoleType");
                    coding.Code.ShouldNotBeNullOrEmpty();
                    coding.Display.ShouldNotBeNullOrEmpty();
                }
            }
        }

        [Then(@"if the location response contains any telecom elements they are valid")]
        public void ThenIfTheLocationResponseContainsAnyTelecomElementsTheyAreValid()
        {
            Location location = (Location)FhirContext.FhirResponseResource;

            if (location.Telecom != null)
            {
                foreach (var contactPoint in location.Telecom)
                {
                    if (contactPoint.Period != null)
                    {
                        contactPoint.Period.Start.ShouldNotBeNullOrWhiteSpace("Telecom Period Start field is mandatory");
                        // System and use fields are already checked by the FHIR library
                    }
                }
            }
        }

        [Then(@"if the location response resource contains an identifier it is valid")]
        public void ThenIfTheLocationResponseResourceContainsAnIdentifierItIsValid()
        {
            Location location = (Location)FhirContext.FhirResponseResource;
            location.Identifier.Count(identifier => identifier.System.Equals("http://fhir.nhs.net/Id/ods-site-code")).ShouldBeLessThanOrEqualTo(1,"There should be a maximum of one ods site code in the location resource");
            location.Identifier.Count(identifier => !identifier.System.Equals("http://fhir.nhs.net/Id/ods-site-code")).ShouldBeLessThanOrEqualTo(1, "There should be a maximum of one other identifier that can be included along with the ods site code in the location resource");
        }

        [Given(@"I add a Location Identifier parameter with System ""([^""]*)"" and Value ""([^""]*)""")]
        public void AddALocationIdentifierParameterWithSystemAndValue(string system, string value)
        {
            HttpContext.RequestParameters.AddParameter("identifier", system + '|' + GlobalContext.OdsCodeMap[value]);
        }

        [Given(@"I add a Location Identifier parameter with default System and Value ""([^""]*)""")]
        public void AddALocationIdentifierParameterWithDefaultSystemAndValue(string value)
        {
            AddALocationIdentifierParameterWithSystemAndValue("http://fhir.nhs.net/Id/ods-site-code", value);

        }

        [Given(@"I get the Location for Location Value ""([^""]*)""")]
        public void GetThePatientForPatientValue(string value)
        {
            _httpSteps.ConfigureRequest(GpConnectInteraction.LocationSearch);

            AddALocationIdentifierParameterWithDefaultSystemAndValue(value);

            _httpSteps.MakeRequest(GpConnectInteraction.LocationSearch);
        }

        [Given(@"I store the Location Id")]
        public void StoreTheLocationId()
        {
            var location = FhirContext.Locations.FirstOrDefault();

            if (location != null)
                HttpContext.GetRequestId = location.Id;
        }

        [Then(@"the Location Id should match the GET request Id")]
        public void TheLocationIdShouldMarchTheGetRequestId()
        {
            var location = FhirContext.Locations.FirstOrDefault();

            location.ShouldNotBeNull();
            location.Id.ShouldBe(HttpContext.GetRequestId);
        }

        [Then(@"the returned Location resource shall contain the business identifier for Location ""([^""]*)""")]
        public void ThenTheReturnedLocationResourceShallContainTheBusinessIdentifierForLocation(string locationName)
        {
            var location = (Location)FhirContext.FhirResponseResource;
            location.Identifier.ShouldNotBeNull("The location should contain a site identifier as the business identifier was used to find the location for this test.");
            location.Identifier.Find(identifier => identifier.System.Equals("http://fhir.nhs.net/Id/ods-site-code")).Value.ShouldBe(GlobalContext.OdsCodeMap[locationName], "Location business identifier does not match the expected business identifier.");
        }
        
        [Then(@"the response should be a Location resource")]
        public void theResponseShouldBeAnLocationResource()
        {
            FhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.Location);
        }
    }
}
