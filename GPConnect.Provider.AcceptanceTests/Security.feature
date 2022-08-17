@security @0.7.4-Full-Pack
Feature: Security

Scenario: Security reject a  non ssl request
	Given I am using the default server
		And I am not using TLS Connection
		And I am connecting to server on port "80"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure

Scenario: Security valid client certificate, ciper and SSL
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success

Scenario: Security no client certificate sent
	Given I am using the default server
		And I am not using a client certificate
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should be "496"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Security invalid client certificate sent
	Given I am using the default server
		And I am using an invalid client certificate
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should be "495"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Security Expired client certificate sent
	Given I am using the default server
		And I am using an expired client certificate
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should be "495"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource
