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
        private readonly HttpContext _httpContext;

        public CompositionSteps(HttpSteps httpSteps, HttpContext httpContext) 
            : base(httpSteps)
        {
            _httpContext = httpContext;
        }

        private Composition GetComposition()
        {
            return _httpContext.FhirResponse.Compositions.First();
        }

        [Then("the Composition should be valid")]
        public void TheCompositionShouldBeValid()
        {
            var composition = GetComposition();

            composition.Date.ShouldNotBeNull();

            TheCompositionTypeShouldBeValid();

            if (composition.Class != null)
            {
               TheCompositionClassShouldBeValid();
            }

            composition.Title.ShouldBe("Patient Care Record");
            composition.Status.ShouldBe(Composition.CompositionStatus.Final);

            composition.Section.Count.ShouldBe(1);
        }

        [Then("the Composition Class should be valid")]
        private void TheCompositionClassShouldBeValid()
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

        [Then("the Composition Type should be valid")]
        public void TheCompositionTypeShouldBeValid()
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

        [Then(@"the Composition Section should be valid for ""([^""]*)"", ""([^""]*)"", ""([^""]*)""")]
        public void TheCompositionSectionShouldBeValidFor(string title, string code, string display)
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

        [Then(@"the Composition Metadata should be valid")]
        public void TheCompositionMetadataShouldBeValid()
        {
            var composition = GetComposition();

            CheckForValidMetaDataInResource(composition, "http://fhir.nhs.net/StructureDefinition/gpconnect-carerecord-composition-1");
        }
    }
}
