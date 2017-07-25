namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Builders.Appointment;
    using Context;
    using Enum;
    using Hl7.Fhir.Model;
    using NUnit.Framework;
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

        [Then(@"the response should be an Appointment resource")]
        public void theResponseShouldBeAnAppointmentResource()
        {
            _fhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.Appointment,
                "The returned resource type was not Appointment");
        }

        [Then(@"the Bundle should contain no Appointments")]
        public void TheBundleShouldContainNoAppointments()
        {
            Appointments.Count.ShouldBe(0, $"The Bundle should contain 0 Appointments, but found {Appointments.Count}.");
        }

        [Then(@"the Bundle should contain a minimum of ""([^""]*)"" Appointments")]
        public void TheResponseBundleShouldContainAtleastAppointments(int minimum)
        {
            Appointments.Count.ShouldBeGreaterThanOrEqualTo(minimum, $"The Bundle should contain a minimum of {minimum} Appointments, but found {Appointments.Count}.");
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
        public void SetCreatedAppointmentStatusTo(string status)
        {
            switch (status) {
                case "Booked":
                    _httpContext.CreatedAppointment.Status = Appointment.AppointmentStatus.Booked;
                    break;
                case "Cancelled":
                    _httpContext.CreatedAppointment.Status = Appointment.AppointmentStatus.Cancelled;
                    break;
            }
        }

        [Given(@"I set the created Appointment priority to ""([^""]*)""")]
        public void  ISetTheCreatedAppointmentPriorityTo(int priority)
        {
            _httpContext.CreatedAppointment.Priority = priority;
        }


        [Given(@"I create an Appointment from the stored Patient and stored Schedule")]
        public void CreateAnAppointmentFromTheStoredPatientAndStoredSchedule()
        {
            var appointmentBuilder = new DefaultAppointmentBuilder(_httpContext);

            _httpContext.CreatedAppointment = appointmentBuilder.BuildAppointment();
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


        [Then(@"the returned appointment participants must contain a type or actor element")]
        public void ThenTheReturnedAppointmentParticipantsMustContainATypeOrActorElement()
        {
            Appointment appointment = (Appointment)_fhirContext.FhirResponseResource;
            foreach (ParticipantComponent participant in appointment.Participant)
            {
                var actor = participant.Actor;
                var type = participant.Type;

                if (null == actor && null == type)
                {
                    Assert.Fail("There must be an actor or type element within the appointment participants");
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
                            coding.System.ShouldBe("http://hl7.org/fhir/ValueSet/encounter-participant-type", "The coding system is incorrect");
                            string[] codes = new string[12] { "translator", "emergency", "ADM", "ATND", "CALLBCK", "CON", "DIS", "ESC", "REF", "SPRF", "PPRF", "PART" };
                            string[] codeDisplays = new string[12] { "Translator", "Emergency", "admitter", "attender", "callback contact", "consultant", "discharger", "escort", "referrer", "secondary performer", "primary performer", "Participation" };
                            coding.Code.ShouldBeOneOf(codes, "The code is incorrect");
                            coding.Display.ShouldBeOneOf(codeDisplays, "The display is incorrect");
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

        [Given(@"I set the URL for the get request with the date ""([^""]*)"" and prefix ""([^""]*)"" for ""([^""]*)""")]
        public void searchAndGetaAppointmentsWithCustomStartDateandPrefix(string startBoundry, string prefix, string patient)
        {

            _httpContext.RequestUrl = _httpContext.RequestUrl + "?start=" + prefix + startBoundry + "";
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
    }
}
