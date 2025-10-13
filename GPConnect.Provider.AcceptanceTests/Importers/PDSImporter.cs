using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using GPConnect.Provider.AcceptanceTests.Data;

namespace GPConnect.Provider.AcceptanceTests.Importers
{
    public static class PDSImporter
    {
        public static List<PDS> LoadCsv(string filename)
        {
            using var reader = new StreamReader(filename);
            using var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            {
                csv.Context.RegisterClassMap<PDSMap>();
                return csv.GetRecords<PDS>().ToList();
            }
        }
    }
}
