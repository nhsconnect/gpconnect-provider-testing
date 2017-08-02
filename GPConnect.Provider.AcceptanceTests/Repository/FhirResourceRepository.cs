namespace GPConnect.Provider.AcceptanceTests.Repository
{
    using Hl7.Fhir.Model;

    public class FhirResourceRepository : IFhirResourceRepository
    {
        public Patient Patient { get; set; }
        public Organization Organization { get; set; }
        public Bundle Bundle { get; set; }
        public Appointment Appointment { get; set; }
        public Location Location { get; set; }
        public Practitioner Practitioner { get; set; }
    }
}
