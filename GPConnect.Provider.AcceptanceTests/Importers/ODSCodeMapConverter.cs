using CsvHelper.Configuration;
using GPConnect.Provider.AcceptanceTests.Data;

namespace GPConnect.Provider.AcceptanceTests.Importers
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class ODSCodeMapConverter : CsvClassMap<ODSCodeMap>
    {
        public ODSCodeMapConverter()
        {
            Map(p => p.NativeODSCode).Name("NATIVE_ODS_CODE");
            Map(p => p.ProviderODSCode).Name("PROVIDER_ODS_CODE");
        }
    }
}