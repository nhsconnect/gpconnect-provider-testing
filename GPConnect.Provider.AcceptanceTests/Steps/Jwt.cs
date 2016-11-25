using TechTalk.SpecFlow;
using GPConnect.Provider.AcceptanceTests.tools;
using System.Net;
using System;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using Shouldly;
using Hl7.Fhir.Model;
using Newtonsoft.Json.Linq;
using Hl7.Fhir.Serialization;

namespace GPConnect.Provider.AcceptanceTests.Steps
{

    [Binding]
    public class Jwt : TechTalk.SpecFlow.Steps
    {
        private readonly ScenarioContext _scenarioContext;
        private HeaderController _headerController;
        private JwtHelper _jwtHelper;

        public Jwt(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _headerController = HeaderController.Instance;
            _jwtHelper = JwtHelper.Instance;
        }


        // JWT configuration steps

        [Given(@"I set the default JWT")]
        public void ISetTheDefaultJWT()
        {
            _jwtHelper.setJwtDefaultValues();
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenOrgResource());
        }

        [Given(@"I set the default JWT without base64 encoding")]
        public void ISetTheJWTWithoutBase64Encoding()
        {
            _jwtHelper.setJwtDefaultValues();
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenOrgResourceWithoutEncoding());
        }

        [Given(@"I set the JWT expiry time to ""(.*)"" seconds after creation time")]
        public void ISetTheJWTExpiryTimeToSecondsAfterCreationTime(double expirySeconds)
        {
            _jwtHelper.setJWTExpiryTimeInSeconds(expirySeconds);
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenOrgResource());
        }
        
        [Given(@"I set the JWT creation time to ""(.*)"" seconds after the current time")]
        public void ISetTheJWTCreationTimeToSecondsAfterTheCurrentTime(double secondsInFuture)
        {
            _jwtHelper.setJWTCreationTimeSeconds(secondsInFuture);
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenOrgResource());
        }
        
        [Given(@"I set the JWT reason for request to ""(.*)""")]
        public void ISetTheJWTReasonForRequestTo(string reasonForRequest)
        {
            _jwtHelper.setJWTReasonForRequest(reasonForRequest);
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenOrgResource());
        }

        [Given(@"I set the JWT authorization server token URL to ""(.*)""")]
        public void ISetTheJWTAuthorizationServerTokenTo(string autTokenUrl)
        {
            _jwtHelper.setJWTAuthTokenURL(autTokenUrl);
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenOrgResource());
        }

        public class InvalidDeviceResource : Device
        {
            public string invalidFieldInObject { get; set; }
        }

        [Given(@"I set an invalid JWT requesting device resource")]
        public void ISetAnInvalidJWTRequestingDeviceResource()
        {
            dynamic jsonDeviceObject = new JObject();
            jsonDeviceObject.resourceType = "Device";
            jsonDeviceObject.id = "1";
            jsonDeviceObject.invalidFhirResourceField = "ValidationTestElement";
            _jwtHelper.setJWTRequestingDevice(jsonDeviceObject.ToString());
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenOrgResource());
        }

        [Given(@"I set an invalid JWT requesting organization resource")]
        public void ISetAnInvalidJWTRequestingOrganizationResource()
        {
            dynamic jsonOrganizationObject = new JObject();
            jsonOrganizationObject.resourceType = "Organization";
            jsonOrganizationObject.id = "1";
            jsonOrganizationObject.invalidFhirResourceField = "ValidationTestElement";
            _jwtHelper.setJWTRequestingOrganization(jsonOrganizationObject.ToString());
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenOrgResource());
        }

        [Given(@"I set JWT requesting organization resource without ODS Code")]
        public void ISetJWTRequestingOrganizaitonResourceWithoutODSCode()
        {
            _jwtHelper.setJWTRequestingOrganization(FhirSerializer.SerializeToJson(
                    new Organization()
                    {
                        Id = "1",
                        Name = "GP Connect Assurance",
                        Identifier = {
                            new Identifier("http://fhir.nhs.net/Id/someOtherCodingSystem", "NoOdsCode")
                        }
                    }
                ));
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenOrgResource());
        }

        [Given(@"I set an invalid JWT requesting practitioner resource")]
        public void ISetAnInvalidJWTRequestingPractitionerResource()
        {
            dynamic jsonPractitionerObject = new JObject();
            jsonPractitionerObject.resourceType = "Practitioner";
            jsonPractitionerObject.id = "1";
            jsonPractitionerObject.invalidFhirResourceField = "ValidationTestElement";
            _jwtHelper.setJWTRequestingPractitioner("1", jsonPractitionerObject.ToString());
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenOrgResource());
        }

        [Given(@"I set a JWT requesting practitioner without SDS id")]
        public void ISetAJWTRequestingPractitionerWithoutSDSId()
        {
            _jwtHelper.setJWTRequestingPractitioner("1", FhirSerializer.SerializeToJson(
                new Practitioner()
                {
                    Id = "1",
                    Name = new HumanName()
                    {
                        Prefix = new[] { "Mr" },
                        Given = new[] { "AssuranceTest" },
                        Family = new[] { "AssurancePractitioner" }
                    },
                    Identifier = {
                        new Identifier("http://IdentifierServer/RandomId", "ABC123"),
                    }
                }
                ));
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenOrgResource());
        }

        [Given(@"I set a JWT requesting practitioner with miss matched user id")]
        public void ISetAJWTRequestingPractitionerWithMissMatchedUserId()
        {
            _jwtHelper.setJWTRequestingPractitioner("2", FhirSerializer.SerializeToJson(
                new Practitioner()
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
                    }
                }
                ));
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenOrgResource());
        }

        //I set a JWT requesting practitioner with miss name element
        [Given(@"I set a JWT requesting practitioner with missing name element")]
        public void ISetAJWTRequestingPractitionerWithMissingNameElement()
        {
            _jwtHelper.setJWTRequestingPractitioner("1", FhirSerializer.SerializeToJson(
                new Practitioner()
                {
                    Id = "1",
                    Identifier = {
                        new Identifier("http://fhir.nhs.net/sds-user-id", "GCASDS0001"),
                    }
                }
                ));
            _headerController.removeHeader("Authorization");
            _headerController.addHeader("Authorization", "Bearer " + _jwtHelper.buildBearerTokenOrgResource());
        }


    }
}
