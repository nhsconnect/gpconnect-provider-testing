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
            return resource.Compose
                .Include
                .SelectMany(ConceptCodes);
        }
        
        public static IEnumerable<GpcCode> WithComposeIncludes(this ValueSet resource)
        {
            return resource.Compose
                .Include
                .SelectMany(GpcCodes);
        }

        private static IEnumerable<GpcCode> GpcCodes(ValueSet.ConceptSetComponent include)
        {
            return include.Concept.Select(concept => new GpcCode(concept.Code, concept.Display, include.System));
        }

        private static IEnumerable<string> ConceptCodes(ValueSet.ConceptSetComponent include)
        {
            return include.Concept.Select(concept => concept.Code);
        }
    }
}
