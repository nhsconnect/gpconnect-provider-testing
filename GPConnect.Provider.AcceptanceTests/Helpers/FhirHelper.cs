using System.Collections.Generic;
using System.Dynamic;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Model;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using GPConnect.Provider.AcceptanceTests.Constants;

namespace GPConnect.Provider.AcceptanceTests.Helpers
{
    public static class FhirHelper
    {
        public static Identifier GetNHSNumberIdentifier(string nhsNumber)
        {
            return GetIdentifier(FhirConst.IdentifierSystems.kNHSNumber, nhsNumber);
        }

        public static Identifier GetIdentifier(string system, string nhsNumber)
        {
            return new Identifier(system, nhsNumber);
        }

        public static CodeableConcept GetRecordSectionCodeableConcept(string recordSectionCode)
        {
            return GetRecordSectionCodeableConcept(FhirConst.ValueSetSystems.kRecordSectionValueSet, recordSectionCode);
        }

        public static CodeableConcept GetRecordSectionCodeableConcept(string system, string recordSectionCode)
        {
            return new CodeableConcept(system, recordSectionCode);
        }

        public static Identifier GetODSCodeIdentifier(string odsCode)
        {
            return new Identifier(FhirConst.IdentifierSystems.kOdsOrgzCode, odsCode);
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

        public static Organization GetOrganization(string id, string odsCode)
        {
            return new Organization
            {
                Id = id,
                Identifier = new List<Identifier>
                { 
                    GetODSCodeIdentifier(odsCode)
                }
            };
        }

        public static Organization GetDefaultOrganization(string odsCode = "GPCA0001")
        {
            return GetOrganization("1", odsCode);
          
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
                    new Identifier(FhirConst.IdentifierSystems.kPracSDSUserId, "GCASDS0001"),
                    new Identifier("LocalIdentifierSystem", "1")
                },
                PractitionerRole = GetPractitionerRoleComponent(FhirConst.ValueSetSystems.kSDSJobRoleName, "AssuranceJobRole")
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

        public static string ChangeParameterResourceTypeString(string jsonResource, string newResourceType)
        {
            Log.WriteLine("Incomming Json Object = " + jsonResource);
            dynamic dynamicDeviceObj = JsonConvert.DeserializeObject<ExpandoObject>(jsonResource);
            foreach (var parameter in dynamicDeviceObj.parameter) {
                parameter.resource.resourceType = newResourceType;
            }
            Log.WriteLine("Converted Type Json Object = " + JsonConvert.SerializeObject(dynamicDeviceObj));
            return JsonConvert.SerializeObject(dynamicDeviceObj);
        }

        public static string AddFieldToParameterResource(string jsonResource, string newResourceType)
        {
            Log.WriteLine("Incomming Json Object = " + jsonResource);
            dynamic dynamicDeviceObj = JsonConvert.DeserializeObject<ExpandoObject>(jsonResource);
            foreach (var parameter in dynamicDeviceObj.parameter)
            {
                parameter.resource.invalidField = "Assurance Testing Invalid Field";
            }
            Log.WriteLine("Converted Type Json Object = " + JsonConvert.SerializeObject(dynamicDeviceObj));
            return JsonConvert.SerializeObject(dynamicDeviceObj);
        }

        public static Period GetDefaultTimePeriodForGetCareRecord() {
            return new Period(new FhirDateTime(DateTime.Now.AddYears(-2)), new FhirDateTime(DateTime.Now));
        }

        public static Period GetTimePeriod(string startDate, string endDate) {
            return new Period(new FhirDateTime(startDate), new FhirDateTime(endDate));
        }

        public static bool isValidNHSNumber(string NHSNumber) {

            NHSNumber = NHSNumber.Trim();
            
            if (NHSNumber.Length != 10 || !Regex.Match(NHSNumber, "(\\d+)").Success)
            {
                return false;
            }
            else {
                
                string checkDigit = NHSNumber.Substring(NHSNumber.Length - 1, 1);
                int checkNumber = Convert.ToInt16(checkDigit);

                int[] multiplers;
                multiplers = new int[9];
                multiplers[0] = 10;
                multiplers[1] = 9;
                multiplers[2] = 8;
                multiplers[3] = 7;
                multiplers[4] = 6;
                multiplers[5] = 5;
                multiplers[6] = 4;
                multiplers[7] = 3;
                multiplers[8] = 2;
                
                int currentNumber = 0;
                int currentSum = 0;

                for (int i = 0; i < 9; i++)
                {
                    currentNumber = Convert.ToInt16(NHSNumber.Substring(i, 1));
                    currentSum = currentSum + (currentNumber * multiplers[i]);
                }

                int remainder = currentSum % 11;
                int total = 11 - remainder;

                if (total.Equals(11))
                {
                    total = 0;
                }

                return total.Equals(checkNumber);
            }
            
        }
    }
}
