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
    [NUnit.Framework.DescriptionAttribute("PractitionerSearch")]
    [NUnit.Framework.CategoryAttribute("practitioner")]
    [NUnit.Framework.CategoryAttribute("1.3.2-Full_Pack")]
    public partial class PractitionerSearchFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "PractitionerSearch.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "PractitionerSearch", null, ProgrammingLanguage.CSharp, new string[] {
                        "practitioner",
                        "1.3.2-Full_Pack"});
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
        [NUnit.Framework.DescriptionAttribute("Practitioner search success and validate the practitioner identifiers")]
        [NUnit.Framework.CategoryAttribute("1.2.3")]
        [NUnit.Framework.TestCaseAttribute("practitioner1", "1", "0", null)]
        [NUnit.Framework.TestCaseAttribute("practitioner2", "1", "1", null)]
        [NUnit.Framework.TestCaseAttribute("practitioner3", "1", "2", null)]
        public virtual void PractitionerSearchSuccessAndValidateThePractitionerIdentifiers(string value, string entrySize, string roleSize, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "1.2.3"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Practitioner search success and validate the practitioner identifiers", null, @__tags);
#line 8
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 9
 testRunner.Given("I configure the default \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 10
  testRunner.And(string.Format("I add a Practitioner Identifier parameter with SDS User Id System and Value \"{0}\"" +
                        "", value), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
 testRunner.When("I make the \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 12
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 13
  testRunner.And(string.Format("the response bundle should contain \"{0}\" entries", entrySize), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
  testRunner.And("the response should be a Bundle resource of type \"searchset\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
  testRunner.And("the Practitioner Id should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
  testRunner.And("the Practitioner Identifiers should be valid fixed values", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
  testRunner.And(string.Format("the Practitioner SDS User Identifier should be valid for Value \"{0}\"", value), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 18
  testRunner.And(string.Format("the Practitioner SDS Role Profile Identifier should be valid for \"{0}\" Role Profi" +
                        "le Identifiers", roleSize), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Practitioner search with failure due to invalid identifier")]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/Id/sds-user-id", "", null)]
        [NUnit.Framework.TestCaseAttribute("", "practitioner2", null)]
        public virtual void PractitionerSearchWithFailureDueToInvalidIdentifier(string system, string value, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Practitioner search with failure due to invalid identifier", null, exampleTags);
#line 25
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 26
 testRunner.Given("I configure the default \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 27
  testRunner.And(string.Format("I add a Practitioner Identifier parameter with System \"{0}\" and Value \"{1}\"", system, value), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 28
 testRunner.When("I make the \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 29
 testRunner.Then("the response status code should be \"422\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 30
  testRunner.And("the response should be a OperationOutcome resource with error code \"INVALID_PARAM" +
                    "ETER\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Practitioner search without the identifier parameter")]
        public virtual void PractitionerSearchWithoutTheIdentifierParameter()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Practitioner search without the identifier parameter", null, ((string[])(null)));
#line 36
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 37
 testRunner.Given("I configure the default \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 38
 testRunner.When("I make the \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 39
 testRunner.Then("the response status code should indicate failure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 40
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 41
  testRunner.And("the response should be a OperationOutcome resource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Practitioner search where identifier contains the incorrect case or spelling")]
        [NUnit.Framework.TestCaseAttribute("Identifier", null)]
        public virtual void PractitionerSearchWhereIdentifierContainsTheIncorrectCaseOrSpelling(string parameterName, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Practitioner search where identifier contains the incorrect case or spelling", null, exampleTags);
#line 43
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 44
 testRunner.Given("I configure the default \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 45
  testRunner.And(string.Format("I add a Practitioner \"{0}\" parameter with System \"https://fhir.nhs.uk/Id/sds-user" +
                        "-id\" and Value \"practitioner2\"", parameterName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
 testRunner.When("I make the \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 47
 testRunner.Then("the response status code should be \"400\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 48
  testRunner.And("the response should be a OperationOutcome resource with error code \"BAD_REQUEST\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Practitioner search testing paramater validity before adding identifier")]
        [NUnit.Framework.TestCaseAttribute("_format", "application/fhir+json", "JSON", null)]
        [NUnit.Framework.TestCaseAttribute("_format", "application/fhir+xml", "XML", null)]
        public virtual void PractitionerSearchTestingParamaterValidityBeforeAddingIdentifier(string param1Name, string param1Value, string bodyFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Practitioner search testing paramater validity before adding identifier", null, exampleTags);
#line 53
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 54
 testRunner.Given("I configure the default \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 55
  testRunner.And(string.Format("I add the parameter \"{0}\" with the value \"{1}\"", param1Name, param1Value), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 56
  testRunner.And("I add a Practitioner Identifier parameter with SDS User Id System and Value \"prac" +
                    "titioner2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 57
 testRunner.When("I make the \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 58
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 59
  testRunner.And(string.Format("the response body should be FHIR {0}", bodyFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 60
  testRunner.And("the response should be a Bundle resource of type \"searchset\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 61
  testRunner.And("the Practitioner Identifiers should be valid fixed values", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 62
  testRunner.And("the Practitioner Name should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 63
  testRunner.And("the Practitioner should exclude disallowed elements", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 64
  testRunner.And("the Practitioner nhsCommunication should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Practitioner search testing paramater validity after adding identifier")]
        [NUnit.Framework.TestCaseAttribute("_format", "application/fhir+json", "JSON", null)]
        [NUnit.Framework.TestCaseAttribute("_format", "application/fhir+xml", "XML", null)]
        public virtual void PractitionerSearchTestingParamaterValidityAfterAddingIdentifier(string param1Name, string param1Value, string bodyFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Practitioner search testing paramater validity after adding identifier", null, exampleTags);
#line 70
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 71
 testRunner.Given("I configure the default \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 72
  testRunner.And("I add a Practitioner Identifier parameter with SDS User Id System and Value \"prac" +
                    "titioner2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 73
  testRunner.And(string.Format("I add the parameter \"{0}\" with the value \"{1}\"", param1Name, param1Value), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 74
 testRunner.When("I make the \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 75
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 76
  testRunner.And(string.Format("the response body should be FHIR {0}", bodyFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
  testRunner.And("the response should be a Bundle resource of type \"searchset\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 78
  testRunner.And("the Practitioner Identifiers should be valid fixed values", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 79
  testRunner.And("the Practitioner Name should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 80
  testRunner.And("the Practitioner should exclude disallowed elements", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 81
  testRunner.And("the Practitioner nhsCommunication should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Practitioner search add accept header to request and check for correct response f" +
            "ormat")]
        [NUnit.Framework.TestCaseAttribute("application/fhir+json", "JSON", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+xml", "XML", null)]
        public virtual void PractitionerSearchAddAcceptHeaderToRequestAndCheckForCorrectResponseFormat(string header, string bodyFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Practitioner search add accept header to request and check for correct response f" +
                    "ormat", null, exampleTags);
#line 88
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 89
 testRunner.Given("I configure the default \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 90
  testRunner.And("I add a Practitioner Identifier parameter with SDS User Id System and Value \"prac" +
                    "titioner2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 91
  testRunner.And(string.Format("I set the Accept header to \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 92
 testRunner.When("I make the \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 93
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 94
  testRunner.And(string.Format("the response body should be FHIR {0}", bodyFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 95
  testRunner.And("the response should be a Bundle resource of type \"searchset\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 96
  testRunner.And("the Practitioner Identifiers should be valid fixed values", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 97
  testRunner.And("the Practitioner Name should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 98
  testRunner.And("the Practitioner should exclude disallowed elements", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 99
  testRunner.And("the Practitioner nhsCommunication should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Practitioner search add accept header and _format parameter to the request and ch" +
            "eck for correct response format")]
        [NUnit.Framework.TestCaseAttribute("application/fhir+json", "application/fhir+json", "JSON", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+json", "application/fhir+xml", "XML", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+xml", "application/fhir+json", "JSON", null)]
        [NUnit.Framework.TestCaseAttribute("application/fhir+xml", "application/fhir+xml", "XML", null)]
        public virtual void PractitionerSearchAddAcceptHeaderAnd_FormatParameterToTheRequestAndCheckForCorrectResponseFormat(string header, string parameter, string bodyFormat, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Practitioner search add accept header and _format parameter to the request and ch" +
                    "eck for correct response format", null, exampleTags);
#line 105
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 106
 testRunner.Given("I configure the default \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 107
  testRunner.And("I add a Practitioner Identifier parameter with SDS User Id System and Value \"prac" +
                    "titioner2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 108
  testRunner.And(string.Format("I set the Accept header to \"{0}\"", header), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 109
  testRunner.And(string.Format("I add a Format parameter with the Value \"{0}\"", parameter), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 110
 testRunner.When("I make the \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 111
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 112
  testRunner.And(string.Format("the response body should be FHIR {0}", bodyFormat), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 113
  testRunner.And("the response should be a Bundle resource of type \"searchset\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 114
  testRunner.And("the Practitioner Metadata should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 115
  testRunner.And("the Practitioner Identifiers should be valid fixed values", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 116
  testRunner.And("the Practitioner Name should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 117
  testRunner.And("the Practitioner should exclude disallowed elements", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 118
  testRunner.And("the Practitioner nhsCommunication should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Practitioner search multiple identifier parameter failure")]
        [NUnit.Framework.CategoryAttribute("1.2.3")]
        public virtual void PractitionerSearchMultipleIdentifierParameterFailure()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Practitioner search multiple identifier parameter failure", null, new string[] {
                        "1.2.3"});
#line 127
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 128
 testRunner.Given("I configure the default \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 129
  testRunner.And("I add a Practitioner Identifier parameter with SDS User Id System and Value \"prac" +
                    "titioner2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 130
  testRunner.And("I add a Practitioner Identifier parameter with SDS User Id System and Value \"prac" +
                    "titioner2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 131
 testRunner.When("I make the \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 132
 testRunner.Then("the response status code should be \"400\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 133
  testRunner.And("the response should be a OperationOutcome resource with error code \"BAD_REQUEST\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Practitioner search multiple multiple identifiers for different practitioner para" +
            "meter failure")]
        public virtual void PractitionerSearchMultipleMultipleIdentifiersForDifferentPractitionerParameterFailure()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Practitioner search multiple multiple identifiers for different practitioner para" +
                    "meter failure", null, ((string[])(null)));
#line 135
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 136
 testRunner.Given("I configure the default \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 137
  testRunner.And("I add a Practitioner Identifier parameter with SDS User Id System and Value \"prac" +
                    "titioner1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 138
  testRunner.And("I add a Practitioner Identifier parameter with SDS User Id System and Value \"prac" +
                    "titioner2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 139
 testRunner.When("I make the \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 140
 testRunner.Then("the response status code should be \"400\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 141
  testRunner.And("the response should be a OperationOutcome resource with error code \"BAD_REQUEST\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Practitioner search include count and sort parameters")]
        public virtual void PractitionerSearchIncludeCountAndSortParameters()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Practitioner search include count and sort parameters", null, ((string[])(null)));
#line 143
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 144
 testRunner.Given("I configure the default \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 145
  testRunner.And("I add a Practitioner Identifier parameter with SDS User Id System and Value \"prac" +
                    "titioner1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 146
  testRunner.And("I add the parameter \"_sort\" with the value \"practitioner.coding\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 147
  testRunner.And("I add the parameter \"_count\" with the value \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 148
 testRunner.When("I make the \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 149
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 150
  testRunner.And("the response bundle should contain \"1\" entries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 151
  testRunner.And("the response should be a Bundle resource of type \"searchset\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("CapabilityStatement profile supports the Practitioner search operation")]
        public virtual void CapabilityStatementProfileSupportsThePractitionerSearchOperation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("CapabilityStatement profile supports the Practitioner search operation", null, ((string[])(null)));
#line 153
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 154
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 155
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 156
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 157
  testRunner.And("the CapabilityStatement REST Resources should contain the \"Practitioner\" Resource" +
                    " with the \"SearchType\" Interaction", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Practitioner search valid response check caching headers exist")]
        public virtual void PractitionerSearchValidResponseCheckCachingHeadersExist()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Practitioner search valid response check caching headers exist", null, ((string[])(null)));
#line 159
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 160
 testRunner.Given("I configure the default \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 161
  testRunner.And("I add a Practitioner Identifier parameter with SDS User Id System and Value \"prac" +
                    "titioner1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 162
 testRunner.When("I make the \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 163
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 164
  testRunner.And("the response should be a Bundle resource of type \"searchset\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 165
  testRunner.And("the required cacheing headers should be present in the response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Practitioner search invalid response check caching headers exist")]
        public virtual void PractitionerSearchInvalidResponseCheckCachingHeadersExist()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Practitioner search invalid response check caching headers exist", null, ((string[])(null)));
#line 167
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 168
 testRunner.Given("I configure the default \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 169
  testRunner.And("I set the Interaction Id header to \"urn:nhs:names:services:gpconnect:fhir:operati" +
                    "on:gpc.getcarerecord\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 170
  testRunner.And("I add a Practitioner Identifier parameter with SDS User Id System and Value \"prac" +
                    "titioner2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 171
 testRunner.When("I make the \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 172
 testRunner.Then("the response status code should be \"400\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 173
  testRunner.And("the response should be a OperationOutcome resource with error code \"BAD_REQUEST\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 174
  testRunner.And("the required cacheing headers should be present in the response", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Practitioner search returned should conform to the GPconnect specification")]
        public virtual void PractitionerSearchReturnedShouldConformToTheGPconnectSpecification()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Practitioner search returned should conform to the GPconnect specification", null, ((string[])(null)));
#line 178
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 179
 testRunner.Given("I configure the default \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 180
  testRunner.And("I add a Practitioner Identifier parameter with SDS User Id System and Value \"prac" +
                    "titioner1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 181
 testRunner.When("I make the \"PractitionerSearch\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 182
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 183
  testRunner.And("the response bundle should contain \"1\" entries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 184
  testRunner.And("the response should be a Bundle resource of type \"searchset\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 185
  testRunner.And("the Practitioner Not In Use should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
