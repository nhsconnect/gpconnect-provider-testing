using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using NUnit.Framework;
using RestSharp;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public void GivenICreateAnAppointmentForPatientCalledFromSchedule(string patientKey, string appointmentName, string getScheduleBundleKey)
        {
            Patient patientResource = (Patient)HttpContext.StoredFhirResources[patientKey];
            Bundle getScheduleResponseBundle = (Bundle)HttpContext.StoredFhirResources[getScheduleBundleKey];

            List<Slot> slotList = new List<Slot>();
            Dictionary<string, Practitioner> practitionerDictionary = new Dictionary<string, Practitioner>();
            Dictionary<string, Location> locationDictionary = new Dictionary<string, Location>();
            Dictionary<string, Schedule> scheduleDictionary = new Dictionary<string, Schedule>();
            
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
            CodeableConcept reason = null;
            Coding coding = null;
            List<Extension> extensionList = null;
            Extension extension = null;
            Identifier identifier = null;
            List<Identifier> identifiers = null;
            AppointmentStatus status = AppointmentStatus.Booked;
            int? priority = null;

            switch (appointmentName)
            {
                case "Appointment1":
                    extensionList = new List<Extension>();
                    extension = new Extension();
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1", "ONL", "Online"));
                    identifier = new Identifier();
                    identifier.System = "http://fhir.nhs.net/Id/gpconnect-appointment-identifier";
                    identifier.Value = "898976578";
                    identifiers = new List<Identifier>();
                    identifiers.Add(identifier);
                    coding = new Coding();
                    coding = buildReasonForAppointment("http://snomed.info/sct", "", "");
                    reason = new CodeableConcept();
                    reason.Coding.Add(coding);
                    priority = 1;
                    break;

                case "Appointment2":
                    extensionList = new List<Extension>();
                    extension = new Extension();
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1", "ONL", "Online"));
                    priority = 0;
                    break;

                case "Appointment3":
                    extensionList = new List<Extension>();
                    extension = new Extension();
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1", "CLI", "Clinical"));
                    identifier = new Identifier();
                    identifier.System = "http://fhir.nhs.net/Id/gpconnect-appointment-identifier";
                    identifier.Value = "898976580";
                    identifiers = new List<Identifier>();
                    identifiers.Add(identifier);
                    break;

                case "Appointment4":
                    extensionList = new List<Extension>();
                    extension = new Extension();
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1", "", "Double booked"));
                    identifier = new Identifier();
                    identifier.System = "http://fhir.nhs.net/Id/gpconnect-appointment-identifier";
                    identifier.Value = "898976581";
                    identifiers = new List<Identifier>();
                    identifiers.Add(identifier);
                    reason = new CodeableConcept();
                    coding = new Coding();
                    coding = buildReasonForAppointment("http://snomed.info/sct", "", "");
                    reason.Coding.Add(coding);
                    coding = buildReasonForAppointment("http://read.info/readv2", "", "");
                    reason.Coding.Add(coding);
                    coding = buildReasonForAppointment("http://read.info/ctv3", "", "");
                    reason.Coding.Add(coding);
                    priority = 9;
                    break;

                case "Appointment5":
                    extensionList = new List<Extension>();
                    extension = new Extension();
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1", "PER", "In person"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1", "ADM", "Administrative"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1", "ONL", "Online"));
                    break;

                case "Appointment6":
                    extensionList = new List<Extension>();
                    extension = new Extension();
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1", "TEL", "Telephone"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1", "VIR", "Virtual"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1", "PER", "In person"));
                    priority = 3;
                    break;

                case "Appointment7":
                    extensionList = new List<Extension>();
                    extension = new Extension();
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1", "EMA", "Email"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1", "REM", "Reminder"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1", "TEL", "Telephone"));
                    priority = 4;
                    break;

                case "Appointment8":
                    extensionList = new List<Extension>();
                    extension = new Extension();
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1", "LET", "Letter"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1", "MSG", "Message"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1", "EMA", "Email"));
                    reason = new CodeableConcept();
                    coding = new Coding();
                    coding = buildReasonForAppointment("http://snomed.info/sct", "", "");
                    reason.Coding.Add(coding);
                    coding = buildReasonForAppointment("http://read.info/readv2", "", "");
                    reason.Coding.Add(coding);
                    coding = buildReasonForAppointment("http://read.info/ctv3", "", "");
                    reason.Coding.Add(coding);
                    priority = 5;
                    break;

                case "Appointment9":
                    extensionList = new List<Extension>();
                    extension = new Extension();
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1", "TEX", "Text"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1", "ADM", "Administrative"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1", "LET", "Letter"));
                    priority = 6;
                    break;

                case "Appointment10":
                    extensionList = new List<Extension>();
                    extension = new Extension();
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1", "PER", "In person"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1", "CLI", "Clinical"));
                    extensionList.Add(buildAppointmentCategoryExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1", "PER", "In person"));
                    identifiers = new List<Identifier>();
                    for (int i = 0; i < 3; i++)
                    {
                        identifier = new Identifier();
                        identifier.System = "http://fhir.nhs.net/Id/gpconnect-appointment-identifier";
                        identifier.Value = "89897652"+i;
                        identifiers.Add(identifier);
                    }
                    reason = new CodeableConcept();
                    coding = new Coding();
                    coding = buildReasonForAppointment("http://snomed.info/sct", "", "");
                    reason.Coding.Add(coding);
                    coding = buildReasonForAppointment("http://read.info/readv2", "", "");
                    reason.Coding.Add(coding);
                    coding = buildReasonForAppointment("http://read.info/ctv3", "", "");
                    reason.Coding.Add(coding);
                    priority = 7;
                    break;
            }

            Appointment appointment = new Appointment();
            appointment.Status = status;
            
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

            if (identifiers != null) appointment.Identifier = identifiers;
            if (priority != null) appointment.Priority = priority;
            if (reason != null) appointment.Reason = reason;
            if (extensionList != null) appointment.Extension = extensionList;
            
            // Store start date for use in other tests
            if (HttpContext.StoredDate.ContainsKey("slotStartDate")) HttpContext.StoredDate.Remove("slotStartDate");
            HttpContext.StoredDate.Add("slotStartDate", firstSlot.StartElement.ToString());
            
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

            // Store appointment
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

        [Given(@"I set the appointment Priority to ""(.*)"" on appointment stored against key ""([^""]*)""")]
        public void GivenISetTheAppointmentPriorityToOnAppointmentStoredAgainstKey(int priority, string appointmentKey)
        {
            ((Appointment)HttpContext.StoredFhirResources[appointmentKey]).Priority = priority;
        }

        [When(@"I book the appointment called ""([^""]*)""")]
        public void WhenIBookTheAppointmentCalledString(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            if (HttpContext.RequestContentType.Contains("xml"))
            {
                HttpSteps.RestRequest(Method.POST, "/Appointment", FhirSerializer.SerializeToXml(appointment));
            }
            else {
                HttpSteps.RestRequest(Method.POST, "/Appointment", FhirSerializer.SerializeToJson(appointment));
            }
        }

        [When(@"I book the appointment called ""([^""]*)"" against the URL ""([^""]*)""")]
        public void WhenIBookTheAppointmentCalledAgainstTheUrlWithTheInteractionId(string appointmentName, string url)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            if (HttpContext.RequestContentType.Contains("xml"))
            {
                HttpSteps.RestRequest(Method.POST, url, FhirSerializer.SerializeToXml(appointment));
            }
            else
            {
                HttpSteps.RestRequest(Method.POST, url, FhirSerializer.SerializeToJson(appointment));
            }
        }

        [When(@"I book the appointment called ""([^""]*)"" with an invalid field")]
        public void ThenIBookTheAppointmentCalledStringWithAnInvalidField(string appointmentName)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            string appointmentString = FhirSerializer.SerializeToJson(appointment);
            appointmentString = FhirHelper.AddInvalidFieldToResourceJson(appointmentString);
            HttpSteps.RestRequest(Method.POST, "/Appointment", appointmentString);
        }
        
        [Given(@"I create an appointment for patient ""([^ ""]*)"" at organization ""([^""]*)"" with priority ""([^""]*)"" and save appintment resources to ""([^""]*)""")]
        public void ICreateAnAppointmentForPatientAtOrganizationWithPriorityAndSaveAppointmentResourceTo(string patient, string organizaitonName, int priority, string patientAppointmentskey)
        {
            Given($@"I perform the getSchedule operation for organization ""{organizaitonName}"" and store the returned bundle resources against key ""getScheduleResponseBundle""");
            IBookAnAppointmentForPatientOnTheProviderSystemUsingASlotFromTheGetScheduleResponseBundleStoredAgainstKeyAndStoreTheAppointmentToWithPriority(patient, "getScheduleResponseBundle", patientAppointmentskey, priority);
        }

        [When(@"I book an appointment for patient ""([^""]*)"" on the provider system using a slot from the getSchedule response bundle stored against key ""([^""]*)"" and store the appointment to ""([^""]*)""")]
        public void IBookAnAppointmentForPatientOnTheProviderSystemUsingASlotFromTheGetScheduleResponseBundleStoredAgainstKeyAndStoreTheAppointmentTo(string patientName, string getScheduleBundleKey, string storeAppointmentKey)
        {
            IBookAnAppointmentForPatientOnTheProviderSystemUsingASlotFromTheGetScheduleResponseBundleStoredAgainstKeyAndStoreTheAppointmentToWithPriority(patientName, getScheduleBundleKey, storeAppointmentKey, 1);
        }

        public void IBookAnAppointmentForPatientOnTheProviderSystemUsingASlotFromTheGetScheduleResponseBundleStoredAgainstKeyAndStoreTheAppointmentToWithPriority(string patientName, string getScheduleBundleKey, string storeAppointmentKey, int priority)
        {
            Given($@"I perform a patient search for patient ""{patientName}"" and store the first returned resources against key ""StoredPatientKey""");
            Given($@"I am using the default server");
            And($@"I set the JWT requested record NHS number to the NHS number of patient stored against key ""StoredPatientKey""");
		    And($@"I set the JWT requested scope to ""patient/*.write""");
		    And($@"I am performing the ""urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"" interaction");
            Given($@"I create an appointment for patient ""StoredPatientKey"" called ""Appointment"" from schedule ""{getScheduleBundleKey}""");
            Given($@"I set the appointment Priority to ""{priority}"" on appointment stored against key ""Appointment""");
            When($@"I book the appointment called ""Appointment""");
            Then($@"the response status code should indicate created");
            Then($@"the response body should be FHIR JSON");
            And($@"the response should be an Appointment resource");
            if (HttpContext.StoredFhirResources.ContainsKey(storeAppointmentKey))
            {
                HttpContext.StoredFhirResources.Remove(storeAppointmentKey);
            }
            HttpContext.StoredFhirResources.Add(storeAppointmentKey, FhirContext.FhirResponseResource);
        }
        
        public void bookWithoutCleanUpAppointment(string interactionID, string relativeUrl, Resource appointment)
        {
            FhirSerializer.SerializeToJson(appointment);
            Given($@"I am performing the ""{interactionID}"" interaction");
            HttpSteps.RestRequest(Method.POST, relativeUrl, FhirSerializer.SerializeToJson(appointment));
        }


        [Then(@"the content-type should not be equal to null")]
        public void ThenTheContentTypeShouldNotBeEqualToNull()
        {
            string contentType = null;
            HttpContext.ResponseHeaders.TryGetValue("Content-Type", out contentType);
            contentType.ShouldNotBeNullOrEmpty("The response should contain a Content-Type header.");
        }

        [Then(@"the content-type should be equal to null")]
        public void ThenTheContentTypeShouldBeEqualToZero()
        {
            string contentType = null;
            HttpContext.ResponseHeaders.TryGetValue("Content-Type", out contentType);
            contentType.ShouldBe(null, "There should not be a content-type header on the response");
        }
        
        [Then(@"the content-length should not be equal to zero")]
        public void ThenTheContentLengthShouldNotBeEqualToZero()
        {
            string contentLength = "";
            HttpContext.ResponseHeaders.TryGetValue("Content-Length", out contentLength);
            contentLength.ShouldNotBe("0", "The response payload should contain a resource.");
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

            location.Identifier.Count.ShouldBeLessThanOrEqualTo(2, "Too many location identifiers.");

            var odsSystemCount = 0;

            foreach (Identifier identifier in location.Identifier)
            {
                if ("http://fhir.nhs.net/Id/ods-site-code".Equals(identifier.System))
                {
                    identifier.Value.ShouldNotBeNullOrEmpty();
                    odsSystemCount++;
                }
            }

            odsSystemCount.ShouldBeLessThanOrEqualTo(1, "Too many ods-site-code systems.");
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
