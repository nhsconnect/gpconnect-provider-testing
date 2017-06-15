using System.Collections.Generic;
using System.Linq;
using GPConnect.Provider.AcceptanceTests.Context;
using Hl7.Fhir.Model;
using NUnit.Framework;
using Shouldly;
using TechTalk.SpecFlow;
using static Hl7.Fhir.Model.Bundle;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public sealed class BundleSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext _fhirContext;

        public BundleSteps(FhirContext fhirContext)
        {
            _fhirContext = fhirContext;
        }

        [Then(@"the response should be a Bundle resource of type ""([^""]*)""")]
        public void ThenTheResponseShouldBeABundleResourceOfType(string resourceType)
        {
            _fhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.Bundle);

            if ("document".Equals(resourceType))
            {
                ((Bundle)_fhirContext.FhirResponseResource).Type.ShouldBe(BundleType.Document);
            }
            else if ("searchset".Equals(resourceType))
            {
                ((Bundle)_fhirContext.FhirResponseResource).Type.ShouldBe(BundleType.Searchset);
            }
            else
            {
                Assert.Fail("Invalid resourceType: " + resourceType);
            }
        }

        [Then(@"the response should be a OperationOutcome resource")]
        public void ThenTheResponseShouldBeAOperationOutcomeResource()
        {
            TestOperationOutcomeResource();
        }

        [Then(@"the response should be a OperationOutcome resource with error code ""([^""]*)""")]
        public void ThenTheResponseShouldBeAOperationOutcomeResourceWithErrorCode(string errorCode)
        {
            TestOperationOutcomeResource(errorCode);
        }

        private void TestOperationOutcomeResource(string errorCode = null)
        {
            var resource = _fhirContext.FhirResponseResource;

            resource.ResourceType.ShouldBe(ResourceType.OperationOutcome);

            var operationOutcome = (OperationOutcome)resource;

            operationOutcome.Meta.ShouldNotBeNull();
            operationOutcome.Meta.Profile.ShouldAllBe(profile => profile.Equals("http://fhir.nhs.net/StructureDefinition/gpconnect-operationoutcome-1"));

            operationOutcome.Issue.ForEach(issue =>
            {
                issue.Details.ShouldNotBeNull();
                issue.Details.Coding.ForEach(coding =>
                {
                    coding.System.ShouldBe("http://fhir.nhs.net/ValueSet/gpconnect-error-or-warning-code-1");
                    coding.Code.ShouldNotBeNull();
                    coding.Display.ShouldNotBeNull();
                });

                if (!string.IsNullOrEmpty(errorCode))
                {
                    issue.Details.Coding.ShouldContain(x => x.Code.Equals(errorCode));
                }
            });
        }

        [Then(@"the response bundle should contain a single Patient resource")]
        public void ThenTheResponseBundleShouldContainASinglePatientResource()
        {
            _fhirContext.Patients.Count.ShouldBe(1);
        }

        [Then(@"the response bundle should contain at least One Practitioner resource")]
        public void ThenTheResponseBundleShouldContainAtLeastOnePractitionerResource()
        {
            _fhirContext.Practitioners.Count.ShouldBeGreaterThan(0);
        }

        [Then(@"the response bundle should contain a single Composition resource")]
        public void ThenTheResponseBundleShouldContainASingleCompositionResource()
        {
            _fhirContext.Compositions.Count.ShouldBe(1);
        }

        [Then(@"the response bundle should contain the composition resource as the first entry")]
        public void ThenTheResponseBundleShouldContainTheCompositionResourceAsTheFirstEntry()
        {
            ((Bundle)_fhirContext.FhirResponseResource)
                .Entry
                .Select(entry => entry.Resource.ResourceType)
                .First()
                .ShouldBe(ResourceType.Composition);
        }

        [Then(@"the patient resource in the bundle should contain meta data profile and version id")]
        public void ThenThePatientResourceInTheBundleShouldContainMetaDataProfileAndVersionId()
        {
            CheckForValidMetaDataInResource(_fhirContext.Patients, "http://fhir.nhs.net/StructureDefinition/gpconnect-patient-1");
        }

        [Then(@"if the response bundle contains an organization resource it should contain meta data profile and version id")]
        public void ThenIfTheResponseBundleContainsAnOrganizationResourceItShouldContainMetaDataProfileAndVersionId()
        {
            CheckForValidMetaDataInResource(_fhirContext.Organizations, "http://fhir.nhs.net/StructureDefinition/gpconnect-organization-1");
        }

        [Then(@"if the response bundle contains a practitioner resource it should contain meta data profile and version id")]
        public void ThenIfTheResponseBundleContainsAPractitionerResourceItShouldContainMetaDataProfileAndVersionId()
        {
            CheckForValidMetaDataInResource(_fhirContext.Practitioners, "http://fhir.nhs.net/StructureDefinition/gpconnect-practitioner-1");
        }

        [Then(@"if the response bundle contains a device resource it should contain meta data profile and version id")]
        public void ThenIfTheResponseBundleContainsADeviceResourceItShouldContainMetaDataProfileAndVersionId()
        {
            CheckForValidMetaDataInResource(_fhirContext.Devices, "http://fhir.nhs.net/StructureDefinition/gpconnect-device-1");
        }

        [Then(@"if the response bundle contains a location resource it should contain meta data profile and version id")]
        public void ThenIfTheResponseBundleContainsALocationResourceItShouldContainMetaDataProfileAndVersionId()
        {
            CheckForValidMetaDataInResource(_fhirContext.Locations, "http://fhir.nhs.net/StructureDefinition/gpconnect-location-1");
        }

        public void CheckForValidMetaDataInResource<T>(List<T> resources, string profileId) where T : Resource
        {
            resources.ForEach(resource =>
            {
                resource.Meta.ShouldNotBeNull();
                resource.Meta.Profile.Count().ShouldBe(1);
                resource.Meta.Profile.ToList().ForEach(profile =>
                {
                    profile.ShouldBe(profileId);
                });
                resource.Meta.VersionId.ShouldNotBeNull();
            });
        }

        public void ResponseBundleContainsReferenceOfType(string reference, ResourceType resourceType)
        {
            const string customMessage = "The reference from the resource was not found in the bundle by fullUrl resource element.";

            ((Bundle)_fhirContext.FhirResponseResource)
                .Entry
                .ShouldContain(entry => reference.Equals(entry.FullUrl) && entry.Resource.ResourceType.Equals(resourceType), customMessage);
        }
    }
}
