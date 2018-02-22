namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using Builders.Appointment;
    using Constants;
    using Context;
    using Enum;
    using Hl7.Fhir.Model;
    using Repository;
    using Shouldly;
    using TechTalk.SpecFlow;
    using static System.Net.WebUtility;
    using static Hl7.Fhir.Model.Appointment;
    using System;
    using GPConnect.Provider.AcceptanceTests.Helpers;

    [Binding]
    public class AppointmentsSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private readonly JwtSteps _jwtSteps;
        private readonly PatientSteps _patientSteps;
        private readonly SearchForFreeSlotsSteps _searchForFreeSlotsSteps;
        private readonly HttpRequestConfigurationSteps _httpRequestConfigurationSteps;
        private readonly IFhirResourceRepository _fhirResourceRepository;
 

        private List<Appointment> Appointments => _httpContext.FhirResponse.Appointments;

        public AppointmentsSteps(
            HttpSteps httpSteps,
            HttpContext httpContext,
            JwtSteps jwtSteps, 
            PatientSteps patientSteps,
            SearchForFreeSlotsSteps searchForFreeSlotsSteps, 
            HttpRequestConfigurationSteps httpRequestConfigurationSteps, 
            IFhirResourceRepository fhirResourceRepository) 
            : base(httpSteps)
        {
            _httpContext = httpContext;
            _jwtSteps = jwtSteps;
            _patientSteps = patientSteps;
            _searchForFreeSlotsSteps = searchForFreeSlotsSteps;
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
          
            _searchForFreeSlotsSteps.GetAvailableFreeSlots();
            _searchForFreeSlotsSteps.StoreTheFreeSlotsBundle();

            _httpSteps.ConfigureRequest(GpConnectInteraction.AppointmentCreate);

            _jwtSteps.SetTheJwtRequestedRecordToTheNhsNumberOfTheStoredPatient();

            CreateAnAppointmentFromTheStoredPatientAndStoredSchedule();

            _httpSteps.MakeRequest(GpConnectInteraction.AppointmentCreate);

        }

        [Given(@"I create an Appointment for an existing Patient and Organization Code ""([^""]*)""")]
        public void CreateAnAppointmentForRandomPatientAndOrganizationCode(string code)
        { 

             var patient = "patient1";

            if (AppSettingsHelper.RandomPatientEnabled == true) {
                 patient = RandomPatientSteps.ReturnRandomPatient();
            }
        
            _patientSteps.GetThePatientForPatientValue(patient);
            _patientSteps.StoreThePatient();

            _searchForFreeSlotsSteps.GetAvailableFreeSlots();
            _searchForFreeSlotsSteps.StoreTheFreeSlotsBundle();

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

        [Given("I set the Created Appointment to a new Appointment")]
        public void SetTheCreatedAppointmentToANewAppointment()
        {
            _fhirResourceRepository.Appointment = new Appointment();
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
            return GetStringExtension(FhirConst.StructureDefinitionSystems.kAppointmentCancellationReason, reason);
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
                CheckForValidMetaDataInResource(appointment, FhirConst.StructureDefinitionSystems.kAppointment);
            });
        }
      
        [Then(@"the Appointment Participant Type and Actor should be valid")]
        public void TheAppointmentParticipantTypeAndActorShouldBeValid()
        {
            Appointments.ForEach(appointment =>
            {
                appointment.Participant.ForEach(participant =>
                {

                    participant.Actor.ShouldNotBeNull("Participant Actor Should Not Be null");

                    participant.Type?.ForEach(type =>
                    {
                        type.Coding.ForEach(coding =>
                        {
                            type.Coding.Count.ShouldBeLessThanOrEqualTo(1,
                                $"The Appointment Participant Type should contain a maximum of 1 Coding, but found {type.Coding.Count}.");

                        });
                    });

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
            var startKey = UrlEncode("start");
            var startValue = UrlEncode($"{prefix}{start}");

            _httpContext.HttpRequestConfiguration.RequestUrl = $"{_httpContext.HttpRequestConfiguration.RequestUrl}?{startKey}={startValue}";
        }

        [Given(@"I add a query parameter to the Request URL with Prefix ""([^""]*)"" for Start ""([^""]*)"" and Prefix ""([^""]*)"" for End ""([^""]*)""")]
        public void AddAQueryParameterToTheRequestUrlWithPrefixForStartAndPrefixForEnd(string prefix, string start, string endPrefix, string end)
        {
            var startKey = UrlEncode("start");
            var startValue = UrlEncode($"{prefix}{start}");
            var endValue = UrlEncode($"{endPrefix}{end}");

            _httpContext.HttpRequestConfiguration.RequestUrl = $"{_httpContext.HttpRequestConfiguration.RequestUrl}?{startKey}={startValue}&{startKey}={endValue}";
        }

        [Given(@"I add a query parameter to the Request URL with Prefix ""([^""]*)"" for the Created Appointment Start")]
        public void AddAQueryParameterToTheRequestUrlWithPrefixForStoredAppointmentStart(string prefix)
        {
            var startKey = UrlEncode("start");
            var startValue = UrlEncode($"{prefix}{_fhirResourceRepository.Appointment.StartElement}");

            _httpContext.HttpRequestConfiguration.RequestUrl = $"{_httpContext.HttpRequestConfiguration.RequestUrl}?{startKey}={startValue}";
        }

        [Then(@"the Appointments returned must be in the future")]
        public void TheAppointmentMustBeInTheFuture()
        {
            Appointments.ForEach(appointment =>
            {
                appointment.Start.Value .ShouldBeGreaterThan(DateTime.UtcNow);
            });
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
