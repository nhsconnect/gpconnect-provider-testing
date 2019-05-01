namespace GPConnect.Provider.AcceptanceTests.Reporting
{
    using Helpers;

    internal static class ReportingConfiguration
    {
        internal static string Url => $"{Protocol}{BaseUrl}:{Port}{Endpoint}";
        internal static bool Enabled => AppSettingsHelper.Get<bool>("Reporting:Enabled");
        private static string BaseUrl => AppSettingsHelper.Get<string>("Reporting:BaseUrl");
        private static string Endpoint => AppSettingsHelper.Get<string>("Reporting:Endpoint");
        private static int Port => AppSettingsHelper.Get<int>("Reporting:Port");
        private static bool Tls => AppSettingsHelper.Get<bool>("Reporting:Tls");
        private static string Protocol => Tls ? "https://" : "http://";
        internal static bool FileReportingEnabled => AppSettingsHelper.Get<bool>("ReportingToFile:Enabled");
        internal static bool FileReportingSortFailFirst => AppSettingsHelper.Get<bool>("ReportingToFile:SortFailFirst");

    }
}
