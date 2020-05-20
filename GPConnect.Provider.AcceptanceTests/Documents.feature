@Documents @1.5.0-Full-Pack
Feature: Documents

Scenario: Searching for Documents for a Patient with Documents
	#Given I get the Patient for Patient Value "patient2"
	#	And I store the Patient
	Given I add an NHS Number to GlobalContext for "patient2"
	Given I configure the default "DocumentsSearch" request
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters for a Documents Search call
	When I make the "DocumentsSearch" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"

