﻿namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using Context;
    using Hl7.Fhir.Model;
    using Shouldly;
    using TechTalk.SpecFlow;
    using Constants;
    using Helpers;
    using Context;
    using Enum;

    [Binding]
    public class HealthcareSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private List<HealthcareService> HealthcareServices => _httpContext.FhirResponse.HealthcareService;

        public HealthcareSteps(HttpContext httpContext, HttpSteps httpSteps)
            : base(httpSteps)
        {
            _httpContext = httpContext;
        }


        [Then(@"the Response Resource should be a Healthcare Service")]
        public void TheResponseResourceShouldBeAHealthcareService()
        {
            _httpContext.FhirResponse.Resource.ResourceType.ShouldBe(ResourceType.HealthcareService);

        }

        [Then(@"the Healthcare Id should match the GET request Id")]
        public void TheHealthcarenIdShouldMarchTheGetRequestId()
        {
            var healthcare = HealthcareServices.FirstOrDefault();
            healthcare.ShouldNotBeNull();
            healthcare.Id.ShouldBe(_httpContext.HttpRequestConfiguration.GetRequestId, "FAIL : Logical ID of the returned healthcareservice doesnt match the requested logical ID");
            Logger.Log.WriteLine("INFO : Validated Healthcare service ID matches request ID : " + healthcare.Id);
        }

        [Then(@"the Healthcare service should be valid")]
        public void TheHealthcareserviceshouldbevalid()
        {
            var healthcare = HealthcareServices.FirstOrDefault();
            CheckHealthcareServiceIsValid(healthcare);
        }

        [Then(@"the response searchset contains atleast one HealthService")]
        public void TheresponsecontainsatleastoneHealthService()
        {
            _httpContext.FhirResponse.Entries.Count.ShouldBeGreaterThanOrEqualTo(1, "Fail : The response bundle does not contain  atleast one healthcare service");

            _httpContext.FhirResponse.Entries.ForEach(entry =>
            {
                entry.Resource.ResourceType.ShouldBe(ResourceType.HealthcareService);
            });


        }

        [Then(@"the response searchset contains valid Healthcare Service resources")]
        public void TheresponsesearchsetcontainsvalidHealthServiceresources()
        {
            _httpContext.FhirResponse.Entries.ForEach(entry =>
            {
                CheckHealthcareServiceIsValid(entry.Resource);
            });
        }


        [Then(@"I Store the DOS id from the first Healthcare service returned")]
        public void IStoretheDOSidfromthefirsthealthcareservicereturned()
        {
            HealthcareService healthcare = (HealthcareService) _httpContext.FhirResponse.Entries.FirstOrDefault().Resource;

           var healthcareServiceIdentifiers = healthcare.Identifier
                   .Where(identifier => identifier.System.Equals(FhirConst.IdentifierSystems.kDosServiceID))
                   .ToList();

            healthcareServiceIdentifiers.Count.ShouldBeGreaterThanOrEqualTo(1, "Fail : There should be atleast One DOS service ID associated with a healthcare service");
            GlobalContext.HealthcareServiceDosID = healthcareServiceIdentifiers.FirstOrDefault().Value;
            Logger.Log.WriteLine("Info : Found Healthcare Service With DOS ID : " + GlobalContext.HealthcareServiceDosID);
        }

        [Then(@"I Store the DOS id from the Healthcare service returned")]
        public void IStoretheDOSidfromthehealthcareservicereturned()
        {
            HealthcareService healthcare = (HealthcareService)_httpContext.FhirResponse.Resource;

            var healthcareServiceIdentifiers = healthcare.Identifier
                    .Where(identifier => identifier.System.Equals(FhirConst.IdentifierSystems.kDosServiceID))
                    .ToList();

            healthcareServiceIdentifiers.Count.ShouldBeGreaterThanOrEqualTo(1, "Fail : There should be atleast One DOS service ID associated with a healthcare service");
            GlobalContext.HealthcareServiceDosID = healthcareServiceIdentifiers.FirstOrDefault().Value;
            Logger.Log.WriteLine("Info : Found Healthcare Service With DOS ID : " + GlobalContext.HealthcareServiceDosID);
        }

        [Given(@"I set the request DOS service ID to the following ""([^""]*)""")]
        public void SetTheGetRequestIdToTheLogicalIdentiferForHealthcareService(string DOSID)
        {
            GlobalContext.HealthcareServiceDosID = DOSID;
        }

        [Then(@"the response searchset has NO Healthcare Service resources")]
        public void theresponsesearchsethasNOHealthcareServiceresources()
        {
            _httpContext.FhirResponse.Entries.Count().ShouldBe(0, " Fail : Should be no healthcare services resouces returned");
        }

        [Then(@"the response searchset has only One Healthcare Service resource")]
        public void theresponsesearchsethasOneHealthcareServiceresource()
        {
            _httpContext.FhirResponse.Entries.Count().ShouldBe(1, " Fail : Should be one healthcare services resouce returned");
        }

        [Then(@"the returned Healthcareservice has the requested DOS ID")]
        public void theretunredHealthcareservicehastherequestedDOSID()
        {
            var found = false;
            HealthcareService healthcare = (HealthcareService)_httpContext.FhirResponse.Entries.FirstOrDefault().Resource;

            var healthcareServiceIdentifiers = healthcare.Identifier
                    .Where(identifier => identifier.System.Equals(FhirConst.IdentifierSystems.kDosServiceID))
                    .ToList();

            healthcareServiceIdentifiers.ForEach(ident => {
                if (ident.Value == GlobalContext.HealthcareServiceDosID)
                {
                    found = true;
                }

            });
            found.ShouldBeTrue("Fail : Requested DOS ID : " + GlobalContext.HealthcareServiceDosID);
            Logger.Log.WriteLine("INFO : Returned heathcareservice has the requested DOS id : " + GlobalContext.HealthcareServiceDosID);
            
        }
        

    }
}