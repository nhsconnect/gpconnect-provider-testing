using CsvHelper.Configuration;
using GPConnect.Provider.AcceptanceTests.Data;

namespace GPConnect.Provider.AcceptanceTests.Importers
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class PractitionerCodeMapConverter : ClassMap<PractitionerCodeMap>
    {
        public PractitionerCodeMapConverter()
        {
            Map(p => p.NativePractitionerCode).Name("NATIVE_PRACTITIONER_CODE");
            Map(p => p.ProviderPractitionerCode).Name("PROVIDER_PRACTITIONER_CODE");
        }
    }
}