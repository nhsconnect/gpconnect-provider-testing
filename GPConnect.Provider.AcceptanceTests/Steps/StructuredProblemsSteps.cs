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

    }
}
