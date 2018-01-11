namespace GPConnect.Provider.AcceptanceTests.Context
{
    using System;
    using System.Collections.Generic;
    using Data;
    using Helpers;
    using Logger;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Rest;
    using Hl7.Fhir.Specification.Source;
    using NUnit.Framework;

    public static class GlobalContext
    {
        private static readonly GlobalContextHelper GlobalContextHelper = new GlobalContextHelper();

        private static class Context
        {
            public const string kTraceDirectory = "traceDirectory";
            public const string kFhirGenderValueSet = "fhirGenderValueSet";
            public const string kFhirMaritalStatusValueSet = "fhirMaritalStatusValueSet";
            public const string kFhirRelationshipValueSet = "fhirRelationshipValueSet";
            public const string kFhirHumanLanguageValueSet = "fhirHumanLanguageValueSet";
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

        // FHIR
        public static ValueSet FhirGenderValueSet
        {
            get { return GlobalContextHelper.GetValue<ValueSet>(Context.kFhirGenderValueSet); }
            set { GlobalContextHelper.SaveValue(Context.kFhirGenderValueSet, value); }
        }

        public static ValueSet FhirMaritalStatusValueSet
        {
            get { return GlobalContextHelper.GetValue<ValueSet>(Context.kFhirMaritalStatusValueSet); }
            set { GlobalContextHelper.SaveValue(Context.kFhirMaritalStatusValueSet, value); }
        }
        
        public static ValueSet FhirRelationshipValueSet
        {
            get { return GlobalContextHelper.GetValue<ValueSet>(Context.kFhirRelationshipValueSet); }
            set { GlobalContextHelper.SaveValue(Context.kFhirRelationshipValueSet, value); }
        }

        public static ValueSet FhirHumanLanguageValueSet
        {
            get { return GlobalContextHelper.GetValue<ValueSet>(Context.kFhirHumanLanguageValueSet); }
            set { GlobalContextHelper.SaveValue(Context.kFhirHumanLanguageValueSet, value); }
        }

        public static ValueSet FhirAppointmentCategoryValueSet { get; set; }
        public static ValueSet FhirAppointmentBookingMethodValueSet { get; set; }
        public static ValueSet FhirAppointmentContactMethodValueSet { get; set; }

        private static Dictionary<string, ValueSet> _fhirExtensibleValueSets { get; set; }
        public static Dictionary<string, string> LocationLogicalIdentifierMap { get; set; }

        public static Dictionary<string, List<Appointment>> CreatedAppointments { get; set; }

        public static ValueSet GetExtensibleValueSet(string system)
        {
            var hasKey = _fhirExtensibleValueSets?.ContainsKey(system);

            if (hasKey.HasValue && hasKey.Value)
            {
                return _fhirExtensibleValueSets[system];
            }

            return FindExtensibleValueSet(system);
        }

        private static ValueSet FindExtensibleValueSet(string system)
        {

            var vsSources = new List<IArtifactSource>();

            if (AppSettingsHelper.FhirCheckWeb && AppSettingsHelper.FhirCheckWebFirst)
            {
                vsSources.Add(new WebArtifactSource(uri => new FhirClient(AppSettingsHelper.FhirWebDirectory)));
            }

            if (AppSettingsHelper.FhirCheckDisk)
            {
                vsSources.Add(new FileDirectoryArtifactSource(AppSettingsHelper.FhirDirectory, true));
            }

            if (AppSettingsHelper.FhirCheckWeb && !AppSettingsHelper.FhirCheckWebFirst)
            {
                vsSources.Add(new WebArtifactSource(uri => new FhirClient(AppSettingsHelper.FhirWebDirectory)));
            }

            //possible store resolver on the context
            var resolver = new ArtifactResolver(new MultiArtifactSource(vsSources));

            var valueSet = resolver.GetValueSet(system);
            if (valueSet == null)
            {
                Assert.Fail($"{system} ValueSet Not Found.");
            }

            Log.WriteLine("{0} Concepts loaded from {1}.", valueSet.CodeSystem.Concept.Count, system);


            if (_fhirExtensibleValueSets == null)
            {
                _fhirExtensibleValueSets = new Dictionary<string, ValueSet>();
            }

            _fhirExtensibleValueSets.Add(system, valueSet);

            return valueSet;
        }
    }
}