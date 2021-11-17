namespace GPConnect.Provider.AcceptanceTests.Importers
{
    using CsvHelper.Configuration;
    using Data;

    internal sealed class HealthcareLogicalIdentifierConverter : CsvClassMap<HealthcareLogicalIdentifierMap>
    {
        public HealthcareLogicalIdentifierConverter()
        {
            Map(l => l.Healthcare).Name("Healthcare");
            Map(p => p.LogicalIdentifier).Name("LogicalIdentifier");
        }
    }
}
