namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using Constants;
    using Context;
    using TechTalk.SpecFlow;
    using Shouldly;
    using Hl7.Fhir.Model;
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using GPConnect.Provider.AcceptanceTests.Enum;
    using static Hl7.Fhir.Model.Parameters;
    using GPConnect.Provider.AcceptanceTests.Helpers;

    [Binding]
    public sealed class StructuredCommonSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private List<Immunization> Immunizations => _httpContext.FhirResponse.Immunizations;
        private List<List> Lists => _httpContext.FhirResponse.Lists;
        private List<Patient> Patients => _httpContext.FhirResponse.Patients;

        public StructuredCommonSteps(HttpSteps httpSteps, HttpContext httpContext)
            : base(httpSteps)
        {
            _httpContext = httpContext;
        }


        [Then(@"Check the list ""(.*)"" contains confidential marking")]
        public void Checkthelistcontainsconfidentialmarking(string listTitleToCheck)
        {
            var listToBeChecked = Lists.Where(list => list.Title == listTitleToCheck).ToList().FirstOrDefault();
            listToBeChecked.ShouldNotBeNull("Fail : No List with title : " + listTitleToCheck + " found to check for confidential markings");

            listToBeChecked.Extension
                    .Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kExtListWarningCode))
                    .Where(extension => extension.Value.ToString().Equals(FhirConst.ListWarnings.ConfidentialItemsCode)).ToList()
                    .Count().ShouldBe(1, " Fail : List : " + listTitleToCheck + " Has no Warnings Extension with correct values");

            listToBeChecked.Note
                .Where(note => note.Text == FhirConst.ListWarnings.ConfidentialItemsAssociatedtext).ToList()
                .Count().ShouldBe(1,"Fail : Confidential Note Not Found On List : " + listTitleToCheck);

            Logger.Log.WriteLine("Info : List : " + listTitleToCheck + " Checked and has passed checks for confidential markings");
        }
    }
}