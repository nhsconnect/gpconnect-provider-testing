using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Model;
using Shouldly;
using TechTalk.SpecFlow;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace GPConnect.Provider.AcceptanceTests.Steps
{

    [Binding]
    public class PatientSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private readonly SecurityContext SecurityContext;
        private readonly HttpContext HttpContext;
        
        // Constructor

        public PatientSteps(SecurityContext securityContext, HttpContext httpContext, FhirContext fhirContext)
        {
            Log.WriteLine("PatientSteps() Constructor");
            SecurityContext = securityContext;
            HttpContext = httpContext;            
            FhirContext = fhirContext;
        }
        
        // Patient Steps

        [When(@"I search for Patient ""([^""]*)""")]
        public void ISearchForPatient(string patient)
        {
            ISearchForPatientWithSystem(patient, FhirConst.IdentifierSystems.kNHSNumber);
        }

        [When(@"I search for Patient ""([^""]*)"" with system ""([^""]*)""")]
        public void ISearchForPatientWithSystem(string patient, string identifierSystem)
        {
            var parameterString = identifierSystem + "|" + FhirContext.FhirPatients[patient];
            ISearchForAPatientWithParameterNameAndParameterString("identifier", parameterString);
        }

        [When(@"I search for Patient ""([^""]*)"" without system in identifier parameter")]
        public void ISearchForPatientWithoutSystemInIdentifierParameter(string patient)
        {
            ISearchForAPatientWithParameterNameAndParameterString("identifier", FhirContext.FhirPatients[patient]);
        }

        [When(@"I search for Patient ""([^""]*)"" with parameter name ""([^""]*)"" and system ""([^""]*)""")]
        public void ISearchForPatientWithParameterNameAndSystem(string patient, string parameterName, string parameterSystem)
        {
            var parameterString = parameterSystem + "|" + FhirContext.FhirPatients[patient];
            ISearchForAPatientWithParameterNameAndParameterString(parameterName, parameterString);
        }

        [When(@"I search for a Patient with patameter name ""([^""]*)"" and parameter string ""([^""]*)""")]
        public void ISearchForAPatientWithParameterNameAndParameterString(string parameterName, string parameterString)
        {
            Given($@"I add the parameter ""{parameterName}"" with the value ""{parameterString}""");
            When($@"I make a GET request to ""/Patient""");
        }
        
        [Then(@"the response bundle Patient entries should contain a single NHS Number identifier for patient ""([^""]*)""")]
        public void ThenTheResponseBundlePatientEntriesShouldContainASingleNHSNumberIdentifierForPatient(string patient)
        {
            Bundle bundle = (Bundle)FhirContext.FhirResponseResource;
            foreach (var entry in bundle.Entry) {
                var patientResource = (Patient)entry.Resource;
                int nhsNumberIdentifierCount = 0;
                foreach (var identifier in patientResource.Identifier) {
                    if (identifier.System == FhirConst.IdentifierSystems.kNHSNumber) {
                        nhsNumberIdentifierCount++;
                        identifier.Value.ShouldBe(FhirContext.FhirPatients[patient],"NHS Number does not match expected NHS Number.");
                    }
                }
                nhsNumberIdentifierCount.ShouldBe(1, "There was more or less than 1 NHS Number identifier.");
            }
        }
    }
}
