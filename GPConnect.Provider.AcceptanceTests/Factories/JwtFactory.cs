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
    }
}
