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
		And the Location Id should be valid
		And the Location Identifier should be valid
		And the Location Metadata should be valid
		And the Location Physical Type should be valid
		And the Location Managing Organization should be valid
		And the Location PartOf Location should be valid
		And the Location Type should be valid
		And the Location Status should be valid
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

Scenario: Location search failure missing identifier
	Given I configure the default "LocationSearch" request
	When I make the "LocationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Location search failure due to invalid identifier name
	Given I configure the default "LocationSearch" request
		And I add a Location "<Identifier>" parameter with default System and Value "SIT1"
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
		And the Location Identifier should be valid
	Examples:
		| Parameter1Name | Parameter1                                 | Parameter2Name | Parameter2                                 | ResponseFormat |
		| _format        | application/json+fhir                      | identifier     | https://fhir.nhs.uk/Id/ods-site-code\|SIT1 | JSON           |
		| _format        | application/xml+fhir                       | identifier     | https://fhir.nhs.uk/Id/ods-site-code\|SIT1 | XML            |
		| identifier     | https://fhir.nhs.uk/Id/ods-site-code\|SIT1 | _format        | application/json+fhir                      | JSON           |
		| identifier     | https://fhir.nhs.uk/Id/ods-site-code\|SIT1 | _format        | application/xml+fhir                       | XML            |

Scenario Outline: Location Search using the accept header to request response format
	Given I configure the default "LocationSearch" request
		And I add a Location Identifier parameter with default System and Value "SIT3"
		And I set the Accept header to "<Header>"
	When I make the "LocationSearch" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "8" entries
		And the Location Id should be valid
		And the Location Metadata should be valid
		And the Location Identifier should be valid
	Examples:
		| Header                | ResponseFormat |
		| application/json+fhir | JSON           |
		| application/xml+fhir  | XML            |

Scenario Outline: Location Search using the _format parameter to request response format
	Given I configure the default "LocationSearch" request
		And I add a Location Identifier parameter with default System and Value "SIT2"
		And I add a Format parameter with the Value "<Format>"
	When I make the "LocationSearch" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "2" entries
		And the Location Id should be valid
		And the Location Metadata should be valid
		And the Location Identifier should be valid
	Examples:
		| Format                | ResponseFormat |
		| application/json+fhir | JSON           |
		| application/xml+fhir  | XML            |

Scenario Outline: Location Search using the accept header and _format parameter to request response format
	Given I configure the default "LocationSearch" request
		And I add a Location Identifier parameter with default System and Value "SIT3"
		And I set the Accept header to "<Header>"
		And I add a Format parameter with the Value "<Format>"
	When I make the "LocationSearch" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "8" entries
		And the Location Id should be valid
		And the Location Metadata should be valid
		And the Location Identifier should be valid
	Examples:
		| Header                | Format                | ResponseFormat |
		| application/json+fhir | application/json+fhir | JSON           |
		| application/json+fhir | application/xml+fhir  | XML            |
		| application/xml+fhir  | application/json+fhir | JSON           |
		| application/xml+fhir  | application/xml+fhir  | XML            |

Scenario Outline: Location search failure due to missing header
	Given I configure the default "LocationSearch" request
		And I add a Location Identifier parameter with default System and Value "SIT1"
	When I make the "LocationSearch" request with missing Header "<Header>"
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
		And I set the Interaction Id header to "<InteractionId>"
	When I make the "LocationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| InteractionId                                                    |
		| urn:nhs:namet45t45s:services:gpconnect:fhir:rest:search:location |
		| InvalidInteractionId                                             |
		|                                                                  |

Scenario: Conformance profile supports the Location search operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the Conformance REST Resources should contain the "Location" Resource with the "SearchType" Interaction

#Will fail until demonstrator is upgraded to accept local identifiers
#Also demonstrator will only accept one identifer
@ignore
Scenario: Location search send multiple identifiers in the request
	Given I configure the default "LocationSearch" request
		And I add a Location Identifier parameter with default System and Value "SIT1"
		And I add a Location Identifier parameter with local System and Value "LOC1"
	When I make the "LocationSearch" request
	Then the response status code should indicate success
		And the response should be the format FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the Location Identifier should be valid

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

Scenario: Location search include count and sort parameters
	Given I configure the default "LocationSearch" request
		And I add a Location Identifier parameter with default System and Value "SIT3"
		And I add the parameter "_count" with the value "2"
		And I add the parameter "_sort" with the value "status"
	When I make the "LocationSearch" request
	Then the response status code should indicate success
		And the response should be the format FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "2" entries

Scenario: Location search valid response check caching headers exist
	Given I configure the default "LocationSearch" request
		And I add a Location Identifier parameter with default System and Value "SIT1"
	When I make the "LocationSearch" request
	Then the response status code should indicate success
		And the response should be the format FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the required cacheing headers should be present in the response


Scenario: Location search invalid response check caching headers exist
	Given I configure the default "LocationSearch" request
	When I make the "LocationSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		And the required cacheing headers should be present in the response


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