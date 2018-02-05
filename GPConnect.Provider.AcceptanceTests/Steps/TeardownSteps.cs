namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using Context;
    using Helpers;
    using Hl7.Fhir.Model;
    using TechTalk.SpecFlow;

    [Binding]
    internal class TeardownSteps : Steps
    {
        private static HttpContext _httpContext;
        private static CancelAppointmentSteps _cancelAppointmentSteps;
        private static PatientSteps _patientSteps;
        private static AppointmentRetrieveSteps _appointmentRetrieveSteps;
        private static bool appointmentCreated;

        public TeardownSteps(
            HttpContext httpContext, 
            CancelAppointmentSteps cancelAppointmentSteps, 
            PatientSteps patientSteps, 
            AppointmentRetrieveSteps appointmentRetrieveSteps
            )
        {
            _httpContext = httpContext;
            _cancelAppointmentSteps = cancelAppointmentSteps;
            _patientSteps = patientSteps;
            _appointmentRetrieveSteps = appointmentRetrieveSteps;
        }
        [BeforeScenario]
        public void SetFlagToFalse()
        {
            appointmentCreated = false;
        }

        [AfterScenario]
        public void Dummy()
        {
            
        }

        [AfterTestRun]
        public static void CancelCreatedAppointments()
        {
            if (AppSettingsHelper.TeardownEnabled && appointmentCreated == true)
            {
                StoreAllCreatedAppointments();
                CancelAllCreatedAppointments();
            }
        }

        public static void AppointmentCreated()
        {
            appointmentCreated = true;
        }

        private static void CancelAllCreatedAppointments()
        {
            var patientAppointmentMappings = GlobalContext.CreatedAppointments;

            foreach (var patientAppointmentMapping in patientAppointmentMappings.Where(pa => pa.Value.Count > 0))
            {
                CancelPatientsAppointments(patientAppointmentMapping);
            }
        }

        private static void CancelPatientsAppointments(KeyValuePair<string, List<Appointment>> patientAppointmentMapping)
        {
            foreach (var appointment in patientAppointmentMapping.Value)
            {
                try
                {
                    _cancelAppointmentSteps.CancelTheAppointmentWithLogicalId(appointment, patientAppointmentMapping.Key);
                }
                catch
                {
                    Logger.Log.WriteLine($"Could not cancel Appointment with Id = {appointment.Id} for Patient with NHS Number = {patientAppointmentMapping.Key}.");
                }
            }
        }

        private static void StoreAllCreatedAppointments()
        {
            var patients = GlobalContext.PatientNhsNumberMap;

            foreach (var patient in patients)
            {
                var patientAppointments = GetAppointments(patient.Key, patient.Value);

                StorePatientCreatedAppointments(patient.Value, patientAppointments);
            }
        }

        private static void StorePatientCreatedAppointments(string nhsNumber, List<Appointment> patientAppointments)
        {
            if (GlobalContext.CreatedAppointments == null)
            {
                GlobalContext.CreatedAppointments = new Dictionary<string, List<Appointment>>();
            }

            GlobalContext.CreatedAppointments.Add(nhsNumber, patientAppointments);
        }

        private static List<Appointment> GetAppointments(string key, string nhsNumber)
        {
            try
            {
                _patientSteps.GetThePatientForPatientValue(key);
                _patientSteps.StoreThePatient();

                _appointmentRetrieveSteps.RetrieveAppoinmentsForNhsNumber(nhsNumber);

                return _httpContext.FhirResponse.Appointments.Where(app => app.Status != Appointment.AppointmentStatus.Cancelled).ToList();
            }
            catch
            {
                Logger.Log.WriteLine($"Could not retrieve Appointments for config Patient ({key}) with NHS Number = {nhsNumber}.");

                return new List<Appointment>();
            }
        }
    }
}
