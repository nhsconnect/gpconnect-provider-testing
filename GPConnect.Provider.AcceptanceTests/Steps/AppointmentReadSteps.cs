using GPConnect.Provider.AcceptanceTests.Context;
using Hl7.Fhir.Model;
using Shouldly;
using TechTalk.SpecFlow;
using static Hl7.Fhir.Model.Appointment;
using NUnit.Framework;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public class AppointmentReadSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private readonly HttpContext HttpContext;
        
        public AppointmentReadSteps(FhirContext fhirContext, HttpContext httpContext)
        {
            FhirContext = fhirContext;
            HttpContext = httpContext;
        }

        [Given(@"I find appointments for patient ""([^""]*)"" at organization ""([^""]*)"" and save the bundle of appointment resources to ""([^""]*)""")]
        public void createFindOrganisationAndAssignSchedule(string patient, string organizaitonName, string bundleOfPatientAppointmentskey) {
            // Search For Patient appointments
            Given($@"I search for patient ""{patient}"" appointments and save the returned bundle of appointment resources against key ""{bundleOfPatientAppointmentskey}""");
            Bundle patientAppointmentsBundle = (Bundle)HttpContext.StoredFhirResources[bundleOfPatientAppointmentskey];

            Given($@"I perform the getSchedule operation for organization ""{organizaitonName}"" and store the returned bundle resources against key ""getScheduleResponseBundle""");
        }

        [Given(@"I find or create ""([^ ""] *)"" appointments for patient ""([^""]*)"" at organization ""([^""]*)"" and save bundle of appintment resources to ""([^""]*)""")]
        public void IFindOrCreateAAppointmentsForPatientAtOrganizationAndSaveAListOfResourceTo(int noApp, string patient, string organizaitonName, string bundleOfPatientAppointmentskey)
        {
            // Search For Patient appointments
            Given($@"I search for patient ""{patient}"" appointments and save the returned bundle of appointment resources against key ""{bundleOfPatientAppointmentskey}""");
            Bundle patientAppointmentsBundle = (Bundle)HttpContext.StoredFhirResources[bundleOfPatientAppointmentskey];

            int numberOfRequiredAdditionalAppointments = noApp - patientAppointmentsBundle.Entry.Count;
            if (numberOfRequiredAdditionalAppointments > 0)
            {
                // Perform get schedule once to get available slots with which to create appointments
                Given($@"I perform the getSchedule operation for organization ""{organizaitonName}"" and store the returned bundle resources against key ""getScheduleResponseBundle""");
                for (int numberOfAppointmentsToCreate = numberOfRequiredAdditionalAppointments; numberOfAppointmentsToCreate > 0; numberOfAppointmentsToCreate--)
                {
                    When($@"I book an appointment for patient ""{patient}"" on the provider system using a slot from the getSchedule response bundle stored against key ""getScheduleResponseBundle"" and store the appointment to ""storedAppointment""");
        
                }

                // Search for appointments again to make sure that enough have been stored in the provider system and store them
                Given($@"I search for patient ""{patient}"" appointments and save the returned bundle of appointment resources against key ""{bundleOfPatientAppointmentskey}""");
                patientAppointmentsBundle = (Bundle)HttpContext.StoredFhirResources[bundleOfPatientAppointmentskey];
            }
            patientAppointmentsBundle.Entry.Count.ShouldBeGreaterThanOrEqualTo(noApp, "We could not create enough appointments for the test to run.");
        }

        [Given(@"I find or create an appointment with status Booked for patient ""([^""]*)"" at organization ""([^""]*)"" and save the appointment resources to ""([^""]*)""")]
        public void IFindOrCreateAnAppointmentWithStatusBookedForPatientAtOrganizationAndSaveTheAppointmentResourceTo(string patient, string organizaitonName, string patientAppointmentkey)
        {
            // Search For Patient appointments
            Given($@"I search for patient ""{patient}"" appointments and save the returned bundle of appointment resources against key ""bundleOfPatientAppointmentskey""");
            Bundle patientAppointmentsBundle = (Bundle)HttpContext.StoredFhirResources["bundleOfPatientAppointmentskey"];

            Appointment appointmentResource = null;
            // Find an appointment of required type
            foreach (var entry in patientAppointmentsBundle.Entry) {
                Appointment appointment = (Appointment)entry.Resource;
                if (appointment.Status == AppointmentStatus.Booked)
                {
                    appointmentResource = appointment;
                    break;
                }
            }
            if (appointmentResource == null)
            {
                // No booked appointment found so create and store one
                Given($@"I perform the getSchedule operation for organization ""{organizaitonName}"" and store the returned bundle resources against key ""getScheduleResponseBundle""");
                When($@"I book an appointment for patient ""{patient}"" on the provider system using a slot from the getSchedule response bundle stored against key ""getScheduleResponseBundle"" and store the appointment to ""{patientAppointmentkey}""");
            }
            else
            {
                // Else one found so I store it for later use
                HttpContext.StoredFhirResources.Add(patientAppointmentkey, appointmentResource);
            }
        }

        [Given(@"I find or create an appointment with status Cancelled for patient ""([^""]*)"" at organization ""([^""]*)"" and save the appointment resources to ""([^""]*)""")]
        public void IFindOrCreateAnAppointmentWithStatusCancelledForPatientAtOrganizationAndSaveTheAppointmentResourceTo(string patient, string organizaitonName, string patientAppointmentkey)
        {
            // Search For Patient appointments
            Given($@"I search for patient ""{patient}"" appointments and save the returned bundle of appointment resources against key ""bundleOfPatientAppointmentskey""");
            Bundle patientAppointmentsBundle = (Bundle)HttpContext.StoredFhirResources["bundleOfPatientAppointmentskey"];

            Appointment cancelledAppointmentResource = null;
            Appointment bookedAppointmentResource = null;
            // Find an appointment of required type
            foreach (var entry in patientAppointmentsBundle.Entry)
            {
                Appointment appointment = (Appointment)entry.Resource;
                if (appointment.Status == AppointmentStatus.Cancelled)
                {
                    cancelledAppointmentResource = appointment;
                }
                else if (appointment.Status == AppointmentStatus.Booked)
                {
                    bookedAppointmentResource = appointment;
                }
            }
            if (cancelledAppointmentResource == null)
            {
                // Find or create a booked appointment to cancel
                if (bookedAppointmentResource == null)
                {
                    // No booked appointment found so create and store one
                    Given($@"I perform the getSchedule operation for organization ""{organizaitonName}"" and store the returned bundle resources against key ""getScheduleResponseBundle""");
                    When($@"I book an appointment for patient ""{patient}"" on the provider system using a slot from the getSchedule response bundle stored against key ""getScheduleResponseBundle"" and store the appointment to ""bookedAppointmentKey""");
                }
                else
                {
                    HttpContext.StoredFhirResources.Add("bookedAppointmentKey", bookedAppointmentResource);
                }
                // Cancel appointment
                Given($@"I cancel appointment resource stored against key ""bookedAppointmentKey"" and store the returned appointment resource against key ""{patientAppointmentkey}""");
            }
            else
            {
                HttpContext.StoredFhirResources.Add(patientAppointmentkey, cancelledAppointmentResource);
            }
        }

        [Given(@"I search for patient ""([^""]*)"" appointments and save the returned bundle of appointment resources against key ""([^""]*)""")]
        public void ISearchForPatientAppointmentsAndSaveTheReturnedBundleOfAppointmentResourcesAgainstKey(string patient, string patientAppointmentSearchBundleKey)
        {
            // Search For Patient
            Given($@"I perform a patient search for patient ""{patient}"" and store the first returned resources against key ""{patient}""");
            // Search For Patients Appointments
            Patient patientResource = (Patient)HttpContext.StoredFhirResources[patient];
            Given($@"I am using the default server");
            And($@"I am performing the ""urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments"" interaction");
            When($@"I make a GET request to ""/Patient/{patientResource.Id}/Appointment""");
            Then($@"the response status code should indicate success");
            And($@"the response body should be FHIR JSON");
            And($@"the response should be a Bundle resource of type ""searchset""");
            var returnedPatientAppointmentSearchBundle = (Bundle)FhirContext.FhirResponseResource;
            if (HttpContext.StoredFhirResources.ContainsKey(patientAppointmentSearchBundleKey)) HttpContext.StoredFhirResources.Remove(patientAppointmentSearchBundleKey);
            HttpContext.StoredFhirResources.Add(patientAppointmentSearchBundleKey, returnedPatientAppointmentSearchBundle);
        }

        [Given(@"I perform the getSchedule operation for organization ""([^""]*)"" and store the returned bundle resources against key ""([^""]*)""")]
        public void IPerformTheGetScheduleOperationForOrganizationAndStoreTheReturnedBundleResourceAgainstKey(string organization, string getScheduleResponseBundleKey)
        {
            // getSchedule operation for organization
            Given($@"I am using the default server");
            And($@"I search for the organization ""{organization}"" on the providers system and save the first response to ""firstOrganizationResource""");
            Given($@"I am using the default server");
            And($@"I am performing the ""urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule"" interaction");
            And($@"I add period request parameter with a start date of today and an end date ""13"" days later");
            When($@"I send a gpc.getschedule operation for the organization stored as ""firstOrganizationResource""");
            Then($@"the response status code should indicate success");
            And($@"the response body should be FHIR JSON");
            And($@"the response should be a Bundle resource of type ""searchset""");
            And($@"the response bundle should include slot resources");
            var returnedGetScheduleResponseBundle = (Bundle)FhirContext.FhirResponseResource;
            if (HttpContext.StoredFhirResources.ContainsKey(getScheduleResponseBundleKey)) HttpContext.StoredFhirResources.Remove(getScheduleResponseBundleKey);
            HttpContext.StoredFhirResources.Add(getScheduleResponseBundleKey, returnedGetScheduleResponseBundle);
        }

        [When(@"I perform an appointment read for the first appointment saved in the bundle of resources stored against key ""([^""]*)""")]
        public void IPerformAnAppointmentReadForTheFirstAppointmentSavedInTheLBundleOfResorcesStoredAgainstKey(string bundleOfPatientAppointmentsKey)
        {
            When($@"I perform an appointment read appointment index ""0"" saved in the bundle of resources stored against key ""{bundleOfPatientAppointmentsKey}""");
        }

        [When(@"I perform an appointment read appointment index ""([^""]*)"" saved in the bundle of resources stored against key ""([^""]*)""")]
        public void IPerformAnAppointmentReadForTheAppointmentIndexSavedInTheBundleOfResourcesStoredAgainstKey(int appointmentIndex, string bundleOfPatientAppointmentsKey)
        {
            Bundle patientAppointmentBundel = (Bundle)HttpContext.StoredFhirResources[bundleOfPatientAppointmentsKey];
            When($@"I perform an appointment read for the appointment with logical id ""{patientAppointmentBundel.Entry[appointmentIndex].Resource.Id}"""); // Get the Id of the first appointment
        }
        
        [When(@"I perform an appointment read appointment stored against key ""([^""]*)""")]
        public void IPerformAnAppointmentReadForAppointmentStoredAgainstKey(string storedAppointmentKey)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[storedAppointmentKey];
            When($@"I perform an appointment read for the appointment with logical id ""{storedAppointment.Id}""");
        }

        [When(@"I perform an appointment read for the appointment with logical id ""([^""]*)""")]
        public void IPerformAnAppointmentReadForTheAppointment(string appointmentLogicalId)
        {
            When($@"I make a GET request to ""/Appointment/{appointmentLogicalId}""");
        }
        
        [When(@"I perform an appointment vread with history for appointment stored against key ""([^""]*)""")]
        public void IPerformAnAppointmentVReadWithHistoryForAppointmentStoredAgainstKey(string storedAppointmentKey)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[storedAppointmentKey];
            When($@"I make a GET request to ""/Appointment/{storedAppointment.Id}/_history/{storedAppointment.Meta.VersionId}""");
        }

        [When(@"I perform an appointment vread with version id ""([^""]*)"" for appointment stored against key ""([^""]*)""")]
        public void IPerformAnAppointmentVReadWithVersionIdForAppointmentStoredAgainstKey(string versionId, string storedAppointmentKey)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[storedAppointmentKey];
            When($@"I make a GET request to ""/Appointment/{storedAppointment.Id}/_history/{versionId}""");
        }

        [Then(@"the response should be an Appointment resource")]
        public void theResponseShouldBeAnAppointmentResource()
        {
            FhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.Appointment);  
        }

        [Then(@"the response should be an Appointment resource which is saved as ""([^""]*)""")]
        public void theResponseShouldBeAnAppointmentResource(string appointmentName)
        {
            HttpContext.StoredAppointment.Remove(appointmentName);
            HttpContext.StoredAppointment.Add(appointmentName, (Appointment)FhirContext.FhirResponseResource);
        }

        [Then(@"the response should be a Location resource")]
        public void theResponseShouldBeAnLocationResource()
        {
            FhirContext.FhirResponseResource.ResourceType.ShouldBe(ResourceType.Location);
        }

        [Then(@"the returned appointment resource should contain meta data profile and version id")]
        public void ThenTheReturnedAppointmentResourceShouldContainMetaDataProfileAndVersionId()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            appointment.Meta.ShouldNotBeNull("Returned resource should contain a meta data element.");
            int metaProfileCount = 0;
            foreach (string profile in appointment.Meta.Profile)
            {
                metaProfileCount++;
                profile.ShouldBe("http://fhir.nhs.net/StructureDefinition/gpconnect-appointment-1", "The Meta data elemenet profile is not correct for the resource.");
            }
            metaProfileCount.ShouldBe(1, "The returned resource meta data element should contain only one profile element");
            appointment.Meta.VersionId.ShouldNotBeNull("The returned resource meta data element should contain a versionId element");
        }

        [Then(@"the appointment response resource contains atleast 2 participants a practitioner and a patient")]
        public void ThenTheAppointmentResponseResourceContainsAtleast2ParticipantsAPractitionerAndAPatient()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            appointment.Participant.ShouldNotBeNull();
            bool patientFound = false;
            bool practitionerFound = false;
            foreach (Appointment.ParticipantComponent participant in appointment.Participant)
            {
                if (participant.Actor.Reference.StartsWith("Patient/")) {
                    patientFound = true;
                }
                else if (participant.Actor.Reference.StartsWith("Practitioner/"))
                {
                    practitionerFound = true;
                }
            }
            patientFound.ShouldBeTrue("Patient reference not found in appointment");
            practitionerFound.ShouldBeTrue("Practitioner reference not found in appointment");
        }

        [Then(@"if the appointment response resource contains any identifiers they must have a value")]
        public void ThenIfTheAppointmentResponseResourceContainsAnyIdentifiersTheyMustHaveAValue()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            foreach (var identifier in appointment.Identifier) {
                identifier.Value.ShouldNotBeNullOrEmpty();
            }
        }

        [Then(@"if the appointment response resource contains a reason element and coding the codings must be one of the three allowed with system code and display elements")]
        public void ThenIfTheAppointmentResponseResourceContainsAReasonElementAndCodingItMustBeOneOfTheThreeAllowedWithSystemCodeAndDisplayElements()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            var reason = appointment.Reason;
            if (reason != null && reason.Coding != null) {
                int sctCount = 0;
                int readv2Count = 0;
                int ctv3Count = 0;
                foreach (var coding in reason.Coding) {
                    var validSystems = new string[3] { "http://snomed.info/sct", "http://read.info/readv2", "http://read.info/ctv3" };
                    coding.System.ShouldBeOneOf(validSystems, "The reason coding System can only be one of the valid value");

                    switch(coding.System){
                        case "http://snomed.info/sct":
                            sctCount++;
                            break;
                        case "http://read.info/readv2":
                            readv2Count++;
                            break;
                        case "http://read.info/ctv3":
                            ctv3Count++;
                            break;
                    }
                    coding.Code.ShouldNotBeNullOrEmpty("The appointment reason coding Code must have a value");
                    coding.Display.ShouldNotBeNullOrEmpty("The appointment reason coding display must have a value");
                }
                // Check there is no more than one of each coding
                sctCount.ShouldBeLessThanOrEqualTo(1);
                readv2Count.ShouldBeLessThanOrEqualTo(1);
                ctv3Count.ShouldBeLessThanOrEqualTo(1);
            }
        }

        [Then(@"if the appointment contains a priority element it should be a valid value")]
        public void ThenIfTheAppointmentContainsAPriorityElementItShouldBeAValidValue()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            int? priority = appointment.Priority;
            if (priority != null)
            {
                priority.Value.ShouldBeLessThanOrEqualTo(9, "The priority should be between 0 and 9");
                priority.Value.ShouldBeGreaterThanOrEqualTo(0, "The priority should be between 0 and 9");
            }
        }

        [Then(@"the returned appointment participants must contain a type or actor element")]
        public void ThenTheReturnedAppointmentParticipantsMustContainATypeOrActorElement()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            foreach (ParticipantComponent participant in appointment.Participant)
            {
                var actor = participant.Actor;
                var type = participant.Type;

                if (null == actor && null == type)
                {
                    Assert.Fail("There must be an actor or type element within the appointment participants");
                }

                if (null != type)
                {
                    int codableConceptCount = 0;
                    foreach (var typeCodableConcept in type)
                    {
                        codableConceptCount++;
                        int codingCount = 0;
                        foreach (var coding in typeCodableConcept.Coding)
                        {
                            coding.System.ShouldBe("http://hl7.org/fhir/ValueSet/encounter-participant-type");
                            string[] codes = new string[12] { "translator", "emergency", "ADM", "ATND", "CALLBCK", "CON", "DIS", "ESC", "REF", "SPRF", "PPRF", "PART" };
                            string[] codeDisplays = new string[12] { "Translator", "Emergency", "admitter", "attender", "callback contact", "consultant", "discharger", "escort", "referrer", "secondary performer", "primary performer", "Participation" };
                            coding.Code.ShouldBeOneOf(codes);
                            coding.Display.ShouldBeOneOf(codeDisplays);
                            for (int i = 0; i < codes.Length; i++) {
                                if (string.Equals(coding.Code, codes[i])) {
                                    coding.Display.ShouldBe(codeDisplays[i], "The participant type code does not match the display element");
                                }
                            }
                            codingCount++;
                        }
                        codingCount.ShouldBeLessThanOrEqualTo(1, "There should be a maximum of 1 participant type coding element for each participant");
                    }
                    codableConceptCount.ShouldBeLessThanOrEqualTo(1, "The participant type element may only contain one codable concept.");
                }

                if (actor != null && actor.Reference != null) {
                    actor.Reference.ShouldNotBeEmpty();
                    if (!actor.Reference.StartsWith("Patient/") && 
                        !actor.Reference.StartsWith("Practitioner/") && 
                        !actor.Reference.StartsWith("Location/")) {
                        Assert.Fail("The actor reference should be a Patient, Practitioner or Location");
                    }
                }
            }
        }

        [Then(@"if the returned appointment contains appointmentCategory extension the value should be valid")]
        public void ThenIfTheReturnedAppointmentContainsAppointmentCategoryExtensionTheValueShouldBeValid()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            foreach (var extension in appointment.Extension) {
                if (string.Equals(extension.Url, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1")) {

                    string[] codes = new string[5] { "CLI", "ADM", "VIR", "REM", "MSG" };
                    string[] displays = new string[5] { "Clinical", "Administrative", "Virtual", "Reminder", "Message" };

                    extension.Value.ShouldNotBeNull("There should be a value element within the appointment category extension");
                    var extensionValueCodeableConcept = (CodeableConcept)extension.Value;
                    extensionValueCodeableConcept.Coding.ShouldNotBeNull("There should be a coding element within the appointment category extension");
                    extensionValueCodeableConcept.Coding.Count.ShouldBe(1, "There should be a single code element within the appointment category extension");
                    foreach (var coding in extensionValueCodeableConcept.Coding) {
                        // Check that the code and display values are valid for the extension and match each other
                        bool codeAndDisplayFound = false;
                        for (int i = 0; i < codes.Length; i++) {
                            if (string.Equals(codes[i], coding.Code) && string.Equals(displays[i], coding.Display)) {
                                codeAndDisplayFound = true;
                                break;
                            }
                        }
                        codeAndDisplayFound.ShouldBeTrue("The code and display values are not valid for the appointmentCategory extension");
                    }
                }
            }
        }

        [Then(@"if the returned appointment contains appointmentBookingMethod extension the value should be valid")]
        public void ThenIfTheReturnedAppointmentContainsAppointmentBookingMethodExtensionTheValueShouldBeValid()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            foreach (var extension in appointment.Extension)
            {
                if (string.Equals(extension.Url, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1"))
                {

                    string[] codes = new string[6] { "ONL", "PER", "TEL", "EMA", "LET", "TEX" };
                    string[] displays = new string[6] { "Online", "In person", "Telephone", "Email", "Letter", "Text" };

                    extension.Value.ShouldNotBeNull("There should be a value element within the appointment booking method extension");
                    var extensionValueCodeableConcept = (CodeableConcept)extension.Value;
                    extensionValueCodeableConcept.Coding.ShouldNotBeNull("There should be a coding element within the appointment booking method extension");
                    extensionValueCodeableConcept.Coding.Count.ShouldBe(1, "There should be a single code element within the appointment booking method extension");
                    foreach (var coding in extensionValueCodeableConcept.Coding)
                    {
                        // Check that the code and display values are valid for the extension and match each other
                        bool codeAndDisplayFound = false;
                        for (int i = 0; i < codes.Length; i++)
                        {
                            if (string.Equals(codes[i], coding.Code) && string.Equals(displays[i], coding.Display))
                            {
                                codeAndDisplayFound = true;
                                break;
                            }
                        }
                        codeAndDisplayFound.ShouldBeTrue("The code and display values are not valid for the appointmentBookingMethod extension");
                    }
                }
            }
        }

        [Then(@"if the returned appointment contains appointmentContactMethod extension the value should be valid")]
        public void ThenIfTheReturnedAppointmentContainsAppointmentContactMethodExtensionTheValueShouldBeValid()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            foreach (var extension in appointment.Extension)
            {
                if (string.Equals(extension.Url, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1"))
                {

                    string[] codes = new string[5] { "ONL", "PER", "TEL", "EMA", "LET" };
                    string[] displays = new string[5] { "Online", "In person", "Telephone", "Email", "Letter" };

                    extension.Value.ShouldNotBeNull("There should be a value element within the appointment ContactMethod extension");
                    var extensionValueCodeableConcept = (CodeableConcept)extension.Value;
                    extensionValueCodeableConcept.Coding.ShouldNotBeNull("There should be a coding element within the appointment ContactMethod extension");
                    extensionValueCodeableConcept.Coding.Count.ShouldBe(1, "There should be a single code element within the appointment ContactMethod extension");
                    foreach (var coding in extensionValueCodeableConcept.Coding)
                    {
                        // Check that the code and display values are valid for the extension and match each other
                        bool codeAndDisplayFound = false;
                        for (int i = 0; i < codes.Length; i++)
                        {
                            if (string.Equals(codes[i], coding.Code) && string.Equals(displays[i], coding.Display))
                            {
                                codeAndDisplayFound = true;
                                break;
                            }
                        }
                        codeAndDisplayFound.ShouldBeTrue("The code and display values are not valid for the appointmentContactMethod extension");
                    }
                }
            }
        }

        [Then(@"if the returned appointment contains appointmentCancellationReason extension the value should be valid")]
        public void ThenIfTheReturnedAppointmentContainsAppointmentCancellationReasonExtensionTheValueShouldBeValid()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            foreach (var extension in appointment.Extension)
            {
                if (string.Equals(extension.Url, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1"))
                {
                    extension.Value.ShouldNotBeNull("There should be a value element within the appointment CancellationReason extension");
                    var extensionValueString = (FhirString)extension.Value;
                    extensionValueString.Value.ShouldNotBeNullOrEmpty("If appointment cancellation extension is included the value should be present");
                }
            }
        }

        [Then(@"the response should contain the ETag header matching the resource version")]
        public void ThenTheResponseShouldContainTheETagHeaderMatchingTheResourceVersion()
        {
            Resource resource = FhirContext.FhirResponseResource;
            string returnedETag = "";
            HttpContext.ResponseHeaders.TryGetValue("ETag", out returnedETag);
            returnedETag.ShouldStartWith("W/\"", "The WTag header should start with W/\"");
            returnedETag.ShouldEndWith(resource.Meta.VersionId + "\"", "The ETag header should contain the resource version enclosed within speech marks");
            returnedETag.ShouldBe("W/\"" + resource.Meta.VersionId + "\"", "The ETag header contains invalid characters");
        }
    }
}
