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
        