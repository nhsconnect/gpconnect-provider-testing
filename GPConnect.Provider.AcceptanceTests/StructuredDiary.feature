@Structured @StructuredDiary @1.5.0-Full-Pack @1.5.0-IncrementalAndRegression
Feature: StructuredDiary

Scenario: Search for Diary Entries for a Patient with Diary Entries With Problems Associated
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the Diary parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "patient2"
		And check that the bundle does not contain any duplicate resources
		And check the response does not contain an operation outcome
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid 
		And the Bundle should contain "2" lists
		And I Check the Diary List is Valid
		And The Structured List Does Not Include Not In Use Fields
		And I Check the Diary ProcedureRequests are Valid
		And I Check the Diary ProcedureRequests Do Not Include Not in Use Fields	
		And I Check The Problems List
		And I Check The Problems List Does Not Include Not In Use Fields
		And I Check The Problems Resources are Valid
		And I check The Problem Resources Do Not Include Not In Use Fields
		And Check a Problem is linked to ProcedureRequest and that it is also included
		
Scenario: Search for Diary Entries for a Patient with Diary Entries and No Problems associated
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient3"
		And I add the Diary parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "patient3"
		And check that the bundle does not contain any duplicate resources
		And check the response does not contain an operation outcome
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid 
		And the Bundle should contain "1" lists
		And I Check the Diary List is Valid
		And The Structured List Does Not Include Not In Use Fields	
		And I Check No Problem Resources are Included
		And I Check the Diary ProcedureRequests are Valid
		And I Check the Diary ProcedureRequests Do Not Include Not in Use Fields
		
Scenario: Search for Diary Entries for a Patient with No Diary Entries
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient4"
		And I add the Diary parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
	And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "patient4"
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid 
		And check the response does not contain an operation outcome
		And I Check Diary list contains a note and emptyReason when no data in section
		And The Structured List Does Not Include Not In Use Fields	

Scenario Outline: Search for Diary Entries Before a Past Date on a Patient with Diary Entries
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		Then I add the Diary Search date parameter with a past date "<DaysinPastToSearch>" days ago
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "<Patient>"
		And check that the bundle does not contain any duplicate resources
		And check the response does not contain an operation outcome
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid 
		And the Bundle should contain "<NumberOfListsExpected>" lists
		And I Check the Diary List is Valid
		And The Structured List Does Not Include Not In Use Fields	
		And I Check the Diary ProcedureRequests are Valid
		And I Check the Diary ProcedureRequests Do Not Include Not in Use Fields		
		And I Check the Diary ProcedureRequests are Within the "<DaysinPastToSearch>" days ago Search Range using Occurrence element
		Examples: 
		| Patient  | DaysinPastToSearch | NumberOfListsExpected |
		| patient2 | 20                 | 2                     |
		| patient3 | 20                 | 1                     |