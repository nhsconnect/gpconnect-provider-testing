@fhir @dstu2
Feature: Fhir Base

Background:
	Given I have the following patient records
		| Id        | NHSNumber  |
		| patient1  | 9000000001 |
		| patient2  | 9000000002 |
		| patient3  | 9000000003 |
		| patient4  | 9000000004 |
		| patient5  | 9000000005 |
		| patient6  | 9000000006 |
		| patient7  | 9000000007 |
		| patient8  | 9000000008 |
		| patient9  | 9000000009 |
		| patient10 | 9000000010 |
		| patient11 | 9000000011 |
		| patient12 | 9000000012 |
		| patient13 | 9000000013 |
		| patient14 | 9000000014 |
		| patient15 | 9000000015 |

Scenario: Fhir Get MetaData
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Conformance"

Scenario: Conformance profile indicates acceptance of xml and json format
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON array "format" should contain "application/xml+fhir" or "xml"
		And the JSON array "format" should contain "application/json+fhir" or "json"

Scenario: Conformance profile suppliers software versions present
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON element "software.name" should be present
		And the JSON element "software.version" should be present

Scenario: Conformance profile supported fhir version
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "fhirVersion" should be "1.0.2"

Scenario: Conformance profile supports the gpc.getcarerecord operation
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the conformance profile should contain the "gpc.getcarerecord" operation
		
Scenario: FHIR request content type XML but no accept header or _format sent with request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the request content type to "application/xml+fhir"
		And I do not send header "Accept"
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: FHIR request content type JSON but no accept header or _format sent with request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the request content type to "application/json+fhir"
		And I do not send header "Accept"
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir content type test where Accept header is JSON and request payload is JSON
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am using "application/json+fhir" to communicate with the server
		And I author a request for the "SUM" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir content type test where Accept header is XML and request payload is XML
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am using "application/xml+fhir" to communicate with the server
		And I author a request for the "SUM" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: Fhir content type test where Accept header is JSON and request payload is XML
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the request content type to "application/xml+fhir"
		And I set the Accept header to "application/json+fhir"
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir content type test where Accept header is XML and request payload is JSON
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the request content type to "application/json+fhir"
		And I set the Accept header to "application/xml+fhir"
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: Fhir content type test where _format parameter is JSON and request payload is JSON
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the request content type to "application/json+fhir"
		And I do not send header "Accept"
		And I add the parameter "_format" with the value "application/json+fhir"
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir content type test where _format parameter is JSON and request payload is XML
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the request content type to "application/xml+fhir"
		And I do not send header "Accept"
		And I add the parameter "_format" with the value "application/json+fhir"
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir content type test where _format parameter is XML and request payload is XML
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the request content type to "application/xml+fhir"
		And I do not send header "Accept"
		And I add the parameter "_format" with the value "application/xml+fhir"
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: Fhir content type test where _format parameter is XML and request payload is JSON
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the request content type to "application/json+fhir"
		And I do not send header "Accept"
		And I add the parameter "_format" with the value "application/xml+fhir"
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: Fhir content type test where Accept header is XML and _format parameter is XML
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the request content type to "application/xml+fhir"
		And I set the Accept header to "application/xml+fhir"
		And I add the parameter "_format" with the value "application/xml+fhir"
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: Fhir content type test where Accept header is XML and _format parameter is JSON
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the request content type to "application/xml+fhir"
		And I set the Accept header to "application/xml+fhir"
		And I add the parameter "_format" with the value "application/json+fhir"
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir content type test where Accept header is JSON and _format parameter is JSON
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the request content type to "application/json+fhir"
		And I set the Accept header to "application/json+fhir"
		And I add the parameter "_format" with the value "application/json+fhir"
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir content type test where Accept header is JSON and _format parameter is XML
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the request content type to "application/json+fhir"
		And I set the Accept header to "application/json+fhir"
		And I add the parameter "_format" with the value "application/xml+fhir"
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: FHIR content type test where Invalid content type application/xml is sent
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the request content type to "application/xml"
	When I make a GET request to "/metadata"
	Then the response status code should indicate unsupported media type error
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: FHIR content type test where Invalid content type application/json is sent
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the request content type to "applicaiton/json"
	When I make a GET request to "/metadata"
	Then the response status code should indicate unsupported media type error
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: FHIR content type test where Invalid content type sent text/xml
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the request content type to "text/xml"
	When I make a GET request to "/metadata"
	Then the response status code should indicate unsupported media type error
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: Fhir content type test where Accept header is unsupported media type and request payload is JSON
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the request content type to "application/json+fhir"
		And I set the Accept header to "text/xml"
	When I make a GET request to "/metadata"
	Then the response status code should indicate unsupported media type error

Scenario: Fhir content type test where _format parameter is an unsupported media type and request payload is xml
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the request content type to "application/xml+fhir"
		And I set the Accept header to "application/xml+fhir"
		And I add the parameter "_format" with the value "text/xml"
	When I make a GET request to "/metadata"
	Then the response status code should indicate unsupported media type error

Scenario Outline: Request and response in XML
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "<Code>" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation using XML
	Then the response status code should indicate success
		And the response body should be FHIR XML
		And the response should be a Bundle resource of type "document"
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

Scenario: endpoint should support gzip compression for metadata endpoint
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I ask for the contents to be gzip encoded
	When I send a metadata request but not decompressed
	Then the response status code should indicate success
		And the response should be gzip encoded

Scenario: endpoint should support gzip compression for metadata endpoint and contain the correct payload
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I ask for the contents to be gzip encoded
	When I send a metadata request and decompressed
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Conformance"

Scenario: endpoint should support gzip compression for getCareRecord operation
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I ask for the contents to be gzip encoded
		And I author a request for the "SUM" care record section for config patient "patient2"
	When I send a gpc.getcarerecord operation request WITH payload but not decompressed
	Then the response status code should indicate success
		And the response should be gzip encoded

Scenario: endpoint should support gzip compression for getCareRecord operation and contain correct payload
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I ask for the contents to be gzip encoded
		And I author a request for the "SUM" care record section for config patient "patient2"
	When I send a gpc.getcarerecord operation request WITH payload
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		
Scenario: endpoint should support chunking of data
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "SUM" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And the response should be chunked

@ignore
@Manual
Scenario: maximum field size in fhir resource
	# String fields must not contain more than 1mb or data, this will require a test patient with data greater than 1mb a field that maps to a string field in the fhir resource.

@ignore
@Manual
Scenario: case sensitive valuesets mapped correctly to resource valuesets