using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Model;
using NUnit.Framework;
using System;
using Shouldly;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using static Hl7.Fhir.Model.Bundle;
using static Hl7.Fhir.Model.Appointment;
using Hl7.Fhir.Serialization;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public class AppointmentReadSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private readonly HttpSteps HttpSteps;
        private readonly HttpContext HttpContext;
      
        // Headers Helper
        public HttpHeaderHelper Headers { get; }

        public AppointmentReadSteps(HttpHeaderHelper headerHelper, FhirContext fhirContext, HttpSteps httpSteps, HttpContext httpContext)
        {
            // Helpers
            FhirContext = fhirContext;
            Headers = headerHelper;
            HttpSteps = httpSteps;
            HttpContext = httpContext;
        }


        [Given(@"I find or create a single appointment for patient ""([^""]*)"" at organization ""([^""]*)"" and save the resource to ""([^""]*)""")]
        public void IFindOrCreateASingleAppointmentForPatientAndSaveTheResourceTo(string patient, string organizaitonName, string patientkey)
        {
            //FhirContext.FhirOrganizations[organizaitonName]
            //FhirContext.FhirPatients[patient]
        }

        [Then(@"the response should be an Appointment resource")]
        public void theResponseShouldBeAnAppointmentResource()
        {
            FhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.Appointment);
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
        }

    }
}



    

