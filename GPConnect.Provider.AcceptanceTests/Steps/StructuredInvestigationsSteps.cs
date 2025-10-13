namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using Constants;
    using Context;
    using TechTalk.SpecFlow;
    using Shouldly;
    using Hl7.Fhir.Model;
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using GPConnect.Provider.AcceptanceTests.Enum;
    using static Hl7.Fhir.Model.Parameters;
    using GPConnect.Provider.AcceptanceTests.Helpers;
    using System.Text.RegularExpressions;

    [Binding]
    public sealed class StructuredInvestigationsSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private List<DiagnosticReport> DiagnosticReports => _httpContext.FhirResponse.DiagnosticReports;
        private List<List> Lists => _httpContext.FhirResponse.Lists;
        private List<Patient> Patients => _httpContext.FhirResponse.Patients;
        private Bundle Bundle => _httpContext.FhirResponse.Bundle;
        private List<ProcedureRequest> ProcedureRequests => _httpContext.FhirResponse.ProcedureRequests;
        private List<Specimen> Specimens => _httpContext.FhirResponse.Specimens;
        private List<Observation> Observations => _httpContext.FhirResponse.Observations;
        private List<Condition> Problems => _httpContext.FhirResponse.Conditions;

        public StructuredInvestigationsSteps(HttpSteps httpSteps, HttpContext httpContext)
            : base(httpSteps)
        {
            _httpContext = httpContext;
        }

        [Given(@"I add the Investigations parameter")]
        public void GivenIAddTheInvestigationsParameter()
        {
            ParameterComponent param = new ParameterComponent();
            param.Name = FhirConst.GetStructuredRecordParams.kInvestigations;
            _httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(param);
        }

        [Then(@"I Check the Investigations List is Valid")]
        public void ThenIChecktheInvestigationsListsareValid()
        {
            //Check atleast one Investigations List exists using Snomed Code
            Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kInvestigations).ToList().Count().ShouldBe(1, "Failed to Find ONE Investigations list using Snomed Code.");

            //Get  Investigations list
            var investigationList = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kInvestigations).ToList();

            investigationList.ForEach(investList =>
            {
                //Check Meta.profile
                CheckForValidMetaDataInResource(investList, FhirConst.StructureDefinitionSystems.kList);

                //Check Status
                investList.Status.ToString().ToLower().ShouldBe("current", "List Status is NOT set to current");

                //Check Mode
                investList.Mode.ShouldBeOfType<ListMode>("Mode List is of wrong type.");
                investList.Mode.ToString().ToLower().ShouldBe("snapshot", "List Status is NOT set to snapshot");

                //Check title
                investList.Title.ShouldBe(FhirConst.ListTitles.kInvestigations, "Investigations List Title is Incorrect");

                //Check code
                investList.Code.Coding.ForEach(coding =>
                {
                    coding.System.ShouldBeOneOf("http://snomed.info/sct", "http://read.info/readv2", "http://read.info/ctv3", "https://fhir.hl7.org.uk/Id/emis-drug-codes", "https://fhir.hl7.org.uk/Id/egton-codes", "https://fhir.hl7.org.uk/Id/multilex-drug-codes", "https://fhir.hl7.org.uk/Id/resipuk-gemscript-drug-codes");
                    coding.Display.ShouldNotBeNullOrEmpty("Display Should not be Null or Empty");
                });

                //Check Patient
                Patients.Where(p => p.Id == (investList.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");

                //check number of DiagnosticReports matches number in list
                DiagnosticReports.Count().ShouldBe(investList.Entry.Count(), "Number of DiagnosticReports does not match the number in the List");

                //Check each Entry/Item/Reference points to a resource that exists
                bool found = false;
                string pattern = @"(.*/)(.*)";
                investList.Entry.ForEach(entry =>
                {
                    string refToFind = Regex.Replace(entry.Item.Reference, pattern, "$2");
                    Bundle.GetResources()
                                .Where(resource => resource.ResourceType.Equals(ResourceType.DiagnosticReport))
                                .Where(resource => resource.Id == refToFind)
                                .ToList().Count().ShouldBe(1, "DiagnosticReport resource Not Found with id : " + refToFind);
                    Logger.Log.WriteLine("Investigations List - Verified the Linked DiagnosticReport has been included In the Bundle: " + refToFind);
                    found = true;
                });

                //Check we found atleast One DiagnosticReport Linked in list
                found.ShouldBeTrue("Fail : Investigations List should be linked to atleast one diagnostic report");

                Logger.Log.WriteLine("Completed Mandatory checks on Investigations List");
            });
        }

        [Then(@"I Check the DiagnosticReports are Valid")]
        public void ThenIChecktheDiagnosticReportsareValid()
        {
            CheckDiagnosticReportCommonElements();
        }

        [Then(@"I Check the DiagnosticReports are Valid and Linked to a ProcedureRequest")]
        public void ThenIChecktheDiagnosticReportsareValidandLinkedtoaProcedureRequest()
        {
            CheckDiagnosticReportCommonElements();
            CheckProcedureRequestLinkedToDiagnosticReport();
        }

        [Then(@"I Check the DiagnosticReports Do Not Include Not in Use Fields")]
        public void ThenIChecktheDiagnosticReportsDoNotIncludeNotinUseFields()
        {
            DiagnosticReports.ForEach(diag =>
            {
                diag.Effective.ShouldBeNull("Fail :  DiagnosticReport - effective element Should not be used - Not In Use Field");
                diag.ImagingStudy.Count().ShouldBe(0, "Fail :  DiagnosticReport - imagingStudy element Should not be used - Not In Use Field");
                diag.Image.Count().ShouldBe(0, "Fail :  DiagnosticReport - image element Should not be used - Not In Use Field");
                diag.PresentedForm.Count().ShouldBe(0, "Fail :  DiagnosticReport - PresentedForm element Should not be used - Not In Use Field");
            });
        }

        public void CheckDiagnosticReportCommonElements()
        {
            //check atleast one
            DiagnosticReports.ToList().Count().ShouldBeGreaterThan(0, "Error Should be Atleast One DiagnosticReport in response as per Data requirements");

            //bool foundProcedureRequest = false;
            bool foundSpecimen = false;
            bool foundlinkedTestGroup = false;
            bool foundSpecimenLinkedToTestGroup = false;
            //bool foundTestReportFiling = false;

            DiagnosticReports.ForEach(diagnostic =>
            {
                //Check Id
                diagnostic.Id.ShouldNotBeNullOrEmpty();

                //Check Meta.profile
                CheckForValidMetaDataInResource(diagnostic, FhirConst.StructureDefinitionSystems.kDiagnosticReport);

                //Check Identifier
                diagnostic.Identifier.Count.ShouldBeGreaterThan(0, "There should be at least 1 Identifier system/value pair");
                diagnostic.Identifier.ForEach(identifier =>
                {
                    identifier.System.ShouldNotBeNullOrEmpty("Identifier System Is Null or Empty - Should be populated");
                    identifier.Value.ShouldNotBeNullOrEmpty("Fail : No Identifier found when resource should have a unique Identifier");
                });

                string refToCheck = "";
                string pattern = @"(.*)(/)(.*)";

                //Check Status
                diagnostic.Status.ToString().ShouldNotBeNull("DiagnosticReport Status should not be null");

                //check Code
                diagnostic.Code.Coding.First().Code.ShouldBe("721981007");
                diagnostic.Code.Coding.First().Display.ShouldBe("Diagnostic studies report");

                //Check Subject/patient
                Patients.Where(p => p.Id == (diagnostic.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");

                //check issued
                diagnostic.Issued.ShouldNotBeNull("Fail : Issued should contain a datetime");

                //check atleast one Specimen Reference found and item included in bundle
                refToCheck = "";
                if (diagnostic.Specimen != null)
                {
                    if (diagnostic.Specimen.Count() >= 1)
                    {
                        refToCheck = diagnostic.Specimen.First().Reference;
                        if (refToCheck.StartsWith("Specimen/"))
                        {
                            string refToFind = Regex.Replace(refToCheck, pattern, "$3");
                            //check Resource Exists
                            VerifyResourceReferenceExists("Specimen", refToFind);
                            Logger.Log.WriteLine("Info : Found and Verified Specimen");
                            foundSpecimen = true;
                        }
                    }
                }

                //check diagnosticreport is linked to a test group and a test report filing that is also linked to a specimen
                if (diagnostic.Result != null)
                {
                    if (diagnostic.Result.Count() >= 1)
                    {
                        diagnostic.Result.ForEach(d =>
                        {
                            refToCheck = d.Reference;
                            if (refToCheck.StartsWith("Observation/"))
                            {
                                //check and verify observation
                                string refToFind = Regex.Replace(refToCheck, pattern, "$3");
                                VerifyResourceReferenceExists("Observation", refToFind);
                                Logger.Log.WriteLine("Info : Found and Verified Observation - Test Result Linked to DiagnosticReport");
                                foundlinkedTestGroup = true;

                                //check if observation has a link to specimen
                                Observation currentObservation = Observations.Where(o => o.Id == refToFind).FirstOrDefault();
                                if (currentObservation.Specimen != null)
                                {
                                    if (currentObservation.Specimen.Reference.StartsWith("Specimen/"))
                                    {
                                        string specimenRefToFind = Regex.Replace(currentObservation.Specimen.Reference, pattern, "$3");
                                        VerifyResourceReferenceExists("Specimen", specimenRefToFind);
                                        Logger.Log.WriteLine("Info : Found Specimen linked to Test group");
                                        foundSpecimenLinkedToTestGroup = true;
                                    }
                                }

                                //16/2/2021 - PG - removed from automation and check moved into manual as EMIS do not Support this linkage
                                //check if observation is a test report filing as per data requirements - should be one
                                //if (currentObservation.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kObservationCommentNoteCode)
                                //{
                                //    //found a Test filing Comment
                                //    foundTestReportFiling = true;
                                //}
                            }
                        });
                    }
                }
            });

            //Check Data Requirements were met
            foundSpecimen.ShouldBeTrue("Fail : No link to a Specimen found on any DiagnosticReport");
            foundlinkedTestGroup.ShouldBeTrue("fail : No link to a Test Group found on any DiagnosticReport");
            foundSpecimenLinkedToTestGroup.ShouldBeTrue("fail : No link to a Specimen from a Test group Found");
            //foundTestReportFiling.ShouldBeTrue("fail : No link found to a Test report Filing from any DiagnosticReport");
        }

        public void CheckProcedureRequestLinkedToDiagnosticReport()
        {
            //check atleast one
            DiagnosticReports.ToList().Count().ShouldBeGreaterThan(0, "Error Should be Atleast One DiagnosticReport in response as per Data requirements");

            bool foundProcedureRequest = false;

            DiagnosticReports.ForEach(diagnostic =>
            {
                ////check atleast one diagnosticReport is linked to a Procedurerequest(and included in bundle) as per data requirements
                string refToCheck = "";
                string pattern = @"(.*)(/)(.*)";

                if (diagnostic.BasedOn != null)
                {
                    if (diagnostic.BasedOn.Count() >= 1)
                    {
                        refToCheck = diagnostic.BasedOn.First().Reference;
                        if (refToCheck.StartsWith("ProcedureRequest/"))
                        {
                            string refToFind = Regex.Replace(refToCheck, pattern, "$3");
                            //check Resource Exists
                            VerifyResourceReferenceExists("ProcedureRequest", refToFind);
                            Logger.Log.WriteLine("Info : Found and Verified ProcedureRequest");
                            foundProcedureRequest = true;
                        }
                    }
                }

                //Check Data Requirements were met
                foundProcedureRequest.ShouldBeTrue("Fail : No link to a ProcedureRequest found on any DiagnosticReport as per data requirements");
            });
        }

        [Then(@"I Check the ProcedureRequests are Valid")]
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
                proc.Intent.ToString().ToLower().ShouldBe("order", "ProcedureRequest Status is NOT set to order");

                //Check Subject/patient
                Patients.Where(p => p.Id == (proc.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");
            });
        }

        [Then(@"I Check the ProcedureRequests Do Not Include Not in Use Fields")]
        public void ThenIChecktheProcedureRequestDoNotIncludeNotinUseFields()
        {
            ProcedureRequests.ForEach(procs =>
            {
                procs.Definition.Count().ShouldBe(0, "Fail :  ProcedureRequest - Definition element Should not be used - Not In Use Field");
                procs.BasedOn.Count().ShouldBe(0, "Fail :  ProcedureRequest - BasedOn element Should not be used - Not In Use Field");
                procs.Replaces.Count().ShouldBe(0, "Fail :  ProcedureRequest - Replaces element Should not be used - Not In Use Field");
                procs.Requisition.ShouldBeNull("Fail :  ProcedureRequest - Requisition element Should not be used - Not In Use Field");
                procs.Priority.ShouldBeNull("Fail :  ProcedureRequest - Priority element Should not be used - Not In Use Field");
                procs.DoNotPerform.ShouldBeNull("Fail :  ProcedureRequest - DoNotPerform element Should not be used - Not In Use Field");
                procs.Category.Count().ShouldBe(0, "Fail :  ProcedureRequest - Category element Should not be used - Not In Use Field");
                procs.Context.ShouldBeNull("Fail :  ProcedureRequest - Context element Should not be used - Not In Use Field");
                procs.Occurrence.ShouldBeNull("Fail :  ProcedureRequest - Occurrence element Should not be used - Not In Use Field");
                procs.AsNeeded.ShouldBeNull("Fail :  ProcedureRequest - AsNeeded element Should not be used - Not In Use Field");
                procs.AuthoredOn.ShouldBeNull("Fail :  ProcedureRequest - AuthoredOn element Should not be used - Not In Use Field");
                procs.PerformerType.ShouldBeNull("Fail :  ProcedureRequest - PerformerType element Should not be used - Not In Use Field");
                procs.SupportingInfo.Count().ShouldBe(0, "Fail :  ProcedureRequest - SupportingInfo element Should not be used - Not In Use Field");
                procs.Specimen.Count().ShouldBe(0, "Fail :  ProcedureRequest - Specimen element Should not be used - Not In Use Field");
                procs.BodySite.Count().ShouldBe(0, "Fail :  ProcedureRequest - BodySite element Should not be used - Not In Use Field");
                procs.RelevantHistory.Count().ShouldBe(0, "Fail :  ProcedureRequest - RelevantHistory element Should not be used - Not In Use Field");
            });
        }

        [Then(@"I Check the Specimens are Valid")]
        public void ThenIChecktheSpecimensareValid()
        {
            //check atleast one
            Specimens.ToList().Count().ShouldBeGreaterThan(0, "Error Should be Atleast One Specimens in response as per Data requirements");

            Specimens.ForEach(specimen =>
            {
                //Check Id
                specimen.Id.ShouldNotBeNullOrEmpty();

                //Check Meta.profile
                CheckForValidMetaDataInResource(specimen, FhirConst.StructureDefinitionSystems.kSpecimen);

                //Check Identfier
                specimen.Identifier.Count.ShouldBeGreaterThan(0, "There should be at least 1 Identifier system/value pair");
                specimen.Identifier.ForEach(identifier =>
                {
                    identifier.System.ShouldNotBeNullOrEmpty("Identifier System Is Null or Empty - Should be populated");
                    identifier.Value.ShouldNotBeNullOrEmpty("Fail : No Identifier found when resource should have a unique Identifier");
                });

                //Check Subject/patient
                Patients.Where(p => p.Id == (specimen.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");
            });
        }

        [Then(@"I Check the Specimens Do Not Include Not in Use Fields")]
        public void ThenIChecktheSpecimensDoNotIncludeNotinUseFields()
        {
            Specimens.ForEach(specimen =>
            {
                specimen.Parent.Count().ShouldBe(0, "Fail :  Specimen - Parent element Should not be used - Not In Use Field");
                specimen.Request.Count().ShouldBe(0, "Fail :  Specimen - Request element Should not be used - Not In Use Field");
                if (specimen.Collection != null)
                {
                    specimen.Collection.Method.ShouldBeNull("Fail :  Specimen - Collection element Should not be used - Not In Use Field");
                }
                specimen.Processing.Count().ShouldBe(0, "Fail :  Specimen - Processing element Should not be used - Not In Use Field");
                specimen.Container.Count().ShouldBe(0, "Fail :  Specimen - Container element Should not be used - Not In Use Field");
            });
        }

        public void VerifyResourceReferenceExists(string refTypeToFind, string refToFind)
        {
            //Switch on Clinical Item type
            switch (refTypeToFind)
            {
                case "DiagnosticReport":
                    CheckResourceExists(ResourceType.DiagnosticReport, refToFind);
                    break;

                case "ProcedureRequest":
                    CheckResourceExists(ResourceType.ProcedureRequest, refToFind);
                    break;

                case "Specimen":
                    CheckResourceExists(ResourceType.Specimen, refToFind);
                    break;

                case "Observation":
                    CheckResourceExists(ResourceType.Observation, refToFind);
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

        [Given(@"I add the investigations data parameter with current date")]
        public void GivenIAddTheinvestigationsDataParameterWithCurrentDate()
        {
            var backDate = DateTime.UtcNow;
            var futureDate = DateTime.UtcNow;
            var startDate = backDate.ToString("yyyy-MM-dd");
            var endDate = futureDate.ToString("yyyy-MM-dd");

            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kInvestigationsSearch, (Base)FhirHelper.GetTimePeriod(startDate, endDate)),
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kInvestigations, tuples);
        }

        [Given(@"I add the investigations data parameter with date permutations ""([^""]*)"" and ""([^""]*)""")]
        public void GivenIAddTheinvestigationsDataParameterWithDatePermutations(string start, string end)
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kInvestigationsSearch, (Base)FhirHelper.GetTimePeriod(start, end)),
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kInvestigations, tuples);
        }

        [Given(@"I add the investigations data parameter with future start date")]
        public void GivenIAddTheinvestigationsDataParameterWithFutureStartDate()
        {
            var futureStartDate = DateTime.UtcNow.AddDays(+10);
            var futureEndDate = DateTime.UtcNow.AddDays(+15);
            var startDate = futureStartDate.ToString("yyyy-MM-dd");
            var endDate = futureEndDate.ToString("yyyy-MM-dd");

            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kInvestigationsSearch, (Base)FhirHelper.GetTimePeriod(startDate, endDate)),
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kInvestigations, tuples);
        }

        [Given(@"I add the investigations data parameter start date after endDate")]
        public void GivenIAddTheinvestigationsParameterStartDateAfterEndDate()
        {
            var backDate = DateTime.UtcNow.AddDays(-10);
            var futureDate = DateTime.UtcNow.AddDays(-15);
            var startDate = backDate.ToString("yyyy-MM-dd");
            var endDate = futureDate.ToString("yyyy-MM-dd");

            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kInvestigationsSearch, (Base)FhirHelper.GetTimePeriod(startDate, endDate)),
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kInvestigations, tuples);
        }

        [Given(@"I add the investigations data parameter with valid start date and no end date")]
        public void GivenIAddTheinvestigationsParameterWithValidStartDateAndNoEndDate()
        {
            var backDate = DateTime.UtcNow.AddYears(-3);
            var startDate = backDate.ToString("yyyy-MM-dd");

            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kInvestigationsSearch, (Base)FhirHelper.SafeGetTimePeriod(startDate, null)),
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kInvestigations, tuples);
        }

        [Given(@"I add the investigations data parameter with no start date and no end date")]
        public void GivenIAddTheinvestigationsParameterWithNoStartDateAndNoEndDate()
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kInvestigationsSearch, (Base)FhirHelper.SafeGetTimePeriod(null, null)),
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kInvestigations, tuples);
        }

        //Not currently used as no easy way to know what is a test group without being very specific about data requirements
        [Then(@"I Check a Test group is linked to a Test Report Filing")]
        public void GiveCheckaTestgroupislinkedtoaTestReportFiling()
        {
            //check atleast one
            Observations.ToList().Count().ShouldBeGreaterThan(0, "Error Should be Atleast One Observation Test Group or Test Report Filing in response as per Data requirements");
            bool foundTestReportFiling = false;

            //loop each observation and check each related reference
            Observations.ForEach(observation =>
            {
                //check each related looking for a link to a test report filing
                observation.Related.ForEach(rel =>
                {
                    string fullRefToFind = rel.Target.Reference;

                    if (fullRefToFind.StartsWith("Observation/"))
                    {
                        string pattern = @"(.*)(/)(.*)";
                        string refToFind = Regex.Replace(fullRefToFind, pattern, "$3");

                        //get handle to the related observation
                        Observation obToCheck = Observations.Where(o => o.Id == refToFind).FirstOrDefault();

                        //check if observation is a test report filing as per data requirements - should be one
                        if (obToCheck.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kObservationCommentNoteCode)
                        {
                            //found a Test report filing
                            foundTestReportFiling = true;
                        }
                    }
                });
            });

            foundTestReportFiling.ShouldBeTrue("Fail : No Test Group found with a link to a test report filing as per data requirements");
        }

        [Then(@"Check a Problem is linked to DiagnosticReport and that it is also included")]
        public void ThenCheckaProblemislinkedtoanthatisalsoincludedintheresponsewithalist()
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
                    if (rr.Reference != null)
                    {
                        if (rr.Reference.StartsWith("DiagnosticReport/"))
                        {
                            string pattern = @"(.*)(/)(.*)";
                            refToFind = Regex.Replace(rr.Reference, pattern, "$3");
                            found = true;
                            Logger.Log.WriteLine("Info : Problem - Found Linked to a DiagnosticReport - with ID : " + refToFind);
                            break;
                        }
                    }
                }
                if (found)
                    break;
            }
            ;

            found.ShouldBeTrue("Fail : No Problems found to be linked to a  DiagnosticReport");

            ////check that Linked Clinical resource has been included in response.
            VerifyResourceReferenceExists("DiagnosticReport", refToFind);
        }

        [Then(@"I Check the Test report Filing is Valid")]
        public void ThenIChecktheTestreportFilingisValid()
        {
            bool found = false;
            Observations.ForEach(obs =>
            {
                if (obs.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kObservationCommentNoteCode)
                {
                    //Check Id
                    obs.Id.ShouldNotBeNullOrEmpty();

                    //Check Meta.profile
                    CheckForValidMetaDataInResource(obs, FhirConst.StructureDefinitionSystems.kObservation);

                    //Check Identfier
                    obs.Identifier.Count.ShouldBeGreaterThan(0, "There should be at least 1 Identifier system/value pair on the Test report Filing");
                    obs.Identifier.ForEach(identifier =>
                    {
                        identifier.System.ShouldNotBeNullOrEmpty("Identifier System Is Null or Empty - Should be populated");
                        identifier.Value.ShouldNotBeNullOrEmpty("Fail : No Identifier found when Test report Filing should have a unique Identifier");
                    });

                    //Check Status
                    obs.Status.ToString().ToLower().ShouldBe("unknown", "Test report Filing Status is NOT set to unknown");

                    //Check Subject/patient
                    Patients.Where(p => p.Id == (obs.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");

                    //Check issued
                    obs.Issued.ShouldNotBeNull("Fail : issued should not be null - mandatory Field");

                    //check performer
                    obs.Performer.FirstOrDefault().Reference.ShouldNotBeNull("Fail : Performer should not be null - mandatory Field");

                    found = true;
                }
            });

            found.ShouldBeTrue("Fail : No Test Report Filing found as per data requirements");
        }

        [Then(@"I Check the Test report Filing Do Not Include Not in Use Fields")]
        public void ThenIChecktheTestreportFilingDoNotIncludeNotiNUseFields()
        {
            bool found = false;
            Observations.ForEach(obs =>
            {
                if (obs.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kObservationCommentNoteCode)
                {
                    obs.BasedOn.Count().ShouldBe(0, "Fail :  Observation Test report Filing - basedon element Should not be used - Not In Use Field");
                    obs.Category.Count().ShouldBe(0, "Fail :  Observation Test report Filing - category element Should not be used - Not In Use Field");
                    //obs.Context.ShouldBeNull("Fail :  Observation Test report Filing  - context element Should not be used - Not In Use Field");
                    obs.DataAbsentReason.ShouldBeNull("Fail :  Observation Test report Filing  - DataAbsentReason element Should not be used - Not In Use Field");
                    obs.Interpretation.ShouldBeNull("Fail :  Observation Test report Filing  - Interpretation element Should not be used - Not In Use Field");
                    obs.BodySite.ShouldBeNull("Fail :  Observation Test report Filing  - BodySite element Should not be used - Not In Use Field");
                    obs.Method.ShouldBeNull("Fail :  Observation Test report Filing  - Method element Should not be used - Not In Use Field");
                    obs.Specimen.ShouldBeNull("Fail :  Observation Test report Filing  - Specimen element Should not be used - Not In Use Field");
                    obs.ReferenceRange.Count().ShouldBe(0, "Fail :  Observation Test report Filing - ReferenceRange element Should not be used - Not In Use Field");
                    obs.Device.ShouldBeNull("Fail :  Observation Test report Filing  - Device element Should not be used - Not In Use Field");
                    obs.Component.Count().ShouldBe(0, "Fail :  Observation Test report Filing - Component element Should not be used - Not In Use Field");

                    found = true;
                }
            });

            found.ShouldBeTrue("Fail : No Test Report Filing found as per data requirements");
        }
    }
}