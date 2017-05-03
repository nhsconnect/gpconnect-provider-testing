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
    [NUnit.Framework.DescriptionAttribute("ReadAppointment")]
    public partial class ReadAppointmentFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ReadAppointment.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ReadAppointment", null, ProgrammingLanguage.CSharp, ((string[])(null)));
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
        
        public virtual void FeatureBackground()
        {
#line 3
#line 4
 testRunner.Given("I have the test patient codes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 5
 testRunner.Given("I have the test ods codes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I perform a successful Read appointment")]
        public virtual void IPerformASuccessfulReadAppointment()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I perform a successful Read appointment", ((string[])(null)));
#line 7
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 8
 testRunner.Given("I find or create \"1\" appointments for patient \"patient1\" at organization \"ORG1\" a" +
                    "nd save bundle of appintment resources to \"Patient1AppointmentsInBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 10
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:read:appointment\"" +
                    " interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
 testRunner.When("I perform an appointment read for the first appointment saved in the bundle of re" +
                    "sources stored against key \"Patient1AppointmentsInBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 12
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 13
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read appointment invalid appointment id")]
        [NUnit.Framework.TestCaseAttribute("Invalid4321", new string[0])]
        [NUnit.Framework.TestCaseAttribute("8888888888", new string[0])]
        [NUnit.Framework.TestCaseAttribute("", new string[0])]
        public virtual void ReadAppointmentInvalidAppointmentId(string id, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read appointment invalid appointment id", exampleTags);
#line 16
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 17
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 18
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:read:appointment\"" +
                    " interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 19
 testRunner.When(string.Format("I make a GET request to \"/Appointment/{0}\"", id), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 20
 testRunner.Then("the response status code should be \"422\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 21
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
  testRunner.And("the response should be a OperationOutcome resource with error code \"REFERENCE_NOT" +
                    "_FOUND\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read appointment failure due to missing header")]
        [NUnit.Framework.TestCaseAttribute("Ssp-TraceID", new string[0])]
        [NUnit.Framework.TestCaseAttribute("Ssp-From", new string[0])]
        [NUnit.Framework.TestCaseAttribute("Ssp-To", new string[0])]
        [NUnit.Framework.TestCaseAttribute("Ssp-InteractionId", new string[0])]
        [NUnit.Framework.TestCaseAttribute("Authorization", new string[0])]
        public virtual void ReadAppointmentFailureDueToMissingHeader(string header, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read appointment failure due to missing header", exampleTags);
#line 29
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 30
 testRunner.Given("I find or create \"1\" appointments for patient \"patient1\" at organization \"ORG1\" a" +
                    "nd save bundle of appintment resources to \"Patient1AppointmentsInBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 31
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 32
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:read:appointment\"" +
                    " interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 33
  testRunner.And(string.Format("I do not send header \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 34
 testRunner.When("I perform an appointment read for the first appointment saved in the bundle of re" +
                    "sources stored against key \"Patient1AppointmentsInBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 35
 testRunner.Then("the response status code should be \"400\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 36
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 37
  testRunner.And("the response should be a OperationOutcome resource with error code \"BAD_REQUEST\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read appointment failure with incorrect interaction id")]
        [NUnit.Framework.TestCaseAttribute("urn:nhs:names:services:gpconnect:fhir:rest:search:organization", new string[0])]
        [NUnit.Framework.TestCaseAttribute("urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord", new string[0])]
        [NUnit.Framework.TestCaseAttribute("", new string[0])]
        [NUnit.Framework.TestCaseAttribute("null", new string[0])]
        public virtual void ReadAppointmentFailureWithIncorrectInteractionId(string interactionId, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read appointment failure with incorrect interaction id", exampleTags);
#line 46
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 47
 testRunner.Given("I find or create \"1\" appointments for patient \"patient1\" at organization \"ORG1\" a" +
                    "nd save bundle of appintment resources to \"Patient1AppointmentsInBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 48
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 49
  testRunner.And(string.Format("I am performing the \"{0}\" interaction", interactionId), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
  testRunner.And("I do not send header \"<Header>\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 51
 testRunner.When("I perform an appointment read for the first appointment saved in the bundle of re" +
                    "sources stored against key \"Patient1AppointmentsInBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 52
 testRunner.Then("the response status code should be \"400\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 53
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 54
  testRunner.And("the response should be a OperationOutcome resource with error code \"BAD_REQUEST\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read appointment accept header only")]
        [NUnit.Framework.TestCaseAttribute("0", "application/json+fhir", "JSON", new string[0])]
        [NUnit.Framework.TestCaseAttribute("0", "application/xml+fhir", "XML", new string[0])]
        [NUnit.Framework.TestCaseAttribute("1", "application/json+fhir", "JSON", new string[0])]
        [NUnit.Framework.TestCaseAttribute("1", "application/xml+fhir", "XML", new string[0])]
        public virtual void ReadAppointmentAcceptHeaderOnly(string appointmentIndex, string header, string bodyFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read appointment accept header only", exampleTags);
#line 62
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 63
 testRunner.Given("I find or create \"2\" appointments for patient \"patient1\" at organization \"ORG1\" a" +
                    "nd save bundle of appintment resources to \"Patient1AppointmentsInBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 65
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 66
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:read:appointment\"" +
                    " interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
  testRunner.And(string.Format("I set the Accept header to \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 68
 testRunner.When(string.Format("I perform an appointment read appointment index \"{0}\" saved in the bundle of reso" +
                        "urces stored against key \"Patient1AppointmentsInBundle\"", appointmentIndex), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 69
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 70
  testRunner.And(string.Format("the response body should be FHIR {0}", bodyFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 71
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read appointment _format parameter only")]
        [NUnit.Framework.TestCaseAttribute("application/json+fhir", "JSON", new string[0])]
        [NUnit.Framework.TestCaseAttribute("application/xml+fhir", "XML", new string[0])]
        public virtual void ReadAppointment_FormatParameterOnly(string parameter, string bodyFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read appointment _format parameter only", exampleTags);
#line 79
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 80
 testRunner.Given("I find or create \"1\" appointments for patient \"patient1\" at organization \"ORG1\" a" +
                    "nd save bundle of appintment resources to \"Patient1AppointmentsInBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 81
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 82
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:read:appointment\"" +
                    " interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 83
        testRunner.And(string.Format("I add the parameter \"_format\" with the value \"{0}\"", parameter), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 84
 testRunner.When("I perform an appointment read for the first appointment saved in the bundle of re" +
                    "sources stored against key \"Patient1AppointmentsInBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 85
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 86
  testRunner.And(string.Format("the response body should be FHIR {0}", bodyFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 87
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read appointment accept header and _format parameter")]
        [NUnit.Framework.TestCaseAttribute("application/json+fhir", "application/json+fhir", "JSON", new string[0])]
        [NUnit.Framework.TestCaseAttribute("application/json+fhir", "application/xml+fhir", "XML", new string[0])]
        [NUnit.Framework.TestCaseAttribute("application/xml+fhir", "application/json+fhir", "JSON", new string[0])]
        [NUnit.Framework.TestCaseAttribute("application/xml+fhir", "application/xml+fhir", "XML", new string[0])]
        public virtual void ReadAppointmentAcceptHeaderAnd_FormatParameter(string header, string parameter, string bodyFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read appointment accept header and _format parameter", exampleTags);
#line 93
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 94
 testRunner.Given("I find or create \"1\" appointments for patient \"patient1\" at organization \"ORG1\" a" +
                    "nd save bundle of appintment resources to \"Patient1AppointmentsInBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 95
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 96
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:read:appointment\"" +
                    " interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 97
  testRunner.And(string.Format("I set the Accept header to \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 98
        testRunner.And(string.Format("I add the parameter \"_format\" with the value \"{0}\"", parameter), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 99
 testRunner.When("I perform an appointment read for the first appointment saved in the bundle of re" +
                    "sources stored against key \"Patient1AppointmentsInBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 100
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 101
  testRunner.And(string.Format("the response body should be FHIR {0}", bodyFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 102
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read appointment valid request shall include id and structure definition profile")]
        public virtual void ReadAppointmentValidRequestShallIncludeIdAndStructureDefinitionProfile()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read appointment valid request shall include id and structure definition profile", ((string[])(null)));
#line 110
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 111
 testRunner.Given("I find or create \"1\" appointments for patient \"patient1\" at organization \"ORG1\" a" +
                    "nd save bundle of appintment resources to \"Patient1AppointmentsInBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 112
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 113
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:read:appointment\"" +
                    " interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 114
 testRunner.When("I perform an appointment read for the first appointment saved in the bundle of re" +
                    "sources stored against key \"Patient1AppointmentsInBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 115
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 116
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 117
  testRunner.And("the returned appointment resource shall contains an id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 118
  testRunner.And("the returned appointment resource should contain meta data profile and version id" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read appointment check response contains required elements")]
        public virtual void ReadAppointmentCheckResponseContainsRequiredElements()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read appointment check response contains required elements", ((string[])(null)));
#line 120
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 121
 testRunner.Given("I find or create \"1\" appointments for patient \"patient1\" at organization \"ORG1\" a" +
                    "nd save bundle of appintment resources to \"Patient1AppointmentsInBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 122
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 123
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:read:appointment\"" +
                    " interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 124
 testRunner.When("I perform an appointment read for the first appointment saved in the bundle of re" +
                    "sources stored against key \"Patient1AppointmentsInBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 125
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 126
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 127
  testRunner.And("the appointment response resource contains an start date", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 128
  testRunner.And("the appointment response resource contains an end date", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 129
  testRunner.And("the appointment response resource contains a slot reference", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 130
  testRunner.And("the appointment response resource contains atleast 2 participants a practitioner " +
                    "and a patient", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read appointment if resource contains identifier then the value is mandatory")]
        public virtual void ReadAppointmentIfResourceContainsIdentifierThenTheValueIsMandatory()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read appointment if resource contains identifier then the value is mandatory", ((string[])(null)));
#line 132
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 133
 testRunner.Given("I find or create \"1\" appointments for patient \"patient1\" at organization \"ORG1\" a" +
                    "nd save bundle of appintment resources to \"Patient1AppointmentsInBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 134
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 135
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:read:appointment\"" +
                    " interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 136
 testRunner.When("I perform an appointment read for the first appointment saved in the bundle of re" +
                    "sources stored against key \"Patient1AppointmentsInBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 137
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 138
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 139
  testRunner.And("if the appointment response resource contains any identifiers they must have a va" +
                    "lue", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
