namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using Context;
    using Enum;
    using Hl7.Fhir.Model;
    using Repository;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    public class LocationSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private readonly HttpRequestConfigurationSteps _httpRequestConfigurationSteps;
        private List<Location> Locations => _httpContext.FhirResponse.Locations;
        private readonly IFhirResourceRepository _fhirResourceRepository;

        public LocationSteps(HttpContext httpContext, HttpSteps httpSteps, HttpRequestConfigurationSteps httpRequestConfigurationSteps, IFhirResourceRepository fhirResourceRepository) 
            : base(httpSteps)
        {
            _httpContext = httpContext;
            _httpRequestConfigurationSteps = httpRequestConfigurationSteps;
            _fhirResourceRepository = fhirResourceRepository;
        }

        [Given(@"I add a Location Identifier parameter with System ""([^""]*)"" and Value ""([^""]*)""")]
        public void AddALocationIdentifierParameterWithSystemAndValue(string system, string value)
        {
            string locationCode;

            GlobalContext.OdsCodeMap.TryGetValue(value, out locationCode);

            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("identifier", system + '|' + locationCode);
        }

        [Given(@"I add a Location Identifier parameter with default System and Value ""([^""]*)""")]
        public void AddALocationIdentifierParameterWithDefaultSystemAndValue(string value)
        {
            AddALocationIdentifierParameterWithSystemAndValue("http://fhir.nhs.net/Id/ods-site-code", value);
        }

        [Given(@"I add a Location ""([^""]*)"" parameter with default System and Value ""([^""]*)""")]
        public void AddALocationParameterWithDefaultSystemAndValue(string parameterName, string value)
        {
            string locationCode;

            GlobalContext.OdsCodeMap.TryGetValue(value, out locationCode);

            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter(parameterName, "http://fhir.nhs.net/Id/ods-site-code" + '|' + GlobalContext.OdsCodeMap[value]);
        }       

        [Given(@"I get the Location for Location Value ""([^""]*)""")]
        public void GetThePatientForPatientValue(string value)
        {
            _httpSteps.ConfigureRequest(GpConnectInteraction.LocationSearch);

            AddALocationIdentifierParameterWithDefaultSystemAndValue(value);

            _httpSteps.MakeRequest(GpConnectInteraction.LocationSearch);
        }

        [Given(@"I store the Location")]
        public void StoreTheLocation()
        {
            var location = Locations.FirstOrDefault();

            if (location != null)
            {
                _httpContext.HttpRequestConfiguration.GetRequestId = location.Id;
                _fhirResourceRepository.Location = location;
            }
        }

        [Given(@"I set the If-None-Match header to the stored Location Id")]
        public void SetTheIfNoneMatchHeaderToTheStoredLocationVerisionId()
        {
            var location = _fhirResourceRepository.Location;

            if (location != null)
                _httpRequestConfigurationSteps.GivenISetTheIfNoneMatchheaderHeaderTo("W/\"" + location.VersionId + "\"");
        }

        [Then(@"the Response Resource should be a Location")]
        public void TheResponseResourceShouldBeALocation()
        {
            _httpContext.FhirResponse.Resource.ResourceType.ShouldBe(ResourceType.Location);
        }

        [Then(@"the Location Name should be valid")]
        public void TheLocationNameShouldBeValid()
        {
            Locations.ForEach(location =>
            {
                location.Name.ShouldNotBeNull("Location resources must contain a Name element");
            });
        }

        [Then(@"the Location Type should be valid")]
        public void TheLocationTypeShouldBeValid()
        {
            Locations.ForEach(location =>
            {
                if (location.Type?.Coding != null)
                {
                    location.Type.Coding.Count.ShouldBeLessThanOrEqualTo(1, "There should be a maximum of one Location Type Coding within the Location Type.");
                    foreach (var coding in location.Type.Coding)
                    {
                        // Need to pull in valueset from URL and validate against that
                        coding.System.ShouldBe("http://hl7.org/fhir/ValueSet/v3-ServiceDeliveryLocationRoleType", "The Location Type Coding System is not valid.");
                        coding.Code.ShouldNotBeNullOrEmpty("The Location Type Coding should contain a Code.");
                        coding.Display.ShouldNotBeNullOrEmpty("The Location Type Coding should contain a Display.");
                    }
                }
            });
        }

        [Then(@"the Location Physical Type should be valid")]
        public void TheLocationPhysicalTypeShouldBeValid()
        {
            Locations.ForEach(location =>
            {
                if (location.PhysicalType?.Coding != null)
                {
                    location.PhysicalType.Coding.Count.ShouldBeLessThanOrEqualTo(1, "There should be a maximum of one Location Physical Type Coding within the Location Physical Type.");
                    foreach (var coding in location.PhysicalType.Coding)
                    {
                        coding.System.ShouldBeOneOf("http://snomed.info/sct", "http://read.info/readv2", "http://read.info/ctv3", "The Location Physical Type Coding System is not valid.");
                        coding.Code.ShouldNotBeNullOrEmpty("The Location Physical Type Coding should contain a Code.");
                        coding.Display.ShouldNotBeNullOrEmpty("The Location Physical Type Coding should contain a Display.");
                    }
                }
            });
        }

        [Then(@"the Location PartOf Location should be valid")]
        public void TheLocationPartOfLocationShouldBeValid()
        {
            Locations.ForEach(location =>
            {
                location.PartOf?.Reference.ShouldNotBeNullOrEmpty("The PartOf element within the location resource should contain a reference element.");
                location.PartOf?.Reference.ShouldStartWith("Location/", "The reference element within the PartOf element of the Location resource should contain a relative Location reference.");
            });
        }

        [Then(@"the Location Managing Organization should be valid")]
        public void TheLocationManagingOrganizationShouldBeValid()
        {
            Locations.ForEach(location =>
            {
                location.ManagingOrganization?.Reference.ShouldNotBeNullOrEmpty("If a managing organization element is included in the location resource it should have a reference element.");
                location.ManagingOrganization?.Reference.ShouldStartWith("Organization/", "The ManagingOrganization reference should be a relative url for an Organization.");
            });
        }

        [Then(@"the Location Metadata should be valid")]
        public void TheLocationMetadataShouldBeValid()
        {
            Locations.ForEach(location =>
            {
                CheckForValidMetaDataInResource(location, "http://fhir.nhs.net/StructureDefinition/gpconnect-location-1");
            });
        }

        [Then(@"the Location Telecom should be valid")]
        public void TheLocationTelecomShouldBeValid()
        {
            Locations.ForEach(location =>
            {
                location.Telecom?.ForEach(contactPoint => 
                {
                    contactPoint.Period?.Start.ShouldNotBeNullOrWhiteSpace("Telecom Period Start field is mandatory");
                });
            });
        }

        [Then(@"the Location Id should match the GET request Id")]
        public void TheLocationIdShouldMarchTheGetRequestId()
        {
            var location = Locations.FirstOrDefault();

            location.ShouldNotBeNull();
            location.Id.ShouldBe(_httpContext.HttpRequestConfiguration.GetRequestId);
        }

        [Then(@"the Location Identifier should be valid")]
        public void TheLocationIdentifierShouldBeValid()
        {
            TheLocationIdentifierShouldBeValidForValue(null);
        }

        [Then(@"the Location Identifier should be valid for Value ""([^""]*)""")]
        public void TheLocationIdentifierShouldBeValidForValue(string locationName)
        {
            Locations.ForEach(location =>
            {
                var siteCodeIdentifiers = location.Identifier
                    .Where(identifier => identifier.System.Equals("http://fhir.nhs.net/Id/ods-site-code"))
                    .ToList();

                siteCodeIdentifiers.Count.ShouldBeLessThanOrEqualTo(1, "There should be a maximum of one ods site code in the location resource");

                if (!string.IsNullOrEmpty(locationName))
                {
                     siteCodeIdentifiers.ForEach(siteCodeIdentifier =>
                    {
                        siteCodeIdentifier.Value.ShouldBe(GlobalContext.OdsCodeMap[locationName], "Location business identifier does not match the expected business identifier.");
                    });
                }

                var nonSiteCodeIdentifierCount = location.Identifier.Count - siteCodeIdentifiers.Count;

                nonSiteCodeIdentifierCount.ShouldBeLessThanOrEqualTo(1, "There should be a maximum of one other identifier that can be included along with the ods site code in the location resource");

            });
        }
    }
}
