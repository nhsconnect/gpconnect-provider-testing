namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using Builders.Appointment;
    using Context;
    using Enum;
    using Hl7.Fhir.Model;
    using Repository;
    using Shouldly;
    using TechTalk.SpecFlow;
    using static Hl7.Fhir.Model.Appointment;

    [Binding]
    public class AppointmentsSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private readonly JwtSteps _jwtSteps;
        private readonly PatientSteps _patientSteps;
        private readonly GetScheduleSteps _getScheduleSteps;
        private readonly HttpRequestConfigurationSteps _httpRequestConfigurationSteps;
        private readonly IFhirResourceRepository _fhirResourceRepository;

        private List<Appointment> Appointments => _httpContext.FhirResponse.Appointments;

        public AppointmentsSteps(
            HttpSteps httpSteps,
            HttpContext httpContext,
            JwtSteps jwtSteps, 
            PatientSteps patientSteps, 
            GetScheduleSteps getScheduleSteps, 
            HttpRequestConfigurationSteps httpRequestConfigurationSteps, 
            IFhirResourceRepository fhirResourceRepository) 
            : base(httpSteps)
        {
            _httpContext = httpContext;
            _jwtSteps = jwtSteps;
            _patientSteps = patientSteps;
            _getScheduleSteps = getScheduleSteps;
            _httpRequestConfigurationSteps = httpRequestConfigurationSteps;
            _fhirResourceRepository = fhirResourceRepository;
        }

        [Then(@"the Response Resource should be an Appointment")]
        public void TheResponseResourceShouldBeAnAppointment()
        {
            _httpContext.FhirResponse.Resource.ResourceType.ShouldBe(ResourceType.Appointment, "the Response Resource should be an Appointment.");
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

        [Then("the Appointment Id should be valid")]
        public void TheAppointmentIdShouldBeValid()
        {
            Appointments.ForEach(appointment =>
            {
                appointment.Id.ShouldNotBeNullOrEmpty($"The Appointment Id should not be null or empty but was {appointment.Id}.");
            });
        }

        [Then(@"the Appointment Start should equal the Created Appointment Start")]
        public void TheAppointmentStartShouldEqualTheCreatedAppointmentStart()
        {
            Appointments.ForEach(appointment =>
            {
                appointment.Start.ShouldBe(_fhirResourceRepository.Appointment.Start, $"The Appointment Start should equal {_fhirResourceRepository.Appointment.Start} but was {appointment.Start}.");
            });
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

        [Given(@"I store the Created Appointment")]
        public void StoreTheCreatedAppointment()
        {
            StoreTheAppointment();
        }

        [Given(@"I store the Appointment")]
        [Then(@"I store the Appointment")]
        public void StoreTheAppointment()
        {
            var appointment = _httpContext.FhirResponse.Appointments.FirstOrDefault();

            if (appointment != null)
            {
                _httpContext.HttpRequestConfiguration.GetRequestId = appointment.Id;
                _fhirResourceRepository.Appointment = appointment;
            }
        }

        [Given(@"I store the Appointment Version Id")]
        public void StoreThePractitionerVersionId()
        {
            var appointment = _httpContext.FhirResponse.Appointments.FirstOrDefault();
            if (appointment != null)
                _httpContext.HttpRequestConfiguration.GetRequestVersionId = appointment.VersionId;
        }

        [Given(@"I set the Created Appointment Status to ""([^""]*)""")]
        public void SetCreatedAppointmentStatusTo(string status)
        {
            switch (status)
            {
                case "Booked":
                    _fhirResourceRepository.Appointment.Status = AppointmentStatus.Booked;
                    break;
                case "Cancelled":
                    _fhirResourceRepository.Appointment.Status = AppointmentStatus.Cancelled;
                    break;
            }
        }

        [Given(@"I create an Appointment from the stored Patient and stored Schedule")]
        public void CreateAnAppointmentFromTheStoredPatientAndStoredSchedule()
        {
            var appointmentBuilder = new DefaultAppointmentBuilder(_fhirResourceRepository);

            _fhirResourceRepository.Appointment = appointmentBuilder.BuildAppointment();
        }

        [Given(@"I set the Created Appointment Reason to ""([^""]*)""")]
        public void SetTheCreatedAppointmentReasonTo(string reason)
        {
            _fhirResourceRepository.Appointment.Reason = new CodeableConcept
            {
                Coding = new List<Coding>
                {
                    new Coding("http://snomed.info/sct", reason, reason)
                }
            };
        }

        [Given(@"I set the Created Appointment to Cancelled with Reason ""([^""]*)""")]
        public void SetTheCreatedAppointmentToCancelledWithReason(string reason)
        {
            var extension = GetCancellationReasonExtension(reason);

            if (_fhirResourceRepository.Appointment.Extension == null)
                _fhirResourceRepository.Appointment.Extension = new List<Extension>();

            _fhirResourceRepository.Appointment.Extension.Add(extension);
            _fhirResourceRepository.Appointment.Status = AppointmentStatus.Cancelled;
        }

        [Given(@"I set the Created Appointment to Cancelled with Url ""([^""]*)"" and Reason ""([^""]*)""")]
        public void SetTheCreatedAppointmentToCancelledWithUrlAndReason(string url, string reason)
        {
            var extension = GetStringExtension(url, reason);

            if (_fhirResourceRepository.Appointment.Extension == null)
                _fhirResourceRepository.Appointment.Extension = new List<Extension>();

            _fhirResourceRepository.Appointment.Extension.Add(extension);
            _fhirResourceRepository.Appointment.Status = AppointmentStatus.Cancelled;
        }

        [Given(@"I add a Category Extension with Code ""([^""]*)"" and Display ""([^""]*)"" to the Created Appointment")]
        public void AddACategoryExtensionWithCodeAndDisplayToTheCreatedAppointment(string code, string display)
        {
            var extension = GetCategoryExtension(code, display);

            if (_fhirResourceRepository.Appointment.Extension == null)
                _fhirResourceRepository.Appointment.Extension = new List<Extension>();

            _fhirResourceRepository.Appointment.Extension.Add(extension);
        }

        [Given(@"I add a Booking Method Extension with Code ""([^""]*)"" and Display ""([^""]*)"" to the Created Appointment")]
        public void AddABookingMethodExtensionWithCodeAndDisplayToTheCreatedAppointment(string code, string display)
        {
            var extension = GetBookingMethodExtension(code, display);

            if (_fhirResourceRepository.Appointment.Extension == null)
                _fhirResourceRepository.Appointment.Extension = new List<Extension>();

            _fhirResourceRepository.Appointment.Extension.Add(extension);
        }

        [Given(@"I add a Contact Method Extension with Code ""([^""]*)"" and Display ""([^""]*)"" to the Created Appointment")]
        public void AddAContactMethodExtensionWithCodeAndDisplayToTheCreatedAppointment(string code, string display)
        {
            var extension = GetContactMethodExtension(code, display);

            if (_fhirResourceRepository.Appointment.Extension == null)
                _fhirResourceRepository.Appointment.Extension = new List<Extension>();

            _fhirResourceRepository.Appointment.Extension.Add(extension);
        }

        [Given("I set the Created Appointment to a new Appointment")]
        public void SetTheCreatedAppointmentToANewAppointment()
        {
            _fhirResourceRepository.Appointment = new Appointment();
        }

        private static Extension GetCategoryExtension(string code, string display)
        {
            return GetCodingExtension("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1",
                                        "http://fhir.nhs.net/ValueSet/gpconnect-appointment-category-1", code, display);
        }

        private static Extension GetBookingMethodExtension(string code, string display)
        {
            return GetCodingExtension("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1",
                                        "http://fhir.nhs.net/ValueSet/gpconnect-appointment-booking-method-1", code, display);
        }

        private static Extension GetContactMethodExtension(string code, string display)
        {
            return GetCodingExtension("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1",
                                        "http://fhir.nhs.net/ValueSet/gpconnect-appointment-contact-method-1", code, display);
        }

        private static Extension GetInvalidMethodExtension(string code, string display)
        {
            return GetCodingExtension("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-invalid-method-0000",
                "http://fhir.nhs.net/ValueSet/gpconnect-appointment-invalid-method-0000", code, display);
        }

        private static Extension GetCodingExtension(string extensionUrl, string codingUrl, string code, string display)
        {
            var coding = new Coding
            {
                Code = code,
                Display = display,
                System = codingUrl
            };

            var reason = new CodeableConcept();
            reason.Coding.Add(coding);

            return new Extension
            {
                Url = extensionUrl,
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
            var versionId = _fhirResourceRepository.Appointment.VersionId;
            var eTag = "W/\"" + versionId + "\"";

            _httpRequestConfigurationSteps.SetTheIfMatchHeaderTo(eTag);
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

            _fhirResourceRepository.Appointment.Extension.AddRange(extensions);
        }

        private static List<Extension> GetExtensions(string extensionCombination)
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
                case "Category+InvalidMethod":
                    extensions.Add(GetCategoryExtension("VIR", "Virtual"));
                    extensions.Add(GetInvalidMethodExtension("INV", "Invalid Method"));
                    break;
                case "Category+InvalidContactMethod+InvalidBookingMethod":
                    extensions.Add(GetCategoryExtension("MSG", "Message"));
                    extensions.Add(GetBookingMethodExtension("LET", "Letterer"));
                    extensions.Add(GetContactMethodExtension("PERS", "In person"));
                    break;
                case "InvalidCategory+InvalidContactMethod+BookingMethod":
                    extensions.Add(GetCategoryExtension("MSGED", "Message"));
                    extensions.Add(GetBookingMethodExtension("LET", "Letter"));
                    extensions.Add(GetContactMethodExtension("PERS", "In person"));
                    break;
            }

            return extensions;
        }

        [Then(@"the Appointment Participant Type and Actor should be valid")]
        public void TheAppointmentParticipantTypeAndActorShouldBeValid()
        {
            Appointments.ForEach(appointment =>
            {
                appointment.Participant.ForEach(participant =>
                {
                    var hasActorOrType = participant.Actor == null && participant.Type == null;

                    hasActorOrType.ShouldBeFalse("The Appointment Participant should have an Actor or Type, but has neither.");

                    if (participant.Type != null)
                    {
                        participant.Type.Count.ShouldBeLessThanOrEqualTo(1, $"The Appointment Participant should contain a maximum of 1 Type, but found {participant.Type.Count}.");

                        participant.Type.ForEach(type =>
                        {
                            type.Coding.Count.ShouldBeLessThanOrEqualTo(1, $"The Appointment Participant Type should contain a maximum of 1 Coding, but found {type.Coding.Count}.");

                            type.Coding.ForEach(coding =>
                            {
                                const string codingSystem = "http://hl7.org/fhir/ValueSet/encounter-participant-type";
                                coding.System.ShouldBe(codingSystem, $"The Appointment Participant Type Coding System should be {codingSystem}, but was {coding.System}.");

                                ParticipantTypeDictionary.ShouldContainKey(coding.Code, $"The Appointment Appointment Participant Type Coding Code {coding.Code} was not valid.");
                                ParticipantTypeDictionary.ShouldContainKeyAndValue(coding.Code, coding.Display, $"The Appointment Appointment Participant Type Coding Display {coding.Code} was not valid.");
                            });
                        });
                    }

                    if (participant.Actor?.Reference != null)
                    {
                        participant.Actor.Reference.ShouldNotBeEmpty();

                        const string patient = "Patient/";
                        const string practitioner = "Practitioner/";
                        const string location = "Location/";

                        var shouldStartWith = participant.Actor.Reference.StartsWith(patient) ||
                                              participant.Actor.Reference.StartsWith(practitioner) ||
                                              participant.Actor.Reference.StartsWith(location);
                       
                        shouldStartWith.ShouldBeTrue($"The Appointment Participant Actor Reference should start with one of {patient}, {practitioner} or {location}, but was {participant.Actor.Reference}.");
                    }
                });
            });
        }

        [Given(@"I add a query parameter to the Request URL with Prefix ""([^""]*)"" for Start ""([^""]*)""")]
        public void AddAQueryParameterToTheRequestUrlWithPrefixForStart(string prefix, string start)
        {
            _httpContext.HttpRequestConfiguration.RequestUrl = $"{_httpContext.HttpRequestConfiguration.RequestUrl}?start={prefix}{start}";
        }

        [Given(@"I add a query parameter to the Request URL with Prefix ""([^""]*)"" for Start ""([^""]*)"" and Prefix ""([^""]*)"" for End ""([^""]*)""")]
        public void AddAQueryParameterToTheRequestUrlWithPrefixForStartAndPrefixForEnd(string prefix, string start, string endPrefix, string end)
        {
            _httpContext.HttpRequestConfiguration.RequestUrl = $"{_httpContext.HttpRequestConfiguration.RequestUrl}?start={prefix}{start}&start={endPrefix}{end}";
        }

        [Given(@"I add a query parameter to the Request URL with Prefix ""([^""]*)"" for the Created Appointment Start")]
        public void AddAQueryParameterToTheRequestUrlWithPrefixForStoredAppointmentStart(string prefix)
        {
            _httpContext.HttpRequestConfiguration.RequestUrl = $"{_httpContext.HttpRequestConfiguration.RequestUrl}?start={prefix}{_fhirResourceRepository.Appointment.StartElement}";
        }

        private static Dictionary<string, string> ParticipantTypeDictionary => new Dictionary<string, string>
        {
            { "translator", "Translator"},
            { "emergency",  "Emergency"},
            { "ADM", "admitter"},
            { "ATND", "attender"},
            { "CALLBCK", "callback contact"},
            { "CON", "consultant"},
            { "DIS", "discharger"},
            { "ESC", "escort"},
            { "REF", "referrer"},
            { "SPRF", "secondary performer"},
            { "PPRF", "primary performer"},
            { "PART", "Participation"}
        };
    }
}
