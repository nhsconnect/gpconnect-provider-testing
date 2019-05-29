// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:3.0.0.0
//      SpecFlow Generator Version:3.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace GPConnect.Provider.AcceptanceTests
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("PatientRead")]
    [NUnit.Framework.CategoryAttribute("patient")]
    public partial class PatientReadFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "PatientRead.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "PatientRead", null, ProgrammingLanguage.CSharp, new string[] {
                        "patient"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read patient 404 if patient not found")]
        [NUnit.Framework.TestCaseAttribute("SomthingIncorrectWhichIsNotTheOnProviderSystem", null)]
        [NUnit.Framework.TestCaseAttribute("4543567638475665845564986758479086840564796854665748763454", null)]
        public virtual void ReadPatient404IfPatientNotFound(string id, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read patient 404 if patient not found", null, exampleTags);
#line 4
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 5
 testRunner.Given("I configure the default \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
  testRunner.And(string.Format("I set the Read Operation logical identifier used in the request to \"{0}\"", id), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 7
 testRunner.When("I make the \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 8
 testRunner.Then("the response status code should be \"404\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 9
  testRunner.And("the response should be a OperationOutcome resource with error code \"PATIENT_NOT_F" +
                    "OUND\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Patient Read with valid identifier which does not exist on providers system")]
        [NUnit.Framework.TestCaseAttribute("aaBA", null)]
        [NUnit.Framework.TestCaseAttribute("1ZEc2", null)]
        [NUnit.Framework.TestCaseAttribute("z.as.dd", null)]
        [NUnit.Framework.TestCaseAttribute("1.1.22", null)]
        [NUnit.Framework.TestCaseAttribute("40-9", null)]
        [NUnit.Framework.TestCaseAttribute("nd-skdm.mks--s", null)]
        public virtual void PatientReadWithValidIdentifierWhichDoesNotExistOnProvidersSystem(string logicalId, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Patient Read with valid identifier which does not exist on providers system", null, exampleTags);
#line 15
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 16
 testRunner.Given("I configure the default \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 17
  testRunner.And(string.Format("I set the Read Operation logical identifier used in the request to \"{0}\"", logicalId), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 18
 testRunner.When("I make the \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 19
 testRunner.Then("the response status code should be \"404\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read patient 400 or 404 if patient id not sent")]
        public virtual void ReadPatient400Or404IfPatientIdNotSent()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read patient 400 or 404 if patient id not sent", null, ((string[])(null)));
#line 29
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 30
 testRunner.Given("I configure the default \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 31
 testRunner.When("I make the \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 32
 testRunner.Then("the Response Status Code should be one of \"400, 404\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 33
  testRunner.And("the response should be a OperationOutcome resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read patient using the Accept header to request response format")]
        [NUnit.Framework.TestCaseAttribute("application/fhir+json", "JSON", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+xml", "XML", null)]
        public virtual void ReadPatientUsingTheAcceptHeaderToRequestResponseFormat(string header, string responseFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read patient using the Accept header to request response format", null, exampleTags);
#line 35
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 36
 testRunner.Given("I get the Patient for Patient Value \"patient1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 37
  testRunner.And("I store the Patient", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 38
 testRunner.Given("I configure the default \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 39
  testRunner.And(string.Format("I set the Accept header to \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 40
 testRunner.When("I make the \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 41
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 42
  testRunner.And(string.Format("the response body should be FHIR {0}", responseFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 43
  testRunner.And("the Response Resource should be a Patient", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
  testRunner.And("the Patient Id should equal the Request Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read patient using the _format parameter to request response format")]
        [NUnit.Framework.TestCaseAttribute("application/fhir+json", "JSON", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+xml", "XML", null)]
        public virtual void ReadPatientUsingThe_FormatParameterToRequestResponseFormat(string format, string bodyFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read patient using the _format parameter to request response format", null, exampleTags);
#line 50
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 51
 testRunner.Given("I get the Patient for Patient Value \"patient1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 52
  testRunner.And("I store the Patient", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 53
 testRunner.Given("I configure the default \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 54
  testRunner.And(string.Format("I add a Format parameter with the Value \"{0}\"", format), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 55
 testRunner.When("I make the \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 56
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 57
  testRunner.And(string.Format("the response body should be FHIR {0}", bodyFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
  testRunner.And("the Response Resource should be a Patient", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
  testRunner.And("the Patient Id should equal the Request Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 60
  testRunner.And("the Patient Identifiers should be valid for Patient \"patient1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read patient sending the Accept header and _format parameter to request response " +
            "format")]
        [NUnit.Framework.TestCaseAttribute("application/fhir+json", "application/fhir+json", "JSON", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+json", "application/fhir+xml", "XML", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+xml", "application/fhir+json", "JSON", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+xml", "application/fhir+xml", "XML", null)]
        public virtual void ReadPatientSendingTheAcceptHeaderAnd_FormatParameterToRequestResponseFormat(string header, string format, string bodyFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read patient sending the Accept header and _format parameter to request response " +
                    "format", null, exampleTags);
#line 66
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 67
 testRunner.Given("I get the Patient for Patient Value \"patient1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 68
  testRunner.And("I store the Patient", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 69
 testRunner.Given("I configure the default \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 70
  testRunner.And(string.Format("I set the Accept header to \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 71
  testRunner.And(string.Format("I add a Format parameter with the Value \"{0}\"", format), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 72
 testRunner.When("I make the \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 73
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 74
  testRunner.And(string.Format("the response body should be FHIR {0}", bodyFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 75
  testRunner.And("the Response Resource should be a Patient", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 76
  testRunner.And("the Patient Id should equal the Request Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
  testRunner.And("the Patient Identifiers should be valid for Patient \"patient1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read patient should contain correct logical identifier")]
        public virtual void ReadPatientShouldContainCorrectLogicalIdentifier()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read patient should contain correct logical identifier", null, ((string[])(null)));
#line 85
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 86
 testRunner.Given("I get the Patient for Patient Value \"patient1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 87
  testRunner.And("I store the Patient", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 88
 testRunner.Given("I configure the default \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 89
 testRunner.When("I make the \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 90
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 91
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 92
  testRunner.And("the Response Resource should be a Patient", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 93
  testRunner.And("the Patient Id should equal the Request Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read patient response should contain an ETag header")]
        public virtual void ReadPatientResponseShouldContainAnETagHeader()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read patient response should contain an ETag header", null, ((string[])(null)));
#line 95
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 96
 testRunner.Given("I get the Patient for Patient Value \"patient1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 97
  testRunner.And("I store the Patient", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 98
 testRunner.Given("I configure the default \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 99
 testRunner.When("I make the \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 100
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 101
  testRunner.And("the Response Resource should be a Patient", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 102
  testRunner.And("the Response should contain the ETag header matching the Resource Version Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 103
  testRunner.And("the Patient Identifiers should be valid for Patient \"patient1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read patient returned should conform to the GPconnect specification")]
        [NUnit.Framework.CategoryAttribute("1.2.3")]
        public virtual void ReadPatientReturnedShouldConformToTheGPconnectSpecification()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read patient returned should conform to the GPconnect specification", null, new string[] {
                        "1.2.3"});
#line 106
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 107
 testRunner.Given("I get the Patient for Patient Value \"patient1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 108
  testRunner.And("I store the Patient", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 109
 testRunner.Given("I configure the default \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 110
 testRunner.When("I make the \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 111
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 112
  testRunner.And("the Response Resource should be a Patient", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 113
  testRunner.And("the Patient Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 114
  testRunner.And("the Patient Metadata should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 115
  testRunner.And("the Patient Identifiers should be valid for Patient \"patient1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 116
  testRunner.And("the Patient GeneralPractitioner Practitioner should be valid and resolvable", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 117
  testRunner.And("the Patient ManagingOrganization Organization should be valid and resolvable", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 118
  testRunner.And("the Patient Deceased should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 119
  testRunner.And("the Patient MultipleBirth should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 120
  testRunner.And("the Patient Telecom should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 121
  testRunner.And("the Patient Contact Relationship should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 122
  testRunner.And("the Patient Communication should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 123
  testRunner.And("the Patient Name should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 124
  testRunner.And("the Patient Contact Name should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 125
  testRunner.And("the Patient should exclude disallowed fields", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 126
  testRunner.And("the Patient Use should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 127
  testRunner.And("the Patient Gender should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 128
  testRunner.And("the Patient MaritalStatus should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 129
  testRunner.And("the Patient Link should be valid and resolvable", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 130
  testRunner.And("the Patient Registration Details should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 133
  testRunner.And("the Patient Not In Use should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("CapabilityStatement profile supports the Patient read operation")]
        public virtual void CapabilityStatementProfileSupportsThePatientReadOperation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("CapabilityStatement profile supports the Patient read operation", null, ((string[])(null)));
#line 135
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 136
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 137
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 138
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 139
  testRunner.And("the CapabilityStatement REST Resources should contain the \"Patient\" Resource with" +
                    " the \"Read\" Interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Patient read valid response check caching headers exist")]
        public virtual void PatientReadValidResponseCheckCachingHeadersExist()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Patient read valid response check caching headers exist", null, ((string[])(null)));
#line 141
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 142
 testRunner.Given("I get the Patient for Patient Value \"patient1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 143
  testRunner.And("I store the Patient", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 144
 testRunner.Given("I configure the default \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 145
 testRunner.When("I make the \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 146
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 147
  testRunner.And("the Response Resource should be a Patient", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 148
  testRunner.And("the required cacheing headers should be present in the response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Patient read invalid response check caching headers exist")]
        public virtual void PatientReadInvalidResponseCheckCachingHeadersExist()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Patient read invalid response check caching headers exist", null, ((string[])(null)));
#line 150
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 151
 testRunner.Given("I configure the default \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 152
  testRunner.And("I set the Read Operation logical identifier used in the request to \"AABa\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 153
 testRunner.When("I make the \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 154
 testRunner.Then("the response status code should be \"404\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 155
  testRunner.And("the required cacheing headers should be present in the response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Patient read valid response check preferred branch")]
        public virtual void PatientReadValidResponseCheckPreferredBranch()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Patient read valid response check preferred branch", null, ((string[])(null)));
#line 157
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 158
 testRunner.Given("I get the Patient for Patient Value \"patient1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 159
  testRunner.And("I store the Patient", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 160
 testRunner.Given("I configure the default \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 161
 testRunner.When("I make the \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 162
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 163
  testRunner.And("the Response Resource should be a Patient", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 164
  testRunner.And("the Patient RegistrationDetails should include preferredBranchSurgery", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Check read patient on a patient with no Consent returns a valid response")]
        public virtual void CheckReadPatientOnAPatientWithNoConsentReturnsAValidResponse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Check read patient on a patient with no Consent returns a valid response", null, ((string[])(null)));
#line 177
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 178
 testRunner.Given("I get the Patient for Patient Value \"patient15\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 179
  testRunner.And("I store the Patient", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 180
 testRunner.Given("I configure the default \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 181
 testRunner.When("I make the \"PatientRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 182
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 183
  testRunner.And("the Response Resource should be a Patient", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 184
  testRunner.And("the Patient Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 185
  testRunner.And("the Patient Metadata should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 186
  testRunner.And("the Patient Identifiers should be valid for Patient \"patient15\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 187
  testRunner.And("the Patient GeneralPractitioner Practitioner should be valid and resolvable", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 188
  testRunner.And("the Patient ManagingOrganization Organization should be valid and resolvable", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 189
  testRunner.And("the Patient Deceased should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 190
  testRunner.And("the Patient MultipleBirth should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 191
  testRunner.And("the Patient Telecom should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 192
  testRunner.And("the Patient Contact Relationship should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 193
  testRunner.And("the Patient Communication should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 194
  testRunner.And("the Patient Name should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 195
  testRunner.And("the Patient Contact Name should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 196
  testRunner.And("the Patient should exclude disallowed fields", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 197
  testRunner.And("the Patient Use should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 198
  testRunner.And("the Patient Gender should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 199
  testRunner.And("the Patient MaritalStatus should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 200
  testRunner.And("the Patient Link should be valid and resolvable", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 201
  testRunner.And("the Patient Registration Details should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 204
  testRunner.And("the Patient Not In Use should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion