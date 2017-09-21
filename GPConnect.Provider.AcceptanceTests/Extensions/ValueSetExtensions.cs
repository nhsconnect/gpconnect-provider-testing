using System.Collections.Generic;
using System.Linq;
using GPConnect.Provider.AcceptanceTests.Models;
using Hl7.Fhir.Model;

namespace GPConnect.Provider.AcceptanceTests.Extensions
{
    public static class ValueSetExtensions
    {
        public static IEnumerable<string> WithComposeImports(this ValueSet resource)
        {
            var codes = resource.CodeSystem?.Concept.Select(cs => cs.Code);

            if (resource.Compose != null && resource.Compose.Import.Any())
            {
                codes = (codes??new List<string>()).Concat(resource.Compose.Import);
            }

            return codes;
        }


        public static IEnumerable<GpcCode> WithComposeIncludes(this ValueSet resource)
        {
            var codes = new List<GpcCode>();
            var codeSystem = resource.CodeSystem;

            if (codeSystem != null)
            {
                var system = codeSystem.System;
                codes = codeSystem.Concept.Select(co => new GpcCode(co.Code, co.Display, system)).ToList();
                if (resource.Compose != null && resource.Compose.Include.Any())
                {
                    resource.Compose.Include.ForEach(include =>
                    {
                        var incSystem = include.System;
                        codes = codes.Concat(include.Concept.Select(co => new GpcCode(co.Code, co.Display, incSystem))).ToList();

                    });

                }
            }


            return codes;
     

        }

    }
}
