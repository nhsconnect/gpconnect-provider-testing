@structuredrecord
Feature: StructuredProblems


@1.3.1
Scenario: Verify Problems structured record for a Patient 
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the Problems parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And I Check The Problems List
		And I Check The Problems List Does Not Include Not In Use Fields
		And I Check The Problems Resources are Valid
		And I check The Problem Resources Do Not Include Not In Use Fields
