namespace GPConnect.Provider.AcceptanceTests.Importers
{
    using CsvHelper.Configuration;
    using Data;

    public sealed class OrganizationSiteCodeClassMap : CsvClassMap<OrganizationSiteCodeMap>
    {
        public OrganizationSiteCodeClassMap()
        {
            Map(p => p.Key).Name("Key");
            Map(p => p.Value).Name("Value");
        }
    }
}
