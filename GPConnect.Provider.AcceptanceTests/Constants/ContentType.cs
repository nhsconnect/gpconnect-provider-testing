namespace GPConnect.Provider.AcceptanceTests.Constants
{
    internal static class ContentType
    {
        internal static class Application
        {
            private const string _application = "application/";

            //STU3
            internal const string FhirJson = _application + "fhir+json";
            internal const string FhirXml = _application + "fhir+xml";

            //Generic
            internal const string Json = _application + "json";
            internal const string Xml = _application + "xml";

            //DSTU2 (Backwards Compatibility)
            internal const string JsonFhirDSTU2 = _application + "fhir+json";
            internal const string XmlFhirDSTU2 = _application + "fhir+xml";
        }

        internal static class Text
        {
            private const string _text = "text/";

            //Generic
            internal const string Json = _text + "json";
        }
    }
}
