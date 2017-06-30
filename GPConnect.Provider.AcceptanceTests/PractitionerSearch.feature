@practitioner
Feature: PractitionerSearch

# Common
# JWT is hard coded value and it should probably be considered what JWT requested resource should be, organization but which?
# Compress successful tests into one possibly

# Test only really checks the identifiers so the test name should really highlight this
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
		# The following step only checks the system of the identifiers are valid, it is not clear from the name, also calls a method getPractitioners which just gets the list
		And the Practitioner Identifiers should be fixed values
		# The following step validates that there is exactly one practitioner identifier in the resource, we should probably check it is the practitioner identifier used in the search.
		And the Practitioner SDS User Identifier should be valid for single User Identifier
		# the following step also checks that role profile identifier value is populated as well as checking count, the naming is not clear and the actual code uses the odd GetPractitioners() method again.
		And the Practitioner SDS Role Profile Identifier should be valid for "<RoleSize>" total Role Profile Identifiers
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
		# The step just puts the string practitioner2 into the request, should we map the value so that it is actually testing the invalid identifier not that the identifer potentially does not exist.
		And I add the parameter "identifier" with the value "<System>|<Value>"
	When I make a GET request to "/Practitioner"
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		# Should probably specify the operation outcome INVALID_PARAMETER
		And the response should be a OperationOutcome resource
	Examples:
		| System                             | Value         |
		| http://fhir.nhs.net/Id/sds-user-id |               |
		|                                    | practitioner2 |

# Test should be combined with above test as they do the same thing
Scenario Outline: Practitioner search failure due to invalid system id in identifier
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		# practitioner2 value should be mapped as to reduce potential for false positives
		And I add the parameter "identifier" with the value "<System>|practitioner2"
	When I make a GET request to "/Practitioner"
	# Error code should be 422
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		# Operation Outcome should be INVALID_PARAMETER as invalid identifier system is supposed to be for operations where the issue is within the request resources.
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
		# Should check for BAD_REQUEST in operation outcome
		And the response should be a OperationOutcome resource

Scenario Outline: Practitioner search where identifier contains the incorrect case or spelling
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		# Should be mapping the identifier value to a valid one on the provider system to eliminate false positives
		And I add the parameter "<ParameterName>" with the value "http://fhir.nhs.net/Id/sds-user-id|practitioner2"
	When I make a GET request to "/Practitioner"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		# Should specify and BAD_REQUEST
		And the response should be a OperationOutcome resource
	Examples:
		| ParameterName |
		| idenddstifier |
		| Idenddstifier |
		| Identifier    |
		# Should add "identifiers" to test that they have not implemented using startsWith()

# Test name could be more descriptive about the parameters being valid
Scenario Outline: Practitioner search parameter order test
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "<Header1>" with the value "<Parameter1>"
		And I add the parameter "<Header2>" with the value "<Parameter2>"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		# We should probably do additional checking that the returned resources is actually valid
	# Nameing Header and Parameter are not decriptive of the actual parameter name and should be changed, the order should probably also be change to be param1name, param1value, param2name, param2value
	Examples:
		| Header1    | Header2    | Parameter1                                        | Parameter2                                        | BodyFormat |
		| _format    | identifier | application/json+fhir                             | http://fhir.nhs.net/Id/sds-user-id\|practitioner2 | JSON       |
		| _format    | identifier | application/xml+fhir                              | http://fhir.nhs.net/Id/sds-user-id\|practitioner2 | XML        |
		| identifier | _format    | http://fhir.nhs.net/Id/sds-user-id\|practitioner2 | application/json+fhir                             | JSON       |
		| identifier | _format    | http://fhir.nhs.net/Id/sds-user-id\|practitioner2 | application/xml+fhir                              | XML        |

# More descriptive name
Scenario Outline: Practitioner search accept header
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the practitioner identifier parameter with system "http://fhir.nhs.net/Id/sds-user-id" and value "practitioner2"
		And I set the Accept header to "<Header>"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		# We should probably do additional checking that the returned resources is actually valid
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

# More descriptive name
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
		# We should probably do additional checking that the returned resources is actually valid
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
		# Family name check commented out?
		And the Practitioner Name should be valid

# This should be part of the test above
Scenario: Practitioner search returns user with only one family name
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the practitioner identifier parameter with system "http://fhir.nhs.net/Id/sds-user-id" and value "practitioner2"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the Practitioner Name FamilyName should be valid

Scenario: Practitioner search returns practitioner role element with valid parameters
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the practitioner identifier parameter with system "http://fhir.nhs.net/Id/sds-user-id" and value "practitioner2"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the Practitioner PractitionerRoles Roles should be valid
		And the Practitioner PractitionerRoles ManagingOrganization should exist

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
		And the Practitioner Communication should be valid

Scenario: Practitioner search multiple identifier parameter failure
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the practitioner identifier parameter with system "http://fhir.nhs.net/Id/sds-user-id" and value "practitioner2"
		And I add the practitioner identifier parameter with system "http://fhir.nhs.net/Id/sds-user-id" and value "practitioner2"
	When I make a GET request to "/Practitioner"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

# Should probably have a test for valid and invalid identifier parameters
# Valid then invalid
# Invalid then valid

# Should probably have a test which has two valid parameters but different practitioner identifiers

Scenario: Conformance profile supports the Practitioner search operation
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the conformance profile should contain the "Practitioner" resource with a "search-type" interaction