using CsvHelper.Configuration;
using GPConnect.Provider.AcceptanceTests.Data;

namespace GPConnect.Provider.AcceptanceTests.Importers
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class PDSMap : ClassMap<PDS>
    {
        public PDSMap()
        {
            Map(p => p.NHSNumber).Name("NHS_NUMBER");
            Map(p => p.DateOfBirth).Name("DATE_OF_BIRTH");
            Map(p => p.DateOfDeath).Name("DATE_OF_DEATH");
            Map(p => p.FamilyName).Name("FAMILY_NAME");
            Map(p => p.GivenName).Name("GIVEN_NAME");
            Map(p => p.OtherGivenName).Name("OTHER_GIVEN_NAME");
            Map(p => p.Title).Name("TITLE");
            Map(p => p.Addr1).Name("ADDR1");
            Map(p => p.Addr2).Name("ADDR2");
            Map(p => p.Addr3).Name("ADDR3");
            Map(p => p.Addr4).Name("ADDR4");
            Map(p => p.Addr5).Name("ADDR5");
            Map(p => p.PostCode).Name("POST_CODE");
            Map(p => p.SensitiveFlag).Name("SENSITIVE_FLAG");
            Map(p => p.PrimaryCareCode).Name("PRIMARY_CARE_CODE");
        }
    }
}