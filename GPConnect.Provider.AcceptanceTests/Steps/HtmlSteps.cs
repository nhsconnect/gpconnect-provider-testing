using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Model;
using Shouldly;
using System;
using System.Text.RegularExpressions;
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

    }
}
