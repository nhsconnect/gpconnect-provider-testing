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

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public class AppointmentsSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private Appointment.AppointmentStatus[] appointmentStatusValues;

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


        [Then(@"the bundle appointment resource should contain contain a single status element")]
        public void appointmentMustContainStatusElement()
        {
            int count = 0;
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {

                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    appointment.Status.ShouldNotBeNull();
                    count++;

                }
            }
            count.ShouldBe<int>(1);

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
                        Assert.Pass();
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

                    if (appointment.Reason.Coding == null)
                    {
                        Assert.Pass();
                    }
                    else
                    {
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
                        Assert.Pass();
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
                    if (appointment.Identifier == null)
                    {
                        Assert.Pass();
                    }
                    else
                    {
                        if (appointment.Identifier != null)
                        {
                            int identifierCount = 0;
                            foreach (Identifier identifer in appointment.Identifier)
                            {
                                identifierCount++;
                                identifer.Value.ShouldNotBeNullOrEmpty();
                                if (identifer.System != null)
                                {
                                    identifer.System.ShouldBe("http://fhir.nhs.net/Id/gpconnect-appointment-identifier");
                                }

                            }

                        }
                    }
                }

            }
        }
        
        [Then(@"the appointment shall contain a slot or multiple slots")]
        public void appointmentMustContainSlot()
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

        [Then(@"if the bundle contains a appointment resource the start and end date days are within range ""(.*)"" days")]
        public void appointmentDaysAreWithinRange(int days)
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    DateTimeOffset? start = appointment.Start;
                    DateTimeOffset? end = appointment.End;

                    string dayDays = start?.ToString("dd");
                    string endDays = end?.ToString("dd");

                    dayDays.ShouldNotBeNullOrEmpty();
                    endDays.ShouldNotBeNullOrEmpty();

                    int x = Int32.Parse(dayDays);
                    int y = Int32.Parse(endDays);
                    //Checks Upper and lower limits, doesnt account for month
                    if (x < 0 || x > 31) { Assert.Fail(); }
                    if (y < 0 || y > 31) { Assert.Fail(); }
                    //Checks the range is not out of bounds
                    if (y - x > days) { Assert.Fail(); }
                
                }
            }
        }

        [Then(@"if the bundle contains a appointment resource the start and end date months are within range ""(.*)"" months")]
        public void appointmentMonthsAreWithinRange(int months)
        {
       
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    DateTimeOffset? start = appointment.Start;
                    DateTimeOffset? end = appointment.End;

                    string dayDays = start?.ToString("MM");
                    string endDays = end?.ToString("MM");

                    dayDays.ShouldNotBeNullOrEmpty();
                    endDays.ShouldNotBeNullOrEmpty();

                    int x = Int32.Parse(dayDays);
                    int y = Int32.Parse(endDays);

                    if (x < 0 || x > 12) { Assert.Fail(); }
                    if (y < 0 || y > 12) { Assert.Fail(); }
                    //Checks the range is not out of bounds
                    if (y - x > months) { Assert.Fail(); }
                }
            }
        }
        [Then(@"if the bundle contains a appointment resource the start and end date years are within range ""(.*)"" years")]
        public void appointmentYearsAreWithinRange(int years)
        {
          
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    DateTimeOffset? start = appointment.Start;
                    DateTimeOffset? end = appointment.End;

                    string dayDays = start?.ToString("yyyy");
                    string endDays = end?.ToString("yyyy");

                    dayDays.ShouldNotBeNullOrEmpty();
                    endDays.ShouldNotBeNullOrEmpty();

                    int x = Int32.Parse(dayDays);
                    int y = Int32.Parse(endDays);

                    if (x < 2016 || x > 2018) { Assert.Fail(); }
                    if (y < 2016 || y > 2018) { Assert.Fail(); }
                    //Checks the range is not out of bounds
                    if (y - x > years) { Assert.Fail(); }

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

        [Then(@"the appointment response resource contains an id")]
        public void ThenTheAppointmentResourceShouldContainAnId()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            appointment.Id.ShouldNotBeNull();
            int idCount = 0;
            foreach (char id in appointment.Id)
            {
                idCount++;
            }
            idCount.ShouldBe(1);
        }



        [Then(@"the appointment response resource should contain meta data profile and version id")]
        public void ThenTheAppointmentResourceShouldContainMetaDataProfile()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
          
            appointment.Meta.ShouldNotBeNull();
            int metaProfileCount = 0;
            foreach (string profile in appointment.Meta.Profile)
            {
                metaProfileCount++;
                profile.ShouldBe("http://fhir.nhs.net/StructureDefinition/gpconnect-appointment-1");
            }
            metaProfileCount.ShouldBe(1);
            appointment.Meta.VersionId.ShouldNotBeNull();
        }


        [Then(@"the appointment response resource contains a status with a valid value")]
        public void ThenTheAppointmentResourceShouldContainAStatus()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            appointment.Status.ShouldNotBeNull();
            string statusValue = appointment.Status.Value.ToString();
            if (statusValue != "Booked" && statusValue != "Pending" && statusValue != "Arrived" && statusValue != "Fufilled" && statusValue != "Cancelled" && statusValue != "Noshow")
            {
                Assert.Fail();
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
            
        }


        [Then(@"the appointment response resource contains a participant which contains a status with a valid value")]
        public void ThenTheAppointmentResourceShouldContainAParticipant()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            appointment.Participant.ShouldNotBeNull();
            foreach (Appointment.ParticipantComponent participant in appointment.Participant)
            {
                string status = participant.Status.ToString();
                if (status != "Accepted" && status != "Declined" && status != "Tentative" && status != "Needs-action")
                {
                    Assert.Fail();
                }
            }

        }

        
    }
}

