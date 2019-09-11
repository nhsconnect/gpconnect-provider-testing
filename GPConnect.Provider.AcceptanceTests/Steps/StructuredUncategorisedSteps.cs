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
	}
}

