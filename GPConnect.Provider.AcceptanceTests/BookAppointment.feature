Feature: BookAppointment

Background:
	Given I have the test patient codes
	Given I have the test ods codes

@Appointment
Scenario Outline: Book Appointment
	Given I am using the default server
		And I search for the organization "ORG1" on the providers system and save the first response to "ORG1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I get the slots avaliable slots for organization "ORG1" for the next 3 days
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	When I book an appointment for patient "<id>" on the provider system
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| id |
		| 3  |


Scenario: Book Appointment with invalid url for booking appointment
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	When I make a GET request to "/Appointment/5"
	Then the response status code should indicate failure

Scenario Outline: Book appointment failure due to missing header
	Given I am using the default server
			And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I do not send header "<Header>"
	When I book an appointment for patient "<id>" on the provider system
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| id | Header            |
		| 3  | Ssp-TraceID       |
		| 3  | Ssp-From          |
		| 3  | Ssp-To            |
		| 3  | Ssp-InteractionId |
		| 3  | Authorization     |

Scenario Outline: Book appointment accept header and _format parameter
    Given I am using the default server
       	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
        And I set the Accept header to "<Header>"
        And I add the parameter "_format" with the value "<Parameter>"
    When I book an appointment for patient "<id>" on the provider system
    Then the response status code should indicate success
        And the response body should be FHIR <BodyFormat>
        And the response should be a Bundle resource of type "searchset"
    Examples:
       | id | Header                | Parameter             | BodyFormat |
       | 1  | application/json+fhir | application/json+fhir | JSON       |
       | 1  | application/json+fhir | application/xml+fhir  | XML        |
       | 1  | application/xml+fhir  | application/json+fhir | JSON       |
       | 1  | application/xml+fhir  | application/xml+fhir  | XML        |  


Scenario Outline: Book appointment accept header variations
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I set the Accept header to "<Header>"
	When I book an appointment for patient "<id>" on the provider system
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| id | Header                | BodyFormat |
		| 1  | application/json+fhir | JSON       |
		| 1  | application/xml+fhir  | XML        |

Scenario Outline: Book appointment interaction id incorrect fail
    Given I am using the default server
		And I search for the organization "ORG1" on the providers system and save the first response to "ORG1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I get the slots avaliable slots for organization "ORG1" for the next 3 days
	Given I am using the default server
   When I book an appointment for patient "<id>" on the provider system using interaction id "<interactionId>"
    Then the response status code should be "400"
        And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
    Examples:
       | id | interactionId                                                     |
       | 1  | urn:nhs:names:services:gpconnect:fhir:rest:search:organization    |
       | 1  | urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
       | 1  |                                                                   |
       | 1  | null                                                              |