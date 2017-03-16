@security
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

Scenario: Security invalid client certificate sent
	Given I am using the default server
	And I am using an invalid client certificate
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should be "495"

Scenario: Security Expired client certificate sent
	Given I am using the default server
	And I am using an expired client certificate
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should be "495"

@ignore
Scenario: Connect with Invalid Secure Cipher
	# Connect to provider using a valid cipher which is secure but is not 256 AES

@ignore
Scenario: Connect with Invalid Broken Cipher
	# Connect to a provider using a cipher which has been broken and is vunerable to attacks

@ignore
Scenario: Connect with Cipher AESGCM+EECDH

@ignore
Scenario: Connect with Cipher AESGCM+EDH

@ignore
Scenario: Connect with Cipher AES256+EECDH

@ignore
Scenario: Connect with Cipher AES256+EDH