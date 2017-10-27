namespace GPConnect.Provider.AcceptanceTests.Repository
{
    using Hl7.Fhir.Model;

    public interface IFhirResourceRepository
    {
        Patient Patient { get; set; }
        Organization Organization { get; set; }
        Bundle Bundle { get; set; }
        Appointment Appointment { get; set; }
        Location Location { get; set; }
        Practitioner Practitioner { get; set; }
    }
}