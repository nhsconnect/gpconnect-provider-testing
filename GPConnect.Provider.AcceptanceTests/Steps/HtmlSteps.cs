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

		[Then(@"the html should contain sub section headers in coma seperated list ""([^""]*)""")]
		public void ThenTheHTMLShouldContainSubSectionHeadersInComaSeperatedList(string listOfHeaders)
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
						foreach (string header in headers)
						{
							html.ShouldContain("<h2>"+header+"</h2>",Case.Sensitive);
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

						}
                        else
                        {
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

		// #317 SJD 26/11/2019 text change
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
						string expectedTimePeriodBanner = "<p>Data items from '" + fromDate +"'</p>";
						html.ShouldContain(expectedTimePeriodBanner, Case.Insensitive);
					}
				}
			}
		}

		// #317 SJD 26/11/2019 text change
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
						string expectedTimePeriodBanner = "<p>Data items until '" + toDate + "'</p>";
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

		//SJD 24/07/2019 added else if .. <h1> to cover single table views 
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
							if (sectionHtml.Contains(sectionHeading + "</h2>"))
							{
								sectionFound = true;
								sectionHtml.ShouldContain("<p>No '" + sectionHeading + "' data is recorded for this patient.</p>");

							}
							else if (sectionHtml.Contains(sectionHeading + "</h1>"))
							{
								sectionFound = true;
								sectionHtml.ShouldContain("<p>No '" + sectionHeading + "' data is recorded for this patient.</p>");
							}
						}
					}
					sectionFound.ShouldBeTrue();
				}
			}
		}

		//#195 SJD 24/07/2019 Not applied date ranges banner
		[Then(@"the response html should contain the not applied date ranges banner for ""([^""]*)""")]
		public void ThenTheResponseHTMLShouldContainTheNotAppliedDateRangeBannerFor(string subSectionTitle)
		{
			foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
			{
				if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
				{
					Composition composition = (Composition)entry.Resource;
					foreach (Composition.SectionComponent section in composition.Section)
					{
						Log.WriteLine("The Date filter not displayed for the expected subsection");
						var html = section.Text.Div;
						string expectedDateBanner = "<h2>" + subSectionTitle + "</h2><div class=\"date-banner\"><p>Date filter not applied</p>";
						html.ShouldContain(expectedDateBanner, Case.Sensitive);
					}
				}
			}
		}

		//#195 SJD 24//2019 section titles <h1> tag 
		[Then(@"the html should contain section headers in coma seperated list ""([^""]*)""")]
		public void ThenTheHTMLShouldContainSectionHeadersInComaSeperatedList(string listOfHeaders)
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
						foreach (string header in headers)
						{
							html.ShouldContain("<h1>" + header + "</h1>", Case.Sensitive);
						}
					}
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


                                topDivNodes.ForEach(i =>
                                    {
                                        var foundAttrib = i.Attributes.Where(a => a.Value == "date-banner");

                                        if (foundAttrib.Count() >= 1)
                                        {
                                            found = true;
                                            Logger.Log.WriteLine("Found Div with Attribute class = date-banner - under Heading : " + headerToFind);
                                        }
                                    });

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

        //202  -PG 25-10-2019
        [Then(@"I Check All Medication Issues are summarised correctly in All Medications")]
        public void ICheckAllMedicationIssuesaresummarisedcorrectlyinAllMedications()
        {
            bool foundValidHTMLFlag = false;

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

                        var AllMedsTableID = "med-tab-all-sum";
                        var AllMedIssuesTableID = "med-tab-all-iss";

                        var allMedIssuesstable = htmlDoc.DocumentNode.Descendants("table").Where(t => t.Id.Equals(AllMedIssuesTableID)).FirstOrDefault();

                        //Scrape Data From All Med Issues Table
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

                            //Split data into groups expected to be seen in All Medication Table
                            var MedItemGroupingQuery = from medIssue in readAllMedIssues
                                                    group medIssue by new { medIssue.Type, medIssue.MedicationItem, medIssue.DosageInstruction, medIssue.Quantity, medIssue.DaysDuration, medIssue.AdditionalInformation } into g
                                                    orderby g.Key.MedicationItem
                                                    select new { Type = g.Key.Type, StartDate = g.Min(x => Convert.ToDateTime(x.IssueDate)),MedicationItem = g.Key.MedicationItem, DosageInstruction = g.Key.DosageInstruction,Quantity = g.Key.Quantity ,LastIssued = g.Max(x => Convert.ToDateTime(x.IssueDate)),AdditionalInfo = g.Key.AdditionalInformation, NumberOfPrescriptionsIssued = g.Count()};

                        //Check Data requirements have been Met

                        //Check That All Medication has two or more uniqe medications
                        if (readAllMedIssues.Count() <= 1)
                        {
                            //Should be more than one row in the All Medication Table as Per Data requirements
                            var failureMessage = "Test data requirements Not Met - Need atleast 2 Unique Medications in the All Medication Table";
                            Logger.Log.WriteLine(failureMessage);
                            Assert.Fail(failureMessage);

                        }

                        //Check Have atleast One Summaried Item, with a Number Of Prescriptions Issued of 2 or more
                        if (MedItemGroupingQuery.Where(medItem => (Convert.ToInt32(medItem.NumberOfPrescriptionsIssued)) >= 2).Count() == 0)
                        {
                            //No medications with a quantity of 2 or more found
                            var failureMessage = "Test data requirements Not Met - Need atleast 4 Unique Medication issues and atleast one medication that has been issued more than twice";
                            Logger.Log.WriteLine(failureMessage);
                            Assert.Fail(failureMessage);
                        }



                        //Compare grouped results from "All Medication Issues" with "All Medication" Table

                        //CheckAllMedsTableMatchesGroupedResultsFromAllMedIssues(allMedstable, MedItemGroupingQuery);
                        var allMedstable = htmlDoc.DocumentNode.Descendants("table").Where(t => t.Id.Equals(AllMedsTableID)).FirstOrDefault();
                        if (allMedstable != null)
                        {

                            var AllRows = allMedstable.SelectNodes("./tbody//tr");
                            var AllMedsFilteredRows = AllRows.ToList().Where(r => r.SelectNodes(".//td").Count() != 1);

                            //Check we have the correct number of rows (excluding Grouping Rows)
                            AllMedsFilteredRows.Count().ShouldBe(MedItemGroupingQuery.Count(), "Number of Medication Items does not match what is Expected");

                            //Check Order of summaried Items and each of the items fields matche the expected
                            for (int i = 0; i < MedItemGroupingQuery.Count(); i++)
                            {
                                var tdNodes = AllMedsFilteredRows.ToArray()[i].SelectNodes(".//td");

                                if (tdNodes[0].InnerHtml == MedItemGroupingQuery.ToArray()[i].Type
                                  && Convert.ToDateTime(tdNodes[1].InnerHtml) == MedItemGroupingQuery.ToArray()[i].StartDate
                                  && tdNodes[2].InnerHtml == MedItemGroupingQuery.ToArray()[i].MedicationItem
                                  && tdNodes[3].InnerHtml == MedItemGroupingQuery.ToArray()[i].DosageInstruction
                                  && tdNodes[4].InnerHtml == MedItemGroupingQuery.ToArray()[i].Quantity
                                  && Convert.ToDateTime(tdNodes[5].InnerHtml) == MedItemGroupingQuery.ToArray()[i].LastIssued
                                  && Convert.ToInt32(tdNodes[6].InnerHtml) == MedItemGroupingQuery.ToArray()[i].NumberOfPrescriptionsIssued
                                  && tdNodes[8].InnerHtml == MedItemGroupingQuery.ToArray()[i].AdditionalInfo
                                 )
                                {
                                    Logger.Log.WriteLine("All Medication row Verified for Entry : " + AllMedsFilteredRows.ToArray()[i].InnerHtml.ToString());
                                    foundValidHTMLFlag = true;
                                }
                                else
                                {
                                    var failureMessage = ("Failed to Compare All Medications Row with Expected\n Expected : " + MedItemGroupingQuery.ToArray()[i].ToString() + "\nActual : " + AllMedsFilteredRows.ToArray()[i].InnerHtml.ToString());
                                    Logger.Log.WriteLine(failureMessage);
                                    Assert.Fail(failureMessage);
                                }
                            }
                        }
                        else
                        {
                            var failureMessage = ("Unable to Find Table :" + AllMedsTableID);
                            Log.WriteLine(failureMessage);
                            Assert.Fail(failureMessage);
                        }
                    }
                }

            }

            if (!foundValidHTMLFlag)
                Assert.Fail("Test Step Failed as No Valid HTML wasd Found or Tested");

        }

        //202  -PG 26-10-2019 - Function to check that the grouping html is correct and has class 
        [Then(@"The Grouped Sections Are Valid And Have Class Attributes")]
        public void AndTheGroupedSectionsAreValidAndHaveClassAttributes()
        {

            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Composition))
                {
                    Composition composition = (Composition)entry.Resource;
                    foreach (Composition.SectionComponent section in composition.Section)
                    {

                        string tablesToCheck = "med-tab-all-sum,med-tab-all-iss";
                        var tablesList = tablesToCheck.Split(',');
                        var html = section.Text.Div;
                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(html);

                        tablesList.ToList().ForEach(tableToCheck =>
                        {

                            var table = htmlDoc.DocumentNode.Descendants("table").Where(t => t.Id.Equals(tableToCheck)).FirstOrDefault();

                            if (table != null)
                            {
                                string currentGrouping = "";
                                int countDataRowsFound = 0;

                                foreach (HtmlNode row in table.SelectNodes("./tbody//tr"))
                                {
                                    var tdNodes = row.SelectNodes(".//td");

                                    //If Grouped By Row
                                    if (tdNodes.ToArray().Count() == 1)
                                    {
                                        //get Grouping Text
                                        currentGrouping = tdNodes[0].InnerText;
                                        countDataRowsFound = 0;

                                        //Check grouping Row has Class Attribute med-item-column
                                        tdNodes[0].Attributes["class"].Value.Equals("med-item-column").ShouldBeTrue("Failed to Find med-item-column Class on Grouping Row In Table");
                                        Logger.Log.WriteLine("Table : " + tableToCheck + " - Found med-item-column Class Attribute for Grouping : " + currentGrouping);

                                    }
                                    //Else Data Row
                                    else
                                    {
                                        if (tdNodes[2].InnerHtml == currentGrouping)
                                        {
                                            countDataRowsFound++;
                                            string message = "Table : " + tableToCheck + " - Grouping : " + currentGrouping +  " Has Matched Medical Item\n\tExpected : " + currentGrouping + "\n\tMedical item " + countDataRowsFound + " - Actual : " + tdNodes[2].InnerHtml;
                                            Logger.Log.WriteLine(message);
                                        }
                                        else
                                        {
                                            string failureMessage = "Table : " + tableToCheck + " - Medical Item Does Not Match Grouping : " + currentGrouping + "\n\tExpected : " + currentGrouping + "\n\tMedical Item " + countDataRowsFound + " - Actual : " + tdNodes[2].InnerHtml;
                                            Logger.Log.WriteLine(failureMessage);
                                            Assert.Fail(failureMessage);
                                        }
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

        //202  -PG 28-10-2019
        [Then(@"The GP Transfer Banner is Present Below Heading ""(.*)""")]
        public void ThenTheGPTransferBannerisPresentBelowHeading(string headingToFind)
        {
            bool foundBannerFlag = false;

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

                        var headingNode = htmlDoc.DocumentNode.Descendants("h1").Where(t => t.InnerHtml.Equals(headingToFind)).FirstOrDefault();

                        if (headingNode != null)
                        {
                            var topDivNodes = headingNode.ParentNode.ChildNodes
                                .Where(c => c.Name == "div")
                                .ToList();

                            topDivNodes.ForEach(node =>
                            {
                                var foundAttrib = node.Attributes.Where(a => a.Value == "gptransfer-banner");

                                if (foundAttrib.Count() >= 1)
                                {
                                    Logger.Log.WriteLine("Found Div with Attribute class = gptransfer-banner - under Heading : " + headingToFind);

                                    //Checking That GP Transfer Text included a date in format dd-Mmm-yyyy
                                    Regex regex = new Regex(@"(([0-9])|([0-2][0-9])|([3][0-1]))\-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{4}");
                                    Match matches = regex.Match(node.InnerText);

                                    if (matches.Success)
                                    {
                                        DateTime dt;
                                        if (DateTime.TryParse(matches.Value, out dt) == true)
                                        {
                                            Logger.Log.WriteLine("Found Date Time in Correct Format in GP Tranfer banner Text : " + node.InnerText);
                                            foundBannerFlag = true;
                                        }
                                        else
                                        {
                                            var failureMessage = "Failed to Find a Date Format in the GP transfer Banner Text : " + node.InnerText;
                                            Logger.Log.WriteLine(failureMessage);
                                            Assert.Fail(failureMessage);
                                        }

                                    }
                                    else
                                    { 
                                        var failMessage = "Failed to Find a string with a Date Contained in as per spec";
                                        Logger.Log.WriteLine(failMessage);
                                        Assert.Fail(failMessage);    
                                    }
                                }
                            });

                        }
                        else
                        {
                            Assert.Fail("Heading Type: " + "h1" + " - Heading Name : " + headingToFind + " - Not Found, so unable to check for gptransfer-banner class attribute");
                        }
                    }
                }
            }

            if (!foundBannerFlag)
                Assert.Fail("No gptransfer-banner class attribute found for heading : " + headingToFind + " of Type : " + "h1");
        }

		//issue 195 SJD 24/10/19 
		[Then(@"The Response Html Should Contain The Discontinued Repeat Medication Banner Text")]
		public void ThenTheResponseHTMLShouldContainTheDiscontinuedRepeatMedicationBannerText()
		{
			var foundTextFlag = false;
			var foundClassAttribFlag = false;

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

						var headingNode = htmlDoc.DocumentNode.Descendants("h2").Where(t => t.InnerHtml.Equals("Discontinued Repeat Medication")).FirstOrDefault();

						if (headingNode != null)
						{
							var topDivNodes = headingNode.ParentNode.ChildNodes
								.Where(c => c.Name == "div")
								.ToList();

							topDivNodes.ForEach(node =>
							{
								var foundAttrib = node.Attributes.Where(a => a.Value == "content-banner");

								if (foundAttrib.Count() >= 1)
								{
									Logger.Log.WriteLine("Found Div with Attribute class = content-banner - under Heading : Discontinued Repeat Medication");
									foundClassAttribFlag = true;
								}

							});
						}
						else
						{
							Assert.Fail("Heading : Discontinued Repeat Medication Not Found, so unable to check for content-banner class attribute");

						}

						if (!foundClassAttribFlag)
						{
							Assert.Fail("Expected Content Banner Not Found");
						}


						//var html = section.Text.Div;
						string expectedDiscontinuedRepeatBanner = "<p>All repeat medication ended by a clinician action</p>";
						html.ShouldContain(expectedDiscontinuedRepeatBanner, Case.Insensitive);
						foundTextFlag = true;
					}
					
				}
			}

			if (!foundTextFlag)
			{
				Assert.Fail("Expected Discontinued Repeat Banner not returned");
			}

		}

		//SJD 26-11-2019
		[Then(@"The HTML ""([^""]*)"" of the type ""([^""]*)"" Should Not Contain The date banner Class Attribute")]
		public void ThenTheResponseHTMLShouldNotContainThedatebannerClassAttribute(string headingsToFind, string headingType)
		{
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


								topDivNodes.ForEach(i =>
								{
									var foundAttrib = i.Attributes.Where(a => a.Value == "date-banner");

									if (foundAttrib.Count() >= 1)
									{
										found = true;
										Assert.Fail("Unexpected date-banner class attribute found for heading : " + headerToFind + " of Type : " + headingType); ;
									}
								});

								if (!found)
									
								Logger.Log.WriteLine("No Attribute class = date-banner returned - under Heading : " + headerToFind);
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
		
	}
}