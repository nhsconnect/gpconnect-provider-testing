@practitioner
Feature: PractitionerSearch

Scenario Outline: Practitioner search success
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the practitioner identifier parameter with system "http://fhir.nhs.net/Id/sds-user-id" and value "<Value>"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response bundle should contain "<EntrySize>" entries
		And the response should be a Bundle resource of type "searchset"
		And all search response entities in bundle should contain a logical identifier
		And all practitioners contain SDS identifier for practitioner "<Value>"
		And practitioner resources must contain one user id and a total of "<RoleSize>" profile ids
	Examples:
		| Value         | EntrySize | RoleSize |
		| practitioner1 | 1         | 0        |
		| practitioner2 | 1         | 1        |
		| practitioner3 | 1         | 2        |
		| practitioner4 | 0         | 0        |
		| practitioner5 | 2         | 3        |

Scenario Outline: Practitioner search with failure due to invalid identifier
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "<System>|<Value>"
	When I make a GET request to "/Practitioner"
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource
	Examples:
		| System                             | Value         |
		| http://fhir.nhs.net/Id/sds-user-id |               |
		|                                    | practitioner2 |

Scenario Outline: Practitioner search failure due to invalid system id in identifier
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "<System>|practitioner2"
	When I make a GET request to "/Practitioner"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_IDENTIFIER_SYSTEM"
	Examples:
		| System                                     |
		| http://fhir.nhs.net/Id/sds-user-id9        |
		| http://fhir.nhs.net/Id/sds-role-profile-id |
		| null                                       |

Scenario: Practitioner search without the identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
	When I make a GET request to "/Practitioner"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario Outline: Practitioner search where identifier contains the incorrect case or spelling
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "<ParameterName>" with the value "http://fhir.nhs.net/Id/sds-user-id|practitioner2"
	When I make a GET request to "/Practitioner"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource
	Examples:
		| ParameterName |
		| idenddstifier |
		| Idenddstifier |
		| Identifier    |

Scenario Outline: Practitioner search parameter order test
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "<Header1>" with the value "<Parameter1>"
		And I add the parameter "<Header2>" with the value "<Parameter2>"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| Header1    | Header2    | Parameter1                                        | Parameter2                                        | BodyFormat |
		| _format    | identifier | application/json+fhir                             | http://fhir.nhs.net/Id/sds-user-id\|practitioner2 | JSON       |
		| _format    | identifier | application/xml+fhir                              | http://fhir.nhs.net/Id/sds-user-id\|practitioner2 | XML        |
		| identifier | _format    | http://fhir.nhs.net/Id/sds-user-id\|practitioner2 | application/json+fhir                             | JSON       |
		| identifier | _format    | http://fhir.nhs.net/Id/sds-user-id\|practitioner2 | application/xml+fhir                              | XML        |

Scenario Outline: Practitioner search accept header
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the practitioner identifier parameter with system "http://fhir.nhs.net/Id/sds-user-id" and value "practitioner2"
		And I set the Accept header to "<Header>"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Practitioner search accept header and _format parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the practitioner identifier parameter with system "http://fhir.nhs.net/Id/sds-user-id" and value "practitioner2"
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/json+fhir | application/json+fhir | JSON       |
		| application/json+fhir | application/xml+fhir  | XML        |
		| application/xml+fhir  | application/json+fhir | JSON       |
		| application/xml+fhir  | application/xml+fhir  | XML        |

Scenario Outline: Practitioner search failure due to missing header
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the practitioner identifier parameter with system "http://fhir.nhs.net/Id/sds-user-id" and value "practitioner2"
		And I do not send header "<Header>"
	When I make a GET request to "/Practitioner"
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

Scenario Outline: Practitioner search failure due to invalid interactionId
	Given I am using the default server
		And I am performing the "<InteractionId>" interaction
		And I add the practitioner identifier parameter with system "http://fhir.nhs.net/Id/sds-user-id" and value "practitioner2"
	When I make a GET request to "/Practitioner"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| InteractionId                                                     |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		| InvalidInteractionId                                              |
		|                                                                   |

Scenario: Practitioner search multiple practitioners contains metadata and populated fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the practitioner identifier parameter with system "http://fhir.nhs.net/Id/sds-user-id" and value "practitioner2"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id

Scenario: Practitioner search returns back user with name element
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the practitioner identifier parameter with system "http://fhir.nhs.net/Id/sds-user-id" and value "practitioner2"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And practitioner resources should contain a single name element

Scenario: Practitioner search returns user with only one family name
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the practitioner identifier parameter with system "http://fhir.nhs.net/Id/sds-user-id" and value "practitioner2"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And practitioner should only have one family name

Scenario: Practitioner search returns practitioner role element with valid parameters
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the practitioner identifier parameter with system "http://fhir.nhs.net/Id/sds-user-id" and value "practitioner2"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And if practitionerRole has role element which contains a coding then the system, code and display must exist
		And if practitionerRole has managingOrganization element then reference must exist

Scenario: Practitioner search should not contain photo or qualification information
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the practitioner identifier parameter with system "http://fhir.nhs.net/Id/sds-user-id" and value "practitioner2"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the practitioner resource should not contain unwanted fields

Scenario: Practitioner search contains communication element
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the practitioner identifier parameter with system "http://fhir.nhs.net/Id/sds-user-id" and value "practitioner2"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And if the practitioner has communicaiton elemenets containing a coding then there must be a system, code and display element

Scenario: Practitioner search multiple identifier parameter failure
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the practitioner identifier parameter with system "http://fhir.nhs.net/Id/sds-user-id" and value "practitioner2"
		And I add the practitioner identifier parameter with system "http://fhir.nhs.net/Id/sds-user-id" and value "practitioner2"
	When I make a GET request to "/Practitioner"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Conformance profile supports the Practitioner search operation
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the conformance profile should contain the "Practitioner" resource with a "search-type" interaction