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
    public sealed class StructuredCommonSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private List<Immunization> Immunizations => _httpContext.FhirResponse.Immunizations;
        private List<List> Lists => _httpContext.FhirResponse.Lists;
        private List<Patient> Patients => _httpContext.FhirResponse.Patients;

        public StructuredCommonSteps(HttpSteps httpSteps, HttpContext httpContext)
            : base(httpSteps)
        {
            _httpContext = httpContext;
        }
    }
}