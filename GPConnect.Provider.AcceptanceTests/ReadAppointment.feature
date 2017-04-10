Feature: ReadAppointment

#PreReq 1 is a valid appointment. Plan to make this automatic for future testing

@Appointment
Scenario Outline: Read appointment valid request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I make a GET request to "/Appointment/<id>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		
	Examples:
		| id     |
		| 1		 |
	
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
	When I make a GET request to "/Appointment/<id>"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource
	Examples:
		| id | Header            |
		| 1  | Ssp-TraceID       |
		| 1  | Ssp-From          |
		| 1  | Ssp-To            |
		| 1  | Ssp-InteractionId |
		| 1  | Authorization     |

Scenario Outline: Read appointment interaction id incorrect fail
    Given I am using the default server
        And I am performing the "<interactionId>" interaction
    When I make a GET request to "/Appointment/<id>"
    Then the response status code should be "400"
        And the response body should be FHIR JSON
        And the response should be a OperationOutcome resource
    Examples:
      | id | interactionId                                                     |
      | 1  | urn:nhs:names:services:gpconnect:fhir:rest:search:organization    |
      | 1  | urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
      | 1  |                                                                   |
      | 1  | null                                                              |

Scenario Outline: Read appointment accept header and _format parameter
    Given I am using the default server
        And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
        And I set the Accept header to "<Header>"
        And I add the parameter "_format" with the value "<Parameter>"
    When I make a GET request to "/Appointment/<id>"
    Then the response status code should indicate success
        And the response body should be FHIR <BodyFormat>
        And the response should be a Bundle resource of type "searchset"
		
    Examples:
        | id | Header                | Parameter             | BodyFormat |
        |  1  | application/json+fhir | application/json+fhir | JSON       |
        |  1  | application/json+fhir | application/xml+fhir  | XML        |
        |  1  | application/xml+fhir  | application/json+fhir | JSON       |
        |  1  | application/xml+fhir  | application/xml+fhir  | XML        |   


Scenario Outline: Read appointment valid request shall include structure definition profile
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I make a GET request to "/Appointment/<id>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the appointment reosurce should contain meta data profile and version id
	Examples:
		| id     |
		| 1		 |

