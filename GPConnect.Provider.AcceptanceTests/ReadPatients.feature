Feature: ReadPatient

Background:
	Given I have the test patient codes
	
Scenario: Read patient 404 if patient not found
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
	When I make a GET request to "/Patient/123"
	Then the response status code should be "404"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"
		
Scenario: Read patient 404 if patient id not sent
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
	When I make a GET request to "/Patient/"
	Then the response status code should be "404"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"
		
Scenario Outline: Read patient _format parameter only
	Given I perform the searchPatient operation for patient "patient1" and store the returned patient
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
        And I add the parameter "_format" with the value "<Parameter>"
	When I make a GET request for patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Patient resource
    Examples:
        | Parameter             | BodyFormat |
        | application/json+fhir | JSON       |
        | application/xml+fhir  | XML        |
		
Scenario Outline: Read patient accept header and _format parameter
	Given I perform the searchPatient operation for patient "patient1" and store the returned patient
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
		And I set the Accept header to "<Header>"
        And I add the parameter "_format" with the value "<Parameter>"
	When I make a GET request for patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Patient resource
    Examples:
        | Header                | Parameter             | BodyFormat |
        | application/json+fhir | application/json+fhir | JSON       |
        | application/json+fhir | application/xml+fhir  | XML        |
        | application/xml+fhir  | application/json+fhir | JSON       |
        | application/xml+fhir  | application/xml+fhir  | XML        |
		
Scenario Outline: Read patient failure due to missing header
	Given I perform the searchPatient operation for patient "patient1" and store the returned patient
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
		And I do not send header "<Header>"
	When I make a GET request for patient "patient1"
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

Scenario: Read patient should contain correct logical identifier
	Given I perform the searchPatient operation for patient "patient1" and store the returned patient
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
	When I make a GET request for patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Patient resource
		And the response patient logical identifier should match that of stored patient "patient1"

Scenario: Read patient should contain ETag
	Given I perform the searchPatient operation for patient "patient1" and store the returned patient
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
	When I make a GET request for patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Patient resource
		And the response should contain the ETag header matching the resource version

Scenario: Read patient If-None-Match should return a 304 on match
	Given I perform the searchPatient operation for patient "patient1" and store the returned patient
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
	When I make a GET request for patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Patient resource
		And the response ETag is saved as "etagPatientRead"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
		And I set "If-None-Match" request header to "etagPatientRead"
	When I make a GET request for patient "patient1"
	Then the response status code should be "304"
	
Scenario: Read patient If-None-Match should return full resource if no match
	Given I perform the searchPatient operation for patient "patient1" and store the returned patient
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
		And I set the If-None-Match header to "W/\"somethingincorrect\""
	When I make a GET request for patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Patient resource
		And the response should contain the ETag header matching the resource version
