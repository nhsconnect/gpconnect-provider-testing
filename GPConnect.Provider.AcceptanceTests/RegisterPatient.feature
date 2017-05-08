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
		| patient   | firstName | secondName | nhsNumber |birthDate  | regStartDate |
		| patient23 | chris     | jobling    | 345554    |1993-03-03| 2017-05-05   | 