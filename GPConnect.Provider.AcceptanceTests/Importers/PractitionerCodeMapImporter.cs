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
            using var reader = new StreamReader(filename);
            using var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            {
                csv.Context.RegisterClassMap<PractitionerCodeMapConverter>();
                return csv.GetRecords<PractitionerCodeMap>().ToDictionary(x => x.NativePractitionerCode, x => x.ProviderPractitionerCode);
            }
        }
    }
}
