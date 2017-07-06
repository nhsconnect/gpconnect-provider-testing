namespace GPConnect.Provider.AcceptanceTests.Factories
{
    using System;
    using Constants;
    using Context;
    using Enum;
    using Helpers;

    public class JwtFactory
    {
        private readonly GpConnectInteraction _gpConnectInteraction;

        public JwtFactory(GpConnectInteraction gpConnectInteraction)
        {
            _gpConnectInteraction = gpConnectInteraction;
        }

        public void ConfigureJwt(JwtHelper jwtHelper, HttpContext httpContext)
        {
            jwtHelper.SetDefaultValues();

            switch (_gpConnectInteraction)
            {
                case GpConnectInteraction.GpcGetCareRecord:
                    ConfigureGpcGetCareRecordJwt(jwtHelper);
                    break;
                case GpConnectInteraction.OrganizationSearch:
                    ConfigureOrganizationSearchJwt(jwtHelper);
                    break;
                case GpConnectInteraction.OrganizationRead:
                    ConfigureOrganizationReadJwt(jwtHelper);
                    break;
                case GpConnectInteraction.PractitionerSearch:
                    ConfigurePractitionerSearchJwt(jwtHelper);
                    break;
                case GpConnectInteraction.PractitionerRead:
                    ConfigurePractitionerReadJwt(jwtHelper);
                    break;
                case GpConnectInteraction.PatientSearch:
                    ConfigurePatientSearchJwt(jwtHelper);
                    break;
                case GpConnectInteraction.PatientRead:
                    ConfigurePatientReadJwt(jwtHelper);
                    break;
                case GpConnectInteraction.LocationSearch:
                    ConfigureLocationSearchJwt(jwtHelper);
                    break;
                case GpConnectInteraction.LocationRead:
                    ConfigureLocationReadJwt(jwtHelper);
                    break;
                case GpConnectInteraction.RegisterPatient:
                    ConfigureRegisterPatientJwt(jwtHelper);
                    break;
                case GpConnectInteraction.GpcGetSchedule:
                    ConfigureGpcGetScheduleJwt(jwtHelper);
                    break;
                case GpConnectInteraction.AppointmentCreate:
                    ConfigureAppointmentCreateJwt(jwtHelper);
                    break;
                case GpConnectInteraction.AppointmentSearch:
                    ConfigureAppointmentSearchJwt(jwtHelper);
                    break;
                case GpConnectInteraction.AppointmentAmend:
                    ConfigureAppointmentAmendJwt(jwtHelper);
                    break;
                case GpConnectInteraction.AppointmentCancel:
                    ConfigureAppointmentCancelJwt(jwtHelper);
                    break;
                case GpConnectInteraction.AppointmentRead:
                    ConfigureAppointmentReadJwt(jwtHelper);
                    break;
                case GpConnectInteraction.MetadataRead:
                    ConfigureMetadataReadJwt(jwtHelper);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, jwtHelper.GetBearerToken());
        }

        private static void ConfigureGpcGetCareRecordJwt(JwtHelper jwtHelper)
        {
            jwtHelper.RequestedScope = JwtConst.Scope.kPatientRead;
        }

        private static void ConfigureOrganizationSearchJwt(JwtHelper jwtHelper)
        {
            jwtHelper.RequestedScope = JwtConst.Scope.kOrganizationRead;
        }

        private static void ConfigureOrganizationReadJwt(JwtHelper jwtHelper)
        {
            jwtHelper.RequestedScope = JwtConst.Scope.kOrganizationRead;
        }

        private static void ConfigurePractitionerSearchJwt(JwtHelper jwtHelper)
        {
            jwtHelper.RequestedScope = JwtConst.Scope.kOrganizationRead;
        }

        private static void ConfigurePatientSearchJwt(JwtHelper jwtHelper)
        {
            jwtHelper.RequestedScope = JwtConst.Scope.kPatientRead;
        }

        private static void ConfigurePractitionerReadJwt(JwtHelper jwtHelper)
        {
            jwtHelper.RequestedScope = JwtConst.Scope.kOrganizationRead;
        }

        private static void ConfigurePatientReadJwt(JwtHelper jwtHelper)
        {
            jwtHelper.RequestedScope = JwtConst.Scope.kPatientRead;
        }

        private static void ConfigureLocationSearchJwt(JwtHelper jwtHelper)
        {
            jwtHelper.RequestedScope = JwtConst.Scope.kOrganizationRead;
        }

        private static void ConfigureLocationReadJwt(JwtHelper jwtHelper)
        {
            jwtHelper.RequestedScope = JwtConst.Scope.kOrganizationRead;
        }

        private static void ConfigureRegisterPatientJwt(JwtHelper jwtHelper)
        {
            jwtHelper.RequestedScope = JwtConst.Scope.kPatientWrite;
        }

        private static void ConfigureGpcGetScheduleJwt(JwtHelper jwtHelper)
        {
            jwtHelper.RequestedScope = JwtConst.Scope.kOrganizationRead;
        }

        private static void ConfigureAppointmentCreateJwt(JwtHelper jwtHelper)
        {
            jwtHelper.RequestedScope = JwtConst.Scope.kPatientWrite;
        }

        private static void ConfigureAppointmentSearchJwt(JwtHelper jwtHelper)
        {
            jwtHelper.RequestedScope = JwtConst.Scope.kPatientRead;
        }

        private static void ConfigureAppointmentAmendJwt(JwtHelper jwtHelper)
        {
            jwtHelper.RequestedScope = JwtConst.Scope.kPatientWrite;
        }

        private static void ConfigureAppointmentCancelJwt(JwtHelper jwtHelper)
        {
            jwtHelper.RequestedScope = JwtConst.Scope.kPatientWrite;
        }

        private static void ConfigureAppointmentReadJwt(JwtHelper jwtHelper)
        {
            jwtHelper.RequestedScope = JwtConst.Scope.kPatientRead;
        }

        private static void ConfigureMetadataReadJwt(JwtHelper jwtHelper)
        {
            jwtHelper.RequestedScope = JwtConst.Scope.kOrganizationRead;
        }
    }
}
