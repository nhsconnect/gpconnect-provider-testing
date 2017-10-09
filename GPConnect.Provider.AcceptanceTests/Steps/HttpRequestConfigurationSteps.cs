using System.Linq;
using GPConnect.Provider.AcceptanceTests.Helpers;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System;
    using System.Net;
    using System.Net.Http;
    using Constants;
    using Context;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    public class HttpRequestConfigurationSteps : Steps
    {
        private readonly HttpContext _httpContext;

        public HttpRequestConfigurationSteps(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        [Given(@"I am not using the SSP")]
        public void GivenIAmNotUsingTheSsp()
        {
            _httpContext.HttpRequestConfiguration.UseSpineProxy = false;
        }

        [Given(@"I am using the SSP")]
        public void IAmUsingTheSsp()
        {
            _httpContext.HttpRequestConfiguration.UseSpineProxy = true;
        }

        // Http Header Configuration Steps

        [Given(@"I set ""(.*)"" request header to ""(.*)""")]
        public void GivenISetRequestHeaderTo(string headerName, string headerValue)
        {
            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(headerName, headerValue);
        }

        [Given(@"I am connecting to accredited system ""(.*)""")]
        public void GivenIConnectingToAccreditedSystem(string toASID)
        {
            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspTo, toASID);
        }

        [Given(@"I am generating a random message trace identifier")]
        public void GivenIAmGeneratingARandomMessageTraceIdentifier()
        {
            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspTraceId, Guid.NewGuid().ToString(""));
        }

        [Given(@"I do not send header ""(.*)""")]
        public void GivenIDoNotSendHeader(string headerKey)
        {
            _httpContext.HttpRequestConfiguration.RequestHeaders.RemoveHeader(headerKey);
        }

        // Http Request Steps
        [Given(@"I set the Accept-Encoding header to gzip")]
        public void SetTheAcceptEncodingHeaderToGzip()
        {
            _httpContext.HttpRequestConfiguration.RequestHeaders.AddHeader(HttpConst.Headers.kAcceptEncoding, "gzip");
        }

        [Given(@"I set the request content type to ""(.*)""")]
        public void GivenISetTheRequestTypeTo(string requestContentType)
        {
            _httpContext.HttpRequestConfiguration.RequestContentType = requestContentType;
        }

        [Given(@"I set the Accept header to ""(.*)""")]
        public void GivenISetTheAcceptHeaderTo(string acceptContentType)
        {
            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kAccept, acceptContentType);
        }

        [Given(@"I set the Prefer header to ""(.*)""")]
        public void GivenISetThePreferHeaderTo(string preferHeaderContent)
        {
            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kPrefer, preferHeaderContent);
        }

        [Given(@"I set the If-None-Match header to ""(.*)""")]
        public void GivenISetTheIfNoneMatchheaderHeaderTo(string ifNoneMatchHeaderContent)
        {
            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kIfNoneMatch, ifNoneMatchHeaderContent);
        }

        [Given(@"I set the If-Match header to ""([^""]*)""")]
        public void SetTheIfMatchHeaderTo(string value)
        {
            if (!value.StartsWith("W/"))
            {
                value = "W/\"" + value + "\"";
            }

            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kIfMatch, value);
        }

        [Given(@"I add the time period parameters for ""(.*)"" days starting today using the start format ""(.*)"" and the end format ""(.*)""")]
        public void GivenIAddTheTimePeriodParametersforDaysStartingTodayWithStartEndFormats(int days, string startFormat, string endFormat)
        {
           var val = TimePeriodHelper.GetTimePeriodStartDateFormatEndDateFormat(startFormat, endFormat, days);
     

            Given($"I add the parameter \"start\" with the value \"{FhirConst.Prefixs.kGreaterThanOrEqualTo}{val.Start}\"");
            Given($"I add the parameter \"end\" with the value \"{FhirConst.Prefixs.kLessThanOrEqualTo}{val.End}\"");
        }

        [Given(@"I add the time period parameters for ""(.*)"" days starting today using the start date prefix ""(.*)"" and the end date prefix ""(.*)""")]
        public void GivenIAddTheTimePeriodParametersforDaysStartingTodayWithStartEndPrefix(int days, string startDatePrefix, string endDatePrefix)
        {
            var val = TimePeriodHelper.GetTimePeriodStartDateTodayEndDateDays(days);

            Given($"I add the parameter \"start\" with the value \"{startDatePrefix}{val.Start}\"");
            Given($"I add the parameter \"end\" with the value \"{endDatePrefix}{val.End}\"");
        }

        [Given(@"I add the time period parameters for ""(.*)"" days starting today")]
        public void GivenIAddTheTimePeriodParametersforDaysStartingToday(int days)
        {
            var val = TimePeriodHelper.GetTimePeriodStartDateTodayEndDateDays(days);

            Given($"I add the parameter \"start\" with the value \"{val.Start}\"");
            Given($"I add the parameter \"end\" with the value \"{val.End}\"");
        }

        [Given(@"I add the time period parameter that is ""(.*)"" days in the future")]
        public void GivenIAddTheTimePeriodParameterThatIsDaysInTheFuture(int days)
        {
            var dateStart = DateTime.UtcNow.AddDays(days);
            var dateEnd = dateStart.AddDays(1);
            var val = TimePeriodHelper.GetTimePeriod(dateStart.ToString("yyyy-MM-dd"), dateEnd.ToString("yyyy-MM-dd"));

            Given($"I add the parameter \"start\" with the value \"{FhirConst.Prefixs.kGreaterThanOrEqualTo}{val.Start}\"");
            Given($"I add the parameter \"end\" with the value \"{FhirConst.Prefixs.kLessThanOrEqualTo}{val.End}\"");
        }

        [Given(@"I add the parameter ""(.*)"" with the value ""(.*)""")]
        public void GivenIAddTheParameterWithTheValue(string parameterName, string parameterValue)
        {
            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter(parameterName, parameterValue);
        }

        [Given(@"I remove the parameters ""(.*)""")]
        public void GivenIRemoveTheParameters(string parameterCsv)
        {
            var keys = parameterCsv.Split(',').ToList();
            keys.ForEach(k => _httpContext.HttpRequestConfiguration.RequestParameters.RemoveParameter(k));
        }

        [Given(@"I update the parameter ""(.*)"" with value ""(.*)""")]
        public void GivenIUpdateTheParameters(string key, string value)
        {
             _httpContext.HttpRequestConfiguration.RequestParameters.UpdatetParameter(key, value);
        }

        [Given(@"I add the parameter ""(.*)"" with the value or sitecode ""(.*)""")]
        public void GivenIAddTheParameterWithTheSiteCode(string parameterName, string parameterValue)
        {
            if (parameterValue.Contains(FhirConst.IdentifierSystems.kOdsSiteCode))
            {
                var siteCode = parameterValue.Substring(parameterValue.LastIndexOf('|') + 1);
                string mappedSiteValue = GlobalContext.OdsCodeMap[siteCode];
                _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter(parameterName, string.Format("{0}|{1}", FhirConst.IdentifierSystems.kOdsSiteCode, mappedSiteValue));
                return;
            }

            _httpContext.HttpRequestConfiguration.RequestParameters.AddParameter(parameterName, parameterValue);
        }


        [Given(@"I set the GET request Id to ""([^""]*)""")]
        public void SetTheGetRequestIdTo(string id)
        {
            _httpContext.HttpRequestConfiguration.RequestUrl = "/fhir/Patient/" + id;
        }

        [Given(@"I set the GET request Version Id to ""([^""]*)""")]
        public void SetTheGetRequestVersionIdTo(string versionId)
        {
            _httpContext.HttpRequestConfiguration.GetRequestVersionId = versionId;
        }

        [Given(@"I set the request Http Method to ""([^""]*)""")]
        public void SetTheRequestHttpMethodTo(string method)
        {
            _httpContext.HttpRequestConfiguration.HttpMethod = new HttpMethod(method);
        }

        [Given(@"I set the request URL to ""([^""]*)""")]
        public void SetTheRequestUrlTo(string url)
        {
            _httpContext.HttpRequestConfiguration.RequestUrl = url;
        }

        [Given(@"I set the Interaction Id header to ""([^""]*)""")]
        public void SetTheInteractionIdHeaderTo(string interactionId)
        {
            _httpContext.HttpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, interactionId);
        }

        [Given(@"I set the Read Operation logical identifier used in the request to ""([^""]*)""")]
        public void SetTheReadOperationLogicalIdentifierUsedInTheRequestTo(string logicalId)
        {
            _httpContext.HttpRequestConfiguration.GetRequestId = logicalId;

            var lastIndex = _httpContext.HttpRequestConfiguration.RequestUrl.LastIndexOf('/');

            if (_httpContext.HttpRequestConfiguration.RequestUrl.Contains("$"))
            {
                var action = _httpContext.HttpRequestConfiguration.RequestUrl.Substring(lastIndex);

                var firstIndex = _httpContext.HttpRequestConfiguration.RequestUrl.IndexOf('/');
                var url = _httpContext.HttpRequestConfiguration.RequestUrl.Substring(0, firstIndex + 1);

                _httpContext.HttpRequestConfiguration.RequestUrl = url + _httpContext.HttpRequestConfiguration.GetRequestId + action;
            }
            else
            {
                _httpContext.HttpRequestConfiguration.RequestUrl = _httpContext.HttpRequestConfiguration.RequestUrl.Substring(0, lastIndex + 1) + _httpContext.HttpRequestConfiguration.GetRequestId;
            }
        }

        [Given(@"I set the Read Operation relative path to ""([^""]*)"" and append the resource logical identifier")]
        public void SetTheReadOperationRelativePathToAndAppendTheResourceLogicalIdentifier(string relativePath)
        {
            _httpContext.HttpRequestConfiguration.RequestUrl = relativePath + "/" + _httpContext.HttpRequestConfiguration.GetRequestId;
        }

        [Given(@"I set the Get Request Id to the Logical Identifer for Location ""([^""]*)""")]
        public void SetTheGetRequestIdToTheLogicalIdentiferForLocation(string location)
        {
            string logicalId;

            GlobalContext.LocationLogicalIdentifierMap.TryGetValue(location, out logicalId).ShouldBe(true, $"There is no record in the map for {location}.");

            logicalId.ShouldNotBeNullOrEmpty($"The LogicalId in the map for {location} should not be null or empty but was {logicalId}.");

            _httpContext.HttpRequestConfiguration.GetRequestId = logicalId;
        }
    }
}
