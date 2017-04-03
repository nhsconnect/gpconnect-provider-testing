@http
Feature: Patient

Background:
	Given I have the following patient records
		| Id                 | NHSNumber  |
		| patientNotInSystem | 9999999999 |
		| patient1           | 9000000001 |
		| patient2           | 9000000002 |
		| patient16          | 9000000016 |

@ignore
Scenario: The provider system should accept the search parameter URL encoded
	# The API being used in the test suite encodes the parameter string by default so no additional test needs to be performed.
	# The FHIR and HTTP standards require the request to be URL encoded so it is mandated that clents encode their requests.

Scenario: Returned patients should contain a logical identifier
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
	When I search for Patient "patient2"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And all search response entities in bundle should contain a logical identifier

Scenario: Provider should return a patient resource when a valid request is sent
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
	When I search for Patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"

Scenario: Provider should return an error when an invalid system is supplied in the identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
	When I search for Patient "patient1" with system "http://test.net/types/internalIdentifier"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Provider should return an error when no system is supplied in the identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
	When I search for Patient "patient1" without system in identifier parameter
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Provider should return an error when a blank system is supplied in the identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
	When I search for Patient "patient1" with system ""
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: When a patient is not found on the provider system an empty bundle should be returned
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patientNotInSystem"
	When I search for Patient "patientNotInSystem"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource
		And response bundle should contain "0" entries

Scenario: Patient search should fail if no identifier parameter is include
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
	When I make a GET request to "/Patient"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: The identifier parameter should be rejected if the case is incorrect
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
	When I search for Patient "patient2" with parameter name "IdentIfier" and system "http://fhir.nhs.net/Id/nhs-number"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: The response should be an error if parameter is not identifier
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
	When I search for Patient "patient2" with parameter name "nhsNumberParam" and system "http://fhir.nhs.net/Id/nhs-number"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: The response should be an error if no value is sent in the identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/nhs-number|"
	When I make a GET request to "/Patient"
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario Outline: The patient search endpoint should accept the format parameter after the identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "<Patient>"
		And I set the Accept header to "<AcceptHeader>"
		And I add the parameter "identifier" with system "http://fhir.nhs.net/Id/nhs-number" for patient "<Patient>"
		And I add the parameter "_format" with the value "<FormatParam>"
	When I make a GET request to "/Patient"
	Then the response status code should indicate success
		And the response body should be FHIR <ResultFormat>
		And the response should be a Bundle resource of type "searchset"
		And response bundle should contain "1" entries
	Examples: 
	| AcceptHeader          | FormatParam           | Patient  | ResultFormat |
	| application/xml+fhir  | application/xml+fhir  | patient2 | XML          |
	| application/json+fhir | application/xml+fhir  | patient2 | XML          |
	| application/json+fhir | application/json+fhir | patient2 | JSON         |
	| application/xml+fhir  | application/json+fhir | patient2 | JSON         |
	
Scenario Outline: The patient search endpoint should accept the format parameter before the identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "<Patient>"
		And I set the Accept header to "<AcceptHeader>"
		And I add the parameter "_format" with the value "<FormatParam>"
		And I add the parameter "identifier" with system "http://fhir.nhs.net/Id/nhs-number" for patient "<Patient>"
	When I make a GET request to "/Patient"
	Then the response status code should indicate success
		And the response body should be FHIR <ResultFormat>
		And the response should be a Bundle resource of type "searchset"
		And response bundle should contain "1" entries
	Examples: 
	| AcceptHeader          | FormatParam           | Patient  | ResultFormat |
	| application/xml+fhir  | application/xml+fhir  | patient2 | XML          |
	| application/json+fhir | application/xml+fhir  | patient2 | XML          |
	| application/json+fhir | application/json+fhir | patient2 | JSON         |
	| application/xml+fhir  | application/json+fhir | patient2 | JSON         |