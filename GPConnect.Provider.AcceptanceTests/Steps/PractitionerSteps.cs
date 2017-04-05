﻿using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Data;
using GPConnect.Provider.AcceptanceTests.Extensions;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Model;
using NUnit.Framework;
using System;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using static Hl7.Fhir.Model.Bundle;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public class PractitionerSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;

        // Headers Helper
        public HttpHeaderHelper Headers { get; }

        public PractitionerSteps(HttpHeaderHelper headerHelper, FhirContext fhirContext)
        {
            FhirContext = fhirContext;
            // Helpers
            Headers = headerHelper;
           
        }

        [Given(@"I have the test practitioner codes")]
        public void GivenIHaveTheTestPractitionerCodes()
        {
            FhirContext.FhirPractitioners.Clear();

            foreach (PractitionerCodeMap practitionerCodeMap in GlobalContext.PractitionerMapData)
            {
                Log.WriteLine("Mapped test Practitioner code {0} to {1}", practitionerCodeMap.NativePractitionerCode, practitionerCodeMap.ProviderPractitionerCode);
                FhirContext.FhirPractitioners.Add(practitionerCodeMap.NativePractitionerCode, practitionerCodeMap.ProviderPractitionerCode);
            }
        }

        [Given(@"I add the practitioner identifier parameter with system ""(.*)"" and value ""(.*)""")]
        public void GivenIAddThePractitionerIdentifierParameterWithTheSystemAndValue(string systemParameter, string valueParameter)
        {
            Given($@"I add the parameter ""identifier"" with the value ""{systemParameter + '|' + FhirContext.FhirPractitioners[valueParameter]}""");
        }
        
        [Then(@"the interactionId ""(.*)"" should be valid")]
        public void ValidInteractionId(String interactionId)
        {
            var id = SpineConst.InteractionIds.kFhirPractitioner;
            interactionId.ShouldBeSameAs(id);
        }

        [Then(@"the interactionId ""(.*)"" should be Invalid")]
        public void InValidInteractionId(String interactionId)
        {
            var id = SpineConst.InteractionIds.kFhirPractitioner;
            interactionId.ShouldNotBeSameAs(id);
        }

        [Then(@"there is a communication element")]
        public void ThenPractitionerResourcesShouldContainCommunicationElement()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    Practitioner practitioner = (Practitioner)entry.Resource;
                    practitioner.Communication.ShouldNotBeNull("Communication should not be null");
                }
            }
        }

        [Then(@"the practitioner resource should not contain unwanted fields")]
        public void ThenThePractitionerResourceShouldNotContainFhirFieldsPhotoOrQualification()
        {
            foreach (EntryComponent entry in ((Bundle)FhirContext.FhirResponseResource).Entry)
            {
                if (entry.Resource.ResourceType.Equals(ResourceType.Practitioner))
                {
                    Practitioner practitioner = (Practitioner)entry.Resource;

                    if (null != practitioner.Photo && practitioner.Photo.Count > 0)
                    {
                        Assert.Fail("Practitioner should not contain a Photo");
                    }

                    if (null != practitioner.Qualification && practitioner.Qualification.Count > 0)
                    {
                        Assert.Fail("Practitioner should not contain a Qualification");
                    }

                    if (null != practitioner.BirthDate)
                    {
                        Assert.Fail("Practitioner should not contain a BirthDate");
                    }

                    if (null != practitioner.BirthDateElement)
                    {
                        Assert.Fail("Practitioner should not contain a BirthDateElement");
                    }

                    foreach (Practitioner.PractitionerRoleComponent practitionerRole in practitioner.PractitionerRole)
                    {
                        if (null != practitionerRole.HealthcareService && practitionerRole.HealthcareService.Count > 0)
                        {
                            Assert.Fail("Practitioner role should not contain a HealthcareService");
                        }

                        if (null != practitionerRole.Location && practitionerRole.Location.Count > 0)
                        {
                            Assert.Fail("Practitioner role should not contain a Location");
                        }
                    }
                }
            }
        }
    }
}
