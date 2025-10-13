@Structured @StructuredConsultations @1.5.0-Full-Pack
Feature: StructuredConsultations

	# These Tests are only Testing this Structured Area in isolation and Not with other Areas or Combinations of Include Parameters
	# Tests around Multiple Structured Areas in one Request are tested in the MultipleRequests Feature

	@1.5.0-IncrementalAndRegression
	Scenario Outline: Scenario Outline name: Verify Consultations Response for a Patient with Topic or Headings linked to clinical items and a problem
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the includeConsultations parameter only
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		#added this step in accordance with the new changes
		And if the response bundle contains a location resource it should contain meta data profile and version id
		And check that the bundle does not contain any duplicate resources
		And the Bundle should be valid for patient "<Patient>"
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid
		And I Check the Consultations List is Valid
		And The Consultations List Does Not Include Not In Use Fields
		And I Check the Encounters are Valid
		And I Check the Encounters Do Not Include Not in Use Fields
		And I Check the Consultation Lists are Valid
		And I Check All The Consultation Lists Do Not Include Not In Use Fields
		And I Check the Topic Lists are Valid
		And I Check one Topic is linked to a problem
		And I Check the Heading Lists are Valid
		And check the response does not contain an operation outcome
		And I Check The Problems Resources are Valid
		And I check The Problem Resources Do Not Include Not In Use Fields
		And I Check that a Topic or Heading is linked to an "Observation" and that is included in response with a list
		And I Check that a Topic or Heading is linked to an "MedicationRequest" and that is included in response with a list
		And Check the MedicationRequests have a link to a medication that has been included in response
		And Check there is a MedicationStatement resource that is linked to the MedicationRequest and Medication
		And I Check the Consultation Medications Secondary List is Valid
		And I Check the Consultation Uncategorised Secondary List is Valid
		And I Check The Problems Secondary Problems List
		And I Check The Problems Secondary Problems List Does Not Include Not In Use Fields
		Examples:
			| Patient   |
			| patient2  |
			| patient31 |

	Scenario Outline: Verify Consultations structured record for a Patient includeConsultation and consultationSearchPeriod partParameter
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the consultation parameter with consultationSearchPeriod partParameter
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
		And I Check the Consultations List is Valid
		And The Consultations List Does Not Include Not In Use Fields
		And I Check the Encounters are Valid
		And I Check the Encounters Do Not Include Not in Use Fields
		And I Check the Consultation Lists are Valid
		And I Check All The Consultation Lists Do Not Include Not In Use Fields
		And I Check the Topic Lists are Valid
		And I Check the Heading Lists are Valid
		And check the response does not contain an operation outcome
		Examples:
			| Patient   |
			| patient2  |
			| patient31 |

	Scenario Outline: Verify Consultations structured record for a Patient includeConsultation and consultationsMostRecent partParameter
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the consultation parameter with consultationsMostRecent partParameter
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
		And I Check the Consultations List is Valid
		And The Consultations List Does Not Include Not In Use Fields
		And I Check the Encounters are Valid
		And I Check the Encounters Do Not Include Not in Use Fields
		And I Check the Consultation Lists are Valid
		And I Check All The Consultation Lists Do Not Include Not In Use Fields
		And I Check the Topic Lists are Valid
		And I Check the Heading Lists are Valid
		And check the response does not contain an operation outcome
		Examples:
			| Patient   |
			| patient2  |
			| patient31 |

	Scenario: Retrieve consultations structured record for a patient that has no consultation data
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient4"
		And I add the consultation parameter with consultationSearchPeriod partParameter
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

	Scenario Outline: Scenario Outline name: Retrieve consultations structured record with startDate only expected success
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the consultation parameter with startDate only
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
		And I Check the Consultations List is Valid
		And The Consultations List Does Not Include Not In Use Fields
		And I Check the Encounters are Valid
		And I Check the Encounters Do Not Include Not in Use Fields
		And I Check the Consultation Lists are Valid
		And I Check All The Consultation Lists Do Not Include Not In Use Fields
		And I Check the Topic Lists are Valid
		And I Check the Heading Lists are Valid
		And check the response does not contain an operation outcome
		Examples:
			| Patient   |
			| patient2  |
			| patient31 |


	Scenario Outline: Retrieve consultations structured record with endDate value only expected success
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the consultation parameter with endDate only
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
		And I Check the Consultations List is Valid
		And The Consultations List Does Not Include Not In Use Fields
		And I Check the Encounters are Valid
		And I Check the Encounters Do Not Include Not in Use Fields
		And I Check the Consultation Lists are Valid
		And I Check All The Consultation Lists Do Not Include Not In Use Fields
		And I Check the Topic Lists are Valid
		And I Check the Heading Lists are Valid
		And check the response does not contain an operation outcome
		Examples:
			| Patient   |
			| patient2  |
			| patient31 |

	Scenario: Retrieve consultations structured record section for an invalid NHS number
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for an invalid NHS Number
		And I add the includeConsultations parameter only
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

	Scenario: Retrieve consultations structured record section for an empty NHS number
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter with an empty NHS Number
		And I add the includeConsultations parameter only
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

	Scenario: Retrieve consultations structured record section for an invalid Identifier System
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an invalid Identifier System
		And I add the includeConsultations parameter only
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

	Scenario: Retrieve consultations structured record section for an empty Identifier System
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an empty Identifier System
		And I add the includeConsultations parameter only
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

	Scenario: Retrieve consultations structured record for a patient that has sensitive flag
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient9"
		And I add the includeConsultations parameter only
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response status code should be "404"
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

	Scenario: Retrieve consultations Structured record where consultationSearchPeriod part parameter is greater than the current date expected failure
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the consultation parameter with consultationSearchPeriod partParameter in the future
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

	Scenario: Retrieve consultations structured record where start date of the consultationSearchPeriod part parameter is greater than the end date expected failure
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the consultation parameter with consultationSearchPeriod partParameter startDate greater than endDate
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

	Scenario: Retrieve Consultations structured record with both partparameters consultationSearchPeriod and includeNumberOfMostRecent applied expected failure
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the consultation parameter with both partParameters
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

	Scenario: Retrieve Consultations structured record malformed with partParameter only expected success
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add malformed Consultations request partParameter only
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate success
		And Check the operation outcome returns the correct text and diagnotics "consultationSearchPeriod"
		And Check the number of issues in the operation outcome "1"

	Scenario Outline: Retrieve the Consultations structured record section with invalid date values
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I set a consultations period parameter "<startDate>" to "<endDate>"
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
		Examples:
			| startDate                  | endDate                    |
			| StartDate                  | endDate                    |
			| X2014                      | 2015-12-01                 |
			| X2014-02                   | 2015-12-01                 |
			| X2015-10-23T11:08:32       | 2016-01-12                 |
			| X2015-10-23T11:08:32+00:00 | 2015-10-30                 |
			|                            | 2019-01-01                 |
			| 2014                       | 2015-01-01                 |
			| 2014-02                    | 2016-01-30                 |
			| null                       | 2019-11-11                 |
			| null                       | null                       |
			|                            |                            |
			| 2015-12-01                 |                            |
			| 2015-12-01                 | X2016-02                   |
			| 2015-12-01                 | X2016-10-23T11:08:32       |
			| 2015-12-01                 | X2016-10-23T11:08:32+00:00 |
			| 2015-12-01                 | 2015                       |
			| 2015-12-01                 | 2016-02                    |
			| 2015-12-01                 | null                       |
			| 2018                       | 2019                       |
