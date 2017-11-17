using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BoDi;
using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Importers;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Specification.Source;
using NUnit.Framework;
using TechTalk.SpecFlow;
// ReSharper disable UnusedMember.Global

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using Hl7.Fhir.Model;
    using Hl7.Fhir.Serialization;
    using Repository;
    using Shouldly;
    using Steps = TechTalk.SpecFlow.Steps;

    [Binding]
    public class GenericSteps : Steps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly HttpContext _httpContext;

        public GenericSteps(IObjectContainer objectContainer, HttpContext httpContext)
        {
            Log.WriteLine("GenericSteps() Constructor");
            _objectContainer = objectContainer;
            _httpContext = httpContext;
        }

        [BeforeTestRun(Order = 1)]
        public static void CreateTraceFolder()
        {
            if (!Directory.Exists(AppSettingsHelper.TraceBaseDirectory)) return;
            var folderName = DateTime.Now.ToString("s").Replace(":", String.Empty);
            var traceDirectory = Path.Combine(AppSettingsHelper.TraceBaseDirectory, folderName);
            Log.WriteLine("Create Trace Directory = '{0}'", traceDirectory);
            Directory.CreateDirectory(traceDirectory);
            // Save The Newly Created Trace Directory To The Global Context
            GlobalContext.TraceDirectory = traceDirectory;
        }

        [BeforeTestRun(Order = 1)]
        public static void LoadODSData()
        {
            if (!Directory.Exists(AppSettingsHelper.DataDirectory))
            {
                Assert.Fail("Data Directory Not Found.");
            }

            var odsCSV = Path.Combine(AppSettingsHelper.DataDirectory, @"ODSCodeMap.csv");
            Log.WriteLine("ODS CSV = '{0}'", odsCSV);
            GlobalContext.OdsCodeMap = ODSCodeMapImporter.LoadCsv(odsCSV);
        }

        [BeforeTestRun(Order = 1)]
        public static void LoadLogicalLocationIdentifierMap()
        {
            if (!Directory.Exists(AppSettingsHelper.DataDirectory))
            {
                Assert.Fail("Data Directory Not Found.");
            }

            var csv = Path.Combine(AppSettingsHelper.DataDirectory, @"LocationLogicalIdentifierMap.csv");

            Log.WriteLine("LocationLogicalIdentifierMap= '{0}'", csv);

            GlobalContext.LocationLogicalIdentifierMap = LocationLogicalIdentifierImporter.LoadCsv(csv);
        }

        [BeforeTestRun(Order = 1)]
        public static void LoadNHSNoMapData()
        {
            if (!Directory.Exists(AppSettingsHelper.DataDirectory))
            {
                Assert.Fail("Data Directory Not Found.");
            }

            var nhsNoMapCSV = Path.Combine(AppSettingsHelper.DataDirectory, @"NHSNoMap.csv");
            Log.WriteLine("NHSNoMap CSV = '{0}'", nhsNoMapCSV);
            GlobalContext.PatientNhsNumberMap = NHSNoMapImporter.LoadCsv(nhsNoMapCSV);
        }

        [BeforeTestRun(Order = 1)]
        public static void LoadRegisterPatientData()
        {
            if (!Directory.Exists(AppSettingsHelper.DataDirectory))
            {
                Assert.Fail("Data Directory Not Found.");
            }

            var registerPatientsCSV = Path.Combine(AppSettingsHelper.DataDirectory, @"RegisterPatients.csv");
            Log.WriteLine("RegisterPatients CSV = '{0}'", registerPatientsCSV);
            GlobalContext.RegisterPatients = RegisterPatientsImporter.LoadCsv(registerPatientsCSV);
        }

        [BeforeTestRun(Order = 1)]
        public static void LoadPractitionerCodeMapData()
        {
            if (!Directory.Exists(AppSettingsHelper.DataDirectory))
            {
                Assert.Fail("Data Directory Not Found.");
            }

            var practitionerCodeMapCSV = Path.Combine(AppSettingsHelper.DataDirectory, @"PractitionerCodeMap.csv");
            Log.WriteLine("practitionerCodeMap CSV = '{0}'", practitionerCodeMapCSV);
            GlobalContext.PractionerCodeMap = PractitionerCodeMapImporter.LoadCsv(practitionerCodeMapCSV);
        }

        private static MultiResolver _resolver => GetResolver();

        private static MultiResolver GetResolver()
        {
            var resolvers = new List<IResourceResolver>
            {
                GetWebResolver()
                //Add other resolvers here
            };

            return new MultiResolver(resolvers);
        }

        private static WebResolver GetWebResolver()
        {
            return new WebResolver(GetFhirClientFactory());
        }

        [BeforeTestRun(Order = 2)]
        public static void LoadValuesets()
        {
            var vs = LoadValueSet(FhirConst.ValueSetSystems.kContactEntityType);

            //vs.Compose.Include.ForEach(inc =>
            //{
            //    var codeSystem = _resolver.FindCodeSystem("http://hl7.org/fhir/contactentity-type");

            //    //var codes = codeSystem.Concept.Select(x => x.CodeElement).ToList();

            //    //codes.AddRange(inc.Concept.Select(x => x.CodeElement).ToList());

            //    //var a = codes;
            //});

            var res = new ResourceIdentity("http://hl7.org/fhir/contactentity-type");
            var cl = new FhirClient("http://hl7.org/fhir/");
            var b = cl.Read<Resource>(res);

            GlobalContext.FhirGenderValueSet = LoadValueSet(FhirConst.ValueSetSystems.kAdministrativeGender);
            GlobalContext.FhirMaritalStatusValueSet = LoadValueSet(FhirConst.ValueSetSystems.kMaritalStatus);
            GlobalContext.FhirRelationshipValueSet = LoadValueSet(FhirConst.ValueSetSystems.kRelationshipStatus);
            GlobalContext.FhirHumanLanguageValueSet = LoadValueSet(FhirConst.ValueSetSystems.kLanguage);
        }

        private static ValueSet LoadValueSet(string url)
        {
            var valueSet = _resolver.FindValueSet(url);

            valueSet.ShouldNotBeNull($"There was no ValueSet found at {url}.");

            Log.WriteLine($"{valueSet.Compose.Include.Sum(x => x.Concept.Count)} {valueSet.Name} Codes Loaded");

            return valueSet;
        }

        [BeforeScenario(Order = 0)]
        public void InitializeContainer()
        {
            Log.WriteLine("InitializeContainer For Dependency Injection");
            _objectContainer.RegisterTypeAs<SecurityContext, ISecurityContext>();
            _objectContainer.RegisterTypeAs<HttpContext, IHttpContext>();
            _objectContainer.RegisterTypeAs<FhirResourceRepository, IFhirResourceRepository>();
            //_objectContainer.Resolve<HttpHeaderHelper>();
            // HACK To Be Able To See What We've Loaded In The BeforeTestRun Phase
            Log.WriteLine("{0} Organisations Loaded From ODS CSV File.", GlobalContext.OdsCodeMap.Count);
            Log.WriteLine("{0} Genders Loaded From FHIR ValueSet File.", GlobalContext.FhirGenderValueSet.Compose.Include.Sum(x => x.Concept.Count));
            Log.WriteLine("{0} MaritalStatus Loaded From FHIR ValueSet File.", GlobalContext.FhirMaritalStatusValueSet.Compose.Include.Sum(x => x.Concept.Count));
            Log.WriteLine("{0} Relationship Loaded From FHIR ValueSet File.", GlobalContext.FhirRelationshipValueSet.Compose.Include.Sum(x => x.Concept.Count));
            Log.WriteLine("{0} HumanLanguage Loaded From FHIR ValueSet File.", GlobalContext.FhirHumanLanguageValueSet.Compose.Include.Sum(x => x.Concept.Count));
        }

        [BeforeScenario(Order = 1)]
        public void OutputScenarioDetails()
        {
            Log.WriteLine("Feature: " + FeatureContext.Current.FeatureInfo.Title);
            Log.WriteLine(FeatureContext.Current.FeatureInfo.Description);
            Log.WriteLine("");
            Log.WriteLine("Scenario: " + ScenarioContext.Current.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void SaveLogOfOutput()
        {
            var traceDirectory = GlobalContext.TraceDirectory;
            if (!Directory.Exists(traceDirectory)) return;
            var scenarioDirectory = Path.Combine(traceDirectory, ScenarioContext.Current.ScenarioInfo.Title);
            int fileIndex = 1;
            while (Directory.Exists(scenarioDirectory + "-" + fileIndex)) fileIndex++;
            scenarioDirectory = scenarioDirectory + "-" + fileIndex;
            Directory.CreateDirectory(scenarioDirectory);
            Log.WriteLine(scenarioDirectory);
            try
            {
                _httpContext.SaveToDisk(Path.Combine(scenarioDirectory, "HttpContext.xml"));
            }
            catch (Exception e) {
                Log.WriteLine("Exception writing HttpContext to Output File");
            }
            try
            {
                _httpContext.SaveToFhirContextToDisk(Path.Combine(scenarioDirectory, "FhirContext.xml"));
            }
            catch (Exception e) {
                Log.WriteLine("Exception writing FhirContext to Output File");
            }
        }


        [BeforeTestRun]
        public static void SetTestRunId()
        {
            GlobalContext.TestRunId = Guid.NewGuid();
        }

        private static Func<Uri, FhirClient> GetFhirClientFactory()
        {
            return uri =>
            {
                var client = new FhirClient(uri)
                {
                    PreferredFormat = ResourceFormat.Json,
                };

                return client;
            };
        }
    }
}