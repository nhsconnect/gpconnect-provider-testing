@structuredrecord
Feature: StructuredMultipleRequests

Scenario: Check success when no Clinical information in request 
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient1"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
	And the patient resource in the bundle should contain meta data profile and version id

Scenario: Check success with one operation outcome returned with unsupported parameter 
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient1"
	And I add the immunisations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
	And Check the operation outcome returns the correct text and diagnotics "includeImmunisations"
	And the patient resource in the bundle should contain meta data profile and version id
	
Scenario: check success with one operation outcome returned with invalid string part parameter
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient1"
	And I add allergies parameter with invalid "RubbishPartParameter"
    When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
	And Check the operation outcome returns the correct text and diagnostics "includeAllergies" and "RubbishPartParameter"
	And the patient resource in the bundle should contain meta data profile and version id

Scenario: Check success with one operation outcome returned with invalid boolean part parameter   
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient1"
	And I add allergies parameter with invalid part parameter boolean
    When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
	And Check the operation outcome returns the correct text and diagnostics "includeAllergies" and "includeResolvedAllergies"
	And the patient resource in the bundle should contain meta data profile and version id

Scenario: Regression - Sensitive patient containing an unsupported parameter
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient9"
	And I send a request that contains all forward compatable structured parameters with optional parameters
    When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "404"
	And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

Scenario: Regression - All invalid parameter values in request
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient1" using an invalid parameter type
	And I send an invalid Consultations parameter containing valid part parameters
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "400"
    And the response should be a OperationOutcome resource with error code "INVALID_NHS_NUMBER"

Scenario: Regression - request that contain only the mandatory values for structured request
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient1"
	And The request only contains mandatory parameters
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
	And the patient resource in the bundle should contain meta data profile and version id
