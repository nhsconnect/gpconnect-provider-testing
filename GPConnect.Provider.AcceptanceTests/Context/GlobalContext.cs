using System.Collections.Generic;
using System.Linq;
using GPConnect.Provider.AcceptanceTests.Data;
using GPConnect.Provider.AcceptanceTests.Helpers;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Specification.Source;
using NUnit.Framework;

namespace GPConnect.Provider.AcceptanceTests.Context
{
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
        public static Dictionary<string, List<string>> OrganizationSiteCodeMap { get; set; }
        
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
        public static ValueSet FhirIdentifierTypeValueSet { get; set; }
        public static ValueSet FhirServiceDeliveryLocationRoleTypeValueSet { get; set; }

        private static Dictionary<string, ValueSet> _fhirExtensibleValueSets { get; set; }
        public static ValueSet GetExtensibleValueSet(string system)
        {
            if (_fhirExtensibleValueSets?.ContainsKey(system) != null)
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

            valueSet.Compose?.Include.ForEach(ci =>
            {
                var systemUri = ci.System;
                if (!string.IsNullOrEmpty(systemUri))
                {
                    
                    var systemSet = resolver.GetValueSet(systemUri);

                    var codes = systemSet?.CodeSystem?.Concept;

                    if (codes != null)
                    {
                        valueSet.setCodeSystem().CodeSystem.Concept.AddRange(codes);
                    }
                }
            });

            if (_fhirExtensibleValueSets == null)
            {
                _fhirExtensibleValueSets = new Dictionary<string, ValueSet>();
            }

            _fhirExtensibleValueSets.Add(system, valueSet);

            return valueSet;
        }

        private static ValueSet setCodeSystem(this ValueSet vs)
        {
            if (vs.CodeSystem == null)
            {
                vs.CodeSystem = new ValueSet.CodeSystemComponent();
            }

            if (vs.CodeSystem.Concept == null)
            {
                vs.CodeSystem.Concept = new List<ValueSet.ConceptDefinitionComponent>();
            }

            return vs;
        }

    }
}