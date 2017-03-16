using CsvHelper.Configuration;
using GPConnect.Provider.AcceptanceTests.Data;

namespace GPConnect.Provider.AcceptanceTests.Importers
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class NHSNoMapMap : CsvClassMap<NHSNoMap>
    {
        public NHSNoMapMap()
        {
            Map(p => p.NativeNHSNumber).Name("NATIVE_NHS_NUMBER");
            Map(p => p.ProviderNHSNumber).Name("PROVIDER_NHS_NUMBER");
        }
    }
}