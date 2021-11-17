namespace GPConnect.Provider.AcceptanceTests.Importers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CsvHelper;
    using Data;

    internal static class HealthcareLogicalIdentifierImporter
    {
        public static Dictionary<string, string> LoadCsv(string filename)
        {
            using (var csv = new CsvReader(new StreamReader(filename)))
            {
                csv.Configuration.RegisterClassMap<HealthcareLogicalIdentifierConverter>();

                return csv
                    .GetRecords<HealthcareLogicalIdentifierMap>()
                    .ToDictionary(x => x.Healthcare, x => x.LogicalIdentifier);
            }
        }
    }
}
