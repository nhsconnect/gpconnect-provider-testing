@structuredrecord
Feature: StructuredUncategorised
	
@1.3.0
Scenario Outline: Verify Uncategorised Data structured record for a Patient with Uncategorised
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the uncategorised data parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "<Patient>"
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid 
		And The Observation Resources are Valid
		#And The Observation Resources Do Not Include Not In Use Fields
		And the Bundle should contain "1" lists
		#check list
		#Check LIst does not include not in use fields.
	Examples:
	| Patient  |
	| patient2 |



@1.3.0
Scenario: Retrieve the uncategorised data structured record section for an invalid NHS number
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for an invalid NHS Number
		And I add the uncategorised data parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

@1.3.0
Scenario: Retrieve the uncategorised data structured record section for an empty NHS number
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter with an empty NHS Number
		And I add the uncategorised data parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

@1.3.0
Scenario: Retrieve the uncategorised data structured record section for an invalid Identifier System
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an invalid Identifier System
		And I add the uncategorised data parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

@1.3.0
Scenario: Retrieve the uncategorised data structured record section for an empty Identifier System
Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an empty Identifier System
		And I add the uncategorised data parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

@1.3.0
Scenario: Retrieve the uncategorised data structured record for a patient that has sensitive flag
	Given I configure the default "GpcGetStructuredRecord" request 
	And I add an NHS Number parameter for "patient9"
	And I add the uncategorised data parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

@1.3.0
Scenario: Retrieve the uncategorised data structured record for a patient that has no uncategorised data
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient4"
		And I add the uncategorised data parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "patient4"
		#TODO need to add check entry.resource.note ="Information not available"  and entry.resource.emptyReason = "noContent"
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid 
		And check the response does not contain an operation outcome
		