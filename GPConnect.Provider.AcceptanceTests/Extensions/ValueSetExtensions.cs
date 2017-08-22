using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Model;

namespace GPConnect.Provider.AcceptanceTests.Extensions
{
    public static class ValueSetExtensions
    {
        public static IEnumerable<string> WithComposeImports(this ValueSet resource)
        {
            var codes = resource.CodeSystem.Concept.Select(cs => cs.Code);

            if (resource.Compose != null && resource.Compose.Import.Any())
            {
                codes = codes.Concat(resource.Compose.Import);
            }

            return codes;
        }
    }
}
