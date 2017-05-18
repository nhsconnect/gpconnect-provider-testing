using System.Collections.Generic;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using Hl7.Fhir.Model;
using Shouldly;
using TechTalk.SpecFlow;
using static Hl7.Fhir.Model.Appointment;
using static Hl7.Fhir.Model.Bundle;
using NUnit.Framework;
using Hl7.Fhir.Serialization;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public class CancelAppointmentSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private readonly HttpSteps HttpSteps;
        private readonly HttpContext HttpContext;

        // Headers Helper
        public HttpHeaderHelper Headers { get; }

        public CancelAppointmentSteps(HttpHeaderHelper headerHelper, FhirContext fhirContext, HttpSteps httpSteps, HttpContext httpContext)
        {
            // Helpers
            FhirContext = fhirContext;
            Headers = headerHelper;
            HttpSteps = httpSteps;
            HttpContext = httpContext;
        }

        [Given(@"I get the first appointment saved in the bundle of resources stored against key ""(.*)"" and call it ""(.*)""")]
        public void WhenICancelTheFirstAppointmentSavedInTheBundleOfResourceStoredAgainstKey(string bundleOfPatientAppointmentsKey, string appointmentName)
        {
            Bundle patientAppointmentBundel = (Bundle)HttpContext.StoredFhirResources[bundleOfPatientAppointmentsKey];
            
            string appointmentLogicalId = patientAppointmentBundel.Entry[0].Resource.Id;
            When($@"I make a GET request to ""/Appointment/{appointmentLogicalId}""");
            Then("the response status code should indicate success");
            And("the response body should be FHIR JSON");
            And($@"the response should be an Appointment resource which is saved as ""{appointmentName}""");
        
        }

        [When(@"I cancel the appointment called ""(.*)""")]
        public void WhenICancelTheAppointmentCalledAppointmentName(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            storedAppointment.Status = AppointmentStatus.Cancelled;
            string url = "Appointment/" + storedAppointment.Id;
            
            Extension extension = new Extension();
            List<Extension> extensionList = new List<Extension>();
            extension = (buildAppointmentCancelExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1-0", "Double booked", "Double booked"));
            storedAppointment.ModifierExtension.Add(extension);
            HttpSteps.RestRequest(RestSharp.Method.PUT, url, FhirSerializer.SerializeToJson(storedAppointment));
        }

        private Extension buildAppointmentCancelExtension(Extension extension, string url, string code, string display)
        {
            extension.Url = url;
            CodeableConcept value = new CodeableConcept();
            Coding coding = new Coding();
            coding.Display = display;
            extension.Value = coding.DisplayElement;
            return extension;
        }
    }
}
