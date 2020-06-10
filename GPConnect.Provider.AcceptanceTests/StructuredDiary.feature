@Structured @StructuredDiary @1.5.0-Full-Pack @1.5.0-IncrementalAndRegression
Feature: StructuredDiary


Scenario: Search for Diary Entries for a Patient with Diary Entries
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
		And the Bundle should contain "1" lists

		#Check Diary List
		#Check ProcedureRequests (get skeleton code from investigations which is already done)
		#Check ProcedureRequests Not In Use (get skeleton code from investigations which is already done)


	    
		#And I Check the Investigations List is Valid
		#And The Structured List Does Not Include Not In Use Fields	

		#And I Check the ProcedureRequests are Valid		
		#And I Check the ProcedureRequests Do Not Include Not in Use Fields
		#And I Check the Specimens are Valid		
		#And I Check the Specimens Do Not Include Not in Use Fields
		#And I Check the Test report Filing is Valid
		#And I Check the Test report Filing Do Not Include Not in Use Fields
