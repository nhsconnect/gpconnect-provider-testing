namespace GPConnect.Provider.AcceptanceTests.Importers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CsvHelper;
    using Data;

    public static class OrganizationSiteCodeMapImporter
    {
        public static Dictionary<string, List<string>> LoadCsv(string filename)
        {
            using (var csv = new CsvReader(new StreamReader(filename)))
            {
                csv.Configuration.RegisterClassMap<OrganizationSiteCodeClassMap>();
                return csv.GetRecords<OrganizationSiteCodeMap>().ToDictionary(x => x.Key, x => x.Value.Split(',').Select(y => y.Trim()).ToList());
            }
        }
    }
}
