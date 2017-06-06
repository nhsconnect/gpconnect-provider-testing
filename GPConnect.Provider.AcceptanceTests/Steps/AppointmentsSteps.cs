using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Extensions;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Model;
using NUnit.Framework;
using System;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using static Hl7.Fhir.Model.Bundle;
using System.Globalization;
using static Hl7.Fhir.Model.Appointment;
using Hl7.Fhir.Serialization;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public class AppointmentsSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private readonly HttpSteps HttpSteps;
        private readonly HttpContext HttpContext;
        private readonly BookAppointmentSteps BookAppointmentSteps;
        
        public AppointmentsSteps(FhirContext fhirContext, HttpSteps httpSteps, HttpContext httpContext, BookAppointmentSteps bookAppointmentSteps)
        {
            FhirContext = fhirContext;
            HttpSteps = httpSteps;
            HttpContext = httpContext;
            BookAppointmentSteps = bookAppointmentSteps;
    }

        [When(@"I search for ""([^""]*)"" from the list of patients and make a get request for their appointments")]
        public void searchAndGetAppointmentsFromPatientListData(string patient)
        {

            Patient patientResource = (Patient)HttpContext.StoredFhirResources[patient];
            string id = patientResource.Id.ToString();
            var url = "/Patient/"+ id+ "/Appointment";
            When($@"I make a GET request to ""{url}""");
        }

        [When(@"I search for ""([^""]*)"" and make a get request for their appointments")]
        public void searchAndGetAppointments(string patient)
        {
            Resource patient1 = (Patient)HttpContext.StoredFhirResources["AppointmentReadPatientResource"];
            string id = patient1.Id.ToString();
            var url = "/Patient/" + id + "/Appointment";
            When($@"I make a GET request to ""{url}""");
        }

        [When(@"I search for patient ""([^""]*)"" and search for the most recently booked appointment using the stored startDate from the last booked appointment as a search parameter")]
        public void ISearchForPatientAndSearchForTheMostRecentlyBookedAppointmentUsingTheStoredStartDateFromTheLastBookedAppointmentAsASearchParameter(string patient)
        {
            string time = HttpContext.StoredDate["slotStartDate"];
            Resource patient1 = (Patient)HttpContext.StoredFhirResources["AppointmentReadPatientResource"];
            string id = patient1.Id.ToString();
            var url = "/Patient/" + id + "/Appointment?start=" + time + "";
            When($@"I make a GET request to ""{url}""");
        }

        [When(@"I search for ""([^""]*)"" and make a get request for their appointments with the date ""([^""]*)""")]
        public void searchAndGetAppointmentsWithCustomStartDate(string patient, string startDate)
        {
            string time = HttpContext.StoredDate[startDate];
            Resource patient1 = (Patient)HttpContext.StoredFhirResources["AppointmentReadPatientResource"];
            string id = patient1.Id.ToString();
            var url = "/Patient/" + id + "/Appointment?start=" + time + "";
            When($@"I make a GET request to ""{url}""");
        }

        [When(@"I search for ""([^""]*)"" and make a get request for their appointments searching with the date ""([^""]*)""")]
        public void searchAndGetAppointmentsWithCustomStartDateSearcg(string patient, string startDate)
        {
           
            Resource patient1 = (Patient)HttpContext.StoredFhirResources["AppointmentReadPatientResource"];
            string id = patient1.Id.ToString();
            var url = "/Patient/" + id + "/Appointment?start=" + startDate + "";
            When($@"I make a GET request to ""{url}""");
        }
        [When(@"I search for ""([^""]*)"" and make a get request for their appointments with the saved slot start date ""([^""]*)"" and prefix ""([^""]*)""")]
        public void searchAndGetAppointmentsWithTheSavedSlotStartDateCustomStartDateandPrefix(string patient, string startDate, string prefix)
        {
            string time = HttpContext.StoredDate[startDate];
            Resource patient1 = (Patient)HttpContext.StoredFhirResources["AppointmentReadPatientResource"];
            string id = patient1.Id.ToString();
            var url = "/Patient/" + id + "/Appointment?start=" + prefix + time + "";
            When($@"I make a GET request to ""{url}""");
        }
       
        [When(@"I search for ""([^""]*)"" and make a get request for their appointments with the date ""([^""]*)"" and prefix ""([^""]*)""")]
        public void searchAndGetAppointmentsWithCustomStartDateandPrefix(string patient, string startDate, string prefix)
        {
            Resource patient1 = (Patient)HttpContext.StoredFhirResources["AppointmentReadPatientResource"];
            string id = patient1.Id.ToString();
            var url = "/Patient/" + id + "/Appointment?start="+prefix+startDate+"";
            When($@"I make a GET request to ""{url}""");
        }

        [When(@"I search for ""([^""]*)"" and make a get request for their appointments with the start range date ""([^""]*)"" with prefix ""([^""]*)"" and end range date ""([^""]*)"" with prefix ""([^""]*)""")]
        public void WhenISearchForAndMakeAGetRequestForTheirAppointmentsWithTheStartRangeDateWithPrefixAndEndDateWithPrefix(string patient, string startDate, string prefixStart, string endDate, string prefixEnd)
        {
            Resource patient1 = (Patient)HttpContext.StoredFhirResources["AppointmentReadPatientResource"];
            string id = patient1.Id.ToString();
            var url = "/Patient/" + id + "/Appointment?start=" + prefixStart + startDate + "&start="+prefixEnd+endDate+"";
            When($@"I make a GET request to ""{url}""");
        }



        [Given(@"I get the slots avaliable slots for organization ""([^""]*)"" for the next 3 days")]
        public void BookSlots(string appointmentCode)
        {
            DateTime currentDateTime = DateTime.Now;
            Period period = new Period(new FhirDateTime(currentDateTime), new FhirDateTime(currentDateTime.AddDays(3)));
            FhirContext.FhirRequestParameters.Add("timePeriod", period);
            Organization organization = (Organization)HttpContext.StoredFhirResources[appointmentCode];
            HttpSteps.RestRequest(Method.POST, "/Organization/" + organization.Id + "/$gpc.getschedule", FhirSerializer.SerializeToJson(FhirContext.FhirRequestParameters));

            HttpContext.ResponseContentType.ShouldStartWith(FhirConst.ContentTypes.kJsonFhir);
            HttpContext.ResponseJSON = JObject.Parse(HttpContext.ResponseBody);
            FhirJsonParser fhirJsonParser = new FhirJsonParser();
            FhirContext.FhirResponseResource = fhirJsonParser.Parse<Resource>(HttpContext.ResponseBody);



            List<Resource> slots = new List<Resource>();



            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Organization))
                {
                    HttpContext.StoredFhirResources.Add("Organization", (Organization)entry.Resource);
                }
                if (entry.Resource.ResourceType.Equals(ResourceType.Location))
                {
                    HttpContext.StoredFhirResources.Add("Location", (Location)entry.Resource);
                }
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    HttpContext.StoredFhirResources.Remove("Practitioner");
                    HttpContext.StoredFhirResources.Add("Practitioner", (Practitioner)entry.Resource);
                }

                if (entry.Resource.ResourceType.Equals(ResourceType.Slot))
                {
                    string id = ((Slot)entry.Resource).Id.ToString();
                    slots.Add((Slot)entry.Resource);
                }
            }
            String here = slots.Count.ToString();
            HttpContext.StoredSlots.Add("Slot", slots);

        }

        [Given(@"I search for an appointments for patient ""([^""]*)"" on the provider system and save the first response to ""([^""]*)""")]
        public void GivenISearchForAnAppointmentOnTheProviderSystemAndSaveTheFirstResponseTo(int id, string storeKey)
        {
            var relativeUrl = "/Patient/" + id + "/Appointment";
            var returnedResourceBundle = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments", relativeUrl);
            returnedResourceBundle.GetType().ShouldBe(typeof(Bundle));
            ((Bundle)returnedResourceBundle).Entry.Count.ShouldBeGreaterThan(0);
            var returnedFirstResource = (Appointment)((Bundle)returnedResourceBundle).Entry[0].Resource;
            string text = returnedFirstResource.ToString();
            returnedFirstResource.GetType().ShouldBe(typeof(Appointment));
            if (HttpContext.StoredFhirResources.ContainsKey(storeKey)) HttpContext.StoredFhirResources.Remove(storeKey);
            HttpContext.StoredFhirResources.Add(storeKey, returnedFirstResource);
        }

        [When(@"I book an appointment for patient ""([^""]*)"" on the provider system with the schedule name ""([^""]*)"" with interaction id ""([^""]*)""")]
        public void bookAppointmentForUser(string patientRef, string scheduleName, string interactionID)
        {
            bookAppointmentForUserWithUrl(patientRef, scheduleName, interactionID, "/Appointment");
        }
        
        [When(@"I book an appointment for patient ""([^""]*)"" on the provider system with the schedule name ""([^""]*)"" with interaction id ""([^""]*)"" via url ""([^""]*)""")]
        public void bookAppointmentForUserWithUrl(string patientRef, string scheduleName, string interactionID, string url)
        {
            Bundle patientBundle = (Bundle)HttpContext.StoredFhirResources[scheduleName];
            Patient patientResource = (Patient)HttpContext.StoredFhirResources[patientRef];

            List<Slot> slotList = new List<Slot>();
            Dictionary<string, Practitioner> practitionerDictionary = new Dictionary<string, Practitioner>();
            Dictionary<string, Location> locationDictionary = new Dictionary<string, Location>();
            Dictionary<string, Schedule> scheduleDictionary = new Dictionary<string, Schedule>();

            // Group together resources
            foreach (EntryComponent entry in patientBundle.Entry)
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
            foreach (EntryComponent entry in patientBundle.Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Slot) && string.Equals(((Slot)entry.Resource).Id, firstSlot.Id))
                {
                    entryToRemove = entry;
                    break;
                }
            }
            patientBundle.Entry.Remove(entryToRemove);

            BookAppointmentSteps.bookAppointment(interactionID, url, FhirSerializer.SerializeToJson(appointment));
        }

        [When(@"I book an appointment for patient ""([^""]*)"" on the provider system with the schedule name ""([^""]*)"" with interaction id ""([^""]*)"" without header clean up")]
        public void IBookAnAppointmentForPatientOnTheProviderSystemWithTheScheduleNameWithInteractionIdWithoutHeaderCleanUp(string patientRef, string scheduleName, string interactionID)
        {

            Bundle patientBundle = (Bundle)HttpContext.StoredFhirResources[scheduleName];
            List<Slot> slotList = new List<Slot>();
            Dictionary<string, Practitioner> practitionerDictionary = new Dictionary<string, Practitioner>();
            Dictionary<string, Location> locationDictionary = new Dictionary<string, Location>();
            Dictionary<string, Schedule> scheduleDictionary = new Dictionary<string, Schedule>();

            // Group together resources
            foreach (EntryComponent entry in patientBundle.Entry)
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
            patientReference.Reference = "Patient/13";
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
            foreach (EntryComponent entry in patientBundle.Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Slot) && string.Equals(((Slot)entry.Resource).Id, firstSlot.Id))
                {
                    entryToRemove = entry;
                    break;
                }
            }
            patientBundle.Entry.Remove(entryToRemove);

            //Book the appointment
            BookAppointmentSteps.bookWithoutCleanUpAppointment(interactionID, "/Appointment", appointment);
        }


        [When(@"I book an invalid appointment for patient ""([^""]*)"" on the provider system with the schedule name ""([^""]*)"" with interaction id ""([^""]*)""")]
        public void WhenIBookAnAppointmentForPatientStringOnTheProviderSystemWithTheScheduleNameStringWithInteractionIdString(string patientRef, string scheduleName, string interactionID)
        {

            Bundle patientBundle = (Bundle)HttpContext.StoredFhirResources[scheduleName];
            List<Slot> slotList = new List<Slot>();
            Dictionary<string, Practitioner> practitionerDictionary = new Dictionary<string, Practitioner>();
            Dictionary<string, Location> locationDictionary = new Dictionary<string, Location>();
            Dictionary<string, Schedule> scheduleDictionary = new Dictionary<string, Schedule>();

            // Group together resources
            foreach (EntryComponent entry in patientBundle.Entry)
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
            patientReference.Reference = "Patient/1";
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
            foreach (EntryComponent entry in patientBundle.Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Slot) && string.Equals(((Slot)entry.Resource).Id, firstSlot.Id))
                {
                    entryToRemove = entry;
                    break;
                }
            }
            patientBundle.Entry.Remove(entryToRemove);

            //Book the appointment
            BookAppointmentSteps.bookAppointmentValidateSuccesfulResponseAndParseResponse(interactionID, "/Appointment", appointment);
        }

        [Then(@"there are zero appointment resources")]
        public void checkForEmptyAppointmentsBundle()
        {
            int count = 0;
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    count++;
                }
            }
            count.ShouldBe<int>(0);
        }

        

        [Then(@"there is one appointment resource")]
        public void checkForOneAppointmentsBundle()
        {
            int count = 0;
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    count++;
                }
            }
            count.ShouldBe<int>(1);
        }

        [Then(@"there are multiple appointment resources")]
        public void checkForMultipleAppointmentsBundle()
        {
            int appointmentCount = 0;
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {

                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    appointmentCount++;
                    Appointment appointment = (Appointment)entry.Resource;
                }
            }
            appointmentCount.ShouldBeGreaterThan<int>(1);
        }

        [Then(@"the response bundle should contain atleast ""([^""]*)"" appointment")]
        public void TheResponseBundleShouldContainAtleastAppointments (int minNumberOfAppointments)
        {
            int appointmentCount = 0;
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {

                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    appointmentCount++;
                    Appointment appointment = (Appointment)entry.Resource;
                }
            }
            appointmentCount.ShouldBeGreaterThanOrEqualTo(minNumberOfAppointments);
        }
        
        [Then(@"the appointment resource should contain a single status element")]
        public void appointmentMustContainStatusElement()
        {

            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            appointment.Status.ShouldNotBeNull();

        }
        [Then(@"the appointment resource should contain a single start element")]
        public void appointmentMustContainStartElement()
        {

            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            appointment.Start.ShouldNotBeNull();


        }
        [Then(@"the appointment resource should contain a single end element")]
        public void appointmentMustContainEndElement()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            appointment.End.ShouldNotBeNull();
        }

        [Then(@"the appointment resource should contain at least one slot reference")]
        public void appointmentMustContainSlotReference()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            appointment.Slot.ShouldNotBeNull();

        }
        [Then(@"the appointment resource should contain at least one participant")]
        public void appointmentMustContainParticipant()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            appointment.Participant.ShouldNotBeNull();


        }

        [Then(@"appointment status should have a valid value")]
        public void statusShouldHaveValidValue()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    appointment.Status.ShouldNotBeNull();

                }
            }
        }

        [Then(@"the bundle response should contain a participant element")]
        public void bundleResponseShouldContainParticipantElement()
        {
            int count = 0;
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    appointment.Participant.ShouldNotBeNull();
                    count++;

                }
            }
            count.ShouldBe<int>(1);
        }

        [Then(@"the appointment status element should be valid")]
        public void appointmentStatusElementShouldBeValid()
        {
            List<String> validAppointmentStatus = new List<string>();
            validAppointmentStatus.Add("Proposed");
            validAppointmentStatus.Add("Pending");
            validAppointmentStatus.Add("Booked");
            validAppointmentStatus.Add("Arrived");
            validAppointmentStatus.Add("Fulfilled");
            validAppointmentStatus.Add("Cancelled");
            validAppointmentStatus.Add("Noshow");

            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    string status = appointment.Status.ToString();
                    validAppointmentStatus.ShouldContain(status);

                }
            }

        }

        [Then(@"the participant element should contain a single status element")]
        public void participantElementShouldContainASingleStatusElement()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                }
            }
        }

        [Then(@"if appointment contains the resource coding READ V2 element the fields should match the fixed values of the specification")]
        public void reasonCodingSnomedCT()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    if (appointment.Type == null)
                    {
                      
                    }
                    else
                    {
                        if (appointment.Reason.Coding != null)
                        {
                            int codingCount = 0;
                            foreach (Coding coding in appointment.Reason.Coding)
                            {
                                codingCount++;
                                coding.System.ShouldBe("http://read.info/readv2");
                                coding.Code.ShouldBe("425173008");
                                coding.Display.ShouldBe("Default Appointment Type");
                            }
                            codingCount.ShouldBeLessThanOrEqualTo(1);
                        }

                        if (appointment.Reason.Text != null)
                        {
                            appointment.Reason.Text.ShouldBe("Default Appointment Type");
                        }
                    }
                }
            }
        }
        [Then(@"if appointment contains the resource coding SREAD CTV3 element the fields should match the fixed values of the specification")]
        public void reasonCodingReadV2()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;

                 
                        if (appointment.Reason.Coding != null)
                        {
                            int codingCount = 0;
                            foreach (Coding coding in appointment.Reason.Coding)
                            {
                                codingCount++;
                                coding.System.ShouldBe("http://read.info/ctv3");
                                coding.Code.ShouldBe("425173008");
                                coding.Display.ShouldBe("Default Appointment Type");
                            }
                            codingCount.ShouldBeLessThanOrEqualTo(1);
                        }

                        if (appointment.Reason.Text != null)
                        {
                            appointment.Reason.Text.ShouldBe("Default Appointment Type");
                        }
                 }
            }
        }

        [Then(@"if appointment contains the resource coding SNOMED CT element the fields should match the fixed values of the specification")]
        public void reasonCodingReadCTV3()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;

                    if (appointment.Reason.Coding == null)
                    {
                  
                    }
                    else
                    {
                        if (appointment.Reason.Coding != null)
                        {
                            int codingCount = 0;
                            foreach (Coding coding in appointment.Reason.Coding)
                            {
                                codingCount++;
                                coding.System.ShouldBe("http://snomed.info/sct");
                                coding.Code.ShouldBe("1");
                                coding.Display.ShouldBe("Default Appointment Type");
                            }
                            codingCount.ShouldBeLessThanOrEqualTo(1);
                        }

                        if (appointment.Reason.Text != null)
                        {
                            appointment.Reason.Text.ShouldBe("Default Appointment Type");
                        }
                    }
                }
            }
        }

        [Then(@"if the appointment resource contains an identifier it contains a valid system and value")]
        public void appointmentContainsValidIdentifierWithSystemAndValue()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    foreach (var identifier in appointment.Identifier)
                    {
                        identifier.Value.ShouldNotBeNullOrEmpty();
                    }
                }

            }
        }

        [Then(@"if the the start date must be before the end date")]
        public void startDateBeforeEndDate()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    DateTimeOffset? start = appointment.Start;
                    DateTimeOffset? end = appointment.End;

                    if (start > end)
                    {
                        { Assert.Fail(); }
                    }
                }
            }
        }


        [Then(@"the appointment response resource contains a status with a valid value")]
        public void ThenTheAppointmentResourceShouldContainAStatus()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            appointment.Status.ShouldNotBeNull();
            string statusValue = appointment.Status.Value.ToString();
            if (statusValue != "Booked" && statusValue != "Pending" && statusValue != "Arrived" && statusValue != "Fulfilled" && statusValue != "Cancelled" && statusValue != "Noshow")
            {
                Assert.Fail("Appointment status value is invalid : " + statusValue);
            }
        }

        [Then(@"the appointment response resource contains an start date")]
        public void ThenTheAppointmentResourceShouldContainAStartDate()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            appointment.Start.ShouldNotBeNull();
        }


        [Then(@"the appointment response resource contains an end date")]
        public void ThenTheAppointmentResourceShouldContainAEndDate()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            appointment.End.ShouldNotBeNull();
        }

        [Then(@"the appointment response resource contains a slot reference")]
        public void ThenTheAppointmentResourceShouldContainASlotReference()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            appointment.Slot.ShouldNotBeNull();
            appointment.Slot.Count.ShouldBeGreaterThanOrEqualTo(1);
            foreach (var slotReference in appointment.Slot) {
                slotReference.Reference.ShouldStartWith("Slot/");
            }
        }

        [Then(@"if appointment is present the single or multiple participant must contain a type or actor")]
        public void ThenTheAppointmentResourceShouldContainAParticipantWithATypeOrActor()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    foreach (ParticipantComponent participant in appointment.Participant)
                    {
                        var actor = participant.Actor;
                        var type = participant.Type;

                        if (null == actor && null == type)
                        {
                            Assert.Fail("Actor and type are null");
                        }
                        if (null != type)
                        {
                            int codableConceptCount = 0;
                            foreach (var typeCodableConcept in type)
                            {
                                codableConceptCount++;
                                int codingCount = 0;
                                foreach (var coding in typeCodableConcept.Coding)
                                {
                                    coding.System.ShouldBe("http://hl7.org/fhir/ValueSet/encounter-participant-type");
                                    string[] codes = new string[12] { "translator", "emergency", "ADM", "ATND", "CALLBCK", "CON", "DIS", "ESC", "REF", "SPRF", "PPRF", "PART" };
                                    string[] codeDisplays = new string[12] { "Translator", "Emergency", "admitter", "attender", "callback contact", "consultant", "discharger", "escort", "referrer", "secondary performer", "primary performer", "Participation" };
                                    coding.Code.ShouldBeOneOf(codes);
                                    coding.Display.ShouldBeOneOf(codeDisplays);
                                    for (int i = 0; i < codes.Length; i++)
                                    {
                                        if (string.Equals(coding.Code, codes[i]))
                                        {
                                            coding.Display.ShouldBe(codeDisplays[i], "The participant type code does not match the display element");
                                        }
                                    }
                                    codingCount++;
                                }
                                codingCount.ShouldBeLessThanOrEqualTo(1, "There should be a maximum of 1 participant type coding element for each participant");
                            }
                            codableConceptCount.ShouldBeLessThanOrEqualTo(1, "The participant type element may only contain one codable concept.");
                        }

                        if (actor != null && actor.Reference != null)
                        {
                            actor.Reference.ShouldNotBeEmpty();
                            if (!actor.Reference.StartsWith("Patient/") &&
                                !actor.Reference.StartsWith("Practitioner/") &&
                                !actor.Reference.StartsWith("Location/"))
                            {
                                Assert.Fail("The actor reference should be a Patient, Practitioner or Location");
                            }
                        }
                    }
                }
            }
        }

        [Then(@"the appointment response contains a type with a valid system code and display")]
        public void ThenTheAppointmentResourceContainsAType()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            appointment.Type.ShouldNotBeNull();
            foreach (Coding coding in appointment.Type.Coding)
            {
                coding.System.ShouldBe("http://hl7.org/fhir/ValueSet/c80-practice-codes");
                coding.Code.ShouldNotBeNull();
            }

        }

        [Then(@"if the appointment participant contains a type is should have a valid system and code")]
        public void AppointmentParticipantValisType()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    foreach (ParticipantComponent part in appointment.Participant)
                    {

                        foreach (CodeableConcept codeConcept in part.Type)
                        {
                            foreach (Coding code in codeConcept.Coding)

                            {
                                code.System.ShouldBe("http://hl7.org/fhir/ValueSet/encounter-participant-type");
                                code.Code.ShouldNotBeNull();
                                code.Display.ShouldNotBeNull();
                            }
                        }
                    }
                }
            }
        }


        [Then(@"the appointment type should contain a valid system code and display")]
        public void ThenTheAppointmentResourceContainsTypeWithValidSystemCodeAndType()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            appointment.Identifier.ShouldNotBeNull();
            String id = appointment.Id.ToString();
            foreach (Identifier identifier in appointment.Identifier)
            {
                identifier.System.ShouldNotBeNull();
                identifier.System.ShouldBe("http://fhir.nhs.net/Id/gpconnect-appointment-identifier");
                identifier.Value.ShouldNotBeNull();
                identifier.Value.ShouldBe(id);
            }
        }

        [Then(@"if the appointment resource contains a priority the value is valid")]
        public void ThenTheAppointmentResourceContainsPriorityAndTheValueIsValid()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;

            if (null != appointment && (appointment.Priority < 0 || appointment.Priority > 9))
            {
                Assert.Fail("Invalid priority value: " + appointment.Priority);
            }
        }
        //Need to check the validity of the reference but currently no GET method
        [Then(@"the slot reference is present and valid")]
        public void checkingTheSlotReferenceIsValid()
        {

            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            foreach (ResourceReference slot in appointment.Slot)
            {
                slot.Reference.ShouldNotBeNull();
            }


        }

        [Then(@"all appointments must have an start element which is populated with a valid date")]
        public void appointmentPopulatedWithAValidStartDate()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    appointment.Start.ShouldNotBeNull();
                    appointment.Start.ShouldBeOfType<Instant>();

                }
            }
        }

        [Then(@"all appointments must have an end element which is populated vith a valid date")]
        public void appointmentPopulatedWithAValidEndDate()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    appointment.End.ShouldNotBeNull();
                    appointment.End.ShouldBeOfType<Instant>();

                }
            }
        }

        [Then(@"the practitioner resource returned in the appointments bundle is present")]
        public void actorPractitionerResourceValid()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    int countPractitioner = 0;
                    foreach (ParticipantComponent participant in appointment.Participant)
                    {
                        if (participant.Actor != null && participant.Actor.Reference != null)
                        {

                            string actor = participant.Actor.Reference.ToString();

                            if (actor.Contains("Practitioner"))
                            {
                                var practitioner = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner", actor);
                                practitioner.ShouldNotBeNull();
                                countPractitioner++;
                            } }
                    }
                    countPractitioner.ShouldBeGreaterThanOrEqualTo(1);
                }
            }
        }

        [Then(@"the location resource returned in the appointments bundle is present")]
        public void actorLocationResourceValid()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    int countLocation = 0; 
                    Appointment appointment = (Appointment)entry.Resource;
                    foreach (ParticipantComponent participant in appointment.Participant)
                    {
                        if (participant.Actor != null && participant.Actor.Reference != null)
                        {

                            string actor = participant.Actor.Reference.ToString();

                            if (actor.Contains("Location"))
                            {
                                var location = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:location", actor);
                                location.ShouldNotBeNull();
                                countLocation++;
                            }

                        }
                    }
                    countLocation.ShouldBe(1);
                }

            }
        }


       [Then(@"the patient resource returned in the appointments bundle is present")]
        public void actorPatientResourceValid()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    int countPatient = 0;
                    foreach (ParticipantComponent participant in appointment.Participant)
                    {
                        if (participant.Actor != null && participant.Actor.Reference != null)
                        {
                            string actor = participant.Actor.Reference.ToString();

                            if (actor.Contains("Patient"))
                            {
                                countPatient++;
                                var patient = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:patient", actor);
                                patient.ShouldNotBeNull();
                            }

                        }
                    }
                    countPatient.ShouldBeGreaterThanOrEqualTo(1);
                }
            }
        }
        
        [Then(@"patient ""(.*)"" should have ""(.*)"" appointments")]
        public void checkPatientHasTheCorrectAmountOfResources(int id, int numberOfAppointments)
        {
            int appointmentCount = 0;
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    appointmentCount++;
                    Appointment appointment = (Appointment)entry.Resource;
                }
            }
            appointmentCount.ShouldBe<int>(numberOfAppointments);
        }

    }
}



    

