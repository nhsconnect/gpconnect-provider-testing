@http
Feature: Patient

Background:
	Given I have the following patient records
		| Id                 | NHSNumber  |
		| patientNotInSystem | 9999999999 |
		| patient1           | 9000000001 |
		| patient2           | 9000000002 |
		| patient16          | 9000000016 |

Scenario: Returned patients should contain a logical identifier
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
	When I search for a Patient "patient2"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And all search response entities in bundle should contain a logical identifier

Scenario: Provider should return a paitent resource when a valid request is sent
		Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
	When I search for a Patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource

@ignore
Scenario: Provider should return an error when an invalid system is supplied in the identifier parameter
	#patient1

@ignore
Scenario: Provider should return an error when no system is supplied in the identifier parameter
	#patient1

@ignore
Scenario: Provider should return an error when a blank system is supplied in the identifier parameter
	#patient1

@ignore
Scenario: When a patient is not found on the provider system an empty bundle should be returned
	#patientNotInSystem

@ignore
Scenario: The provider system should accept the search parameter URL encoded
	#patient1

@ignore
Scenario: The provider system should accept the search parameter without URL encoding
	#patient1

