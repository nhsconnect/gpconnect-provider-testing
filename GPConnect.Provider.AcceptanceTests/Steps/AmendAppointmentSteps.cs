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
    public class AmendAppointmentSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private readonly HttpSteps HttpSteps;
        private readonly HttpContext HttpContext;

        // Headers Helper
        public HttpHeaderHelper Headers { get; }

        public AmendAppointmentSteps(HttpHeaderHelper headerHelper, FhirContext fhirContext, HttpSteps httpSteps, HttpContext httpContext)
        {
            // Helpers
            FhirContext = fhirContext;
            Headers = headerHelper;
            HttpSteps = httpSteps;
            HttpContext = httpContext;
        }

        [Given(@"I store the schedule for ""(.*)"" called ""(.*)"" and create an appointment called ""(.*)"" for patient ""(.*)"" using the interaction id ""(.*)""")]
        public void IPerformAnAppointmentAmendDorTheFirstAppointmentSavedInTheLBundleOfResorcesStoredAgainstKey(string ORG1, string getScheduleResponseBundle, string appointment, string patient, string interactionId)
        {
            Given($@"I perform the getSchedule operation for organization ""{ORG1}"" and store the returned bundle resources against key ""{getScheduleResponseBundle}""");
            Given($@"I am using the default server");
            Given($@"I am performing the ""{interactionId}"" interaction");
            Then($@"I create an appointment for patient ""{patient}"" called ""{appointment}"" from schedule ""{getScheduleResponseBundle}""");
            Then($@"I book the appointment called ""{appointment}""");
            Then("the response status code should indicate created");
            And("the response body should be FHIR JSON");
            And($@"the response should be an Appointment resource which is saved as ""savedFhirResource""");
        }

        [When(@"I amend ""(.*)"" by changing the comment to ""(.*)""")]
        public void IAmendByChangingTheCommentToMessage(string appointment, string message)
        {
            Appointment savedAppointment = (Appointment)HttpContext.StoredFhirResources[appointment];
            Appointment fhirApp = (Appointment)HttpContext.StoredAppointment["appointmentName"];
            savedAppointment.Id = fhirApp.Id;
            savedAppointment.Comment = message;
            string appointmentId = "/Appointment/" + savedAppointment.Id;
            HttpSteps.RestRequest(RestSharp.Method.PUT, appointmentId, FhirSerializer.SerializeToJson(savedAppointment));
        }

        [When(@"I set the URL to ""(.*)"" and amend ""(.*)"" by changing the comment to ""(.*)""")]
        public void IAmendByChangingTheCommentToMessageSettingUrl(string URL,string appointment, string message)
        {
            Appointment savedAppointment = (Appointment)HttpContext.StoredFhirResources[appointment];
            savedAppointment.Comment = message;
            HttpSteps.RestRequest(RestSharp.Method.PUT, URL, FhirSerializer.SerializeToJson(savedAppointment));
        }

        [Then(@"the appointment resource should contain a comment which equals ""(.*)""")]
        public void TheAppointmentResourceShouldContainACommentWhichEquals(string message)
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            appointment.Comment.ShouldBe(message);
        }
    }
}
