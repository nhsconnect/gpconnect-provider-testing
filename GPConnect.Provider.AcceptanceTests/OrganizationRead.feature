Feature: OrganizationRead

Background:
	Given I have the test ods codes


Scenario Outline: Organization Read successful request
	Given I get organization "<Organization>" id and save it as "ORG1ID"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:organization" interaction
	When I get "ORG1ID" id then make a GET request to organization url "Organization"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Organization resource
		Examples: 
		| Organization |
		| ORG1         |
		| ORG2         |
		| ORG3         |

Scenario: Organization Read invalid ORG code
	Given I get organization "unknownORG" id and save it as "ORG1ID"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:organization" interaction
	When I get "ORG1ID" id then make a GET request to organization url "Organization"
	Then the response status code should indicate failure
	
Scenario Outline: Organization read invalid request invalid id
	Given I get organization "ORG1" id and save it as "ORG1ID"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:organization" interaction
	When I make a GET request for a organization using an invalid id of "<InvalidId>"
	Then the response status code should be "404"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "ORGANISATION_NOT_FOUND"
		Examples: 
		| InvalidId |
		| 1@        |
		| 9i        |
		| 40-9      |

Scenario Outline: Organization read invalid request invalid URL
	Given I get organization "ORG1" id and save it as "ORG1ID"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:organization" interaction
	When I get "ORG1ID" id then make a GET request to organization url "<InvalidURL>"
	Then the response status code should be "400"
		Examples: 
		| InvalidURL    |
		| Organizations |
		| Organization! |
		| Organization2 |

Scenario Outline: Organization read failure due to missing header
	Given I get organization "ORG1" id and save it as "ORG1ID"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:organization" interaction
		And I do not send header "<Header>"
	When I get "ORG1ID" id then make a GET request to organization url "Organization"
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

Scenario Outline: Organization read failure with incorrect interaction id
	Given I get organization "ORG1" id and save it as "ORG1ID"
	Given I am using the default server
		And I am performing the "<interactionId>" interaction
	When I get "ORG1ID" id then make a GET request to organization url "Organization"
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

Scenario Outline: Organization read _format parameter only
	Given I get organization "ORG1" id and save it as "ORG1ID"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:organization" interaction
		And I add the parameter "_format" with the value "<Parameter>"
	When I get "ORG1ID" id then make a GET request to organization url "Organization"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Organization resource
		Examples:
        | Parameter             | BodyFormat |
        | application/json+fhir | JSON       |
        | application/xml+fhir  | XML        |

Scenario Outline: Organization read accept header and _format
	Given I get organization "ORG1" id and save it as "ORG1ID"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:organization" interaction
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I get "ORG1ID" id then make a GET request to organization url "Organization"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Organization resource
		  Examples:
        | Header                | Parameter             | BodyFormat |
        | application/json+fhir | application/json+fhir | JSON       |
        | application/json+fhir | application/xml+fhir  | XML        |
        | application/xml+fhir  | application/json+fhir | JSON       |
        | application/xml+fhir  | application/xml+fhir  | XML        |
	
Scenario: Conformance profile supports the Organization read operation
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the conformance profile should contain the "Organization" resource with a "read" interaction

Scenario: Organization read check meta data profile and version id
	Given I get organization "ORG1" id and save it as "ORG1ID"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:organization" interaction
	When I get "ORG1ID" id then make a GET request to organization url "Organization"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Organization resource
		And the organization resource it should contain meta data profile and version id

Scenario: Organization read organization contains identifier it is valid
	Given I get organization "ORG1" id and save it as "ORG1ID"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:organization" interaction
	When I get "ORG1ID" id then make a GET request to organization url "Organization"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Organization resource
		And if the organization resource contains an identifier it is valid

Scenario: Organization read organization contains valid partOf element with a valid reference
	Given I get organization "unknownORG" id and save it as "ORG1ID"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:organization" interaction
	When I get "ORG1ID" id then make a GET request to organization url "Organization"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Organization resource
		And if the organization resource contains a partOf reference it is valid