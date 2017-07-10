@patient
Feature: PatientRead

Scenario Outline: Read patient 404 if patient not found
	Given I configure the default "PatientRead" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
		And I set the GET request Id to "<id>"
	When I make the "PatientRead" request
	Then the response status code should be "404"
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"
	Examples:
		| id                                                         |
		| SomthingIncorrectWhichIsNotTheOnProviderSystem             |
		| 4543567638475665845564986758479086840564796854665748763454 |

Scenario Outline: Patient Read with valid identifier which does not exist on providers system
	Given I configure the default "PatientRead" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
		And I set the Read Operation logical identifier used in the request to "<LogicalId>"
	When I make the "PatientRead" request
	Then the response status code should be "404"
	Examples:
		| LogicalId      |
		| aaBA           |
		| 1ZEc2          |
		| z.as.dd        |
		| 1.1.22         |
		| 40-9           |
		| nd-skdm.mks--s |

Scenario: Read patient 404 if patient id not sent
	Given I configure the default "PatientRead" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
	When I make the "PatientRead" request
	Then the response status code should be "404"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

Scenario Outline: Read patient using the Accept header to request response format
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient Id
	Given I configure the default "PatientRead" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
		And I set the Accept header to "<Header>"
	When I make the "PatientRead" request
	Then the response status code should indicate success
		And the response body should be FHIR <ResponseFormat>
		And the response should be a Patient resource
		And the returned resource shall contain a logical id matching the requested read logical identifier
		And the patient resource should contain a single NHS Number identifier for patient "patient1"
	Examples:
		| Header                | ResponseFormat |
		| application/json+fhir | JSON           |
		| application/xml+fhir  | XML            |

Scenario Outline: Read patient using the _format parameter to request response format
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient Id
	Given I configure the default "PatientRead" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "PatientRead" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Patient resource
		And the returned resource shall contain a logical id matching the requested read logical identifier
		And the patient resource should contain a single NHS Number identifier for patient "patient1"
	Examples:
		| Parameter             | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Read patient sending the Accept header and _format parameter to request response format
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient Id
	Given I configure the default "PatientRead" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "PatientRead" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Patient resource
		And the returned resource shall contain a logical id matching the requested read logical identifier
		And the patient resource should contain a single NHS Number identifier for patient "patient1"
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/json+fhir | application/json+fhir | JSON       |
		| application/json+fhir | application/xml+fhir  | XML        |
		| application/xml+fhir  | application/json+fhir | JSON       |
		| application/xml+fhir  | application/xml+fhir  | XML        |

Scenario Outline: Read patient failure due to missing header
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient Id
	Given I configure the default "PatientRead" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
		And I do not send header "<Header>"
	When I make the "PatientRead" request
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
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient Id
	Given I configure the default "PatientRead" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
	When I make the "PatientRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Patient resource
		And the returned resource shall contain a logical id matching the requested read logical identifier

Scenario: Read patient response should contain an ETag header
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient Id
	Given I configure the default "PatientRead" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
	When I make the "PatientRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Patient resource
		And the response should contain the ETag header matching the resource version
		And the patient resource should contain a single NHS Number identifier for patient "patient1"

Scenario: Read patient If-None-Match should return a 304 on match
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "patient1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I make a GET request for patient "patient1" with If-None-Match header
	Then the response status code should be "304"
	
Scenario: Read patient If-None-Match should return full resource if no match
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient Id
	Given I configure the default "PatientRead" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
		And I set the If-None-Match header to "W/\"somethingincorrect\""
	When I make the "PatientRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Patient resource
		And the response should contain the ETag header matching the resource version
		And the patient resource should contain a single NHS Number identifier for patient "patient1"

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
		And the patient resource should contain a single NHS Number identifier for patient "patient1"

Scenario: VRead patient _history with invalid etag should give a 404
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "patient1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I perform a patient vread for patient "patient1" with invalid ETag
	Then the response status code should be "404"

Scenario: Read patient resurned should conform to the GPconnect specification
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient Id
	Given I configure the default "PatientRead" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
	When I make the "PatientRead" request
	Then the response status code should indicate success
		And the response should be a Patient resource
		And the patient resource should contain an id
		And the patient resource should contain valid meta data
		And the patient resource should contain a single NHS Number identifier for patient "patient1"
		And if the patient resource contains a careProvider Practitioner reference it is valid
		And if the patient resource contains a managingOrganization Organization reference it is valid
		And if the patient resource contains a deceased dateTime field it is valid
		And if the patient resource contains a multipleBirth boolean field it is valid
		And if the patient resource contains telecom fields they are valid
		And if the patient resource contains relationship coding fields they are valid
		And if the patient resource contains maritalStatus coding fields they are valid
		And if the patient resource contains language coding fields for each communication they are valid
		And the patient resource should contain no more than one family or given name
		And the patient resource should contain no more than one family name field for each contact
		And the Patient should exclude photo and link and animal fields
