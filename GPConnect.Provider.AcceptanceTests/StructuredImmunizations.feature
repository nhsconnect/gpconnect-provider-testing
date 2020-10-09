@Structured @StructuredImmunizations @1.5.0-Full-Pack
Feature: StructuredImmunizations

# These Tests are only Testing this Structured Area in isolation and Not with other Areas or Combinations of Include Parameters
# Tests around Multiple Structured Areas in one Request are tested in the MultipleRequests Feature

@1.3.2-IncrementalAndRegression	
Scenario: Verify Immunizations structured record for a Patient with Immunizations not linked to any problems
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient3"
		And I add the immunizations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "patient3"
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid 
		And The Immunization Resources are Valid
		And The Immunization Resources Do Not Include Not In Use Fields
		And the Bundle should contain "1" lists
		And The Immunization List is Valid
		And The Structured List Does Not Include Not In Use Fields	
		And I Check There is No Primary Problems List
		And I Check No Problem Resources are Included

#PG 19-2-2020 - Added for 1.3.2 - To check that associated problems and the problems list are sent.
@1.3.2-IncrementalAndRegression
Scenario: Verify Immunizations structured record for a Patient with Immunizations associated to Problems
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the immunizations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "patient2"
		And check that the bundle does not contain any duplicate resources
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid 
		And The Immunization Resources are Valid
		And The Immunization Resources Do Not Include Not In Use Fields
		And the Bundle should contain "2" lists
		And The Immunization List is Valid
		And The Structured List Does Not Include Not In Use Fields	
		And check the response does not contain an operation outcome
		And I Check The Primary Problems List
		And I Check The Primary Problems List Does Not Include Not In Use Fields
		And I Check The Problems Resources are Valid
		And I check The Problem Resources Do Not Include Not In Use Fields
		And Check a Problem is linked to an "Immunization" that is also included in the response with its list

Scenario: Retrieve the immunizations structured record section for an invalid NHS number
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for an invalid NHS Number
		And I add the immunizations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Retrieve the immunizations structured record section for an empty NHS number
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter with an empty NHS Number
		And I add the immunizations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Retrieve the immunizations structured record section for an invalid Identifier System
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an invalid Identifier System
		And I add the immunizations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Retrieve the immunizations structured record section for an empty Identifier System
Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an empty Identifier System
		And I add the immunizations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Retrieve the immunizations structured record for a patient that has sensitive flag
	Given I configure the default "GpcGetStructuredRecord" request 
		And I add an NHS Number parameter for "patient9"
		And I add the immunizations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response status code should be "404"
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

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
	   	And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid 
		And check the response does not contain an operation outcome
		And check structured list contains a note and emptyReason when no data in section

Scenario: Retrieve the immunizations structured record with a madeUp partParameter
	Given I configure the default "GpcGetStructuredRecord" request 
		And I add an NHS Number parameter for "patient2"
		And I add a madeUp immunizations part parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And Check the operation outcome returns the correct text and diagnostics includes "includeImmunisations" and "madeUp"
		And Check the number of issues in the operation outcome "1"