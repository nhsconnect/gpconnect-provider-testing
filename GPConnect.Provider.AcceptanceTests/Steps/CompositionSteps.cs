namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Linq;
    using Context;
    using Hl7.Fhir.Model;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    public class CompositionSteps : BaseSteps
    {
        public CompositionSteps(FhirContext fhirContext, HttpSteps httpSteps) 
            : base(fhirContext, httpSteps)
        {
        }

        [Then(@"the composition is valid for ""([^""]*)"", ""([^""]*)"", ""([^""]*)""")]
        public void TheCompositionIsValidFor(string title, string code, string display)
        {
            _fhirContext.Compositions.ForEach(composition =>
            {
                composition.Date.ShouldNotBeNull();
                composition.Title.ShouldBe("Patient Care Record");
                composition.Status.ShouldBe(Composition.CompositionStatus.Final);
                composition.Section[0].Title.ShouldBe(title);
                composition.Section[0].Code.Coding[0].System.ShouldBe("http://fhir.nhs.net/ValueSet/gpconnect-record-section-1");
                composition.Section[0].Code.Coding[0].Code.ShouldBe(code);
                composition.Section[0].Code.Coding[0].Display.ShouldBe(display);
                composition.Section[0].Code.Text.ShouldNotBeNull();
                composition.Section[0].Text.Status.ShouldNotBeNull();
                composition.Section[0].Text.Div.ShouldNotBeNull();
            });
        }

        [Then(@"the HTML in the response matches the Regex check ""([^""]*)""")]
        public void ThenTheHtmlInTheResponseMatchesTheRegexCheck(string regexPattern)
        {
            _fhirContext.Compositions.ForEach(composition =>
            {
                composition.Section.Count.ShouldBe(1);
                composition.Section.ForEach(section =>
                {
                    section.Text.Div.ShouldMatch(regexPattern);
                });
            });
        }

        [Then(@"the composition resource in the bundle should contain meta data profile")]
        public void ThenTheCompositionResourceInTheBundleShouldContainMetaDataProfile()
        {
            _fhirContext.Compositions.ForEach(composition =>
            {
                composition.Meta.ShouldNotBeNull();
                composition.Meta.Profile.ToList().ForEach(profile =>
                {
                    profile.ShouldBe("http://fhir.nhs.net/StructureDefinition/gpconnect-carerecord-composition-1");
                });
            });
        }

        [Then(@"if composition contains the resource type element the fields should match the fixed values of the specification")]
        public void ThenIfCompositionContainsTheResourceTypeElementTheFieldsShouldMatchTheFixedValuesOfTheSpecification()
        {
            _fhirContext.Compositions.ForEach(composition =>
            {
                if (composition.Type != null)
                {
                    if (composition.Type.Coding != null)
                    {
                        composition.Type.Coding.Count.ShouldBeLessThanOrEqualTo(1);
                        composition.Type.Coding.ForEach(coding =>
                        {
                            coding.System.ShouldBe("http://snomed.info/sct");
                            coding.Code.ShouldBe("425173008");
                            coding.Display.ShouldBe("record extract (record artifact)");
                        });
                    }

                    composition.Type.Text?.ShouldBe("record extract (record artifact)");
                }
            });
        }

        [Then(@"if composition contains the resource class element the fields should match the fixed values of the specification")]
        public void ThenIfCompositionContainsTheResourceClassElementTheFieldsShouldMatchTheFixedValuesOfTheSpecification()
        {
            _fhirContext.Compositions.ForEach(composition =>
            {
                if (composition.Class != null)
                {
                    if (composition.Class.Coding != null)
                    {
                        composition.Class.Coding.Count.ShouldBeLessThanOrEqualTo(1);
                        composition.Class.Coding.ForEach(coding =>
                        {
                            coding.System.ShouldBe("http://snomed.info/sct");
                            coding.Code.ShouldBe("700232004");
                            coding.Display.ShouldBe("general medical service (qualifier value)");
                        });
                    }

                    composition.Class.Text?.ShouldBe("general medical service (qualifier value)");
                }
            });
        }
    }
}
