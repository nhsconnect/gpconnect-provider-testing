@structuredrecord
Feature: StructuredUncategorised
	
	@1.3.0
	Scenario: Pete Get Structured Uncategorised 
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the Uncategorised parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
	And the response should be a Bundle resource of type "collection"