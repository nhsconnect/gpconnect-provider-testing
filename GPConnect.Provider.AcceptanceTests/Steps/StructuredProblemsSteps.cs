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
    using System.Text.RegularExpressions;

    [Binding]
    public sealed class StructuredProblemsSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private List<List> Lists => _httpContext.FhirResponse.Lists;
        private List<Patient> Patients => _httpContext.FhirResponse.Patients;
        private List<Condition> Problems => _httpContext.FhirResponse.Conditions;

        public StructuredProblemsSteps(HttpSteps httpSteps, HttpContext httpContext)
            : base(httpSteps)
        {
            _httpContext = httpContext;
        }


        [Given(@"I add the Problems parameter")]
        public void GivenIAddTheProblemsParameter()
        {
            ParameterComponent param = new ParameterComponent();
            param.Name = FhirConst.GetStructuredRecordParams.kProblems;
            _httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(param);
        }

		[Given(@"I add the problems parameter with filterStatus ""(.*)""")]
		public void GivenIAddTheProblemsParameterWithfilterStatus(string value)
		{
			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kProblemsStatus, (Base)new Code (value))
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kProblems, tuples);
		}
		
		[Given(@"I add the problems parameter with filterSignificance ""(.*)""")]
		public void GivenIAddTheProblemsParameterWithFilterSignificance(string value)
		{
			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kProblemsSignificance, (Base)new Code (value))
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kProblems, tuples);
		}

		[Given(@"I add the problems parameter including status and significance value ""([^ ""]*)"" ""([^ ""]*)""")]
		public void GivenIAddTheProblemsParameterIncludingStatusAndSignificanceValue(string statusValue , string sigValue)
		{
			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kProblemsStatus, (Base)new Code (statusValue)),
				Tuple.Create(FhirConst.GetStructuredRecordParams.kProblemsSignificance, (Base)new Code (sigValue ))
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kProblems, tuples);
		}

		[Given(@"I add the problems parameter including repeating filter pairs")]
		public void GivenIAddTheProblemsParameterIncludingRepeatingFilterPairs()
		{

			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kProblemsStatus, (Base)new Code ("active")),
				Tuple.Create(FhirConst.GetStructuredRecordParams.kProblemsSignificance, (Base)new Code ("minor")),
							};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kProblems, tuples);

			IEnumerable<Tuple<string, Base>> tuples2 = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kProblemsStatus, (Base)new Code("inactive")),
				Tuple.Create(FhirConst.GetStructuredRecordParams.kProblemsSignificance, (Base)new Code("major")),
					
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kProblems, tuples2);
		}
				

		[Given(@"I add a madeUpProblems part parameter")]
		public void GivenIAddAMadeUpProblemsPartParameter()
		{
			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create("madeUpProblems", (Base)new Code ("madeUpProblemsValue1")),
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kProblems, tuples);

		}
	}
}



