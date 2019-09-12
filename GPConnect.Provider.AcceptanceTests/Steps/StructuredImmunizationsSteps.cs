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
    public sealed class StructuredImmunizationsSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private List<Immunization> Immunizations => _httpContext.FhirResponse.Immunizations;
        private List<List> Lists => _httpContext.FhirResponse.Lists;

        public StructuredImmunizationsSteps(HttpSteps httpSteps, HttpContext httpContext)
            : base(httpSteps)
        {
            _httpContext = httpContext;
        }


        [Given(@"I add the immunizations parameter")]
        public void GivenIAddTheImmunizationsParameter()
        {
            ParameterComponent param = new ParameterComponent();
            param.Name = FhirConst.GetStructuredRecordParams.kImmunizations;
            _httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(param);
        }


        [Then(@"The Immunization Resource is Valid")]
        public void GivenTheImmunizationResourceIsValid()
        {

            Immunizations.ForEach(immunization =>
           {
                //ID
                immunization.Id.ShouldNotBeNullOrEmpty();

                //Check Meta Profile
                CheckForValidMetaDataInResource(immunization, FhirConst.StructureDefinitionSystems.kImmunization);

                //recordedDate Extension
                //tbd

                //vaccinationProcedure
                immunization.GetExtension(" https://fhir.hl7.org.uk/STU3/StructureDefinition/Extension-CareConnect-VaccinationProcedure-1").ShouldNotBeNull();

                //identifier
                immunization.Identifier.Count.ShouldBeGreaterThan(0, "There should be at least 1 Identifier system/value pair");
                immunization.Identifier.ForEach(identifier =>
                   {
                       identifier.System.Equals("https://fhir.nhs.uk/Id/cross-care-setting-identifier").ShouldBeTrue("Cross Care Setting Identfier NOT Found");

                       identifier.Value.ShouldNotBeNullOrEmpty("Identifier Value Is Null or Not Valid");
                        //Guid guidResult;
                        //Guid.TryParse(identifier.Value, out guidResult).ShouldBeTrue("Immunization identifier GUID is not valid or Null");
                    });
                
               //status
                immunization.Status.ToString().ShouldBe("completed", StringCompareShould.IgnoreCase);

                //notGiven
                immunization.NotGiven.ShouldBe(false, "Immunization.NotGiven is not FALSE");

                //vaccineCode
                immunization.VaccineCode.Coding.ForEach(code =>
               {
                   code.System.ShouldNotBeNullOrWhiteSpace("VaccineCode.Coding.Code.system is Null or WhiteSpace");
                   code.Code.ShouldNotBeNullOrWhiteSpace("VaccineCode.Coding.Code.Code is Null or WhiteSpace");
               });

                //Check Patient reference is contained in bundle
               immunization.Patient.Reference.ShouldContain("Patient/", "Patient reference Not Found");

               //primarySource
               immunization.PrimarySource.ShouldBeOfType<bool>();

           });


        }

        [Then(@"The Immunization Resource Does Not Include Not In Use Fields")]
        public void GivenTheImmunizationResourceDoesNotIncludeMustNotFields()
        {

            Immunizations.ForEach(immunization =>
            {
                immunization.Explanation.ReasonNotGiven.Count().ShouldBe(0, ".Explanation.ReasonNotGiven is Not Supposed to be Sent - Not In Use Field");
                immunization.Reaction.Count().ShouldBe(0, "Reaction is Not Supposed to be Sent - Not In Use Field");

                immunization.VaccinationProtocol.ForEach(vaccinationprotocol =>
                {
                    vaccinationprotocol.Authority.ShouldBeNull("vaccinationprotocol.Authority is Not Supposed to be Sent - Not In Use Field");
                    vaccinationprotocol.Series.ShouldBeNull("vaccinationprotocol.Series is Not Supposed to be Sent - Not In Use Field");
                    vaccinationprotocol.DoseStatusReason.ShouldBeNull("vaccinationprotocol.DoseStatusReason is Not Supposed to be Sent - Not In Use Field");
                });

            });
        }

        [Then(@"The Immunization List is Valid")]
        public void GivenTheImmunizationListisValid()
        {

            Lists.ForEach(list =>
            {
                    //list.Title.ShouldBe("Immunisations", "List Title is Incorrect");
                    CheckForValidMetaDataInResource(list, FhirConst.StructureDefinitionSystems.kList);
                list.Status.ToString().ToLower().ShouldBe("current", "List Status is NOT set to completed");
                list.Mode.ToString().ToLower().ShouldBe("snapshot", "List Status is NOT set to completed");


                list.Code.Coding.ForEach(coding =>
                {
                    coding.System.ShouldBeOneOf("http://snomed.info/sct", "http://read.info/readv2", "http://read.info/ctv3", "https://fhir.hl7.org.uk/Id/emis-drug-codes", "https://fhir.hl7.org.uk/Id/egton-codes", "https://fhir.hl7.org.uk/Id/multilex-drug-codes", "https://fhir.hl7.org.uk/Id/resipuk-gemscript-drug-codes");
                    coding.Code.ShouldBe("1102181000000102", "Code is not Correct");
                    coding.Display = "Immunisations";
                });


                list.Subject.Reference.ShouldContain("Patient/", "Patient reference Not Found");

                list.Entry.ForEach(entry =>
                {
                    string guidToFind = entry.Item.Reference.Replace("Immunization/", "");
                    Immunizations.Where(i => i.Id == guidToFind).Count().ShouldBe(1, "Not Found Reference to Immunization");
                });
            });

        }
        
        [Then(@"The Immunization List Does Not Include Not In Use Fields")]
        public void GivenTheImmunizationListDoesNotIncludeMustNotFields()
        {
            Lists.ForEach(list =>
            {
                list.Id.ShouldBeNull("List Id is Not Supposed to be Sent - Not In Use Field");
                list.Meta.VersionId.ShouldBeNull("List Meta.VersionId is Not Supposed to be Sent - Not In Use Field");
                list.Meta.LastUpdated.ShouldBeNull("List Meta.LastUpdated is Not Supposed to be Sent - Not In Use Field");
                list.Source.ShouldBeNull("List Source is Not Supposed to be Sent - Not In Use Field");
            });
        }
    
    }
}
