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
            public const string kFhirRequestParameters = "fhirRequestParameters";
            public const string kFhirResponseResource = "fhirResponseResource";
        }

        public FhirContext(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        // FHIR Request

        public Parameters FhirRequestParameters
        {
            get { return _scenarioContext.Get<Parameters>(Context.kFhirRequestParameters); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kFhirRequestParameters, value);
                _scenarioContext.Set(value, Context.kFhirRequestParameters);
            }
        }

        // FHIR Response

        public Resource FhirResponseResource
        {
            get { return _scenarioContext.Get<Resource>(Context.kFhirResponseResource); }
            set
            {
                Log.WriteLine("{0}={1}", Context.kFhirResponseResource, value);
                _scenarioContext.Set(value, Context.kFhirResponseResource);
            }
        }
    }
}
