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
    }
}
    

