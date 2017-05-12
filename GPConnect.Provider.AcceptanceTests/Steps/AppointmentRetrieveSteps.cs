using System.Collections.Generic;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using Hl7.Fhir.Model;
using Shouldly;
using TechTalk.SpecFlow;
using static Hl7.Fhir.Model.Appointment;
using static Hl7.Fhir.Model.Bundle;
using System;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public class AppointmentRetrieveSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private readonly HttpSteps HttpSteps;
        private readonly HttpContext HttpContext;

        // Headers Helper
        public HttpHeaderHelper Headers { get; }

        public AppointmentRetrieveSteps(HttpHeaderHelper headerHelper, FhirContext fhirContext, HttpSteps httpSteps, HttpContext httpContext)
        {
            // Helpers
            FhirContext = fhirContext;
            Headers = headerHelper;
            HttpSteps = httpSteps;
            HttpContext = httpContext;
        }

        [Then(@"the bundle of appointments should contain meta data profile and version id")]
        public void TheBundleOfAppointmentShouldContainMetaDataProfileAndVersionid()
        {
            string profileId = "http://fhir.nhs.net/StructureDefinition/gpconnect-appointment-1";
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    {
                        var resource = entry.Resource;
                        resource.Meta.ShouldNotBeNull();
                        int metaProfileCount = 0;
                        foreach (string profile in resource.Meta.Profile)
                        {
                            metaProfileCount++;
                            profile.ShouldBe(profileId);
                        }
                        metaProfileCount.ShouldBe(1);
                        resource.Meta.VersionId.ShouldNotBeNull();
                    }
                }
            }

        }


        [Then(@"the response bundle should contain ""([^""]*)"" appointment")]
        public void TheResponseBundleShouldContainAppointments(int numOfAppointments)
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
            appointmentCount.ShouldBe(numOfAppointments);
        }





        [Then(@"the bundle of appointments should all contain a single status element")]
        public void ThenTheBundleOfAppointmentsShouldAllContainASingleStatusElement()
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
                    appointment.Status.ShouldNotBeNull();
                    string status = appointment.Status.ToString();
                    validAppointmentStatus.ShouldContain(status);

                }
            }

        }
        [Then(@"the bundle of appointments should all contain a single start element")]
        public void ThenTheBundleOfAppointmentShouldAllContainASingleStartElement()
        {

            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    appointment.Start.ShouldNotBeNull();

                }
            }


        }
        [Then(@"the bundle of appointments should all contain a single end element")]
        public void ThenTheBundleOfAppointmentsShouldAllContainASingleEndElement()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    appointment.End.ShouldNotBeNull();
                }
            }
        }

        [Then(@"the bundle of appointments should all contain at least one slot reference")]
        public void ThenTheBundleOfAppointmentsShouldAllContain()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {

                    Appointment appointment = (Appointment)entry.Resource;
                    appointment.Slot.ShouldNotBeNull();
                    appointment.Slot.Count.ShouldBeGreaterThanOrEqualTo(1);
                    foreach (var slotReference in appointment.Slot)
                    {
                        slotReference.Reference.ShouldStartWith("Slot/");
                    }
                }
            }
        }

        //Need to check the validity of the reference but currently no GET method
        [Then(@"the appointments slot reference in the bundle is present and valid")]
        public void ThenTheAppointmentsSlotReferenceInTheBundleIsPresentAndValid()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    foreach (ResourceReference slot in appointment.Slot)
                    {
                        slot.Reference.ShouldNotBeNull();
                        string slotRef = slot.Reference.ToString();
                        slotRef.ShouldContain("Slot/");

                    }

                }
            }
        }


        [Then(@"if the bundle of appointment response resource contains a reason element and coding the codings must be one of the three allowed with system code and display elements")]
        public void ThenIfTheBundleAppointmentResponseResourceContainsAReasonElementAndCodingItMustBeOneOfTheThreeAllowedWithSystemCodeAndDisplayElements()
        {


            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    var reason = appointment.Reason;
                    if (reason != null && reason.Coding != null)
                    {
                        int sctCount = 0;
                        int readv2Count = 0;
                        int ctv3Count = 0;
                        foreach (var coding in reason.Coding)
                        {
                            var validSystems = new string[3] { "http://snomed.info/sct", "http://read.info/readv2", "http://read.info/ctv3" };
                            coding.System.ShouldBeOneOf(validSystems, "The reason coding System can only be one of the valid value");

                            switch (coding.System)
                            {
                                case "http://snomed.info/sct":
                                    sctCount++;
                                    break;
                                case "http://read.info/readv2":
                                    readv2Count++;
                                    break;
                                case "http://read.info/ctv3":
                                    ctv3Count++;
                                    break;
                            }
                            coding.Code.ShouldNotBeNullOrEmpty("The appointment reason coding Code must have a value");
                            coding.Display.ShouldNotBeNullOrEmpty("The appointment reason coding display must have a value");
                        }
                        // Check there is no more than one of each coding
                        sctCount.ShouldBeLessThanOrEqualTo(1);
                        readv2Count.ShouldBeLessThanOrEqualTo(1);
                        ctv3Count.ShouldBeLessThanOrEqualTo(1);
                    }
                }
            }
        }



        [Then(@"the bundle of appointments should all contain one participant which is a patient and one which is a practitioner")]
        public void ThenTheBundleOfAppointmentsShouldAllContainOneParticipantWhichIsAPatientAndOneWhichIsAPractitioner()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    appointment.Participant.ShouldNotBeNull();
                    bool patientFound = false;
                    bool practitionerFound = false;
                    foreach (Appointment.ParticipantComponent participant in appointment.Participant)
                    {
                        if (participant.Actor.Reference.StartsWith("Patient/"))
                        {
                            patientFound = true;
                        }
                        else if (participant.Actor.Reference.StartsWith("Practitioner/"))
                        {
                            practitionerFound = true;
                        }
                    }
                    patientFound.ShouldBeTrue("Patient reference not found in appointment");
                    practitionerFound.ShouldBeTrue("Practitioner reference not found in appointment");
                }
            }

        }
        [Then(@"the bundle of appointments should all contain one participant which is a practitioner")]
        public void ThenTheBundleOfAppointmentsShouldAllContainOneParticipant()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    appointment.Participant.ShouldNotBeNull();
                }
            }


        }


        [Then(@"the appointment resource within the bundle should contain a single status element")]
        public void ThenTheAppointmentResourceWithinTheBundleShouldContainASingleStatusElement()
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


        [Given(@"I create ""([^ ""] *)"" appointments for patient ""([^""]*)"" at organization ""([^""]*)"" and save bundle of appintment resources to ""([^""]*)""")]
        public void ICreateAAppointmentsForPatientAtOrganizationAndSaveAListOfResourceTo(int noApp, string patient, string organizaitonName, string bundleOfPatientAppointmentskey)
        {
            // Search For Patient appointments
            Given($@"I search for patient ""{patient}"" appointments and save the returned bundle of appointment resources against key ""{bundleOfPatientAppointmentskey}""");
            Bundle patientAppointmentsBundle = (Bundle)HttpContext.StoredFhirResources[bundleOfPatientAppointmentskey];

            int numberOfRequiredAdditionalAppointments = noApp - patientAppointmentsBundle.Entry.Count;


            // Perform get schedule once to get available slots with which to create appointments
            Given($@"I perform the getSchedule operation for organization ""{organizaitonName}"" and store the returned bundle resources against key ""getScheduleResponseBundle""");

            for (; noApp > 0; noApp--)
            {
                When($@"I book an appointment for patient ""{patient}"" on the provider system using a slot from the getSchedule response bundle stored against key ""getScheduleResponseBundle""");
            }

            // Search for appointments again to make sure that enough have been stored in the provider system and store them
            Given($@"I search for patient ""{patient}"" appointments and save the returned bundle of appointment resources against key ""{bundleOfPatientAppointmentskey}""");
            patientAppointmentsBundle = (Bundle)HttpContext.StoredFhirResources[bundleOfPatientAppointmentskey];

            patientAppointmentsBundle.Entry.Count.ShouldBeGreaterThanOrEqualTo(noApp, "We could not create enough appointments for the test to run.");
        }


        [Then(@"if the appointment category element is present it is populated with the correct values")]
        public void ThenIfTheAppointmentCategoryElementIsPresentItIsPopulatedWithTheCorrectValues()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    foreach (Extension appointmentCategory in appointment.Extension)
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
            }
        }

        [Then(@"if the appointment booking element is present it is populated with the correct values")]
        public void ThenIfTheAppointmentBookingElementIsPresentItIsPopulated()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    foreach (Extension appointmentBooking in appointment.Extension)
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
            }
        }


        [Then(@"if the appointment contact element is present it is populated with the correct values")]
        public void ThenIfTheAppointmentContactElementIsPresentItIsPopulatedWithTheCorrectValues()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    foreach (Extension appointmentContact in appointment.Extension)
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
            }
        }

        [Then(@"if the appointment cancellation reason element is present it is populated with the correct values")]
        public void ThenIfTheAppointmentCancellationReasonElementIsPresentItIsPopulatedWithTheCorrectValues()
        {

            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    int extensionCount = 0;

                    foreach (Extension appointmentCancellationReason in appointment.Extension)
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
            }
        }


        [Then(@"the returned appointment start date should match ""([^""]*)"" start Date")]
        public void ThenTheReturnedAppointmentStartDateShouldMatchStartDate(string appointmentName)
        {
            Appointment savedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];

            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    savedAppointment.Start.ShouldBe(appointment.Start);

                }
            }
        }



        [Then(@"the returned appointment end date should match ""([^""]*)"" end date")]
        public void ThenTheReturnedAppointmentEndtDateShouldMatchEndDate(string appointmentName)
        {
            Appointment savedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];

            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    savedAppointment.End.ShouldBe(appointment.End);

                }
            }
        }



        [Then(@"the returned appointment patient reference should match ""([^""]*)"" patient reference")]
        public void ThenTheReturnedAppointmentPatientReferenceShouldMatchPatientReference(string appointmentName)
        {
            Appointment savedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            string savedAppointmentPatientReference = "";
            string returnedResponseAppointmentPatientReference = "";

            foreach (Appointment.ParticipantComponent participant in savedAppointment.Participant)
            {
                if (participant.Actor.Reference.StartsWith("Patient/"))
                {
                    savedAppointmentPatientReference = participant.Actor.Reference.ToString();
                }

            }
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    appointment.Participant.ShouldNotBeNull();
                    foreach (Appointment.ParticipantComponent participant in appointment.Participant)
                    {
                        if (participant.Actor.Reference.StartsWith("Patient/"))
                        {
                            returnedResponseAppointmentPatientReference = participant.Actor.Reference.ToString();
                        }

                    }
                }
            }

            savedAppointmentPatientReference.ShouldBe(returnedResponseAppointmentPatientReference);
        }



        [Then(@"the returned appointment slot reference should match ""([^""]*)"" slot reference")]
        public void ThenTheReturnedAppointmentSlotReferenceShouldMatchSlotReference(string appointmentName)
        {
            Appointment savedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            List<string> savedAppointmentSlotURLs = new List<string>();
            List<string> returnedResponseAppointmentsSlotURL = new List<string>();
            foreach (ResourceReference slot in savedAppointment.Slot)
            {
                savedAppointmentSlotURLs.Add(slot.Url.ToString());

            }
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    foreach (ResourceReference slot in appointment.Slot)
                    {
                        returnedResponseAppointmentsSlotURL.Add(slot.Url.ToString());

                    }
                }
            }
            savedAppointmentSlotURLs.ShouldBe(returnedResponseAppointmentsSlotURL);
        }
    



        [Then(@"the returned appointment participant status should match ""([^""]*)"" participant status")]
        public void ThenTheReturnedAppointmentParticipantStatusShouldMatchParticipantStatus(string appointmentName)
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
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    appointment.Participant.ShouldNotBeNull();
                    foreach (Appointment.ParticipantComponent participant in appointment.Participant)
                    {
                        if (participant.Actor.Reference.StartsWith("Patient/"))
                        {
                            returnedResponseAppointmentParticipantStatus = participant.Status.Value;
                        }

                    }
                }
            }

            savedAppointmentParticipantStatus.ShouldBe(returnedResponseAppointmentParticipantStatus);
        }

    }
    }
    

