namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using Hl7.Fhir.Model;
    using Shouldly;
    using TechTalk.SpecFlow;

    public abstract class BaseSteps : Steps
    {
        protected readonly HttpSteps _httpSteps;

        protected BaseSteps(HttpSteps httpSteps)
        {
            _httpSteps = httpSteps;
        }

        public void ShouldBeSingleCodingWhichIsInValueSet(ValueSet valueSet, List<Coding> codingList)
        {
            codingList.Count.ShouldBeLessThanOrEqualTo(1);
            codingList.ForEach(coding =>
            {
                ValueSetContainsCodeAndDisplay(valueSet, coding);
            });
        }

        private static void ValueSetContainsCodeAndDisplay(ValueSet valueSet, Coding coding)
        {
            coding.System.ShouldBe(valueSet.CodeSystem.System);

            valueSet.CodeSystem.Concept.ShouldContain(valueSetConcept => valueSetConcept.Code.Equals(coding.Code) && valueSetConcept.Display.Equals(coding.Display));
        }

        public void CheckForValidMetaDataInResource<T>(T resource, string profileId) where T : Resource
        {
            resource.Meta.ShouldNotBeNull();
            resource.Meta.Profile.Count().ShouldBe(1);
            resource.Meta.Profile.First().ShouldBe(profileId);

            if (resource.GetType() != typeof(Composition) && resource.GetType() != typeof(Bundle))
            {
                resource.Meta.VersionId.ShouldNotBeNull();
            }
        }
    }
}
