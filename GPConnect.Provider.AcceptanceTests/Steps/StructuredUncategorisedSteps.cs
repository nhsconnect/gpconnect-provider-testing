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



        [Given(@"I add the Uncategorised parameter")]
        public void GivenIAddTheUncategorisedParameter()
        {
            ParameterComponent param = new ParameterComponent();
            param.Name = FhirConst.GetStructuredRecordParams.kUncategorised;
            _httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(param);
        }


    }
}

