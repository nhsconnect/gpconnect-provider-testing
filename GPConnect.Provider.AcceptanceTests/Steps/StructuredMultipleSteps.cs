namespace GPConnect.Provider.AcceptanceTests.Steps
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using TechTalk.SpecFlow;
	using GPConnect.Provider.AcceptanceTests.Steps;
	using Shouldly;
	using GPConnect.Provider.AcceptanceTests.Helpers;
	using GPConnect.Provider.AcceptanceTests.Logger;
	using Constants;
	using Context;
	using static Hl7.Fhir.Model.Parameters;
	using Hl7.Fhir.Model;


	[Binding]
	public sealed class StructuredMultipleSteps : BaseSteps
	{
		private readonly HttpContext _httpContext;

		public StructuredMultipleSteps(HttpSteps httpSteps, HttpContext httpContext)
			: base(httpSteps)
		{
			_httpContext = httpContext;
		}

		[Then(@"Check the operation outcome returns the correct text and diagnotics ""([^""]*)""")]
		public void checkTheOperationOutcomeCheckThatDiagnosticReturnsRelevant(string parameters)
		{
			var entries = _httpContext.FhirResponse.Entries;
			entries.ForEach(entry =>
			{

				if (entry.Resource.ResourceType.ToString() == "OperationOutcome")
				{

					foreach (var issue in ((Hl7.Fhir.Model.OperationOutcome)entry.Resource).Issue)
					{
						issue.Code.ToString().ShouldBe("NotSupported");
						issue.Severity.ToString().ShouldBe("Warning");

						issue.Details.Coding[0].System.ShouldBe("https://fhir.nhs.uk/STU3/CodeSystem/Spine-ErrorOrWarningCode-1");
						issue.Details.Coding[0].Code.ShouldBe("NOT_IMPLEMENTED");
						issue.Details.Coding[0].Display.ShouldBe("Not implemented");
						issue.Details.Text.ShouldContain(parameters + " is an unrecognised parameter");
						issue.Diagnostics.ShouldContain(parameters);

						Log.WriteLine("The response is not returning the expected value for " + parameters);

					}
				}
			});
		}
		[Then(@"Check the operation outcome for each unsupported structured request")]
		public void checkTheOperationOutcomeForEachUnsupportedStructuredRequest()
		{
			var entries = _httpContext.FhirResponse.Entries;
			string paramsToCheck = "includeImmunisations,includeProblems,includeConsultations,includeUncategorisedData, include a rubbish one";

			var parameterList = paramsToCheck.Split(',');
			entries.ForEach(entry =>
			{

				if (entry.Resource.ResourceType.ToString() == "OperationOutcome")
				{

					foreach (var param in parameterList)
					{

						bool found = false;

						foreach (var issue in ((Hl7.Fhir.Model.OperationOutcome)entry.Resource).Issue)
						{
							if (issue.Details.Text == (param + " is an unrecognised parameter"))
							{
								found = true;
								break;
							}

							//issue.Code.ToString().ShouldBe("NotSupported");
							//issue.Severity.ToString().ShouldBe("Warning");

							//issue.Details.Coding[0].System.ShouldBe("https://fhir.nhs.uk/STU3/CodeSystem/Spine-ErrorOrWarningCode-1");
							//issue.Details.Coding[0].Code.ShouldBe("NOT_IMPLEMENTED");
							//issue.Details.Coding[0].Display.ShouldBe("Not implemented");
							//issue.Details.Text.ShouldContain(parameters + " is an unrecognised parameter");
							//issue.Diagnostics.ShouldContain(parameters);

							//Log.WriteLine("The response is not returning the expected value for " + parameters);

						}

						if (!found)
						{
							Log.WriteLine("operation outcome issue not found for:" + param);
							found.ShouldBeTrue("operation outcome issue not found for:" + param);
						}
						else
						{
							Log.WriteLine("operation outcome issue not found for:" + param);

						}
					}
				}
			}
			);
		}

		[Then(@"Check the operation outcome returns the correct text and diagnotics ""([^""]*)"" and ""([^""]*)""")]
		public void checkTheOperationOutcomeCheckThatDiagnosticReturnsRelevant(string parameter, string partparameter)
		{
			var entries = _httpContext.FhirResponse.Entries;

			entries.ForEach(entry =>
			{

				if (entry.Resource.ResourceType.ToString() == "OperationOutcome")
				{

					foreach (var issue in ((Hl7.Fhir.Model.OperationOutcome)entry.Resource).Issue)
					{

						issue.Code.ToString().ShouldBe("NotSupported");
						issue.Severity.ToString().ShouldBe("Warning");

						issue.Details.Coding[0].System.ShouldBe("https://fhir.nhs.uk/STU3/CodeSystem/Spine-ErrorOrWarningCode-1");
						issue.Details.Coding[0].Code.ShouldBe("NOT_IMPLEMENTED");
						issue.Details.Coding[0].Display.ShouldBe("Not implemented");
						issue.Details.Text.ShouldBe(parameter + "." + partparameter + " is an unrecognised parameter");
						issue.Diagnostics.ShouldBe(parameter + "." + partparameter);
						Log.WriteLine("The response is not returning the expected value for " + parameter);

					}

				}
			});
		}


		[Given(@"The request only contains mandatory parameters")]
		public void GivenTheRequestOnlyContainsMandatoryParameters()
		{
			Given($"I add the allergies parameter with resolvedAllergies set to \"false\"");
			Given($"I add the medication parameter with includePrescriptionIssues set to \"false\"");
		}

		[Given(@"I send a request that contains all forward compatable structured parameters with optional parameters")]
		public void GivenISendARequestThatContainsAllForwardCompatibleStructuredParametersWithOptionalParameters()
		{
			Given($"I add the allergies parameter with resolvedAllergies set to \"true\"");
			Given($"I add the immunisations parameter");
			Given($"I add the uncategorised parameter with optional parameters");
			Given($"I add the consultation parameter with optional parameters");
			Given($"I add the problems parameter with optional parameters");
		}
		[Given(@"I add the immunisations parameter")]
		public void GivenIAddTheImmunisationsParameter()
		{
			ParameterComponent param = new ParameterComponent();
			param.Name = FhirConst.GetStructuredRecordParams.kImmunisations;
			_httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(param);
		}

		[Given(@"I add allergies parameter with invalid ""(.*)""")]
		public void GivenIAddAllergiesParameterWithInvalid(string rubbishPartParameter)
		{
			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[]
			{
				Tuple.Create("RubbishPartParameter", (Base)new FhirBoolean(true))
				};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kAllergies, tuples);

		}

		[Given(@"I add allergies parameter with invalid part parameter2nd")]
		public void GivenIAddAllergiesParameterWithInvalidPartParameter2Nd()
		{
			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[]
			{
				Tuple.Create(FhirConst.GetStructuredRecordParams.kResolvedAllergies, (Base)new FhirBoolean(null))
				};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kAllergies, tuples);

		}

		[Given(@"I add the uncategorised parameter with optional parameters")]
		public void GivenIAddTheUncategorisedParameter()
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

		[Given(@"I add the consultation parameter with optional parameters")]
		public void GivenIAddTheConsultationParameter()
		{
			var backDate = DateTime.UtcNow.AddDays(-10);
			var futureDate = DateTime.UtcNow.AddDays(5);
			var startDate = backDate.ToString("yyyy-MM-dd");
			var endDate = futureDate.ToString("yyyy-MM-dd");

			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kConsultationSearch, (Base)FhirHelper.GetTimePeriod(startDate, endDate)),
				Tuple.Create(FhirConst.GetStructuredRecordParams.kConsultationsMostRecent, (Base)new FhirString("3"))
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kConsultations, tuples);
		}

		[Given(@"I send an invalid Consultations parameter containing valid part parameters")]
		public void GivenISendAnInvalidConsultationsParameterContainingValidPartParameters()
		{
			var backDate = DateTime.UtcNow.AddDays(-10);
			var futureDate = DateTime.UtcNow.AddDays(5);
			var startDate = backDate.ToString("yyyy-MM-dd");
			var endDate = futureDate.ToString("yyyy-MM-dd");

			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kConsultationSearch, (Base)FhirHelper.GetTimePeriod(startDate, endDate)),
				Tuple.Create(FhirConst.GetStructuredRecordParams.kConsultationsMostRecent, (Base)new FhirString("3"))
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add("Consultations", tuples);
		}

		[Given(@"I add the problems parameter with optional parameters")]
		public void GivenIAddTheProblemsParameter()
		{
			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kProblemsStatus, (Base)new FhirString ("active")),
				Tuple.Create(FhirConst.GetStructuredRecordParams.kProblemsSignificance, (Base)new FhirString ("major"))
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kProblems, tuples);
		}

		//[Given(@"I send an invalid Consultations parameter invalid part parameters")]
		//public void GivenISendAnInvalidConsultationsParameterContainingValidPartParameters()
		//{
		//	var backDate = DateTime.UtcNow.AddDays(-10);
		//	var futureDate = DateTime.UtcNow.AddDays(5);
		//	var startDate = backDate.ToString("yyyy-MM-dd");
		//	var endDate = futureDate.ToString("yyyy-MM-dd");

		//	IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
		//		Tuple.Create("ConsultationSearchPeriod"), (Base)new FhirBoolean(true)),
		//		Tuple.Create(FhirConst.GetStructuredRecordParams.kConsultationsMostRecent, (Base)new FhirString(null))
		//	};
		//	_httpContext.HttpRequestConfiguration.BodyParameters.Add("Consultations", tuples);
		//}

		//[Given(@"I add allergies parameter with invalid ""(.*)""")]
		//public void GivenIAddAllergiesParameterWithInvalid(string rubbishPartParameter)
		//{
		//	IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[]
		//	{
		//		Tuple.Create("RubbishPartParameter", (Base)new FhirBoolean(true))
		//		};
		//	_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kAllergies, tuples);

		//}

	}
}

