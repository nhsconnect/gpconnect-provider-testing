using GPConnect.Provider.AcceptanceTests.Constants;

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
    using static System.Net.WebUtility;

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
                    slot.Reference.ShouldStartWith(reference, Case.Sensitive, $"The Appointment Slot Reference should start with {reference}, but was {slot.Reference}.");
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

        [Then(@"the Appointment DeliveryChannel must be valid")]
        public void TheAppointmentDeliveryChannelMustBeValid()
        {
            Appointments.ForEach(appointment =>
            {
                var deliveryChannelExtensions = appointment.Extension.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kDeliveryChannel2Ext)).ToList();
                deliveryChannelExtensions.Count.ShouldBeGreaterThanOrEqualTo(0, "Incorrect number of Appointment delivery Channel Extensions have been returned.");
            });
        }
        // github ref 120
        // RMB 25/10/2018        
        [Then(@"the Appointment Not In Use should be valid")]
        public void TheAppointmentNotInUseShouldBeValid()
        {
            Appointments.ForEach(appointment =>
            {
                appointment.Specialty.Count.ShouldBe(0);
                appointment.Reason.Count.ShouldBe(0);
            });
        }

        [Then(@"the Appointment PractitionerRole must be valid")]
        public void TheAppointmentPractitionerRoleMustBeValid()
        {
            Appointments.ForEach(appointment =>
            {
                var practitionerRoleExtensions = appointment.Extension.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kPractitionerRoleExt)).ToList();
                practitionerRoleExtensions.Count.ShouldBeGreaterThanOrEqualTo(0, "Incorrect number of Appointment practitionerRole Extensions have been returned.");
            });
        }

        // added 1.2.1 RMB 4/10/2018
        [Then(@"the Appointment DeliveryChannel must be present")]
        public void TheAppointmentDeliveryChannelMustBePresent()
        {
            Appointments.ForEach(appointment =>
            {
                var deliveryChannelExtensions = appointment.Extension.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kDeliveryChannel2Ext)).ToList();
                deliveryChannelExtensions.Count.ShouldBeGreaterThanOrEqualTo(1, "Incorrect number of Appointment delivery Channel Extensions have been returned.");
            });
        }


        [Then(@"the Appointment PractitionerRole must be present")]
        public void TheAppointmentPractitionerRoleMustBePresent()
        {
            Appointments.ForEach(appointment =>
            {
                var practitionerRoleExtensions = appointment.Extension.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kPractitionerRoleExt)).ToList();
                practitionerRoleExtensions.Count.ShouldBeGreaterThanOrEqualTo(1, "Incorrect number of Appointment practitionerRole Extensions have been returned.");
            });
        }
        public void RetrieveAppoinmentsForNhsNumber(string nhsNumber)
        {
            _httpSteps.ConfigureRequest(GpConnectInteraction.AppointmentSearch);
            var date = DateTime.UtcNow;
            var startDate = date.ToString("yyyy-MM-dd");
            var endDate = date.AddDays(30).ToString("yyyy-MM-dd");

            var startKey = UrlEncode("start");
            var startValue = UrlEncode($"ge{startDate}");
            var endValue = UrlEncode($"le{endDate}");

            _httpContext.HttpRequestConfiguration.RequestUrl = $"{_httpContext.HttpRequestConfiguration.RequestUrl}?{startKey}={startValue}&{startKey}={endValue}";

            _httpSteps.MakeRequest(GpConnectInteraction.AppointmentSearch);
        }
    }
}
