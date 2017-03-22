@ssp
Feature: SpineSecurityProxy

Background:
	Given I have the following patient records
		| Id                 | NHSNumber  |
		| patientNotInSystem | 9999999999 |
		| patient1           | 9000000001 |
		| patient2           | 9000000002 |
		| patient3           | 9000000003 |
		| patient4           | 9000000004 |
		| patient5           | 9000000005 |
		| patient6           | 9000000006 |
		| patient7           | 9000000007 |
		| patient8           | 9000000008 |
		| patient9           | 9000000009 |
		| patient10          | 9000000010 |
		| patient11          | 9000000011 |
		| patient12          | 9000000012 |
		| patient13          | 9000000013 |
		| patient14          | 9000000014 |
		| patient15          | 9000000015 |

Scenario: SSP TraceID header not included in request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I do not send header "Ssp-TraceID"
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: SSP From header not included in request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I do not send header "Ssp-From"
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: SSP To header not included in request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I do not send header "Ssp-To"
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: SSP InteractionId header not included in request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I do not send header "Ssp-InteractionId"
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Authorization header not included in request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I do not send header "Authorization"
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Mismatched interactionId and endpoint in request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: invalid interactionId in request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.invalidoperation" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Send to endpoint with incorrect To asid for the provider endpoint
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "SUM" care record section
		And I am connecting to accredited system "123456789123"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource with error code "BAD_REQUEST"