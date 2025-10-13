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
    using Newtonsoft.Json.Linq;
    using Hl7.Fhir.Serialization;
    using System.IO;

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
        private List<AllergyIntolerance> ActiveAllergyIntolerances => _httpContext.FhirResponse.AllergyIntolerances;
        private List<Observation> Observations => _httpContext.FhirResponse.Observations;
        private List<Immunization> Immunizations => _httpContext.FhirResponse.Immunizations;
        private List<Encounter> Encounters => _httpContext.FhirResponse.Encounters;

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

        [Then(@"I Check The Primary Problems List")]
        public void ThenICheckThePrimaryProblemsList()
        {
            //Check there is ONE Problems List with snomed code
            Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kProblems).ToList().Count().ShouldBe(1, "Failed to Find ONE Primary Problems list using Snomed Code.");

            //Get Var to List
            var problemsList = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kProblems).First();

            //Check title
            problemsList.Title.ShouldBe("Problems", "Primary Problems List Title is Incorrect");

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

            //check number of Problems matches number in list
            if (Problems.Count() != problemsList.Entry.Count())
            {
                Problems.Count().ShouldBe(problemsList.Entry.Count(), "Number of Problems does not match the number in the List");
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

        [Then(@"I Check The Problems Secondary Problems List")]
        public void ThenICheckTheSecondaryProblemsList()
        {
            //Check there is ONE Problems List with snomed code
            Lists.Where(l => l.Code.Coding.First().Code == FhirConst.SecondaryListCodeAndDisplayValues.kSecProblemsNotRelatedToPrimaryQueryCode).ToList().Count().ShouldBe(1, "Failed to Find ONE Problems Secondary Problems list using Code.");

            //Get Var to List
            var problemsList = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.SecondaryListCodeAndDisplayValues.kSecProblemsNotRelatedToPrimaryQueryCode).First();

            //Check title
            problemsList.Title.ShouldBe(FhirConst.ListTitles.kSecProblemsNotRelatedToPrimary, "Problems Secondary Problems List Title is Incorrect");

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
                coding.System.ShouldBeOneOf("http://snomed.info/sct", "http://read.info/readv2", "http://read.info/ctv3", "https://fhir.hl7.org.uk/Id/emis-drug-codes", "https://fhir.hl7.org.uk/Id/egton-codes", "https://fhir.hl7.org.uk/Id/multilex-drug-codes", "https://fhir.hl7.org.uk/Id/resipuk-gemscript-drug-codes", "https://fhir.hl7.org.uk/STU3/CodeSystem/GPConnect-SecondaryListValues-1");
                coding.Display.ShouldNotBeNullOrEmpty("Display Should not be Null or Empty");
            });

            //Check subject/patient ref
            Patients.Where(p => p.Id == (problemsList.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");

            //check number of Problems matches number in list
            if (Problems.Count() != problemsList.Entry.Count())
            {
                Problems.Count().ShouldBe(problemsList.Entry.Count(), "Number of Problems does not match the number in the List");
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

        [Then(@"I Check The Primary Problems List Does Not Include Not In Use Fields")]
        public void ThenICheckThePrimaryProblemsListDoesNotIncludeNotInUseFields()
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

        [Then(@"I Check The Problems Secondary Problems List Does Not Include Not In Use Fields")]
        public void ThenICheckTheSecondaryProblemsListDoesNotIncludeNotInUseFields()
        {
            //Check there is ONE Problems List with snomed code
            Lists.Where(l => l.Code.Coding.First().Code == FhirConst.SecondaryListCodeAndDisplayValues.kSecProblemsNotRelatedToPrimaryQueryCode).ToList().Count().ShouldBe(1, "Failed to Find ONE Secondary Problems list using Code.");

            //Get Var to List
            var problemsList = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.SecondaryListCodeAndDisplayValues.kSecProblemsNotRelatedToPrimaryQueryCode).First();

            //Check that - Not In Use Fields are not present
            problemsList.Id.ShouldBeNull("List Id is Not Supposed to be Sent - Not In Use Field");
            problemsList.Meta.VersionId.ShouldBeNull("List Meta.VersionId is Not Supposed to be Sent - Not In Use Field");
            problemsList.Meta.LastUpdated.ShouldBeNull("List Meta.LastUpdated is Not Supposed to be Sent - Not In Use Field");
            problemsList.Source.ShouldBeNull("List Source is Not Supposed to be Sent - Not In Use Field");
        }

        [Then(@"I Check There is No Primary Problems List")]
        public void ThenICheckThereisNoProblemsList()
        {
            //Check there is NO Problems List with snomed code
            Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kProblems).ToList().Count().ShouldBe(0, "Fail : NO Primary Problems List Should Exist for this Patient");
            Logger.Log.WriteLine("INFO : NO Primary Problem List has been included in the bundle");
        }

        [Then(@"I Check There is No Problems Secondary Problems List")]
        public void ThenICheckThereisNoSecondaryProblemsList()
        {
            //Check there is NO Problems List with snomed code
            Lists.Where(l => l.Code.Coding.First().Code == FhirConst.SecondaryListCodeAndDisplayValues.kSecConsultMedicationsCode).ToList().Count().ShouldBe(0, "Fail : NO Secondary Problems List Should Exist for this Patient");
            Logger.Log.WriteLine("INFO : NO Secondary Problem List has been included in the bundle");
        }

        [Then(@"I Check No Problem Resources are Included")]
        public void ThenICheckNoProblemResourcesareIncluded()
        {
            Problems.ToList().Count().ShouldBe(0, "Fail : NO Problems should be included in the response as per Data requirements");
            Logger.Log.WriteLine("INFO : NO Problems have been included in the bundle");
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
                problemExtensions.Count.ShouldBe(1, "Fail : problemSignificance not detected and is mandatory on Problem ID : " + problem.Id);
                Code clinicalSetting = (Code)problemExtensions.First().Value;
                clinicalSetting.Value.ShouldBeOneOf("major", "minor");

                //Check identifier
                problem.Identifier.Count.ShouldBeGreaterThan(0, "There should be at least 1 Identifier system/value pair on the Problem");
                problem.Identifier.ForEach(identifier =>
                {
                    identifier.System.ShouldNotBeNullOrEmpty("Identifier System Is Null or Empty - Should be populated");
                    identifier.Value.ShouldNotBeNullOrEmpty("Identifier Value Is Null or Empty - Should be populated");
                });

                //Check clinicalStatus
                problem.ClinicalStatus.ToString().ShouldBeOneOf("Active", "Inactive");

                //check category
                problem.Category.Where(c => c.Coding.First().System == FhirConst.ValueSetSystems.kVsCareConditionCategory).Count().ShouldBe(1);
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

        [Then(@"Check a Problem is Linked to a MedicationRequest resource that has been included in the response")]
        public void ThenCheckaProblemisLinkedtoaMedicationRequestresourcethathasbeenincludedintheresponse()
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
                    if (rr.Reference != null)
                    {
                        if (rr.Reference.StartsWith("MedicationRequest/"))
                        {
                            refToFind = rr.Reference;
                            found = true;
                            break;
                        }
                    }
                }
                if (found)
                    break;
            }
            ;

            found.ShouldBeTrue("Fail : No Problems found with a linked MedicationRequest");
        }

        [Then(@"Check the MedicationRequests have a link to a medication that has been included in response")]
        public void ThenChecktheMedicationRequestshavealinktoamedicationthathasbeenincludedinresponse()
        {
            bool found = false;
            //Loop all medRequests and check has a medication reference and also exists in response.
            MedicationRequests.ForEach(medr =>
            {
                string rr = ((ResourceReference)medr.Medication).Reference;
                VerifyResourceReferenceExists("Medication", rr);
                //string checkText = "Check if " + "Medication" + " resource with ID " + rr + " exists in the bundle";
                //Given(checkText);
                found = true;
            });

            found.ShouldBeTrue("Fail : No MedicationRequest found with a linked Medication");
            Logger.Log.WriteLine("Info : MedicationRequest found with a link to a Medication");
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
                //check link to medication exists
                string medRefToCheck = "";
                if (medS.Medication != null)
                {
                    medRefToCheck = ((ResourceReference)medS.Medication).Reference;
                    if (medRefToCheck.StartsWith("Medication/"))
                    {
                        //check Resource Exists
                        VerifyResourceReferenceExists("Medication", medRefToCheck);
                        medFound = true;
                    }
                }

                //Check Link to MedicationRequest
                string medrRefToCheck = "";
                if (medS.BasedOn != null)
                {
                    if (medS.BasedOn.Count() >= 1)
                    {
                        medrRefToCheck = medS.BasedOn.First().Reference;
                        if (medrRefToCheck.StartsWith("MedicationRequest/"))
                        {
                            //check Resource Exists
                            VerifyResourceReferenceExists("MedicationRequest", medrRefToCheck);
                            medRequestFound = true;
                        }
                    }
                }

                //Assert have found Medication reference and resource
                medFound.ShouldBeTrue("Fail : No link to a Medication found on MedicationStatement - ID : " + medS.Id);

                //Assert have found MedicationRequest reference and resource
                medRequestFound.ShouldBeTrue("Fail : No link to a MedicationRequest found on MedicationStatement - ID : " + medS.Id);

                medStatementFound = true;
            });

            medStatementFound.ShouldBeTrue("Fail : No MedicationStatements found");
            Logger.Log.WriteLine("Info : Found MedicationStatements");
        }

        [Then(@"Check the Medications List resource has been included in response")]
        public void ThenChecktheMedicationsListresourcehasbeenincludedinresponse()
        {
            //Check List is Present
            Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kMeds).ToList().Count().ShouldBe(1, "Fail : 0 or more than one Medications list Detected - Expected 1");
            var medsList = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kMeds).First();

            bool foundMedStatement = false;

            medsList.Entry.ForEach(e =>
            {
                //Check references is to a MedicationStatement
                e.Item.Reference.ShouldStartWith("MedicationStatement/");

                //Checkresource has been inluded in response
                VerifyResourceReferenceExists("MedicationStatement", e.Item.Reference);
                foundMedStatement = true;
            });

            foundMedStatement.ShouldBeTrue("Fail : No MedicationStatements Linked on Medications List");
            Logger.Log.WriteLine("Info : Found MedicationStatements Linked on Medications List");
        }

        [Then(@"Check a Problem is linked to an ""(.*)"" that is also included in the response with its list")]
        public void ThenCheckaProblemislinkedtoanthatisalsoincludedintheresponsewithalist(string resourceType)
        {
            //loop problems and check one has been linked to the Clinical item type
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
                        if (rr.Reference.StartsWith((resourceType + "/")))
                        {
                            refToFind = rr.Reference;
                            found = true;
                            Logger.Log.WriteLine("Info : Problem - Found Linked Clinical Item of type : " + resourceType + " - with ID : " + refToFind);
                            break;
                        }
                    }
                }
                if (found)
                    break;
            }
            ;

            found.ShouldBeTrue("Fail : No Problems found to be linked to a  " + resourceType + " - resource type");

            //check that Linked Clinical resource has been included in response.
            VerifyResourceReferenceExists(resourceType, refToFind);
        }

        [Then(@"Check that a Problem is linked via context to a consultation but only a reference is sent in response")]
        public void ThenCheckthataProblemislinkedtoaconsultationbutonlyareferenceissentinresponse()
        {
            var found = false;

            foreach (var p in Problems)
            {
                if (p.Context != null)
                {
                    if (p.Context.Reference.StartsWith("Encounter/"))
                    {
                        Logger.Log.WriteLine("Info : Problem - Found with Context Link To an Encounter with ID : " + p.Context.Reference);

                        //check any Encounter References are not inluded in bundle
                        string pattern = @"(.*/)(.*)";
                        string refToFind = Regex.Replace(p.Context.Reference, pattern, "$2");

                        //check exists in bundle
                        Encounters.Where(e => e.Id == refToFind).Count().ShouldBe(1, "Fail : Context Linked Encounter NOT found in bundle when it should BE - missing id: " + p.Context.Reference);
                        Logger.Log.WriteLine("Info : Encounter Reference has been included in bundle with ID : " + p.Context.Reference);
                        found = true;
                    }
                }
            }
            ;

            found.ShouldBeTrue("Fail : No Problems found with a Context link to an Encounter as per the data requirements for this test");

            //check no Consultations lists in bundle

            //Check Consultation lists do not exxist in response
            Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kConsultation)
                .ToList().Count()
                .ShouldBe(0, "Fail :Consultation List Found - Expect No Consultation List is sent");
            Logger.Log.WriteLine("Info : No Consultation List Found as Expected");

            //Check Consultations List is not Included
            Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kConsultations)
                .ToList().Count()
                .ShouldBe(0, "Fail : Found atleast 1 Consultations list using Snomed Code. - Expect NONE");
            Logger.Log.WriteLine("Info : No Consultations List Found as Expected");

            //Check No Heading List is sent
            Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kHeadings)
                .ToList().Count()
                .ShouldBe(0, "Fail : Found atleast 1 Heading list using Snomed Code. - Expect NONE");
            Logger.Log.WriteLine("Info : No Heading List Found as Expected");

            //Check No Topic List is sent
            Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kTopics)
                .ToList().Count()
                .ShouldBe(0, "Fail : Found atleast 1 Topic list using Snomed Code. - Expect NONE");
            Logger.Log.WriteLine("Info : No Topic List Found as Expected");
        }

        public void CheckResourceExists<T>(T resourceType, string resourceID)
        {
            var count = Bundle.GetResources()
                           .Where(resource => resource.ResourceType.Equals(resourceType))
                           .Where(resource => resource.Id == resourceID)
                           .ToList().Count();

            if (count == 1) //only one found
            {
                Logger.Log.WriteLine("Info : Found Linked Resource : " + resourceType.ToString() + " - Found ID: " + resourceID);
            }
            else if (count > 1) //more than one
            {
                count.ShouldBe(1, "Fail : Duplicate Resource Found : " + resourceType.ToString() + " - Duplicate ID : " + resourceID);
            }
            else //none found
            {
                count.ShouldBe(1, "Fail : Resource NOT Found : " + resourceType.ToString() + " - Missing ID : " + resourceID);
            }
        }

        public void VerifyResourceReferenceExists(string refTypeToFind, string fullRefToFind)
        {
            string pattern = @"(.*/)(.*)";
            string refToFind = Regex.Replace(fullRefToFind, pattern, "$2");

            //Switch on Clinical Item type
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

                case "Encounter":
                    CheckResourceExists(ResourceType.Encounter, refToFind);
                    break;

                //unknown type ignore - could be not supported message
                default:
                    Logger.Log.WriteLine("Ignored, Entry/Item/Reference for : " + refTypeToFind);
                    break;
            }
        }

        [Then(@"I Check that a problem is linked to another problem")]
        public void ThenICheckthataproblemislinkedtoanotherproblem()
        {
            //check atleast two problems
            Problems.ToList().Count().ShouldBeGreaterThan(1, "Error Should be Atleast Two Problems Related in response as per Data requirements");

            var foundParenttoChildRelationShip = false;
            string pattern = @"(.*)(/)(.*)";

            //Loop each Problem
            Problems.ForEach(problem =>
            {
                //reset vars each loop
                var parentProbRef = "";
                var childProbRef = "";
                var childRelatedRefValue = "";
                var parentRelatedRefValue = "";

                //check if problem has related problem extension
                List<Extension> ChildRelatedProblemExtensions = problem.Extension.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kExtProblem)).ToList();

                //loop and find a child or sibling with a parent
                ChildRelatedProblemExtensions.ForEach(childExtension =>
                {
                    //Check if found a child or sibling
                    var typeChildExt = childExtension.Extension.Where(e => e.Url == "type"
                        && ((((Hl7.Fhir.Model.Code)e.Value).Value.ToLower() == "child") || (((Hl7.Fhir.Model.Code)e.Value).Value.ToLower() == "sibling")));

                    if (typeChildExt.Count() > 0)
                    {
                        parentProbRef = problem.Id;

                        //Get Target and Reference contained on child or sibling
                        var childTargetRefExt = (childExtension.Extension.Where(e => e.Url == "target")).FirstOrDefault().Value;
                        var childRefValue = ((Hl7.Fhir.Model.ResourceReference)childTargetRefExt).Reference;
                        childRelatedRefValue = Regex.Replace(childRefValue, pattern, "$3");

                        //Now find matching Parent
                        Problems.ForEach(prob =>
                        {
                            childProbRef = prob.Id;

                            //check if problem has related problem extension
                            List<Extension> parentRelatedProblemExtensions = prob.Extension.Where(ext => ext.Url.Equals(FhirConst.StructureDefinitionSystems.kExtProblem)).ToList();

                            //loop and find a child or sibling with a parent
                            parentRelatedProblemExtensions.ForEach(parentExtension =>
                            {
                                var typeParentExt = parentExtension.Extension.Where(e => e.Url == "type" && (((Hl7.Fhir.Model.Code)e.Value).Value.ToLower() == "parent"));
                                if (typeParentExt.Count() > 0)
                                {
                                    //Get Target and Reference contained on child or sibling
                                    var parentTargetRefExt = (parentExtension.Extension.Where(e => e.Url == "target")).FirstOrDefault().Value;
                                    var parentRefValue = ((Hl7.Fhir.Model.ResourceReference)parentTargetRefExt).Reference;
                                    parentRelatedRefValue = Regex.Replace(parentRefValue, pattern, "$3");

                                    //Check relationships are correct
                                    if (parentProbRef == parentRelatedRefValue && childProbRef == childRelatedRefValue)
                                    {
                                        foundParenttoChildRelationShip = true;
                                        Logger.Log.WriteLine("INFO : Found Problem linked to another problem. \nParent :" + parentProbRef + "\nChild :" + childProbRef);
                                    }
                                }
                            });
                        });
                    }
                });
            });

            foundParenttoChildRelationShip.ShouldBeTrue("Fail : Problems linked to Problems Test has Not Found a Parent-Child relationship or Parent-Sibling Relationship as per data requirements");
        }

        [Then(@"I Check the Problems Medications Secondary List is Valid")]
        public void ThenIChecktheProblemsMedicationsSecondaryListisValid()
        {
            //Check List Exists and has correct title/code/display
            Lists.Where(l => l.Title == FhirConst.ListTitles.kSecProblemsMedications).Count().ShouldBe(1, "Fail - No Secondary List Found with Title : " + FhirConst.ListTitles.kSecProblemsMedications);
            var list1 = Lists.Where(l => l.Title == FhirConst.ListTitles.kSecProblemsMedications).FirstOrDefault();
            list1.Title.ShouldBe(FhirConst.ListTitles.kSecProblemsMedications, "Fail - No Secondary List Found with Title: " + FhirConst.ListTitles.kSecProblemsMedications);
            list1.Code.Coding.First().Code.ShouldBe(FhirConst.SecondaryListCodeAndDisplayValues.kSecProblemsMedicationsCode, "Fail : Secondary List : " + FhirConst.ListTitles.kSecProblemsMedications + " -- Failed Code Check");
            list1.Code.Coding.First().Display.ShouldBe(FhirConst.SecondaryListCodeAndDisplayValues.kSecProblemsMedicationsDisplay, "Fail : Secondary List : " + FhirConst.ListTitles.kSecProblemsMedications + " -- Failed Display Check");

            //common function to check Mandatory List elements
            checkCommonMandatoryStructuredList(list1);

            //Check Entries on List
            MedicationStatements.Count().ShouldBeGreaterThan(0, "Fail : No Medication Statements Found in Response - Test expects Links to Medications");
            list1.Entry.Count().ShouldBeGreaterThan(0, "Fail : Secondary list Should Have Medication Statements Linked - None Found");
            //if (MedicationStatements.Count() != list1.Entry.Count())
            //{
            //    MedicationStatements.Count().ShouldBe(list1.Entry.Count(), "Number of Medication Statements does not match the number on the List");
            //}
            //else
            //{
            list1.Entry.ForEach(entry =>
            {
                string guidToFind = entry.Item.Reference.Replace("MedicationStatement/", "");
                MedicationStatements.Where(i => i.Id == guidToFind).Count().ShouldBe(1, "Not Found Reference to MedicationStatement On List : " + FhirConst.ListTitles.kSecProblemsMedications);
                Logger.Log.WriteLine("Info : Found MedicationStatement linked on List and Verified It was Contained in Bundle -ID : " + guidToFind);
            });
            //}
            Logger.Log.WriteLine("Info : Validated Secondary list with Title : " + FhirConst.ListTitles.kSecProblemsMedications);
        }

        [Then(@"I Check the Problems Uncategorised Secondary List is Valid")]
        public void ThenIChecktheProblemsUncategorisedSecondaryListisValid()
        {
            //Check List Exists and has correct title/code/display
            Lists.Where(l => l.Title == FhirConst.ListTitles.kSecProblemsUncat).Count().ShouldBe(1, "Fail - No Secondary List Found with Title : " + FhirConst.ListTitles.kSecProblemsUncat);
            var list2 = Lists.Where(l => l.Title == FhirConst.ListTitles.kSecProblemsUncat).FirstOrDefault();
            list2.Title.ShouldBe(FhirConst.ListTitles.kSecProblemsUncat, "Fail - No Secondary List Found with Title: " + FhirConst.ListTitles.kSecProblemsUncat);
            list2.Code.Coding.First().Code.ShouldBe(FhirConst.SecondaryListCodeAndDisplayValues.kSecProblemsUncatCode, "Fail : Secondary List : " + FhirConst.ListTitles.kSecProblemsUncat + " -- Failed Code Check");
            list2.Code.Coding.First().Display.ShouldBe(FhirConst.SecondaryListCodeAndDisplayValues.kSecProblemsUncatDisplay, "Fail : Secondary List : " + FhirConst.ListTitles.kSecProblemsUncat + " -- Failed Display Check");
            //common function to check Mandatory List elements
            checkCommonMandatoryStructuredList(list2);

            //Check Entries on List
            Observations.Count().ShouldBeGreaterThan(0, "Fail : No Observations Found in Response - Test expects Links to Observations");
            list2.Entry.Count().ShouldBeGreaterThan(0, "Fail : Secondary list Should Have Observations Linked - None Found");

            //can have observations that are not uncategorised, so cannot do a check of number of observations in bundle vs list items
            list2.Entry.ForEach(entry =>
            {
                string guidToFind = entry.Item.Reference.Replace("Observation/", "");
                Observations.Where(i => i.Id == guidToFind).Count().ShouldBe(1, "Not Found Reference to Observation : ID : " + guidToFind);
                Logger.Log.WriteLine("Info : Found Observation linked on List and Verified It was Contained in Bundle -ID : " + guidToFind);
            });

            Logger.Log.WriteLine("Info : Validated Secondary list with Title : " + FhirConst.ListTitles.kSecProblemsUncat);
        }

        public void checkCommonMandatoryStructuredList(List listToCheck)
        {
            //Check Meta.profile
            CheckForValidMetaDataInResource(listToCheck, FhirConst.StructureDefinitionSystems.kList);

            //Check Status
            listToCheck.Status.ShouldBeOfType<List.ListStatus>("Status List is of wrong type.");
            listToCheck.Status.ToString().ToLower().ShouldBe("current", "List Status is NOT set to completed");

            //Check Mode
            listToCheck.Mode.ShouldBeOfType<ListMode>("Mode List is of wrong type.");
            listToCheck.Mode.ToString().ToLower().ShouldBe("snapshot", "List Status is NOT set to completed");

            //Check Patient
            Patients.Where(p => p.Id == (listToCheck.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");
        }
    }
}