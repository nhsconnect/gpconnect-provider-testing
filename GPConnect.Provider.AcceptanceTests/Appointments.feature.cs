﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.1.0.0
//      SpecFlow Generator Version:2.0.0.0
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
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("SpecFlowFeature1")]
    public partial class SpecFlowFeature1Feature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Appointments.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "SpecFlowFeature1", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
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
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Appointment retrieve success valid id")]
        [NUnit.Framework.CategoryAttribute("Appointment")]
        [NUnit.Framework.TestCaseAttribute("1", new string[0])]
        [NUnit.Framework.TestCaseAttribute("2", new string[0])]
        [NUnit.Framework.TestCaseAttribute("400000", new string[0])]
        public virtual void AppointmentRetrieveSuccessValidId(string id, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "Appointment"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Appointment retrieve success valid id", @__tags);
#line 4
this.ScenarioSetup(scenarioInfo);
#line 5
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:search:patient_ap" +
                    "pointments\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 7
 testRunner.When(string.Format("I make a GET request to \"/Patient/{0}/Appointment\"", id), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 8
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 9
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
  testRunner.And("the JSON response should be a Bundle resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
  testRunner.And("the JSON response bundle should be type searchset", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Appointment retrieve fail invalid id")]
        [NUnit.Framework.TestCaseAttribute("**", new string[0])]
        [NUnit.Framework.TestCaseAttribute("dd", new string[0])]
        [NUnit.Framework.TestCaseAttribute("", new string[0])]
        [NUnit.Framework.TestCaseAttribute("null", new string[0])]
        public virtual void AppointmentRetrieveFailInvalidId(string id, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Appointment retrieve fail invalid id", exampleTags);
#line 19
this.ScenarioSetup(scenarioInfo);
#line 20
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 21
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:search:patient_ap" +
                    "pointments\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
 testRunner.When(string.Format("I make a GET request to \"/Patient/{0}/Appointment\"", id), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 23
 testRunner.Then("the response status code should be \"400\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 24
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Appointment retrieve failure due to missing header")]
        [NUnit.Framework.TestCaseAttribute("Ssp-TraceID", "1", new string[0])]
        [NUnit.Framework.TestCaseAttribute("Ssp-From", "1", new string[0])]
        [NUnit.Framework.TestCaseAttribute("Ssp-To", "1", new string[0])]
        [NUnit.Framework.TestCaseAttribute("Ssp-InteractionId", "1", new string[0])]
        [NUnit.Framework.TestCaseAttribute("Authorization", "1", new string[0])]
        public virtual void AppointmentRetrieveFailureDueToMissingHeader(string header, string id, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Appointment retrieve failure due to missing header", exampleTags);
#line 32
this.ScenarioSetup(scenarioInfo);
#line 33
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 34
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:search:patient_ap" +
                    "pointments\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
  testRunner.And(string.Format("I do not send header \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
 testRunner.When(string.Format("I make a GET request to \"/Patient/{0}/Appointment\"", id), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 37
 testRunner.Then("the response status code should be \"400\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 38
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 39
  testRunner.And("the JSON response should be a OperationOutcome resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
