using System;
using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;
using Shouldly;
using TechTalk.SpecFlow;
using System.Collections.Generic;
using GPConnect.Provider.AcceptanceTests.tools;

namespace GPConnect.Provider.AcceptanceTests.Steps
{

    [Binding]
    public class FhirSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private HeaderController headerController;
        private JwtHelper jwtHelper = new JwtHelper();

        public FhirSteps(ScenarioContext scenarioContext)
        {
            this._scenarioContext = scenarioContext;

            headerController = HeaderController.Instance;
        }

        [BeforeScenario]
        public void preScenarioSteps()
        {
            headerController.headerClearDown();
        }

        [Given(@"Test FhirStep")]
        public void GivenTestFhirStep()
        {
            Console.WriteLine("Test Fhir Step");
            Console.WriteLine(headerController.getRequestHeaders());
        }
    }
}
