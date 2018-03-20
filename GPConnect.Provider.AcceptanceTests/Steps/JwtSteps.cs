namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Linq;
    using Constants;
    using Context;
    using Extensions;
    using Helpers;
    using Hl7.Fhir.Model;
    using Logger;
    using Repository;
    using TechTalk.SpecFlow;

    [Binding]
    public class JwtSteps : Steps
    {
        private readonly HttpHeaderHelper _headerHelper;
        private readonly JwtHelper _jwtHelper;
        private readonly IFhirResourceRepository _fhirResourceRepository;
        private readonly HttpContext _httpContext;

        public JwtSteps(HttpHeaderHelper headerHelper, JwtHelper jwtHelper, IFhirResourceRepository fhirResourceRepository, HttpContext httpContext)
        {
            Log.WriteLine("JwtSteps() Constructor");
            _headerHelper = headerHelper;
            _jwtHelper = jwtHelper;
            _fhirResourceRepository = fhirResourceRepository;
            _httpContext = httpContext;
        }

        [Given(@"I set the JWT requested scope to ""(.*)""")]
        public void SetTheJwtRequestedScopeTo(string requestedScope)
        {
            _jwtHelper.RequestedScope = requestedScope;
            _headerHelper.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set the JWT Requested Scope to Patient Read")]
        public void SetTheJwtRequestedScopeToPatientRead()
        {
            _jwtHelper.RequestedScope = JwtConst.Scope.kPatientRead;
        }

        [Given(@"I set the JWT Requested Scope to be incorrect")]
        public void SetTheJwtRequestedScopeToBeIncorrect()
        {
            _jwtHelper.RequestedScope = "badScope";
        }

        [Given(@"I set the JWT Requested Scope to Organization Read")]
        public void SetTheJwtRequestedScopeToOrganizationRead()
        {
            _jwtHelper.RequestedScope = JwtConst.Scope.kOrganizationRead;
        }

        [Given(@"I set the JWT with missing Expiry Time")]
        public void SetTheJwtWithMissingExpiryTime()
        {
            _jwtHelper.ExpiryTime = null;
        }

        [Given(@"I set the JWT Expiry Time to ""(.*)"" seconds after Creation Time")]
        public void SetTheJwtExpiryTimeToSecondsAfterCreationTime(double seconds)
        {
            _jwtHelper.SetExpiryTimeInSeconds(seconds);
        }
        
        [Given(@"I set the JWT with missing Creation Time")]
        public void SetTheJwtWithMissingCreationTime()
        {
            _jwtHelper.CreationTime = null;
        }

        [Given(@"I set the JWT Creation Time to ""(.*)"" seconds in the future")]
        public void SetTheJwtCreationTimeToSecondsAfterTheCurrentTime(double seconds)
        {
            _jwtHelper.SetCreationTimeSeconds(seconds);
        }

        [Given(@"I ""(.*)"" JWT Creation Time and expiry time by ""(.*)"" seconds")]
        public void SetTheJwtCreationTimeAndExpiraryTimeToSecondsInThePast(string symbol,double seconds)
        {
            if (symbol == "-")
            {
                _jwtHelper.SetCreationTimeSecondsPast(seconds);
                _jwtHelper.SetExpiryTimeInSecondsPast(seconds);
            }
            if (symbol == "+")
            {
                _jwtHelper.SetCreationTimeSeconds(seconds);
                _jwtHelper.SetExpiryTimeInSeconds(seconds);
            }
        }

        [Given(@"I set the JWT Reason For Request to ""(.*)""")]
        public void SetTheJwtReasonForRequestTo(string reasonForRequest)
        {
            _jwtHelper.ReasonForRequest = reasonForRequest;
        }

        [Given(@"I set the JWT Authorization Server Token URL to ""(.*)""")]
        public void SetTheJwtAuthorizationServerTokenTo(string url)
        {
            _jwtHelper.AuthTokenURL = url;
        }

        [Given(@"I set the JWT with missing Requesting Device")]
        public void SetTheJwtWithMissingRequestingDevice()
        {
            _jwtHelper.RequestingDevice = null;
        }

        [Given(@"I set the JWT Requesting Device as an invalid Device")]
        public void SetTheJwtRequestingDeviceAsAnInvalidDevice()
        {
            _jwtHelper.RequestingDevice = FhirHelper.AddInvalidFieldToResourceJson(FhirHelper.GetDefaultDevice().ToFhirJson());
        }

        [Given(@"I set the JWT Requesting Device Resource Type as an invalid Resource Type")]
        public void SetTheJwtRequestingDeviceResourceTypeAsAnInvalidResourceType()
        {
            _jwtHelper.RequestingDevice = FhirHelper.ChangeResourceTypeString(_jwtHelper.RequestingDevice, FhirConst.Resources.kInvalidResourceType);
        }

        [Given(@"I set the JWT with missing Requesting Organization")]
        public void SetTheJwtWithMissingRequestingOrganization()
        {
            _jwtHelper.RequestingOrganization = null;
        }

        [Given(@"I set the JWT Requesting Organization as an invalid Organization")]
        public void SetTheJwtRequestingOrganizationAsAnInvalidOrganization()
        {
            _jwtHelper.RequestingOrganization = FhirHelper.AddInvalidFieldToResourceJson(FhirHelper.GetDefaultOrganization().ToFhirJson());
        }

        [Given(@"I set the JWT Requesting Organization Identifier with missing ODS Code")]
        public void SetTheJwtRequestingOrganizationIdentifierWithMissingOdsCode()
        {
            var organization = FhirHelper.GetDefaultOrganization();
            organization.Identifier.Clear();

            var identifier = new Identifier("http://fhir.nhs.net/Id/someOtherCodingSystem", "NoOdsCode");
            organization.Identifier.Add(identifier);

            _jwtHelper.RequestingOrganization = organization.ToFhirJson();
        }

        [Given(@"I set the JWT Requesting Organization with missing Identifier")]
        public void SetTheJwtRequestingOrganizationWithMissingIdentifier()
        {
            var organization = FhirHelper.GetDefaultOrganization();
            organization.Identifier.Clear();

            _jwtHelper.RequestingOrganization = organization.ToFhirJson();
        }

        [Given(@"I set the JWT Requesting Organization Resource Type as an invalid Resource Type")]
        public void SetTheJwtRequestingOrganizationResourceTypeAsAnInvalidResourceType()
        {
            _jwtHelper.RequestingOrganization = FhirHelper.ChangeResourceTypeString(_jwtHelper.RequestingOrganization, FhirConst.Resources.kInvalidResourceType);
        }

        [Given(@"I set the JWT with missing Requesting Identity")]
        public void SetTheJwtWithMissingRequestingPractitioner()
        {
            _jwtHelper.RequestingIdentity = null;
        }

        [Given(@"I set the JWT Requesting Identity as an invalid Identity")]
        public void SetTheJwtRequestingPractitionerAsAnInvalidPractitioner()
        {
            _jwtHelper.SetRequestingPractitioner("1", FhirHelper.AddInvalidFieldToResourceJson(FhirHelper.GetDefaultPractitioner().ToFhirJson()));
        }

        [Given(@"I set the JWT Requesting Practitioner with missing SDS Id")]
        public void SetTheJwtRequestingPractitionerWithMissingSdsId()
        {
            var practitioner = FhirHelper.GetDefaultPractitioner();
            practitioner.Identifier.Clear();

            var identifier = new Identifier("http://IdentifierServer/RandomId", "ABC123");
            practitioner.Identifier.Add(identifier);

            _jwtHelper.SetRequestingPractitioner("1", practitioner.ToFhirJson());
        }

        [Given(@"I set the JWT Requesting Practitioner with missing Identifier")]
        public void SetTheJwtRequestingPractitionerWithMissingIdentifier()
        {
            var practitioner = FhirHelper.GetDefaultPractitioner();
            practitioner.Identifier.Clear();

            _jwtHelper.SetRequestingPractitioner("1", practitioner.ToFhirJson());
        }

        [Given(@"I set the JWT Requesting Practitioner with User Id not matching")]
        public void SetTheJwtRequestingPractitionerWithUserIdNotMatching()
        {
            _jwtHelper.SetRequestingPractitioner("2", FhirHelper.GetDefaultPractitioner().ToFhirJson());
        }

        [Given(@"I set the JWT Requesting Practitioner with missing Name")]
        public void SetTheJwtRequestingPractitionerWithMissingName()
        {
            var practitioner = FhirHelper.GetDefaultPractitioner();
            practitioner.Name = null;

            _jwtHelper.SetRequestingPractitioner("1", practitioner.ToFhirJson());
        }

        [Given(@"I set the JWT Requesting Identity Resource Type as an invalid Resource Type")]
        public void SetTheJwtRequestingPractitionerResourceTypeAsAnInvalidResourceType()
        {
            _jwtHelper.SetRequestingPractitioner(_jwtHelper.RequestingIdentityId, FhirHelper.ChangeResourceTypeString(_jwtHelper.RequestingIdentity, FhirConst.Resources.kInvalidResourceType));
        }


       
        [Given(@"I set the JWT with missing Requesting System URL")]
        public void SetTheJwtWIthMissingRequestingSystemUrl()
        {
            _jwtHelper.RequestingSystemUrl = null;
        }

        [Given(@"I set the JWT with missing Requesting Practitioner Id")]
        public void SetTheJwtWIthMissingRequestingPractitionerId()
        {
            _jwtHelper.RequestingIdentityId = null;
        }

        [Given(@"I set the JWT with missing Authorization Server Token URL")]
        public void SetTheJwtWithMissingAuthorizationServerTokenUrl()
        {
            _jwtHelper.AuthTokenURL = null;
        }

        [Given(@"I set the JWT with missing Reason For Request")]
        public void SetTheJwtWithMissingReasonForRequest()
        {
            _jwtHelper.ReasonForRequest = null;
        }

        [Given(@"I set the JWT with missing Requested Scope")]
        public void SetTheJwtWithMissingRequestedScope()
        {
            _jwtHelper.RequestedScope = null;
        }

        [Given(@"I set the JWT Requesting Organization Identifier system to match the rc5 specification")]
        public void SetJWTRequestingOrganizationToOldURL()
        {
            var organization = FhirHelper.GetDefaultOrganization();
            organization.Identifier[0].System = FhirConst.IdentifierSystems.kOdsOrgzCodeBackwardCom;

            _jwtHelper.RequestingOrganization = organization.ToFhirJson();
        }
              
    }
}
