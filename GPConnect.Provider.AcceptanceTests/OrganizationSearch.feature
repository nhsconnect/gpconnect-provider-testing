@Organization
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
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "<Entries>" entries
		And the response bundle Organization entries should contain "<OrgCodeQuantity>" "http://fhir.nhs.net/Id/ods-organization-code" system identifiers
		And the response bundle Organization entries should contain "<SiteCodeQuantity>" "http://fhir.nhs.net/Id/ods-site-code" system identifiers
	Examples:
		| System                                       | Value      | Entries | OrgCodeQuantity | SiteCodeQuantity |
		| http://fhir.nhs.net/Id/ods-organization-code | unknownORG | 0       | 0               | 0                |
		| http://fhir.nhs.net/Id/ods-organization-code | ORG1       | 1       | 1               | 1                |
		| http://fhir.nhs.net/Id/ods-organization-code | ORG2       | 1       | 1               | 2                |
		| http://fhir.nhs.net/Id/ods-organization-code | ORG3       | 1       | 1               | 1                |
		| http://fhir.nhs.net/Id/ods-site-code         | unknownSIT | 0       | 0               | 0                |
		| http://fhir.nhs.net/Id/ods-site-code         | SIT1       | 1       | 1               | 1                |
		| http://fhir.nhs.net/Id/ods-site-code         | SIT2       | 1       | 1               | 1                |
		| http://fhir.nhs.net/Id/ods-site-code         | SIT3       | 2       | 2               | 2                |
		
Scenario: Organization search failure with parameter cruft
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the parameter "ohyeah" with the value "woohoo"
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "ORG2"
		And I add the parameter "ticktock" with the value "boom"
	When I make a GET request to "/Organization"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Organization search multiple identifier parameter failure
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "ORG2"
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "ORG2"
    When I make a GET request to "/Organization"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Organization search by organization code success single result contains correct fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "ORG1"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the response bundle "Organization" entries should contain element "fullUrl"
		And the response bundle "Organization" entries should contain element "resource.meta.versionId"
		And the response bundle "Organization" entries should contain element "resource.meta.profile[0]" with value "http://fhir.nhs.net/StructureDefinition/gpconnect-organization-1"
		And the response bundle Organization entries should contain "1" "http://fhir.nhs.net/Id/ods-organization-code" system identifiers
		And the response bundle Organization entries should contain "1" "http://fhir.nhs.net/Id/ods-site-code" system identifiers
		And the response should contain ods-organization-codes "ORG1"
		And the response should contain ods-site-codes "SIT1"
		
Scenario: Organization search by organization code success multiple results contains correct fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "ORG2"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the response bundle "Organization" entries should contain element "fullUrl"
		And the response bundle "Organization" entries should contain element "resource.meta.versionId"
		And the response bundle "Organization" entries should contain element "resource.meta.profile[0]" with value "http://fhir.nhs.net/StructureDefinition/gpconnect-organization-1"
		And the response bundle Organization entries should contain "1" "http://fhir.nhs.net/Id/ods-organization-code" system identifiers
		And the response bundle Organization entries should contain "2" "http://fhir.nhs.net/Id/ods-site-code" system identifiers
		And the response should contain ods-organization-codes "ORG2"
		And the response should contain ods-site-codes "SIT2|SIT3"

Scenario: Organization search by site code success single result contains correct fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "SIT1"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the response bundle "Organization" entries should contain element "fullUrl"
		And the response bundle "Organization" entries should contain element "resource.meta.versionId"
		And the response bundle "Organization" entries should contain element "resource.meta.profile[0]" with value "http://fhir.nhs.net/StructureDefinition/gpconnect-organization-1"
		And the response bundle Organization entries should contain "1" "http://fhir.nhs.net/Id/ods-organization-code" system identifiers
		And the response bundle Organization entries should contain "1" "http://fhir.nhs.net/Id/ods-site-code" system identifiers
		And the response should contain ods-organization-codes "ORG1"
		And the response should contain ods-site-codes "SIT1"
		
Scenario: Organization search by site code success multiple results contains correct fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "SIT3"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "2" entries
		And the response bundle "Organization" entries should contain element "fullUrl"
		And the response bundle "Organization" entries should contain element "resource.meta.versionId"
		And the response bundle "Organization" entries should contain element "resource.meta.profile[0]" with value "http://fhir.nhs.net/StructureDefinition/gpconnect-organization-1"
		And the response bundle Organization entries should contain "2" "http://fhir.nhs.net/Id/ods-organization-code" system identifiers
		And the response bundle Organization entries should contain "2" "http://fhir.nhs.net/Id/ods-site-code" system identifiers
		And the response should contain ods-organization-codes "ORG2|ORG3"
		And the response should contain ods-site-codes "SIT3"
		
Scenario Outline: Organization search failure due to invalid identifier
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the parameter "identifier" with the value "<Identifier>"
	When I make a GET request to "/Organization"
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
	Examples:
		| Identifier                             |
		| GPC001                                 |
		| http://fhir.nhs.net/Id/ods-site-code   |
		| http://fhir.nhs.net/Id/ods-site-code\| |
		| \|GPC001                               |

Scenario: Organization search failure due to invalid system
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the parameter "identifier" with the value "badSystem|GPC001"
	When I make a GET request to "/Organization"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_IDENTIFIER_SYSTEM"

Scenario: Organization search failure due to no identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
	When I make a GET request to "/Organization"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario Outline: Organization search failure due to invalid identifier parameter name
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the parameter "<Identifier>" with the value "http://fhir.nhs.net/Id/ods-organization-code\|GPC001"
	When I make a GET request to "/Organization"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource
	Examples:
		| Identifier    |
		| idenddstifier | 
		| Idenddstifier | 
		| Identifier    | 

Scenario Outline: Organization search failure due to invalid interactionId
	Given I am using the default server
		And I am performing the "<InteractionId>" interaction
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "ORG1"
	When I make a GET request to "/Organization"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource
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
		And the response should be a OperationOutcome resource
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
		And the response should be a Bundle resource of type "searchset"
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
		And the response should be a Bundle resource of type "searchset"
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
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/json+fhir | application/json+fhir | JSON       |
		| application/json+fhir | application/xml+fhir  | XML        |
		| application/xml+fhir  | application/json+fhir | JSON       |
		| application/xml+fhir  | application/xml+fhir  | XML        |

Scenario: Conformance profile supports the Organization search operation
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the conformance profile should contain the "Organization" resource with a "search-type" interaction