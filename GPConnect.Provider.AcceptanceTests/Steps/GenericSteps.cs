﻿using System;
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
    using Repository;
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

        [BeforeTestRun(Order = 2)]
        public static void LoadFhirDefinitions()
        {
            if (!Directory.Exists(AppSettingsHelper.DataDirectory))
            {
                Assert.Fail("Data Directory Not Found.");
            }

            var vsSources = new List<IResourceResolver>();

            if (AppSettingsHelper.FhirCheckWeb && AppSettingsHelper.FhirCheckWebFirst)
            {
                vsSources.Add(new WebResolver(uri => new FhirClient(AppSettingsHelper.FhirWebDirectory)));
            }

            if (AppSettingsHelper.FhirCheckDisk)
            {
                vsSources.Add(new DirectorySource(AppSettingsHelper.FhirDirectory, true));
            }

            if (AppSettingsHelper.FhirCheckWeb && !AppSettingsHelper.FhirCheckWebFirst)
            {
                vsSources.Add(new WebResolver(uri => new FhirClient(AppSettingsHelper.FhirWebDirectory)));
            }

            var resolver = new MultiResolver(vsSources);
            var gender = resolver.FindValueSet("http://fhir.nhs.net/ValueSet/administrative-gender-1");
            if (gender == null)
                Assert.Fail("Gender ValueSet Not Found.");
            Log.WriteLine("{0} Genders Loaded.", gender.CodeSystem.Concept.Count);
            GlobalContext.FhirGenderValueSet = gender;

            var maritalStatus = resolver.FindValueSet("https://fhir.nhs.uk/STU3/ValueSet/CareConnect-MaritalStatus-1");
            if (maritalStatus == null)
                Assert.Fail("MaritalStatus ValueSet Not Found.");
            Log.WriteLine("{0} MaritalStatus Loaded.", maritalStatus.CodeSystem.Concept.Count);
            GlobalContext.FhirMaritalStatusValueSet = maritalStatus;

            var relationship = resolver.FindValueSet("http://hl7.org/fhir/ValueSet/v2-0131");
            if (relationship == null)
                Assert.Fail("Relationship ValueSet Not Found.");
            Log.WriteLine("{0} Relationship Loaded.", relationship.CodeSystem.Concept.Count);
            GlobalContext.FhirRelationshipValueSet = relationship;

            var humanLanguage = resolver.FindValueSet("http://hl7.org/fhir/stu3/valueset-languages.html");
            if (humanLanguage == null)
                Assert.Fail("HumanLanguage ValueSet Not Found.");
            Log.WriteLine("{0} HumanLanguage Loaded.", humanLanguage.CodeSystem.Concept.Count);
            GlobalContext.FhirHumanLanguageValueSet = humanLanguage;

            var appointmentCategory = resolver.FindValueSet("http://fhir.nhs.net/ValueSet/gpconnect-appointment-category-1");
            if (appointmentCategory == null)
                Assert.Fail("AppointmentCategory ValueSet Not Found.");
            Log.WriteLine("{0} AppointmentCategory Loaded.", appointmentCategory.CodeSystem.Concept.Count);
            GlobalContext.FhirAppointmentCategoryValueSet = appointmentCategory;

            var appointmentBookingMethod = resolver.FindValueSet("http://fhir.nhs.net/ValueSet/gpconnect-appointment-booking-method-1");
            if (appointmentBookingMethod == null)
                Assert.Fail("AppointmentBookingMethod ValueSet Not Found.");
            Log.WriteLine("{0} AppointmentBookingMethod Loaded.", appointmentBookingMethod.CodeSystem.Concept.Count);
            GlobalContext.FhirAppointmentBookingMethodValueSet = appointmentBookingMethod;

            var appointmentContactMethod = resolver.FindValueSet("http://fhir.nhs.net/ValueSet/gpconnect-appointment-contact-method-1");
            if (appointmentContactMethod == null)
                Assert.Fail("AppointmentContactMethod ValueSet Not Found.");
            Log.WriteLine("{0} AppointmentContactMethod Loaded.", appointmentContactMethod.CodeSystem.Concept.Count);
            GlobalContext.FhirAppointmentContactMethodValueSet = appointmentContactMethod;

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
    }
}