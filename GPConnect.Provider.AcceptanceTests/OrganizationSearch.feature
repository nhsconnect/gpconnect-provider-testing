@security
Feature: OrganizationSearch

Background:
	Given I have the test ods codes

Scenario Outline: Organization search success
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the organization identifier parameter with system "<System>" and value "<Value>"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And response bundle should contain "<ExpectedSize>" entries
	Examples:
		| System                                       | Value      | ExpectedSize |
		| http://fhir.nhs.net/Id/ods-organization-code | unknownORG | 0            |
		| http://fhir.nhs.net/Id/ods-organization-code | ORG1       | 1            |
		| http://fhir.nhs.net/Id/ods-organization-code | ORG2       | 2            |
		| http://fhir.nhs.net/Id/ods-organization-code | ORG3       | 1            |
		| http://fhir.nhs.net/Id/ods-site-code         | unknownSIT | 0            |
		| http://fhir.nhs.net/Id/ods-site-code         | SIT1       | 1            |
		| http://fhir.nhs.net/Id/ods-site-code         | SIT2       | 1            |
		| http://fhir.nhs.net/Id/ods-site-code         | SIT3       | 2            |
		
Scenario: Organization search by organization code success single result contains correct fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "ORG1"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Bundle"
		And the JSON value "type" should be "searchset"
		And response bundle should contain "1" entries
		And response bundle entry "Organization" should contain element "fullUrl"
		And response bundle entry "Organization" should contain element "resource.meta.versionId"
		And response bundle entry "Organization" should contain element "resource.meta.profile[0]" with value "http://fhir.nhs.net/StructureDefinition/gpconnect-organization-1"
		And response should contain ods-organization-codes "ORG1"
		And response should contain ods-site-codes "SIT1"
		
Scenario: Organization search by organization code success multiple results contains correct fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "ORG2"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Bundle"
		And the JSON value "type" should be "searchset"
		And response bundle should contain "2" entries
		And response bundle "Organization" entries should contain element "fullUrl"
		And response bundle "Organization" entries should contain element "resource.meta.versionId"
		And response bundle "Organization" entries should contain element "resource.meta.profile[0]" with value "http://fhir.nhs.net/StructureDefinition/gpconnect-organization-1"
		And response should contain ods-organization-codes "ORG2"
		And response should contain ods-site-codes "SIT2|SIT3"

Scenario: Organization search by site code success single result contains correct fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "SIT1"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Bundle"
		And the JSON value "type" should be "searchset"
		And response bundle should contain "1" entries
		And response bundle entry "Organization" should contain element "fullUrl"
		And response bundle entry "Organization" should contain element "resource.meta.versionId"
		And response bundle entry "Organization" should contain element "resource.meta.profile[0]" with value "http://fhir.nhs.net/StructureDefinition/gpconnect-organization-1"
		And response should contain ods-organization-codes "ORG1"
		And response should contain ods-site-codes "SIT1"
		
Scenario: Organization search by site code success multiple results contains correct fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "SIT3"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Bundle"
		And the JSON value "type" should be "searchset"
		And response bundle should contain "2" entries
		And response bundle "Organization" entries should contain element "fullUrl"
		And response bundle "Organization" entries should contain element "resource.meta.versionId"
		And response bundle "Organization" entries should contain element "resource.meta.profile[0]" with value "http://fhir.nhs.net/StructureDefinition/gpconnect-organization-1"
		And response should contain ods-organization-codes "ORG2|ORG3"
		And response should contain ods-site-codes "SIT3"

Scenario Outline: Organization search failure due to invalid identifier
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the parameter "identifier" with the value "<Identifier>"
	When I make a GET request to "/Organization"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource
	Examples:
		| Identifier                             |
		| GPC001                                 |
		| http://fhir.nhs.net/Id/ods-site-code   |
		| http://fhir.nhs.net/Id/ods-site-code\| |
		| \|GPC001                               |
		| badSystem\|GPC001                      |

Scenario: Organization search failure due to no identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
	When I make a GET request to "/Organization"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Organization search failure due to invalid identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the parameter "identifierrrrrrrrrrrr" with the value "http://fhir.nhs.net/Id/ods-organization-code\|GPC001"
	When I make a GET request to "/Organization"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Organization search failure due to invalid identifier parameter case
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the parameter "Identifier" with the value "http://fhir.nhs.net/Id/ods-organization-code\|GPC001"
	When I make a GET request to "/Organization"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario Outline: Organization search failure due to invalid interactionId
	Given I am using the default server
		And I am performing the "<InteractionId>" interaction
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "ORG1"
	When I make a GET request to "/Organization"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource
	Examples:
		| InteractionId                                                     |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		| InvalidInteractionId                                              |
		|                                                                   |
		
Scenario Outline: Organization search failure due to missing header
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "ORG1"
		And I do not send header "<Header>"
	When I make a GET request to "/Organization"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource
	Examples:
		| Header            |
		| Ssp-TraceID       |
		| Ssp-From          |
		| Ssp-To            |
		| Ssp-InteractionId |
		| Authorization     |
		
Scenario Outline: Organization search accept header
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "ORG1"
		And I set the Accept header to "<Header>"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Organization search _format parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "ORG1"
		And I do not send header "Accept"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
	Examples:
		| Parameter             | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Organization search accept header and _format parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "ORG1"
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/json+fhir | application/json+fhir | JSON       |
		| application/json+fhir | application/xml+fhir  | XML        |
		| application/xml+fhir  | application/json+fhir | JSON       |
		| application/xml+fhir  | application/xml+fhir  | XML        |