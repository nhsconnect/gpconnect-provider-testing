using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Logger;
using Newtonsoft.Json.Linq;
using RestSharp;
using Shouldly;
using TechTalk.SpecFlow;
using Hl7.Fhir.Serialization;
using Hl7.Fhir.Model;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable InconsistentNaming

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Text;

    [Binding]
    public class HttpSteps : TechTalk.SpecFlow.Steps
    {
        private readonly HttpContext HttpContext;
        private readonly FhirContext FhirContext;

        // Constructor

        public HttpSteps(HttpContext httpContext, FhirContext fhirContext)
        {
            Log.WriteLine("HttpSteps() Constructor");
            HttpContext = httpContext;
            FhirContext = fhirContext;
        }

        // Before Scenarios

        [BeforeScenario(Order = 3)]
        public void LoadAppConfig()
        {
            HttpContext.LoadAppConfig();
        }
        
        [BeforeScenario(Order = 3)]
        public void ClearHeaders()
        {
            HttpContext.RequestHeaders.Clear();
        }

        [BeforeScenario(Order = 3)]
        public void ClearParameters()
        {
            HttpContext.RequestParameters.ClearParameters();
        }

        // Security Validation Steps

        [Then(@"the response status code should indicate authentication failure")]
        public void ThenTheResponseStatusCodeShouldIndicateAuthenticationFailure()
        {
            HttpContext.ResponseStatusCode.ShouldBe(HttpStatusCode.Forbidden);
            Log.WriteLine("Response HttpStatusCode={0}", HttpContext.ResponseStatusCode);
        }

        [Then(@"the response status code should be ""(.*)""")]
        public void ThenTheResponseStatusCodeShouldBe(string statusCode)
        {
            ((int)HttpContext.ResponseStatusCode).ToString().ShouldBe(statusCode);
            Log.WriteLine("Response HttpStatusCode should be {0} but was {1}", statusCode, HttpContext.ResponseStatusCode);
        }

        // Provider Configuration Steps

        [Given(@"I am using the default server")]
        public void GivenIAmUsingTheDefaultServer()
        {
            // Load The Default Settings From The App.config File
            HttpContext.LoadAppConfig();            

            // Given(@"I configure server certificate and ssl");
            Given($@"I am using ""{FhirConst.ContentTypes.kJsonFhir}"" to communicate with the server");
            And(@"I am generating a random message trace identifier");
            And($@"I am accredited system ""{HttpContext.ConsumerASID}""");
            And($@"I am connecting to accredited system ""{HttpContext.ProviderASID}""");
            And(@"I am generating an organization JWT header");
        }
        
        [Given(@"I am connecting to server on port ""([^\s]*)""")]
        public void GivenIAmConnectingToServerOnPort(string serverPort)
        {
            HttpContext.FhirServerPort = serverPort;
        }
        
        // Http Header Configuration Steps

        [Given(@"I am using ""(.*)"" to communicate with the server")]
        public void GivenIAmUsingToCommunicateWithTheServer(string requestContentType)
        {
            HttpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAccept, requestContentType);
            HttpContext.RequestContentType = requestContentType;
        }

        [Given(@"I set ""(.*)"" request header to ""(.*)""")]
        public void GivenISetRequestHeaderTo(string headerKey, string headerValue)
        {
            HttpContext.RequestHeaders.ReplaceHeader(headerKey, headerValue);
        }

        [Given(@"I am accredited system ""(.*)""")]
        public void GivenIAmAccreditedSystem(string fromASID)
        {
            HttpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspFrom, fromASID);
        }

        [Given(@"I am performing the ""(.*)"" interaction")]
        public void GivenIAmPerformingTheInteraction(string interactionId)
        {
            HttpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, interactionId);
        }

        [Given(@"I am connecting to accredited system ""(.*)""")]
        public void GivenIConnectingToAccreditedSystem(string toASID)
        {
            HttpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspTo, toASID);
        }

        [Given(@"I am generating a random message trace identifier")]
        public void GivenIAmGeneratingARandomMessageTraceIdentifier()
        {
            HttpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspTraceId, Guid.NewGuid().ToString(""));
        }

        [Given(@"I am generating an organization JWT header")]
        public void GivenIAmGeneratingAnOrganizationAuthorizationHeader()
        {
            HttpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAuthorization, HttpContext.Jwt.GetBearerToken());
        }
        
        [Given(@"I do not send header ""(.*)""")]
        public void GivenIDoNotSendHeader(string headerKey)
        {
            HttpContext.RequestHeaders.RemoveHeader(headerKey);
        }

        [Given(@"I ask for the contents to be gzip encoded")]
        public void GivenIAskForTheContentsToBeGZipEncoded()
        {
            HttpContext.RequestHeaders.AddHeader(HttpConst.Headers.kAcceptEncoding, "gzip");
        }

        // Http Request Steps
        
        [Given(@"I set the request content type to ""(.*)""")]
        public void GivenISetTheRequestTypeTo(string requestContentType)
        {
            HttpContext.RequestContentType = requestContentType;
        }

        [Given(@"I set the Accept header to ""(.*)""")]
        public void GivenISetTheAcceptHeaderTo(string acceptContentType)
        {
            HttpContext.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAccept, acceptContentType);
        }

        [Given(@"I add the parameter ""(.*)"" with the value ""(.*)""")]
        public void GivenIAddTheParameterWithTheValue(string parameterName, string parameterValue)
        {
            HttpContext.RequestParameters.AddParameter(parameterName, parameterValue);
        }

        [When(@"I make a GET request to ""(.*)""")]
        public void WhenIMakeAGETRequestTo(string relativeUrl)
        {
            RestRequest(Method.GET, relativeUrl);
        }

        [When(@"I make a POST request to ""(.*)""")]
        public void WhenIMakeAPOSTRequestTo(string relativeUrl)
        {
            RestRequest(Method.POST, relativeUrl);
        }

        [When(@"I make a PUT request to ""(.*)""")]
        public void WhenIMakeAPUTRequestTo(string relativeUrl)
        {
            RestRequest(Method.PUT, relativeUrl);
        }

        [When(@"I make a PATCH request to ""(.*)""")]
        public void WhenIMakeAPATCHRequestTo(string relativeUrl)
        {
            RestRequest(Method.PATCH, relativeUrl);
        }

        [When(@"I make a DELETE request to ""(.*)""")]
        public void WhenIMakeADELETERequestTo(string relativeUrl)
        {
            RestRequest(Method.DELETE, relativeUrl);
        }

        [When(@"I make a OPTIONS request to ""(.*)""")]
        public void WhenIMakeAOPTIONSRequestTo(string relativeUrl)
        {
            RestRequest(Method.OPTIONS, relativeUrl);
        }

        // Rest Request Helper

        private void RestRequest(Method method, string relativeUrl, string body = null)
        {
            var timer = new System.Diagnostics.Stopwatch();

            Log.WriteLine("{0} relative URL = {1}", method, relativeUrl);

            // Save The Request Details
            HttpContext.RequestMethod = method.ToString();
            HttpContext.RequestUrl = relativeUrl;
            HttpContext.RequestBody = body;

            // Build The Rest Request
            var restClient = new RestClient(HttpContext.EndpointAddress);

            restClient.Encoding = Encoding.UTF8;

            var restRequest = new RestRequest(relativeUrl, method);

            // Setup The Web Proxy
            if (HttpContext.UseWebProxy)
            {
                restClient.Proxy = new WebProxy(new Uri(HttpContext.WebProxyAddress, UriKind.Absolute));
            }

            // Setup The Client Certificate
            if (HttpContext.SecurityContext.SendClientCert)
            {
                var clientCert = HttpContext.SecurityContext.ClientCert;
                if (restClient.ClientCertificates == null)
                {
                    restClient.ClientCertificates = new X509CertificateCollection();
                }
                restClient.ClientCertificates.Clear();
                restClient.ClientCertificates.Add(clientCert);
            }

            // Remove default handlers to stop it sending default Accept header
            restClient.ClearHandlers();

            // Set the Content-Type header
            restRequest.AddParameter(HttpContext.RequestContentType, body, ParameterType.RequestBody);
            HttpContext.RequestHeaders.AddHeader(HttpConst.Headers.kContentType, HttpContext.RequestContentType);

            // Add The Headers
            foreach (var header in HttpContext.RequestHeaders.GetRequestHeaders())
            {
                Log.WriteLine("Header - {0} -> {1}", header.Key, header.Value);
                restRequest.AddHeader(header.Key, header.Value);
            }

            // Add Parameters
            foreach (var parameter in HttpContext.RequestParameters.GetRequestParameters())
            {
                Log.WriteLine("Parameter - {0} -> {1}", parameter.Key, parameter.Value);
                restRequest.AddParameter(parameter.Key, parameter.Value);
            }

            // Execute The Request
            IRestResponse restResponse = null;
            try
            {
                // Start The Performance Timer Running
                timer.Start();

                // Perform The Rest Request
                restResponse = restClient.Execute(restRequest);
            }
            catch (Exception e) {
                Log.WriteLine(e.StackTrace);
            }
            finally
            {
                // Always Stop The Performance Timer Running
                timer.Stop();
            }

            // Save The Time Taken To Perform The Request
            HttpContext.ResponseTimeInMilliseconds = timer.ElapsedMilliseconds;

            // TODO Save The Error Message And Exception Details
            Log.WriteLine("Error Message = " + restResponse.ErrorMessage);
            Log.WriteLine("Error Exception = " + restResponse.ErrorException);

            // Save The Response Details
            HttpContext.ResponseStatusCode = restResponse.StatusCode;
            HttpContext.ResponseContentType = restResponse.ContentType;
            
            using (var reader = new StreamReader(new MemoryStream(restResponse.RawBytes), Encoding.UTF8))
            {
                HttpContext.ResponseBody = reader.ReadToEnd();
            }

            //HttpContext.ResponseBody = restResponse.Content;

            foreach(var parameter in restResponse.Headers)
            {
                Log.WriteLine("Response Header: " + parameter.Name + " : " + parameter.Value);
                HttpContext.ResponseHeaders.Add(parameter.Name, (string)parameter.Value);
            }
            
            // TODO Parse The XML or JSON For Easier Processing

            LogToDisk();
        }

        private void HttpRequest(HttpMethod method, string relativeUrl, string body = null, bool decompressGzip = false)
        {
            var timer = new System.Diagnostics.Stopwatch();

            // Save The Request Details
            HttpContext.RequestMethod = method.ToString();
            HttpContext.RequestUrl = relativeUrl;
            HttpContext.RequestBody = body;

            WebRequestHandler handler = new WebRequestHandler();

            if (decompressGzip)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip;
            }

            // Setup The Client Certificate
            if (HttpContext.SecurityContext.SendClientCert)
            {
                var clientCert = HttpContext.SecurityContext.ClientCert;                
                handler.ClientCertificates.Add(clientCert);

            }
            // Setup The Web Proxy
            if (HttpContext.UseWebProxy)
            {
                handler.Proxy = new WebProxy(new Uri(HttpContext.WebProxyAddress, UriKind.Absolute));
            }

            /*
            // Add Parameters
            foreach (var parameter in HttpContext.RequestParameters.GetRequestParameters())
            {
                Log.WriteLine("Parameter - {0} -> {1}", parameter.Key, parameter.Value);
                .AddParameter(parameter.Key, parameter.Value);
            }
            */
            
            var sspAddress = HttpContext.UseSpineProxy ? HttpContext.SpineProxyAddress + "/" : string.Empty;

            //string baseUrl = sspAddress + HttpContext.Protocol + HttpContext.FhirServerUrl + ":" + HttpContext.FhirServerPort + HttpContext.FhirServerFhirBase;

            // PG - 27/3/2019 - SSP has been upgraded and will not allow a port number in the URL - so change below removes port when UseTls is true in app.config             
            string baseUrl;

            if (HttpContext.SecurityContext.UseTLS)
            {
                baseUrl = sspAddress + HttpContext.Protocol + HttpContext.FhirServerUrl + HttpContext.FhirServerFhirBase;

            }
            else
            {
                baseUrl = sspAddress + HttpContext.Protocol + HttpContext.FhirServerUrl + ":" + HttpContext.FhirServerPort + HttpContext.FhirServerFhirBase;
            }

            // Move the forward slash or the HttpClient will remove everything after the port number
            if (baseUrl[baseUrl.Length-1] != '/')
            {
                baseUrl = baseUrl + "/";
            }
            if (relativeUrl[0] == '/') {
                relativeUrl = relativeUrl.Substring(1);
            }

            // Build The Request
            var httpClient = new HttpClient(handler);
            httpClient.BaseAddress = new Uri(baseUrl);
            
            HttpRequestMessage requestMessage = new HttpRequestMessage(method, relativeUrl);
            if (body != null)
            {
                requestMessage.Content = new StringContent(body, System.Text.Encoding.UTF8, HttpContext.RequestContentType);
            }

            // Add The Headers
            HttpContext.RequestHeaders.AddHeader(HttpConst.Headers.kContentType, HttpContext.RequestContentType);
            foreach (var header in HttpContext.RequestHeaders.GetRequestHeaders())
            {
                try
                {
                    Log.WriteLine("Header - {0} -> {1}", header.Key, header.Value);
                    requestMessage.Headers.Add(header.Key, header.Value);
                }
                catch (Exception e)
                {
                    Log.WriteLine("Could not add header: " + header.Key);
                    Log.WriteLine(e.Message);
                }
            }

            // Start The Performance Timer Running
            timer.Start();

            // Perform The Http Request
            var result = httpClient.SendAsync(requestMessage).ConfigureAwait(false).GetAwaiter().GetResult();
                        
            // Always Stop The Performance Timer Running
            timer.Stop();

            // Save The Time Taken To Perform The Request
            HttpContext.ResponseTimeInMilliseconds = timer.ElapsedMilliseconds;

            // Save The Response Details
            HttpContext.ResponseStatusCode = result.StatusCode;
            HttpContext.ResponseContentType = result.Content.Headers.ContentType.MediaType;
            using (StreamReader reader = new StreamReader(result.Content.ReadAsStreamAsync().Result, Encoding.UTF8))
            {
                HttpContext.ResponseBody = reader.ReadToEnd();
            }

            // Add headers
            foreach (var headerKey in result.Headers)
            {
                foreach (var headerKeyValues in headerKey.Value)
                {
                    HttpContext.ResponseHeaders.Add(headerKey.Key, headerKeyValues);
                    Log.WriteLine("Header - " + headerKey.Key + " : " + headerKeyValues);
                }
            }
            foreach (var header in result.Content.Headers) {
                foreach (var headerValues in header.Value)
                {
                    HttpContext.ResponseHeaders.Add(header.Key, headerValues);
                    Log.WriteLine("Header - " + header.Key + " : " + headerValues);
                }
            }

            if (decompressGzip)
            {
                LogToDisk();
            }
        }

        [When(@"I send a gpc.getcarerecord operation request with invalid resource type payload")]
        public void ISendAGpcGetcarerecordOperationRequestWithInvalidResourceTypePayload()
        {
            var parameterPayload = FhirHelper.ChangeResourceTypeString(FhirSerializer.SerializeToJson(FhirContext.FhirRequestParameters), FhirConst.Resources.kInvalidResourceType);
            RestRequest(Method.POST, "/Patient/$gpc.getcarerecord", parameterPayload);
        }

        [When(@"I send a gpc.getcarerecord operation request WITH payload")]
        public void ISendAGpcGetcarerecordOperationRequestWithPayload()
        {
            HttpRequest(HttpMethod.Post, "/Patient/$gpc.getcarerecord", FhirSerializer.SerializeToJson(FhirContext.FhirRequestParameters), true);
        }

        [When(@"I send a gpc.getcarerecord operation request WITH payload but not decompressed")]
        public void ISendAGpcGetcarerecordOperationRequestWithPayloadButNotDecompressed()
        {
            HttpRequest(HttpMethod.Post, "/Patient/$gpc.getcarerecord", FhirSerializer.SerializeToJson(FhirContext.FhirRequestParameters), false);
        }

        [When(@"I send a metadata request but not decompressed")]
        public void ISendAMetadataRequestButNotDecompressed()
        {
            HttpRequest(HttpMethod.Get, "/metadata", null, false);
        }

        [When(@"I send a metadata request and decompressed")]
        public void ISendAMetadataRequestAndDecompressed()
        {
            HttpRequest(HttpMethod.Get, "/metadata", null, true);
        }

        // Response Validation Steps

        [Then(@"the response status code should indicate success")]
        public void ThenTheResponseStatusCodeShouldIndicateSuccess()
        {
            HttpContext.ResponseStatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Then(@"the response status code should indicate failure")]
        public void ThenTheResponseStatusCodeShouldIndicateFailure()
        {
            HttpContext.ResponseStatusCode.ShouldNotBe(HttpStatusCode.OK);
        }

        //PG - #192 - 2/3/2019 - added for s-pateints returning 404 not found
        [Then(@"the response status code should indicate notfound")]
        public void ThenTheResponseStatusCodeShouldIndicateNofound()
        {
            HttpContext.ResponseStatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Then(@"the response status code should indicate unsupported media type error")]
        public void ThenTheResponseShouldIndicateUnsupportedMediaTypeError()
        {
            HttpContext.ResponseStatusCode.ShouldBe(HttpStatusCode.UnsupportedMediaType);
            Log.WriteLine("Response HttpStatusCode should be {0} but was {1}", HttpStatusCode.UnsupportedMediaType, HttpContext.ResponseStatusCode);
        }

        [Then(@"the response body should be JSON")]
        public void ThenTheResponseBodyShouldBeJSON()
        {
            HttpContext.ResponseContentType.ShouldStartWith(HttpConst.ContentTypes.kJson);
            HttpContext.ResponseJSON = JObject.Parse(HttpContext.ResponseBody);
            FhirJsonParser fhirJsonParser = new FhirJsonParser();
            FhirContext.FhirResponseResource = fhirJsonParser.Parse<Resource>(HttpContext.ResponseBody);
        }

        [Then(@"the response body should be XML")]
        public void ThenTheResponseBodyShouldBeXML()
        {
            HttpContext.ResponseContentType.ShouldStartWith(HttpConst.ContentTypes.kXml);
            HttpContext.ResponseXML = XDocument.Parse(HttpContext.ResponseBody);
            FhirXmlParser fhirXmlParser = new FhirXmlParser();
            FhirContext.FhirResponseResource = fhirXmlParser.Parse<Resource>(HttpContext.ResponseBody);
        }

        [Then(@"the response should be gzip encoded")]
        public void ThenTheResponseShouldBeGZipEncoded()
        {
            bool gZipHeaderFound = false;
            foreach (var header in HttpContext.ResponseHeaders) {
                if (header.Key.Equals(HttpConst.Headers.kContentEncoding, StringComparison.CurrentCultureIgnoreCase) && header.Value.Equals("gzip", StringComparison.CurrentCultureIgnoreCase)) {
                    gZipHeaderFound = true;
                }
            }
            gZipHeaderFound.ShouldBeTrue();
        }

        [Then(@"response should be chunked")]
        public void ThenReesponseShouldBeChunked()
        {
            bool chunkedHeaderFound = false;
            foreach (var header in HttpContext.ResponseHeaders)
            {
                if (header.Key.Equals(HttpConst.Headers.kTransferEncoding, StringComparison.CurrentCultureIgnoreCase) && header.Value.Equals("chunked", StringComparison.CurrentCultureIgnoreCase))
                {
                    chunkedHeaderFound = true;
                }
            }
            chunkedHeaderFound.ShouldBeTrue();
        }

        // Logger

        private void LogToDisk()
        {
            var traceDirectory = GlobalContext.TraceDirectory;
            if (!Directory.Exists(traceDirectory)) return;
            var scenarioDirectory = Path.Combine(traceDirectory, HttpContext.ScenarioContext.ScenarioInfo.Title);
            int fileIndex = 1;
            while (Directory.Exists(scenarioDirectory + "-" + fileIndex)) fileIndex++;
            scenarioDirectory = scenarioDirectory + "-" + fileIndex;
            Directory.CreateDirectory(scenarioDirectory);
            Log.WriteLine(scenarioDirectory);
            HttpContext.SaveToDisk(Path.Combine(scenarioDirectory,"HttpContext.xml"));

            //Output JSON trace
            if (AppSettingsHelper.TraceOutputJSONResponse)
            {
                //OutputJson Response as Pretty Printed Separate File
                try
                {
                    HttpContext.SaveJSONResponseToDisk(Path.Combine(scenarioDirectory, "JSONResponse.txt"));
                }
                catch
                {
                    Log.WriteLine("Exception writing JSONResponse to Output File");
                }

            }

            //Output JWT trace
            if (AppSettingsHelper.TraceOutputJWT)
            {
                //Dump JWT Token to output file
                try
                {
                    HttpContext.SaveJWTToDisk(Path.Combine(scenarioDirectory, "JWTToken.txt"));
                }
                catch (Exception Ex)
                {
                    Log.WriteLine("Exception writing JWT Header to Output File");
                }
            }

            //output Pretty Printed Request body
            if (AppSettingsHelper.TraceOutputJSONRequestBody)
            {
                //Output Json Request Body to a Pretty Printed Separate File
                try
                {
                    HttpContext.SaveJSONRequestBodyToDisk(Path.Combine(scenarioDirectory, "JSONRequestBody.txt"));
                }
                catch
                {
                    Log.WriteLine("Exception writing JSONRequestBody to Output File - Request May Not be JSON");
                }

            }

            //Output Pretty Printed HTML from the Reponse Payload
            if (AppSettingsHelper.TraceOutputHTMLResponse)
            {
                //Output HTML From Reponse
                try
                {
                    HttpContext.SaveHTMLReponseToDisk(Path.Combine(scenarioDirectory, "HTMLResponse.txt"));
                }
                catch (Exception Ex)
                {
                    Log.WriteLine("Exception writing HTMLResponse to Output File - Reponse May not Include HTML. Exception Message : " + Ex.Message);
                }
            }


        }
    }
}
