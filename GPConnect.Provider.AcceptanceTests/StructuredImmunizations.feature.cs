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
    [NUnit.Framework.DescriptionAttribute("StructuredImmunizations")]
    [NUnit.Framework.CategoryAttribute("Structured")]
    [NUnit.Framework.CategoryAttribute("StructuredImmunizations")]
    [NUnit.Framework.CategoryAttribute("1.6.0-Full-Pack")]
    public partial class StructuredImmunizationsFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "StructuredImmunizations.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "StructuredImmunizations", null, ProgrammingLanguage.CSharp, new string[] {
                        "Structured",
                        "StructuredImmunizations",
                        "1.6.0-Full-Pack"});
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
        [NUnit.Framework.DescriptionAttribute("Verify Immunizations structured record for a Patient with Immunizations not linke" +
            "d to any problems")]
        public virtual void VerifyImmunizationsStructuredRecordForAPatientWithImmunizationsNotLinkedToAnyProblems()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify Immunizations structured record for a Patient with Immunizations not linke" +
                    "d to any problems", null, ((string[])(null)));
#line 8
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 9
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 10
  testRunner.And("I add an NHS Number parameter for \"patient3\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
  testRunner.And("I add the immunizations parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
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
  testRunner.And("the Patient Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
  testRunner.And("the Practitioner Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
  testRunner.And("the Organization Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 23
  testRunner.And("The Immunization Resources are Valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
  testRunner.And("The Immunization Resources Do Not Include Not In Use Fields", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 25
  testRunner.And("the Bundle should contain \"1\" lists", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 26
  testRunner.And("The Immunization List is Valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
  testRunner.And("The Structured List Does Not Include Not In Use Fields", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 28
  testRunner.And("I Check There is No Problems Secondary Problems List", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 29
  testRunner.And("I Check No Problem Resources are Included", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Verify Immunizations structured record for a Patient with Immunizations associate" +
            "d to Problems")]
        public virtual void VerifyImmunizationsStructuredRecordForAPatientWithImmunizationsAssociatedToProblems()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verify Immunizations structured record for a Patient with Immunizations associate" +
                    "d to Problems", null, ((string[])(null)));
#line 33
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 34
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 35
  testRunner.And("I add an NHS Number parameter for \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
  testRunner.And("I add the immunizations parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 37
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 38
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 39
  testRunner.And("the response should be a Bundle resource of type \"collection\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 40
  testRunner.And("the response meta profile should be for \"structured\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 41
  testRunner.And("the patient resource in the bundle should contain meta data profile and version i" +
                    "d", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 42
  testRunner.And("if the response bundle contains a practitioner resource it should contain meta da" +
                    "ta profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 43
  testRunner.And("if the response bundle contains an organization resource it should contain meta d" +
                    "ata profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
  testRunner.And("the Bundle should be valid for patient \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 45
  testRunner.And("check that the bundle does not contain any duplicate resources", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
  testRunner.And("the Patient Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 47
  testRunner.And("the Practitioner Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
  testRunner.And("the Organization Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 49
  testRunner.And("The Immunization Resources are Valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
  testRunner.And("The Immunization Resources Do Not Include Not In Use Fields", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 51
  testRunner.And("the Bundle should contain \"2\" lists", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 52
  testRunner.And("The Immunization List is Valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 53
  testRunner.And("The Structured List Does Not Include Not In Use Fields", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 54
  testRunner.And("check the response does not contain an operation outcome", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 55
  testRunner.And("I Check The Problems Secondary Problems List", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 56
  testRunner.And("I Check The Problems Secondary Problems List Does Not Include Not In Use Fields", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 57
  testRunner.And("I Check The Problems Resources are Valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
  testRunner.And("I check The Problem Resources Do Not Include Not In Use Fields", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
  testRunner.And("Check a Problem is linked to an \"Immunization\" that is also included in the respo" +
                    "nse with its list", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve the immunizations structured record section for an invalid NHS number")]
        public virtual void RetrieveTheImmunizationsStructuredRecordSectionForAnInvalidNHSNumber()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve the immunizations structured record section for an invalid NHS number", null, ((string[])(null)));
#line 61
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 62
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 63
  testRunner.And("I add an NHS Number parameter for an invalid NHS Number", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 64
  testRunner.And("I add the immunizations parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 65
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 66
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 67
  testRunner.And("the response should be a OperationOutcome resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve the immunizations structured record section for an empty NHS number")]
        public virtual void RetrieveTheImmunizationsStructuredRecordSectionForAnEmptyNHSNumber()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve the immunizations structured record section for an empty NHS number", null, ((string[])(null)));
#line 69
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 70
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 71
  testRunner.And("I add an NHS Number parameter with an empty NHS Number", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 72
  testRunner.And("I add the immunizations parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 73
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 74
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 75
  testRunner.And("the response should be a OperationOutcome resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve the immunizations structured record section for an invalid Identifier Sy" +
            "stem")]
        public virtual void RetrieveTheImmunizationsStructuredRecordSectionForAnInvalidIdentifierSystem()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve the immunizations structured record section for an invalid Identifier Sy" +
                    "stem", null, ((string[])(null)));
#line 77
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 78
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 79
  testRunner.And("I add an NHS Number parameter for \"patient1\" with an invalid Identifier System", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 80
  testRunner.And("I add the immunizations parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 81
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 82
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 83
  testRunner.And("the response should be a OperationOutcome resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve the immunizations structured record section for an empty Identifier Syst" +
            "em")]
        public virtual void RetrieveTheImmunizationsStructuredRecordSectionForAnEmptyIdentifierSystem()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve the immunizations structured record section for an empty Identifier Syst" +
                    "em", null, ((string[])(null)));
#line 85
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 86
testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 87
  testRunner.And("I add an NHS Number parameter for \"patient1\" with an empty Identifier System", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 88
  testRunner.And("I add the immunizations parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 89
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 90
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 91
  testRunner.And("the response should be a OperationOutcome resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve the immunizations structured record for a patient that has sensitive fla" +
            "g")]
        public virtual void RetrieveTheImmunizationsStructuredRecordForAPatientThatHasSensitiveFlag()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve the immunizations structured record for a patient that has sensitive fla" +
                    "g", null, ((string[])(null)));
#line 93
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 94
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 95
  testRunner.And("I add an NHS Number parameter for \"patient9\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 96
  testRunner.And("I add the immunizations parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 97
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 98
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 99
  testRunner.And("the response status code should be \"404\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 100
  testRunner.And("the response should be a OperationOutcome resource with error code \"PATIENT_NOT_F" +
                    "OUND\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve the immunizations structured record for a patient that has no immunizati" +
            "ons")]
        public virtual void RetrieveTheImmunizationsStructuredRecordForAPatientThatHasNoImmunizations()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve the immunizations structured record for a patient that has no immunizati" +
                    "ons", null, ((string[])(null)));
#line 102
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 103
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 104
  testRunner.And("I add an NHS Number parameter for \"patient4\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 105
  testRunner.And("I add the immunizations parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 106
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 107
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 108
  testRunner.And("the response should be a Bundle resource of type \"collection\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 109
  testRunner.And("the response meta profile should be for \"structured\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 110
  testRunner.And("the patient resource in the bundle should contain meta data profile and version i" +
                    "d", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 111
  testRunner.And("if the response bundle contains a practitioner resource it should contain meta da" +
                    "ta profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 112
  testRunner.And("if the response bundle contains an organization resource it should contain meta d" +
                    "ata profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 113
  testRunner.And("the Bundle should be valid for patient \"patient4\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 114
     testRunner.And("the Patient Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 115
  testRunner.And("the Practitioner Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 116
  testRunner.And("the Organization Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 117
  testRunner.And("check the response does not contain an operation outcome", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 118
  testRunner.And("check structured list contains a note and emptyReason when no data in section", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve the immunizations structured record with a madeUp partParameter")]
        public virtual void RetrieveTheImmunizationsStructuredRecordWithAMadeUpPartParameter()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve the immunizations structured record with a madeUp partParameter", null, ((string[])(null)));
#line 120
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 121
 testRunner.Given("I configure the default \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 122
  testRunner.And("I add an NHS Number parameter for \"patient2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 123
  testRunner.And("I add a madeUp immunizations part parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 124
 testRunner.When("I make the \"GpcGetStructuredRecord\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 125
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 126
  testRunner.And("Check the operation outcome returns the correct text and diagnostics includes \"in" +
                    "cludeImmunisations\" and \"madeUp\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 127
  testRunner.And("Check the number of issues in the operation outcome \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
