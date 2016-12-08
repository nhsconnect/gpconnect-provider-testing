using System;
using GPConnect.Provider.AcceptanceTests.Helpers;
using TechTalk.SpecFlow;

namespace GPConnect.Provider.AcceptanceTests.Steps
{

    [Binding]
    public class GenericSteps : TechTalk.SpecFlow.Steps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly HttpHeaderHelper _httpHeaderHelper;

        public GenericSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _httpHeaderHelper = HttpHeaderHelper.Instance;
        }

        [BeforeScenario(Order=1)]
        public void OutputScenarioDetails()
        {
            Console.WriteLine("Feature: " + FeatureContext.Current.FeatureInfo.Title);
            Console.WriteLine(FeatureContext.Current.FeatureInfo.Description);
            Console.WriteLine();
            Console.WriteLine("Scenario: " + ScenarioContext.Current.ScenarioInfo.Title);
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
