@Structured @StructuredDiary @1.5.0-Full-Pack @1.5.0-IncrementalAndRegression
Feature: StructuredDiary

	# These Tests are only Testing this Structured Area in isolation and Not with other Areas or Combinations of Include Parameters
	# Tests around Multiple Structured Areas in one Request are tested in the MultipleRequests Feature

	Scenario Outline: Search for Diary Entries for a Patient with Diary Entries With Problems Associated
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the Diary parameter
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "<Patient>"
		And check that the bundle does not contain any duplicate resources
		And check the response does not contain an operation outcome
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid
		And the Bundle should contain "2" lists
		And I Check the Diary List is Valid
		And The Structured List Does Not Include Not In Use Fields
		And I Check the Diary ProcedureRequests are Valid
		And I Check the Diary ProcedureRequests Do Not Include Not in Use Fields
		And I Check The Problems Secondary Problems List
		And I Check The Problems Secondary Problems List Does Not Include Not In Use Fields
		And I Check The Problems Resources are Valid
		And I check The Problem Resources Do Not Include Not In Use Fields
		And Check a Problem is linked to ProcedureRequest and that it is also included
		Examples:
			| Patient   |
			| patient2  |
			| patient39 |

	Scenario Outline: Search for Diary Entries for a Patient with Diary Entries and No Problems associated
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the Diary parameter
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "<Patient>"
		And check that the bundle does not contain any duplicate resources
		And check the response does not contain an operation outcome
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid
		And the Bundle should contain "1" lists
		And I Check the Diary List is Valid
		And The Structured List Does Not Include Not In Use Fields
		And I Check There is No Primary Problems List
		And I Check There is No Problems Secondary Problems List
		And I Check No Problem Resources are Included
		And I Check the Diary ProcedureRequests are Valid
		And I Check the Diary ProcedureRequests Do Not Include Not in Use Fields
		Examples:
			| Patient   |
			| patient3  |
			| patient39 |

	Scenario: Search for Diary Entries for a Patient with No Diary Entries
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient4"
		And I add the Diary parameter
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
		And I Check Diary list contains a note and emptyReason when no data in section
		And The Structured List Does Not Include Not In Use Fields

	Scenario Outline: Search for Diary Entries with Todays Date and a Future Date on a Patient with Diary Entries
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		Then I add the Diary Search date parameter of "<YearsInFutureToSearch>" days in future
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "<Patient>"
		And check that the bundle does not contain any duplicate resources
		And check the response does not contain an operation outcome
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid
		And I Check the Diary List is Valid
		And The Structured List Does Not Include Not In Use Fields
		And I Check the Diary ProcedureRequests are Valid
		And I Check the Diary ProcedureRequests Do Not Include Not in Use Fields
		Examples:
			| YearsInFutureToSearch | Patient   |
			| 0                     | patient2  |
			| 10                    | patient39 |

	Scenario: Search for Diary Entries with a Past Date Expect Fail
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		Then I add the Diary Search date parameter with a past date "20" days ago
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

	Scenario Outline: Search for Diary Entries with invalid date values
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I set a Diary Search date to "<InvalidSearchDate>"
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
		Examples:
			| InvalidSearchDate          |
			| X2014                      |
			| X2014-02                   |
			| X2015-10-23T11:08:32       |
			| X2015-10-23T11:08:32+00:00 |
			|                            |
			| 2014                       |
			| 2014-02                    |
			| null                       |
