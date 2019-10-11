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


        [Then(@"I Check the Consultations List")]
        public void ThenIChecktheConsultationsList()
        {
            //Check for List using Snomed Code
            Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kConsultations).ToList().Count().ShouldBe(1, "Failed to Find ONE Consultations list using Snomed Code." );
            var consultationsList = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kConsultations).First();
            
            //Check Code display has correct display value
            consultationsList.Code.Coding.First().Display.ShouldBe("List of Consultations");

            //Check ID
            consultationsList.Id.ShouldNotBeNullOrEmpty("The Consultations List Has No ID");

            //Check Meta Profile
            CheckForValidMetaDataInResource(consultationsList, FhirConst.StructureDefinitionSystems.kList);

            //Check List Status
            consultationsList.Status.ToString().ToLower().ShouldBe("current", "List Status is NOT set to current");

            //Check Mode
            consultationsList.Mode.ShouldBeOfType<ListMode>("Mode List is of wrong type.");
            consultationsList.Mode.ToString().ToLower().ShouldBe("snapshot", "List Status is NOT set to completed");

            //Check Patients Ref Exists
            Patients.Where(p => p.Id == (consultationsList.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");

            //Check Date
            consultationsList.Date.ShouldNotBeNull();
            
            //Check OrderedBy Condable Concept
            CodeableConcept orderedBy = (CodeableConcept)consultationsList.OrderedBy;
            orderedBy.Coding.Count.Equals(1);
            orderedBy.Coding.First().System.Equals(FhirConst.CodeSystems.kListOrder);

            //Check List Title
            consultationsList.Title.ShouldBe(FhirConst.ListTitles.kConsultations, "Consultations List Title is Incorrect");

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




        }

        [Then(@"I Check All The Consultation Lists Do Not Include Not In Use Fields")]
        public void GivenTheConsultatatonsListDoesNotIncludeNotInUseFields()
        {
            var ConsultationLists = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kConsultations ||
                                                   l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kConsultation ||
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



        [Then(@"I Check the Consultation Lists")]
        public void ThenIChecktheConsultationLists()
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
                });

            });
        }


        [Then(@"I Check the Topic Lists")]
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

                //-----------------------------------
                //Mkae below into a function to be re-used by headings function for checkign clincal items
                //-----------------------------------

                //Check Linked Clinical or headings Exist

                //loop each item/reference

                //if List (heading) - check exists type and id

                //else
                //get first part of string to work out the type

                //switch based on type
                //  if medication request
                //check object tyope and id exists
                //check related meds items exist
                //if observation
                //check object tyope and id exists
                //if immunization





            });


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
            found.ShouldBeTrue("Failed : No Topic Lists have a related problem as per the Data requirements for Test");
        }


        //PG - 1.3.1 - Function to check Consultations references are included in bundle
        [Then(@"I Check the Consultations Resource linking")]
        public void ThenIChecktheConsultationsResourcelinking()
        {
            //find all consultations lists (find snowmed code)
            var consultationsList = Lists.Where(l => l.Code.Coding.First().Code == "999999");
            string pattern = @"(.*/)(.*)";

            consultationsList.Count().ShouldBeGreaterThan(0, "Failed to Find any Consultation lists with Snomed Code for Consultations");
           

            //check each consultation list
            foreach (var consultList in consultationsList)
            {
                consultList.Code.Coding.First().Display.ShouldBe("Consultation");

                //check each entry (item/reference) exists
                foreach (var listEntry in consultList.Entry)
                {
                    
                    //if List Ref (topics) - Check list exists then parse that list
                    if(listEntry.Item.Reference.StartsWith("List/"))
                    {
                        //get Topics List
                        string refToFind = Regex.Replace(listEntry.Item.Reference, pattern, "$2");
                        var topicList = Lists.Where(l => l.Id == refToFind).First();
                        Logger.Log.WriteLine("Found Topic List : " + refToFind);

                        topicList.Code.Coding.First().Code.ShouldBe("25851000000105");
                        topicList.Code.Coding.First().Display.ShouldBe("Topic (EHR)");

                        var topicEntries = topicList.Entry;

                        //check each Topic Entry
                        foreach (var topicEntry in topicEntries)
                        {
                            if (topicEntry.Item.Reference != null)
                            {
                                //If List - ie Header
                                if (topicEntry.Item.Reference.StartsWith("List/"))
                                {
                                    //check header references
                                    string listRefToFind = Regex.Replace(topicEntry.Item.Reference, pattern, "$2");
                                    var headerList = Lists.Where(l => l.Id == listRefToFind).First();
                                    Logger.Log.WriteLine("Found Header List : " + listRefToFind);

                                    headerList.Code.Coding.First().Code.ShouldBe("24781000000107");
                                    headerList.Code.Coding.First().Display.ShouldBe("Category (EHR)");

                                    var headerEntries = headerList.Entry;

                                    //Check Header Entries Exist (should be clinical items only)
                                    foreach (var headerEntry in headerEntries)
                                    {
                                        //check if a reference or just text to ignore
                                        if (headerEntry.Item.Reference != null)
                                        {
                                            string headerRefToFind = Regex.Replace(headerEntry.Item.Reference, pattern, "$2");
                                            _httpContext.FhirResponse.Entries.Where(e => e.Resource.Id == headerRefToFind).Count().ShouldBe(1, "Linked Resource Not Found: " + headerEntry.Item.Reference);
                                            Logger.Log.WriteLine("Found Reference :" + headerRefToFind + " In Bundle");
                                        }
                                    }
                                }
                                //Check clinical Items
                                else
                                {
                                    string topicEntryToFind = Regex.Replace(topicEntry.Item.Reference, pattern, "$2");
                                    _httpContext.FhirResponse.Entries.Where(e => e.Resource.Id == topicEntryToFind).Count().ShouldBe(1, "Linked Resource Not Found: " + topicEntry.Item.Reference);
                                    Logger.Log.WriteLine("Found Reference :" + topicEntryToFind + " In Bundle");
                                }
                            }
                        }
                    }
                    //Clinical Items To Check
                    else
                    {
                        string refToFind = Regex.Replace(listEntry.Item.Reference, pattern, "$2");
                        _httpContext.FhirResponse.Entries.Where(entry => entry.Resource.Id == refToFind).Count().ShouldBe(1, "Linked Resource Not Found: " + listEntry.Item.Reference);
                        Logger.Log.WriteLine("Found Reference :" + refToFind + " In Bundle");
                    }


                }

                 


            }

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



	}


}
