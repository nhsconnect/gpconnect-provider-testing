@Structured @StructuredReferrals @1.5.0-Full-Pack
Feature: StructuredReferrals

	# These Tests are only Testing this Structured Area in isolation and Not with other Areas or Combinations of Include Parameters
	# Tests around Multiple Structured Areas in one Request are tested in the MultipleRequests Feature

	@1.5.0-IncrementalAndRegression
	Scenario Outline: Verify Referrals structured record for a Patient with Referrals not linked to any problems
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the Referrals parameter
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
		And I Check the Referrals List is Valid
		And The Structured List Does Not Include Not In Use Fields
		And I Check the ReferralRequests are Valid
		And I Check the ReferralRequests Do Not Include Not in Use Fields
		And I Check There is No Problems Secondary Problems List
		And I Check No Problem Resources are Included
		Examples:
			| Patient   |
			| patient3  |
			| patient38 |

	@1.5.0-IncrementalAndRegression
	Scenario Outline: Verify Referrals structured record for a Patient with Referrals associated to Problems
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the Referrals parameter
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
		And I Check the Referrals List is Valid
		And The Structured List Does Not Include Not In Use Fields
		And I Check the ReferralRequests are Valid
		And I Check the ReferralRequests Do Not Include Not in Use Fields
		And I Check The Problems Secondary Problems List
		And I Check The Problems Secondary Problems List Does Not Include Not In Use Fields
		And I Check The Problems Resources are Valid
		And I check The Problem Resources Do Not Include Not In Use Fields
		And the Bundle should contain "2" lists
		And Check a Problem is linked to ReferralRequest and that it is also included
		Examples:
			| Patient   |
			| patient2  |
			| patient38 |

	@1.5.0-IncrementalAndRegression
	Scenario: Retrieve Referrals structured record for a patient that has no Referrals data
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient4"
		And I add the Referrals parameter
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

	#Demonstrator does not have this data, so test will fail against it
	@1.5.0-IncrementalAndRegression
	Scenario Outline: Patient with Referrals Has Warnings and Associated Notes
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the Referrals parameter
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate success
		And the Bundle should contain "1" lists
		And Check the list contains the following warning "<Warning>"
		And Check the warning "<Warning>" has associated note "<Note>"
		Examples:
			| Patient   | Warning            | Note                                                                                                                        |
			| patient16 | confidential-items | Items excluded due to confidentiality and/or patient preferences.                                                           |
			| patient13 | data-in-transit    | Patient record transfer from previous GP practice not yet complete; information recorded before dd-Mmm-yyyy may be missing. |

	@1.5.0-IncrementalAndRegression
	Scenario: Retrieve the Referrals data structured record with period dates equal to current date expected success and no operation outcome
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the Referrals data parameter with current date
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate success
		And check the response does not contain an operation outcome

	Scenario: Retrieve the Referrals data structured record with valid start date and no end date expected success and no operation outcome
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the Referrals data parameter with valid start date and no end date
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate success
		And check the response does not contain an operation outcome

	Scenario: Retrieve the Referrals data structured record with no start date and no end date expected success and no operation outcome
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the Referrals data parameter with no start date and no end date
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate success
		And check the response does not contain an operation outcome

	@1.5.0-IncrementalAndRegression
	Scenario: Retrieve Referrals data structured record section for an invalid Identifier System
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an invalid Identifier System
		And I add the Referrals parameter
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

	@1.5.0-IncrementalAndRegression
	Scenario: Retrieve Referrals data structured record section for an empty Identifier System
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an empty Identifier System
		And I add the Referrals parameter
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

	@1.5.0-IncrementalAndRegression
	Scenario: Retrieve Referrals data structured record for a patient that has sensitive flag
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient9"
		And I add the Referrals parameter
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response status code should be "404"
		#And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND" and display "Patient not found"

	@1.5.0-IncrementalAndRegression
	Scenario Outline: Retrieve the Referrals data structured record with invalid dates expected failure
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the Referrals data parameter with date permutations "<startDate>" and "<endDate>"
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
		Examples:
			| startDate                 | endDate                   | Parameter        | PartParameter        |
			| 2014                      | 2016-01-01                | includeReferrals | referralSearchPeriod |
			| 2014-02                   | 2014-08-20                | includeReferrals | referralSearchPeriod |
			| 2015-10-23T11:08:32       | 2016-11-01                | includeReferrals | referralSearchPeriod |
			| 2015-10-23T11:08:32+00:00 | 2019-10-01                | includeReferrals | referralSearchPeriod |
			| 2014-01-01                | 2016                      | includeReferrals | referralSearchPeriod |
			| 2014-02-01                | 2014-08                   | includeReferrals | referralSearchPeriod |
			| 2015-10-01                | 2016-11-23T11:08:32       | includeReferrals | referralSearchPeriod |
			| 2014-01-01                | 2015-10-23T11:08:32+00:00 | includeReferrals | referralSearchPeriod |

	@1.5.0-IncrementalAndRegression
	Scenario: Retrieve the Referrals data structured record with referralSearchPeriod in future expected failure
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the Referrals data parameter with future start date
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

	@1.5.0-IncrementalAndRegression
	Scenario: Retrieve the Referrals data structured record startDate after endDate expected failure
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the Referrals data parameter start date after endDate
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
