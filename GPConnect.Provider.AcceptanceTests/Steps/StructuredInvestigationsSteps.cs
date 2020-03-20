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
            //check atleast one
            DiagnosticReports.ToList().Count().ShouldBeGreaterThan(0, "Error Should be Atleast One DiagnosticReport in response as per Data requirements");

            bool foundResult = false;

            DiagnosticReports.ForEach(diagnostic =>
            {
                //Check Id
                diagnostic.Id.ShouldNotBeNullOrEmpty();

                //Check Meta.profile
                CheckForValidMetaDataInResource(diagnostic, FhirConst.StructureDefinitionSystems.kDiagnosticReport);

                //Check Identfier
                diagnostic.Identifier.Count.ShouldBeGreaterThan(0, "There should be at least 1 Identifier system/value pair");
                diagnostic.Identifier.ForEach(identifier =>
                {
                    identifier.Value.ShouldNotBeNullOrEmpty("Fail : No Identifier found when resource should have a unique Identifier");
                });

                //check atleast one Procedure Reference found and item included in bundle
                string refToCheck = "";
                bool found = false;
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
                            found = true;
                        }
                    }
                }

                found.ShouldBeTrue("Fail : No link to a Procedure found on DiagnosticReport - ID : " + diagnostic.Id);

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
                found = false;
               
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
                            found = true;
                        }
                    }
                }

                found.ShouldBeTrue("Fail : No link to a Specimen found on DiagnosticReport - ID : " + diagnostic.Id);

                //check one diagnosticreport is linked to a report as per data requirements
                if (diagnostic.Result != null)
                {
                    if (diagnostic.Result.Count() >= 1)
                    {
                        diagnostic.Result.ForEach(d =>
                        {
                            refToCheck = d.Reference;
                            if (refToCheck.StartsWith("Observation/"))
                            {
                                string refToFind = Regex.Replace(refToCheck, pattern, "$3");
                                //check Resource Exists
                                VerifyResourceReferenceExists("Observation", refToFind);
                                Logger.Log.WriteLine("Info : Found and Verified Observation - Test Result Linked to DiagnosticReport");
                                foundResult = true;
                            }
                        });                       
                    }
                }

            });


            foundResult.ShouldBeTrue("fail : Atleast one DiagnosticReport should be linked to a test result");



        }

        [Then(@"I Check the DiagnosticReports Do Not Include Not in Use Fields")]
        public void ThenIChecktheDiagnosticReportsDoNotIncludeNotinUseFields()
        {
            DiagnosticReports.ForEach(diag =>
            {
                diag.Effective.ShouldBeNull("Fail :  DiagnosticReport - effective element Should not be used - Not In Use Field");
                diag.Context.ShouldBeNull("Fail :  DiagnosticReport - context element Should not be used - Not In Use Field");
                diag.ImagingStudy.Count().ShouldBe(0,"Fail :  DiagnosticReport - imagingStudy element Should not be used - Not In Use Field");
                diag.Image.Count().ShouldBe(0, "Fail :  DiagnosticReport - image element Should not be used - Not In Use Field");
                diag.PresentedForm.Count().ShouldBe(0, "Fail :  DiagnosticReport - PresentedForm element Should not be used - Not In Use Field");
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
                specimen.Collection.Method.ShouldBeNull("Fail :  Specimen - Collection element Should not be used - Not In Use Field");
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


    }
}