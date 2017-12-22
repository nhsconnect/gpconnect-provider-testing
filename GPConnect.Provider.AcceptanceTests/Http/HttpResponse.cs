namespace GPConnect.Provider.AcceptanceTests.Http
{
    using System.Collections.Generic;
    using System.Net;
    using System.Xml.Linq;
    using Constants;
    using CurlSharp;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Serialization;
    using Newtonsoft.Json.Linq;

    public class HttpResponse
    {
        public HttpResponse()
        {
            StatusCode = default(HttpStatusCode);
            ResponseTimeInMilliseconds = -1;
            ContentType = null;
            Body = null;
            Headers = new Dictionary<string, string>();
        }

        private long _responseTimeInMilliseconds;
        public HttpStatusCode StatusCode { get; set; }
        public string ContentType { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }
        public JObject ResponseJSON { get; set; }
        public XDocument ResponseXML { get; set; }

        public long ResponseTimeInMilliseconds
        {
            get { return _responseTimeInMilliseconds; }
            set
            {
                _responseTimeInMilliseconds = value;
                ResponseTimeAcceptable = value <= 1000;
            }
        }

        public bool ResponseTimeAcceptable { get; set; }
        public CurlCode CurlCode { get; set; }
        public bool Redirected { get; set; }
        public bool ConnectionClosed { get; set; }

        public FhirResponse ParseFhirResource()
        {
            var fhirResponse = new FhirResponse();

            switch (ContentType)
            {
                case Constants.ContentType.Application.FhirJson:
                    ResponseJSON = JObject.Parse(Body);
                    var jsonParser = new FhirJsonParser();
                    fhirResponse.Resource = jsonParser.Parse<Resource>(Body);
                    break;
                case Constants.ContentType.Application.FhirXml:
                    ResponseXML = XDocument.Parse(Body);
                    var xmlParser = new FhirXmlParser();
                    fhirResponse.Resource = xmlParser.Parse<Resource>(Body);
                    break;
            }

            return fhirResponse;
        }
    }
}
