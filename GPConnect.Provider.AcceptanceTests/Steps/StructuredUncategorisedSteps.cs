﻿namespace GPConnect.Provider.AcceptanceTests.Steps
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
	public sealed class StructuredUncategorisedSteps : BaseSteps
	{
		private readonly HttpContext _httpContext;
        private List<Observation> Observations => _httpContext.FhirResponse.Observations;
        private List<List> Lists => _httpContext.FhirResponse.Lists;
        private List<Patient> Patients => _httpContext.FhirResponse.Patients;

        public StructuredUncategorisedSteps(HttpSteps httpSteps, HttpContext httpContext)
			: base(httpSteps)
		{
			_httpContext = httpContext;
		}


		[Given(@"I add the uncategorised data parameter with optional parameters")]
		public void GivenIAddTheUncategorisedDataParameterWithOptionalParameters()
		{
			var backDate = DateTime.UtcNow.AddDays(-10);
			var futureDate = DateTime.UtcNow.AddDays(5);
			var startDate = backDate.ToString("yyyy-MM-dd");
			var endDate = futureDate.ToString("yyyy-MM-dd");

			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kUncategorisedData, (Base)FhirHelper.GetTimePeriod(startDate, endDate)),
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kUncategorised, tuples);
		}

		[Given(@"I add the uncategorised data parameter")]
		public void GivenIAddTheUncategorisedDataParameter()
		{
			{
				ParameterComponent param = new ParameterComponent();
				param.Name = FhirConst.GetStructuredRecordParams.kUncategorised;
				_httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(param);
			}
		}
		
		[Given(@"I add the uncategorised data parameter with date permutations ""([^""]*)"" and ""([^""]*)""")]
		public void GivenIAddTheUncategorisedDataParameterWithDatePermutations(string start, string end)
		{
			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kUncategorisedData, (Base)FhirHelper.GetTimePeriod(start, end)),
				
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kUncategorised, tuples);
		}

		[Given(@"I add the uncategorised data parameter with future start date")]
		public void GivenIAddTheUncategorisedDataParameterWithFutureStartDate()
		{
			var backDate = DateTime.UtcNow.AddDays(+10);
			var futureDate = DateTime.UtcNow.AddDays(+15);
			var startDate = backDate.ToString("yyyy-MM-dd");
			var endDate = futureDate.ToString("yyyy-MM-dd");

			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kUncategorisedData, (Base)FhirHelper.GetTimePeriod(startDate, endDate)),
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kUncategorised, tuples);
		}

		[Given(@"I add the uncategorised data parameter start date after endDate")]
		public void GivenIAddTheUncategorisedDataParameterStartDateAfterEndDate()
		{
			var backDate = DateTime.UtcNow.AddDays(-10);
			var futureDate = DateTime.UtcNow.AddDays(-15);
			var startDate = backDate.ToString("yyyy-MM-dd");
			var endDate = futureDate.ToString("yyyy-MM-dd");

			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kUncategorisedData, (Base)FhirHelper.GetTimePeriod(startDate, endDate)),
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kUncategorised, tuples);
		}

		[Given(@"I add the uncategorised data parameter with current date")]
		public void GivenIAddTheUncategorisedDataParameterWithCurrentDate()
		{
			var backDate = DateTime.UtcNow;
			var futureDate = DateTime.UtcNow;
			var startDate = backDate.ToString("yyyy-MM-dd");
			var endDate = futureDate.ToString("yyyy-MM-dd");

			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kUncategorisedData, (Base)FhirHelper.GetTimePeriod(startDate, endDate)),
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kUncategorised, tuples);
		}

		[Then(@"The Observation Resources are Valid")]
        public void GivenTheObservationResourcesareValid()
        {
            Observations.ForEach(observation =>
            {
                observation.Id.ShouldNotBeNullOrEmpty();
                CheckForValidMetaDataInResource(observation, FhirConst.StructureDefinitionSystems.kObservation);
                observation.Status.ToString().ShouldBe("final", StringCompareShould.IgnoreCase);

                Patients.Where(p => p.Id == (observation.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");

                observation.Code.ShouldNotBeNull("Code Element should not be null");

                observation.Identifier.Count.ShouldBeGreaterThan(0, "There should be at least 1 Identifier system/value pair");
                observation.Identifier.ForEach(identifier =>
                {
                    identifier.System.Equals(FhirConst.ValueSetSystems.kCrossCareIdentifier).ShouldBeTrue("Cross Care Setting Identfier NOT Found");

                    //identifier.Value format is still being debated, hence notnull check
                    identifier.Value.ShouldNotBeNullOrEmpty("Identifier Value Is Null or Not Valid");
                    //Guid guidResult;
                    //Guid.TryParse(identifier.Value, out guidResult).ShouldBeTrue("Immunization identifier GUID is not valid or Null");
                });

                observation.Issued.ShouldNotBeNull("Issued is Mandatory Fields and Should be included in th payload");
                observation.Performer.First().Reference.ShouldNotBeNull("Performer is Null and should not be");

            });
        }

        [Then(@"The Observation Resources Do Not Include Not In Use Fields")]
        public void GivenTheObservationResourcesDoNotIncludeNotInUseFields()
        {
            Observations.ForEach(observation =>
            {
                observation.BasedOn.Count().ShouldBe(0, "BasedOn is Not Supposed to be Sent - Not In Use Field");
                observation.Category.Count().ShouldBe(0, "Category is Not Supposed to be Sent - Not In Use Field");
                observation.DataAbsentReason.ShouldBeNull("DataAbsentReason is Not Supposed to be Sent - Not In Use Field");
                observation.Interpretation.ShouldBeNull("Interpretation is Not Supposed to be Sent - Not In Use Field");
                observation.BodySite.ShouldBeNull("BodySite is Not Supposed to be Sent - Not In Use Field");
                observation.Method.ShouldBeNull("Method is Not Supposed to be Sent - Not In Use Field");
                observation.Specimen.ShouldBeNull("Specimen is Not Supposed to be Sent - Not In Use Field");
                observation.Device.ShouldBeNull("Device is Not Supposed to be Sent - Not In Use Field");
            });
        }


        [Then(@"The Observation List is Valid")]
        public void GivenTheObservationListisValid()
        {
            var obvList = Lists.Where(l => l.Title == "Miscellaneous record");

            if (obvList.Count() == 1)
            {
                var list = obvList.First();

                list.Title.ShouldBe("Miscellaneous record", "List Title is Incorrect");
                CheckForValidMetaDataInResource(list, FhirConst.StructureDefinitionSystems.kList);

                list.Status.ShouldBeOfType<List.ListStatus>("Status List is of wrong type.");
                list.Status.ToString().ToLower().ShouldBe("current", "List Status is NOT set to completed");

                list.Mode.ShouldBeOfType<ListMode>("Mode List is of wrong type.");
                list.Mode.ToString().ToLower().ShouldBe("snapshot", "List Status is NOT set to completed");

                list.Code.Coding.ForEach(coding =>
                {
                    coding.System.ShouldBeOneOf("http://snomed.info/sct", "http://read.info/readv2", "http://read.info/ctv3", "https://fhir.hl7.org.uk/Id/emis-drug-codes", "https://fhir.hl7.org.uk/Id/egton-codes", "https://fhir.hl7.org.uk/Id/multilex-drug-codes", "https://fhir.hl7.org.uk/Id/resipuk-gemscript-drug-codes");
                    coding.Code.ShouldBe("826501000000100", "Code is not Correct");
                    coding.Display.ShouldNotBeNullOrEmpty("Display Should not be Null or Empty");
                });

                Patients.Where(p => p.Id == (list.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");

                //check number of Observations matches number in list
                if (Observations.Count() != list.Entry.Count())
                {
                    Observations.Count().ShouldBe(list.Entry.Count(), "Number of Observations does not match the number in the List");
                }
                else
                {
                    list.Entry.ForEach(entry =>
                    {
                        string guidToFind = entry.Item.Reference.Replace("Observation/", "");
                        Observations.Where(i => i.Id == guidToFind).Count().ShouldBe(1, "Not Found Reference to Observation");
                    });
                }
            }
            else
            {
                obvList.Count().ShouldBe(1, "Expected One Observation List But Found Zero or more than 1");
            }
        }
    }
}

