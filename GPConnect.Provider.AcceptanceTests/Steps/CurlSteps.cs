namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Text;
    using Constants;
    using Context;
    using CurlSharp;
    using Enum;
    using Helpers;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    public class CurlSteps : Steps
    {
        private readonly HttpSteps _httpSteps;
        private readonly HttpContext _httpContext;
        private JwtHelper _jwtHelper;
        private readonly SecuritySteps _securitySteps;
        private readonly SecurityContext _securityContext;

        public CurlSteps(HttpSteps httpSteps, HttpContext httpContext, JwtHelper jwtHelper, SecuritySteps securitySteps, SecurityContext securityContext)
        {
            _httpSteps = httpSteps;
            _httpContext = httpContext;
            _jwtHelper = jwtHelper;
            _securitySteps = securitySteps;
            _securityContext = securityContext;
        }

        [Given(@"I configure the default ""(.*)"" cURL request")]
        public void ConfigureRequest(GpConnectInteraction interaction)
        {
            _httpContext.SetDefaults();

            _httpContext.HttpRequestConfiguration = _httpSteps.GetHttpRequestConfiguration(interaction, _httpContext.HttpRequestConfiguration);

            _jwtHelper = _httpSteps.GetJwtHelper(interaction, _jwtHelper);

            _securitySteps.ConfigureServerCertificatesAndSsl();
        }

        [When(@"I make the ""(.*)"" cURL request")]
        public void MakeTheCurlRequestWithCipher(GpConnectInteraction interaction)
        {
            _httpContext.HttpRequestConfiguration = _httpSteps.GetRequestBody(interaction, _httpContext.HttpRequestConfiguration);

            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, _jwtHelper.GetBearerToken());

            var headers = _httpContext.HttpRequestConfiguration.RequestHeaders;
            var url = _httpContext.HttpRequestConfiguration.EndpointAddress + '/' + _httpContext.HttpRequestConfiguration.RequestUrl;
            var cipher = _securityContext.Cipher;

            Curl.GlobalInit(CurlInitFlag.All);

            var curlHeaders = GetHeaders(headers);
     
            var curlEasy = GetCurlEasy(url, cipher, curlHeaders);

            MakeCurlRequest(curlEasy);
        }

        private void MakeCurlRequest(CurlEasy curlEasy)
        {
            CurlCode curlCode;

            using (curlEasy)
            {
                curlCode = curlEasy.Perform();
            }

            _httpContext.HttpResponse.CurlCode = curlCode;
        }

        private CurlEasy GetCurlEasy(string url, string cipher, CurlSlist headers)
        {
            var curlEasy = new CurlEasy
            {       
                //TODO: Add certificates to cURL tests SslVerifyPeer = true, CaInfo = full path to certs
                WriteFunction = HandleResponse,
                SslVerifyPeer = false,
                SslVerifyhost = false,
                Url = url,
                SslCipherList = cipher,
                HttpHeader = headers
            };

            //TODO: Add certificates to cURL tests SslVerifyPeer
            if (_securityContext.SendClientCert)
            {
                curlEasy.SslCert = AppSettingsHelper.CurlClientCertificate;
                curlEasy.SslKey = AppSettingsHelper.CurlClientKey;
                curlEasy.SslKeyPasswd = AppSettingsHelper.CurlClientPassword;
            }

            return curlEasy;
        }

        private int HandleResponse(byte[] buf, int size, int nmemb, object extradata)
        {
            _httpContext.HttpResponse.Body = Encoding.UTF8.GetString(buf);
            _httpContext.HttpResponse.ContentType = ContentType.Application.FhirJson;
            _httpContext.FhirResponse.Resource = _httpContext.HttpResponse.ParseFhirResource().Resource;

            return size * nmemb;
        }

        private CurlSlist GetHeaders(HttpHeaderHelper httpHeaders)
        {
            var headers = new CurlSlist();

            foreach (var httpHeader in httpHeaders.GetRequestHeaders())
            {
                headers.Append($"{httpHeader.Key}: {httpHeader.Value}");
            }

            return headers;
        }

        [Then(@"the cURL Code should be ""(.*)""")]
        private void TheCurlCodeShouldBe(CurlCode curlCode)
        {
            _httpContext.HttpResponse.CurlCode.ShouldBe(curlCode, $"The cURL Code should be {curlCode} but was {_httpContext.HttpResponse.CurlCode}");
        }
    }
}
