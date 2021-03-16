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
    public class StructuredDocumentsSteps : BaseSteps
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
        private List<Patient> Patients => _httpContext.FhirResponse.Patients;

        public StructuredDocumentsSteps(HttpContext httpContext, HttpSteps httpSteps, BundleSteps bundleSteps, JwtSteps jwtSteps, HttpRequestConfigurationSteps httpRequestConfigurationSteps, IFhirResourceRepository fhirResourceRepository)
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

        [Given(@"I set the author parameters for a Documents Search call to ""(.*)""")]
        public void IsettheauthorparametersforaDocumentsSearchcall(string code)
        {
            var orgCode = GlobalContext.OdsCodeMap[code];
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("author", "https://fhir.nhs.uk/Id/ods-organization-code" + '|' + orgCode);
        }

        [Given(@"I set the author parameters with an invalid identifier for a Documents Search call to ""(.*)""")]
        public void IsettheauthorparameterswithaninvalididentifierforaDocumentsSearchcallto(string code)
        {
            var orgCode = GlobalContext.OdsCodeMap[code];
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("author", "https://bad-identifier" + '|' + orgCode);
        }

        [Given(@"I set an invalid parameter for a Documents Search call")]
        public void IsetaninvalidparameterforaDocumentsSearchcall()
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("BadParameter", "BadParamValue");
        }

        [Then(@"I save a document url for retrieving later")]
        public void Isaveadocumenturlforretrievinglater()
        {
            Documents.Count().ShouldBeGreaterThanOrEqualTo(1, "Fail :Expect atleast One DocumentReference Returned for Test");

            GlobalContext.DocumentURL = Documents.FirstOrDefault().Content.FirstOrDefault().Attachment.Url;
            Logger.Log.WriteLine("Info : Found Document URL in DocumentReference : " + GlobalContext.DocumentURL);
            if (Documents.FirstOrDefault().Content.FirstOrDefault().Attachment.Size != null)
            {
                Logger.Log.WriteLine("Info : Found Document Size in DocumentReference : " + Documents.FirstOrDefault().Content.FirstOrDefault().Attachment.Size);
            }
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
            //WIP - This is not finished as work was stopped as endpoint to retrieve document may
            //be different to document endpoint and we cannot know that to request a fake doc

            //var httpRequestConfiguration = new HttpRequestConfiguration();
            ////GlobalContext.DocumentURL = "/Binary/99999999999";

            ////Make Fake document address to generate error condition
            //if (AppSettingsHelper.UseTLSDocuments)
            //{
            //    //create fake address from real endpoint address and adding fake doc id
            //    GlobalContext.DocumentURL = "https://" + AppSettingsHelper.ServerUrlDocuments + AppSettingsHelper.ServerBaseDocuments + "/Binary/99999999999";
            //}
            //else
            //{
            //    GlobalContext.DocumentURL = "http://" + AppSettingsHelper.ServerUrlDocuments + ":" + AppSettingsHelper.ServerHttpPortDocuments + AppSettingsHelper.ServerBaseDocuments + "/Binary/99999999999";
            //}
        }

        [Given(@"I change the patient logical id to a non existent id")]
        public void Ichangethepatientlogicalidtoanonexistantid()
        {
            _httpContext.HttpRequestConfiguration.RequestUrl = "Patient/" + "9999999999999" + "/DocumentReference";
        }

        [Then(@"I clear the saved document url")]
        public void Iclearthesaveddocumenturl()
        {
            GlobalContext.DocumentURL = "";
            Logger.Log.WriteLine("Info : Cleared Document URL");
        }

        [Then(@"I set the documents search parameters le to today and ge to 365 days ago")]
        public void Isetthedocumentssearchparametersletotodayandgeto365daysago()
        {
            var val = TimePeriodHelper.GetTimePeriodStartDateTomorrowEndDateDays(365);
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

        [Then(@"I Check the returned DocumentReference is Valid")]
        public void ICheckthereturnedDocumentReferenceisValid()
        {
            Documents.Count().ShouldBeGreaterThanOrEqualTo(1, "Fail :Expect atleast One DocumentReference Returned for Test");

            Documents.ForEach(doc =>
            {
                //Check ID
                doc.Id.ShouldNotBeNullOrEmpty();

                //Check Meta.Profile
                CheckForValidMetaDataInResource(doc, FhirConst.StructureDefinitionSystems.kDocumentReference);

                //Check identifier
                doc.Identifier.Count.ShouldBeGreaterThan(0, "Fail : There should be at least 1 Identifier system/value pair");
                doc.Identifier.ForEach(identifier =>
                {
                    identifier.System.ShouldNotBeNullOrEmpty("Identifier System Is Null or Empty - Should be populated");
                    identifier.Value.ShouldNotBeNullOrEmpty("Fail : Identifier Value Is Null or Not Empty - Expect Value");
                });

                //check status
                doc.Status.ShouldBe(DocumentReferenceStatus.Current, "Fail : Status should be set to value current");

                //Check Type (should be codable concept with snomed code or a type.text
                if (doc.Type.Coding.Count() >= 1)
                {
                    doc.Type.Coding.ForEach(code =>
                    {
                        code.System.Equals(FhirConst.CodeSystems.kCCSnomed);
                        code.Code.ShouldNotBeNullOrEmpty();
                        code.Display.ShouldNotBeNullOrEmpty();
                    });
                }
                else
                {
                    doc.Type.Text.ShouldNotBeNullOrEmpty("Fail : DocumentReference Type should be either a codable concept with a snomed code or have type.text populated");
                }

                //Check Subject/patient
                doc.Subject.Reference.ShouldNotBeNullOrEmpty("Fail : Patient Reference should be included in DocumentReference");
                Patients.Where(p => p.Id == (doc.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Fail : Patient Not Found in Bundle");

                //check indexed
                doc.Indexed.ShouldBeOfType<DateTimeOffset>();

                //check content.attachment
                doc.Content.ForEach(content =>
                {
                    content.Attachment.Size.ShouldNotBeNull("Fail : Content Attachment Size should not be null");
                    content.Attachment.ContentType.ShouldNotBeNullOrEmpty("Fail : ContentType should be populated");
                });
            });
        }

        [Then(@"I Check the returned DocumentReference Do Not Include Not In Use Fields")]
        public void ICheckthereturnedDocumentReferenceDoNotIncludeNotInUseFields()
        {
            Documents.ForEach(doc =>
            {
                doc.DocStatus.ShouldBeNull("Fail :  DocumentReference - DocStatus element Should not be used - Not In Use Field");
                doc.Class.ShouldBeNull("Fail :  DocumentReference - Class element Should not be used - Not In Use Field");
                doc.Authenticator.ShouldBeNull("Fail :  DocumentReference - Authenticator element Should not be used - Not In Use Field");
                doc.Content.ForEach(content =>
                {
                    content.Attachment.Data.ShouldBeNull("Fail :  DocumentReference - Content.Attachment.Data element Should not be used - Not In Use Field");
                });
                doc.RelatesTo.Count().ShouldBe(0, "Fail :  DocumentReference - RelatesTo element Should not be used - Not In Use Field");
                doc.SecurityLabel.Count().ShouldBe(0, "Fail :  DocumentReference - SecurityLabel element Should not be used - Not In Use Field");
            });
        }

        [Then(@"I Check the returned Binary Document is Valid")]
        public void ICheckthereturnedBinaryDocumentisValid()
        {
            BinaryDocument.ContentType.ShouldNotBeNullOrEmpty("Fail : Binary Document Should be sent with a ContentType Element populated");
            BinaryDocument.Content.ShouldNotBeNull("Fail : Binary Document Should be sent with a Content Element populated");
        }

        [Then(@"I Check the returned Binary Document Do Not Include Not In Use Fields")]
        public void ICheckthereturnedBinaryDocumentDoNotIncludeNotInUseFields()
        {
            BinaryDocument.SecurityContext.ShouldBeNull("Fail :  Binary Document - SecurityContext element Should not be used - Not In Use Field");
        }

        [Then(@"The Bundle should contain NO Documents")]
        public void TheBundleshouldcontainNODocuments()
        {
            Documents.Count().ShouldBe(0, "Fail :Expect NO DocumentReferences Returned for Test");
        }
    }
}