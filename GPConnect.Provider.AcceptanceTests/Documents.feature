@Documents @1.5.0-Full-Pack
Feature: Documents


Scenario: Searching for Documents on a patient
	Given I configure the default "DocumentsSearch" request
		And I set the JWT Requested Scope to Organization Read
		#And I set the required parameters with a time period of "<Days>" days
		#When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		#And the response should be a Bundle resource of type "searchset"

