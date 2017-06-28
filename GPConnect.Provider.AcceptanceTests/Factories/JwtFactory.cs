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
                    ConfigureGpcGetCareRecordJwt(jwtHelper, httpContext);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void ConfigureGpcGetCareRecordJwt(JwtHelper jwtHelper, HttpContext httpContext)
        {
            jwtHelper.RequestedScope = JwtConst.Scope.kPatientRead;
            httpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, jwtHelper.GetBearerToken());
        }
    }
}
