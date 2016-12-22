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
            public const string kODSData = "odsData";
            public const string kFhirGenderValueSet = "fhirGenderValueSet";
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

        public static List<ODS> ODSData
        {
            get { return GlobalContextHelper.GetValue<List<ODS>>(Context.kODSData); }
            set { GlobalContextHelper.SaveValue(Context.kODSData, value); }
        }

        // FHIR

        public static ValueSet FhirGenderValueSet
        {
            get { return GlobalContextHelper.GetValue<ValueSet>(Context.kFhirGenderValueSet); }
            set { GlobalContextHelper.SaveValue(Context.kFhirGenderValueSet, value); }
        }

    }
}