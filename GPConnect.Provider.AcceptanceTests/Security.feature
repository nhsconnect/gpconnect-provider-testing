Feature: Security

Scenario: JWT expiry time greater than 300 seconds
	Given I am using the gpconnect FHIR demonstator
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set the JWT expiry time to "301" seconds after creation time
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure

Scenario: JWT expiry time less than 300 seconds
	Given I am using the gpconnect FHIR demonstator
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set the JWT expiry time to "299" seconds after creation time
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
