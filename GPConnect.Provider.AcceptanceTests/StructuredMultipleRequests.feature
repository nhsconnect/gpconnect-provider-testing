@structuredrecord
Feature: StructuredMultipleRequests

@1.3.1
Scenario: Structured request with one parameter and no Clinical information expected success
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient1"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
	And the patient resource in the bundle should contain meta data profile and version id

@1.3.1
Scenario: Structured request sent with three parameters expected success with operation outcome for unknown allergies part parameter
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient2"
	And I add the immunizations parameter
	And I add allergies parameter with invalid "RubbishPartParameter"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
	And Check the operation outcome returns the correct text and diagnostics includes "includeAllergies" and "RubbishPartParameter"
	And the patient resource in the bundle should contain meta data profile and version id
	And Check the number of issues in the operation outcome "1"
	
@1.3.1	
Scenario: Structured request sent with multpile parameters expected success with no operation outcome
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient2"
	And I send a request that contains known multiple structured parameters including optional part parameters
    When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
    And the patient resource in the bundle should contain meta data profile and version id
	And check the response does not contain an operation outcome
	

@1.3.1
Scenario: Structured request sent with two parameters one invalid boolean part parameter expected success with operation outcome
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient1"
	And I add allergies parameter with invalid part parameter boolean
    When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
	And Check the operation outcome returns the correct text and diagnostics includes "includeAllergies" and "includeResolvedAllergies"
	And the patient resource in the bundle should contain meta data profile and version id
	And Check the number of issues in the operation outcome "1"

@1.3.1
Scenario: Structured request sent with multiple parameters and part parameters with expected success including operation outcomes
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient1"
	And I add 3 invalid structured parameters and or part parameters
    When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
	And Check the operation outcome for each unsupported structured request
	And the patient resource in the bundle should contain meta data profile and version id
	And Check the number of issues in the operation outcome "3"

@1.3.1
Scenario: Structured request sent with two invalid parameters expected failure
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient1" using an invalid parameter type
	And I send an invalid Consultations parameter containing valid part parameters
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "400"
    And the response should be a OperationOutcome resource with error code "INVALID_NHS_NUMBER"

@1.3.1
Scenario: Structured request sent with multiple parameters for a Sensitive patient expected failure
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient9"
	And I add the medication parameter with includePrescriptionIssues set to "true"
	And I add allergies parameter with invalid part parameter boolean
    When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "404"
	And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"


