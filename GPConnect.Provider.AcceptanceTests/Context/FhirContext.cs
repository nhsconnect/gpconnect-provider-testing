using System.Collections.Generic;
using System.Xml.Linq;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using TechTalk.SpecFlow;

namespace GPConnect.Provider.AcceptanceTests.Context
{
    public interface IFhirContext
    {
        Dictionary<string, string> FhirPatients { get; set; }
        Parameters FhirRequestParameters { get; set; }
        Resource FhirResponseResource { get; set; }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class FhirContext : IFhirContext
    {
        private readonly ScenarioContext _scenarioContext;

        private static class Context
        {
            public const string kFhirOrganizations = "fhirOrganizations";
            public const string kFhirPractitioners = "fhirPractitioners";
            public const string kFhirPatients = "fhirPatients";
            public const string kFhirRequestParameters = "fhirRequestParameters";
            public const string kFhirResponseResource = "fhirResponseResource";
        }

        public FhirContext(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            FhirOrganizations = new Dictionary<string, string>();
            FhirPatients = new Dictionary<string, string>();
        }

        public Dictionary<string, string> FhirOrganizations
        {
            get { return _scenarioContext.Get<Dictionary<string, string>>(Context.kFhirOrganizations); }
            set { _scenarioContext.Set(value, Context.kFhirOrganizations); }
        }

        public Dictionary<string, string> FhirPractitioners
        {
            get { return _scenarioContext.Get<Dictionary<string, string>>(Context.kFhirPractitioners); }
            set { _scenarioContext.Set(value, Context.kFhirPractitioners); }
        }

        public Dictionary<string, string> FhirPatients
        {
            get { return _scenarioContext.Get<Dictionary<string, string>>(Context.kFhirPatients); }
            set { _scenarioContext.Set(value, Context.kFhirPatients); }
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

        public void SaveToDisk(string filename)
        {
            var doc = new XDocument(
                new XElement("fhirContext",
                    new XElement("request",
                        new XElement(Context.kFhirRequestParameters, FhirSerializer.SerializeResourceToJson(FhirRequestParameters))
                    ),
                    new XElement("response",
                        new XElement(Context.kFhirResponseResource, FhirSerializer.SerializeResourceToJson(FhirResponseResource))
                    )
                )
            );
            doc.Save(filename);
        }
    }
}
