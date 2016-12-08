using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace GPConnect.Provider.AcceptanceTests.Helpers
{
    public class JwtHelper
    {
        private const string Bearer = "Bearer ";
        private const int MaxExpiryTimeInMinutes = 5;

        private static JwtHelper jwtHelper;

        public DateTime? CreationTime { get; set; }
        public DateTime? ExpiryTime { get; set; }
        public string ReasonForRequest { get; set; }
        public string AuthTokenURL { get; set; }
        public string RequestingDevice { get; set; }
        public string RequestingOrganization { get; set; }
        public string RequestingPractitioner { get; set; }
        public string RequestingPractitionerId { get; set; }
        public string RequestedScope { get; set; }
        public string RequestedPatientNHSNumber { get; set; }
        public string RequestedOrganizationODSCode { get; set; }
        public string RequestingSystemUrl { get; set; }

        private JwtHelper()
        {
        }

        public static JwtHelper Instance => jwtHelper ?? (jwtHelper = new JwtHelper());

        public void SetJwtDefaultValues()
        {
            CreationTime = DateTime.UtcNow;
            ExpiryTime = CreationTime.Value.AddMinutes(MaxExpiryTimeInMinutes);
            ReasonForRequest = JwtConst.Values.DirectCare;
            AuthTokenURL = JwtConst.Values.AuthTokenURL;
            RequestingDevice = FhirHelper.GetDefaultDevice().ToJson();
            RequestingOrganization = FhirHelper.GetDefaultOrganization().ToJson();
            RequestingPractitionerId = FhirHelper.GetDefaultPractitioner().Id;
            RequestingPractitioner = FhirHelper.GetDefaultPractitioner().ToJson();
            RequestedScope = JwtConst.Scope.OrganizationRead;
            // TODO Check We're Using The Correct Scope For Metadata vs. GetCareRecord
            RequestedPatientNHSNumber = null;
            // TODO Move Dummy Data Out Into App.Config Or Somewhere Else
            RequestedOrganizationODSCode = "OrgODSCode0001";
            RequestingSystemUrl = "https://ConsumerSystemURL";
        }


        private static string BuildEncodedHeader()
        {
            return new JwtHeader().Base64UrlEncode();
        }

        private string BuildEncodedPayload()
        {
            return BuildPayload().Base64UrlEncode();
        }

        private JwtPayload BuildPayload()
        {

            var claims = new List<Claim>();

            if (RequestingSystemUrl != null)
                claims.Add(new Claim(JwtConst.Claims.RequestingSystemUrl, RequestingSystemUrl, ClaimValueTypes.String));
            if (RequestingPractitionerId != null)
                claims.Add(new Claim(JwtConst.Claims.PractitionerId, RequestingPractitionerId, ClaimValueTypes.String));
            if (AuthTokenURL != null)
                claims.Add(new Claim(JwtConst.Claims.AuthTokenURL, AuthTokenURL, ClaimValueTypes.String));
            if (ExpiryTime != null)
                claims.Add(new Claim(JwtConst.Claims.ExpiryTime, EpochTime.GetIntDate(ExpiryTime.Value).ToString(), ClaimValueTypes.Integer64));
            if (CreationTime != null)
                claims.Add(new Claim(JwtConst.Claims.CreationTime, EpochTime.GetIntDate(CreationTime.Value).ToString(), ClaimValueTypes.Integer64));
            if (ReasonForRequest != null)
                claims.Add(new Claim(JwtConst.Claims.ReasonForRequest, ReasonForRequest, ClaimValueTypes.String));
            if (RequestingDevice != null)
                claims.Add(new Claim(JwtConst.Claims.RequestingDevice, RequestingDevice, JsonClaimValueTypes.Json));
            if (RequestingOrganization != null)
                claims.Add(new Claim(JwtConst.Claims.RequestingOrganization, RequestingOrganization, JsonClaimValueTypes.Json));
            if (RequestingPractitioner != null)
                claims.Add(new Claim(JwtConst.Claims.RequestingPractitioner, RequestingPractitioner, JsonClaimValueTypes.Json));
            if (RequestedScope != null)
                claims.Add(new Claim(JwtConst.Claims.RequestedScope, RequestedScope, ClaimValueTypes.String));

            if (RequestedPatientNHSNumber != null)
            {
                claims.Add(new Claim(JwtConst.Claims.RequestedRecord, FhirHelper.GetDefaultPatient(RequestedPatientNHSNumber).ToJson(), JsonClaimValueTypes.Json));
            }
            else if (RequestedOrganizationODSCode != null)
            {
                claims.Add(new Claim(JwtConst.Claims.RequestedRecord, FhirHelper.GetDefaultOrganization(RequestedOrganizationODSCode).ToJson(), JsonClaimValueTypes.Json));
            }

            return new JwtPayload(claims);
        }

        public string GetBearerTokenWithoutEncoding()
        {
            return Bearer + BuildBearerTokenOrgResourceWithoutEncoding();
        }

        private string BuildBearerTokenOrgResourceWithoutEncoding()
        {
            return new JwtHeader().SerializeToJson() + "." + BuildPayload().SerializeToJson() + ".";
        }

        public string GetBearerToken()
        {
            return Bearer + BuildBearerTokenOrgResource();
        }

        private string BuildBearerTokenOrgResource()
        {
            return BuildEncodedHeader() + "." + BuildEncodedPayload() + ".";
        }

        public void SetExpiryTimeInSeconds(double seconds)
        {
            Debug.Assert(CreationTime != null, "_jwtCreationTime != null");
            ExpiryTime = CreationTime.Value.AddSeconds(seconds);
        }

        public void SetCreationTimeSeconds(double seconds)
        {
            CreationTime = DateTime.UtcNow.AddSeconds(seconds);
            ExpiryTime = CreationTime.Value.AddMinutes(MaxExpiryTimeInMinutes);
        }

        public void SetRequestingPractitioner(string practitionerId, string practitionerJson)
        {
            // TODO Make The RequestingPractitionerId Use The Business Identifier And Not The Logical Identifier 
            RequestingPractitionerId = practitionerId;
            RequestingPractitioner = practitionerJson;
        }
    }
}
