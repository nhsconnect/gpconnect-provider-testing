using BoDi;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Logger;
using TechTalk.SpecFlow;

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
