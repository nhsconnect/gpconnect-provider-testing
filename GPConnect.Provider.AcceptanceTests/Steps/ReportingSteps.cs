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

        [AfterScenario]
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
        }

        private HttpRequestMessage GetHttpRequestMessage()
        {
            var report = new Report(HttpContext, ScenarioContext).ToJson();
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
