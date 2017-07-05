@organization
Feature: OrganizationSearch

#Common
#Add JWT organization currently uses default hardcoded value // Implemented JWT in all tests, now must remove hardcoded

Scenario Outline: Organization search success
	Given I configure the default "OrganizationSearch" request
		And I add an Organization Identifier parameter with System "<System>" and Value "<Value>"
	When I make the "OrganizationSearch" request	
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "<Entries>" entries
		And the response bundle Organization entries should contain a maximum of 1 http://fhir.nhs.net/Id/ods-organization-code system identifier
	Examples:
		| System                                       | Value      | Entries |
		| http://fhir.nhs.net/Id/ods-organization-code | unknownORG | 0       | 
		| http://fhir.nhs.net/Id/ods-organization-code | ORG1       | 1       |
		| http://fhir.nhs.net/Id/ods-organization-code | ORG2       | 1       | 
		| http://fhir.nhs.net/Id/ods-organization-code | ORG3       | 1       |
		| http://fhir.nhs.net/Id/ods-site-code         | unknownSIT | 0       |
		| http://fhir.nhs.net/Id/ods-site-code         | SIT1       | 1       |
		| http://fhir.nhs.net/Id/ods-site-code         | SIT2       | 1       |
		| http://fhir.nhs.net/Id/ods-site-code         | SIT3       | 2       |

Scenario: Organization search failure with two invalid parameters sent in the request
	Given I configure the default "OrganizationSearch" request
		And I add the parameter "incorrectParameter" with the value "incorrectParameter"
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "ORG2"
		And I add the parameter "invalidParameter" with the value "invalidParameter"
	When I make the "OrganizationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Organization search failure with invalid parameter before the identifier sent in the request
	Given I configure the default "OrganizationSearch" request
		And I add the parameter "incorrectParameter" with the value "incorrectParameter"
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "ORG2"
	When I make the "OrganizationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Organization search failure with invalid parameter after the identifier sent in the request
	Given I configure the default "OrganizationSearch" request
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "ORG2"
		And I add the parameter "invalidParameter" with the value "invalidParameter"
	When I make the "OrganizationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Organization search sending multiple identifiers resulting in failure
	Given I configure the default "OrganizationSearch" request
		And I add the organization identifier parameter with system "<System1>" and value "<Value1>"
		And I add the organization identifier parameter with system "<System2>" and value "<Value2>"
	When I make the "OrganizationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| System1                                      | Value1 | System2                                      | Value2 |
		| http://fhir.nhs.net/Id/ods-organization-code | ORG1   | http://fhir.nhs.net/Id/ods-organization-code | ORG2   |
		| http://fhir.nhs.net/Id/ods-organization-code | ORG2   | http://fhir.nhs.net/Id/ods-organization-code | ORG2   |
		| http://fhir.nhs.net/Id/ods-organization-code | ORG2   | http://fhir.nhs.net/Id/ods-site-code         | SIT2   |
		| http://fhir.nhs.net/Id/ods-site-code         | SIT1   | http://fhir.nhs.net/Id/ods-site-code         | SIT2   |
		| http://fhir.nhs.net/Id/ods-site-code         | SIT2   | http://fhir.nhs.net/Id/ods-site-code         | SIT2   |
		| http://fhir.nhs.net/Id/ods-site-code         | SIT2   | badSystem                                    | SIT2   |
		| badSystem                                    | SIT2   | http://fhir.nhs.net/Id/ods-site-code         | SIT2   |

Scenario: Organization search by organization code successfully returns single result containing the correct fields
	Given I configure the default "OrganizationSearch" request
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "ORG1"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the response bundle "Organization" entries should contain element "fullUrl"
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the "First" organization returned in the bundle is saved and given the name "organization1"
		And an organization returned in the bundle has "1" "http://fhir.nhs.net/Id/ods-organization-code" system identifier with "ORG1" and "1" "http://fhir.nhs.net/Id/ods-site-code" system identifier with site code "SIT1"
	
Scenario: Organization search by organization code successfully returns multiple results containing the correct fields
	Given I configure the default "OrganizationSearch" request
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "ORG2"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the response bundle "Organization" entries should contain element "fullUrl"
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the "First" organization returned in the bundle is saved and given the name "organization1"
			And an organization returned in the bundle has "1" "http://fhir.nhs.net/Id/ods-organization-code" system identifier with "ORG2|ORG3" and "2" "http://fhir.nhs.net/Id/ods-site-code" system identifier with site code "SIT2|SIT3"

		
Scenario: Organization search by site code successfully returns single result containing the correct fields
	Given I configure the default "OrganizationSearch" request
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "SIT1"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the response bundle "Organization" entries should contain element "fullUrl"
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And an organization returned in the bundle has "1" "http://fhir.nhs.net/Id/ods-organization-code" system identifier with "ORG1" and "1" "http://fhir.nhs.net/Id/ods-site-code" system identifier with site code "SIT1"

Scenario: Organization search by site code successfully returns multiple results containing the correct fields
	Given I configure the default "OrganizationSearch" request
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "SIT3"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "2" entries
		And the response bundle "Organization" entries should contain element "fullUrl"
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And an organization returned in the bundle has "1" "http://fhir.nhs.net/Id/ods-organization-code" system identifier with "ORG2|ORG3" and "1" "http://fhir.nhs.net/Id/ods-site-code" system identifier with site code "SIT2|SIT3"
		And an organization returned in the bundle has "1" "http://fhir.nhs.net/Id/ods-organization-code" system identifier with "ORG2|ORG3" and "2" "http://fhir.nhs.net/Id/ods-site-code" system identifier with site code "SIT2|SIT3"

Scenario Outline: Organization search failure due to invalid identifier
	Given I configure the default "OrganizationSearch" request
		And I add the parameter "identifier" with the value "<Identifier>"
	When I make the "OrganizationSearch" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
	Examples:
		| Identifier                                     |
		| GPC001                                         |
		| http://fhir.nhs.net/Id/ods-site-code           |
		| http://fhir.nhs.net/Id/ods-site-code\|         |
		| \|GPC001                                       |
		| badSystem\|ORG1                                |
		| http://fhir.nhs.net/Id/ods-organization-code\? |
		| ORG1                                           |
		| ORG2                                           |

Scenario: Organization search failure due to no identifier parameter
	Given I configure the default "OrganizationSearch" request
	When I make the "OrganizationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Organization search failure due to invalid identifier parameter name
	Given I configure the default "OrganizationSearch" request
		And I add the parameter "<Identifier>" with the value "<value>"
	When I make the "OrganizationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Identifier    | value                                              |
		| idenddstifier | http://fhir.nhs.net/Id/ods-organization-code\|ORG1 |
		| Idenddstifier | http://fhir.nhs.net/Id/ods-organization-code\|ORG1 |
		| Identifier    | http://fhir.nhs.net/Id/ods-organization-code\|ORG1 |
		| identifiers   | http://fhir.nhs.net/Id/ods-organization-code\|ORG1 |
		| idenddstifier | http://fhir.nhs.net/Id/ods-site-code\|SIT1         |
		| Idenddstifier | http://fhir.nhs.net/Id/ods-site-code\|SIT1         |
		| Identifier    | http://fhir.nhs.net/Id/ods-site-code\|SIT1         |
		| identifiers   | http://fhir.nhs.net/Id/ods-site-code\|SIT1         |           

Scenario Outline: Organization search failure due to invalid interactionId
	Given I configure the default "OrganizationSearch" request
		And I am performing the "<InteractionId>" interaction
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "ORG1"
	When I make the "OrganizationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| InteractionId                                                     |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		| InvalidInteractionId                                              |
		|                                                                   |

Scenario Outline: Organization search failure due to missing header
	Given I configure the default "OrganizationSearch" request
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "ORG1"
		And I do not send header "<Header>"
	When I make the "OrganizationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Header            |
		| Ssp-TraceID       |
		| Ssp-From          |
		| Ssp-To            |
		| Ssp-InteractionId |
		| Authorization     |


Scenario Outline: Organization search add accept header to request and check for correct response format 
	Given I configure the default "OrganizationSearch" request
		And I add the organization identifier parameter with system "<System>" and value "<Value>"
		And I set the Accept header to "<Header>"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And the "First" organization returned in the bundle is saved and given the name "organization1"
		And the stored organization "organization1" contains "1" "http://fhir.nhs.net/Id/ods-organization-code" system identifier with "organization" code "ORG1"
		And the stored organization "organization1" contains "1" "http://fhir.nhs.net/Id/ods-site-code" system identifier with "site" code "SIT1"
	Examples:
		| Header                | BodyFormat | System                                       | Value |
		| application/json+fhir | JSON       | http://fhir.nhs.net/Id/ods-organization-code | ORG1  |
		| application/xml+fhir  | XML        | http://fhir.nhs.net/Id/ods-organization-code | ORG1  |
		| application/json+fhir | JSON       | http://fhir.nhs.net/Id/ods-site-code         | SIT1  |
		| application/xml+fhir  | XML        | http://fhir.nhs.net/Id/ods-site-code         | SIT1  |

Scenario Outline: Organization search add _format parameter to request and check for correct response format 
	Given I configure the default "OrganizationSearch" request
		And I add the organization identifier parameter with system "<System>" and value "<Value>"
		And I do not send header "Accept"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
	And the response should be a Bundle resource of type "searchset"
		And the "First" organization returned in the bundle is saved and given the name "organization1"
		And the stored organization "organization1" contains "1" "http://fhir.nhs.net/Id/ods-organization-code" system identifier with "organization" code "ORG1"
		And the stored organization "organization1" contains "1" "http://fhir.nhs.net/Id/ods-site-code" system identifier with "site" code "SIT1"
	Examples:
		| Header                | Parameter             | BodyFormat | System                                       | Value |
		| application/json+fhir | application/json+fhir | JSON       | http://fhir.nhs.net/Id/ods-organization-code | ORG1  |
		| application/json+fhir | application/xml+fhir  | XML        | http://fhir.nhs.net/Id/ods-organization-code | ORG1  |
		| application/xml+fhir  | application/json+fhir | JSON       | http://fhir.nhs.net/Id/ods-site-code         | SIT1  |
		| application/xml+fhir  | application/xml+fhir  | XML        | http://fhir.nhs.net/Id/ods-site-code         | SIT1  |

Scenario Outline: Organization search add accept header and _format parameter to the request and check for correct response format 
	Given I configure the default "OrganizationSearch" request
		And I add the organization identifier parameter with system "<System>" and value "<Value>"
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the "First" organization returned in the bundle is saved and given the name "organization1"
		And the stored organization "organization1" contains "1" "http://fhir.nhs.net/Id/ods-organization-code" system identifier with "organization" code "ORG1"
		And the stored organization "organization1" contains "1" "http://fhir.nhs.net/Id/ods-site-code" system identifier with "site" code "SIT1"
	Examples:
		| Header                | Parameter             | BodyFormat | System                                       | Value |
		| application/json+fhir | application/json+fhir | JSON       | http://fhir.nhs.net/Id/ods-organization-code | ORG1  |
		| application/json+fhir | application/xml+fhir  | XML        | http://fhir.nhs.net/Id/ods-organization-code | ORG1  |
		| application/xml+fhir  | application/json+fhir | JSON       | http://fhir.nhs.net/Id/ods-site-code         | SIT1  |
		| application/xml+fhir  | application/xml+fhir  | XML        | http://fhir.nhs.net/Id/ods-site-code         | SIT1  |

Scenario Outline: Organization search add _format parameter to request before the identifer and check for correct response format 
	Given I configure the default "OrganizationSearch" request
		And I add the parameter "_format" with the value "<Parameter>"
		And I add the organization identifier parameter with system "<System>" and value "<Value>"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And the "First" organization returned in the bundle is saved and given the name "organization1"
		And the stored organization "organization1" contains "1" "http://fhir.nhs.net/Id/ods-organization-code" system identifier with "organization" code "ORG1"
		And the stored organization "organization1" contains "1" "http://fhir.nhs.net/Id/ods-site-code" system identifier with "site" code "SIT1"
	Examples:
		| Parameter             | BodyFormat | System                                       | Value |
		| application/json+fhir | JSON       | http://fhir.nhs.net/Id/ods-organization-code | ORG1  |
		| application/xml+fhir  | XML        | http://fhir.nhs.net/Id/ods-organization-code | ORG1  |
		| application/json+fhir | JSON       | http://fhir.nhs.net/Id/ods-site-code         | SIT1  |
		| application/xml+fhir  | XML        | http://fhir.nhs.net/Id/ods-site-code         | SIT1  |

Scenario Outline: Organization search add _format parameter to request after the identifer and check for correct response format 
	Given I configure the default "OrganizationSearch" request
		And I add the organization identifier parameter with system "<System>" and value "<Value>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make a GET request to "/Organization"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| Parameter             | BodyFormat | System                                       | Value |
		| application/json+fhir | JSON       | http://fhir.nhs.net/Id/ods-organization-code | ORG1  |
		| application/xml+fhir  | XML        | http://fhir.nhs.net/Id/ods-organization-code | ORG1  |
		| application/json+fhir | JSON       | http://fhir.nhs.net/Id/ods-site-code         | SIT1  |
		| application/xml+fhir  | XML        | http://fhir.nhs.net/Id/ods-site-code         | SIT1  |

Scenario: Conformance profile supports the Organization search operation
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the conformance profile should contain the "Organization" resource with a "search-type" interaction

Scenario Outline: Organization search check organization response contains logical identifier
	Given I configure the default "OrganizationSearch" request
		And I add the organization identifier parameter with system "<System>" and value "<ORGCode>"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Organization Identifiers should be valid
		Examples:
		| System                                       | ORGCode |
		| http://fhir.nhs.net/Id/ods-organization-code | ORG1    |
		| http://fhir.nhs.net/Id/ods-organization-code | ORG2    |
		| http://fhir.nhs.net/Id/ods-organization-code | ORG3    |
		| http://fhir.nhs.net/Id/ods-site-code         | SIT1    |
		| http://fhir.nhs.net/Id/ods-site-code         | SIT1    |
		| http://fhir.nhs.net/Id/ods-site-code         | SIT1    |
