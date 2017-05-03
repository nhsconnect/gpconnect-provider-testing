using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using RestSharp;
using Shouldly;
using TechTalk.SpecFlow;
using static Hl7.Fhir.Model.Bundle;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public class AppointmentCancelSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private readonly HttpSteps HttpSteps;
        private readonly HttpContext HttpContext;

        // Headers Helper
        public HttpHeaderHelper Headers { get; }

        public AppointmentCancelSteps(HttpHeaderHelper headerHelper, FhirContext fhirContext, HttpSteps httpSteps, HttpContext httpContext)
        {
            // Helpers
            FhirContext = fhirContext;
            Headers = headerHelper;
            HttpSteps = httpSteps;
            HttpContext = httpContext;
        }
        
        [Given(@"I cancel appointment resource stored against key ""([^""]*)"" and store the returned appointment resource against key ""([^""]*)""")]
        public void ICancelAppointmentOnTheProviderSystemAndStoreTheReturnedAppointmentResourceAgainstKey(string storedAppointmentKey, string appointmentStorageKey)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[storedAppointmentKey];
            storedAppointment.Status = Appointment.AppointmentStatus.Cancelled;
            storedAppointment.Extension.Add(new Extension("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1", new FhirString("GP Connect Test Suite Default Cancellation Reason")));
            string payloadString = FhirSerializer.SerializeToJson(storedAppointment);

            Given($@"I am using the default server");
            And($@"I am performing the ""urn:nhs:names:services:gpconnect:fhir:rest:update:appointment"" interaction");
            
            string relativeUrl = "/Appointment/" + storedAppointment.Id;
            HttpSteps.RestRequest(Method.PUT, relativeUrl, payloadString);

            Then($@"the response status code should indicate success");
            And($@"the response body should be FHIR JSON");
            And($@"the response should be an Appointment resource");

            var returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            HttpContext.StoredFhirResources.Add(appointmentStorageKey, returnedAppointment);
        }

    }
}
    

