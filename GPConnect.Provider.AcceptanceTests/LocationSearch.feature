Feature: LocationSearch

Background:
	Given I have the test practitioner codes
	Given I have the test ods codes

@ignore
Scenario: if location contains status elements
# There is no need to check that the location resource status element value sets are correct if included as this is done by the parse of the response within scenario above.
# The Fhir Patient object checks the values passed in are within the standard value sets as the values are mapped to an enum and throw an exception if the value does not map to a allowed value.

@ignore
Scenario: if location contains description elements
# There is no need to check that the location resource description element value sets are correct if included as this is done by the parse of the response within scenario above.
# The Fhir Patient object checks the values passed in are within the standard value sets as the values are mapped to an enum and throw an exception if the value does not map to a allowed value.

@ignore
Scenario: if location contains address
# There is no need to check that the location address value sets are valid as this is done by the parse of the response within scenario above.
# The Fhir Patient object checks the values passed in are within the standard value sets as the values are mapped to an enum and throw an exception if the value does not map to a allowed value.

@ignore
Scenario: if location contains telecom
# There is no need to check that the location telecom value sets are valid as this is done by the parse of the response within scenario above.
# The Fhir Patient object checks the values passed in are within the standard value sets as the values are mapped to an enum and throw an exception if the value does not map to a allowed value.

Scenario Outline: Location search success
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:location" interaction
		And I add the location identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "<Value>"
	When I make a GET request to "/Location"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response bundle should contain "<EntrySize>" entries
		And the response should be a Bundle resource of type "searchset"
		And all search response entities in bundle should contain a logical identifier
		And the response bundle Location entries should contain a maximum of one ODS Site Code and one other identifier
		Examples:
		| Value | EntrySize |
		| SIT1  | 1         |
		| SIT2  | 2         |
		| SIT3  | 8         |

Scenario: Location search no entrys found
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:location" interaction
		And I add the location identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "SIT4"
	When I make a GET request to "/Location"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response bundle should contain "0" entries
		And the response should be a Bundle resource of type "searchset"
		And all search response entities in bundle should contain a logical identifier
	
Scenario Outline: Location search failure invalid system
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:location" interaction
		And I add the location identifier parameter with system "<system>" and value "SIT1"
	When I make a GET request to "/Location"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_IDENTIFIER_SYSTEM"
	Examples:
		| System                                     |
		| http://fhir.nhs.net/Id/ods-site-code9      |
		| http://fhir.nhs.net/Id/sds-role-profile-id |
		| null                                       |
		|                                            |

Scenario: Location search failure missing identifier
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:location" interaction
	When I make a GET request to "/Location"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Location search failure due to invalid identifier name
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:location" interaction
		And I add the parameter "<Identifier>" with the value or sitecode "<ParameterValue>"
	When I make a GET request to "/Location"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		Examples:
		| Identifier | ParameterValue                             |
		| IDENTIFIER | http://fhir.nhs.net/Id/ods-site-code\|SIT1 |
		| identiffer | http://fhir.nhs.net/Id/ods-site-code\|SIT1 |

Scenario Outline: Location search parameter order test
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:location" interaction
		And I add the parameter "<Parameter1Name>" with the value or sitecode "<Parameter1>"
		And I add the parameter "<Parameter2Name>" with the value or sitecode "<Parameter2>"
	When I make a GET request to "/Location"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle Location entries should contain a maximum of one ODS Site Code and one other identifier
		Examples:
		| Parameter1Name | Parameter2Name | Parameter1                                 | Parameter2                                 | BodyFormat |
		| _format        | identifier     | application/json+fhir                      | http://fhir.nhs.net/Id/ods-site-code\|SIT1 | JSON       |
		| _format        | identifier     | application/xml+fhir                       | http://fhir.nhs.net/Id/ods-site-code\|SIT1 | XML        |
		| identifier     | _format        | http://fhir.nhs.net/Id/ods-site-code\|SIT1 | application/json+fhir                      | JSON       |
		| identifier     | _format        | http://fhir.nhs.net/Id/ods-site-code\|SIT1 | application/xml+fhir                       | XML        |

Scenario Outline: Location search accept header
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:location" interaction
		And I add the location identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "SIT1"
		And I set the Accept header to "<Header>"
	When I make a GET request to "/Location"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Location search _format parameter only
Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:location" interaction
		And I add the location identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "SIT2"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make a GET request to "/Location"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |


Scenario Outline: Location search accept header and _format parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:location" interaction
		And I add the location identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "SIT3"
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make a GET request to "/Location"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		Examples:
		| Header                | Parameter             | BodyFormat |
		| application/json+fhir | application/json+fhir | JSON       |
		| application/json+fhir | application/xml+fhir  | XML        |
		| application/xml+fhir  | application/json+fhir | JSON       |
		| application/xml+fhir  | application/xml+fhir  | XML        |

Scenario Outline: Location search failure due to missing header
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:location" interaction
		And I add the location identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "SIT1"
		And I do not send header "<Header>"
	When I make a GET request to "/Location"
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

Scenario Outline: Location search failure due to invalid interactionId
	Given I am using the default server
		And I am performing the "<InteractionId>" interaction
		And I add the location identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "SIT1"
	When I make a GET request to "/Location"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| InteractionId                                              |
		| urn:nhs:names:services:gpconnect:fhir:rest:search:location |
		| InvalidInteractionId                                       |
		|                                                            |

Scenario: Conformance profile supports the Location search operation
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the conformance profile should contain the "Location" resource with a "search-type" interaction

Scenario Outline: Location search locations contain metadata and populated fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:location" interaction
		And I add the location identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "<Value>"
	When I make a GET request to "/Location"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response bundle should contain "<EntrySize>" entries
		And the response should be a Bundle resource of type "searchset"
		And if the response bundle contains a location resource it should contain meta data profile and version id
		Examples:
		| Value | EntrySize |
		| SIT1  | 1         |
		| SIT2  | 2         |
		| SIT3  | 8         |

Scenario: Location search send multiple identifiers in the request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:location" interaction
		And I add the location identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "SIT1"
		And I add the location identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "SIT2"
	When I make a GET request to "/Location"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Location search send duplicate siteCodes in the request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:location" interaction
		And I add the location identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "SIT1"
		And I add the location identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "SIT1"
	When I make a GET request to "/Location"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Location search send organization identifier in the request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:location" interaction
		And I add the location identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "SIT1"
		And I add the organization identifier parameter with system "http://fhir.nhs.net/Id/ods-organization-code" and value "ORG2"
	When I make a GET request to "/Location"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Location search send invalid parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:location" interaction
		And I add the location identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "SIT1"
		And I add the parameter "additionalParam" with the value "invalidParameter"
	When I make a GET request to "/Location"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"


Scenario Outline: Location search response contains name element
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:location" interaction
		And I add the location identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "<siteCode>"
	When I make a GET request to "/Location"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle Location entries should contain a name element
		Examples: 
		| siteCode |
		| SIT1     |
		| SIT2     |
		| SIT3     |

Scenario Outline: Location search response contains correct coding if present
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:location" interaction
		And I add the location identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "<siteCode>"
	When I make a GET request to "/Location"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle location entries should contain valid system code and display if the PhysicalType coding is included in the resource
		Examples: 
		| siteCode |
		| SIT1     |
		| SIT2     |
		| SIT3     |

Scenario Outline: Location search response contains correct managing organization
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:location" interaction
		And I add the location identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "<siteCode>"
	When I make a GET request to "/Location"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And if the response bundle location entries contain managingOrganization element the reference should reference a resource in the response bundle
		Examples: 
		| siteCode |
		| SIT1     |
		| SIT2     |
		| SIT3     |

Scenario Outline: Location search response contains correct partOf
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:location" interaction
		And I add the location identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "<siteCode>"
	When I make a GET request to "/Location"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And if the response bundle location entries contain partOf element the reference should reference a resource in the response bundle
		Examples: 
		| siteCode |
		| SIT1     |
		| SIT2     |
		| SIT3     |

Scenario Outline: Location search response contains correct type
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:location" interaction
		And I add the location identifier parameter with system "http://fhir.nhs.net/Id/ods-site-code" and value "<siteCode>"
	When I make a GET request to "/Location"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle location entries should contain system code and display if the Type coding is included in the resource
		Examples: 
		| siteCode |
		| SIT1     |
		| SIT2     |
		| SIT3     |

@ignore
@Manual
Scenario: Ensure out of date site codes are no longer returning any resource
	# Run tests with a site code which has recently expired and expect an error
	
@Manual
@ignore
Scenario: Check that the optional fields are populated in the Location resource if they are available in the provider system
	# telecom - Telecom information for the Location, can be multiple instances for different types.
	# address - Address(s) for the Location.
	# contact - the details of a person, telecom or address for contact with the location.