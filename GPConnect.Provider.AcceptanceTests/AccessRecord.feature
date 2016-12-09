@fhir @accessrecord
Feature: AccessRecord

Scenario: Fhir Retrieve the care record for a patient
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "SUM" care record section for patient with NHS Number "9000000033"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Bundle"

Scenario Outline: Fhir Retrieve the care record sectons for a patient
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for patient with NHS Number "9000000033"
		And I am requesting the "<Code>" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Bundle"

Examples:
	| Code |
	| ADM |
	| ALL |
	| CLI |
	| IMM |
	| INV |
	| MED |
	| OBS |
	| PAT |
	| PRB |
	| REF |
	| SUM |

Scenario: Successfully retrieve the patient summary

Scenario: Successfully retrieve a care record section

Scenario: Empty request

Scenario: No record section requested

Scenario: Invalid record section requested

Scenario: Multiple record sections requested

Scenario: No patient NHS number supplied

Scenario: Invalid NHS number supplied

Scenario: No patient found with NHS number

Scenario: Invalid start date parameter

Scenario: Invalid end date parameter

Scenario: Time period specified for a care record section that must not be filtered

Scenario: Access blocked to care record as no patient consent
