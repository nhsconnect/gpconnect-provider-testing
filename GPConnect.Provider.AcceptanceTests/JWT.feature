Feature: JWT

Scenario: JWT expiry time greater than 300 seconds
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set the JWT expiry time to "301" seconds after creation time
	When I make a GET request to "/metadata"
	Then the response status code should indicate authentication failure

Scenario: JWT expiry time less than 300 seconds
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set the JWT expiry time to "299" seconds after creation time
	When I make a GET request to "/metadata"
	Then the response status code should indicate authentication failure

Scenario: JWT not base64 encoded
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set the default JWT without base64 encoding
	When I make a GET request to "/metadata"
	Then the response status code should indicate authentication failure
