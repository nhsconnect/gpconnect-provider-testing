namespace GPConnect.Provider.AcceptanceTests.Builders.Appointment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Hl7.Fhir.Model;
    using Repository;
    using static Hl7.Fhir.Model.Appointment;
    using Constants;
    using Context;
    using static Hl7.Fhir.Model.Bundle;

    public class DefaultAppointmentBuilder
    {
        private readonly IFhirResourceRepository _fhirResourceRepository;

        public DefaultAppointmentBuilder(IFhirResourceRepository fhirResourceRepository)
        {
            _fhirResourceRepository = fhirResourceRepository;
        }

        public Appointment BuildAppointment()
        {
            var storedPatient = _fhirResourceRepository.Patient;
            var storedBundle = _fhirResourceRepository.Bundle;

            var firstSlot = storedBundle.Entry
                .Where(entry => entry.Resource.ResourceType.Equals(ResourceType.Slot))
                .Select(entry => (Slot)entry.Resource)
                .First();

            var schedule = storedBundle.Entry
                .Where(entry =>
                        entry.Resource.ResourceType.Equals(ResourceType.Schedule) &&
                        ComposeReferenceFromEntry(entry) == firstSlot.Schedule.Reference)
                .Select(entry => (Schedule)entry.Resource)
                .First();
            
         

            //Patient
            var patient = GetPatient(storedPatient);

            //Practitioners
            var practitionerReferences = schedule.Extension.Where(extension => extension is ResourceReference).Select(extension => ((ResourceReference)extension.Value).Reference);
            var practitioners = GetPractitioners(practitionerReferences);

            //Location
            var locationReference = schedule.Actor.First(actor => actor.Reference.Contains("Location")).Reference;
            var location = GetLocation(locationReference);

            //Participants
            var participants = new List<ParticipantComponent>
            {
                patient
            };
            participants.AddRange(practitioners);
            participants.Add(location);

            //Slots
            var slot = GetSlot(firstSlot);

            var slots = new List<ResourceReference>
            {
                slot
            };

            //Extensions
            var bookingOrganizationExtension = GetBookingOrganizationExtension();

            //Contained Resources
            var bookingOrganization = GetBookingOrganization();
            
            //Initialize Appointment
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Booked,
                Start = firstSlot.Start,
                End = firstSlot.End,
                Participant = participants,
                Slot = slots,
                Description = "Default Description",
                CreatedElement = new FhirDateTime(DateTime.UtcNow)
            };

            //Add Extensions & Contained Resources
            appointment.Extension.Add(bookingOrganizationExtension);
            appointment.Contained.Add(bookingOrganization);

            return appointment;
        }

        private static Organization GetBookingOrganization()
        {
            var bookingOrganization = new Organization
            {
                Id = "1",
                Name = "Test Suite Validator"
            };

            bookingOrganization.Identifier.Add(new Identifier(FhirConst.IdentifierSystems.kOdsOrgzCode, GlobalContext.OdsCodeMap["ORG1"]));
            bookingOrganization.Telecom.Add(new ContactPoint(ContactPoint.ContactPointSystem.Phone, ContactPoint.ContactPointUse.Temp, "01823938938"));

            return bookingOrganization;
        }

        private static Extension GetBookingOrganizationExtension()
        {
            var organizationReference = new ResourceReference { Reference = "#1" };
            return new Extension(FhirConst.StructureDefinitionSystems.kAppointmentBookingOrganization, organizationReference);
        }

        private static string ComposeReferenceFromEntry(EntryComponent entry)
        {
            return $"{entry.Resource.TypeName}/{entry.Resource.Id}";
        }


        private static ParticipantComponent GetLocation(string locationReference)
        {
            return new ParticipantComponent
            {
                Actor = new ResourceReference
                {
                    Reference = locationReference
                },
                Status = ParticipationStatus.Accepted,
                Type = new List<CodeableConcept>
                {
                    new CodeableConcept(FhirConst.ValueSetSystems.kEncounterParticipantType, "PART", "Participation", "Participation")
                }
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
                    new CodeableConcept(FhirConst.ValueSetSystems.kEncounterParticipantType, "PART", "Participation", "Participation")
                }
            };
        }

        private static IEnumerable<ParticipantComponent> GetPractitioners(IEnumerable<string> practitionerReferences)
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
                        new CodeableConcept(FhirConst.ValueSetSystems.kEncounterParticipantType, "PPRF", "primary performer", "primary performer")
                    }
                })
                .ToList();
        }

     }
}
