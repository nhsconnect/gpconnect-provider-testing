Feature: RegisterPatient

Background:
	Given I have the test patient codes

Scenario Outline: Register patient 
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I register patient "<patient>" with first name "<firstName>" and family name "<secondName>" with NHS number "<nhsNumber>" and birth date "<birthDate>"
		And I add the registration period with start date "<regStartDate>" to "<patient>"
		And I add the registration status with code "A" to "<patient>"
		And I add the registration type with code "T" to "<patient>"
	When I send a gpc.registerpatients to register "<patient>"
	Then the response status code should indicate success
	Examples: 
		| patient   | firstName | secondName | nhsNumber | birthDate  | regStartDate |
		| patient23 | tom     | johnson    | 345554    | 1993-03-03 | 2017-05-05   |


Scenario Outline: Register patient send request to incorrect URL
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I register patient "<patient>" with first name "<firstName>" and family name "<secondName>" with NHS number "<nhsNumber>" and birth date "<birthDate>"
		And I add the registration period with start date "<regStartDate>" to "<patient>"
		And I add the registration status with code "A" to "<patient>"
		And I add the registration type with code "T" to "<patient>"
	When I register "<patient>" with url "<url>"
	Then the response status code should indicate failure
	Examples: 
		| patient   | firstName | secondName | nhsNumber | birthDate  | regStartDate | url                            |
		| patient23 | tom       | johnson    | 345554    | 1993-03-03 | 2017-05-05   | /Patient/$gpc.registerpatien   |
		| patient23 | tom       | johnson    | 345554    | 1993-03-03 | 2017-05-05   | /PAtient/$gpc.registerpatient  |
		| patient23 | tom       | johnson    | 345554    | 1993-03-03 | 2017-05-05   | /$gpc.registerpatient          |
		| patient23 | tom       | johnson    | 345554    | 1993-03-03 | 2017-05-05   | /Patient/$gpc.registerpati#ent |


Scenario Outline: Register patient with invalid interactionIds 
	Given I am using the default server
		And I am performing the "<interactionId>" interaction
		And I register patient "<patient>" with first name "<firstName>" and family name "<secondName>" with NHS number "<nhsNumber>" and birth date "<birthDate>"
		And I add the registration period with start date "<regStartDate>" to "<patient>"
		And I add the registration status with code "A" to "<patient>"
		And I add the registration type with code "T" to "<patient>"
	When I send a gpc.registerpatients to register "<patient>"
	Then the response status code should indicate failure
	Examples: 
		| interactionId                                                          | patient   | firstName | secondName | nhsNumber | birthDate  | regStartDate |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatsssient | patient23 | tom       | johnson    | 345554    | 1993-03-03 | 2017-05-05   |
		|                                                                        | patient23 | tom       | johnson    | 345554    | 1993-03-03 | 2017-05-05   |
		| null                                                                   | patient23 | tom       | johnson    | 345554    | 1993-03-03 | 2017-05-05   |
		| urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments | patient23 | tom       | johnson    | 345554    | 1993-03-03 | 2017-05-05   |


Scenario Outline: Register patient with missing header
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I do not send header "<Header>"
		And I register patient "<patient>" with first name "<firstName>" and family name "<secondName>" with NHS number "<nhsNumber>" and birth date "<birthDate>"
		And I add the registration period with start date "<regStartDate>" to "<patient>"
		And I add the registration status with code "A" to "<patient>"
		And I add the registration type with code "T" to "<patient>"
	When I send a gpc.registerpatients to register "<patient>"
	Then the response status code should indicate failure
	Examples: 
		| Header            | patient   | firstName | secondName | nhsNumber | birthDate  | regStartDate |
		| Ssp-TraceID       | patient23 | tom       | johnson    | 345554    | 1993-03-03 | 2017-05-05   |
		| Ssp-From          | patient23 | tom       | johnson    | 345554    | 1993-03-03 | 2017-05-05   |
		| Ssp-To            | patient23 | tom       | johnson    | 345554    | 1993-03-03 | 2017-05-05   |
		| Ssp-InteractionId | patient23 | tom       | johnson    | 345554    | 1993-03-03 | 2017-05-05   |
		| Authorization     | patient23 | tom       | johnson    | 345554    | 1993-03-03 | 2017-05-05   |