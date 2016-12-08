// ReSharper disable ClassNeverInstantiated.Global

namespace GPConnect.Provider.AcceptanceTests.Constants
{
    internal static class JwtConst
    {
        internal static class Claims
        {
            public const string RequestingSystemUrl = "iss";
            public const string PractitionerId = "sub";
            public const string AuthTokenURL = "aud";
            public const string ExpiryTime = "exp";
            public const string CreationTime = "iat";
            public const string ReasonForRequest = "reason_for_request";
            public const string RequestingDevice = "requesting_device";
            public const string RequestingOrganization = "requesting_organization";
            public const string RequestingPractitioner = "requesting_practitioner";
            public const string RequestedScope = "requested_scope";
            public const string RequestedRecord = "requested_record";
        }

        internal static class Values
        {
            public const string DirectCare = "directcare";
            public const string AuthTokenURL = "https://authorize.fhir.nhs.net/token";
        }

        internal static class Scope
        {
            public const string PatientRead = "patient/*.read";
            public const string OrganizationRead = "organization/*.read";
        }
    }
}
