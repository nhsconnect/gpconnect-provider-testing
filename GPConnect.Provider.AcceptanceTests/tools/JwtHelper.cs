using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static Hl7.Fhir.Model.Practitioner;

namespace GPConnect.Provider.AcceptanceTests.tools
{
    public class JwtHelper
    {
        private static JwtHelper _jwtHelper;
        private DateTime _jwtCreationTime;
        private DateTime _jwtExpiryTime;
        private string _jwtReasonForRequest;
        private string _jwtAuthTokenURL;
        private string _jwtDevice;
        private string _jwtOrganization;
        private string _jwtPractitioner;
        private string _jwtPractitionerId;
        private string _jwtRequestedScope;
        private string _jwtRequestPatientNHSNumber;
        private string _jwtRequestOrganizationODSCode;
        private string _jwtRequestingSystemUrl;

        private JwtHelper() {
        }

        public static JwtHelper Instance => _jwtHelper ?? (_jwtHelper = new JwtHelper());

        public void setJwtDefaultValues() {
            _jwtCreationTime = DateTime.UtcNow;
            _jwtExpiryTime = _jwtCreationTime.AddMinutes(5);
            _jwtReasonForRequest = "directcare";
            _jwtAuthTokenURL = "https://authorize.fhir.nhs.net/token";
            _jwtDevice = FhirSerializer.SerializeToJson(getDefaultDevice());
            _jwtOrganization = FhirSerializer.SerializeToJson(getDefaultOrganization());
            _jwtPractitionerId = getDefaultPractitioner().Id;
            _jwtPractitioner = FhirSerializer.SerializeToJson(getDefaultPractitioner());
            _jwtRequestedScope = "organization/*.read";
            _jwtRequestPatientNHSNumber = null;
            _jwtRequestOrganizationODSCode = "OrgODSCode0001";
            _jwtRequestingSystemUrl = "https://ConsumerSystemURL";
    }

        public List<PractitionerRoleComponent> getPractitionerRoleComponent(string system, string value) {
            var practitionerRoleList = new List<PractitionerRoleComponent>();
            PractitionerRoleComponent practitionerRole = new PractitionerRoleComponent()
            {
                Role = new CodeableConcept(system, value)
            };
            practitionerRoleList.Add(practitionerRole);
            return practitionerRoleList;
        }

        public Practitioner getDefaultPractitioner() {
            return new Practitioner
            {
                Id = "1",
                Name = new HumanName()
                {
                    Prefix = new[] { "Mr" },
                    Given = new[] { "AssuranceTest" },
                    Family = new[] { "AssurancePractitioner" }
                },
                Identifier = {
                    new Identifier("http://fhir.nhs.net/sds-user-id", "GCASDS0001"),
                    new Identifier("LocalIdentifierSystem", "1")
                },
                PractitionerRole = getPractitionerRoleComponent("http://fhir.nhs.net/ValueSet/sds-job-role-name-1", "AssuranceJobRole")
            };
        }

        public Organization getDefaultOrganization()
        {
            return new Organization()
            {
                Id = "1",
                Name = "GP Connect Assurance",
                Identifier = {
                            new Identifier("http://fhir.nhs.net/Id/ods-organization-code", "GPCA0001")
                        }
            };
        }

        public Device getDefaultDevice()
        {
            return new Device()
            {
                Id = "1",
                Model = "v1",
                Version = "1.1",
                Identifier = {
                            new Identifier("GPConnectTestSystem", "Client")
                        },
                Type = new CodeableConcept("DeviceIdentifierSystem", "DeviceIdentifier")
            };
        }

        public string buildEncodedHeader() {
            return new JwtHeader().Base64UrlEncode();
        }

        public string buildEncodedPayload() {
            return buildPayload().Base64UrlEncode();
        }

        public JwtPayload buildPayload()
        {
            
            var claims = new List<System.Security.Claims.Claim>();
            
            if (_jwtRequestingSystemUrl != null) claims.Add(new System.Security.Claims.Claim("iss", _jwtRequestingSystemUrl, ClaimValueTypes.String));
            if (_jwtPractitionerId != null) claims.Add(new System.Security.Claims.Claim("sub", _jwtPractitionerId, ClaimValueTypes.String));
            if (_jwtAuthTokenURL != null) claims.Add(new System.Security.Claims.Claim("aud", _jwtAuthTokenURL, ClaimValueTypes.String));
            if (_jwtExpiryTime != DateTime.MinValue) claims.Add(new System.Security.Claims.Claim("exp", EpochTime.GetIntDate(_jwtExpiryTime).ToString(), ClaimValueTypes.Integer64));
            if (_jwtCreationTime != DateTime.MinValue) claims.Add(new System.Security.Claims.Claim("iat", EpochTime.GetIntDate(_jwtCreationTime).ToString(), ClaimValueTypes.Integer64));
            if (_jwtReasonForRequest != null) claims.Add(new System.Security.Claims.Claim("reason_for_request", _jwtReasonForRequest, ClaimValueTypes.String));
            if (_jwtDevice != null) claims.Add(new System.Security.Claims.Claim("requesting_device", _jwtDevice, JsonClaimValueTypes.Json));
            if (_jwtOrganization != null) claims.Add(new System.Security.Claims.Claim("requesting_organization", _jwtOrganization, JsonClaimValueTypes.Json));
            if (_jwtPractitioner != null) claims.Add(new System.Security.Claims.Claim("requesting_practitioner", _jwtPractitioner, JsonClaimValueTypes.Json));
            if (_jwtRequestedScope != null) claims.Add(new System.Security.Claims.Claim("requested_scope", _jwtRequestedScope, ClaimValueTypes.String));

            if (_jwtRequestPatientNHSNumber != null)
            {
                var subject_patient = new Patient
                {
                    Identifier = {
                        new Identifier("http://fhir.nhs.net/Id/nhs-number", _jwtRequestPatientNHSNumber)
                    }
                };
                claims.Add(new System.Security.Claims.Claim("requested_record", FhirSerializer.SerializeToJson(subject_patient), JsonClaimValueTypes.Json));
            } else if (_jwtRequestOrganizationODSCode != null)
            {
                var subject_organization = new Organization
                {
                    Identifier = {
                        new Identifier("http://fhir.nhs.net/Id/ods-organization-code", _jwtRequestOrganizationODSCode)
                    }
                };
                if (subject_organization != null) claims.Add(new System.Security.Claims.Claim("requested_record", FhirSerializer.SerializeToJson(subject_organization), JsonClaimValueTypes.Json));
            }
            
            // Serialize To Json and base64 encode
            return new JwtPayload(claims);
        }

        public string buildBearerTokenOrgResourceWithoutEncoding() {
            var token = new JwtHeader().SerializeToJson() + "." + buildPayload().SerializeToJson() + ".";
            Console.WriteLine("Token = " + token);
            return token;
        }

        public string buildBearerTokenOrgResource() {
            return buildEncodedHeader() + "." + buildEncodedPayload() + ".";
        }

        public string buildBearerTokenOrgResource(string orgODSCode)
        {
            _jwtRequestOrganizationODSCode = orgODSCode;
            return buildEncodedHeader() + "." + buildEncodedPayload() + ".";
        }

        public string buildBearerTokenPatientResource(string nhsNumber)
        {
            _jwtRequestPatientNHSNumber = nhsNumber;
            return buildEncodedHeader() + "." + buildEncodedPayload() + ".";
        }

        public void removeJWTRequestedRecord() {
            _jwtRequestOrganizationODSCode = null;
            _jwtRequestPatientNHSNumber = null;
        }
        
        public void setJWTExpiryTimeInSeconds(double seconds) {
            _jwtExpiryTime = _jwtCreationTime.AddSeconds(seconds);
        }

        public void removeJWTExpiryTime() {
            _jwtExpiryTime = DateTime.MinValue;
        }

        public void setJWTCreationTimeSeconds(double seconds)
        {
            _jwtCreationTime = DateTime.UtcNow.AddSeconds(seconds);
            _jwtExpiryTime = _jwtCreationTime.AddMinutes(5);
        }

        public void removeJWTCreationTime()
        {
            _jwtCreationTime = DateTime.MinValue;
        }

        public void setJWTReasonForRequest(string reasonForRequest) {
            _jwtReasonForRequest = reasonForRequest;
        }

        public void removeJWTReasonForRequest()
        {
            _jwtReasonForRequest = null;
        }

        public void setJWTAuthTokenURL(string autTokenUrl)
        {
            _jwtAuthTokenURL = autTokenUrl;
        }

        public void removeJWTTokenURL()
        {
            _jwtAuthTokenURL = null;
        }

        public void setJWTRequestingDevice(string deviceJson)
        {
            _jwtDevice = deviceJson;
        }

        public string getJWTRequestingDevice()
        {
            return _jwtDevice;
        }

        public void removeJWTRequestingDevice()
        {
            _jwtDevice = null;
        }

        public string getJWTRequestingOrganization()
        {
            return _jwtOrganization;
        }

        public void setJWTRequestingOrganization(string organizationJson)
        {
            _jwtOrganization = organizationJson;
        }

        public void removeJWTRequestingOrganization()
        {
            _jwtOrganization = null;
        }

        public string getJWTRequestingPractitioner()
        {
            return _jwtPractitioner;
        }

        public void setJWTRequestingPractitioner(string practitionerId, string practitionerJson)
        {
            _jwtPractitionerId = practitionerId;
            _jwtPractitioner = practitionerJson;
        }

        public void removeJWTRequestingPractitioner()
        {
            _jwtPractitioner = null;
        }

        public string getJWTRequestingPractitionerId()
        {
            return _jwtPractitionerId;
        }

        public void removeJWTRequestingPractitionerId()
        {
            _jwtPractitionerId = null;
        }

        public void setJWTRequestedScope(string requestedScope) {
            _jwtRequestedScope = requestedScope;
        }

        public void removeJWTRequestedScope()
        {
            _jwtRequestedScope = null;
        }

        public void setJWTRequestingSystemUrl(string requestingSystemUrl) {
            _jwtRequestingSystemUrl = requestingSystemUrl;
        }

        public void removeJWTRequestingSystemUrl() {
            _jwtRequestingSystemUrl = null;
        }

        // To create an invalid fhir resource this method is called with the default fhir resource and this 
        // adds an additional field to the resource which should make it invalid
        public string addInvalidFieldToResourceJson(string jsonResource) {
            var expandoConvert = new ExpandoObjectConverter();
            dynamic dynamicDeviceObj = JsonConvert.DeserializeObject<ExpandoObject>(jsonResource);
            dynamicDeviceObj.invalidField = "Assurance Testing";
            Console.WriteLine("Dynamic Object = " + JsonConvert.SerializeObject(dynamicDeviceObj));
            return JsonConvert.SerializeObject(dynamicDeviceObj);
        }

        public string changeResourceTypeString(string jsonResource, string newResourceType)
        {
            Console.WriteLine("Incomming JSon Object = " + jsonResource);
            var expandoConvert = new ExpandoObjectConverter();
            dynamic dynamicDeviceObj = JsonConvert.DeserializeObject<ExpandoObject>(jsonResource);
            dynamicDeviceObj.resourceType = newResourceType;
            Console.WriteLine("COnverted Type Json Object = " + JsonConvert.SerializeObject(dynamicDeviceObj));
            return JsonConvert.SerializeObject(dynamicDeviceObj);
        }

    }
}
