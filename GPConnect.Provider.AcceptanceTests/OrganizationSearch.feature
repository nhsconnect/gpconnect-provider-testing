@organization
Feature: OrganizationSearch

Scenario Outline: Organization search success
	Given I configure the default "OrganizationSearch" request
		And I add an Organization Identifier parameter with System "<System>" and Value "<Value>"
	When I make the "OrganizationSearch" request	
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "<Entries>" entries
		And the response bundle Organization entries should contain a maximum of 1 "https://fhir.nhs.uk/Id/ods-organization-code" system identifier
		And the Organization Id should be valid
		And the Organization Name should be valid
		And the Organization Telecom should be valid
		And the Organization Address should be valid
		And the Organization Contact should be valid
		And the Organization Extensions should be valid
	Examples:
		| System                                       | Value      | Entries |
		| https://fhir.nhs.uk/Id/ods-organization-code | unknownORG | 0       | 
		| https://fhir.nhs.uk/Id/ods-organization-code | ORG1       | 1       |

Scenario: Organization search sending multiple identifiers resulting in failure
	Given I configure the default "OrganizationSearch" request		
		And I add an Organization Identifier parameter with System "https://fhir.nhs.uk/Id/ods-organization-code" and Value "ORG1"
		And I add an Organization Identifier parameter with System "https://fhir.nhs.uk/Id/ods-organization-code" and Value "ORG1"
	When I make the "OrganizationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Organization search by organization code successfully returns single result containing the correct fields
	Given I configure the default "OrganizationSearch" request		
		And I add an Organization Identifier parameter with Organization Code System and Value "ORG1"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Organization Full Url should be valid
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And an organization returned in the bundle has "1" "https://fhir.nhs.uk/Id/ods-organization-code" system identifier with "ORG1"

Scenario: Organization search by organization code successfully returns multiple results containing the correct fields
	Given I configure the default "OrganizationSearch" request		
		And I add an Organization Identifier parameter with Organization Code System and Value "ORG1"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Organization Full Url should be valid
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And an organization returned in the bundle has "1" "https://fhir.nhs.uk/Id/ods-organization-code" system identifier with "ORG1"
		
Scenario: Organization search failure due to no identifier parameter
	Given I configure the default "OrganizationSearch" request		
	When I make the "OrganizationSearch" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario Outline: Organization search add accept header to request and check for correct response format 
	Given I configure the default "OrganizationSearch" request		
		And I add an Organization Identifier parameter with System "<System>" and Value "ORG1"
		And I set the Accept header to "<Header>"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
	And an organization returned in the bundle has "1" "https://fhir.nhs.uk/Id/ods-organization-code" system identifier with "ORG1"
	Examples:
		| Header                | BodyFormat | System                                       |
		| application/fhir+json | JSON       | https://fhir.nhs.uk/Id/ods-organization-code |
		| application/fhir+xml  | XML        | https://fhir.nhs.uk/Id/ods-organization-code |

Scenario Outline: Organization search add _format parameter to request and check for correct response format 
	Given I configure the default "OrganizationSearch" request		
		And I add an Organization Identifier parameter with System "<System>" and Value "ORG1"
		And I do not send header "Accept"
		And I add a Format parameter with the Value "<Format>"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And an organization returned in the bundle has "1" "https://fhir.nhs.uk/Id/ods-organization-code" system identifier with "ORG1"
	Examples: 
		| Format                | BodyFormat | System                                       | 
		| application/fhir+json | JSON       | https://fhir.nhs.uk/Id/ods-organization-code | 
		| application/fhir+xml  | XML        | https://fhir.nhs.uk/Id/ods-organization-code |

Scenario Outline: Organization search add accept header and _format parameter to the request and check for correct response format 
	Given I configure the default "OrganizationSearch" request		
		And I add an Organization Identifier parameter with System "<System>" and Value "ORG1"
		And I set the Accept header to "<Header>"
		And I add a Format parameter with the Value "<Format>"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And an organization returned in the bundle has "1" "https://fhir.nhs.uk/Id/ods-organization-code" system identifier with "ORG1"
	Examples:
		| Header                | Format                | BodyFormat | System                                       |
		| application/fhir+json | application/fhir+json | JSON       | https://fhir.nhs.uk/Id/ods-organization-code |
		| application/fhir+json | application/fhir+xml  | XML        | https://fhir.nhs.uk/Id/ods-organization-code |

Scenario Outline: Organization search add _format parameter to request before the identifer and check for correct response format 
	Given I configure the default "OrganizationSearch" request		
		And I add a Format parameter with the Value "<Format>"
		And I add an Organization Identifier parameter with System "<System>" and Value "ORG1"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And an organization returned in the bundle has "1" "https://fhir.nhs.uk/Id/ods-organization-code" system identifier with "ORG1"
	Examples:
		| Format	            | BodyFormat | System                                       |
		| application/fhir+json | JSON       | https://fhir.nhs.uk/Id/ods-organization-code |
		| application/fhir+xml  | XML        | https://fhir.nhs.uk/Id/ods-organization-code |
		
Scenario Outline: Organization search add _format parameter to request after the identifer and check for correct response format 
	Given I configure the default "OrganizationSearch" request	
		And I add an Organization Identifier parameter with System "<System>" and Value "ORG1"
		And I add a Format parameter with the Value "<Format>"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| Format                | BodyFormat | System                                       |
		| application/fhir+json | JSON       | https://fhir.nhs.uk/Id/ods-organization-code |
		| application/fhir+xml  | XML        | https://fhir.nhs.uk/Id/ods-organization-code |

Scenario: CapabilityStatement profile supports the Organization search operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement REST Resources should contain the "Organization" Resource with the "SearchType" Interaction

Scenario: Organization search check organization response contains logical identifier
	Given I configure the default "OrganizationSearch" request		
		And I add an Organization Identifier parameter with System "https://fhir.nhs.uk/Id/ods-organization-code" and Value "ORG1"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Organization Identifiers should be valid

Scenario: Organization search include count and sort parameters
	Given I configure the default "OrganizationSearch" request
		And I add an Organization Identifier parameter with Organization Code System and Value "ORG1"
		And I add the parameter "_count" with the value "1"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries				

Scenario: Organization search valid response check caching headers exist
	Given I configure the default "OrganizationSearch" request		
		And I add an Organization Identifier parameter with Organization Code System and Value "ORG1"
	When I make the "OrganizationSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Organization Full Url should be valid
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And an organization returned in the bundle has "1" "https://fhir.nhs.uk/Id/ods-organization-code" system identifier with "ORG1"
		And the required cacheing headers should be present in the response
