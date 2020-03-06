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
    public sealed class StructuredInvestigationsSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private List<Immunization> Immunizations => _httpContext.FhirResponse.Immunizations;
        private List<List> Lists => _httpContext.FhirResponse.Lists;
        private List<Patient> Patients => _httpContext.FhirResponse.Patients;

        public StructuredInvestigationsSteps(HttpSteps httpSteps, HttpContext httpContext)
            : base(httpSteps)
        {
            _httpContext = httpContext;
        }

                 
        [Given(@"I add the Investigations parameter")]
        public void GivenIAddTheInvestigationsParameter()
        {
            ParameterComponent param = new ParameterComponent();
            param.Name = FhirConst.GetStructuredRecordParams.kInvestigations;
            _httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(param);
        }


    }
}