namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using Context;
    using Hl7.Fhir.Model;
    using Logger;
    using NUnit.Framework;
    using Shouldly;
    using TechTalk.SpecFlow;
    using static Hl7.Fhir.Model.Bundle;

    [Binding]
    public sealed class HtmlSteps : Steps
    {
        private readonly HttpContext _httpContext;
        private List<Composition> Compositions => _httpContext.FhirResponse.Compositions;

        public HtmlSteps(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        [Then(@"the html should be valid xhtml")]
        public void ThenTheHtmlShouldBeValidXHTML()
        {
            Compositions.ForEach(composition =>
            {
                foreach (var section in composition.Section)
                {
                    var xhtml = section.Text.Div;
                    XDocument doc = null;
                    try
                    {
                        doc = XDocument.Parse(xhtml);
                        doc.ShouldNotBeNull();
                    }
                    catch (Exception e)
                    {
                        Log.WriteLine("Failed to parse div to xhtml");
                        Log.WriteLine(e.StackTrace);
                        doc.ShouldNotBeNull();
                    }
                }
            });
        }

        [Then(@"the html should not contain ""([^""]*)"" tags")]
        public void ThenTheHtmlShouldNotContaintags(string tagName)
        {
            Compositions.ForEach(composition =>
            {
                foreach (Composition.SectionComponent section in composition.Section)
                {
                    Regex regex = new Regex("<" + tagName);
                    regex.Matches(section.Text.Div).Count.ShouldBe(0);
                }
            });

        }

        [Then(@"the html should not contain any attributes")]
        public void ThenTheHtmlShouldNotContainAnyAttributes()
        {
            Compositions.ForEach(composition =>
            {
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

                    foreach (Match match in matches)
                    {
                        Log.WriteLine("Attribute Regex Match = " + match.Value);
                    }

                    var numberOfNonXmlnsAttributes = matches.Count - matchesXmlns.Count;
                    numberOfNonXmlnsAttributes.ShouldBe(0);
                    matchesXmlns.Count.ShouldBeGreaterThanOrEqualTo(1);
                }
            });
        }

        [Then(@"the html should contain headers in coma seperated list ""([^""]*)""")]
        public void ThenTheHTMLShouldNotContainHeadersInComaSeperatedList(string expectedHeaders)
        {
            const string h1 = "h1";
            const string h2 = "h2";

            var headers = expectedHeaders
                .Split(',')
                .ToList();

            var hasSingleHeader = headers.Count == 1;

            Compositions.ForEach(composition =>
            {
                composition.Section.ForEach(section =>
                {
                    var html = section.Text.Div;

                    if (hasSingleHeader)
                    {
                        var headerHtml = $"<{h1}>{headers[0]}</{h1}>";

                        html.ShouldContain(headerHtml, Case.Sensitive, $"The Section HTML should contain the <{h1}> header {headerHtml}, but did not.");
                    }
                    else
                    {
                        html.ShouldContain(h1, Case.Sensitive, $"The Section HTML should contain a <{h1}> header, but did not.");

                        headers.ForEach(header =>
                        {
                            var headerHtml = $"<{h2}>{header}</{h2}>";

                            html.ShouldContain(headerHtml, Case.Sensitive, $"The Section HTML should contain the <{h2}> header {headerHtml}, but did not.");
                        });
                    }
                });
            });
        }

        [Then(@"the html should contain table headers in coma seperated list order ""([^""]*)"" for the ""([^""]*)""")]
        public void ThenTheHTMLShouldNotContainTableHeadersInComaSeperatedListOrder(string listOfTableHeadersInOrder, int pageSectionIndex)
        {
            Compositions.ForEach(composition =>
            {
                foreach (Composition.SectionComponent section in composition.Section)
                {
                    var html = section.Text.Div;
                    var headerList = listOfTableHeadersInOrder.Split(',');
                    Regex regexHeaderSection = new Regex("<thead[\\w\\W]*?thead>");
                    MatchCollection tableHeaderSectionMatches = regexHeaderSection.Matches(html);
                    if (tableHeaderSectionMatches.Count < pageSectionIndex)
                    {
                        Log.WriteLine("The html table that is expected does not exist in the response.");
                        Assert.Fail();
                    }
                    else
                    {
                        string tableHeaderSectionHTML = tableHeaderSectionMatches[pageSectionIndex - 1].Value;
                        Log.WriteLine("HeaderSection = " + tableHeaderSectionHTML);
                        Regex regexHeaders = new Regex("<th>[^<]*</th>");
                        MatchCollection matchesForTableHeadersInHTML = regexHeaders.Matches(tableHeaderSectionHTML);
                        Log.WriteLine("Number of <th> headers in html {0}, expected {1}",
                            matchesForTableHeadersInHTML.Count, headerList.Length);
                        if (headerList.Length != matchesForTableHeadersInHTML.Count)
                        {
                            Log.WriteLine(
                                "The number of table headers in HTML section does not match the required number of headers.");
                            Assert.Fail();
                        }
                        else
                        {
                            for (int index = 0; index < headerList.Length; index++)
                            {
                                Console.WriteLine("Expected Header = {0} and was {1}",
                                    "<th>" + headerList[index] + "</th>", matchesForTableHeadersInHTML[index].Value);
                                (matchesForTableHeadersInHTML[index].Value).ShouldBe("<th>" + headerList[index] +
                                                                                     "</th>");
                            }
                        }
                    }
                }
            });
        }

        [Then(@"the response html should contain the applied date range text ""([^""]*)"" to ""([^""]*)""")]
        public void ThenTheResponseHTMLShouldContainTheAppliedDateRangeTest(string fromDate, string toDate)
        {
            Compositions.ForEach(composition =>
            {
                foreach (Composition.SectionComponent section in composition.Section)
                {
                    var html = section.Text.Div;
                    string expectedTimePeriodBanner = "<p>For the period '" + fromDate + "' to '" + toDate + "'</p>";
                    html.ShouldContain(expectedTimePeriodBanner, Case.Insensitive);
                }
            });
        }

        [Then(@"the response html for ""([^""]*)"" section should contain a table with ""([^""]*)"" rows")]
        public void ThenTheResponseHTMLShouldContainATableWithXRows(string sectionName, int expectedRowQuantity)
        {
            GetSectionTableRows(sectionName).ShouldBe(expectedRowQuantity);
        }

        [Then(@"the response html for ""([^""]*)"" section should contain a table with at least ""([^""]*)"" rows")]
        public void ThenTheResponseHTMLShouldContainATableWithAtLeastXRows(string sectionName, int rowQuantity)
        {
            GetSectionTableRows(sectionName).ShouldBeGreaterThanOrEqualTo(rowQuantity);
        }

        private int GetSectionTableRows(string sectionName)
        {
            foreach (var composition in Compositions)
            {
                foreach (Composition.SectionComponent section in composition.Section)
                {
                    string html = section.Text.Div
                        .Split(new string[] { "<h2>" + sectionName + "</h2>" }, StringSplitOptions.None)[1]
                        .Split(new string[] { "<tbody>" }, StringSplitOptions.None)[1]
                        .Split(new string[] { "</tbody>" }, StringSplitOptions.None)[0];

                    return Regex.Matches(html, "<tr>").Count;
                }
            }

            return -1;
        }

        [Then(@"the response html should contain the all data items text")]
        public void ThenTheResponseHTMLShouldContainTheAllDataItemsText()
        {
            Compositions.ForEach(composition =>
            {
                foreach (Composition.SectionComponent section in composition.Section)
                {
                    var html = section.Text.Div;
                    html.ShouldContain("<p>All relevant items</p>");
                }
            });
        }

        [Then(@"the html should not contain ""([^""]*)""")]
        public void ThenTheHTMLShouldNotContain(string value)
        {
            Compositions.ForEach(composition =>
            {
                foreach (Composition.SectionComponent section in composition.Section)
                {
                    var html = section.Text.Div;
                    html.ShouldNotContain(value);
                }
            });
        }

        [Then(@"the response html should contain the no data available html banner in section ""([^""]*)""")]
        public void ThenTheResponseHTMLShouldContainTheNoDataAvailableHTMLBannerInSection(string sectionHeading)
        {
            Compositions.ForEach(composition =>
            {
                var sectionFound = false;
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
                            sectionHtml.ShouldContain("<p>No '" + sectionHeading + "' data is recorded for this patient.</p>");
                        }
                    }
                }

                sectionFound.ShouldBeTrue();
            });
        }
    }
}
