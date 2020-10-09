@Structured @StructuredMultipleRequests @1.5.0-Full-Pack
Feature: StructuredMultipleRequests

@1.3.1-IncrementalAndRegression
Scenario: Structured request with one parameter and no Clinical information expected success
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the patient resource in the bundle should contain meta data profile and version id

@1.3.1-IncrementalAndRegression
Scenario: Structured request sent with three parameters and no expected mandatory for includeResolvedAllergies - expected failure
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the immunizations parameter
		And I add allergies parameter with invalid "RubbishPartParameter"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

@1.3.1-IncrementalAndRegression
Scenario: Structured request sent with multiple valid parameters but not including the mandatory part parameters - expected failure 
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the immunizations parameter
		And I add allergies parameter with invalid "RubbishPartParameter"
		And I add the medications parameter without mandatory partParameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

@1.3.1-IncrementalAndRegression
Scenario: Structured request sent with valid partParameter for includeMedications and without the mandatory partParameter for includeAllergies - expected failure 
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the immunizations parameter
		And I add the medication parameter with includePrescriptionIssues set to "false"
		And I add the allergies parameter without mandatory part parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

@1.3.1-IncrementalAndRegression
#PG 19-2-2020 - 1.3.2 - updated to possitive check as part parm now not mandatory
Scenario: Structured request sent with valid partParameter for includeAllergies and without includeMedications - expected success
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the immunizations parameter
		And I add the allergies parameter with resolvedAllergies set to "true"
		And I add the medications parameter without mandatory partParameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
	
@1.3.1-IncrementalAndRegression
Scenario: Structured request sent with two parameters and an invalid boolean part parameter expected failure
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add allergies parameter with invalid part parameter boolean
    When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

@1.3.1-IncrementalAndRegression
Scenario: Structured request sent with multiple parameters and part parameters with expected success including operation outcomes
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add 3 unknown structured parameters including part parameters
    When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And Check the operation outcome for each unsupported structured request
		And the patient resource in the bundle should contain meta data profile and version id
		And Check the number of issues in the operation outcome "3"

@1.3.1-IncrementalAndRegression
#SJD 04/10/2019 changed the response code as per specification for invalid parameter	
Scenario: Structured request sent with two invalid parameters expected failure
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" using an invalid parameter type
		And I send an unknownConsultations parameterName containing valid part parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

@1.3.2-IncrementalAndRegression	
Scenario: Structured request sent with multiple parameters expected success and no operation outcome
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the medication parameter with includePrescriptionIssues set to "true"
		And I add the allergies parameter with resolvedAllergies set to "true"
		And I add the includeConsultations parameter only
		And I add the immunizations parameter
		And I add the uncategorised data parameter
		And I add the Problems parameter
    When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And check that the bundle does not contain any duplicate resources
		And the patient resource in the bundle should contain meta data profile and version id
		And check the response does not contain an operation outcome
	

	#Check Invalid Param Combinations

Scenario: Structured request sent with invalid parameter combination 1 expected failure 
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the includeConsultations parameter only
		And I add the medications parameter to search from "3" years back
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Structured request sent with invalid parameter combination 2 expected failure 
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the includeConsultations parameter only
		And I add the uncategorised data parameter with date permutations "2014" and "2015"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Structured request sent with invalid parameter combination 3 expected failure 
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the includeConsultations parameter only
		And I add the problems parameter with filterSignificance "major"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
		
Scenario: Structured request sent with invalid parameter combination 4 expected failure 
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the includeConsultations parameter only
		And I add the problems parameter with filterStatus "active"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
				
Scenario: Structured request sent with invalid parameter combination 5 expected failure 
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the includeConsultations parameter only
		And I add the Referrals data parameter with date permutations "2014" and "2016"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Structured request sent with invalid parameter combination 6 expected failure 
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the includeConsultations parameter only
		Then I add the Diary Search date parameter with a past date "20" days ago
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Structured request sent with invalid parameter combination 7 expected failure 
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the includeConsultations parameter only
		And I add a NotGiven immunizations part parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Structured request sent with invalid parameter combination 8 expected failure 
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the includeConsultations parameter only
		And I add a Dissent immunizations part parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
				

Scenario: Structured request sent with invalid parameter combination 9 expected failure 
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the Problems parameter
		And I add the medications parameter to search from "3" years back
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Structured request sent with invalid parameter combination 10 expected failure 
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the Problems parameter
		And I add the uncategorised data parameter with date permutations "2014" and "2015"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
				
Scenario: Structured request sent with invalid parameter combination 11 expected failure 
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the Problems parameter
		And I add the Referrals data parameter with date permutations "2014" and "2016"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Structured request sent with invalid parameter combination 12 expected failure 
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the Problems parameter
		Then I add the Diary Search date parameter with a past date "20" days ago
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Structured request sent with invalid parameter combination 13 expected failure 
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the Problems parameter
		And I add a NotGiven immunizations part parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Structured request sent with invalid parameter combination 14 expected failure 
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the Problems parameter
		And I add a Dissent immunizations part parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

		
Scenario: Structured request sent for consultations and problems expect success
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the includeConsultations parameter only
		And I add the Problems parameter
    When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And check that the bundle does not contain any duplicate resources
		And the patient resource in the bundle should contain meta data profile and version id
		And check the response does not contain an operation outcome		



