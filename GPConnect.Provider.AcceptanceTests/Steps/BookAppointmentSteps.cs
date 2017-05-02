using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Model;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using static Hl7.Fhir.Model.Appointment;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    class BookAppointmentSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private readonly HttpSteps HttpSteps;
        private readonly HttpContext HttpContext;
    
        // Headers Helper
        public HttpHeaderHelper Headers { get; }

        public BookAppointmentSteps(HttpHeaderHelper headerHelper, FhirContext fhirContext, HttpSteps httpSteps, HttpContext httpContext)
        {
            // Helpers
            FhirContext = fhirContext;
            Headers = headerHelper;
            HttpSteps = httpSteps;
            HttpContext = httpContext;
        }

    

        [Then(@"I create an appointment for patient ""(.*)"" unless 1 exists and save the appointment called ""(.*)""")]
        public void GivenISearchForAnAppointmentOnTheProviderSystemAndBookAppointment(int id, string appointmentName)
        {
            
            var relativeUrl = "/Patient/" + id + "/Appointment";
            var returnedResourceBundle = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments", relativeUrl);
            returnedResourceBundle.GetType().ShouldBe(typeof(Bundle));
            if (((Bundle)returnedResourceBundle).Entry.Count < 2 || ((Bundle)returnedResourceBundle).Entry.Count == 0)
           {
                for (int i = 0; i < 1; i++)
               {
                    Then($@"I find a patient with id ""{id}"" and search for a slot and create ""{1}"" appointment called ""{appointmentName}""");
                }
            }
            else
           {

            }
        }

        [Then(@"I find a patient with id ""(.*)"" and search for a slot and create ""(.*)"" appointment called ""(.*)""")]
        public void bookAppointmentForPatient(int id, int numOfAppointments, string appointmentName)
        {
            Organization organization = (Organization)HttpContext.StoredFhirResources["ORG1"];
            List<Resource> slot = (List<Resource>)HttpContext.StoredSlots["Slot"];
            Location locationSaved = (Location)HttpContext.StoredFhirResources["Location"];
            string locationId = locationSaved.Id;
         

            Practitioner practitionerSaved = (Practitioner)HttpContext.StoredFhirResources["Practitioner"];
            string practitionerId = practitionerSaved.Id;

            Appointment appointment = new Appointment();

            //Patient Resource
            ParticipantComponent patient = new ParticipantComponent();
            ResourceReference patientReference = new ResourceReference();
            patientReference.Reference = "Patient/" + id;

            Code code = new Code();
            code.Equals("accepted");
            ParticipationStatus stat = new ParticipationStatus();

            patient.Status = stat;
            patient.Actor = patientReference;
            appointment.Participant.Add(patient);

            //Practitioner Resource
            ParticipantComponent practitioner = new ParticipantComponent();
            ResourceReference practitionerReference = new ResourceReference();

            practitionerReference.Reference = "Practitioner/" + practitionerId;
            practitioner.Actor = practitionerReference;
            appointment.Participant.Add(practitioner);

            //Location Resource
            ParticipantComponent location = new ParticipantComponent();
            ResourceReference locationReference = new ResourceReference();
            locationReference.Reference = "Location/" + locationId;
            location.Actor = locationReference;
            appointment.Participant.Add(location);

            //Slot Resource
            ResourceReference slots = new ResourceReference();
            foreach (Slot slotResource in slot)
            {
                string freeBusy = slotResource.FreeBusyType.Value.ToString();
                Boolean val = freeBusy.Equals("Free");
                if (val)
                {
                    string ids = slotResource.Id.ToString();
                    slots.Reference = slotResource.Id;
                    appointment.Slot.Add(slots);
                    slot.Remove(slotResource);
                    appointment.Start = slotResource.Start;
                    appointment.End = slotResource.End;
                    break;
                }
            }
            //AppointmentResources
            HttpContext.StoredSlots.Remove("Slot");
            HttpContext.StoredSlots.Add("Slot", slot);

            AppointmentStatus status = new AppointmentStatus();
            
            appointment.Status = status;
            //Store The Appointment
            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);


        }

        [Then(@"I remove the patient participant from the appointment called ""(.*)""")]
        public void removePatientParticipant(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);
            int count = 0;
            foreach (ParticipantComponent patient in appointment.Participant)
            {
                               
                string resource = patient.Actor.Url.ToString();
                if (resource.Contains("Patient"))
                {   
                 
                    patient.Actor.Reference.Remove(count);
                    break;
                }
                count++;
           
            }

            appointment.Participant.RemoveAt(count);
            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);

        }

        [Then(@"I remove the location participant from the appointment called ""(.*)""")]
        public void removeLocationParticipant(string appointmentName)
        {
             Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);
            int count = 0;
            foreach (ParticipantComponent patient in appointment.Participant)
            {
                             
                string resource = patient.Actor.Url.ToString();
                if (resource.Contains("Location"))
                {
                    patient.Actor.Reference.Remove(count);
                    break;
                }
                count++;
           
            }
            appointment.Participant.RemoveAt(count);
            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }

        [Then(@"I remove the practitioner participant from the appointment called ""(.*)""")]
        public void removePractitonerParticipant(string appointmentName)
        {
        
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);
            int count = 0;
            foreach (ParticipantComponent patient in appointment.Participant)
            {

                string resource = patient.Actor.Url.ToString();
                if (resource.Contains("Practitioner"))
                {

                    patient.Actor.Reference.Remove(count);
                    break;
                }
                count++;
  
            }
            appointment.Participant.RemoveAt(count);
            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }

        [Then(@"I book the appointment called ""(.*)""")]
        public void bookAppointment(string appointmentName) {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpSteps.bookAppointment("urn:nhs:names:services:gpconnect:fhir:rest:create:appointment", "/Appointment", appointment);
        }

    }
}
