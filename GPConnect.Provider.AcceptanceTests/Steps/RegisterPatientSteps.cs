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
        
        [Given(@"I remove the patients identifiers from the patient stored against key ""([^""]*)""")]
        public void GivenIRemoveThePatientsIdentifiersFromThePatientStoredAgainstKey(string storedPatientKey)
        {
            Patient patient = (Patient)HttpContext.StoredFhirResources[storedPatientKey];
            patient.Identifier = null;
            HttpContext.StoredFhirResources.Remove(storedPatientKey);
            HttpContext.StoredFhirResources.Add(storedPatientKey, patient);
        }
        
        [Given(@"I remove the name element from the patient stored against key ""([^""]*)""")]
        public void GivenIRemoveTheNameElementFromThePatientStoredAgainstKey(string storedPatientKey)
        {
            Patient patient = (Patient)HttpContext.StoredFhirResources[storedPatientKey];
            patient.Name = null;
            HttpContext.StoredFhirResources.Remove(storedPatientKey);
            HttpContext.StoredFhirResources.Add(storedPatientKey, patient);
        }

        [Given(@"I remove the gender element from the patient stored against key ""([^""]*)""")]
        public void GivenIRemoveTheGenderElementFromThePatientStoredAgainstKey(string storedPatientKey)
        {
            Patient patient = (Patient)HttpContext.StoredFhirResources[storedPatientKey];
            patient.Gender = null;
            HttpContext.StoredFhirResources.Remove(storedPatientKey);
            HttpContext.StoredFhirResources.Add(storedPatientKey, patient);
        }

        [Given(@"I remove the DOB element from the patient stored against key ""([^""]*)""")]
        public void GivenIRemoveTheDOBElementFromThePatientStoredAgainstKey(string storedPatientKey)
        {
            Patient patient = (Patient)HttpContext.StoredFhirResources[storedPatientKey];
            patient.BirthDate = null;
            HttpContext.StoredFhirResources.Remove(storedPatientKey);
            HttpContext.StoredFhirResources.Add(storedPatientKey, patient);
        }

        [Given(@"I clear exisiting identifiers in the patient stored against key ""([^""]*)"" and add an NHS number identifier ""([^""]*)""")]
        public void GivenIRemoveTheDOBElementFromThePatientStoredAgainstKey(string storedPatientKey, string nhsNumber)
        {
            Patient patient = (Patient)HttpContext.StoredFhirResources[storedPatientKey];
            patient.Identifier.Clear();
            patient.Identifier.Add(new Identifier(FhirConst.IdentifierSystems.kNHSNumber, nhsNumber));
            HttpContext.StoredFhirResources.Remove(storedPatientKey);
            HttpContext.StoredFhirResources.Add(storedPatientKey, patient);
        }

        [Given(@"I add the registration period with start date ""([^""]*)"" to ""([^""]*)""")]
        public void GivenIAddTheRegistrationPeriodWithStartDateTo(string regStartDate, string storedPatientKey)
        {
            Patient patient = (Patient)HttpContext.StoredFhirResources[storedPatientKey];
            Extension registrationPeriod = new Extension();
            registrationPeriod.Url = "http://fhir.nhs.net/StructureDefinition/extension-registration-period-1";
            Period period = new Period();
            period.Start = regStartDate;
            registrationPeriod.Value = period;
            patient.Extension.Add(registrationPeriod);
            HttpContext.StoredFhirResources.Remove(storedPatientKey);
            HttpContext.StoredFhirResources.Add(storedPatientKey, patient);
        }

        [Given(@"I add the registration period with start date ""([^""]*)"" and end date ""([^""]*)"" to ""([^""]*)""")]
        public void GivenIAddTheRegistrationPeriodWithStartDateAndEndDateTo(string regStartDate, string regEndDate, string storedPatientKey)
        {
            Patient patient = (Patient)HttpContext.StoredFhirResources[storedPatientKey];
            Extension registrationPeriod = new Extension();
            registrationPeriod.Url = "http://fhir.nhs.net/StructureDefinition/extension-registration-period-1";
            Period period = new Period();
            period.Start = regStartDate;
            period.End = regEndDate;
            registrationPeriod.Value = period;
            patient.Extension.Add(registrationPeriod);
            HttpContext.StoredFhirResources.Remove(storedPatientKey);
            HttpContext.StoredFhirResources.Add(storedPatientKey, patient);
        }
              
        [Given(@"I add the registration status with code ""([^""]*)"" to ""([^""]*)""")]
        public void GivenIAddRegistrationStatusWithCodeTo(string code, string storedPatientKey)
        {
            Patient patient = (Patient)HttpContext.StoredFhirResources[storedPatientKey];
            Extension registrationStatus = new Extension();
            registrationStatus.Url = "http://fhir.nhs.net/StructureDefinition/extension-registration-status-1";
            CodeableConcept codableConcept = new CodeableConcept();
            Coding code1 = new Coding();
            code1.Code = code;
            codableConcept.Coding.Add(code1);
            registrationStatus.Value = codableConcept;
            patient.Extension.Add(registrationStatus);
            HttpContext.StoredFhirResources.Remove(storedPatientKey);
            HttpContext.StoredFhirResources.Add(storedPatientKey, patient);

        }

        [Given(@"I add the registration type with code ""([^""]*)"" to ""([^""]*)""")]
        public void GivenIAddRegistrationTypeWithCodeTo(string code, string storedPatientKey)
        {
            Patient patient = (Patient)HttpContext.StoredFhirResources[storedPatientKey];
            Extension registrationType = new Extension();
            registrationType.Url = "http://fhir.nhs.net/StructureDefinition/extension-registration-type-1";
            CodeableConcept codableConcepts = new CodeableConcept();
            Coding code2 = new Coding();
            code2.Code = code;
            codableConcepts.Coding.Add(code2);
            registrationType.Value = codableConcepts;
            patient.Extension.Add(registrationType);
            HttpContext.StoredFhirResources.Remove(storedPatientKey);
            HttpContext.StoredFhirResources.Add(storedPatientKey, patient);
        }

        [Given(@"I add the resource stored against key ""([^""]*)"" as a parameter named ""([^""]*)"" to the request")]
        public void GivenIAddTheResourceStoredAgainstKeyAsAParameternamedToTheRequest(string storedPatientKey, string parameterName)
        {
            FhirContext.FhirRequestParameters.Add(parameterName, HttpContext.StoredFhirResources[storedPatientKey]);
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

        [Then(@"the response bundle should contain a patient resource which contains atleast a single NHS number identifier matching patient stored against key ""([^""]*)""")]
        public void ThenTheResponseBundleShouldContainAPatientResourceWhichContainsAtleastASingleNHSNumberIdentifier(string storedPatientKey)
        {
            Patient storedPatient = (Patient)HttpContext.StoredFhirResources[storedPatientKey];
            string storedPatientNHSNumber = null;
            foreach (Identifier identifier in storedPatient.Identifier) {
                if (identifier.System != null && string.Equals(identifier.System, FhirConst.IdentifierSystems.kNHSNumber)) {
                    storedPatientNHSNumber = identifier.Value;
                }
            }
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    int nhsNumberIdentifierCount = 0;
                    string patientNHSNumber = null;
                    Patient patient = (Patient)entry.Resource;
                    foreach (Identifier identifier in patient.Identifier)
                    {
                        if (identifier.System != null && string.Equals(identifier.System, FhirConst.IdentifierSystems.kNHSNumber))
                        {
                            identifier.Value.ShouldNotBeNullOrEmpty("The NHS Number identifier must have a value element.");
                            patientNHSNumber = identifier.Value;
                            nhsNumberIdentifierCount++;
                        }
                    }
                    nhsNumberIdentifierCount.ShouldBe(1, "The returned Patient Resource should contain a single NHS Number identifier");
                    if (storedPatientNHSNumber != null)
                    {
                        storedPatientNHSNumber.ShouldBe(patientNHSNumber, "The patient NHS Number does not match the created patient NHS number");
                    }
                }
            }

        }
        
        [Then(@"the response bundle should contain a patient resource which contains exactly 1 family name matching the patient stored against key ""([^""]*)""")]
        public void ThenTheResponseBundleShouldContainAPatientResourceWhichContainsExactly1FamilyName(string storedPatientKey)
        {
            Patient storedPatient = (Patient)HttpContext.StoredFhirResources[storedPatientKey];
            string storedFamilyName = "";
            foreach (HumanName name in storedPatient.Name)
            {
                foreach (var familyname in name.Family)
                {
                    storedFamilyName = familyname;
                }
            }
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    patient.Name.Count.ShouldBe(1, "There should be a single name element within the returned patient resource");
                    foreach (HumanName name in patient.Name)
                    {
                        name.Family.ShouldNotBeNull();
                        int count = 0;
                        foreach (var familyname in name.Family)
                        {
                            familyname.ShouldBe(storedFamilyName, Case.Insensitive, "Returned patient family name does not match created patient family name");
                            count++;
                        }
                        count.ShouldBe(1, "The returned Patient Resource should contain a single family name");
                    }
                    
                }
            }
        }

     
        [Then(@"the response bundle should contain a patient resource which contains exactly 1 given name matching the patient stored against key ""([^""]*)""")]
        public void ThenTheResponseBundleShouldContainAPatientResourceWhichContainsExactly1GivenName(string storedPatientKey)
        {
            Patient storedPatient = (Patient)HttpContext.StoredFhirResources[storedPatientKey];
            string storedGivenName = "";
            foreach (HumanName name in storedPatient.Name)
            {
                foreach (var givenname in name.Given)
                {
                    storedGivenName = givenname;
                }
            }
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    patient.Name.Count.ShouldBe(1, "There should be a single name element within the returned patient resource");
                    foreach (HumanName name in patient.Name)
                    {
                        name.Given.ShouldNotBeNull();
                        int count = 0;
                        foreach (var givenname in name.Given)
                        {
                            givenname.ShouldBe(storedGivenName, Case.Insensitive, "Returned patient given name does not match created patient family name");
                            count++;
                        }
                        count.ShouldBe(1, "The returned Patient Resource should contain a single given name");
                    }
                }
            }
        }

        [Then(@"the response bundle should contain a patient resource which contains exactly 1 gender element matching the patient stored against key ""([^""]*)""")]
        public void ThenTheResponseBundleShouldContainAPatientResourceWhichContainsExactly1GenerElement(string storedPatientKey)
        {
            Patient storedPatient = (Patient)HttpContext.StoredFhirResources[storedPatientKey];
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    patient.Gender.ShouldNotBeNull("The patient resource should contain a gender element");
                    patient.Gender.ShouldBe(storedPatient.Gender, "The returned patient gender does not match the creted patient gender");
                }
            }
        }

        [Then(@"the response bundle should contain a patient resource which contains exactly 1 birthDate element matching the patient stored against key ""([^""]*)""")]
        public void ThenTheResponseBundleShouldContainAPatientResourceWhichContainsExactly1BirthDateElement(string storedPatientKey)
        {
            Patient storedPatient = (Patient)HttpContext.StoredFhirResources[storedPatientKey];
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    patient.BirthDate.ShouldNotBeNull();
                    patient.BirthDate.ShouldBe(storedPatient.BirthDate, "The returned patient DOB does not match the creted patient DOB");
                }
            }
        }
        
        [When(@"I send a gpc.registerpatient to create patient stored against key ""([^""]*)""")]
        public void ISendAGpcRegisterPatientToCreatepatientStoredAgainstKey(string storedPatientKey)
        {
            IRegisterPatientStoredAgainstKeyWithURL(storedPatientKey, "/Patient/$gpc.registerpatient");
        }

        [When(@"I register patient stored against key ""(.*)"" with url ""(.*)""")]
        public void IRegisterPatientStoredAgainstKeyWithURL(string storedPatientKey, string url)
        {
            Patient patient = (Patient)HttpContext.StoredFhirResources[storedPatientKey];
            FhirContext.FhirRequestParameters.Add("registerPatient", patient);
            IRegisterPatientWithURL(url);
        }

        [When(@"I send a gpc.registerpatient to create patient")]
        public void ISendAGPCRegisterPatientToCreatePatient()
        {
            IRegisterPatientWithURL("/Patient/$gpc.registerpatient");
        }

        [When(@"I register patient with url ""(.*)""")]
        public void IRegisterPatientWithURL(string url)
        {
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

        [When(@"I register Patient stored against key ""(.*)"" using JSON but change the patient resource type to INVALIDRESOURCE")]
        public void IRegisterPatientStoredAgainstKeyUsingJSONButChangeThePatientResourceTypeToINVaLIDRESOURCE(string storedPatientKey)
        {
            Resource patient = HttpContext.StoredFhirResources[storedPatientKey];
            string patientString = FhirSerializer.SerializeToJson(patient);
            patientString = FhirHelper.ChangeResourceTypeString(patientString, "INVALIDRESOURCE");
            string body = "{\"resourceType\":\"Parameters\",\"parameter\":[{\"name\":\"registerPatient\",\"resource\":" + patientString + "}]}";
            HttpSteps.RestRequest(Method.POST, "/Patient/$gpc.registerpatient", body);
        }

        [When(@"I register Patient stored against key ""(.*)"" using JSON but add an additional invalid field to the patient resource")]
        public void IRegisterPatientStoredAgainstKeyUsingJSONButAddAnAdditionalInvalidFieldToThepatientResource(string storedPatientKey)
        {
            Resource patient = HttpContext.StoredFhirResources[storedPatientKey];
            string patientString = FhirSerializer.SerializeToJson(patient);
            patientString = FhirHelper.AddInvalidFieldToResourceJson(patientString);
            string body = "{\"resourceType\":\"Parameters\",\"parameter\":[{\"name\":\"registerPatient\",\"resource\":" + patientString + "}]}";
            HttpSteps.RestRequest(Method.POST, "/Patient/$gpc.registerpatient", body);
        }

        [When(@"I register Patient stored against key ""(.*)"" using JSON but change the bundle resource type to INVALIDRESOURCE")]
        public void IRegisterPatientStoredAgainstKeyUsingJSONButChangeTheBundleResourceTypeToINVaLIDRESOURCE(string storedPatientKey)
        {
            Resource patient = HttpContext.StoredFhirResources[storedPatientKey];
            FhirContext.FhirRequestParameters.Add("registerPatient", patient);
            string body = FhirSerializer.SerializeToJson(FhirContext.FhirRequestParameters);
            body = FhirHelper.ChangeResourceTypeString(body, "INVALIDRESOURCE");
            HttpSteps.RestRequest(Method.POST, "/Patient/$gpc.registerpatient", body);
        }

    }
}
