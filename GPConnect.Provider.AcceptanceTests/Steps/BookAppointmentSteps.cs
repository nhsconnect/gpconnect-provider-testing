namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using Context;
    using Hl7.Fhir.Model;
    using Shouldly;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Enum;
    using Repository;
    using TechTalk.SpecFlow;

    [Binding]
    public class BookAppointmentSteps : BaseSteps
    {
        private readonly IFhirResourceRepository _fhirResourceRepository;
        private readonly HttpContext _httpContext;

        private List<Appointment> Appointments => _httpContext.FhirResponse.Appointments;

        public BookAppointmentSteps(HttpSteps httpSteps, HttpContext httpContext, IFhirResourceRepository fhirResourceRepository)
            : base(httpSteps)
        {
            _httpContext = httpContext;
            _fhirResourceRepository = fhirResourceRepository;
        }

        [Given(@"I add an Invalid Extension with Code only to the Created Appointment")]
        public void AddAnInvalidExtensionWithCodeOnlyToTheCreatedAppointment()
        {
            var coding = new Coding
            {
                Code = "TEL"
            };

            var codableConcept = new CodeableConcept();
            codableConcept.Coding.Add(coding);

            var extension = new Extension
            {
                Value = codableConcept
            };

            _fhirResourceRepository.Appointment.Extension.Add(extension);
        }

        [Given(@"I add an Invalid Extension with Url only to the Created Appointment")]
        public void AddAnInvalidExtensionWithUrlToTheCreatedAppointment()
        {
            var extension = new Extension
            {
                Url = "RandomExtensionUsedForTesting"
            };

            _fhirResourceRepository.Appointment.Extension.Add(extension);
        }

        [Given(@"I add an Invalid Extension with Url, Code and Display to the Created Appointment")]
        public void AddAnInvalidExtensionWithUrlCodeAndDisplayToTheCreatedAppointment()
        {
            var coding = new Coding
            {
                Code = "TEL",
                Display = "Telephone"
            };

            var codableConcept = new CodeableConcept();
            codableConcept.Coding.Add(coding);

            var extension = new Extension
            {
                Url = "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-notanextension-1",
                Value = codableConcept
            };

            _fhirResourceRepository.Appointment.Extension.Add(extension);
        }

        [Given(@"I set the Created Appointment Id to ""([^""]*)""")]
        public void SetTheCreatedAppointmentIdTo(string id)
        {
            _fhirResourceRepository.Appointment.Id = id;
        }

        [Given(@"I set the Created Appointment Slot Reference to ""([^""]*)""")]
        public void SetTheCreatedAppointmentSlotReferenceTo(string slotReference)
        {
            var reference = new ResourceReference
            {
                Reference = slotReference
            };

            _fhirResourceRepository.Appointment.Slot.Clear();
            _fhirResourceRepository.Appointment.Slot.Add(reference);
        }

        [Given(@"I remove the Start from the Created Appointment")]
        public void RemoveTheStartFromTheCreatedAppointment()
        {
            _fhirResourceRepository.Appointment.Start = null;
        }

        [Given(@"I remove the End from the Created Appointment")]
        public void RemoveTheEndFromTheCreatedAppointment()
        {
            _fhirResourceRepository.Appointment.End = null;
        }

        [Given(@"I remove the Status from the Created Appointment")]
        public void RemoveTheStatusFromTheCreatedAppointment()
        {
            _fhirResourceRepository.Appointment.Status = null;
        }

        [Given(@"I remove the Slot from the Created Appointment")]
        public void RemoveTheSlotFromTheCreatedAppointment()
        {
            _fhirResourceRepository.Appointment.Slot = null;
        }

        [Given(@"I set the Created Appointment Identifier Value to null")]
        public void SetTheCreatedAppointmentIdentifierValueToNull()
        {
            var identifiers = new List<Identifier>
            {
                new Identifier("http://fhir.nhs.net/Id/gpconnect-appointment-identifier", null)
            };

            _fhirResourceRepository.Appointment.Identifier = identifiers;
        }

        private static CodeableConcept GetReason(string system, string code, string display)
        {
            return new CodeableConcept
            {
                Coding = new List<Coding>
                {
                    new Coding(system, code, display)
                }
            };
        }

        [Given(@"I set the Created Appointment Reason Coding System to null")]
        public void SetTheCreatedAppointmentReasonCodingSystemToNull()
        {
            _fhirResourceRepository.Appointment.Reason = GetReason(null, "Code", "Display");
        }

        [Given(@"I set the Created Appointment Reason Coding Code to null")]
        public void SetTheCreatedAppointmentReasonCodingCodeToNull()
        {
            _fhirResourceRepository.Appointment.Reason = GetReason("http://snomed.info/sct", null, "Display");
        }

        [Given(@"I set the Created Appointment Reason Coding Display to null")]
        public void SetTheCreatedAppointmentReasonCodingDisplayToNull()
        {
            _fhirResourceRepository.Appointment.Reason = GetReason("http://snomed.info/sct", "Code", null);
        }

        [Given(@"I set the Created Appointment Patient Participant Status to null")]
        public void SetTheCreatedAppointmentPatientParticipantStatusToNull()
        {
            _fhirResourceRepository.Appointment.Participant.ForEach(participant =>
            {
                if (participant.Actor.Reference.ToString().Contains("Patient"))
                {
                    participant.Status = null;
                }
            });
        }

        [Given(@"I set the Created Appointment Practitioner Participant Status to null")]
        public void SetTheCreatedAppointmentPractitionerParticipantStatusToNull()
        {
            _fhirResourceRepository.Appointment.Participant.ForEach(participant =>
            {
                if (participant.Actor.Reference.ToString().Contains("Practitioner"))
                {
                    participant.Status = null;
                }
            });
        }

        [Given(@"I set the Created Appointment Location Participant Status to null")]
        public void SetTheCreatedAppointmentLocationParticipantStatusToNull()
        {
            _fhirResourceRepository.Appointment.Participant.ForEach(participant =>
            {
                if (participant.Actor.Reference.ToString().Contains("Location"))
                {
                    participant.Status = null;
                }
            });
        }

        [Given(@"I set the Created Appointment Participant Type Coding ""([^""]*)"" to null for ""([^""]*)"" Participants")]
        public void SetTheCreatedAppointmentParticipantTypeCodingToNullForParticipants(string codingType, string participantType)
        {
            _fhirResourceRepository.Appointment.Participant.ForEach(particpant =>
            {
                if (particpant.Actor.Reference.Contains(participantType))
                {
                    switch (codingType)
                    {
                        case "system":
                            particpant.Type.First().Coding.First().System = null;
                            break;
                        case "code":
                            particpant.Type.First().Coding.First().Code = null;
                            break;
                        case "display":
                            particpant.Type.First().Coding.First().Display = null;
                            break;
                    }
                }
            });
        }

        [Given(@"I remove the ""([^""]*)"" Participants from the Created Appointment")]
        public void RemoveTheParticipantsFromTheCreatedAppointment(string participantType)
        {
            _fhirResourceRepository.Appointment
                .Participant
                .RemoveAll(participant => participant.Actor.Reference.Contains(participantType));
        }

        [Then("the Appointment Category Extension should be valid")]
        public void TheAppointmentCategoryExtensionShouldBeValid()
        {
            Appointments.ForEach(appointment =>
            {
                const string categoryUrl =
                    "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1";

                var appointmentCategoryExtension = appointment.GetExtensionValue<CodeableConcept>(categoryUrl);

                if (appointmentCategoryExtension?.Coding != null)
                {
                    ShouldBeSingleCodingWhichIsInValueSet(GlobalContext.FhirAppointmentCategoryValueSet,
                        appointmentCategoryExtension.Coding);
                }
            });
        }

        [Then("the Appointment Booking Method Extension should be valid")]
        public void TheAppointmentBookingMethodExtensionShouldBeValid()
        {
            Appointments.ForEach(appointment =>
            {
                const string bookinMethodUrl =
                    "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1";

                var appointmentBookingMethodExtension = appointment.GetExtensionValue<CodeableConcept>(bookinMethodUrl);

                if (appointmentBookingMethodExtension?.Coding != null)
                {
                    ShouldBeSingleCodingWhichIsInValueSet(GlobalContext.FhirAppointmentBookingMethodValueSet,
                        appointmentBookingMethodExtension.Coding);
                }
            });
        }

        [Then("the Appointment Contact Method Extension should be valid")]
        public void TheAppointmentContactMethodExtensionShouldBeValid()
        {
            Appointments.ForEach(appointment =>
            {
                const string contactMethodUrl =
                    "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1";

                var appointmentContactMethodExtension = appointment.GetExtensionValue<CodeableConcept>(contactMethodUrl);

                if (appointmentContactMethodExtension?.Coding != null)
                {
                    ShouldBeSingleCodingWhichIsInValueSet(GlobalContext.FhirAppointmentCategoryValueSet,
                        appointmentContactMethodExtension.Coding);
                }
            });
        }

        [Then("the Appointment Cancellation Reason Extension should be valid")]
        public void TheAppointmentCancellationReasonExtensionShouldBeValid()
        {
            Appointments.ForEach(appointment =>
            {
                const string cancellationReasonUrl =
                    "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1";

                var appointmentCancellationReasonExtensions = appointment
                    .Extension
                    .Where(extension => extension.Url == cancellationReasonUrl)
                    .ToList();

                appointmentCancellationReasonExtensions.Count.ShouldBeLessThanOrEqualTo(1,
                    $"There should be a maximum of 1 Appointment Cancellation Reason Extensions but found {appointmentCancellationReasonExtensions.Count}");

                if (appointmentCancellationReasonExtensions.Any())
                {
                    appointmentCancellationReasonExtensions[0].Url.ShouldBeOfType<Uri>();
                    appointmentCancellationReasonExtensions[0].Value.ShouldBeOfType<string>();
                }
            });
        }

        [Then(@"the Appointment Location Participant should be valid and resolvable")]
        public void TheAppointmentLocationParticipantShouldBeValidAndResolvable()
        {
            Appointments.ForEach(appointment =>
            {
                appointment.Participant.ForEach(participant =>
                {
                    if (participant.Actor.Reference.StartsWith("Location/"))
                    {
                        var location = _httpSteps.GetResourceForRelativeUrl(GpConnectInteraction.LocationRead, participant.Actor.Reference);

                        location.ShouldNotBeNull(
                            $"The Appointment Participant with Reference {participant.Actor.Reference} returned a null Location.");
                        location.GetType()
                            .ShouldBe(typeof(Location),
                                $"The Appointment Participant with Reference {participant.Actor.Reference} returned a {location.GetType().ToString()}.");
                    }
                });
            });
        }
    }
}
