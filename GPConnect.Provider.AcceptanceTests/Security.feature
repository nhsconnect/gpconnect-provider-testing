@security
Feature: Security

#These tests expect some Apache/nginx specific http errors 495, 496 - need to consider providers not using Apache/nginx.

Scenario: Security - Non-SSL to SSL
	Given I configure the default "MetadataRead" request
		And I am not using the SSP
		And I am using the valid SSP client certificate
		And I am not using TLS Connection
	When I make the "MetadataRead" request
	Then the Response should indicate the connection was closed by the server or the Request was redirected 
		And if redirected the Response Headers should contain a Strict-Transport-Security header

Scenario: Security - SSP Client Certificate - Valid
	Given I configure the default "MetadataRead" request
		And I am not using the SSP
		And I am using the valid SSP client certificate
		And I am using a TLS Connection
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the Response Resource should be a Conformance

Scenario: Security - SSP Client Certificate - Invalid - Expired
	Given I configure the default "MetadataRead" request
		And I am not using the SSP
		And I am using the SSP client certificate which has expired
		And I am using a TLS Connection
	When I make the "MetadataRead" request
	Then the Response Status Code should be one of "495, 496"
		And the Response should indicate the connection was closed by the server

Scenario: Security - SSP Client Certificate - Invalid - FQDN
	Given I configure the default "MetadataRead" request
		And I am not using the SSP
		And I am using the SSP client certificate with invalid FQDN
		And I am using a TLS Connection
	When I make the "MetadataRead" request
	Then the Response Status Code should be one of "495, 496"
		And the Response should indicate the connection was closed by the server

Scenario: Security - SSP Client Certificate - Invalid - Authority
	Given I configure the default "MetadataRead" request
		And I am not using the SSP
		And I am using the SSP client certificate not signed by Spine CA
		And I am using a TLS Connection
	When I make the "MetadataRead" request
	Then the Response Status Code should be one of "495, 496"
		And the Response should indicate the connection was closed by the server

Scenario: Security - SSP Client Certificate - Invalid - Revoked
	Given I configure the default "MetadataRead" request
		And I am not using the SSP
		And I am using the SSP client certificate which has been revoked
		And I am using a TLS Connection
	When I make the "MetadataRead" request
	Then the Response Status Code should be one of "495, 496"
		And the Response should indicate the connection was closed by the server

Scenario: Security - SSP Client Certificate - Invalid - Missing
	Given I configure the default "MetadataRead" request
		And I am not using the SSP
		And I am not using a client certificate
		And I am using a TLS Connection
	When I make the "MetadataRead" request
	Then the response status code should be "496"

Scenario Outline: Security - Connect with valid Cipher
	Given I configure the default "MetadataRead" cURL request
		And I am not using the SSP
		And I am using the valid SSP client certificate
		And I set the Cipher to "<Cipher>"
	When I make the "MetadataRead" cURL request
	Then the cURL Code should be "Ok"
	Examples: 
	| Cipher                      |
	| ECDHE-RSA-AES128-GCM-SHA256 |
	| ECDHE-RSA-AES256-GCM-SHA384 |
	| ECDHE-RSA-AES256-SHA384     |
	| ECDHE-RSA-AES256-SHA        |
	| DHE-RSA-AES128-GCM-SHA256   |
	| DHE-RSA-AES256-GCM-SHA384   |
	| DHE-RSA-AES256-SHA256       |
	| DHE-RSA-AES256-SHA          |

Scenario: Security - Connect with invalid nonexistent Cipher
	Given I configure the default "MetadataRead" cURL request
		And I am not using the SSP
		And I am using the valid SSP client certificate
		And I set the Cipher to "ABC-DEF"		
	When I make the "MetadataRead" cURL request
	Then the cURL Code should be "SslCipher"

Scenario: Security - Connect with invalid insecure Cipher
	Given I configure the default "MetadataRead" cURL request
		And I am not using the SSP
		And I am using the valid SSP client certificate
		And I set the Cipher to "NULL-MD5"
	When I make the "MetadataRead" cURL request
	Then the cURL Code should be "SslConnectError"

Scenario: Security - Connect with invalid secure Cipher
	Given I configure the default "MetadataRead" cURL request
		And I am not using the SSP
		And I am using the valid SSP client certificate
		And I set the Cipher to "AES128-SHA256"
	When I make the "MetadataRead" cURL request
	Then the cURL Code should be "SslConnectError"