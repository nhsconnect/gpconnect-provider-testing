namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Xml.Linq;
    using Constants;
    using Context;
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Serialization;
    using Logger;
    using Newtonsoft.Json.Linq;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    public class FhirSteps : Steps
    {
        private readonly HttpContext _httpContext;

        // Constructor
        public FhirSteps(HttpContext httpContext)
        {
            Log.WriteLine("FhirSteps() Constructor");
            _httpContext = httpContext;            
        }

        [Then(@"the response body should be FHIR JSON")]
        public void ThenTheResponseBodyShouldBeFHIRJSON()
        {
            _httpContext.HttpResponse.ContentType.ShouldStartWith(ContentType.Application.FhirJson);
            Log.WriteLine("Response ContentType={0}", _httpContext.HttpResponse.ContentType);
            _httpContext.HttpResponse.ResponseJSON = JObject.Parse(_httpContext.HttpResponse.Body);
            FhirJsonParser fhirJsonParser = new FhirJsonParser();
            _httpContext.FhirResponse.Resource = fhirJsonParser.Parse<Resource>(_httpContext.HttpResponse.Body);
        }

        [Then(@"the response should be the format FHIR JSON")]
        public void TheResponseShouldBeTheFormatFHIRJSON()
        {
            _httpContext.HttpResponse.ContentType.ShouldStartWith(ContentType.Application.FhirJson);
        }

        [Then(@"the response should be the format FHIR XML")]
        public void TheResponseShouldBeTheFormatXMLJSON()
        {
            _httpContext.HttpResponse.ContentType.ShouldStartWith(ContentType.Application.FhirXml);
        }

        [Then(@"the response body should be empty")]
        public void ThenTheResponseBodyShouldBeEmpty()
        {
            _httpContext.HttpResponse.Body.ShouldBeNullOrEmpty("The response should not contain a payload but the response was not null or empty");
        }

        [Then(@"the response body should be FHIR XML")]
        public void ThenTheResponseBodyShouldBeFHIRXML()
        {
            _httpContext.HttpResponse.ContentType.ShouldStartWith(ContentType.Application.FhirXml);
            Log.WriteLine("Response ContentType={0}", _httpContext.HttpResponse.ContentType);
            // TODO Move XML Parsing Out Of Here
            _httpContext.HttpResponse.ResponseXML = XDocument.Parse(_httpContext.HttpResponse.Body);
            FhirXmlParser fhirXmlParser = new FhirXmlParser();
            _httpContext.FhirResponse.Resource = fhirXmlParser.Parse<Resource>(_httpContext.HttpResponse.Body);
        }
    }
}
