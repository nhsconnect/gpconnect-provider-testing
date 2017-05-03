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
using static Hl7.Fhir.Model.Bundle;

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



        [Then(@"I create an appointment for patient ""(.*)"" called ""(.*)"" from schedule ""(.*)""")]
        public void GivenISearchForAnAppointmentOnTheProviderSystemAndBookAppointment(string patientName, string appointmentName, string getScheduleBundleKey)
        {
            Given($@"I perform a patient search for patient ""{patientName}"" and store the first returned resources against key ""AppointmentReadPatientResource""");
            Patient patientResource = (Patient)HttpContext.StoredFhirResources["AppointmentReadPatientResource"];
            Bundle getScheduleResponseBundle = (Bundle)HttpContext.StoredFhirResources[getScheduleBundleKey];

            List<Slot> slotList = new List<Slot>();
            Dictionary<string, Practitioner> practitionerDictionary = new Dictionary<string, Practitioner>();
            Dictionary<string, Location> locationDictionary = new Dictionary<string, Location>();
            Dictionary<string, Schedule> scheduleDictionary = new Dictionary<string, Schedule>();

            // Group together resources
            foreach (EntryComponent entry in getScheduleResponseBundle.Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Slot))
                {
                    slotList.Add((Slot)entry.Resource);
                }
                else if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    if (!practitionerDictionary.ContainsKey(entry.FullUrl))
                    {
                        practitionerDictionary.Add(entry.FullUrl, (Practitioner)entry.Resource);
                    }
                }
                else if (entry.Resource.ResourceType.Equals(ResourceType.Location))
                {
                    if (!locationDictionary.ContainsKey(entry.FullUrl))
                    {
                        locationDictionary.Add(entry.FullUrl, (Location)entry.Resource);
                    }
                }
                else if (entry.Resource.ResourceType.Equals(ResourceType.Schedule))
                {
                    if (!scheduleDictionary.ContainsKey(entry.FullUrl))
                    {
                        scheduleDictionary.Add(entry.FullUrl, (Schedule)entry.Resource);
                    }
                }
            }

            // Select first slot
            Slot firstSlot = slotList[0];

            string scheduleReference = firstSlot.Schedule.Reference;
            Schedule schedule = null;
            scheduleDictionary.TryGetValue(scheduleReference, out schedule);

            string locationReferenceForSelectedSlot = schedule.Actor.Reference;

            List<string> practitionerReferenceForSelectedSlot = new List<string>();
            foreach (var practitionerReferenceExtension in schedule.Extension)
            {
                practitionerReferenceForSelectedSlot.Add(((ResourceReference)practitionerReferenceExtension.Value).Reference);
            }

            // Create Appointment
            Appointment appointment = new Appointment();
            appointment.Status = AppointmentStatus.Booked;

            // Appointment Patient Resource
            ParticipantComponent patient = new ParticipantComponent();
            ResourceReference patientReference = new ResourceReference();
            patientReference.Reference = "Patient/" + patientResource.Id;
            patient.Actor = patientReference;
            patient.Status = ParticipationStatus.Accepted;
            appointment.Participant.Add(patient);

            // Appointment Practitioner Resource
            foreach (var practitionerSlotReference in practitionerReferenceForSelectedSlot)
            {
                ParticipantComponent practitioner = new ParticipantComponent();
                ResourceReference practitionerReference = new ResourceReference();
                practitionerReference.Reference = practitionerSlotReference;
                practitioner.Actor = practitionerReference;
                practitioner.Status = ParticipationStatus.Accepted;
                appointment.Participant.Add(practitioner);
            }

            // Appointment Location Resource
            ParticipantComponent location = new ParticipantComponent();
            ResourceReference locationReference = new ResourceReference();
            locationReference.Reference = locationReferenceForSelectedSlot;
            location.Actor = locationReference;
            location.Status = ParticipationStatus.Accepted;
            appointment.Participant.Add(location);

            // Appointment Slot Resource
            ResourceReference slot = new ResourceReference();
            slot.Reference = "Slot/" + firstSlot.Id;
            appointment.Slot.Add(slot);
            appointment.Start = firstSlot.Start;
            appointment.End = firstSlot.End;

            // Now we have used the slot remove from it from the getScheduleBundle so it is not used to book other appointments same getSchedule is used
            EntryComponent entryToRemove = null;
            foreach (EntryComponent entry in getScheduleResponseBundle.Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Slot) && string.Equals(((Slot)entry.Resource).Id, firstSlot.Id))
                {
                    entryToRemove = entry;
                    break;
                }
            }
            getScheduleResponseBundle.Entry.Remove(entryToRemove);
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
