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
		And I author a request for the "SUM" care record section for patient with NHS Number "900 000 0033"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Bundle"

Scenario: Fhir Response ContentType JSON with Accept Header same as request type
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am using "application/json+fhir" to communicate with the server
		And I author a request for the "SUM" care record section for patient with NHS Number "900 000 0033"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir Response ContentType XML with Accept Header same as request type
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am using "application/xml+fhir" to communicate with the server
		And I author a request for the "SUM" care record section for patient with NHS Number "900 000 0033"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR XML
