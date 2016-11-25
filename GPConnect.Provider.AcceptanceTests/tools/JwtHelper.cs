﻿using Hl7.Fhir.Model;
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

        public string buildEncodedPayload(string nhsNumber) {
            return buildPayload(nhsNumber).Base64UrlEncode();
        }

        public JwtPayload buildPayload(string nhsNumber)
        {
            var subject_patient = new Patient
            {
                Identifier = {
                    new Identifier("http://fhir.nhs.net/Id/nhs-number",nhsNumber)
                }
            };

            var subject_organization = new Organization
            {
                Identifier = {
                    new Identifier("http://fhir.nhs.net/Id/ods-organization-code","[OrganizationODSCode]")
                }
            };

            var requesting_system_url = "https://[ConsumerSystemURL]";
            
            var claims = new List<System.Security.Claims.Claim> {
                new System.Security.Claims.Claim("iss", requesting_system_url, ClaimValueTypes.String),
                new System.Security.Claims.Claim("sub", _jwtPractitionerId, ClaimValueTypes.String),
                new System.Security.Claims.Claim("aud", _jwtAuthTokenURL, ClaimValueTypes.String),
                new System.Security.Claims.Claim("exp", EpochTime.GetIntDate(_jwtExpiryTime).ToString(), ClaimValueTypes.Integer64),
                new System.Security.Claims.Claim("iat", EpochTime.GetIntDate(_jwtCreationTime).ToString(), ClaimValueTypes.Integer64),
                new System.Security.Claims.Claim("reason_for_request", _jwtReasonForRequest, ClaimValueTypes.String),
                new System.Security.Claims.Claim("requesting_device", _jwtDevice, JsonClaimValueTypes.Json),
                new System.Security.Claims.Claim("requesting_organization", _jwtOrganization, JsonClaimValueTypes.Json),
                new System.Security.Claims.Claim("requesting_practitioner", _jwtPractitioner, JsonClaimValueTypes.Json),
                new System.Security.Claims.Claim("requested_scope", _jwtRequestedScope, ClaimValueTypes.String)
            };

            if (nhsNumber != null)
            {
                claims.Add(new System.Security.Claims.Claim("requested_record", FhirSerializer.SerializeToJson(subject_patient), JsonClaimValueTypes.Json));
            }
            else {
                claims.Add(new System.Security.Claims.Claim("requested_record", FhirSerializer.SerializeToJson(subject_organization), JsonClaimValueTypes.Json));
            }

            // Serialize To Json and base64 encode
            return new JwtPayload(claims);
        }

        public string buildBearerTokenOrgResourceWithoutEncoding() {
            var token = new JwtHeader().SerializeToJson() + "." + buildPayload(null).SerializeToJson() + ".";
            Console.WriteLine("Token = " + token);
            return token;
        }

        public string buildBearerTokenOrgResource() {
            return buildEncodedHeader() + "." + buildEncodedPayload(null) + ".";
        }

        public string buildBearerTokenPatientResource(string nhsNumber)
        {
            return buildEncodedHeader() + "." + buildEncodedPayload(nhsNumber) + ".";
        }

        public void setJWTExpiryTimeInSeconds(double seconds) {
            _jwtExpiryTime = _jwtCreationTime.AddSeconds(seconds);
        }

        public void setJWTCreationTimeSeconds(double seconds)
        {
            _jwtCreationTime = DateTime.UtcNow.AddSeconds(seconds);
            _jwtExpiryTime = _jwtCreationTime.AddMinutes(5);
        }

        public void setJWTReasonForRequest(string reasonForRequest) {
            _jwtReasonForRequest = reasonForRequest;
        }

        public void setJWTAuthTokenURL(string autTokenUrl)
        {
            _jwtAuthTokenURL = autTokenUrl;
        }

        public void setJWTRequestingDevice(string deviceJson)
        {
            _jwtDevice = deviceJson;
        }

        public void setJWTRequestingOrganization(string organizationJson)
        {
            _jwtOrganization = organizationJson;
        }

        public void setJWTRequestingPractitioner(string practitionerId, string practitionerJson)
        {
            _jwtPractitionerId = practitionerId;
            _jwtPractitioner = practitionerJson;
        }

        public void setJWTRequestedScope(string requestedScope) {
            _jwtRequestedScope = requestedScope;
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
    }
}