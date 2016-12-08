﻿using System;
using GPConnect.Provider.AcceptanceTests.Logger;
using GPConnect.Provider.AcceptanceTests.Steps;
using TechTalk.SpecFlow;

namespace GPConnect.Provider.AcceptanceTests.Helpers
{
    public static class EndpointHelper
    {
        public static string GetProviderURL(ScenarioContext scenarioContext)
        {
            var httpProtocol = scenarioContext.Get<bool>(SecuritySteps.Context.UseTLS) ? "https://" : "http://";
            var spineProxyUrl = scenarioContext.Get<bool>(HttpSteps.Context.UseSpineProxy) ? httpProtocol + scenarioContext.Get<string>(HttpSteps.Context.SpineProxyUrl) + ":" + scenarioContext.Get<string>(HttpSteps.Context.SpineProxyPort) + "/" : "";
            var serverUrl = httpProtocol + scenarioContext.Get<string>(HttpSteps.Context.FhirServerUrl) + ":" + scenarioContext.Get<string>(HttpSteps.Context.FhirServerPort) + scenarioContext.Get<string>(HttpSteps.Context.FhirServerFhirBase);
            Log.WriteLine("SpineProxyURL = " + spineProxyUrl);
            Log.WriteLine("ServerURL = " + serverUrl);
            var providerURL = spineProxyUrl + serverUrl;
            Log.WriteLine("ProviderURL = " + providerURL);
            return providerURL;
        }

        public static string GetWebProxyURL(ScenarioContext scenarioContext)
        {
            var httpProtocol = scenarioContext.Get<bool>(SecuritySteps.Context.UseTLS) ? "https://" : "http://";
            var webProxyUrl = httpProtocol + scenarioContext.Get<string>(HttpSteps.Context.WebProxyUrl) + ":" + scenarioContext.Get<string>(HttpSteps.Context.WebProxyPort);
            Log.WriteLine("WebProxyURL = " + webProxyUrl);
            return webProxyUrl;
        }
    }
}