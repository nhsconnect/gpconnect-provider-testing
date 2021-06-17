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
    using NUnit.Framework;

    [Binding]
    public sealed class StructuredMiscSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;

        public StructuredMiscSteps(HttpSteps httpSteps, HttpContext httpContext)
            : base(httpSteps)
        {
            _httpContext = httpContext;
        }

    }
}
