using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using GPConnect.Provider.AcceptanceTests.Data;

namespace GPConnect.Provider.AcceptanceTests.Importers
{
    public static class PractitionerCodeMapImporter
    {
        public static Dictionary<string, string> LoadCsv(string filename)
        {
            using (var csv = new CsvReader(new StreamReader(filename)))
            {
                csv.Configuration.RegisterClassMap<PractitionerCodeMapConverter>();
                return csv.GetRecords<PractitionerCodeMap>().ToDictionary(x => x.NativePractitionerCode, x => x.ProviderPractitionerCode);
            }
        }
    }
}
