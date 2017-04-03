using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Data;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using static Hl7.Fhir.Model.Bundle;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public sealed class OrganizationSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;

        public OrganizationSteps(FhirContext fhirContext)
        {
            FhirContext = fhirContext;
        }

        [Given(@"I have the test ods codes")]
        public void GivenIHaveTheTestODSCodes()
        {
            FhirContext.FhirOrganizations.Clear();

            foreach (ODSCodeMap odsMap in GlobalContext.ODSCodeMapData)
            {
                Log.WriteLine("Mapped test ODS code {0} to {1}", odsMap.NativeODSCode, odsMap.ProviderODSCode);
                FhirContext.FhirOrganizations.Add(odsMap.NativeODSCode, odsMap.ProviderODSCode);
            }
        }
        
        [Given(@"I add the organization identifier parameter with system ""(.*)"" and value ""(.*)""")]
        public void GivenIAddTheIdentifierParameterWithTheSystemAndValue(string systemParameter, string valueParameter)
        {
            Given($@"I add the parameter ""identifier"" with the value ""{systemParameter + '|' + FhirContext.FhirOrganizations[valueParameter]}""");
        }

        [Then(@"response should contain ods-organization-codes ""([^""]*)""")]
        public void ThenResponseShouldContainODSOrganizationCodesWithValues(string elementValues)
        {
            List<string> referenceValueList = new List<string>();

            foreach (var element in elementValues.Split(new char[] { '|' }))
            {
                referenceValueList.Add(FhirContext.FhirOrganizations[element]);
            }

            string referenceValues = String.Join("|", referenceValueList);
            
            Then($@"response bundle ""Organization"" entries should contain element ""resource.identifier[?(@.system == 'http://fhir.nhs.net/Id/ods-organization-code')].value"" with values ""{referenceValues}""");
        }

        [Then(@"response should contain ods-site-codes ""([^""]*)""")]
        public void ThenResponseShouldContainODSSiteCodesWithValues(string elementValues)
        {
            List<string> referenceValueList = new List<string>();

            foreach (var element in elementValues.Split(new char[] { '|' }))
            {
                referenceValueList.Add(FhirContext.FhirOrganizations[element]);
            }

            string referenceValues = String.Join("|", referenceValueList);

            Then($@"response bundle ""Organization"" entries should contain element ""resource.identifier[?(@.system == 'http://fhir.nhs.net/Id/ods-site-code')].value"" with values ""{referenceValues}""");
        }
    }
}
