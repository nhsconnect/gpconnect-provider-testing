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

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public class AppointmentsSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private readonly HttpSteps HttpSteps;
        private readonly HttpContext HttpContext;
   

        // Headers Helper
        public HttpHeaderHelper Headers { get; }

        public AppointmentsSteps(HttpHeaderHelper headerHelper, FhirContext fhirContext, HttpSteps httpSteps, HttpContext httpContext)
        {
            // Helpers
            FhirContext = fhirContext;
            Headers = headerHelper;
            HttpSteps = httpSteps;
            HttpContext = httpContext;

        }

        [Given(@"I search for an appointments for patient ""([^""]*)"" on the provider system and save the first response to ""([^""]*)""")]
        public void GivenISearchForAnAppointmentOnTheProviderSystemAndSaveTheFirstResponseTo(int id, string storeKey)
        {

            var relativeUrl = "/Patient/"+id+"/Appointment";
            var returnedResourceBundle = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments", relativeUrl);
            returnedResourceBundle.GetType().ShouldBe(typeof(Bundle));
            ((Bundle)returnedResourceBundle).Entry.Count.ShouldBeGreaterThan(0);
            var returnedFirstResource = (Appointment)((Bundle)returnedResourceBundle).Entry[0].Resource;
            string text = returnedFirstResource.ToString();
            returnedFirstResource.GetType().ShouldBe(typeof(Appointment));
            if (HttpContext.StoredFhirResources.ContainsKey(storeKey)) HttpContext.StoredFhirResources.Remove(storeKey);
            HttpContext.StoredFhirResources.Add(storeKey, returnedFirstResource);
        }

        [Given(@"I search for an appointments for patient ""([^""]*)"" on the provider system and if zero booked i book ""([^""]*)"" appointment")]
        public void GivenISearchForAnAppointmentOnTheProviderSystemAndBookAppointment(int id, int numOfAppointments)
        {
         
            var relativeUrl = "/Patient/" + id + "/Appointment";
            var returnedResourceBundle = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments", relativeUrl);
            returnedResourceBundle.GetType().ShouldBe(typeof(Bundle));
            if (((Bundle)returnedResourceBundle).Entry.Count == 0)
            {

                Then($@"I find a patient with id ""{id}"" and search for a slot and create ""{numOfAppointments}"" appointment");
            }
            else { Assert.Pass(); }

           
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


        [Then(@"the bundle appointment resource should contain a single status element")]
        public void appointmentMustContainStatusElement()
        {
            
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                int count = 0;

                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    appointment.Status.ShouldNotBeNull();
                    count++;

                    
                }
                count.ShouldBe<int>(1);
            }
          

        }
        [Then(@"the bundle appointment resource should contain a single start element")]
        public void appointmentMustContainStartElement()
        {
            int count = 0;
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {

                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    appointment.Start.ShouldNotBeNull();
                    count++;
                }
            }
            count.ShouldBe<int>(1);

        }
        [Then(@"the bundle appointment resource should contain a single end element")]
        public void appointmentMustContainEndElement()
        {
            int count = 0;
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {

                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    appointment.End.ShouldNotBeNull();
                    count++;

                }
            }
            count.ShouldBe<int>(1);

        }
        [Then(@"the bundle appointment resource should contain at least one slot reference")]
        public void appointmentMustContainSlotReference()
        {
            int count = 0;
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {

                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    appointment.Slot.ShouldNotBeNull();
                    count++;

                }
            }
            count.ShouldBeGreaterThanOrEqualTo<int>(1);

        }
        [Then(@"the bundle appointment resource should contain at least one participant")]
        public void appointmentMustContainParticipant()
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
            count.ShouldBeGreaterThanOrEqualTo<int>(1);

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

            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
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
            }
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

        [Then(@"if appointment is present the single or multiple participant must contain a type or actor")]
        public void ThenTheAppointmentResourceShouldContainAParticipantWithATypeOrActor()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                     foreach (ParticipantComponent part in appointment.Participant)
                    {
                        string actor = part.Actor.ToString();
                        string type = part.Type.ToString();

                        Log.WriteLine(actor);
                        Log.WriteLine(type);
                        if (null == actor && null == type)
                        {
                            Assert.Fail();
                        }
                    }
                 }
            }
        }

        [Then(@"the appointment response resource contains an identifier with a valid system and value")]
        public void ThenTheAppointmentResourceShouldContainAnIdentifier()
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
            if (appointment.Priority < 0 || appointment.Priority > 9)
            {
                Assert.Fail();
            }
        }
        //Need to check the validity of the reference but currently no GET method
        [Then(@"the slot reference is present and valid")]
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
               
                    }
                }
            }
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
                            appointmentCategory.Url.ShouldBeOfType<Uri>();
                            appointmentCategory.Value.ShouldBeOfType<CodeableConcept>();
                            appointmentCategory.ShouldNotBeNull();


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
                            appointmentBooking.Url.ShouldBeOfType<Uri>();
                            appointmentBooking.Value.ShouldBeOfType<CodeableConcept>();
                            appointmentBooking.Value.ShouldNotBeNull();
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

                            appointmentContact.Url.ShouldBeOfType<Uri>();
                            appointmentContact.Value.ShouldBeOfType<CodeableConcept>();
                            appointmentContact.Value.ShouldNotBeNull();

                        }

                    }
                }
            }
        }

        [Then(@"if the appointment cancellation reason  element is present it is populated with the correct values")]
        public void appointmentCancellationIsPresentAndValid()
        {
      
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
         
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    int extensionCount = 0;
                    Appointment appointment = (Appointment)entry.Resource;


                    foreach (Extension appointmentCancellationReason in appointment.ModifierExtension)
                    {
                    
                        if (appointmentCancellationReason.Url.Equals("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1-0"))
                        {
                       
                            appointmentCancellationReason.Url.ShouldBeOfType<Uri>();
                            appointmentCancellationReason.Value.ShouldBeOfType<String>();
                            appointmentCancellationReason.Value.ShouldNotBeNull();

                            extensionCount++;
                        }
                    }
                    extensionCount.ShouldBe(1);
                }
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
                    appointment.Start.ShouldBeOfType<DateTimeOffset>();

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
                    appointment.End.ShouldBeOfType<DateTimeOffset>();

                }
            }
        }

        [Then(@"if actor returns a practitioner resource the resource is valid")]
        public void actorPractitionerResourceValid()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    int countPractitioner = 0;
                    foreach (ParticipantComponent part in appointment.Participant)
                    {
                        string actor = part.Actor.Reference.ToString();
                       
                        if (actor.Contains("Practitioner"))
                        {
                            var practitioner = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner", actor);
                            practitioner.ShouldNotBeNull();
                            countPractitioner++;

                        }
                      
                    }
                    countPractitioner.ShouldBe(1);

                }

            }
        }

        [Then(@"if actor returns a location resource the resource is valid")]
        public void actorLocationResourceValid()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    int countLocation = 0;
                    foreach (ParticipantComponent part in appointment.Participant)
                    {
                        string actor = part.Actor.Reference.ToString();
                      
                        if (actor.Contains("Location"))
                        {
                            var location = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:location", actor);
                            location.ShouldNotBeNull();
                            countLocation++;
                        }
                  
                    }
                    countLocation.ShouldBe(1);
                }

            }
        }

      

        [Then(@"if actor returns a patient resource the resource is valid")]
        public void actorPatientResourceValid()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    int countPatient = 0;
                    foreach (ParticipantComponent part in appointment.Participant)
                    {
                        string actor = part.Actor.Reference.ToString();
                
                        if (actor.Contains("Patient"))
                        {
                            countPatient++;
                            var patient = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:patient", actor);
                            patient.ShouldNotBeNull();
                        }
                        
                    }
                    countPatient.ShouldBe(1);

                }

            }
        }

        [Then(@"I find a patient with id ""(.*)"" and search for a slot and create ""(.*)"" appointment")]
        public void bookAppointmentForPatient(int id, int numOfAppointments)
        {
            for (int i = 0; i < numOfAppointments; i++)
            {
                Log.WriteLine(numOfAppointments.ToString());
                Appointment appointment = new Appointment();

                var patientResource = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:patient", "/Patient/1");

                //Patient Resource
                ParticipantComponent patient = new ParticipantComponent();
                ResourceReference patientReference = new ResourceReference();
                patientReference.Reference = "Patient/" + id;

                Code code = new Code();
                code.Equals("accepted");
                ParticipationStatus stat = new ParticipationStatus();

                patient.Status = stat;
                patient.Actor = patientReference;
                appointment.Participant.Add(patient);

                //Practitioner Resource
                ParticipantComponent practitioner = new ParticipantComponent();
                ResourceReference practitionerReference = new ResourceReference();
                practitionerReference.Reference = "Practitioner/2";
                practitioner.Actor = practitionerReference;
                appointment.Participant.Add(practitioner);

                //Location Resource
                ParticipantComponent location = new ParticipantComponent();
                ResourceReference locationReference = new ResourceReference();
                locationReference.Reference = "Location/3";
                location.Actor = locationReference;
                appointment.Participant.Add(location);

                //Slot Resource
                ResourceReference slot = new ResourceReference();
                slot.Reference = "Slot/40";
                appointment.Slot.Add(slot);

                //AppointmentResources

                DateTime start = new DateTime(2017, 4, 20, 7, 0, 0);
                DateTime end = new DateTime(2017, 4, 20, 7, 0, 0);
                appointment.Start = start;
                appointment.End = end;

                AppointmentStatus status = new AppointmentStatus();
                appointment.Status = status;

                //Book the appointment
                HttpSteps.bookAppointment("urn:nhs:names:services:gpconnect:fhir:rest:create:appointment", "/Appointment", appointment);

                //var appointmentResource = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments", "/Patient/2/Appointment");
                //Log.WriteLine(appointmentResource);
            }
        }
    }
}
    

