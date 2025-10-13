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
            var futureStartDate = DateTime.UtcNow.AddDays(+10);
            var futureEndDate = DateTime.UtcNow.AddDays(+15);
            var startDate = futureStartDate.ToString("yyyy-MM-dd");
            var endDate = futureEndDate.ToString("yyyy-MM-dd");

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

        [Given(@"I add the uncategorised data parameter with valid start date and no end date")]
        public void GivenIAddTheUncategorisedParameterWithValidStartDateAndNoEndDate()
        {
            var backDate = DateTime.UtcNow.AddYears(-3);
            var startDate = backDate.ToString("yyyy-MM-dd");

            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kUncategorisedData, (Base)FhirHelper.SafeGetTimePeriod(startDate, null)),
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kUncategorisedData, tuples);
        }

        [Given(@"I add the uncategorised data parameter with no start date and no end date")]
        public void GivenIAddTheUncategorisedParameterWithNoStartDateAndNoEndDate()
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kUncategorisedData, (Base)FhirHelper.SafeGetTimePeriod(null, null)),
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kUncategorisedData, tuples);
        }

        [Then(@"The Observation Resources are Valid")]
        public void GivenTheObservationResourcesareValid()
        {
            //check atleast one
            Observations.ToList().Count().ShouldBeGreaterThan(0, "Error Should be Atleast One Observation in response as per Data requirements");

            Observations.ForEach(observation =>
            {
                //Check Id
                observation.Id.ShouldNotBeNullOrEmpty();

                //Check Meta.profile
                CheckForValidMetaDataInResource(observation, FhirConst.StructureDefinitionSystems.kObservation);

                //Chck Status
                observation.Status.ToString().ShouldBe("final", StringCompareShould.IgnoreCase);

                //Check Patient
                Patients.Where(p => p.Id == (observation.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");

                //Check Code
                observation.Code.ShouldNotBeNull("Code Element should not be null");

                //Check Identifier
                observation.Identifier.Count.ShouldBeGreaterThan(0, "There should be at least 1 Identifier system/value pair");
                observation.Identifier.ForEach(identifier =>
                {
                    identifier.System.ShouldNotBeNullOrEmpty("Identifier System Is Null or Empty - Should be populated");
                    identifier.Value.ShouldNotBeNullOrEmpty("Identifier Value Is Null or Not Valid");
                });

                //Check Issued
                observation.Issued.ShouldNotBeNull("Issued is Mandatory Fields and Should be included in th payload");

                //Check Performer
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
            //Check there is ONE Observation snomed code
            Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kUncategorised).ToList().Count().ShouldBe(1, "Failed to Find ONE Uncategorised  list using Snomed Code.");

            //Get Var to List
            var obvList = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kUncategorised).First();

            //Check title
            obvList.Title.ShouldBe("Uncategorised data", "List Title is Incorrect");
            CheckForValidMetaDataInResource(obvList, FhirConst.StructureDefinitionSystems.kList);

            //Check Status
            obvList.Status.ShouldBeOfType<List.ListStatus>("Status List is of wrong type.");
            obvList.Status.ToString().ToLower().ShouldBe("current", "List Status is NOT set to completed");

            //Check Mode
            obvList.Mode.ShouldBeOfType<ListMode>("Mode List is of wrong type.");
            obvList.Mode.ToString().ToLower().ShouldBe("snapshot", "List Status is NOT set to completed");

            //Check Code
            obvList.Code.Coding.ForEach(coding =>
            {
                coding.System.ShouldBeOneOf("http://snomed.info/sct", "http://read.info/readv2", "http://read.info/ctv3", "https://fhir.hl7.org.uk/Id/emis-drug-codes", "https://fhir.hl7.org.uk/Id/egton-codes", "https://fhir.hl7.org.uk/Id/multilex-drug-codes", "https://fhir.hl7.org.uk/Id/resipuk-gemscript-drug-codes");
                coding.Display.ShouldNotBeNullOrEmpty("Display Should not be Null or Empty");
            });

            //Check Patient
            Patients.Where(p => p.Id == (obvList.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");

            //check number of Observations matches number in list
            if (Observations.Count() != obvList.Entry.Count())
            {
                Observations.Count().ShouldBe(obvList.Entry.Count(), "Number of Observations does not match the number in the List");
            }
            else
            {
                obvList.Entry.ForEach(entry =>
                {
                    string guidToFind = entry.Item.Reference.Replace("Observation/", "");
                    Observations.Where(i => i.Id == guidToFind).Count().ShouldBe(1, "Not Found Reference to Observation");
                });
            }
        }
    }
}

