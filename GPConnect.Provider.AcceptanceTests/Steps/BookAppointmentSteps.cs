using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TechTalk.SpecFlow;
using static Hl7.Fhir.Model.Appointment;
using static Hl7.Fhir.Model.Bundle;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public class BookAppointmentSteps : TechTalk.SpecFlow.Steps
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
        
        [Given(@"I create an appointment for patient ""(.*)"" called ""(.*)"" from schedule ""(.*)""")]
        public void GivenISearchForAnAppointmentOnTheProviderSystemAndBookAppointment(string patientKey, string appointmentName, string getScheduleBundleKey)
        {
            Patient patientResource = (Patient)HttpContext.StoredFhirResources[patientKey];
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

            //Elements of the appointment
            CodeableConcept reason = new CodeableConcept();
            Coding coding = new Coding();
            List<Extension> extensionList = new List<Extension>();
            Extension extension = new Extension();
            Identifier identifier = new Identifier();
            List<Identifier> identifiers = new List<Identifier>();
            AppointmentStatus status = AppointmentStatus.Booked;
            int priority = new int();

            switch (appointmentName)
            {
                case "patient8":
                    //Define category
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1", "ONL", "Online"));
                    //Define Identifier
                    identifier.System = "http://fhir.nhs.net/Id/gpconnect-appointment-identifier";
                    identifier.Value = "898976578";
                    identifiers.Add(identifier);
                    //Define Reason
                    coding = buildReasonForAppointment("http://snomed.info/sct", "", "");
                    reason.Coding.Add(coding);
                    //Define Priority
                    priority = 1;
                    //Define participant
                    break;

                case "patient9":
                    //Define category
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1", "ONL", "Online"));
                    //Define Identifier
                    identifier.System = "http://fhir.nhs.net/Id/gpconnect-appointment-identifier";
                    identifier.Value = "898976579";
                    identifiers.Add(identifier);
                    //Define Reason
                    coding = buildReasonForAppointment("http://read.info/readv2", "", "");
                    reason.Coding.Add(coding);
                    //Define Priority
                    priority = 7;
                    //Define participant
                    break;

                case "patient10":
                    //Define category
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1", "CLI", "Clinical"));
                    //Define Identifier
                    identifier.System = "http://fhir.nhs.net/Id/gpconnect-appointment-identifier";
                    identifier.Value = "898976580";
                    identifiers.Add(identifier);
                    //Define Reason
                    coding = buildReasonForAppointment("http://read.info/ctv3", "", "");
                    reason.Coding.Add(coding);
                    //Define Priority
                    priority = 9;
                    //Define participant
                    break;

                case "patient11":
                    //Define category
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1", "", "Double booked"));
                    //Define Identifier
                    identifier.System = "http://fhir.nhs.net/Id/gpconnect-appointment-identifier";
                    identifier.Value = "898976581";
                    identifiers.Add(identifier);
                    //Define Reason
                    coding = buildReasonForAppointment("http://snomed.info/sct", "", "");
                    reason.Coding.Add(coding);
                    coding = buildReasonForAppointment("http://read.info/readv2", "", "");
                    reason.Coding.Add(coding);
                    coding = buildReasonForAppointment("http://read.info/ctv3", "", "");
                    reason.Coding.Add(coding);
                    //Define Priority
                    priority = 9;
                     //Define participant
                    break;

                case "patient12":
                    //Define category
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1", "PER", "In person"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1", "ADM", "Administrative"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1", "ONL", "Online"));
                    //Define Identifier
                    identifier.System = "http://fhir.nhs.net/Id/gpconnect-appointment-identifier";
                    identifier.Value = "898976582";
                    identifiers.Add(identifier);
                    //Define Reason
                    coding = buildReasonForAppointment("http://snomed.info/sct", "", "");
                    reason.Coding.Add(coding);
                    coding = buildReasonForAppointment("http://read.info/readv2", "", "");
                    reason.Coding.Add(coding);
                    coding = buildReasonForAppointment("http://read.info/ctv3", "", "");
                    reason.Coding.Add(coding);
                    //Define Priority
                    priority = 2;

                    //Define participant
                    break;

                case "patient13":
                    //Define category
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1", "TEL", "Telephone"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1", "VIR", "Virtual"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1", "PER", "In person"));
                    //Define Identifier
                    identifier.System = "http://fhir.nhs.net/Id/gpconnect-appointment-identifier";
                    identifier.Value = "898976583";
                    identifiers.Add(identifier);
                    //Define Reason
                    coding = buildReasonForAppointment("http://snomed.info/sct", "", "");
                    reason.Coding.Add(coding);
                    coding = buildReasonForAppointment("http://read.info/readv2", "", "");
                    reason.Coding.Add(coding);
                    coding = buildReasonForAppointment("http://read.info/ctv3", "", "");
                    reason.Coding.Add(coding);
                    //Define Priority
                    priority = 3;

                    //Define participant

                    break;

                case "patient14":
                    //Define category
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1", "EMA", "Email"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1", "REM", "Reminder"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1", "TEL", "Telephone"));
                    //Define Identifier
                    identifier.System = "http://fhir.nhs.net/Id/gpconnect-appointment-identifier";
                    identifier.Value = "898976584";
                    identifiers.Add(identifier);
                    //Define Reason
                    coding = buildReasonForAppointment("http://snomed.info/sct", "", "");
                    reason.Coding.Add(coding);
                    coding = buildReasonForAppointment("http://read.info/readv2", "", "");
                    reason.Coding.Add(coding);
                    coding = buildReasonForAppointment("http://read.info/ctv3", "", "");
                    reason.Coding.Add(coding);
                    //Define Priority
                    priority = 4;

                    //Define participant

                    break;

                case "patient15":
                    //Define category
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1", "LET", "Letter"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1", "MSG", "Message"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1", "EMA", "Email"));
                    //Define Identifier
                    identifier.System = "http://fhir.nhs.net/Id/gpconnect-appointment-identifier";
                    identifier.Value = "898976585";
                    identifiers.Add(identifier);
                    //Define Reason
                    coding = buildReasonForAppointment("http://snomed.info/sct", "", "");
                    reason.Coding.Add(coding);
                    coding = buildReasonForAppointment("http://read.info/readv2", "", "");
                    reason.Coding.Add(coding);
                    coding = buildReasonForAppointment("http://read.info/ctv3", "", "");
                    reason.Coding.Add(coding);
                    //Define Priority
                    priority = 5;

                    //Define participant

                    break;

                case "patient16":
                    //Define category
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1", "TEX", "Text"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1", "ADM", "Administrative"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1", "LET", "Letter"));
                    //Define Identifier
                    identifier.System = "http://fhir.nhs.net/Id/gpconnect-appointment-identifier";
                    identifier.Value = "898976586";
                    identifiers.Add(identifier);
                    //Define Reason
                    coding = buildReasonForAppointment("http://snomed.info/sct", "", "");
                    reason.Coding.Add(coding);
                    coding = buildReasonForAppointment("http://read.info/readv2", "", "");
                    reason.Coding.Add(coding);
                    coding = buildReasonForAppointment("http://read.info/ctv3", "", "");
                    reason.Coding.Add(coding);
                    //Define Priority
                    priority = 6;

                    //Define participant

                    break;

                case "patient17":
                    //Define category
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1", "PER", "In person"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1", "CLI", "Clinical"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1", "PER", "In person"));
                    //Define Identifier
                    Identifier multipleIdentifier;
                    for (int i = 0; i < 3; i++)
                    {
                        multipleIdentifier = new Identifier();
                        multipleIdentifier.System = "http://fhir.nhs.net/Id/gpconnect-appointment-identifier";
                        multipleIdentifier.Value = "89897652"+i;
                        identifiers.Add(multipleIdentifier);
                    }
                    //Define Reason
                    coding = buildReasonForAppointment("http://snomed.info/sct", "", "");
                    reason.Coding.Add(coding);
                    coding = buildReasonForAppointment("http://read.info/readv2", "", "");
                    reason.Coding.Add(coding);
                    coding = buildReasonForAppointment("http://read.info/ctv3", "", "");
                    reason.Coding.Add(coding);
                    //Define Priority
                    priority = 7;

                    //Define participant
                 
                    break;
            }

            Appointment appointment = buildAndReturnAppointment(patientResource, practitionerReferenceForSelectedSlot, locationReferenceForSelectedSlot, firstSlot , reason, extension, identifiers, status, priority);
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

        // Will be changing
        [Then(@"I create an appointment for patient ""(.*)"" called ""(.*)"" using a patient resource")]
        public void GivenISearchForAnAppointmentOnTheProviderSystemAndBookAppointmentWithSlotReference2(string patientName, string appointmentName)
        {
            Given($@"I perform a patient search for patient ""{patientName}"" and store the first returned resources against key ""AppointmentReadPatientResource""");
            Patient patientResource = (Patient)HttpContext.StoredFhirResources["AppointmentReadPatientResource"];
            HttpContext.StoredFhirResources.Add(appointmentName, patientResource);
        }
        
        // Will be changing as it is not clear what it is doing
        [Then(@"I create a new bundle to contain an appointment for patient ""(.*)"" called ""(.*)""")]
        public void GivenISearchForAnAppointmentOnTheProviderSystemAndBookAppointmentWithSlotReference3(string patientName, string appointmentName)
        {
            Bundle bundle = new Bundle();
            HttpContext.StoredFhirResources.Add(appointmentName, bundle);
        }
        
        // Aim to remove this
        [Then(@"I create an appointment with slot reference ""(.*)"" for patient ""(.*)"" called ""(.*)"" from schedule ""(.*)""")]
        public void GivenISearchForAnAppointmentOnTheProviderSystemAndBookAppointmentWithSlotReference(string slotReference, string patientName, string appointmentName, string getScheduleBundleKey)
        {
            Given($@"I perform a patient search for patient ""{patientName}"" and store the first returned resources against key ""AppointmentReadPatientResource""");
            Patient patientResource = (Patient)HttpContext.StoredFhirResources["AppointmentReadPatientResource"];
            Bundle getScheduleResponseBundle = (Bundle)HttpContext.StoredFhirResources[getScheduleBundleKey];

            List<Slot> slotList = new List<Slot>();
            Dictionary<string, Practitioner> practitionerDictionary = new Dictionary<string, Practitioner>();
            Dictionary<string, Location> locationDictionary = new Dictionary<string, Location>();
            Dictionary<string, Schedule> scheduleDictionary = new Dictionary<string, Schedule>();
            List<Identifier> identifiers = new List<Identifier>();

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

            CodeableConcept reason = new CodeableConcept();
            Coding coding = new Coding();
            List<Extension> extensionList = new List<Extension>();
            Extension extension = new Extension();
            Identifier identifier = new Identifier();
           
            AppointmentStatus status = new AppointmentStatus();
            int priority = new int();

            switch (appointmentName)
            {
                case "CustomAppointment1":
                    //Define category
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1", "ONL", "Online"));
                    //Define Identifier
                    identifier.System = "http://fhir.nhs.net/Id/gpconnect-appointment-identifier";
                    identifier.Value = "898976578";
                    identifiers.Add(identifier);
                    //Define Status
                    status = AppointmentStatus.Fulfilled;
                    //Define Reason
                    coding = buildReasonForAppointment("http://snomed.info/sct", "SBJ", "SBJ");
                    reason.Coding.Add(coding);
                    //Define Priority
                    priority = 1;
                    //Define participant
                    break;
            }

                    // Create Appointment
                Appointment appointment = buildAndReturnAppointment(patientResource, practitionerReferenceForSelectedSlot, locationReferenceForSelectedSlot, firstSlot, reason, extension, identifiers, status, priority);

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
        
        [Then(@"I add an extra invalid extension to the appointment called ""(.*)"" and populate the value")]
        public void ThenIAddAnExtraInvalidExtensionToTheAppointmentCalledStringAndPopulateTheValue(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);

            Extension ext = new Extension();
            Code code = new Code();
            code.Value = "INVALID VALUE";
            ext.Value = code;
            appointment.Extension.Add(ext);
            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }

        [Then(@"I add an extra invalid extension to the appointment called ""(.*)"" and populate the system")]
        public void ThenIAddAnExtraInvalidExtensionToTheAppointmentCalledStringAndPopulateTheSystem (String appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);

            Extension ext = new Extension();
            ext.Url = "RANDOM EXTENSION USED FOR TESTING";
            appointment.Extension.Add(ext);

            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }
        
        [Then(@"I add an extra invalid extension to the appointment called ""(.*)"" and populate the system and value")]
        public void ThenIAddAnExtraInvalidExtensionToTheAppointmentCalledStringAndPopulateTheSystemAndValue(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);
            Extension ext = new Extension();
            Code code = new Code();
            code.Value = "INVALID VALUE";
            ext.Url = "RANDOM EXTENSION USED FOR TESTING";
            ext.Value = code;
            appointment.Extension.Add(ext);

            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }

        [Given(@"I change the appointment id to ""(.*)"" to the appointment called ""(.*)""")]
        public void GivenIChangeTheAppointmentIdToToTheAppointmentCalled(string randomId, string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);

            appointment.Id = randomId;

            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }
        
        [Then(@"I set the appointment start element to null for ""(.*)""")]
        public void ThenISetTheAppointmentStartElementToNull(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);
            appointment.Start = null;
            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }

        [Then(@"I set the appointment end element to null for ""(.*)""")]
        public void ThenISetTheAppointmentEndElementToNull(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);
            appointment.End = null;
            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }
        
        [Then(@"I set the appointment status element to null for ""(.*)""")]
        public void ThenISetTheAppointmentStatusElementToNull(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);
            appointment.Status = null;
            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }

        [Then(@"I set the appointment slot element to null for ""(.*)""")]
        public void ThenISetTheAppointmentSlotElementToNull(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);
            appointment.Slot = null;
            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }
        
        [Then(@"I set the appointment identifier value element to null for ""(.*)""")]
        public void ThenISetTheAppointmentIdentifierValueElementToNull(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);
            appointment.Identifier.First().Value = null;
            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }

        [Then(@"I set the appointment reason coding system element to null for ""(.*)""")]
        public void ThenISetTheAppointmentReasonCodingSystemElementToNull(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);
            appointment.Reason.Coding.First().System = null;
            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }

        [Then(@"I set the appointment reason coding code element to null for ""(.*)""")]
        public void ThenISetTheAppointmentReasonCodingCodeElementToNull(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);
            appointment.Reason.Coding.First().Code = null;
            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }

        [Then(@"I set the appointment reason coding display element to null for ""(.*)""")]
        public void ThenISetTheAppointmentReasonCodingDisplayElementToNull(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);
            appointment.Reason.Coding.First().Display = null;
            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }

        [Then(@"I set the appointment Patient participant status element to null for ""(.*)""")]
        public void ThenISetTheAppointmentPatientParticipantStatusElementToNull(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);

            foreach (ParticipantComponent component in appointment.Participant)
            {
                if (component.Actor.Reference.ToString().Contains("Patient"))
                {
                    component.Status = null;
                }
            }

            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }

        [Then(@"I set the appointment Practitioner participant status element to null for ""(.*)""")]
        public void ThenISetTheAppointmentPractitionerParticipantStatusElementToNull(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);

            foreach (ParticipantComponent component in appointment.Participant)
            {
                if (component.Actor.Reference.ToString().Contains("Practitioner"))
                {
                    component.Status = null;
                }
            }

            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }

        [Then(@"I set the appointment Location participant status element to null for ""(.*)""")]
        public void ThenISetTheAppointmentLocationParticipantStatusElementToNull(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);

            foreach (ParticipantComponent component in appointment.Participant)
            {
                if (component.Actor.Reference.ToString().Contains("Location"))
                {
                    component.Status = null;
                }
            }

            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }

        [Then(@"I set the appointment Patient participant type coding system element to null for ""(.*)""")]
        public void ThenISetTheAppointmentPatientParticipantTypeCodingSystemElementToNull(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);

            foreach (ParticipantComponent component in appointment.Participant)
            {
                if (component.Actor.Reference.ToString().Contains("Patient"))
                {
                    component.Type.First().Coding.First().System = null;
                }
            }

            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }

        [Then(@"I set the appointment Patient participant type coding code element to null for ""(.*)""")]
        public void ThenISetTheAppointmentPatientParticipantTypeCodingCodeElementToNull(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);

            foreach (ParticipantComponent component in appointment.Participant)
            {
                if (component.Actor.Reference.ToString().Contains("Patient"))
                {
                    component.Type.First().Coding.First().Code = null;
                }
            }

            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }

        [Then(@"I set the appointment Patient participant type coding display element to null for ""(.*)""")]
        public void ThenISetTheAppointmentPatientParticipantTypeCodingDisplayElementToNull(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);

            foreach (ParticipantComponent component in appointment.Participant)
            {
                if (component.Actor.Reference.ToString().Contains("Patient"))
                {
                    component.Type.First().Coding.First().Display = null;
                }
            }

            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }

        [Then(@"I set the appointment Practitioner participant type coding system element to null for ""(.*)""")]
        public void ThenISetTheAppointmentPractitionerParticipantTypeCodingSystemElementToNull(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);

            foreach (ParticipantComponent component in appointment.Participant)
            {
                if (component.Actor.Reference.ToString().Contains("Practitioner"))
                {
                    component.Type.First().Coding.First().System = null;
                }
            }

            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }

        [Then(@"I set the appointment Practitioner participant type coding code element to null for ""(.*)""")]
        public void ThenISetTheAppointmentPractitionerParticipantTypeCodingCodeElementToNull(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);

            foreach (ParticipantComponent component in appointment.Participant)
            {
                if (component.Actor.Reference.ToString().Contains("Practitioner"))
                {
                    component.Type.First().Coding.First().Code = null;
                }
            }

            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }

        [Then(@"I set the appointment Practitioner participant type coding display element to null for ""(.*)""")]
        public void ThenISetTheAppointmentPractitionerParticipantTypeCodingDisplayElementToNull(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);

            foreach (ParticipantComponent component in appointment.Participant)
            {
                if (component.Actor.Reference.ToString().Contains("Practitioner"))
                {
                    component.Type.First().Coding.First().Display = null;
                }
            }

            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }

        [Then(@"I remove the participant from the appointment called ""(.*)"" which starts with reference ""(.*)""")]
        public void ThenIRemoveTheParticipantFromTheAppointmentCalledWhichStartsWithReference(string appointmentName, string reference)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpContext.StoredFhirResources.Remove(appointmentName);

            List<ParticipantComponent> componentList = new List<ParticipantComponent>();

            foreach (ParticipantComponent participant in appointment.Participant)
            {
                componentList.Add(participant);
            }
            for (int i = 0; i < componentList.Count; i++)
            {
                if (componentList[i].Actor.Reference.Contains(reference))
                {
                    appointment.Participant.RemoveAt(i);
                }
            }

            HttpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }

        [When(@"I book the appointment called ""([^""]*)""")]
        public void WhenIBookTheAppointmentCalledString(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            bookCustomAppointment("urn:nhs:names:services:gpconnect:fhir:rest:create:appointment", "/Appointment", FhirSerializer.SerializeToJson(appointment));
        }

        [When(@"I book the appointment called ""([^""]*)"" against the URL ""([^""]*)"" with the interactionId ""([^""]*)""")]
        public void WhenIBookTheAppointmentCalledAgainstTheUrlWithTheInteractionId(string appointmentName, string url, string interactionId)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            bookCustomAppointment(interactionId, url, FhirSerializer.SerializeToJson(appointment));
        }

        [When(@"I book the appointment called ""([^""]*)"" with an invalid field")]
        public void ThenIBookTheAppointmentCalledStringWithAnInvalidField(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            string appointmentString = FhirSerializer.SerializeToJson(appointment);
            appointmentString = FhirHelper.AddInvalidFieldToResourceJson(appointmentString);
            bookCustomAppointment("urn:nhs:names:services:gpconnect:fhir:rest:create:appointment", "/Appointment", appointmentString);
        }

        [When(@"I book the appointment called ""([^""]*)"" without status check")]
        public void ThenIBookTheAppointmentCalledStringWithoutStatusCheck(string appointmentName)
        {
            Resource appointment = HttpContext.StoredFhirResources[appointmentName];
            bookCustomAppointment("urn:nhs:names:services:gpconnect:fhir:rest:create:appointment", "/Appointment", FhirSerializer.SerializeToJson(appointment));
        }


        public void bookAppointment(string interactionID, string relativeUrl, Resource appointment)
        {
            bookCustomAppointment(interactionID, relativeUrl, FhirSerializer.SerializeToJson(appointment));

            // Convert the response to resource
            Then($@"the response status code should indicate created");
            Then($@"the response body should be FHIR JSON");
            And($@"the response should be an Appointment resource");
        }

        public void bookCustomAppointment(string interactionID, string relativeUrl, String appointment)
        {
            // Clear down previous requests
            HttpContext.RequestHeaders.Clear();
            HttpContext.RequestUrl = "";
            HttpContext.RequestParameters.ClearParameters();
            HttpContext.RequestBody = null;
            HttpContext.ResponseHeaders.Clear();

            // Setup configuration
            Given($@"I am using the default server");
            And($@"I set the default JWT");
            And($@"I am performing the ""{interactionID}"" interaction");

            // Book the apppointment
            HttpSteps.RestRequest(Method.POST, relativeUrl, appointment);
        }

        public void bookWithoutCleanUpAppointment(string interactionID, string relativeUrl, Resource appointment)
        {
            FhirSerializer.SerializeToJson(appointment);
            Given($@"I am performing the ""{interactionID}"" interaction");
            HttpSteps.RestRequest(Method.POST, relativeUrl, FhirSerializer.SerializeToJson(appointment));
        }


        [Then(@"the content-type should not be equal to null")]
        public void ThenTheContentTypeShouldNotBeEqualToZero()
        {
            string contentType = "";
            HttpContext.ResponseHeaders.TryGetValue("Content-Type", out contentType);
            if ((!contentType.Contains("json+fhir")) && (!contentType.Contains("xml+fhir")))
            {
                Assert.Fail("Content type incorrect");
            }
        }

        [Then(@"the content-type should be equal to null")]
        public void ThenTheContentTypeShouldBeEqualToZero()
        {
            string contentType = "";
            HttpContext.ResponseHeaders.TryGetValue("Content-Type", out contentType);
            contentType.ShouldBe(null);
        }
        
        [Then(@"the content-length should not be equal to zero")]
        public void ThenTheContentLengthShouldNotBeEqualToZero()
        {
            string contentLength = "";
            HttpContext.ResponseHeaders.TryGetValue("Content-Length", out contentLength);
            contentLength.ShouldNotBe("0");
        }

        [Then(@"the content-length should be equal to zero")]
        public void ThenTheContentLengthShouldBeEqualToZero()
        {
            string contentLength = "";
            HttpContext.ResponseHeaders.TryGetValue("Content-Length", out contentLength);
            contentLength.ShouldBe("0");
        }
        
        [Then(@"the appointment resource participant must contain a type or actor")]
        public void ThenTheAppointmentResourceShouldContainAParticipantWithATypeOrActor()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            foreach (ParticipantComponent part in appointment.Participant)
            {
                string actor = part.Actor.ToString();
                string type = part.Type.ToString();

                if (null == actor && null == type)
                {
                    Assert.Fail();
                }
            }
        }
        
        [Then(@"the appointment location reference is present and is saved as ""(.*)""")]
        public void ThenIMakeAGetRequestAndValidateTheReferences(string responseLocation)
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            foreach (ParticipantComponent part in appointment.Participant)
            {
                string actor = part.Actor.Reference.ToString();
                if (actor.Contains("Location"))
                {
                    HttpContext.resourceNameStored.Add(responseLocation, actor); 
                }
            }
        }
        
        [Then(@"the appointment participant contains a type is should have a valid system and code")]
        public void ThenTheAppointmentParticipantContainsATypeIsShouldHaveAValidSystemAndCode()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
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

        [Then(@"if the location response resource contains an identifier it is valid")]
        public void ThenIfTheLocationResponseResourceContainsAnIdentifierItIsValid()
        {
            FhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.Location);
            Location location = (Location)FhirContext.FhirResponseResource;
            foreach (Identifier identifier in location.Identifier)
            {
                identifier.System.ShouldNotBeNull();
                identifier.Value.ShouldNotBeNull();
            }
        }
        
        [Then(@"if the returned appointment category element is present it is populated with the correct values")]
        public void ThenIfTheReturnedAppointmentCategoryElementIsPresentItIsPopulatedWithTheCorrectValues()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            foreach (Extension appointmentCategory in appointment.ModifierExtension)
            {
                if (appointmentCategory.Url.Equals("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1"))
                {
                    string[] codes = new string[5] { "CLI", "ADM", "VIR", "REM", "MSG" };
                    string[] displays = new string[5] { "Clinical", "Administrative", "Virtual", "Reminder", "Message" };

                    appointmentCategory.Value.ShouldNotBeNull("There should be a value element within the appointment category extension");
                    var extensionValueCodeableConcept = (CodeableConcept)appointmentCategory.Value;
                    extensionValueCodeableConcept.Coding.ShouldNotBeNull("There should be a coding element within the appointment category extension");
                    extensionValueCodeableConcept.Coding.Count.ShouldBe(1, "There should be a single code element within the appointment category extension");
                    foreach (var coding in extensionValueCodeableConcept.Coding)
                    {
                        // Check that the code and display values are valid for the extension and match each other
                        bool codeAndDisplayFound = false;
                        for (int i = 0; i < codes.Length; i++)
                        {
                            if (string.Equals(codes[i], coding.Code) && string.Equals(displays[i], coding.Display))
                            {
                                codeAndDisplayFound = true;
                                break;
                            }
                        }
                        codeAndDisplayFound.ShouldBeTrue("The code and display values are not valid for the appointmentCategory extension");
                    }
                }
            }
        }

        [Then(@"if the returned appointment booking element is present it is populated with the correct values")]
        public void ThenIfTheReturnedAppointmentBookingElementIsPresentItIsPopulated()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            foreach (Extension appointmentBooking in appointment.ModifierExtension)
            {
                if (appointmentBooking.Url.Equals("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1"))
                {
                    string[] codes = new string[6] { "ONL", "PER", "TEL", "EMA", "LET", "TEX" };
                    string[] displays = new string[6] { "Online", "In person", "Telephone", "Email", "Letter", "Text" };

                    appointmentBooking.Value.ShouldNotBeNull("There should be a value element within the appointment booking method extension");
                    var extensionValueCodeableConcept = (CodeableConcept)appointmentBooking.Value;
                    extensionValueCodeableConcept.Coding.ShouldNotBeNull("There should be a coding element within the appointment booking method extension");
                    extensionValueCodeableConcept.Coding.Count.ShouldBe(1, "There should be a single code element within the appointment booking method extension");
                    foreach (var coding in extensionValueCodeableConcept.Coding)
                    {
                        // Check that the code and display values are valid for the extension and match each other
                        bool codeAndDisplayFound = false;
                        for (int i = 0; i < codes.Length; i++)
                        {
                            if (string.Equals(codes[i], coding.Code) && string.Equals(displays[i], coding.Display))
                            {
                                codeAndDisplayFound = true;
                                break;
                            }
                        }
                        codeAndDisplayFound.ShouldBeTrue("The code and display values are not valid for the appointmentBookingMethod extension");
                    }
                }
            }
        }
        
        [Then(@"if the returned appointment contact element is present it is populated with the correct values")]
        public void ThenIfTheReturnedAppointmentContactElementIsPresentItIsPopulatedWithTheCorrectValues()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            foreach (Extension appointmentContact in appointment.ModifierExtension)
            {
                if (appointmentContact.Url.Equals("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1"))
                {
                    string[] codes = new string[5] { "ONL", "PER", "TEL", "EMA", "LET" };
                    string[] displays = new string[5] { "Online", "In person", "Telephone", "Email", "Letter" };

                    appointmentContact.Value.ShouldNotBeNull("There should be a value element within the appointment ContactMethod extension");
                    var extensionValueCodeableConcept = (CodeableConcept)appointmentContact.Value;
                    extensionValueCodeableConcept.Coding.ShouldNotBeNull("There should be a coding element within the appointment ContactMethod extension");
                    extensionValueCodeableConcept.Coding.Count.ShouldBe(1, "There should be a single code element within the appointment ContactMethod extension");
                    foreach (var coding in extensionValueCodeableConcept.Coding)
                    {
                        // Check that the code and display values are valid for the extension and match each other
                        bool codeAndDisplayFound = false;
                        for (int i = 0; i < codes.Length; i++)
                        {
                            if (string.Equals(codes[i], coding.Code) && string.Equals(displays[i], coding.Display))
                            {
                                codeAndDisplayFound = true;
                                break;
                            }
                        }
                        codeAndDisplayFound.ShouldBeTrue("The code and display values are not valid for the appointmentContactMethod extension");
                    }
                }
            }
        }

        [Then(@"if the returned appointment cancellation reason element is present it is populated with the correct values")]
        public void ThenIfTheReturnedAppointmentCancellationReasonElementIsPresentItIsPopulatedWithTheCorrectValues()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            int extensionCount = 0;

            foreach (Extension appointmentCancellationReason in appointment.ModifierExtension)
            {
                if (appointmentCancellationReason.Url.Equals("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1-0"))
                {
                    appointmentCancellationReason.ShouldNotBeNull();
                    appointmentCancellationReason.Url.ShouldBeOfType<Uri>();
                    appointmentCancellationReason.Value.ShouldBeOfType<String>();

                    extensionCount++;
                }
                extensionCount.ShouldBe(1);
            }
        }

        private Appointment buildAndReturnAppointment(Patient patientResource, List<string> practitionerReferenceForSelectedSlot, string locationReferenceForSelectedSlot, Slot firstSlot, CodeableConcept reason,Extension extension, List<Identifier> identifier, AppointmentStatus appointmentStatus, int priority)
        {
            Appointment appointment = new Appointment();
            appointment.Status = appointmentStatus;
            appointment.Identifier = identifier;
            appointment.Priority = priority;

            // Appointment Patient Resource
            ParticipantComponent patient = new ParticipantComponent();
            ResourceReference patientReference = new ResourceReference();
            patientReference.Reference = "Patient/" + patientResource.Id;
            patient.Actor = patientReference;
            patient.Status = ParticipationStatus.Accepted;
            patient.Type.Add(new CodeableConcept("http://hl7.org/fhir/ValueSet/encounter-participant-type", "SBJ", "patient", "patient"));
            appointment.Participant.Add(patient);

            // Appointment Practitioner Resource
            foreach (var practitionerSlotReference in practitionerReferenceForSelectedSlot)
            {
                ParticipantComponent practitioner = new ParticipantComponent();
                ResourceReference practitionerReference = new ResourceReference();
                practitionerReference.Reference = practitionerSlotReference;
                practitioner.Actor = practitionerReference;
                practitioner.Status = ParticipationStatus.Accepted;
                practitioner.Type.Add(new CodeableConcept("http://hl7.org/fhir/ValueSet/encounter-participant-type", "PPRF", "practitioner", "practitioner"));
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
            
            if (HttpContext.StoredDate.ContainsKey("slotStartDate")) HttpContext.StoredDate.Remove("slotStartDate");
            HttpContext.StoredDate.Add("slotStartDate", firstSlot.StartElement.ToString());
            Coding coding = buildReasonForAppointment("http://snomed.info/sct", "1", "1");
            reason.Coding.Add(coding);
            appointment.Reason = reason;
            appointment.Extension.Add(extension);
            
            appointment.Identifier.Add(new Identifier(null, Guid.NewGuid().ToString()));
            appointment.Reason = new CodeableConcept("http://read.info/readv2", "1", "reason", null);
            return appointment;
        }

        private Coding buildReasonForAppointment(string system, string code, string display)
        {
            Coding codingSnomedCT = new Coding();
            codingSnomedCT.System = system;
            codingSnomedCT.Code = code;
            codingSnomedCT.Display = display;
            return codingSnomedCT;
        }

        private Extension buildAppointmentCategoryExtension(Extension extension, string url, string code, string display)
        {
            extension.Url = url;
            CodeableConcept value = new CodeableConcept();
            Coding coding = new Coding();
            coding.Code = code;
            coding.Display = display;
            value.Coding.Add(coding);
            extension.Value = value;
            return extension;
        }
    }
}
