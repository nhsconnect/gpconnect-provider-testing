﻿namespace GPConnect.Provider.AcceptanceTests.Steps
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using Constants;
	using Context;
	using GPConnect.Provider.AcceptanceTests.Helpers;
	using GPConnect.Provider.AcceptanceTests.Logger;
	using Hl7.Fhir.Model;
	using Hl7.Fhir.Serialization;
	using Newtonsoft.Json.Linq;
	using Shouldly;
	using TechTalk.SpecFlow;

	[Binding]
	public class CapabilityStatementSteps
	{
		private readonly HttpContext _httpContext;
		private List<CapabilityStatement> CapabilityStatements => _httpContext.FhirResponse.CapabilityStatements;
		public CapabilityStatementSteps(HttpContext httpContext)
		{
			_httpContext = httpContext;
		}

		[Then("the Response Resource should be a CapabilityStatement")]
		public void TheResponseResourceShouldBeACapabilityStatement()
		{
			_httpContext.FhirResponse.Resource.ResourceType.ShouldBe(ResourceType.CapabilityStatement);
		}

		[Then(@"the CapabilityStatement version should match the GP Connect specification release")]
		public void TheCapabilityStatementVersionShouldMatchTheGPConnectSpecificationRelease()
		{
			CapabilityStatements.ForEach(capabilityStatement =>
			{
				capabilityStatement.Version.ShouldBe(AppSettingsHelper.GPConnectSpecVersion, $"The CapabilityStatement should match the specification version {AppSettingsHelper.GPConnectSpecVersion} but was {capabilityStatement.Version}");
			});
		}


		[Then("the CapabilityStatement Format should contain XML and JSON")]
		public void TheCapabilityStatementFormatShouldContainXmlAndJson()
		{
			CapabilityStatements.ForEach(capabilityStatement =>
			{
				capabilityStatement.Format.ShouldContain(ContentType.Application.FhirJson, $"The CapabilityStatement Format should contain {ContentType.Application.FhirJson} but did not.");
				capabilityStatement.Format.ShouldContain(ContentType.Application.FhirXml, $"The CapabilityStatement Format should contain {ContentType.Application.FhirXml} but did not.");
			});
		}

		[Then("the CapabilityStatement Software should be valid")]
		public void TheCapabilityStatementSoftwareShouldBeValid()
		{
			CapabilityStatements.ForEach(capabilityStatement =>
			{
				capabilityStatement.Software.Name.ShouldNotBeNullOrEmpty($"The CapabilityStatement Software Name should not be be null or empty but was {capabilityStatement.Software.Name}.");
				capabilityStatement.Software.Version.ShouldNotBeNullOrEmpty($"The CapabilityStatement Software Version should not be be null or empty but was {capabilityStatement.Software.Version}.");
			});
		}

		[Then(@"the CapabilityStatement FHIR Version should be ""([^""]*)""")]
		public void TheCapabilityStatementFhirVerionShouldBe(string version)
		{
			CapabilityStatements.ForEach(capabilityStatement =>
			{
				capabilityStatement.FhirVersion.ShouldBe(version, $"The CapabilityStatement FHIR Version should be {version} but was {capabilityStatement.FhirVersion}.");
			});
		}

		[Then(@"the CapabilityStatement REST Operations should contain ""([^""]*)""")]
		public void TheCapabilityStatementRestOperationsShouldContain(string operation)
		{
			CapabilityStatements.ForEach(capabilityStatement =>
			{
				capabilityStatement.Rest.ForEach(rest =>
				{
					rest.Operation
						.Select(op => op.Name)
						.ShouldContain(operation, $"The CapabilityStatement REST Operations should contain {operation} but did not.");
				});
			});
		}

		//#292 - PG 30/8/2019 - Check The structured record URL 
		[Then(@"the CapabilityStatement Operation ""([^""]*)"" has url ""([^""]*)""")]
		public void TheCapabilityStatementOperationsHasURL(string operation, string url)
		{
			CapabilityStatements.ForEach(capabilityStatement =>
			{
				var foundFlag = false;
				capabilityStatement.Rest.ForEach(rest =>
				{
					foreach (var op in rest.Operation)
					{
						if (op.Name == operation)
						{
							if (op.Definition.Reference == url)
								foundFlag = true;
						}
					}
				});

				if (foundFlag)
				{
					var message = "URL :" + url + " found on Operation :" + operation;
					Log.WriteLine(message);
					foundFlag.ShouldBeTrue(message);
				}
				else
				{
					var message = "URL :" + url + " Has NOT been found on Operation :" + operation;
					Log.WriteLine(message);
					foundFlag.ShouldBeTrue(message);
				}

			});
		}


		[Then(@"the CapabilityStatement REST Resources should contain the ""([^""]*)"" Resource with the ""([^""]*)"" Interaction")]
		public void TheCapabilityStatementRestResourcesShouldContainAResourceWithInteraction(ResourceType resourceType, CapabilityStatement.TypeRestfulInteraction interaction)
		{
			CapabilityStatements.ForEach(capabilityStatement =>
			{
				capabilityStatement.Rest.ForEach(rest =>
				{
					var resource = rest.Resource.FirstOrDefault(r => r.Type == resourceType);

					resource.ShouldNotBeNull($"The CapabilityStatement REST Resources should contain {resourceType.ToString()} but did not.");

					var interactions = resource.Interaction.Where(i => i.Code == interaction);

					interactions.ShouldNotBeNull($"The CapabilityStatement REST {resourceType.ToString()} Resource Interactions should contain the {interaction.ToString()} Interaction but did not.");
				});
			});
		}

		//PG 12-4-2019 #225 - Check that CapabilityStatement includes specified searchInclude
		[Then(@"the CapabilityStatement has a searchInclude called ""(.*)""")]
		public void theCapabilityStatementhasasearchIncludecalled(string searchIncludeToCheck)
		{
			CapabilityStatements.ForEach(capabilityStatement =>
			{
				capabilityStatement.Rest.ForEach(rest =>
				{
					//Get Handle to Slot Resouce
					var slotResource = rest.Resource.FirstOrDefault(r => r.Type == ResourceType.Slot);

					//find searchinclude passed in.
					var searchInclude = slotResource.SearchIncludeElement.FirstOrDefault(i => i.ToString() == searchIncludeToCheck);

					//Assert That Text Is Found
					searchInclude.ShouldNotBeNull("Not Found searchInclude: " + searchIncludeToCheck);
				});
			});
		}

		//SJD 1.2.6
		[Then(@"the CapabilityStatement Profile should contain the correct reference and version history ""(.*)""")]
		public void theCapabilityStatementProfileShouldContainTheCorrectReferencesAndVersionHistory(string urlToCheck)
		{
			CapabilityStatements.ForEach(capabilityStatement =>
			{
				List<ResourceReference> profiles = capabilityStatement.Profile;
				profiles.Where(p => p.Reference == urlToCheck).Count().ShouldBe(1, "Fail : Mismatch of expected profile.reference  " + urlToCheck);
                Logger.Log.WriteLine("Info : Found Profile in CapabilityStatement : " + urlToCheck);
			});
		}

        [Then(@"the CapabilityStatement REST Extension should contain ""([^""]*)""")]
        public void TheCapabilityStatementRestExtensionShouldContain(string operation)
        {
            CapabilityStatements.ForEach(capabilityStatement =>
            {
                capabilityStatement.Rest.ForEach(rest =>
                {
                    //Get Handle to Extension Resouce
                    //var slotResource = rest.Resource.FirstOrDefault(r => r.Type == ResourceType.Slot);
                    var extensionResource = rest.Extension.FirstOrDefault();


                    Console.WriteLine("stop");
                    //rest.Operation
                    //    .Select(op => op.Name)
                    //    .ShouldContain(operation, $"The CapabilityStatement REST Operations should contain {operation} but did not.");
                });
            });
        }
    }
}			   
