namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Linq;
    using Constants;
    using Context;
    using Extensions;
    using Helpers;
    using Hl7.Fhir.Model;
    using Logger;
    using TechTalk.SpecFlow;

    [Binding]
    public class JwtSteps : Steps
    {
        private readonly HttpContext _httpContext;
        private readonly HttpHeaderHelper _headerHelper;
        private readonly JwtHelper _jwtHelper;

        public JwtSteps(HttpHeaderHelper headerHelper, JwtHelper jwtHelper, HttpContext httpContext)
        {
            Log.WriteLine("JwtSteps() Constructor");
            _httpContext = httpContext;
            _headerHelper = headerHelper;
            _jwtHelper = jwtHelper;
        }

        // Before Scenario
        [BeforeScenario(Order = 3)]
        public void SetDefaultJwtValues()
        {
            Log.WriteLine("SetDefaultJWTValues()");
            _jwtHelper.SetDefaultValues();
        }

        // JWT Configuration Steps
        [Given(@"I set the default JWT")]
        public void SetTheDefaultJwt()
        {
            _jwtHelper.SetDefaultValues();
            _headerHelper.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerToken());
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
            _jwtHelper.RequestingDevice = FhirHelper.AddInvalidFieldToResourceJson(FhirHelper.GetDefaultDevice().ToJson());
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
            _jwtHelper.RequestingOrganization = FhirHelper.AddInvalidFieldToResourceJson(FhirHelper.GetDefaultOrganization().ToJson());
        }

        [Given(@"I set the JWT Requesting Organization Identifier with missing ODS Code")]
        public void SetTheJwtRequestingOrganizationIdentifierWithMissingOdsCode()
        {
            var organization = FhirHelper.GetDefaultOrganization();
            organization.Identifier.Clear();

            var identifier = new Identifier("http://fhir.nhs.net/Id/someOtherCodingSystem", "NoOdsCode");
            organization.Identifier.Add(identifier);

            _jwtHelper.RequestingOrganization = organization.ToJson();
        }

        [Given(@"I set the JWT Requesting Organization with missing Identifier")]
        public void SetTheJwtRequestingOrganizationWithMissingIdentifier()
        {
            var organization = FhirHelper.GetDefaultOrganization();
            organization.Identifier.Clear();

            _jwtHelper.RequestingOrganization = organization.ToJson();
        }

        [Given(@"I set the JWT Requesting Organization Resource Type as an invalid Resource Type")]
        public void SetTheJwtRequestingOrganizationResourceTypeAsAnInvalidResourceType()
        {
            _jwtHelper.RequestingOrganization = FhirHelper.ChangeResourceTypeString(_jwtHelper.RequestingOrganization, FhirConst.Resources.kInvalidResourceType);
        }

        [Given(@"I set the JWT with missing Requesting Practitioner")]
        public void SetTheJwtWithMissingRequestingPractitioner()
        {
            _jwtHelper.RequestingPractitioner = null;
        }

        [Given(@"I set the JWT Requesting Practitioner as an invalid Practitioner")]
        public void SetTheJwtRequestingPractitionerAsAnInvalidPractitioner()
        {
            _jwtHelper.SetRequestingPractitioner("1", FhirHelper.AddInvalidFieldToResourceJson(FhirHelper.GetDefaultPractitioner().ToJson()));
        }

        [Given(@"I set the JWT Requesting Practitioner with missing SDS Id")]
        public void SetTheJwtRequestingPractitionerWithMissingSdsId()
        {
            var practitioner = FhirHelper.GetDefaultPractitioner();
            practitioner.Identifier.Clear();

            var identifier = new Identifier("http://IdentifierServer/RandomId", "ABC123");
            practitioner.Identifier.Add(identifier);

            _jwtHelper.SetRequestingPractitioner("1", practitioner.ToJson());
        }

        [Given(@"I set the JWT Requesting Practitioner with missing Identifier")]
        public void SetTheJwtRequestingPractitionerWithMissingIdentifier()
        {
            var practitioner = FhirHelper.GetDefaultPractitioner();
            practitioner.Identifier.Clear();

            _jwtHelper.SetRequestingPractitioner("1", practitioner.ToJson());
        }

        [Given(@"I set the JWT Requesting Practitioner with User Id not matching")]
        public void SetTheJwtRequestingPractitionerWithUserIdNotMatching()
        {
            _jwtHelper.SetRequestingPractitioner("2", FhirHelper.GetDefaultPractitioner().ToJson());
        }

        [Given(@"I set the JWT Requesting Practitioner with missing Name")]
        public void SetTheJwtRequestingPractitionerWithMissingName()
        {
            var practitioner = FhirHelper.GetDefaultPractitioner();
            practitioner.Name = null;

            _jwtHelper.SetRequestingPractitioner("1", practitioner.ToJson());
        }

        [Given(@"I set the JWT Requesting Practitioner with missing Practitioner Role")]
        public void SetTheJwtRequestingPractitionerWithMissingPractitionerRole()
        {
            var practitioner = FhirHelper.GetDefaultPractitioner();
            practitioner.PractitionerRole = null;

            _jwtHelper.SetRequestingPractitioner("1", practitioner.ToJson());
        }

        [Given(@"I set the JWT Requesting Practitioner Pratitioner Role with missing SDS Job Role")]
        public void SetTheJwtRequestingPractitionerPractitionerRoleWithMissingSdsJobRole()
        {
            var practitioner = FhirHelper.GetDefaultPractitioner();
            practitioner.PractitionerRole = FhirHelper.GetPractitionerRoleComponent("http://invalidValueSetServer.nhs.uk", "NonSDSJobRoleName");

            _jwtHelper.SetRequestingPractitioner("1", practitioner.ToJson());
        }

        [Given(@"I set the JWT Requesting Practitioner Resource Type as an invalid Resource Type")]
        public void SetTheJwtRequestingPractitionerResourceTypeAsAnInvalidResourceType()
        {
            _jwtHelper.SetRequestingPractitioner(_jwtHelper.RequestingPractitionerId, FhirHelper.ChangeResourceTypeString(_jwtHelper.RequestingPractitioner, FhirConst.Resources.kInvalidResourceType));
        }

        [Given(@"I set the JWT with missing Requesting System URL")]
        public void SetTheJwtWIthMissingRequestingSystemUrl()
        {
            _jwtHelper.RequestingSystemUrl = null;
        }

        [Given(@"I set the JWT with missing Requesting Practitioner Id")]
        public void SetTheJwtWIthMissingRequestingPractitionerId()
        {
            _jwtHelper.RequestingPractitionerId = null;
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

        [Given(@"I set the JWT with missing Requested Record")]
        public void SetTheJwtWithMissingRequestedRecord()
        {
            _jwtHelper.RequestedPatientNHSNumber = null;
            _jwtHelper.RequestedOrganizationODSCode = null;
        }

        [Given(@"I set the JWT with missing Requested Scope")]
        public void SetTheJwtWithMissingRequestedScope()
        {
            _jwtHelper.RequestedScope = null;
        }
        
        [Given(@"I set the JWT Requested Record to the NHS Number for ""(.*)""")]
        public void SetTheJwtRequestedRecordToTheNhsNumberFor(string patient)
        {
            _jwtHelper.RequestedPatientNHSNumber = GlobalContext.PatientNhsNumberMap[patient];
        }

        [Given(@"I set the JWT Requested Record to the NHS Number ""(.*)""")]
        public void SetTheJwtRequestedRecordToTheNhsNumber(string nhsNumber)
        {
            _jwtHelper.RequestedPatientNHSNumber = nhsNumber;
        }

        [Given(@"I set the JWT Requested Record to the NHS Number of the stored Patient")]
        public void SetTheJwtRequestedRecordToTheNhsNumberOfTheStoredPatient()
        {
            var patient = _httpContext.StoredPatient;

            var identifier = patient.Identifier.FirstOrDefault(x => x.System == FhirConst.IdentifierSystems.kNHSNumber);
       
            _jwtHelper.RequestedPatientNHSNumber = identifier?.Value;
        }

        [Given(@"I set the JWT requested record NHS number to the NHS number of patient stored against key ""([^""]*)""")]
        public void SetTheJwtRequestRecordNHSNumberToTheNNhsNumberOfPatientSotredAgainstKey(string storedPatientKey)
        {
            Patient storedPatient = (Patient)_httpContext.StoredFhirResources[storedPatientKey];
            foreach (Identifier identifier in storedPatient.Identifier)
            {
                if (identifier.System != null && string.Equals(identifier.System, FhirConst.IdentifierSystems.kNHSNumber))
                {
                    _jwtHelper.RequestedPatientNHSNumber = identifier.Value;
                    break;
                }
            }
            _headerHelper.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set the JWT requested record patient NHS number to ""(.*)""")]
        public void SetTheJwtRequestedRecordPatientNhsNumberTo(string nhsNumber)
        {
            _jwtHelper.RequestedPatientNHSNumber = nhsNumber;
            _headerHelper.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerToken());
        }

        [Given(@"I set the JWT requested record NHS number to config patient ""(.*)""")]
        public void SetTheJwtRequestedRecordNhsnumberToConfigPatient(string patient)
        {
            _jwtHelper.RequestedPatientNHSNumber = GlobalContext.PatientNhsNumberMap[patient];
            _headerHelper.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerToken());
        }
    }
}
