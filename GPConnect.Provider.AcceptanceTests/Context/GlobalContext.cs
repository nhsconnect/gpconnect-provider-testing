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
    }
}