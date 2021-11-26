﻿namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using Cache;
    using Cache.ValueSet;
    using Constants;
    using Hl7.Fhir.Model;
    using Models;
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
                ValueSetContainsCodeAndDisplayAndSystem(valueSet, coding);
            });
        }

        public void ShouldBeExactSingleCodingWhichIsInValueSet(ValueSet valueSet, List<Coding> codingList)
        {
            codingList.Count.ShouldBe(1);
            codingList.ForEach(coding =>
            {
                ValueSetContainsCodeAndDisplayAndSystem(valueSet, coding);
            });
        }

        public void ShouldBeSingleCodingWhichIsInCodeList(Coding code, List<GpcCode> codingList)
        {
            var validCode = codingList.FirstOrDefault(f => f.Code.Equals(code.Code) && f.Display.Equals(code.Display) && f.System.Equals(code.System));

            validCode.ShouldNotBeNull("Code, System and display combination does not match required valueset");
        }

        public static void ValueSetContainsCodeAndDisplayAndSystem(ValueSet valueSet, Coding coding)
        {
            valueSet.Expansion.Contains.ShouldContain(component => component.Code.Equals(coding.Code)  
                                                    && component.Display.Equals(coding.Display) 
                                                    && component.System.Equals(coding.System));
        }
// git hub ref 173
// RMB 24/1/19
        public static void ValueSetContainsCode(ValueSet valueSet, Code warningCode)
        {
            valueSet.Expansion.Contains.ShouldContain(component => component.CodeElement.Matches(warningCode));
        }

        public void CheckForValidMetaDataInResource<T>(T resource, string profileId) where T : Resource
        {
                resource.Meta.ShouldNotBeNull();
// removed 1.2.1 RMB 1/10/2018
//              resource.Meta.LastUpdated.ShouldNotBeNull();
                resource.Meta.Profile.Count().ShouldBe(1);
                resource.Meta.Profile.First().ShouldBe(profileId);
            
// Removed 1.2.1 RMB 1/10/2018
//            if (resource.GetType() != typeof(Composition) && resource.GetType() != typeof(Bundle))
//            {
//                resource.Meta.VersionId.ShouldNotBeNull();
//            }
        }

        public void ValidateTelecom(List<ContactPoint> telecoms, string from, bool svRequired = false)
        {
            telecoms.ForEach(teleCom =>
            {
                teleCom.Extension.ForEach(ext => ext.Url.ShouldNotBeNullOrEmpty($"{from} has an invalid extension. Extensions must have a URL element."));
                teleCom.System?.ShouldBeOfType<ContactPoint.ContactPointSystem>($"{from} System is not a valid value within the value set {FhirConst.CodeSystems.kContactPointSystem}");
                if (svRequired)
                {
                    teleCom.System.ShouldNotBeNull($"{from} System is required");
                    teleCom.Value.ShouldNotBeNullOrEmpty($"{from} Value is required");
                }
                teleCom.Use?.ShouldBeOfType<ContactPoint.ContactPointUse>($"{from} Use is not a valid value within the value set {FhirConst.CodeSystems.kNContactPointUse}");
            });
        }

        public void ValidateAddress(Address address, string from)
        {
            if (address != null)
            {
                address.Extension.ForEach(ext => ext.Url.ShouldNotBeNullOrEmpty($"{from} has an invalid extension. Extensions must have a URL element."));
                address.Type?.ShouldBeOfType<Address.AddressType>($"{from} Type is not a valid value within the value set {FhirConst.CodeSystems.kAddressType}");
                address.Use?.ShouldBeOfType<Address.AddressUse>($"{from} Use is not a valid value within the value set {FhirConst.CodeSystems.kAddressUse}");
            }
        }

        protected void ValidateCodeConceptExtension(Extension extension, string vsetUri)
        {
            extension.Value.ShouldNotBeNull();
            extension.Value.ShouldBeOfType<CodeableConcept>();
            var concept = (CodeableConcept)extension.Value;

            var vset = ValueSetCache.Get(vsetUri);
            ShouldBeExactSingleCodingWhichIsInValueSet(vset, concept.Coding);
        }

        public void CheckHealthcareServiceIsValid(Resource healthservice)
        {
            var hs = (HealthcareService)healthservice;
            hs.ShouldNotBeNull();
            hs.Id.ShouldNotBeNull();
            CheckForValidMetaDataInResource(hs, FhirConst.StructureDefinitionSystems.kHealthcareService);
            //check dos idenfier
            var healthcareServiceIdentifiers = hs.Identifier
                   .Where(identifier => identifier.System.Equals(FhirConst.IdentifierSystems.kDosServiceID))
                   .ToList();

            healthcareServiceIdentifiers.Count.ShouldBeGreaterThanOrEqualTo(1, "There should be atleast One DOS service ID associated with healthcare service with ID :  " + hs.Id);

            healthcareServiceIdentifiers.ForEach(hsi =>
            {
                hsi.Value.ShouldNotBeNullOrEmpty("Healthcare Service Identifier Value must have a value. ID : " + hs.Id);
            });

            hs.ProvidedBy.ShouldNotBeNull("Error: Healthcare Service ProvidedBy should be populated with a reference");
            hs.Name.ShouldNotBeNullOrEmpty();


            Logger.Log.WriteLine("INFO : Validated Healthcare service with ID : " + hs.Id);
        }
    }
}
