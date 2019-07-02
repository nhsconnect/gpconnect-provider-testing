using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BoDi;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Importers;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Specification.Source;
using NUnit.Framework;
using TechTalk.SpecFlow;
// ReSharper disable UnusedMember.Global

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public class GenericSteps : TechTalk.SpecFlow.Steps
    {
        private readonly IObjectContainer _objectContainer;

        public GenericSteps(IObjectContainer objectContainer)
        {
            Log.WriteLine("GenericSteps() Constructor");
            _objectContainer = objectContainer;
        }

        [BeforeTestRun(Order = 1)]
        public static void CreateTraceFolder()
        {
            if (!Directory.Exists(AppSettingsHelper.TraceBaseDirectory)) return;
            var folderName = DateTime.Now.ToString("s").Replace(":", string.Empty);
            var traceDirectory = Path.Combine(AppSettingsHelper.TraceBaseDirectory, folderName);
            Log.WriteLine("Create Trace Directory = '{0}'", traceDirectory);
            Directory.CreateDirectory(traceDirectory);
            // Save The Newly Created Trace Directory To The Global Context
            GlobalContext.TraceDirectory = traceDirectory;
        }

        [BeforeTestRun(Order = 1)]
        public static void LoadPDSData()
        {
            if (Directory.Exists(AppSettingsHelper.DataDirectory) == false)
                Assert.Fail("Data Directory Not Found.");
            var pdsCSV = Path.Combine(AppSettingsHelper.DataDirectory, @"PDS.csv");
            Log.WriteLine("PDS CSV = '{0}'", pdsCSV);
            GlobalContext.PDSData = PDSImporter.LoadCsv(pdsCSV);
        }

        [BeforeTestRun(Order = 1)]
        public static void LoadODSData()
        {
            if (Directory.Exists(AppSettingsHelper.DataDirectory) == false)
                Assert.Fail("Data Directory Not Found.");
            var odsCSV = Path.Combine(AppSettingsHelper.DataDirectory, @"ODS.csv");
            Log.WriteLine("ODS CSV = '{0}'", odsCSV);
            GlobalContext.ODSData = ODSImporter.LoadCsv(odsCSV);
        }

        [BeforeTestRun(Order = 1)]
        public static void LoadNHSNoMapData()
        {
            if (Directory.Exists(AppSettingsHelper.DataDirectory) == false)
                Assert.Fail("Data Directory Not Found.");
            var nhsNoMapCSV = Path.Combine(AppSettingsHelper.DataDirectory, @"NHSNoMap.csv");
            Log.WriteLine("NHSNoMap CSV = '{0}'", nhsNoMapCSV);
            GlobalContext.NHSNoMapData = NHSNoMapImporter.LoadCsv(nhsNoMapCSV);
        }

        [BeforeTestRun(Order = 2)]
        public static void LoadFhirDefinitions()
        {
            if (Directory.Exists(AppSettingsHelper.FhirDirectory) == false)
                Assert.Fail("FHIR Directory Not Found.");
            var resolver = new ArtifactResolver(new FileDirectoryArtifactSource(AppSettingsHelper.FhirDirectory, true));
            var gender = resolver.GetValueSet("http://fhir.nhs.net/ValueSet/administrative-gender-1");
            if (gender == null)
                Assert.Fail("Gender ValueSet Not Found.");
            Log.WriteLine("{0} Genders Loaded.", gender.CodeSystem.Concept.Count);
            GlobalContext.FhirGenderValueSet = gender;

            var maritalStatus = resolver.GetValueSet("http://fhir.nhs.net/ValueSet/marital-status-1");
            if (maritalStatus == null)
                Assert.Fail("MaritalStatus ValueSet Not Found.");
            Log.WriteLine("{0} MaritalStatus Loaded.", maritalStatus.CodeSystem.Concept.Count);
            GlobalContext.FhirMaritalStatusValueSet = maritalStatus;

            var relationship = resolver.GetValueSet("http://fhir.nhs.net/ValueSet/cda-person-relationship-type-1");
            if (relationship == null)
                Assert.Fail("Relationship ValueSet Not Found.");
            Log.WriteLine("{0} Relationship Loaded.", relationship.CodeSystem.Concept.Count);
            GlobalContext.FhirRelationshipValueSet = relationship;

            var humanLanguage = resolver.GetValueSet("http://fhir.nhs.net/ValueSet/human-language-1");
            if (humanLanguage == null)
                Assert.Fail("HumanLanguage ValueSet Not Found.");
            Log.WriteLine("{0} HumanLanguage Loaded.", humanLanguage.CodeSystem.Concept.Count);
            GlobalContext.FhirHumanLanguageValueSet = humanLanguage;
        }

        [BeforeScenario(Order = 0)]
        public void InitializeContainer()
        {
            Log.WriteLine("InitializeContainer For Dependency Injection");
            _objectContainer.RegisterTypeAs<SecurityContext, ISecurityContext>();
            _objectContainer.RegisterTypeAs<HttpContext, IHttpContext>();
            // HACK To Be Able To See What We've Loaded In The BeforeTestRun Phase
            Log.WriteLine("{0} Patients Loaded From PDS CSV File.", GlobalContext.PDSData.ToList().Count);
            Log.WriteLine("{0} Organisations Loaded From ODS CSV File.", GlobalContext.ODSData.ToList().Count);
            Log.WriteLine("{0} Genders Loaded From FHIR ValueSet File.", GlobalContext.FhirGenderValueSet.CodeSystem.Concept.Count);
            Log.WriteLine("{0} MaritalStatus Loaded From FHIR ValueSet File.", GlobalContext.FhirMaritalStatusValueSet.CodeSystem.Concept.Count);
            Log.WriteLine("{0} Relationship Loaded From FHIR ValueSet File.", GlobalContext.FhirRelationshipValueSet.CodeSystem.Concept.Count);
            Log.WriteLine("{0} HumanLanguage Loaded From FHIR ValueSet File.", GlobalContext.FhirHumanLanguageValueSet.CodeSystem.Concept.Count);
        }

        [BeforeScenario(Order = 1)]
        public void OutputScenarioDetails()
        {
            Log.WriteLine("Feature: " + FeatureContext.Current.FeatureInfo.Title);
            Log.WriteLine(FeatureContext.Current.FeatureInfo.Description);
            Log.WriteLine("");
            Log.WriteLine("Scenario: " + ScenarioContext.Current.ScenarioInfo.Title);
        }

        [BeforeScenario]
        internal void SetOutlineIndex()
        {
            if (GlobalContext.PreviousScenarioTitle == ScenarioContext.Current.ScenarioInfo.Title)
                GlobalContext.ScenarioIndex++;
            else
                GlobalContext.ScenarioIndex = 1;
        }

        [AfterScenario(Order = 2)]
        internal void SetPreviousTitle()
        {
            GlobalContext.PreviousScenarioTitle = ScenarioContext.Current.ScenarioInfo.Title;
        }


        [AfterScenario(Order = 1)]
        internal void SendReport()
        {


            //Add Test Details to Report List
            if (AppSettingsHelper.FileReportingEnabled)
            {
                var traceDirectory = GlobalContext.TraceDirectory;

                string ScenarioName = ScenarioContext.Current.ScenarioInfo.Title + GlobalContext.ScenarioIndex.ToString();
                string ErrorMessage = ScenarioContext.Current.TestError?.Message;
                string ScenarioOutcome = string.IsNullOrEmpty(ErrorMessage) ? "Pass" : "Fail";

                //init vars if needed
                if (GlobalContext.FileBasedReportList == null)
                {

                    GlobalContext.FileBasedReportList = new List<GlobalContext.FileBasedReportEntry>();
                    GlobalContext.CountTestRunPassed = 0;
                    GlobalContext.CountTestRunPassed = 0;
                }

                //Keep count of Pass and Fails
                if (ScenarioOutcome == "Pass")
                    GlobalContext.CountTestRunPassed++;
                else
                    GlobalContext.CountTestRunFailed++;

                var FileLogEntry = new GlobalContext.FileBasedReportEntry()
                {
                    TestRunDateTime = DateTime.UtcNow.ToLocalTime(),
                    Testname = ScenarioName,
                    TestResult = ScenarioOutcome,
                    FailureMessage = ErrorMessage
                };

                GlobalContext.FileBasedReportList.Add(FileLogEntry);

            }
        }

        [AfterTestRun]
        public static void outputFileBasedTestReport()
        {
            //output test run sumamry file
            if (AppSettingsHelper.FileReportingEnabled)
            {

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(GlobalContext.TraceDirectory + @"\TestRunLog.txt"))
                {
                    //writes overall stats first
                    file.WriteLine("****************************************************************");
                    file.WriteLine("Overall Stats from test Run: Total=" + (GlobalContext.CountTestRunPassed + GlobalContext.CountTestRunFailed).ToString() + "  Passed=" + GlobalContext.CountTestRunPassed.ToString() + "  Failed: " + GlobalContext.CountTestRunFailed.ToString());
                    file.WriteLine("****************************************************************");

                    //Add inormation about the test run
                    file.WriteLine("TestRunDateTime : " + DateTime.UtcNow.ToLocalTime().ToString());
                    file.WriteLine("consumerASID : " + AppSettingsHelper.ConsumerASID);
                    file.WriteLine("providerASID : " + AppSettingsHelper.ProviderASID);
                    file.WriteLine("useTLS Flag : " + AppSettingsHelper.UseTLS.ToString());
                    file.WriteLine("serverURL: " + AppSettingsHelper.ServerUrl);
                    file.WriteLine("serverPort : " + AppSettingsHelper.ServerPort);
                    file.WriteLine("serverBase : " + AppSettingsHelper.ServerBase);
                    file.WriteLine("useSpineProxy Flag : " + AppSettingsHelper.UseSpineProxy.ToString());
                    file.WriteLine("spineProxyUrl : " + AppSettingsHelper.SpineProxyUrl.ToString());
                    file.WriteLine("spineProxyPort : " + AppSettingsHelper.SpineProxyPort.ToString());

                    file.WriteLine("****************************************************************");
                    file.WriteLine("Individual Test Results Below");
                    file.WriteLine("****************************************************************");

                    if (AppSettingsHelper.FileReportingSortFailFirst)
                        GlobalContext.FileBasedReportList = GlobalContext.FileBasedReportList.OrderBy(i => i.TestResult).ToList();
                    else
                        GlobalContext.FileBasedReportList = GlobalContext.FileBasedReportList.OrderBy(i => i.TestRunDateTime).ToList();

                    bool lastEntryFailed = false;

                    foreach (GlobalContext.FileBasedReportEntry entry in GlobalContext.FileBasedReportList)
                    {

                        // Output test results with or without error message
                        if (AppSettingsHelper.FileReportingOutputFailureMessage)
                        {

                            //If have error message for test then output 
                            if (!string.IsNullOrEmpty(entry.FailureMessage))
                            {

                                //Write Test Result
                                if (!lastEntryFailed)
                                    file.Write("----------------------------------------------------------------\n");

                                file.Write(entry.TestRunDateTime.ToLocalTime() + "," + entry.Testname + "," + entry.TestResult + "\n");

                                file.Write(entry.FailureMessage + "\n");
                                file.Write("----------------------------------------------------------------\n");

                                lastEntryFailed = true;
                            }
                            //No failure so dont output a lines around pass entry
                            else
                            {
                                file.Write(entry.TestRunDateTime.ToLocalTime() + "," + entry.Testname + "," + entry.TestResult + "\n");
                                lastEntryFailed = false;

                            }
                        }
                        //output without error message
                        else
                        {
                            file.Write(entry.TestRunDateTime.ToLocalTime() + "," + entry.Testname + "," + entry.TestResult + "\n");
                        }
                    }


                }

            }
        }


    }
}