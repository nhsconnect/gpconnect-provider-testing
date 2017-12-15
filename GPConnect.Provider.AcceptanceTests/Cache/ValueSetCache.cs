namespace GPConnect.Provider.AcceptanceTests.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Helpers;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;
    using Hl7.Fhir.Specification.Source;
    using Hl7.Fhir.Specification.Terminology;
    using Shouldly;

    internal static class ValueSetCache
    {
        private static Dictionary<string, ValueSet> _entries;
        private static WebResolver _webResolver => GetWebResolver();
        private static DirectorySource _directorySource => GetDirectorySource();
        private static MultiResolver _resolver => GetResolver();

        public static IReadOnlyList<ValueSet> GetAll()
        {
            if (_entries == null)
            {
                _entries = new Dictionary<string, ValueSet>();
            }

            return _entries.Values.ToList();
        }

        public static ValueSet Get(string key)
        {
            if (_entries == null)
            {
                _entries = new Dictionary<string, ValueSet>();
            }

            return _entries.ContainsKey(key)
                ? _entries[key]
                : GetValueSet(key);
        }

        private static void Set(string key, ValueSet valueSet)
        {
            if (_entries == null)
            {
                _entries = new Dictionary<string, ValueSet>();
            }
            else if (_entries.ContainsKey(key))
            {
                _entries.Remove(key);
            }

            _entries.Add(key, valueSet);
        }

        private static ValueSet GetValueSet(string key)
        {
            var valueSet = _resolver.FindValueSet(key);

            valueSet.ShouldNotBeNull($"There was no ValueSet found at {key}.");

            ExpandValueSet(valueSet);

            Set(key, valueSet);

            return valueSet;
        }

        private static void ExpandValueSet(ValueSet valueSet)
        {
            var settings = new ValueSetExpanderSettings
            {
                ValueSetSource = GetDirectorySource()
            };

            var expander = new ValueSetExpander(settings);

            expander.Expand(valueSet);
        }

        #region Resolvers

        private static MultiResolver GetResolver()
        {
            var resolvers = new List<IResourceResolver>();

            if (AppSettingsHelper.FhirCheckDisk)
                resolvers.Add(_directorySource);

            if (AppSettingsHelper.FhirCheckWeb)
                resolvers.Add(_webResolver);

            if (AppSettingsHelper.FhirCheckWebFirst)
                resolvers.Reverse();

            return new MultiResolver(resolvers);
        }

        private static WebResolver GetWebResolver()
        {
            return new WebResolver(GetFhirClientFactory());
        }

        private static DirectorySource GetDirectorySource()
        {
            return new DirectorySource(AppSettingsHelper.FhirDirectory, true);
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

        #endregion
    }
}
