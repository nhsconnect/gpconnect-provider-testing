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

        public Appointment BuildAppointment(Boolean addOrgType, Boolean addDeliveryChannel, Boolean addPracRole)
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
            var bookingOrganization = GetBookingOrganization(addOrgType);

            // git hub ref 203 (demonstrator)
            // RMB 27/2/19
            // Meta Profile
            var ApptMeta = new Meta();
            IEnumerable<string> MetaProfile = new string[] { FhirConst.StructureDefinitionSystems.kAppointment };
            ApptMeta.Profile = MetaProfile;

            string serviceType = "";
            if (firstSlot.ServiceType != null)
            {
                if (firstSlot.ServiceType.Count() >= 1)
                {
                    serviceType = firstSlot.ServiceType.First().Text;
                }

            }

            string serviceCategory = "";
            if (schedule.ServiceCategory != null)
            {
                if (schedule.ServiceCategory.Text != null)
                {
                    serviceCategory = schedule.ServiceCategory.Text;
                }

            }

            //Initialize Appointment
            var appointment = new Appointment
            {
                Meta = ApptMeta,
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



            //Practitioner Role
            if (addPracRole)
            {
                CodeableConcept roleCode = new CodeableConcept("https://fhir.nhs.uk/STU3/CodeSystem/CareConnect-SDSJobRoleName-1", "R0260", "General Medical Practitioner");
                Extension pracRole = new Extension("https://fhir.nhs.uk/STU3/StructureDefinition/Extension-GPConnect-PractitionerRole-1", roleCode);
                appointment.Extension.Add(pracRole);
            }

            //Delivery Channel
            if (addDeliveryChannel)
            {
                // git hub ref 203 (demonstrator) failed if channel code <> In-Person
                // RMB 28/2/19
                Extension sExt = firstSlot.GetExtension(FhirConst.StructureDefinitionSystems.kDeliveryChannel2Ext);
                var channelCode = sExt.Value;
                //Code channelCode = new Code("In-person"); 
                Extension delChannel = new Extension("https://fhir.nhs.uk/STU3/StructureDefinition/Extension-GPConnect-DeliveryChannel-2", channelCode);
                appointment.Extension.Add(delChannel);
            }

            // git hub ref 203 (demonstrator)
            // RMB 27/2/19
            //CodeableConcept type = new CodeableConcept("http://hl7.org/fhir/ValueSet/c80-practice-codes", "394802001", "General medicine", null);
            //appointment.AppointmentType = type;


            //add serviceCategory
            if (serviceCategory != "")
            {
                CodeableConcept sc = new CodeableConcept(null, null, null, serviceCategory);
                appointment.ServiceCategory = sc;
            }

            //add serviceType
            if (serviceType != "")
            {
                List<CodeableConcept> stList = new List<CodeableConcept>
                {
                        new CodeableConcept(null, null, null, serviceType)
                };
                appointment.ServiceType = stList;
            }


            return appointment;
        }

        private static Organization GetBookingOrganization(Boolean addOrgType)
        {
            var bookingOrganization = new Organization
            {
                Id = "1",
                Name = "Test Suite Validator"
            };
            // git hub ref 203 (demonstrator)
            // RMB 27/2/19
            // Meta Profile
            var OrgMeta = new Meta();
            IEnumerable<string> MetaProfile = new string[] { FhirConst.StructureDefinitionSystems.kOrganisation };
            OrgMeta.Profile = MetaProfile;
            bookingOrganization.Meta = OrgMeta;

            bookingOrganization.Identifier.Add(new Identifier(FhirConst.IdentifierSystems.kOdsOrgzCode, GlobalContext.OdsCodeMap["ORG1"]));
            if (addOrgType)
            {
                CodeableConcept ot = new CodeableConcept("https://fhir.nhs.uk/STU3/CodeSystem/GPConnect-OrganisationType-1", "urgent-care", "Urgent Care");
                bookingOrganization.Type.Add(ot);
            }
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
                    new CodeableConcept(FhirConst.CodeSystems.kEncounterParticipantType, "PART", "Participation", "Participation")
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
                    new CodeableConcept(FhirConst.CodeSystems.kEncounterParticipantType, "PART", "Participation", "Participation")
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
                        new CodeableConcept(FhirConst.CodeSystems.kEncounterParticipantType, "PPRF", "primary performer", "primary performer")
                    }
                })
                .ToList();
        }

    }
}
