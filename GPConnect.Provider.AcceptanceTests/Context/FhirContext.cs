using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Model;
using TechTalk.SpecFlow;

namespace GPConnect.Provider.AcceptanceTests.Context
{
    public interface IFhirContext
    {
        Parameters FhirRequestParameters { get; set; }
        Resource FhirResponseResource { get; set; }
    }

    public class FhirContext : IFhirContext
    {
        private readonly ScenarioContext _scenarioContext;

        private static class Context
        {
            public const string FhirRequestParameters = "fhirRequestParameters";
            public const string FhirResponseResource = "fhirResponseResource";
        }

        public FhirContext(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        // FHIR Request

        public Parameters FhirRequestParameters
        {
            get { return _scenarioContext.Get<Parameters>(Context.FhirRequestParameters); }
            set
            {
                Log.WriteLine("{0}={1}", Context.FhirRequestParameters, value);
                _scenarioContext.Set(value, Context.FhirRequestParameters);
            }
        }

        // FHIR Response

        public Resource FhirResponseResource
        {
            get { return _scenarioContext.Get<Resource>(Context.FhirResponseResource); }
            set
            {
                Log.WriteLine("{0}={1}", Context.FhirResponseResource, value);
                _scenarioContext.Set(value, Context.FhirResponseResource);
            }
        }
    }
}
