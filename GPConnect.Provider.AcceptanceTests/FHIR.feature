Feature: Fhir

Scenario: Fhir Get MetaData
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Conformance"

Scenario: Fhir Retrieve the care record for a patient
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "SUM" care record section for patient with NHS Number "9000000033"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Bundle"

Scenario Outline: Fhir Retrieve the care record sectons for a patient
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for patient with NHS Number "9000000033"
		And I am requesting the "<Code>" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Bundle"

Examples:
	| Code |
	| ADM |
	| ALL |
	| CLI |
	| IMM |
	| INV |
	| MED |
	| OBS |
	| PAT |
	| PRB |
	| REF |
	| SUM |

Scenario: Fhir Response ContentType JSON with Accept Header same as request type
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am using "application/json+fhir" to communicate with the server
		And I author a request for the "SUM" care record section for patient with NHS Number "9000000033"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir Response ContentType XML with Accept Header same as request type
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am using "application/xml+fhir" to communicate with the server
		And I author a request for the "SUM" care record section for patient with NHS Number "9000000033"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR XML

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
