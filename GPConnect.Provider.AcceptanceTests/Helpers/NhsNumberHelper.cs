namespace GPConnect.Provider.AcceptanceTests.Helpers
{
    using Constants;
    using Hl7.Fhir.Model;

    public static class NhsNumberHelper
    {
        private static string DefaultSystem => FhirConst.IdentifierSystems.kNHSNumber;

        private static string InvalidNhsNumber => "1234567891";

        private static string InvalidSystem => "Invalid-System";

        public static Identifier GetDefaultIdentifier(string nhsNumber)
        {
            return new Identifier(DefaultSystem, nhsNumber);
        }

        public static Identifier GetDefaultIdentifierWithInvalidNhsNumber()
        {
            return new Identifier(DefaultSystem, InvalidNhsNumber);
        }

        public static Identifier GetDefaultIdentifierWithInvalidSystem(string nhsNumber)
        {
           return new Identifier(InvalidSystem, nhsNumber);
        }

        public static Identifier GetIdentifierWithEmptySystem(string nhsNumber)
        {
            return new Identifier(string.Empty, nhsNumber);
        }

        public static Identifier GetIdentifierWithEmptyNhsNumber()
        {
            return new Identifier(DefaultSystem, string.Empty);
        }

        public static bool IsNhsNumberValid(string nhsNumber)
        {
            nhsNumber = nhsNumber.Trim();

            long number;

            if (nhsNumber.Length != 10 || !long.TryParse(nhsNumber, out number))
            {
                return false;
            }

            var total = 0;

            for (var i = 0; i < 9; i++)
            {
                var digit = int.Parse(nhsNumber.Substring(i, 1));

                total = total + (digit * (10 - i));
            }

            var checkDigit = (11 - (total % 11) % 11);

            return checkDigit == int.Parse(nhsNumber.Substring(9, 1));
        }
    }
}
