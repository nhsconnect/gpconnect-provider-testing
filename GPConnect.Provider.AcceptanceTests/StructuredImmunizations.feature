@structuredrecord
Feature: StructuredImmunizations
	
	@1.3.0
	Scenario: Pete Get Structured Immunizations
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the immunisations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
	And the response should be a Bundle resource of type "collection"


	#Scenario Outline: Retrieve the medication structured record section for a patient excluding prescription issues
	#Given I configure the default "GpcGetStructuredRecord" request
		#And I add an NHS Number parameter for "<Patient>"
		#And I add the medication parameter with includePrescriptionIssues set to "false"
	#When I make the "GpcGetStructuredRecord" request
	#Then the response status code should indicate success
		#And the response should be a Bundle resource of type "collection"
		
		#look to add below

	#	And the response meta profile should be for "structured"
	#	And the patient resource in the bundle should contain meta data profile and version id
	#	And if the response bundle contains a practitioner resource it should contain meta data profile and version id
	#	And if the response bundle contains an organization resource it should contain meta data profile and version id
	#	And the Bundle should be valid for patient "<Patient>"
	#	And the Bundle should contain "1" lists
	#	And the Medications should be valid
	#	And the Medication Statements should be valid
	#	And the Medication Requests should be valid
	#	And the List of MedicationStatements should be valid
	#	And the Medication Requests should not contain any issues
	#	And the Patient Id should be valid
	#	And the Practitioner Id should be valid
	#	And the Organization Id should be valid 
	#Examples:
	#	| Patient  |
	#	| patient2 |
	#	| patient3 |
	#	| patient5 |
	#	| patient12 |