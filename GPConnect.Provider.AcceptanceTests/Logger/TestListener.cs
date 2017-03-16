using System;
using TechTalk.SpecFlow.Tracing;

// https://gist.github.com/chrisoldwood/fce752bab1f7060dc7b2

// <specFlow>
//     <trace traceSuccessfulSteps="false" listener="GPConnect.Provider.AcceptanceTests.Logger, GPConnect.Provider.AcceptanceTests" />
// </specFlow>

namespace GPConnect.Provider.AcceptanceTests.Logger
{
    public class TestListener : ITraceListener
    {
        public TestListener()
        {
            var disableTrace = Environment.GetEnvironmentVariable(DisableTraceVariable);

            if (string.IsNullOrWhiteSpace(disableTrace))
                _listener = new DefaultListener();
        }

        public void WriteTestOutput(string message)
        {
            _listener?.WriteTestOutput(message);
        }

        public void WriteToolOutput(string message)
        {
            _listener?.WriteToolOutput(message);
        }

        private readonly ITraceListener _listener;

        private const string DisableTraceVariable = "DISABLE_SPECFLOW_TRACE_OUTPUT";
    }
}