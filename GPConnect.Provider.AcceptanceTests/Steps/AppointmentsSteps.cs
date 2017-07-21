namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Context;
    using Enum;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Serialization;
    using NUnit.Framework;
    using RestSharp;
    using Shouldly;
    using TechTalk.SpecFlow;
    using static Hl7.Fhir.Model.Bundle;
    using static Hl7.Fhir.Model.Appointment;

    [Binding]
    public class AppointmentsSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private readonly JwtSteps _jwtSteps;
        private readonly PatientSteps _patientSteps;
        private readonly GetScheduleSteps _getScheduleSteps;
        private List<Appointment> Appointments => _fhirContext.Appointments;

        public AppointmentsSteps(FhirContext fhirContext, HttpSteps httpSteps, HttpContext httpContext, JwtSteps jwtSteps, PatientSteps patientSteps, GetScheduleSteps getScheduleSteps) 
            : base(fhirContext, httpSteps)
        {
            _httpContext = httpContext;
            _jwtSteps = jwtSteps;
            _patientSteps = patientSteps;
            _getScheduleSteps = getScheduleSteps;
        }
        
        [When(@"I search for patient ""([^""]*)"" and search for the most recently booked appointment ""([^""]*)"" using the stored startDate from the last booked appointment as a search parameter")]
        public void ISearchForPatientAndSearchForTheMostRecentlyBookedAppointmentUsingTheStoredStartDateFromTheLastBookedAppointmentAsASearchParameter(string patient, string appointmentKey)
        {
            Appointment appointment = _httpContext.CreatedAppointment;
            Resource patient1 = (Patient)_httpContext.StoredPatient;
            string id = patient1.Id.ToString();
            var url = "/Patient/" + id + "/Appointment?start=" + appointment.StartElement + "";
            When($@"I make a GET request to ""{url}""");
        }

     
        [Given(@"I set the URL for the get request with the date ""([^""]*)"" and prefix ""([^""]*)"" for ""([^""]*)""")]
        public void searchAndGetaAppointmentsWithCustomStartDateandPrefix(string startBoundry, string prefix, string patient)
        {
           
            _httpContext.RequestUrl = _httpContext.RequestUrl +"?start="+prefix+ startBoundry + ""; 
        }

        [Given(@"I set the URL for the get request with the slotStartDate date ""([^""]*)"" and prefix ""([^""]*)"" for ""([^""]*)""")]
        public void searchAndGetaAppointmentsWithCustomStartDatessandPrefix(string slotName, string prefix, string patient)
        {
            var time = _httpContext.CreatedAppointment.StartElement;
            string time2 = time.ToString(); 
            _httpContext.RequestUrl = _httpContext.RequestUrl + "?start=" + prefix + time2 + "";
        }
                
        [Given(@"I set the URL for the get request with the date ""([^""]*)"" and prefix ""([^""]*)"" for and upper end date boundary ""([^""]*)"" with prefix ""([^""]*)"" for ""([^""]*)""")]
        public void IsettheURLforthegetrequestwiththedateandprefixforandupperenddateboundarywithprefixfor(string startBoundry, string prefix, string endBoundry, string prefixEnd, string patient)
        {

            _httpContext.RequestUrl = _httpContext.RequestUrl + "?start=" + prefix + startBoundry + "&start=" + prefixEnd + endBoundry + "";
        }

        [Then(@"there are zero appointment resources")]
        public void checkForEmptyAppointmentsBundle()
        {
            int count = 0;
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Appointment))
                {
                    count++;
                }
            }
            count.ShouldBe<int>(0);
        }

        [Then(@"the response bundle should contain atleast ""([^""]*)"" appointment")]
        public void TheResponseBundleShouldContainAtleastAppointments (int minNumberOfAppointments)
        {
            int appointmentCount = 0;
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
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
            ((Appointment)_fhirContext.FhirResponseResource).Status.ShouldNotBeNull("Appointment Status is a mandatory element and should be populated but is not in the returned resource.");
        }

        [Then(@"the appointment resource should contain a single start element")]
        public void appointmentMustContainStartElement()
        {

            Appointment appointment = (Appointment)_fhirContext.FhirResponseResource;
            appointment.Start.ShouldNotBeNull();


        }
        [Then(@"the appointment resource should contain a single end element")]
        public void appointmentMustContainEndElement()
        {
            Appointment appointment = (Appointment)_fhirContext.FhirResponseResource;
            appointment.End.ShouldNotBeNull();
        }

        [Then(@"the appointment resource should contain at least one slot reference")]
        public void appointmentMustContainSlotReference()
        {
            Appointment appointment = (Appointment)_fhirContext.FhirResponseResource;
            appointment.Slot.ShouldNotBeNull();

        }
        [Then(@"the appointment resource should contain at least one participant")]
        public void appointmentMustContainParticipant()
        {
            Appointment appointment = (Appointment)_fhirContext.FhirResponseResource;
            appointment.Participant.ShouldNotBeNull();


        }

        [Then(@"if the appointment resource contains an identifier it contains a valid system and value")]
        public void appointmentContainsValidIdentifierWithSystemAndValue()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
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

        [Then(@"the appointment response resource contains a status with a valid value")]
        public void ThenTheAppointmentResourceShouldContainAStatus()
        {
            Appointment appointment = (Appointment)_fhirContext.FhirResponseResource;
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
            Appointment appointment = (Appointment)_fhirContext.FhirResponseResource;
            appointment.Start.ShouldNotBeNull();
        }


        [Then(@"the appointment response resource contains an end date")]
        public void ThenTheAppointmentResourceShouldContainAEndDate()
        {
            Appointment appointment = (Appointment)_fhirContext.FhirResponseResource;
            appointment.End.ShouldNotBeNull();
        }

        [Then(@"the appointment response resource contains a slot reference")]
        public void ThenTheAppointmentResourceShouldContainASlotReference()
        {
            Appointment appointment = (Appointment)_fhirContext.FhirResponseResource;
            appointment.Slot.ShouldNotBeNull("The returned appointment does not contain a slot reference");
            appointment.Slot.Count.ShouldBeGreaterThanOrEqualTo(1, "The returned appointment does not contain a slot reference");
            foreach (var slotReference in appointment.Slot) {
                slotReference.Reference.ShouldStartWith("Slot/", "The returned appointment does not contain a valid slot reference");
            }
        }

        [Then(@"if appointment is present the single or multiple participant must contain a type or actor")]
        public void ThenTheAppointmentResourceShouldContainAParticipantWithATypeOrActor()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
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

        [Then(@"if the appointment resource contains a priority the value is valid")]
        public void ThenTheAppointmentResourceContainsPriorityAndTheValueIsValid()
        {
            Appointment appointment = (Appointment)_fhirContext.FhirResponseResource;

            if (null != appointment && (appointment.Priority < 0 || appointment.Priority > 9))
            {
                Assert.Fail("Invalid priority value: " + appointment.Priority);
            }
        }

        [Then(@"all appointments must have an start element which is populated with a valid date")]
        public void appointmentPopulatedWithAValidStartDate()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
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
            DateTimeOffset? time = _httpContext.CreatedAppointment.Start;
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
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
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
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
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
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
                                var practitioner = _httpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner", actor);
                                practitioner.ShouldNotBeNull();
                                countPractitioner++;
                            }
                        }
                    }
                    countPractitioner.ShouldBeGreaterThanOrEqualTo(1);
                }
            }
        }
        
        [Then(@"the patient resource returned in the appointments bundle is present")]
        public void actorPatientResourceValid()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
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
                                var patient = _httpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:patient", actor);
                                patient.ShouldNotBeNull();
                            }
                        }
                    }
                    countPatient.ShouldBeGreaterThanOrEqualTo(1);
                }
            }
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

            _httpSteps.ConfigureRequest(GpConnectInteraction.AppointmentCreate);

            _jwtSteps.SetTheJwtRequestedRecordToTheNhsNumberOfTheStoredPatient();

            CreateAnAppointmentFromTheStoredPatientAndStoredSchedule();

            _httpSteps.MakeRequest(GpConnectInteraction.AppointmentCreate);
        }

        [Given(@"I store the created Appointment")]
        public void StoreTheCreatedAppointment()
        {
            var appointment = _fhirContext.Appointments.FirstOrDefault();

            if (appointment != null)
                _httpContext.CreatedAppointment = appointment;
       
        }

        [Given(@"I store the Appointment")]
        public void StoreTheAppointment()
        {
            var appointment = _fhirContext.Appointments.FirstOrDefault();

            if (appointment != null)
                _httpContext.CreatedAppointment = appointment;
        }

        [Given(@"I store the Appointment Id")]
        public void StoreTheAppointmentId()
        {
            var appointment = _fhirContext.Appointments.FirstOrDefault();

            if (appointment != null)
                _httpContext.GetRequestId = appointment.Id;
        }

        [Given(@"I store the Appointment Version Id")]
        public void StoreThePractitionerVersionId()
        {
            var appointment = _fhirContext.Appointments.FirstOrDefault();
            if (appointment != null)
                _httpContext.GetRequestVersionId = appointment.VersionId;
        }

        [Given(@"I set the created Appointment status to ""([^""]*)""")]
        public void GivenISetCreatedAppointmentStatusTo(string status)
        {
            var appointment = _fhirContext.Appointments.FirstOrDefault();
            switch (status) {
                case "Booked":
                    appointment.Status = Appointment.AppointmentStatus.Booked;
                    break;
                case "Cancelled":
                    appointment.Status = Appointment.AppointmentStatus.Cancelled;
                    break;
            }

            if (appointment != null)
                _httpContext.CreatedAppointment = appointment;
        }

        [Given(@"I set the created Appointment priority to ""([^""]*)""")]
        public void  ISetTheCreatedAppointmentPriorityTo(int priority)
        {
            _httpContext.CreatedAppointment.Priority = priority;
        }


        [Given(@"I create an Appointment from the stored Patient and stored Schedule")]
        public void CreateAnAppointmentFromTheStoredPatientAndStoredSchedule()
        {
            var storedPatient = _httpContext.StoredPatient;
            var storedBundle = _httpContext.StoredBundle;

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
                Status = Appointment.AppointmentStatus.Booked,
                Start = firstSlot.Start,
                End = firstSlot.End,
                Participant = participants,
                Slot = slots
            };

            _httpContext.CreatedAppointment = appointment;
        }

        private static ParticipantComponent GetLocation(string locationReference)
        {
            return new ParticipantComponent
            {
                Actor = new ResourceReference
                {
                    Reference = locationReference
                },
                Status = Appointment.ParticipationStatus.Accepted
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
                Status = Appointment.ParticipationStatus.Accepted,
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
                    Status = Appointment.ParticipationStatus.Accepted,
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
            _httpContext.CreatedAppointment.Comment = comment;
      
        }

        [Given(@"I set the created Appointment reason to ""([^""]*)""")]
        public void SetTheCreatedAppointmentReasonTo(string reason)
        {
            _httpContext.CreatedAppointment.Reason = new CodeableConcept();
            _httpContext.CreatedAppointment.Reason.Coding = new List<Coding>();
            _httpContext.CreatedAppointment.Reason.Coding.Add(new Coding("http://snomed.info/sct", reason, reason));
        }

        [Given(@"I set the created Appointment description to ""([^""]*)""")]
        public void SetTheCreatedAppointmentDescriptionTo(string description)
        {
            _httpContext.CreatedAppointment.Description = description;
      
        }

        [Given(@"I set the Created Appointment to Cancelled with Reason ""([^""]*)""")]
        public void SetTheCreatedAppointmentToCancelledWithReason(string reason)
        {
            var extension = GetCancellationReasonExtension(reason);

            if (_httpContext.CreatedAppointment.Extension == null)
                _httpContext.CreatedAppointment.Extension = new List<Extension>();

            _httpContext.CreatedAppointment.Extension.Add(extension);
            _httpContext.CreatedAppointment.Status = Appointment.AppointmentStatus.Cancelled;
        }

        [Given(@"I set the Created Appointment to Cancelled with Url ""([^""]*)"" and Reason ""([^""]*)""")]
        public void SetTheCreatedAppointmentToCancelledWithUrlAndReason(string url, string reason)
        {
            var extension = GetStringExtension(url, reason);

            if (_httpContext.CreatedAppointment.Extension == null)
                _httpContext.CreatedAppointment.Extension = new List<Extension>();

            _httpContext.CreatedAppointment.Extension.Add(extension);
            _httpContext.CreatedAppointment.Status = Appointment.AppointmentStatus.Cancelled;
        }


        [Given(@"I add a Category Extension with Code ""([^""]*)"" and Display ""([^""]*)"" to the Created Appointment")]
        public void AddACategoryExtensionWithCodeAndDisplayToTheCreatedAppointment(string code, string display)
        {
            var extension = GetCategoryExtension(code, display);

            if (_httpContext.CreatedAppointment.Extension == null)
                _httpContext.CreatedAppointment.Extension = new List<Extension>();

            _httpContext.CreatedAppointment.Extension.Add(extension);
        }

        [Given(@"I add a Booking Method Extension with Code ""([^""]*)"" and Display ""([^""]*)"" to the Created Appointment")]
        public void AddABookingMethodExtensionWithCodeAndDisplayToTheCreatedAppointment(string code, string display)
        {
            var extension = GetBookingMethodExtension(code, display);

            if (_httpContext.CreatedAppointment.Extension == null)
                _httpContext.CreatedAppointment.Extension = new List<Extension>();

            _httpContext.CreatedAppointment.Extension.Add(extension);
        }

        [Given(@"I add a Contact Method Extension with Code ""([^""]*)"" and Display ""([^""]*)"" to the Created Appointment")]
        public void AddAContactMethodExtensionWithCodeAndDisplayToTheCreatedAppointment(string code, string display)
        {
            var extension = GetContactMethodExtension(code, display);

            if (_httpContext.CreatedAppointment.Extension == null)
                _httpContext.CreatedAppointment.Extension = new List<Extension>();

            _httpContext.CreatedAppointment.Extension.Add(extension);
        }


        [Given("I set created appointment to a new appointment resource")]
        public void ISetTheAppointmentResourceToANewAppointmentResource()
        {
            _httpContext.CreatedAppointment = new Appointment();
        }
        private static Extension GetCategoryExtension(string code, string display)
        {
            return GetCodingExtension("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1", code, display);
        }
        private static Extension GetBookingMethodExtension(string code, string display)
        {
            return GetCodingExtension("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1", code, display);
        }

        private static Extension GetContactMethodExtension(string code, string display)
        {
            return GetCodingExtension("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1", code, display);
        }

        private static Extension GetCodingExtension(string url, string code, string display)
        {
            var coding = new Coding
            {
                Code = code,
                Display = display,
                System = url
            };

            var reason = new CodeableConcept();
            reason.Coding.Add(coding);

            return new Extension
            {
                Url = url,
                Value = reason
            };
        }

        private static Extension GetStringExtension(string url, string reason)
        {
            return new Extension
            {
                Url = url,
                Value = new FhirString(reason)
            };
        }

        private static Extension GetCancellationReasonExtension(string reason)
        {
            return GetStringExtension("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1", reason);
        }

        [Given(@"I read the Stored Appointment")]
        public void ReadTheStoredAppointment()
        {
            _httpSteps.ConfigureRequest(GpConnectInteraction.AppointmentRead);

            _jwtSteps.SetTheJwtRequestedRecordToTheNhsNumberOfTheStoredPatient();

            _httpSteps.MakeRequest(GpConnectInteraction.AppointmentRead);
        }

        [Given(@"I set the If-Match header to the Stored Appointment Version Id")]
        public void SetTheIfMatchHeaderToTheStoreAppointmentVersionId()
        {
            var versionId = _httpContext.CreatedAppointment.VersionId;
            var eTag = "W/\"" + versionId + "\"";

            _httpSteps.SetTheIfMatchHeaderTo(eTag);
        }

        [Then(@"the Appointment Metadata should be valid")]
        public void TheAppointmentMetadataShouldBeValid()
        {
            Appointments.ForEach(appointment =>
            {
                CheckForValidMetaDataInResource(appointment, "http://fhir.nhs.net/StructureDefinition/gpconnect-appointment-1");
            });
        }


        [Given(@"I add the ""([^""]*)"" Extensions to the Created Appointment")]
        public void AddTheExtensionsToTheCreatedAppointment(string extensionCombination)
        {
            var extensions = GetExtensions(extensionCombination);

            _httpContext.CreatedAppointment.Extension.AddRange(extensions);
        }

        private List<Extension> GetExtensions(string extensionCombination)
        {
            var extensions = new List<Extension>();
            switch (extensionCombination)
            {
                case "Category":
                    extensions.Add(GetCategoryExtension("CLI", "Clinical"));
                    break;
                case "BookingMethod":
                    extensions.Add(GetBookingMethodExtension("ONL", "Online"));
                    break;
                case "ContactMethod":
                    extensions.Add(GetContactMethodExtension("ONL", "Online"));
                    break;
                case "Category+BookingMethod":
                    extensions.Add(GetCategoryExtension("ADM", "Administrative"));
                    extensions.Add(GetBookingMethodExtension("TEL", "Telephone"));
                    break;
                case "Category+ContactMethod":
                    extensions.Add(GetCategoryExtension("MSG", "Message"));
                    extensions.Add(GetContactMethodExtension("PER", "In person"));
                    break;
                case "BookingMethod+ContactMethod":
                    extensions.Add(GetBookingMethodExtension("LET", "Letter"));
                    extensions.Add(GetContactMethodExtension("EMA", "Email"));
                    break;
                case "Category+BookingMethod+ContactMethod":
                    extensions.Add(GetCategoryExtension("VIR", "Virtual"));
                    extensions.Add(GetBookingMethodExtension("TEX", "Text"));
                    extensions.Add(GetContactMethodExtension("LET", "Letter"));
                    break;
            }

            return extensions;
        }


        private Extension buildAppointmentCategoryExtension(string url, string code, string display)
        {
            Extension extension = new Extension();
            extension.Url = url;
            CodeableConcept codeableConcept = new CodeableConcept();
            Coding coding = new Coding();
            coding.Code = code;
            coding.Display = display;
            codeableConcept.Coding.Add(coding);
            extension.Value = codeableConcept;
            return extension;
        }

        [Given(@"I set the appointment Priority to ""([^""]*)"" on appointment stored against key ""([^""]*)""")]
        public void GivenISetTheAppointmentPriorityToOnAppointmentStoredAgainstKey(int priority, string appointmentKey)
        {
            ((Appointment)_httpContext.StoredFhirResources[appointmentKey]).Priority = priority;
        }

        [When(@"I book the appointment called ""([^""]*)""")]
        public void WhenIBookTheAppointmentCalledString(string appointmentName)
        {
            var appointment = _httpContext.StoredFhirResources[appointmentName];
            if (_httpContext.RequestContentType.Contains("xml"))
            {
                _httpSteps.RestRequest(Method.POST, "/Appointment", FhirSerializer.SerializeToXml(appointment));
            }
            else
            {
                _httpSteps.RestRequest(Method.POST, "/Appointment", FhirSerializer.SerializeToJson(appointment));
            }
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
            if (_httpContext.StoredFhirResources.ContainsKey(storeAppointmentKey))
            {
                _httpContext.StoredFhirResources.Remove(storeAppointmentKey);
            }
            _httpContext.StoredFhirResources.Add(storeAppointmentKey, _fhirContext.FhirResponseResource);
        }

        [Given(@"I create an appointment for patient ""([^""]*)"" called ""([^""]*)"" from schedule ""([^""]*)""")]
        public void GivenICreateAnAppointmentForPatientCalledFromSchedule(string patientKey, string appointmentName, string getScheduleBundleKey)
        {
            Patient patientResource = (Patient)_httpContext.StoredFhirResources[patientKey];
            Bundle getScheduleResponseBundle = (Bundle)_httpContext.StoredFhirResources[getScheduleBundleKey];

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
            List<Extension> extensionList = null;
            List<Identifier> identifiers = null;
            Appointment.AppointmentStatus status = Appointment.AppointmentStatus.Booked;
            int? priority = null;

            switch (appointmentName)
            {
                case "Appointment1":
                    extensionList = new List<Extension>();
                    extensionList.Add(buildAppointmentCategoryExtension("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1", "CLI", "Clinical"));
                    break;

                case "Appointment2":
                    extensionList = new List<Extension>();
                    extensionList.Add(buildAppointmentCategoryExtension("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1", "ONL", "Online"));
                    break;

                case "Appointment3":
                    extensionList = new List<Extension>();
                    extensionList.Add(buildAppointmentCategoryExtension("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1", "ONL", "Online"));
                    break;

                case "Appointment4":
                    extensionList = new List<Extension>();
                    extensionList.Add(buildAppointmentCategoryExtension("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1", "ADM", "Administrative"));
                    extensionList.Add(buildAppointmentCategoryExtension("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1", "TEL", "Telephone"));
                    break;

                case "Appointment5":
                    extensionList = new List<Extension>();
                    extensionList.Add(buildAppointmentCategoryExtension("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1", "PER", "In person"));
                    extensionList.Add(buildAppointmentCategoryExtension("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1", "MSG", "Message"));
                    break;

                case "Appointment6":
                    extensionList = new List<Extension>();
                    extensionList.Add(buildAppointmentCategoryExtension("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1", "LET", "Letter"));
                    extensionList.Add(buildAppointmentCategoryExtension("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1", "EMA", "Email"));
                    break;

                case "Appointment7":
                    extensionList = new List<Extension>();
                    extensionList.Add(buildAppointmentCategoryExtension("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1", "TEX", "Text"));
                    extensionList.Add(buildAppointmentCategoryExtension("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1", "VIR", "Virtual"));
                    extensionList.Add(buildAppointmentCategoryExtension("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1", "LET", "Letter"));
                    break;
            }

            Appointment appointment = new Appointment();
            appointment.Status = status;

            // Appointment Patient Resource
            ParticipantComponent patient = new ParticipantComponent();
            ResourceReference patientReference = new ResourceReference();
            patientReference.Reference = "Patient/" + patientResource.Id;
            patient.Actor = patientReference;
            patient.Status = Appointment.ParticipationStatus.Accepted;
            patient.Type.Add(new CodeableConcept("http://hl7.org/fhir/ValueSet/encounter-participant-type", "SBJ", "patient", "patient"));
            appointment.Participant.Add(patient);

            // Appointment Practitioner Resource
            foreach (var practitionerSlotReference in practitionerReferenceForSelectedSlot)
            {
                ParticipantComponent practitioner = new ParticipantComponent();
                ResourceReference practitionerReference = new ResourceReference();
                practitionerReference.Reference = practitionerSlotReference;
                practitioner.Actor = practitionerReference;
                practitioner.Status = Appointment.ParticipationStatus.Accepted;
                practitioner.Type.Add(new CodeableConcept("http://hl7.org/fhir/ValueSet/encounter-participant-type", "PPRF", "practitioner", "practitioner"));
                appointment.Participant.Add(practitioner);
            }

            // Appointment Location Resource
            ParticipantComponent location = new ParticipantComponent();
            ResourceReference locationReference = new ResourceReference();
            locationReference.Reference = locationReferenceForSelectedSlot;
            location.Actor = locationReference;
            location.Status = Appointment.ParticipationStatus.Accepted;
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
            if (_httpContext.StoredDate.ContainsKey("slotStartDate")) _httpContext.StoredDate.Remove("slotStartDate");
            _httpContext.StoredDate.Add("slotStartDate", firstSlot.StartElement.ToString());

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
            if (_httpContext.StoredFhirResources.ContainsKey(appointmentName)) _httpContext.StoredFhirResources.Remove(appointmentName);
            _httpContext.StoredFhirResources.Add(appointmentName, (Appointment)appointment);
        }
    }
}
