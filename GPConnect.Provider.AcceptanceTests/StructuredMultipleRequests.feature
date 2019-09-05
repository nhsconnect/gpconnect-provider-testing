@structuredrecord
Feature: StructuredMultipleRequests

Scenario: Valid sample request
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient1"
	And I add the medication parameter with includePrescriptionIssues set to "true"
	And I add the allergies parameter with resolvedAllergies set to "true"
    When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
	And the patient resource in the bundle should contain meta data profile and version id
	
Scenario: I send a request with no Clinical information in request 
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient1"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
	And the patient resource in the bundle should contain meta data profile and version id
	
Scenario: I send a request with an invalid part parameter - 1st  
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient1"
	And I add allergies parameter with invalid "RubbishPartParameter"
    When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
	And Check the operation outcome returns the correct text and diagnotics "includeAllergies" and "RubbishPartParameter"
	And the patient resource in the bundle should contain meta data profile and version id

Scenario: I send a request with an invalid part parameter - 2nd  
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient1"
	And I add allergies parameter with invalid part parameter2nd
    When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
	And Check the operation outcome returns the correct text and diagnotics "includeAllergies" and "includeResolvedAllergies"
	And the patient resource in the bundle should contain meta data profile and version id

Scenario: I send a request with the Immunisation parameter
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient1"
	And I add the immunisations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
	And Check the operation outcome returns the correct text and diagnotics "includeImmunisations"
	And the patient resource in the bundle should contain meta data profile and version id

Scenario: I send a request that contain only the mandatory values for parameter and part parameters
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient1"
	And The request only contains mandatory parameters
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
	And the patient resource in the bundle should contain meta data profile and version id

Scenario: I send a request containing all structured parameters including all optional parameters
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient1"
	And I send a request that contains all forward compatable structured parameters with optional parameters
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
	And the patient resource in the bundle should contain meta data profile and version id
	And Check the operation outcome for each unsupported structured request

Scenario: Regression a Sensitive patient containing a forward compatible parameter
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient9"
	And I send a request that contains all forward compatable structured parameters with optional parameters
    When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "404"
	And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

Scenario: Regression when all parameters in the request are invalid 
	Given I configure the default "GpcGetStructuredRecord" request
	And I add an NHS Number parameter for "patient1" using an invalid parameter type
	And I send an invalid Consultations parameter containing valid part parameters
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "400"
    And the response should be a OperationOutcome resource with error code "INVALID_NHS_NUMBER"


