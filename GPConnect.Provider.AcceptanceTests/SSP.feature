@ssp
Feature: SSP

# These tests are intended for the SSP security and do not apply to the provider directly

Scenario: Send to endpoint with incorrect To asid for the provider endpoint
	Given I configure the default "GpcGetCareRecord" request
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I am connecting to accredited system "123456789123"
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
	When I make the "GpcGetCareRecord" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: SSP - non ssl request
	Given I configure the default "MetadataRead" request
		And I am using the SSP
		And I am not using TLS Connection
	When I make the "MetadataRead" request
	Then the Response should indicate the connection was closed by the server or the Request was redirected 
		And if redirected the Response Headers should contain a Strict-Transport-Security header

Scenario: SSP - valid client certificate
	Given I configure the default "GpcGetCareRecord" request
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"

Scenario: SSP - client certificate invalid FQDN
	Given I configure the default "GpcGetCareRecord" request
		And I am using the client certificate with invalid FQDN
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
	When I make the "GpcGetCareRecord" request
	Then the response status code should be "495"
		And the response should be a OperationOutcome resource

Scenario: SSP - client certificate not issued by the Spine CA
	Given I configure the default "GpcGetCareRecord" request
		And I am using the client certificate not signed by Spine CA
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
	When I make the "GpcGetCareRecord" request
	Then the response status code should be "495"
		And the response should be a OperationOutcome resource

Scenario: SSP - client certificate revoked
	Given I configure the default "GpcGetCareRecord" request
		And I am using the client certificate which has been revoked
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
	When I make the "GpcGetCareRecord" request
	Then the response status code should be "495"
		And the response should be a OperationOutcome resource

Scenario: SSP - client certificate out of date
	Given I configure the default "GpcGetCareRecord" request
		And I am using the client certificate which is out of date
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
	When I make the "GpcGetCareRecord" request
	Then the response status code should be "495"
		And the response should be a OperationOutcome resource

Scenario: Security - no client certificate included in request
	Given I configure the default "GpcGetCareRecord" request
		And I am not using a client certificate
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
	When I make the "GpcGetCareRecord" request
	Then the response status code should be "496"
		And the response should be a OperationOutcome resource

Scenario Outline: SSP - Connect with valid Cipher
	Given I configure the default "MetadataRead" cURL request
		And I am using the SSP
		And I am using the client certificate
		And I set the Cipher to "<Cipher>"
	When I make the "MetadataRead" cURL request
	Then the cURL Code should be "Ok"
		And the Response Resource should be a Conformance
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
		And I am using the SSP
		And I am using the client certificate
		And I set the Cipher to "ABC-DEF"
	When I make the "MetadataRead" cURL request
	Then the cURL Code should be "SslCipher"

Scenario: Security - Connect with invalid insecure Cipher
	Given I configure the default "MetadataRead" cURL request
		And I am using the SSP
		And I am using the client certificate
		And I set the Cipher to "NULL-MD5"
	When I make the "MetadataRead" cURL request
	Then the cURL Code should be "SslConnectError"

Scenario: Security - Connect with invalid secure Cipher
	Given I configure the default "MetadataRead" cURL request
		And I am using the SSP
		And I am using the client certificate
		And I set the Cipher to "AES128-SHA256"
	When I make the "MetadataRead" cURL request
	Then the cURL Code should be "SslConnectError"

Scenario Outline: SSP - CORS
	Given I configure the default "<Interaction>" request
		And I am using the SSP
		And I am using the client certificate
		And I set the request Http Method to "OPTIONS"
	When I make the "<Interaction>" request
	Then the response status code should indicate success
		And the Response Headers should contain an Access-Control-Request-Method header
		Then the Access-Control-Request-Method header should contain the "<Method>" request methods
	Examples: 
	| Interaction        | Method   | Url (for reference only)			|
	| GpcGetCareRecord   | POST     | Patient/$gpc.getcarerecord        |
	| OrganizationSearch | GET      | Organization                      |
	| OrganizationRead   | GET      | Organization/{id}                 |
	| PractitionerSearch | GET      | Practitioner                      |
	| PractitionerRead   | GET      | Practitioner/{id}                 |
	| PatientSearch      | GET      | Patient                           |
	| PatientRead        | GET      | Patient/{id}                      |
	| LocationSearch     | GET      | Location                          |
	| LocationRead       | GET      | Location/{id}                     |
	| RegisterPatient    | POST     | Patient/$gpc.registerpatient      |
	| GpcGetSchedule     | POST     | Organization/{id}/$gpcgetscehdule |
	| AppointmentCreate  | POST     | Appointment                       |
	| AppointmentSearch  | GET      | Patient/{id}/Appointment          |
	| AppointmentAmend   | GET, PUT | Appointment/{id}                  |
	| AppointmentCancel  | GET, PUT | Appointment/{id}                  |
	| AppointmentRead    | GET, PUT | Appointment/{id}                  |
	| MetadataRead       | GET      | metadata                          |
