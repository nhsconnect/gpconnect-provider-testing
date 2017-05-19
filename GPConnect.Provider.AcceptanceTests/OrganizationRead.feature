Feature: OrganizationRead

Background:
	Given I have the test ods codes


Scenario: OrganizationRead
	Given I get organization "ORG1" id and save it as "ORG1ID"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:organization" interaction
 