namespace GPConnect.Provider.AcceptanceTests.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Hl7.Fhir.Model;
    using Models;

    public static class ValueSetExtensions
    {
        public static IEnumerable<string> WithComposeImports(this ValueSet resource)
        {
            return resource.Expansion
                .Contains
                .Select(ConceptCodes);
        }
        
        public static IEnumerable<GpcCode> WithComposeIncludes(this ValueSet resource)
        {
            return resource.Expansion
                .Contains
                .Select(GpcCodes);
        }

        private static GpcCode GpcCodes(ValueSet.ContainsComponent contains)
        {
            return new GpcCode(contains.Code, contains.Display, contains.System);
        }

        private static string ConceptCodes(ValueSet.ContainsComponent contains)
        {
            return contains.Code;
        }
    }
}
