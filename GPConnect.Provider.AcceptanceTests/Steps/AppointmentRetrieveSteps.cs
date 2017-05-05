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
        public void CheckAppointmentBundleContainsMetaProfileAndVersionId()
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

        [Then(@"the response total should be at least 1")]
        public void responseTotalSgouldBeAtleast1()
        {
            Bundle bundle = (Bundle)FhirContext.FhirResponseResource;
            bundle.Total.ShouldNotBeNull<int?>();
            bundle.Total.ShouldBe<int?>(1);
        }

        [Given(@"I save to current time called ""([^""]*)""")]
        public void saveCurrentTimeToUseForAppointmentSearch(string timeName)
        {
            String currentDateTime = DateTime.Now.ToString("yyyy-MM-dd");
            HttpContext.StoredDate.Add(timeName, currentDateTime);


        }



        [Then(@"the bundle of appointments should all contain a single status element")]
        public void appointmentMustContainStatusElement()
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
        [Then(@"the bundle of appointments should all contain a single start element")]
        public void appointmentMustContainStartElement()
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
        public void appointmentMustContainEndElement()
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
        public void appointmentMustContainSlotReference()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    appointment.Slot.ShouldNotBeNull();
                }
            }

        }

        //Need to check the validity of the reference but currently no GET method
        [Then(@"the appointments slot reference in the bundle is present and valid")]
        public void checkingTheSlotReferenceIsValid()
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


        [Then(@"the bundle of appointments should all contain at least one participant")]
        public void appointmentMustContainParticipant()
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
        public void appointmentInBundleMustContainStatusElement()
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
        public void IFindOrCreateAAppointmentsForPatientAtOrganizationAndSaveAListOfResourceTo(int noApp, string patient, string organizaitonName, string bundleOfPatientAppointmentskey)
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
        public void appointmentCategoryIsPresentAndValid()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    foreach (Extension appointmentCategory in appointment.ModifierExtension)
                    {
                        if (appointmentCategory.Url.Equals("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1"))
                        {
                            appointmentCategory.ShouldNotBeNull();
                            appointmentCategory.Url.ShouldBeOfType<Uri>();
                            appointmentCategory.Value.ShouldBeOfType<CodeableConcept>();

                        }
                    }
                }
            }
        }

        [Then(@"if the appointment booking element is present it is populated with the correct values")]
        public void appointmentBookingMethodIsPresentAndValid()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    foreach (Extension appointmentBooking in appointment.ModifierExtension)
                    {
                        if (appointmentBooking.Url.Equals("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1"))
                        {
                            appointmentBooking.ShouldNotBeNull();
                            appointmentBooking.Url.ShouldBeOfType<Uri>();
                            appointmentBooking.Value.ShouldBeOfType<CodeableConcept>();

                        }

                    }
                }
            }
        }


        [Then(@"if the appointment contact element is present it is populated with the correct values")]
        public void appointmentContactIsPresentAndValid()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    foreach (Extension appointmentContact in appointment.ModifierExtension)
                    {
                        if (appointmentContact.Url.Equals("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1"))
                        {
                            appointmentContact.ShouldNotBeNull();
                            appointmentContact.Url.ShouldBeOfType<Uri>();
                            appointmentContact.Value.ShouldBeOfType<CodeableConcept>();

                        }
                    }
                }

            }
        }

        [Then(@"if the appointment cancellation reason element is present it is populated with the correct values")]
        public void appointmentCancellationIsPresentAndValid()
        {

            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
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
            }
        }

    }
}
    

