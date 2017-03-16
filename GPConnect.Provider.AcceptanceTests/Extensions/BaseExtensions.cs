using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;

namespace GPConnect.Provider.AcceptanceTests.Extensions
{
    public static class BaseExtensions
    {
        public static string ToJson(this Base resource)
        {
            return FhirSerializer.SerializeToJson(resource);
        }
    }
}
