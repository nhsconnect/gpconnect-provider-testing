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
