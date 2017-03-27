using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using GPConnect.Provider.AcceptanceTests.Data;

namespace GPConnect.Provider.AcceptanceTests.Importers
{
    public static class ODSCodeMapImporter
    {
        public static List<ODSCodeMap> LoadCsv(string filename)
        {
            using (var csv = new CsvReader(new StreamReader(filename)))
            {
                csv.Configuration.RegisterClassMap<ODSCodeMapConverter>();
                return csv.GetRecords<ODSCodeMap>().ToList();
            }
        }
    }
}
