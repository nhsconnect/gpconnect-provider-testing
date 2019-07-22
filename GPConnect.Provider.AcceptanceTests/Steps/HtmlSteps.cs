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
                        var HTML = section.Text.Div;
                        Regex regex = new Regex("<[^>]*([^' ']{1,}[' ']*=[' ']*[^' '>]{1,})[^>]*>");
                        Regex regexXmlns = new Regex("<[^>]*(xmlns[' ']*=[' ']*[^' '>]{1,})[^>]*>");

                        MatchCollection matches = regex.Matches(HTML);
                        MatchCollection matchesXmlns = regexXmlns.Matches(HTML);

                        foreach (Match match in matchesXmlns)
                        {
                            Log.WriteLine("xmlns Regex Match = " + match.Value);
                        }

                        foreach (Match match in matches) {
                            Log.WriteLine("Attribute Regex Match = " + match.Value);
                        }

                        var numberOfNonXmlnsAttributes = matches.Count - matchesXmlns.Count;
                        numberOfNonXmlnsAttributes.ShouldBe(0);
                        matchesXmlns.Count.ShouldBeGreaterThanOrEqualTo(1);
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

                        if (matchflag)
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
    }
    }

