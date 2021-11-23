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
        private List<HealthcareService> HealthcareServices => _httpContext.FhirResponse.HealthcareService;

        public HealthcareSteps(HttpContext httpContext, HttpSteps httpSteps)
            : base(httpSteps)
        {
            _httpContext = httpContext;
        }


        [Then(@"the Response Resource should be a Healthcare Service")]
        public void TheResponseResourceShouldBeAHealthcareService()
        {
            _httpContext.FhirResponse.Resource.ResourceType.ShouldBe(ResourceType.HealthcareService);

        }

        [Then(@"the Healthcare Id should match the GET request Id")]
        public void TheHealthcarenIdShouldMarchTheGetRequestId()
        {
            var healthcare = HealthcareServices.FirstOrDefault();
            healthcare.ShouldNotBeNull();
            healthcare.Id.ShouldBe(_httpContext.HttpRequestConfiguration.GetRequestId);
            Logger.Log.WriteLine("INFO : Validated Healthcare service ID matches request ID : " + healthcare.Id);
        }

        [Then(@"the Healthcare service should be valid")]
        public void TheHealthcareserviceshouldbevalid()
        {
            var healthcare = HealthcareServices.FirstOrDefault();
            checkHealthcareServiceIsValid(healthcare);
        }

        public void checkHealthcareServiceIsValid(Resource healthservice)
        {
            var hs = (HealthcareService)healthservice;
            hs.ShouldNotBeNull();
            hs.Id.ShouldNotBeNull();
            CheckForValidMetaDataInResource(hs, FhirConst.StructureDefinitionSystems.kHealthcareService);

            Logger.Log.WriteLine("INFO : Validated Healthcare service with ID : " + hs.Id);
        }


        [Then(@"the response searchset contains atleast one HealthService")]
        public void TheresponsecontainsatleastoneHealthService()
        {
            _httpContext.FhirResponse.Entries.Count.ShouldBeGreaterThanOrEqualTo(1, "The response bundle does not contain  atleast one healthcare service");

            _httpContext.FhirResponse.Entries.ForEach(entry =>
            {
                entry.Resource.ResourceType.ShouldBe(ResourceType.HealthcareService);
            });


        }

        [Then(@"the response searchset contains valid Healthcare Service resources")]
        public void TheresponsesearchsetcontainsvalidHealthServiceresources()
        {
            _httpContext.FhirResponse.Entries.ForEach(entry =>
            {
                checkHealthcareServiceIsValid(entry.Resource);
            });
        }



    }
}
