Feature: RegisterPatient

Background:
	Given I have the test patient codes

Scenario: Successful registration of a temporary patient
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration period with start date "2017-05-05" and end date "2018-03-12" to "registerPatient"
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"

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
		And I add the registration period with start date "2017-05-05" and end date "2018-03-12" to "registerPatient"
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
		And I add the registration period with start date "2017-04-12" and end date "2017-12-24" to "registerPatient"
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

Scenario Outline: Register patient and set identifier to null before sending the request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I register patient "<patient>" with first name "<firstName>" and family name "<secondName>" with NHS number "<nhsNumber>" and birth date "<birthDate>"
		And I add the registration period with start date "<regStartDate>" to "<patient>"
		And I add the registration status with code "A" to "<patient>"
		And I add the registration type with code "T" to "<patient>"
		And I set the identifier from "<patient>" to null
	When I send a gpc.registerpatients to register "<patient>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
	Examples: 
		| patient   | firstName | secondName | nhsNumber | birthDate  | regStartDate |
		| patient23 | tom       | johnson    | 345554    | 1993-03-03 | 2017-05-05   |

Scenario Outline: Register patient and set active element to null before sending the request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I register patient "<patient>" with first name "<firstName>" and family name "<secondName>" with NHS number "<nhsNumber>" and birth date "<birthDate>"
		And I add the registration period with start date "<regStartDate>" to "<patient>"
		And I add the registration status with code "A" to "<patient>"
		And I add the registration type with code "T" to "<patient>"
		And I set the active element from "<patient>" to null
	When I send a gpc.registerpatients to register "<patient>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
	Examples: 
		| patient   | firstName | secondName | nhsNumber | birthDate  | regStartDate |
		| patient23 | tom       | johnson    | 345554    | 1993-03-03 | 2017-05-05   |

Scenario Outline: Register patient and set name element to null before sending the request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I register patient "<patient>" with first name "<firstName>" and family name "<secondName>" with NHS number "<nhsNumber>" and birth date "<birthDate>"
		And I add the registration period with start date "<regStartDate>" to "<patient>"
		And I add the registration status with code "A" to "<patient>"
		And I add the registration type with code "T" to "<patient>"
		And I set the name element from "<patient>" to null
	When I send a gpc.registerpatients to register "<patient>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
	Examples: 
		| patient   | firstName | secondName | nhsNumber | birthDate  | regStartDate |
		| patient23 | tom       | johnson    | 345554    | 1993-03-03 | 2017-05-05   |

Scenario Outline: Register patient and set gender element to null before sending the request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I register patient "<patient>" with first name "<firstName>" and family name "<secondName>" with NHS number "<nhsNumber>" and birth date "<birthDate>"
		And I add the registration period with start date "<regStartDate>" to "<patient>"
		And I add the registration status with code "A" to "<patient>"
		And I add the registration type with code "T" to "<patient>"
		And I set the gender element from "<patient>" to null
	When I send a gpc.registerpatients to register "<patient>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
	Examples: 
		| patient   | firstName | secondName | nhsNumber | birthDate  | regStartDate |
		| patient23 | tom       | johnson    | 345554    | 1993-03-03 | 2017-05-05   |

Scenario Outline: Register patient without manadatory values and send request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I do not set "<doNotSet>" and register patient "<patient>" with first name "<firstName>" and family name "<secondName>" with NHS number "<nhsNumber>" and birth date "<birthDate>"
		And I add the registration period with start date "<regStartDate>" to "<patient>"
		And I add the registration status with code "A" to "<patient>"
		And I add the registration type with code "T" to "<patient>"
		And I set the gender element from "<patient>" to null
	When I send a gpc.registerpatients to register "<patient>"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource
	Examples: 
		| patient   | firstName | secondName | nhsNumber | birthDate  | regStartDate | doNotSet   |
		| patient25 | tom       | johnson    | 3455545   | 1993-03-03 | 2017-05-05   | active     |
		| patient26 | tom       | johnson    | 3455544   | 1993-03-03 | 2017-05-05   | gender     |
		| patient27 | tom       | johnson    | 3455546   | 1993-03-03 | 2017-05-05   | birthDate  |
		| patient28 | tom       | johnson    | 3455547   | 1993-03-03 | 2017-05-05   | name       |

Scenario Outline: Register patient without identifier and send request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I do not set "<doNotSet>" and register patient "<patient>" with first name "<firstName>" and family name "<secondName>" with NHS number "<nhsNumber>" and birth date "<birthDate>"
		And I add the registration period with start date "<regStartDate>" to "<patient>"
		And I add the registration status with code "A" to "<patient>"
		And I add the registration type with code "T" to "<patient>"
		And I set the gender element from "<patient>" to null
	When I send a gpc.registerpatients to register "<patient>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
	Examples: 
		| patient   | firstName | secondName | nhsNumber | birthDate  | regStartDate | doNotSet   |
		| patient24 | tom       | johnson    | 3455543   | 1993-03-03 | 2017-05-05   | Identifier |

Scenario Outline: Register patient with an invalid NHS number
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I register patient "<patient>" with first name "<firstName>" and family name "<secondName>" with NHS number "<nhsNumber>" and birth date "<birthDate>"
		And I add the registration period with start date "<regStartDate>" to "<patient>"
		And I add the registration status with code "A" to "<patient>"
		And I add the registration type with code "T" to "<patient>"
	When I send a gpc.registerpatients to register "<patient>"
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "400"
	Examples: 
		| patient   | firstName | secondName | nhsNumber | birthDate  | regStartDate |
		| patient23 | tom       | johnson    | 34555##4  | 1993-03-03 | 2017-05-05   |
		| patient23 | tom       | johnson    |           | 1993-03-03 | 2017-05-05   |
		| patient23 | tom       | johnson    | hello     | 1993-03-03 | 2017-05-05   |

Scenario Outline: Register patient and check registration period is not null
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I register patient "<patient>" with first name "<firstName>" and family name "<secondName>" with NHS number "<nhsNumber>" and birth date "<birthDate>"
		And I add the registration period with start date "<regStartDate>" to "<patient>"
		And I add the registration status with code "A" to "<patient>"
		And I add the registration type with code "T" to "<patient>"
	When I send a gpc.registerpatients to register "<patient>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the bundle should contain a registration type
		And the bundle should contain a registration status
		And the bundle should contain a registration period
	Examples: 
		| patient   | firstName | secondName | nhsNumber | birthDate  | regStartDate |
		| patient23 | tom       | johnson    | 34555455  | 1993-03-03 | 2017-05-05   |

Scenario Outline: Register patient and validate patient response contains the correct quantity of elements
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I register patient "<patient>" with first name "<firstName>" and family name "<secondName>" with NHS number "<nhsNumber>" and birth date "<birthDate>"
		And I add the registration period with start date "<regStartDate>" to "<patient>"
		And I add the registration status with code "A" to "<patient>"
		And I add the registration type with code "T" to "<patient>"
	When I send a gpc.registerpatients to register "<patient>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the bundle patient response should contain exactly 1 family name
		And the bundle patient response should contain exactly 1 given name
		And the bundle patient response should contain exactly 1 gender element
		And the bundle patient response should contain exactly 1 birthDate element
	Examples: 
		| patient   | firstName | secondName | nhsNumber | birthDate  | regStartDate |
		| patient23 | tom       | johnson    | 34555455  | 1993-03-03 | 2017-05-05   |