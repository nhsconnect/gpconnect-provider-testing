@http
Feature: Patient

Background:
	Given I have the following patient records
		| Id                 | NHSNumber  |
		| patientNotInSystem | 9999999999 |
		| patient1           | 9000000001 |
		| patient2           | 9000000002 |
		| patient3           | 9000000003 |
		| patient16          | 9000000016 |

@ignore
Scenario: The provider system should accept the search parameter URL encoded
	# The API being used in the test suite encodes the parameter string by default so no additional test needs to be performed.
	# The FHIR and HTTP standards require the request to be URL encoded so it is mandated that clents encode their requests.

Scenario: Returned patients should contain a logical identifier
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "patient2"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And all search response entities in bundle should contain a logical identifier

Scenario: Provider should return a patient resource when a valid request is sent
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries

Scenario: Provider should return an error when an invalid system is supplied in the identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "patient1" with system "http://test.net/types/internalIdentifier"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Provider should return an error when no system is supplied in the identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "patient1" without system in identifier parameter
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Provider should return an error when a blank system is supplied in the identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "patient1" with system ""
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: When a patient is not found on the provider system an empty bundle should be returned
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patientNotInSystem"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "patientNotInSystem"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "0" entries

Scenario: Patient search should fail if no identifier parameter is include
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
		And I set the JWT requested scope to "patient/*.read"
	When I make a GET request to "/Patient"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: The identifier parameter should be rejected if the case is incorrect
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "patient2" with parameter name "IdentIfier" and system "http://fhir.nhs.net/Id/nhs-number"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: The response should be an error if parameter is not identifier
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "patient2" with parameter name "nhsNumberParam" and system "http://fhir.nhs.net/Id/nhs-number"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: The response should be an error if no value is sent in the identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
		And I set the JWT requested scope to "patient/*.read"
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/nhs-number|"
	When I make a GET request to "/Patient"
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario Outline: The patient search endpoint should accept the format parameter after the identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
		And I set the JWT requested scope to "patient/*.read"
		And I set the Accept header to "<AcceptHeader>"
		And I add the parameter "identifier" with system "http://fhir.nhs.net/Id/nhs-number" for patient "patient2"
		And I add the parameter "_format" with the value "<FormatParam>"
	When I make a GET request to "/Patient"
	Then the response status code should indicate success
		And the response body should be FHIR <ResultFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
	Examples: 
	| AcceptHeader          | FormatParam           | ResultFormat |
	| application/xml+fhir  | application/xml+fhir  | XML          |
	| application/json+fhir | application/xml+fhir  | XML          |
	| application/json+fhir | application/json+fhir | JSON         |
	| application/xml+fhir  | application/json+fhir | JSON         |
	
Scenario Outline: The patient search endpoint should accept the format parameter before the identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
		And I set the JWT requested scope to "patient/*.read"
		And I set the Accept header to "<AcceptHeader>"
		And I add the parameter "_format" with the value "<FormatParam>"
		And I add the parameter "identifier" with system "http://fhir.nhs.net/Id/nhs-number" for patient "patient2"
	When I make a GET request to "/Patient"
	Then the response status code should indicate success
		And the response body should be FHIR <ResultFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
	Examples: 
	| AcceptHeader          | FormatParam           | ResultFormat |
	| application/xml+fhir  | application/xml+fhir  | XML          |
	| application/json+fhir | application/xml+fhir  | XML          |
	| application/json+fhir | application/json+fhir | JSON         |
	| application/xml+fhir  | application/json+fhir | JSON         |

Scenario Outline: Patient search failure due to invalid interactionId
	Given I am using the default server
		And I am performing the "<InteractionId>" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
	When I search for Patient "patient2"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource
	Examples:
		| InteractionId                                                     |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		| InvalidInteractionId                                              |
		|                                                                   |
		
Scenario Outline: Patient search failure due to missing header
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
		And I set the JWT requested scope to "patient/*.read"
		And I do not send header "<Header>"
	When I search for Patient "patient2"
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

Scenario Outline: Patient search with invalid identifier value
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
		And I set the JWT requested scope to "patient/*.read"
	When I search for a Patient with patameter name "identifier" and parameter string "http://fhir.nhs.net/Id/nhs-number|<IdentifierValue>"
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource
	Examples:
		| IdentifierValue |
		| 1234567891      |
		| 999999999       |
		| abcdefghij      |
		|                 |

Scenario: Patient resource should contain meta data elements
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the patient resource in the bundle should contain meta data profile and version id

Scenario Outline: Patient resource should contain NHS number identifier
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "<Patient>"
		And I set the JWT requested scope to "patient/*.read"
		And I set the Accept header to "application/xml+fhir"
	When I search for Patient "<Patient>"
	Then the response status code should indicate success
		And the response body should be FHIR XML
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the response bundle Patient entries should contain a single NHS Number identifier for patient "<Patient>"
	Examples: 
	| Patient  |
	| patient1 |
	| patient2 |
	| patient3 |

Scenario Outline: Patient resource should not contain disallowed fields in resource
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "<Patient>"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "<Patient>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And patient resource should not contain the fhir fields photo animal or link
	Examples: 
	| Patient  |
	| patient1 |
	| patient2 |
	| patient3 |

Scenario Outline: Check that if a care provider exists in the Patient resource it is valid
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "<Patient>"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "<Patient>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And if Patient resource contains careProvider the reference must be valid
	Examples: 
	| Patient  |
	| patient1 |
	| patient2 |
	| patient3 |
