@location
Feature: LocationRead

#COMMON
#Refactor steps to use haydens code which moves set up code behind the scenes
#Put slash before URL to make it clearer it is a path
#SIT1 used throughout, think about using other sites

Scenario Outline: Location read successful request
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
#Make test name more clear
Scenario Outline: Location read invalid id
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
		#Put slash before URL to make it clearer it is a path
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
		#Put slash before URL to make it clearer it is a path
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
#Make test name clearer
Scenario Outline: Location read _format parameter only
	Given I get location "SIT1" id and save it as "location1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:location" interaction
		And I add the parameter "_format" with the value "<Parameter>"
		#add slash before URL
	When I get location "location1" and use the id to make a get request to the url "Location"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Location resource
		# We should check more in the response to make sure the resource is valid
	Examples:
		| Parameter             | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

#Make test name clearer
Scenario Outline: Location read accept header
	Given I get location "SIT1" id and save it as "location1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:location" interaction
		And I set the Accept header to "<Header>"
		#add slash before URL
	When I get location "location1" and use the id to make a get request to the url "Location"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Location resource
		# We should check more in the response to make sure the resource is valid
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

#Make test name clearer
Scenario Outline: Location read accept header and _format
	Given I get location "SIT1" id and save it as "location1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:location" interaction
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
		#add slash before URL
	When I get location "location1" and use the id to make a get request to the url "Location"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Location resource
		# We should check more in the response to make sure the resource is valid
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

Scenario Outline: Location read resource conforms to GP-Connect specification
	Given I get location "SIT1" id and save it as "location1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:location" interaction
		And I set the Accept header to "<Header>"
	#add slash before URL
	When I get location "location1" and use the id to make a get request to the url "Location"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Location resource
		And the location resource should contain meta data profile and version id
		And if the location response resource contains an identifier it is valid
#		status // This is checked by the FHIR .NET library
		And the response Location entry should contain a name element
		And if the location response contains a type element it is valid
		And if the location response contains any telecom elements they are valid
#		address // This is checked by the FHIR .NET library
		And the location response should contain valid system code and display if the PhysicalType coding is included in the resource
		And if the location response contains a managing organization it contains a valid reference
		And if the location response contains a partOf element its reference is valid
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

#Include further detail in the test name, eg matching resource version
Scenario: Read location should contain ETag
	Given I get location "SIT1" id and save it as "location1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:location" interaction
		#add slash before URL
	When I get location "location1" and use the id to make a get request to the url "Location"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Location resource
		And the response should contain the ETag header matching the resource version

Scenario: Read location If-None-Match should return a 304 on match
	Given I get location "SIT1" id and save it as "location1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:location" interaction
	When I make a GET request for location "location1" with If-None-Match header
	Then the response status code should be "304"
	
Scenario: Read location If-None-Match should return full resource if no match
	Given I get location "SIT1" id and save it as "location1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:location" interaction
		And I set the If-None-Match header to "W/\"somethingincorrect\""
	When I get location "location1" and use the id to make a get request to the url "Location"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Location resource
		And the response should contain the ETag header matching the resource version

Scenario: VRead location _history with current etag should return current location
	Given I get location "SIT1" id and save it as "location1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:location" interaction
	When I perform a location vread for location "location1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Location resource
		#Further validation to ensure the returned resource is correct

Scenario: VRead location _history with invalid etag should give a 404
	Given I get location "SIT1" id and save it as "location1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:location" interaction
	When I perform a location vread for location "location1" with invalid ETag
	Then the response status code should be "404"