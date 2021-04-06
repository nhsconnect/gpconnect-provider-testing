namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System;
    using System.IO;
    using BoDi;
    using Cache;
    using Cache.ValueSet;
    using Constants;
    using Context;
    using Reporting;
    using Helpers;
    using Importers;
    using Logger;
    using NUnit.Framework;
    using Repository;
    using TechTalk.SpecFlow;
    using Steps = TechTalk.SpecFlow.Steps;
    using System.Linq;

    [Binding]
    public class GenericSteps : Steps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly HttpContext _httpContext;

        public GenericSteps(IObjectContainer objectContainer, HttpContext httpContext)
        {
            Log.WriteLine("GenericSteps() Constructor");
            _objectContainer = objectContainer;
            _httpContext = httpContext;
        }

        [BeforeTestRun(Order = 1)]
        public static void CreateTraceFolder()
        {
            if (!Directory.Exists(AppSettingsHelper.TraceBaseDirectory)) return;
            var folderName = DateTime.Now.ToString("s").Replace(":", String.Empty);
            var traceDirectory = Path.Combine(AppSettingsHelper.TraceBaseDirectory, folderName);
            Log.WriteLine("Create Trace Directory = '{0}'", traceDirectory);
            Directory.CreateDirectory(traceDirectory);
            // Save The Newly Created Trace Directory To The Global Context
            GlobalContext.TraceDirectory = traceDirectory;
        }

        [BeforeTestRun(Order = 1)]
        public static void LoadODSData()
        {
            if (!Directory.Exists(AppSettingsHelper.DataDirectory))
            {
                Assert.Fail("Data Directory Not Found.");
            }

            var odsCSV = Path.Combine(AppSettingsHelper.DataDirectory, @"ODSCodeMap.csv");
            Log.WriteLine("ODS CSV = '{0}'", odsCSV);
            GlobalContext.OdsCodeMap = ODSCodeMapImporter.LoadCsv(odsCSV);
        }

        [BeforeTestRun(Order = 1)]
        public static void LoadLogicalLocationIdentifierMap()
        {
            if (!Directory.Exists(AppSettingsHelper.DataDirectory))
            {
                Assert.Fail("Data Directory Not Found.");
            }

            var csv = Path.Combine(AppSettingsHelper.DataDirectory, @"LocationLogicalIdentifierMap.csv");

            Log.WriteLine("LocationLogicalIdentifierMap= '{0}'", csv);

            GlobalContext.LocationLogicalIdentifierMap = LocationLogicalIdentifierImporter.LoadCsv(csv);
        }

        [BeforeTestRun(Order = 1)]
        public static void LoadNHSNoMapData()
        {
            if (!Directory.Exists(AppSettingsHelper.DataDirectory))
            {
                Assert.Fail("Data Directory Not Found.");
            }

            var nhsNoMapCSV = Path.Combine(AppSettingsHelper.DataDirectory, @"NHSNoMap.csv");
            Log.WriteLine("NHSNoMap CSV = '{0}'", nhsNoMapCSV);
            GlobalContext.PatientNhsNumberMap = NHSNoMapImporter.LoadCsv(nhsNoMapCSV);
        }

        [BeforeTestRun(Order = 1)]
        public static void LoadRegisterPatientData()
        {
            if (!Directory.Exists(AppSettingsHelper.DataDirectory))
            {
                Assert.Fail("Data Directory Not Found.");
            }

            var registerPatientsCSV = Path.Combine(AppSettingsHelper.DataDirectory, @"RegisterPatients.csv");
            Log.WriteLine("RegisterPatients CSV = '{0}'", registerPatientsCSV);
            GlobalContext.RegisterPatients = RegisterPatientsImporter.LoadCsv(registerPatientsCSV);
        }

        [BeforeTestRun(Order = 1)]
        public static void LoadPractitionerCodeMapData()
        {
            if (!Directory.Exists(AppSettingsHelper.DataDirectory))
            {
                Assert.Fail("Data Directory Not Found.");
            }

            var practitionerCodeMapCSV = Path.Combine(AppSettingsHelper.DataDirectory, @"PractitionerCodeMap.csv");
            Log.WriteLine("practitionerCodeMap CSV = '{0}'", practitionerCodeMapCSV);
            GlobalContext.PractionerCodeMap = PractitionerCodeMapImporter.LoadCsv(practitionerCodeMapCSV);
        }




        [BeforeScenario(Order = 0)]
        public void InitializeContainer()
        {
            Log.WriteLine("InitializeContainer For Dependency Injection");
            _objectContainer.RegisterTypeAs<SecurityContext, ISecurityContext>();
            _objectContainer.RegisterTypeAs<HttpContext, IHttpContext>();
            _objectContainer.RegisterTypeAs<FhirResourceRepository, IFhirResourceRepository>();
            //_objectContainer.Resolve<HttpHeaderHelper>();
            // HACK To Be Able To See What We've Loaded In The BeforeTestRun Phase
            Log.WriteLine("{0} Organisations Loaded From ODS CSV File.", GlobalContext.OdsCodeMap.Count);
        }

        [BeforeScenario(Order = 1)]
        public void OutputScenarioDetails()
        {
            Log.WriteLine("Feature: " + FeatureContext.Current.FeatureInfo.Title);
            Log.WriteLine(FeatureContext.Current.FeatureInfo.Description);
            Log.WriteLine("");
            Log.WriteLine("Scenario: " + ScenarioContext.Current.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void SaveLogOfOutput()
        {
            var traceDirectory = GlobalContext.TraceDirectory;

            if (Directory.Exists(traceDirectory))
            {
                var scenarioDirectory = Path.Combine(traceDirectory, ScenarioContext.Current.ScenarioInfo.Title);
                var fileIndex = 1;

                while (Directory.Exists($"{scenarioDirectory}-{fileIndex}"))
                {
                    fileIndex++;
                }

                scenarioDirectory = $"{scenarioDirectory}-{fileIndex}";

                Directory.CreateDirectory(scenarioDirectory);

                Log.WriteLine(scenarioDirectory);

                try
                {
                    _httpContext.SaveToDisk(Path.Combine(scenarioDirectory, "HttpContext.xml"));
                }
                catch
                {
                    Log.WriteLine("Exception writing HttpContext to Output File");
                }
                try
                {
                    _httpContext.SaveToFhirContextToDisk(Path.Combine(scenarioDirectory, "FhirContext.xml"));
                }
                catch
                {
                    Log.WriteLine("Exception writing FhirContext to Output File");
                }


                //Output JSON trace
                if (AppSettingsHelper.TraceOutputJSONResponse)
                {
                    //OutputJson Response as Pretty Printed Separate File
                    try
                    {
                        _httpContext.SaveJSONResponseToDisk(Path.Combine(scenarioDirectory, "JSONResponse.txt"));
                    }
                    catch
                    {
                        Log.WriteLine("Exception writing JSONResponse to Output File");
                    }

                }

                //Output JWT trace
                if (AppSettingsHelper.TraceOutputJWT)
                {
                    //Dump JWT Token to output file
                    try
                    {
                        _httpContext.SaveJWTToDisk(Path.Combine(scenarioDirectory, "JWTToken.txt"));
                    }
                    catch (Exception Ex)
                    {
                        Log.WriteLine("Exception writing JWT Header to Output File");
                    }
                }


                //output Pretty Printed Request body
                if (AppSettingsHelper.TraceOutputJSONRequestBody)
                {
                    //Output Json Request Body to a Pretty Printed Separate File
                    try
                    {
                        _httpContext.SaveJSONRequestBodyToDisk(Path.Combine(scenarioDirectory, "JSONRequestBody.txt"));
                    }
                    catch
                    {
                        Log.WriteLine("Exception writing JSONRequestBody to Output File");
                    }

                }


                }
        }

        [BeforeTestRun]
        public static void SetTestRunId()
        {
            GlobalContext.TestRunId = Guid.NewGuid();
        }

        [AfterTestRun]
        public static void outputFileBasedTestReport()
        {
            //output test run sumamry file
            if (ReportingConfiguration.FileReportingEnabled)
            {

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(GlobalContext.TraceDirectory + @"\TestRunLog.txt"))
                {
                    //writes overall stats first
                    file.WriteLine("****************************************************************");
                    file.WriteLine("Overall Stats from test Run: Total=" + (GlobalContext.CountTestRunPassed + GlobalContext.CountTestRunFailed).ToString() + "  Passed=" + GlobalContext.CountTestRunPassed.ToString() + "  Failed: " + GlobalContext.CountTestRunFailed.ToString());
                    file.WriteLine("****************************************************************");

                    file.WriteLine("gpConnectSpecVersion : " + AppSettingsHelper.GPConnectSpecVersion);

                    //Add inormation about the test run
                    file.WriteLine("TestRunDateTime : " + DateTime.UtcNow.ToLocalTime().ToString());
                    file.WriteLine("consumerASID : " + AppSettingsHelper.ConsumerASID);
                    file.WriteLine("providerASID : " + AppSettingsHelper.ProviderASID);
                    file.WriteLine("useTLS Flag : " + AppSettingsHelper.UseTLS.ToString());
                    file.WriteLine("serverURL: " + AppSettingsHelper.ServerUrl);
                    file.WriteLine("serverPort HTTP: " + AppSettingsHelper.ServerHttpPort);
                    file.WriteLine("serverPort HTTPS: " + AppSettingsHelper.ServerHttpsPort);
                    file.WriteLine("serverBase : " + AppSettingsHelper.ServerBase);
                    file.WriteLine("useSpineProxy Flag : " + AppSettingsHelper.UseSpineProxy.ToString());
                    file.WriteLine("spineProxyUrl : " + AppSettingsHelper.SpineProxyUrl.ToString());
                    file.WriteLine("spineProxyPort : " + AppSettingsHelper.SpineProxyPort.ToString());
                    file.WriteLine("****************************************************************");
                    file.WriteLine("Individual Test Results Below");
                    file.WriteLine("****************************************************************");

                    if (ReportingConfiguration.FileReportingSortFailFirst)
                        GlobalContext.FileBasedReportList = GlobalContext.FileBasedReportList.OrderBy(i => i.TestResult).ToList();
                    else
                        GlobalContext.FileBasedReportList = GlobalContext.FileBasedReportList.OrderBy(i => i.TestRunDateTime).ToList();

                    bool lastEntryFailed = false;

                    foreach (GlobalContext.FileBasedReportEntry entry in GlobalContext.FileBasedReportList)
                    {
                        //Output test results with or without error message
                        if (ReportingConfiguration.FileReportingOutputFailureMessage)
                        {

                                //If have error message for test then output 
                                if (!string.IsNullOrEmpty(entry.FailureMessage))
                                {

                                     //Write Test Result
                                    if(!lastEntryFailed)
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