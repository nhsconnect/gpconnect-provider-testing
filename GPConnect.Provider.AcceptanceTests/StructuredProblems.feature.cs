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
    [NUnit.Framework.DescriptionAttribute("StructuredProblems")]
    [NUnit.Framework.CategoryAttribute("structuredrecord")]
    public partial class StructuredProblemsFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "StructuredProblems.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "StructuredProblems", null, ProgrammingLanguage.CSharp, new string[] {
                        "structuredrecord"});
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
        [NUnit.Framework.DescriptionAttribute("Verify Problems structured record for a Patient")]
        [NUnit.Framework.CategoryAttribute("1.3.1")]
        public virtual void VerifyProblemsStructuredRecordForAPatient()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify Problems structured record for a Patient", null, new string[] {
                        "1.3.1"});
#line 5
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 6
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 7
  testRunner.And("I add an NHS Number parameter for \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 8
  testRunner.And("I add the Problems parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 9
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 10
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 11
  testRunner.And("the response should be a Bundle resource of type \"collection\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
  testRunner.And("the response meta profile should be for \"structured\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
  testRunner.And("the patient resource in the bundle should contain meta data profile and version i" +
                    "d", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
  testRunner.And("if the response bundle contains a practitioner resource it should contain meta da" +
                    "ta profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
  testRunner.And("if the response bundle contains an organization resource it should contain meta d" +
                    "ata profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
  testRunner.And("the Bundle should be valid for patient \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
  testRunner.And("the Patient Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 18
  testRunner.And("the Practitioner Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 19
  testRunner.And("the Organization Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
  testRunner.And("I Check The Problems List", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
  testRunner.And("I Check The Problems List Does Not Include Not In Use Fields", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
  testRunner.And("I Check The Problems Resources are Valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 23
  testRunner.And("I check The Problem Resources Do Not Include Not In Use Fields", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
  testRunner.And("check the response does not contain an operation outcome", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve problems structured record with status partParameter expected success")]
        public virtual void RetrieveProblemsStructuredRecordWithStatusPartParameterExpectedSuccess()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve problems structured record with status partParameter expected success", null, ((string[])(null)));
#line 27
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 28
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 29
  testRunner.And("I add an NHS Number parameter for \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
  testRunner.And("I add the problems parameter with filterStatus \"active\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 31
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 32
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 33
  testRunner.And("the response should be a Bundle resource of type \"collection\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 34
  testRunner.And("the response meta profile should be for \"structured\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
  testRunner.And("the patient resource in the bundle should contain meta data profile and version i" +
                    "d", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
  testRunner.And("if the response bundle contains a practitioner resource it should contain meta da" +
                    "ta profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 37
  testRunner.And("if the response bundle contains an organization resource it should contain meta d" +
                    "ata profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 38
  testRunner.And("the Bundle should be valid for patient \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 39
  testRunner.And("the Patient Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 40
  testRunner.And("the Practitioner Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 41
  testRunner.And("the Organization Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
  testRunner.And("check the response does not contain an operation outcome", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve problems structured record with significance partParameter expected succ" +
            "ess")]
        [NUnit.Framework.TestCaseAttribute("major", null)]
        [NUnit.Framework.TestCaseAttribute("minor", null)]
        public virtual void RetrieveProblemsStructuredRecordWithSignificancePartParameterExpectedSuccess(string value, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve problems structured record with significance partParameter expected succ" +
                    "ess", null, exampleTags);
#line 52
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 53
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 54
  testRunner.And("I add an NHS Number parameter for \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 55
  testRunner.And(string.Format("I add the problems parameter with filterSignificance \"{0}\"", value), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 56
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 57
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 58
  testRunner.And("the response should be a Bundle resource of type \"collection\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
  testRunner.And("the response meta profile should be for \"structured\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 60
  testRunner.And("the patient resource in the bundle should contain meta data profile and version i" +
                    "d", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 61
  testRunner.And("if the response bundle contains a practitioner resource it should contain meta da" +
                    "ta profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 62
  testRunner.And("if the response bundle contains an organization resource it should contain meta d" +
                    "ata profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 63
  testRunner.And("the Bundle should be valid for patient \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 64
  testRunner.And("the Patient Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 65
  testRunner.And("the Practitioner Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 66
  testRunner.And("the Organization Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 71
  testRunner.And("check the response does not contain an operation outcome", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve problems structured record with status and significance partParameter ex" +
            "pected success")]
        [NUnit.Framework.TestCaseAttribute("active", "major", null)]
        [NUnit.Framework.TestCaseAttribute("active", "minor", null)]
        [NUnit.Framework.TestCaseAttribute("inactive", "major", null)]
        [NUnit.Framework.TestCaseAttribute("inactive", "minor", null)]
        public virtual void RetrieveProblemsStructuredRecordWithStatusAndSignificancePartParameterExpectedSuccess(string statusValue, string sigValue, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve problems structured record with status and significance partParameter ex" +
                    "pected success", null, exampleTags);
#line 77
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 78
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 79
  testRunner.And("I add an NHS Number parameter for \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 80
  testRunner.And(string.Format("I add the problems parameter including status and significance value \"{0}\" \"{1}\"", statusValue, sigValue), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 81
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 82
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 83
  testRunner.And("the response should be a Bundle resource of type \"collection\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 84
  testRunner.And("the response meta profile should be for \"structured\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 85
  testRunner.And("the patient resource in the bundle should contain meta data profile and version i" +
                    "d", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 86
  testRunner.And("if the response bundle contains a practitioner resource it should contain meta da" +
                    "ta profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 87
  testRunner.And("if the response bundle contains an organization resource it should contain meta d" +
                    "ata profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 88
  testRunner.And("the Bundle should be valid for patient \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 89
  testRunner.And("the Patient Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 90
  testRunner.And("the Practitioner Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 91
  testRunner.And("the Organization Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 96
  testRunner.And("check the response does not contain an operation outcome", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve problems structured record for a patient that has no problems data")]
        public virtual void RetrieveProblemsStructuredRecordForAPatientThatHasNoProblemsData()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve problems structured record for a patient that has no problems data", null, ((string[])(null)));
#line 104
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 105
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 106
  testRunner.And("I add an NHS Number parameter for \"patient4\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 107
  testRunner.And("I add the Problems parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 108
  testRunner.And("I add the medication parameter with includePrescriptionIssues set to \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 109
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 110
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 111
  testRunner.And("the response should be a Bundle resource of type \"collection\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 112
  testRunner.And("the response meta profile should be for \"structured\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 113
  testRunner.And("the patient resource in the bundle should contain meta data profile and version i" +
                    "d", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 114
  testRunner.And("if the response bundle contains a practitioner resource it should contain meta da" +
                    "ta profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 115
  testRunner.And("if the response bundle contains an organization resource it should contain meta d" +
                    "ata profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 116
  testRunner.And("the Bundle should be valid for patient \"patient4\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 117
  testRunner.And("the Patient Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 118
  testRunner.And("the Practitioner Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 119
  testRunner.And("the Organization Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 121
  testRunner.And("check the response does not contain an operation outcome", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 122
  testRunner.And("check structured list contains a note and emptyReason when no data in section", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 123
  testRunner.And("check the response does not contain an operation outcome", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve problems structured record for a patient that has repeating pair values " +
            "expected success")]
        public virtual void RetrieveProblemsStructuredRecordForAPatientThatHasRepeatingPairValuesExpectedSuccess()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve problems structured record for a patient that has repeating pair values " +
                    "expected success", null, ((string[])(null)));
#line 125
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 126
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 127
  testRunner.And("I add an NHS Number parameter for \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 128
  testRunner.And("I add the problems parameter including repeating filter pairs", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 129
  testRunner.And("I add the medication parameter with includePrescriptionIssues set to \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 130
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 131
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 132
  testRunner.And("the response should be a Bundle resource of type \"collection\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 133
  testRunner.And("the response meta profile should be for \"structured\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 134
  testRunner.And("the patient resource in the bundle should contain meta data profile and version i" +
                    "d", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 135
  testRunner.And("if the response bundle contains a practitioner resource it should contain meta da" +
                    "ta profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 136
  testRunner.And("if the response bundle contains an organization resource it should contain meta d" +
                    "ata profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 137
  testRunner.And("the Bundle should be valid for patient \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 138
  testRunner.And("the Patient Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 139
  testRunner.And("the Practitioner Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 140
  testRunner.And("the Organization Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 142
  testRunner.And("check the response does not contain an operation outcome", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve problems structured record with invalid status partParameter expected fa" +
            "ilure")]
        [NUnit.Framework.TestCaseAttribute("resolved", null)]
        [NUnit.Framework.TestCaseAttribute("recurrence", null)]
        [NUnit.Framework.TestCaseAttribute("remission", null)]
        [NUnit.Framework.TestCaseAttribute("null", null)]
        [NUnit.Framework.TestCaseAttribute("off", null)]
        public virtual void RetrieveProblemsStructuredRecordWithInvalidStatusPartParameterExpectedFailure(string value, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve problems structured record with invalid status partParameter expected fa" +
                    "ilure", null, exampleTags);
#line 144
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 145
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 146
  testRunner.And("I add an NHS Number parameter for \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 147
  testRunner.And(string.Format("I add the problems parameter with filterStatus \"{0}\"", value), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 148
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 149
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 150
  testRunner.And("the response status code should be \"422\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 151
  testRunner.And("the response should be a OperationOutcome resource with error code \"INVALID_PARAM" +
                    "ETER\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve problems structured record with invalid significance partParameter expec" +
            "ted failure")]
        [NUnit.Framework.TestCaseAttribute("HIGH", null)]
        [NUnit.Framework.TestCaseAttribute("null", null)]
        [NUnit.Framework.TestCaseAttribute("off", null)]
        public virtual void RetrieveProblemsStructuredRecordWithInvalidSignificancePartParameterExpectedFailure(string value, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve problems structured record with invalid significance partParameter expec" +
                    "ted failure", null, exampleTags);
#line 160
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 161
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 162
  testRunner.And("I add an NHS Number parameter for \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 163
  testRunner.And(string.Format("I add the problems parameter with filterSignificance \"{0}\"", value), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 164
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 165
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 166
  testRunner.And("the response status code should be \"422\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 167
  testRunner.And("the response should be a OperationOutcome resource with error code \"INVALID_PARAM" +
                    "ETER\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve problems structured record with invalid status and significance partPara" +
            "meter expected failure")]
        [NUnit.Framework.TestCaseAttribute("invalid", "invalid", null)]
        [NUnit.Framework.TestCaseAttribute("active", "low", null)]
        [NUnit.Framework.TestCaseAttribute("resolved", "major", null)]
        public virtual void RetrieveProblemsStructuredRecordWithInvalidStatusAndSignificancePartParameterExpectedFailure(string statusValue, string sigValue, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve problems structured record with invalid status and significance partPara" +
                    "meter expected failure", null, exampleTags);
#line 174
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 175
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 176
  testRunner.And("I add an NHS Number parameter for \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 177
  testRunner.And(string.Format("I add the problems parameter including status and significance value \"{0}\" \"{1}\"", statusValue, sigValue), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 178
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 179
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 180
  testRunner.And("the response status code should be \"422\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 181
  testRunner.And("the response should be a OperationOutcome resource with error code \"INVALID_PARAM" +
                    "ETER\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve Problems structured record with made up partParameter expected success")]
        public virtual void RetrieveProblemsStructuredRecordWithMadeUpPartParameterExpectedSuccess()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve Problems structured record with made up partParameter expected success", null, ((string[])(null)));
#line 188
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 189
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 190
  testRunner.And("I add an NHS Number parameter for \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 191
  testRunner.And("I add a madeUpProblems part parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 192
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 193
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 194
  testRunner.And("Check the operation outcome returns the correct text and diagnotics \"madeUpProble" +
                    "ms\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 195
  testRunner.And("Check the number of issues in the operation outcome \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve consultations structured record section for an invalid NHS number")]
        public virtual void RetrieveConsultationsStructuredRecordSectionForAnInvalidNHSNumber()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve consultations structured record section for an invalid NHS number", null, ((string[])(null)));
#line 197
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 198
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 199
  testRunner.And("I add an NHS Number parameter for an invalid NHS Number", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 200
  testRunner.And("I add the Problems parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 201
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 202
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 203
  testRunner.And("the response should be a OperationOutcome resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve consultations structured record section for an empty NHS number")]
        public virtual void RetrieveConsultationsStructuredRecordSectionForAnEmptyNHSNumber()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve consultations structured record section for an empty NHS number", null, ((string[])(null)));
#line 205
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 206
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 207
  testRunner.And("I add an NHS Number parameter with an empty NHS Number", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 208
  testRunner.And("I add the Problems parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 209
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 210
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 211
  testRunner.And("the response should be a OperationOutcome resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve consultations structured record section for an invalid Identifier System" +
            "")]
        public virtual void RetrieveConsultationsStructuredRecordSectionForAnInvalidIdentifierSystem()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve consultations structured record section for an invalid Identifier System" +
                    "", null, ((string[])(null)));
#line 213
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 214
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 215
  testRunner.And("I add an NHS Number parameter for \"patient1\" with an invalid Identifier System", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 216
  testRunner.And("I add the Problems parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 217
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 218
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 219
  testRunner.And("the response should be a OperationOutcome resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve consultations structured record section for an empty Identifier System")]
        public virtual void RetrieveConsultationsStructuredRecordSectionForAnEmptyIdentifierSystem()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve consultations structured record section for an empty Identifier System", null, ((string[])(null)));
#line 221
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 222
testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 223
  testRunner.And("I add an NHS Number parameter for \"patient1\" with an empty Identifier System", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 224
  testRunner.And("I add the Problems parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 225
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 226
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 227
  testRunner.And("the response should be a OperationOutcome resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve consultations structured record for a patient that has sensitive flag")]
        public virtual void RetrieveConsultationsStructuredRecordForAPatientThatHasSensitiveFlag()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve consultations structured record for a patient that has sensitive flag", null, ((string[])(null)));
#line 229
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 230
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 231
  testRunner.And("I add an NHS Number parameter for \"patient9\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 232
  testRunner.And("I add the Problems parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 233
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 234
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 235
  testRunner.And("the response status code should be \"404\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 236
  testRunner.And("the response should be a OperationOutcome resource with error code \"PATIENT_NOT_F" +
                    "OUND\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion