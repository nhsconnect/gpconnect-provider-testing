@appointment
Feature: AppointmentAmend

Background:
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "patient1"

Scenario: I perform a successful amend appointment and change the comment to a custom message
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I amend "CustomAppointment1" by changing the comment to "customComment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the appointment resource should contain a comment which equals "customComment"

Scenario: I perform a successful amend appointment and change the reason text to a custom message
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I amend "CustomAppointment1" by changing the reason text to "customComment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the appointment resource should contain a reason text which equals "customComment"

Scenario: I perform a successful amend appointment and change the description text to a custom message
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I amend "CustomAppointment1" by changing the description text to "customComment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the appointment resource should contain description text which equals "customComment"

Scenario: Amend appointment and update element which cannot be updated
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I amend "CustomAppointment1" by changing the priority to "1"
	Then the response status code should indicate authentication failure
		And the response should be a OperationOutcome resource

Scenario Outline: Amend appointment sending invalid URL
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I set the URL to "<url>" and amend "CustomAppointment1" by changing the comment to "hello"
	Then the response status code should indicate failure
	Examples:
		| url                 |
		| /Appointment/!      |
		| /APPointment/23     |
		| /Appointment/#      |
		| /Appointment/update |

Scenario Outline: Amend appointment failure due to missing header
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I do not send header "<Header>"
	When I amend "CustomAppointment1" by changing the comment to "customComment"
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

Scenario Outline: Amend appointment failure with incorrect interaction id
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I am performing the "<interactionId>" interaction
	When I amend "CustomAppointment1" by changing the comment to "customComment"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| interactionId                                                     |
		| urn:nhs:names:services:gpconnect:fhir:rest:update:appointmentt    |
		| urn:nSs:namEs:servIces:gpconnect:fhir:rest:update:appointmenT     |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		|                                                                   |
		| null                                                              |

Scenario Outline: Amend appointment _format parameter only
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I add the parameter "_format" with the value "<Parameter>"
	When I amend "CustomAppointment1" by changing the comment to "customComment"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the appointment resource should contain a comment which equals "customComment"
	Examples:
		| Parameter             | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Amend appointment accept header and _format parameter
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I amend "CustomAppointment1" by changing the comment to "customComment"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the appointment resource should contain a comment which equals "customComment"
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/json+fhir | application/json+fhir | JSON       |
		| application/json+fhir | application/xml+fhir  | XML        |
		| application/xml+fhir  | application/json+fhir | JSON       |
		| application/xml+fhir  | application/xml+fhir  | XML        |

Scenario: Amend appointment and check the returned appointment resource is a valid resource
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I amend "CustomAppointment1" by changing the comment to "customComment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the appointment resource should contain a comment which equals "customComment"
		And the appointment response resource contains a status with a valid value
		And the appointment response resource contains an start date
		And the appointment response resource contains an end date
		And the appointment response resource contains a slot reference
		And the appointment response resource contains atleast 2 participants a practitioner and a patient
		And the returned appointment resource should contain meta data profile and version id

Scenario: Amend appointment if resource contains identifier then the value is manadatory
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I amend "CustomAppointment1" by changing the comment to "customComment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And if the appointment response resource contains any identifiers they must have a value
		And the appointment resource should contain a comment which equals "customComment"

Scenario: Amend appointment if reason is included in response check that it conforms to one of the three valid types
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I amend "CustomAppointment1" by changing the comment to "customComment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And if the appointment response resource contains a reason element and coding the codings must be one of the three allowed with system code and display elements
		And the appointment resource should contain a comment which equals "customComment"

Scenario: Amend appointment containing a priority element and check that the priority is valid
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I amend "CustomAppointment1" by changing the comment to "customComment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And if the appointment contains a priority element it should be a valid value
		And the appointment resource should contain a comment which equals "customComment"

Scenario: Amend appointment and all participants must have a type or actor element
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I amend "CustomAppointment1" by changing the comment to "customComment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment participants must contain a type or actor element
		And the appointment resource should contain a comment which equals "customComment"

Scenario: Amend appointment prefer header set to representation
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I set the Prefer header to "return=representation"
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I amend "CustomAppointment1" by changing the comment to "customComment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the content-type should not be equal to null
		And the content-length should not be equal to zero

Scenario: Amend appointment prefer header set to minimal
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I set the Prefer header to "return=minimal"
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I amend "CustomAppointment1" by changing the comment to "customComment"
	Then the response status code should indicate success
		And the response body should be empty
		And the content-type should be equal to null

Scenario: Conformance profile supports the amend appointment operation
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the conformance profile should contain the "Appointment" resource with a "update" interaction

Scenario: Amend appointment send an update with an invalid if-match header
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I set If-Match request header to "hello"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I amend "CustomAppointment1" by changing the comment to "customComment"
	Then the response status code should be "409"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: Amend appointment valid if-match header
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I perform an appointment read for the appointment called "CustomAppointment1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response ETag is saved as "etagAmend"
		And the response should be an Appointment resource
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I set "If-Match" request header to resource stored "etagAmend"
	When I amend "CustomAppointment1" by changing the comment to "customComment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the appointment resource should contain a comment which equals "customComment"

Scenario: Amend appointment and send an invalid bundle resource
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I amend "CustomAppointment1" by changing the comment to "customComment" and send a bundle resource in the request
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Amend appointment and send an invalid appointment resource
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I amend "CustomAppointment1" by changing the comment to "customComment" and send an empty appointment resource
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"