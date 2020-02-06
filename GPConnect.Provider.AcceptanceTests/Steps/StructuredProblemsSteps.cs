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
    public sealed class StructuredProblemsSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private List<List> Lists => _httpContext.FhirResponse.Lists;
        private List<Patient> Patients => _httpContext.FhirResponse.Patients;
        private List<Condition> Problems => _httpContext.FhirResponse.Conditions;
        private Bundle Bundle => _httpContext.FhirResponse.Bundle;
        private List<Medication> Medications => _httpContext.FhirResponse.Medications;
        private List<MedicationStatement> MedicationStatements => _httpContext.FhirResponse.MedicationStatements;
        private List<MedicationRequest> MedicationRequests => _httpContext.FhirResponse.MedicationRequests;

        public StructuredProblemsSteps(HttpSteps httpSteps, HttpContext httpContext)
            : base(httpSteps)
        {
            _httpContext = httpContext;
        }


        [Given(@"I add the Problems parameter")]
        public void GivenIAddTheProblemsParameter()
        {
            ParameterComponent param = new ParameterComponent();
            param.Name = FhirConst.GetStructuredRecordParams.kProblems;
            _httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(param);
        }

        [Then(@"I Check The Problems List")]
        public void ThenICheckTheProblemsList()
        {
            //Check there is ONE Problems List with snomed code
            Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kProblems).ToList().Count().ShouldBe(1, "Failed to Find ONE Problems list using Snomed Code.");

            //Get Var to List
            var problemsList = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kProblems).First();

            //Check title
            problemsList.Title.ShouldBe("Problems", "Problems List Title is Incorrect");

            //Check Meta.profile
            CheckForValidMetaDataInResource(problemsList, FhirConst.StructureDefinitionSystems.kList);

            //Check Status
            problemsList.Status.ShouldBeOfType<List.ListStatus>("Status List is of wrong type.");
            problemsList.Status.ToString().ToLower().ShouldBe("current", "List Status is NOT set to completed");

            //Check Mode
            problemsList.Mode.ShouldBeOfType<ListMode>("Mode List is of wrong type.");
            problemsList.Mode.ToString().ToLower().ShouldBe("snapshot", "List Status is NOT set to completed");

            //Check Code
            problemsList.Code.Coding.ForEach(coding =>
            {
                coding.System.ShouldBeOneOf("http://snomed.info/sct", "http://read.info/readv2", "http://read.info/ctv3", "https://fhir.hl7.org.uk/Id/emis-drug-codes", "https://fhir.hl7.org.uk/Id/egton-codes", "https://fhir.hl7.org.uk/Id/multilex-drug-codes", "https://fhir.hl7.org.uk/Id/resipuk-gemscript-drug-codes");
                coding.Display.ShouldNotBeNullOrEmpty("Display Should not be Null or Empty");
            });

            //Check subject/patient ref
            Patients.Where(p => p.Id == (problemsList.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");

            //check number of Conditions matches number in list
            if (Problems.Count() != problemsList.Entry.Count())
            {
                Problems.Count().ShouldBe(problemsList.Entry.Count(), "Number of Conditions does not match the number in the List");
            }
            else
            {
                //Check each references condition is present in bundle
                problemsList.Entry.ForEach(entry =>
                {
                    string guidToFind = entry.Item.Reference.Replace("Condition/", "");
                    Problems
                        .Where(resource => resource.ResourceType.Equals(ResourceType.Condition))
                        .Where(c => c.Id == guidToFind)
                        .Count().ShouldBe(1, "Not Found Reference to Condition");
                });
            }
        }

        [Then(@"I Check The Problems List Does Not Include Not In Use Fields")]
        public void ThenICheckTheProblemsListDoesNotIncludeNotInUseFields()
        {
            //Check there is ONE Problems List with snomed code
            Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kProblems).ToList().Count().ShouldBe(1, "Failed to Find ONE Problems list using Snomed Code.");

            //Get Var to List
            var problemsList = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kProblems).First();

            //Check that - Not In Use Fields are not present
            problemsList.Id.ShouldBeNull("List Id is Not Supposed to be Sent - Not In Use Field");
            problemsList.Meta.VersionId.ShouldBeNull("List Meta.VersionId is Not Supposed to be Sent - Not In Use Field");
            problemsList.Meta.LastUpdated.ShouldBeNull("List Meta.LastUpdated is Not Supposed to be Sent - Not In Use Field");
            problemsList.Source.ShouldBeNull("List Source is Not Supposed to be Sent - Not In Use Field");

        }

        [Then(@"I Check The Problems Resources are Valid")]
        public void ThenIChecktheProblemsareValid()
        {
            //check atleast one
            Problems.ToList().Count().ShouldBeGreaterThan(0, "Error Should be Atleast One Problem in response as per Data requirements");

            //Loop each Problem
            Problems.ForEach(problem =>
            {
                //Check ID
                problem.Id.ShouldNotBeNullOrEmpty();

                //Check Meta.profile
                CheckForValidMetaDataInResource(problem, FhirConst.StructureDefinitionSystems.kProblems);

                //Check extension[problemSignificance]
                List<Extension> problemExtensions = problem.Extension.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kExtProblemSignificance)).ToList();
                problemExtensions.Count.ShouldBe(1);
                Code clinicalSetting = (Code)problemExtensions.First().Value;
                clinicalSetting.Value.ShouldBeOneOf("major", "minor");
                
                //Check identifier
                problem.Identifier.Count.ShouldBeGreaterThan(0, "There should be at least 1 Identifier system/value pair");
                problem.Identifier.ForEach(identifier =>
                {
                    identifier.System.Equals(FhirConst.ValueSetSystems.kCrossCareIdentifier).ShouldBeTrue("Cross Care Setting Identfier NOT Found");

                    //identifier.Value format is still being debated, hence notnull check
                    identifier.Value.ShouldNotBeNullOrEmpty("Identifier Value Is Null or Not Valid");
                    //Guid guidResult;
                    //Guid.TryParse(identifier.Value, out guidResult).ShouldBeTrue("Immunization identifier GUID is not valid or Null");
                });

                //Check clinicalStatus
                problem.ClinicalStatus.ToString().ShouldBeOneOf("Active", "Inactive");

                //check category
                problem.Category.Where(c => c.Coding.First().Code == "problem-list-item").Count().ShouldBe(1);
                
                //Check assertedDate
                problem.AssertedDate.ShouldNotBeNull();

                //Check asserter               
                if (!(problem.Asserter.Reference.Contains("Practitioner/") || problem.Asserter.Display.Contains("Unknown")))
                    NUnit.Framework.Assert.Fail("Problem Asserter.Reference should either be a Practitioner Reference or Asserter Display should be Unknown");
                
                //CheckSubejct/patient
                Patients.Where(p => p.Id == (problem.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");


            });

        }

        [Then(@"I check The Problem Resources Do Not Include Not In Use Fields")]
        public void GivenTheProblemsResourcesDoNotIncludeNotInUseFields()
        {
            Problems.ForEach(problem =>
            {
                problem.Severity.ShouldBeNull("Problem Check Failed : Severity is a Not In Use Field");
                problem.VerificationStatus.ShouldBeNull("Problem Check Failed : VerificationStatus is a Not In Use Field");
                problem.BodySite.Count().ShouldBe(0, "Problem Check Failed : bodySite is a Not In Use Field");
                problem.Stage.ShouldBeNull("Problem Check Failed : Stage is a Not In Use Field");
                problem.Evidence.Count().ShouldBe(0, "Problem Check Failed : Evidence is a Not In Use Field");
            });
        }

        [Given(@"I add the problems parameter with filterStatus ""(.*)""")]
        public void GivenIAddTheProblemsParameterWithfilterStatus(string value)
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kProblemsStatus, (Base)new Code (value))
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kProblems, tuples);
        }

        [Given(@"I add the problems parameter with filterSignificance ""(.*)""")]
        public void GivenIAddTheProblemsParameterWithFilterSignificance(string value)
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kProblemsSignificance, (Base)new Code (value))
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kProblems, tuples);
        }

        [Given(@"I add the problems parameter including status and significance value ""([^ ""]*)"" ""([^ ""]*)""")]
        public void GivenIAddTheProblemsParameterIncludingStatusAndSignificanceValue(string statusValue, string sigValue)
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kProblemsStatus, (Base)new Code (statusValue)),
                Tuple.Create(FhirConst.GetStructuredRecordParams.kProblemsSignificance, (Base)new Code (sigValue ))
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kProblems, tuples);
        }

        [Given(@"I add the problems parameter including repeating filter pairs")]
        public void GivenIAddTheProblemsParameterIncludingRepeatingFilterPairs()
        {

            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kProblemsStatus, (Base)new Code ("active")),
                Tuple.Create(FhirConst.GetStructuredRecordParams.kProblemsSignificance, (Base)new Code ("minor")),
                            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kProblems, tuples);

            IEnumerable<Tuple<string, Base>> tuples2 = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kProblemsStatus, (Base)new Code("inactive")),
                Tuple.Create(FhirConst.GetStructuredRecordParams.kProblemsSignificance, (Base)new Code("major")),

            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kProblems, tuples2);
        }

        [Given(@"I add a madeUpProblems part parameter")]
        public void GivenIAddAMadeUpProblemsPartParameter()
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create("madeUpProblems", (Base)new Code ("madeUpProblemsValue1")),
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kProblems, tuples);

        }

        public void CheckResourceExists<T>(T resourceType, string resourceID)
        {
            Bundle.GetResources()
                           .Where(resource => resource.ResourceType.Equals(resourceType))
                           .Where(resource => resource.Id == resourceID)
                           .ToList().Count().ShouldBe(1, "Linked Resource : " + resourceType.ToString() + " - Not Found ID : " + resourceID);

            Logger.Log.WriteLine("Found Linked resource : " + resourceID + " Of Type : " + resourceType);

        }

        public void VerifyResourceReferenceExists(string refTypeToFind, string fullRefToFind)
        {
            string pattern = @"(.*/)(.*)";
            string refToFind = Regex.Replace(fullRefToFind, pattern, "$2");

            //Switch on Clincal Item type
            switch (refTypeToFind)
            {
                case "Observation":
                    CheckResourceExists(ResourceType.Observation, refToFind);
                    break;

                case "AllergyIntolerance":
                    CheckResourceExists(ResourceType.AllergyIntolerance, refToFind);
                    break;

                case "Medication":
                    CheckResourceExists(ResourceType.Medication, refToFind);
                    break;

                case "MedicationStatement":
                    CheckResourceExists(ResourceType.MedicationStatement, refToFind);
                    break;

                case "MedicationRequest":
                    CheckResourceExists(ResourceType.MedicationRequest, refToFind);
                    break;

                case "Immunization":
                    CheckResourceExists(ResourceType.Immunization, refToFind);
                    break;

                case "Condition":
                    CheckResourceExists(ResourceType.Condition, refToFind);
                    break;

                case "Appointment":
                    CheckResourceExists(ResourceType.Appointment, refToFind);
                    break;

                //unknown type ignore - could be not supported message
                default:
                    Logger.Log.WriteLine("Ignored, Entry/Item/Reference for : " + refTypeToFind);
                    break;
            }
        }

        [Then(@"Check there is a Linked MedicationRequest resource that has been included in the response")]
        public void ThenCheckLinkedMedicationRequestClinicalresourcesareincludedinresponse()
        {
            //check there is atleast one problem with a MedicationRequest linked            
            var found = false;
            string refToFind = "";
      
            foreach (var p in Problems)
            { 
                Condition problem = (Condition)p;
                List<Extension> problemRelatedContentExtensions = p.Extension.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kExtProblemRelatedContent)).ToList();

                foreach (var rcc in problemRelatedContentExtensions)
                {
                    ResourceReference rr = (ResourceReference)rcc.Value;
                    if (rr.Reference.StartsWith("MedicationRequest/"))
                    {
                        refToFind = rr.Reference;
                        found = true;
                        break;
                    }
                }
                if (found)
                    break;
            };

            found.ShouldBeTrue("Fail : No Problems found with a linked MedicationRequest");

            //check that MedicationRequest linked has been included in response.
            VerifyResourceReferenceExists("MedicationRequest", refToFind);

        }

        [Then(@"Check there is a Linked Medication resource that has been included in the response")]
        public void ThenCheckthereisaLinkedMedicationresourcethathasbeenincludedintheresponse()
        {
            bool found = false;
            //Loop all medRequests and check has a medication reference and also exists in response.
            MedicationRequests.ForEach(medr =>
            {
                string rr = ((ResourceReference)medr.Medication).Reference;
                VerifyResourceReferenceExists("Medication", rr);
                found = true;
            });

            found.ShouldBeTrue("Fail : No MedicationRequest found with a linked Medication");

        }



        [Then(@"Check there is a MedicationStatement resource that is linked to the MedicationRequest and Medication")]
        public void ThenCheckthereisaMedicationStatementresourcethatislinkedtotheMedicationRequestandMedication()
        {
            bool medStatementFound = false;
            bool medFound = false;
            bool medRequestFound = false;

            //loop all MedicationStatements
            MedicationStatements.ForEach(medS =>
            {
               
                //resultString += "\tMedication Reference : " + ((Hl7.Fhir.Model.ResourceReference)ms.Medication).Reference + checkClinicalResourceExists(((Hl7.Fhir.Model.ResourceReference)ms.Medication).Reference) + "\n";
                //resultString += "\tBasedOn : " + ms.BasedOn.First().Reference + checkClinicalResourceExists(ms.BasedOn.First().Reference);

                //check link to medication exists
                string medRefToCheck = ((ResourceReference)medS.Medication).Reference;
                if (medRefToCheck.StartsWith("Medication/"))
                {
                    //check Resource Exists
                    VerifyResourceReferenceExists("Medication", medRefToCheck);
                    medFound = true;
                }

                //Check Link to MedicationRequest
                string medrRefToCheck = medS.BasedOn.First().Reference;
                if (medrRefToCheck.StartsWith("MedicationRequest/"))
                {
                    //check Resource Exists
                    VerifyResourceReferenceExists("MedicationRequest", medrRefToCheck);
                    medRequestFound = true;
                }


                //Assert have found Medication reference and resource
                medFound.ShouldBeTrue("Fail : No link to a Medication found on MedicationStatement - ID : " + medRefToCheck);
                
                //Assert have found MedicationRequest reference and resource
                medRequestFound.ShouldBeTrue("Fail : No link to a MedicationRequest found on MedicationStatement - ID : " + medrRefToCheck);

                medStatementFound = true;
                
            });

            medStatementFound.ShouldBeTrue("Fail : No MedicationStatements found");

        }


    }
}