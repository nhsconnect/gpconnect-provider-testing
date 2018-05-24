@structuredrecord
Feature: AccessStructuredRecordMedications

Scenario Outline: Retrieve the medication structured record section for a patient including prescription issues
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the medication parameter with includePrescriptionIssues set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "<Patient>"
		And the Bundle should contain "1" lists
		And the Medications should be valid
		And the Medication Statements should be valid
		And the Medication Requests should be valid
		And the List of MedicationStatements should be valid
		And order requests should have the same authoredOn date as their plan
		And there should only be one order request for acute prescriptions
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |
		| patient4 |
		| patient5 |
		| patient6 |
		| patient12 |
		
Scenario Outline: Retrieve the medication structured record section for a patient excluding prescription issues
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the medication parameter with includePrescriptionIssues set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "<Patient>"
		And the Bundle should contain "1" lists
		And the Medications should be valid
		And the Medication Statements should be valid
		And the Medication Requests should be valid
		And the List of MedicationStatements should be valid
		And the Medication Requests should not contain any issues
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |
		| patient4 |
		| patient5 |
		| patient6 |
		| patient12 |
		
Scenario Outline: Retrieve the structured record section for a patient without the medications parameter
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "<Patient>"
		And the response bundle should not contain any medications data
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |
		| patient4 |
		| patient5 |
		| patient6 |
		| patient12 |
		
Scenario: Retrieve the medication structured record section for a patient without the prescription issue parameter
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the medications parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Retrieve the medication structured record section for a patient with an invalid include parameter
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add an invalid medications parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Retrieve the medication structured record section for an invalid NHS number
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for an invalid NHS Number
		And I add the medication parameter with includePrescriptionIssues set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Retrieve the medication structured record section for an empty NHS number
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter with an empty NHS Number
		And I add the medication parameter with includePrescriptionIssues set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Retrieve the medication structured record section for an invalid Identifier System
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an invalid Identifier System
		And I add the medication parameter with includePrescriptionIssues set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Retrieve the medication structured record section for an empty Identifier System
Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an empty Identifier System
		And I add the medication parameter with includePrescriptionIssues set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource
		
Scenario: Retrieve the medication structured record section for an invalid parameter type
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" using an invalid parameter type
		And I add the medication parameter with includePrescriptionIssues set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario Outline: Retrieve the medication structured record section for a patient with a timePeriod
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the medications parameter with a timePeriod
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "<Patient>"
		And the Bundle should contain "1" lists
		And the Medications should be valid
		And the Medication Statements should be valid
		And the Medication Requests should be valid
		And the List of MedicationStatements should be valid
		And the MedicationStatement dates are with the default period with start "true" and end "true"
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |
		| patient4 |
		| patient5 |
		| patient6 |
		| patient12 |

Scenario Outline: Retrieve the medication structured record section for a patient with a start date
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the medications parameter with a start date
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "<Patient>"
		And the Bundle should contain "1" lists
		And the Medications should be valid
		And the Medication Statements should be valid
		And the Medication Requests should be valid
		And the List of MedicationStatements should be valid
		And the MedicationStatement dates are with the default period with start "true" and end "false"
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |
		| patient4 |
		| patient5 |
		| patient6 |
		| patient12 |

Scenario Outline: Retrieve the medication structured record section for a patient with an end date
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the medications parameter with an end date
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "<Patient>"
		And the Bundle should contain "1" lists
		And the Medications should be valid
		And the Medication Statements should be valid
		And the Medication Requests should be valid
		And the List of MedicationStatements should be valid
		And the MedicationStatement dates are with the default period with start "false" and end "true"
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |
		| patient4 |
		| patient5 |
		| patient6 |
		| patient12 |

Scenario Outline: Retrieve the medication structured record section for a patient with invalid time period
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I set a medications period parameter start date to "<StartDate>" and end date to "<EndDate>"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource
	Examples:
		| StartDate                 | EndDate                   |
		| 2014                      | 2016-02-02                |
		| 2014-02                   | 2016-02-02                |
		| 2015-10-23T11:08:32       | 2016-02-02                |
		| 2015-10-23T11:08:32+00:00 | 2016-02-02                |
		| 2016-02-02                | 2017                      |
		| 2016-02-02                | 2017-02                   |
		| 2016-02-02                | 2017-10-23T11:08:32       |
		| 2016-02-02                | 2017-10-23T11:08:32+00:00 |
		| 2014-02-02                | 2012-02-02                |

Scenario: Retrieve the medication structured record section for a patient with medication prescribed elsewhere
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient12"
		And I add the medication parameter with includePrescriptionIssues set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "patient12"
		And the Bundle should contain "1" lists
		And the Medications should be valid
		And the Medication Statements should be valid
		And the Medication Requests should be valid
		And the List of MedicationStatements should be valid
		And the MedicationStatement dates are with the default period with start "false" and end "true"
		And the MedicationStatement for prescriptions prescribed elsewhere should be valid

@Ignore @Manual
Scenario: Notes that are present in the record are formatted correctly
	# All patient notes and prescriber notes at authorisation(plan) and issue(order) level SHOULD be included in this field.
	# They SHOULD be concatenated and indicate the level the notes come from, e.g. 1st Issue and also be prefixed with either
	# ‘Patient Notes:’ or ‘Prescriber Notes:’ as appropriate.

@Ignore @Manual
Scenario: A request with an intent of order is generated for each issue of a medication
	# Each time the medication is issued then it SHOULD be represented using a MedicationRequest with the intent element set to order. 

@Ignore @Manual
Scenario: Degraded medications in the patient record have the correct degrade code and the original medication name 
	# Where degraded medication records arising from GP2GP record transfer are present in the patient record then these SHOULD be coded using the 
	# appropriate degrade code (196421000000109, Transfer-degraded medication entry) with the original medication name conveyed by CodeableConcept.text. 

@Ignore @Manual
Scenario: Medication mixtures are expressed in the correct format with the correct degradable code
	# In some systems it is possible to prescribe custom formulations compounded from other medications (extemporaneous preparations). 
	# Mixtures SHOULD be expressed using the degrade code (196421000000109, Transfer-degraded medication entry) with the constituents of the
	# mixture expressed via CodeableConcept.text.

@Ignore @Manual
Scenario: Medications recorded as free text and without a dm+d code are in the correct format with the correct degradable code
	# In some cases, drugs may be recorded as free text or may be present in the original system’s drug dictionary, but not in dm+d. 
	# Where no dm+d code is available to describe the medication then the medication code SHOULD be expressed using the degrade 
	# (196421000000109, Transfer-degraded medication entry) with the original drug name present in CodeableConcept.text

@Ignore @Manual
Scenario: Historical non-dm+d medication names are preserved correctly
	# It is possible for historic/legacy medications to be displayed with a name corresponding to the name in the original system’s drug dictionary 
	# rather than the dm+d name. This name SHOULD be preserved via CodeableConcept.text when representing the medication via resources. 
	# CodeableConcept.text is redundant when the displayed medication name on the original system and the dm+d name is identical, and, in these cases, 
	# CodeableConcept.text SHOULD be omitted.

@Ignore @Manual
Scenario: Amended and re-issued medications arehandled correctly
	# Where an authorisation is amended – for example, Proprietary/Generic switch, altered dates, change of quantities and so on, then the existing 
	# authorisation/plan SHOULD be stopped or discontinued, and an appropriate reason supplied via detectedIssue. 
	# A new authorisation SHOULD be created, in the form of a MedicationStatement and MedicationRequest with intent of plan, to hold the amended details. 
	# Subsequent issues of the medication SHOULD reference the amended authorisation rather than the previous version.

@Ignore @Manual
Scenario: An expired medication authorisation does not have a statusReason set
	#A statusReason SHOULD NOT be generated when an authorisation has simply expired (exceeded review date or number of issues).