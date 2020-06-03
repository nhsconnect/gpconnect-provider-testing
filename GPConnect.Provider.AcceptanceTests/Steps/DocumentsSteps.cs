using System.Linq;
using GPConnect.Provider.AcceptanceTests.Constants;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Context;
    using Enum;
    using GPConnect.Provider.AcceptanceTests.Helpers;
    using GPConnect.Provider.AcceptanceTests.Http;
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
           
            GlobalContext.DocumentContent = BinaryDocument.Content;
            GlobalContext.DocumentID = BinaryDocument.Id;
            GlobalContext.DocumentContentType = BinaryDocument.ContentType;
        }

        [Given(@"I change the document to retrieve to one that doesnt exist")]
        public void Ichangethedocumenttoretrievetoonethatdoesntexist()
        {
            var httpRequestConfiguration = new HttpRequestConfiguration();
            GlobalContext.DocumentURL = httpRequestConfiguration.EndpointAddress + "/Binary/99999999999";
        }

        [Then(@"I clear the saved document url")]
        public void Iclearthesaveddocumenturl()
        {
            GlobalContext.DocumentURL = "";
        }


        [Then(@"I set the created search parameters with a time period of ""(.*)"" days")]
        public void Isetthecreatedsearchparameterswithatimeperiodofdays(int days)
        {
            var val = TimePeriodHelper.GetTimePeriodStartDateTomorrowEndDateDays(days);
            Given($"I add the parameter \"created\" with the value \"ge{val.Start}\"");
            Given($"I add the parameter \"created\" with the value \"le{val.End}\"");
        }

        [Then(@"I set the created search parameter to less than ""(.*)"" days ago")]
        public void Isetthecreatedsearchparametestolessthandaysago(int days)
        {
            var date = DateTime.UtcNow.Date.ToLocalTime();
            var val = new FhirDateTime(date.AddDays(-days));
            Given($"I add the parameter \"created\" with the value \"le{val}\"");
        }

        [Then(@"I set the created search parameter to greater than ""(.*)"" days ago")]
        public void Isetthecreatedsearchparametertogreaterthandaysago(int days)
        {
            var date = DateTime.UtcNow.Date.ToLocalTime();
            var val = new FhirDateTime(date.AddDays(-days));
            Given($"I add the parameter \"created\" with the value \"ge{val}\"");
        }


    }
}

