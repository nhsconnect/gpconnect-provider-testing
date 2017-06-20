@patient
Feature: PatientRead
	
Scenario Outline: Read patient 404 if patient not found
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "patient1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I make a GET request to "/Patient/<id>"
	Then the response status code should be "404"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"
	Examples:
		| id                                                         |
		| somethngVryRongThtAbsalutelyWontBeExistinOnTheRemostSistam |
		| 4543567638475665845564986758479086840564796854665748763454 |

Scenario: Read patient 404 if patient id not sent
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
	When I make a GET request to "/Patient/"
	Then the response status code should be "404"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

Scenario Outline: Read patient _format parameter only
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "patient1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make a GET request for patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Patient resource
	Examples:
		| Parameter             | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Read patient accept header and _format parameter
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "patient1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make a GET request for patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Patient resource
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/json+fhir | application/json+fhir | JSON       |
		| application/json+fhir | application/xml+fhir  | XML        |
		| application/xml+fhir  | application/json+fhir | JSON       |
		| application/xml+fhir  | application/xml+fhir  | XML        |

Scenario Outline: Read patient failure due to missing header
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "patient1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
		And I do not send header "<Header>"
	When I make a GET request for patient "patient1"
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

Scenario: Read patient should contain correct logical identifier
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "patient1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I make a GET request for patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Patient resource
		And the response patient logical identifier should match that of stored patient "patient1"

Scenario: Read patient should contain ETag
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "patient1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I make a GET request for patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Patient resource
		And the response should contain the ETag header matching the resource version

Scenario: Read patient If-None-Match should return a 304 on match
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "patient1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I make a GET request for patient "patient1" with If-None-Match header
	Then the response status code should be "304"
	
Scenario: Read patient If-None-Match should return full resource if no match
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "patient1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
		And I set the If-None-Match header to "W/\"somethingincorrect\""
	When I make a GET request for patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Patient resource
		And the response should contain the ETag header matching the resource version

Scenario: VRead patient _history with current etag should return current patient
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "patient1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I perform a patient vread for patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Patient resource

Scenario: VRead patient _history with invalid etag should give a 404
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "patient1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I perform a patient vread for patient "patient1" with invalid ETag
	Then the response status code should be "404"

Scenario Outline: Read patient with accept header should contain valid resource
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "patient1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
		And I set the Accept header to "<Header>"
	When I make a GET request for patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Patient resource
		And the patient resource should contain an id
		And the patient resource should contain valid meta data
		And the patient resource should contain a single NHS Number identifier for patient "patient1"
		And the patient resource should contain a valid careProvider Practitioner reference
		And the patient resource should contain a valid managingOrganization Organization reference
		And the patient resource should contain a valid deceased dateTime field
		And the patient resource should contain a valid multipleBirth boolean field
		And the patient resource should contain valid telecom fields
		And the patient resource should contain valid relationship coding fields
		And the patient resource should contain valid maritalStatus coding fields
		And the patient resource should contain valid language coding fields for each communication
		And the patient resource should contain no more than one family or given name
		And the patient resource should contain no more than one family name field for each contact
		And the patient resource should not contain the fhir fields photo animal or link
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |