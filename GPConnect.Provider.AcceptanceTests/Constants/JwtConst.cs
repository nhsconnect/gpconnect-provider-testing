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
        }

        internal static class Values
        {
            public const string kDirectCare = "directcare";
            public const string kMigrate = "migration";
        }

        internal static class Scope
        {
            public const string kPatientWrite = "patient/*.write";
            public const string kPatientRead = "patient/*.read";
            public const string kOrganizationRead = "organization/*.read";
            public const string kMigrateWithoutSensitive = "patient/*.read conf/N";
            public const string kMigrateWithSensitive = "patient/*.read conf/R";
        }
    }
}
