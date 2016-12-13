@fhir @dstu2
Feature: Fhir Base

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
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: FHIR request content type JSON but no accept header or _format sent with request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the request content type to "application/json+fhir"
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir content type test where Accept header is JSON and request payload is JSON
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am using "application/json+fhir" to communicate with the server
		And I author a request for the "SUM" care record section for patient with NHS Number "9000000033"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir content type test where Accept header is XML and request payload is XML
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am using "application/xml+fhir" to communicate with the server
		And I author a request for the "SUM" care record section for patient with NHS Number "9000000033"
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

Scenario: Fhir content type test where _format parameter is JSON and request payload is XML

Scenario: Fhir content type test where _format parameter is XML and request payload is XML

Scenario: Fhir content type test where _format parameter is XML and request payload is JSON

Scenario: Fhir content type test where Accept header is XML and _format parameter is XML

Scenario: Fhir content type test where Accept header is XML and _format parameter is JSON

Scenario: Fhir content type test where Accept header is JSON and _format parameter is JSON

Scenario: Fhir content type test where Accept header is JSON and _format parameter is XML

Scenario: FHIR content type test where Invalid content type application/xml is sent
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the request content type to "application/xml"
	When I make a GET request to "/metadata"
	Then the response status code should indicate unsupported media type error

Scenario: FHIR content type test where Invalid content type application/json is sent
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the request content type to "applicaiton/json"
	When I make a GET request to "/metadata"
	Then the response status code should indicate unsupported media type error

Scenario: FHIR content type test where Invalid content type sent text/xml
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the request content type to "text/xml"
	When I make a GET request to "/metadata"
	Then the response status code should indicate unsupported media type error

Scenario: Fhir content type test where Accept header is unsupported media type and request payload is JSON
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the request content type to "application/json+fhir"
		And I set the Accept header to "text/xml"
	When I make a GET request to "/metadata"
	Then the response status code should indicate unsupported media type error

Scenario: Fhir content type test where _format parameter is an unsupported media type and request payload is xml
