@organization
Feature: OrganizationSearch

#Common
#Add JWT organization currently uses default hardcoded value
#Come up with standard error param names and values

Scenario Outline: Organization search success
	Given I configure the default "OrganizationSearch" request
		And I add an Identifier parameter with System "<System>" and Value "<Value>"
	When I make the "OrganizationSearch" request	
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "<Entries>" entries
		And the response bundle Organization entries should contain a maximum of 1 http://fhir.nhs.net/Id/ods-organization-code system identifier
		#Remove below steps due to testing in other scenarios and the steps are not very targeted
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
		| http://fhir.nhs.net/Id/ods-site-code         | SIT2       | 1       | 1               | 2                |
		| http://fhir.nhs.net/Id/ods-site-code         | SIT3       | 2       | 2               | 3                |

Scenario: Organization search failure with parameter cruft
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		#Change the parameter names 
		And I add the parameter "ohyeah" with the value "woohoo"
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "ORG2"
		And I add the parameter "invalidParam" with the value "notValid"
	When I make a GET request to "/Organization"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

#Add further scenarios to test veriety of invalid parameters
#Valid identifier + invalid only
#Invalid + valid

#Change scenario name to make more clear, multiple valid identifiers
Scenario Outline: Organization search multiple identifier parameter failure
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the organization identifier parameter with system "<System1>" and value "<Value1>"
		And I add the organization identifier parameter with system "<System2>" and value "<Value2>"
	When I make a GET request to "/Organization"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
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
#Make name clearer
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
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		# Change steps to validate individual resources rather then bundle as a whole
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
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		# Change steps to validate individual resources rather then bundle as a whole
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
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		# Change steps to validate individual resources rather then bundle as a whole
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
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		# Change steps to validate individual resources rather then bundle as a whole
		And the response bundle Organization entries should contain "2" "http://fhir.nhs.net/Id/ods-organization-code" system identifiers
		And the response bundle Organization entries should contain "3" "http://fhir.nhs.net/Id/ods-site-code" system identifiers
		And the response should contain ods-organization-codes "ORG2|ORG3"
		And the response should contain ods-site-codes "SIT2|SIT3"

#Map identifier value to site code
#Add organization codes to the test variables 
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

#Merge value with above test, remove test scenario as error code is invalid
Scenario: Organization search failure due to invalid system
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
		And I add the parameter "identifier" with the value "badSystem|GPC001"
	When I make a GET request to "/Organization"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_IDENTIFIER_SYSTEM"

#Check for bad request in operation outcome
Scenario: Organization search failure due to no identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
	When I make a GET request to "/Organization"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

#Add identifiers to the variables to test
#Add site identifier to test and map to identifier value
#Add bad request to operation outcome
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
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
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
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Header            |
		| Ssp-TraceID       |
		| Ssp-From          |
		| Ssp-To            |
		| Ssp-InteractionId |
		| Authorization     |

#Update name of test to make more clear
#Add searching of site codes
#Check the response is containg the correct site/org information
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

#Update name of test to make more clear
#Add searching of site codes
#Check the response is containg the correct site/org information
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

#Update name of test to make more clear
#Add searching of site codes
#Replace validation with more detailed validation
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
		And the response bundle should contain "1" entries
		And the response bundle Organization entries should contain "1" "http://fhir.nhs.net/Id/ods-organization-code" system identifiers
		And the response bundle Organization entries should contain "1" "http://fhir.nhs.net/Id/ods-site-code" system identifiers
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


# Add a test validating that all organizations resources should contain a logical identifier
# Should add test where we change the order of the identifier parameter and the _format parameter
