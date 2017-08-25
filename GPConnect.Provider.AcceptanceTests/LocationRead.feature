@location
Feature: LocationRead

Scenario Outline: Location read successful request validate the response contains logical identifier
	Given I get the Location for Location Value "<Location>"
		And I store the Location
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

Scenario Outline: Location Read with invalid resource path in URL
	Given I get the Location for Location Value "SIT1"
		And I store the Location
	Given I configure the default "LocationRead" request
		And I set the Read Operation relative path to "<RelativePath>" and append the resource logical identifier
	When I make the "LocationRead" request
	Then the response status code should be "404"
	Examples:
		| RelativePath |
		| Locationss   |
		| Location!    |
		| Location2    |
		| locations    |

Scenario Outline: Location Read with missing mandatory header
	Given I get the Location for Location Value "SIT1"
		And I store the Location
	Given I configure the default "LocationRead" request
	When I make the "LocationRead" request with missing Header "<Header>"
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Header            |
		| Ssp-TraceID       |
		| Ssp-From          |
		| Ssp-To            |
		| Ssp-InteractionId |
		| Authorization     |

Scenario Outline: Location Read with incorrect interaction id
	Given I get the Location for Location Value "SIT1"
		And I store the Location
	Given I configure the default "LocationRead" request
		And I set the Interaction Id header to "<interactionId>"
	When I make the "LocationRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| interactionId                                                     |
		| urn:nhs:names:servxices:gpconnect:fhir:rest:read:location3         |
		| urn:nhs:names:services:gpcsonnect:fhir:rest:read:locations         |
		| urn:nhs:names:xservices:gpconnect:fhir:operation:gpc.getcarerecord |
		|                                                                   |
		| null                                                              |

Scenario Outline: Location Read using the _format parameter to request response format
	Given I get the Location for Location Value "SIT1"
		And I store the Location
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
		| application/json+fhir | JSON           |
		| application/xml+fhir  | XML            |

Scenario Outline: Location Read sending the Accept header and _format parameter to request response format
	Given I get the Location for Location Value "SIT3"
		And I store the Location
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
		| application/json+fhir | application/json+fhir | JSON           |
		| application/json+fhir | application/xml+fhir  | XML            |
		| application/xml+fhir  | application/json+fhir | JSON           |
		| application/xml+fhir  | application/xml+fhir  | XML            |

Scenario: Conformance profile supports the Location read operation
	Given I configure the default "Metadataread" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the Conformance REST Resources should contain the "Location" Resource with the "Read" Interaction

Scenario Outline: Location read resource conforms to GP-Connect specification
	Given I get the Location for Location Value "SIT2"
		And I store the Location
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
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario: Location read should contain ETag
	Given I get the Location for Location Value "SIT1"
		And I store the Location
	Given I configure the default "LocationRead" request
	When I make the "LocationRead" request
	Then the response status code should indicate success
		And the Response should contain the ETag header matching the Resource Version Id

Scenario: Location read valid response check caching headers exist
		Given I get the Location for Location Value "SIT1"
		And I store the Location
	Given I configure the default "LocationRead" request
	When I make the "LocationRead" request
	Then the response status code should indicate success
		And the Response Resource should be a Location
		And the Location Id should match the GET request Id
		And the required cacheing headers should be present in the response

Scenario: Location read invalid response check caching headers exist
	Given I get the Location for Location Value "SIT1"
		And I store the Location
	Given I configure the default "LocationRead" request
		And I set the Interaction Id header to "urn:nhs:names:servxices:gpconnect:fhir:rest:read:location3"
	When I make the "LocationRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		And the required cacheing headers should be present in the response
	