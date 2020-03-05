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
    [NUnit.Framework.DescriptionAttribute("OrganizationRead")]
    [NUnit.Framework.CategoryAttribute("organization")]
    [NUnit.Framework.CategoryAttribute("1.3.2-Full-Pack")]
    public partial class OrganizationReadFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "OrganizationRead.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "OrganizationRead", null, ProgrammingLanguage.CSharp, new string[] {
                        "organization",
                        "1.3.2-Full-Pack"});
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
        [NUnit.Framework.DescriptionAttribute("Organization Read successful request validate all of response")]
        [NUnit.Framework.CategoryAttribute("1.2.3")]
        public virtual void OrganizationReadSuccessfulRequestValidateAllOfResponse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization Read successful request validate all of response", null, new string[] {
                        "1.2.3"});
#line 5
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 6
 testRunner.Given("I get the Organization for Organization Code \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 7
  testRunner.And("I store the Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 8
 testRunner.Given("I configure the default \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
 testRunner.When("I make the \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 10
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 11
  testRunner.And("the Response should contain the ETag header matching the Resource Version Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
  testRunner.And("the Response Resource should be an Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
  testRunner.And("the Organization Identifiers should be valid for Organization \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
  testRunner.And("the Organization Id should equal the Request Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
  testRunner.And("the Organization Metadata should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
  testRunner.And("the Organization PartOf Organization should be resolvable", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
  testRunner.And("the Organization Name should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 18
  testRunner.And("the Organization Telecom should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 19
  testRunner.And("the Organization Address should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
  testRunner.And("the Organization Contact should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
  testRunner.And("the Organization Extensions should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
  testRunner.And("the Organization Not In Use should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization Read with valid identifier which does not exist on providers system")]
        [NUnit.Framework.TestCaseAttribute("ddBm", null)]
        [NUnit.Framework.TestCaseAttribute("1Zcr4", null)]
        [NUnit.Framework.TestCaseAttribute("z.as.e", null)]
        [NUnit.Framework.TestCaseAttribute("1.1445.23", null)]
        [NUnit.Framework.TestCaseAttribute("40-9223", null)]
        [NUnit.Framework.TestCaseAttribute("nc-sfem.mks--s", null)]
        public virtual void OrganizationReadWithValidIdentifierWhichDoesNotExistOnProvidersSystem(string logicalId, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization Read with valid identifier which does not exist on providers system", null, exampleTags);
#line 27
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 28
 testRunner.Given("I configure the default \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 29
  testRunner.And(string.Format("I set the Read Operation logical identifier used in the request to \"{0}\"", logicalId), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
 testRunner.When("I make the \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 31
 testRunner.Then("the response status code should be \"404\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 32
 testRunner.And("the response should be a OperationOutcome resource with error code \"ORGANISATION_" +
                    "NOT_FOUND\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization Read using the _format parameter to request response format")]
        [NUnit.Framework.TestCaseAttribute("application/fhir+json", "JSON", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+xml", "XML", null)]
        public virtual void OrganizationReadUsingThe_FormatParameterToRequestResponseFormat(string parameter, string responseFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization Read using the _format parameter to request response format", null, exampleTags);
#line 42
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 43
 testRunner.Given("I get the Organization for Organization Code \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 44
  testRunner.And("I store the Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 45
 testRunner.Given("I configure the default \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 46
  testRunner.And(string.Format("I add the parameter \"_format\" with the value \"{0}\"", parameter), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 47
 testRunner.When("I make the \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 48
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 49
  testRunner.And(string.Format("the response should be the format FHIR {0}", responseFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
  testRunner.And("the Response Resource should be an Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 51
  testRunner.And("the Organization Identifiers should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 52
  testRunner.And("the Organization Id should equal the Request Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 53
  testRunner.And("the Organization Identifiers should be valid for Organization \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization Read using the Accept header to request response format")]
        [NUnit.Framework.TestCaseAttribute("application/fhir+json", "JSON", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+xml", "XML", null)]
        public virtual void OrganizationReadUsingTheAcceptHeaderToRequestResponseFormat(string header, string responseFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization Read using the Accept header to request response format", null, exampleTags);
#line 59
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 60
 testRunner.Given("I get the Organization for Organization Code \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 61
  testRunner.And("I store the Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 62
 testRunner.Given("I configure the default \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 63
  testRunner.And(string.Format("I set the Accept header to \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 64
 testRunner.When("I make the \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 65
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 66
  testRunner.And(string.Format("the response should be the format FHIR {0}", responseFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
  testRunner.And("the Response Resource should be an Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 68
  testRunner.And("the Organization Id should equal the Request Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 69
  testRunner.And("the Organization Identifiers should be valid for Organization \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization Read sending the Accept header and _format parameter to request resp" +
            "onse format")]
        [NUnit.Framework.TestCaseAttribute("application/fhir+json", "application/fhir+json", "JSON", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+json", "application/fhir+xml", "XML", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+xml", "application/fhir+json", "JSON", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+xml", "application/fhir+xml", "XML", null)]
        public virtual void OrganizationReadSendingTheAcceptHeaderAnd_FormatParameterToRequestResponseFormat(string header, string parameter, string responseFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization Read sending the Accept header and _format parameter to request resp" +
                    "onse format", null, exampleTags);
#line 75
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 76
 testRunner.Given("I get the Organization for Organization Code \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 77
  testRunner.And("I store the Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 78
 testRunner.Given("I configure the default \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 79
  testRunner.And(string.Format("I set the Accept header to \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 80
  testRunner.And(string.Format("I add the parameter \"_format\" with the value \"{0}\"", parameter), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 81
 testRunner.When("I make the \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 82
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 83
  testRunner.And(string.Format("the response should be the format FHIR {0}", responseFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 84
  testRunner.And("the Response Resource should be an Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 85
  testRunner.And("the Organization Id should equal the Request Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 86
  testRunner.And("the Organization Identifiers should be valid for Organization \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("CapabilityStatement profile supports the Organization read operation")]
        public virtual void CapabilityStatementProfileSupportsTheOrganizationReadOperation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("CapabilityStatement profile supports the Organization read operation", null, ((string[])(null)));
#line 94
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 95
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 96
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 97
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 98
  testRunner.And("the CapabilityStatement REST Resources should contain the \"Organization\" Resource" +
                    " with the \"Read\" Interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization read valid response check caching headers exist")]
        public virtual void OrganizationReadValidResponseCheckCachingHeadersExist()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization read valid response check caching headers exist", null, ((string[])(null)));
#line 100
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 101
 testRunner.Given("I get the Organization for Organization Code \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 102
  testRunner.And("I store the Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 103
 testRunner.Given("I configure the default \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 104
 testRunner.When("I make the \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 105
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 106
  testRunner.And("the Response should contain the ETag header matching the Resource Version Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 107
  testRunner.And("the Response Resource should be an Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 108
  testRunner.And("the required cacheing headers should be present in the response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization read invalid response check caching headers exist")]
        public virtual void OrganizationReadInvalidResponseCheckCachingHeadersExist()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization read invalid response check caching headers exist", null, ((string[])(null)));
#line 110
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 111
 testRunner.Given("I get the Organization for Organization Code \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 112
  testRunner.And("I store the Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 113
 testRunner.Given("I configure the default \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 116
  testRunner.And("I set the Interaction Id header to \"urn:nhs:names:services:gpconnect:fhir:rest:re" +
                    "ad:practitioner-1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 117
 testRunner.When("I make the \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 118
 testRunner.Then("the response status code should be \"400\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 119
  testRunner.And("the response should be a OperationOutcome resource with error code \"BAD_REQUEST\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 120
  testRunner.And("the required cacheing headers should be present in the response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
