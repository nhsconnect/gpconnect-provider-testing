using CsvHelper.Configuration;
using GPConnect.Provider.AcceptanceTests.Data;

namespace GPConnect.Provider.AcceptanceTests.Importers
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class ODSMap : CsvClassMap<ODS>
    {
        public ODSMap()
        {
            Map(p => p.ODSCode).Name("ODS Code");
            Map(p => p.ASID).Name("ASID");
            Map(p => p.OrganisationType).Name("Organisation Type");
            Map(p => p.OrganisationName).Name("Organisation Name");
            Map(p => p.DataSharingGroup).Name("Data Sharing Group");
            Map(p => p.FHIREndpoint).Name("FHIR Endpoint");
        }
    }
}