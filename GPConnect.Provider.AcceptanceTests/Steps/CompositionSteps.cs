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

        private Composition GetComposition()
        {
            return _fhirContext.Compositions.First();
        }

        [Then("the Composition is valid")]
        public void TheCompositionIsValid()
        {
            var composition = GetComposition();

            composition.Date.ShouldNotBeNull();

            TheCompositionTypeIsValid();

            if (composition.Class != null)
            {
               TheCompositionClassIsValid();
            }

            composition.Title.ShouldBe("Patient Care Record");
            composition.Status.ShouldBe(Composition.CompositionStatus.Final);

            composition.Section.Count.ShouldBe(1);
        }

        [Then("the Composition Class is valid")]
        private void TheCompositionClassIsValid()
        {
            var compositionClass = GetComposition().Class;

            if (compositionClass.Coding != null)
            {
                compositionClass.Coding.Count.ShouldBeLessThanOrEqualTo(1);
                compositionClass.Coding.ForEach(coding =>
                {
                    coding.System.ShouldBe("http://snomed.info/sct");
                    coding.Code.ShouldBe("700232004");
                    coding.Display.ShouldBe("general medical service (qualifier value)");
                });
            }

            compositionClass.Text?.ShouldBe("general medical service (qualifier value)");
        }

        [Then("the Composition Type is valid")]
        public void TheCompositionTypeIsValid()
        {
            var compositionType = GetComposition().Type;

            compositionType.ShouldNotBeNull();
            compositionType.Text?.ShouldBe("record extract (record artifact)");
            if (compositionType.Coding != null)
            {
                var coding = compositionType.Coding.First();

                coding.System.ShouldBe("http://snomed.info/sct");
                coding.Code.ShouldBe("425173008");
                coding.Display.ShouldBe("record extract (record artifact)");
            }

        }

        [Then(@"the Composition Section is valid for ""([^""]*)"", ""([^""]*)"", ""([^""]*)""")]
        public void TheCompositionSectionIsValidFor(string title, string code, string display)
        {
            var section = GetComposition().Section.First();

            section.Title.ShouldBe(title);
            section.Code.Coding[0].System.ShouldBe("http://fhir.nhs.net/ValueSet/gpconnect-record-section-1");
            section.Code.Coding[0].Code.ShouldBe(code);
            section.Code.Coding[0].Display.ShouldBe(display);
            section.Code.Text.ShouldNotBeNull();
            section.Text.Status.ShouldNotBeNull();
            section.Text.Div.ShouldNotBeNull();
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
