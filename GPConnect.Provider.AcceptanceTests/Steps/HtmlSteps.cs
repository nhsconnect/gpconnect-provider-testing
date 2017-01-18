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
        public void ThenTheHTMLShouldNotContainTableHeadersInComaSeperatedListOrder(string listOfTableHeadersInOrder, int pageSectionIndex)
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
                            Log.WriteLine("Section which contains the table does not exist.");
                            Assert.Fail();
                        } else {

                            // Checked if the context contains not supported text

                            // Check table headers
                            Regex regexHeaders = new Regex("<h2>([^<]*</h2>)");
                            MatchCollection matchesForTableHeadersInHTML = regexHeaders.Matches(html);

                            Console.WriteLine("Number of <h2> headers in html = " + matchesForTableHeadersInHTML.Count);
                            Console.WriteLine("Number of <h2> headers expected = " + headerList.Length);

                            headerList.Length.ShouldBe(matchesForTableHeadersInHTML.Count);

                            for (int index = 0; index < headerList.Length; index++)
                            {

                            }
                        }
                    }
                }
            }
        }

    }
}
