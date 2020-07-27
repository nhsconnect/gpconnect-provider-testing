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
            //Check atleast One Immunization
            Immunizations.Count().ShouldBeGreaterThan(0, "Failed - expected atleast ONE Immunization in response");

            //Loop each immunization
            Immunizations.ForEach(immunization =>
            { 
                 //Check ID
                immunization.Id.ShouldNotBeNullOrEmpty();

                //Check Meta.profile
                CheckForValidMetaDataInResource(immunization, FhirConst.StructureDefinitionSystems.kImmunization);

                //Check daterecorded
                var dateRecorded = immunization.GetExtension(FhirConst.StructureDefinitionSystems.kDateRecorded);
                DateTime daterecordedOut;
				if (dateRecorded != null)
					DateTime.TryParse(dateRecorded.Value.ToString(), out daterecordedOut).ShouldBeTrue("Daterecorded is Not a valid DateTime");
				else
					NUnit.Framework.Assert.Fail("DateRecorded is null");

                //Check vaccinationProcedure
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
             
                 //Check Identifier
                immunization.Identifier.Count.ShouldBeGreaterThan(0, "There should be at least 1 Identifier system/value pair");
                immunization.Identifier.ForEach(identifier =>
                   {
                       identifier.System.ShouldNotBeNullOrEmpty("Identifier System Is Null or Empty - Should be populated");
                       identifier.Value.ShouldNotBeNullOrEmpty("Identifier Value Is Null or Not Valid");
                   });
               
                //Check Status
                immunization.Status.ToString().ShouldBe("completed", StringCompareShould.IgnoreCase);

                //Check NotGiven
                //immunization.NotGiven.ShouldBe(false, "Immunization.NotGiven is not FALSE");

                //Check vaccineCode
                immunization.VaccineCode.Coding.ForEach(code =>
               {
                   code.System.ShouldNotBeNullOrWhiteSpace("VaccineCode.Coding.Code.system is Null or WhiteSpace");
                   code.Code.ShouldNotBeNullOrWhiteSpace("VaccineCode.Coding.Code.Code is Null or WhiteSpace");
               });

                //Check Patient
                Patients.Where(p => p.Id == (immunization.Patient.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");
            
                //Check Primary Source
                immunization.PrimarySource.ShouldBeOfType<bool>();

           });


        }

        [Then(@"The Immunization Resources Do Not Include Not In Use Fields")]
        public void GivenTheImmunizationResourcesDoNotIncludeNotInUseFields()
        {            
            Immunizations.ForEach(immunization =>
            {
                if (immunization.Explanation != null)
                    if(immunization.Explanation.ReasonNotGiven != null) 
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
            //Check there is ONE Immunization List with snomed code
            Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kImmunizations).ToList().Count().ShouldBe(1, "Failed to Find ONE Immunzations list using Snomed Code.");

            //Get Var to List
            var immList = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kImmunizations).First();

            //Check title
            immList.Title.ShouldBe("Immunisations", "List Title is Incorrect");

            //Check Meta.profile
            CheckForValidMetaDataInResource(immList, FhirConst.StructureDefinitionSystems.kList);

            //Check Status
            immList.Status.ShouldBeOfType<List.ListStatus>("Status List is of wrong type.");
            immList.Status.ToString().ToLower().ShouldBe("current", "List Status is NOT set to completed");

            //Check Mode
            immList.Mode.ShouldBeOfType<ListMode>("Mode List is of wrong type.");
            immList.Mode.ToString().ToLower().ShouldBe("snapshot", "List Status is NOT set to completed");

            //Check code
            immList.Code.Coding.ForEach(coding =>
                {
                    coding.System.ShouldBeOneOf("http://snomed.info/sct", "http://read.info/readv2", "http://read.info/ctv3", "https://fhir.hl7.org.uk/Id/emis-drug-codes", "https://fhir.hl7.org.uk/Id/egton-codes", "https://fhir.hl7.org.uk/Id/multilex-drug-codes", "https://fhir.hl7.org.uk/Id/resipuk-gemscript-drug-codes");
                    coding.Display.ShouldNotBeNullOrEmpty("Display Should not be Null or Empty");
                });

            //Check Patient
            Patients.Where(p => p.Id == (immList.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");


            //check number of Immunizations matches number in list
            if (Immunizations.Count() != immList.Entry.Count())
            {
                Immunizations.Count().ShouldBe(immList.Entry.Count(), "Number of Immunizations does not match the number in the List");
            }
            else
            {
            immList.Entry.ForEach(entry =>
                {
                    string guidToFind = entry.Item.Reference.Replace("Immunization/", "");
                    Immunizations.Where(i => i.Id == guidToFind).Count().ShouldBe(1, "Not Found Reference to Immunization");
                });
            }
           
        }

      


    }
}
