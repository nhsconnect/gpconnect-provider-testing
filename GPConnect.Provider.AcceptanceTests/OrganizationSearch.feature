@security
Feature: OrganizationSearch

Background:
	Given I have the following ods codes
		| Id         | Code   |
		| unknownORG | INVOC1 |
		| ORG1       | GPC001 |
		| ORG2       | R1A14  |
		| ORG3       | R1A17  |
		| unknownSIT | INVSC1 |
		| SIT1       | Z26556 |
		| SIT2       | Z33432 |
		| SIT3		 | Z33433 |

Scenario Outline: Organization search success
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the identifier parameter with system "<System>" and value "<Value>"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And response bundle should contain "<ExpectedSize>" entries
	Examples:
		| System                                       | Value  | ExpectedSize |
		| http://fhir.nhs.net/Id/ods-organization-code | INVOC1 | 0            |
		| http://fhir.nhs.net/Id/ods-organization-code | GPC001 | 1            |
		| http://fhir.nhs.net/Id/ods-organization-code | R1A14  | 2            |
		| http://fhir.nhs.net/Id/ods-site-code         | INVSC1 | 0            |
		| http://fhir.nhs.net/Id/ods-site-code         | Z26556 | 1            |
		| http://fhir.nhs.net/Id/ods-site-code         | Z33433 | 2            |
		
Scenario: Organization search by organization code success single result contains correct fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "GPC001"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Bundle"
		And the JSON value "type" should be "searchset"
		And response bundle should contain "1" entries
		And response bundle entry "Organization" should contain element "fullUrl"
		And response bundle entry "Organization" should contain element "resource.meta.versionId"
		And response bundle entry "Organization" should contain element "resource.meta.profile[0]" with value "http://fhir.nhs.net/StructureDefinition/gpconnect-organization-1"
		And response bundle entry "Organization" should contain element "resource.identifier[?(@.system == 'http://fhir.nhs.net/Id/ods-organization-code')].value" with value "GPC001"
		And response bundle entry "Organization" should contain element "resource.identifier[?(@.system == 'http://fhir.nhs.net/Id/ods-site-code')].value" with value "Z26556"
		
Scenario: Organization search by organization code success multiple results contains correct fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "R1A14"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Bundle"
		And the JSON value "type" should be "searchset"
		And response bundle should contain "2" entries
		And response bundle "Organization" entries should contain element "fullUrl"
		And response bundle "Organization" entries should contain element "resource.meta.versionId"
		And response bundle "Organization" entries should contain element "resource.meta.profile[0]" with value "http://fhir.nhs.net/StructureDefinition/gpconnect-organization-1"
		And response bundle "Organization" entries should contain element "resource.identifier[?(@.system == 'http://fhir.nhs.net/Id/ods-organization-code')].value" with value "R1A14"
		And response bundle "Organization" entries should contain element "resource.identifier[?(@.system == 'http://fhir.nhs.net/Id/ods-site-code')].value" with values "Z33432|Z33433"

Scenario: Organization search by site code success single result contains correct fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "Z26556"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Bundle"
		And the JSON value "type" should be "searchset"
		And response bundle should contain "1" entries
		And response bundle entry "Organization" should contain element "fullUrl"
		And response bundle entry "Organization" should contain element "resource.meta.versionId"
		And response bundle entry "Organization" should contain element "resource.meta.profile[0]" with value "http://fhir.nhs.net/StructureDefinition/gpconnect-organization-1"
		And response bundle entry "Organization" should contain element "resource.identifier[?(@.system == 'http://fhir.nhs.net/Id/ods-site-code')].value" with value "Z26556"
		And response bundle entry "Organization" should contain element "resource.identifier[?(@.system == 'http://fhir.nhs.net/Id/ods-organization-code')].value" with value "GPC001"
		
Scenario: Organization search by site code success multiple results contains correct fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "Z33433"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Bundle"
		And the JSON value "type" should be "searchset"
		And response bundle should contain "2" entries
		And response bundle "Organization" entries should contain element "fullUrl"
		And response bundle "Organization" entries should contain element "resource.meta.versionId"
		And response bundle "Organization" entries should contain element "resource.meta.profile[0]" with value "http://fhir.nhs.net/StructureDefinition/gpconnect-organization-1"
		And response bundle "Organization" entries should contain element "resource.identifier[?(@.system == 'http://fhir.nhs.net/Id/ods-site-code')].value" with value "Z33433"
		And response bundle "Organization" entries should contain element "resource.identifier[?(@.system == 'http://fhir.nhs.net/Id/ods-organization-code')].value" with values "R1A14|R1A17"

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

Scenario: Organization search failure due to multiple identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "GPC001"
		And I add the identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "GPC001"
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
		And I add the identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "GPC001"
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
		And I add the identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "GPC001"
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