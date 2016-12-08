using System.Collections.Generic;
using System.Dynamic;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Model;
using Newtonsoft.Json;

namespace GPConnect.Provider.AcceptanceTests.Helpers
{
    static public class FhirHelper
    {
        public static Identifier GetNHSNumberIdentifier(string nhsNumber)
        {
            return new Identifier("http://fhir.nhs.net/Id/nhs-number", nhsNumber);
        }

        public static CodeableConcept GetRecordSectionCodeableConcept(string recordSectionCode)
        {
            return new CodeableConcept("http://fhir.nhs.net/ValueSet/gpconnect-record-section-1-0", recordSectionCode);
        }

        public static Identifier GetODSCodeIdentifier(string odsCode)
        {
            return new Identifier("http://fhir.nhs.net/Id/ods-organization-code", odsCode);
        }

        public static Patient GetDefaultPatient(string nhsNumber = "123456")
        {
            return new Patient
            {
                Identifier = {
                        GetNHSNumberIdentifier(nhsNumber)
                    }
            };
        }

        public static Organization GetDefaultOrganization(string odsCode = "GPCA0001")
        {
            return new Organization
            {
                Id = "1",
                Name = "GP Connect Assurance",
                Identifier = {
                        GetODSCodeIdentifier(odsCode)
                    }
            };
        }

        public static List<Practitioner.PractitionerRoleComponent> GetPractitionerRoleComponent(string system, string value)
        {
            var practitionerRoleList = new List<Practitioner.PractitionerRoleComponent>();
            var practitionerRole = new Practitioner.PractitionerRoleComponent()
            {
                Role = new CodeableConcept(system, value)
            };
            practitionerRoleList.Add(practitionerRole);
            return practitionerRoleList;
        }

        public static Practitioner GetDefaultPractitioner()
        {
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
                PractitionerRole = GetPractitionerRoleComponent("http://fhir.nhs.net/ValueSet/sds-job-role-name-1", "AssuranceJobRole")
            };
        }

        public static Device GetDefaultDevice()
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

        // To create an invalid fhir resource this method is called with the default fhir resource and this 
        // adds an additional field to the resource which should make it invalid
        public static string AddInvalidFieldToResourceJson(string jsonResource)
        {
            dynamic dynamicDeviceObj = JsonConvert.DeserializeObject<ExpandoObject>(jsonResource);
            dynamicDeviceObj.invalidField = "Assurance Testing";
            Log.WriteLine("Dynamic Object = " + JsonConvert.SerializeObject(dynamicDeviceObj));
            return JsonConvert.SerializeObject(dynamicDeviceObj);
        }

        public static string ChangeResourceTypeString(string jsonResource, string newResourceType)
        {
            Log.WriteLine("Incomming Json Object = " + jsonResource);
            dynamic dynamicDeviceObj = JsonConvert.DeserializeObject<ExpandoObject>(jsonResource);
            dynamicDeviceObj.resourceType = newResourceType;
            Log.WriteLine("Converted Type Json Object = " + JsonConvert.SerializeObject(dynamicDeviceObj));
            return JsonConvert.SerializeObject(dynamicDeviceObj);
        }
    }
}
