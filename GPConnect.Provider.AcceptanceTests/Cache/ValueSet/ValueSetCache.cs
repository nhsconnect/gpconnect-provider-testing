namespace GPConnect.Provider.AcceptanceTests.Cache.ValueSet
{
    using System.Collections.Generic;
    using System.Linq;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Specification.Source;
    using Hl7.Fhir.Specification.Terminology;
    using Resolvers;
    using Shouldly;
    using ValueSetExpander;

    internal static class ValueSetCache
    {
        private static Dictionary<string, ValueSet> _entries;

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
            var valueSet = ValueSetResolvers.GetResolver().FindValueSet(key);

            valueSet.ShouldNotBeNull($"There was no ValueSet found at {key}.");

            ExpandValueSet(valueSet);

            Set(key, valueSet);

            return valueSet;
        }

        private static void ExpandValueSet(ValueSet valueSet)
        {
            var settings = new ValueSetExpanderSettings
            {
                ValueSetSource = ValueSetResolvers.GetResolver()
            };

            var expander = new CustomValueSetExpander(settings);

            expander.Expand(valueSet);
        }
    }
}
