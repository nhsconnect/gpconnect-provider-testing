@location
Feature: LocationRead

Scenario Outline: Location read successful request validate the response contains logical identifier
	Given I set the Get Request Id to the Logical Identifer for Location "<Location>"
	Given I configure the default "LocationRead" request
	When I make the "LocationRead" request
	Then the response status code should indicate success
		And the Response Resource should be a Location
		And the Location Id should match the GET request Id
	Examples:
		| Location |
		| SIT1     |
		| SIT2     |
		| SIT3     |

Scenario Outline: Location Read with valid identifier which does not exist on providers system
	Given I configure the default "LocationRead" request
		And I set the Read Operation logical identifier used in the request to "<LogicalId>"
	When I make the "LocationRead" request
	Then the response status code should be "404"
	Examples:
		| LogicalId   |
		| mfpBm       |
		| 231Zcr64    |
		| th.as.e     |
		| 11dd4.45-23 |
		| 40-95-3     |
		| a-tm.mss..s |

Scenario Outline: Location Read using the _format parameter to request response format
	Given I set the Get Request Id to the Logical Identifer for Location "SIT1"
	Given I configure the default "LocationRead" request
		And I add a Format parameter with the Value "<Format>"
	When I make the "LocationRead" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the Response Resource should be a Location
		And the Location Id should equal the Request Id
		And the Location Identifier should be valid for Value "SIT1"
	Examples:
		| Format	            | ResponseFormat |
		| application/fhir+json | JSON           |
		| application/fhir+xml  | XML            |

Scenario Outline: Location Read sending the Accept header and _format parameter to request response format
	Given I set the Get Request Id to the Logical Identifer for Location "SIT3"
	Given I configure the default "LocationRead" request
		And I set the Accept header to "<Header>"
		And I add a Format parameter with the Value "<Format>"
	When I make the "LocationRead" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the Response Resource should be a Location
		And the Location Id should equal the Request Id
		And the Location Identifier should be valid for Value "SIT3"
	Examples:
		| Header                | Format                | ResponseFormat |
		| application/fhir+json | application/fhir+json | JSON           |
		| application/fhir+json | application/fhir+xml  | XML            |
		| application/fhir+xml  | application/fhir+json | JSON           |
		| application/fhir+xml  | application/fhir+xml  | XML            |

Scenario: CapabilityStatement profile supports the Location read operation
	Given I configure the default "Metadataread" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement REST Resources should contain the "Location" Resource with the "Read" Interaction

Scenario Outline: Location read resource conforms to GP-Connect specification
	Given I set the Get Request Id to the Logical Identifer for Location "SIT2"
	Given I configure the default "LocationRead" request
		And I set the Accept header to "<Header>"
	When I make the "LocationRead" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be a Location
		And the Location Id should equal the Request Id
		And the Location Identifier should be valid for Value "SIT2"
		And the Location Metadata should be valid
#		status // This is checked by the FHIR .NET library
		And the Location Type should be valid
		And the Location Telecom should be valid
#		address // This is checked by the FHIR .NET library
		And the Location Physical Type should be valid
		And the Location Managing Organization should be valid
		And the Location PartOf Location should be valid
	Examples:
		| Header                | BodyFormat |
		| application/fhir+json | JSON       |
		| application/fhir+xml  | XML        |

Scenario: Location read should contain ETag
	Given I set the Get Request Id to the Logical Identifer for Location "SIT1"
	Given I configure the default "LocationRead" request
	When I make the "LocationRead" request
	Then the response status code should indicate success
		And the Response should contain the ETag header matching the Resource Version Id

Scenario: Location read valid response check caching headers exist
	Given I set the Get Request Id to the Logical Identifer for Location "SIT1"
	Given I configure the default "LocationRead" request
	When I make the "LocationRead" request
	Then the response status code should indicate success
		And the Response Resource should be a Location
		And the Location Id should match the GET request Id
		And the required cacheing headers should be present in the response

Scenario: Location read invalid response check caching headers exist
	Given I set the Get Request Id to the Logical Identifer for Location "SIT1"
	Given I configure the default "LocationRead" request
		And I set the Interaction Id header to "urn:nhs:names:servxices:gpconnect:fhir:rest:read:location3"
	When I make the "LocationRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		And the required cacheing headers should be present in the response
	