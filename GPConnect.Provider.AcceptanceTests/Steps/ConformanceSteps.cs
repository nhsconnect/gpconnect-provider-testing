namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Context;
    using Hl7.Fhir.Model;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    public class ConformanceSteps 
    {
        private readonly HttpContext _httpContext;
        private List<Conformance> Conformances => _httpContext.HttpResponse.Conformances;
        public ConformanceSteps(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        [Then("the Response Resource should be a Conformance")]
        public void TheResponseResourceShouldBeALocation()
        {
            _httpContext.HttpResponse.Resource.ResourceType.ShouldBe(ResourceType.Conformance);
        }

        [Then("the Conformance Format should contain XML and JSON")]
        public void TheConformanceFormatShouldContainXmlAndJson()
        {
            Conformances.ForEach(conformance =>
            {
                conformance.Format.ShouldContain(FhirConst.ContentTypes.kJsonFhir, $"The Conformance Format should contain {FhirConst.ContentTypes.kJsonFhir} but did not.");
                conformance.Format.ShouldContain(FhirConst.ContentTypes.kXmlFhir, $"The Conformance Format should contain {FhirConst.ContentTypes.kXmlFhir} but did not.");
            });
        }

        [Then("the Conformance Software should be valid")]
        public void TheConformanceSoftwareShouldBeValid()
        {
            Conformances.ForEach(conformance =>
            {
                conformance.Software.Name.ShouldNotBeNullOrEmpty($"The Conformance Software Name should not be be null or empty but was {conformance.Software.Name}.");
                conformance.Software.Version.ShouldNotBeNullOrEmpty($"The Conformance Software Version should not be be null or empty but was {conformance.Software.Version}.");
            });
        }

        [Then(@"the Conformance FHIR Version should be ""([^""]*)""")]
        public void TheConformanceFhirVerionShouldBe(string version)
        {
            Conformances.ForEach(conformance =>
            {
                conformance.FhirVersion.ShouldBe(version, $"The Conformance FHIR Version should be {version} but was {conformance.FhirVersion}.");
            });
        }

        [Then(@"the Conformance REST Operations should contain ""([^""]*)""")]
        public void TheConformanceRestOperationsShouldContain(string operation)
        {
            Conformances.ForEach(conformance =>
            {
                conformance.Rest.ForEach(rest =>
                {
                    rest.Operation
                        .Select(op => op.Name)
                        .ShouldContain(operation, $"The Conformance REST Operations should contain {operation} but did not.");
                });
            });
        }
    }
}
