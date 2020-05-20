using System.Linq;
using GPConnect.Provider.AcceptanceTests.Constants;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System;
    using System.Collections.Generic;
    using Context;
    using Enum;
    using Hl7.Fhir.Model;
    using Repository;
    using Shouldly;
    using TechTalk.SpecFlow;
    using static Hl7.Fhir.Model.Slot;

    [Binding]
    public class DocumentsSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private readonly BundleSteps _bundleSteps;
        private readonly JwtSteps _jwtSteps;
        private readonly HttpRequestConfigurationSteps _httpRequestConfigurationSteps;
        private readonly IFhirResourceRepository _fhirResourceRepository;

        private List<Slot> Slots => _httpContext.FhirResponse.Slots;
        private List<Schedule> Schedules => _httpContext.FhirResponse.Schedules;

        public DocumentsSteps(HttpContext httpContext, HttpSteps httpSteps, BundleSteps bundleSteps, JwtSteps jwtSteps, HttpRequestConfigurationSteps httpRequestConfigurationSteps, IFhirResourceRepository fhirResourceRepository)
            : base(httpSteps)
        {
            _httpContext = httpContext;
            _bundleSteps = bundleSteps;
            _jwtSteps = jwtSteps;
            _httpRequestConfigurationSteps = httpRequestConfigurationSteps;
            _fhirResourceRepository = fhirResourceRepository;
        }


        [Given(@"I set the required parameters for a Documents Search call")]
        public void SetRequiredParametersWithTimePeriod()
        {
            //_httpRequestConfigurationSteps.GivenIAddTheTimePeriodParametersforDaysStartingTomorrowWithStartEndPrefix(days, "ge", "le");
            //_httpRequestConfigurationSteps.GivenIAddTheParameterWithTheValue("status", "free");
            //_httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("searchFilter", "https://fhir.nhs.uk/Id/ods-organization-code" + '|' + GlobalContext.OdsCodeMap["ORG1"]);
            //_httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("searchFilter", "https://fhir.nhs.uk/STU3/CodeSystem/GPConnect-OrganisationType-1" + '|' + "urgent-care");
            

            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("_include", "DocumentReference:subject:Patient");
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("_include", "DocumentReference:custodian:Organization");
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("_include", "DocumentReference:author:Organization");
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("_include", "DocumentReference:author:Practitioner");
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter("_revinclude:recurse", "PractitionerRole:practitioner");

        }

    }
}
