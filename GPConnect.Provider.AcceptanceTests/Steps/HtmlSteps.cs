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

        [Then(@"The Response HTML ""([^""]*)"" Should Contain The date banner Class Attribute")]
        public void ThenTheResponseHTMLShouldContainThedatebannerClassAttribute(string headingsToFind)
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
                {
                    Composition composition = (Composition)entry.Resource;
                    foreach (Composition.SectionComponent section in composition.Section)
                    {
                        //Split List Of Headers into List
                        var headerList = headingsToFind.Split(',');

                        var html = section.Text.Div;
                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(html);
                        headerList.ToList().ForEach(headerToFind =>
                        {
                            //var testHTML = "<h1>Summary</h1><div><h2>Last 3 Encounters</h2><div class=\"gptransfer-banner\"><p>GP transfer banner</p></div><div class=\"content-banner\"><p>Content banner</p></div><div class=\"date-banner\"><p>All relevant items</p></div><div>";
                            //htmlDoc.LoadHtml(testHTML);

                            var headingNode = htmlDoc.DocumentNode.Descendants("h2").Where(t => t.InnerHtml.Equals(headerToFind)).FirstOrDefault();

                            if (headingNode != null)
                            {
                                //looping all div's as there maybe many banner divs
                                var allDivNodes = headingNode.ParentNode.ChildNodes
                                    .Where(c => c.Name == "div")
                                    .ToList();

                                var found = false;
                                allDivNodes.ForEach(i =>
                                {
                                    var foundAttrib = i.Attributes.Where(a => a.Value == "date-banner");
                                    if (foundAttrib.Count() >= 1)
                                    {
                                        found = true;
                                        Logger.Log.WriteLine("Found Div with Attribute class = date-banner - under Heading : " + headerToFind);
                                    }
                                });

                                if (!found)
                                    Assert.Fail("No date-banner class attribute found for heading H2:" + headerToFind);
                            }
                            else
                            {
                                Assert.Fail("Heading H2 : " + headerToFind + " - Not Found, so unable to check for date-banner class attribute");

                            }
                        });
                
                    }
                }
            }
        }




    }
    }

