@practitioner
Feature: PractitionerSearch

# Common
# JWT is hard coded value and it should probably be considered what JWT requested resource should be, organization but which?
# Compress successful tests into one possibly // For clarity it may be better to keep successful seperated as if one failes it is easier to see where the problem is

Scenario Outline: Practitioner search success and validate the practitioner identifiers
	Given I configure the default "PractitionerSearch" request		
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "<Value>"
	When I make the "PractitionerSearch" request
	Then the response status code should indicate success
		And the response bundle should contain "<EntrySize>" entries
		And the response should be a Bundle resource of type "searchset" 
		And all search response entities in bundle should contain a logical identifier
		And the Practitioner Identifiers should be valid fixed values
		And the Practitioner SDS User Identifier should be valid for Value "<Value>"
		And the Practitioner SDS Role Profile Identifier should be valid for "<RoleSize>" Role Profile Identifiers
	Examples:
		| Value         | EntrySize | RoleSize |
		| practitioner1 | 1         | 0        |
		| practitioner2 | 1         | 1        |
		| practitioner3 | 1         | 2        |
		| practitioner4 | 0         | 0        |
		| practitioner5 | 2         | 3        |

Scenario Outline: Practitioner search with failure due to invalid identifier
	Given I configure the default "PractitionerSearch" request
		And I add a Practitioner Identifier parameter with System "<System>" and Value "<Value>"
	When I make the "PractitionerSearch" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
	Examples:
		| System                                     | Value         |
		| http://fhir.nhs.net/Id/sds-user-id         |               |
		|                                            | practitioner2 |


Scenario Outline: Practitioner search with failure due to bad request identifier
	Given I configure the default "PractitionerSearch" request
		And I add a Practitioner Identifier parameter with System "<System>" and Value "<Value>"
	When I make the "PractitionerSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "INVALID_IDENTIFIER_SYSTEM"
	Examples:
		| System                                     | Value         |
		| http://fhir.nhs.net/Id/sds-user-id9        | practitioner2 |
		| http://fhir.nhs.net/Id/sds-role-profile-id | practitioner2 |
		| null                                       | practitioner2 |

Scenario: Practitioner search without the identifier parameter
	Given I configure the default "PractitionerSearch" request
	When I make the "PractitionerSearch" request
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Practitioner search where identifier contains the incorrect case or spelling
	Given I configure the default "PractitionerSearch" request
		And I add a Practitioner "<ParameterName>" parameter with System "http://fhir.nhs.net/Id/sds-user-id" and Value "practitioner2"
	When I make the "PractitionerSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| ParameterName |
		| idenddstifier |
		| Idenddstifier |
		| Identifier    |
		| identifiers   |

Scenario Outline: Practitioner search testing paramater validity and order sent in the request
	Given I configure the default "PractitionerSearch" request
		And I add the parameter "<Param1Name>" with the value "<Param1Value>"
		And I add the parameter "<Param2Name>" with the value "<Param2Value>"
	When I make the "PractitionerSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And the Practitioner Identifiers should be valid fixed values
		And the Practitioner PractitionerRoles Roles should be valid
		And the Practitioner PractitionerRoles ManagingOrganization should be valid and resolvable
		And the Practitioner Name should be valid
		And the Practitioner should exclude disallowed elements
		And the Practitioner Communication should be valid
	Examples:
		| Param1Name | Param1Value                                       | Param2Name | Param2Value                                       | BodyFormat |
		| _format    | application/json+fhir                             | identifier | http://fhir.nhs.net/Id/sds-user-id\|practitioner2 | JSON       |
		| _format    | application/xml+fhir                              | identifier | http://fhir.nhs.net/Id/sds-user-id\|practitioner2 | XML        |
		| identifier | http://fhir.nhs.net/Id/sds-user-id\|practitioner2 | _format    | application/json+fhir                             | JSON       |
		| identifier | http://fhir.nhs.net/Id/sds-user-id\|practitioner2 | _format    | application/xml+fhir                              | XML        |

Scenario Outline: Practitioner search add accept header to request and check for correct response format
	Given I configure the default "PractitionerSearch" request
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner2"
		And I set the Accept header to "<Header>"
	When I make the "PractitionerSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And the Practitioner Identifiers should be valid fixed values
		And the Practitioner PractitionerRoles Roles should be valid
		And the Practitioner Name should be valid
		And the Practitioner should exclude disallowed elements
		And the Practitioner Communication should be valid
		And the Practitioner PractitionerRoles ManagingOrganization should be valid and resolvable
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Practitioner search add accept header and _format parameter to the request and check for correct response format
	Given I configure the default "PractitionerSearch" request
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner2"
		And I set the Accept header to "<Header>"
		And I add a Format parameter with the Value "<Parameter>"
	When I make the "PractitionerSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And the Practitioner Identifiers should be valid fixed values
		And the Practitioner PractitionerRoles Roles should be valid
		And the Practitioner Name should be valid
		And the Practitioner should exclude disallowed elements
		And the Practitioner Communication should be valid
		And the Practitioner PractitionerRoles ManagingOrganization should be valid and resolvable
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/json+fhir | application/json+fhir | JSON       |
		| application/json+fhir | application/xml+fhir  | XML        |
		| application/xml+fhir  | application/json+fhir | JSON       |
		| application/xml+fhir  | application/xml+fhir  | XML        |

Scenario Outline: Practitioner search failure due to missing header
	Given I configure the default "PractitionerSearch" request
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner2"
		And I do not send header "<Header>"
	When I make the "PractitionerSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Header            |
		| Ssp-TraceID       |
		| Ssp-From          |
		| Ssp-To            |
		| Ssp-InteractionId |
		| Authorization     |

Scenario Outline: Practitioner search failure due to invalid interactionId
	Given I configure the default "PractitionerSearch" request
		And I am performing the "<InteractionId>" interaction
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner2"
	When I make the "PractitionerSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| InteractionId                                                     |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		| InvalidInteractionId                                              |
		|                                                                   |

Scenario: Practitioner search multiple practitioners contains metadata and populated fields
	Given I configure the default "PractitionerSearch" request
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner2"
	When I make the "PractitionerSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Practitioner Metadata should be valid

Scenario: Practitioner search returns back user with name element
	Given I configure the default "PractitionerSearch" request
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner2"
	When I make the "PractitionerSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Practitioner Name should be valid

Scenario: Practitioner search returns practitioner role element with valid parameters
	Given I configure the default "PractitionerSearch" request
		  And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner2"
	When I make the "PractitionerSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Practitioner PractitionerRoles Roles should be valid
		And the Practitioner PractitionerRoles ManagingOrganization should be valid and resolvable

Scenario: Practitioner search should not contain photo or qualification information
	Given I configure the default "PractitionerSearch" request
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner2"
	When I make the "PractitionerSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Practitioner should exclude disallowed elements

Scenario: Practitioner search contains communication element
	Given I configure the default "PractitionerSearch" request
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner2"
	When I make the "PractitionerSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Practitioner Communication should be valid

Scenario: Practitioner search multiple identifier parameter failure
	Given I configure the default "PractitionerSearch" request
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner2"
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner2"
	When I make the "PractitionerSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Practitioner search with multiple identifiers sending an invalid identifier after a valid identifier
	Given I configure the default "PractitionerSearch" request
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner2"
		And I add an Identifier parameter with the Value "INVALID SYSTEM|INVALID VALUE"
	When I make the "PractitionerSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Practitioner search with multiple identifiers sending a valid identifier after an invalid identifier
	Given I configure the default "PractitionerSearch" request
		And I add an Identifier parameter with the Value "INVALID SYSTEM|INVALID VALUE"
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner2"
	When I make the "PractitionerSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Practitioner search multiple multiple identifiers for different practitioner parameter failure
	Given I configure the default "PractitionerSearch" request
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner1"
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner2"
	When I make the "PractitionerSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Conformance profile supports the Practitioner search operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the conformance profile should contain the "Practitioner" resource with a "search-type" interaction