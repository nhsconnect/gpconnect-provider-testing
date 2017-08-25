using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Extensions;

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
        private readonly HttpResponseSteps _httpResponseSteps;

        public LocationSteps(HttpContext httpContext, HttpSteps httpSteps, HttpRequestConfigurationSteps httpRequestConfigurationSteps, IFhirResourceRepository fhirResourceRepository, HttpResponseSteps httpResponseSteps) 
            : base(httpSteps)
        {
            _httpContext = httpContext;
            _httpRequestConfigurationSteps = httpRequestConfigurationSteps;
            _fhirResourceRepository = fhirResourceRepository;
            _httpResponseSteps = httpResponseSteps;
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
            AddALocationIdentifierParameterWithSystemAndValue(FhirConst.IdentifierSystems.kOdsSiteCode, value);
        }

        [Given(@"I add a Location Identifier parameter with local System and Value ""([^""]*)""")]
        public void AddALocationIdentifierParameterWithLocalSystemAndValue(string value)
        {
            AddALocationIdentifierParameterWithSystemAndValue(FhirConst.IdentifierSystems.kLocalLocationCode, value);
        }

        [Given(@"I add a Location ""([^""]*)"" parameter with default System and Value ""([^""]*)""")]
        public void AddALocationParameterWithDefaultSystemAndValue(string parameterName, string value)
        {
            string locationCode;

            GlobalContext.OdsCodeMap.TryGetValue(value, out locationCode);

            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter(parameterName, $"{FhirConst.IdentifierSystems.kOdsSiteCode}|{GlobalContext.OdsCodeMap[value]}");
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

        [Then(@"the Location Id should be valid")]
        public void TheLocationIdShouldBeValid()
        {
            Locations.ForEach(location =>
            {
                location.Id.ShouldNotBeNullOrEmpty($"The Location Id should not be null or empty but was {location.Id}.");
            });
        }

        [Then(@"the Location Id should equal the Request Id")]
        public void TheLocationIdShouldEqualTheRequestId()
        {
            Locations.ForEach(location =>
            {
                location.Id.ShouldBe(_httpContext.HttpRequestConfiguration.GetRequestId, $"The Location Id should be equal to {_httpContext.HttpRequestConfiguration.GetRequestId} but was {location.Id}.");
            });
        }

        [Then(@"the Location Type should be valid")]
        public void TheLocationTypeShouldBeValid()
        {
            Locations.ForEach(location =>
            {
                var locationType = location.Type;

                if (locationType != null)
                {
                    locationType.Extension.ForEach(ext => ext.Url.ShouldNotBeNullOrEmpty("Location Type has an invalid extension. Extensions must have a URL element."));

                    // locationType codeable concept binding is extensible
                    // ideally I would check if it was of FhirConst.ValueSetSystems.kServDelLocationRoleType
                    // and if so check the code
                    // as extensible is does not need to be of FhirConst.ValueSetSystems.kServDelLocationRoleType

                    //var serviceDeliveryLocationRoleTypes = GlobalContext.GetFhirGpcValueSet(FhirConst.ValueSetSystems.kServDelLocationRoleType).WithComposeImports();

                    foreach (var coding in locationType.Coding)
                    {
                        coding.System.ShouldNotBeNullOrEmpty("The Location Type Coding should contain a System Value.");

                        //if (coding.System.Equals(FhirConst.ValueSetSystems.kServDelLocationRoleType) && serviceDeliveryLocationRoleTypes != null)
                        //{
                        //        coding.Code.ShouldBeOneOf(serviceDeliveryLocationRoleTypes.ToArray(), "The Location Type Coding should contain a Code.");
                        //}

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
                location.PartOf?.Reference?.ShouldStartWith("Location/", "The reference element within the PartOf element of the Location resource should contain a relative Location reference.");
            });
        }

        [Then(@"the Location Managing Organization should be valid")]
        public void TheLocationManagingOrganizationShouldBeValid()
        {
            Locations.ForEach(location =>
            {
                location.ManagingOrganization?.Reference?.ShouldStartWith("Organization/", "The ManagingOrganization reference should be a relative url for an Organization.");
            });
        }

        [Then(@"the Location Metadata should be valid")]
        public void TheLocationMetadataShouldBeValid()
        {
            Locations.ForEach(location =>
            {
                CheckForValidMetaDataInResource(location, FhirConst.StructureDefinitionSystems.kLocation);
            });
        }

        [Then(@"the Location Telecom should be valid")]
        public void TheLocationTelecomShouldBeValid()
        {

            Locations.ForEach(location =>
            {
                location.Telecom?.ForEach(contactPoint =>
                {
                    contactPoint.Extension.ForEach(ext => ext.Url.ShouldNotBeNullOrEmpty(string.Format("{0} has an invalid extension. Extensions must have a URL element.", "Location Telecom")));
                    contactPoint.System?.ShouldBeOfType<ContactPoint.ContactPointSystem>($"Telecom System is invalid. Should be one of {System.Enum.GetNames(typeof(ContactPoint.ContactPointSystem))}");
                    contactPoint.Use?.ShouldBeOfType<ContactPoint.ContactPointUse>($"Telecom Use is invalid. Should be one of {System.Enum.GetNames(typeof(ContactPoint.ContactPointUse))}");
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
                    .Where(identifier => identifier.System.Equals(FhirConst.IdentifierSystems.kOdsSiteCode))
                    .ToList();

                siteCodeIdentifiers.Count.ShouldBeLessThanOrEqualTo(1, "There should be a maximum of one ods site code in the location resource");

                siteCodeIdentifiers.ForEach(si =>
                {
                    si.System.ShouldNotBeNullOrEmpty("Location Identifier System must have a value.");
                    si.Value.ShouldNotBeNullOrEmpty("Location Identifier Value must have a value.");
                });

                if (!string.IsNullOrEmpty(locationName))
                {
                     siteCodeIdentifiers.ForEach(siteCodeIdentifier =>
                    {
                        siteCodeIdentifier.Value.ShouldBe(GlobalContext.OdsCodeMap[locationName], "Location business identifier does not match the expected business identifier.");
                    });
                }

                var localCodeIdentifiers = location.Identifier
                    .Where(identifier => identifier.System.Equals(FhirConst.IdentifierSystems.kLocalLocationCode))
                    .ToList();

                localCodeIdentifiers.Count.ShouldBeLessThanOrEqualTo(1, "There should be a maximum of one local identifier code in the location resource");


                localCodeIdentifiers.ForEach(li =>
                {
                    CheckForValidLocalIdentifier(li, () => ValidateAssignerRequest(location.PartOf.Reference));
                });

            });
        }

        private void ValidateAssignerRequest(string reference)
        {
            _httpSteps.ConfigureRequest(GpConnectInteraction.OrganizationRead);

            _httpContext.HttpRequestConfiguration.RequestUrl = reference;

            _httpSteps.MakeRequest(GpConnectInteraction.OrganizationRead);

            _httpResponseSteps.ThenTheResponseStatusCodeShouldIndicateSuccess();

            StoreTheOrganization();

            var returnedReference = _fhirResourceRepository.Organization.ResourceIdentity().ToString();

            returnedReference.ShouldBe(FhirConst.StructureDefinitionSystems.kOrganisation);
        }

        private void StoreTheOrganization()
        {
            var organization = _httpContext.FhirResponse.Organizations.FirstOrDefault();
            if (organization != null)
            {
                _httpContext.HttpRequestConfiguration.GetRequestId = organization.Id;
                _fhirResourceRepository.Organization = organization;
            }
        }
    }
}
