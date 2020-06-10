﻿@Structured @StructuredMultipleRequests @1.5.0-Full-Pack
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
Scenario: Structured request sent with multpile parameters expected success and no operation outcome
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
	
	#Add all Problems Steps

	#Add all Consultations Steps


		# Check Have no duplicates
		# Check have Meds Resource and Linkages and List
		# Check have allergies and List
		# Check have Consultations and List
		# Check have Immunizations and List
		# Check have Uncategorised and List
		# Check have Problems and List


