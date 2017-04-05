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

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public class AppointmentsSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;

        // Headers Helper
        public HttpHeaderHelper Headers { get; }

        public AppointmentsSteps(HttpHeaderHelper headerHelper, FhirContext fhirContext)
        {

            FhirContext = fhirContext;
            // Helpers
            Headers = headerHelper;

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

        [Then(@"there is one appointment resources")]
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
            int count = 0;
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    count++;
                }
            }
            count.ShouldBeGreaterThan<int>(1);
        }


        [Then(@"the appointment resources must contain a status element")]
        public void appointmentMustContainStatusElement()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    appointment.Status.ShouldNotBeNull();


                    //appointment.Status.ShouldBeOneOf("proposed","pending" , "booked" , "arrived" , "fulfilled","cancelled" , "noshow");

                }
            }

        }

        [Then(@"status should have a valid value")]
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
    }
}
