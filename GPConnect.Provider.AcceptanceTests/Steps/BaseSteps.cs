using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Models;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using Context;
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

        public void ShouldBeExactSingleCodingWhichIsInValueSet(ValueSet valueSet, List<Coding> codingList)
        {
            codingList.Count.ShouldBe(1);
            codingList.ForEach(coding =>
            {
                ValueSetContainsCodeAndDisplay(valueSet, coding);
            });
        }

        public void ShouldBeSingleCodingWhichIsInCodeList(Coding code, List<GpcCode> codingList)
        {
            var validCode = codingList.FirstOrDefault(f => f.Code.Equals(code.Code) && f.Display.Equals(code.Display) && f.System.Equals(code.System));

            validCode.ShouldNotBeNull();
        }

        private static void ValueSetContainsCodeAndDisplay(ValueSet valueSet, Coding coding)
        {
            //coding.System.ShouldBe(valueSet.CodeSystem.System);

            //valueSet.CodeSystem.Concept.ShouldContain(valueSetConcept => valueSetConcept.Code.Equals(coding.Code) && valueSetConcept.Display.Equals(coding.Display));
        }

        //public static void ValueSetContainsCode(ValueSet valueSet, Coding coding)
        //{
        //    coding.System.ShouldBe(valueSet.CodeSystem.System);

        //    valueSet.CodeSystem.Concept.ShouldContain(valueSetConcept => valueSetConcept.Code.Equals(coding.Code));
        //}

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

        public void ValidateTelecom(List<ContactPoint> telecoms, string from, bool svRequired = false)
        {
            telecoms.ForEach(teleCom =>
            {
                teleCom.Extension.ForEach(ext => ext.Url.ShouldNotBeNullOrEmpty($"{from} has an invalid extension. Extensions must have a URL element."));
                teleCom.System?.ShouldBeOfType<ContactPoint.ContactPointSystem>($"{from} System is not a valid value within the value set {FhirConst.ValueSetSystems.kContactPointSystem}");
                if (svRequired)
                {
                    teleCom.System.ShouldNotBeNull($"{from} System is required");
                    teleCom.Value.ShouldNotBeNullOrEmpty($"{from} Value is required");
                }
                teleCom.Use?.ShouldBeOfType<ContactPoint.ContactPointUse>($"{from} Use is not a valid value within the value set {FhirConst.ValueSetSystems.kNContactPointUse}");
            });
        }

        public void ValidateAddress(Address address, string from)
        {
            if (address != null)
            {
                address.Extension.ForEach(ext => ext.Url.ShouldNotBeNullOrEmpty($"{from} has an invalid extension. Extensions must have a URL element."));
                address.Type?.ShouldBeOfType<Address.AddressType>($"{from} Type is not a valid value within the value set {FhirConst.ValueSetSystems.kAddressType}");
                address.Use?.ShouldBeOfType<Address.AddressUse>($"{from} Use is not a valid value within the value set {FhirConst.ValueSetSystems.kAddressUse}");
            }
        }

        protected void ValidateCodeConceptExtension(Extension extension, string vsetUri)
        {
            extension.Value.ShouldNotBeNull();
            extension.Value.ShouldBeOfType<CodeableConcept>();
            var concept = (CodeableConcept)extension.Value;

            var vset = GlobalContext.GetExtensibleValueSet(vsetUri);
            ShouldBeExactSingleCodingWhichIsInValueSet(vset, concept.Coding);
        }

    }
}
