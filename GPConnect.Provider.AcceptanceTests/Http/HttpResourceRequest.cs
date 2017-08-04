namespace GPConnect.Provider.AcceptanceTests.Http
{
    using Context;

    public class HttpResourceRequest : HttpRequestBase
    {
        public HttpResourceRequest(HttpRequestConfiguration httpRequestConfiguration, SecurityContext securityContext)
            : base(httpRequestConfiguration, securityContext)
        {
        }

        public FhirResponse MakeRequest()
        {
            var httpClient = GetHttpClient();

            var requestMessage = GetHttpRequestMessage();

            var result = httpClient.SendAsync(requestMessage).Result;

            var httpResponse = GetHttpResponse(result);

            return httpResponse.ParseFhirResource();
        }
    }
}
