@Structured @StructuredUncategorised @1.5.0-Full-Pack
Feature: StructuredUncategorised

	# These Tests are only Testing this Structured Area in isolation and Not with other Areas or Combinations of Include Parameters
	# Tests around Multiple Structured Areas in one Request are tested in the MultipleRequests Feature


	Scenario Outline: Verify Uncategorised structured record for a Patient with Uncategorised data not linked to any problems
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
		And The Observation Resources Do Not Include Not In Use Fields
		And the Bundle should contain "1" lists
		And The Observation List is Valid
		And The Structured List Does Not Include Not In Use Fields
		And check the response does not contain an operation outcome
		And I Check There is No Problems Secondary Problems List
		And I Check No Problem Resources are Included
		Examples:
			| Patient   |
			| patient3  |
			| patient37 |

	#PG 19-2-2020 - Added for 1.3.2 - To check that associated problems and the problems list are sent.

	Scenario Outline: Verify Uncategorised structured record for a Patient with Uncategorised data associated to Problems
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
		And check that the bundle does not contain any duplicate resources
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid
		And The Observation Resources are Valid
		And The Observation Resources Do Not Include Not In Use Fields
		And the Bundle should contain "2" lists
		And The Observation List is Valid
		And The Structured List Does Not Include Not In Use Fields
		And check the response does not contain an operation outcome
		And I Check The Problems Secondary Problems List
		And I Check The Problems Secondary Problems List Does Not Include Not In Use Fields
		And I Check The Problems Resources are Valid
		And I check The Problem Resources Do Not Include Not In Use Fields
		And Check a Problem is linked to an "Observation" that is also included in the response with its list
		Examples:
			| Patient   |
			| patient2  |
			| patient37 |

	Scenario: Retrieve uncategorised data structured record section for an invalid NHS number
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for an invalid NHS Number
		And I add the uncategorised data parameter
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

	Scenario: Retrieve uncategorised data structured record section for an empty NHS number
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter with an empty NHS Number
		And I add the uncategorised data parameter
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

	Scenario: Retrieve uncategorised data structured record section for an invalid Identifier System
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an invalid Identifier System
		And I add the uncategorised data parameter
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

	Scenario: Retrieve uncategorised data structured record section for an empty Identifier System
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an empty Identifier System
		And I add the uncategorised data parameter
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

	Scenario: Retrieve uncategorised data structured record for a patient that has sensitive flag
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient9"
		And I add the uncategorised data parameter
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response status code should be "404"
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

	Scenario: Retrieve uncategorised data structured record for a patient that has no uncategorised data
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
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid
		And check the response does not contain an operation outcome
		And check structured list contains a note and emptyReason when no data in section

	Scenario Outline: Retrieve the uncategorised data structured record with invalid dates expected failure
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the uncategorised data parameter with date permutations "<startDate>" and "<endDate>"
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
		Examples:
			| startDate                 | endDate                   | Parameter                | PartParameter                 |
			| 2014                      | 2016-01-01                | includeUncategorisedData | uncategorisedDataSearchPeriod |
			| 2014-02                   | 2014-08-20                | includeUncategorisedData | uncategorisedDataSearchPeriod |
			| 2015-10-23T11:08:32       | 2016-11-01                | includeUncategorisedData | uncategorisedDataSearchPeriod |
			| 2015-10-23T11:08:32+00:00 | 2019-10-01                | includeUncategorisedData | uncategorisedDataSearchPeriod |
			| 2014-01-01                | 2016                      | includeUncategorisedData | uncategorisedDataSearchPeriod |
			| 2014-02-01                | 2014-08                   | includeUncategorisedData | uncategorisedDataSearchPeriod |
			| 2015-10-01                | 2016-11-23T11:08:32       | includeUncategorisedData | uncategorisedDataSearchPeriod |
			| 2014-01-01                | 2015-10-23T11:08:32+00:00 | includeUncategorisedData | uncategorisedDataSearchPeriod |

	Scenario: Retrieve the uncategorised data structured record with uncategorisedDataSearchPeriod in future expected failure
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the uncategorised data parameter with future start date
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

	Scenario: Retrieve the uncategorised data structured record with period dates equal to current date expected success and no operation outcome
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the uncategorised data parameter with current date
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate success
		And check the response does not contain an operation outcome

	Scenario: Retrieve the uncategorised data structured record with valid start date and no end date expected success and no operation outcome
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the uncategorised data parameter with valid start date and no end date
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate success
		And check the response does not contain an operation outcome

	Scenario: Retrieve the uncategorised data structured record with no start date and no end date expected success and no operation outcome
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the uncategorised data parameter with no start date and no end date
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate success
		And check the response does not contain an operation outcome

	Scenario: Retrieve the uncategorised data structured record startDate after endDate expected failure
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the uncategorised data parameter start date after endDate
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"