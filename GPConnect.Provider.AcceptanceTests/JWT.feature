@jwt
Feature: JWT

Scenario: JWT expiry time greater than 300 seconds
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set the JWT expiry time to "301" seconds after creation time
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT expiry time less than 300 seconds
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set the JWT expiry time to "299" seconds after creation time
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT not base64 encoded
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set the default JWT without base64 encoding
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT expiry time before creation time
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set the JWT expiry time to "-1" seconds after creation time
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT creation time in the future
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set the JWT creation time to "200" seconds after the current time
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT reason for request is not directcare
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set the JWT reason for request to "notdirectcare"
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT authorization server token incorrect
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set the JWT authorization server token URL to "https://notValid.fhir.nhs.net/tokenEndpoint"
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT requesting device is not valid FHIR device resource
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set an invalid JWT requesting device resource
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT requesting organization is not valid FHIR organization resource
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set an invalid JWT requesting organization resource
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT requesting organization identifier does not contain an ODS code
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set JWT requesting organization resource without ODS Code
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT requesting organization does not contain and identifier
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set JWT requesting organization resource without identifier
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT requesting practitioner is not valid FHIR practitioner resource
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set an invalid JWT requesting practitioner resource
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT requesting practitioner identifier does not contain an SDS Id
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set a JWT requesting practitioner without SDS id
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT requesting practitioner does not contain identifier
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set a JWT requesting practitioner without identifier
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT User Id does not match requesting practitioner id
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set a JWT requesting practitioner with miss matched user id
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT requesting practitioner name does not contain a family or given name
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set a JWT requesting practitioner with missing name element
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT requesting practitioner practitionerRole does not contain a SDS Job Role name
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set a JWT requesting practitioner with missing SDS Job Role
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT missing sub claim
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set a JWT without sub claim
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT missing aud claim
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set a JWT without aud claim
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT missing exp claim
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set a JWT without exp claim
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT missing iat claim
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set a JWT without iat claim
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT missing reason for request claim
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set a JWT without reason for request claim
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT missing requested record claim
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set a JWT without requested record claim
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT missing requested scope claim
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set a JWT without requested scope claim
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT missing requesting device claim
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set a JWT without requesting device claim
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT missing requesting organization claim
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set a JWT without requesting organization claim
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT missing requesting practitioner claim
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set a JWT without requesting practitioner claim
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT requesting device invalid resourceType
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I change the JWT requesting device resource type to InvalidResourceType
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT requesting organization invalid resourceType
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I change the JWT requesting organization resource type to InvalidResourceType
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT requesting practitioner invalid resourceType
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I change the JWT requesting practitioner resource type to InvalidResourceType
	When I make a GET request to "/metadata"
	Then the response status code should be "400"

Scenario: JWT requested record patient does not match getCareRecord Payload patient
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
	And I author a request for the "SUM" care record section for patient with NHS Number "9000000033"
	And I set the JWT requested record patient NHS number to "9000000009"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"

Scenario: JWT requested scope for getCareRecord does not match type of request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
	And I author a request for the "SUM" care record section for patient with NHS Number "9000000033"
	And I set the JWT requested scope to "organization/*.write"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"

Scenario: JWT requested scope is invalid type
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
	And I author a request for the "ENC" care record section for patient with NHS Number "9000000033"
	And I set the JWT requested scope to "encounter/*.read"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"

Scenario: JWT requested scope for metaData request does not match organization read
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I set the JWT requested scope to "patient/*.read"
	When I make a GET request to "/metadata"
	Then the response status code should be "400"
