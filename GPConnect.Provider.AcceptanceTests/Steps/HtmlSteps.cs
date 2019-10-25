using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Model;
using NUnit.Framework;
using Shouldly;
using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using TechTalk.SpecFlow;
using static Hl7.Fhir.Model.Bundle;
using HtmlAgilityPack;
using System.Linq;
using GPConnect.Provider.AcceptanceTests.Data;
using System.Collections.Generic;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public sealed class HtmlSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private readonly HttpContext HttpContext;

        public HtmlSteps(FhirContext fhirContext, HttpContext httpContext)
        {
            FhirContext = fhirContext;
            HttpContext = httpContext;
        }

        [Then(@"the html should be valid xhtml")]
        public void ThenTheHtmlShouldBeValidXHTML()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
                {
                    Composition composition = (Composition)entry.Resource;
                    foreach (Composition.SectionComponent section in composition.Section)
                    {
                        var xhtml = section.Text.Div;
                        XDocument doc = null;
                        try
                        { 
                            doc = XDocument.Parse(xhtml);
                            doc.ShouldNotBeNull();
                        }
                        catch (Exception e) {
                            Log.WriteLine("Failed to parse div to xhtml");
                            Log.WriteLine(e.StackTrace);
                            doc.ShouldNotBeNull();
                        }
                    }
                }
            }
        }

        [Then(@"the html should not contain ""([^""]*)"" tags")]
        public void ThenTheHtmlShouldNotContaintags(string tagName)
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
                {
                    Composition composition = (Composition)entry.Resource;
                    foreach (Composition.SectionComponent section in composition.Section)
                    {
                        Regex regex = new Regex("<" + tagName);
                        regex.Matches(section.Text.Div).Count.ShouldBe(0);
                    }
                }
            }
        }

        [Then(@"the html should not contain any attributes")]
        public void ThenTheHtmlShouldNotContainAnyAttributes()
        {
            // Find all matches to regex for attributes and use log to print out all instances then fail if any found.
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
                {
                    Composition composition = (Composition)entry.Resource;
                    foreach (Composition.SectionComponent section in composition.Section)
                    {
                        var html = section.Text.Div;
                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(html);

                        //check for any on attribues and the xmlns attribute
                        var onEventNodes = htmlDoc.DocumentNode
                                .Descendants().Where(n => n.Attributes.Any(a => a.Name.StartsWith("on")));

                        //Assert we have no ON event attributes in HTML
                        onEventNodes.Count().ShouldBe(0, "Found HTML tag with an ON event atribute that should not exist in HTML");
                    }
                }
            }
        }

        [Then(@"the html should contain headers in coma seperated list ""([^""]*)""")]
        public void ThenTheHTMLShouldNotContainHeadersInComaSeperatedList(string listOfHeaders)
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
                {
                    Composition composition = (Composition)entry.Resource;
                    foreach (Composition.SectionComponent section in composition.Section)
                    {
                        var html = section.Text.Div;
                        var headers = listOfHeaders.Split(',');
                        foreach (string header in headers) {
                            html.ShouldContain("<h2>"+header+"</h2>");
                        }
                    }
                }
            }
        }

        [Then(@"the html should contain table headers in coma seperated list order ""([^""]*)"" for the ""([^""]*)""")]
        public void ThenTheHTMLShouldContainTableHeadersInComaSeperatedListOrder(string listOfTableHeadersInOrder, int pageSectionIndex)
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
                {
                    Composition composition = (Composition)entry.Resource;
                    foreach (Composition.SectionComponent section in composition.Section)
                    {
                        var html = section.Text.Div;
                        var headerList = listOfTableHeadersInOrder.Split(',');
                        Regex regexHeaderSection = new Regex("<thead[\\w\\W]*?thead>");
                        MatchCollection tableHeaderSectionMatches = regexHeaderSection.Matches(html);

                        if (tableHeaderSectionMatches.Count < pageSectionIndex) {
                            Log.WriteLine("The table header count doesn't match the expected.");
                            Assert.Fail("The table header count doesn't match the expected.");
                        } else {
                            string tableHeaderSectionHTML = tableHeaderSectionMatches[pageSectionIndex - 1].Value;
                            Log.WriteLine("HeaderSection = " + tableHeaderSectionHTML);
                            Regex regexHeaders = new Regex("<th>[^<]*</th>");
                            MatchCollection matchesForTableHeadersInHTML = regexHeaders.Matches(tableHeaderSectionHTML);
                            Log.WriteLine("Number of <th> headers in html {0}, expected {1}", matchesForTableHeadersInHTML.Count, headerList.Length);
                            if (headerList.Length != matchesForTableHeadersInHTML.Count)
                            {
                                Log.WriteLine("The number of table headers in HTML section does not match the required number of headers.");
                                Assert.Fail("The number of table headers in HTML section does not match the required number of headers.");
                            }
                            else
                            {
                                for (int index = 0; index < headerList.Length; index++)
                                {
                                    Console.WriteLine("Expected Header = {0} and was {1}", "<th>"+headerList[index]+"</th>", matchesForTableHeadersInHTML[index].Value);
                                    (matchesForTableHeadersInHTML[index].Value).ShouldBe("<th>" + headerList[index] + "</th>");
                                }
                            }
                        }
                    }
                }
            }
        }

        [Then(@"the response html should contain the applied date range text ""([^""]*)"" to ""([^""]*)""")]
        public void ThenTheResponseHTMLShouldContainTheAppliedDateRangeTest(string fromDate, string toDate)
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
                {
                    Composition composition = (Composition)entry.Resource;
                    foreach (Composition.SectionComponent section in composition.Section)
                    {
                        var html = section.Text.Div;
                        string expectedTimePeriodBanner = "<p>For the period '"+ fromDate + "' to '" + toDate + "'</p>";
                        html.ShouldContain(expectedTimePeriodBanner, Case.Insensitive);
                    }
                }
            }
        }

		// #270 SJD 22/7/19 included single quotes around date banner
		//issue 193 SJD 01/05/19 no end date provided
		[Then(@"the response html should contain the applied start date banner text ""([^""]*)""")]
        public void ThenTheResponseHTMLShouldContainTheAppliedStartDateBannerText(string fromDate)
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
                {
                    Composition composition = (Composition)entry.Resource;
                    foreach (Composition.SectionComponent section in composition.Section)
                    {
                        var html = section.Text.Div;
                        string expectedTimePeriodBanner = "<p>All data items from '" + fromDate +"'</p>";
                        html.ShouldContain(expectedTimePeriodBanner, Case.Insensitive);
                    }
                }
            }
        }

		// #270 SJD 22/7/19 included single quotes around date banner
		//issue 193 SJD 01/05/19 no start date provided
		[Then(@"the response html should contain the applied end date banner text ""([^""]*)""")]
        public void ThenTheResponseHTMLShouldContainTheAppliedEndDateBannerText(string toDate)
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
                {
                    Composition composition = (Composition)entry.Resource;
                    foreach (Composition.SectionComponent section in composition.Section)
                    {
                        var html = section.Text.Div;
                        string expectedTimePeriodBanner = "<p>All data items until '" + toDate + "'</p>";
                        html.ShouldContain(expectedTimePeriodBanner, Case.Insensitive);
                    }
                }
            }
        }

        //issue 194 SJD 01/05/19 change to Banner message
        [Then(@"the response html should contain the all data items text")]
        public void ThenTheResponseHTMLShouldContainTheAllDataItemsText()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
                {
                    Composition composition = (Composition)entry.Resource;
                    foreach (Composition.SectionComponent section in composition.Section)
                    {
                        var html = section.Text.Div;
                        bool matchflag = false;

                        Regex regexHeadersPattern1 = new Regex("<p>All relevant items</p>");
                        Regex regexHeadersPattern2 = new Regex("<p>All relevant items subject to patient preferences and / or RCGP exclusions</p>");

                        MatchCollection tableBannerText1Matches = regexHeadersPattern1.Matches(html);
                        if (tableBannerText1Matches.Count >= 1)
                            matchflag = true;

                        MatchCollection tableBannerText2Matches = regexHeadersPattern2.Matches(html);
                        if (tableBannerText2Matches.Count >= 1)
                            matchflag = true;

                        matchflag.ShouldBeTrue("All data items text not found");

                    }
                }
            }
        }

        [Then(@"the html should not contain ""([^""]*)""")]
        public void ThenTheHTMLShouldNotContain(string value)
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
                {
                    Composition composition = (Composition)entry.Resource;
                    foreach (Composition.SectionComponent section in composition.Section)
                    {
                        var html = section.Text.Div;
                        html.ShouldNotContain(value);
                    }
                }
            }
        }

        [Then(@"the response html should contain the no data available html banner in section ""([^""]*)""")]
        public void ThenTheResponseHTMLShouldContainTheNoDataAvailableHTMLBannerInSection(string sectionHeading)
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
                {
                    var sectionFound = false;
                    Composition composition = (Composition)entry.Resource;
                    foreach (Composition.SectionComponent section in composition.Section)
                    {
                        var html = section.Text.Div;
                        string[] sections = html.Split(new[] { "<h2>" }, StringSplitOptions.None);

                        // Find relavant section
                        foreach (string sectionHtml in sections)
                        {
                            if (sectionHtml.Contains(sectionHeading))
                            {
                                sectionFound = true;
                                sectionHtml.Contains("<p>No '" + sectionHeading + "' data is recorded for this patient.</p>");
                                // rem #197 caused failure // var tempstring = "<p>No '" + sectionHeading + "' data is recorded for this patient.</p>";
                            }
                        }
                    }
                    sectionFound.ShouldBeTrue();
                }
            }
        }

        //#202 PG - 15/8/2019
        [Then(@"the html response contains all the following table ids ""([^""]*)""")]
        public void TheHtmlResponseContainsAllTheFollowingTableids(string listOfTableIdsToCheck)
        {

            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
                {
                    Composition composition = (Composition)entry.Resource;
                    foreach (Composition.SectionComponent section in composition.Section)
                    {
                        var html = section.Text.Div;
                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(html);

                        var tableidsListToCheck = listOfTableIdsToCheck.Split(',');

                        var tables = htmlDoc.DocumentNode
                                .Descendants("table");

                        //Check the number of Tables matches the number we expect
                        if (tableidsListToCheck.Length == tables.Count())
                        {
                            //Check each id is correct
                            for (int index = 0; index < tableidsListToCheck.Length; index++)
                            {
                                Log.WriteLine("Expected Table ID = {0} and was {1}", tableidsListToCheck[index], tables.ToArray()[index].Id);

                                tables.ToArray()[index].Id.ShouldBe(tableidsListToCheck[index], "Table ID does not match expected");
                            }

                        }
                        //Number of table id's does not match
                        else
                        {
                            //Build List of what is found to reportback
                            var idsFound = "";

                            for (int index = 0; index < tables.Count(); index++)
                            {

                                idsFound += tables.ToArray()[index].Id;
                                if (index != tables.Count() - 1)
                                    idsFound += ",";
                            }

                            string outputMessage = "The number of table ID's in HTML section does not match the required number of ID's. Actual:" +
                               tables.Count().ToString() + " Expected:" + tableidsListToCheck.Length.ToString() +
                                "   Values Expected :" + listOfTableIdsToCheck + " Found : " + idsFound;

                            Log.WriteLine(outputMessage);
                            Assert.Fail(outputMessage);
                        }

                    }
                }
            }




        }

        //202  -PG 24-10-2019
        [Then(@"the html table ""([^""]*)"" has a date-column class attribute on these ""([^""]*)""")]
        public void TheHtmlTtableHasaDateColumnClassAttributeOnTheHtmlResponsetableColumns(string TableId, string ListOfColumnsToCheck)
        {

            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
                {
                    Composition composition = (Composition)entry.Resource;
                    foreach (Composition.SectionComponent section in composition.Section)
                    {
                        var html = section.Text.Div;
                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(html);

                        var colstoCheck = ListOfColumnsToCheck.Split(',');

                        var table = htmlDoc.DocumentNode.Descendants("table").Where(t => t.Id.Equals(TableId)).FirstOrDefault();
                        if (table != null)
                        {
                            foreach (HtmlNode row in table.SelectNodes("./tbody//tr"))
                            {
                                var tdNodes = row.SelectNodes(".//td");

                                //If to ignore Grouped by row
                                if (tdNodes.ToArray().Count() != 1)
                                {
                                    foreach (var item in colstoCheck)
                                    {
                                        int indexToCheck = Int32.Parse(item)-1;
                                        try
                                        {
                                            //if TD cell is empty do not test
                                            if (!string.IsNullOrEmpty(tdNodes[indexToCheck].InnerHtml))
                                            {
                                                string passMessage = "Test for Class Attribute on Table : " + TableId + " Passed for Column " + item.ToString() + " date-column class found";
                                                tdNodes[indexToCheck].Attributes["class"].Value.Equals("date-column").ShouldBeTrue(passMessage);
                                                Log.WriteLine(passMessage);
                                            }
                                            else
                                            {
                                                Log.WriteLine("Skipped class Check on TD Cell with no Data");
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            string failureMessage = "Test for Class Attribute on Table : " + TableId + " Failed for Column " + item.ToString() + " Did not have a date-column class";
                                            Log.WriteLine(failureMessage);
                                            Assert.Fail(failureMessage);
                                        }
                                    }
                                }


                            }
                        }
                        else
                        {
                            var failureMessage = ("date-column Class Test Failed - No table found with ID : "  + TableId);
                            Log.WriteLine(failureMessage);
                            Assert.Fail(failureMessage);
                        }
                    }
                }
            }





        }

        //202  -PG 24-10-2019
        [Then(@"The HTML ""([^""]*)"" of the type ""([^""]*)"" Should Contain The date banner Class Attribute")]
        public void ThenTheResponseHTMLShouldContainThedatebannerClassAttribute(string headingsToFind, string headingType)
        {
            //todo maybe add check, we atleast get one section or fail.

            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
                {
                    Composition composition = (Composition)entry.Resource;
                    foreach (Composition.SectionComponent section in composition.Section)
                    {
                        var headerList = headingsToFind.Split(',');
                        var html = section.Text.Div;
                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(html);

                        headerList.ToList().ForEach(headerToFind =>
                        {
                            var headingNode = htmlDoc.DocumentNode.Descendants(headingType).Where(t => t.InnerHtml.Equals(headerToFind)).FirstOrDefault();
                            
                            if (headingNode != null)
                            {
                                var topDivNodes = headingNode.ParentNode.ChildNodes
                                    .Where(c => c.Name == "div")
                                    .ToList();

                                var found = false;
                                
                                if (headingType == "h2")
                                {
                                    topDivNodes.ForEach(i =>
                                    {
                                        var foundAttrib = i.Attributes.Where(a => a.Value == "date-banner");

                                        if (foundAttrib.Count() >= 1)
                                        {
                                            found = true;
                                            Logger.Log.WriteLine("Found Div with Attribute class = date-banner - under Heading : " + headerToFind);
                                        }
                                    });
                                }
                                //Else h1 - date-banner is nested in a div in a div unlike h2
                                else
                                {
                                    var lowerDivNodes = topDivNodes.First().ChildNodes
                                                        .Where(c => c.Name == "div")
                                                        .ToList();

                                    lowerDivNodes.ForEach(i =>
                                    {
                                        var foundAttrib = i.Attributes.Where(a => a.Value == "date-banner");

                                        if (foundAttrib.Count() >= 1)
                                        {
                                            found = true;
                                            Logger.Log.WriteLine("Found Div with Attribute class = date-banner - under Heading : " + headerToFind);
                                        }
                                    });

                                }

                                if (!found)
                                    Assert.Fail("No date-banner class attribute found for heading : " + headerToFind + " of Type : " + headingType);
                            }
                            else
                            {
                                Assert.Fail("Heading Type: " + headingType + " - Heading Name : " + headerToFind + " - Not Found, so unable to check for date-banner class attribute");

                            }
                        });
                
                    }
                }
            }
        }

        //202  -PG 24-10-2019
        [Then(@"The Grouped Sections Exist in Table ""(.*)""")]
        public void AndTheGroupedSectionsExistinTable(string tablesToCheck)
        {

            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
                {
                    Composition composition = (Composition)entry.Resource;
                    foreach (Composition.SectionComponent section in composition.Section)
                    {
                        var tablesList = tablesToCheck.Split(',');
                        var html = section.Text.Div;
                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(html);


                        tablesList.ToList().ForEach(tableToCheck =>
                        {

                            var table = htmlDoc.DocumentNode.Descendants("table").Where(t => t.Id.Equals(tableToCheck)).FirstOrDefault();

                            if (table != null)
                            {
                                //String groupItemFound
                                foreach (HtmlNode row in table.SelectNodes("./tbody//tr"))
                                {
                                    var tdNodes = row.SelectNodes(".//td");

                                    //If to ignore Grouped by row
                                    if (tdNodes.ToArray().Count() != 1)
                                    {

                                    }

                                }
                            }
                            else
                            {
                                var failureMessage = ("Unable to Check Grouping Exists Becuase Table Not Found : " + tableToCheck);
                                Log.WriteLine(failureMessage);
                                Assert.Fail(failureMessage);
                            }

                        });

                    }
                }
            }
        }


        //202  -PG 25-10-2019
        [Then(@"I Check All Medication Issues are summarised correctly in All Medications")]
        public void ICheckAllMedicationIssuesaresummarisedcorrectlyinAllMedications()
        {

            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
                {
                    Composition composition = (Composition)entry.Resource;
                    foreach (Composition.SectionComponent section in composition.Section)
                    {
                        //var tablesList = tablesToCheck.Split(',');
                        var html = section.Text.Div;
                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(html);

                        var AllMedsTableID = "med-tab-all-sum";
                        var AllMedIssuesTableID = "med-tab-all-iss";
                        
                        var allMedIssuesstable = htmlDoc.DocumentNode.Descendants("table").Where(t => t.Id.Equals(AllMedIssuesTableID)).FirstOrDefault();

                        List<AllMedicationIssues> readAllMedIssues = new List<AllMedicationIssues>();

                        if (allMedIssuesstable != null)
                        { 
                            foreach (HtmlNode row in allMedIssuesstable.SelectNodes("./tbody//tr"))
                            {
                                var tdNodes = row.SelectNodes(".//td");
                                if (tdNodes.ToArray().Count() != 1)
                                {

                                    AllMedicationIssues newAllMedIssuesrecord = new AllMedicationIssues();

                                    newAllMedIssuesrecord.Type = tdNodes[0].InnerHtml;
                                    newAllMedIssuesrecord.IssueDate = tdNodes[1].InnerHtml;
                                    newAllMedIssuesrecord.MedicationItem = tdNodes[2].InnerHtml;
                                    newAllMedIssuesrecord.DosageInstruction = tdNodes[3].InnerHtml;
                                    newAllMedIssuesrecord.Quantity = tdNodes[4].InnerHtml;
                                    newAllMedIssuesrecord.DaysDuration = tdNodes[5].InnerHtml;
                                    newAllMedIssuesrecord.AdditionalInformation = tdNodes[6].InnerHtml;

                                    readAllMedIssues.Add(newAllMedIssuesrecord);
                                }
                            }
                        }
                        else
                        {
                            var failureMessage = ("Unable to Check All Issues as Table Not Found : " + allMedIssuesstable);
                            Log.WriteLine(failureMessage);
                            Assert.Fail(failureMessage);
                        }

                        var MedItemGroupingQuery = from medIssue in readAllMedIssues
                                                    group medIssue by new { medIssue.Type, medIssue.MedicationItem, medIssue.DosageInstruction, medIssue.Quantity, medIssue.DaysDuration, medIssue.AdditionalInformation } into g
                                                    orderby g.Key.MedicationItem
                                                    select new { Type = g.Key.Type, StartDate = g.Min(x => Convert.ToDateTime(x.IssueDate)),MedicationItem = g.Key.MedicationItem, DosageInstruction = g.Key.DosageInstruction,Quantity = g.Key.Quantity ,LastIssued = g.Max(x => Convert.ToDateTime(x.IssueDate)),AdditionalInfo = g.Key.AdditionalInformation, NumberOfPrescriptionsIssued = g.Count()};


                        var allMedstable = htmlDoc.DocumentNode.Descendants("table").Where(t => t.Id.Equals(AllMedsTableID)).FirstOrDefault();

                        //Compare grouped results from "All Medication Issues" with "All Medication" Table


                        CheckAllMedsTableMatchesGroupedResultsFromAllMedIssues(allMedstable, MedItemGroupingQuery);

                        //todo convert to using for loop and pass index into function so we can check position in table at same time
                        
                        //for (int i = 0; i < MedItemGroupingQuery.Count()-1; i++)
                        //{
                        //    if (CheckTableHasARowThatMatches(allMedstable, MedItemGroupingQuery.ToArray()[i],i))
                        //    {
                        //        var message = ("Matched Table Row.\n Looking For : " + MedItemGroupingQuery.ToArray()[i].ToString());
                        //        Log.WriteLine(message);
                        //    }
                        //    else
                        //    {
                        //        Assert.Fail("Failed to Find Row in All Medication Table That matched the Grouped Entry From the \"All Medication Issues Table\".\n Could not Find : " + MedItemGroupingQuery.ToArray()[i].ToString());
                        //    }


                        //}

                        //MedItemGroupingQuery.ToList().ForEach(item =>
                        //{
                        //    if (CheckTableHasARowThatMatches(allMedstable, item))
                        //    {
                        //        var message = ("Matched Table Row.\n Looking For : " + item.ToString());
                        //        Log.WriteLine(message);
                        //    }
                        //    else
                        //    {
                        //        Assert.Fail("Failed to Find Row in All Medication Table That matched the Grouped Entry From the \"All Medication Issues Table\".\n Could not Find : " + item.ToString());
                        //    }

                        //});                   

                    }
                }
            }


        }

        //202  -PG 25-10-2019
        
        //public bool CheckTableHasARowThatMatches(HtmlNode htmlNode, dynamic groupedResult,int indexToCheck)
        public bool CheckAllMedsTableMatchesGroupedResultsFromAllMedIssues(HtmlNode htmlNode, IEnumerable<dynamic> groupedResults)
        {
            var foundFlag = false;
            if (htmlNode != null)
            {


                //use a for loop to check the order at the same time.
                var AllRows = htmlNode.SelectNodes("./tbody//tr");
                var filteredRows = AllRows.ToList().Where(r => r.SelectNodes(".//td").Count() != 1);

                //to check order loop Grouped Antries We Expect
                for (int i = 0; i < groupedResults.Count() - 1; i++)
                {

                    //Loop Rows in All Meds to See if they match outter loop
                    for (int r = 0; r < filteredRows.Count() - 1; r++)
                    {
                        var tdNodes = filteredRows.ToArray()[r].SelectNodes(".//td");

                        if ( groupedResults.ToArray()[i].Type == tdNodes[0].InnerHtml
                            && groupedResults.ToArray()[i].StartDate == tdNodes[1].InnerHtml

                            )
                        {

                        }
                        else
                        {
                            Assert.Fail(failureMessage);
                        }

                    }


                }



               

                foreach (HtmlNode row in htmlNode.SelectNodes("./tbody//tr"))
                {
                    var tdNodes = row.SelectNodes(".//td");

                    //Ignore Grouping Rows
                    if (tdNodes.ToArray().Count() != 1)
                    {
                        //Loop Each Node and see if any row matches
                        if (tdNodes[0].InnerHtml == groupedResult.Type 
                            && Convert.ToDateTime(tdNodes[1].InnerHtml) == groupedResult.StartDate
                            && tdNodes[2].InnerHtml == groupedResult.MedicationItem
                            && tdNodes[3].InnerHtml == groupedResult.DosageInstruction
                            && tdNodes[4].InnerHtml == groupedResult.Quantity
                            && Convert.ToDateTime(tdNodes[5].InnerHtml) == groupedResult.LastIssued
                            && Convert.ToInt32(tdNodes[6].InnerHtml) == groupedResult.NumberOfPrescriptionsIssued
                            && tdNodes[8].InnerHtml == groupedResult.AdditionalInfo
                            )
                        {
                            foundFlag = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                var failureMessage = ("Unable to Check ALL Medication Data as Table Not Found" );
                Log.WriteLine(failureMessage);
                Assert.Fail(failureMessage);
            }


            if (foundFlag)
                return true;
            else
                return false;
        }

    }
}

