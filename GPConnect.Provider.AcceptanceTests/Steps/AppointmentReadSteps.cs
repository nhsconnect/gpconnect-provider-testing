using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using Hl7.Fhir.Model;
using Shouldly;
using TechTalk.SpecFlow;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public class AppointmentReadSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private readonly HttpSteps HttpSteps;
        private readonly HttpContext HttpContext;
      
        // Headers Helper
        public HttpHeaderHelper Headers { get; }

        public AppointmentReadSteps(HttpHeaderHelper headerHelper, FhirContext fhirContext, HttpSteps httpSteps, HttpContext httpContext)
        {
            // Helpers
            FhirContext = fhirContext;
            Headers = headerHelper;
            HttpSteps = httpSteps;
            HttpContext = httpContext;
        }
        
        [Given(@"I find or create ""([^ ""] *)"" appointments for patient ""([^""]*)"" at organization ""([^""]*)"" and save a list of resources to ""([^""]*)""")]
        public void IFindOrCreateAAppointmentsForPatientAtOrganizationAndSaveAListOfResourceTo(string noApp, string patient, string organizaitonName, string appointmentListkey)
        {
            // Search For Patient appointments
            Given($@"I search for patient ""{patient}"" appointments and save the returned bundle of appointment resources against key ""patientCurrentAppointments""");

        }


        [Given(@"I search for patient ""([^""]*)"" appointments and save the returned bundle of appointment resources against key ""([^""]*)""")]
        public void ISearchForPatientAppointmentsAndSaveTheReturnedBundleOfAppointmentResourcesAgainstKey(string patient, string patientAppointmentSearchBundleKey)
        {
            // Search For Patient
            Given($@"I perform a patient search for patient ""{patient}"" and store the first returned resources against key ""AppointmentReadPatientResource""");

            // Search For Patients Appointments
            Patient patientResource = (Patient)HttpContext.StoredFhirResources["AppointmentReadPatientResource"];
            Given($@"I am using the default server");
            And($@"I am performing the ""urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments"" interaction");
            When($@"I make a GET request to ""/Patient/{patientResource.Id}/Appointment""");
            Then($@"the response status code should indicate success");
            And($@"the response body should be FHIR JSON");
            And($@"the response should be a Bundle resource of type ""searchset""");

            var returnedPatientAppointmentSearchBundle = (Bundle)FhirContext.FhirResponseResource;
            if (HttpContext.StoredFhirResources.ContainsKey(patientAppointmentSearchBundleKey)) HttpContext.StoredFhirResources.Remove(patientAppointmentSearchBundleKey);
            HttpContext.StoredFhirResources.Add(patientAppointmentSearchBundleKey, returnedPatientAppointmentSearchBundle);

        }


        [Then(@"the response should be an Appointment resource")]
        public void theResponseShouldBeAnAppointmentResource()
        {
            FhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.Appointment);
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
        }


    }
}



    

