namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using Context;
    using Hl7.Fhir.Model;
    using Shouldly;
    using TechTalk.SpecFlow;
    using System.Linq;
    using System;
    using System.Globalization;
    using Enum;

    [Binding]
    public class AppointmentRetrieveSteps : Steps
    {
        private readonly HttpContext _httpContext;
        private readonly HttpSteps _httpSteps;
        private readonly JwtSteps _jwtSteps;

        private List<Appointment> Appointments => _httpContext.FhirResponse.Appointments;

        public AppointmentRetrieveSteps(HttpContext httpContext, HttpSteps httpSteps, JwtSteps jwtSteps)
        {
            _httpContext = httpContext;
            _httpSteps = httpSteps;
            _jwtSteps = jwtSteps;
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

        [Then(@"the Appointment Description must be valid")]
        public void TheAppointmentDescriptionShouldNotBeNull()
        {
            Appointments.ForEach(appointment =>
            {
                appointment.Description.ShouldNotBeNull("Appointment description should not be null");
            });
        }

        [Then(@"the Appointment Created must be valid")]
        public void TheAppointmentCreatedMustBeValid()
        {
            Appointments.ForEach(appointment =>
            {
                appointment.Created.ShouldNotBeNullOrEmpty("The Appointment Created should not be null or empty, but was.");
            });
        }

        public void RetrieveAppoinmentsForNhsNumber(string nhsNumber)
        {
            _httpSteps.ConfigureRequest(GpConnectInteraction.AppointmentSearch);

            _httpSteps.MakeRequest(GpConnectInteraction.AppointmentSearch);
        }
    }
}
