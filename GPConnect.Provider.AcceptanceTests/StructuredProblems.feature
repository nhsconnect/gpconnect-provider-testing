@structuredrecord
Feature: StructuredProblems


@1.3.1
Scenario: Verify Problems structured record for a Patient 
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the Problems parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
