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
		And the Response Resource should be a Patient
		And the returned resource shall contain a logical id matching the requested read logical identifier
	Examples:
		| Header                | ResponseFormat |
		| application/json+fhir | JSON           |
		| application/xml+fhir  | XML            |

Scenario Outline: Read patient using the _format parameter to request response format
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient Id
	Given I configure the default "PatientRead" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
		And I add a Format parameter with the Value "<Format>"
	When I make the "PatientRead" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be a Patient
		And the returned resource shall contain a logical id matching the requested read logical identifier
		And the Patient Identifiers should be valid for Patient "patient1"
	Examples:
		| Format                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Read patient sending the Accept header and _format parameter to request response format
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient Id
	Given I configure the default "PatientRead" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
		And I set the Accept header to "<Header>"
		And I add a Format parameter with the Value "<Format>"
	When I make the "PatientRead" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be a Patient
		And the returned resource shall contain a logical id matching the requested read logical identifier
		And the Patient Identifiers should be valid for Patient "patient1"
	Examples:
		| Header                | Format                | BodyFormat |
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
		And the Response Resource should be a Patient
		And the returned resource shall contain a logical id matching the requested read logical identifier

Scenario: Read patient response should contain an ETag header
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient Id
	Given I configure the default "PatientRead" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
	When I make the "PatientRead" request
	Then the response status code should indicate success
		And the Response Resource should be a Patient
		And the Response should contain the ETag header matching the Resource Version Id
		And the Patient Identifiers should be valid for Patient "patient1"

Scenario: Read patient If-None-Match should return a 304 on match
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient Id
		And I store the Patient
	Given I configure the default "PatientRead" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
		And I set the If-None-Match header to the stored Patient Version Id
	When I make the "PatientRead" request
	Then the response status code should be "304"
	
Scenario: Read patient If-None-Match should return full resource if no match
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient Id
	Given I configure the default "PatientRead" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
		And I set the If-None-Match header to "W/\"somethingincorrect\""
	When I make the "PatientRead" request
	Then the response status code should indicate success
		And the Response Resource should be a Patient
		And the Response should contain the ETag header matching the Resource Version Id
		And the Patient Identifiers should be valid for Patient "patient1"

Scenario: VRead patient _history with current etag should return current patient
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient Id
		And I store the Patient Version Id
	Given I configure the default "PatientRead" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
	When I make the "PatientRead" request
	Then the response status code should indicate success
		And the Response Resource should be a Patient
		And the Patient Identifiers should be valid for Patient "patient1"

Scenario: VRead patient _history with invalid etag should give a 404
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient Id
		And I set an invalid Patient Version Id
	Given I configure the default "PatientRead" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
	When I make the "PatientRead" request
	Then the response status code should be "404"

Scenario: Read patient resurned should conform to the GPconnect specification
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient Id
	Given I configure the default "PatientRead" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
	When I make the "PatientRead" request
	Then the response status code should indicate success
		And the Response Resource should be a Patient
		And the Patient Id should be valid
		And the Patient Metadata should be valid
		And the Patient Identifiers should be valid for Patient "patient1"
		And the Patient CareProvider Practitioner should be valid and resolvable
		And the Patient ManagingOrganization Organization should be valid and resolvable
		And the Patient Deceased should be valid
		And the Patient MultipleBirth should be valid
		And the Patient Telecom should be valid
		And the Patient Contact Relationship should be valid
		And the Patient MaritalStatus should be valid
		And the Patient Communication should be valid
		And the Patient Name should be valid
		And the Patient Contact Name should be valid
		And the Patient should exclude disallowed fields

Scenario: Conformance profile supports the Patient read operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the conformance profile should contain the "Patient" resource with a "read" interaction
#Manual tests need adding 