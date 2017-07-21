@security
Feature: Security

Scenario: Security - non ssl request
	Given I configure the default "GpcGetCareRecord" request
		And I am not using the SSP
		And I am not using TLS Connection
		And I am connecting to server on port "80"
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Security - valid client certificate
	Given I configure the default "GpcGetCareRecord" request
		And I am not using the SSP
		And I am using the SSP client certificate
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "document"

Scenario: Security - client certificate invalid FQDN
	Given I configure the default "GpcGetCareRecord" request
		And I am not using the SSP
		And I am using the SSP client certificate with invalid FQDN
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
	When I make the "GpcGetCareRecord" request
	Then the response status code should be "495"
		And the response should be a OperationOutcome resource

Scenario: Security - client certificate not issued by the Spine CA
	Given I configure the default "GpcGetCareRecord" request
		And I am not using the SSP
		And I am using the SSP client certificate not signed by Spine CA
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
	When I make the "GpcGetCareRecord" request
	Then the response status code should be "495"
		And the response should be a OperationOutcome resource

Scenario: Security - client certificate revoked
	Given I configure the default "GpcGetCareRecord" request
		And I am not using the SSP
		And I am using the SSP client certificate which has been revoked
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
	When I make the "GpcGetCareRecord" request
	Then the response status code should be "495"
		And the response should be a OperationOutcome resource

Scenario: Security - client certificate out of date
	Given I configure the default "GpcGetCareRecord" request
		And I am not using the SSP
		And I am using the SSP client certificate which is out of date
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
	When I make the "GpcGetCareRecord" request
	Then the response status code should be "495"
		And the response should be a OperationOutcome resource

Scenario: Security - no client certificate included in request
	Given I configure the default "GpcGetCareRecord" request
		And I am not using the SSP
		And I am not using a client certificate
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
	When I make the "GpcGetCareRecord" request
	Then the response status code should be "496"
		And the response should be a OperationOutcome resource

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