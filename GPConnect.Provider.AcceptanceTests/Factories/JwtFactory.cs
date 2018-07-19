namespace GPConnect.Provider.AcceptanceTests.Factories
{
    using System;
    using Constants;
    using Enum;
    using Helpers;

    public class JwtFactory
    {
        private readonly GpConnectInteraction _gpConnectInteraction;

        public JwtFactory(GpConnectInteraction gpConnectInteraction)
        {
            _gpConnectInteraction = gpConnectInteraction;
        }

        public void ConfigureJwt(JwtHelper jwtHelper)
        {
            jwtHelper.SetDefaultValues();

            switch (_gpConnectInteraction)
            {
                case GpConnectInteraction.MetadataRead:
                case GpConnectInteraction.OrganizationSearch:
                case GpConnectInteraction.OrganizationRead:
                case GpConnectInteraction.PractitionerSearch:
                case GpConnectInteraction.PractitionerRead:
                case GpConnectInteraction.LocationRead:
                case GpConnectInteraction.SearchForFreeSlots:
                    jwtHelper.RequestedScope = JwtConst.Scope.kOrganizationRead;
                    break;
                case GpConnectInteraction.GpcGetCareRecord:
                case GpConnectInteraction.GpcGetStructuredRecord:
                case GpConnectInteraction.PatientSearch:
                case GpConnectInteraction.PatientRead:
                case GpConnectInteraction.AppointmentSearch:
                case GpConnectInteraction.AppointmentRead:
                    jwtHelper.RequestedScope = JwtConst.Scope.kPatientRead;
                    break;
                case GpConnectInteraction.RegisterPatient:
                case GpConnectInteraction.AppointmentCreate:
                case GpConnectInteraction.AppointmentAmend:
                case GpConnectInteraction.AppointmentCancel:
                    jwtHelper.RequestedScope = JwtConst.Scope.kPatientWrite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
