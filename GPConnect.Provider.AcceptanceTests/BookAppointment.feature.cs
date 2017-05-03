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
    [NUnit.Framework.DescriptionAttribute("BookAppointment")]
    public partial class BookAppointmentFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "BookAppointment.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "BookAppointment", null, ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("Book single appointment for patient")]
        [NUnit.Framework.CategoryAttribute("Appointment")]
        public virtual void BookSingleAppointmentForPatient()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Book single appointment for patient", new string[] {
                        "Appointment"});
#line 9
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 10
 testRunner.Given("I perform the getSchedule operation for organization \"ORG1\" and store the returne" +
                    "d bundle resources against key \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 11
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 12
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:create:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
 testRunner.When("I book an appointment for patient \"patient1\" on the provider system with the sche" +
                    "dule name \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 14
 testRunner.Then("the response status code should indicate created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 15
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Book Appointment with invalid url for booking appointment")]
        public virtual void BookAppointmentWithInvalidUrlForBookingAppointment()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Book Appointment with invalid url for booking appointment", ((string[])(null)));
#line 18
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 19
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 20
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:create:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
 testRunner.When("I make a GET request to \"/Appointment/5\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 22
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Book appointment failure due to missing header")]
        [NUnit.Framework.TestCaseAttribute("Ssp-TraceID", new string[0])]
        [NUnit.Framework.TestCaseAttribute("Ssp-From", new string[0])]
        [NUnit.Framework.TestCaseAttribute("Ssp-To", new string[0])]
        [NUnit.Framework.TestCaseAttribute("Ssp-InteractionId", new string[0])]
        [NUnit.Framework.TestCaseAttribute("Authorization", new string[0])]
        public virtual void BookAppointmentFailureDueToMissingHeader(string header, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Book appointment failure due to missing header", exampleTags);
#line 24
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 25
 testRunner.Given("I perform the getSchedule operation for organization \"ORG1\" and store the returne" +
                    "d bundle resources against key \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 26
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 27
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:create:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 28
  testRunner.And(string.Format("I do not send header \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 29
 testRunner.When("I book an appointment for patient \"patient1\" on the provider system with the sche" +
                    "dule name \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 30
 testRunner.Then("the response status code should be \"400\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 31
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 32
  testRunner.And("the response should be a OperationOutcome resource with error code \"BAD_REQUEST\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Book appointment accept header and _format parameter")]
        [NUnit.Framework.TestCaseAttribute("application/json+fhir", "application/json+fhir", "JSON", new string[0])]
        [NUnit.Framework.TestCaseAttribute("application/json+fhir", "application/xml+fhir", "XML", new string[0])]
        [NUnit.Framework.TestCaseAttribute("application/xml+fhir", "application/json+fhir", "JSON", new string[0])]
        [NUnit.Framework.TestCaseAttribute("application/xml+fhir", "application/xml+fhir", "XML", new string[0])]
        public virtual void BookAppointmentAcceptHeaderAnd_FormatParameter(string header, string parameter, string bodyFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Book appointment accept header and _format parameter", exampleTags);
#line 41
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 42
    testRunner.Given("I perform the getSchedule operation for organization \"ORG1\" and store the returne" +
                    "d bundle resources against key \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 43
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 44
        testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:create:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 45
        testRunner.And(string.Format("I set the Accept header to \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
        testRunner.And(string.Format("I add the parameter \"_format\" with the value \"{0}\"", parameter), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 47
 testRunner.When("I book an appointment for patient \"patient1\" on the provider system with the sche" +
                    "dule name \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 48
    testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 49
        testRunner.And(string.Format("the response body should be FHIR {0}", bodyFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Book appointment accept header variations")]
        [NUnit.Framework.TestCaseAttribute("application/json+fhir", "JSON", new string[0])]
        [NUnit.Framework.TestCaseAttribute("application/xml+fhir", "XML", new string[0])]
        public virtual void BookAppointmentAcceptHeaderVariations(string header, string bodyFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Book appointment accept header variations", exampleTags);
#line 59
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 60
 testRunner.Given("I perform the getSchedule operation for organization \"ORG1\" and store the returne" +
                    "d bundle resources against key \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 61
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 62
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:create:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 63
  testRunner.And(string.Format("I set the Accept header to \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 64
 testRunner.When("I book an appointment for patient \"patient1\" on the provider system with the sche" +
                    "dule name \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 65
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 66
  testRunner.And(string.Format("the response body should be FHIR {0}", bodyFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Book appointment prefer header variations")]
        [NUnit.Framework.TestCaseAttribute("representation", new string[0])]
        [NUnit.Framework.TestCaseAttribute("minimal", new string[0])]
        public virtual void BookAppointmentPreferHeaderVariations(string header, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Book appointment prefer header variations", exampleTags);
#line 73
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 74
 testRunner.Given("I perform the getSchedule operation for organization \"ORG1\" and store the returne" +
                    "d bundle resources against key \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 75
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 76
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:create:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
  testRunner.And(string.Format("I set the Prefer header to \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 78
 testRunner.When("I book an appointment for patient \"patient1\" on the provider system with the sche" +
                    "dule name \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 79
 testRunner.Then("the response status code should indicate created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 80
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 81
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Book appointment interaction id incorrect fail")]
        [NUnit.Framework.TestCaseAttribute("urn:nhs:names:services:gpconnect:fhir:rest:search:organization", new string[0])]
        [NUnit.Framework.TestCaseAttribute("urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord", new string[0])]
        [NUnit.Framework.TestCaseAttribute("", new string[0])]
        [NUnit.Framework.TestCaseAttribute("null", new string[0])]
        public virtual void BookAppointmentInteractionIdIncorrectFail(string interactionId, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Book appointment interaction id incorrect fail", exampleTags);
#line 87
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 88
 testRunner.Given("I perform the getSchedule operation for organization \"ORG1\" and store the returne" +
                    "d bundle resources against key \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 89
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 90
 testRunner.When(string.Format("I book an appointment for patient \"patient1\" on the provider system with the sche" +
                        "dule name \"getScheduleResponseBundle\" with interaction id \"{0}\"", interactionId), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 91
    testRunner.Then("the response status code should be \"400\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 92
        testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 93
  testRunner.And("the response should be a OperationOutcome resource with error code \"BAD_REQUEST\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Book Appointment and check response returns the correct values")]
        public virtual void BookAppointmentAndCheckResponseReturnsTheCorrectValues()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Book Appointment and check response returns the correct values", ((string[])(null)));
#line 101
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 102
 testRunner.Given("I perform the getSchedule operation for organization \"ORG1\" and store the returne" +
                    "d bundle resources against key \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 103
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 104
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:create:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 105
 testRunner.When("I book an appointment for patient \"patient1\" on the provider system with the sche" +
                    "dule name \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 106
 testRunner.Then("the response status code should indicate created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 107
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 108
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 109
  testRunner.And("the returned appointment resource shall contains an id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 110
  testRunner.And("the appointment resource should contain a single status element", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 111
  testRunner.And("the appointment resource should contain a single start element", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 112
  testRunner.And("the appointment resource should contain a single end element", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 113
  testRunner.And("the appointment resource should contain at least one participant", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 114
  testRunner.And("the appointment resource should contain at least one slot reference", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 115
  testRunner.And("the appointment response contains a type with a valid system code and display", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 116
  testRunner.And("if the appointment resource contains a priority the value is valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Book Appointment and check response returns the relevent structured definition")]
        public virtual void BookAppointmentAndCheckResponseReturnsTheReleventStructuredDefinition()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Book Appointment and check response returns the relevent structured definition", ((string[])(null)));
#line 118
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 119
 testRunner.Given("I perform the getSchedule operation for organization \"ORG1\" and store the returne" +
                    "d bundle resources against key \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 120
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 121
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:create:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 122
 testRunner.When("I book an appointment for patient \"patient1\" on the provider system with the sche" +
                    "dule name \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 123
 testRunner.Then("the response status code should indicate created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 124
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 125
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 126
  testRunner.And("the returned appointment resource should contain meta data profile and version id" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Book Appointment and check slot reference is valid")]
        public virtual void BookAppointmentAndCheckSlotReferenceIsValid()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Book Appointment and check slot reference is valid", ((string[])(null)));
#line 129
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 130
 testRunner.Given("I perform the getSchedule operation for organization \"ORG1\" and store the returne" +
                    "d bundle resources against key \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 131
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 132
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:create:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 133
 testRunner.When("I book an appointment for patient \"patient1\" on the provider system with the sche" +
                    "dule name \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 134
 testRunner.Then("the response status code should indicate created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 135
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 136
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 137
  testRunner.And("the appointment response resource contains a slot reference", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 138
  testRunner.And("the slot reference is present and valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Book Appointment and appointment participant is valid")]
        public virtual void BookAppointmentAndAppointmentParticipantIsValid()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Book Appointment and appointment participant is valid", ((string[])(null)));
#line 141
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 142
 testRunner.Given("I perform the getSchedule operation for organization \"ORG1\" and store the returne" +
                    "d bundle resources against key \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 143
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 144
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:create:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 145
 testRunner.When("I book an appointment for patient \"patient1\" on the provider system with the sche" +
                    "dule name \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 146
 testRunner.Then("the response status code should indicate created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 147
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 148
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 149
  testRunner.And("the appointment resource should contain at least one participant", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 150
  testRunner.And("the appointment resource participant must contain a type or actor", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 151
  testRunner.And("the appointment participant contains a type is should have a valid system and cod" +
                    "e", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Book Appointment and check extension methods are valid")]
        public virtual void BookAppointmentAndCheckExtensionMethodsAreValid()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Book Appointment and check extension methods are valid", ((string[])(null)));
#line 153
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 154
 testRunner.Given("I perform the getSchedule operation for organization \"ORG1\" and store the returne" +
                    "d bundle resources against key \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 155
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 156
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:create:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 157
 testRunner.When("I book an appointment for patient \"patient1\" on the provider system with the sche" +
                    "dule name \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 158
 testRunner.Then("the response status code should indicate created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 159
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 160
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 161
  testRunner.And("if the appointment category element is present it is populated with the correct v" +
                    "alues", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 162
  testRunner.And("if the appointment booking element is present it is populated with the correct va" +
                    "lues", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 163
  testRunner.And("if the appointment contact element is present it is populated with the correct va" +
                    "lues", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 164
  testRunner.And("if the appointment cancellation reason element is present it is populated with th" +
                    "e correct values", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Book Appointment and remove patient participant")]
        [NUnit.Framework.TestCaseAttribute("Appointment1", new string[0])]
        public virtual void BookAppointmentAndRemovePatientParticipant(string appointment, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Book Appointment and remove patient participant", exampleTags);
#line 167
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 168
 testRunner.Given("I perform the getSchedule operation for organization \"ORG1\" and store the returne" +
                    "d bundle resources against key \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 169
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 170
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:create:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 171
 testRunner.Then(string.Format("I create an appointment for patient \"patient1\" called \"{0}\" from schedule \"getSch" +
                        "eduleResponseBundle\"", appointment), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 172
 testRunner.Then(string.Format("I remove the patient participant from the appointment called \"{0}\"", appointment), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 173
 testRunner.Then(string.Format("I book the appointment called \"{0}\"", appointment), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 174
 testRunner.Then("the response status code should indicate created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 175
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 176
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Book Appointment and remove location participant")]
        [NUnit.Framework.TestCaseAttribute("Appointment2", new string[0])]
        public virtual void BookAppointmentAndRemoveLocationParticipant(string appointment, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Book Appointment and remove location participant", exampleTags);
#line 181
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 182
 testRunner.Given("I perform the getSchedule operation for organization \"ORG1\" and store the returne" +
                    "d bundle resources against key \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 183
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 184
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:create:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 185
 testRunner.Then(string.Format("I create an appointment for patient \"patient1\" called \"{0}\" from schedule \"getSch" +
                        "eduleResponseBundle\"", appointment), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 186
 testRunner.Then(string.Format("I remove the location participant from the appointment called \"{0}\"", appointment), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 187
 testRunner.Then(string.Format("I book the appointment called \"{0}\"", appointment), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 188
 testRunner.Then("the response status code should indicate created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 189
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 190
  testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Book Appointment and remove practitioner participant")]
        [NUnit.Framework.TestCaseAttribute("Appointment3", new string[0])]
        public virtual void BookAppointmentAndRemovePractitionerParticipant(string appointment, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Book Appointment and remove practitioner participant", exampleTags);
#line 195
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 196
testRunner.Given("I perform the getSchedule operation for organization \"ORG1\" and store the returne" +
                    "d bundle resources against key \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 197
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 198
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:create:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 199
 testRunner.Then(string.Format("I create an appointment for patient \"patient1\" called \"{0}\" from schedule \"getSch" +
                        "eduleResponseBundle\"", appointment), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 200
 testRunner.Then(string.Format("I remove the practitioner participant from the appointment called \"{0}\"", appointment), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 201
 testRunner.Then(string.Format("I book the appointment called \"{0}\"", appointment), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 202
 testRunner.Then("the response status code should indicate created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 203
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 204
 testRunner.And("the response should be an Appointment resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Book Appointment and remove all participant")]
        [NUnit.Framework.TestCaseAttribute("Appointment1", new string[0])]
        public virtual void BookAppointmentAndRemoveAllParticipant(string appointment, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Book Appointment and remove all participant", exampleTags);
#line 209
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 210
 testRunner.Given("I perform the getSchedule operation for organization \"ORG1\" and store the returne" +
                    "d bundle resources against key \"getScheduleResponseBundle\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 211
 testRunner.Given("I am using the default server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 212
  testRunner.And("I am performing the \"urn:nhs:names:services:gpconnect:fhir:rest:create:appointmen" +
                    "t\" interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 213
 testRunner.Then(string.Format("I create an appointment for patient \"patient1\" called \"{0}\" from schedule \"getSch" +
                        "eduleResponseBundle\"", appointment), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 214
 testRunner.Then(string.Format("I remove the patient participant from the appointment called \"{0}\"", appointment), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 215
 testRunner.Then(string.Format("I remove the location participant from the appointment called \"{0}\"", appointment), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 216
 testRunner.Then(string.Format("I remove the practitioner participant from the appointment called \"{0}\"", appointment), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 217
 testRunner.Then(string.Format("I book the appointment called \"{0}\"", appointment), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 218
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 219
  testRunner.And("the response status code should be \"422\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
