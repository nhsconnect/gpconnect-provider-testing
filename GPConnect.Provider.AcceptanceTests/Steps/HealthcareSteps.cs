namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using Context;
    using Hl7.Fhir.Model;
    using Shouldly;
    using TechTalk.SpecFlow;
    using Constants;
    using Helpers;
    using Context;
    using Enum;

    [Binding]
    public class HealthcareSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private List<HealthcareService> HealthcareService => _httpContext.FhirResponse.HealthcareService;

        public HealthcareSteps(HttpContext httpContext, HttpSteps httpSteps)
            : base(httpSteps)
        {
            _httpContext = httpContext;
        }


        [Then(@"the Response Resource should be a Healthcare Service")]
        public void TheResponseResourceShouldBeAHealthcareService()
        {
            _httpContext.FhirResponse.Resource.ResourceType.ShouldBe(ResourceType.HealthcareService);

            var healthcare = (HealthcareService)_httpContext.FhirResponse.Resource;

            healthcare.Id.ShouldNotBeNullOrEmpty();

            //healthcare.Name.ShouldNotBeNullOrEmpty();
            //healthcare.Address.ShouldNotBeNull();
        }

        [Then(@"the Healthcare Id should match the GET request Id")]
        public void TheHealthcarenIdShouldMarchTheGetRequestId()
        {
            var healthcare = HealthcareService.FirstOrDefault();

            healthcare.ShouldNotBeNull();
            healthcare.Id.ShouldBe(_httpContext.HttpRequestConfiguration.GetRequestId);
        }

        //[Then(@"the Response Resource should be a Location")]
        //public void TheResponseResourceShouldBeALocation()
        //{
        //    _httpContext.FhirResponse.Resource.ResourceType.ShouldBe(ResourceType.Location);

        //    var location = (Location)_httpContext.FhirResponse.Resource;

        //    location.Name.ShouldNotBeNullOrEmpty();
        //    location.Address.ShouldNotBeNull();
        //}

        //[Then(@"the Location Id should be valid")]
        //public void TheLocationIdShouldBeValid()
        //{
        //    Locations.ForEach(location =>
        //    {
        //        location.Id.ShouldNotBeNullOrEmpty($"The Location Id should not be null or empty but was {location.Id}.");
        //    });
        //}        

    }
}
