Feature: LocationRead

Background:
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

Scenario Outline: Location read successful request
	Given I get location "<Location>" id and save it as "location1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:location" interaction
	When I get location "location1" and use the id to make a get request to the url "Location"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Location resource
	Examples: 
		| Location |
		| SIT1     |
		| SIT2     |
		| SIT3     |

Scenario Outline: Location read invalid id
	Given I get location "SIT1" id and save it as "location1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:location" interaction
	When I make a GET request for a location with id "<InvalidId>"
	Then the response status code should be "404"
	Examples: 
		| InvalidId         |
		| thisIsAnInv@lidId |
		|                   |
		| null              |

Scenario Outline: Location read invalid request URL
	Given I get location "SIT1" id and save it as "location1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:location" interaction
	When I get location "location1" and use the id to make a get request to the url "<InvalidURL>"
	Then the response status code should be "404"
	Examples: 
		| InvalidURL  |
		| Locationss/ |
		| Location!/  |
		| Location2/  |

Scenario Outline: Location read failure due to missing header
	Given I get location "SIT1" id and save it as "location1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:location" interaction
		And I do not send header "<Header>"
	When I get location "location1" and use the id to make a get request to the url "Location"
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

Scenario Outline: Location read failure with incorrect interaction id
	Given I get location "SIT1" id and save it as "location1"
	Given I am using the default server
		And I am performing the "<interactionId>" interaction
	When I get location "location1" and use the id to make a get request to the url "Location"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| interactionId                                                     |
		| urn:nhs:names:services:gpconnect:fhir:rest:read:location3         |
		| urn:nhs:names:services:gpconnect:fhir:rest:read:locations         |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		|                                                                   |
		| null                                                              |

Scenario Outline: Location read _format parameter only
	Given I get location "SIT1" id and save it as "location1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:location" interaction
		And I add the parameter "_format" with the value "<Parameter>"
	When I get location "location1" and use the id to make a get request to the url "Location"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Location resource
	Examples:
        | Parameter             | BodyFormat |
        | application/json+fhir | JSON       |
        | application/xml+fhir  | XML        |

Scenario Outline: Location read accept header
	Given I get location "SIT1" id and save it as "location1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:location" interaction
		And I set the Accept header to "<Header>"
	When I get location "location1" and use the id to make a get request to the url "Location"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Location resource
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |
		
Scenario Outline: Location read accept header and _format
	Given I get location "SIT1" id and save it as "location1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:location" interaction
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I get location "location1" and use the id to make a get request to the url "Location"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Location resource
	Examples:
        | Header                | Parameter             | BodyFormat |
        | application/json+fhir | application/json+fhir | JSON       |
        | application/json+fhir | application/xml+fhir  | XML        |
        | application/xml+fhir  | application/json+fhir | JSON       |
        | application/xml+fhir  | application/xml+fhir  | XML        |

Scenario: Conformance profile supports the Location read operation
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the conformance profile should contain the "Location" resource with a "read" interaction
	
Scenario: Location read check meta data profile and version id
	Given I get location "SIT1" id and save it as "location1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:location" interaction
	When I get location "location1" and use the id to make a get request to the url "Location"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Location resource
		And the location resource should contain meta data profile and version id

Scenario: Location read contains valid identifiers
Given I get location "SIT1" id and save it as "location1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:location" interaction
	When I get location "location1" and use the id to make a get request to the url "Location"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Location resource
		And if the location response resource contains an identifier it is valid

Scenario: Location read location contains valid name element
	Given I get location "SIT1" id and save it as "location1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:location" interaction
	When I get location "location1" and use the id to make a get request to the url "Location"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Location resource
		And the response Location entry should contain a name element

Scenario: Location read resource conforms to GP-Connect specification
Given I get location "SIT1" id and save it as "location1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:location" interaction
	When I get location "location1" and use the id to make a get request to the url "Location"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Location resource
		And the location response should contain valid system code and display if the PhysicalType coding is included in the resource
		And if the location response contains a managing organization it contains a valid reference
		And if the location response contains a partOf element its reference is valid
		And if the location response contains a type element it is valid

@ignore
Scenario: If-None-Match read location on a matching version
	# Need to check if this is supported

@ignore
Scenario: If-None-Match read location on a non matching version
	# Need to check if this is supported

@Manual
@ignore
Scenario: Check that the optional fields are populated in the Location resource if they are available in the provider system
	# telecom - Telecom information for the Location, can be multiple instances for different types.
	# address - Address(s) for the Location.
	# contact - the details of a person, telecom or address for contact with the location.