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
    [NUnit.Framework.DescriptionAttribute("FHIR")]
    [NUnit.Framework.CategoryAttribute("fhir")]
    [NUnit.Framework.CategoryAttribute("1.3.2-Full-Pack")]
    public partial class FHIRFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "FHIR.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "FHIR", null, ProgrammingLanguage.CSharp, new string[] {
                        "fhir",
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
        [NUnit.Framework.DescriptionAttribute("Fhir Get MetaData")]
        [NUnit.Framework.CategoryAttribute("1.3.2-IncrementalAndRegression")]
        public virtual void FhirGetMetaData()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Fhir Get MetaData", null, new string[] {
                        "1.3.2-IncrementalAndRegression"});
#line 5
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 6
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 7
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 8
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 9
  testRunner.And("the Response Resource should be a CapabilityStatement", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
  testRunner.And("the CapabilityStatement version should match the GP Connect specification release" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("CapabilityStatement profile indicates acceptance of xml and json format")]
        public virtual void CapabilityStatementProfileIndicatesAcceptanceOfXmlAndJsonFormat()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("CapabilityStatement profile indicates acceptance of xml and json format", null, ((string[])(null)));
#line 12
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 13
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 14
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 15
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 16
  testRunner.And("the CapabilityStatement Format should contain XML and JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("CapabilityStatement profile suppliers software versions present")]
        public virtual void CapabilityStatementProfileSuppliersSoftwareVersionsPresent()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("CapabilityStatement profile suppliers software versions present", null, ((string[])(null)));
#line 18
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 19
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 20
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 21
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 22
  testRunner.And("the CapabilityStatement Software should be valid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("CapabilityStatement profile supported fhir version")]
        public virtual void CapabilityStatementProfileSupportedFhirVersion()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("CapabilityStatement profile supported fhir version", null, ((string[])(null)));
#line 24
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 25
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 26
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 27
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 28
  testRunner.And("the CapabilityStatement FHIR Version should be \"3.0.1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("CapabilityStatement profile supports the RegisterPatient operation")]
        public virtual void CapabilityStatementProfileSupportsTheRegisterPatientOperation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("CapabilityStatement profile supports the RegisterPatient operation", null, ((string[])(null)));
#line 32
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 33
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 34
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 35
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 36
  testRunner.And("the CapabilityStatement REST Operations should contain \"gpc.registerpatient\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("CapabilityStatement profile supports the GetStructuredRecord operation")]
        public virtual void CapabilityStatementProfileSupportsTheGetStructuredRecordOperation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("CapabilityStatement profile supports the GetStructuredRecord operation", null, ((string[])(null)));
#line 40
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 41
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 42
 testRunner.When("I make the \"MetaDataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 43
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 44
 testRunner.And("the CapabilityStatement REST Operations should contain \"gpc.getstructuredrecord\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 45
 testRunner.And("the CapabilityStatement Operation \"gpc.getstructuredrecord\" has url \"https://fhir" +
                    ".nhs.uk/STU3/OperationDefinition/GPConnect-GetStructuredRecord-Operation-1/_hist" +
                    "ory/1.15\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Fhir content type test where Accept header is JSON and request payload is XML")]
        public virtual void FhirContentTypeTestWhereAcceptHeaderIsJSONAndRequestPayloadIsXML()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Fhir content type test where Accept header is JSON and request payload is XML", null, ((string[])(null)));
#line 47
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 48
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 49
  testRunner.And("I set the request content type to \"application/fhir+xml\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
  testRunner.And("I set the Accept header to \"application/fhir+json\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 51
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 52
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 53
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Fhir content type test where Accept header is XML and request payload is JSON")]
        public virtual void FhirContentTypeTestWhereAcceptHeaderIsXMLAndRequestPayloadIsJSON()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Fhir content type test where Accept header is XML and request payload is JSON", null, ((string[])(null)));
#line 55
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 56
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 57
  testRunner.And("I set the request content type to \"application/fhir+json\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
  testRunner.And("I set the Accept header to \"application/fhir+xml\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 60
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 61
  testRunner.And("the response body should be FHIR XML", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Fhir content type test where _format parameter is JSON and request payload is JSO" +
            "N")]
        public virtual void FhirContentTypeTestWhere_FormatParameterIsJSONAndRequestPayloadIsJSON()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Fhir content type test where _format parameter is JSON and request payload is JSO" +
                    "N", null, ((string[])(null)));
#line 63
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 64
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 65
  testRunner.And("I set the request content type to \"application/fhir+json\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 66
  testRunner.And("I do not send header \"Accept\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
  testRunner.And("I add a Format parameter with the Value \"application/fhir+json\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 68
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 69
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 70
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Fhir content type test where _format parameter is JSON and request payload is XML" +
            "")]
        public virtual void FhirContentTypeTestWhere_FormatParameterIsJSONAndRequestPayloadIsXML()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Fhir content type test where _format parameter is JSON and request payload is XML" +
                    "", null, ((string[])(null)));
#line 72
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 73
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 74
  testRunner.And("I set the request content type to \"application/fhir+xml\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 75
  testRunner.And("I do not send header \"Accept\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 76
  testRunner.And("I add a Format parameter with the Value \"application/fhir+json\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 78
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 79
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Fhir content type test where _format parameter is XML and request payload is XML")]
        public virtual void FhirContentTypeTestWhere_FormatParameterIsXMLAndRequestPayloadIsXML()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Fhir content type test where _format parameter is XML and request payload is XML", null, ((string[])(null)));
#line 81
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 82
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 83
  testRunner.And("I set the request content type to \"application/fhir+xml\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 84
  testRunner.And("I do not send header \"Accept\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 85
  testRunner.And("I add a Format parameter with the Value \"application/fhir+xml\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 86
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 87
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 88
  testRunner.And("the response body should be FHIR XML", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Fhir content type test where _format parameter is XML and request payload is JSON" +
            "")]
        public virtual void FhirContentTypeTestWhere_FormatParameterIsXMLAndRequestPayloadIsJSON()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Fhir content type test where _format parameter is XML and request payload is JSON" +
                    "", null, ((string[])(null)));
#line 90
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 91
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 92
  testRunner.And("I set the request content type to \"application/fhir+json\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 93
  testRunner.And("I do not send header \"Accept\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 94
  testRunner.And("I add a Format parameter with the Value \"application/fhir+xml\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 95
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 96
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 97
  testRunner.And("the response body should be FHIR XML", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Fhir content type test where Accept header is XML and _format parameter is XML")]
        public virtual void FhirContentTypeTestWhereAcceptHeaderIsXMLAnd_FormatParameterIsXML()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Fhir content type test where Accept header is XML and _format parameter is XML", null, ((string[])(null)));
#line 99
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 100
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 101
  testRunner.And("I set the request content type to \"application/fhir+xml\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 102
  testRunner.And("I set the Accept header to \"application/fhir+xml\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 103
  testRunner.And("I add a Format parameter with the Value \"application/fhir+xml\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 104
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 105
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 106
  testRunner.And("the response body should be FHIR XML", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Fhir content type test where Accept header is XML and _format parameter is JSON")]
        public virtual void FhirContentTypeTestWhereAcceptHeaderIsXMLAnd_FormatParameterIsJSON()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Fhir content type test where Accept header is XML and _format parameter is JSON", null, ((string[])(null)));
#line 108
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 109
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 110
  testRunner.And("I set the request content type to \"application/fhir+xml\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 111
  testRunner.And("I set the Accept header to \"application/fhir+xml\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 112
  testRunner.And("I add a Format parameter with the Value \"application/fhir+json\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 113
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 114
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 115
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Fhir content type test where Accept header is JSON and _format parameter is JSON")]
        public virtual void FhirContentTypeTestWhereAcceptHeaderIsJSONAnd_FormatParameterIsJSON()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Fhir content type test where Accept header is JSON and _format parameter is JSON", null, ((string[])(null)));
#line 117
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 118
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 119
  testRunner.And("I set the request content type to \"application/fhir+json\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 120
  testRunner.And("I set the Accept header to \"application/fhir+json\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 121
  testRunner.And("I add a Format parameter with the Value \"application/fhir+json\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 122
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 123
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 124
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Fhir content type test where Accept header is JSON and _format parameter is XML")]
        public virtual void FhirContentTypeTestWhereAcceptHeaderIsJSONAnd_FormatParameterIsXML()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Fhir content type test where Accept header is JSON and _format parameter is XML", null, ((string[])(null)));
#line 126
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 127
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 128
  testRunner.And("I set the request content type to \"application/fhir+json\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 129
  testRunner.And("I set the Accept header to \"application/fhir+json\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 130
  testRunner.And("I add a Format parameter with the Value \"application/fhir+xml\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 131
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 132
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 133
  testRunner.And("the response body should be FHIR XML", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("endpoint should support gzip compression for metadata endpoint and contain the co" +
            "rrect payload")]
        public virtual void EndpointShouldSupportGzipCompressionForMetadataEndpointAndContainTheCorrectPayload()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("endpoint should support gzip compression for metadata endpoint and contain the co" +
                    "rrect payload", null, ((string[])(null)));
#line 135
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 136
 testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 137
  testRunner.And("I set the Accept-Encoding header to gzip", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 138
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 139
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 140
  testRunner.And("the response should be gzip encoded", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 141
  testRunner.And("the response body should be FHIR JSON", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 142
  testRunner.And("the Response Resource should be a CapabilityStatement", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("CapabilityStatement GetMetaDataRead operation returns correct profile versions")]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Patient-1", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Organization-1", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Practitioner-1", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-PractitionerRole-1", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Location-1", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-OperationOutcome-1", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-Appointment-1", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-Schedule-1", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-Slot-1", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-AllergyIntolerance-1" +
            "", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Medication-1", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-MedicationStatement-" +
            "1", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-MedicationRequest-1", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-List-1", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-StructuredRecord-Bundle-1", null)]
        public virtual void CapabilityStatementGetMetaDataReadOperationReturnsCorrectProfileVersions(string urlToCheck, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("CapabilityStatement GetMetaDataRead operation returns correct profile versions", null, exampleTags);
#line 144
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 145
testRunner.Given("I configure the default \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 146
 testRunner.When("I make the \"MetadataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 147
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 148
 testRunner.And("the CapabilityStatement REST Operations should contain \"gpc.getstructuredrecord\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 149
    testRunner.And(string.Format("the CapabilityStatement Profile should contain the correct reference and version " +
                        "history \"{0}\"", urlToCheck), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("CapabilityStatement GetStructuredMetaDataRead operation returns correct profile v" +
            "ersions")]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Patient-1/_history/1" +
            ".8", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Organization-1/_hist" +
            "ory/1.4", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Practitioner-1/_hist" +
            "ory/1.2", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-PractitionerRole-1/_" +
            "history/1.2", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-AllergyIntolerance-1" +
            "/_history/1.6", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Medication-1/_histor" +
            "y/1.2", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-MedicationStatement-" +
            "1/_history/1.6", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-MedicationRequest-1/" +
            "_history/1.5", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-List-1/_history/1.1", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-StructuredRecord-Bundle-1/" +
            "_history/1.3", null)]
        [NUnit.Framework.TestCaseAttribute("https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-OperationOutcome-1/_histor" +
            "y/1.2", null)]
        public virtual void CapabilityStatementGetStructuredMetaDataReadOperationReturnsCorrectProfileVersions(string urlToCheck, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("CapabilityStatement GetStructuredMetaDataRead operation returns correct profile v" +
                    "ersions", null, exampleTags);
#line 168
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 169
testRunner.Given("I configure the default \"StructuredMetaDataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 170
 testRunner.When("I make the \"StructuredMetaDataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 171
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 172
 testRunner.And("the CapabilityStatement REST Operations should contain \"gpc.getstructuredrecord\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 173
    testRunner.And(string.Format("the CapabilityStatement Profile should contain the correct reference and version " +
                        "history \"{0}\"", urlToCheck), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("CapabilityStatement profile supports the GetStructuredMetaDataRead operation")]
        public virtual void CapabilityStatementProfileSupportsTheGetStructuredMetaDataReadOperation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("CapabilityStatement profile supports the GetStructuredMetaDataRead operation", null, ((string[])(null)));
#line 188
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 189
testRunner.Given("I configure the default \"StructuredMetaDataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 190
 testRunner.When("I make the \"StructuredMetaDataRead\" request", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 191
 testRunner.Then("the response status code should indicate success", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 192
 testRunner.And("the CapabilityStatement REST Operations should contain \"gpc.getstructuredrecord\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 193
 testRunner.And("the CapabilityStatement Operation \"gpc.getstructuredrecord\" has url \"https://fhir" +
                    ".nhs.uk/STU3/OperationDefinition/GPConnect-GetStructuredRecord-Operation-1/_hist" +
                    "ory/1.15\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
