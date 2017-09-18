using System;
using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Enum;
using GPConnect.Provider.AcceptanceTests.Extensions;
using GPConnect.Provider.AcceptanceTests.Models;

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
            var validCode = codingList.FirstOrDefault(f => f.Code.Equals(code.Code) && f.Display.Equals(code.Display));

            validCode.ShouldNotBeNull();
        }

        private static void ValueSetContainsCodeAndDisplay(ValueSet valueSet, Coding coding)
        {
            coding.System.ShouldBe(valueSet.CodeSystem.System);

            valueSet.CodeSystem.Concept.ShouldContain(valueSetConcept => valueSetConcept.Code.Equals(coding.Code) && valueSetConcept.Display.Equals(coding.Display));
        }

        public static void ValueSetContainsCode(ValueSet valueSet, Coding coding)
        {
            coding.System.ShouldBe(valueSet.CodeSystem.System);

            valueSet.CodeSystem.Concept.ShouldContain(valueSetConcept => valueSetConcept.Code.Equals(coding.Code));
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

        public void CheckForValidLocalIdentifier(Identifier identifier, Action assignerRead)
        {
            identifier.Extension.ForEach(ext => ext.Url.ShouldNotBeNullOrEmpty("Local Identifier has an invalid extension. Extensions must have a URL element."));
            identifier.Use?.ShouldBeOfType<Identifier.IdentifierUse>(string.Format("Local Identifier use is Invalid. But be from the value set: {0}", FhirConst.ValueSetSystems.kIdentifierUse));

            identifier.System.ShouldNotBeNullOrEmpty("The Identifier System should not be null or empty");
            identifier.Value.ShouldNotBeNull("The included identifier should have a value.");

            var localOrgzType = identifier.Type;

            if (localOrgzType != null)
            {
                localOrgzType.Extension.ForEach(ext => ext.Url.ShouldNotBeNullOrEmpty("Local Identifier Type has an invalid extension. Extensions must have a URL element."));

                var localOrgzTypeValues = GlobalContext.FhirIdentifierTypeValueSet.WithComposeIncludes();
                var localOrgzTypeCodes = localOrgzType.Coding.Where(lzc => lzc.System.Equals(FhirConst.ValueSetSystems.kIdentifierType)).ToList();
                localOrgzTypeCodes.ForEach(lztc =>
                {
                    lztc.Extension.ForEach(ext => ext.Url.ShouldNotBeNullOrEmpty("Local Identifier Type Coding has an invalid extension. Extensions must have a URL element."));
                    ShouldBeSingleCodingWhichIsInCodeList(lztc, localOrgzTypeValues.ToList());
                });
            }

            if (identifier.Assigner != null)
            {
                //assignerRead?.Invoke();

                identifier.Assigner.Extension.ForEach(ext => ext.Url.ShouldNotBeNullOrEmpty("Local Identifier Assigner has an invalid extension. Extensions must have a URL element."));

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

    }
}
