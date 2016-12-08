// ReSharper disable ClassNeverInstantiated.Global
namespace GPConnect.Provider.AcceptanceTests.Constants
{
    internal static class HttpConst
    {
        internal static class ContentTypes
        {
            public const string Json = "application/json";
            public const string Xml = "application/xml";
        }

        internal static class Headers
        {
            public const string Accept = "Accept";
            public const string Authorization = "Authorization";
            public const string SspFrom = "Ssp-From";
            public const string SspTo = "Ssp-To";
            public const string SspInteractionId = "Ssp-InteractionId";
            public const string SspTraceID = "Ssp-TraceID";
        }
    }
}
