// ReSharper disable ClassNeverInstantiated.Global

namespace GPConnect.Provider.AcceptanceTests.Constants
{
    internal static class JwtConst
    {
        internal static class Claims
        {
            public const string kRequestingSystemUrl = "iss";
            public const string kPractitionerId = "sub";
            public const string kAuthTokenURL = "aud";
            public const string kExpiryTime = "exp";
            public const string kCreationTime = "iat";
            public const string kReasonForRequest = "reason_for_request";
            public const string kRequestingDevice = "requesting_device";
            public const string kRequestingOrganization = "requesting_organization";
            public const string kRequestingPractitioner = "requesting_practitioner";
            public const string kRequestedScope = "requested_scope";
            public const string kRequestedRecord = "requested_record";
        }

        internal static class Values
        {
            public const string kDirectCare = "directcare";
            public const string kAuthTokenURL = "https://authorize.fhir.nhs.net/token";
        }

        internal static class Scope
        {
            public const string kPatientWrite = "patient/*.write";
            public const string kPatientRead = "patient/*.read";
            public const string kOrganizationRead = "organization/*.read";
        }
    }
}
