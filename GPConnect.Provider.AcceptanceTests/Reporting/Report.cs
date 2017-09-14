namespace GPConnect.Provider.AcceptanceTests.Reporting
{
    using System;
    using TechTalk.SpecFlow;
    using Context;
    using Http;
   
    internal class Report
    {
        private readonly HttpContext _httpContext;
        private readonly ScenarioContext _scenarioContext;

        public Report(HttpContext httpContext, ScenarioContext scenarioContext)
        {
            _httpContext = httpContext;
            _scenarioContext = scenarioContext;
        }

        public Guid TestRunId => GlobalContext.TestRunId;
        public string ScenarioName => _scenarioContext.ScenarioInfo.Title;
        public string ScenarioOutcome => string.IsNullOrEmpty(ErrorMessage) ? "Pass" : "Fail";
        public string ErrorMessage => _scenarioContext.TestError?.Message;
        public HttpRequestConfiguration HttpRequest => _httpContext.HttpRequestConfiguration;
        public HttpResponse HttpResponse => _httpContext.HttpResponse;
    }
}
