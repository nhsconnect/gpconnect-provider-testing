@ssp
Feature: SpineSecurityProxy

Background:
	Given I have the test patient codes

Scenario: SSP TraceID header not included in request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I do not send header "Ssp-TraceID"
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: SSP From header not included in request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I do not send header "Ssp-From"
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: SSP To header not included in request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I do not send header "Ssp-To"
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: SSP InteractionId header not included in request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I do not send header "Ssp-InteractionId"
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: Authorization header not included in request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I do not send header "Authorization"
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: Mismatched interactionId and endpoint in request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: invalid interactionId in request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.invalidoperation" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: Send to endpoint with incorrect To asid for the provider endpoint
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "SUM" care record section
		And I am connecting to accredited system "123456789123"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"