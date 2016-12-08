using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Extensions;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Model;
using TechTalk.SpecFlow;

// ReSharper disable InconsistentNaming

namespace GPConnect.Provider.AcceptanceTests.Steps
{

    [Binding]
    public class JwtSteps : TechTalk.SpecFlow.Steps
    {
        // Headers Helper
        public HttpHeaderHelper Headers { get; }

        // JWT Helper
        public JwtHelper Jwt { get; }

        public JwtSteps(HttpHeaderHelper headerHelper, JwtHelper jwtHelper)
        {
            Log.WriteLine("JwtSteps() Constructor");
            // Helpers
            Headers = headerHelper;
            Jwt = jwtHelper;
        }

        // Before Scenario

        [BeforeScenario(Order = 3)]
        public void SetDefaultJWTValues()
        {
            Log.WriteLine("SetDefaultJWTValues()");
            Jwt.SetDefaultValues();
        }

        // JWT Configuration Steps

        [Given(@"I set the default JWT")]
        public void ISetTheDefaultJWT()
        {
            Jwt.SetDefaultValues();
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set the default JWT without base64 encoding")]
        public void ISetTheJWTWithoutBase64Encoding()
        {
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerTokenWithoutEncoding());
        }

        [Given(@"I set the JWT expiry time to ""(.*)"" seconds after creation time")]
        public void ISetTheJWTExpiryTimeToSecondsAfterCreationTime(double expirySeconds)
        {
            Jwt.SetExpiryTimeInSeconds(expirySeconds);
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set the JWT creation time to ""(.*)"" seconds after the current time")]
        public void ISetTheJWTCreationTimeToSecondsAfterTheCurrentTime(double secondsInFuture)
        {
            Jwt.SetCreationTimeSeconds(secondsInFuture);
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set the JWT reason for request to ""(.*)""")]
        public void ISetTheJWTReasonForRequestTo(string reasonForRequest)
        {
            Jwt.ReasonForRequest = reasonForRequest;
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set the JWT authorization server token URL to ""(.*)""")]
        public void ISetTheJWTAuthorizationServerTokenTo(string autTokenUrl)
        {
            Jwt.AuthTokenURL = autTokenUrl;
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set an invalid JWT requesting device resource")]
        public void ISetAnInvalidJWTRequestingDeviceResource()
        {
            Jwt.RequestingDevice = FhirHelper.AddInvalidFieldToResourceJson(FhirHelper.GetDefaultDevice().ToJson());
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set an invalid JWT requesting organization resource")]
        public void ISetAnInvalidJWTRequestingOrganizationResource()
        {
            Jwt.RequestingOrganization = FhirHelper.AddInvalidFieldToResourceJson(FhirHelper.GetDefaultOrganization().ToJson());
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set JWT requesting organization resource without ODS Code")]
        public void ISetJWTRequestingOrganizaitonResourceWithoutODSCode()
        {
            var organization = FhirHelper.GetDefaultOrganization();
            organization.Identifier.Clear();
            organization.Identifier.Add(new Identifier("http://fhir.nhs.net/Id/someOtherCodingSystem", "NoOdsCode"));
            Jwt.RequestingOrganization = organization.ToJson();
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set JWT requesting organization resource without identifier")]
        public void ISetJWTRequestingOrganizaitonResourceWithoutIdentifier()
        {
            var organization = FhirHelper.GetDefaultOrganization();
            organization.Identifier.Clear();
            Jwt.RequestingOrganization = organization.ToJson();
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set an invalid JWT requesting practitioner resource")]
        public void ISetAnInvalidJWTRequestingPractitionerResource()
        {
            Jwt.SetRequestingPractitioner("1", FhirHelper.AddInvalidFieldToResourceJson(FhirHelper.GetDefaultPractitioner().ToJson()));
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set a JWT requesting practitioner without SDS id")]
        public void ISetAJWTRequestingPractitionerWithoutSDSId()
        {
            var practitioner = FhirHelper.GetDefaultPractitioner();
            practitioner.Identifier.Clear();
            practitioner.Identifier.Add(new Identifier("http://IdentifierServer/RandomId", "ABC123"));
            Jwt.SetRequestingPractitioner("1", practitioner.ToJson());
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set a JWT requesting practitioner without identifier")]
        public void ISetAJWTRequestingPractitionerWithoutIdentifier()
        {
            var practitioner = FhirHelper.GetDefaultPractitioner();
            practitioner.Identifier.Clear();
            Jwt.SetRequestingPractitioner("1", practitioner.ToJson());
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set a JWT requesting practitioner with miss matched user id")]
        public void ISetAJWTRequestingPractitionerWithMissMatchedUserId()
        {
            Jwt.SetRequestingPractitioner("2", FhirHelper.GetDefaultPractitioner().ToJson());
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set a JWT requesting practitioner with missing name element")]
        public void ISetAJWTRequestingPractitionerWithMissingNameElement()
        {
            var practitioner = FhirHelper.GetDefaultPractitioner();
            practitioner.Name = null;
            Jwt.SetRequestingPractitioner("1", practitioner.ToJson());
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set a JWT requesting practitioner with missing SDS Job Role")]
        public void ISetAJWTRequestingPractitionerWithMissingSDSJobRole()
        {
            var practitioner = FhirHelper.GetDefaultPractitioner();
            practitioner.PractitionerRole = FhirHelper.GetPractitionerRoleComponent("http://invalidValueSetServer.nhs.uk", "NonSDSJobRoleName");
            Jwt.SetRequestingPractitioner("1", practitioner.ToJson());
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set the JWT requested scope to ""(.*)""")]
        public void ISetTheJWTRequestedScopeTo(string requestedScope)
        {
            Jwt.RequestedScope = requestedScope;
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set a JWT without iis claim")]
        public void ISetAJWTWithoutIISClaim()
        {
            Jwt.RequestingSystemUrl = null;
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set a JWT without sub claim")]
        public void ISetAJWTWithoutSubClaim()
        {
            Jwt.RequestingPractitionerId = null;
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set a JWT without aud claim")]
        public void ISetAJWTWithoutAudClaim()
        {
            Jwt.AuthTokenURL = null;
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set a JWT without exp claim")]
        public void ISetAJWTWithoutExpClaim()
        {
            Jwt.ExpiryTime= null;
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set a JWT without iat claim")]
        public void ISetAJWTWithoutIatClaim()
        {
            Jwt.CreationTime = null;
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set a JWT without reason for request claim")]
        public void ISetAJWTWithoutReasonForRequestClaim()
        {
            Jwt.ReasonForRequest = null;
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set a JWT without requested record claim")]
        public void ISetAJWTWithoutRequestedRecordClaim()
        {
            Jwt.RequestedPatientNHSNumber = null;
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set a JWT without requested scope claim")]
        public void ISetAJWTWithoutRequestedScopeClaim()
        {
            Jwt.RequestedScope = null;
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set a JWT without requesting device claim")]
        public void ISetAJWTWithoutRequestingDeviceClaim()
        {
            Jwt.RequestingDevice = null;
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set a JWT without requesting organization claim")]
        public void ISetAJWTWithoutRequestingOrganizationClaim()
        {
            Jwt.RequestingOrganization = null;
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set a JWT without requesting practitioner claim")]
        public void ISetAJWTWithoutRequestingPractitionerClaim()
        {
            Jwt.RequestingPractitioner = null;
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I change the JWT requesting device resource type to InvalidResourceType")]
        public void IChangeTheJWTRequestingDeviceResourceTypeToInvalidResourceType()
        {
            Jwt.RequestingDevice = FhirHelper.ChangeResourceTypeString(Jwt.RequestingDevice, FhirConst.Resources.InvalidResourceType);
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I change the JWT requesting organization resource type to InvalidResourceType")]
        public void IChangeTheJWTRequestingOrganizationResourceTypeToInvalidResourceType()
        {
            Jwt.RequestingOrganization = FhirHelper.ChangeResourceTypeString(Jwt.RequestingOrganization, FhirConst.Resources.InvalidResourceType);
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I change the JWT requesting practitioner resource type to InvalidResourceType")]
        public void IChangeTheJWTRequestingPractitionerResourceTypeToInvalidResourceType()
        {
            Jwt.SetRequestingPractitioner(Jwt.RequestingPractitionerId, FhirHelper.ChangeResourceTypeString(Jwt.RequestingPractitioner, FhirConst.Resources.InvalidResourceType));
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }

        [Given(@"I set the JWT requested record patient NHS number to ""(.*)""")]
        public void ISetTheJWTRequestedRecordPatientNHSNumberTo(string nhsNumber)
        {
            Jwt.RequestedPatientNHSNumber = nhsNumber;
            Headers.ReplaceHeader(HttpConst.Headers.Authorization, Jwt.GetBearerToken());
        }
    }
}
