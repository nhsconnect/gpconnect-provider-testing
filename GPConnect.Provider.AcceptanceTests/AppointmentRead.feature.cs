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
    [NUnit.Framework.DescriptionAttribute("AppointmentRead")]
    [NUnit.Framework.CategoryAttribute("appointment")]
    [NUnit.Framework.CategoryAttribute("1.2.7-Full-Pack")]
    public partial class AppointmentReadFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "AppointmentRead.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "AppointmentRead", null, ProgrammingLanguage.CSharp, new string[] {
                        "appointment",
                        "1.2.7-Full-Pack"});
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
        [NUnit.Framework.DescriptionAttribute("I perform a successful Read appointment")]
        [NUnit.Framework.CategoryAttribute("1.2.3")]
        [NUnit.Framework.TestCaseAttribute("patient1", null)]
        [NUnit.Framework.TestCaseAttribute("patient2", null)]
        [NUnit.Framework.TestCaseAttribute("patient3", null)]
        public virtual void IPerformASuccessfulReadAppointment(string patientName, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "1.2.3"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I perform a successful Read appointment", null, @__tags);
#line 5
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 6
 testRunner.Given(string.Format("I create an Appointment for Patient \"{0}\"", patientName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 7
  testRunner.And("I store the Created Appointment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 8
 testRunner.Given("I configure the default \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
 testRunner.When("I make the \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 10
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 11
  testRunner.And("the Response Resource should be an Appointment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
  testRunner.And("the Appointments returned must be in the future", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
  testRunner.And("the Appointment Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
  testRunner.And("the Appointment Metadata should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
  testRunner.And("the Appointment DeliveryChannel must be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
  testRunner.And("the Appointment PractitionerRole must be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 19
  testRunner.And("the Appointment Not In Use should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I perform a successful Read appointment with Extensions")]
        [NUnit.Framework.TestCaseAttribute("patient1", "true", "true", "true", null)]
        public virtual void IPerformASuccessfulReadAppointmentWithExtensions(string patientName, string orgType, string deliveryChannel, string pracRole, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I perform a successful Read appointment with Extensions", null, exampleTags);
#line 26
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 27
 testRunner.Given(string.Format("I create an Appointment for Patient \"{0}\"", patientName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 28
  testRunner.And(string.Format("I create an Appointment with org type \"{0}\" with channel \"{1}\" with prac role \"{2" +
                        "}\"", orgType, deliveryChannel, pracRole), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 29
  testRunner.And("I store the Created Appointment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
 testRunner.Given("I configure the default \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 31
 testRunner.When("I make the \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 32
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 33
  testRunner.And("the Response Resource should be an Appointment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 34
  testRunner.And("the Appointments returned must be in the future", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
  testRunner.And("the Appointment Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
  testRunner.And("the Appointment Metadata should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 37
  testRunner.And("the Appointment DeliveryChannel must be present", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 38
  testRunner.And("the Appointment PractitionerRole must be present", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("I perform a successful Read appointment with JWT Org Different")]
        public virtual void IPerformASuccessfulReadAppointmentWithJWTOrgDifferent()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I perform a successful Read appointment with JWT Org Different", null, ((string[])(null)));
#line 43
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 46
 testRunner.Given("I create an Appointment for Patient \"patient1\" and Organization Code \"ORG3\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 47
  testRunner.And("I store the Created Appointment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
 testRunner.Given("I configure the default \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 49
 testRunner.When("I make the \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 50
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 51
  testRunner.And("the Response Resource should be an Appointment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 52
  testRunner.And("the Appointment Organisation Code should equal \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read appointment invalid appointment id")]
        [NUnit.Framework.TestCaseAttribute("Invalid4321", null)]
        [NUnit.Framework.TestCaseAttribute("8888888888", null)]
        [NUnit.Framework.TestCaseAttribute("", null)]
        public virtual void ReadAppointmentInvalidAppointmentId(string id, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read appointment invalid appointment id", null, exampleTags);
#line 54
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 55
 testRunner.Given("I create an Appointment for Patient \"patient1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 56
  testRunner.And("I store the Created Appointment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 57
 testRunner.Given("I configure the default \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 58
  testRunner.And(string.Format("I set the Read Operation logical identifier used in the request to \"{0}\"", id), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
 testRunner.When("I make the \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 60
 testRunner.Then("the response status code should be \"404\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 61
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read appointment using the _format parameter to request response format")]
        [NUnit.Framework.TestCaseAttribute("application/fhir+json", "JSON", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+xml", "XML", null)]
        public virtual void ReadAppointmentUsingThe_FormatParameterToRequestResponseFormat(string parameter, string bodyFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read appointment using the _format parameter to request response format", null, exampleTags);
#line 68
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 69
 testRunner.Given("I create an Appointment for Patient \"patient1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 70
  testRunner.And("I store the Created Appointment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 71
 testRunner.Given("I configure the default \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 72
  testRunner.And(string.Format("I add the parameter \"_format\" with the value \"{0}\"", parameter), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 73
 testRunner.When("I make the \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 74
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 75
  testRunner.And(string.Format("the response body should be FHIR {0}", bodyFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 76
  testRunner.And("the Response Resource should be an Appointment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read appointment using the _format parameter and accept header to request respons" +
            "e format")]
        [NUnit.Framework.TestCaseAttribute("application/fhir+json", "application/fhir+json", "JSON", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+json", "application/fhir+xml", "XML", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+xml", "application/fhir+json", "JSON", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+xml", "application/fhir+xml", "XML", null)]
        public virtual void ReadAppointmentUsingThe_FormatParameterAndAcceptHeaderToRequestResponseFormat(string header, string parameter, string bodyFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read appointment using the _format parameter and accept header to request respons" +
                    "e format", null, exampleTags);
#line 83
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 84
 testRunner.Given("I create an Appointment for Patient \"patient1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 85
  testRunner.And("I store the Created Appointment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 86
 testRunner.Given("I configure the default \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 87
  testRunner.And(string.Format("I set the Accept header to \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 88
  testRunner.And(string.Format("I add the parameter \"_format\" with the value \"{0}\"", parameter), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 89
 testRunner.When("I make the \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 90
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 91
  testRunner.And(string.Format("the response body should be FHIR {0}", bodyFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 92
  testRunner.And("the Response Resource should be an Appointment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read appointment ensure response appointments contain the manadatory elements")]
        [NUnit.Framework.TestCaseAttribute("application/fhir+json", "JSON", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+xml", "XML", null)]
        public virtual void ReadAppointmentEnsureResponseAppointmentsContainTheManadatoryElements(string header, string bodyFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read appointment ensure response appointments contain the manadatory elements", null, exampleTags);
#line 100
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 101
 testRunner.Given("I create an Appointment for Patient \"patient1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 102
  testRunner.And("I store the Created Appointment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 103
 testRunner.Given("I configure the default \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 104
  testRunner.And(string.Format("I set the Accept header to \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 105
 testRunner.When("I make the \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 106
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 107
  testRunner.And(string.Format("the response body should be FHIR {0}", bodyFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 108
  testRunner.And("the Response Resource should be an Appointment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 109
  testRunner.And("the Appointment Status should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 110
  testRunner.And("the Appointment Start should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 111
  testRunner.And("the Appointment End should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 112
  testRunner.And("the Appointment Slots should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 113
  testRunner.And("the Appointment Participants should be valid and resolvable", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 114
  testRunner.And("the Appointment Description must be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 115
  testRunner.And("the Appointment Created must be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 116
  testRunner.And("the Appointment Identifiers should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 117
  testRunner.And("the Appointment Priority should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 118
  testRunner.And("the Appointment Participant Type and Actor should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 119
  testRunner.And("the Response should contain the ETag header matching the Resource Version Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 120
  testRunner.And("the Appointment booking organization extension and contained resource must be val" +
                    "id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 121
  testRunner.And("the appointment reason must not be included", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read appointment valid response check caching headers exist")]
        public virtual void ReadAppointmentValidResponseCheckCachingHeadersExist()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read appointment valid response check caching headers exist", null, ((string[])(null)));
#line 127
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 128
 testRunner.Given("I create an Appointment for Patient \"patient1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 129
  testRunner.And("I store the Created Appointment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 130
 testRunner.Given("I configure the default \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 131
 testRunner.When("I make the \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 132
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 133
  testRunner.And("the Response Resource should be an Appointment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 134
  testRunner.And("the required cacheing headers should be present in the response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read appointment invalid response check caching headers exist")]
        public virtual void ReadAppointmentInvalidResponseCheckCachingHeadersExist()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read appointment invalid response check caching headers exist", null, ((string[])(null)));
#line 136
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 137
 testRunner.Given("I create an Appointment for Patient \"patient1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 138
  testRunner.And("I store the Created Appointment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 139
 testRunner.Given("I configure the default \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 140
  testRunner.And("I set the Read Operation logical identifier used in the request to \"555555\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 141
 testRunner.When("I make the \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 142
 testRunner.Then("the response status code should be \"404\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 143
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("CapabilityStatement profile supports the read appointment operation")]
        public virtual void CapabilityStatementProfileSupportsTheReadAppointmentOperation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("CapabilityStatement profile supports the read appointment operation", null, ((string[])(null)));
#line 147
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 148
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 149
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 150
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 151
  testRunner.And("the CapabilityStatement REST Resources should contain the \"Appointment\" Resource " +
                    "with the \"Read\" Interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read a patient’s appointments expecting servicecategory is populated")]
        [NUnit.Framework.CategoryAttribute("1.2.7-IncrementalAndRegression")]
        [NUnit.Framework.TestCaseAttribute("patient1", "true", "true", "true", null)]
        public virtual void ReadAPatientSAppointmentsExpectingServicecategoryIsPopulated(string patientName, string orgType, string deliveryChannel, string pracRole, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "1.2.7-IncrementalAndRegression"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read a patient’s appointments expecting servicecategory is populated", null, @__tags);
#line 155
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 156
 testRunner.Given("I create an Appointment in \"2\" days time for Patient \"patient1\" and Organization " +
                    "Code \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 157
  testRunner.And("I store the Created Appointment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 158
 testRunner.Given("I configure the default \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 159
 testRunner.When("I make the \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 160
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 161
  testRunner.And("the Response Resource should be an Appointment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 162
  testRunner.And("the Appointments returned must be in the future", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 163
  testRunner.And("the Appointment Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 164
  testRunner.And("the Appointment Metadata should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 165
  testRunner.And("the Appointment DeliveryChannel must be present", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 166
  testRunner.And("the Appointment PractitionerRole must be present", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 167
  testRunner.And("One Appointment contains serviceCategory element", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read a patient’s appointments expecting serviceType is populated")]
        [NUnit.Framework.CategoryAttribute("1.2.7-IncrementalAndRegression")]
        [NUnit.Framework.TestCaseAttribute("patient1", "true", "true", "true", null)]
        public virtual void ReadAPatientSAppointmentsExpectingServiceTypeIsPopulated(string patientName, string orgType, string deliveryChannel, string pracRole, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "1.2.7-IncrementalAndRegression"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read a patient’s appointments expecting serviceType is populated", null, @__tags);
#line 173
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 174
 testRunner.Given("I create an Appointment in \"2\" days time for Patient \"patient1\" and Organization " +
                    "Code \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 175
  testRunner.And("I store the Created Appointment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 176
 testRunner.Given("I configure the default \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 177
 testRunner.When("I make the \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 178
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 179
  testRunner.And("the Response Resource should be an Appointment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 180
  testRunner.And("the Appointments returned must be in the future", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 181
  testRunner.And("the Appointment Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 182
  testRunner.And("the Appointment Metadata should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 183
  testRunner.And("the Appointment DeliveryChannel must be present", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 184
  testRunner.And("the Appointment PractitionerRole must be present", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 185
  testRunner.And("One Appointment contains serviceType element", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Read appointment for patient where a HealthcareService was associated")]
        [NUnit.Framework.CategoryAttribute("1.2.8-Only")]
        [NUnit.Framework.TestCaseAttribute("HEALTHCARE2", null)]
        public virtual void ReadAppointmentForPatientWhereAHealthcareServiceWasAssociated(string healthCareService, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "1.2.8-Only"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Read appointment for patient where a HealthcareService was associated", null, @__tags);
#line 192
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 193
 testRunner.Given(string.Format("I set the Get Request Id to the Logical Identifier for Read Healthcare Service \"{" +
                        "0}\"", healthCareService), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 194
  testRunner.And("I configure the default \"HealthcareRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 195
  testRunner.When("I make the \"HealthcareRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 196
  testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 197
  testRunner.And("the Response Resource should be a Healthcare Service", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 198
  testRunner.And("the Healthcare Id should match the GET request Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 199
  testRunner.And("the Healthcare service should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 200
  testRunner.And("I Store the DOS id from the Healthcare service returned", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 201
 testRunner.Given("I get an existing patients nshNumber", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 202
  testRunner.And("I store the Patient", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 203
 testRunner.Given("I get Available Free Slots With a DOS Id in request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 204
  testRunner.Then("I Check that atleast One Slot is returned", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 205
 testRunner.Given("I store the Free Slots Bundle", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 206
  testRunner.Then("the Bundle Meta should be contain service filtering status set to \"enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 207
 testRunner.Given("I configure the default \"AppointmentCreate\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 208
  testRunner.And("I create an Appointment from the stored Patient and stored Schedule", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 209
  testRunner.When("I make the \"AppointmentCreate\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 210
 testRunner.Then("the response status code should indicate created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 211
  testRunner.And("the Response Resource should be an Appointment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 212
  testRunner.Given("I store the Created Appointment", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 213
 testRunner.Given("I configure the default \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 214
  testRunner.When("I make the \"AppointmentRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 215
  testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 216
  testRunner.And("the Appointment Participant Type and Actor should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 217
  testRunner.And("the Appointment Participant Actor should contains a HealthcareService Reference", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
