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

        [Given(@"I have the following ods codes")]
        public void GivenIHaveTheFollowingODSCodes(Table table)
        {
            FhirContext.FhirOrganizations.Clear();
            foreach (var row in table.Rows)
            {
                string odsCode = row["Code"];
                // Map the native ODS code to provider equivalent from CSV file
                foreach (ODSCodeMap odsMap in GlobalContext.ODSCodeMapData)
                {
                    if (String.Equals(odsMap.NativeODSCode, odsCode))
                    {
                        Log.WriteLine("Mapped test ODS code {0} to {1}", odsCode, odsMap.ProviderODSCode);
                        FhirContext.FhirOrganizations.Add(row["Id"], odsMap.ProviderODSCode);
                        break;
                    }
                }
            }
        }
        
        [Given(@"I add the identifier parameter with system ""(.*)"" and value ""(.*)""")]
        public void GivenIAddTheIdentifierParameterWithTheSystemAndValue(string systemParameter, string valueParameter)
        {
            Given($@"I add the parameter ""identifier"" with the value ""{systemParameter + '|' + valueParameter}""");
        }
    }
}
