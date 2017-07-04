namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using Constants;
    using Context;
    using Helpers;
    using Hl7.Fhir.Model;
    using TechTalk.SpecFlow;

    [Binding]
    public sealed class AccessRecordSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;

        public AccessRecordSteps(FhirContext fhirContext, HttpSteps httpSteps, HttpContext httpContext) 
            : base(fhirContext, httpSteps)
        {
            _httpContext = httpContext;
        }

        [Given(@"I set the Parameter name ""(.*)"" to ""(.*)""")]
        public void SetTheParameterNameTo(string parameterName, string invalidParameterName)
        {
            _httpContext.BodyParameters.GetSingle(parameterName).Name = invalidParameterName;
        }

        #region NHS Number Parameter

        [Given(@"I add an NHS Number parameter for ""(.*)""")]
        public void AddAnNhsNumberParameterFor(string patient)
        {
            var nhsNumber = GlobalContext.PatientNhsNumberMap[patient];
            _httpContext.BodyParameters.Add(FhirConst.GetCareRecordParams.kPatientNHSNumber, NhsNumberHelper.GetDefaultIdentifier(nhsNumber));
        }

        [Given(@"I add an NHS Number parameter for an invalid NHS Number")]
        public void AddAnNhsNumberParameterForInvalidNhsNumber()
        {
            _httpContext.BodyParameters.Add(FhirConst.GetCareRecordParams.kPatientNHSNumber, NhsNumberHelper.GetDefaultIdentifierWithInvalidNhsNumber());
        }

        [Given(@"I add an NHS Number parameter with an empty NHS Number")]
        public void AddAnNhsNumberParameterWithAnEmptyNhsNumber()
        {
            _httpContext.BodyParameters.Add(FhirConst.GetCareRecordParams.kPatientNHSNumber, NhsNumberHelper.GetIdentifierWithEmptyNhsNumber());
        }

        [Given(@"I add an NHS Number parameter for ""(.*)"" with an invalid Identifier System")]
        public void AddAnNhsNumberParameterForWithAnInvalidIdentifierSystem(string patient)
        {
            var nhsNumber = GlobalContext.PatientNhsNumberMap[patient];
            _httpContext.BodyParameters.Add(FhirConst.GetCareRecordParams.kPatientNHSNumber, NhsNumberHelper.GetDefaultIdentifierWithInvalidSystem(nhsNumber));
        }

        [Given(@"I add an NHS Number parameter for ""(.*)"" with an empty Identifier System")]
        public void AddAnNhsNumberParameterForWithAnEmptyIdentifierSystem(string patient)
        {
            var nhsNumber = GlobalContext.PatientNhsNumberMap[patient];
            _httpContext.BodyParameters.Add(FhirConst.GetCareRecordParams.kPatientNHSNumber, NhsNumberHelper.GetIdentifierWithEmptySystem(nhsNumber));
        }

        [Given(@"I add an NHS Number parameter for ""(.*)"" using an invalid parameter type")]
        public void AddANhsNumberParameterForUsingAnInvalidParameterType(string patient)
        {
            var nhsNumber = GlobalContext.PatientNhsNumberMap[patient];
            _httpContext.BodyParameters.Add(FhirConst.GetCareRecordParams.kPatientNHSNumber, new FhirString(nhsNumber));
        }

        #endregion

        #region Record Section Parameter

        [Given(@"I add a Record Section parameter for ""(.*)""")]
        public void AddARecordSectionParameterFor(string recordSection)
        {
            _httpContext.BodyParameters.Add(FhirConst.GetCareRecordParams.kRecordSection, RecordSectionHelper.GetRecordSection(recordSection));
        }

        [Given(@"I add a Record Section parameter with invalid Code")]
        public void AddARecordSectionParameterWithInvalidCode()
        {
            _httpContext.BodyParameters.Add(FhirConst.GetCareRecordParams.kRecordSection, RecordSectionHelper.GetRecordSectionWithInvalidCode());
        }

        [Given(@"I add a Record Section parameter for ""(.*)"" with invalid System")]
        public void AddARecordSectionParameterForWithInvalidSystem(string recordSection)
        {
            _httpContext.BodyParameters.Add(FhirConst.GetCareRecordParams.kRecordSection, RecordSectionHelper.GetRecordSectionWithInvalidSystem(recordSection));
        }

        [Given(@"I add a Record Section parameter for ""(.*)"" with empty System")]
        public void AddARecordSectionParameterForWithEmptySystem(string recordSection)
        {
            _httpContext.BodyParameters.Add(FhirConst.GetCareRecordParams.kRecordSection, RecordSectionHelper.GetRecordSectionWithEmptySystem(recordSection));
        }

        [Given(@"I add a Record Section parameter with empty Code")]
        public void AddARecordSectionParameterWithEmptyCode()
        {
            _httpContext.BodyParameters.Add(FhirConst.GetCareRecordParams.kRecordSection, RecordSectionHelper.GetRecordSystemWithEmptyCode());
        }

        [Given(@"I add a Record Section parameter for ""(.*)"" using an invalid parameter type")]
        public void AddARecordSectionParameterForUsingAnInvalidParameterType(string recordSection)
        {
            _httpContext.BodyParameters.Add(FhirConst.GetCareRecordParams.kRecordSection, new FhirString(recordSection));
        }

        #endregion

        #region Time Period Parameter

        [Given(@"I add a valid Time Period parameter")]
        public void AddAValidTimePeriodParameter()
        {
            _httpContext.BodyParameters.Add(FhirConst.GetCareRecordParams.kTimePeriod, TimePeriodHelper.GetDefaultTimePeriod());
        }

        [Given(@"I add a Time Period parameter with invalid Start Date")]
        public void AddATimePeriodParameterWithInvalidStartDate()
        {
            _httpContext.BodyParameters.Add(FhirConst.GetCareRecordParams.kTimePeriod, TimePeriodHelper.GetTimePeriodInvalidStartDate());
        }

        [Given(@"I add a Time Period parameter with invalid End Date")]
        public void AddATimePeriodParameterWithInvalidEndDate()
        {
            _httpContext.BodyParameters.Add(FhirConst.GetCareRecordParams.kTimePeriod, TimePeriodHelper.GetTimePeriodInvalidEndDate());
        }

        [Given(@"I add a Time Period parameter with Start Date after End Date")]
        public void AddATimePeriodParameterWithStartDateAfterEndDate()
        {
            _httpContext.BodyParameters.Add(FhirConst.GetCareRecordParams.kTimePeriod, TimePeriodHelper.GetTimePeriodStartDateAfterEndDate());
        }

        [Given(@"I add a Time Period parameter with Start Date only")]
        public void AddATimePeriodParameterWithStartDateOnly()
        {
            _httpContext.BodyParameters.Add(FhirConst.GetCareRecordParams.kTimePeriod, TimePeriodHelper.GetTimePeriodStartDateOnly());
        }

        [Given(@"I add a Time Period parameter with End Date only")]
        public void AddATimePeriodParameterWithEndDateOnly()
        {
            _httpContext.BodyParameters.Add(FhirConst.GetCareRecordParams.kTimePeriod, TimePeriodHelper.GetTimePeriodEndDateOnly());
        }

        [Given(@"I add a Time Period parameter with ""(.*)"" and ""(.*)""")]
        public void AddATimePeriodParameterWithStartDateAndEndDate(string startDate, string endDate)
        {
            _httpContext.BodyParameters.Add(FhirConst.GetCareRecordParams.kTimePeriod, TimePeriodHelper.GetTimePeriod(startDate, endDate));
        }

        [Given(@"I add a Time Period parameter with Start Date today and End Date in ""(.*)"" days")]
        public void AddATimePeriodParameterWithStartDateTodayAndEndDateInDays(int days)
        {
            _httpContext.BodyParameters.Add(FhirConst.GetCareRecordParams.kTimePeriod, TimePeriodHelper.GetTimePeriodStartDateTodayEndDateDays(days));
        }

        #endregion
    }
}
