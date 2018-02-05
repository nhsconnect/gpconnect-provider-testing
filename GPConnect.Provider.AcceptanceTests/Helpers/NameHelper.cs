namespace GPConnect.Provider.AcceptanceTests.Helpers
{
    using System.Collections.Generic;
    using Hl7.Fhir.Model;

    public static class NameHelper
    {
        public static HumanName CreateOfficialName(string givenName, string familyName)
        {
            return CreateName(HumanName.NameUse.Official, givenName, familyName);
        }

        public static HumanName CreateName(HumanName.NameUse use, string givenName, string familyName)
        {
            var humanName = new HumanName
            {
                FamilyElement = new FhirString(familyName),
                GivenElement = new List<FhirString>
                {
                    new FhirString(givenName)
                },
                Use = use
            };

            return humanName;
        }
    }
}
