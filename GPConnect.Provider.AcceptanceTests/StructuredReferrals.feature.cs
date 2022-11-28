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
    [NUnit.Framework.DescriptionAttribute("StructuredReferrals")]
    [NUnit.Framework.CategoryAttribute("Structured")]
    [NUnit.Framework.CategoryAttribute("StructuredReferrals")]
    [NUnit.Framework.CategoryAttribute("1.5.0-Full-Pack")]
    public partial class StructuredReferralsFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "StructuredReferrals.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "StructuredReferrals", null, ProgrammingLanguage.CSharp, new string[] {
                        "Structured",
                        "StructuredReferrals",
                        "1.5.0-Full-Pack"});
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
        [NUnit.Framework.DescriptionAttribute("Verify Referrals structured record for a Patient with Referrals not linked to any" +
            " problems")]
        [NUnit.Framework.CategoryAttribute("1.5.0-IncrementalAndRegression")]
        public virtual void VerifyReferralsStructuredRecordForAPatientWithReferralsNotLinkedToAnyProblems()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify Referrals structured record for a Patient with Referrals not linked to any" +
                    " problems", null, new string[] {
                        "1.5.0-IncrementalAndRegression"});
#line 8
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 9
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 10
  testRunner.And("I add an NHS Number parameter for \"patient3\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
  testRunner.And("I add the Referrals parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 13
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 14
  testRunner.And("the response should be a Bundle resource of type \"collection\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
  testRunner.And("the response meta profile should be for \"structured\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
  testRunner.And("the patient resource in the bundle should contain meta data profile and version i" +
                    "d", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
  testRunner.And("if the response bundle contains a practitioner resource it should contain meta da" +
                    "ta profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 18
  testRunner.And("if the response bundle contains an organization resource it should contain meta d" +
                    "ata profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 19
  testRunner.And("the Bundle should be valid for patient \"patient3\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
     testRunner.And("check that the bundle does not contain any duplicate resources", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
  testRunner.And("check the response does not contain an operation outcome", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
  testRunner.And("the Patient Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 23
  testRunner.And("the Practitioner Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
  testRunner.And("the Organization Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 25
  testRunner.And("the Bundle should contain \"1\" lists", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 26
  testRunner.And("I Check the Referrals List is Valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
  testRunner.And("The Structured List Does Not Include Not In Use Fields", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 28
  testRunner.And("I Check the ReferralRequests are Valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 29
  testRunner.And("I Check the ReferralRequests Do Not Include Not in Use Fields", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
  testRunner.And("I Check There is No Problems Secondary Problems List", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 31
  testRunner.And("I Check No Problem Resources are Included", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Verify Referrals structured record for a Patient with Referrals associated to Pro" +
            "blems")]
        [NUnit.Framework.CategoryAttribute("1.5.0-IncrementalAndRegression")]
        public virtual void VerifyReferralsStructuredRecordForAPatientWithReferralsAssociatedToProblems()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify Referrals structured record for a Patient with Referrals associated to Pro" +
                    "blems", null, new string[] {
                        "1.5.0-IncrementalAndRegression"});
#line 34
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 35
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 36
  testRunner.And("I add an NHS Number parameter for \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 37
  testRunner.And("I add the Referrals parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 38
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 39
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 40
  testRunner.And("the response should be a Bundle resource of type \"collection\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 41
  testRunner.And("the response meta profile should be for \"structured\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 42
  testRunner.And("the patient resource in the bundle should contain meta data profile and version i" +
                    "d", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 43
  testRunner.And("if the response bundle contains a practitioner resource it should contain meta da" +
                    "ta profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
  testRunner.And("if the response bundle contains an organization resource it should contain meta d" +
                    "ata profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 45
  testRunner.And("the Bundle should be valid for patient \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
     testRunner.And("check that the bundle does not contain any duplicate resources", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 47
  testRunner.And("check the response does not contain an operation outcome", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
  testRunner.And("the Patient Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 49
  testRunner.And("the Practitioner Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
  testRunner.And("the Organization Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 51
  testRunner.And("I Check the Referrals List is Valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 52
  testRunner.And("The Structured List Does Not Include Not In Use Fields", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 53
  testRunner.And("I Check the ReferralRequests are Valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 54
  testRunner.And("I Check the ReferralRequests Do Not Include Not in Use Fields", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 55
  testRunner.And("I Check The Problems Secondary Problems List", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 56
  testRunner.And("I Check The Problems Secondary Problems List Does Not Include Not In Use Fields", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 57
  testRunner.And("I Check The Problems Resources are Valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
  testRunner.And("I check The Problem Resources Do Not Include Not In Use Fields", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
  testRunner.And("the Bundle should contain \"2\" lists", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 60
  testRunner.And("Check a Problem is linked to ReferralRequest and that it is also included", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve Referrals structured record for a patient that has no Referrals data")]
        [NUnit.Framework.CategoryAttribute("1.5.0-IncrementalAndRegression")]
        public virtual void RetrieveReferralsStructuredRecordForAPatientThatHasNoReferralsData()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve Referrals structured record for a patient that has no Referrals data", null, new string[] {
                        "1.5.0-IncrementalAndRegression"});
#line 63
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 64
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 65
  testRunner.And("I add an NHS Number parameter for \"patient4\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 66
  testRunner.And("I add the Referrals parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 68
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 69
  testRunner.And("the response should be a Bundle resource of type \"collection\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 70
  testRunner.And("the response meta profile should be for \"structured\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 71
  testRunner.And("the patient resource in the bundle should contain meta data profile and version i" +
                    "d", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 72
  testRunner.And("if the response bundle contains a practitioner resource it should contain meta da" +
                    "ta profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 73
  testRunner.And("if the response bundle contains an organization resource it should contain meta d" +
                    "ata profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 74
  testRunner.And("the Bundle should be valid for patient \"patient4\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 75
  testRunner.And("the Patient Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 76
  testRunner.And("the Practitioner Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
  testRunner.And("the Organization Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 78
  testRunner.And("check structured list contains a note and emptyReason when no data in section", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 79
  testRunner.And("check the response does not contain an operation outcome", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Patient with Referrals Has Warnings and Associated Notes")]
        [NUnit.Framework.CategoryAttribute("1.5.0-IncrementalAndRegression")]
        [NUnit.Framework.TestCaseAttribute("patient16", "confidential-items", "Items excluded due to confidentiality and/or patient preferences.", null)]
        [NUnit.Framework.TestCaseAttribute("patient13", "data-in-transit", "Patient record transfer from previous GP practice not yet complete; information r" +
            "ecorded before dd-Mmm-yyyy may be missing.", null)]
        public virtual void PatientWithReferralsHasWarningsAndAssociatedNotes(string patient, string warning, string note, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "1.5.0-IncrementalAndRegression"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Patient with Referrals Has Warnings and Associated Notes", null, @__tags);
#line 83
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 84
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 85
  testRunner.And(string.Format("I add an NHS Number parameter for \"{0}\"", patient), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 86
  testRunner.And("I add the Referrals parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 87
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 88
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 89
  testRunner.And("the Bundle should contain \"1\" lists", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 90
  testRunner.And(string.Format("Check the list contains the following warning \"{0}\"", warning), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 91
  testRunner.And(string.Format("Check the warning \"{0}\" has associated note \"{1}\"", warning, note), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve the Referrals data structured record with period dates equal to current " +
            "date expected success and no operation outcome")]
        [NUnit.Framework.CategoryAttribute("1.5.0-IncrementalAndRegression")]
        public virtual void RetrieveTheReferralsDataStructuredRecordWithPeriodDatesEqualToCurrentDateExpectedSuccessAndNoOperationOutcome()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve the Referrals data structured record with period dates equal to current " +
                    "date expected success and no operation outcome", null, new string[] {
                        "1.5.0-IncrementalAndRegression"});
#line 98
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 99
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 100
  testRunner.And("I add an NHS Number parameter for \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 101
  testRunner.And("I add the Referrals data parameter with current date", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 102
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 103
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 104
  testRunner.And("check the response does not contain an operation outcome", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve Referrals data structured record section for an invalid Identifier Syste" +
            "m")]
        [NUnit.Framework.CategoryAttribute("1.5.0-IncrementalAndRegression")]
        public virtual void RetrieveReferralsDataStructuredRecordSectionForAnInvalidIdentifierSystem()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve Referrals data structured record section for an invalid Identifier Syste" +
                    "m", null, new string[] {
                        "1.5.0-IncrementalAndRegression"});
#line 107
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 108
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 109
  testRunner.And("I add an NHS Number parameter for \"patient1\" with an invalid Identifier System", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 110
  testRunner.And("I add the Referrals parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 111
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 112
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 113
  testRunner.And("the response should be a OperationOutcome resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve Referrals data structured record section for an empty Identifier System")]
        [NUnit.Framework.CategoryAttribute("1.5.0-IncrementalAndRegression")]
        public virtual void RetrieveReferralsDataStructuredRecordSectionForAnEmptyIdentifierSystem()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve Referrals data structured record section for an empty Identifier System", null, new string[] {
                        "1.5.0-IncrementalAndRegression"});
#line 116
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 117
testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 118
  testRunner.And("I add an NHS Number parameter for \"patient1\" with an empty Identifier System", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 119
  testRunner.And("I add the Referrals parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 120
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 121
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 122
  testRunner.And("the response should be a OperationOutcome resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve Referrals data structured record for a patient that has sensitive flag")]
        [NUnit.Framework.CategoryAttribute("1.5.0-IncrementalAndRegression")]
        public virtual void RetrieveReferralsDataStructuredRecordForAPatientThatHasSensitiveFlag()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve Referrals data structured record for a patient that has sensitive flag", null, new string[] {
                        "1.5.0-IncrementalAndRegression"});
#line 125
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 126
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 127
  testRunner.And("I add an NHS Number parameter for \"patient9\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 128
  testRunner.And("I add the Referrals parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 129
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 130
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 131
  testRunner.And("the response status code should be \"404\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 133
  testRunner.And("the response should be a OperationOutcome resource with error code \"PATIENT_NOT_F" +
                    "OUND\" and display \"Patient not found\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve the Referrals data structured record with invalid dates expected failure" +
            "")]
        [NUnit.Framework.CategoryAttribute("1.5.0-IncrementalAndRegression")]
        [NUnit.Framework.TestCaseAttribute("2014", "2016-01-01", "includeReferrals", "referralSearchPeriod", null)]
        [NUnit.Framework.TestCaseAttribute("2014-02", "2014-08-20", "includeReferrals", "referralSearchPeriod", null)]
        [NUnit.Framework.TestCaseAttribute("2015-10-23T11:08:32", "2016-11-01", "includeReferrals", "referralSearchPeriod", null)]
        [NUnit.Framework.TestCaseAttribute("2015-10-23T11:08:32+00:00", "2019-10-01", "includeReferrals", "referralSearchPeriod", null)]
        [NUnit.Framework.TestCaseAttribute("2014-01-01", "2016", "includeReferrals", "referralSearchPeriod", null)]
        [NUnit.Framework.TestCaseAttribute("2014-02-01", "2014-08", "includeReferrals", "referralSearchPeriod", null)]
        [NUnit.Framework.TestCaseAttribute("2015-10-01", "2016-11-23T11:08:32", "includeReferrals", "referralSearchPeriod", null)]
        [NUnit.Framework.TestCaseAttribute("2014-01-01", "2015-10-23T11:08:32+00:00", "includeReferrals", "referralSearchPeriod", null)]
        public virtual void RetrieveTheReferralsDataStructuredRecordWithInvalidDatesExpectedFailure(string startDate, string endDate, string parameter, string partParameter, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "1.5.0-IncrementalAndRegression"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve the Referrals data structured record with invalid dates expected failure" +
                    "", null, @__tags);
#line 136
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 137
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 138
  testRunner.And("I add an NHS Number parameter for \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 139
  testRunner.And(string.Format("I add the Referrals data parameter with date permutations \"{0}\" and \"{1}\"", startDate, endDate), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 140
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 141
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 142
  testRunner.And("the response status code should be \"422\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 143
  testRunner.And("the response should be a OperationOutcome resource with error code \"INVALID_PARAM" +
                    "ETER\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve the Referrals data structured record with referralSearchPeriod in future" +
            " expected failure")]
        [NUnit.Framework.CategoryAttribute("1.5.0-IncrementalAndRegression")]
        public virtual void RetrieveTheReferralsDataStructuredRecordWithReferralSearchPeriodInFutureExpectedFailure()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve the Referrals data structured record with referralSearchPeriod in future" +
                    " expected failure", null, new string[] {
                        "1.5.0-IncrementalAndRegression"});
#line 156
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 157
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 158
  testRunner.And("I add an NHS Number parameter for \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 159
  testRunner.And("I add the Referrals data parameter with future start date", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 160
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 161
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 162
  testRunner.And("the response status code should be \"422\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 163
  testRunner.And("the response should be a OperationOutcome resource with error code \"INVALID_PARAM" +
                    "ETER\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve the Referrals data structured record startDate after endDate expected fa" +
            "ilure")]
        [NUnit.Framework.CategoryAttribute("1.5.0-IncrementalAndRegression")]
        public virtual void RetrieveTheReferralsDataStructuredRecordStartDateAfterEndDateExpectedFailure()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve the Referrals data structured record startDate after endDate expected fa" +
                    "ilure", null, new string[] {
                        "1.5.0-IncrementalAndRegression"});
#line 166
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 167
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 168
  testRunner.And("I add an NHS Number parameter for \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 169
  testRunner.And("I add the Referrals data parameter start date after endDate", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 170
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 171
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 172
  testRunner.Then("the response status code should be \"422\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 173
  testRunner.And("the response should be a OperationOutcome resource with error code \"INVALID_PARAM" +
                    "ETER\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
