using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using TechTalk.SpecFlow;

namespace GPConnect.Provider.AcceptanceTests.Context
{
    using Hl7.Fhir.Specification.Navigation;

    public interface IFhirContext
    {
        Parameters FhirRequestParameters { get; set; }
        Resource FhirResponseResource { get; set; }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
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
            get
            {
                try
                {
                    return _scenarioContext.Get<Resource>(Context.kFhirResponseResource);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set
            {
                Log.WriteLine("{0}={1}", Context.kFhirResponseResource, value);
                _scenarioContext.Set(value, Context.kFhirResponseResource);
            }
        }

        //FHIR Entries
        public List<Bundle.EntryComponent> Entries => ((Bundle)FhirResponseResource).Entry;

        //FHIR Resources
        public List<Patient> Patients => GetResources<Patient>();
        public List<Organization> Organizations => GetResources<Organization>();
        public List<Composition> Compositions => GetResources<Composition>();
        public List<Device> Devices => GetResources<Device>();
        public List<Practitioner> Practitioners => GetResources<Practitioner>();
        public List<Location> Locations => GetResources<Location>();
        public Bundle Bundle => (Bundle) FhirResponseResource;
        public List<Slot> Slots => GetResources<Slot>();
        public List<Appointment> Appointments => GetResources<Appointment>();

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

        private List<T> GetResources<T>() where T : Resource
        {
            //Need to consider cases where T isn't in ResourceTypeMap (and implementation!!)
            var type = typeof(T);

            if (FhirResponseResource.ResourceType == ResourceType.Bundle)
            {
                return Entries
                    .Where(entry => entry.Resource.ResourceType.Equals(ResourceTypeMap[type]))
                    .Select(entry => (T) entry.Resource)
                    .ToList();
            }

            return new List<T>
            {
                (T)FhirResponseResource
            };
        }

        private static Dictionary<Type, ResourceType> ResourceTypeMap => new Dictionary<Type, ResourceType>
        {

            {typeof(Patient), ResourceType.Patient},
            {typeof(Organization), ResourceType.Organization},
            {typeof(Composition), ResourceType.Composition},
            {typeof(Device), ResourceType.Device},
            {typeof(Practitioner), ResourceType.Practitioner},
            {typeof(Location), ResourceType.Location},
            {typeof(Slot), ResourceType.Slot},
            {typeof(Appointment), ResourceType.Appointment}
        };

    }
}
