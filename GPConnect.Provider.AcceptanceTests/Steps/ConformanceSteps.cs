namespace GPConnect.Provider.AcceptanceTests.Steps
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

		[Then(@"the CapabilityStatement has a searchInclude called ""(.*)""")]
		public void theCapabilityStatementhasasearchIncludecalled(string searchIncludeToCheck)
		{
			CapabilityStatements.ForEach(capabilityStatement =>
			{
				capabilityStatement.Rest.ForEach(rest =>
				{
					var slotResource = rest.Resource.FirstOrDefault(r => r.Type == ResourceType.Slot);
					var searchInclude = slotResource.SearchIncludeElement.FirstOrDefault(i => i.ToString() == searchIncludeToCheck);
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

        [Then(@"the CapabilityStatement should contain the Extension with a status of ""([^""]*)""")]
        public void theCapabilityStatementshouldcontaintheExtensionwithavalueof(string statusValue)
        {
            var found = false;
            CapabilityStatements.ForEach(capabilityStatement =>
            {
                capabilityStatement.Extension.ShouldNotBeNull("Fail : Extension for Service Filtering is missing from the capability Statement");
                capabilityStatement.Extension.ForEach(ext =>
                {
                    if (ext.Url == FhirConst.StructureDefinitionSystems.kServiceFiltering)
                    {
                        ext.Value.ToString().ShouldBe(statusValue, "FAIL : Capability Statement Extension for Service Filtering Status is not set to the value : " + statusValue + "  Found Value : " + ext.Value);
                        Logger.Log.WriteLine("INFO : Found extension for Service Filtering in CapabilityStatement with value : " + ext.Value.ToString());
                        found = true;
                    }
                });
            });
            found.ShouldBeTrue("FAIL : Not found Extension in Capability Statement for Service Filtering Status");
        }

        [Then(@"the CapabilityStatement slot resource has a valid searchParam called service identifier")]
        public void theCapabilityStatementslotresourcehasavalidsearchParamcalledidentifier( )
        {
            var found = false;
            CapabilityStatements.ForEach(capabilityStatement =>
            {
                capabilityStatement.Rest.ForEach(rest =>
                {
                    var slotResource = rest.Resource.FirstOrDefault(r => r.Type == ResourceType.Slot);
                  
                    slotResource.SearchParam.ForEach(searchParam =>
                    {
                        if(searchParam.Name == "service.identifier")
                        {
                            searchParam.Type.ShouldBe(SearchParamType.Token, "FAIL : searchParam.Type for service.identifier is not of type token");
                            searchParam.Definition.ShouldBe(FhirConst.SearchParameters.kServiceIdentfier, "FAIL : searchParam.Definition for service.identifier is not set to the correct value");
                            searchParam.Documentation.ShouldContain("UEC DOS service ID", "FAIL : searchParam.Documentation for service.identifier is incorrect set, should contain : UEC DOS service ID ");
                            found = true;
                            Logger.Log.WriteLine("INFO : Verified slot resource has a valid searchParam called service identifier");
                        }
                    });
                });
            });
            found.ShouldBeTrue("FAIL : Not found a slot resource in capability statement resource section that has a valid searchParam called service identifier");
        }
    }
}			   
