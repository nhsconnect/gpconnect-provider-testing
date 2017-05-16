Feature: RegisterPatient

Background:
	Given I have the test patient codes

Scenario Outline: Register patient send request to incorrect URL
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration period with start date "<regStartDate>" to "registerPatient"
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
	When I register patient stored against key "registerPatient" with url "<url>"
	Then the response status code should be "404"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "REFERENCE_NOT_FOUND"
	Examples: 
		| regStartDate | url                            |
		| 2017-05-05   | /Patient/$gpc.registerpatien   |
		| 2016-12-05   | /PAtient/$gpc.registerpatient  |
		| 1999-01-22   | /Patient/$gpc.registerpati#ent |
		| 2017-08-12   | /Patient                       |

Scenario Outline: Register patient with invalid interactionIds 
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I am performing the "<interactionId>" interaction
		And I add the registration period with start date "2017-05-05" and end date "2018-11-12" to "registerPatient"
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples: 
		| interactionId                                                          |
		| urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatsssient |
		| urn:nhs:names:services:gpconnect:fhir:rest:create:appointment          |
		|                                                                        |
		| null                                                                   |

Scenario Outline: Register patient with missing header
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration period with start date "2017-04-12" and end date "2018-12-24" to "registerPatient"
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I do not send header "<Header>"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
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

Scenario: Register patient without sending identifier within patient
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration period with start date "2017-04-12" and end date "2018-12-24" to "registerPatient"
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I remove the patients identifiers from the patient stored against key "registerPatient"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient without name element
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration period with start date "2017-04-12" and end date "2018-12-24" to "registerPatient"
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I remove the name element from the patient stored against key "registerPatient"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient without gender element
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration period with start date "2017-04-12" and end date "2018-12-24" to "registerPatient"
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I remove the gender element from the patient stored against key "registerPatient"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient without date of birth element
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration period with start date "2017-04-12" and end date "2018-12-24" to "registerPatient"
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I remove the DOB element from the patient stored against key "registerPatient"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Register patient with an invalid NHS number
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration period with start date "2017-04-12" and end date "2018-12-24" to "registerPatient"
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I clear exisiting identifiers in the patient stored against key "registerPatient" and add an NHS number identifier "<nhsNumber>"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_IDENTIFIER_VALUE"
	Examples: 
		| nhsNumber   |
		| 34555##4    |
		|             |
		| hello       |
		| 999999999   |
		| 9999999990  |
		| 99999999999 |
		| 9000000008  |
		| 90000000090 |

Scenario: Register patient and check all elements conform to the gp connect profile
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration period with start date "2017-05-05" and end date "2018-09-12" to "registerPatient"
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain a single Patient resource
		And the bundle should contain a registration period
		And the bundle should contain a registration status
		And the bundle should contain a registration type
		And the response bundle should contain a patient resource which contains atleast a single NHS number identifier matching patient stored against key "registerPatient"
		And the response bundle should contain a patient resource which contains exactly 1 family name matching the patient stored against key "registerPatient"
		And the response bundle should contain a patient resource which contains exactly 1 given name matching the patient stored against key "registerPatient"
		And the response bundle should contain a patient resource which contains exactly 1 gender element matching the patient stored against key "registerPatient"
		And the response bundle should contain a patient resource which contains exactly 1 birthDate element matching the patient stored against key "registerPatient"
