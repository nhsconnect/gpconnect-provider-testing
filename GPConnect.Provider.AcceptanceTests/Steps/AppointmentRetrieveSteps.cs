namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using Context;
    using Hl7.Fhir.Model;
    using Shouldly;
    using TechTalk.SpecFlow;
    using System.Linq;

    [Binding]
    public class AppointmentRetrieveSteps : Steps
    {
        private readonly FhirContext _fhirContext;
        private List<Appointment> Appointments => _fhirContext.Appointments;

        public AppointmentRetrieveSteps(FhirContext fhirContext)
        {
            _fhirContext = fhirContext;
        }

        [Then(@"the Appointment Status should be valid")]
        public void TheAppointmentStatusShouldBeValid()
        {
            Appointments.ForEach(appointment =>
            {
                appointment.Status.ShouldNotBeNull("The Appointment Status should not be null.");
            });
        }

        [Then("the Appointment Start should be valid")]
        public void TheAppointmentStartShouldBeValid()
        {
            Appointments.ForEach(appointment =>
            {
                appointment.Start.ShouldNotBeNull("The Appointment Start should not be null.");
            });
        }

        [Then("the Appointment End should be valid")]
        public void TheAppointmentEndShouldBeValid()
        {
            Appointments.ForEach(appointment =>
            {
                appointment.End.ShouldNotBeNull("The Appointment End should not be null.");
            });
        }

        [Then("the Appointment Slots should be valid")]
        public void TheAppointmentSlotsShouldBeValid()
        {
            Appointments.ForEach(appointment =>
            {
                appointment.Slot?.Count.ShouldBeGreaterThanOrEqualTo(1, "The Appointment should contain at least 1 Slot, but contained none.");

                appointment.Slot?.ForEach(slot =>
                {
                    var reference = "Slot/";
                    slot.Reference.ShouldStartWith(reference, $"The Appointment Slot Reference should start with {reference}, but was {slot.Reference}.");
                });
            });
        }

        [Then(@"the Appointment Reason should be valid")]
        public void TheAppointmentReasonShouldBeValid()
        {
            Appointments.ForEach(appointment =>
            {
                if (appointment.Reason?.Coding != null)
                {
                    const string snomed = "http://snomed.info/sct";
                    const string readV2 = "http://read.info/readv2";
                    const string ctv3 = "http://read.info/ctv3";

                    appointment.Reason.Coding.ForEach(coding =>
                    {
                        coding.System.ShouldBeOneOf(snomed, readV2, ctv3, $"The Appointment Reason Coding System should be one of {snomed}, {readV2}, {ctv3} but was {coding.System}.");
                    });

                    var snomedCodingCount = appointment.Reason.Coding.Count(coding => coding.System == snomed);
                    snomedCodingCount.ShouldBeLessThanOrEqualTo(1, $"The Appointment Reason Coding Systems should contain a maximum of 1 SNOMED Coding Systems but contained {snomedCodingCount}.");

                    var readV2Count = appointment.Reason.Coding.Count(coding => coding.System == readV2);
                    readV2Count.ShouldBeLessThanOrEqualTo(1, $"The Appointment Reason Coding Systems should contain a maximum of 1 READV2 Coding Systems but contained {readV2Count}.");

                    var ctv3Count = appointment.Reason.Coding.Count(coding => coding.System == ctv3);
                    ctv3Count.ShouldBeLessThanOrEqualTo(1, $"The Appointment Reason Coding Systems should contain a maximum of 1 CTV3 Coding Systems but contained {ctv3Count}.");
                }
            });
        }
    }
}
