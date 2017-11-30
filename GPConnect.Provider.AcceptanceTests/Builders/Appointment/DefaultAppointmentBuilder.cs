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
                Slot = slots,
              

            };

            appointment.Extension.Add(new Extension("https://fhir.nhs.uk/StructureDefinition/extension-gpconnect-appointment-created-1", FhirDateTime.Now()));

            var orgRef = new ResourceReference();
            orgRef.Reference = "#1";
            appointment.Extension.Add(new Extension("https://fhir.nhs.uk/StructureDefinition/extension-gpconnect-booking-organisation-1", orgRef));
            Organization bookingOrg = new Organization();
            bookingOrg.Identifier.Add(new Identifier(FhirConst.IdentifierSystems.kOdsOrgzCode, GlobalContext.OdsCodeMap["ORG1"]));
            bookingOrg.Id = "#1";
            bookingOrg.Name = "Test Suite Validator";
            bookingOrg.Telecom.Add(new ContactPoint(ContactPoint.ContactPointSystem.Phone, ContactPoint.ContactPointUse.Temp, "01823938938"));
            appointment.Contained.Add(bookingOrg);

            appointment.Description = "Default Description";


            return appointment;
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
                    new CodeableConcept("http://hl7.org/fhir/ValueSet/encounter-participant-type", "PART", "Participation", "Participation")
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
                    new CodeableConcept("http://hl7.org/fhir/ValueSet/encounter-participant-type", "PART", "Participation", "Participation")
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
                        new CodeableConcept("http://hl7.org/fhir/ValueSet/encounter-participant-type", "PPRF", "primary performer", "primary performer")
                    }
                })
                .ToList();
        }

     }
}
