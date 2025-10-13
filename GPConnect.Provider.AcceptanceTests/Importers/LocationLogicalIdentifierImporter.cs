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
            using var reader = new StreamReader(filename);
            using var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<LocationLogicalIdentifierConverter>();

            return csv
                .GetRecords<LocationLogicalIdentifierMap>()
                .ToDictionary(x => x.Location, x => x.LogicalIdentifier);
        }
    }
}
