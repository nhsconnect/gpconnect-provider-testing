using System.Linq;
using GPConnect.Provider.AcceptanceTests.Constants;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using Context;
    using Enum;
    using GPConnect.Provider.AcceptanceTests.Helpers;
    using GPConnect.Provider.AcceptanceTests.Http;
    using Hl7.Fhir.Model;
    using Repository;
    using Shouldly;
    using TechTalk.SpecFlow;
    using static Hl7.Fhir.Model.Parameters;

    [Binding]
    public class StructuredDiarySteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private readonly BundleSteps _bundleSteps;
        private readonly JwtSteps _jwtSteps;
        private readonly HttpRequestConfigurationSteps _httpRequestConfigurationSteps;
        private readonly IFhirResourceRepository _fhirResourceRepository;

        private Bundle Bundle => _httpContext.FhirResponse.Bundle;
        private List<List> Lists => _httpContext.FhirResponse.Lists;
        private List<Patient> Patients => _httpContext.FhirResponse.Patients;
        private List<ProcedureRequest> ProcedureRequests => _httpContext.FhirResponse.ProcedureRequests;
        private List<Condition> Problems => _httpContext.FhirResponse.Conditions;

        public StructuredDiarySteps(HttpContext httpContext, HttpSteps httpSteps, BundleSteps bundleSteps, JwtSteps jwtSteps, HttpRequestConfigurationSteps httpRequestConfigurationSteps, IFhirResourceRepository fhirResourceRepository)
            : base(httpSteps)
        {
            _httpContext = httpContext;
            _bundleSteps = bundleSteps;
            _jwtSteps = jwtSteps;
            _httpRequestConfigurationSteps = httpRequestConfigurationSteps;
            _fhirResourceRepository = fhirResourceRepository;
        }


        [Given(@"I add the Diary parameter")]
        public void GivenIAddTheInvestigationsParameter()
        {
            ParameterComponent param = new ParameterComponent();
            param.Name = FhirConst.GetStructuredRecordParams.kDiary;
            _httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(param);
        }

        //[Given(@"I add the investigations data parameter with future start date")]
        //public void GivenIAddTheinvestigationsDataParameterWithFutureStartDate()
        //{
        //    var futureStartDate = DateTime.UtcNow.AddDays(+10);
        //    var futureEndDate = DateTime.UtcNow.AddDays(+15);
        //    var startDate = futureStartDate.ToString("yyyy-MM-dd");
        //    var endDate = futureEndDate.ToString("yyyy-MM-dd");

        //    IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
        //        Tuple.Create(FhirConst.GetStructuredRecordParams.kInvestigationsSearch, (Base)FhirHelper.GetTimePeriod(startDate, endDate)),
        //    };
        //    _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kInvestigations, tuples);
        //}


        [Then(@"I Check the Diary List is Valid")]
        public void ThenIChecktheDiaryListareValid()
        {
            //Check atleast one Diary List exists using Snomed Code
            Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kDiary).ToList().Count().ShouldBe(1, "Failed to Find ONE Diary list using Snomed Code.");

            //Get  Diary list
            var diaryLists = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kDiary).ToList();

            diaryLists.ForEach(diaryList =>
            {
                //Check Meta.profile
                CheckForValidMetaDataInResource(diaryList, FhirConst.StructureDefinitionSystems.kList);

                //Check Status
                diaryList.Status.ToString().ToLower().ShouldBe("current", "Diary List Status is NOT set to current");

                //Check Mode
                diaryList.Mode.ShouldBeOfType<ListMode>("Mode List is of wrong type.");
                diaryList.Mode.ToString().ToLower().ShouldBe("snapshot", "List Status is NOT set to snapshot");

                //Check title
                diaryList.Title.ShouldBe(FhirConst.ListTitles.kDiary, "Investigations List Title is Incorrect");

                //Check code
                diaryList.Code.Coding.ForEach(coding =>
                {
                    coding.System.ShouldBeOneOf("http://snomed.info/sct", "http://read.info/readv2", "http://read.info/ctv3", "https://fhir.hl7.org.uk/Id/emis-drug-codes", "https://fhir.hl7.org.uk/Id/egton-codes", "https://fhir.hl7.org.uk/Id/multilex-drug-codes", "https://fhir.hl7.org.uk/Id/resipuk-gemscript-drug-codes");
                    coding.Display.ShouldNotBeNullOrEmpty("Display Should not be Null or Empty");
                });

                //Check Patient
                Patients.Where(p => p.Id == (diaryList.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");

                //check number of Diary -ProcedureRequest's matches number in list
                ProcedureRequests.Count().ShouldBe(diaryList.Entry.Count(), "Number of ProcedureRequest's does not match the number in the Diary List");

                //Check each Entry/Item/Reference points to a resource that exists
                bool found = false;
                string pattern = @"(.*/)(.*)";
                diaryList.Entry.Count().ShouldBeGreaterThan(1, "Fail : Atleast One Diary ProcedureRequest entry should be on the Diary List");
                diaryList.Entry.ForEach(entry =>
                {
                    string refToFind = Regex.Replace(entry.Item.Reference, pattern, "$2");
                    Bundle.GetResources()
                                .Where(resource => resource.ResourceType.Equals(ResourceType.ProcedureRequest))
                                .Where(resource => resource.Id == refToFind)
                                .ToList().Count().ShouldBe(1, "ProcedureRequest resource Not Found with id : " + refToFind);
                    Logger.Log.WriteLine("Diary List - Verified the Linked ProcedureRequest's has been included In the Bundle: " + refToFind);
                    found = true;
                });

                //Check we found atleast One ProcedureRequest Linked in list
                found.ShouldBeTrue("Fail : Diary List should be linked to atleast one ProcedureRequest");

                Logger.Log.WriteLine("Completed Mandatory checks on Diary List");
            });
        }


        [Then(@"I Check the Diary ProcedureRequests are Valid")]
        public void ThenIChecktheProcedureRequestareValid()
        {
            //check atleast one
            ProcedureRequests.ToList().Count().ShouldBeGreaterThan(0, "Error Should be Atleast One ProcedureRequest in response as per Data requirements");

            ProcedureRequests.ForEach(proc =>
            {
                //Check Id
                proc.Id.ShouldNotBeNullOrEmpty();

                //Check Meta.profile
                CheckForValidMetaDataInResource(proc, FhirConst.StructureDefinitionSystems.kProcedureRequest);

                //Check Identfier
                proc.Identifier.Count.ShouldBeGreaterThan(0, "There should be at least 1 Identifier system/value pair");
                proc.Identifier.ForEach(identifier =>
                {
                    identifier.Value.ShouldNotBeNullOrEmpty("Fail : No Identifier found when resource should have a unique Identifier");
                });

                //Check Status
                proc.Status.ToString().ToLower().ShouldBe("active", "ProcedureRequest Status is NOT set to active");

                //check intent
                proc.Intent.ToString().ToLower().ShouldBe("plan", "ProcedureRequest Status is NOT set to plan");

                //Check Subject/patient
                Patients.Where(p => p.Id == (proc.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");

                //Check authoredOn
                proc.AuthoredOn.ShouldNotBeNullOrEmpty("Fail : Diary ProcedureRequest - AuthoredOn should be populated with a dateTime");

                //Check requester
                proc.Requester.Agent.Reference.ShouldNotBeNullOrEmpty("Fail : Requester.Agent.Reference Should be populated with either a Practitioner or Organization Reference");
                string pattern = @"(.*)(/.*)";
                string refType = Regex.Replace(proc.Requester.Agent.Reference, pattern, "$1");
                refType.ShouldBeOneOf("Practitioner", "Organization");




            });
        }


        [Then(@"I Check the Diary ProcedureRequests Do Not Include Not in Use Fields")]
        public void ThenIChecktheProcedureRequestDoNotIncludeNotinUseFields()
        {
            ProcedureRequests.ForEach(procs =>
            {
                procs.Priority.ShouldBeNull("Fail :  ProcedureRequest - Priority element Should not be used - Not In Use Field");
                procs.DoNotPerform.ShouldBeNull("Fail :  ProcedureRequest - DoNotPerform element Should not be used - Not In Use Field");
                procs.Category.Count().ShouldBe(0, "Fail :  ProcedureRequest - Category element Should not be used - Not In Use Field");
                procs.AsNeeded.ShouldBeNull("Fail :  ProcedureRequest - AsNeeded element Should not be used - Not In Use Field");
                procs.Requester.OnBehalfOf.ShouldBeNull("Fail :  ProcedureRequest - Requester.OnBehalfOf element Should not be used - Not In Use Field");
                procs.PerformerType.ShouldBeNull("Fail :  ProcedureRequest - PerformerType element Should not be used - Not In Use Field");
                procs.Performer.ShouldBeNull("Fail :  ProcedureRequest - Performer element Should not be used - Not In Use Field");
                procs.ReasonReference.Count().ShouldBe(0, "Fail :  ProcedureRequest - ReasonReference element Should not be used - Not In Use Field");
                procs.Specimen.Count().ShouldBe(0, "Fail :  ProcedureRequest - Specimen element Should not be used - Not In Use Field");
                procs.BodySite.Count().ShouldBe(0, "Fail :  ProcedureRequest - BodySite element Should not be used - Not In Use Field");
                procs.RelevantHistory.Count().ShouldBe(0, "Fail :  ProcedureRequest - RelevantHistory element Should not be used - Not In Use Field");
            });
        }


        [Then(@"Check a Problem is linked to ProcedureRequest and that it is also included")]
        public void ThenCheckaProblemislinkedtoProcedureRequestandthatitisalsoincluded()
        {
            var found = false;
            string refToFind = "";

            foreach (var p in Problems)
            {
                Condition problem = (Condition)p;
                List<Extension> problemRelatedContentExtensions = p.Extension.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kExtProblemRelatedContent)).ToList();

                foreach (var rcc in problemRelatedContentExtensions)
                {
                    ResourceReference rr = (ResourceReference)rcc.Value;
                    if (rr.Reference.StartsWith("ProcedureRequest/"))
                    {
                        string pattern = @"(.*)(/)(.*)";
                        refToFind = Regex.Replace(rr.Reference, pattern, "$3");
                        found = true;
                        Logger.Log.WriteLine("Info : Problem - Found Linked to a ProcedureRequest - with ID : " + refToFind);
                        break;
                    }
                }
                if (found)
                    break;
            };

            found.ShouldBeTrue("Fail : No Problems found to be linked to a  ProcedureRequest");

            //check that Linked Clinical resource has been included in response.
            VerifyResourceReferenceExists("ProcedureRequest", refToFind);

        }


        public void VerifyResourceReferenceExists(string refTypeToFind, string refToFind)
        {
            //Switch on Clinical Item type
            switch (refTypeToFind)
            {
                case "ProcedureRequest":
                    CheckResourceExists(ResourceType.ProcedureRequest, refToFind);
                    break;

                //unknown type ignore - could be not supported message
                default:
                    Logger.Log.WriteLine("Ignored, Entry/Item/Reference for : " + refTypeToFind);
                    break;
            }
        }

        public void CheckResourceExists<T>(T resourceType, string resourceID)
        {
            Bundle.GetResources()
                           .Where(resource => resource.ResourceType.Equals(resourceType))
                           .Where(resource => resource.Id == resourceID)
                           .ToList().Count().ShouldBe(1, "Fail : Linked Resource Not Contained in Response - type : " + resourceType + " - ID : " + resourceID);

            Logger.Log.WriteLine("Found Linked resource : " + resourceID + " Of Type : " + resourceType);

        }

    }
}

