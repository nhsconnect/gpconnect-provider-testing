@ssp
Feature: SpineSecurityProxy

Scenario: SSP TraceID header not included in request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I do not send header "Ssp-TraceID"
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure

Scenario: SSP From header not included in request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I do not send header "Ssp-From"
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure

Scenario: SSP To header not included in request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I do not send header "Ssp-To"
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure

Scenario: SSP InteractionId header not included in request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I do not send header "Ssp-InteractionId"
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure

Scenario: Authorization header not included in request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I do not send header "Authorization"
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure