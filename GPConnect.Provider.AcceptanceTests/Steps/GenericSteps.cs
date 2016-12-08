using System;
using System.IO;
using BoDi;
using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Logger;
using TechTalk.SpecFlow;
// ReSharper disable UnusedMember.Global

namespace GPConnect.Provider.AcceptanceTests.Steps
{

    [Binding]
    public class GenericSteps : TechTalk.SpecFlow.Steps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private readonly HttpHeaderHelper _httpHeaderHelper;

        public GenericSteps(IObjectContainer objectContainer, ScenarioContext scenarioContext, HttpHeaderHelper headerHelper)
        {
            Log.WriteLine("GenericSteps() Constructor");
            _objectContainer = objectContainer;
            _scenarioContext = scenarioContext;
            _httpHeaderHelper = headerHelper;
        }

        [BeforeTestRun]
        public static void CreateTraceFolder()
        {
            if (!Directory.Exists(AppSettingsHelper.TraceBaseDirectory)) return;
            var folderName = DateTime.Now.ToString("s").Replace(":", string.Empty);
            var traceDirectory = Path.Combine(AppSettingsHelper.TraceBaseDirectory, folderName);
            Log.WriteLine("Create Trace Directory = '{0}'", traceDirectory);
            Directory.CreateDirectory(traceDirectory);
            // Save The Newly Created Trace Directory To The Global Context
            GlobalContext.SaveValue(GlobalConst.Trace.TraceDirectory, traceDirectory);
        }

        [BeforeScenario(Order = 0)]
        public void InitializeContainer()
        {
            Log.WriteLine("InitializeContainer For Dependency Injection");
            _objectContainer.RegisterTypeAs<SecuritySteps, ISecuritySteps>();
            _objectContainer.RegisterTypeAs<HttpSteps, IHttpSteps>();
        }

        [BeforeScenario(Order=1)]
        public void OutputScenarioDetails()
        {
            Log.WriteLine("Feature: " + FeatureContext.Current.FeatureInfo.Title);
            Log.WriteLine(FeatureContext.Current.FeatureInfo.Description);
            Log.WriteLine("");
            Log.WriteLine("Scenario: " + ScenarioContext.Current.ScenarioInfo.Title);
        }

        [BeforeScenario(Order = 2)]
        public void LoadAppConfig()
        {
            AppSettingsHelper.LoadAppSettings(_scenarioContext);
        }

        [BeforeScenario(Order = 3)]
        public void ResetHttpHeaders()
        {
            _httpHeaderHelper.Clear();
        }

        [BeforeScenario(Order = 4)]
        public void DoNotValidateServerCertificate()
        {
            SecurityHelper.DoNotValidateServerCertificate();
        }        

    }
}
