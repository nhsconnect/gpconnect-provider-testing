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
		And the response bundle Organization entries should contain a maximum of 1 https://fhir.nhs.uk/Id/ods-organization-code system identifier
	Examples:
		| System                                       | Value      | Entries |
		| https://fhir.nhs.uk/Id/ods-organization-code | unknownORG | 0       | 
		| https://fhir.nhs.uk/Id/ods-organization-code | ORG1       | 1       |
		| https://fhir.nhs.uk/Id/ods-organization-code | ORG2       | 1       | 
		| https://fhir.nhs.uk/Id/ods-organization-code | ORG3       | 1       |
		| https://fhir.nhs.uk/Id/ods-site-code         | unknownSIT | 0       |
		| https://fhir.nhs.uk/Id/ods-site-code         | SIT1       | 1       |
		| https://fhir.nhs.uk/Id/ods-site-code         | SIT2       | 1       |
		| https://fhir.nhs.uk/Id/ods-site-code         | SIT3       | 2       |

Scenario: Organization search failure with two invalid parameters sent in the request
	Given I configure the default "OrganizationSearch" request
		And I add the parameter "incorrectParameter" with the value "incorrectParameter"
		And I add an Organization Identifier parameter with Organization Code System and Value "ORG2"
		And I add the parameter "invalidParameter" with the value "invalidParameter"
	When I make the "OrganizationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Organization search failure with invalid parameter before the identifier sent in the request
	Given I configure the default "OrganizationSearch" request
		And I add the parameter "incorrectParameter" with the value "incorrectParameter"
		And I add an Organization Identifier parameter with Organization Code System and Value "ORG2"
	When I make the "OrganizationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Organization search failure with invalid parameter after the identifier sent in the request
	Given I configure the default "OrganizationSearch" request
		And I add an Organization Identifier parameter with Organization Code System and Value "ORG2"
		And I add the parameter "invalidParameter" with the value "invalidParameter"
	When I make the "OrganizationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Organization search sending multiple identifiers resulting in failure
	Given I configure the default "OrganizationSearch" request
		And I add an Organization Identifier parameter with System "<System1>" and Value "<Value1>"
		And I add an Organization Identifier parameter with System "<System2>" and Value "<Value2>"
	When I make the "OrganizationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| System1                                      | Value1 | System2                                      | Value2 |
		| https://fhir.nhs.uk/Id/ods-organization-code | ORG1   | https://fhir.nhs.uk/Id/ods-organization-code | ORG2   |
		| https://fhir.nhs.uk/Id/ods-organization-code | ORG2   | https://fhir.nhs.uk/Id/ods-organization-code | ORG2   |
		| https://fhir.nhs.uk/Id/ods-organization-code | ORG2   | https://fhir.nhs.uk/Id/ods-site-code         | SIT2   |
		| https://fhir.nhs.uk/Id/ods-site-code         | SIT1   | https://fhir.nhs.uk/Id/ods-site-code         | SIT2   |
		| https://fhir.nhs.uk/Id/ods-site-code         | SIT2   | https://fhir.nhs.uk/Id/ods-site-code         | SIT2   |
		| https://fhir.nhs.uk/Id/ods-site-code         | SIT2   | badSystem                                    | SIT2   |
		| badSystem                                    | SIT2   | https://fhir.nhs.uk/Id/ods-site-code         | SIT2   |

Scenario: Organization search by organization code successfully returns single result containing the correct fields
	Given I configure the default "OrganizationSearch" request
		And I add an Organization Identifier parameter with Organization Code System and Value "ORG1"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Organization Full Url should be valid
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And an organization returned in the bundle has "1" "https://fhir.nhs.uk/Id/ods-organization-code" system identifier with "ORG1" and "1" "https://fhir.nhs.uk/Id/ods-site-code" system identifier with site code "SIT1"
	
Scenario Outline: Organization - Identifier - have correct Organization Codes and Site Codes when searching by Organization Code
	Given I configure the default "OrganizationSearch" request
		And I add an Organization Identifier parameter with Organization Code System and Value "<OrganizationCode>"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Organization Identifiers are correct for Organization Code "<OrganizationCode>"
	Examples: 
		| OrganizationCode |
		| ORG1             |
		| ORG2             |
		| ORG3             |

Scenario Outline: Organization - Identifier - have correct Organization Codes and Site Codes when searching by Site Code
	Given I configure the default "OrganizationSearch" request
		And I add an Organization Identifier parameter with Site Code System and Value "<SiteCode>"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Organization Identifiers are correct for Site Code "<SiteCode>"
	Examples: 
		| SiteCode | 
		| SIT1	   | 
		| SIT2	   | 
		| SIT3	   | 

Scenario: Organization search by organization code successfully returns multiple results containing the correct fields
	Given I configure the default "OrganizationSearch" request
		And I add an Organization Identifier parameter with Organization Code System and Value "ORG2"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Organization Full Url should be valid
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And an organization returned in the bundle has "1" "https://fhir.nhs.uk/Id/ods-organization-code" system identifier with "ORG2" and "2" "https://fhir.nhs.uk/Id/ods-site-code" system identifier with site code "SIT2|SIT3"
		And an organization returned in the bundle has "1" "https://fhir.nhs.uk/Id/ods-organization-code" system identifier with "ORG2|ORG3" and "2" "https://fhir.nhs.uk/Id/ods-site-code" system identifier with site code "SIT2|SIT3"
		
Scenario: Organization search by site code successfully returns single result containing the correct fields
	Given I configure the default "OrganizationSearch" request
		And I add an Organization Identifier parameter with Site Code System and Value "SIT1"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Organization Full Url should be valid
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And an organization returned in the bundle has "1" "https://fhir.nhs.uk/Id/ods-organization-code" system identifier with "ORG1" and "1" "https://fhir.nhs.uk/Id/ods-site-code" system identifier with site code "SIT1"

Scenario: Organization search by site code successfully returns multiple results containing the correct fields
	Given I configure the default "OrganizationSearch" request
		And I add an Organization Identifier parameter with Site Code System and Value "SIT3"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "2" entries
		And the Organization Full Url should be valid
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And an organization returned in the bundle has "1" "https://fhir.nhs.uk/Id/ods-organization-code" system identifier with "ORG3" and "1" "https://fhir.nhs.uk/Id/ods-site-code" system identifier with site code "SIT3"
		And an organization returned in the bundle has "1" "https://fhir.nhs.uk/Id/ods-organization-code" system identifier with "ORG2" and "2" "https://fhir.nhs.uk/Id/ods-site-code" system identifier with site code "SIT2|SIT3"

Scenario: Organization search failure due to no identifier parameter
	Given I configure the default "OrganizationSearch" request
	When I make the "OrganizationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Organization search failure due to invalid identifier parameter name
	Given I configure the default "OrganizationSearch" request
		And I add a "<Parameter>" parameter with the Value "<Value>"
	When I make the "OrganizationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Parameter     | Value                                              |
		| idenddstifier | https://fhir.nhs.uk/Id/ods-organization-code\|ORG1 |
		| Idenddstifier | https://fhir.nhs.uk/Id/ods-organization-code\|ORG1 |
		| Identifier    | https://fhir.nhs.uk/Id/ods-organization-code\|ORG1 |
		| identifiers   | https://fhir.nhs.uk/Id/ods-organization-code\|ORG1 |
		| idenddstifier | https://fhir.nhs.uk/Id/ods-site-code\|SIT1         |
		| Idenddstifier | https://fhir.nhs.uk/Id/ods-site-code\|SIT1         |
		| Identifier    | https://fhir.nhs.uk/Id/ods-site-code\|SIT1         |
		| identifiers   | https://fhir.nhs.uk/Id/ods-site-code\|SIT1         |           

Scenario Outline: Organization search failure due to invalid interactionId
	Given I configure the default "OrganizationSearch" request
		And I add an Organization Identifier parameter with Organization Code System and Value "ORG1"
		And I set the Interaction Id header to "<InteractionId>"
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
		And I add an Organization Identifier parameter with Organization Code System and Value "ORG1"
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
		And I add an Organization Identifier parameter with System "<System>" and Value "<Value>"
		And I set the Accept header to "<Header>"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
	And an organization returned in the bundle has "1" "https://fhir.nhs.uk/Id/ods-organization-code" system identifier with "ORG1" and "1" "https://fhir.nhs.uk/Id/ods-site-code" system identifier with site code "SIT1"
	Examples:
		| Header                | BodyFormat | System                                       | Value |
		| application/json+fhir | JSON       | https://fhir.nhs.uk/Id/ods-organization-code | ORG1  |
		| application/xml+fhir  | XML        | https://fhir.nhs.uk/Id/ods-organization-code | ORG1  |
		| application/json+fhir | JSON       | https://fhir.nhs.uk/Id/ods-site-code         | SIT1  |
		| application/xml+fhir  | XML        | https://fhir.nhs.uk/Id/ods-site-code         | SIT1  |

Scenario Outline: Organization search add _format parameter to request and check for correct response format 
	Given I configure the default "OrganizationSearch" request
		And I add an Organization Identifier parameter with System "<System>" and Value "<Value>"
		And I do not send header "Accept"
		And I add a Format parameter with the Value "<Format>"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
	And the response should be a Bundle resource of type "searchset"
		And an organization returned in the bundle has "1" "https://fhir.nhs.uk/Id/ods-organization-code" system identifier with "ORG1" and "1" "https://fhir.nhs.uk/Id/ods-site-code" system identifier with site code "SIT1"
	Examples:
		| Format                | BodyFormat | System                                       | Value |
		| application/json+fhir | JSON       | https://fhir.nhs.uk/Id/ods-organization-code | ORG1  |
		| application/xml+fhir  | XML        | https://fhir.nhs.uk/Id/ods-organization-code | ORG1  |
		| application/json+fhir | JSON       | https://fhir.nhs.uk/Id/ods-site-code         | SIT1  |
		| application/xml+fhir  | XML        | https://fhir.nhs.uk/Id/ods-site-code         | SIT1  |

Scenario Outline: Organization search add accept header and _format parameter to the request and check for correct response format 
	Given I configure the default "OrganizationSearch" request
		And I add an Organization Identifier parameter with System "<System>" and Value "<Value>"
		And I set the Accept header to "<Header>"
		And I add a Format parameter with the Value "<Format>"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And an organization returned in the bundle has "1" "https://fhir.nhs.uk/Id/ods-organization-code" system identifier with "ORG1" and "1" "https://fhir.nhs.uk/Id/ods-site-code" system identifier with site code "SIT1"
	Examples:
		| Header                | Format                | BodyFormat | System                                       | Value |
		| application/json+fhir | application/json+fhir | JSON       | https://fhir.nhs.uk/Id/ods-organization-code | ORG1  |
		| application/json+fhir | application/xml+fhir  | XML        | https://fhir.nhs.uk/Id/ods-organization-code | ORG1  |
		| application/xml+fhir  | application/json+fhir | JSON       | https://fhir.nhs.uk/Id/ods-site-code         | SIT1  |
		| application/xml+fhir  | application/xml+fhir  | XML        | https://fhir.nhs.uk/Id/ods-site-code         | SIT1  |

Scenario Outline: Organization search add _format parameter to request before the identifer and check for correct response format 
	Given I configure the default "OrganizationSearch" request
		And I add a Format parameter with the Value "<Format>"
		And I add an Organization Identifier parameter with System "<System>" and Value "<Value>"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And an organization returned in the bundle has "1" "https://fhir.nhs.uk/Id/ods-organization-code" system identifier with "ORG1" and "1" "https://fhir.nhs.uk/Id/ods-site-code" system identifier with site code "SIT1"
	Examples:
		| Format	            | BodyFormat | System                                       | Value |
		| application/json+fhir | JSON       | https://fhir.nhs.uk/Id/ods-organization-code | ORG1  |
		| application/xml+fhir  | XML        | https://fhir.nhs.uk/Id/ods-organization-code | ORG1  |
		| application/json+fhir | JSON       | https://fhir.nhs.uk/Id/ods-site-code         | SIT1  |
		| application/xml+fhir  | XML        | https://fhir.nhs.uk/Id/ods-site-code         | SIT1  |

Scenario Outline: Organization search add _format parameter to request after the identifer and check for correct response format 
	Given I configure the default "OrganizationSearch" request
		And I add an Organization Identifier parameter with System "<System>" and Value "<Value>"
		And I add a Format parameter with the Value "<Format>"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| Format                | BodyFormat | System                                       | Value |
		| application/json+fhir | JSON       | https://fhir.nhs.uk/Id/ods-organization-code | ORG1  |
		| application/xml+fhir  | XML        | https://fhir.nhs.uk/Id/ods-organization-code | ORG1  |
		| application/json+fhir | JSON       | https://fhir.nhs.uk/Id/ods-site-code         | SIT1  |
		| application/xml+fhir  | XML        | https://fhir.nhs.uk/Id/ods-site-code         | SIT1  |

Scenario: Conformance profile supports the Organization search operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the Conformance REST Resources should contain the "Organization" Resource with the "SearchType" Interaction

Scenario Outline: Organization search check organization response contains logical identifier
	Given I configure the default "OrganizationSearch" request
		And I add an Organization Identifier parameter with System "<System>" and Value "<Value>"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Organization Identifiers should be valid
		Examples:
		| System                                       | Value |
		| https://fhir.nhs.uk/Id/ods-organization-code | ORG1  |
		| https://fhir.nhs.uk/Id/ods-organization-code | ORG2  |
		| https://fhir.nhs.uk/Id/ods-organization-code | ORG3  |
		| https://fhir.nhs.uk/Id/ods-site-code         | SIT1  |
		| https://fhir.nhs.uk/Id/ods-site-code         | SIT2  |
		| https://fhir.nhs.uk/Id/ods-site-code         | SIT3  |

Scenario: Organization search include count and sort parameters
	Given I configure the default "OrganizationSearch" request
		And I add an Organization Identifier parameter with Organization Code System and Value "ORG1"
		And I add the parameter "_count" with the value "1"
		And I add the parameter "_sort" with the value "status"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries

Scenario: Organization search valid response check caching headers exist
	Given I configure the default "OrganizationSearch" request
		And I add an Organization Identifier parameter with Site Code System and Value "SIT1"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Organization Identifiers are correct for Site Code "SIT1"
		And the required cacheing headers should be present in the response

Scenario: Organization search invalid response check caching headers exist
	Given I configure the default "OrganizationSearch" request
		And I add the parameter "incorrectParameter" with the value "incorrectParameter"
		And I add an Organization Identifier parameter with Organization Code System and Value "ORG2"
		And I add the parameter "invalidParameter" with the value "invalidParameter"
	When I make the "OrganizationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		And the required cacheing headers should be present in the response