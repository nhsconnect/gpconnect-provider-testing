using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Extensions;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Logger;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public class PractitionerSteps : TechTalk.SpecFlow.Steps
    {

        private readonly FhirContext FhirContext;
        private JwtHelper jwtHelper;

        // Headers Helper
        public HttpHeaderHelper Headers { get; }

        // JWT Helper
        public JwtHelper Jwt { get; }

        public PractitionerSteps(HttpHeaderHelper headerHelper, FhirContext fhirContext)
        {

            FhirContext = fhirContext;
            // Helpers
            Headers = headerHelper;
            Jwt = jwtHelper;


        }


        [Then(@"the interactionId ""(.*)"" should be valid")]
        public void ValidInteractionId(String interactionId)
        {
            Log.WriteLine("hello");
            Log.WriteLine(interactionId);
            Log.WriteLine("hello");

            var id = SpineConst.InteractionIds.kFhirPractitioner;
            if (interactionId == id)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }



        [Then(@"the interactionId ""(.*)"" should be Invalid")]
        public void InValidInteractionId(String interactionId)
        {
            Log.WriteLine("hello");
            Log.WriteLine(interactionId);
            Log.WriteLine("hello");

            var id = SpineConst.InteractionIds.kFhirPractitioner;
            if (interactionId != id)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }
    }
    
}
