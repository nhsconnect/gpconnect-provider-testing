using System.Collections.Generic;
using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Data;
using GPConnect.Provider.AcceptanceTests.Helpers;

namespace GPConnect.Provider.AcceptanceTests.Context
{
    public static class GlobalContext
    {
        private static readonly GlobalContextHelper GlobalContextHelper = new GlobalContextHelper();

        private static class Context
        {
            public const string kTraceDirectory = "traceDirectory";
            public const string kPDSData = "pdsData";
        }

        public static string TraceDirectory
        {
            get { return GlobalContextHelper.GetValue<string>(Context.kTraceDirectory); }
            set { GlobalContextHelper.SaveValue(Context.kTraceDirectory, value); }
        }

        public static List<PDS> PDSData
        {
            get { return GlobalContextHelper.GetValue<List<PDS>>(Context.kPDSData); }
            set { GlobalContextHelper.SaveValue(Context.kPDSData, value); }
        }
    }
}