namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using Context;
    using Hl7.Fhir.Model;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    public class AmendAppointmentSteps : Steps
    {
        private readonly HttpContext _httpContext;
        private List<Appointment> Appointments => _httpContext.FhirResponse.Appointments;

        public AmendAppointmentSteps(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        [Then(@"the Appointment Reason should equal ""(.*)""")]
        public void TheAppointmentReasonShouldBeValidFor(string value)
        {
            Appointments.ForEach(appointment =>
            {
                appointment.Reason?.Text.ShouldBe(value, $@"The Appointment Comment should be ""{value} but was ""{appointment.Comment}"".");
            });
        }

        [Then(@"the Appointment Reason Text should be valid for ""(.*)""")]
        public void TheAppointmentReasonTextShouldBeValidFor(string value)
        {
            Appointments.ForEach(appointment =>
            {
                appointment.Reason?.Text.ShouldBe(value, $@"The Appointment Reason Text should be ""{value}"" but was ""{appointment.Reason?.Text}"".");
            });
        }
        
        [Then(@"the Appointment Description should be valid for ""(.*)""")]
        public void TheAppointmentDescriptionShouldBeValidFor(string value)
        {
            Appointments.ForEach(appointment =>
            {
                appointment.Description.ShouldBe(value, $@"The Appointment Description should be ""{value}"" but was ""{appointment.Description}"".");
            });
        }

        [Then(@"the Appointment Comment should be valid for ""(.*)""")]
        public void TheAppointmentCommentShouldBeValidFor(string value)
        {
            Appointments.ForEach(appointment =>
            {
                appointment.Comment.ShouldBe(value, $@"The Appointment Description should be ""{value}"" but was ""{appointment.Comment}"".");
            });
        }
    }
}
