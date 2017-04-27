Feature: ReadAppointment

Background:
	Given I have the test patient codes
	Given I have the test ods codes

Scenario: I perform a successful Read appointment
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I perform an appointment read for the first appointment saved in the list of resources stored against key "Patient1Appointments"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
	
Scenario Outline: Read appointment invalid request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I make a GET request to "/Appointment/<id>"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource
	Examples:
		| id     |
		| 2		 |
		| 400000 |

Scenario Outline: Read appointment failure due to missing header
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I do not send header "<Header>"
	When I make a GET request to "/Appointment/1"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource
	Examples:
		| Header            |
		| Ssp-TraceID       |
		| Ssp-From          |
		| Ssp-To            |
		| Ssp-InteractionId |
		| Authorization     |

Scenario Outline: Read appointment interaction id incorrect fail
    Given I am using the default server
        And I am performing the "<interactionId>" interaction
    When I make a GET request to "/Appointment/1"
    Then the response status code should be "400"
        And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
    Examples:
      | interactionId                                                     |
      | urn:nhs:names:services:gpconnect:fhir:rest:search:organization    |
      | urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
      |                                                                   |
      | null                                                              |

Scenario Outline: Read appointment accept header and _format parameter
    Given I am using the default server
        And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
        And I set the Accept header to "<Header>"
        And I add the parameter "_format" with the value "<Parameter>"
    When I make a GET request to "/Appointment/1"
    Then the response status code should indicate success
        And the response body should be FHIR <BodyFormat>
        And the response should be a Bundle resource of type "searchset"
    Examples:
        | Header                | Parameter             | BodyFormat |
        | application/json+fhir | application/json+fhir | JSON       |
        | application/json+fhir | application/xml+fhir  | XML        |
        | application/xml+fhir  | application/json+fhir | JSON       |
        | application/xml+fhir  | application/xml+fhir  | XML        |   


Scenario: Read appointment valid request shall include id and structure definition profile
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I make a GET request to "/Appointment/1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the appointment response resource contains an id
		And the appointment response resource should contain meta data profile and version id

Scenario: Read appointment valid request contains necessary elements with valid values
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I make a GET request to "/Appointment/1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the appointment response resource contains a status with a valid value
		And the appointment response resource contains an start date
		And the appointment response resource contains an end date
		And the appointment response resource contains a slot reference
		And the appointment response resource contains a participant which contains a status with a valid value

Scenario: Read appointment valid request contains valid identifier
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I make a GET request to "/Appointment/1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the appointment response resource contains an identifier with a valid system and value

Scenario: Read appointment request contains valid type with system and code
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I make a GET request to "/Appointment/1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the appointment response contains a type with a valid system code and display

Scenario: Read appointment request contains a valid priority
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I make a GET request to "/Appointment/1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And if the appointment resource contains a priority the value is valid