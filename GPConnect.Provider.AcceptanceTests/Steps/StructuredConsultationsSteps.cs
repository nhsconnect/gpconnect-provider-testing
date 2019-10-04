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


        //PG - 1.3.1 - Function to check Consultations references are included in bundle
        [Then(@"I Check the Consultations Resource linking")]
        public void ThenIChecktheConsultationsResourcelinking()
        {
            //find all consultations lists (find snowmed code)
            var consultationsList = Lists.Where(l => l.Code.Coding.First().Code == "325851000000107");
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


    }


}
