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
    [NUnit.Framework.DescriptionAttribute("StructuredUncategorised")]
    [NUnit.Framework.CategoryAttribute("structuredrecord")]
    public partial class StructuredUncategorisedFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "StructuredUncategorised.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "StructuredUncategorised", null, ProgrammingLanguage.CSharp, new string[] {
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
        [NUnit.Framework.DescriptionAttribute("Verify Uncategorised Data structured record for a Patient with Uncategorised")]
        [NUnit.Framework.CategoryAttribute("1.3.1")]
        [NUnit.Framework.TestCaseAttribute("patient2", null)]
        public virtual void VerifyUncategorisedDataStructuredRecordForAPatientWithUncategorised(string patient, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "1.3.1"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify Uncategorised Data structured record for a Patient with Uncategorised", null, @__tags);
#line 5
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 6
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 7
  testRunner.And(string.Format("I add an NHS Number parameter for \"{0}\"", patient), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 8
  testRunner.And("I add the uncategorised data parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
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
  testRunner.And(string.Format("the Bundle should be valid for patient \"{0}\"", patient), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
  testRunner.And("the Patient Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 18
  testRunner.And("the Practitioner Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 19
  testRunner.And("the Organization Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
  testRunner.And("The Observation Resources are Valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
  testRunner.And("The Observation Resources Do Not Include Not In Use Fields", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
  testRunner.And("the Bundle should contain \"1\" lists", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 23
  testRunner.And("The Observation List is Valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
  testRunner.And("The Structured List Does Not Include Not In Use Fields", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 25
  testRunner.And("check the response does not contain an operation outcome", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve uncategorised data structured record section for an invalid NHS number")]
        [NUnit.Framework.CategoryAttribute("1.3.1")]
        public virtual void RetrieveUncategorisedDataStructuredRecordSectionForAnInvalidNHSNumber()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve uncategorised data structured record section for an invalid NHS number", null, new string[] {
                        "1.3.1"});
#line 31
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 32
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 33
  testRunner.And("I add an NHS Number parameter for an invalid NHS Number", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 34
  testRunner.And("I add the uncategorised data parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 36
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 37
  testRunner.And("the response should be a OperationOutcome resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve uncategorised data structured record section for an empty NHS number")]
        [NUnit.Framework.CategoryAttribute("1.3.1")]
        public virtual void RetrieveUncategorisedDataStructuredRecordSectionForAnEmptyNHSNumber()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve uncategorised data structured record section for an empty NHS number", null, new string[] {
                        "1.3.1"});
#line 40
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 41
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 42
  testRunner.And("I add an NHS Number parameter with an empty NHS Number", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 43
  testRunner.And("I add the uncategorised data parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 45
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 46
  testRunner.And("the response should be a OperationOutcome resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve uncategorised data structured record section for an invalid Identifier S" +
            "ystem")]
        [NUnit.Framework.CategoryAttribute("1.3.1")]
        public virtual void RetrieveUncategorisedDataStructuredRecordSectionForAnInvalidIdentifierSystem()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve uncategorised data structured record section for an invalid Identifier S" +
                    "ystem", null, new string[] {
                        "1.3.1"});
#line 49
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 50
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 51
  testRunner.And("I add an NHS Number parameter for \"patient1\" with an invalid Identifier System", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 52
  testRunner.And("I add the uncategorised data parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 53
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 54
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 55
  testRunner.And("the response should be a OperationOutcome resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve uncategorised data structured record section for an empty Identifier Sys" +
            "tem")]
        [NUnit.Framework.CategoryAttribute("1.3.1")]
        public virtual void RetrieveUncategorisedDataStructuredRecordSectionForAnEmptyIdentifierSystem()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve uncategorised data structured record section for an empty Identifier Sys" +
                    "tem", null, new string[] {
                        "1.3.1"});
#line 58
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 59
testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 60
  testRunner.And("I add an NHS Number parameter for \"patient1\" with an empty Identifier System", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 61
  testRunner.And("I add the uncategorised data parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 62
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 63
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 64
  testRunner.And("the response should be a OperationOutcome resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve uncategorised data structured record for a patient that has sensitive fl" +
            "ag")]
        [NUnit.Framework.CategoryAttribute("1.3.1")]
        public virtual void RetrieveUncategorisedDataStructuredRecordForAPatientThatHasSensitiveFlag()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve uncategorised data structured record for a patient that has sensitive fl" +
                    "ag", null, new string[] {
                        "1.3.1"});
#line 67
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 68
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 69
 testRunner.And("I add an NHS Number parameter for \"patient9\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 70
 testRunner.And("I add the uncategorised data parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 71
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 72
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 73
  testRunner.And("the response should be a OperationOutcome resource with error code \"PATIENT_NOT_F" +
                    "OUND\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve uncategorised data structured record for a patient that has no uncategor" +
            "ised data")]
        [NUnit.Framework.CategoryAttribute("1.3.1")]
        public virtual void RetrieveUncategorisedDataStructuredRecordForAPatientThatHasNoUncategorisedData()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve uncategorised data structured record for a patient that has no uncategor" +
                    "ised data", null, new string[] {
                        "1.3.1"});
#line 76
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 77
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 78
  testRunner.And("I add an NHS Number parameter for \"patient4\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 79
  testRunner.And("I add the uncategorised data parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 80
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 81
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 82
  testRunner.And("the response should be a Bundle resource of type \"collection\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 83
  testRunner.And("the response meta profile should be for \"structured\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 84
  testRunner.And("the patient resource in the bundle should contain meta data profile and version i" +
                    "d", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 85
  testRunner.And("if the response bundle contains a practitioner resource it should contain meta da" +
                    "ta profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 86
  testRunner.And("if the response bundle contains an organization resource it should contain meta d" +
                    "ata profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 87
  testRunner.And("the Bundle should be valid for patient \"patient4\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 88
  testRunner.And("the Patient Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 89
  testRunner.And("the Practitioner Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 90
  testRunner.And("the Organization Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 91
  testRunner.And("check the response does not contain an operation outcome", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 92
  testRunner.And("check structured list contains a note and emptyReason when no data in section", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve the uncategorised data structured record with invalid dates expected suc" +
            "cess to include an operation outcome")]
        [NUnit.Framework.CategoryAttribute("1.3.1")]
        [NUnit.Framework.TestCaseAttribute("2014", "2016-01-01", "includeUncategorisedData", "uncategorisedDataSearchPeriod", null)]
        [NUnit.Framework.TestCaseAttribute("2014-02", "2014-08-20", "includeUncategorisedData", "uncategorisedDataSearchPeriod", null)]
        [NUnit.Framework.TestCaseAttribute("2015-10-23T11:08:32", "2016-11-01", "includeUncategorisedData", "uncategorisedDataSearchPeriod", null)]
        [NUnit.Framework.TestCaseAttribute("2015-10-23T11:08:32+00:00", "2019-10-01", "includeUncategorisedData", "uncategorisedDataSearchPeriod", null)]
        [NUnit.Framework.TestCaseAttribute("2014-01-01", "2016", "includeUncategorisedData", "uncategorisedDataSearchPeriod", null)]
        [NUnit.Framework.TestCaseAttribute("2014-02-01", "2014-08", "includeUncategorisedData", "uncategorisedDataSearchPeriod", null)]
        [NUnit.Framework.TestCaseAttribute("2015-10-01", "2016-11-23T11:08:32", "includeUncategorisedData", "uncategorisedDataSearchPeriod", null)]
        [NUnit.Framework.TestCaseAttribute("2014-01-01", "2015-10-23T11:08:32+00:00", "includeUncategorisedData", "uncategorisedDataSearchPeriod", null)]
        public virtual void RetrieveTheUncategorisedDataStructuredRecordWithInvalidDatesExpectedSuccessToIncludeAnOperationOutcome(string startDate, string endDate, string parameter, string partParameter, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "1.3.1"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve the uncategorised data structured record with invalid dates expected suc" +
                    "cess to include an operation outcome", null, @__tags);
#line 95
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 96
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 97
  testRunner.And("I add an NHS Number parameter for \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 98
  testRunner.And(string.Format("I add the uncategorised data parameter with date permutations \"{0}\" and \"{1}\"", startDate, endDate), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 99
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 100
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 101
  testRunner.And(string.Format("Check the operation outcome returns INVALID_PARAMETER for \"{0}\" and \"{1}\"", parameter, partParameter), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 102
  testRunner.And("Check the number of issues in the operation outcome \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve the uncategorised data structured record with start date in future expec" +
            "ted success with operation outcome")]
        [NUnit.Framework.CategoryAttribute("1.3.1")]
        [NUnit.Framework.TestCaseAttribute("includeUncategorisedData", "uncategorisedDataSearchPeriod", null)]
        public virtual void RetrieveTheUncategorisedDataStructuredRecordWithStartDateInFutureExpectedSuccessWithOperationOutcome(string parameter, string partParameter, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "1.3.1"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve the uncategorised data structured record with start date in future expec" +
                    "ted success with operation outcome", null, @__tags);
#line 116
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 117
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 118
  testRunner.And("I add an NHS Number parameter for \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 119
  testRunner.And("I add the uncategorised data parameter with future start date", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 120
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 121
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 122
  testRunner.And(string.Format("Check the operation outcome returns INVALID_PARAMETER for \"{0}\" and \"{1}\"", parameter, partParameter), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 123
  testRunner.And("Check the number of issues in the operation outcome \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve the uncategorised data structured record with period dates equal to curr" +
            "ent date expected success no operation outcome")]
        [NUnit.Framework.CategoryAttribute("1.3.1")]
        public virtual void RetrieveTheUncategorisedDataStructuredRecordWithPeriodDatesEqualToCurrentDateExpectedSuccessNoOperationOutcome()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve the uncategorised data structured record with period dates equal to curr" +
                    "ent date expected success no operation outcome", null, new string[] {
                        "1.3.1"});
#line 130
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 131
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 132
  testRunner.And("I add an NHS Number parameter for \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 133
  testRunner.And("I add the uncategorised data parameter with current date", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 134
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 135
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 136
  testRunner.And("check the response does not contain an operation outcome", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve the uncategorised data structured record startDate after endDate expecte" +
            "d success with operation outcome")]
        [NUnit.Framework.CategoryAttribute("1.3.1")]
        public virtual void RetrieveTheUncategorisedDataStructuredRecordStartDateAfterEndDateExpectedSuccessWithOperationOutcome()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve the uncategorised data structured record startDate after endDate expecte" +
                    "d success with operation outcome", null, new string[] {
                        "1.3.1"});
#line 139
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 140
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 141
  testRunner.And("I add an NHS Number parameter for \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 142
  testRunner.And("I add the uncategorised data parameter start date after endDate", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 143
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 144
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 145
  testRunner.And("Check the operation outcome returns INVALID_PARAMETER for \"includeUncategorisedDa" +
                    "ta\" and \"uncategorisedDataSearchPeriod\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 146
  testRunner.And("Check the number of issues in the operation outcome \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
