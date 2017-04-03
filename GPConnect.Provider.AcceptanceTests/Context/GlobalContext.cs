using System.Collections.Generic;
using GPConnect.Provider.AcceptanceTests.Data;
using GPConnect.Provider.AcceptanceTests.Helpers;
using Hl7.Fhir.Model;

namespace GPConnect.Provider.AcceptanceTests.Context
{
    public static class GlobalContext
    {
        private static readonly GlobalContextHelper GlobalContextHelper = new GlobalContextHelper();

        private static class Context
        {
            public const string kTraceDirectory = "traceDirectory";
            public const string kPDSData = "pdsData";
            public const string kODSCodeMapData = "odsCodeMapData";
            public const string kNHSNoMapData = "NHSNoMapData";
            public const string kPractitionerCodeMapData = "practitionerCodeMapData";
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

        public static List<PDS> PDSData
        {
            get { return GlobalContextHelper.GetValue<List<PDS>>(Context.kPDSData); }
            set { GlobalContextHelper.SaveValue(Context.kPDSData, value); }
        }

        public static List<ODSCodeMap> ODSCodeMapData
        {
            get { return GlobalContextHelper.GetValue<List<ODSCodeMap>>(Context.kODSCodeMapData); }
            set { GlobalContextHelper.SaveValue(Context.kODSCodeMapData, value); }
        }

        public static List<NHSNoMap> NHSNoMapData
        {
            get { return GlobalContextHelper.GetValue<List<NHSNoMap>>(Context.kNHSNoMapData); }
            set { GlobalContextHelper.SaveValue(Context.kNHSNoMapData, value); }
        }

        public static List<PractitionerCodeMap> PractitionerMapData
        {
            get { return GlobalContextHelper.GetValue<List<PractitionerCodeMap>>(Context.kPractitionerCodeMapData); }
            set { GlobalContextHelper.SaveValue(Context.kPractitionerCodeMapData, value); }
        }
        
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
    }
}