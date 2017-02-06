@ssp
Feature: SpineSecurityProxy

Scenario: SSP TraceID header not included in request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I do not send header "Ssp-TraceID"
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
	And the response body should be FHIR JSON

Scenario: SSP From header not included in request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I do not send header "Ssp-From"
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
	And the response body should be FHIR JSON

Scenario: SSP To header not included in request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I do not send header "Ssp-To"
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
	And the response body should be FHIR JSON

Scenario: SSP InteractionId header not included in request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I do not send header "Ssp-InteractionId"
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
	And the response body should be FHIR JSON

Scenario: Authorization header not included in request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I do not send header "Authorization"
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
	And the response body should be FHIR JSON

Scenario: Mismatched interactionId and endpoint in request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
	And the response body should be FHIR JSON

Scenario: invalid interactionId in request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.invalidoperation" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
	And the response body should be FHIR JSON

@ignore
Scenario: Send to endpoint with incorrect To asid for the provider endpoint

