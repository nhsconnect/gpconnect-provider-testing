@ssp
Feature: SSP

# These tests are intended for the SSP security and do not apply to the provider directly

Scenario: SSP - Non-SSL to SSL
	Given I configure the default "MetadataRead" request
		And I am using the SSP
		And I am not using TLS Connection
	When I make the "MetadataRead" request
	Then the Response should indicate the connection was closed by the server or the Request was redirected 
		And if redirected the Response Headers should contain a Strict-Transport-Security header

Scenario: SSP - Consumer Client Certificate - Valid
	Given I configure the default "MetadataRead" request
		And I am using the SSP
		And I am using a TLS Connection
		And I am using the valid Consumer client certificate
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the Response Resource should be a CapabilityStatement

Scenario: SSP - Consumer Client Certificate - Invalid - FQDN
	Given I configure the default "MetadataRead" request
		And I am using the SSP
		And I am using a TLS Connection
		And I am using the Consumer client certificate with invalid FQDN
	When I make the "MetadataRead" request
	Then the Response Status Code should be one of "495, 496"
		And the Response should indicate the connection was closed by the server

Scenario: SSP - Consumer Client Certificate - Invalid - Authority
	Given I configure the default "MetadataRead" request
		And I am using the SSP
		And I am using a TLS Connection
		And I am using the Consumer client certificate not signed by Spine CA
	When I make the "MetadataRead" request
	Then the Response Status Code should be one of "495, 496"
		And the Response should indicate the connection was closed by the server

Scenario: SSP - Consumer Client Certificate - Invalid - Revoked
	Given I configure the default "MetadataRead" request
		And I am using the SSP
		And I am using a TLS Connection
		And I am using the Consumer client certificate which has been revoked
	When I make the "MetadataRead" request
	Then the Response Status Code should be one of "495, 496"
		And the Response should indicate the connection was closed by the server

Scenario: SSP - Consumer Client Certificate - Invalid - Expired
	Given I configure the default "MetadataRead" request
		And I am using the SSP
		And I am using a TLS Connection
		And I am using the Consumer client certificate which is out of date
	When I make the "MetadataRead" request
	Then the Response Status Code should be one of "495, 496"
		And the Response should indicate the connection was closed by the server

Scenario: SSP - Consumer Client Certificate - Invalid - Missing
	Given I configure the default "MetadataRead" request
		And I am using the SSP
		And I am using a TLS Connection
		And I am not using a client certificate
	When I make the "MetadataRead" request
	Then the Response Status Code should be one of "495, 496"
		And the Response should indicate the connection was closed by the server

Scenario Outline: SSP - Connect with valid Cipher
	Given I configure the default "MetadataRead" cURL request
		And I am using the SSP
		And I am using the valid Consumer client certificate
		And I set the Cipher to "<Cipher>"
	When I make the "MetadataRead" cURL request
	Then the cURL Code should be "Ok"
		And the Response Resource should be a CapabilityStatement
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

Scenario: SSP - Connect with invalid nonexistent Cipher
	Given I configure the default "MetadataRead" cURL request
		And I am using the SSP
		And I am using the valid Consumer client certificate
		And I set the Cipher to "ABC-DEF"
	When I make the "MetadataRead" cURL request
	Then the cURL Code should be "SslCipher"

Scenario: SSP - Connect with invalid insecure Cipher
	Given I configure the default "MetadataRead" cURL request
		And I am using the SSP
		And I am using the valid Consumer client certificate
		And I set the Cipher to "NULL-MD5"
	When I make the "MetadataRead" cURL request
	Then the cURL Code should be "SslConnectError"

Scenario: SSP - Connect with invalid secure Cipher
	Given I configure the default "MetadataRead" cURL request
		And I am using the SSP
		And I am using the valid Consumer client certificate
		And I set the Cipher to "AES128-SHA256"
	When I make the "MetadataRead" cURL request
	Then the cURL Code should be "SslConnectError"

Scenario Outline: SSP - CORS
	Given I configure the default "<Interaction>" request
		And I am using the SSP
		And I am using the valid Consumer client certificate
		And I set the request Http Method to "OPTIONS"
	When I make the "<Interaction>" request
	Then the response status code should indicate success
		And the Response Headers should contain an Access-Control-Request-Method header
		Then the Access-Control-Request-Method header should contain the "<Method>" request methods
	Examples: 
	| Interaction        | Method   | Url (for reference only)     |
	| GpcGetCareRecord   | POST     | Patient/$gpc.getcarerecord   |
	| OrganizationSearch | GET      | Organization                 |
	| OrganizationRead   | GET      | Organization/{id}            |
	| PractitionerSearch | GET      | Practitioner                 |
	| PractitionerRead   | GET      | Practitioner/{id}            |
	| PatientSearch      | GET      | Patient                      |
	| PatientRead        | GET      | Patient/{id}                 |
	| LocationRead       | GET      | Location/{id}                |
	| RegisterPatient    | POST     | Patient/$gpc.registerpatient |
	| SearchForFreeSlots | GET      | Slot						   |
	| AppointmentCreate  | POST     | Appointment                  |
	| AppointmentSearch  | GET      | Patient/{id}/Appointment     |
	| AppointmentAmend   | GET, PUT | Appointment/{id}             |
	| AppointmentCancel  | GET, PUT | Appointment/{id}             |
	| AppointmentRead    | GET, PUT | Appointment/{id}             |
	| MetadataRead       | GET      | metadata                     |

Scenario: SSP - To ASID - Invalid
	Given I configure the default "MetadataRead" request
		And I am using the SSP
		And I am using the valid Consumer client certificate
		And I am connecting to accredited system "123456789123"
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"