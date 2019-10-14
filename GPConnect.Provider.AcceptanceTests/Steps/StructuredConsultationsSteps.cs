﻿namespace GPConnect.Provider.AcceptanceTests.Steps
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
    public sealed class StructuredConsultationsSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private List<Encounter> Encounters => _httpContext.FhirResponse.Encounters;
        private List<Condition> Conditions => _httpContext.FhirResponse.Conditions;
        private List<List> Lists => _httpContext.FhirResponse.Lists;
        private List<Patient> Patients => _httpContext.FhirResponse.Patients;
        private List<Condition> Problems => _httpContext.FhirResponse.Conditions;
        private Bundle Bundle => _httpContext.FhirResponse.Bundle;

        public StructuredConsultationsSteps(HttpSteps httpSteps, HttpContext httpContext)
            : base(httpSteps)
        {
            _httpContext = httpContext;
        }

        [Given(@"I add the Consultations parameter")]
        public void GivenIAddTheConsultationsParameter()
        {
            ParameterComponent param = new ParameterComponent();
            param.Name = FhirConst.GetStructuredRecordParams.kConsultations;
            _httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(param);
        }

        [Then(@"I Check the Consultations List is Valid")]
        public void ThenIChecktheConsultationsListisValid()
        {
            //Check for List using Snomed Code
            Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kConsultations).ToList().Count().ShouldBe(1, "Failed to Find ONE Consultations list using Snomed Code." );
            var consultationsList = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kConsultations).First();

            //Check Code display has correct display value
            consultationsList.Code.Coding.First().Display.ShouldBe("List of Consultations");
            consultationsList.Code.Coding.ForEach(coding =>
            {
                coding.System.ShouldBeOneOf("http://snomed.info/sct", "http://read.info/readv2", "http://read.info/ctv3", "https://fhir.hl7.org.uk/Id/emis-drug-codes", "https://fhir.hl7.org.uk/Id/egton-codes", "https://fhir.hl7.org.uk/Id/multilex-drug-codes", "https://fhir.hl7.org.uk/Id/resipuk-gemscript-drug-codes");
            });

            //Check List Title
            consultationsList.Title.ShouldBe(FhirConst.ListTitles.kConsultations, "Consultations List Title is Incorrect");

            //Check Meta Profile
            CheckForValidMetaDataInResource(consultationsList, FhirConst.StructureDefinitionSystems.kList);

            //Check Status
            consultationsList.Status.ShouldBeOfType<List.ListStatus>("Status List is of wrong type.");
            consultationsList.Status.ToString().ToLower().ShouldBe("current", "List Status is NOT set to completed");

            //Check Mode
            consultationsList.Mode.ShouldBeOfType<ListMode>("Mode List is of wrong type.");
            consultationsList.Mode.ToString().ToLower().ShouldBe("snapshot", "List Status is NOT set to completed");

            //CheckSubhect- Patient ref
            Patients.Where(p => p.Id == (consultationsList.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");

            //check Count of Encounters referenced vs Encounters in Bundle
            consultationsList.Entry.Count().ShouldBe(Encounters.Count());

            //check each encounter is included in the bundle
            string pattern = @"(.*/)(.*)";
            consultationsList.Entry.ForEach(entry =>
            {
                string refToFind = Regex.Replace(entry.Item.Reference, pattern, "$2");
                Bundle.GetResources()
                            .Where(resource => resource.ResourceType.Equals(ResourceType.Encounter))
                            .Where(resource => resource.Id == refToFind)
                            .ToList().Count().ShouldBe(1,"Encounter resource type Not Found");
            });

            //Check each encounter reference is included on a consultation
            var ConsultationLists = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kConsultation ).ToList();
            ConsultationLists.ForEach(list =>
            {                 
                consultationsList.Entry.Where(entry => entry.Item.Reference == list.Encounter.Reference).Count().ShouldBe(1, "Encounter Reference on Consultation not found in Consultations List :" + list.Encounter.Reference);
            });

            Logger.Log.WriteLine("Completed Mandatory Checks on Consultations List");


        }

        [Then(@"The Consultations List Does Not Include Not In Use Fields")]
        public void GivenTheConsultationsListDoesNotIncludeMustNotFields()
        {
            Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kConsultations).ToList().Count().ShouldBe(1, "Failed to Find ONE Consultations list using Snomed Code.");
            var consultationsList = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kConsultations).First();

            //consultationsList.Id.ShouldBeNull("List Id is Not Supposed to be Sent - Not In Use Field");
            consultationsList.Meta.VersionId.ShouldBeNull("List Meta.VersionId is Not Supposed to be Sent - Not In Use Field");
            consultationsList.Meta.LastUpdated.ShouldBeNull("List Meta.LastUpdated is Not Supposed to be Sent - Not In Use Field");
            consultationsList.Source.ShouldBeNull("List Source is Not Supposed to be Sent - Not In Use Field");
        }

        [Then(@"I Check the Consultation Lists are Valid")]
        public void ThenIChecktheConsultationListsareValid()
        {
            //Check atleast one Consultation List exists using Snomed Code
            Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kConsultation).ToList().Count().ShouldBeGreaterThan(0, "Failed to Find ONE Consultation list using Snomed Code.");

            //Get all Consultation lists
            var AllConsultationLists = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kConsultation).ToList();

            AllConsultationLists.ForEach(consultationList =>
            {
                //Check Code display has correct display value
                consultationList.Code.Coding.First().Display.ShouldBe("Consultation");

                //Check ID
                consultationList.Id.ShouldNotBeNullOrEmpty("The Consultations List Has No ID");

                //Check Meta Profile
                CheckForValidMetaDataInResource(consultationList, FhirConst.StructureDefinitionSystems.kList);

                //Check List Status
                consultationList.Status.ToString().ToLower().ShouldBe("current", "List Status is NOT set to current");

                //Check Mode
                consultationList.Mode.ShouldBeOfType<ListMode>("Mode List is of wrong type.");
                consultationList.Mode.ToString().ToLower().ShouldBe("snapshot", "List Status is NOT set to completed");

                //Check Patients Ref Exists
                Patients.Where(p => p.Id == (consultationList.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");

                //Check Date
                consultationList.Date.ShouldNotBeNull();

                //Check OrderedBy Condable Concept
                CodeableConcept orderedBy = (CodeableConcept)consultationList.OrderedBy;
                orderedBy.Coding.Count.Equals(1);
                orderedBy.Coding.First().System.Equals(FhirConst.CodeSystems.kListOrder);

                //Check List Title (removed as title is required not mandatory)
                //consultationList.Title.ShouldBe(FhirConst.ListTitles.kConsultation, "Consultation List Title is Incorrect");

                //Encounter Ref is checked in Consultations List Check

                //Check each Entry/Item/Reference points to a resource that exists
                string pattern = @"(.*/)(.*)";
                consultationList.Entry.ForEach(entry =>
                {
                    string refToFind = Regex.Replace(entry.Item.Reference, pattern, "$2");
                    Bundle.GetResources()
                                .Where(resource => resource.ResourceType.Equals(ResourceType.List))
                                .Where(resource => resource.Id == refToFind)
                                .ToList().Count().ShouldBe(1, "Topic List resource Not Found");
                    Logger.Log.WriteLine("Consultations List - Verified the Linked Topic has been included In the Bundle: " + refToFind);
                });

                Logger.Log.WriteLine("Completed Mandatory checks on Consultation List : " + consultationList.Id);
            });


        }

        [Then(@"I Check All The Consultation Lists Do Not Include Not In Use Fields")]
        public void GivenTheConsultatatonsListDoesNotIncludeNotInUseFields()
        {
            var ConsultationLists = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kConsultation ||
                                                   l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kTopics ||
                                                   l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kHeadings).ToList();

            ConsultationLists.ForEach(list =>
            {
                list.Identifier.Count().ShouldBe(0);
                list.Source.ShouldBeNull();
                list.Note.Count().ShouldBe(0);
                list.Entry.Where(entry => entry.Flag != null).Count().ShouldBe(0, "Entry.flag should not be used");
                list.Entry.Where(entry => entry.Deleted != null).Count().ShouldBe(0, "Entry.Deleted should not be used");
                list.Entry.Where(entry => entry.Date != null).Count().ShouldBe(0, "Entry.Date should not be used");
            });
        }

        [Then(@"I Check the Topic Lists are Valid")]
        public void ThenIChecktheTopicLists()
        {
            //Get all Topic lists
            var AllTopicLists = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kTopics).ToList();

            //check have atleast one Topic as per Data Requirements
            AllTopicLists.Count().ShouldBeGreaterThan(0, "No Topics Found in response. Patient is supposed to have Consultation linked to a Topic");

            AllTopicLists.ForEach(topicList =>
            {

                // Check Code display has correct display value
                topicList.Code.Coding.First().Display.ShouldBe("Topic (EHR)");

                //Check ID
                topicList.Id.ShouldNotBeNullOrEmpty("The Topic List Has No ID");

                //Check Meta Profile
                CheckForValidMetaDataInResource(topicList, FhirConst.StructureDefinitionSystems.kList);

                //Check List Status
                topicList.Status.ToString().ToLower().ShouldBe("current", "List Status is NOT set to current");

                //Check Mode
                topicList.Mode.ShouldBeOfType<ListMode>("Mode List is of wrong type.");
                topicList.Mode.ToString().ToLower().ShouldBe("snapshot", "List Status is NOT set to completed");

                //Check Patients Ref Exists
                Patients.Where(p => p.Id == (topicList.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");

                //Check Date
                topicList.Date.ShouldNotBeNull();

                //Check OrderedBy Condable Concept
                CodeableConcept orderedBy = (CodeableConcept)topicList.OrderedBy;
                orderedBy.Coding.Count.Equals(1);
                orderedBy.Coding.First().System.Equals(FhirConst.CodeSystems.kListOrder);

                //Check List Title (removed as title is required not mandatory)
                //topicList.Title.ShouldBe(FhirConst.ListTitles.kConsultation, "Topic List Title is Incorrect");

                //Encounter Ref is checked in Consultations List Check

                Logger.Log.WriteLine("Completed Mandatory Checks on Topic List : " + topicList.Id);

                //Check Linked Clinical or headings Resources Exist
                string pattern = @"(.*)(/)(.*)";
                var topicEntries = topicList.Entry;

                Logger.Log.WriteLine("Checking Entries on Topic List : " + topicList.Id);

                //check each Topic Entry
                foreach (var topicEntry in topicEntries)
                {
                    if (topicEntry.Item.Reference != null)
                    {
                        string refToFind = Regex.Replace(topicEntry.Item.Reference, pattern, "$3");
                        string refTypeToFind = Regex.Replace(topicEntry.Item.Reference, pattern, "$1");

                        //If List - ie Header
                        if (topicEntry.Item.Reference.StartsWith("List/"))
                        {
                            Bundle.GetResources()
                                .Where(resource => resource.ResourceType.Equals(ResourceType.List))
                                .Where(resource => resource.Id == refToFind)
                                .ToList().Count().ShouldBe(1, "Heading List resource Not Found");
                            Logger.Log.WriteLine("Found Heading List : " + topicEntry.Item.Reference);
                        }
                        else
                        {
                            VerifyResourceReferenceExists(refTypeToFind, refToFind);
                        }
                    }
                }
                        
            }); //end loop all topics


            //Check Atleast One Topic is linked to a Problem (as per data requirements)
            var found = false;
            AllTopicLists.ForEach(topicList =>
            {
                var problems = topicList.Extension.Where(e => e.Url == FhirConst.StructureDefinitionSystems.kExtProblem).ToList();

                //check each Problem reference exists in bundle
                string pattern = @"(.*/)(.*)";
                problems.ForEach(problem =>
                {
                    string refToFind = Regex.Replace(((Hl7.Fhir.Model.ResourceReference)problem.Value).Reference, pattern, "$2");
                    
                    int count = Bundle.GetResources()
                                .Where(resource => resource.ResourceType.Equals(ResourceType.Condition))
                                .Where(resource => resource.Id == refToFind)
                                .ToList().Count();
                    if (count >= 1)
                        found = true;

                });
            });

            //check had atleast one topic is related to a problem as per data requirements for test
            if (found)
            {
                Logger.Log.WriteLine("Found a Problem Linked to a topic");
            }
            else
            {
                found.ShouldBeTrue("Failed : No Topic Lists have a related problem as per the Data requirements for Test");
                Logger.Log.WriteLine("Failed to Find One topic Linked to a Problme as per the Data requirements");
            }
        }

        [Then(@"I Check the Heading Lists are Valid")]
        public void ThenIChecktheHeadingListsareValid()
        {
            //Get all Heading lists
            var AllHeadingLists = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kHeadings).ToList();

            //check have atleast one Heading as per Data Requirements
            AllHeadingLists.Count().ShouldBeGreaterThan(0, "No Heading Found in response. Patient is supposed to have Consultation linked to a Topic/Heading");

            AllHeadingLists.ForEach(headingList =>
            {
                // Check Code display has correct display value
                headingList.Code.Coding.First().Display.ShouldBe("Category (EHR)");

                //Check ID
                headingList.Id.ShouldNotBeNullOrEmpty("The Heading List Has No ID");

                //Check Meta Profile
                CheckForValidMetaDataInResource(headingList, FhirConst.StructureDefinitionSystems.kList);

                //Check List Status
                headingList.Status.ToString().ToLower().ShouldBe("current", "List Status is NOT set to current");

                //Check Mode
                headingList.Mode.ShouldBeOfType<ListMode>("Mode List is of wrong type.");
                headingList.Mode.ToString().ToLower().ShouldBe("snapshot", "List Status is NOT set to completed");

                //Check Patients Ref Exists
                Patients.Where(p => p.Id == (headingList.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");

                //Check Date
                headingList.Date.ShouldNotBeNull();

                //Check OrderedBy Condable Concept
                CodeableConcept orderedBy = (CodeableConcept)headingList.OrderedBy;
                orderedBy.Coding.Count.Equals(1);
                orderedBy.Coding.First().System.Equals(FhirConst.CodeSystems.kListOrder);

                //Check List Title (removed as title is required not mandatory)
                //headingList.Title.ShouldBe(FhirConst.ListTitles.kConsultation, "Heading List Title is Incorrect");

                //Encounter Ref is checked in Consultations List Check

                Logger.Log.WriteLine("Completed Mandatory Checks on Heading List : " + headingList.Id);

                //Check Linked Clinical or headings Resources Exist
                string pattern = @"(.*)(/)(.*)";
                var headingEntries = headingList.Entry;

                Logger.Log.WriteLine("Checking Entries on Heading List : " + headingList.Id);

                //check each Heading Entry
                foreach (var headingEntry in headingEntries)
                {
                    if (headingEntry.Item.Reference != null)
                    {
                        string refToFind = Regex.Replace(headingEntry.Item.Reference, pattern, "$3");
                        string refTypeToFind = Regex.Replace(headingEntry.Item.Reference, pattern, "$1");

                        //Verify Clinical Items are included in Bundle
                        VerifyResourceReferenceExists(refTypeToFind, refToFind);
                    }
                }

            });
        }

        [Then(@"I Check The Problems List")]
        public void ThenICheckTheProblemsList()
        {

            var probList = Lists.Where(l => l.Title == "Problems");

            if (probList.Count() == 1)
            {
                var list = probList.First();

                list.Title.ShouldBe("Problems", "Problems List Title is Incorrect");
                CheckForValidMetaDataInResource(list, FhirConst.StructureDefinitionSystems.kList);

                list.Status.ShouldBeOfType<List.ListStatus>("Status List is of wrong type.");
                list.Status.ToString().ToLower().ShouldBe("current", "List Status is NOT set to completed");

                list.Mode.ShouldBeOfType<ListMode>("Mode List is of wrong type.");
                list.Mode.ToString().ToLower().ShouldBe("snapshot", "List Status is NOT set to completed");

                list.Code.Coding.ForEach(coding =>
                {
                    coding.System.ShouldBeOneOf("http://snomed.info/sct", "http://read.info/readv2", "http://read.info/ctv3", "https://fhir.hl7.org.uk/Id/emis-drug-codes", "https://fhir.hl7.org.uk/Id/egton-codes", "https://fhir.hl7.org.uk/Id/multilex-drug-codes", "https://fhir.hl7.org.uk/Id/resipuk-gemscript-drug-codes");
                    coding.Code.ShouldBe("717711000000103", "Code is not Correct");
                    coding.Display.ShouldNotBeNullOrEmpty("Display Should not be Null or Empty");
                });

                Patients.Where(p => p.Id == (list.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");

                //check number of Conditions matches number in list
                if (Conditions.Count() != list.Entry.Count())
                {
                    Conditions.Count().ShouldBe(list.Entry.Count(), "Number of Conditions does not match the number in the List");
                }
                else
                {
                    list.Entry.ForEach(entry =>
                    {
                        string guidToFind = entry.Item.Reference.Replace("Condition/", "");
                        Conditions.Where(i => i.Id == guidToFind).Count().ShouldBe(1, "Not Found Reference to Condition");
                    });
                }
            }
            else
            {
                probList.Count().ShouldBe(1, "Expected One Problems List But Found Zero or more than 1");
            }


        }

		//SJD 04/10/2019 for 1.3.1
		[Given(@"I add a madeUp consultation part parameter")]
		public void GivenIAddAMadeUpConsultationPartParameter()
		{
			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create("madeUp", (Base)new FhirString ("madeUpValue1")),
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kConsultations, tuples);

		}

        //PG - 14/10/2019 - Added for 1.3.1
        public void CheckResourceExists<T>(T resourceType, string resourceID)
        {
            Bundle.GetResources()
                           .Where(resource => resource.ResourceType.Equals(resourceType))
                           .Where(resource => resource.Id == resourceID)
                           .ToList().Count().ShouldBe(1, "Resource Not Found : " + resourceID);

            Logger.Log.WriteLine("Found Linked resource : " + resourceID + " Of Type : " + resourceType);

        }

        public void VerifyResourceReferenceExists(string refTypeToFind, string refToFind)
        {
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

        public void VerifyCommonConsultationListMandatoryFields()
        {


        }


    



    }


}
