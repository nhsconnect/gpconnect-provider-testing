using System.Collections.Generic;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using Hl7.Fhir.Model;
using Shouldly;
using TechTalk.SpecFlow;
using static Hl7.Fhir.Model.Bundle;
using System.Net.Http;
using Hl7.Fhir.Serialization;
using RestSharp;
using GPConnect.Provider.AcceptanceTests.Data;
using GPConnect.Provider.AcceptanceTests.Constants;
using NUnit.Framework;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public class RegisterPatientSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private readonly HttpSteps HttpSteps;
        private readonly HttpContext HttpContext;
 
        // Headers Helper
        public HttpHeaderHelper Headers { get; }

        public RegisterPatientSteps(HttpHeaderHelper headerHelper, FhirContext fhirContext, HttpSteps httpSteps, HttpContext httpContext)
        {
            // Helpers
            FhirContext = fhirContext;
            Headers = headerHelper;
            HttpSteps = httpSteps;
            HttpContext = httpContext;
        }
        
        [Given(@"I find the next patient to register and store the Patient Resource against key ""([^""]*)""")]
        public void GivenIFindTheNextPatientToRegisterAndStoreThePatientResourceAgainstKey(string patientResourceKey)
        {
            Patient returnPatient = null;

            List<RegisterPatient> registerPatients = GlobalContext.RegisterPatientsData;
            for (int index = 0; index < registerPatients.Count; index++) {
                RegisterPatient registerPatient = registerPatients[index];
                // Search for patient
                Given($@"I perform a patient search for patient with NHSNumber ""{registerPatient.SPINE_NHS_NUMBER}"" and store the response bundle against key ""registerPatient""");
                // See if number of returned patients is > zero, ie patient already registered, else use patient
                Bundle patientSearchBiundle = (Bundle)HttpContext.StoredFhirResources["registerPatient"];
                if (patientSearchBiundle.Entry.Count == 0) {
                    // Patient not registered yet
                    returnPatient = new Patient();
                    returnPatient.Identifier.Add(new Identifier(FhirConst.IdentifierSystems.kNHSNumber, registerPatient.SPINE_NHS_NUMBER));
                    HumanName name = new HumanName();
                    name.FamilyElement.Add(new FhirString(registerPatient.NAME_FAMILY));
                    name.GivenElement.Add(new FhirString(registerPatient.NAME_GIVEN));
                    returnPatient.Name = new List<HumanName>();
                    returnPatient.Name.Add(name);
                    switch (registerPatient.GENDER) {
                        case "MALE":
                            returnPatient.Gender = AdministrativeGender.Male;
                            break;
                        case "FEMALE":
                            returnPatient.Gender = AdministrativeGender.Female;
                            break;
                        case "OTHER":
                            returnPatient.Gender = AdministrativeGender.Other;
                            break;
                        case "UNKNOWN":
                            returnPatient.Gender = AdministrativeGender.Unknown;
                            break;
                    }
                    returnPatient.BirthDateElement = new Date(registerPatient.DOB);
                    break; // Stop looking for the next patient
                }
            }
            if (returnPatient != null)
            {
                // Store the created patient
                if (HttpContext.StoredFhirResources.ContainsKey(patientResourceKey)) HttpContext.StoredFhirResources.Remove(patientResourceKey);
                HttpContext.StoredFhirResources.Add(patientResourceKey, returnPatient);
            }
            else {
                Assert.Fail("No patients left to register patient with");
            }
        }

        [Given(@"I build the register patient from stored patient resource against key ""(.*)""")]
        public void GivenIBuildTheRegisterPatientFromStoredPatientResourceAgainstKey(string storedPatientKey)
        {
            HttpContext.registerPatient.Add(storedPatientKey, (Patient)HttpContext.StoredFhirResources[storedPatientKey]);
        }

        [Given(@"I register patient ""(.*)"" with first name ""(.*)"" and family name ""(.*)"" with NHS number ""(.*)"" and birth date ""(.*)""")]
        public void GivenIRegisterPatient(string patientSavedName ,string firstName, string familyName, string nhsNumber,string birthDate)
        {
            Patient patient = new Patient();
            Identifier id = new Identifier();

            id.Value = nhsNumber;
            id.System = "http://fhir.nhs.net/Id/nhs-number";
            patient.Identifier.Add(id);

            bool active = true;
            patient.Active = active;
        
            AdministrativeGender code = new AdministrativeGender();
            string date = birthDate;

            patient.Gender = code;
            patient.BirthDate = date;

            HumanName name = new HumanName();

            string familyString = familyName;
            string givenString = firstName;

            List<string> familyList = new List<string>();
            List<string> givenList = new List<string>();

            familyList.Add(familyString);
            givenList.Add(givenString);

            name.Family = familyList;
            name.Given = givenList;

            patient.Name.Add(name);

            HttpContext.registerPatient.Add(patientSavedName, patient);
    
        }

        [Given(@"I do not set ""(.*)"" and register patient ""(.*)"" with first name ""(.*)"" and family name ""(.*)"" with NHS number ""(.*)"" and birth date ""(.*)""")]
        public void GivenIRegisterPatientSkipStep(string doNotSet,string patientSavedName, string firstName, string familyName, string nhsNumber, string birthDate)
        {
            Patient patient = new Patient();

            if (doNotSet != "Identifier")
            {
                Identifier id = new Identifier();

                id.Value = nhsNumber;
                id.System = "http://fhir.nhs.net/Id/nhs-number";
                patient.Identifier.Add(id);
            }

            if (doNotSet != "active")
            {
                bool active = true;
                patient.Active = active;
            }


            if (doNotSet != "gender")
            {
                AdministrativeGender code = new AdministrativeGender();
               patient.Gender = code;
            }

            if (doNotSet != "birthDate")
            {
                string date = birthDate;
                patient.BirthDate = date;
            }

            if (doNotSet != "name")
            {
                HumanName name = new HumanName();

                string familyString = familyName;
                string givenString = firstName;

                List<string> familyList = new List<string>();
                List<string> givenList = new List<string>();

                familyList.Add(familyString);
                givenList.Add(givenString);

                name.Family = familyList;
                name.Given = givenList;

                patient.Name.Add(name);
            }



            HttpContext.registerPatient.Add(patientSavedName, patient);

        }


        [Given(@"I set the identifier from ""(.*)"" to null")]
        public void GivenIRemoveTheIdentifier(string patientSavedName)
        {
            Patient patient = HttpContext.registerPatient[patientSavedName];

            patient.Identifier = null;
            HttpContext.registerPatient.Remove(patientSavedName);
            HttpContext.registerPatient.Add(patientSavedName, patient);
        }

        [Given(@"I set the active element from ""(.*)"" to null")]
        public void GivenIRemoveTheActiveElement(string patientSavedName)
        {
            Patient patient = HttpContext.registerPatient[patientSavedName];

            patient.Active = null;
            HttpContext.registerPatient.Remove(patientSavedName);
            HttpContext.registerPatient.Add(patientSavedName, patient);
        }

        [Given(@"I set the name element from ""(.*)"" to null")]
        public void GivenIRemoveTheNameElement(string patientSavedName)
        {
            Patient patient = HttpContext.registerPatient[patientSavedName];

            patient.Name = null;
            HttpContext.registerPatient.Remove(patientSavedName);
            HttpContext.registerPatient.Add(patientSavedName, patient);
        }

        [Given(@"I set the gender element from ""(.*)"" to null")]
        public void GivenIRemoveTheGenderElement(string patientSavedName)
        {
            Patient patient = HttpContext.registerPatient[patientSavedName];

            patient.Gender = null;
            HttpContext.registerPatient.Remove(patientSavedName);
            HttpContext.registerPatient.Add(patientSavedName, patient);
        }
        
        [Given(@"I add the registration period with start date ""(.*)"" to ""(.*)""")]
        public void GivenIAddRegistrationPeriodToPatient(string regStartDate, string patientSavedName)
        {
            Patient patient = HttpContext.registerPatient[patientSavedName];

            Extension registrationPeriod = new Extension();

            registrationPeriod.Url = "http://fhir.nhs.net/StructureDefinition/extension-registration-period-1";

            Period period = new Period();
            period.Start = regStartDate;
            period.End = "";
            registrationPeriod.Value = period;

            patient.Extension.Add(registrationPeriod);
            HttpContext.registerPatient.Remove(patientSavedName);
            HttpContext.registerPatient.Add(patientSavedName, patient);

        }
              
        [Given(@"I add the registration status with code ""(.*)"" to ""(.*)""")]
        public void GivenIAddRegistrationStatusToPatient(string code, string patientSavedName)
        {
            Patient patient = HttpContext.registerPatient[patientSavedName];
            Extension registrationStatus = new Extension();

            registrationStatus.Url = "http://fhir.nhs.net/StructureDefinition/extension-registration-status-1";
            CodeableConcept codableConcept = new CodeableConcept();

            Coding code1 = new Coding();
            code1.Code = code;
            codableConcept.Coding.Add(code1);

            registrationStatus.Value = codableConcept;

            patient.Extension.Add(registrationStatus);

            HttpContext.registerPatient.Remove(patientSavedName);
            HttpContext.registerPatient.Add(patientSavedName, patient);

        }

        [Given(@"I add the registration type with code ""(.*)"" to ""(.*)""")]
        public void GivenIAddRegistrationTypeToPatient(string code, string patientSavedName)
        {
            Patient patient = HttpContext.registerPatient[patientSavedName];
            Extension registrationType = new Extension();

            registrationType.Url = "http://fhir.nhs.net/StructureDefinition/extension-registration-type-1";
            CodeableConcept codableConcepts = new CodeableConcept();

            Coding code2 = new Coding();
            code2.Code = code;
            codableConcepts.Coding.Add(code2);

            registrationType.Value = codableConcepts;

            patient.Extension.Add(registrationType);
            HttpContext.registerPatient.Remove(patientSavedName);
            HttpContext.registerPatient.Add(patientSavedName, patient);

        }

        [Then(@"the bundle should contain a registration type")]
        public void GivenTheResponseValidRegType()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient)) {
                    Patient patient = (Patient)entry.Resource;
                    bool regTypePresent = false;
                    foreach (Extension ext in patient.Extension)
                    {
                        string url = ext.Url.ToString();
                        if (url == "http://fhir.nhs.net/StructureDefinition/extension-registration-type-1")
                        {
                            regTypePresent = true;
                            ext.Value.ShouldNotBeNull();

                        }
                  
                     }
                    regTypePresent.ShouldBe(true);
                 }
            }
        }

        [Then(@"the bundle should contain a registration status")]
        public void GivenTheResponseValidRegStatus()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    bool regTypePresent = false;
                    foreach (Extension ext in patient.Extension)
                    {
                        string url = ext.Url.ToString();
                        if (url == "http://fhir.nhs.net/StructureDefinition/extension-registration-status-1")
                        {
                            regTypePresent = true;
                            ext.Value.ShouldNotBeNull();
                        }
                    }
                    regTypePresent.ShouldBe(true);
                }
            }
        }


        [Then(@"the bundle should contain a registration period")]
        public void GivenTheResponseValidRegPeriod()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    bool regTypePresent = false;
                    foreach (Extension ext in patient.Extension)
                    {
                        string url = ext.Url.ToString();
                        if (url == "http://fhir.nhs.net/StructureDefinition/extension-registration-period-1")
                        {
                            regTypePresent = true;
                            ext.Value.ShouldNotBeNull();
                        }
                    }
                    regTypePresent.ShouldBe(true);
                    
                }
            }
        }


        [Then(@"the bundle patient response should contain exactly 1 family name")]
        public void checkResponseForExactlyOneFamilyName()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    int count = 0;
                    Patient patient = (Patient)entry.Resource;
                    foreach (HumanName name in patient.Name)
                    {
                        name.Family.ShouldNotBeNull();
                        count++;
                    }

                    count.ShouldBe(1);
                }
            }
        }

     
        [Then(@"the bundle patient response should contain exactly 1 given name")]
        public void checkResponseForExactlyOneGivenName()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    int count = 0;
                    Patient patient = (Patient)entry.Resource;
                    foreach (HumanName name in patient.Name)
                    {
                        name.Given.ShouldNotBeNull();
                        count++;
                    }

                    count.ShouldBe(1);
                }
            }
        }

        [Then(@"the bundle patient response should contain exactly 1 gender element")]
        public void checkResponseForExactlyOneGenderElement()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    patient.Gender.ShouldNotBeNull();
                }
            }
        }

        [Then(@"the bundle patient response should contain exactly 1 birthDate element")]
        public void checkResponseForExactlyOneBirthDateElement()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    patient.BirthDate.ShouldNotBeNull();
                }
            }
        }




        [When(@"I register ""(.*)"" with url ""(.*)""")]
        public void ISendAGpcGetScheduleOperationForTheOrganizationWithLogicalIdWithIncorrectUrl(string patientSavedName, string url)
        {

            Patient patient = HttpContext.registerPatient[patientSavedName];

            FhirContext.FhirRequestParameters.Add("registerPatient", patient);

            string body = null;
            if (HttpContext.RequestContentType.Contains("xml"))
            {
                body = FhirSerializer.SerializeToXml(FhirContext.FhirRequestParameters);
            }
            else
            {
                body = FhirSerializer.SerializeToJson(FhirContext.FhirRequestParameters);
            }
            HttpSteps.RestRequest(Method.POST, url, body);
        }



        [When(@"I send a gpc.registerpatients to register ""(.*)""")]
        public void ISendAGpcGetScheduleOperationForTheOrganizationWithLogicalId(string patientSavedName)
        {

            Patient patient = HttpContext.registerPatient[patientSavedName];

            FhirContext.FhirRequestParameters.Add("registerPatient", patient);

            string body = null;
            if (HttpContext.RequestContentType.Contains("xml"))
            {
                body = FhirSerializer.SerializeToXml(FhirContext.FhirRequestParameters);
            }
            else
            {
                body = FhirSerializer.SerializeToJson(FhirContext.FhirRequestParameters);
            }
            HttpSteps.RestRequest(Method.POST, "/Patient/$gpc.registerpatient", body);
        }

     

    }
}
