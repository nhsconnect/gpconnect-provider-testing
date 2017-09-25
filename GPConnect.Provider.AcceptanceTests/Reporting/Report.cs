namespace GPConnect.Provider.AcceptanceTests.Reporting
{
    using System;
    using TechTalk.SpecFlow;
    using Context;
    using Http;
   
    internal class Report
    {
        private readonly HttpContext _httpContext;

        public Report(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public Guid TestRunId => GlobalContext.TestRunId;
        public string ScenarioName => $"{ScenarioContext.Current.ScenarioInfo.Title} ({GlobalContext.ScenarioIndex})";
        public string ScenarioOutcome => string.IsNullOrEmpty(ErrorMessage) ? "Pass" : "Fail";
        public string ErrorMessage => ScenarioContext.Current.TestError?.Message;
        public HttpRequestConfiguration HttpRequest => _httpContext.HttpRequestConfiguration;
        public HttpResponse HttpResponse => _httpContext.HttpResponse;
    }
}
