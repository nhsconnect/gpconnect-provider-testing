namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using System.Linq;
    using Context;
    using Constants;
    using Enum;
    using Hl7.Fhir.Model;
    using Repository;
    using Shouldly;
    using TechTalk.SpecFlow;
    using static Hl7.Fhir.Model.Appointment;
    using System;
    using Http;

    [Binding]
    public class CancelAppointmentSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private readonly IFhirResourceRepository _fhirResourceRepository;
        private readonly JwtSteps _jwtSteps;
        private readonly AppointmentsSteps _appointmentsSteps;
        private List<Appointment> Appointments => _httpContext.FhirResponse.Appointments;
        private readonly HttpRequestConfigurationSteps _httpRequestConfiguration;

        public CancelAppointmentSteps(HttpSteps httpSteps, HttpContext httpContext, IFhirResourceRepository fhirResourceRepository, JwtSteps jwtSteps, AppointmentsSteps appointmentsSteps, HttpRequestConfigurationSteps httpRequestConfiguration) 
            : base(httpSteps)
        {
            _httpContext = httpContext;
            _fhirResourceRepository = fhirResourceRepository;
            _jwtSteps = jwtSteps;
            _appointmentsSteps = appointmentsSteps;
            _httpRequestConfiguration = httpRequestConfiguration;
        }

        [Given(@"I set the Created Appointment Description to ""(.*)""")]
        public void SetTheCreatedAppointmentDescription(string value)
        {
            _fhirResourceRepository.Appointment.Description = value;
        }

        // git hub ref 200
        // RMB 20/2/19
        [Given(@"I amend the cancel organization reference to absolute reference")]
        public void Iamendthecancelorganizationreferencetoabsolutereference()
        {
            var organizationReference = new ResourceReference { Reference = "https://test1.supplier.thirdparty.nhs.uk/A11111/STU3/1/GPConnect/#1" };
            var arExt = new Extension(FhirConst.StructureDefinitionSystems.kAppointmentBookingOrganization, organizationReference);

            _fhirResourceRepository.Appointment.RemoveExtension(FhirConst.StructureDefinitionSystems.kAppointmentBookingOrganization);
            _fhirResourceRepository.Appointment.Extension.Add(arExt);
        }

        [Given(@"I amend an invalid appointment field")]
        public void IAmendAnInvalidAppointmentField()
        {
            _fhirResourceRepository.Appointment.Status = AppointmentStatus.Noshow;
        }

        [Given(@"I set the Created Appointment Priority to ""(.*)""")]
        public void SetTheCreatedAppointmentPriorityTo(int value)
        {
            _fhirResourceRepository.Appointment.Priority = value;
        }

        [Given(@"I set the Created Appointment Minutes Duration to ""(.*)""")]
        public void SetTheCreatedAppointmentMinutesDurationTo(int value)
        {
            _fhirResourceRepository.Appointment.MinutesDuration = value;
        }

        [Given(@"I set the Created Appointment Comment to ""(.*)""")]
        public void SetTheCreatedAppointmentCommentTo(string value)
        {
            _fhirResourceRepository.Appointment.Comment = value;
        }

        [Given(@"I set the Created Appointment Type Text to ""(.*)""")]
        public void SetTheCreatedAppointmentTypeTextTo(string value)
        {
            _fhirResourceRepository.Appointment.AppointmentType = new CodeableConcept
             {
                    Text = value
             };
        }

        [Given(@"I add an Appointment Identifier with default System and Value ""(.*)"" to the Created Appointment")]
        public void AddAnAppointmentIdentifierWithSystemAndValue(string value)
        {
            _fhirResourceRepository.Appointment.Identifier.Add(new Identifier(FhirConst.IdentifierSystems.kAppointment, value));
        }

        [Given(@"I add a Participant with Reference ""(.*)"" to the Created Appointment")]
        public void AddAParticipantWithReferenceToTheCreatedAppointment(string reference)
        {
            var practitioner = new ParticipantComponent
            {
                Actor = new ResourceReference {Reference = reference},
                Status = ParticipationStatus.Declined
            };
       
            _fhirResourceRepository.Appointment.Participant.Add(practitioner);
        }

        [Then("the Appointment Status should be Cancelled")]
        public void TheAppointmentStatusShouldBeCancelled()
        {
            Appointments.ForEach(appointment =>
            {
                appointment.Status.ShouldBe(AppointmentStatus.Cancelled, $"The Appointment Status should be {AppointmentStatus.Cancelled.ToString()} but was {appointment.Status}.");
            });
        }

        [Then(@"the Appointment Cancellation Reason Extension should be valid for ""(.*)""")]
        public void TheAppointmentCancellationReasonExtensionShouldBeValidFor(string value)
        {
            Appointments.ForEach(appointment =>
            {
                appointment.Extension.ForEach(extension =>
                {
                    if (extension.Url.ToString().Equals("https://fhir.nhs.uk/STU3/StructureDefinition/Extension-GPConnect-AppointmentCancellationReason-1"))
                        {
                            extension.ShouldNotBeNull("The Appointment did not contain a Cancellation Reason Extension.");
                            var result = extension.Value.ToString();
                            result.ShouldBe(value, $"The Cancellation Reason Extension value should be {value} but was {result}.");
                    }
                });
            });
        }

        [Then("the Appointment Id should equal the Created Appointment Id")]
        public void TheAppointmentIdShouldEqualTheCreatedAppointmentId()
        {
            Appointments.ForEach(appointment =>
            {
                var createdAppointmentId = _fhirResourceRepository.Appointment.Id;

                appointment.Id.ShouldBe(createdAppointmentId, $"The Appointment Id should be {createdAppointmentId} but was {appointment.Id}.");
            });
        }

        [Then("the Appointment Status should equal the Created Appointment Status")]
        public void TheAppointmentStatusShouldEqualTheCreatedAppointmentStatus()
        {
            Appointments.ForEach(appointment =>
            {
                var createdAppointmentStatus = _fhirResourceRepository.Appointment.Status;

                appointment.Status.ShouldBe(createdAppointmentStatus, $"The Appointment Status should be {createdAppointmentStatus} but was {appointment.Status}.");
            });
        }

        [Then("the Appointment Extensions should equal the Created Appointment Extensions")]
        public void TheAppointmentExtensionsShoudEqualTheCreatedAppointmentExtensions()
        {
            Appointments.ForEach(appointment =>
            {
                var createdAppointmentExtensions = _fhirResourceRepository.Appointment.Extension;

                var appointmentExtensions = appointment.Extension;

                appointmentExtensions.Count.ShouldBe(createdAppointmentExtensions.Count, $"There should be {createdAppointmentExtensions.Count} Appointment Extensions but found {appointmentExtensions.Count}.");

                foreach (var createdAppointmentExtension in createdAppointmentExtensions)
                {
                    var appointmentExtension = appointmentExtensions.FirstOrDefault(x => x.Url == createdAppointmentExtension.Url);

                    appointmentExtension.ShouldNotBeNull($"The Appointment did not contain a {createdAppointmentExtension.Url} Extension.");

                    appointmentExtension.Value.ToString().ShouldBe(createdAppointmentExtension.Value.ToString(), $"The Appointment {createdAppointmentExtension.Url} Extension should be {createdAppointmentExtension.Value.ToString()} but was {appointmentExtension.Value.ToString()}.");
                }
            });
        }

        [Then("the Appointment Slots should equal the Created Appointment Slots")]
        public void TheAppointmentSlotsShouldEqualTheCreatedAppointmentSlots()
        {
            Appointments.ForEach(appointment =>
            {
                var createdAppointmentSlots = _fhirResourceRepository.Appointment.Slot;

                var appointmentSlots = appointment.Slot;

                appointmentSlots.Count.ShouldBe(createdAppointmentSlots.Count, $"There should be {createdAppointmentSlots.Count} Appointment Slots but found {appointmentSlots.Count}.");

                foreach (var createdAppointmentSlot in createdAppointmentSlots)
                {
                    var appointmentSlot = appointmentSlots.FirstOrDefault(x => x.Reference == createdAppointmentSlot.Reference);

                    appointmentSlot.ShouldNotBeNull($"The Appointment did not contain a Slot with Reference {createdAppointmentSlot.Reference}.");

                    appointmentSlot.Display.ShouldBe(createdAppointmentSlot.Display, $"The Appointment Slot with Reference {createdAppointmentSlot.Reference} Display should be {createdAppointmentSlot.Display} but was {appointmentSlot.Display}.");
                }
            });
        }


        [Then("the Appointment Participants should be equal to the Created Appointment Participants")]
        public void TheAppointmentParticipantsShouldBeEqualToTheCreatedAppointmentParticipants()
        {
            Appointments.ForEach(appointment =>
            {
                var createdAppointmentParticipants = _fhirResourceRepository.Appointment.Participant;
                var appointmentParticipants = appointment.Participant;

                appointmentParticipants.Count.ShouldBe(createdAppointmentParticipants.Count, $"There should be {createdAppointmentParticipants.Count} Appointment Participants but found {appointmentParticipants.Count}.");

                foreach (var createdAppointmentParticipant in createdAppointmentParticipants)
                {
                    //Reference
                    var createdAppointmentReference = createdAppointmentParticipant.Actor.Reference;
                    var appointmentParticipant = appointmentParticipants.FirstOrDefault(x => x.Actor.Reference == createdAppointmentReference);

                    appointmentParticipant.ShouldNotBeNull($"The Appointment did not contain a Participant with Reference {createdAppointmentReference}.");

                    //URL
                    var createdAppointmenturl = createdAppointmentParticipant.Actor.Url.ToString();
                    var appointmentUrl = appointmentParticipant.Actor.Url.ToString();

                    appointmentUrl.ShouldBe(createdAppointmenturl, $"The Appointment Participant with Reference {createdAppointmentReference} URL should be {createdAppointmenturl} but was {appointmentUrl}." );

                    //Status
                    var createdAppointmentStatus = createdAppointmentParticipant.Status;
                    var appointmentStatus = appointmentParticipant.Status;

                    appointmentStatus.ShouldBe(createdAppointmentStatus, $"The Appointment Participant with Reference {createdAppointmentReference} Status should be {createdAppointmentStatus} but was {appointmentStatus}.");
                }
            });
        }

        [Then("the Appointment Description should equal the Created Appointment Description")]
        public void TheAppointmentDescriptionShouldEqualTheCreatedAppointmentDescription()
        {
            Appointments.ForEach(appointment =>
            {
                var createdAppointmentDescription = _fhirResourceRepository.Appointment.Description;
                appointment.Description.ShouldBe(createdAppointmentDescription, $"The Appointment Description should be {createdAppointmentDescription} but was {appointment.Description}.");
            });
        }

        [Then("the Appointment Start and End Dates should equal the Created Appointment Start and End Dates")]
        public void TheAppointmentStartAndEndDatesShouldEqualTheCreatedAppointmentStartAndEndDates()
        {
            Appointments.ForEach(appointment =>
            {
                var createdAppointmentStartDate = _fhirResourceRepository.Appointment.Start;
                appointment.Start.ShouldNotBeNull("The Appointment Start Date should not be null.");
                appointment.Start.ShouldBe(createdAppointmentStartDate, $"The Appointment Start Date should be {createdAppointmentStartDate} but was {appointment.Start}.");

                var createdAppointmentEndDate = _fhirResourceRepository.Appointment.End;
                appointment.End.ShouldNotBeNull("The Appointment End Date should not be null.");
                appointment.End.ShouldBe(createdAppointmentEndDate, $"The Appointment End Date should be {createdAppointmentEndDate} but was {appointment.End}.");
            });
        }

        [Then("the Appointment Participants should be valid and resolvable")]
        public void TheAppointmentParticipantsShouldBeValidAndResolvable()
        {
            Appointments.ForEach(appointment =>
            {
                var locationCount = appointment.Participant.Count(participant => participant.Actor.Reference.StartsWith("Location/"));
                locationCount.ShouldBeGreaterThanOrEqualTo(1, "There Appointment Participants should a minimum of 1 Location but found none.");

                var patientCount = appointment.Participant.Count(participant => participant.Actor.Reference.StartsWith("Patient/"));
                patientCount.ShouldBeGreaterThanOrEqualTo(1, "There Appointment Participants should a minimum of 1 Patient but found none.");

                appointment.Participant.ForEach(participant =>
                {
                    participant.Actor.ShouldNotBeNull("Participant actor should not be null");

                    if (participant.Actor.Reference.StartsWith("Practitioner/"))
                    {
                        var practitioner = _httpSteps.GetResourceForRelativeUrl(GpConnectInteraction.PractitionerRead, participant.Actor.Reference);

                        practitioner.ShouldNotBeNull($"The Appointment Participant with Reference {participant.Actor.Reference} returned a null Practitioner.");
                        practitioner.GetType().ShouldBe(typeof(Practitioner), $"The Appointment Participant with Reference {participant.Actor.Reference} returned a {practitioner.GetType().ToString()}.");
                    }

                    if (participant.Actor.Reference.StartsWith("Patient/"))
                    {
                        var patient = _httpSteps.GetResourceForRelativeUrl(GpConnectInteraction.PatientRead, participant.Actor.Reference);

                        patient.ShouldNotBeNull($"The Appointment Participant with Reference {participant.Actor.Reference} returned a null Patient.");
                        patient.GetType().ShouldBe(typeof(Patient), $"The Appointment Participant with Reference {participant.Actor.Reference} returned a {patient.GetType().ToString()}.");
                    }

                    if (participant.Actor.Reference.StartsWith("Location/"))
                    {

                        var location = _httpSteps.GetResourceForRelativeUrl(GpConnectInteraction.LocationRead, participant.Actor.Reference);

                        location.ShouldNotBeNull($"The Appointment Participant with Reference {participant.Actor.Reference} returned a null Location.");
                        location.GetType().ShouldBe(typeof(Location), $"The Appointment Participant with Reference {participant.Actor.Reference} returned a {location.GetType().ToString()}.");
                    }
                });
            });
        }

        [Then(@"the Appointment Version Id should not equal the Created Appointment Version Id")]
        public void TheAppointmentVersionIdShouldNotEqualTheCreatedAppointmentVersionId()
        {
            Appointments.ForEach(appointment =>
            {
                var createdAppointmentVersionId = _fhirResourceRepository.Appointment.VersionId;

                appointment.VersionId.ShouldNotBe(createdAppointmentVersionId, $"The Appointment Version Id and the Created Appointment Version Id were both {appointment.VersionId}");
            });
        }

        [Then(@"the Appointment Created should be equal to the Created Appointment Created")]
        public void TheAppointmentCreatedShouldBeEqualToTheCreatedApppointmentCreated()
        {
            Appointments.ForEach(appointment =>
            {
                var createdAppointmentCreated = _fhirResourceRepository.Appointment.Created;
                DateTime dt = Convert.ToDateTime(createdAppointmentCreated);
                DateTime dt2 = Convert.ToDateTime(appointment.Created);
                bool dateComparison = compareDates(dt, dt2);


                dateComparison.ShouldBe(true, $"The Appointment Created should be {createdAppointmentCreated}, but was {appointment.Created}.");
            });
        }

        private bool compareDates(DateTime storedDate, DateTime createdDate)
        {
            if (storedDate.Day.Equals(createdDate.Day) 
                && storedDate.Minute.Equals(createdDate.Minute) 
                && storedDate.Month.Equals(createdDate.Month)
                && storedDate.Year.Equals(createdDate.Year))
            {
                return true;
            }
            else {
                return false;
            }
        }

        [Given(@"I cancel the Appointment with Logical Id and NHS Number")]
        public void CancelTheAppointmentWithLogicalId(Appointment appointment, string nhsNumber)
        {
            _httpSteps.ConfigureRequest(GpConnectInteraction.AppointmentCancel);

            _httpRequestConfiguration.SetTheReadOperationLogicalIdentifierUsedInTheRequestTo(appointment.Id);

            _fhirResourceRepository.Appointment = appointment;
            _appointmentsSteps.SetTheCreatedAppointmentToCancelledWithReason("double booked");

            _httpSteps.MakeRequest(GpConnectInteraction.AppointmentCancel);
        }
    }
}
