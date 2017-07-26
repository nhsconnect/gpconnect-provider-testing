namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using Context;
    using Hl7.Fhir.Model;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    public class AppointmentReadSteps : Steps
    {
        private readonly HttpContext _httpContext;
        private List<Appointment> Appointments => _httpContext.HttpResponse.Appointments;

        public AppointmentReadSteps(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        [Then("the Appointment Identifiers should be valid")]
        public void AppointmentIdentifiersShouldBeValid()
        {
            Appointments.ForEach(appointment =>
            {
                appointment.Identifier.ForEach(identifier =>
                {
                    identifier.Value.ShouldNotBeNullOrEmpty($"The Appointment Identifier Value should not be null or empty but was {identifier.Value}.");
                });
            });
        }

        [Then("the Appointment Priority should be valid")]
        public void TheAppointmentPriorityShouldBeValid()
        {
            Appointments.ForEach(appointment =>
            {
                appointment.Priority?.ShouldBeInRange(0, 9, $"The priority should be between 0 and 9, but was {appointment.Priority.Value}.");
            });
        }
    }
}
