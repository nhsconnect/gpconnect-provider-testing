using System.Linq;
using GPConnect.Provider.AcceptanceTests.Constants;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Context;
    using Enum;
    using Hl7.Fhir.Model;
    using Repository;
    using Shouldly;
    using TechTalk.SpecFlow;
    using static Hl7.Fhir.Model.Slot;

    [Binding]
    public class DocumentsSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private readonly BundleSteps _bundleSteps;
        private readonly JwtSteps _jwtSteps;
        private readonly HttpRequestConfigurationSteps _httpRequestConfigurationSteps;
        private readonly IFhirResourceRepository _fhirResourceRepository;

        private List<Slot> Slots => _httpContext.FhirResponse.Slots;
        private List<Schedule> Schedules => _httpContext.FhirResponse.Schedules;
        private List<DocumentReference> Documents => _httpContext.FhirResponse.Documents;
        private Binary BinaryDocument => _httpContext.FhirResponse.BinaryDocument;

        public DocumentsSteps(HttpContext httpContext, HttpSteps httpSteps, BundleSteps bundleSteps, JwtSteps jwtSteps, HttpRequestConfigurationSteps httpRequestConfigurationSteps, IFhirResourceRepository fhirResourceRepository)
            : base(httpSteps)
        {
            _httpContext = httpContext;
            _bundleSteps = bundleSteps;
            _jwtSteps = jwtSteps;
            _httpRequestConfigurationSteps = httpRequestConfigurationSteps;
            _fhirResourceRepository = fhirResourceRepository;
        }


        [Given(@"I set the required parameters for a Documents Search call")]
        public void SetRequiredParametersWithTimePeriod()
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("_include", "DocumentReference:subject:Patient");
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("_include", "DocumentReference:custodian:Organization");
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("_include", "DocumentReference:author:Organization");
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("_include", "DocumentReference:author:Practitioner");
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("_revinclude:recurse", "PractitionerRole:practitioner");
        }


        [Then(@"I save a document url for retrieving later")]
        public void Isaveadocumenturlforretrievinglater()
        {
            Documents.Count().ShouldBeGreaterThanOrEqualTo(1, "Fail :Expect atleast One DocumentReference Returned for Test");

            GlobalContext.DocumentURL = Documents.FirstOrDefault().Content.FirstOrDefault().Attachment.Url;
            Logger.Log.WriteLine("Info : Found Document URL in DocumentReference : " + GlobalContext.DocumentURL);

        }

        [Then(@"I save the binary document from the retrieve")]
        public void Isavethebinarydocumentfromtheretrieve()
        {
            BinaryDocument.ShouldNotBeNull("Fail : Expect Binary Document to have been Returned - failed to retrieve one");
            
            
            File.WriteAllBytes("c:\\testdoc.xml", BinaryDocument.Content);

            //put content of mime encoded doc on  global context so it can be saved into evidence folder
            
            //need to add new variable for content on globalcontext
            //GlobalContext.DocumentURL
        }

    }
}

