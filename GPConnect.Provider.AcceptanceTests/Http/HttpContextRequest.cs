﻿namespace GPConnect.Provider.AcceptanceTests.Http
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using Context;

    public class HttpContextRequest : HttpRequestBase
    {
        private readonly HttpContext _httpContext;

        public HttpContextRequest(HttpContext httpContext, SecurityContext securityContext)
            : base(httpContext.HttpRequestConfiguration, securityContext)
        {
            _httpContext = httpContext;
        }

        public void MakeRequest()
        {
            var httpClient = GetHttpClient();
  
            var requestMessage = GetHttpRequestMessage();

            var timer = new Stopwatch();

            timer.Start();

            try
            {
                var result = httpClient.SendAsync(requestMessage).Result;

                timer.Stop();

                _httpContext.HttpResponse = GetHttpResponse(result);

                _httpContext.HttpResponse.ResponseTimeInMilliseconds = timer.ElapsedMilliseconds;

                _httpContext.FhirResponse.Resource = _httpContext.HttpResponse.ParseFhirResource().Resource;
            }
            catch (Exception exception)
            {
                _httpContext.HttpResponse = new HttpResponse
                {
                    ConnectionClosed = true
                };
            }
        }
    }
}
