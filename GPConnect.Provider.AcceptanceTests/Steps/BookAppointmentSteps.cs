using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Model;
using NUnit.Framework;
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
            Appointment appointment = buildAndReturnAppointment(patientResource, practitionerReferenceForSelectedSlot, locationReferenceForSelectedSlot,firstSlot);


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

        [Then(@"I create an appointment for patient ""(.*)"" called ""(.*)"" using a patient resource")]
        public void GivenISearchForAnAppointmentOnTheProviderSystemAndBookAppointmentWithSlotReference2(string patientName, string appointmentName)
        {
            Given($@"I perform a patient search for patient ""{patientName}"" and store the first returned resources against key ""AppointmentReadPatientResource""");
            Patient patientResource = (Patient)HttpContext.StoredFhirResources["AppointmentReadPatientResource"];
            HttpContext.StoredFhirResources.Add(appointmentName, patientResource);
        }

        [Then(@"I create an appointment for patient ""(.*)"" called ""(.*)"" using a bundle resource")]
        public void GivenISearchForAnAppointmentOnTheProviderSystemAndBookAppointmentWithSlotReference3(string patientName, string appointmentName)
        {
            Given($@"I perform a patient search for patient ""{patientName}"" and store the first returned resources against key ""AppointmentReadPatientResource""");
            Bundle bundle = new Bundle();
            HttpContext.StoredFhirResources.Add(appointmentName, bundle);
        }

  

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
            Appointment appointment = buildAndReturnAppointment(patientResource, practitionerReferenceForSelectedSlot, locationReferenceForSelectedSlot, firstSlot);

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

        [Then(@"I change the appointment id to ""(.*)"" to the appointment called ""(.*)""")]
        public void ThenIAddAnExtraFieldsToTheAppointmentCalledStringAndPopulateTheSystemAndValue(string randomId, string appointmentName)
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

        [Then(@"I book the appointment called ""(.*)""")]
        public void ThenIBookTheAppointmentCalledString(string appointmentName) {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            HttpSteps.bookAppointment("urn:nhs:names:services:gpconnect:fhir:rest:create:appointment", "/Appointment", appointment);
        }

        [Then(@"I book the appointment called ""(.*)"" which is an incorrect resource")]
        public void ThenIBookTheAppointmentCalledStringWhichIsAnIncorrectResource(string appointmentName)
        {
            Resource appointment = HttpContext.StoredFhirResources[appointmentName];
            HttpSteps.bookAppointment("urn:nhs:names:services:gpconnect:fhir:rest:create:appointment", "/Appointment", appointment);
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
            string contentType = "";
            HttpContext.ResponseHeaders.TryGetValue("Content-Length", out contentType);
            contentType.ShouldNotBe("0");
        }

        [Then(@"the content-length should be equal to zero")]
        public void ThenTheContentLengthShouldBeEqualToZero()
        {
            string contentType = "";
            HttpContext.ResponseHeaders.TryGetValue("Content-Length", out contentType);
            contentType.ShouldBe("0");
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

        private Appointment buildAndReturnAppointment(Patient patientResource, List<string> practitionerReferenceForSelectedSlot, string locationReferenceForSelectedSlot, Slot firstSlot)
        {
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

            return appointment;
        }
    }
}
