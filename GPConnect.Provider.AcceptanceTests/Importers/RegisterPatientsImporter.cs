using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using GPConnect.Provider.AcceptanceTests.Data;
using CsvHelper.Configuration;

namespace GPConnect.Provider.AcceptanceTests.Importers
{
    public static class RegisterPatientsImporter
    {
        public static List<RegisterPatient> LoadCsv(string filename)
        {
            using (var csv = new CsvReader(new StreamReader(filename)))
            {
                csv.Configuration.RegisterClassMap<RegisterPatientConverter>();
                return csv.GetRecords<RegisterPatient>().ToList();
            }
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class RegisterPatientConverter : CsvClassMap<RegisterPatient>
    {
        public RegisterPatientConverter()
        {
            Map(p => p.SPINE_NHS_NUMBER).Name("SPINE_NHS_NUMBER");
            Map(p => p.NAME_FAMILY).Name("NAME_FAMILY");
            Map(p => p.NAME_GIVEN).Name("NAME_GIVEN");
            Map(p => p.GENDER).Name("GENDER");
            Map(p => p.DOB).Name("DOB");
            Map(p => p.IsRegistered).Ignore(true);
        }
    }
}
