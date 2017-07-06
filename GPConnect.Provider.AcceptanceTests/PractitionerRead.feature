@practitioner
Feature: PractitionerRead

Scenario Outline: Practitioner read successful request validate all of response
	Given I get the Practitioner for Practitioner Code "<practitioner>"
		And I store the Practitioner Id
	Given I configure the default "PractitionerRead" request
	When I make the "PractitionerRead" request
	Then the response status code should indicate success
		And the response should be the format FHIR JSON
		And the response should be an Practitioner resource
		And the practitioner resource shall contain meta data profile and version id
		And the returned resource shall contain a logical id matching the requested read logical identifier
		And if the practitioner resource contains any identifiera they should contain a valid system element
		And the returned resource shall contain the business identifier for Practitioner "practitioner1"
		And the practitioner resource should contain a single name element with a maximum of one family name
		And if the practitioner resource contains a practitionerRole it should have a valid system code and display
		And if the returned practitioner resource contains a communication element it must match the specified valueset
		And the single practitioner resource should not contain dissallowed fields
		And the returned Practitioner resource should contain "<numberOfRoleIdentifiers>" role identifiers
	Examples: 
		| practitioner  | numberOfRoleIdentifiers |
		| practitioner1 | 0                       |
		| practitioner2 | 1                       |
		| practitioner3 | 2                       |

Scenario Outline: Practitioner Read with valid identifier which does not exist on providers system
	Given I configure the default "PractitionerRead" request
		And I set the Read Operation logical identifier used in the request to "<LogicalId>"
	When I make the "PractitionerRead" request
	Then the response status code should be "404"
	Examples:
		| LogicalId      |
		| aaBA           |
		| 1ZEc2          |
		| z.as.dd        |
		| 1.1.22         |
		| 40-9           |
		| nd-skdm.mks--s |

Scenario Outline: Practitioner Read with invalid resource path in URL
	Given I get the Practitioner for Practitioner Code "practitioner1"
		And I store the Practitioner Id
	Given I configure the default "PractitionerRead" request
		And I set the Read Operation relative path to "<RelativePath>" and append the resource logical identifier
	When I make the "PractitionerRead" request
	Then the response status code should be "404"
	Examples:
		| RelativePath  |
		| Practitioners |
		| Practitioner! |
		| Practitioner2 |
		| practitioners |

Scenario Outline: Practitioner Read with missing mandatory header
	Given I get the Practitioner for Practitioner Code "practitioner1"
		And I store the Practitioner Id
	Given I configure the default "PractitionerRead" request
			And I do not send header "<Header>"
	When I make the "PractitionerRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Header            |
		| Ssp-TraceID       |
		| Ssp-From          |
		| Ssp-To            |
		| Ssp-InteractionId |
		| Authorization     |

Scenario Outline: Practitioner Read with incorrect interaction id
	Given I get the Practitioner for Practitioner Code "practitioner1"
		And I store the Practitioner Id
	Given I configure the default "PractitionerRead" request
		And I am performing the "<interactionId>" interaction
	When I make the "PractitionerRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| interactionId                                                     |
		| urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner3     |
		| urn:nhs:names:services:gpconnect:fhir:rest:read:practitioners     |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		|                                                                   |
		| null                                                              |

Scenario Outline: Practitioner Read using the _format parameter to request response format
	Given I get the Practitioner for Practitioner Code "practitioner1"
		And I store the Practitioner Id
	Given I configure the default "PractitionerRead" request
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "PractitionerRead" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be an Practitioner resource
		And the returned resource shall contain a logical id matching the requested read logical identifier
		And the returned resource shall contain the business identifier for Practitioner "practitioner1"
	Examples:
		| Parameter             | ResponseFormat |
		| application/json+fhir | JSON           |
		| application/xml+fhir  | XML            |

Scenario Outline: Practitioner Read using the Accept header to request response format
	Given I get the Practitioner for Practitioner Code "practitioner1"
		And I store the Practitioner Id
	Given I configure the default "PractitionerRead" request
		And I set the Accept header to "<Header>"
	When I make the "PractitionerRead" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be an Practitioner resource
		And the returned resource shall contain a logical id matching the requested read logical identifier
		And the returned resource shall contain the business identifier for Practitioner "practitioner1"
	Examples:
		| Header                | ResponseFormat |
		| application/json+fhir | JSON           |
		| application/xml+fhir  | XML            |

Scenario Outline: Practitioner Read sending the Accept header and _format parameter to request response format
	Given I get the Practitioner for Practitioner Code "practitioner2"
		And I store the Practitioner Id
	Given I configure the default "PractitionerRead" request
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "PractitionerRead" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be an Practitioner resource
		And the returned resource shall contain a logical id matching the requested read logical identifier
		And the returned resource shall contain the business identifier for Practitioner "practitioner2"
	Examples:
		| Header                | Parameter             | ResponseFormat |
		| application/json+fhir | application/json+fhir | JSON           |
		| application/json+fhir | application/xml+fhir  | XML            |
		| application/xml+fhir  | application/json+fhir | JSON           |
		| application/xml+fhir  | application/xml+fhir  | XML            |

Scenario: Conformance profile supports the Practitioner read operation
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the conformance profile should contain the "Practitioner" resource with a "read" interaction

#Potentially out of scope, needs verifiying
Scenario: Practitioner read response should contain an ETag header
	Given I find practitioner "practitioner1" and save it with the key "practitionerSaved"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I get practitioner "practitionerSaved" and use the id to make a get request to the url "Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Practitioner resource
		And the response should contain the ETag header matching the resource version

#Potentially out of scope, needs verifiying
Scenario: Practitioner read VRead of current resource should return resource
	Given I find practitioner "practitioner1" and save it with the key "practitionerSaved"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I perform a vread for practitioner "practitionerSaved"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Practitioner resource

#Potentially out of scope, needs verifiying
Scenario: Practitioner read VRead of non existant version should return error
	Given I find practitioner "practitioner1" and save it with the key "practitionerSaved"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I perform an practitioner vread with version id "NotRealVersionId" for practitioner stored against key "practitionerSaved"
	Then the response status code should be "404"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

@Manual
@ignore
Scenario: If provider sysstems allow the practitioner to be associated with multiple languages shown by communication element manual testing is required
		#communication - A language the practitioner is able to use in patient communication
@Manual
@ignore
Scenario: If provider supports versioning test that once a resource is updated that the old version can be retrieved

@Manual
@ignore
Scenario: If the provider supports active and inactive practitioners is this information visible within the returned practitioner resource

@Manual
@ignore
Scenario: Check that the optional fields are populated in the practitioner resource if they are available in the provider system
	# telecom - Telecom information for the practitioner, can be multiple instances for different types.
	# address - Address(s) for the Practitioner.
	# gender - Gender of the practitioner
	# practitionerRole - The list of Roles/Organizations that the Practitioner is associated with