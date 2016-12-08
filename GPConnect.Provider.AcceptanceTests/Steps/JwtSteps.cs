using System;
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
        private readonly HttpHeaderHelper _headerController;
        private readonly JwtHelper _jwtHelper;

        public JwtSteps(HttpHeaderHelper headerHelper, JwtHelper jwtHelper)
        {
            Log.WriteLine("JwtSteps() Constructor");
            _headerController = headerHelper;
            _jwtHelper = jwtHelper;
        }

        // JWT Configuration Steps

        [Given(@"I set the default JWT")]
        public void ISetTheDefaultJWT()
        {
            _jwtHelper.SetDefaultValues();
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set the default JWT without base64 encoding")]
        public void ISetTheJWTWithoutBase64Encoding()
        {
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerTokenWithoutEncoding());
        }

        [Given(@"I set the JWT expiry time to ""(.*)"" seconds after creation time")]
        public void ISetTheJWTExpiryTimeToSecondsAfterCreationTime(double expirySeconds)
        {
            _jwtHelper.SetExpiryTimeInSeconds(expirySeconds);
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set the JWT creation time to ""(.*)"" seconds after the current time")]
        public void ISetTheJWTCreationTimeToSecondsAfterTheCurrentTime(double secondsInFuture)
        {
            _jwtHelper.SetCreationTimeSeconds(secondsInFuture);
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set the JWT reason for request to ""(.*)""")]
        public void ISetTheJWTReasonForRequestTo(string reasonForRequest)
        {
            _jwtHelper.ReasonForRequest = reasonForRequest;
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set the JWT authorization server token URL to ""(.*)""")]
        public void ISetTheJWTAuthorizationServerTokenTo(string autTokenUrl)
        {
            _jwtHelper.AuthTokenURL = autTokenUrl;
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set an invalid JWT requesting device resource")]
        public void ISetAnInvalidJWTRequestingDeviceResource()
        {
            _jwtHelper.RequestingDevice = FhirHelper.AddInvalidFieldToResourceJson(FhirHelper.GetDefaultDevice().ToJson());
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set an invalid JWT requesting organization resource")]
        public void ISetAnInvalidJWTRequestingOrganizationResource()
        {
            _jwtHelper.RequestingOrganization = FhirHelper.AddInvalidFieldToResourceJson(FhirHelper.GetDefaultOrganization().ToJson());
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set JWT requesting organization resource without ODS Code")]
        public void ISetJWTRequestingOrganizaitonResourceWithoutODSCode()
        {
            var organization = FhirHelper.GetDefaultOrganization();
            organization.Identifier.Clear();
            organization.Identifier.Add(new Identifier("http://fhir.nhs.net/Id/someOtherCodingSystem", "NoOdsCode"));
            _jwtHelper.RequestingOrganization = organization.ToJson();
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set JWT requesting organization resource without identifier")]
        public void ISetJWTRequestingOrganizaitonResourceWithoutIdentifier()
        {
            var organization = FhirHelper.GetDefaultOrganization();
            organization.Identifier.Clear();
            _jwtHelper.RequestingOrganization = organization.ToJson();
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set an invalid JWT requesting practitioner resource")]
        public void ISetAnInvalidJWTRequestingPractitionerResource()
        {
            _jwtHelper.SetRequestingPractitioner("1", FhirHelper.AddInvalidFieldToResourceJson(FhirHelper.GetDefaultPractitioner().ToJson()));
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set a JWT requesting practitioner without SDS id")]
        public void ISetAJWTRequestingPractitionerWithoutSDSId()
        {
            var practitioner = FhirHelper.GetDefaultPractitioner();
            practitioner.Identifier.Clear();
            practitioner.Identifier.Add(new Identifier("http://IdentifierServer/RandomId", "ABC123"));
            _jwtHelper.SetRequestingPractitioner("1", practitioner.ToJson());
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set a JWT requesting practitioner without identifier")]
        public void ISetAJWTRequestingPractitionerWithoutIdentifier()
        {
            var practitioner = FhirHelper.GetDefaultPractitioner();
            practitioner.Identifier.Clear();
            _jwtHelper.SetRequestingPractitioner("1", practitioner.ToJson());
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set a JWT requesting practitioner with miss matched user id")]
        public void ISetAJWTRequestingPractitionerWithMissMatchedUserId()
        {
            _jwtHelper.SetRequestingPractitioner("2", FhirHelper.GetDefaultPractitioner().ToJson());
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set a JWT requesting practitioner with missing name element")]
        public void ISetAJWTRequestingPractitionerWithMissingNameElement()
        {
            var practitioner = FhirHelper.GetDefaultPractitioner();
            practitioner.Name = null;
            _jwtHelper.SetRequestingPractitioner("1", practitioner.ToJson());
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set a JWT requesting practitioner with missing SDS Job Role")]
        public void ISetAJWTRequestingPractitionerWithMissingSDSJobRole()
        {
            var practitioner = FhirHelper.GetDefaultPractitioner();
            practitioner.PractitionerRole = FhirHelper.GetPractitionerRoleComponent("http://invalidValueSetServer.nhs.uk", "NonSDSJobRoleName");
            _jwtHelper.SetRequestingPractitioner("1", practitioner.ToJson());
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set the JWT requested scope to ""(.*)""")]
        public void ISetTheJWTRequestedScopeTo(string requestedScope)
        {
            _jwtHelper.RequestedScope = requestedScope;
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set a JWT without iis claim")]
        public void ISetAJWTWithoutIISClaim()
        {
            _jwtHelper.RequestingSystemUrl = null;
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set a JWT without sub claim")]
        public void ISetAJWTWithoutSubClaim()
        {
            _jwtHelper.RequestingPractitionerId = null;
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set a JWT without aud claim")]
        public void ISetAJWTWithoutAudClaim()
        {
            _jwtHelper.AuthTokenURL = null;
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set a JWT without exp claim")]
        public void ISetAJWTWithoutExpClaim()
        {
            _jwtHelper.ExpiryTime= null;
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set a JWT without iat claim")]
        public void ISetAJWTWithoutIatClaim()
        {
            _jwtHelper.CreationTime = null;
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set a JWT without reason for request claim")]
        public void ISetAJWTWithoutReasonForRequestClaim()
        {
            _jwtHelper.ReasonForRequest = null;
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set a JWT without requested record claim")]
        public void ISetAJWTWithoutRequestedRecordClaim()
        {
            _jwtHelper.RequestedPatientNHSNumber = null;
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set a JWT without requested scope claim")]
        public void ISetAJWTWithoutRequestedScopeClaim()
        {
            _jwtHelper.RequestedScope = null;
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set a JWT without requesting device claim")]
        public void ISetAJWTWithoutRequestingDeviceClaim()
        {
            _jwtHelper.RequestingDevice = null;
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set a JWT without requesting organization claim")]
        public void ISetAJWTWithoutRequestingOrganizationClaim()
        {
            _jwtHelper.RequestingOrganization = null;
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set a JWT without requesting practitioner claim")]
        public void ISetAJWTWithoutRequestingPractitionerClaim()
        {
            _jwtHelper.RequestingPractitioner = null;
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I change the JWT requesting device resource type to InvalidResourceType")]
        public void IChangeTheJWTRequestingDeviceResourceTypeToInvalidResourceType()
        {
            _jwtHelper.RequestingDevice = FhirHelper.ChangeResourceTypeString(_jwtHelper.RequestingDevice, FhirConst.Resources.InvalidResourceType);
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I change the JWT requesting organization resource type to InvalidResourceType")]
        public void IChangeTheJWTRequestingOrganizationResourceTypeToInvalidResourceType()
        {
            _jwtHelper.RequestingOrganization = FhirHelper.ChangeResourceTypeString(_jwtHelper.RequestingOrganization, FhirConst.Resources.InvalidResourceType);
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I change the JWT requesting practitioner resource type to InvalidResourceType")]
        public void IChangeTheJWTRequestingPractitionerResourceTypeToInvalidResourceType()
        {
            _jwtHelper.SetRequestingPractitioner(_jwtHelper.RequestingPractitionerId, FhirHelper.ChangeResourceTypeString(_jwtHelper.RequestingPractitioner, FhirConst.Resources.InvalidResourceType));
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set the JWT requested record patient NHS number to ""(.*)""")]
        public void ISetTheJWTRequestedRecordPatientNHSNumberTo(string nhsNumber)
        {
            _jwtHelper.RequestedPatientNHSNumber = nhsNumber;
            _headerController.ReplaceHeader(HttpConst.Headers.Authorization, _jwtHelper.GetBearerToken());
        }
    }
}
