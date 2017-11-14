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
        private readonly BundleSteps _bundleSteps;
        private Composition _composition => _httpContext.FhirResponse.Compositions.First();

        public CompositionSteps(HttpSteps httpSteps, HttpContext httpContext, BundleSteps bundleSteps) 
            : base(httpSteps)
        {
            _httpContext = httpContext;
            _bundleSteps = bundleSteps;
        }

        [Then("the Composition should be valid")]
        public void TheCompositionShouldBeValid()
        {
            _composition.Date.ShouldNotBeNull();

            TheCompositionTypeShouldBeValid();

            if (_composition.Class != null)
            {
               TheCompositionClassShouldBeValid();
            }

            _composition.Title.ShouldBe("Patient Care Record");
            _composition.Status.ShouldBe(CompositionStatus.Final);

            _composition.Section.Count.ShouldBe(1);
        }

        [Then("the Composition Class should be valid")]
        private void TheCompositionClassShouldBeValid()
        {
            var compositionClass = _composition.Class;

            if (compositionClass.Coding != null)
            {
                compositionClass.Coding.Count.ShouldBeLessThanOrEqualTo(1);
                compositionClass.Coding.ForEach(coding =>
                {
                   // coding.System.ShouldBe("http://snomed.info/sct");
                    coding.Code.ShouldBe("700232004");
                    coding.Display.ShouldBe("general medical service (qualifier value)");
                });
            }

            compositionClass.Text?.ShouldBe("general medical service (qualifier value)");
        }

        [Then("the Composition Type should be valid")]
        public void TheCompositionTypeShouldBeValid()
        {
            var compositionType = _composition.Type;

            compositionType.ShouldNotBeNull();
            compositionType.Text?.ShouldBe("record extract (record artifact)");
            if (compositionType.Coding != null)
            {
                var coding = compositionType.Coding.First();

                //coding.System.ShouldBe("http://snomed.info/sct");
                coding.Code.ShouldBe("425173008");
                coding.Display.ShouldBe("record extract (record artifact)");
            }
        }

        [Then(@"the Composition Section should be valid for ""([^""]*)"", ""([^""]*)"", ""([^""]*)""")]
        public void TheCompositionSectionShouldBeValidFor(string title, string code, string display)
        {
            var section = _composition.Section.First();

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
            CheckForValidMetaDataInResource(_composition, "http://fhir.nhs.net/StructureDefinition/gpconnect-carerecord-composition-1");
        }

        [Then("the Composition Subject should be referenced in the Bundle")]
        public void TheCompositionSubjectShouldReferencedInTheBundle()
        {
            var subject = _composition.Subject;

            if (subject != null)
            {
                subject.Reference.ShouldNotBeNull();
                _bundleSteps.ResponseBundleContainsReferenceOfType(subject.Reference, ResourceType.Patient);
            }
        }

        [Then("the Composition Author should be referenced in the Bundle")]
        public void TheCompositionAuthorShouldReferencedInTheBundle()
        {
            var author = _composition.Author?[0];

            if (author != null)
            {
                author.Reference.ShouldNotBeNull();

                _bundleSteps.ResponseBundleContainsReferenceOfType(author.Reference, ResourceType.Practitioner);
            }
        }

        [Then("the Composition Custodian should be referenced in the Bundle")]
        public void TheCompositionCustodianShouldReferencedInTheBundle()
        {
            var custodian = _composition.Custodian;

            if (custodian != null)
            {
                custodian.Reference.ShouldNotBeNull();

                _bundleSteps.ResponseBundleContainsReferenceOfType(custodian.Reference, ResourceType.Organization);
            }
        }
    }
}
