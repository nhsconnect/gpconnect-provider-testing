Feature: SpecFlowFeature1

@Appointment
Scenario Outline: Appointment retrieve success valid id
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I make a GET request to "/Patient/<id>/Appointment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset
	Examples:
		| id		|
		| 1			|
		| 2			|
		| 400000	|
			

Scenario Outline: Appointment retrieve fail invalid id
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I make a GET request to "/Patient/<id>/Appointment"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
	Examples:
		| id |
		| ** |
		| dd |
		|    |
		|null|		

Scenario Outline: Appointment retrieve failure due to missing header
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I do not send header "<Header>"
	When I make a GET request to "/Patient/1/Appointment"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource
	Examples:
		| Header            |
		| Ssp-TraceID       |
		| Ssp-From          |
		| Ssp-To            |
		| Ssp-InteractionId |
		| Authorization     |

Scenario Outline: Appointment retrieve interaction Id incorrect fail
    Given I am using the default server
        And I am performing the "<interactionId>" interaction
    When I make a GET request to "/Patient/<id>/Appointment"
    Then the response status code should be "400"
        And the response body should be FHIR JSON
        And the JSON response should be a OperationOutcome resource
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
        And the JSON response should be a Bundle resource
        And the JSON response bundle should be type searchset
    Examples:
        | Header                | Parameter             | BodyFormat |
        | application/json+fhir | application/json+fhir | JSON       |
        | application/json+fhir | application/xml+fhir  | XML        |
        | application/xml+fhir  | application/json+fhir | JSON       |
        | application/xml+fhir  | application/xml+fhir  | XML        |           

Scenario Outline: Appointment retrieve bundle resource with empty appointment resource
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I make a GET request to "/Patient/<id>/Appointment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset
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
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset
		And there are multiple appointment resources
	Then the appointment resources must contain a status element
		And status should have a valid value
	Examples:
        | id|  
        | 2 |


		






            