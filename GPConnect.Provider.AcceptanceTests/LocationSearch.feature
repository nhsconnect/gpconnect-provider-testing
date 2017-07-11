@location
Feature: LocationSearch

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
	Given I configure the default "LocationSearch" request
		And I add a Location Identifier parameter with default System and Value "<Value>"
	When I make the "LocationSearch" request
	Then the response status code should indicate success
		And the response should be the format FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "<EntrySize>" entries
		And all search response entities in bundle should contain a logical identifier
		And if the response bundle contains a location resource it should contain meta data profile and version id
		And the response bundle Location entries should contain a maximum of one ODS Site Code and one other identifier
		And the response bundle Location entries should contain a name element
		And the response bundle location entries should contain valid system code and display if the PhysicalType coding is included in the resource
		And if the response bundle location entries contain managingOrganization element the reference should exist
		And if the response bundle location entries contain partOf element the reference should exist
		And the response bundle location entries should contain system code and display if the Type coding is included in the resource
	Examples:
		| Value | EntrySize |
		| SIT1  | 1         |
		| SIT2  | 2         |
		| SIT3  | 8         |

Scenario: Location search no entrys found
	Given I configure the default "LocationSearch" request
		And I add a Location Identifier parameter with default System and Value "SIT4"
	When I make the "LocationSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "0" entries
	
Scenario Outline: Location search failure invalid system
	Given I configure the default "LocationSearch" request
		And I add a Location Identifier parameter with System "<System>" and Value "SIT1"
	When I make the "LocationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "INVALID_IDENTIFIER_SYSTEM"
	Examples:
		| System                                         |
		| http://fhir.nhs.net/Id/ods-site-code9          |
		| http://fhir.nhs.net/Id/sds-role-profile-id     |
		| http://fhir.nh5555555555555555555/555555555555 |

Scenario: Location search failure missing identifier
	Given I configure the default "LocationSearch" request
	When I make the "LocationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Location search failure due to invalid identifier name
	Given I configure the default "LocationSearch" request
		And I add a Location Identifier parameter with parameter name "<Identifier>" and Value "SIT1"
	When I make the "LocationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Identifier  |
		| IDENTIFIER  |
		| identiffer  |
		| identifiers |

Scenario Outline: Location search parameter order test
	Given I configure the default "LocationSearch" request
		And I add the parameter "<Parameter1Name>" with the value or sitecode "<Parameter1>"
		And I add the parameter "<Parameter2Name>" with the value or sitecode "<Parameter2>"
	When I make the "LocationSearch" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle Location entries should contain a maximum of one ODS Site Code and one other identifier
	Examples:
		| Parameter1Name | Parameter1                                 | Parameter2Name | Parameter2                                 | ResponseFormat |
		| _format        | application/json+fhir                      | identifier     | http://fhir.nhs.net/Id/ods-site-code\|SIT1 | JSON           |
		| _format        | application/xml+fhir                       | identifier     | http://fhir.nhs.net/Id/ods-site-code\|SIT1 | XML            |
		| identifier     | http://fhir.nhs.net/Id/ods-site-code\|SIT1 | _format        | application/json+fhir                      | JSON           |
		| identifier     | http://fhir.nhs.net/Id/ods-site-code\|SIT1 | _format        | application/xml+fhir                       | XML            |

Scenario Outline: Location Search using the accept header to request response format
	Given I configure the default "LocationSearch" request
		And I add a Location Identifier parameter with default System and Value "SIT3"
		And I set the Accept header to "<Header>"
	When I make the "LocationSearch" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be a Bundle resource of type "searchset"
		And all search response entities in bundle should contain a logical identifier
		And if the response bundle contains a location resource it should contain meta data profile and version id
		And the response bundle Location entries should contain a maximum of one ODS Site Code and one other identifier
		And the response bundle Location entries should contain a name element
		And the response bundle should contain "8" entries
	Examples:
		| Header                | ResponseFormat |
		| application/json+fhir | JSON           |
		| application/xml+fhir  | XML            |

Scenario Outline: Location Search using the _format parameter to request response format
	Given I configure the default "LocationSearch" request
		And I add a Location Identifier parameter with default System and Value "SIT2"
		And I add the parameter "_format" with the value "<parameter>"
	When I make the "LocationSearch" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be a Bundle resource of type "searchset"
		And all search response entities in bundle should contain a logical identifier
		And if the response bundle contains a location resource it should contain meta data profile and version id
		And the response bundle Location entries should contain a maximum of one ODS Site Code and one other identifier
		And the response bundle Location entries should contain a name element
		And the response bundle should contain "2" entries
	Examples:
		| parameter             | ResponseFormat |
		| application/json+fhir | JSON           |
		| application/xml+fhir  | XML            |

Scenario Outline: Location Search using the accept header and _format parameter to request response format
	Given I configure the default "LocationSearch" request
		And I add a Location Identifier parameter with default System and Value "SIT3"
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "LocationSearch" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be a Bundle resource of type "searchset"
		And all search response entities in bundle should contain a logical identifier
		And if the response bundle contains a location resource it should contain meta data profile and version id
		And the response bundle Location entries should contain a maximum of one ODS Site Code and one other identifier
		And the response bundle Location entries should contain a name element
		And the response bundle should contain "8" entries
	Examples:
		| Header                | Parameter             | ResponseFormat |
		| application/json+fhir | application/json+fhir | JSON           |
		| application/json+fhir | application/xml+fhir  | XML            |
		| application/xml+fhir  | application/json+fhir | JSON           |
		| application/xml+fhir  | application/xml+fhir  | XML            |

Scenario Outline: Location search failure due to missing header
	Given I configure the default "LocationSearch" request
		And I add a Location Identifier parameter with default System and Value "SIT1"
		And I do not send header "<Header>"
	When I make the "LocationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Header            |
		| Ssp-TraceID       |
		| Ssp-From          |
		| Ssp-To            |
		| Ssp-InteractionId |
		| Authorization     |

Scenario Outline: Location search failure due to invalid interactionId
	Given I configure the default "LocationSearch" request
		And I add a Location Identifier parameter with default System and Value "SIT1"
		And I am performing the "<InteractionId>" interaction
	When I make the "LocationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| InteractionId                                                    |
		| urn:nhs:namet45t45s:services:gpconnect:fhir:rest:search:location |
		| InvalidInteractionId                                             |
		|                                                                  |

Scenario: Conformance profile supports the Location search operation
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the conformance profile should contain the "Location" resource with a "search-type" interaction

Scenario: Location search send multiple identifiers in the request
	Given I configure the default "LocationSearch" request
		And I add a Location Identifier parameter with default System and Value "SIT1"
		And I add a Location Identifier parameter with default System and Value "SIT2"
	When I make the "LocationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Location search send duplicate siteCodes in the request
	Given I configure the default "LocationSearch" request
		And I add a Location Identifier parameter with default System and Value "SIT1"
		And I add a Location Identifier parameter with default System and Value "SIT1"
	When I make the "LocationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Location search send site identifier and organization identifier in the request
	Given I configure the default "LocationSearch" request
		And I add a Location Identifier parameter with default System and Value "SIT1"
		And I add an Organization Identifier parameter with Organization Code System and Value "ORG2"
	When I make the "LocationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Location search send organization identifier in the request
	Given I configure the default "LocationSearch" request
		And I add an Organization Identifier parameter with Organization Code System and Value "ORG2"
	When I make the "LocationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "INVALID_IDENTIFIER_SYSTEM"

Scenario: Location search send additional invalid parameter
	Given I configure the default "LocationSearch" request
		And I add a Location Identifier parameter with default System and Value "SIT1"
		And I add the parameter "additionalParam" with the value "invalidParameter"
	When I make the "LocationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

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