Feature: JWT

Scenario: JWT expiry time greater than 300 seconds
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set the JWT expiry time to "301" seconds after creation time
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure

Scenario: JWT expiry time less than 300 seconds
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set the JWT expiry time to "299" seconds after creation time
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure

Scenario: JWT not base64 encoded
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set the default JWT without base64 encoding
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure

Scenario: JWT expiry time before creation time
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set the JWT expiry time to "-1" seconds after creation time
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure

Scenario: JWT creation time in the future
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set the JWT creation time to "200" seconds after the current time
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure

Scenario: JWT reason for request is not directcare
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set the JWT reason for request to "notdirectcare"
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure

Scenario: JWT authorization server token incorrect
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set the JWT authorization server token URL to "https://notValid.fhir.nhs.net/tokenEndpoint"
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure

Scenario: JWT requesting device is not valid FHIR device resource
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set an invalid JWT requesting device resource
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
