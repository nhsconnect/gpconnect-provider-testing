namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Context;
    using GPConnect.Provider.AcceptanceTests.Helpers;
    using Hl7.Fhir.Model;
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

    }
}
