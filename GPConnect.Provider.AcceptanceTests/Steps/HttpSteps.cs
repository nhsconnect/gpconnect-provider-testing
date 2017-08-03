namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using Constants;
    using Context;
    using Helpers;
    using Logger;
    using TechTalk.SpecFlow;
    using Hl7.Fhir.Model;
    using Enum;
    using Factories;
    using Http;
    using Repository;

    [Binding]
    public class HttpSteps : Steps
    {
        private readonly HttpContext _httpContext;
        private readonly JwtHelper _jwtHelper;
        private readonly SecuritySteps _securitySteps;
        private readonly SecurityContext _securityContext;
        private readonly IFhirResourceRepository _fhirResourceRepository;

        public HttpSteps(HttpContext httpContext, JwtHelper jwtHelper, SecuritySteps securitySteps, SecurityContext securityContext, IFhirResourceRepository fhirResourceRepository)
        {
            Log.WriteLine("HttpSteps() Constructor");
            _httpContext = httpContext;
            _jwtHelper = jwtHelper;
            _securitySteps = securitySteps;
            _securityContext = securityContext;
            _fhirResourceRepository = fhirResourceRepository;
        }

        [Given(@"I configure the default ""(.*)"" request")]
        public void ConfigureRequest(GpConnectInteraction interaction)
        {
            _httpContext.SetDefaults();

            var httpRequestConfigurationFactory = new HttpRequestConfigurationFactory(interaction, _httpContext.HttpRequestConfiguration);

            _httpContext.HttpRequestConfiguration = httpRequestConfigurationFactory.GetHttpRequestConfiguration();

            var jwtFactory = new JwtFactory(interaction);

            jwtFactory.ConfigureJwt(_jwtHelper, _httpContext.HttpRequestConfiguration);

            _securitySteps.ConfigureServerCertificatesAndSsl();
        }

        [When(@"I make the ""(.*)"" request")]
        public void MakeRequest(GpConnectInteraction interaction)
        {
            var requestFactory = new RequestFactory(interaction, _fhirResourceRepository);

            requestFactory.ConfigureBody(_httpContext.HttpRequestConfiguration);

            if (!string.IsNullOrEmpty(_httpContext.HttpRequestConfiguration.RequestHeaders.GetHeaderValue(HttpConst.Headers.kAuthorization)))
            {
                _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerToken());
            }

            var httpRequest = new HttpRequest(_httpContext, _securityContext);

            httpRequest.MakeHttpRequest();
        }

        [When(@"I make the ""(.*)"" request with an unencoded JWT Bearer Token")]
        public void MakeRequestWithAnUnencodedJwtBearerToken(GpConnectInteraction interaction)
        {
            var requestFactory = new RequestFactory(interaction, _fhirResourceRepository);

            requestFactory.ConfigureBody(_httpContext.HttpRequestConfiguration);

            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerTokenWithoutEncoding());

            var httpRequest = new HttpRequest(_httpContext, _securityContext);

            httpRequest.MakeHttpRequest();
        }

        [When(@"I make the ""(.*)"" request with invalid Resource type")]
        public void MakeRequestWithInvalidResourceType(GpConnectInteraction interaction)
        {
            var requestFactory = new RequestFactory(interaction, _fhirResourceRepository);

            requestFactory.ConfigureBody(_httpContext.HttpRequestConfiguration);
            requestFactory.ConfigureInvalidResourceType(_httpContext.HttpRequestConfiguration);

            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerToken());

            var httpRequest = new HttpRequest(_httpContext, _securityContext);

            httpRequest.MakeHttpRequest();
        }

        [When(@"I make the ""(.*)"" request with Invalid Additional Field in the Resource")]
        public void MakeRequestWithInvalidAdditionalFieldInTheResource(GpConnectInteraction interaction)
        {
            var requestFactory = new RequestFactory(interaction, _fhirResourceRepository);

            requestFactory.ConfigureBody(_httpContext.HttpRequestConfiguration);
            requestFactory.ConfigureAdditionalInvalidFieldInResource(_httpContext.HttpRequestConfiguration);

            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerToken());

            var httpRequest = new HttpRequest(_httpContext, _securityContext);

            httpRequest.MakeHttpRequest();
        }

        [When(@"I make the ""(.*)"" request with invalid parameter Resource type")]
        public void MakeRequestWithInvalidParameterResourceType(GpConnectInteraction interaction)
        {
            var requestFactory = new RequestFactory(interaction, _fhirResourceRepository);
            requestFactory.ConfigureBody(_httpContext.HttpRequestConfiguration);
            requestFactory.ConfigureInvalidParameterResourceType(_httpContext.HttpRequestConfiguration);
            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerToken());

            var httpRequest = new HttpRequest(_httpContext, _securityContext);

            httpRequest.MakeHttpRequest();
        }

        [When(@"I make the ""(.*)"" request with additional field in parameter Resource")]
        public void MakeRequestWithAdditionalFieldInParameterResource(GpConnectInteraction interaction)
        {
            var requestFactory = new RequestFactory(interaction, _fhirResourceRepository);
            requestFactory.ConfigureBody(_httpContext.HttpRequestConfiguration);
            requestFactory.ConfigureParameterResourceWithAdditionalField(_httpContext.HttpRequestConfiguration);
            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerToken());

            var httpRequest = new HttpRequest(_httpContext, _securityContext);

            httpRequest.MakeHttpRequest();
        }

        public Resource GetResourceForRelativeUrl(GpConnectInteraction gpConnectInteraction, string relativeUrl)
        {
            var httpContext = new HttpContext();
            httpContext.SetDefaults();
            httpContext.HttpRequestConfiguration.SetDefaultHeaders();

            var httpContextFactory = new HttpRequestConfigurationFactory(gpConnectInteraction, httpContext.HttpRequestConfiguration);
            httpContextFactory.GetHttpRequestConfiguration();
            
            var jwtHelper = new JwtHelper();
            var jwtFactory = new JwtFactory(gpConnectInteraction);

            jwtFactory.ConfigureJwt(jwtHelper, httpContext.HttpRequestConfiguration);

            if (relativeUrl.Contains("Patient"))
            {
                var patient = relativeUrl.ToLower().Replace("/", string.Empty);
                jwtHelper.RequestedPatientNHSNumber = GlobalContext.PatientNhsNumberMap[patient];
            }

            _securitySteps.ConfigureServerCertificatesAndSsl();

            httpContext.HttpRequestConfiguration.RequestUrl = relativeUrl;

            var requestFactory = new RequestFactory(gpConnectInteraction, _fhirResourceRepository);
            requestFactory.ConfigureBody(_httpContext.HttpRequestConfiguration);

            httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, jwtHelper.GetBearerToken());

            var httpRequest = new HttpRequest(httpContext, _securityContext);

            httpRequest.MakeHttpRequest();

            return httpContext.FhirResponse.Resource;
        }
    }
}
