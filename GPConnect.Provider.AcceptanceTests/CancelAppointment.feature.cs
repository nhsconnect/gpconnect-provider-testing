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
    [NUnit.Framework.DescriptionAttribute("CancelAppointment")]
    public partial class CancelAppointmentFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "CancelAppointment.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "CancelAppointment", null, ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("I perform a successful cancel appointment")]
        [NUnit.Framework.TestCaseAttribute("patient1", new string[0])]
        [NUnit.Framework.TestCaseAttribute("patient2", new string[0])]
        [NUnit.Framework.TestCaseAttribute("patient3", new string[0])]
        [NUnit.Framework.TestCaseAttribute("CustomAppointment1", new string[0])]
        public virtual void IPerformASuccessfulCancelAppointment(string patientName, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I perform a successful cancel appointment", exampleTags);
#line 7
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 8
 testRunner.Given(string.Format("I find or create an appointment with status Booked for patient \"{0}\" at organizat" +
                        "ion \"ORG1\" and save the appointment resources to \"patientApp\"", patientName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 10
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:update:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
 testRunner.When("I cancel the appointment called \"patientApp\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 12
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 13
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
  testRunner.And("the returned appointment resource should be cancelled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cancel appointment sending invalid URL")]
        [NUnit.Framework.TestCaseAttribute("/Appointment/!", new string[0])]
        [NUnit.Framework.TestCaseAttribute("/APPointment/23", new string[0])]
        [NUnit.Framework.TestCaseAttribute("/Appointment/#", new string[0])]
        [NUnit.Framework.TestCaseAttribute("/Appointment/cancel", new string[0])]
        public virtual void CancelAppointmentSendingInvalidURL(string url, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cancel appointment sending invalid URL", exampleTags);
#line 23
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 24
 testRunner.Given("I find or create an appointment with status Booked for patient \"patient1\" at orga" +
                    "nization \"ORG1\" and save the appointment resources to \"patientApp\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 25
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 26
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:update:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
 testRunner.When(string.Format("I set the URL to \"{0}\" and cancel \"patientApp\"", url), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 28
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cancel appointment failure due to missing header")]
        [NUnit.Framework.TestCaseAttribute("Ssp-TraceID", new string[0])]
        [NUnit.Framework.TestCaseAttribute("Ssp-From", new string[0])]
        [NUnit.Framework.TestCaseAttribute("Ssp-To", new string[0])]
        [NUnit.Framework.TestCaseAttribute("Ssp-InteractionId", new string[0])]
        [NUnit.Framework.TestCaseAttribute("Authorization", new string[0])]
        public virtual void CancelAppointmentFailureDueToMissingHeader(string header, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cancel appointment failure due to missing header", exampleTags);
#line 36
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 37
  testRunner.Given("I find or create an appointment with status Booked for patient \"patient1\" at orga" +
                    "nization \"ORG1\" and save the appointment resources to \"patientApp\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 38
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 39
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:update:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 40
  testRunner.And(string.Format("I do not send header \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 41
 testRunner.When("I cancel the appointment called \"patientApp\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 42
 testRunner.Then("the response status code should be \"400\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 43
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
  testRunner.And("the response should be a OperationOutcome resource with error code \"BAD_REQUEST\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cancel appointment failure with incorrect interaction id")]
        [NUnit.Framework.TestCaseAttribute("urn:nhs:names:services:gpconnect:fhir:rest:update:appointmentss", new string[0])]
        [NUnit.Framework.TestCaseAttribute("urn:nhs:names:services:gpconnect:fhir:rest:update:appointmenT", new string[0])]
        [NUnit.Framework.TestCaseAttribute("urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord", new string[0])]
        [NUnit.Framework.TestCaseAttribute("", new string[0])]
        [NUnit.Framework.TestCaseAttribute("null", new string[0])]
        public virtual void CancelAppointmentFailureWithIncorrectInteractionId(string interactionId, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cancel appointment failure with incorrect interaction id", exampleTags);
#line 53
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 54
 testRunner.Given("I find or create an appointment with status Booked for patient \"patient1\" at orga" +
                    "nization \"ORG1\" and save the appointment resources to \"patientApp\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 55
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 56
  testRunner.And(string.Format("I am performing the \"{0}\" interaction", interactionId), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 57
 testRunner.When("I cancel the appointment called \"patientApp\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 58
 testRunner.Then("the response status code should be \"400\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 59
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 60
  testRunner.And("the response should be a OperationOutcome resource with error code \"BAD_REQUEST\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cancel appointment _format parameter only")]
        [NUnit.Framework.TestCaseAttribute("application/json+fhir", "JSON", new string[0])]
        [NUnit.Framework.TestCaseAttribute("application/xml+fhir", "XML", new string[0])]
        public virtual void CancelAppointment_FormatParameterOnly(string parameter, string bodyFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cancel appointment _format parameter only", exampleTags);
#line 69
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 70
 testRunner.Given("I find or create an appointment with status Booked for patient \"patient1\" at orga" +
                    "nization \"ORG1\" and save the appointment resources to \"patientApp\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 71
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 72
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:update:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 73
  testRunner.And(string.Format("I add the parameter \"_format\" with the value \"{0}\"", parameter), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 74
 testRunner.When("I cancel the appointment called \"patientApp\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 75
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 76
  testRunner.And(string.Format("the response body should be FHIR {0}", bodyFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 78
  testRunner.And("the returned appointment resource should be cancelled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cancel appointment accept header and _format parameter")]
        [NUnit.Framework.TestCaseAttribute("application/json+fhir", "application/json+fhir", "JSON", new string[0])]
        [NUnit.Framework.TestCaseAttribute("application/json+fhir", "application/xml+fhir", "XML", new string[0])]
        [NUnit.Framework.TestCaseAttribute("application/xml+fhir", "application/json+fhir", "JSON", new string[0])]
        [NUnit.Framework.TestCaseAttribute("application/xml+fhir", "application/xml+fhir", "XML", new string[0])]
        public virtual void CancelAppointmentAcceptHeaderAnd_FormatParameter(string header, string parameter, string bodyFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cancel appointment accept header and _format parameter", exampleTags);
#line 84
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 85
 testRunner.Given("I find or create an appointment with status Booked for patient \"patient1\" at orga" +
                    "nization \"ORG1\" and save the appointment resources to \"patientApp\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 86
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 87
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:update:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 88
  testRunner.And(string.Format("I set the Accept header to \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 89
        testRunner.And(string.Format("I add the parameter \"_format\" with the value \"{0}\"", parameter), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 90
  testRunner.When("I cancel the appointment called \"patientApp\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 91
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 92
  testRunner.And(string.Format("the response body should be FHIR {0}", bodyFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 93
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 94
  testRunner.And("the returned appointment resource should be cancelled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cancel appointment check cancellation reason is equal to the request cancellation" +
            " reason")]
        [NUnit.Framework.TestCaseAttribute("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellat" +
            "ion-reason-1-0", "aa", "Too busy", new string[0])]
        [NUnit.Framework.TestCaseAttribute("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellat" +
            "ion-reason-1-0", "aa", "Car crashed", new string[0])]
        [NUnit.Framework.TestCaseAttribute("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellat" +
            "ion-reason-1-0", "aa", "Too tired", new string[0])]
        public virtual void CancelAppointmentCheckCancellationReasonIsEqualToTheRequestCancellationReason(string url, string code, string display, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cancel appointment check cancellation reason is equal to the request cancellation" +
                    " reason", exampleTags);
#line 103
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 104
 testRunner.Given("I find or create an appointment with status Booked for patient \"CustomAppointment" +
                    "1\" at organization \"ORG1\" and save the appointment resources to \"patientApp\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 105
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 106
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:update:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 107
 testRunner.When(string.Format("I cancel the appointment with cancel extension with url \"{0}\" code \"{1}\" and disp" +
                        "lay \"{2}\" called \"patientApp\"", url, code, display), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 108
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 109
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 110
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 111
  testRunner.And("the returned appointment resource should be cancelled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 112
  testRunner.And(string.Format("the cancellation reason in the returned response should be equal to \"{0}\"", display), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cancel appointment invalid cancellation extension")]
        [NUnit.Framework.TestCaseAttribute("", "", "", new string[0])]
        [NUnit.Framework.TestCaseAttribute("", "ff", "", new string[0])]
        [NUnit.Framework.TestCaseAttribute("", "", "ee", new string[0])]
        [NUnit.Framework.TestCaseAttribute("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellat" +
            "ion-reason-1-0", "", "", new string[0])]
        [NUnit.Framework.TestCaseAttribute("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellat" +
            "ion-reason-1-0", "ff", "", new string[0])]
        [NUnit.Framework.TestCaseAttribute("http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellat" +
            "ion-reason-1-0", "", "ee", new string[0])]
        public virtual void CancelAppointmentInvalidCancellationExtension(string url, string code, string display, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cancel appointment invalid cancellation extension", exampleTags);
#line 120
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 121
 testRunner.Given("I find or create an appointment with status Booked for patient \"CustomAppointment" +
                    "1\" at organization \"ORG1\" and save the appointment resources to \"patientApp\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 122
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 123
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:update:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 124
 testRunner.When(string.Format("I cancel the appointment with cancel extension with url \"{0}\" code \"{1}\" and disp" +
                        "lay \"{2}\" called \"patientApp\"", url, code, display), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 125
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 126
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 127
  testRunner.And("the response should be a OperationOutcome resource with error code \"500\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cancel appointment and check the returned appointment resource is a valid resourc" +
            "e")]
        public virtual void CancelAppointmentAndCheckTheReturnedAppointmentResourceIsAValidResource()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cancel appointment and check the returned appointment resource is a valid resourc" +
                    "e", ((string[])(null)));
#line 137
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 138
 testRunner.Given("I find or create an appointment with status Booked for patient \"patient1\" at orga" +
                    "nization \"ORG1\" and save the appointment resources to \"patientApp\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 139
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 140
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:update:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 141
 testRunner.When("I cancel the appointment called \"patientApp\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 142
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 143
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 144
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 145
  testRunner.And("the returned appointment resource should be cancelled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 146
  testRunner.And("the returned appointment resource should contain meta data profile and version id" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Conformance profile supports the cancel appointment operation")]
        public virtual void ConformanceProfileSupportsTheCancelAppointmentOperation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Conformance profile supports the cancel appointment operation", ((string[])(null)));
#line 148
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 149
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 150
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:read:metadata\" in" +
                    "teraction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 151
 testRunner.When("I make a GET request to \"/metadata\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 152
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 153
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 154
  testRunner.And("the conformance profile should contain the \"Appointment\" resource with a \"update\"" +
                    " interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cancel appointment verify resource is updated when an valid ETag value is provide" +
            "d")]
        public virtual void CancelAppointmentVerifyResourceIsUpdatedWhenAnValidETagValueIsProvided()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cancel appointment verify resource is updated when an valid ETag value is provide" +
                    "d", ((string[])(null)));
#line 156
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 157
testRunner.Given("I find or create an appointment with status Booked for patient \"patient1\" at orga" +
                    "nization \"ORG1\" and save the appointment resources to \"patientApp\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 158
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 159
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:read:appointment\"" +
                    " interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 160
 testRunner.When("I perform an appointment read for the appointment called \"patientApp\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 161
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 162
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 163
  testRunner.And("the response ETag is saved as \"etagCancel\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 164
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 165
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 166
 testRunner.And("I set \"If-Match\" request header to \"etagCancel\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 167
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:update:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 168
 testRunner.When("I cancel the appointment called \"patientApp\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 169
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 170
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 171
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 172
  testRunner.And("the returned appointment resource should be cancelled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cancel appointment verify resource is not updated when an out of date ETag value " +
            "is provided")]
        public virtual void CancelAppointmentVerifyResourceIsNotUpdatedWhenAnOutOfDateETagValueIsProvided()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cancel appointment verify resource is not updated when an out of date ETag value " +
                    "is provided", ((string[])(null)));
#line 174
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 175
testRunner.Given("I find or create an appointment with status Booked for patient \"patient1\" at orga" +
                    "nization \"ORG1\" and save the appointment resources to \"patientApp\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 176
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 177
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:read:appointment\"" +
                    " interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 178
 testRunner.When("I perform an appointment read for the appointment called \"patientApp\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 179
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 180
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 181
  testRunner.And("the response ETag is saved as \"etagCancel\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 182
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 183
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 184
 testRunner.And("I set If-Match request header to \"hello\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 185
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:update:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 186
 testRunner.When("I cancel the appointment called \"patientApp\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 187
 testRunner.Then("the response status code should be \"409\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 188
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 189
  testRunner.And("the response should be a OperationOutcome resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
