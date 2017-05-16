Feature: AmendAppointment

Background:
	Given I have the test patient codes
	Given I have the test ods codes

Scenario: I perform a successful amend appointment and change the comment to a custom message
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I amend "CustomAppointment1" by changing the comment to "I hate doctors so changed the comment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the appointment resource should contain a comment which equals "I hate doctors so changed the comment"

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
	When I amend "CustomAppointment1" by changing the comment to "I hate doctors so changed the comment"
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
	When I amend "CustomAppointment1" by changing the comment to "I hate doctors so changed the comment"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
    Examples:
	  | interactionId                                                     |
	  | urn:nhs:names:services:gpconnect:fhir:rest:update:appointmentt    |
	  | urn:nhs:names:services:gpconnect:fhir:rest:update:appointmenT     |
	  | urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
	  |                                                                   |
	  | null                                                              |

Scenario Outline: Amend appointment _format parameter only
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I add the parameter "_format" with the value "<Parameter>"
	When I amend "CustomAppointment1" by changing the comment to "I hate doctors so changed the comment"
		Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
    Examples:
        | Parameter             | BodyFormat |
        | application/json+fhir | JSON       |
        | application/xml+fhir  | XML        |

Scenario Outline: Amend appointment accept header and _format parameter
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I amend "CustomAppointment1" by changing the comment to "I hate doctors so changed the comment"
		Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
	  Examples:
        | Header                | Parameter             | BodyFormat |
        | application/json+fhir | application/json+fhir | JSON       |
        | application/json+fhir | application/xml+fhir  | XML        |
        | application/xml+fhir  | application/json+fhir | JSON       |
        | application/xml+fhir  | application/xml+fhir  | XML        |   


	



