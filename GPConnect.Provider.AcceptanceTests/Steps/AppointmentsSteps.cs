﻿using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using Hl7.Fhir.Model;
using NUnit.Framework;
using System;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using static Hl7.Fhir.Model.Bundle;
using static Hl7.Fhir.Model.Appointment;
using Hl7.Fhir.Serialization;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using Enum;

    [Binding]
    public class AppointmentsSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private readonly HttpSteps HttpSteps;
        private readonly HttpContext HttpContext;
        private readonly JwtSteps _jwtSteps;
        private readonly PatientSteps _patientSteps;
        private readonly GetScheduleSteps _getScheduleSteps;

        public AppointmentsSteps(FhirContext fhirContext, HttpSteps httpSteps, HttpContext httpContext, JwtSteps jwtSteps, PatientSteps patientSteps, GetScheduleSteps getScheduleSteps)
        {
            FhirContext = fhirContext;
            HttpSteps = httpSteps;
            HttpContext = httpContext;
            _jwtSteps = jwtSteps;
            _patientSteps = patientSteps;
            _getScheduleSteps = getScheduleSteps;
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
            Resource patient1 = (Patient)HttpContext.StoredFhirResources[patient];
            string id = patient1.Id.ToString();
            var url = "/Patient/" + id + "/Appointment";
            When($@"I make a GET request to ""{url}""");
        }

        [When(@"I search for patient ""([^""]*)"" and search for the most recently booked appointment ""([^""]*)"" using the stored startDate from the last booked appointment as a search parameter")]
        public void ISearchForPatientAndSearchForTheMostRecentlyBookedAppointmentUsingTheStoredStartDateFromTheLastBookedAppointmentAsASearchParameter(string patient, string appointmentKey)
        {
            Appointment appointment = (Appointment)HttpContext.StoredFhirResources[appointmentKey];
            Resource patient1 = (Patient)HttpContext.StoredFhirResources[patient];
            string id = patient1.Id.ToString();
            var url = "/Patient/" + id + "/Appointment?start=" + appointment.StartElement + "";
            When($@"I make a GET request to ""{url}""");
        }

        [When(@"I search for ""([^""]*)"" and make a get request for their appointments with the date ""([^""]*)""")]
        public void searchAndGetAppointmentsWithCustomStartDate(string patient, string startBoundry)
        {
           
            Resource patient1 = (Patient)HttpContext.StoredFhirResources[patient];
            int time = 1;
            string id = patient1.Id.ToString();
            var url = "/Patient/" + id + "/Appointment?start=" + startBoundry + "";
            When($@"I make a GET request to ""{url}""");
        }

       
        [When(@"I search for ""([^""]*)"" and make a get request for their appointments with the saved slot start date ""([^""]*)"" and prefix ""([^""]*)""")]
        public void searchAndGetAppointmentsWithTheSavedSlotStartDateCustomStartDateandPrefix(string patient, string startBoundry, string prefix)
        {
            string time = HttpContext.StoredDate[startBoundry];
            Resource patient1 = (Patient)HttpContext.StoredFhirResources[patient];
            string id = patient1.Id.ToString();
            var url = "/Patient/" + id + "/Appointment?start=" + prefix + time + "";
            When($@"I make a GET request to ""{url}""");
        }
       
        [When(@"I search for ""([^""]*)"" and make a get request for their appointments with the date ""([^""]*)"" and prefix ""([^""]*)""")]
        public void searchAndGetAppointmentsWithCustomStartDateandPrefix(string patient, string startBoundry, string prefix)
        {
            Resource patient1 = (Patient)HttpContext.StoredFhirResources[patient];
            string id = patient1.Id.ToString();
            var url = "/Patient/" + id + "/Appointment?start="+prefix+ startBoundry + "";
            When($@"I make a GET request to ""{url}""");
        }

        [When(@"I search for ""([^""]*)"" and make a get request for their appointments with lower start date boundry ""([^""]*)"" with prefix ""([^""]*)"" and upper end date boundary ""([^""]*)"" with prefix ""([^""]*)""")]
        public void WhenISearchForAndMakeAGetRequestForTheirAppointmentsWithLowerStartDateBoundryWithPrefixAndUpperEndDateBoundaryWithPrefix(string patient, string startBoundry, string prefixStart, string endBoundry, string prefixEnd)
        {
            Resource patient1 = (Patient)HttpContext.StoredFhirResources[patient];
            string id = patient1.Id.ToString();
            var url = "/Patient/" + id + "/Appointment?start=" + prefixStart + startBoundry+ "&start="+prefixEnd+ endBoundry + "";
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
        
        [Then(@"the appointment resource should contain a status element")]
        public void appointmentMustContainStatusElement()
        {
            ((Appointment)FhirContext.FhirResponseResource).Status.ShouldNotBeNull("Appointment Status is a mandatory element and should be populated but is not in the returned resource.");
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
            appointment.Slot.ShouldNotBeNull("The returned appointment does not contain a slot reference");
            appointment.Slot.Count.ShouldBeGreaterThanOrEqualTo(1, "The returned appointment does not contain a slot reference");
            foreach (var slotReference in appointment.Slot) {
                slotReference.Reference.ShouldStartWith("Slot/", "The returned appointment does not contain a valid slot reference");
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
                }
            }
        }

        [Then(@"all appointments must have a start element which is populated with a date that equals ""([^""]*)""")]
        public void appointmentPopulatedWithAStartDateEquals(string startBoundry)
        {
            //string time = HttpContext.StoredDate[startBoundry];
            DateTimeOffset time = DateTimeOffset.Parse(HttpContext.StoredDate[startBoundry]);

            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    Appointment appointment = (Appointment)entry.Resource;
                    appointment.StartElement.Value.ShouldBe(time);
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
                            }
                        }
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

        [Given(@"I create ""([^""]*)"" Appointments for Patient ""([^""]*)"" and Organization Code ""([^""]*)""")]
        public void CreateAppointmentsForPatientAndOrganizationCode(int appointments, string patient, string code)
        {
            while (appointments != 0)
            {
                CreateAnAppointmentForPatientAndOrganizationCode(patient, code);
                appointments--;
            }
        }


        [Given(@"I create an Appointment for Patient ""([^""]*)"" and Organization Code ""([^""]*)""")]
        public void CreateAnAppointmentForPatientAndOrganizationCode(string patient, string code)
        {
            _patientSteps.GetThePatientForPatientValue(patient);
            _patientSteps.StoreThePatient();

            _getScheduleSteps.GetTheScheduleForOrganizationCode(code);
            _getScheduleSteps.StoreTheSchedule();

            HttpSteps.ConfigureRequest(GpConnectInteraction.AppointmentCreate);

            _jwtSteps.SetTheJwtRequestedRecordToTheNhsNumberOfTheStoredPatient();

            CreateAnAppointmentFromTheStoredPatientAndStoredSchedule();

            HttpSteps.MakeRequest(GpConnectInteraction.AppointmentCreate);
        }

        [Given(@"I store the created Appointment")]
        public void StoreTheCreatedAppointment()
        {
            var appointment = FhirContext.Appointments.FirstOrDefault();

            if (appointment != null)
                HttpContext.CreatedAppointment = appointment;
        }
     

        [Given(@"I create an Appointment from the stored Patient and stored Schedule")]
        public void CreateAnAppointmentFromTheStoredPatientAndStoredSchedule()
        {
            var storedPatient = HttpContext.StoredPatient;
            var storedBundle = HttpContext.StoredBundle;

            var firstSlot = storedBundle.Entry
                .Where(entry => entry.Resource.ResourceType.Equals(ResourceType.Slot))
                .Select(entry => (Slot)entry.Resource)
                .First();

            var schedule = storedBundle.Entry
                .Where(entry =>
                        entry.Resource.ResourceType.Equals(ResourceType.Schedule) &&
                        entry.FullUrl == firstSlot.Schedule.Reference)
                .Select(entry => (Schedule) entry.Resource)
                .First();


            //Patient
            var patient = GetPatient(storedPatient);

            //Practitioners
            var practitionerReferences = schedule.Extension.Select(extension => ((ResourceReference)extension.Value).Reference);
            var practitioners = GetPractitioners(practitionerReferences);

            //Location
            var locationReference = schedule.Actor.Reference;
            var location = GetLocation(locationReference);

            //Participants
            var participants = new List<ParticipantComponent>();
            participants.Add(patient);
            participants.AddRange(practitioners);
            participants.Add(location);

            //Slots
            var slot = GetSlot(firstSlot);

            var slots = new List<ResourceReference>();
            slots.Add(slot);

            var appointment = new Appointment
            {
                Status = AppointmentStatus.Booked,
                Start = firstSlot.Start,
                End = firstSlot.End,
                Participant = participants,
                Slot = slots
            };

            HttpContext.CreatedAppointment = appointment;
        }

        private static ParticipantComponent GetLocation(string locationReference)
        {
            return new ParticipantComponent
            {
                Actor = new ResourceReference
                {
                    Reference = locationReference
                },
                Status = ParticipationStatus.Accepted
            };
        }

        private static ResourceReference GetSlot(Slot firstSlot)
        {
            return new ResourceReference
            {
                Reference = "Slot/" + firstSlot.Id
            };
        }

        private static ParticipantComponent GetPatient(Patient storedPatient)
        {
            return new ParticipantComponent
            {
                Actor = new ResourceReference
                {
                    Reference = "Patient/" + storedPatient.Id
                },
                Status = ParticipationStatus.Accepted,
                Type = new List<CodeableConcept>
                {
                    new CodeableConcept("http://hl7.org/fhir/ValueSet/encounter-participant-type", "SBJ", "patient", "patient")
                }
            };
        }

        private static List<ParticipantComponent> GetPractitioners(IEnumerable<string> practitionerReferences)
        {
            return practitionerReferences
                .Select(practitionerReference => new ParticipantComponent
                {
                    Actor = new ResourceReference
                    {
                        Reference = practitionerReference
                    },
                    Status = ParticipationStatus.Accepted,
                    Type = new List<CodeableConcept>
                    {
                        new CodeableConcept("http://hl7.org/fhir/ValueSet/encounter-participant-type", "PPRF", "practitioner", "practitioner")
                    }
                })
                .ToList();
        }

        [Given(@"I set the created Appointment Comment to ""([^""]*)""")]
        public void SetTheCreatedAppointmentComment(string comment)
        {
            HttpContext.CreatedAppointment.Comment = comment;
        }
    }
}
