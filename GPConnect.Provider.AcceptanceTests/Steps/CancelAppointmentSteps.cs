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


        [When(@"I set the URL to ""(.*)"" and cancel ""(.*)""")]
        public void WhenISetTheURLtoStringAndCancel(string URL, string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            storedAppointment.Status = AppointmentStatus.Cancelled;
          

            Extension extension = new Extension();
            List<Extension> extensionList = new List<Extension>();
            extension = (buildAppointmentCancelExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1-0", "Double booked", "Double booked"));
            storedAppointment.ModifierExtension.Add(extension);
            HttpSteps.RestRequest(RestSharp.Method.PUT, URL, FhirSerializer.SerializeToJson(storedAppointment));
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

        [When(@"I cancel the appointment with cancel extension with url ""(.*)"" code ""(.*)"" and display ""(.*)"" called ""(.*)""")]
        public void ICancelTheAppointmentCalledAppointmentNameWithAnInvalidExtension(string url, string code, string display,string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            storedAppointment.Status = AppointmentStatus.Cancelled;
            string fullUrl = "Appointment/" + storedAppointment.Id;

            Extension extension = new Extension();
            List<Extension> extensionList = new List<Extension>();
            extension = (buildAppointmentCancelExtension(extension, url, code, display));
            storedAppointment.ModifierExtension.Add(extension);
            HttpSteps.RestRequest(RestSharp.Method.PUT, fullUrl, FhirSerializer.SerializeToJson(storedAppointment));
        }


        [Then("the returned appointment resource should be cancelled")]
        public void ThenTheReturnedAppointmentResourceShouldBeCancelled()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            //Only manadatory field on location specification
            appointment.Status.ShouldBe(AppointmentStatus.Cancelled);
        }
          [Then(@"the cancellation reason in the returned response should be equal to ""(.*)""")]
        public void ThenTheCancellationReasonInTheReturnedResponseShouldBeEqualToStrings(string display)
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            foreach (var extension in appointment.Extension)
            {
                if (string.Equals(extension.Url, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1"))
                {
                    extension.Value.ShouldNotBeNull("There should be a value element within the appointment CancellationReason extension");
                    var extensionValueString = (FhirString)extension.Value;
                    extensionValueString.Value.ShouldBe(display);
                }
            }
        }


        [Then(@"the resource type of ""(.*)"" and the returned response should be equal")]
        public void TheResponseTypeOfStringAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            storedAppointment.ResourceType.ShouldBe(returnedAppointment.ResourceType);
        }

        [Then(@"the id of ""(.*)"" and the returned response should be equal")]
        public void TheIdOfStringAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            storedAppointment.Id.ShouldBe(returnedAppointment.Id);
        }

        [Then(@"the status of ""(.*)"" and the returned response should be equal")]
        public void TheStatusOfStringAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            storedAppointment.Status.ShouldBe(returnedAppointment.Status);
        }

        [Then(@"the extension of ""(.*)"" and the returned response should be equal")]
        public void TheExtensionOfStringAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            FhirString extensionValueString = new FhirString();
            foreach (var extension in returnedAppointment.Extension)
            {
                if (string.Equals(extension.Url, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1"))
                {
                    extension.Value.ShouldNotBeNull("There should be a value element within the appointment CancellationReason extension");
                    extensionValueString = (FhirString)extension.Value;
                 }
            }

            foreach (var extension in storedAppointment.Extension)
            {
                if (string.Equals(extension.Url, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1"))
                {
                    extension.Value.ShouldNotBeNull("There should be a value element within the appointment CancellationReason extension");
                    var value = (FhirString)extension.Value;
                    value.ShouldBe(extensionValueString);
                }
            }
        }

        [Then(@"the description of ""(.*)"" and the returned response should be equal")]
        public void ThenTheDescriptionOfAppointmentAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            storedAppointment.Description.ShouldBe(returnedAppointment.Description);
        }


        [Then(@"the start and end date of ""(.*)"" and the returned response should be equal")]
        public void ThenTheStartAndEndDateOfAppointmentAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            storedAppointment.Start.ShouldBe(returnedAppointment.Start);
            storedAppointment.End.ShouldBe(returnedAppointment.End);
        }

        [Then(@"the reason of ""(.*)"" and the returned response should be equal")]
        public void ThenTheReasonDateOfAppointmentAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            storedAppointment.Reason.Text.ShouldBe(returnedAppointment.Reason.Text);
          }

        [Then(@"the slot display and reference of ""(.*)"" and the returned response should be equal")]
        public void ThenTheSlotDisplayOfAppointmentAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            string storedSlotReference = "";
            string storedSlotDisplay = "";


            foreach (var slotReference in storedAppointment.Slot)
            {
                storedSlotReference = slotReference.Reference;
                storedSlotDisplay = slotReference.Display;
            }

            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            foreach (var slotReference in returnedAppointment.Slot)
            {
                storedSlotReference.ShouldBe(slotReference.Reference);
                storedSlotDisplay.ShouldBe(slotReference.Display);
            }
        }

        [Then(@"the type and reference of ""(.*)"" and the returned response should be equal")]
        public void ThenTheTypeOfAppointmentAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            var code = "";
            returnedAppointment.Type.Text.ShouldBe(storedAppointment.Type.Text);
            foreach (var coding in returnedAppointment.Type.Coding)
            {
                code = coding.Code;
            }
            foreach (var coding in storedAppointment.Type.Coding)
            {
                coding.Code.ShouldBe(code);
              
            }
            storedAppointment.Comment.ShouldBe(returnedAppointment.Comment);
        }

        [Then(@"the patient participant of ""(.*)"" and the returned response should be equal")]
        public void ThenTheParticipantsOfAppointmentAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment savedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            ParticipationStatus savedAppointmentParticipantStatus = new ParticipationStatus();
            ParticipationStatus returnedResponseAppointmentParticipantStatus = new ParticipationStatus();

            foreach (Appointment.ParticipantComponent participant in savedAppointment.Participant)
            {
                if (participant.Actor.Reference.StartsWith("Patient/"))
                {
                    savedAppointmentParticipantStatus = participant.Status.Value;
                }

            }

            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            returnedAppointment.Participant.ShouldNotBeNull();
                    foreach (Appointment.ParticipantComponent participant in returnedAppointment.Participant)
                    {
                        if (participant.Actor.Reference.StartsWith("Patient/"))
                        {
                            returnedResponseAppointmentParticipantStatus = participant.Status.Value;
                        }

            }

            savedAppointmentParticipantStatus.ShouldBe(returnedResponseAppointmentParticipantStatus);
        }



        [Then(@"the location participant of ""(.*)"" and the returned response should be equal")]
        public void ThenTheLocationOfAppointmentAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment savedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            ParticipationStatus savedAppointmentParticipantStatus = new ParticipationStatus();
            ParticipationStatus returnedResponseAppointmentParticipantStatus = new ParticipationStatus();

            foreach (Appointment.ParticipantComponent participant in savedAppointment.Participant)
            {
                if (participant.Actor.Reference.StartsWith("Patient/"))
                {
                    savedAppointmentParticipantStatus = participant.Status.Value;
                }

            }

            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            returnedAppointment.Participant.ShouldNotBeNull();
            foreach (Appointment.ParticipantComponent participant in returnedAppointment.Participant)
            {
                if (participant.Actor.Reference.StartsWith("Patient/"))
                {
                    returnedResponseAppointmentParticipantStatus = participant.Status.Value;
                }

            }

            savedAppointmentParticipantStatus.ShouldBe(returnedResponseAppointmentParticipantStatus);
        }











        [Then(@"the practitioner participant of ""(.*)"" and the returned response should be equal")]
        public void ThenThePractitionerOfAppointmentAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment savedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            ParticipationStatus savedAppointmentParticipantStatus = new ParticipationStatus();
            ParticipationStatus returnedResponseAppointmentParticipantStatus = new ParticipationStatus();

            foreach (Appointment.ParticipantComponent participant in savedAppointment.Participant)
            {
                if (participant.Actor.Reference.StartsWith("Patient/"))
                {
                    savedAppointmentParticipantStatus = participant.Status.Value;
                }

            }

            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            returnedAppointment.Participant.ShouldNotBeNull();
            foreach (Appointment.ParticipantComponent participant in returnedAppointment.Participant)
            {
                if (participant.Actor.Reference.StartsWith("Patient/"))
                {
                    returnedResponseAppointmentParticipantStatus = participant.Status.Value;
                }

            }

            savedAppointmentParticipantStatus.ShouldBe(returnedResponseAppointmentParticipantStatus);
        }










        [Then(@"the comment of ""(.*)"" and the returned response should be equal")]
        public void ThenTheCommentOfAppointmentAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            storedAppointment.Comment.ShouldBe(returnedAppointment.Comment);
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
