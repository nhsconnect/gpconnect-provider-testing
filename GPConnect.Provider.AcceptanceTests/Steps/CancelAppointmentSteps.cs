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
        public void WhenISetTheURLToAndCancel(string URL, string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            storedAppointment.Status = AppointmentStatus.Cancelled;
            Extension extension = new Extension();
            List<Extension> extensionList = new List<Extension>();
            extension = (buildAppointmentCancelExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1-0", "Double booked", "Double booked"));
            storedAppointment.ModifierExtension.Add(extension);
            HttpSteps.RestRequest(RestSharp.Method.PUT, URL, FhirSerializer.SerializeToJson(storedAppointment));
        }



        [When(@"I cancel the appointment with the key ""(.*)""")]
        public void WhenICancelTheAppointmentWithTheKey(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            storedAppointment.Status = AppointmentStatus.Cancelled;
            string url = "Appointment/" + storedAppointment.Id;
            Extension extension = new Extension();
            List<Extension> extensionList = new List<Extension>();
            extension = (buildAppointmentCancelExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1-0", "Double booked", "Double booked"));
            storedAppointment.ModifierExtension.Add(extension);
            HttpSteps.RestRequest(RestSharp.Method.PUT, url, FhirSerializer.SerializeToJson(storedAppointment));
        }

        [When(@"I cancel the appointment with the key ""(.*)"" and change the ""(.*)"" element")]
        public void WhenICancelTheAppointmentWithTheKeyAndChangeTheElement(string appointmentName, string element)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            storedAppointment.Status = AppointmentStatus.Cancelled;
            string url = "Appointment/" + storedAppointment.Id;

            Extension extension = new Extension();
            List<Extension> extensionList = new List<Extension>();
            extension = (buildAppointmentCancelExtension(extension, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1-0", "Double booked", "Double booked"));
            storedAppointment.ModifierExtension.Add(extension);
            switch (element)
            {
                case "description":
                    storedAppointment.Description = "RANDOM DESCRIPTION";
                    break;
                case "priority":
                    storedAppointment.Priority = 30;
                    break;
                case "minutesDuration":
                    storedAppointment.MinutesDuration = 30;
                    break;
                case "comment":
                    storedAppointment.Comment = "RANDOM COMMENT";
                    break;
                case "typeText":
                    storedAppointment.Type.Text = "RANDOM TEXT";
                    break;
                case "identifier":
                    Identifier identifier = new Identifier();
                    identifier.System = "RANDOM SYSTEM";
                    identifier.Value = "RANDOM VALUE";
                    storedAppointment.Identifier.Add(identifier);
                    break;
                case "reason":
                    var reason = storedAppointment.Reason;
                    Coding coding = new Coding();
                    coding.System = "http://snomed.info/sct";
                    coding.Display = "RANDOM DISPLAY";
                    coding.Code = "RANDOM CODE";
                    reason.Coding.Add(coding);
                    storedAppointment.Reason = reason;
                    break;
                case "participant":
                    ParticipantComponent location = new ParticipantComponent();
                    ResourceReference locationReference = new ResourceReference();
                    locationReference.Reference = "RANDOM/REFERENCE";
                    location.Actor = locationReference;
                    location.Status = ParticipationStatus.Declined;
                    storedAppointment.Participant.Add(location);
                    break;

            }
                
            HttpSteps.RestRequest(RestSharp.Method.PUT, url, FhirSerializer.SerializeToJson(storedAppointment));
        }



        [When(@"I cancel the appointment and set the cancel extension to have url ""(.*)"" code ""(.*)"" and display ""(.*)"" called ""(.*)""")]
        public void WhenICancelTheAppointmentAndSetTheCancelExtensionToHaveURLCodeAndDisplayCalled(string url, string code, string display,string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            storedAppointment.Status = AppointmentStatus.Cancelled;
            string fullUrl = "Appointment/" + storedAppointment.Id;
            Extension extension = new Extension();
            List<Extension> extensionList = new List<Extension>();
            extension = (buildAppointmentCancelExtension(extension, url, code, display));
            storedAppointment.ModifierExtension.Add(extension);
            HttpSteps.RestRequest(RestSharp.Method.PUT, fullUrl, FhirSerializer.SerializeToJson(storedAppointment));
        }


        [Then("the returned appointment resource status should be set to cancelled")]
        public void ThenTheReturnedAppointmentResourceStatusShouldBeCancelled()
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            //Only manadatory field on location specification
            appointment.Status.ShouldBe(AppointmentStatus.Cancelled);
        }

        [Then(@"the cancellation reason in the returned appointment response should be equal to ""(.*)""")]
        public void ThenTheCancellationReasonInTheReturnedAppointmentResponseShouldBeEqualToStrings(string display)
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            foreach (var extension in appointment.Extension)
            {
                if (string.Equals(extension.Url, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1"))
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

        [Then(@"the extension of the appointment with key ""(.*)"" and the returned response should be equal")]
        public void ThenTheExtensionOfTheAppointmentWithKeyAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            FhirString extensionValueString = new FhirString();
            foreach (var extension in returnedAppointment.Extension)
            {
                if (string.Equals(extension.Url, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1"))
                {
                    extension.Value.ShouldNotBeNull("There should be a value element within the appointment CancellationReason extension");
                    extensionValueString = (FhirString)extension.Value;
                 }
            }

            foreach (var extension in storedAppointment.Extension)
            {
                if (string.Equals(extension.Url, "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1"))
                {
                    extension.Value.ShouldNotBeNull("There should be a value element within the appointment CancellationReason extension");
                    var value = (FhirString)extension.Value;
                    value.ShouldBe(extensionValueString);
                }
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

            foreach (var slotReference in storedAppointment.Slot)
            {
                storedSlotReferences.Add(slotReference);
            }

            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            foreach (var slotReference in returnedAppointment.Slot)
            {
                returnedSlotReferences.Add(slotReference);
            }

            storedSlotReferences.ShouldNotBeNull("The stored appointment contains zero slots which is invalid");
            returnedSlotReferences.ShouldNotBeNull("The returned appointment resource contains zero slots which is invalid");
            storedSlotReferences.Sort();
            returnedSlotReferences.Sort();

            for (int i = 0; i < returnedSlotReferences.Count; i++)
            {
                storedSlotReferences[i].Equals(returnedSlotReferences[i]);
            }
        }


        [Then(@"the ""(.*)"" participant of the appointment with key ""(.*)"" and the returned response should be equal")]
        public void ThenThePatientParticipantsOfTheAppointmentWithKeyAndTheReturnedResponseShouldBeEqual(string participantValue, string appointmentName)
        {
            Appointment savedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            List<ParticipationStatus> savedAppointmentParticipantStatus = new List<ParticipationStatus>();
            List<ParticipationStatus> returnedResponseAppointmentParticipantStatus = new List<ParticipationStatus>();

            foreach (Appointment.ParticipantComponent participant in savedAppointment.Participant)
            {
                if (participant.Actor.Reference.StartsWith(participantValue + "/"))
                {
                    savedAppointmentParticipantStatus.Add(participant.Status.Value);
                }

            }

            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            returnedAppointment.Participant.ShouldNotBeNull();
            foreach (Appointment.ParticipantComponent participant in returnedAppointment.Participant)
            {
                if (participant.Actor.Reference.StartsWith(participantValue + "/"))
                {
                    returnedResponseAppointmentParticipantStatus.Add(participant.Status.Value);
                }

            }
            returnedResponseAppointmentParticipantStatus.Sort();
            savedAppointmentParticipantStatus.Sort();
            for (int i = 0; i < returnedResponseAppointmentParticipantStatus.Count; i++)
            {
                returnedResponseAppointmentParticipantStatus[i].Equals(savedAppointmentParticipantStatus[i]);
            }
        }
    
        

        [Then(@"the appointment participant ""(.*)"" reference must be a valid reference and is saved as ""(.*)""")]
        public void ThenTheAppointmentParticipantPractitionerReferenceMustBeAValidReference(string participantSent, string practitionerSaved)
        {
            Appointment appointment = (Appointment)FhirContext.FhirResponseResource;
            foreach (Appointment.ParticipantComponent participant in appointment.Participant)
            {
                if (participant.Actor.Reference.StartsWith("Practitioner/") && "Practitioner" == participantSent)
                {
                    var returnedResource = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner", participant.Actor.Reference);
                    returnedResource.ShouldNotBeNull("Practitioner reference returns a null practitioner");
                    returnedResource.GetType().ShouldBe(typeof(Practitioner));
                    HttpContext.StoredFhirResources.Add(practitionerSaved, returnedResource);
                }


                if (participant.Actor.Reference.StartsWith("Patient/") && "Patient" == participantSent)
                {
                    var returnedResource = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:patient", participant.Actor.Reference);
                    returnedResource.ShouldNotBeNull("Practitioner reference returns a null patient");
                    returnedResource.GetType().ShouldBe(typeof(Patient));
                    HttpContext.StoredFhirResources.Add(practitionerSaved, returnedResource);
                }

                if (participant.Actor.Reference.StartsWith("Location/") && "Location" == participantSent)
                {
                    var returnedResource = HttpSteps.getReturnedResourceForRelativeURL("urn:nhs:names:services:gpconnect:fhir:rest:read:location", participant.Actor.Reference);
                    returnedResource.ShouldNotBeNull("Practitioner reference returns a null location");
                    returnedResource.GetType().ShouldBe(typeof(Location));
                    HttpContext.StoredFhirResources.Add(practitionerSaved, returnedResource);
                }

            }    
        }

        [Then(@"the comment of ""(.*)"" and the returned response should be equal")]
        public void ThenTheCommentOfAndTheReturnedResponseShouldBeEqual(string appointmentName)
        {
            Appointment storedAppointment = (Appointment)HttpContext.StoredFhirResources[appointmentName];
            Appointment returnedAppointment = (Appointment)FhirContext.FhirResponseResource;
            storedAppointment.Comment.ShouldBe(returnedAppointment.Comment);
        }


        [Then(@"the practitoner resource saved with name ""(.*)"" must contain name with a valid subset of elements")]
        public void ThenThePractitionerResourceSavedWithTheNameMustContainNameWithAValidSubsetOfElements(string practitionerName)
        {
            Practitioner practitioner = (Practitioner)HttpContext.StoredFhirResources[practitionerName];
            practitioner.ShouldNotBeNull("The stored practitioner resource is being returned as null, this is invalid");
            var familyNameCount = 0;
            practitioner.Name.ShouldNotBeNull("Practitioner Name is null, this is invalid as this is manadatory");
            foreach (string name in practitioner.Name.Family)
            {
                familyNameCount++;
            }

            familyNameCount.ShouldBeLessThanOrEqualTo(1);
        }

        [Then(@"if the practitioner resource saved with name ""(.*)"" contains an identifier then it is valid")]
        public void ThenThePractitionerResourceSavedWithTheNameContainsAnIdentifierThenItIsValid(string practitionerName)
        {
            Practitioner practitioner = (Practitioner)HttpContext.StoredFhirResources[practitionerName];
            practitioner.ShouldNotBeNull("The stored practitioner resource is being returned as null, this is invalid");
            foreach (Identifier identifier in practitioner.Identifier)
            {
                identifier.System.ShouldNotBeNullOrEmpty();
                var validSystems = new string[2] { "http://fhir.nhs.net/Id/sds-role-profile-id", "http://fhir.nhs.net/Id/sds-user-id" };
                identifier.System.ShouldBeOneOf(validSystems, "The identifier System can only be one of the valid value");
                identifier.Value.ShouldNotBeNullOrEmpty();
            }
        }

        [Then(@"if the practitioner resource saved with name ""(.*)"" contains a practitioner role it is valid")]
        public void ThenIfThePractitionerResourceSavedWithTheNameContainsAPractitionerRoleItIsValid(string practitionerName)
        {
            Practitioner practitioner = (Practitioner)HttpContext.StoredFhirResources[practitionerName];
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
        
        [Then(@"if the practitioner resource saved with name ""(.*)"" has communicaiton elemenets containing a coding then there must be a system, code and display element")]
        public void ThenIfThePractitionerResourceSavedWithNameHasCommunicationElementsContainingACodingThenThereMustBeASystemCodeAndDisplayElement(string practitionerName)
        {
            Practitioner practitioner = (Practitioner)HttpContext.StoredFhirResources[practitionerName];
            practitioner.ShouldNotBeNull("The stored practitioner resource is being returned as null, this is invalid");
            //If the practitioner has a communicaiton elemenets containing a coding then there must be a system, code and display element. There must only be one coding per communication element.
            foreach (CodeableConcept codeableConcept in practitioner.Communication)
            {
                AccessRecordSteps.shouldBeSingleCodingWhichIsInValuest(GlobalContext.FhirHumanLanguageValueSet, codeableConcept.Coding);
            }
        
         }

        [Then(@"the patient resource saved with name ""(.*)"" must contain identifier with valid system and value")]
        public void ThenThePatientResourceSavedWithNameMustContainIdentifierWithValidSystemAndValue(string patientName)
        {
            Patient patient = (Patient)HttpContext.StoredFhirResources[patientName];
            patient.Identifier.ShouldNotBeNull();
            foreach (Identifier identifier in patient.Identifier)
            {
                identifier.System.ShouldNotBeNull();
                identifier.System.ShouldBe("http://fhir.nhs.net/Id/nhs-number");
                identifier.Value.ShouldNotBeNull();
            }
        }

        [Then(@"the patient resource saved with name ""(.*)"" should contain meta data profile and version id")]
        public void ThenThePatientResourceSaveWithNameShouldContainMetaDataProfileAndVersionId(string patientName)
        {
            var resource = (Patient)HttpContext.StoredFhirResources[patientName];
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


        [Then(@"if the location resource saved with the name ""(.*)"" should contain a name element")]
        public void ThenIfTheLocationResourceSavedWithTheNameShouldContainANameANameElement(string locationName)
        {
            Location location = (Location)HttpContext.StoredFhirResources[locationName];
            location.Name.ShouldNotBeNullOrEmpty();
        }

        [Then(@"if the location resource saved with the name ""(.*)"" should contain a maximum of one ODS Site Code and one other identifier")]
        public void ThenIfTheLocationResourceSavedWithTheNameContainsAnIdentifierThenItIsValid(string locationName)
        {
            Location resource = (Location)HttpContext.StoredFhirResources[locationName];
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

        [Then(@"if the location resource saved with the name ""(.*)"" should contain system code and display if the Type coding is included in the resource")]
        public void ThenIfTheLocationResourceSavedWithTheNameShouldContainSystemCodeAndDisplayIfTheTypeCodingIsIncludedInTheResource(string locationName)
        {
            Location location = (Location)HttpContext.StoredFhirResources[locationName];
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

        [Then(@"if the location resource saved with the name ""(.*)"" should contain valid system code and display if the PhysicalType coding is included in the resource")]
        public void ThenIfTheLocationResourceSavedWithTheNameShouldContainValidSystemCodeAndDisplayIfThePhysicalTypeCodingIsIncludedInTheResource(string locationName)
        {
            Location location = (Location)HttpContext.StoredFhirResources[locationName];
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

        [Then(@"if the location resource saved with the name ""(.*)"" contains partOf element the reference should reference a resource in the response bundle")]
        public void ThenIfTheLocationResourceSavedWithTheNameContainsPartOfElementTheReferenceShouldReferenceAResourceInTheResponseBundle(string locationName)
        {
            Location location = (Location)HttpContext.StoredFhirResources[locationName];
            if (location.PartOf != null)
            {
                location.PartOf.Reference.ShouldNotBeNullOrEmpty();
                AccessRecordSteps.responseBundleContainsReferenceOfType(location.PartOf.Reference, ResourceType.Location);
            }
        }

        [Then(@"if the location resource saved with the name ""(.*)"" contains managingOrganization element the reference should reference a resource in the response bundle")]
        public void ThenIfTheLocationResourceSavedWithTheNameContainsManagingOrganizationElementTheReferenceShouldReferenceAResourceInTheResponseBundle(string locationName)
        {
            Location location = (Location)HttpContext.StoredFhirResources[locationName];
            if (location.ManagingOrganization != null)
            {
                location.ManagingOrganization.Reference.ShouldNotBeNullOrEmpty();
                AccessRecordSteps.responseBundleContainsReferenceOfType(location.ManagingOrganization.Reference, ResourceType.Organization);
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
        
        private Extension buildAppointmentCancelExtension(Extension extension, string url, string code, string display)
        {
            extension.Url = url;
            CodeableConcept value = new CodeableConcept();
            Coding coding = new Coding();
            coding.Display = display;
            extension.Value = coding.DisplayElement;
            return extension;
        }


    }
}
