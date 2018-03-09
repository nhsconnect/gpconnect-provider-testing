@structuredrecord
Feature: AccessStructuredRecordAllergies

Scenario: Retrieve the allergy structured record section for a patient
	Given I configure the default "GpcGetStructuredRecord" request
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add the allergies parameter with resolvedAllergies set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
