﻿using GPConnect.Provider.AcceptanceTests.Constants;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
	using Context;
	using GPConnect.Provider.AcceptanceTests.Logger;
	using Hl7.Fhir.Model;
	using NUnit.Framework;
	using Shouldly;
	using System;
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
			_httpContext.FhirResponse.Resource.ResourceType.ShouldBe(ResourceType.Bundle);

			if ("document".Equals(resourceType))
			{
				_httpContext.FhirResponse.Bundle.Type.ShouldBe(BundleType.Document);
			}
			else if ("searchset".Equals(resourceType))
			{
				_httpContext.FhirResponse.Bundle.Type.ShouldBe(BundleType.Searchset);
			}
			else if ("collection".Equals(resourceType))
			{
				_httpContext.FhirResponse.Bundle.Type.ShouldBe(BundleType.Collection);
			}
			else
			{
				Assert.Fail("Invalid resourceType: " + resourceType);
			}
		}

		//PG 8/4/2019 for #220 - Function to check bundle entries have no fullurl key/value pairs as per 1.2.3 spec
		[Then(@"the Bundle Entries should not contain a fullurl")]
		public void ThenTheBundleEntriesShouldNotContainAFullurl()
		{

			var entries = _httpContext.FhirResponse.Entries;

			int foundFullurlCounter = 0;

			entries.ForEach(entry =>
			{
				var fullUrlProperty = entry.FullUrl;

				if (fullUrlProperty != null)
					foundFullurlCounter++;
			});

			//check that no fullurl entries found, else fail test and report how many found
			foundFullurlCounter.ShouldBe(0,("Expected 0 fullurl key/value pairs but found : " + foundFullurlCounter.ToString() ));

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
			var resource = _httpContext.FhirResponse.Resource;

			resource.ResourceType.ShouldBe(ResourceType.OperationOutcome);

			var operationOutcome = (OperationOutcome)resource;

			operationOutcome.Meta.ShouldNotBeNull();
			//operationOutcome.Meta.Profile.ShouldAllBe(profile => profile.Equals("http://fhir.nhs.net/StructureDefinition/gpconnect-operationoutcome-1"));

			operationOutcome.Issue?.Count.ShouldBeGreaterThanOrEqualTo(1);

			operationOutcome.Issue?.ForEach(issue =>
			{
				var errorValue = issue.Severity;

				Log.WriteLine("issue.severity value is incorrect should be issue.severity.error");
				errorValue.ToString().ShouldBe("Error", "the severity value is incorrect should be Error"); //#259 SJD 26/6/19 assertion added

				issue.Code.ShouldNotBeNull();

				issue.Details?.Coding?.Count.ShouldBe(1);
				if (issue.Details?.Coding != null)
				{
					var coding = issue.Details.Coding[0];

					coding.Code.ShouldNotBeNull();
					coding.Display.ShouldNotBeNull();

					if (!string.IsNullOrEmpty(errorCode))
					{
						coding.Code.ShouldBe(errorCode);
					}
				}
			});
		}

		[Then(@"the response bundle should contain ""([^""]*)"" entries")]
		public void ThenResponseBundleEntryShouldNotBeEmpty(int expectedSize)
		{
			_httpContext.FhirResponse.Entries.Count.ShouldBe(expectedSize, "The response bundle does not contain the expected number of entries");
		}

		[Then(@"the response bundle should contain a single Patient resource")]
		public void ThenTheResponseBundleShouldContainASinglePatientResource()
		{
			_httpContext.FhirResponse.Patients.Count.ShouldBe(1);
		}

		[Then(@"the response bundle should contain at least One Practitioner resource")]
		public void ThenTheResponseBundleShouldContainAtLeastOnePractitionerResource()
		{
			_httpContext.FhirResponse.Practitioners.Count.ShouldBeGreaterThan(0);
		}

		[Then(@"the response bundle should contain a single Composition resource")]
		public void ThenTheResponseBundleShouldContainASingleCompositionResource()
		{
			_httpContext.FhirResponse.Compositions.Count.ShouldBe(1);
		}

		[Then(@"the response Bundle should contain a single Composition resource as the first Entry")]
		public void ThenTheResponseBundleShouldContainASingleCompositionResourceAsTheFirstEntry()
		{
			_httpContext.FhirResponse.Compositions.Count.ShouldBe(1);

			_httpContext.FhirResponse.Entries
				.Select(entry => entry.Resource.ResourceType)
				.First()
				.ShouldBe(ResourceType.Composition);
		}

		[Then(@"the response bundle should contain the composition resource as the first entry")]
		public void ThenTheResponseBundleShouldContainTheCompositionResourceAsTheFirstEntry()
		{
			_httpContext.FhirResponse.Bundle
				.Entry
				.Select(entry => entry.Resource.ResourceType)
				.First()
				.ShouldBe(ResourceType.Composition);
		}

		[Then(@"the response meta profile should be for ""([^""]*)""")]
		public void ThenTheResponseMetaProfileShouldBe(string metaProfileType)
		{
			var profiles = _httpContext.FhirResponse.Resource.Meta.Profile.ToList();
			profiles.ShouldNotBeNull();

			if ("searchset".Equals(metaProfileType))
			{
				var requireProfile = profiles.FirstOrDefault(p => p.Equals(FhirConst.StructureDefinitionSystems.kGpcSearchSet));
				requireProfile.ShouldNotBeNull();
			}
			else if ("structured".Equals(metaProfileType))
			{
				var requireProfile = profiles.FirstOrDefault(p => p.Equals(FhirConst.StructureDefinitionSystems.kGpcStructuredRecordBundle));
				requireProfile.ShouldNotBeNull();
			}
		}

		[Then(@"the patient resource in the bundle should contain meta data profile and version id")]
		public void ThenThePatientResourceInTheBundleShouldContainMetaDataProfileAndVersionId()
		{
			CheckForValidMetaDataInResource(_httpContext.FhirResponse.Patients,FhirConst.StructureDefinitionSystems.kPatient);
		}

		[Then(@"if the response bundle contains an organization resource it should contain meta data profile and version id")]
		public void ThenIfTheResponseBundleContainsAnOrganizationResourceItShouldContainMetaDataProfileAndVersionId()
		{
			CheckForValidMetaDataInResource(_httpContext.FhirResponse.Organizations, FhirConst.StructureDefinitionSystems.kOrganisation);
		}

		[Then(@"if the response bundle contains a practitioner resource it should contain meta data profile and version id")]
		public void ThenIfTheResponseBundleContainsAPractitionerResourceItShouldContainMetaDataProfileAndVersionId()
		{
			CheckForValidMetaDataInResource(_httpContext.FhirResponse.Practitioners, FhirConst.StructureDefinitionSystems.kPractitioner);
		}

		[Then(@"if the response bundle contains a device resource it should contain meta data profile and version id")]
		public void ThenIfTheResponseBundleContainsADeviceResourceItShouldContainMetaDataProfileAndVersionId()
		{
			CheckForValidMetaDataInResource(_httpContext.FhirResponse.Devices, "http://fhir.nhs.net/StructureDefinition/gpconnect-device-1");
		}

		[Then(@"if the response bundle contains a location resource it should contain meta data profile and version id")]
		public void ThenIfTheResponseBundleContainsALocationResourceItShouldContainMetaDataProfileAndVersionId()
		{
			CheckForValidMetaDataInResource(_httpContext.FhirResponse.Locations, "http://fhir.nhs.net/StructureDefinition/CareConnect-GPC-Location-1");
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
			var customMessage = $"The {resourceType} reference from the resource was not found in the bundle by fullUrl resource element.";

			var lowerReference = reference.ToLowerInvariant();

			_httpContext.FhirResponse.Bundle.Entry.ShouldContain(entry => lowerReference.Equals(ComposeReferenceFromEntry(entry)) && entry.Resource.ResourceType.Equals(resourceType), customMessage);
		}

		private static string ComposeReferenceFromEntry(EntryComponent entry)
		{
			return $"{entry.Resource.TypeName}/{entry.Resource.Id}".ToLowerInvariant();
		}

		public void ResponseBundleDoesNotContainReferenceOfType(ResourceType resourceType)
		{
			const string customMessage = "The reference from the resource was found in the bundle by fullUrl resource element but has not been requested.";

			_httpContext.FhirResponse.Bundle.Entry.Count(ent => ent.Resource.ResourceType.Equals(resourceType)).ShouldBe(0, customMessage);
		}

	}
}
