using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GPConnect.Provider.AcceptanceTests.tools
{
    public class JwtHelper
    {
        private static JwtHelper _jwtHelper;
        private DateTime _jwtCreationTime;
        private DateTime _jwtExpiryTime;

        private JwtHelper() {
        }

        public static JwtHelper Instance => _jwtHelper ?? (_jwtHelper = new JwtHelper());

        public void setJwtDefaultValues() {
            _jwtCreationTime = DateTime.UtcNow;
            _jwtExpiryTime = _jwtCreationTime.AddMinutes(5);
        }

        public string buildEncodedHeader() {
            return new JwtHeader().Base64UrlEncode();
        }

        public string buildEncodedPayload(string nhsNumber) {
            return buildPayload(nhsNumber).Base64UrlEncode();
        }

        public JwtPayload buildPayload(string nhsNumber)
        {
            var requesting_device = new Device
            {
                Id = "[DeviceID]",
                Model = "[SoftwareName]",
                Version = "[SoftwareVersion]",
                Identifier = {
                    new Identifier("[DeviceSystem]", "[DeviceID]")
                }
            };

            var requesting_organization = new Organization
            {
                Id = "[OrganizationID]",
                Name = "Requesting Organisation Name",
                Identifier = {
                    new Identifier("http://fhir.nhs.net/Id/ods-organization-code", "[ODSCode]")
                }
            };

            var requesting_practitioner = new Practitioner
            {
                Id = "1",
                Name = new HumanName()
                {
                    Prefix = new[] { "[Prefix]" },
                    Given = new[] { "[Given]" },
                    Family = new[] { "[Family]" }
                },
                Identifier = {
                    new Identifier("http://fhir.nhs.net/sds-user-id", "[SDSUserID]"),
                    new Identifier("[UserSystem]", "[UserID]")
                }
            };

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
            var requesting_system_token_url = "https://authorize.fhir.nhs.net/token";
            
            var claims = new List<System.Security.Claims.Claim> {
                new System.Security.Claims.Claim("iss", requesting_system_url, ClaimValueTypes.String),
                new System.Security.Claims.Claim("sub", requesting_practitioner.Id, ClaimValueTypes.String),
                new System.Security.Claims.Claim("aud", requesting_system_token_url, ClaimValueTypes.String),
                new System.Security.Claims.Claim("exp", EpochTime.GetIntDate(_jwtExpiryTime).ToString(), ClaimValueTypes.Integer64),
                new System.Security.Claims.Claim("iat", EpochTime.GetIntDate(_jwtCreationTime).ToString(), ClaimValueTypes.Integer64),
                new System.Security.Claims.Claim("reason_for_request", "directcare", ClaimValueTypes.String),
                new System.Security.Claims.Claim("requesting_device", FhirSerializer.SerializeToJson(requesting_device), JsonClaimValueTypes.Json),
                new System.Security.Claims.Claim("requesting_organization", FhirSerializer.SerializeToJson(requesting_organization), JsonClaimValueTypes.Json),
                new System.Security.Claims.Claim("requesting_practitioner", FhirSerializer.SerializeToJson(requesting_practitioner), JsonClaimValueTypes.Json)
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
    }
}
