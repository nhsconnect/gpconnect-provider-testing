@fhir
Feature: FHIR

Scenario: Fhir Get MetaData
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the Response Resource should be a CapabilityStatement

Scenario: CapabilityStatement profile indicates acceptance of xml and json format
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement Format should contain XML and JSON

Scenario: CapabilityStatement profile suppliers software versions present
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement Software should be valid

Scenario: CapabilityStatement profile supported fhir version
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement FHIR Version should be "3.0.1"

Scenario: CapabilityStatement profile supports the gpc.getcarerecord operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement REST Operations should contain "gpc.getcarerecord"

Scenario: FHIR request content type XML but no accept header or _format sent with request
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I set the request content type to "application/fhir+xml"
		And I do not send header "Accept"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: FHIR request content type JSON but no accept header or _format sent with request
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I set the request content type to "application/fhir+json"
		And I do not send header "Accept"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir content type test where Accept header is JSON and request payload is JSON
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I set the request content type to "application/fhir+json"
		And I set the Accept header to "application/fhir+json"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir content type test where Accept header is XML and request payload is XML
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I set the request content type to "application/fhir+xml"
		And I set the Accept header to "application/fhir+xml"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: Fhir content type test where Accept header is JSON and request payload is XML
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+xml"
		And I set the Accept header to "application/fhir+json"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir content type test where Accept header is XML and request payload is JSON
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+json"
		And I set the Accept header to "application/fhir+xml"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: Fhir content type test where _format parameter is JSON and request payload is JSON
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+json"
		And I do not send header "Accept"
		And I add a Format parameter with the Value "application/fhir+json"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir content type test where _format parameter is JSON and request payload is XML
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+xml"
		And I do not send header "Accept"
		And I add a Format parameter with the Value "application/fhir+json"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir content type test where _format parameter is XML and request payload is XML
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+xml"
		And I do not send header "Accept"
		And I add a Format parameter with the Value "application/fhir+xml"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: Fhir content type test where _format parameter is XML and request payload is JSON
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+json"
		And I do not send header "Accept"
		And I add a Format parameter with the Value "application/fhir+xml"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: Fhir content type test where Accept header is XML and _format parameter is XML
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+xml"
		And I set the Accept header to "application/fhir+xml"
		And I add a Format parameter with the Value "application/fhir+xml"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: Fhir content type test where Accept header is XML and _format parameter is JSON
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+xml"
		And I set the Accept header to "application/fhir+xml"
		And I add a Format parameter with the Value "application/fhir+json"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir content type test where Accept header is JSON and _format parameter is JSON
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+json"
		And I set the Accept header to "application/fhir+json"
		And I add a Format parameter with the Value "application/fhir+json"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir content type test where Accept header is JSON and _format parameter is XML
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+json"
		And I set the Accept header to "application/fhir+json"
		And I add a Format parameter with the Value "application/fhir+xml"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: FHIR content type test where Invalid content type application/xml is sent
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I set the request content type to "application/xml"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate unsupported media type error
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: FHIR content type test where Invalid content type application/json is sent
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I set the request content type to "application/json"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate unsupported media type error
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: FHIR content type test where Invalid content type sent text/xml
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I set the request content type to "text/xml"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate unsupported media type error
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: Fhir content type test where Accept header is unsupported media type and request payload is JSON
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I set the request content type to "application/fhir+json"
		And I set the Accept header to "text/xml"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate unsupported media type error

Scenario: Fhir content type test where _format parameter is an unsupported media type and request payload is xml
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I set the request content type to "application/fhir+xml"
		And I set the Accept header to "application/fhir+xml"
		And I add a Format parameter with the Value "text/xml"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate unsupported media type error

Scenario Outline: Request and response in XML
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "<Code>"
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I set the request content type to "application/fhir+xml"
		And I set the Accept header to "application/fhir+xml"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate success
		And the response body should be FHIR XML
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| Code |
		| ADM |
		| ALL |
		| CLI |
		| ENC |
		| IMM |
		#| INV |
		| MED |
		| OBS |
		#| PAT |
		| PRB |
		| REF |
		| SUM |

Scenario: endpoint should support gzip compression for metadata endpoint and contain the correct payload
	Given I configure the default "MetadataRead" request
		And I set the Accept-Encoding header to gzip
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response should be gzip encoded
		And the response body should be FHIR JSON
		And the Response Resource should be a CapabilityStatement

Scenario: endpoint should support gzip compression for getCareRecord operation and contain correct payload
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I set the Accept-Encoding header to gzip
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate success
		And the response should be gzip encoded
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"

Scenario: endpoint should support chunking of data
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
		And I set the JWT Requested Record to the NHS Number for "patient2"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response should be chunked

@ignore
@Manual
Scenario: maximum field size in fhir resource
	# String fields must not contain more than 1mb or data, this will require a test patient with data greater than 1mb a field that maps to a string field in the fhir resource.

@ignore
@Manual
Scenario: case sensitive valuesets mapped correctly to resource valuesets