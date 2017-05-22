Feature: PractitionerRead

Background:
	Given I have the test patient codes
	Given I have the test practitioner codes
	Given I have the test ods codes

Scenario: Practitioner read successful request
	Given I get "practitioner1" id and save it as "practitioner1Id"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I get "practitioner1Id" id then make a GET request to "Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Practitioner resource

Scenario Outline: Practitioner read invalid request invalid id
	Given I get "practitioner1" id and save it as "practitioner1Id"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I make a GET request for a practitioner using an invalid id of "<InvalidId>" and url "Practitioner"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		Examples: 
		| InvalidId |
		| ##        |
		| 1@        |
		| 9i        |
		| 40-9      |

Scenario Outline: Practitioner read invalid request invalid URL
	Given I get "practitioner1" id and save it as "practitioner1Id"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I get "practitioner1Id" id then make a GET request to "<InvalidURL>"
	Then the response status code should be "404"
		Examples: 
		| InvalidURL      |
		| Practitioners |
		| Practitioner! |
		| Practitioner2 |
		
Scenario Outline: Practitioner read failure due to missing header
	Given I get "practitioner1" id and save it as "practitioner1Id"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
		And I do not send header "<Header>"
	When I get "practitioner1Id" id then make a GET request to "Practitioner"
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

Scenario Outline: Practitioner read failure with incorrect interaction id
	Given I get "practitioner1" id and save it as "practitioner1Id"
	Given I am using the default server
		And I am performing the "<interactionId>" interaction
	When I get "practitioner1Id" id then make a GET request to "Practitioner"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		Examples:
		  | interactionId                                                     |
		  | urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner3     |
		  | urn:nhs:names:services:gpconnect:fhir:rest:read:practitioners     |
		  | urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		  |                                                                   |
		  | null                                                              |

Scenario Outline: Practitioner read _format parameter only
	Given I get "practitioner1" id and save it as "practitioner1Id"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
		And I add the parameter "_format" with the value "<Parameter>"
	When I get "practitioner1Id" id then make a GET request to "Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Practitioner resource
		Examples:
        | Parameter             | BodyFormat |
        | application/json+fhir | JSON       |
        | application/xml+fhir  | XML        |

Scenario Outline: Practitioner read accept header and _format
	Given I get "practitioner1" id and save it as "practitioner1Id"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I get "practitioner1Id" id then make a GET request to "Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Practitioner resource
		  Examples:
        | Header                | Parameter             | BodyFormat |
        | application/json+fhir | application/json+fhir | JSON       |
        | application/json+fhir | application/xml+fhir  | XML        |
        | application/xml+fhir  | application/json+fhir | JSON       |
        | application/xml+fhir  | application/xml+fhir  | XML        |

Scenario: Conformance profile supports the Practitioner read operation
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the conformance profile should contain the "Practitioner" resource with a "read" interaction

Scenario: Practitioner read check meta data profile and version id
	Given I get "practitioner1" id and save it as "practitioner1Id"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I get "practitioner1Id" id then make a GET request to "Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Practitioner resource
		And the practitioner resource it should contain meta data profile and version id

Scenario: Practitioner read practitioner contains single name element
	Given I get "practitioner1" id and save it as "practitioner1Id"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I get "practitioner1Id" id then make a GET request to "Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Practitioner resource
		And the practitioner resource should contain a single name element

Scenario: Practitioner read practitioner contains identifier it is valid
	Given I get "practitioner1" id and save it as "practitioner1Id"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I get "practitioner1Id" id then make a GET request to "Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Practitioner resource
		And if the practitioner resource contains an identifier it is valid

Scenario: Practitioner read practitioner contains practitioner role it is valid
	Given I get "practitioner1" id and save it as "practitioner1Id"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I get "practitioner1Id" id then make a GET request to "Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Practitioner resource
		And if the practitioner resource contains a practitioner role it has a valid coding and system

Scenario: Practitioner read practitioner contains communication which is valid
	Given I get "practitioner1" id and save it as "practitioner1Id"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I get "practitioner1Id" id then make a GET request to "Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Practitioner resource
		And the returned practitioner resource contains a communication element

Scenario: Practitioner read practitioner returned does not contain elements not in the specification
	Given I get "practitioner1" id and save it as "practitioner1Id"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I get "practitioner1Id" id then make a GET request to "Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Practitioner resource
		And the single practitioner resource should not contain unwanted fields