using System.Linq;
using GPConnect.Provider.AcceptanceTests.Constants;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Context;
    using Enum;
    using GPConnect.Provider.AcceptanceTests.Helpers;
    using GPConnect.Provider.AcceptanceTests.Http;
    using Hl7.Fhir.Model;
    using Repository;
    using Shouldly;
    using TechTalk.SpecFlow;
    using static Hl7.Fhir.Model.Parameters;

    [Binding]
    public class StructuredDiarySteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private readonly BundleSteps _bundleSteps;
        private readonly JwtSteps _jwtSteps;
        private readonly HttpRequestConfigurationSteps _httpRequestConfigurationSteps;
        private readonly IFhirResourceRepository _fhirResourceRepository;

        private List<Patient> Patients => _httpContext.FhirResponse.Patients;

        public StructuredDiarySteps(HttpContext httpContext, HttpSteps httpSteps, BundleSteps bundleSteps, JwtSteps jwtSteps, HttpRequestConfigurationSteps httpRequestConfigurationSteps, IFhirResourceRepository fhirResourceRepository)
            : base(httpSteps)
        {
            _httpContext = httpContext;
            _bundleSteps = bundleSteps;
            _jwtSteps = jwtSteps;
            _httpRequestConfigurationSteps = httpRequestConfigurationSteps;
            _fhirResourceRepository = fhirResourceRepository;
        }


        [Given(@"I add the Diary parameter")]
        public void GivenIAddTheInvestigationsParameter()
        {
            ParameterComponent param = new ParameterComponent();
            param.Name = FhirConst.GetStructuredRecordParams.kDiary;
            _httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(param);
        }

        //[Given(@"I add the investigations data parameter with future start date")]
        //public void GivenIAddTheinvestigationsDataParameterWithFutureStartDate()
        //{
        //    var futureStartDate = DateTime.UtcNow.AddDays(+10);
        //    var futureEndDate = DateTime.UtcNow.AddDays(+15);
        //    var startDate = futureStartDate.ToString("yyyy-MM-dd");
        //    var endDate = futureEndDate.ToString("yyyy-MM-dd");

        //    IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
        //        Tuple.Create(FhirConst.GetStructuredRecordParams.kInvestigationsSearch, (Base)FhirHelper.GetTimePeriod(startDate, endDate)),
        //    };
        //    _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kInvestigations, tuples);
        //}


    }
}

