﻿using System;
using System.IO;
using System.Linq;
using BoDi;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using GPConnect.Provider.AcceptanceTests.Importers;
using GPConnect.Provider.AcceptanceTests.Logger;
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
        private readonly FhirContext _fhirContext;

        public GenericSteps(IObjectContainer objectContainer, FhirContext fhirContext)
        {
            Log.WriteLine("GenericSteps() Constructor");
            _objectContainer = objectContainer;
            _fhirContext = fhirContext;
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

        [BeforeTestRun(Order = 2)]
        public static void LoadFhirDefinitions()
        {
            if (!Directory.Exists(AppSettingsHelper.DataDirectory))
            {
                Assert.Fail("Data Directory Not Found.");
            }

            var resolver = new ArtifactResolver(new FileDirectoryArtifactSource(AppSettingsHelper.FhirDirectory, true));
            var gender = resolver.GetValueSet("http://fhir.nhs.net/ValueSet/administrative-gender-1");
            if (gender == null)
                Assert.Fail("Gender ValueSet Not Found.");
            Log.WriteLine("{0} Genders Loaded.", gender.CodeSystem.Concept.Count);
            GlobalContext.FhirGenderValueSet = gender;

            var maritalStatus = resolver.GetValueSet("http://fhir.nhs.net/ValueSet/marital-status-1");
            if (maritalStatus == null)
                Assert.Fail("MaritalStatus ValueSet Not Found.");
            Log.WriteLine("{0} MaritalStatus Loaded.", maritalStatus.CodeSystem.Concept.Count);
            GlobalContext.FhirMaritalStatusValueSet = maritalStatus;

            var relationship = resolver.GetValueSet("http://fhir.nhs.net/ValueSet/cda-person-relationship-type-1");
            if (relationship == null)
                Assert.Fail("Relationship ValueSet Not Found.");
            Log.WriteLine("{0} Relationship Loaded.", relationship.CodeSystem.Concept.Count);
            GlobalContext.FhirRelationshipValueSet = relationship;

            var humanLanguage = resolver.GetValueSet("http://fhir.nhs.net/ValueSet/human-language-1");
            if (humanLanguage == null)
                Assert.Fail("HumanLanguage ValueSet Not Found.");
            Log.WriteLine("{0} HumanLanguage Loaded.", humanLanguage.CodeSystem.Concept.Count);
            GlobalContext.FhirHumanLanguageValueSet = humanLanguage;
        }

        [BeforeScenario(Order = 0)]
        public void InitializeContainer()
        {
            Log.WriteLine("InitializeContainer For Dependency Injection");
            _objectContainer.RegisterTypeAs<SecurityContext, ISecurityContext>();
            _objectContainer.RegisterTypeAs<HttpContext, IHttpContext>();
            // HACK To Be Able To See What We've Loaded In The BeforeTestRun Phase
            Log.WriteLine("{0} Organisations Loaded From ODS CSV File.", GlobalContext.OdsCodeMap.Count);
            Log.WriteLine("{0} Genders Loaded From FHIR ValueSet File.", GlobalContext.FhirGenderValueSet.CodeSystem.Concept.Count);
            Log.WriteLine("{0} MaritalStatus Loaded From FHIR ValueSet File.", GlobalContext.FhirMaritalStatusValueSet.CodeSystem.Concept.Count);
            Log.WriteLine("{0} Relationship Loaded From FHIR ValueSet File.", GlobalContext.FhirRelationshipValueSet.CodeSystem.Concept.Count);
            Log.WriteLine("{0} HumanLanguage Loaded From FHIR ValueSet File.", GlobalContext.FhirHumanLanguageValueSet.CodeSystem.Concept.Count);
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