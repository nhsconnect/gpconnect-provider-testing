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
            //FhirContext.FhirOrganizations[organizaitonName]

            Given($@"I perform a patient search for patient ""{patient}"" and store the first returned resources against key ""AppointmentReadPatientResource""");
            // Search for above patient appointments

        }

        [Then(@"the response should be an Appointment resource")]
        public void theResponseShouldBeAnAppointmentResource()
        {
            FhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.Appointment);
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
        }

    }
}



    

