namespace GPConnect.Provider.AcceptanceTests.Cache.ValueSet
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using GPConnect.Provider.AcceptanceTests.Helpers;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Serialization;
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
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            var keyCleaned = new string(key.Where(ch => !invalidFileNameChars.Contains(ch)).ToArray()); // This is important to remove characters which stop the file name being valid
            var filePath = AppSettingsHelper.FhirDirectory  + "/valuesets/" + keyCleaned; 

            var valueSet = new ValueSet();

            try
            {
                valueSet = ValueSetResolvers.GetResolver().FindValueSet(key);

                valueSet.ShouldNotBeNull($"There was no ValueSet found at {key}.");

                ExpandValueSet(valueSet);
                Set(key, valueSet);

                // Store the valueset so it can be used to load when no network connection
                StreamWriter jsonFile = new StreamWriter(filePath);
                jsonFile.WriteLine(FhirSerializer.SerializeResourceToJson(valueSet));
                jsonFile.Close();

            }
            catch (Exception exVS)
            {
                // Load from file
                StreamReader jsonFile = new StreamReader(filePath);
                FhirJsonParser parser = new FhirJsonParser();
                valueSet = parser.Parse<ValueSet>(jsonFile.ReadLine());
                jsonFile.Close();

                Set(key, valueSet);
            }

            return valueSet;
        }


        private static void ExpandValueSet(ValueSet valueSet)
        {
            var settings = new ValueSetExpanderSettings
            {
                ValueSetSource = ValueSetResolvers.GetDirectorySource()
            };

            var expander = new CustomValueSetExpander(settings);

            expander.Expand(valueSet);
        }
    }
}
