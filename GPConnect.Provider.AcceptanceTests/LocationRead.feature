@location
Feature: LocationRead

Scenario Outline: Location read successful request validate the response contains logical identifier
	Given I get the Location for Location Value "<Location>"
		And I store the Location Id
	Given I configure the default "LocationRead" request
	When I make the "LocationRead" request
	Then the response status code should indicate success
		And the response should be a Location resource
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
		And I store the Location Id
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
		And I store the Location Id
	Given I configure the default "LocationRead" request
		And I do not send header "<Header>"
	When I make the "LocationRead" request
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
		And I store the Location Id
	Given I configure the default "LocationRead" request
		And I am performing the "<interactionId>" interaction
	When I make the "LocationRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| interactionId                                                     |
		| urn:nhs:names:services:gpconnect:fhir:rest:read:location3         |
		| urn:nhs:names:services:gpconnect:fhir:rest:read:locations         |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		|                                                                   |
		| null                                                              |

Scenario Outline: Location Read using the _format parameter to request response format
	Given I get the Location for Location Value "SIT1"
		And I store the Location Id
	Given I configure the default "LocationRead" request
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "LocationRead" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be a Location resource
		And the returned resource shall contain a logical id matching the requested read logical identifier
		And the returned Location resource shall contain the business identifier for Location "SIT1"
	Examples:
		| Parameter             | ResponseFormat |
		| application/json+fhir | JSON           |
		| application/xml+fhir  | XML            |

Scenario Outline: Location Read sending the Accept header and _format parameter to request response format
	Given I get the Location for Location Value "SIT3"
		And I store the Location Id
	Given I configure the default "LocationRead" request
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be a Location resource
		And the returned resource shall contain a logical id matching the requested read logical identifier
		And the returned Location resource shall contain the business identifier for Location "SIT3"
	Examples:
		| Header                | Parameter             | ResponseFormat |
		| application/json+fhir | application/json+fhir | JSON           |
		| application/json+fhir | application/xml+fhir  | XML            |
		| application/xml+fhir  | application/json+fhir | JSON           |
		| application/xml+fhir  | application/xml+fhir  | XML            |

Scenario: Conformance profile supports the Location read operation
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the conformance profile should contain the "Location" resource with a "read" interaction

Scenario Outline: Location read resource conforms to GP-Connect specification
	Given I get the Location for Location Value "SIT2"
		And I store the Location Id
	Given I configure the default "LocationRead" request
		And I set the Accept header to "<Header>"
	When I make the "LocationRead" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Location resource
		And the returned resource shall contain a logical id matching the requested read logical identifier
		And the returned Location resource shall contain the business identifier for Location "SIT2"
		And the location resource should contain meta data profile and version id
		And if the location response resource contains an identifier it is valid
#		status // This is checked by the FHIR .NET library
		And the response Location entry should contain a name element
		And if the location response contains a type element coding it should contain system code and display elements
		And if the location response contains any telecom elements they are valid
#		address // This is checked by the FHIR .NET library
		And if the location response contains a PhysicalType coding it should contain system code and display elements
		And if the location response contains a managing organization it contains a reference
		And if the location response contains a partOf element its reference is valid
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario: Read location should contain ETag
	Given I get the Location for Location Value "SIT1"
		And I store the Location Id
	Given I configure the default "LocationRead" request
	When I make the "LocationRead" request
	Then the response status code should indicate success
		And the response should contain the ETag header matching the resource version

# Potentially out of scope, outstanding issue on github "https://github.com/nhsconnect/gpconnect/issues/189"
Scenario: Read location If-None-Match should return a 304 on match
	Given I get the Location for Location Value "SIT1"
		And I store the Location Id
		And I store the Location Resource
	Given I configure the default "LocationRead" request
		And I set the If-None-Match header with the version from the stored "Location" Resource
	When I make the "LocationRead" request
	Then the response status code should be "304"
	
# Potentially out of scope, outstanding issue on github "https://github.com/nhsconnect/gpconnect/issues/189"
Scenario: Read location If-None-Match should return full resource if no match
	Given I get the Location for Location Value "SIT1"
		And I store the Location Id
	Given I configure the default "LocationRead" request
		And I set the If-None-Match header to "W/\"somethingincorrect\""
	When I make the "LocationRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Location resource
		And the response should contain the ETag header matching the resource version

# Potentially out of scope, outstanding issue on github "https://github.com/nhsconnect/gpconnect/issues/189"
@ignore
Scenario: VRead location _history with current etag should return current location

# Potentially out of scope, outstanding issue on github "https://github.com/nhsconnect/gpconnect/issues/189"
@ignore
Scenario: VRead location _history with invalid etag should give a 404
