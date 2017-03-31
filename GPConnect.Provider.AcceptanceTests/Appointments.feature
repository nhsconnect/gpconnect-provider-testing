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
	When I make a GET request to "/Patient/<id>/Appointment"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource
	Examples:
		| Header            | id |
		| Ssp-TraceID       | 1   |
		| Ssp-From          |  1  |
		| Ssp-To            |   1 |
		| Ssp-InteractionId |  1  |
		| Authorization     |  1  |
