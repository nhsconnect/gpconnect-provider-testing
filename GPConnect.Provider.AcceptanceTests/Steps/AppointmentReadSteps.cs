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
        
        [Given(@"I find or create ""([^ ""] *)"" appointments for patient ""([^""]*)"" at organization ""([^""]*)"" and save bundle of appintment resources to ""([^""]*)""")]
        public void IFindOrCreateAAppointmentsForPatientAtOrganizationAndSaveAListOfResourceTo(int noApp, string patient, string organizaitonName, string bundleOfPatientAppointmentskey)
        {
            // Search For Patient appointments
            Given($@"I search for patient ""{patient}"" appointments and save the returned bundle of appointment resources against key ""{bundleOfPatientAppointmentskey}""");
            Bundle patientAppointmentsBundle = (Bundle)HttpContext.StoredFhirResources[bundleOfPatientAppointmentskey];

            int numberOfRequiredAdditionalAppointments = noApp - patientAppointmentsBundle.Entry.Count;
            if (numberOfRequiredAdditionalAppointments > 0) {

                // Perform get schedule once to get available slots with which to create appointments
                Given($@"I perform the getSchedule operation for organization ""{organizaitonName}"" and store the returned bundle resources against key ""getScheduleResponseBundle""");

                for (int numberOfAppointmentsToCreate = numberOfRequiredAdditionalAppointments; numberOfAppointmentsToCreate > 0; numberOfAppointmentsToCreate--)
                {
                    // TODO - Create a new appointment for the patient using the slots and details from the getSchedule response saved object above
                }

                // Search for appointments again to make sure that enough have been stored in the provider system and store them
                Given($@"I search for patient ""{patient}"" appointments and save the returned bundle of appointment resources against key ""{bundleOfPatientAppointmentskey}""");
                patientAppointmentsBundle = (Bundle)HttpContext.StoredFhirResources[bundleOfPatientAppointmentskey];
            }
            patientAppointmentsBundle.Entry.Count.ShouldBeGreaterThanOrEqualTo(noApp, "We could not create enough appointments for the test to run.");
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

        [Given(@"I perform the getSchedule operation for organization ""([^""]*)"" and store the returned bundle resources against key ""([^""]*)""")]
        public void IPerformTheGetScheduleOperationForOrganizationAndStoreTheReturnedBundleResourceAgainstKey(string organization, string getScheduleResponseBundleKey)
        {
            // getSchedule operation for organization
            Given($@"I am using the default server");
            And($@"I search for the organization ""{organization}"" on the providers system and save the first response to ""firstOrganizationResource""");
            Given($@"I am using the default server");
            And($@"I am performing the ""urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule"" interaction");
            And($@"I add period request parameter with a start date of today and an end date ""13"" days later");
            When($@"I send a gpc.getschedule operation for the organization stored as ""firstOrganizationResource""");
            Then($@"the response status code should indicate success");
            And($@"the response body should be FHIR JSON");
            And($@"the response should be a Bundle resource of type ""searchset""");
            And($@"the response bundle should include slot resources");
            var returnedGetScheduleResponseBundle = (Bundle)FhirContext.FhirResponseResource;
            if (HttpContext.StoredFhirResources.ContainsKey(getScheduleResponseBundleKey)) HttpContext.StoredFhirResources.Remove(getScheduleResponseBundleKey);
            HttpContext.StoredFhirResources.Add(getScheduleResponseBundleKey, returnedGetScheduleResponseBundle);
        }

        [Then(@"the response should be an Appointment resource")]
        public void theResponseShouldBeAnAppointmentResource()
        {
            FhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.Appointment);
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
        }


    }
}



    

