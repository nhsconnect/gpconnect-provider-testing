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
                problemExtensions.Count.ShouldBeLessThanOrEqualTo(1);
                if (problemExtensions.Count == 1)
                {
                    CodeableConcept clinicalSetting = (CodeableConcept)problemExtensions.First().Value;
                    clinicalSetting.Coding.First().Code.ShouldBe("major");
                }

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

                //check code
                //TODO - Query ouutstanding with Pete s - requirements not clear.

                //Check assertedDate
                problem.AssertedDate.ShouldNotBeNull();

                //Check asserter
                problem.Asserter.Reference.ShouldStartWith("Practitioner/");

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
    }
}