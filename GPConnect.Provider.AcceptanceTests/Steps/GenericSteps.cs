using System;
using System.IO;
using System.Linq;
using BoDi;
using GPConnect.Provider.AcceptanceTests.Constants;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Importers;
using GPConnect.Provider.AcceptanceTests.Logger;
using Hl7.Fhir.Model;
using Hl7.Fhir.Specification.Source;
using NUnit.Framework;
using TechTalk.SpecFlow;
// ReSharper disable UnusedMember.Global

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public class GenericSteps : TechTalk.SpecFlow.Steps
    {
        private readonly IObjectContainer _objectContainer;

        public GenericSteps(IObjectContainer objectContainer)
        {
            Log.WriteLine("GenericSteps() Constructor");
            _objectContainer = objectContainer;
        }

        [BeforeTestRun(Order = 1)]
        public static void CreateTraceFolder()
        {
            if (!Directory.Exists(AppSettingsHelper.TraceBaseDirectory)) return;
            var folderName = DateTime.Now.ToString("s").Replace(":", string.Empty);
            var traceDirectory = Path.Combine(AppSettingsHelper.TraceBaseDirectory, folderName);
            Log.WriteLine("Create Trace Directory = '{0}'", traceDirectory);
            Directory.CreateDirectory(traceDirectory);
            // Save The Newly Created Trace Directory To The Global Context
            GlobalContext.TraceDirectory = traceDirectory;
        }

        [BeforeTestRun(Order = 1)]
        public static void LoadPDSData()
        {
            if (Directory.Exists(AppSettingsHelper.DataDirectory) == false)
                Assert.Fail("Data Directory Not Found.");
            var pdsCSV = Path.Combine(AppSettingsHelper.DataDirectory, @"PDS.csv");
            Log.WriteLine("PDS CSV = '{0}'", pdsCSV);
            GlobalContext.PDSData = PDSImporter.LoadCsv(pdsCSV);
        }

        [BeforeTestRun(Order = 2)]
        public static void LoadFhirDefinitions()
        {
            if (Directory.Exists(AppSettingsHelper.FhirDirectory) == false)
                Assert.Fail("FHIR Directory Not Found.");
            var resolver = new ArtifactResolver(new FileDirectoryArtifactSource(AppSettingsHelper.FhirDirectory, true));
            var gender = resolver.GetValueSet("http://fhir.nhs.net/ValueSet/administrative-gender-1");
            if (gender == null)
                Assert.Fail("Gender ValueSet Not Found.");
            Log.WriteLine("{0} Genders Loaded.", gender.CodeSystem.Concept.Count);
            GlobalContext.FhirGenderValueSet = gender;
        }

        [BeforeScenario(Order = 0)]
        public void InitializeContainer()
        {
            Log.WriteLine("InitializeContainer For Dependency Injection");
            _objectContainer.RegisterTypeAs<SecurityContext, ISecurityContext>();
            _objectContainer.RegisterTypeAs<HttpContext, IHttpContext>();
            // HACK To Be Able To See What We've Loaded In The BeforeTestRun Phase
            Log.WriteLine("{0} Patients Loaded From PDS CSV File.", GlobalContext.PDSData.ToList().Count);
            Log.WriteLine("{0} Genders Loaded From FHIR ValueSet File.", GlobalContext.FhirGenderValueSet.CodeSystem.Concept.Count);
        }

        [BeforeScenario(Order = 1)]
        public void OutputScenarioDetails()
        {
            Log.WriteLine("Feature: " + FeatureContext.Current.FeatureInfo.Title);
            Log.WriteLine(FeatureContext.Current.FeatureInfo.Description);
            Log.WriteLine("");
            Log.WriteLine("Scenario: " + ScenarioContext.Current.ScenarioInfo.Title);
        }
    }
}