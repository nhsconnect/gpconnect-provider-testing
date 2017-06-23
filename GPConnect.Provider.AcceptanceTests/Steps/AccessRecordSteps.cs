namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using Context;
    using Hl7.Fhir.Model;
    using TechTalk.SpecFlow;

    [Binding]
    public sealed class AccessRecordSteps : BaseSteps
    {
        public AccessRecordSteps(FhirContext fhirContext, HttpSteps httpSteps) 
            : base(fhirContext, httpSteps)
        {
        }

        #region Given

        //Used elsewhere
        [Given(@"I am requesting the record for config patient ""([^""]*)""")]
        public void GivenIAmRequestingTheRecordForConfigPatient(string patient)
        {
            Given($@"I am requesting the record for patient with NHS Number ""{GlobalContext.PatientNhsNumberMap[patient]}""");
        }

        [Given(@"I replace the parameter name ""([^""]*)"" with ""([^""]*)""")]
        public void GivenIReplaceTheParameterNameWith(string parameterName, string newParameterName)
        {
            _fhirContext.FhirRequestParameters.GetSingle(parameterName).Name = newParameterName;
        }

        [Given(@"I set the parameter patientNHSNumber with an empty value")]
        public void GivenISetThePatientNHSNumberParameterWithAnEmptyValue()
        {
            var parameter = _fhirContext.FhirRequestParameters.GetSingle("patientNHSNumber");
            ((Identifier)parameter.Value).Value = "";
        }

        [Given(@"I set the parameter patientNHSNumber with an empty system")]
        public void GivenISetThePatientNHSNumberParameterWithAnEmptySystem()
        {
            var parameter = _fhirContext.FhirRequestParameters.GetSingle("patientNHSNumber");
            ((Identifier)parameter.Value).System = "";
        }

        [Given(@"I set the parameter recordSection with an empty system")]
        public void GivenISetTheRecordSectionParameterWithAnEmptySystem()
        {
            var parameter = _fhirContext.FhirRequestParameters.GetSingle("recordSection");
            ((CodeableConcept)parameter.Value).Coding[0].System = "";
        }

        #endregion
    }
}
