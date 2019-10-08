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
        private List<Patient> Patients => _httpContext.FhirResponse.Patients;

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

		[Given(@"I add a madeUp immunizations part parameter")]
		public void GivenIAddAMadeUpImmunizationsPartParameter()
		{
			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create("madeUp", (Base)new FhirString ("madeUpValue1")),
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kImmunizations, tuples);

		}

		[Then(@"The Immunization Resources are Valid")]
        public void GivenTheImmunizationResourcesAreValid()
        {

            Immunizations.ForEach(immunization =>
           {
                
             immunization.Id.ShouldNotBeNullOrEmpty();
             CheckForValidMetaDataInResource(immunization, FhirConst.StructureDefinitionSystems.kImmunization);

             var dateRecorded = immunization.GetExtension(FhirConst.StructureDefinitionSystems.kDateRecorded);
             DateTime daterecordedOut;
             DateTime.TryParse(dateRecorded.Value.ToString(), out daterecordedOut).ShouldBeTrue("Daterecorded is Not a valid DateTime");

             List<Extension> vaccinationProcedure = immunization.Extension.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kVaccinationProcedure)).ToList();
             vaccinationProcedure.Count.ShouldBeLessThanOrEqualTo(1);
             if (vaccinationProcedure.Count == 1)
               {
                   CodeableConcept clinicalSetting = (CodeableConcept)vaccinationProcedure.First().Value;
                   clinicalSetting.Coding.Count.Equals(1);
                   clinicalSetting.Coding.First().System.Equals(FhirConst.CodeSystems.kCCSnomed);
                   clinicalSetting.Coding.First().Code.ShouldNotBeNullOrEmpty();
                   clinicalSetting.Coding.First().Display.ShouldNotBeNullOrEmpty();
               }
             
            immunization.Identifier.Count.ShouldBeGreaterThan(0, "There should be at least 1 Identifier system/value pair");
            immunization.Identifier.ForEach(identifier =>
               {
                       identifier.System.Equals(FhirConst.ValueSetSystems.kCrossCareIdentifier).ShouldBeTrue("Cross Care Setting Identfier NOT Found");

                       //identifier.Value format is still being debated, hence notnull check
                       identifier.Value.ShouldNotBeNullOrEmpty("Identifier Value Is Null or Not Valid");
                        //Guid guidResult;
                        //Guid.TryParse(identifier.Value, out guidResult).ShouldBeTrue("Immunization identifier GUID is not valid or Null");
               });
               
            immunization.Status.ToString().ShouldBe("completed", StringCompareShould.IgnoreCase);
            immunization.NotGiven.ShouldBe(false, "Immunization.NotGiven is not FALSE");
            immunization.VaccineCode.Coding.ForEach(code =>
               {
                   code.System.ShouldNotBeNullOrWhiteSpace("VaccineCode.Coding.Code.system is Null or WhiteSpace");
                   code.Code.ShouldNotBeNullOrWhiteSpace("VaccineCode.Coding.Code.Code is Null or WhiteSpace");
               });


            Patients.Where(p => p.Id == (immunization.Patient.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");
            

            immunization.PrimarySource.ShouldBeOfType<bool>();

           });


        }

        [Then(@"The Immunization Resources Do Not Include Not In Use Fields")]
        public void GivenTheImmunizationResourcesDoNotIncludeNotInUseFields()
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
            var immList = Lists.Where(l => l.Title == "Immunisations");

            if (immList.Count() == 1)
            {
                 var list = immList.First();
                
                list.Title.ShouldBe("Immunisations", "List Title is Incorrect");
                CheckForValidMetaDataInResource(list, FhirConst.StructureDefinitionSystems.kList);

                list.Status.ShouldBeOfType<List.ListStatus>("Status List is of wrong type.");
                list.Status.ToString().ToLower().ShouldBe("current", "List Status is NOT set to completed");

                list.Mode.ShouldBeOfType<ListMode>("Mode List is of wrong type.");
                list.Mode.ToString().ToLower().ShouldBe("snapshot", "List Status is NOT set to completed");

                list.Code.Coding.ForEach(coding =>
                {
                    coding.System.ShouldBeOneOf("http://snomed.info/sct", "http://read.info/readv2", "http://read.info/ctv3", "https://fhir.hl7.org.uk/Id/emis-drug-codes", "https://fhir.hl7.org.uk/Id/egton-codes", "https://fhir.hl7.org.uk/Id/multilex-drug-codes", "https://fhir.hl7.org.uk/Id/resipuk-gemscript-drug-codes");
                    coding.Code.ShouldBe("1102181000000102", "Code is not Correct");
                    coding.Display.ShouldNotBeNullOrEmpty("Display Should not be Null or Empty");
                });

                Patients.Where(p => p.Id == (list.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");

                //check number of Immunizations matches number in list
                if (Immunizations.Count() != list.Entry.Count())
                {
                    Immunizations.Count().ShouldBe(list.Entry.Count(), "Number of Immunizations does not match the number in the List");
                }
                else
                {
                    list.Entry.ForEach(entry =>
                    {
                        string guidToFind = entry.Item.Reference.Replace("Immunization/", "");
                        Immunizations.Where(i => i.Id == guidToFind).Count().ShouldBe(1, "Not Found Reference to Immunization");
                    });
                }
            }
            else
            {
                immList.Count().ShouldBe(1, "Expected One immunization List But Found Zero or more than 1");
            }


        }

      


    }
}
