namespace GPConnect.Provider.AcceptanceTests.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Helpers;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;
    using Hl7.Fhir.Specification.Source;
    using Hl7.Fhir.Specification.Terminology;
    using Shouldly;

    public static class GlobalContext
    {
        private static readonly GlobalContextHelper GlobalContextHelper = new GlobalContextHelper();

        private static class Context
        {
            public const string kTraceDirectory = "traceDirectory";
        }

        public static string TraceDirectory
        {
            get { return GlobalContextHelper.GetValue<string>(Context.kTraceDirectory); }
            set { GlobalContextHelper.SaveValue(Context.kTraceDirectory, value); }
        }
        
        // Data
        public static List<RegisterPatient> RegisterPatients { get; set; }
        public static Dictionary<string, string> PractionerCodeMap { get; set; }
        public static Dictionary<string, string> PatientNhsNumberMap { get; set; }
        public static Dictionary<string, string> OdsCodeMap { get; set; }

        public static Guid TestRunId { get; set; }
        public static int ScenarioIndex { get; set; }
        public static string PreviousScenarioTitle { get; set; }

        private static Dictionary<string, ValueSet> _fhirExtensibleValueSets { get; set; }
        public static Dictionary<string, string> LocationLogicalIdentifierMap { get; set; }

        private static MultiResolver _resolver => GetResolver();

        private static MultiResolver GetResolver()
        {
            var resolvers = new List<IResourceResolver>
            {
                GetWebResolver()
                //Add other resolvers here
            };

            return new MultiResolver(resolvers);
        }

        private static WebResolver GetWebResolver()
        {
            return new WebResolver(GetFhirClientFactory());
        }

        private static ValueSet LoadValueSet(string uri)
        {
            var valueSet = _resolver.FindValueSet(uri);

            valueSet.ShouldNotBeNull($"There was no ValueSet found at {uri}.");

            ExpandValueSet(valueSet);

           
            if (_fhirExtensibleValueSets == null)
            {
                _fhirExtensibleValueSets = new Dictionary<string, ValueSet>();
            }

            _fhirExtensibleValueSets.Add(uri, valueSet);

            return valueSet;
        }

        private static void ExpandValueSet(ValueSet valueSet)
        {
            var settings = new ValueSetExpanderSettings
            {
                ValueSetSource = new DirectorySource(AppSettingsHelper.FhirDirectory, true)
            };

            var expander = new ValueSetExpander(settings);

            expander.Expand(valueSet);
        }

        private static Func<Uri, FhirClient> GetFhirClientFactory()
        {
            return uri =>
            {
                var client = new FhirClient(uri)
                {
                    PreferredFormat = ResourceFormat.Json,
                };

                return client;
            };
        }

        public static ValueSet GetValueSet(string system)
        {
            var hasKey = _fhirExtensibleValueSets?.ContainsKey(system);

            if (hasKey.HasValue && hasKey.Value)
            {
                return _fhirExtensibleValueSets[system];
            }

            return LoadValueSet(system);
        }
    }
}