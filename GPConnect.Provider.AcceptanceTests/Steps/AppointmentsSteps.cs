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
    using GPConnect.Provider.AcceptanceTests.Extensions;

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

        [Then(@"the appointment reason must not be included")]
        public void theAppointmentReasonShouldBeEmpty()
        {
            Appointments.ForEach(appointment =>
            {
                appointment.Reason?.Count.ShouldBeLessThanOrEqualTo(0, "Appointment Reason should not be included in the appointment");
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

        [Given(@"I create an Appointment for Patient ""([^""]*)""")]
        public void CreateAnAppointmentForPatient(string patient)
        {
            _patientSteps.GetThePatientForPatientValue(patient);
            _patientSteps.StoreThePatient();

            _searchForFreeSlotsSteps.GetAvailableFreeSlots();
            _searchForFreeSlotsSteps.StoreTheFreeSlotsBundle();

            _httpSteps.ConfigureRequest(GpConnectInteraction.AppointmentCreate);

            CreateAnAppointmentFromTheStoredPatientAndStoredSchedule();

            _httpSteps.MakeRequest(GpConnectInteraction.AppointmentCreate);

        }

        [Given(@"I create an Appointment for Patient ""([^""]*)"" and Organization Code ""([^""]*)""")]
        public void CreateAnAppointmentForPatientAndOrganizationCode(string patient, string code)
        {
            _patientSteps.GetThePatientForPatientValue(patient);
            _patientSteps.StoreThePatient();

            _searchForFreeSlotsSteps.GetAvailableFreeSlots();
            _searchForFreeSlotsSteps.StoreTheFreeSlotsBundle();

            _httpSteps.ConfigureRequest(GpConnectInteraction.AppointmentCreate);
            Organization changed = FhirHelper.GetDefaultOrganization();
            changed.Identifier.First().Value = GlobalContext.OdsCodeMap[code];
            _httpSteps.jwtHelper.RequestingOrganization = changed.ToFhirJson();

            CreateAnAppointmentFromTheStoredPatientAndStoredSchedule();

            _httpSteps.MakeRequest(GpConnectInteraction.AppointmentCreate);

        }


        [Given(@"I create an Appointment in ""([^ ""]*)"" days time for Patient ""([^""]*)"" and Organization Code ""([^""]*)""")]
        public void CreateanAppointmentInXDaysTimeforPatientAndOrganizationCode(int days, string patient, string code)
        {
            _patientSteps.GetThePatientForPatientValue(patient);
            _patientSteps.StoreThePatient();

            _searchForFreeSlotsSteps.GetAvailableFreeSlotsSearchingXDaysInFuture(days);

            _searchForFreeSlotsSteps.StoreTheFreeSlotsBundle();

            _httpSteps.ConfigureRequest(GpConnectInteraction.AppointmentCreate);
            Organization changed = FhirHelper.GetDefaultOrganization();
            changed.Identifier.First().Value = GlobalContext.OdsCodeMap[code];
            _httpSteps.jwtHelper.RequestingOrganization = changed.ToFhirJson();

            CreateAnAppointmentFromTheStoredPatientAndStoredSchedule();

            _httpSteps.MakeRequest(GpConnectInteraction.AppointmentCreate);

        }

        [Given(@"I create an Appointment in ""([^ ""]*)"" days time for Patient ""([^""]*)"" and Organization Code ""([^""]*)"" Removing ServiceCategory and ServiceType from Created Appointment")]
        public void CreateanAppointmentInXDaysTimeforPatientAndOrganizationCodeRemovingServiceCategoryandServiceTypefromCreatedAppointment(int days, string patient, string code)
        {
            _patientSteps.GetThePatientForPatientValue(patient);
            _patientSteps.StoreThePatient();

            _searchForFreeSlotsSteps.GetAvailableFreeSlotsSearchingXDaysInFuture(days);

            _searchForFreeSlotsSteps.StoreTheFreeSlotsBundle();

            _httpSteps.ConfigureRequest(GpConnectInteraction.AppointmentCreate);
            Organization changed = FhirHelper.GetDefaultOrganization();
            changed.Identifier.First().Value = GlobalContext.OdsCodeMap[code];
            _httpSteps.jwtHelper.RequestingOrganization = changed.ToFhirJson();

            CreateAnAppointmentFromTheStoredPatientAndStoredSchedule();

            if (_fhirResourceRepository.Appointment.ServiceCategory != null)
                _fhirResourceRepository.Appointment.ServiceCategory = null;

            if (_fhirResourceRepository.Appointment.ServiceType != null)
                _fhirResourceRepository.Appointment.ServiceType = null;

            _httpSteps.MakeRequest(GpConnectInteraction.AppointmentCreate);

        }

        [Given(@"I create an Appointment in ""([^ ""]*)"" days time for Patient ""([^""]*)"" and Organization Code ""([^""]*)"" With serviceCategory and serviceType in Request")]
        public void CreateanAppointmentInXDaysTimeForPatientAndOrganizationCodeWithserviceCategoryandserviceTypeinRequest(int days, string patient, string code)
        {
            _patientSteps.GetThePatientForPatientValue(patient);
            _patientSteps.StoreThePatient();

            _searchForFreeSlotsSteps.GetAvailableFreeSlotsSearchingXDaysInFuture(days);
            _searchForFreeSlotsSteps.StoreTheFreeSlotsBundle();

            _httpSteps.ConfigureRequest(GpConnectInteraction.AppointmentCreate);
            Organization changed = FhirHelper.GetDefaultOrganization();
            changed.Identifier.First().Value = GlobalContext.OdsCodeMap[code];
            _httpSteps.jwtHelper.RequestingOrganization = changed.ToFhirJson();

            CreateAnAppointmentFromTheStoredPatientAndStoredSchedule();

            //Add two new elements into request
            //add ServiceCategory
            _fhirResourceRepository.Appointment.ServiceCategory = new CodeableConcept();
            _fhirResourceRepository.Appointment.ServiceCategory.Text = "Test-ServiceCategory";

            //add ServiceType
            _fhirResourceRepository.Appointment.ServiceType = new List<CodeableConcept>();
            var st = new CodeableConcept();
            st.Text = "Test-ServiceType";
            _fhirResourceRepository.Appointment.ServiceType.Add(st);

            _httpSteps.MakeRequest(GpConnectInteraction.AppointmentCreate);

        }

        [Given(@"I create an Appointment for Today for Patient ""([^""]*)"" and Organization Code ""([^""]*)""")]
        public void CreateAnAppointmentForTodayForPatientAndOrganizationCode(string patient, string code)
        {
            _patientSteps.GetThePatientForPatientValue(patient);
            _patientSteps.StoreThePatient();

            _searchForFreeSlotsSteps.GetAvailableFreeSlotsForToday();
            _searchForFreeSlotsSteps.StoreTheFreeSlotsBundle();

            _httpSteps.ConfigureRequest(GpConnectInteraction.AppointmentCreate);
            Organization changed = FhirHelper.GetDefaultOrganization();
            changed.Identifier.First().Value = GlobalContext.OdsCodeMap[code];
            _httpSteps.jwtHelper.RequestingOrganization = changed.ToFhirJson();

            CreateAnAppointmentFromTheStoredPatientAndStoredSchedule();

            _httpSteps.MakeRequest(GpConnectInteraction.AppointmentCreate);

        }

        [Given(@"I create an Appointment for an existing Patient and Organization Code ""([^""]*)""")]
        public void CreateAnAppointmentForRandomPatientAndOrganizationCode(string code)
        {

            var patient = "patient1";

            if (AppSettingsHelper.RandomPatientEnabled == true)
            {
                patient = RandomPatientSteps.ReturnRandomPatient();
            }

            _patientSteps.GetThePatientForPatientValue(patient);
            _patientSteps.StoreThePatient();

            _searchForFreeSlotsSteps.GetAvailableFreeSlots();
            _searchForFreeSlotsSteps.StoreTheFreeSlotsBundle();

            _httpSteps.ConfigureRequest(GpConnectInteraction.AppointmentCreate);

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

            _fhirResourceRepository.Appointment = appointmentBuilder.BuildAppointment(true, false, false);

            _fhirResourceRepository.Appointment.ShouldNotBeNull("Built appointment is null.");
        }

        [Given(@"I create an Appointment with org type ""(.*)"" with channel ""(.*)"" with prac role ""(.*)""")]
        public void CreateAnAppointmentWithOrgTypeWithChannelWithPracRole(Boolean addOrgType, Boolean addDeliveryChannel, Boolean addPracRole)
        {
            var appointmentBuilder = new DefaultAppointmentBuilder(_fhirResourceRepository);

            _fhirResourceRepository.Appointment = appointmentBuilder.BuildAppointment(addOrgType, addDeliveryChannel, addPracRole);

            _fhirResourceRepository.Appointment.ShouldNotBeNull("Built appointment is null.");
        }

        [Given(@"I create an Appointment without organisationType from the stored Patient and stored Schedule")]
        public void CreateAnAppointmentWithoutOrgTypeFromTheStoredPatientAndStoredSchedule()
        {
            var appointmentBuilder = new DefaultAppointmentBuilder(_fhirResourceRepository);

            _fhirResourceRepository.Appointment = appointmentBuilder.BuildAppointment(false, false, false);

            _fhirResourceRepository.Appointment.ShouldNotBeNull("Built appointment is null.");
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

        // git hub ref 145
        // RMB 12/10/2018		
        [Given(@"I set the Created Appointment Cancellation Reason ""([^""]*)""")]
        public void SetTheCreatedAppointmentCancellationReason(string reason)
        {
            var extension = GetCancellationReasonExtension(reason);

            if (_fhirResourceRepository.Appointment.Extension == null)
                _fhirResourceRepository.Appointment.Extension = new List<Extension>();

            _fhirResourceRepository.Appointment.Extension.Add(extension);
        }

        // git hub ref 200
        // RMB 20/2/19
        [Given(@"I amend the Appointment reference to absolute reference")]
        public void Iamendthereferencetoabsolutereference()
        {
            var organizationReference = new ResourceReference { Reference = "https://test1.supplier.thirdparty.nhs.uk/A11111/STU3/1/GPConnect/#1" };
            var arExt = new Extension(FhirConst.StructureDefinitionSystems.kAppointmentBookingOrganization, organizationReference);

            _fhirResourceRepository.Appointment.RemoveExtension(FhirConst.StructureDefinitionSystems.kAppointmentBookingOrganization);
            _fhirResourceRepository.Appointment.Extension.Add(arExt);
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

        [Given(@"I add a query parameter to the Request URL for Start ""([^""]*)"" and End ""([^""]*)""")]
        public void AddAQueryParameterToTheRequestUrlForStartAndEnd(string start, string end)
        {
            var startKey = UrlEncode("start");
            var startValue = UrlEncode($"ge{start}");
            var endValue = UrlEncode($"le{end}");

            _httpContext.HttpRequestConfiguration.RequestUrl = $"{_httpContext.HttpRequestConfiguration.RequestUrl}?{startKey}={startValue}&{startKey}={endValue}";
            System.Diagnostics.Debug.WriteLine(_httpContext.HttpRequestConfiguration.RequestUrl);
        }

        [Given(@"I add start query parameters to the Request URL for Period starting today for ""([^""]*)"" days")]
        public void AddStartQueryParametersToTheRequestUrlForStartAndEnd(int days)
        {
            var date = DateTime.UtcNow;
            var startDate = date.ToString("yyyy-MM-dd");
            var endDate = date.AddDays(days).ToString("yyyy-MM-dd");

            var startKey = UrlEncode("start");
            var startValue = UrlEncode($"ge{startDate}");
            var endValue = UrlEncode($"le{endDate}");

            _httpContext.HttpRequestConfiguration.RequestUrl = $"{_httpContext.HttpRequestConfiguration.RequestUrl}?{startKey}={startValue}&{startKey}={endValue}";
            System.Diagnostics.Debug.WriteLine(_httpContext.HttpRequestConfiguration.RequestUrl);
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
                //appointment.Start.Value .ShouldBeGreaterThan(DateTime.UtcNow);
                appointment.Start.Value.Date.ShouldBeGreaterThanOrEqualTo(DateTime.UtcNow.Date);
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

        [Then(@"the Appointment Organisation Code should equal ""(.*)""")]
        public void TheAppointmentOrganisationCodeShouldEqual(String org)
        {
            Appointments.ForEach(readAppointment =>
            {
                readAppointment.Contained.ForEach(resource =>
                {
                    if (resource.ResourceType.Equals(ResourceType.Organization))
                    {
                        Organization organisation = (Organization)resource;
                        organisation.Identifier.First().Value.ShouldBe(GlobalContext.OdsCodeMap[org]);

                    }
                });
            });
        }

        [Then(@"One Appointment contains serviceCategory and serviceType elements")]
        public void OneAppointmentcontainsserviceCategoryandserviceTypeelements()
        {
            Appointments.Count().ShouldBeGreaterThanOrEqualTo(1, "Fail : Test expects atleast one Appointment is returned");
            bool foundServiceCategory = false;
            bool foundserviceType = false;

            Appointments.ForEach(appointment =>
            {

                if (appointment.ServiceCategory != null)
                {
                    if (!String.IsNullOrEmpty(appointment.ServiceCategory.Text))
                    {
                        foundServiceCategory = true;
                        Logger.Log.WriteLine("Info : Found an Appointment resource with ServiceCategory set");
                    }
                }

                if (appointment.ServiceType != null)
                {
                    foreach (var st in appointment.ServiceType)
                    {
                        if (!String.IsNullOrEmpty(st.Text))
                        {
                            foundserviceType = true;
                            Logger.Log.WriteLine("Info : Found an Appointment resource with ServiceType set");
                            break;
                        }
                    }
                }
            });

            foundServiceCategory.ShouldBeTrue("Fail : At least one Appointment Resource should contain a ServiceCategory set as per the data requirements");
            foundserviceType.ShouldBeTrue("Fail : At least one Appointment Resource should contain a ServiceType set as per the data requirements");
        }

        [Then(@"One Appointment contains serviceCategory element")]
        public void OneAppointmentcontainsserviceCategoryelement()
        {
            Appointments.Count().ShouldBeGreaterThanOrEqualTo(1, "Fail : Test expects atleast one Appointment is returned");
            bool foundServiceCategory = false;

            Appointments.ForEach(appointment =>
            {

                if (appointment.ServiceCategory != null)
                {
                    if (!String.IsNullOrEmpty(appointment.ServiceCategory.Text))
                    {
                        foundServiceCategory = true;
                        Logger.Log.WriteLine("Info : Found an Appointment resource with ServiceCategory set");
                    }
                }
            });
            foundServiceCategory.ShouldBeTrue("Fail : At least one Appointment Resource should contain a ServiceCategory set as per the data requirements");
        }


        [Then(@"One Appointment contains serviceType element")]
        public void OneAppointmentcontainsserviceTypeelement()
        {
            Appointments.Count().ShouldBeGreaterThanOrEqualTo(1, "Fail : Test expects atleast one Appointment is returned");
            bool foundserviceType = false;

            Appointments.ForEach(appointment =>
            {
                if (appointment.ServiceType != null)
                {
                    foreach (var st in appointment.ServiceType)
                    {
                        if (!String.IsNullOrEmpty(st.Text))
                        {
                            foundserviceType = true;
                            Logger.Log.WriteLine("Info : Found an Appointment resource with ServiceType set");
                            break;
                        }
                    }
                }

            });
            foundserviceType.ShouldBeTrue("Fail : At least one Appointment Resource should contain a ServiceType set as per the data requirements");
        }


        [Then(@"Appointments Do not contain serviceCategory and serviceType elements")]
        public void AppointmentDoesntcontainsserviceCategoryandserviceTypeelements()
        {
            Appointments.Count().ShouldBeGreaterThanOrEqualTo(1, "Fail : Test expects atleast one Appointment is returned");
            Appointments.ForEach(appointment =>
            {
                if (appointment.ServiceCategory != null)
                {
                    appointment.ServiceCategory.Text.ShouldBeNullOrEmpty("Fail : Appointment Resource should Not contain a ServiceCategory set as per the data requirements");
                }

                if (appointment.ServiceType != null)
                {
                    foreach (var st in appointment.ServiceType)
                    {
                        st.Text.ShouldBeNullOrEmpty("Fail : Appointment Resource should Not contain a ServiceType set as per the data requirements");
                    }
                }
            });
        }

        [Given(@"I set the Created Appointment ServiceCategory and serviceType to new values")]
        public void SetTheCreatedAppointmentServiceCategoryandserviceTypetonewvalues()
        {
            _fhirResourceRepository.Appointment.ServiceCategory.Text = "Test-ServiceCategory";
            _fhirResourceRepository.Appointment.ServiceType.First().Text = "Test-ServiceType";
        }

        [Given(@"I set the Created Appointment ServiceCategory to ""(.*)""")]
        public void SetTheCreatedAppointmentServiceCategoryto(string value)
        {
            _fhirResourceRepository.Appointment.ServiceCategory.Text = value;
        }

        [Given(@"I set the Created Appointment ServiceType to ""(.*)""")]
        public void SetTheCreatedAppointmentserviceTypetoanewvalue(string value)
        {
            _fhirResourceRepository.Appointment.ServiceType.First().Text = value;
        }

    }
}
