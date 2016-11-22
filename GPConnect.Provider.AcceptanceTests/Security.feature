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
	Then the response status code should indicate authentication failure

Scenario: Security invalid client certificate sent
	Given I am using the default server
	And I am using client certificate with thumbprint "ea 57 be 3c 6c da 33 e6 d8 75 88 99 44 ee 61 28 4e 39 d9 1d"
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate authentication failure
