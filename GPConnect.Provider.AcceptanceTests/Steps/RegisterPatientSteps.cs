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
    using System.Linq;

    [Binding]
    public class RegisterPatientSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext _fhirContext;
        private readonly HttpSteps _httpSteps;
        private readonly HttpContext _httpContext;
        private readonly PatientSteps _patientSteps;
        // Headers Helper
        public HttpHeaderHelper Headers { get; }

        public RegisterPatientSteps(HttpHeaderHelper headerHelper, FhirContext fhirContext, HttpSteps httpSteps, HttpContext httpContext, PatientSteps patientSteps)
        {
            // Helpers
            _fhirContext = fhirContext;
            Headers = headerHelper;
            _httpSteps = httpSteps;
            _httpContext = httpContext;
            _patientSteps = patientSteps;
        }

        [Given(@"I create a patient to register which does not exist on PDS and store the Patient")]
        public void GivenICreateAPatientToRegisterWhichDoesNoteExistOnPDSAndStoreThePatient()
        {
            Patient returnPatient = new Patient();
            returnPatient.Identifier.Add(new Identifier(FhirConst.IdentifierSystems.kNHSNumber, "9019546082"));
            HumanName name = new HumanName();
            name.FamilyElement.Add(new FhirString("GPConnectFamilyName"));
            name.GivenElement.Add(new FhirString("GPConnectGivenName"));
            returnPatient.Name = new List<HumanName>();
            returnPatient.Name.Add(name);
            returnPatient.Gender = AdministrativeGender.Other;
            returnPatient.BirthDateElement = new Date("2017-05-05");
            _httpContext.StoredPatient = returnPatient;
        }

        [Given(@"I update the stored patient details to not match the NHS number")]
        public void GivenIUpdateTheStoredPatientDetailsToNotMatchTheNHSNumber()
        {
            HumanName name = new HumanName();
            name.FamilyElement.Add(new FhirString("GPConnectFamilyName"));
            name.GivenElement.Add(new FhirString("GPConnectGivenName"));
            _httpContext.StoredPatient.Name = new List<HumanName>();
            _httpContext.StoredPatient.Name.Add(name);
            _httpContext.StoredPatient.Gender = AdministrativeGender.Other;
            _httpContext.StoredPatient.BirthDateElement = new Date("2017-05-05");
        }

        [Given(@"I find the next patient to register and store the Patient Resource against key ""([^""]*)""")]
        public void GivenIFindTheNextPatientToRegisterAndStoreThePatientResourceAgainstKey(string patientResourceKey)
        {
            Patient returnPatient = null;
            List<RegisterPatient> registerPatients = GlobalContext.RegisterPatients;
            for (int index = 0; index < registerPatients.Count; index++) {
                RegisterPatient registerPatient = registerPatients[index];
                // Search for patient
                Given($@"I perform a patient search for patient with NHSNumber ""{registerPatient.SPINE_NHS_NUMBER}"" and store the response bundle against key ""registerPatient""");
                // See if number of returned patients is > zero, ie patient already registered, else use patient
                Bundle patientSearchBundle = (Bundle)_httpContext.StoredFhirResources["registerPatient"];
                if (patientSearchBundle.Entry.Count == 0) {
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
                if (_httpContext.StoredFhirResources.ContainsKey(patientResourceKey)) _httpContext.StoredFhirResources.Remove(patientResourceKey);
                _httpContext.StoredFhirResources.Add(patientResourceKey, returnPatient);
            }
            else {
                Assert.Fail("No patients left to register patient with");
            }
        }

        [Given(@"I build the register patient from stored patient resource against key ""(.*)""")]
        public void GivenIBuildTheRegisterPatientFromStoredPatientResourceAgainstKey(string storedPatientKey)
        {
            _httpContext.registerPatient.Add(storedPatientKey, (Patient)_httpContext.StoredFhirResources[storedPatientKey]);
        }
        
        [Given(@"I remove the patients identifiers from the stored patient")]
        public void GivenIRemoveThePatientsIdentifiersFromTheStoredPatient()
        {
            _httpContext.StoredPatient.Identifier = null;
        }

        [Given(@"I remove the name element from the stored patient")]
        public void GivenIRemoveTheNameElementFromTheStoredPatient()
        {
            _httpContext.StoredPatient.Name = null;
        }

        [Given(@"I remove the family name element from the stored patient")]
        public void GivenIRemoveTheFamilyNameElementFromTheStoredPatient()
        {
            _httpContext.StoredPatient.Name[0].Family = null;
        }

        [Given(@"I remove the given name element from the stored patient")]
        public void GivenIRemoveTheGivenNameElementFromTheStoredPatient()
        {
            _httpContext.StoredPatient.Name[0].Given = null;
        }

        [Given(@"I remove the gender element from the stored patient")]
        public void GivenIRemoveTheGenderElementFromTheStoredPatient()
        {
            _httpContext.StoredPatient.Gender = null;
        }

        [Given(@"I remove the DOB element from the stored patient")]
        public void GivenIRemoveTheDOBElementFromTheStoredPatient()
        {
            _httpContext.StoredPatient.BirthDate = null;
        }

        [Given(@"I add the NHS Number identifier ""([^""]*)"" to the stored patient")]
        public void GivenIAddTheNHSNumberIdentifierToTheStoredPatient(string nhsNumber)
        {
            _httpContext.StoredPatient.Identifier.Add(new Identifier(FhirConst.IdentifierSystems.kNHSNumber, nhsNumber));
        }

        [Given(@"I add the registration period with start date ""([^""]*)""")]
        public void GivenIAddTheRegistrationPeriodWithStartDateTo(string regStartDate)
        {
            Extension registrationPeriod = new Extension();
            registrationPeriod.Url = "http://fhir.nhs.net/StructureDefinition/extension-registration-period-1";
            Period period = new Period();
            period.Start = regStartDate;
            registrationPeriod.Value = period;
            _httpContext.StoredPatient.Extension.Add(registrationPeriod);
        }

        [Given(@"I set the stored Patient registration period with start date ""([^""]*)"" and end date ""([^""]*)""")]
        public void GivenISetTheStoredPatientRegistrationPeriodWithStartDateAndEndDateTo(string regStartDate, string regEndDate)
        {
            Extension registrationPeriod = new Extension();
            registrationPeriod.Url = "http://fhir.nhs.net/StructureDefinition/extension-registration-period-1";
            Period period = new Period();
            if (!string.IsNullOrEmpty(regStartDate)) { period.Start = regStartDate; }
            if (!string.IsNullOrEmpty(regEndDate)) { period.End = regEndDate; }
            registrationPeriod.Value = period;
            _httpContext.StoredPatient.Extension.Add(registrationPeriod);
        }
        
        [Given(@"I add the stored patient as a parameter")]
        public void GivenIAddTheStoredPatientAsAParameter()
        {
            _httpContext.BodyParameters.Add("registerPatient", _httpContext.StoredPatient);
        }

        [Given(@"I add the stored patient as a parameter with name ""([^""]*)""")]
        public void GivenIAddTheStoredPatientAsAParameterWithName(string parameterName)
        {
            _httpContext.BodyParameters.Add(parameterName, _httpContext.StoredPatient);
        }

        [Then(@"the patient resources within the bundle should contain a registration type")]
        public void ThenThePatientResourcesWithinTheBundleShouldContainARegistrationType()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
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
                            ext.Value.ShouldNotBeNull("The registration type extension should have a value element.");

                        }
                  
                     }
                    regTypePresent.ShouldBe(true, "The patient resource should have a registration type extension.");
                 }
            }
        }

        [Given(@"I convert patient stored in ""([^""]*)"" to a register temporary patient against key ""([^""]*)""")]
        public void GivenIConvertPatientStoredInToARegisterTemporaryPatientAgainsKey(string storedPatientKey, string returnPatientKey)
        {
            Patient storedPatient = (Patient) _httpContext.StoredFhirResources[storedPatientKey];
            Patient returnPatient = new Patient();

            foreach(var identifier in storedPatient.Identifier) {
                if (string.Equals(identifier.System, FhirConst.IdentifierSystems.kNHSNumber))
                {
                    returnPatient.Identifier.Add(new Identifier(FhirConst.IdentifierSystems.kNHSNumber, identifier.Value));
                    break;
                }
            }

            string familyName = "GPConnectFamilyName";
            string givenName = "GPConnectGivenName";
            foreach (var storedPatientName in storedPatient.Name)
            {
                foreach (var storedPatientFamilyName in storedPatientName.Family)
                {
                    familyName = storedPatientFamilyName;
                    break;
                }
                foreach (var storedPatientGivenName in storedPatientName.Given)
                {
                    givenName = storedPatientGivenName;
                    break;
                }
            }
            HumanName name = new HumanName();
            name.FamilyElement.Add(new FhirString(familyName));
            name.GivenElement.Add(new FhirString(givenName));
            returnPatient.Name = new List<HumanName>();
            returnPatient.Name.Add(name);

            returnPatient.Gender = storedPatient.Gender != null ? storedPatient.Gender : AdministrativeGender.Unknown;
            returnPatient.BirthDateElement = storedPatient.BirthDateElement != null ? storedPatient.BirthDateElement : new Date();
            
            if (_httpContext.StoredFhirResources.ContainsKey(returnPatientKey)) _httpContext.StoredFhirResources.Remove(returnPatientKey);
            _httpContext.StoredFhirResources.Add(returnPatientKey, returnPatient);
        }

        [Given(@"I add the family name ""([^""]*)"" to the store patient")]
        public void GivenIAddTheFamilyNameToTheStoredPatient(string familyName)
        {
            foreach (var name in _httpContext.StoredPatient.Name) {
                name.FamilyElement.Add(new FhirString(familyName));
            }
        }

        [Given(@"I add the given name ""([^""]*)"" to the stored patient")]
        public void GivenIAddTheGivenNameToTheStoredPatient(string givenName)
        {
            foreach (var name in _httpContext.StoredPatient.Name)
            {
                name.GivenElement.Add(new FhirString(givenName));
            }
        }

        [Given(@"I add a name with given name ""([^""]*)"" and family name ""([^""]*)"" to the stored patient")]
        public void GivenIAddTheGivenNameAndFamilyNameToThePatientStoredAgainstKey(string givenName, string familyName)
        {
            var name = new HumanName();
            name.GivenElement.Add(new FhirString(givenName));
            name.FamilyElement.Add(new FhirString(familyName));
            _httpContext.StoredPatient.Name.Add(name);
        }

        [Given(@"I add an identifier with no system element to stored patient")]
        public void GivenIAddAnIdentifierWithNoSystemElementToStoredPatient()
        {
            var identifier = new Identifier();
            identifier.Value = "NewIdentifierNoSystem";
            _httpContext.StoredPatient.Identifier.Add(identifier);
        }
        
        [Given(@"I add a generic identifier to stored patient")]
        public void GivenIAddAGenericIdentifierToStoredPatient()
        {
            var identifier = new Identifier();
            identifier.Value = "GenericIdentifierValue";
            identifier.System = "GenericIdentifierSystem";
            _httpContext.StoredPatient.Identifier.Add(identifier);
        }

        [Given(@"I add a telecom element to stored patient")]
        public void GivenIAddATelecomElementToStoredPatient()
        {
            _httpContext.StoredPatient.Telecom.Add(new ContactPoint(ContactPoint.ContactPointSystem.Phone, ContactPoint.ContactPointUse.Home, "01234567891"));
        }

        [Given(@"I add a address element to stored patient")]
        public void GivenIAddAAddressElementToStoredPatient()
        {
            var address = new Address();
            address.LineElement.Add(new FhirString("1 Trevelyan Square"));
            address.LineElement.Add(new FhirString("Boar Lane"));
            address.CityElement = new FhirString("Leeds");
            address.PostalCode = "LS1 6AE";
            _httpContext.StoredPatient.Address.Add(address);
        }

        [Given(@"I add a active element to stored patient")]
        public void GivenIAddAActiveElementToStoredPatient()
        {
            _httpContext.StoredPatient.Active = true;
        }

        [Given(@"I add a deceased element to stored patient")]
        public void GivenIAddADeceasedElementToStoredPatient()
        {
            _httpContext.StoredPatient.Deceased = new FhirBoolean(false);
        }
        
        [Given(@"I add a marital element to stored patient")]
        public void GivenIAddAMaritalElementToStoredPatient()
        {
            _httpContext.StoredPatient.MaritalStatus = new CodeableConcept("http://hl7.org/fhir/v3/MaritalStatus", "M");
        }

        [Given(@"I add a births element to stored patient")]
        public void GivenIAddABirthsElementToStoredPatient()
        {
            _httpContext.StoredPatient.MultipleBirth = new FhirBoolean(true);
        }
        
        [Given(@"I add a photo element to stored patient")]
        public void GivenIAddAPhotoElementToStoredPatient()
        {
            var attachment = new Attachment();
            attachment.Url = "Test Photo Element";
            _httpContext.StoredPatient.Photo.Add(attachment);
        }

        [Given(@"I add a contact element to stored patient")]
        public void GivenIAddAContactElementToStoredPatient()
        {
            var contact = new Patient.ContactComponent();
            contact.Name = new HumanName();
            contact.Name.GivenElement.Add(new FhirString("TestGiven"));
            contact.Name.FamilyElement.Add(new FhirString("TestFamily"));
            _httpContext.StoredPatient.Contact.Add(contact);
        }

        [Given(@"I add a animal element to stored patient")]
        public void GivenIAddAAnimalElementToStoredPatient()
        {
            _httpContext.StoredPatient.Animal = new Patient.AnimalComponent();
            _httpContext.StoredPatient.Animal.Species = new CodeableConcept("AllSpecies", "Human");
        }

        [Given(@"I add a communication element to stored patient")]
        public void GivenIAddACommunicationElementToStoredPatient()
        {
            var com = new Patient.CommunicationComponent();
            com.Language = new CodeableConcept("https://tools.ietf.org/html/bcp47", "en");
            _httpContext.StoredPatient.Communication.Add(com);
        }
        
        [Given(@"I add a careprovider element to stored patient")]
        public void GivenIAddACareProviderElementToStoredPatient()
        {
            var reference = new ResourceReference();
            reference.Display = "Test Care Provider";
            _httpContext.StoredPatient.CareProvider.Add(reference);
        }

        [Given(@"I add a managingorg element to stored patient")]
        public void GivenIAddAManagingOrgElementToStoredPatient()
        {
            var reference = new ResourceReference();
            reference.Display = "Test Managing Org";
            _httpContext.StoredPatient.ManagingOrganization = reference;
        }

        [Given(@"I add a link element to stored patient")]
        public void GivenIAddALinkElementToStoredPatient()
        {
            var reference = new ResourceReference();
            reference.Display = "Test Care Provider";
            var link = new Patient.LinkComponent();
            link.Other = reference;
            link.Type = Patient.LinkType.Refer;
            _httpContext.StoredPatient.Link.Add(link);
        }

        [Then(@"the patient resources within the bundle should contain a registration status")]
        public void ThenThePatientResourcesWithinTheBundleShouldContainARegistrationStatus()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
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
                            ext.Value.ShouldNotBeNull("The resgistration status extension should have a value element.");
                        }
                    }
                    regTypePresent.ShouldBe(true, "The patient resource should contain a registration status extension.");
                }
            }
        }


        [Then(@"the patient resources within the bundle should contain a registration period")]
        public void ThenThePatientResourcesWithinBundleShouldContainARegistrationPeriod()
        {
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
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
                            ext.Value.ShouldNotBeNull("If an extension is included in the patient resource it must contain a value.");
                        }
                    }
                    regTypePresent.ShouldBe(true, "The expected registration extension was not present in the returned patient resource.");
                    
                }
            }
        }
        
        [Then(@"the patient resources within the response bundle should contain the same demographic information as the stored patient")]
        public void ThenThePatientResourceWithinTheResponseBundleShouldContainTheSameDemographicInformationAsTheStoredPatient()
        {
            // Get Stored Patient NHS Number Identifier
            string storedPatientNHSNumber = null;
            foreach (Identifier identifier in _httpContext.StoredPatient.Identifier) {
                if (identifier.System != null && string.Equals(identifier.System, FhirConst.IdentifierSystems.kNHSNumber)) {
                    storedPatientNHSNumber = identifier.Value;
                }
            }
            
            // Check the content of the returned patients matches the sent stored patient
            foreach (EntryComponent entry in ((Bundle)_fhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Patient))
                {
                    Patient patient = (Patient)entry.Resource;
                    patient.BirthDate.ShouldNotBeNull("The returned patient resource should contain a birthDate element.");
                    patient.BirthDate.ShouldBe(_httpContext.StoredPatient.BirthDate, "The returned patient DOB does not match the creted patient DOB");

                    patient.Gender.ShouldNotBeNull("The patient resource should contain a gender element");
                    patient.Gender.ShouldBe(_httpContext.StoredPatient.Gender, "The returned patient gender does not match the creted patient gender");
                    
                    patient.Name.Count.ShouldBe(1, "There should be a single name element within the returned patient resource");
                    
                    var storedGivenName = _httpContext.StoredPatient.Name.First().Given.First();
                    var storedFamilyName = _httpContext.StoredPatient.Name.First().Family.First();

                    foreach (HumanName name in patient.Name)
                    {
                        name.Given.ShouldNotBeNull("There should be a given name in the returned patient resource.");
                        name.Family.ShouldNotBeNull("There should be a family name in the returned patient resource.");

                        int familyNameCount = 0;
                        foreach (var familyname in name.Family)
                        {
                            familyname.ShouldBe(storedFamilyName, "Returned patient family name does not match created patient family name", StringCompareShould.IgnoreCase);
                            familyNameCount++;
                        }
                        familyNameCount.ShouldBe(1, "The returned Patient Resource should contain a single family name");

                        int givenCount = 0;
                        foreach (var givenname in name.Given)
                        {
                            givenname.ShouldBe(storedGivenName, "Returned patient given name does not match created patient family name", StringCompareShould.IgnoreCase);
                            givenCount++;
                        }
                        givenCount.ShouldBe(1, "The returned Patient Resource should contain a single given name");
                    }

                    int nhsNumberIdentifierCount = 0;
                    foreach (Identifier identifier in patient.Identifier)
                    {
                        if (identifier.System != null && string.Equals(identifier.System, FhirConst.IdentifierSystems.kNHSNumber))
                        {
                            identifier.Value.ShouldNotBeNullOrEmpty("The NHS Number identifier must have a value element.");
                            identifier.Value.ShouldBe(storedPatientNHSNumber, "The returned NHS Number does not match the sent NHS Number");
                            nhsNumberIdentifierCount++;
                        }
                    }
                    nhsNumberIdentifierCount.ShouldBe(1, "The returned Patient Resource should contain a single NHS Number identifier");
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
            Patient patient = (Patient)_httpContext.StoredFhirResources[storedPatientKey];
            _fhirContext.FhirRequestParameters.Add("registerPatient", patient);
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
            if (_httpContext.RequestContentType.Contains("xml"))
            {
                body = FhirSerializer.SerializeToXml(_fhirContext.FhirRequestParameters);
            }
            else
            {
                body = FhirSerializer.SerializeToJson(_fhirContext.FhirRequestParameters);
            }
            _httpSteps.RestRequest(Method.POST, url, body);
        }

        [Then(@"the response location header should resolve to a patient resource with matching details to stored patient ""([^""]*)""")]
        public void ThenTheResponseLocationHeaderShouldResolveToAPatientResourceWithMatchingDetailsToStoredPatient(string storedPatientKey)
        {
            string patientResourceLocationHeader = _httpContext.RequestHeaders.GetHeaderValue(HttpConst.Headers.kLocation);
            patientResourceLocationHeader.ShouldNotBeNullOrEmpty();
            Patient returnedResource = (Patient)_httpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:patient", patientResourceLocationHeader);
            returnedResource.GetType().ShouldBe(typeof(Patient));
            Patient storedPatient = (Patient)_httpContext.StoredFhirResources[storedPatientKey];
            returnedResource.Name.Count.ShouldBe(storedPatient.Name.Count);
            // Check names match in resources
            foreach (var returnedName in returnedResource.Name)
            {
                foreach (var storedName in storedPatient.Name)
                {
                    foreach (var givenName in storedName.Given)
                    {
                        returnedName.Given.ShouldContain(givenName, "The create patient name is not in the returned patient when read from the resource location.");
                    }
                    foreach (var familyName in storedName.Family)
                    {
                        returnedName.Family.ShouldContain(familyName, "The create patient name is not in the returned patient when read from the resource location.");
                    }
                }
            }
            // Check DOB matches
            returnedResource.BirthDate.ShouldBe(storedPatient.BirthDate);
            // Check Gender matches
            returnedResource.Gender.ShouldBe(storedPatient.Gender);
        }

        [Given(@"I get the next Patient to register and store it")]
        public void GetTheNextPatientToRegisterAndStoreIt()
        {
            var registerPatients = GlobalContext.RegisterPatients;

            foreach (var registerPatient in registerPatients)
            {
                _patientSteps.GetThePatientForPatientNhsNumber(registerPatient.SPINE_NHS_NUMBER);

                var entries = _fhirContext.Entries;

                if (!entries.Any())
                {
                    var name = new HumanName
                    {
                        FamilyElement = new List<FhirString> {new FhirString(registerPatient.NAME_FAMILY)},
                        GivenElement = new List<FhirString> {new FhirString(registerPatient.NAME_GIVEN)}
                    };

                    var patientToRegister = new Patient
                    {
                        BirthDateElement = new Date(registerPatient.DOB),
                        Name = new List<HumanName>
                        {
                            name
                        },
                        Identifier = new List<Identifier>
                        {
                            new Identifier(FhirConst.IdentifierSystems.kNHSNumber, registerPatient.SPINE_NHS_NUMBER)
                        }
                    };

                    switch (registerPatient.GENDER)
                    {
                        case "MALE":
                            patientToRegister.Gender = AdministrativeGender.Male;
                            break;
                        case "FEMALE":
                            patientToRegister.Gender = AdministrativeGender.Female;
                            break;
                        case "OTHER":
                            patientToRegister.Gender = AdministrativeGender.Other;
                            break;
                        case "UNKNOWN":
                            patientToRegister.Gender = AdministrativeGender.Unknown;
                            break;
                    }


                    _httpContext.StoredPatient = patientToRegister;

                    return;
                }
            }
        }

        [Given(@"I store the patient in the register patient resource format")]
        public void GivenIStoreThePatientInTheRegisterPatientResourceFormat()
        {
            ((Bundle)_fhirContext.FhirResponseResource).Entry.Count.ShouldBeGreaterThanOrEqualTo(1, "No patients were returned for the patient search.");
            var foundPatient = (Patient)((Bundle)_fhirContext.FhirResponseResource).Entry[0].Resource;
            
            Patient storePatient = new Patient();

            foreach (var identifier in foundPatient.Identifier)
            {
                if (string.Equals(identifier.System, FhirConst.IdentifierSystems.kNHSNumber))
                {
                    storePatient.Identifier.Add(new Identifier(FhirConst.IdentifierSystems.kNHSNumber, identifier.Value));
                    break;
                }
            }
            string familyName = "GPConnectFamilyName";
            string givenName = "GPConnectGivenName";
            foreach (var foundPatientName in foundPatient.Name)
            {
                foreach (var foundPatientFamilyName in foundPatientName.Family)
                {
                    familyName = foundPatientFamilyName;
                    break;
                }
                foreach (var foundPatientGivenName in foundPatientName.Given)
                {
                    givenName = foundPatientGivenName;
                    break;
                }
            }
            HumanName name = new HumanName();
            name.FamilyElement.Add(new FhirString(familyName));
            name.GivenElement.Add(new FhirString(givenName));
            storePatient.Name = new List<HumanName>();
            storePatient.Name.Add(name);
            storePatient.Gender = foundPatient.Gender != null ? foundPatient.Gender : AdministrativeGender.Unknown;
            storePatient.BirthDateElement = foundPatient.BirthDateElement != null ? foundPatient.BirthDateElement : new Date();

            _httpContext.StoredPatient = storePatient;
        }

        [Given(@"I set the stored Patient Registration Period to ""([^""]*)""")]
        public void SetTheStorePatientRegistrationPeriodTo(string startDate)
        {
            var registrationPeriod = new Extension
            {
                Url = "http://fhir.nhs.net/StructureDefinition/extension-registration-period-1",
                Value = new Period {Start = startDate }
                
            };

            _httpContext.StoredPatient.Extension.Add(registrationPeriod);
        }

        [Given(@"I set the stored Patient Registration Status to ""([^""]*)""")]
        public void SetTheStorePatientRegistrationStatusTo(string code)
        {
            var codableConcept = new CodeableConcept
            {
                Coding = new List<Coding>
                {
                    new Coding { Code = code }
                }
            };

            var registrationStatus = new Extension
            {
                Url = "http://fhir.nhs.net/StructureDefinition/extension-registration-status-1",
                Value = codableConcept
            };

            _httpContext.StoredPatient.Extension.Add(registrationStatus);
        }

        [Given(@"I set the stored Patient Registration Type to ""([^""]*)""")]
        public void SetTheStorePatientRegistrationTypeTo(string code)
        {
            var codableConcept = new CodeableConcept
            {
                Coding = new List<Coding>
                {
                    new Coding { Code = code }
                }
            };

            var registrationType = new Extension
            {
                Url = "http://fhir.nhs.net/StructureDefinition/extension-registration-type-1",
                Value = codableConcept
            };

            _httpContext.StoredPatient.Extension.Add(registrationType);
        }


        // Patient Steps
        [Given(@"I perform a patient search for patient ""([^""]*)"" and store the first returned resources against key ""([^""]*)""")]
        public void IPerformAPatientSearchForPatientAndStoreTheFirstReturnedResourceAgainstKey(string patient, string patientResourceKey)
        {
            Given($@"I am using the default server");
            And($@"I am performing the ""urn:nhs:names:services:gpconnect:fhir:rest:search:patient"" interaction");
            And($@"I set the JWT requested record NHS number to config patient ""{patient}""");
            And($@"I set the JWT requested scope to ""patient/*.read""");
            When($@"I search for Patient ""{patient}""");
            Then($@"the response status code should indicate success");
            And($@"the response body should be FHIR JSON");
            And($@"the response should be a Bundle resource of type ""searchset""");
            And($@"the response bundle should contain ""1"" entries");

            var returnedFirstResource = (Patient)((Bundle)_fhirContext.FhirResponseResource).Entry[0].Resource;
            returnedFirstResource.GetType().ShouldBe(typeof(Patient));
            if (_httpContext.StoredFhirResources.ContainsKey(patientResourceKey)) _httpContext.StoredFhirResources.Remove(patientResourceKey);
            _httpContext.StoredFhirResources.Add(patientResourceKey, returnedFirstResource);
        }

        [Given(@"I perform a patient search for patient with NHSNumber ""([^""]*)"" and store the response bundle against key ""([^""]*)""")]
        public void IPerformAPatientSearchForPatientWithNHSNumberAndStoreTheResponseBundleAgainstKey(string nhsNumber, string patientSearchResponseBundleKey)
        {
            Given($@"I am using the default server");
            And($@"I am performing the ""urn:nhs:names:services:gpconnect:fhir:rest:search:patient"" interaction");
            And($@"I set the JWT requested record patient NHS number to ""{nhsNumber}""");
            And($@"I set the JWT requested scope to ""patient/*.read""");
            When($@"I search for Patient with NHS Number ""{nhsNumber}""");
            Then($@"the response status code should indicate success");
            And($@"the response body should be FHIR JSON");
            And($@"the response should be a Bundle resource of type ""searchset""");
            if (_httpContext.StoredFhirResources.ContainsKey(patientSearchResponseBundleKey)) _httpContext.StoredFhirResources.Remove(patientSearchResponseBundleKey);
            _httpContext.StoredFhirResources.Add(patientSearchResponseBundleKey, (Bundle)_fhirContext.FhirResponseResource);
        }

        [When(@"I search for Patient with NHS Number ""([^""]*)""")]
        public void ISearchForPatientWithNHSNumber(string nhsNumber)
        {
            var parameterString = FhirConst.IdentifierSystems.kNHSNumber + "|" + nhsNumber;
            ISearchForAPatientWithParameterNameAndParameterString("identifier", parameterString);
        }


        [When(@"I search for a Patient with patameter name ""([^""]*)"" and parameter string ""([^""]*)""")]
        public void ISearchForAPatientWithParameterNameAndParameterString(string parameterName, string parameterString)
        {
            Given($@"I add the parameter ""{parameterName}"" with the value ""{parameterString}""");
            When($@"I make a GET request to ""/Patient""");
        }
    }
}
