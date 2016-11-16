using System;
using TechTalk.SpecFlow;
using GPConnect.Provider.AcceptanceTests.tools;

namespace GPConnect.Provider.AcceptanceTests.Steps
{

    [Binding]
    public class GenericSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private HeaderController headerController;
        private JwtHelper jwtHelper;

        public GenericSteps(ScenarioContext scenarioContext)
        {
            this._scenarioContext = scenarioContext;
            headerController = HeaderController.Instance;
        }

        [BeforeScenario]
        public void preScenarioSteps()
        {
            Console.WriteLine("Header Clear Down");
            headerController.headerClearDown();
        }
        
    }
}
