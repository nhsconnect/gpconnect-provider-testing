namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using Context;
    using Hl7.Fhir.Model;
    using NUnit.Framework;
    using Shouldly;
    using System.Collections.Generic;
    using System.Linq;
    using TechTalk.SpecFlow;
    using static Hl7.Fhir.Model.Bundle;

    [Binding]
    public sealed class BundleSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;

        public BundleSteps(HttpSteps httpSteps, HttpContext httpContext) : base(httpSteps)
        {
            _httpContext = httpContext;
        }

        [Then(@"the response should be a Bundle resource of type ""([^""]*)""")]
        public void ThenTheResponseShouldBeABundleResourceOfType(string resourceType)
        {
            _httpContext.HttpResponse.Resource.ResourceType.ShouldBe(ResourceType.Bundle);

            if ("document".Equals(resourceType))
            {
                _httpContext.HttpResponse.Bundle.Type.ShouldBe(BundleType.Document);
            }
            else if ("searchset".Equals(resourceType))
            {
                _httpContext.HttpResponse.Bundle.Type.ShouldBe(BundleType.Searchset);
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
            var resource = _httpContext.HttpResponse.Resource;

            resource.ResourceType.ShouldBe(ResourceType.OperationOutcome);

            var operationOutcome = (OperationOutcome)resource;

            operationOutcome.Meta.ShouldNotBeNull();
            operationOutcome.Meta.Profile.ShouldAllBe(profile => profile.Equals("http://fhir.nhs.net/StructureDefinition/gpconnect-operationoutcome-1"));

            operationOutcome.Issue?.Count.ShouldBeGreaterThanOrEqualTo(1);

            operationOutcome.Issue?.ForEach(issue =>
            {
                issue.Severity.ShouldNotBeNull();
                issue.Code.ShouldNotBeNull();

                issue.Details?.Coding?.Count.ShouldBe(1);
                if (issue.Details?.Coding != null)
                {
                    var coding = issue.Details.Coding[0];

                    coding.System.ShouldBe("http://fhir.nhs.net/ValueSet/gpconnect-error-or-warning-code-1");
                    coding.Code.ShouldNotBeNull();
                    coding.Display.ShouldNotBeNull();

                    if (!string.IsNullOrEmpty(errorCode))
                    {
                        coding.Code.ShouldBe(errorCode);
                    }
                }
            });
        }

        [Then(@"the response bundle should contain a single Patient resource")]
        public void ThenTheResponseBundleShouldContainASinglePatientResource()
        {
            _httpContext.HttpResponse.Patients.Count.ShouldBe(1);
        }

        [Then(@"the response bundle should contain at least One Practitioner resource")]
        public void ThenTheResponseBundleShouldContainAtLeastOnePractitionerResource()
        {
            _httpContext.HttpResponse.Practitioners.Count.ShouldBeGreaterThan(0);
        }

        [Then(@"the response bundle should contain a single Composition resource")]
        public void ThenTheResponseBundleShouldContainASingleCompositionResource()
        {
            _httpContext.HttpResponse.Compositions.Count.ShouldBe(1);
        }

        [Then(@"the response Bundle should contain a single Composition resource as the first Entry")]
        public void ThenTheResponseBundleShouldContainASingleCompositionResourceAsTheFirstEntry()
        {
            _httpContext.HttpResponse.Compositions.Count.ShouldBe(1);

            _httpContext.HttpResponse.Entries
                .Select(entry => entry.Resource.ResourceType)
                .First()
                .ShouldBe(ResourceType.Composition);
        }

        [Then(@"the response bundle should contain the composition resource as the first entry")]
        public void ThenTheResponseBundleShouldContainTheCompositionResourceAsTheFirstEntry()
        {
            _httpContext.HttpResponse.Bundle
                .Entry
                .Select(entry => entry.Resource.ResourceType)
                .First()
                .ShouldBe(ResourceType.Composition);
        }

        [Then(@"the patient resource in the bundle should contain meta data profile and version id")]
        public void ThenThePatientResourceInTheBundleShouldContainMetaDataProfileAndVersionId()
        {
            CheckForValidMetaDataInResource(_httpContext.HttpResponse.Patients, "http://fhir.nhs.net/StructureDefinition/gpconnect-patient-1");
        }

        [Then(@"if the response bundle contains an organization resource it should contain meta data profile and version id")]
        public void ThenIfTheResponseBundleContainsAnOrganizationResourceItShouldContainMetaDataProfileAndVersionId()
        {
            CheckForValidMetaDataInResource(_httpContext.HttpResponse.Organizations, "http://fhir.nhs.net/StructureDefinition/gpconnect-organization-1");
        }

        [Then(@"if the response bundle contains a practitioner resource it should contain meta data profile and version id")]
        public void ThenIfTheResponseBundleContainsAPractitionerResourceItShouldContainMetaDataProfileAndVersionId()
        {
            CheckForValidMetaDataInResource(_httpContext.HttpResponse.Practitioners, "http://fhir.nhs.net/StructureDefinition/gpconnect-practitioner-1");
        }

        [Then(@"if the response bundle contains a device resource it should contain meta data profile and version id")]
        public void ThenIfTheResponseBundleContainsADeviceResourceItShouldContainMetaDataProfileAndVersionId()
        {
            CheckForValidMetaDataInResource(_httpContext.HttpResponse.Devices, "http://fhir.nhs.net/StructureDefinition/gpconnect-device-1");
        }

        [Then(@"if the response bundle contains a location resource it should contain meta data profile and version id")]
        public void ThenIfTheResponseBundleContainsALocationResourceItShouldContainMetaDataProfileAndVersionId()
        {
            CheckForValidMetaDataInResource(_httpContext.HttpResponse.Locations, "http://fhir.nhs.net/StructureDefinition/gpconnect-location-1");
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

            _httpContext.HttpResponse.Bundle
                .Entry
                .ShouldContain(entry => reference.Equals(entry.FullUrl) && entry.Resource.ResourceType.Equals(resourceType), customMessage);
        }
    }
}
