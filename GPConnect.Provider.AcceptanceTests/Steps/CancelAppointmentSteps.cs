using System.Collections.Generic;
using GPConnect.Provider.AcceptanceTests.Context;
using GPConnect.Provider.AcceptanceTests.Helpers;
using Hl7.Fhir.Model;
using Shouldly;
using TechTalk.SpecFlow;
using static Hl7.Fhir.Model.Appointment;
using static Hl7.Fhir.Model.Bundle;
using NUnit.Framework;
using Hl7.Fhir.Serialization;
using System;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public class CancelAppointmentSteps : TechTalk.SpecFlow.Steps
    {
        private readonly FhirContext FhirContext;
        private readonly AccessRecordSteps AccessRecordSteps;
        private readonly HttpSteps HttpSteps;
        private readonly HttpContext HttpContext;

        // Headers Helper
        public HttpHeaderHelper Headers { get; }

        public CancelAppointmentSteps(HttpHeaderHelper headerHelper, AccessRecordSteps accessRecordSteps, FhirContext fhirContext, HttpSteps httpSteps, HttpContext httpContext)
        {
            // Helpers
            FhirContext = fhirContext;
            AccessRecordSteps = accessRecordSteps;
            Headers = headerHelper;
            HttpSteps = httpSteps;
            HttpContext = httpContext;
        }

        [Given(@"I get the first appointment saved in the bundle of resources stored against key ""(.*)"" and call it ""(.*)""")]
        public void GivenIGetTheFirstAppointmnetSavedInTheBundleOfResourcesStoredAgainstKeyAndCallIt(string bundleOfPatientAppointmentsKey, string appointmentName)
        {
            Bundle patientAppointmentBundel = (Bundle)HttpContext.StoredFhirResources[bundleOfPatientAppointmentsKey];
            string appointmentLogicalId = patientAppointmentBundel.Entry[0].Resource.Id;
            When($@"I make a GET request to ""/Appointment/{appointmentLogicalId}""");
            Then("the response status code should indicate success");
            And("the response body should be FHIR JSON");
            And($@"the response should be an Appointment resource which is saved as ""{appointmentName}""");
        }

        [Given(@"I perform an appointment read on appointment saved with key ""(.*)"" and read the etag and save it as ""(.*)""")]
        public void GivenIperformAnAppointmentReadOnAppointmentSavedWithKeyAndReadTheEtagAndSaveItAs(string appointmentName, string etagName)
        {
            Given("I am using the default server");
            And(@"I am performing the ""urn:nhs:names:services:gpconnect:fhir:rest:read:appointment"" interaction");
            When($@"I perform an appointment read for the appointment called ""{appointmentName}""");
            Then("the response status code should indicate success");
            And("the response body should be FHIR JSON");
            And($@"the response ETag is saved as ""{etagName}""");
            And("the response should be an Appointment resource");
        }


        [When(@"I set the URL to ""(.*)"" and cancel appointment with key ""(.*)""")]
        public void WhenISetTheURLToAndCancelAppointmentWithKey(string URL, string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            storedAppointment.Status = AppointmentStatus.Cancelled;
            Extension extension = new Extension();
            List<Extension> extensionList = new List<Extension>();
            extension = (buildAppointmentCancelExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1", "Double booked"));
            storedAppointment.ModifierExtension.Add(extension);
            HttpSteps.RestRequest(RestSharp.Method.PUT, URL, FhirSerializer.SerializeToJson(storedAppointment));
        }

        [When(@"I cancel the appointment with the key ""(.*)"" and set the reason to double booked")]
        public void WhenICancelTheAppointmentWithTheKeyAndSetTheReasonToDoubleBooked(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            storedAppointment.Status = AppointmentStatus.Cancelled;
            string url = "Appointment/" + storedAppointment.Id;
            Extension extension = new Extension();
            List<Extension> extensionList = new List<Extension>();
            extension = (buildAppointmentCancelExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1", "Double booked"));
            storedAppointment.ModifierExtension.Add(extension);
            HttpSteps.RestRequest(RestSharp.Method.PUT, url, FhirSerializer.SerializeToJson(storedAppointment));
        }

        [Given(@"I add an extension to ""(.*)"" with url ""(.*)"" code ""(.*)"" and display ""(.*)""")]
        public void GivenIAddAnExtensionToWithUrlCodeAndDisplay(string appointmentName, string url, string code, string display)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            Extension extensionToAdd = new Extension();
            extensionToAdd = buildAppointmentOtherExtension(extensionToAdd,url, code , display);
            storedAppointment.ModifierExtension.Add(extensionToAdd);
        }

        [Given(@"I set the description to ""(.*)"" for appointment ""(.*)""")]
        public void GivenISetTheDescriptionToForAppointment(string description, string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            storedAppointment.Description = description;
        }

        [Given(@"I set the priority to ""(.*)"" for appointment ""(.*)""")]
        public void GivenISetThePriorityToForAppointment(int priority, string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            storedAppointment.Priority = priority;
        }

        [Given(@"I set the minutes to ""(.*)"" for appointment ""(.*)""")]
        public void GivenISetTheMinutesToForAppointment(int minutes, string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            storedAppointment.MinutesDuration = minutes;
        }

        [Given(@"I set the comment to ""(.*)"" for appointment ""(.*)""")]
        public void GivenISetTheCommentToForAppointment(string comment, string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            storedAppointment.Comment = comment;
        }

        [Given(@"I set the type text to ""(.*)"" for appointment ""(.*)""")]
        public void GivenISetTheTypeTextToForAppointment(string typeText, string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            storedAppointment.Type.Text = typeText;

        }

        [Given(@"I set the identifier with system ""(.*)"" and value ""(.*)"" for the appointment ""(.*)""")]
        public void GivenISetTheIdentifierWithSystemAndValueForTheAppointment(string system,string value, string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            Identifier identifier = new Identifier();
            identifier.System = system;
            identifier.Value = value;
            storedAppointment.Identifier.Add(identifier);
        }

        [Given(@"I add participant ""(.*)"" with reference ""(.*)"" to appointment ""(.*)""")]
        public void GivenIAddParticipantWithReferenceToAppointment(string participant,string reference, string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            switch (participant)
            {
                case "location":

                    ParticipantComponent location = new ParticipantComponent();
                    ResourceReference locationReference = new ResourceReference();
                    locationReference.Reference = reference;
                    location.Actor = locationReference;
                    location.Status = ParticipationStatus.Declined;
                    storedAppointment.Participant.Add(location);

                    break;
                case "patient":

                    ParticipantComponent patient = new ParticipantComponent();
                    ResourceReference patientReference = new ResourceReference();
                    patientReference.Reference = reference;
                    patient.Actor = patientReference;
                    patient.Status = ParticipationStatus.Declined;
                    storedAppointment.Participant.Add(patient);
                    break;
                case "practitioner":

                    ParticipantComponent practitioner = new ParticipantComponent();
                    ResourceReference practitionerReference = new ResourceReference();
                    practitionerReference.Reference = reference;
                    practitioner.Actor = practitionerReference;
                    practitioner.Status = ParticipationStatus.Declined;
                    storedAppointment.Participant.Add(practitioner);

                    break;
            }
        }

        [When(@"I cancel the appointment and set the cancel extension to have url ""(.*)"" and reason ""(.*)"" called ""(.*)""")]
        public void WhenICancelTheAppointmentAndSetTheCancelExtensionToHaveURLCodeAndDisplayCalled(string url, string reasonString, string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            storedAppointment.Status = AppointmentStatus.Cancelled;
            string fullUrl = "Appointment/" + storedAppointment.Id;
            Extension extension = new Extension();
            extension = (buildAppointmentCancelExtension(extension, url, reasonString));
            storedAppointment.ModifierExtension.Add(extension);
            HttpSteps.RestRequest(RestSharp.Method.PUT, fullUrl, FhirSerializer.SerializeToJson(storedAppointment));
        }

        [When(@"I cancel the appointment and set the cancel extension to have url ""(.*)"" and missing reason called ""(.*)""")]
        public void WhenICancelTheAppointmentAndSetTheCancelExtensionToHaveURLAndMissingReasonCalled(string url, string appointmentName)
        {
            WhenICancelTheAppointmentAndSetTheCancelExtensionToHaveURLCodeAndDisplayCalled(url, null, appointmentName);
        }

        [Then("the returned appointment resource status should be set to cancelled")]
        public void ThenTheReturnedAppointmentResourceStatusShouldBeCancelled()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            appointment.Status.ShouldBe(AppointmentStatus.Cancelled);
        }

        [Then(@"the cancellation reason in the returned appointment response should be equal to ""(.*)""")]
        public void ThenTheCancellationReasonInTheReturnedAppointmentResponseShouldBeEqualToStrings(string display)
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            foreach (var extension in appointment.Extension)
            {
                if (string.Equals(extension.Url, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1"))
                {
                    extension.Value.ShouldNotBeNull("There should be a value element within the appointment CancellationReason extension");
                    var extensionValueString = (FhirString)extension.Value;
                    extensionValueString.Value.ShouldBe(display);
                }
            }
        }


        [Then(@"the resource type of the appointment with key ""(.*)"" and the returned response should be equal")]
        public void ThenTheResponseTypeOfTheAppointmentWithKeyAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            storedAppointment.ResourceType.ShouldBe(returnedAppointment.ResourceType);
        }

        [Then(@"the id of the appointment with key ""(.*)"" and the returned response should be equal")]
        public void ThenTheIdOfTheAppointmentWithKeyAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            storedAppointment.Id.ShouldBe(returnedAppointment.Id);
        }

        [Then(@"the status of the appointment with key ""(.*)"" and the returned response should be equal")]
        public void ThenTheStatusOfTheAppointmentWithKeyAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            returnedAppointment.ShouldNotBeNull("The status is not allowed to be null");
            storedAppointment.Status.ShouldBe(returnedAppointment.Status);
        }

        [Then(@"the extensions of the appointment with key ""(.*)"" and the returned response should be equal")]
        public void ThenTheExtensionOfTheAppointmentWithKeyAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            FhirString extensionValueString = new FhirString();

            List<Extension> returnedExtensions = new List<Extension>();
            List<Extension> storedExtensions = new List<Extension>();

            
            foreach (var extension in returnedAppointment.ModifierExtension)
            {
                returnedExtensions.Add(extension);
            }

            foreach (var extension in storedAppointment.ModifierExtension)
            {
                storedExtensions.Add(extension);
            }

            returnedExtensions.Sort();
            storedExtensions.Sort();

            returnedExtensions.Count.ShouldBe(storedExtensions.Count, "Appointment extenstion count is not equal.");
           
            for (int i = 0; i < returnedExtensions.Count; i++)
            {
                storedExtensions[i].Equals(returnedExtensions[i]);
                storedExtensions[i].Url.ShouldBe(returnedExtensions[i].Url);
                storedExtensions[i].Value.ToString().ShouldBe(returnedExtensions[i].Value.ToString());
            }
        }

        [Then(@"the description of the appointment with key ""(.*)"" and the returned response should be equal")]
        public void ThenTheDescriptionOfTheAppointmentWithKeyAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            storedAppointment.Description.ShouldBe(returnedAppointment.Description);
        }
        
        [Then(@"the start and end date of the appointment with key ""(.*)"" and the returned response should be equal")]
        public void ThenTheStartAndEndDateOfTheAppointmentWithKeyAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            returnedAppointment.Start.ShouldNotBeNull("The start date is not allowed to be null");
            returnedAppointment.End.ShouldNotBeNull("The end date is not allowed to be null");
            storedAppointment.Start.ShouldBe(returnedAppointment.Start);
            storedAppointment.End.ShouldBe(returnedAppointment.End);
        }

        [Then(@"the reason of the appointment with key ""(.*)"" and the returned response should be equal")]
        public void ThenTheReasonOfTheAppointmentWithKeyAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            storedAppointment.Reason.Text.ShouldBe(returnedAppointment.Reason.Text);
          }

        [Then(@"the slot display and reference of the appointment with key ""(.*)"" and the returned response should be equal")]
        public void ThenTheSlotDisplayAndReferenceOfTheAppointmentWithKeyAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            List <ResourceReference> storedSlotReferences = new List<ResourceReference>();
            List<ResourceReference> returnedSlotReferences = new List<ResourceReference>();
            List<String> storedSlotDisplay = new List<String>();
            List<String> returnedSlotDisplay = new List<String>();

            foreach (var slotReference in storedAppointment.Slot)
            {
                storedSlotReferences.Add(slotReference);
                storedSlotDisplay.Add(slotReference.Display);
            }

            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            foreach (var slotReference in returnedAppointment.Slot)
            {
                returnedSlotReferences.Add(slotReference);
                returnedSlotDisplay.Add(slotReference.Display);
            }

            storedSlotReferences.ShouldNotBeEmpty("The stored appointment contains zero slots which is invalid");
            returnedSlotReferences.ShouldNotBeEmpty("The returned appointment resource contains zero slots which is invalid");
            storedSlotReferences.Sort();
            returnedSlotReferences.Sort();
            storedSlotDisplay.Sort();
            returnedSlotDisplay.Sort();

            storedSlotReferences.Count.ShouldBe(returnedSlotReferences.Count);

            for (int i = 0; i < returnedSlotReferences.Count; i++)
            {
                storedSlotReferences[i].Reference.Equals(returnedSlotReferences[i].Reference);
            }

            for (int i = 0; i < returnedSlotDisplay.Count; i++)
            {
                if (storedSlotDisplay[i] != null && returnedSlotDisplay[i] != null)
                {
                    storedSlotDisplay[i].Equals(returnedSlotDisplay[i]);
                }
            }
        }
        
        [Then(@"the participants of the appointment with key ""(.*)"" and the returned response should be equal")]
        public void ThenThePatientParticipantsOfTheAppointmentWithKeyAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment savedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            List<ParticipantComponent> savedAppointmentParticipants = new List<ParticipantComponent>();
            List<ParticipantComponent> returnedResponseAppointmentParticipants = new List<ParticipantComponent>();

            foreach (Appointment.ParticipantComponent participant in savedAppointment.Participant)
            {
              savedAppointmentParticipants.Add(participant);
            }

            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            returnedAppointment.Participant.ShouldNotBeNull();
            foreach (Appointment.ParticipantComponent participant in returnedAppointment.Participant)
            {
                returnedResponseAppointmentParticipants.Add(participant);
            }

            returnedResponseAppointmentParticipants.Count.ShouldBe(savedAppointmentParticipants.Count);

            for (int i = 0; i < returnedResponseAppointmentParticipants.Count; i++)
            {
                returnedResponseAppointmentParticipants[i].Actor.Url.ToString().ShouldBe(savedAppointmentParticipants[i].Actor.Url.ToString());
                returnedResponseAppointmentParticipants[i].Actor.Reference.ToString().ShouldBe(savedAppointmentParticipants[i].Actor.Reference.ToString());
                returnedResponseAppointmentParticipants[i].Status.ToString().ShouldBe(savedAppointmentParticipants[i].Status.ToString());
            }
        }

        [Then(@"the appointment participants of the appointment must conform to the gp connect specifications")]
        public void ThenTheAppointmentParticipantPractitionerReferenceMustBeAValidReference()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            foreach (Appointment.ParticipantComponent participant in appointment.Participant)
            {
                if (participant.Actor.Reference.StartsWith("Practitioner/"))
                {
                    var returnedResource = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner", participant.Actor.Reference);
                    returnedResource.ShouldNotBeNull("Practitioner reference returns a null practitioner");
                    returnedResource.GetType().ShouldBe(typeof(Practitioner));
                    verifyPractitioner(returnedResource);
                }
                
                if (participant.Actor.Reference.StartsWith("Patient/"))
                {
                    var returnedResource = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:patient", participant.Actor.Reference);
                    returnedResource.ShouldNotBeNull("Practitioner reference returns a null patient");
                    returnedResource.GetType().ShouldBe(typeof(Patient));
                    verifyPatient(returnedResource);
                }

                if (participant.Actor.Reference.StartsWith("Location/"))
                {
                    var returnedResource = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:location", participant.Actor.Reference);
                    returnedResource.ShouldNotBeNull("Practitioner reference returns a null location");
                    returnedResource.GetType().ShouldBe(typeof(Location));
                    verifyLocation(returnedResource);
                }
            }
        }

        private void verifyLocation(Resource location)
        {
            ThenIfTheLocationResourceContainsAnIdentifierThenItIsValid(location);
            ThenIfTheLocationResourceShouldContainANameANameElement(location);
            ThenIfTheLocationResourceShouldContainSystemCodeAndDisplayIfTheTypeCodingIsIncludedInTheResource(location);
            ThenIfTheLocationResourceContainsValidSystemCodeAndDisplayIfThePhysicalTypeCodingIsIncludedInTheResource(location);
            ThenIfTheLocationResourceContainsPartOfElementTheReferenceShouldReferenceAResourceInTheResponseBundle(location);
        }
        
        private void verifyPatient(Resource patient)
        {
            ThenThePatientResourceShouldContainMetaDataProfileAndVersionId(patient);
            ThenThePatientResourceMustContainIdentifierWithValidSystemAndValue(patient);
        }
        
        private void verifyPractitioner(Resource practitioner)
        {
            ThenThePractitionerResourceMustContainNameWithAValidSubsetOfElements(practitioner);
            ThenThePractitionerResourceContainsAnIdentifierThenItIsValid(practitioner);
            ThenIfThePractitionerResourceContainsAPractitionerRoleItIsValid(practitioner);
            ThenIfThePractitionerResourceHasCommunicationElementsContainingACodingThenThereMustBeASystemCodeAndDisplayElement(practitioner);
        }

        [Then(@"the comment of ""(.*)"" and the returned response should be equal")]
        public void ThenTheCommentOfAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            storedAppointment.Comment.ShouldBe(returnedAppointment.Comment);
        }
        
        [Then(@"the practitoner resource ""(.*)"" must contain name with a valid subset of elements")]
        public void ThenThePractitionerResourceMustContainNameWithAValidSubsetOfElements(Resource practitionerPassed)
        {
            Practitioner practitioner = (Practitioner)practitionerPassed;
            practitioner.ShouldNotBeNull("The stored practitioner resource is being returned as null, this is invalid");
            var familyNameCount = 0;
            practitioner.Name.ShouldNotBeNull("Practitioner Name is null, this is invalid as this is manadatory");
            foreach (string name in practitioner.Name.Family)
            {
                familyNameCount++;
            }

            familyNameCount.ShouldBeLessThanOrEqualTo(1);
        }

        [Then(@"if the practitioner resource ""(.*)"" contains an identifier then it is valid")]
        public void ThenThePractitionerResourceContainsAnIdentifierThenItIsValid(Resource practitionerName)
        {
            Practitioner practitioner = (Practitioner)practitionerName;
            practitioner.ShouldNotBeNull("The stored practitioner resource is being returned as null, this is invalid");
            foreach (Identifier identifier in practitioner.Identifier)
            {
                identifier.System.ShouldNotBeNullOrEmpty();
                var validSystems = new string[2] { "http://fhir.nhs.net/Id/sds-role-profile-id", "http://fhir.nhs.net/Id/sds-user-id" };
                identifier.System.ShouldBeOneOf(validSystems, "The identifier System can only be one of the valid value");
                identifier.Value.ShouldNotBeNullOrEmpty();
            }
        }

        [Then(@"if the practitioner resource ""(.*)"" contains a practitioner role it is valid")]
        public void ThenIfThePractitionerResourceContainsAPractitionerRoleItIsValid(Resource practitionerName)
        {
            Practitioner practitioner = (Practitioner)practitionerName;
            practitioner.ShouldNotBeNull("The stored practitioner resource is being returned as null, this is invalid");

            foreach (Practitioner.PractitionerRoleComponent practitionerRole in practitioner.PractitionerRole)
            {
                practitionerRole.Specialty.ShouldBeEmpty("Practitioner speciality should be empty but is not");
                practitionerRole.Period.ShouldBeNull("Practitioner period should be null but is not");
                practitionerRole.Location.ShouldBeEmpty("Practitioner location should be empty but is not");
                practitionerRole.HealthcareService.ShouldBeEmpty("Practitioner health care service should be empty but is not");

                practitionerRole.ManagingOrganization.Reference.ShouldNotBeNullOrEmpty();
                var returnedResource = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:organization", practitionerRole.ManagingOrganization.Reference);
                returnedResource.ShouldNotBeNull("Practitioner reference returns a null patient");
                returnedResource.GetType().ShouldBe(typeof(Organization));

                if (practitionerRole.Role != null && practitionerRole.Role.Coding != null)
                {
                    var codingCount = 0;
                    foreach (Coding coding in practitionerRole.Role.Coding)
                    {
                        codingCount++;
                        coding.System.ShouldBe("http://fhir.nhs.net/ValueSet/sds-job-role-name-1");
                        coding.Code.ShouldNotBeNull("Coding code should not be null");
                        coding.Display.ShouldNotBeNull("Coding display should not be null");
                        coding.Display.GetType().ShouldBe(typeof(string));
                    }
                    codingCount.ShouldBeLessThanOrEqualTo(1);
                }
            }
        }
        
        [Then(@"if the practitioner resource ""(.*)"" has communicaiton elemenets containing a coding then there must be a system, code and display element")]
        public void ThenIfThePractitionerResourceHasCommunicationElementsContainingACodingThenThereMustBeASystemCodeAndDisplayElement(Resource practitionerName)
        {
            Practitioner practitioner = (Practitioner)practitionerName;
            practitioner.ShouldNotBeNull("The stored practitioner resource is being returned as null, this is invalid");
            //If the practitioner has a communicaiton elemenets containing a coding then there must be a system, code and display element. There must only be one coding per communication element.
            foreach (CodeableConcept codeableConcept in practitioner.Communication)
            {
                AccessRecordSteps.ShouldBeSingleCodingWhichIsInValueSet(GlobalContext.FhirHumanLanguageValueSet, codeableConcept.Coding);
            }
        }

        [Then(@"the patient resource ""(.*)"" must contain identifier with valid system and value")]
        public void ThenThePatientResourceMustContainIdentifierWithValidSystemAndValue(Resource patientName)
        {
            Patient patient = (Patient)patientName;
            patient.Identifier.ShouldNotBeNull();
            foreach (Identifier identifier in patient.Identifier)
            {
                identifier.System.ShouldNotBeNull();
                identifier.System.ShouldBe("http://fhir.nhs.net/Id/nhs-number");
                identifier.Value.ShouldNotBeNull();
            }
        }

        [Then(@"the patient resource ""(.*)"" should contain meta data profile and version id")]
        public void ThenThePatientResourceShouldContainMetaDataProfileAndVersionId(Resource patientName)
        {
            var resource = (Patient)patientName;
            resource.Meta.ShouldNotBeNull();
            int metaProfileCount = 0;
            foreach (string profile in resource.Meta.Profile)
            {
                metaProfileCount++;
                profile.ShouldBe("http://fhir.nhs.net/StructureDefinition/gpconnect-patient-1");
            }
            metaProfileCount.ShouldBe(1);
            resource.Meta.VersionId.ShouldNotBeNull();
        }


        [Then(@"if the location resource ""(.*)"" should contain a name element")]
        public void ThenIfTheLocationResourceShouldContainANameANameElement(Resource locationName)
        {
            Location location = (Location)locationName;
            location.Name.ShouldNotBeNullOrEmpty();
        }

        [Then(@"if the location resource ""(.*)"" should contain a maximum of one ODS Site Code and one other identifier")]
        public void ThenIfTheLocationResourceContainsAnIdentifierThenItIsValid(Resource locationName)
        {
            Location resource = (Location)locationName;
            int odsSiteCodeIdentifierCount = 0;
            foreach (var identifier in resource.Identifier)
            {
                if (string.Equals(identifier.System, "http://fhir.nhs.net/Id/ods-site-code"))
                {
                    odsSiteCodeIdentifierCount++;
                }
            }
            odsSiteCodeIdentifierCount.ShouldBeLessThanOrEqualTo(1, "There should be a maximum of one ODS Site Code within the Location resource.");
            resource.Identifier.Count.ShouldBeLessThanOrEqualTo(2, "There should be no more than one ODS Site Code and One other identifier, there is more than 2 identifiers in the Location resource.");
        }

        [Then(@"if the location resource ""(.*)"" should contain system code and display if the Type coding is included in the resource")]
        public void ThenIfTheLocationResourceShouldContainSystemCodeAndDisplayIfTheTypeCodingIsIncludedInTheResource(Resource locationName)
        {
            Location location = (Location)locationName;
            if (location.Type != null && location.Type.Coding != null)
            {
                location.Type.Coding.Count.ShouldBeLessThanOrEqualTo(1);
                foreach (var coding in location.Type.Coding)
                {
                    // Need to pull in valueset from URL and validate against that
                    coding.System.ShouldBe("http://hl7.org/fhir/ValueSet/v3-ServiceDeliveryLocationRoleType");
                    coding.Code.ShouldNotBeNullOrEmpty();
                    coding.Display.ShouldNotBeNullOrEmpty();
                }
            }
        }

        [Then(@"if the location resource ""(.*)"" contains valid system code and display if the PhysicalType coding is included in the resource")]
        public void ThenIfTheLocationResourceContainsValidSystemCodeAndDisplayIfThePhysicalTypeCodingIsIncludedInTheResource(Resource locationName)
        {
            Location location = (Location)locationName;
            if (location.PhysicalType != null && location.PhysicalType.Coding != null)
            {
                location.PhysicalType.Coding.Count.ShouldBeLessThanOrEqualTo(1);
                foreach (var coding in location.PhysicalType.Coding)
                {
                    var validSystems = new String[] { "http://snomed.info/sct", "http://read.info/readv2", "http://read.info/ctv3" };
                    coding.System.ShouldBeOneOf(validSystems);
                    coding.Code.ShouldNotBeNullOrEmpty();
                    coding.Display.ShouldNotBeNullOrEmpty();
                }
            }
        }

        [Then(@"if the location resource ""(.*)"" contains partOf element the reference should reference a resource in the response bundle")]
        public void ThenIfTheLocationResourceContainsPartOfElementTheReferenceShouldReferenceAResourceInTheResponseBundle(Resource locationName)
        {
            Location location = (Location)locationName;
            if (location.PartOf != null)
            {
                location.PartOf.Reference.ShouldNotBeNullOrEmpty();
                AccessRecordSteps.ResponseBundleContainsReferenceOfType(location.PartOf.Reference, ResourceType.Location);
            }
        }

        [Then(@"if the location resource ""(.*)"" contains managingOrganization element the reference should reference a resource in the response bundle")]
        public void ThenIfTheLocationResourceContainsManagingOrganizationElementTheReferenceShouldReferenceAResourceInTheResponseBundle(Resource locationName)
        {
            Location location = (Location)locationName;
            if (location.ManagingOrganization != null)
            {
                location.ManagingOrganization.Reference.ShouldNotBeNullOrEmpty();
                AccessRecordSteps.ResponseBundleContainsReferenceOfType(location.ManagingOrganization.Reference, ResourceType.Organization);
            }
        }

        [Then(@"I make a GET request for the appointment with key ""(.*)"" to ensure the status has not been changed to cancelled")]
        public void ThenIMakeAGetRequestForTheAppointmentWithKeyToEnsureTheStatusHasNotBeenChangedToCancelled(string appointmentName)
        {
            Given("I am using the default server");
            And(@"I am performing the ""urn:nhs:names:services:gpconnect:fhir:rest:read:appointment"" interaction");
            When($@"I perform an appointment read for the appointment called ""{appointmentName}""");
            Then("the response status code should indicate success");
            And("the response body should be FHIR JSON");
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            appointment.Status.ShouldNotBe(AppointmentStatus.Cancelled);
        }

        [Then(@"I make a GET request for the appointment with key ""(.*)"" to ensure the status has been changed to cancelled")]
        public void ThenIMakeAGetRequestForTheAppointmentWithKeyToEnsureTheStatusHasBeenChangedToCancelled(string appointmentName)
        {
            Given("I am using the default server");
            And(@"I am performing the ""urn:nhs:names:services:gpconnect:fhir:rest:read:appointment"" interaction");
            When($@"I perform an appointment read for the appointment called ""{appointmentName}""");
            Then("the response status code should indicate success");
            And("the response body should be FHIR JSON");
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            appointment.Status.ShouldBe(AppointmentStatus.Cancelled);
        }

        [Then(@"the response version id should be different to the version id stored in ""(.*)""")]
        public void ThenTheResponseVersionIdShouldBeDifferentToTheVersionIdStoredIn(string storedAppointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[storedAppointmentName];
            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            storedAppointment.VersionId.ToString().ShouldNotBe(returnedAppointment.VersionId.ToString());
        }
        
        private Extension buildAppointmentCancelExtension(Extension extension, string url, string reasonString)
        {
            extension.Url = url;
            FhirString reason = new FhirString(reasonString);
            extension.Value = reason;
            return extension;
        }

        private Extension buildAppointmentOtherExtension(Extension extension, string url, string codes, string display)
        {
            extension.Url = url;
            CodeableConcept reason = new CodeableConcept();
            Coding code = new Coding();
            code.Code = codes;
            code.Display = display;
            reason.Coding.Add(code);
            extension.Value = reason;
            return extension;
        }
    }
}
