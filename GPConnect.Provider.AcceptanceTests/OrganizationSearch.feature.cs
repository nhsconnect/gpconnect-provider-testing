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
    [NUnit.Framework.DescriptionAttribute("OrganizationSearch")]
    [NUnit.Framework.CategoryAttribute("organization")]
    [NUnit.Framework.CategoryAttribute("1.5.0-Full-Pack")]
    public partial class OrganizationSearchFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "OrganizationSearch.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "OrganizationSearch", null, ProgrammingLanguage.CSharp, new string[] {
                        "organization",
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
        [NUnit.Framework.DescriptionAttribute("Organization search success")]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/Id/ods-organization-code", "unknownORG", "0", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/Id/ods-organization-code", "ORG1", "1", null)]
        public virtual void OrganizationSearchSuccess(string system, string value, string entries, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization search success", null, exampleTags);
#line 4
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 5
 testRunner.Given("I configure the default \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
  testRunner.And(string.Format("I add an Organization Identifier parameter with System \"{0}\" and Value \"{1}\"", system, value), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 7
 testRunner.When("I make the \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 8
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 9
  testRunner.And("the response should be a Bundle resource of type \"searchset\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
  testRunner.And(string.Format("the response bundle should contain \"{0}\" entries", entries), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
  testRunner.And("the response bundle Organization entries should contain a maximum of 1 \"https://f" +
                    "hir.nhs.uk/Id/ods-organization-code\" system identifier", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
  testRunner.And("the Organization Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
  testRunner.And("the Organization Name should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
  testRunner.And("the Organization Telecom should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
  testRunner.And("the Organization Address should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
  testRunner.And("the Organization Contact should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
  testRunner.And("the Organization Extensions should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
  testRunner.And("the Organization Not In Use should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization search by organization code successfully returns single result conta" +
            "ining the correct fields")]
        public virtual void OrganizationSearchByOrganizationCodeSuccessfullyReturnsSingleResultContainingTheCorrectFields()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization search by organization code successfully returns single result conta" +
                    "ining the correct fields", null, ((string[])(null)));
#line 40
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 41
 testRunner.Given("I configure the default \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 42
  testRunner.And("I add an Organization Identifier parameter with Organization Code System and Valu" +
                    "e \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 43
 testRunner.When("I make the \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 44
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 45
  testRunner.And("the response should be a Bundle resource of type \"searchset\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
  testRunner.And("the response bundle should contain \"1\" entries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 47
  testRunner.And("if the response bundle contains an organization resource it should contain meta d" +
                    "ata profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
  testRunner.And("an organization returned in the bundle has \"1\" \"https://fhir.nhs.uk/Id/ods-organi" +
                    "zation-code\" system identifier with \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization search by organization code successfully returns multiple results co" +
            "ntaining the correct fields")]
        public virtual void OrganizationSearchByOrganizationCodeSuccessfullyReturnsMultipleResultsContainingTheCorrectFields()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization search by organization code successfully returns multiple results co" +
                    "ntaining the correct fields", null, ((string[])(null)));
#line 50
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 51
 testRunner.Given("I configure the default \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 52
  testRunner.And("I add an Organization Identifier parameter with Organization Code System and Valu" +
                    "e \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 53
 testRunner.When("I make the \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 54
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 55
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 56
  testRunner.And("the response should be a Bundle resource of type \"searchset\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 57
  testRunner.And("the response bundle should contain \"1\" entries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
  testRunner.And("if the response bundle contains an organization resource it should contain meta d" +
                    "ata profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
  testRunner.And("an organization returned in the bundle has \"1\" \"https://fhir.nhs.uk/Id/ods-organi" +
                    "zation-code\" system identifier with \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization search failure due to no identifier parameter")]
        public virtual void OrganizationSearchFailureDueToNoIdentifierParameter()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization search failure due to no identifier parameter", null, ((string[])(null)));
#line 61
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 62
 testRunner.Given("I configure the default \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 63
 testRunner.When("I make the \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 64
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 65
  testRunner.And("the response should be a OperationOutcome resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization search add accept header to request and check for correct response f" +
            "ormat")]
        [NUnit.Framework.TestCaseAttribute("application/fhir+json", "JSON", "https://fhir.nhs.uk/Id/ods-organization-code", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+xml", "XML", "https://fhir.nhs.uk/Id/ods-organization-code", null)]
        public virtual void OrganizationSearchAddAcceptHeaderToRequestAndCheckForCorrectResponseFormat(string header, string bodyFormat, string system, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization search add accept header to request and check for correct response f" +
                    "ormat", null, exampleTags);
#line 67
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 68
 testRunner.Given("I configure the default \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 69
  testRunner.And(string.Format("I add an Organization Identifier parameter with System \"{0}\" and Value \"ORG1\"", system), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 70
  testRunner.And(string.Format("I set the Accept header to \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 71
 testRunner.When("I make the \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 72
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 73
  testRunner.And(string.Format("the response body should be FHIR {0}", bodyFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 74
  testRunner.And("the response should be a Bundle resource of type \"searchset\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 75
 testRunner.And("an organization returned in the bundle has \"1\" \"https://fhir.nhs.uk/Id/ods-organi" +
                    "zation-code\" system identifier with \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization search add _format parameter to request and check for correct respon" +
            "se format")]
        [NUnit.Framework.TestCaseAttribute("application/fhir+json", "JSON", "https://fhir.nhs.uk/Id/ods-organization-code", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+xml", "XML", "https://fhir.nhs.uk/Id/ods-organization-code", null)]
        public virtual void OrganizationSearchAdd_FormatParameterToRequestAndCheckForCorrectResponseFormat(string format, string bodyFormat, string system, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization search add _format parameter to request and check for correct respon" +
                    "se format", null, exampleTags);
#line 81
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 82
 testRunner.Given("I configure the default \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 83
  testRunner.And(string.Format("I add an Organization Identifier parameter with System \"{0}\" and Value \"ORG1\"", system), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 84
  testRunner.And("I do not send header \"Accept\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 85
  testRunner.And(string.Format("I add a Format parameter with the Value \"{0}\"", format), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 86
 testRunner.When("I make the \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 87
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 88
  testRunner.And(string.Format("the response body should be FHIR {0}", bodyFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 89
  testRunner.And("the response should be a Bundle resource of type \"searchset\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 90
  testRunner.And("an organization returned in the bundle has \"1\" \"https://fhir.nhs.uk/Id/ods-organi" +
                    "zation-code\" system identifier with \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization search add accept header and _format parameter to the request and ch" +
            "eck for correct response format")]
        [NUnit.Framework.TestCaseAttribute("application/fhir+json", "application/fhir+json", "JSON", "https://fhir.nhs.uk/Id/ods-organization-code", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+json", "application/fhir+xml", "XML", "https://fhir.nhs.uk/Id/ods-organization-code", null)]
        public virtual void OrganizationSearchAddAcceptHeaderAnd_FormatParameterToTheRequestAndCheckForCorrectResponseFormat(string header, string format, string bodyFormat, string system, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization search add accept header and _format parameter to the request and ch" +
                    "eck for correct response format", null, exampleTags);
#line 96
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 97
 testRunner.Given("I configure the default \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 98
  testRunner.And(string.Format("I add an Organization Identifier parameter with System \"{0}\" and Value \"ORG1\"", system), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 99
  testRunner.And(string.Format("I set the Accept header to \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 100
  testRunner.And(string.Format("I add a Format parameter with the Value \"{0}\"", format), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 101
 testRunner.When("I make the \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 102
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 103
  testRunner.And(string.Format("the response body should be FHIR {0}", bodyFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 104
  testRunner.And("the response should be a Bundle resource of type \"searchset\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 105
  testRunner.And("the response bundle should contain \"1\" entries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 106
  testRunner.And("an organization returned in the bundle has \"1\" \"https://fhir.nhs.uk/Id/ods-organi" +
                    "zation-code\" system identifier with \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization search add _format parameter to request before the identifer and che" +
            "ck for correct response format")]
        [NUnit.Framework.TestCaseAttribute("application/fhir+json", "JSON", "https://fhir.nhs.uk/Id/ods-organization-code", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+xml", "XML", "https://fhir.nhs.uk/Id/ods-organization-code", null)]
        public virtual void OrganizationSearchAdd_FormatParameterToRequestBeforeTheIdentiferAndCheckForCorrectResponseFormat(string format, string bodyFormat, string system, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization search add _format parameter to request before the identifer and che" +
                    "ck for correct response format", null, exampleTags);
#line 112
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 113
 testRunner.Given("I configure the default \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 114
  testRunner.And(string.Format("I add a Format parameter with the Value \"{0}\"", format), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 115
  testRunner.And(string.Format("I add an Organization Identifier parameter with System \"{0}\" and Value \"ORG1\"", system), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 116
 testRunner.When("I make the \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 117
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 118
  testRunner.And(string.Format("the response body should be FHIR {0}", bodyFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 119
  testRunner.And("the response should be a Bundle resource of type \"searchset\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 120
  testRunner.And("an organization returned in the bundle has \"1\" \"https://fhir.nhs.uk/Id/ods-organi" +
                    "zation-code\" system identifier with \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization search add _format parameter to request after the identifer and chec" +
            "k for correct response format")]
        [NUnit.Framework.TestCaseAttribute("application/fhir+json", "JSON", "https://fhir.nhs.uk/Id/ods-organization-code", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+xml", "XML", "https://fhir.nhs.uk/Id/ods-organization-code", null)]
        public virtual void OrganizationSearchAdd_FormatParameterToRequestAfterTheIdentiferAndCheckForCorrectResponseFormat(string format, string bodyFormat, string system, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization search add _format parameter to request after the identifer and chec" +
                    "k for correct response format", null, exampleTags);
#line 126
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 127
 testRunner.Given("I configure the default \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 128
  testRunner.And(string.Format("I add an Organization Identifier parameter with System \"{0}\" and Value \"ORG1\"", system), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 129
  testRunner.And(string.Format("I add a Format parameter with the Value \"{0}\"", format), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 130
 testRunner.When("I make the \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 131
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 132
  testRunner.And(string.Format("the response body should be FHIR {0}", bodyFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 133
  testRunner.And("the response should be a Bundle resource of type \"searchset\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("CapabilityStatement profile supports the Organization search operation")]
        public virtual void CapabilityStatementProfileSupportsTheOrganizationSearchOperation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("CapabilityStatement profile supports the Organization search operation", null, ((string[])(null)));
#line 139
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 140
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 141
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 142
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 143
  testRunner.And("the CapabilityStatement REST Resources should contain the \"Organization\" Resource" +
                    " with the \"SearchType\" Interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization search check organization response contains logical identifier")]
        public virtual void OrganizationSearchCheckOrganizationResponseContainsLogicalIdentifier()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization search check organization response contains logical identifier", null, ((string[])(null)));
#line 145
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 146
 testRunner.Given("I configure the default \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 147
  testRunner.And("I add an Organization Identifier parameter with System \"https://fhir.nhs.uk/Id/od" +
                    "s-organization-code\" and Value \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 148
 testRunner.When("I make the \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 149
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 150
  testRunner.And("the response should be a Bundle resource of type \"searchset\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 151
  testRunner.And("the Organization Identifiers should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization search include count and sort parameters")]
        public virtual void OrganizationSearchIncludeCountAndSortParameters()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization search include count and sort parameters", null, ((string[])(null)));
#line 153
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 154
 testRunner.Given("I configure the default \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 155
  testRunner.And("I add an Organization Identifier parameter with Organization Code System and Valu" +
                    "e \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 156
  testRunner.And("I add the parameter \"_count\" with the value \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 157
 testRunner.When("I make the \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 158
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 159
  testRunner.And("the response should be a Bundle resource of type \"searchset\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 160
  testRunner.And("the response bundle should contain \"1\" entries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization search valid response check caching headers exist")]
        public virtual void OrganizationSearchValidResponseCheckCachingHeadersExist()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization search valid response check caching headers exist", null, ((string[])(null)));
#line 162
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 163
 testRunner.Given("I configure the default \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 164
  testRunner.And("I add an Organization Identifier parameter with Organization Code System and Valu" +
                    "e \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 165
 testRunner.When("I make the \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 166
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 167
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 168
  testRunner.And("the response should be a Bundle resource of type \"searchset\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 169
  testRunner.And("the response bundle should contain \"1\" entries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 170
  testRunner.And("if the response bundle contains an organization resource it should contain meta d" +
                    "ata profile and version id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 171
  testRunner.And("an organization returned in the bundle has \"1\" \"https://fhir.nhs.uk/Id/ods-organi" +
                    "zation-code\" system identifier with \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 172
  testRunner.And("the required cacheing headers should be present in the response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Organization search invalid response check caching headers exist")]
        public virtual void OrganizationSearchInvalidResponseCheckCachingHeadersExist()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Organization search invalid response check caching headers exist", null, ((string[])(null)));
#line 177
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 178
 testRunner.Given("I get the Organization for Organization Code \"ORG1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 179
  testRunner.And("I store the Organization", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 180
 testRunner.Given("I configure the default \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 181
  testRunner.And("I set the Interaction Id header to \"urn:nhs:names:services:gpconnect:fhir:rest:re" +
                    "ad:practitioner-1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 182
 testRunner.When("I make the \"OrganizationSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 183
 testRunner.Then("the response status code should be \"400\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 184
  testRunner.And("the response should be a OperationOutcome resource with error code \"BAD_REQUEST\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 185
  testRunner.And("the required cacheing headers should be present in the response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
