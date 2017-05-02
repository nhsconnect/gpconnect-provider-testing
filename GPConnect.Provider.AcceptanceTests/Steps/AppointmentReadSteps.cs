using System.Collections.Generic;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using Hl7.Fhir.Model;
using Shouldly;
using TechTalk.SpecFlow;
using static Hl7.Fhir.Model.Appointment;
using static Hl7.Fhir.Model.Bundle;

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

        [Given(@"I find appointments for patient ""([^""]*)"" at organization ""([^""]*)"" and save the bundle of appointment resources to ""([^""]*)""")]
        public void createFindOrganisationAndAssignSchedule(string patient, string organizaitonName, string bundleOfPatientAppointmentskey) {
            // Search For Patient appointments
            Given($@"I search for patient ""{patient}"" appointments and save the returned bundle of appointment resources against key ""{bundleOfPatientAppointmentskey}""");
            Bundle patientAppointmentsBundle = (Bundle)HttpContext.StoredFhirResources[bundleOfPatientAppointmentskey];

            Given($@"I perform the getSchedule operation for organization ""{organizaitonName}"" and store the returned bundle resources against key ""getScheduleResponseBundle""");

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
                    When($@"I book an appointment for patient ""{patient}"" on the provider system using a slot from the getSchedule response bundle stored against key ""getScheduleResponseBundle""");
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

        [When(@"I book an appointment for patient ""([^""]*)"" on the provider system using a slot from the getSchedule response bundle stored against key ""([^""]*)""")]
        public void IBookAnAppointmentForPatientOnTheProviderSystemUsingASlotFromTheGetScheduleResponseBundleStoredAgainstKey(string patientName, string getScheduleBundleKey)
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
            foreach (var practitionerReferenceExtension in schedule.Extension) {
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

            //Book the appointment
            HttpSteps.bookAppointment("urn:nhs:names:services:gpconnect:fhir:rest:create:appointment", "/Appointment", appointment);
        }
        
        [When(@"I perform an appointment read for the first appointment saved in the list of resources stored against key ""([^""]*)""")]
        public void IPerformTheGetScheduleOperationForOrganizationAndStoreTheReturnedBundleResourceAgainstKey(string bundleOfPatientAppointmentsKey)
        {
            Bundle patientAppointmentBundel = (Bundle)HttpContext.StoredFhirResources[bundleOfPatientAppointmentsKey];
            When($@"I perform an appointment read for the appointment with logical id ""{patientAppointmentBundel.Entry[0].Resource.Id}"""); // Get the Id of the first appointment
        }

        [When(@"I perform an appointment read for the appointment with logical id ""([^""]*)""")]
        public void IPerformAnAppointmentReadForTheAppointment(string appointmentLogicalId)
        {
            When($@"I make a GET request to ""/Appointment/{appointmentLogicalId}""");
        }

        [Then(@"the response should be an Appointment resource")]
        public void theResponseShouldBeAnAppointmentResource()
        {
            FhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.Appointment);
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
        }
        
    }
}
