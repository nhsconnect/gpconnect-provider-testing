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
    [NUnit.Framework.DescriptionAttribute("OrganizationRead")]
    [NUnit.Framework.CategoryAttribute("organization")]
    public partial class OrganizationReadFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "OrganizationRead.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "OrganizationRead", null, ProgrammingLanguage.CSharp, new string[] {
                        "organization"});
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
        [NUnit.Framework.DescriptionAttribute("Organization Read successful request validate all of response")]
        public virtual void OrganizationReadSuccessfulRequestValidateAllOfResponse()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization Read successful request validate all of response", ((string[])(null)));
#line 4
this.ScenarioSetup(scenarioInfo);
#line 5
 testRunner.Given("I get the Organization for Organization Code \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
  testRunner.And("I store the Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 7
 testRunner.Given("I configure the default \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 8
  testRunner.And("I set the JWT Requested Record Id to the Request Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
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
  testRunner.And("the Organization Type should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 18
  testRunner.And("the Organization Name should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 19
  testRunner.And("the Organization Telecom should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
  testRunner.And("the Organization Address should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
  testRunner.And("the Organization Contact should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
  testRunner.And("the Organization Extensions should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization Read with valid identifier which does not exist on providers system")]
        [NUnit.Framework.TestCaseAttribute("ddBm", new string[0])]
        [NUnit.Framework.TestCaseAttribute("1Zcr4", new string[0])]
        [NUnit.Framework.TestCaseAttribute("z.as.e", new string[0])]
        [NUnit.Framework.TestCaseAttribute("1.1445.23", new string[0])]
        [NUnit.Framework.TestCaseAttribute("40-9223", new string[0])]
        [NUnit.Framework.TestCaseAttribute("nc-sfem.mks--s", new string[0])]
        public virtual void OrganizationReadWithValidIdentifierWhichDoesNotExistOnProvidersSystem(string logicalId, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization Read with valid identifier which does not exist on providers system", exampleTags);
#line 26
this.ScenarioSetup(scenarioInfo);
#line 27
 testRunner.Given("I configure the default \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 28
  testRunner.And(string.Format("I set the Read Operation logical identifier used in the request to \"{0}\"", logicalId), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 29
  testRunner.And("I set the JWT Requested Record Id to the Request Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
 testRunner.When("I make the \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 31
 testRunner.Then("the response status code should be \"404\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization Read with invalid resource path in URL")]
        [NUnit.Framework.TestCaseAttribute("Organizations", new string[0])]
        [NUnit.Framework.TestCaseAttribute("Organization!", new string[0])]
        [NUnit.Framework.TestCaseAttribute("Organization2", new string[0])]
        [NUnit.Framework.TestCaseAttribute("organizations", new string[0])]
        public virtual void OrganizationReadWithInvalidResourcePathInURL(string relativePath, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization Read with invalid resource path in URL", exampleTags);
#line 41
this.ScenarioSetup(scenarioInfo);
#line 42
 testRunner.Given("I get the Organization for Organization Code \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 43
  testRunner.And("I store the Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
 testRunner.Given("I configure the default \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 45
  testRunner.And(string.Format("I set the Read Operation relative path to \"{0}\" and append the resource logical i" +
                        "dentifier", relativePath), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
  testRunner.And("I set the JWT Requested Record Id to the Request Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 47
 testRunner.When("I make the \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 48
 testRunner.Then("the response status code should be \"404\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization Read with missing mandatory header")]
        [NUnit.Framework.TestCaseAttribute("Ssp-TraceID", new string[0])]
        [NUnit.Framework.TestCaseAttribute("Ssp-From", new string[0])]
        [NUnit.Framework.TestCaseAttribute("Ssp-To", new string[0])]
        [NUnit.Framework.TestCaseAttribute("Ssp-InteractionId", new string[0])]
        [NUnit.Framework.TestCaseAttribute("Authorization", new string[0])]
        public virtual void OrganizationReadWithMissingMandatoryHeader(string header, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization Read with missing mandatory header", exampleTags);
#line 56
this.ScenarioSetup(scenarioInfo);
#line 57
 testRunner.Given("I get the Organization for Organization Code \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 58
  testRunner.And("I store the Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
 testRunner.Given("I configure the default \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 60
  testRunner.And("I set the JWT Requested Record Id to the Request Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 61
 testRunner.When(string.Format("I make the \"OrganizationRead\" request with missing Header \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 62
 testRunner.Then("the response status code should be \"400\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 63
  testRunner.And("the response should be a OperationOutcome resource with error code \"BAD_REQUEST\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization Read with incorrect interaction id")]
        [NUnit.Framework.TestCaseAttribute("urn:nhs:names:servic3es:gpconnect:fhir:rest:read:practitioner", new string[0])]
        [NUnit.Framework.TestCaseAttribute("urn:nhs:names:services:gpconnect:fhir:rest:read:practitioners", new string[0])]
        [NUnit.Framework.TestCaseAttribute("urn:nhs:names:services:gpconnect:gpc.getcarerecord", new string[0])]
        [NUnit.Framework.TestCaseAttribute("", new string[0])]
        [NUnit.Framework.TestCaseAttribute("null", new string[0])]
        public virtual void OrganizationReadWithIncorrectInteractionId(string interactionId, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization Read with incorrect interaction id", exampleTags);
#line 72
this.ScenarioSetup(scenarioInfo);
#line 73
 testRunner.Given("I get the Organization for Organization Code \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 74
  testRunner.And("I store the Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 75
 testRunner.Given("I configure the default \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 76
  testRunner.And(string.Format("I set the Interaction Id header to \"{0}\"", interactionId), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
  testRunner.And("I set the JWT Requested Record Id to the Request Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 78
 testRunner.When("I make the \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 79
 testRunner.Then("the response status code should be \"400\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 80
  testRunner.And("the response should be a OperationOutcome resource with error code \"BAD_REQUEST\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization Read using the _format parameter to request response format")]
        [NUnit.Framework.TestCaseAttribute("application/json+fhir", "JSON", new string[0])]
        [NUnit.Framework.TestCaseAttribute("application/xml+fhir", "XML", new string[0])]
        public virtual void OrganizationReadUsingThe_FormatParameterToRequestResponseFormat(string parameter, string responseFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization Read using the _format parameter to request response format", exampleTags);
#line 89
this.ScenarioSetup(scenarioInfo);
#line 90
 testRunner.Given("I get the Organization for Organization Code \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 91
  testRunner.And("I store the Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 92
 testRunner.Given("I configure the default \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 93
  testRunner.And(string.Format("I add the parameter \"_format\" with the value \"{0}\"", parameter), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 94
  testRunner.And("I set the JWT Requested Record Id to the Request Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 95
 testRunner.When("I make the \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 96
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 97
  testRunner.And(string.Format("the response should be the format FHIR {0}", responseFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 98
  testRunner.And("the Response Resource should be an Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 99
  testRunner.And("the Organization Identifiers should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 100
  testRunner.And("the Organization Id should equal the Request Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 101
  testRunner.And("the Organization Identifiers should be valid for Organization \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization Read using the Accept header to request response format")]
        [NUnit.Framework.TestCaseAttribute("application/json+fhir", "JSON", new string[0])]
        [NUnit.Framework.TestCaseAttribute("application/xml+fhir", "XML", new string[0])]
        public virtual void OrganizationReadUsingTheAcceptHeaderToRequestResponseFormat(string header, string responseFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization Read using the Accept header to request response format", exampleTags);
#line 107
this.ScenarioSetup(scenarioInfo);
#line 108
 testRunner.Given("I get the Organization for Organization Code \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 109
  testRunner.And("I store the Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 110
 testRunner.Given("I configure the default \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 111
  testRunner.And(string.Format("I set the Accept header to \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 112
  testRunner.And("I set the JWT Requested Record Id to the Request Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 113
 testRunner.When("I make the \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 114
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 115
  testRunner.And(string.Format("the response should be the format FHIR {0}", responseFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 116
  testRunner.And("the Response Resource should be an Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 117
  testRunner.And("the Organization Id should equal the Request Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 118
  testRunner.And("the Organization Identifiers should be valid for Organization \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization Read sending the Accept header and _format parameter to request resp" +
            "onse format")]
        [NUnit.Framework.TestCaseAttribute("application/json+fhir", "application/json+fhir", "JSON", new string[0])]
        [NUnit.Framework.TestCaseAttribute("application/json+fhir", "application/xml+fhir", "XML", new string[0])]
        [NUnit.Framework.TestCaseAttribute("application/xml+fhir", "application/json+fhir", "JSON", new string[0])]
        [NUnit.Framework.TestCaseAttribute("application/xml+fhir", "application/xml+fhir", "XML", new string[0])]
        public virtual void OrganizationReadSendingTheAcceptHeaderAnd_FormatParameterToRequestResponseFormat(string header, string parameter, string responseFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization Read sending the Accept header and _format parameter to request resp" +
                    "onse format", exampleTags);
#line 124
this.ScenarioSetup(scenarioInfo);
#line 125
 testRunner.Given("I get the Organization for Organization Code \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 126
  testRunner.And("I store the Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 127
 testRunner.Given("I configure the default \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 128
  testRunner.And(string.Format("I set the Accept header to \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 129
  testRunner.And(string.Format("I add the parameter \"_format\" with the value \"{0}\"", parameter), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 130
  testRunner.And("I set the JWT Requested Record Id to the Request Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 131
 testRunner.When("I make the \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 132
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 133
  testRunner.And(string.Format("the response should be the format FHIR {0}", responseFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 134
  testRunner.And("the Response Resource should be an Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 135
  testRunner.And("the Organization Id should equal the Request Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 136
  testRunner.And("the Organization Identifiers should be valid for Organization \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("CapabilityStatement profile supports the Organization read operation")]
        public virtual void CapabilityStatementProfileSupportsTheOrganizationReadOperation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("CapabilityStatement profile supports the Organization read operation", ((string[])(null)));
#line 144
this.ScenarioSetup(scenarioInfo);
#line 145
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 146
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 147
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 148
  testRunner.And("the CapabilityStatement REST Resources should contain the \"Organization\" Resource" +
                    " with the \"Read\" Interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization read valid response check caching headers exist")]
        public virtual void OrganizationReadValidResponseCheckCachingHeadersExist()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization read valid response check caching headers exist", ((string[])(null)));
#line 150
this.ScenarioSetup(scenarioInfo);
#line 151
 testRunner.Given("I get the Organization for Organization Code \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 152
  testRunner.And("I store the Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 153
 testRunner.Given("I configure the default \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 154
  testRunner.And("I set the JWT Requested Record Id to the Request Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 155
 testRunner.When("I make the \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 156
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 157
  testRunner.And("the Response should contain the ETag header matching the Resource Version Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 158
  testRunner.And("the Response Resource should be an Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 159
  testRunner.And("the required cacheing headers should be present in the response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization read invalid response check caching headers exist")]
        public virtual void OrganizationReadInvalidResponseCheckCachingHeadersExist()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization read invalid response check caching headers exist", ((string[])(null)));
#line 161
this.ScenarioSetup(scenarioInfo);
#line 162
 testRunner.Given("I get the Organization for Organization Code \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 163
  testRunner.And("I store the Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 164
 testRunner.Given("I configure the default \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 165
  testRunner.And("I set the Interaction Id header to \"urn:nhs:names:services:gpconnect:fhir:rest:re" +
                    "ad:practitioner3\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 166
  testRunner.And("I set the JWT Requested Record Id to the Request Id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 167
 testRunner.When("I make the \"OrganizationRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 168
 testRunner.Then("the response status code should be \"400\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 169
  testRunner.And("the response should be a OperationOutcome resource with error code \"BAD_REQUEST\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 170
  testRunner.And("the required cacheing headers should be present in the response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
