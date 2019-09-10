@structuredrecord
Feature: StructuredImmunizations
	
	@1.3.0
	Scenario Outline: Verify Immunizations structured record for a Patient with Immunizations
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the immunizations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "<Patient>"
		And the Bundle should contain "1" lists
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid 
	Examples:
	| Patient  |
	| patient2 |
		
		#look to add below
		
	
	#	And the Medications should be valid
	#	And the Medication Statements should be valid
	#	And the Medication Requests should be valid
	#	And the List of MedicationStatements should be valid
	#	And the Medication Requests should not contain any issues
	

	#Examples:
	#	| Patient  |
	#	| patient2 |
	#	| patient3 |
	#	| patient5 |
	#	| patient12 |

@1.3.0
Scenario: Retrieve the immunizations structured record section for an invalid NHS number
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for an invalid NHS Number
		And I add the immunizations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

@1.3.0
Scenario: Retrieve the immunizations structured record section for an empty NHS number
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter with an empty NHS Number
		And I add the immunizations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

@1.3.0
Scenario: Retrieve the immunizations structured record section for an invalid Identifier System
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an invalid Identifier System
		And I add the immunizations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

@1.3.0
Scenario: Retrieve the immunizations structured record section for an empty Identifier System
Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an empty Identifier System
		And I add the immunizations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

@1.3.0
Scenario: Retrieve the immunizations structured record for a patient that has sensitive flag
	Given I configure the default "GpcGetStructuredRecord" request 
	And I add an NHS Number parameter for "patient9"
	And I add the immunizations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

@1.3.0
Scenario: Retrieve the immunizations structured record for a patient that has no immunizations
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient4"
		And I add the immunizations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "patient4"
		And the Bundle should contain "1" lists
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid 
