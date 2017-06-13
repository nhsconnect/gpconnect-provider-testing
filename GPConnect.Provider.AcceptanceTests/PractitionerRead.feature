@practitioner
Feature: PractitionerRead

Background:
	Given I have the test patient codes
	Given I have the test practitioner codes
	Given I have the test ods codes

Scenario: Practitioner read successful request
	Given I find practitioner "practitioner1" and save it with the key "practitionerSaved"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I get practitioner "practitionerSaved" and use the id to make a get request to the url "Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Practitioner resource

Scenario Outline: Practitioner read successful request checking the correct SDS role id is returned
	Given I find practitioner "<practitioner>" and save it with the key "practitionerSaved"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I get practitioner "practitionerSaved" and use the id to make a get request to the url "Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Practitioner resource
		And the practitioner resource should contain a role id equal to role id "<roleId>" or role id "<roleId2>" or role id "<roleId3>"
	Examples:
		# na = not applicable, some practitioner have only 1 role id and some have numerous possibilitys
		| practitioner  | roleId | roleId2 | roleId3 |
		| practitioner2 | PT1234 | na      | na      |
		| practitioner3 | PT1122 | PT1234  | na      |
		| practitioner5 | PT3333 | PT2222  | PT4444  |

Scenario Outline: Practitioner read invalid request invalid id
	Given I find practitioner "practitioner1" and save it with the key "practitioner1Saved"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I make a GET request for a practitioner using an invalid id of "<InvalidId>" and url "Practitioner"
	Then the response status code should be "404"
	Examples:
		| InvalidId |
		| ##        |
		| 1@        |
		| 9i        |
		| 40-9      |

Scenario Outline: Practitioner read invalid request invalid URL
	Given I find practitioner "practitioner1" and save it with the key "practitioner1Saved"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I get practitioner "practitioner1Saved" and use the id to make a get request to the url "<InvalidURL>"
	Then the response status code should be "404"
	Examples:
		| InvalidURL    |
		| Practitioners |
		| Practitioner! |
		| Practitioner2 |

Scenario Outline: Practitioner read failure due to missing header
	Given I find practitioner "practitioner1" and save it with the key "practitioner1Saved"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
		And I do not send header "<Header>"
	When I get practitioner "practitioner1Saved" and use the id to make a get request to the url "Practitioner"
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
	Given I find practitioner "practitioner1" and save it with the key "practitioner1Saved"
	Given I am using the default server
		And I am performing the "<interactionId>" interaction
	When I get practitioner "practitioner1Saved" and use the id to make a get request to the url "Practitioner"
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
	Given I find practitioner "practitioner1" and save it with the key "practitioner1Saved"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
		And I add the parameter "_format" with the value "<Parameter>"
	When I get practitioner "practitioner1Saved" and use the id to make a get request to the url "Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Practitioner resource
	Examples:
		| Parameter             | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Practitioner read accept header and _format
	Given I find practitioner "practitioner1" and save it with the key "practitioner1Saved"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I get practitioner "practitioner1Saved" and use the id to make a get request to the url "Practitioner"
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

Scenario Outline: Practitioner read check meta data profile and version id
	Given I find practitioner "<practitioner>" and save it with the key "practitioner1Saved"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I get practitioner "practitioner1Saved" and use the id to make a get request to the url "Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Practitioner resource
		And the practitioner resource it should contain meta data profile and version id
	Examples:
		| practitioner  |
		| practitioner1 |
		| practitioner2 |
		| practitioner3 |
		| practitioner5 |

Scenario: Practitioner read practitioner contains single name element
	Given I find practitioner "practitioner1" and save it with the key "practitioner1Saved"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I get practitioner "practitioner1Saved" and use the id to make a get request to the url "Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Practitioner resource
		And the practitioner resource should contain a single name element

Scenario: Practitioner read practitioner contains identifier it is valid
	Given I find practitioner "practitioner1" and save it with the key "practitioner1Saved"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I get practitioner "practitioner1Saved" and use the id to make a get request to the url "Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Practitioner resource
		And if the practitioner resource contains an identifier it is valid
		And the identifier used to search for "practitioner1" is the same as the identifier returned in practitioner read

Scenario: Practitioner read practitioner contains practitioner role it is valid
	Given I find practitioner "practitioner1" and save it with the key "practitioner1Saved"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I get practitioner "practitioner1Saved" and use the id to make a get request to the url "Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Practitioner resource
		And if the practitioner resource contains a practitioner role it has a valid coding and system

Scenario: Practitioner read practitioner contains communication which is valid
	Given I find practitioner "practitioner1" and save it with the key "practitioner1Saved"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I get practitioner "practitioner1Saved" and use the id to make a get request to the url "Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Practitioner resource
		And the returned practitioner resource contains a communication element

Scenario Outline: Practitioner read practitioner returned does not contain elements not in the specification
	Given I find practitioner "practitioner1" and save it with the key "practitioner1Saved"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I get practitioner "practitioner1Saved" and use the id to make a get request to the url "Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Practitioner resource
		And the single practitioner resource should not contain unwanted fields
	Examples:
		| practitioner  |
		| practitioner1 |
		| practitioner2 |
		| practitioner3 |
		| practitioner5 |

Scenario: Practitioner read response should contain an ETag header
	Given I find practitioner "practitioner1" and save it with the key "practitionerSaved"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I get practitioner "practitionerSaved" and use the id to make a get request to the url "Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Practitioner resource
		And the response should contain the ETag header matching the resource version

Scenario: Practitioner read VRead of current resource should return resource
	Given I find practitioner "practitioner1" and save it with the key "practitionerSaved"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I perform a vread for practitioner "practitionerSaved"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Practitioner resource

Scenario: Practiotioner read VRead of non existant version should return error
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