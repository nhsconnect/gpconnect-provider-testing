@fhir
Feature: FHIR

Scenario: Fhir Get MetaData
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the Response Resource should be a CapabilityStatement
		And the CapabilityStatement version should match the GP Connect specification release

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

# github ref 132
# RMB 29/10/2018
Scenario: CapabilityStatement profile supported rp operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement REST Operations should contain "gpc.registerpatient"

# github ref 132
# RMB 29/10/2018
Scenario: CapabilityStatement profile supported sr operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement REST Operations should contain "gpc.getstructuredrecord"

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

Scenario: endpoint should support gzip compression for metadata endpoint and contain the correct payload
	Given I configure the default "MetadataRead" request
		And I set the Accept-Encoding header to gzip
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response should be gzip encoded
		And the response body should be FHIR JSON
		And the Response Resource should be a CapabilityStatement

@ignore
@Manual
Scenario: maximum field size in fhir resource
	# String fields must not contain more than 1mb or data, this will require a test patient with data greater than 1mb a field that maps to a string field in the fhir resource.

@ignore
@Manual
Scenario: case sensitive valuesets mapped correctly to resource valuesets