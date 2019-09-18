@fhir
Feature: FHIR

@1.2.4
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
@1.2.4
Scenario: CapabilityStatement profile supports the RegisterPatient operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement REST Operations should contain "gpc.registerpatient"

#132 - RMB 29/10/2018
#292 - PG 30/8/2019 - added check for correct url on operation
@1.2.4
Scenario: CapabilityStatement profile supports the GetStructuredRecord operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
	And the CapabilityStatement REST Operations should contain "gpc.getstructuredrecord"
	And the CapabilityStatement Operation "gpc.getstructuredrecord" has url "https://fhir.nhs.uk/STU3/OperationDefinition/GPConnect-GetStructuredRecord-Operation-1/_history/1.9" 
	
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
