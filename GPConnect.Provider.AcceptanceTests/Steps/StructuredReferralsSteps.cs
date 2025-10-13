namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using Constants;
    using Context;
    using TechTalk.SpecFlow;
    using Shouldly;
    using Hl7.Fhir.Model;
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using GPConnect.Provider.AcceptanceTests.Enum;
    using static Hl7.Fhir.Model.Parameters;
    using GPConnect.Provider.AcceptanceTests.Helpers;
    using System.Text.RegularExpressions;

    [Binding]
    public sealed class StructuredReferralsSteps : BaseSteps
    {
        private readonly HttpContext _httpContext;
        private List<ReferralRequest> ReferralRequests => _httpContext.FhirResponse.ReferralRequests;
        private List<List> Lists => _httpContext.FhirResponse.Lists;
        private List<Patient> Patients => _httpContext.FhirResponse.Patients;
        private Bundle Bundle => _httpContext.FhirResponse.Bundle;
        private List<Condition> Problems => _httpContext.FhirResponse.Conditions;

        public StructuredReferralsSteps(HttpSteps httpSteps, HttpContext httpContext)
            : base(httpSteps)
        {
            _httpContext = httpContext;
        }


        [Given(@"I add the Referrals parameter")]
        public void GivenIAddTheReferralsParameter()
        {
            ParameterComponent param = new ParameterComponent();
            param.Name = FhirConst.GetStructuredRecordParams.kReferrals;
            _httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(param);
        }


        [Then(@"I Check the Referrals List is Valid")]
        public void ThenIChecktheReferralsListsareValid()
        {
            //Check atleast one Referrals List exists using Snomed Code
            Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kOutboundReferrals).ToList().Count().ShouldBe(1, "Failed to Find ONE Referrals list using Snomed Code.");

            //Get  Investigations list
            var referralsList = Lists.Where(l => l.Code.Coding.First().Code == FhirConst.GetSnoMedParams.kOutboundReferrals).ToList();

            referralsList.ForEach(referList =>
            {
                //Check Meta.profile
                CheckForValidMetaDataInResource(referList, FhirConst.StructureDefinitionSystems.kList);

                //Check Status
                referList.Status.ToString().ToLower().ShouldBe("current", "List Status is NOT set to current");

                //Check Mode
                referList.Mode.ShouldBeOfType<ListMode>("Mode List is of wrong type.");
                referList.Mode.ToString().ToLower().ShouldBe("snapshot", "List Status is NOT set to snapshot");

                //Check title
                referList.Title.ShouldBe(FhirConst.ListTitles.kOutboundReferrals, "Referrals List Title is Incorrect");

                //Check code
                referList.Code.Coding.ForEach(coding =>
                {
                    coding.System.ShouldBeOneOf("http://snomed.info/sct", "http://read.info/readv2", "http://read.info/ctv3", "https://fhir.hl7.org.uk/Id/emis-drug-codes", "https://fhir.hl7.org.uk/Id/egton-codes", "https://fhir.hl7.org.uk/Id/multilex-drug-codes", "https://fhir.hl7.org.uk/Id/resipuk-gemscript-drug-codes");
                    coding.Display.ShouldNotBeNullOrEmpty("Display Should not be Null or Empty");
                });

                //Check Patient
                Patients.Where(p => p.Id == (referList.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");

                //check number of DiagnosticReports matches number in list
                ReferralRequests.Count().ShouldBe(referList.Entry.Count(), "Number of ReferralRequests does not match the number in the List");

                //Check each Entry/Item/Reference points to a resource that exists
                bool found = false;
                string pattern = @"(.*/)(.*)";
                referList.Entry.ForEach(entry =>
                {
                    string refToFind = Regex.Replace(entry.Item.Reference, pattern, "$2");
                    Bundle.GetResources()
                                .Where(resource => resource.ResourceType.Equals(ResourceType.ReferralRequest))
                                .Where(resource => resource.Id == refToFind)
                                .ToList().Count().ShouldBe(1, "ReferralRequest resource Not Found with id : " + refToFind);
                    Logger.Log.WriteLine("Referrals List - Verified the Linked ReferralRequest has been included In the Bundle: " + refToFind);
                    found = true;
                });

                //Check we found atleast One DiagnosticReport Linked in list
                found.ShouldBeTrue("Fail : Referrals List should be linked to atleast one ReferralRequest report");

                Logger.Log.WriteLine("Completed Mandatory checks on Referrals List");
            });


        }

        [Then(@"I Check the ReferralRequests are Valid")]
        public void ThenIChecktheReferralRequestsareValid()
        {
            //check atleast one
            ReferralRequests.ToList().Count().ShouldBeGreaterThan(0, "Error Should be Atleast One ReferralRequest in response as per Data requirements");

            ReferralRequests.ForEach(referalRequest =>
            {
                //Check Id
                referalRequest.Id.ShouldNotBeNullOrEmpty();

                //Check Meta.profile
                CheckForValidMetaDataInResource(referalRequest, FhirConst.StructureDefinitionSystems.kReferrals);

                //Check Identfier
                referalRequest.Identifier.Count.ShouldBeGreaterThan(0, "There should be at least 1 Identifier system/value pair");
                referalRequest.Identifier.ForEach(identifier =>
                {
                    identifier.System.ShouldNotBeNullOrEmpty("Identifier System Is Null or Empty - Should be populated");
                    identifier.Value.ShouldNotBeNullOrEmpty("Fail : No Identifier found when resource should have a unique Identifier");
                });

                //Check Status
                referalRequest.Status.ToString().ToLower().ShouldBe("unknown", "ReferalRequest Status should be set to unknown");

                //Check intent
                referalRequest.Intent.ToString().ToLower().ShouldBe("order", "ReferalRequest Intent should be set to order");

                //Check Subject/patient
                Patients.Where(p => p.Id == (referalRequest.Subject.Reference.Replace("Patient/", ""))).Count().ShouldBe(1, "Patient Not Found in Bundle");

            });

        }

        [Then(@"I Check the ReferralRequests Do Not Include Not in Use Fields")]
        public void ThenIChecktheReferralRequestsDoNotIncludeNotinUseFields()
        {
            ReferralRequests.ForEach(referalRequest =>
            {
                referalRequest.Definition.Count().ShouldBe(0, "Fail :  ReferralRequest - Definition element Should not be used - Not In Use Field");
                referalRequest.Replaces.Count().ShouldBe(0, "Fail :  ReferralRequest - Replaces element Should not be used - Not In Use Field");
                referalRequest.GroupIdentifier.ShouldBeNull("Fail :  ReferralRequest - GroupIdentifier element Should not be used - Not In Use Field");
                referalRequest.Type.ShouldBeNull("Fail :  ReferralRequest - Type element Should not be used - Not In Use Field");
                referalRequest.Occurrence.ShouldBeNull("Fail :  ReferralRequest - Occurrence element Should not be used - Not In Use Field");
                referalRequest.ReasonReference.Count().ShouldBe(0, "Fail :  ReferralRequest - ReasonReference element Should not be used - Not In Use Field");
                referalRequest.RelevantHistory.Count().ShouldBe(0, "Fail :  ReferralRequest - RelevantHistory element Should not be used - Not In Use Field");
            });
        }

        [Given(@"I add the Referrals data parameter with current date")]
        public void GivenIAddTheReferralsParameterWithCurrentDate()
        {
            var backDate = DateTime.UtcNow;
            var futureDate = DateTime.UtcNow;
            var startDate = backDate.ToString("yyyy-MM-dd");
            var endDate = futureDate.ToString("yyyy-MM-dd");

            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kReferralSearch, (Base)FhirHelper.GetTimePeriod(startDate, endDate)),
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kReferrals, tuples);
        }

        [Given(@"I add the Referrals data parameter with valid start date and no end date")]
        public void GivenIAddTheReferralsParameterWithValidStartDateAndNoEndDate()
        {
            var backDate = DateTime.UtcNow.AddYears(-3);
            var startDate = backDate.ToString("yyyy-MM-dd");

            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kReferralSearch, (Base)FhirHelper.SafeGetTimePeriod(startDate, null)),
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kReferrals, tuples);
        }

        [Given(@"I add the Referrals data parameter with no start date and no end date")]
        public void GivenIAddTheReferralsParameterWithNoStartDateAndNoEndDate()
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kReferralSearch, (Base)FhirHelper.SafeGetTimePeriod(null, null)),
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kReferrals, tuples);
        }

        [Given(@"I add the Referrals data parameter with date permutations ""([^""]*)"" and ""([^""]*)""")]
        public void GivenIAddTheReferralsParameterWithDatePermutations(string start, string end)
        {
            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kReferralSearch, (Base)FhirHelper.GetTimePeriod(start, end)),

            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kReferrals, tuples);
        }

        [Given(@"I add the Referrals data parameter with future start date")]
        public void GivenIAddTheReferralsParameterWithFutureStartDate()
        {
            var futureStartDate = DateTime.UtcNow.AddDays(+10);
            var futureEndDate = DateTime.UtcNow.AddDays(+15);
            var startDate = futureStartDate.ToString("yyyy-MM-dd");
            var endDate = futureEndDate.ToString("yyyy-MM-dd");

            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kReferralSearch, (Base)FhirHelper.GetTimePeriod(startDate, endDate)),
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kReferrals, tuples);
        }

        [Given(@"I add the Referrals data parameter start date after endDate")]
        public void GivenIAddTheReferralsParameterStartDateAfterEndDate()
        {
            var backDate = DateTime.UtcNow.AddDays(-10);
            var futureDate = DateTime.UtcNow.AddDays(-15);
            var startDate = backDate.ToString("yyyy-MM-dd");
            var endDate = futureDate.ToString("yyyy-MM-dd");

            IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
                Tuple.Create(FhirConst.GetStructuredRecordParams.kReferralSearch, (Base)FhirHelper.GetTimePeriod(startDate, endDate)),
            };
            _httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kReferrals, tuples);
        }


        [Then(@"Check a Problem is linked to ReferralRequest and that it is also included")]
        public void ThenCheckaProblemislinkedtoReferralRequestandthatitisalsoincluded()
        {
            var found = false;
            string refToFind = "";

            foreach (var p in Problems)
            {
                Condition problem = (Condition)p;
                List<Extension> problemRelatedContentExtensions = p.Extension.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kExtProblemRelatedContent)).ToList();

                foreach (var rcc in problemRelatedContentExtensions)
                {
                    ResourceReference rr = (ResourceReference)rcc.Value;
                    if (rr.Reference != null)
                    {
                        if (rr.Reference.StartsWith("ReferralRequest/"))
                        {
                            string pattern = @"(.*)(/)(.*)";
                            refToFind = Regex.Replace(rr.Reference, pattern, "$3");
                            found = true;
                            Logger.Log.WriteLine("Info : Problem - Found Linked to a ReferralRequest - with ID : " + refToFind);
                            break;
                        }
                    }
                }
                if (found)
                    break;
            }
            ;

            found.ShouldBeTrue("Fail : No Problems found to be linked to a  ReferralRequest");

            //check that Linked Clinical resource has been included in response.
            VerifyResourceReferenceExists("ReferralRequest", refToFind);

        }

        public void VerifyResourceReferenceExists(string refTypeToFind, string refToFind)
        {
            //Switch on Clinical Item type
            switch (refTypeToFind)
            {
                case "ReferralRequest":
                    CheckResourceExists(ResourceType.ReferralRequest, refToFind);
                    break;

                //unknown type ignore - could be not supported message
                default:
                    Logger.Log.WriteLine("Ignored, Entry/Item/Reference for : " + refTypeToFind);
                    break;
            }
        }

        public void CheckResourceExists<T>(T resourceType, string resourceID)
        {
            Bundle.GetResources()
                           .Where(resource => resource.ResourceType.Equals(resourceType))
                           .Where(resource => resource.Id == resourceID)
                           .ToList().Count().ShouldBe(1, "Fail : Linked Resource Not Contained in Response - type : " + resourceType + " - ID : " + resourceID);

            Logger.Log.WriteLine("Found Linked resource : " + resourceID + " Of Type : " + resourceType);

        }



    }
}
