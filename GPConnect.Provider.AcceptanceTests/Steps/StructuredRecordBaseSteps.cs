using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Shouldly;
using Hl7.Fhir.Model;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Constants;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public sealed class StructuredRecordBaseSteps : BaseSteps
    {

        private readonly HttpContext _httpContext;

        private List<AllergyIntolerance> AllergyIntolerances => _httpContext.FhirResponse.AllergyIntolerances;
        private List<List> Lists => _httpContext.FhirResponse.Lists;
        private Bundle Bundle => _httpContext.FhirResponse.Bundle;

        public StructuredRecordBaseSteps(HttpSteps httpSteps, HttpContext httpContext)
            : base(httpSteps)
        {
            _httpContext = httpContext;
        }

        [Then(@"the Bundle should be valid")]
        public void TheBundleShouldBeValid()
        {
            Bundle.Meta.ShouldNotBeNull();
            CheckForValidMetaDataInResource(Bundle, FhirConst.StructureDefinitionSystems.kGpcStructuredRecordBundle);
            Bundle.Type.HasValue.ShouldBeTrue();
            Bundle.Type.Value.ShouldBe(Bundle.BundleType.Collection);
            Bundle.Entry.ShouldNotBeEmpty();
            Bundle.Entry.ForEach(entry =>
            {
                entry.Resource.ShouldNotBeNull();
            });
            CheckBundleResources();

        }

        private void CheckBundleResources()
        {
            Boolean hasPatient = false;
            Boolean hasOrganization = false;
            Boolean hasPractitioner = false;
            Boolean hasPractitionerRole = false;
            Bundle.GetResources().ToList().ForEach(resource =>
            {
                if (resource.ResourceType.Equals(ResourceType.Patient))
                {
                    hasPatient = true;
                }
                else if (resource.ResourceType.Equals(ResourceType.Organization))
                {
                    hasOrganization = true;
                }
                else if (resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    hasPractitioner = true;
                }
                else if (resource.ResourceType.Equals(ResourceType.PractitionerRole))
                {
                    hasPractitionerRole = true;
                }
            });

            hasPatient.ShouldBe(true);
            hasOrganization.ShouldBe(true);
            hasPractitioner.ShouldBe(true);
            //hasPractitionerRole.ShouldBe(true);
        }

        public static void BaseListParametersAreValid(List list)
        {
            list.Id.ShouldNotBeNull("The list must have an id.");

            list.Status.ShouldNotBeNull("The List status is a mandatory field.");
            list.Status.ShouldBeOfType<List.ListStatus>("Status of allergies list is of wrong type.");
            list.Status.ShouldBe(List.ListStatus.Current, "The list's status must be set to Current.");

            list.Mode.ShouldNotBeNull("The List mode is a mandatory field.");
            list.Mode.ShouldBeOfType<ListMode>("Mode of allergies list is of wrong type.");
            list.Mode.ShouldBe(ListMode.Snapshot, "The list's mode must be set to Snapshot.");

            list.Code.ShouldNotBeNull("The List code is a mandatory field.");

            list.Subject.ShouldNotBeNull("The List subject is a mandatory field.");
            isTheListSubjectValid(list.Subject).ShouldBeTrue();

            list.Title.ShouldNotBeNull("The List title is a mandatory field.");
        }


        private static Boolean isTheListSubjectValid(ResourceReference subject)
        {
            return !(null == subject.Reference && null == subject.Identifier);
        }
    }
}
