using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace GPConnect.Provider.AcceptanceTests.Logger
{
    using LogBuffer = List<string>;

    [Binding]
    internal static class Log
    {
        public static void WriteLine(string format, params object[] args)
        {
            if (format == null) return;
            if (ScenarioContext.Current == null) return;
            var message = args != null ? string.Format(format, args) : format;

            if (!ScenarioContext.Current.ContainsKey(ScenarioLogKey))
                ScenarioContext.Current[ScenarioLogKey] = new LogBuffer();

            var log = (LogBuffer)ScenarioContext.Current[ScenarioLogKey];

            log.Add(message);
        }

        [AfterScenario]
        public static void HandleScenarioFailure()
        {
            if (ScenarioContext.Current.TestError != null)
                DumpLog();
        }

        private static void DumpLog()
        {
            if (!ScenarioContext.Current.ContainsKey(ScenarioLogKey)) return;
            var log = (LogBuffer)ScenarioContext.Current[ScenarioLogKey];
            foreach (var message in log)
                Console.WriteLine(message);
        }

        private const string ScenarioLogKey = "ScenarioLog";
    }
}
