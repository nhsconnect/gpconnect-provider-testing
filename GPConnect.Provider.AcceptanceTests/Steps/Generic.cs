using System;
using TechTalk.SpecFlow;
using GPConnect.Provider.AcceptanceTests.tools;

namespace GPConnect.Provider.AcceptanceTests.Steps
{

    [Binding]
    public class Generic : TechTalk.SpecFlow.Steps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly HeaderController _headerController;
        private readonly JwtHelper _jwtHelper;

        public Generic(ScenarioContext scenarioContext, JwtHelper jwtHelper)
        {
            _scenarioContext = scenarioContext;
            _jwtHelper = jwtHelper;
            _headerController = HeaderController.Instance;
        }

        [BeforeScenario]
        public void preScenarioSteps()
        {
            Console.WriteLine("Header Clear Down");
            _headerController.headerClearDown();
        }
        
    }
}
