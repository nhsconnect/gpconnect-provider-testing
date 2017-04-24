Feature: SpecFlowFeature1

Background:
	Given I have the test patient codes

@Appointment
Scenario Outline: Appointment retrieve success valid id
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I make a GET request to "/Patient/<id>/Appointment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| id     |
		| 1		 |
		| 2		 |
		| 400000 |

Scenario Outline: Appointment retrieve fail invalid id
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I make a GET request to "/Patient/<id>/Appointment"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource
	Examples:
		| id  |
		| **  |
		| dd  |
		|     |
		| null|		

Scenario Outline: Appointment retrieve failure due to missing header
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I do not send header "<Header>"
	When I make a GET request to "/Patient/1/Appointment"
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

Scenario Outline: Appointment retrieve interaction id incorrect fail
    Given I am using the default server
        And I am performing the "<interactionId>" interaction
    When I make a GET request to "/Patient/1/Appointment"
    Then the response status code should be "400"
        And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
    Examples:
        | interactionId |
        | urn:nhs:names:services:gpconnect:fhir:rest:search:organization |
        | urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
        |    |
        |null| 
	
Scenario Outline: Appointment retrieve accept header and _format parameter
    Given I am using the default server
        And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
        And I set the Accept header to "<Header>"
        And I add the parameter "_format" with the value "<Parameter>"
    When I make a GET request to "/Patient/1/Appointment"
    Then the response status code should indicate success
        And the response body should be FHIR <BodyFormat>
        And the response should be a Bundle resource of type "searchset"
    Examples:
        | Header                | Parameter             | BodyFormat |
        | application/json+fhir | application/json+fhir | JSON       |
        | application/json+fhir | application/xml+fhir  | XML        |
        | application/xml+fhir  | application/json+fhir | JSON       |
        | application/xml+fhir  | application/xml+fhir  | XML        |           

Scenario Outline: Appointment retrieve accept header
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I set the Accept header to "<Header>"
	When I make a GET request to "/Patient/2/Appointment"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Appointment retrieve bundle resource with empty appointment resource
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I make a GET request to "/Patient/<id>/Appointment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
        And the response should be a Bundle resource of type "searchset"
		And there are zero appointment resources
	 Examples:
        | id|  
        | 1  | 
        |1000| 
        |45  | 
        |99  |      
		
Scenario Outline: Appointment retrieve bundle resource with multiple appointment resources
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I make a GET request to "/Patient/<id>/Appointment"
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "JSON"
        And the response should be a Bundle resource of type "searchset"
		And there are multiple appointment resources
	Then the bundle appointment resource should contain contain a single status element
		And status should have a valid value
	Examples:
        | id|  
        | 2 |

Scenario: Appointment retrieve bundle resource must contain status and participant 
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I make a GET request to "/Patient/2/Appointment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
        And the response should be a Bundle resource of type "searchset"
	Then the bundle appointment resource should contain contain a single status element
		And the appointment status element should be valid

Scenario: Appointment retrieve bundle of coding type SNOMED resource must contain coding with valid system and code and display
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I make a GET request to "/Patient/2/Appointment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And if appointment contains the resource coding SNOMED CT element the fields should match the fixed values of the specification

Scenario: Appointment retrieve bundle of coding type READ V2 resource must contain coding with valid system and code and display
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I make a GET request to "/Patient/2/Appointment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And if appointment contains the resource coding READ V2 element the fields should match the fixed values of the specification

Scenario: Appointment retrieve bundle of coding type SREAD CTV3 resource must contain coding with valid system and code and display
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I make a GET request to "/Patient/2/Appointment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And if appointment contains the resource coding SREAD CTV3 element the fields should match the fixed values of the specification

Scenario: Appointment retrieve bundle contains appointment with identifer with correct system and value
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I make a GET request to "/Patient/2/Appointment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And if the appointment resource contains an identifier it contains a valid system and value

@Ignore Checks the ranges are sensible but can probably be removed as an appointment is lokely no longer then an hour; NEEDS FURTHER THINKING
Scenario Outline: Appointment retrieve bundle contains appointment with valid start and end date format
Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I make a GET request to "/Patient/2/Appointment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And if the bundle contains a appointment resource the start and end date days are within range "<days>" days
		And if the bundle contains a appointment resource the start and end date months are within range "<months>" months
		And if the bundle contains a appointment resource the start and end date years are within range "<years>" years
		And if the the start date must be before the end date
		 Examples:
        | days | months | years |
        | 1    | 1      | 1     |
        | 0    | 0      | 0     |
        | 4    | 12     | 3     |
        | 9    | 1      | 4     |
		
Scenario: Appointment retrieve bundle contains appointment with slot 
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I make a GET request to "/Patient/2/Appointment"
		Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the appointment shall contain a slot or multiple slots