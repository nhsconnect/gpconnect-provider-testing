namespace GPConnect.Provider.AcceptanceTests.Importers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CsvHelper;
    using Data;

    internal static class LocationLogicalIdentifierImporter
    {
        public static Dictionary<string, string> LoadCsv(string filename)
        {
            using (var csv = new CsvReader(new StreamReader(filename)))
            {
                csv.Configuration.RegisterClassMap<LocationLogicalIdentifierConverter>();

                return csv
                    .GetRecords<LocationLogicalIdentifierMap>()
                    .ToDictionary(x => x.Location, x => x.LogicalIdentifier);
            }
        }
    }
}
