@structured @structuredinvestigations @1.5.0-Full-Pack
Feature: StructuredInvestigations
	
@1.5.0-IncrementalAndRegression
Scenario: Verify Investigations structured record for a Patient with Investigations not linked to any problems
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient3"
		And I add the Investigations parameter
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
		And I Check the Investigations List is Valid
		And The Structured List Does Not Include Not In Use Fields	
		And I Check the DiagnosticReports are Valid
		And I Check the DiagnosticReports Do Not Include Not in Use Fields
		And I Check the ProcedureRequests are Valid		
		And I Check the ProcedureRequests Do Not Include Not in Use Fields
		And I Check the Specimens are Valid		
		And I Check the Specimens Do Not Include Not in Use Fields

@1.5.0-IncrementalAndRegression
Scenario: Verify Investigations structured record for a Patient with Investigations associated to Problems
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the Investigations parameter
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
		And I Check the Investigations List is Valid
		And The Structured List Does Not Include Not In Use Fields	
		And I Check the DiagnosticReports are Valid
		And I Check the DiagnosticReports Do Not Include Not in Use Fields
		And I Check the ProcedureRequests are Valid		
		And I Check the ProcedureRequests Do Not Include Not in Use Fields
		And I Check the Specimens are Valid		
		And I Check the Specimens Do Not Include Not in Use Fields
		And I Check The Problems List
		And I Check The Problems List Does Not Include Not In Use Fields
		And I Check The Problems Resources are Valid
		And I check The Problem Resources Do Not Include Not In Use Fields
		And Check a Problem is linked to DiagnosticReport and that it is also included
		And the Bundle should contain "2" lists
		

@1.5.0-IncrementalAndRegression
Scenario: Retrieve Investigations structured record for a patient that has no Investigations data
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient4"
		And I add the Investigations parameter
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
		And check structured list contains a note and emptyReason when no data in section
		And check the response does not contain an operation outcome
	
@1.5.0-IncrementalAndRegression		
Scenario: Retrieve the Investigations data structured record with period dates equal to current date expected success and no operation outcome
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the investigations data parameter with current date
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And check the response does not contain an operation outcome

@1.5.0-IncrementalAndRegression				
Scenario: Retrieve Investigations data structured record section for an invalid Identifier System
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an invalid Identifier System
		And I add the Investigations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

@1.5.0-IncrementalAndRegression	
Scenario: Retrieve Investigations data structured record section for an empty Identifier System
Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an empty Identifier System
		And I add the Investigations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

@1.5.0-IncrementalAndRegression	
Scenario: Retrieve Investigations data structured record for a patient that has sensitive flag
	Given I configure the default "GpcGetStructuredRecord" request 
		And I add an NHS Number parameter for "patient9"
		And I add the Investigations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response status code should be "404"
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

@1.5.0-IncrementalAndRegression	
Scenario Outline: Retrieve the Investigations data structured record with invalid dates expected failure
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the investigations data parameter with date permutations "<startDate>" and "<endDate>"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
	Examples:
		| startDate                 | endDate                   | Parameter                | PartParameter                 |
		| 2014                      | 2016-01-01                | includeInvestigations | investigationSearchPeriod |
		| 2014-02                   | 2014-08-20                | includeInvestigations | investigationSearchPeriod |
		| 2015-10-23T11:08:32       | 2016-11-01                | includeInvestigations | investigationSearchPeriod |
		| 2015-10-23T11:08:32+00:00 | 2019-10-01                | includeInvestigations | investigationSearchPeriod |
		| 2014-01-01                | 2016                      | includeInvestigations | investigationSearchPeriod |
		| 2014-02-01                | 2014-08                   | includeInvestigations | investigationSearchPeriod |
		| 2015-10-01                | 2016-11-23T11:08:32       | includeInvestigations | investigationSearchPeriod |
		| 2014-01-01                | 2015-10-23T11:08:32+00:00 | includeInvestigations | investigationSearchPeriod |

@1.5.0-IncrementalAndRegression	
Scenario: Retrieve the Investigations data structured record with investigationSearchPeriod in future expected failure 
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the investigations data parameter with future start date
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

@1.5.0-IncrementalAndRegression	
Scenario: Retrieve the Investigations data structured record startDate after endDate expected failure
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the investigations data parameter start date after endDate
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER" 
		