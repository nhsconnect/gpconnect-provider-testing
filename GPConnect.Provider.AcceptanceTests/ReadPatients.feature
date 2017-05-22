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

Scenario: Read patient should contain correct logical identifier
	Given I perform the searchPatient operation for patient "patient1" and store the returned patient
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
	When I make a GET request for patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Patient resource
		And the response patient logical identifier should match that of stored patient "patient1"
