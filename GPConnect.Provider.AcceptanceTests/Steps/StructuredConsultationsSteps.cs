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
        private List<List> Lists => _httpContext.FhirResponse.Lists;
        private List<Patient> Patients => _httpContext.FhirResponse.Patients;
        private List<Condition> Problems => _httpContext.FhirResponse.Conditions;
        private Bundle Bundle => _httpContext.FhirResponse.Bundle;
        private List<AllergyIntolerance> ActiveAllergyIntolerances => _httpContext.FhirResponse.AllergyIntolerances;
        private List<Observation> Observations => _httpContext.FhirResponse.Observations;
        private List<Immunization> Immunizations => _httpContext.FhirResponse.Immunizations;
        private List<Medication> Medications => _httpContext.FhirResponse.Medications;
        private List<MedicationStatement> MedicationStatements => _httpContext.FhirResponse.MedicationStatements;
        private List<MedicationRequest> MedicationRequests => _httpContext.FhirResponse.MedicationRequests;

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
            consultationsList.Code.Coding.First().Display.ShouldBe("List of consultations");
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

            consultationsList.Meta.VersionId.ShouldBeNull("List Meta.VersionId is Not Supposed to be Sent - Not In Use Field");
            consultationsList.Meta.LastUpdated.ShouldBeNull("List Meta.LastUpdated is Not Supposed to be Sent - Not In Use Field");
            consultationsList.Source.ShouldBeNull("List Source is Not Supposed to be Sent - Not In Use Field");
        }

        [Then(@"I Check the Encounters are Valid")]
        public void ThenIChecktheEncountersareValid()
        {
            //check atleast one
            Encounters.ToList().Count().ShouldBeGreaterThan(0, "Error Should be Atleast One Encounter in response as per Data requirements");

            Encounters.ForEach(encounter =>
            {
                //Check Id
                encounter.Id.ShouldNotBeNullOrEmpty();

                //Check Meta.profile
                CheckForValidMetaDataInResource(encounter, FhirConst.StructureDefinitionSystems.kEncounter);

                //Check Identfier
                encounter.Identifier.Count.ShouldBeGreaterThan(0, "There should be at least 1 Identifier system/value pair");
                encounter.Identifier.ForEach(identifier =>
                {
                    identifier.System.Equals(FhirConst.ValueSetSystems.kCrossCareIdentifier).ShouldBeTrue("Cross Care Setting Identfier NOT Found");

                    //identifier.Value format is still being debated, hence notnull check
                    identifier.Value.ShouldNotBeNullOrEmpty("Identifier Value Is Null or Not Valid");
                    //Guid guidResult;
                    //Guid.TryParse(identifier.Value, out guidResult).ShouldBeTrue("Immunization identifier GUID is not valid or Null");
                });

                //Check Status
                encounter.Status.ToString().ShouldNotBeNull("Encounter Status should not be null");

                //Check type
                encounter.Type.ShouldNotBeEmpty("Encounter Type should not be null");

                //Check Subject/patient
                Patients.Where(p => p.Id == (encounter.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");

                //Check Participant
                var found = false;
                encounter.Participant.ForEach(participant =>
                {
                    participant.Type.ForEach(type =>
                    {
                        type.Coding.ForEach(code =>
                        {
                            if (code.System == FhirConst.CodeSystems.kCcPartipationType && code.Code == "AUTH")
                                found = true;
                        });
                    });
                });

               found.ShouldBeTrue("Failed to Find Participant.type with value of AUTH");
                
            });


        }

        [Then(@"I Check the Encounters Do Not Include Not in Use Fields")]
        public void ThenIChecktheEncountersDoNotIncludeNotinUseFields()
        {
            //check atleast one
            Encounters.ToList().Count().ShouldBeGreaterThan(0, "Error Should be Atleast One Encounter in response as per Data requirements");

            Encounters.ForEach(encounter =>
            {
                encounter.StatusHistory.Count().ShouldBe(0, "Failed Encounter Check: StatusHistory Should not be used - Not In Use Field");
                encounter.Class.ShouldBeNull("Failed Encounter Check: Class Should not be used - Not In Use Field");
                encounter.ClassHistory.Count().ShouldBe(0, "Failed Encounter Check: ClassHistory Should not be used - Not In Use Field");
                encounter.Priority.ShouldBeNull("Failed Encounter Check: Priority Should not be used - Not In Use Field");
                encounter.EpisodeOfCare.Count().ShouldBe(0, "Failed Encounter Check: EpisodeOfCare Should not be used - Not In Use Field");
                encounter.IncomingReferral.Count().ShouldBe(0, "Failed Encounter Check: IncomingReferral Should not be used - Not In Use Field");
                encounter.Appointment.ShouldBeNull("Failed Encounter Check: Appointment Should not be used - Not In Use Field");
                encounter.Reason.Count().ShouldBe(0, "Failed Encounter Check: Reason Should not be used - Not In Use Field");
                encounter.Diagnosis.Count().ShouldBe(0, "Failed Encounter Check: Diagnosis Should not be used - Not In Use Field");
                encounter.Account.Count().ShouldBe(0, "Failed Encounter Check: Account Should not be used - Not In Use Field");
                encounter.Hospitalization.ShouldBeNull("Failed Encounter Check: Hospitalization Should not be used - Not In Use Field");
                encounter.PartOf.ShouldBeNull("Failed Encounter Check: IncomingReferral Should not be used - Not In Use Field");
            });
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

                //Check Common Mandatory fields
                VerifyCommonConsultationListMandatoryFields(consultationList);

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
                    Logger.Log.WriteLine("Consultation List - Verified the Linked Topic has been included In the Bundle: " + refToFind);
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

                //Check Common Mandatory fields
                VerifyCommonConsultationListMandatoryFields(topicList);

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


        }

        [Then(@"I Check one Topic is linked to a problem")]
        public void ThenICheckoneTopicislinkedtoaproblem()
        {
            //Get all Topic lists
            var AllTopicLists = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kTopics).ToList();

            //Check Atleast One Topic is linked to a Problem (as per data requirements)
            var found = false;
            AllTopicLists.ForEach(topicList =>
            {
                try
                {
                    var problemHeaders = topicList.Extension.Where(e => e.Url == FhirConst.StructureDefinitionSystems.kExtProblem).ToList();

                    //check each Problem reference exists in bundle
                    string pattern = @"(.*/)(.*)";
                    problemHeaders.ForEach(problemHeader =>
                    {
                        var ProbHeaderExtensions = ((Extension)problemHeader).Extension;

                        ProbHeaderExtensions.ForEach(phe =>
                        {
                            string refToFind = Regex.Replace(((Hl7.Fhir.Model.ResourceReference)phe.Value).ReferenceElement.Value, pattern, "$2");

                            int count = Bundle.GetResources()
                                        .Where(resource => resource.ResourceType.Equals(ResourceType.Condition))
                                        .Where(resource => resource.Id == refToFind)
                                        .ToList().Count();
                            if (count >= 1)
                                found = true;

                        });

                    });


                }
                catch (Exception)
                {
                }


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

                //Check Common Mandatory fields
                VerifyCommonConsultationListMandatoryFields(headingList);

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

        [Given(@"I add a madeUp consultation part parameter")]
		public void GivenIAddAMadeUpConsultationPartParameter()
		{
			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create("madeUp", (Base)new FhirString ("madeUpValue1")),
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kConsultations, tuples);

		}

        public void CheckResourceExists<T>(T resourceType, string resourceID)
        {
            Bundle.GetResources()
                           .Where(resource => resource.ResourceType.Equals(resourceType))
                           .Where(resource => resource.Id == resourceID)
                           .ToList().Count().ShouldBe(1, "Fail : Linked Resource Not Contained in Response - type : " + resourceType + " - ID : " + resourceID);

            Logger.Log.WriteLine("Found Linked resource : " + resourceID + " Of Type : " + resourceType);

        }
        
        public void VerifyResourceReferenceExists(string refTypeToFind, string refToFind)
        {
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

                //unknown type ignore - could be not supported message
                default:
                    Logger.Log.WriteLine("Ignored, Entry/Item/Reference for : " + refTypeToFind);
                    break;
            }
        }

        public void VerifyCommonConsultationListMandatoryFields(List listToCheck)
        {
            //Check ID
            listToCheck.Id.ShouldNotBeNullOrEmpty("The Consultations List Has No ID");

            //Check Meta Profile
            CheckForValidMetaDataInResource(listToCheck, FhirConst.StructureDefinitionSystems.kList);

            //Check List Status
            listToCheck.Status.ToString().ToLower().ShouldBe("current", "List Status is NOT set to current");

            //Check Mode
            listToCheck.Mode.ShouldBeOfType<ListMode>("Mode List is of wrong type.");
            listToCheck.Mode.ToString().ToLower().ShouldBe("snapshot", "List Status is NOT set to completed");

            //Check Patients Ref Exists
            Patients.Where(p => p.Id == (listToCheck.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");

            //Check Date
            listToCheck.Date.ShouldNotBeNull();

            //Check OrderedBy Condable Concept
            CodeableConcept orderedBy = (CodeableConcept)listToCheck.OrderedBy;
            orderedBy.Coding.Count.Equals(1);
            orderedBy.Coding.First().System.Equals(FhirConst.CodeSystems.kListOrder);

        }


        [Given(@"I add the includeConsultations parameter only")]
		public void GivenIAddTheConsultationsParameterOnly()
		{
			ParameterComponent param = new ParameterComponent();
			param.Name = FhirConst.GetStructuredRecordParams.kConsultations;
			_httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(param);
		}

		[Given(@"I add malformed Consultations request partParameter only")]
		public void GivenIAddMalformedConsultationsRequestPartParameterOnly()
		{
			ParameterComponent param = new ParameterComponent();
			param.Name = FhirConst.GetStructuredRecordParams.kConsultationSearch;
			_httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(param);
		}

		[Given(@"I add the consultation parameter with consultationSearchPeriod partParameter")]
		public void GivenIAddTheConsultationParameterWithConsultationSearchPeriodParameter()
		{
			var backDate = DateTime.UtcNow.AddYears(-2);
			var futureDate = DateTime.UtcNow.AddDays(-1);
			var startDate = backDate.ToString("yyyy-MM-dd");
			var endDate = futureDate.ToString("yyyy-MM-dd");

			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kConsultationSearch, (Base)FhirHelper.GetTimePeriod(startDate, endDate)),
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kConsultations, tuples);
		}

		[Given(@"I add the consultation parameter with consultationSearchPeriod partParameter in the future")]
		public void GivenIAddTheConsultationParameterWithConsultationSearchPeriodParameterInTheFuture()
		{
			var backDate = DateTime.UtcNow.AddDays(1);
			var futureDate = DateTime.UtcNow.AddDays(1);
			var startDate = backDate.ToString("yyyy-MM-dd");
			var endDate = futureDate.ToString("yyyy-MM-dd");

			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kConsultationSearch, (Base)FhirHelper.GetTimePeriod(startDate, endDate)),
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kConsultations, tuples);
		}


		[Given(@"I add the consultation parameter with startDate only")]
		public void GivenIAddTheConsultationParameterWithStartDateOnly()
		{
			var backDate = DateTime.UtcNow.AddYears(-2);
			var futureDate = DateTime.UtcNow.AddDays(-1);
			var startDate = backDate.ToString("yyyy-MM-dd");
			//var endDate = futureDate.ToString("yyyy-MM-dd");

			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kConsultationSearch, (Base)FhirHelper.GetTimePeriod(startDate, null)),
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kConsultations, tuples);
		}

		[Given(@"I add the consultation parameter with endDate only")]
		public void GivenIAddTheConsultationParameterWithEndDateOnly()
		{
			var backDate = DateTime.UtcNow.AddDays(-1);
			var futureDate = DateTime.UtcNow.AddDays(-1);
			//var startDate = backDate.ToString("yyyy-MM-dd");
			var endDate = futureDate.ToString("yyyy-MM-dd");

			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kConsultationSearch, (Base)FhirHelper.GetTimePeriod(null,endDate)),
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kConsultations, tuples);
		}

		[Given(@"I add the consultation parameter with consultationSearchPeriod partParameter startDate greater than endDate")]
		public void GivenIAddTheConsultationParameterWithConsultationSearchPeriodParameterStartDateGreaterThanEndDate()
		{
			var backDate = DateTime.UtcNow.AddDays(-5);
			var futureDate = DateTime.UtcNow.AddDays(-6);
			var startDate = backDate.ToString("yyyy-MM-dd");
			var endDate = futureDate.ToString("yyyy-MM-dd");

			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kConsultationSearch, (Base)FhirHelper.GetTimePeriod(startDate, endDate)),
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kConsultations, tuples);
		}

		[Given(@"I add the consultation parameter with consultationsMostRecent partParameter")]
		public void GivenIAddTheConsultationParameterWithConsultationMostRecentPartParameter()
		{
			var backDate = DateTime.UtcNow.AddDays(-10);
			var futureDate = DateTime.UtcNow.AddDays(-5);
			var startDate = backDate.ToString("yyyy-MM-dd");
			var endDate = futureDate.ToString("yyyy-MM-dd");

			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kConsultationsMostRecent, (Base) new Integer(3)),
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kConsultations, tuples);
		}

		[Given(@"I add the consultation parameter with both partParameters")]
		public void GivenIAddTheConsultationParameterWithWithBothPartParameter()
		{
			var backDate = DateTime.UtcNow.AddDays(-10);
			var futureDate = DateTime.UtcNow.AddDays(-5);
			var startDate = backDate.ToString("yyyy-MM-dd");
			var endDate = futureDate.ToString("yyyy-MM-dd");


			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kConsultationsMostRecent, (Base) new Integer(4)),
				Tuple.Create(FhirConst.GetStructuredRecordParams.kConsultationSearch, (Base)FhirHelper.GetTimePeriod(startDate, endDate))
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kConsultations, tuples);
		}


		[Given(@"I send an unknownConsultations parameterName containing valid part parameter")]
		public void GivenISendAnUnknownConsultationsParameterNameContainingValidPartParameters()
		{
			var backDate = DateTime.UtcNow.AddDays(-10);
			var futureDate = DateTime.UtcNow.AddDays(5);
			var startDate = backDate.ToString("yyyy-MM-dd");
			var endDate = futureDate.ToString("yyyy-MM-dd");

			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kConsultationSearch, (Base)FhirHelper.GetTimePeriod(startDate, endDate)),
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add("unknownConsultations", tuples);
		}

		[Given(@"I set a consultations period parameter ""([^ ""]*)"" to ""([^ ""]*)""")]
		public void GivenISetAConsultationsPeriodParameterTo(string startDate, string endDate)
		{
					   			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kConsultationSearch, (Base)FhirHelper.GetTimePeriod(startDate, endDate))
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kConsultations, tuples);

		}

        [Then(@"I Check that a Topic or Heading is linked to an ""(.*)"" and that is included in response with a list")]
        public void GivenICheckthataTopicorHeadingislinkedtoanandthatisincludedinresponsewithalist(string resourcetypeToCheck)
        {
            //Check If ResourceType is linked on a Topic
            bool foundAndVerified = false;

            //loop all topics grabbing all Clinical item refs
            var AllTopicLists = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kTopics).ToList();

            AllTopicLists.ForEach(topicList =>
            {
                var topicEntries = topicList.Entry;
                string pattern = @"(.*)(/)(.*)";

                foreach (var topicEntry in topicEntries)
                {
                    if (topicEntry.Item.Reference != null)
                    {
                        string refToFind = Regex.Replace(topicEntry.Item.Reference, pattern, "$3");
                        string refTypeToFind = Regex.Replace(topicEntry.Item.Reference, pattern, "$1");

                        //If List - ie Header
                        if (topicEntry.Item.Reference.StartsWith((resourcetypeToCheck + "/")))
                        {
                            VerifyResourceReferenceExists(refTypeToFind, refToFind);
                            foundAndVerified = true;
                        }
                    }
                }
            });


            //If Not on a Topic - Check If ResourceType is linked on a Heading
            if (!foundAndVerified)
            {
                var AllHeadingLists = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kHeadings).ToList();
                
                //loop all topics grabbing all Clinical item refs
                AllHeadingLists.ForEach(headingList =>
                {
                    string pattern = @"(.*)(/)(.*)";
                    var headingEntries = headingList.Entry;

                    foreach (var headingEntry in headingEntries)
                    {
                        if (headingEntry.Item.Reference != null)
                        {
                            string refToFind = Regex.Replace(headingEntry.Item.Reference, pattern, "$3");
                            string refTypeToFind = Regex.Replace(headingEntry.Item.Reference, pattern, "$1");

                            if (headingEntry.Item.Reference.StartsWith((resourcetypeToCheck + "/")))
                            {
                                VerifyResourceReferenceExists(refTypeToFind, refToFind);
                                foundAndVerified = true;
                            }
                        }
                    }

                });

            }

            foundAndVerified.ShouldBeTrue("Fail : Expected Clinical Item of type : " + resourcetypeToCheck + " - to be Linked to a Topic or Heading");

            //check clinical item has the correct list in response
            Hl7.Fhir.Model.List listToCheck = new Hl7.Fhir.Model.List();
            switch (resourcetypeToCheck)
            {
                case "AllergyIntolerance":
                    Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kActiveAllergies).ToList().Count().ShouldBe(1, "Fail : 0 or more than one Active Allergies lists Detected - Expected 1");
                    listToCheck = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kActiveAllergies).First();
                    Logger.Log.WriteLine("Info : Found Allergies List with Snomed ID : " + FhirConst.GetSnoMedParams.kActiveAllergies);
                    break;

                case "Observation": //uncat
                    Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kUncategorised).ToList().Count().ShouldBe(1, "Fail : 0 or more than one Observation list Detected - Expected 1");
                    listToCheck = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kUncategorised).First();
                    Logger.Log.WriteLine("Info : Found Observation List with Snomed ID : " + FhirConst.GetSnoMedParams.kUncategorised);
                    break;

                case "Immunization":
                    Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kImmunizations).ToList().Count().ShouldBe(1, "Fail : 0 or more than one Immunizations list Detected - Expected 1");
                    listToCheck = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kImmunizations).First();
                    Logger.Log.WriteLine("Info : Found Immunization List with Snomed ID : " + FhirConst.GetSnoMedParams.kImmunizations);
                    break;

                case "MedicationRequest":
                    Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kMeds).ToList().Count().ShouldBe(1, "Fail : 0 or more than one Medication list Detected - Expected 1");
                    listToCheck = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kMeds).First();
                    Logger.Log.WriteLine("Info : Found Medications List with Snomed ID : " + FhirConst.GetSnoMedParams.kMeds);
                    break;

                //unknown type ignore - could be not supported message
                default:
                    Logger.Log.WriteLine("Info : Ignored, ResourceType for : " + resourcetypeToCheck);
                    break;
            }

            //Check all Items on List are in response
            bool foundClinicalItemOnList = false;
            listToCheck.Entry.ForEach(a =>
            {
                string pattern = @"(.*)(/)(.*)";
                string refToFind = Regex.Replace(a.Item.Reference, pattern, "$3");

                //Check references is to the type wanted
                if (resourcetypeToCheck == "MedicationRequest")
                {
                    a.Item.Reference.ShouldStartWith(("MedicationStatement" + "/"));
                    //Check resource has been included in response
                    VerifyResourceReferenceExists("MedicationStatement", refToFind);
                    foundClinicalItemOnList = true;
                }
                else
                {
                    a.Item.Reference.ShouldStartWith((resourcetypeToCheck + "/"));
                    //Check resource has been included in response
                    VerifyResourceReferenceExists(resourcetypeToCheck, refToFind);
                    foundClinicalItemOnList = true;
                }
         
            });

            foundClinicalItemOnList.ShouldBeTrue("Fail : List for : " + resourcetypeToCheck + "-Does not contain any references to clinical item of type : " + resourcetypeToCheck);
            Logger.Log.WriteLine("Info : List Found for : " + resourcetypeToCheck + "- contains references to clinical item of type : " + resourcetypeToCheck);


            //check count of item linked on list with items of that type in bundle as integrity check
            switch (resourcetypeToCheck)
            {
                case "AllergyIntolerance":
                    ActiveAllergyIntolerances.Count().ShouldBe(listToCheck.Entry.Count(), "Fail : Clinical Item Count does Not Match list Entry Count for resource type : " + resourcetypeToCheck);
                    Logger.Log.WriteLine("Info : Passed Count Check for ResourceType  : " + resourcetypeToCheck + " - Bundle count equal to list entry count of : " + listToCheck.Entry.Count().ToString());
                    break;

                case "Observation": //uncat
                    //uncat - these are not always uncategorised data resources as they are used for other purposes so cannot do a stright
                    //count check against list and resources. all items on list will be checked that they have been included above which covers checking the list
                    //Observations.Count().ShouldBe(listToCheck.Entry.Count(), "Fail : Clinical Item Count does Not Match list Entry Count for resource type : " + resourcetypeToCheck);
                    //Logger.Log.WriteLine("Info : Passed Count Check for ResourceType  : " + resourcetypeToCheck + " - Bundle count equal to list entry count of : " + listToCheck.Entry.Count().ToString());
                    break;

                case "Immunization":
                    Immunizations.Count().ShouldBe(listToCheck.Entry.Count(), "Fail : Clinical Item Count does Not Match list Entry Count for resource type : " + resourcetypeToCheck);
                    Logger.Log.WriteLine("Info : Passed Count Check for ResourceType  : " + resourcetypeToCheck + " - Bundle count equal to list entry count of : " + listToCheck.Entry.Count().ToString());
                    break;

                case "MedicationRequest":
                    MedicationStatements.Count().ShouldBe(listToCheck.Entry.Count(), "Fail : Clinical Item Count does Not Match list Entry Count for resource type : MedicationStatements" );
                    Logger.Log.WriteLine("Info : Passed Count Check for ResourceType  : " + resourcetypeToCheck + " - Bundle count equal to list entry count of : " + listToCheck.Entry.Count().ToString());
                    break;

                //unknown type ignore - could be not supported message
                default:
                    Logger.Log.WriteLine("Ignored Count Chcek for ResourceType : " + resourcetypeToCheck);
                    break;
            }

        }

    }


}
