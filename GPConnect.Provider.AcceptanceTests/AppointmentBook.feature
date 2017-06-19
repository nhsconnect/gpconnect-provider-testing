@appointment
Feature: BookAppointment

Scenario: Book single appointment for patient
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
	When I book the appointment called "Appointment"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource

Scenario: Book Appointment with invalid url for booking appointment
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
	When I book the appointment called "Appointment" against the URL "/Appointments"
	Then the response status code should be "404"

Scenario Outline: Book appointment failure due to missing header
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
		And I do not send header "<Header>"
	When I book the appointment called "Appointment"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Header            |
		| Ssp-TraceID       |
		| Ssp-From          |
		| Ssp-To            |
		| Ssp-InteractionId |
		| Authorization     |

Scenario Outline: Book appointment accept header and _format parameter
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I book the appointment called "Appointment"
	Then the response status code should indicate created
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the returned resource shall contains a logical id
		And the appointment resource should contain a status element
		And the appointment response resource contains a slot reference
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/json+fhir | application/json+fhir | JSON       |
		| application/json+fhir | application/xml+fhir  | XML        |
		| application/xml+fhir  | application/json+fhir | JSON       |
		| application/xml+fhir  | application/xml+fhir  | XML        |

Scenario Outline: Book appointment _format parameter only but varying request content types
	Given I perform a patient search for patient "patient2" and store the first returned resources against key "storedPatient"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I set the request content type to "<ContentType>"
		And I create an appointment for patient "storedPatient" called "Appointment" from schedule "getScheduleResponseBundle"
		And I add the parameter "_format" with the value "<Parameter>"
	When I book the appointment called "Appointment"
	Then the response status code should indicate created
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the returned resource shall contains a logical id
		And the appointment resource should contain a status element
		And the appointment response resource contains a slot reference
	Examples:
		| ContentType           | Parameter             | BodyFormat |
		| application/json+fhir | application/json+fhir | JSON       |
		| application/json+fhir | application/xml+fhir  | XML        |
		| application/xml+fhir  | application/json+fhir | JSON       |
		| application/xml+fhir  | application/xml+fhir  | XML        |

Scenario Outline: Book appointment accept header variations
	Given I perform a patient search for patient "patient3" and store the first returned resources against key "storedPatient"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient" called "Appointment" from schedule "getScheduleResponseBundle"
		And I set the Accept header to "<Header>"
	When I book the appointment called "Appointment"
	Then the response status code should indicate created
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the returned resource shall contains a logical id
		And the appointment resource should contain a status element
		And the appointment response resource contains a slot reference
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario: Book appointment prefer header set to representation
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
		And I set the Prefer header to "return=representation"
	When I book the appointment called "Appointment"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the content-type should not be equal to null
		And the content-length should not be equal to zero

Scenario: Book appointment prefer header set to minimal
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
		And I set the Prefer header to "return=minimal"
	When I book the appointment called "Appointment"
	Then the response status code should indicate created
		And the response body should be empty
		And the content-type should be equal to null

Scenario Outline: Book appointment with invalid interaction id
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "<interactionId>" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
	When I book the appointment called "Appointment"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| interactionId                                                     |
		| urn:nhs:names:services:gpconnect:fhir:rest:search:organization    |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		|                                                                   |
		| null                                                              |

Scenario: Book Appointment and check response returns the correct values
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
	When I book the appointment called "Appointment"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned resource shall contains a logical id
		And the appointment resource should contain a status element
		And the appointment resource should contain a single start element
		And the appointment resource should contain a single end element
		And the appointment resource should contain at least one participant
		And the appointment resource should contain at least one slot reference
		And if the appointment resource contains a priority the value is valid

Scenario: Book Appointment and check returned appointment resource contains meta data
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
	When I book the appointment called "Appointment"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource should contain meta data profile and version id

Scenario: Book Appointment and appointment participant is valid
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
	When I book the appointment called "Appointment"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the appointment response resource contains atleast 2 participants a practitioner and a patient
		And the returned appointment participants must contain a type or actor element

Scenario Outline: Book Appointment and check extensions are valid
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "<AppointmentName>" from schedule "getScheduleResponseBundle"
	When I book the appointment called "<AppointmentName>"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And if the returned appointment category element is present it is populated with the correct values
		And if the returned appointment booking element is present it is populated with the correct values
		And if the returned appointment contact element is present it is populated with the correct values
		And if the returned appointment cancellation reason element is present it is populated with the correct values
	Examples:
		| AppointmentName |
		| Appointment     |
		| Appointment1    |
		| Appointment2    |
		| Appointment3    |
		| Appointment4    |
		| Appointment5    |
		| Appointment6    |
		| Appointment7    |

Scenario: Book Appointment without location participant
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
		And I remove the participant from the appointment called "Appointment" which starts with reference "Location"
	When I book the appointment called "Appointment"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource

Scenario Outline: Book Appointment and remove manadatory resources from the appointment booking
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
		And I remove the participant from the appointment called "Appointment" which starts with reference "<ParticipantToRemove>"
	When I book the appointment called "Appointment"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| ParticipantToRemove |
		| Patient             |
		| Practitioner        |

Scenario: Book Appointment and remove all participants
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
		And I remove the participant from the appointment called "Appointment" which starts with reference "Patient"
		And I remove the participant from the appointment called "Appointment" which starts with reference "Practitioner"
		And I remove the participant from the appointment called "Appointment" which starts with reference "Location"
	When I book the appointment called "Appointment"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Book appointment containing additional extension with only value populated
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
		And I add an extra invalid extension to the appointment called "Appointment" only populating the value
	When I book the appointment called "Appointment"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Book appointment containing additional extensions with only the system populated
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
		And I add an extra invalid extension to the appointment called "Appointment" only populating the url
	When I book the appointment called "Appointment"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Book single appointment for patient and send additional extensions with url and value populated
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment2" from schedule "getScheduleResponseBundle"
		And I add an extra invalid extension to the appointment called "Appointment2" containing the url code and display
	When I book the appointment called "Appointment2"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Book appointment for patient with id
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
		And I change the appointment id to "1111222233334444" in the appointment stored against key "Appointment"
	When I book the appointment called "Appointment"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Book appointment for patient and send extra fields in the resource
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
	When I book the appointment called "Appointment" with an invalid field
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Book appointment with invalid slot reference
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
		And I change the appointment slot reference to "<slotReference>" in the appointment stored against key "Appointment"
	When I book the appointment called "Appointment"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"	
	Examples:
		| slotReference    |
		| Slot/44445555555 |
		| Slot/45555g55555 |
		| Slot/45555555##  |
		| Slot/hello       |

Scenario: Book single appointment for patient and check the location reference is valid
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
	When I book the appointment called "Appointment"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And any location participant references included in returned appointment should be valid
	
Scenario: Book appointment with missing start element in appointment resource
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
		And I remove the appointment start element in appointment stored against key "Appointment"
	When I book the appointment called "Appointment"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Book appointment with missing end element in appointment resource
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
		And I remove the appointment end element in appointment stored against key "Appointment"
	When I book the appointment called "Appointment"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Book appointment with missing status element in appointment resource
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
		And I remove the appointment status element in appointment stored against key "Appointment"
	When I book the appointment called "Appointment"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

# Here
Scenario: Book appointment with missing slot element in appointment resource
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "storedPatient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
		And I remove the appointment slot element in appointment stored against key "Appointment"
	When I book the appointment called "Appointment"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Book Appointment and remove identifier value from the appointment booking
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
	Then I set the appointment identifier value element to null for "Appointment"
	When I book the appointment called "Appointment"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Book Appointment and remove reason coding element from the appointment booking
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
	Then I set the appointment reason coding <CodingElement> element to null for "Appointment"
	When I book the appointment called "Appointment"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| CodingElement |
		| system        |
		| code          |
		| display       |

Scenario Outline: Book Appointment and remove participant status from the appointment booking
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
	Then I set the appointment <Participant> participant status element to null for "Appointment"
	When I book the appointment called "Appointment"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Participant  |
		| Patient      |
		| Practitioner |
		| Location     |

Scenario Outline: Book Appointment and remove participant type coding element from the appointment booking
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I create an appointment for patient "storedPatient1" called "Appointment" from schedule "getScheduleResponseBundle"
	Then I set the appointment <Participant> participant type coding <CodingElement> element to null for "Appointment"
	When I book the appointment called "Appointment"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Participant  | CodingElement |
		| Patient      | system        |
		| Patient      | code          |
		| Patient      | display       |
		| Practitioner | system        |
		| Practitioner | code          |
		| Practitioner | display       |

Scenario: Book appointment and send patient resource in the request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create an appointment for patient "patient1" called "Appointment" using a patient resource
	When I book the appointment called "Appointment"
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Book appointment and send bundle resource in the request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create a new bundle to contain an appointment for patient "patient1" called "Appointment"
	When I book the appointment called "Appointment"
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

@ignore
Scenario: Book appointment for temporary patient

@ignore
@Manual
Scenario: Multi slot booking
	# Multiple adjacent slots success
	# Non adjacent slot failure
	# Slots from different schedules that are adjacent failure
	# Slots from different schedules which are not adjacent failure

@ignore
@Manual
Scenario: Extension supported
	# Is the data represented by the extensions such as booking method supported by the provider system? If so are the details saved when sent in and returned when resource is returned.