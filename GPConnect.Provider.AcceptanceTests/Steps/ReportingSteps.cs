namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using TechTalk.SpecFlow;
    using Context;
    using Extensions;
    using Reporting;
    using static Logger.Log;
    using System.Collections.Generic;

    [Binding]
    internal class ReportingSteps : Steps
    {
        private static HttpContext HttpContext;
        private static string Url => ReportingConfiguration.Url;
        private static string Message => $"Report send failure: {Url}";

        internal ReportingSteps(HttpContext httpContext)
        {
            HttpContext = httpContext;
        }

        [BeforeScenario]
        internal void SetOutlineIndex()
        {
            if (GlobalContext.PreviousScenarioTitle == ScenarioContext.Current.ScenarioInfo.Title)
                GlobalContext.ScenarioIndex++;
            else
                GlobalContext.ScenarioIndex = 1;
        }

        [AfterScenario(Order = 1)]
        internal void SendReport()
        {
            if (ReportingConfiguration.Enabled)
            {
                var httpRequestMessage = GetHttpRequestMessage();

                var httpClient = GetHttpClient();

                try
                {
                    var result = httpClient.SendAsync(httpRequestMessage).Result;

                    if (result.StatusCode >= HttpStatusCode.BadRequest)
                    {
                        WriteLine($"{Message} - {(int)result.StatusCode} : {result.StatusCode.ToString()}");
                    }
                }
                catch (Exception exception)
                {
                    WriteLine($"{Message} - {exception.InnerException?.InnerException?.Message}");
                }
            }

            //Add Test Details to Report List
            if (ReportingConfiguration.FileReportingEnabled)
            {
                var traceDirectory = GlobalContext.TraceDirectory;

                string ScenarioName = ScenarioContext.Current.ScenarioInfo.Title +  GlobalContext.ScenarioIndex.ToString();  
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
                    TestResult = ScenarioOutcome
                };

                GlobalContext.FileBasedReportList.Add(FileLogEntry);

            }
        }

        [AfterScenario(Order = 2)]
        internal void SetPreviousTitle()
        {
            GlobalContext.PreviousScenarioTitle = ScenarioContext.Current.ScenarioInfo.Title;
        }

        private static HttpRequestMessage GetHttpRequestMessage()
        {
            var report = new Report(HttpContext).ToJson();
            var content = new StringContent(report, Encoding.UTF8, Constants.HttpConst.ContentTypes.kJson);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Url)
            {
                Content = content
            };
            
            return requestMessage;
        }
        
        private static HttpClient GetHttpClient()
        {
            var handler = new HttpClientHandler();
            
            return new HttpClient(handler)
            {
                BaseAddress = new Uri(Url)
            };
        }
    }
}
