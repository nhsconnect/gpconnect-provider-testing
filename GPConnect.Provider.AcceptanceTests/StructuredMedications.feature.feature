@structuredrecord
Feature: StructuredMedications

@1.2.4 @1.3.1
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
		And there should only be one order request for acute prescriptions
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid
		And check the response does not contain an operation outcome

	Examples:
		| Patient   |
		| patient2  |
		| patient3  |
		| patient5  |
		| patient12 |
		| patient16 |

@1.2.4	@1.3.1
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
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid
		And check the response does not contain an operation outcome
	Examples:
		| Patient  |
		| patient2 |
		| patient3 |
		| patient5 |
		| patient12 |

Scenario: Retrieve the medication structured record section for a patient with no medications including issues
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient4"
		And I add the medication parameter with includePrescriptionIssues set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "patient4"
		And the Bundle should contain "1" lists
		And the List of MedicationStatements should be valid
		And the Medications should be valid
		And the Medication Statements should be valid
		And the Medication Requests should be valid
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid
		
Scenario: Retrieve the medication structured record section for a patient with no medications
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient4"
		And I add the medication parameter with includePrescriptionIssues set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "patient4"
		And the Bundle should contain "1" lists
		And the List of MedicationStatements should be valid
		And the Medications should be valid
		And the Medication Statements should be valid
		And the Medication Requests should be valid
		
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
		| patient2 |
		| patient3 |
		| patient5 |
		| patient12 |
		
#SJD 06/09/2019 #295 this is now accepted under forward compatability for 1.3.0
@1.2.4
Scenario: Retrieve the medication structured record section for a patient without the prescription issue parameter
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the medications parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And Check the operation outcome PARAMETER_NOT_FOUND for "includeMedication" and "includePrescriptionIssues"
		And Check the number of issues in the operation outcome "1"

#SJD 06/09/2019 #295 this is now accepted under forward compatability for 1.3.0
@1.2.4 @1.3.1
Scenario: Retrieve the medication structured record section for a patient with an invalid include parameter
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add an invalid medications parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And Check the operation outcome returns the correct text and diagnotics "includeInvalidMedications"
		And Check the number of issues in the operation outcome "1"

@1.2.4
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
		
@1.2.4
Scenario: Retrieve the medication structured record section for an invalid parameter type
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" using an invalid parameter type
		And I add the medication parameter with includePrescriptionIssues set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

@1.2.4
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
		| patient2 |
		| patient3 |
		| patient5 |
		| patient12 |

Scenario Outline: Retrieve the medication structured record section for a patient with a start date
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the medications parameter with a start date equal to current date
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
		| patient2 |
		| patient3 |
		| patient5 |
		| patient12 |
#
# github ref 127 end date tests removed RMB 5/11/2018
#Scenario Outline: Retrieve the medication structured record section for a patient with an end date
#	Given I configure the default "GpcGetStructuredRecord" request
#		And I add an NHS Number parameter for "<Patient>"
#		And I add the medications parameter with an end date
#	When I make the "GpcGetStructuredRecord" request
#	Then the response status code should indicate success
#		And the response should be a Bundle resource of type "collection"
#		And the response meta profile should be for "structured"
#		And the patient resource in the bundle should contain meta data profile and version id
#		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
#		And if the response bundle contains an organization resource it should contain meta data profile and version id
#		And the Bundle should be valid for patient "<Patient>"
#		And the Bundle should contain "1" lists
#		And the Medications should be valid
#		And the Medication Statements should be valid
#		And the Medication Requests should be valid
#		And the List of MedicationStatements should be valid
#		And the MedicationStatement dates are with the default period with start "false" and end "true"
#	Examples:
#		| Patient  |
#		| patient2 |
#		| patient3 |
#		| patient5 |
#		| patient12 |
#
#Scenario Outline: Retrieve the medication structured record section for a patient with invalid time period
#	Given I configure the default "GpcGetStructuredRecord" request
#		And I add an NHS Number parameter for "patient1"
#		And I set a medications period parameter start date to "<StartDate>" and end date to "<EndDate>"
#	When I make the "GpcGetStructuredRecord" request
#	Then the response status code should indicate failure
#		And the response should be a OperationOutcome resource
#	Examples:
#		| StartDate                 | EndDate                   |
#		| 2014                      | 2016-02-02                |
#		| 2014-02                   | 2016-02-02                |
#		| 2015-10-23T11:08:32       | 2016-02-02                |
#		| 2015-10-23T11:08:32+00:00 | 2016-02-02                |
#		| 2016-02-02                | 2017                      |
#		| 2016-02-02                | 2017-02                   |
#		| 2016-02-02                | 2017-10-23T11:08:32       |
#		| 2016-02-02                | 2017-10-23T11:08:32+00:00 |
#		| 2014-02-02                | 2012-02-02                |
#
# github ref 127
# RMB 5/11/2018
#SJD 06/09/2019 #295 amended invalid date formats to allow for forward compatability for 1.3.0
@1.2.4 @1.3.1
Scenario Outline: Retrieve the medication structured record section for a patient with invalid start date
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I set a medications period parameter start date to "<StartDate>" 
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource
Examples:
		| StartDate                  |
		| X2014                      |
		| X2014-02                   |
		| X2015-10-23T11:08:32       |
		| X2015-10-23T11:08:32+00:00 |
		|                            | 

#SJD 06/09/2019 #295 invalid date formats are now accepted under forward compatability for 1.3.0
@1.2.4 @1.3.1
Scenario Outline: Retrieve the medication structured record expected success with invalid date used includes an operation outcome 
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I set a medications period parameter start date to "<StartDate>" 
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And Check the operation outcome returns INVALID_PARAMETER for "<Parameter>" and "<PartParameter>"
		And Check the number of issues in the operation outcome "1"

	Examples:
		| StartDate                 | Parameter         | PartParameter            |
		| 2014                      | includeMedication | medicationSearchFromDate |
		| 2014-02                   | includeMedication | medicationSearchFromDate |
		| 2015-10-23T11:08:32       | includeMedication | medicationSearchFromDate |
		| 2015-10-23T11:08:32+00:00 | includeMedication | medicationSearchFromDate |

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

Scenario: Check warning code is populated for a patient
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the medication parameter with includePrescriptionIssues set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the Bundle should contain "1" lists
# RMB 13/9/2018 github ref 84 Replaced 'Medication List' with 'Medications and medical devices'
		And the Bundle should contain a list with the title "Medications and medical devices"
		And the Lists are valid for a patient without allergies

# Added for github ref 110 (Demonstrator)
# 1.2.1 RMB 15/10/2018

@1.2.4
Scenario:  structured record for a patient that is not in the database 
	Given I configure the default "GpcGetStructuredRecord" request 
	And I add an NHS Number parameter for "patientNotInSystem"
	And I add the medication parameter with includePrescriptionIssues set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"
#
# Added for github ref 129
# RMB 5/11/2018
#
@1.2.4
Scenario:  structured record for a patient that is deceased
	Given I configure the default "GpcGetStructuredRecord" request 
	And I add an NHS Number parameter for "patient18"
	And I add the medication parameter with includePrescriptionIssues set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

@1.2.4
Scenario:  structured record for a patient that has sensitive flag
	Given I configure the default "GpcGetStructuredRecord" request 
	And I add an NHS Number parameter for "patient9"
	And I add the medication parameter with includePrescriptionIssues set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

@1.2.4
Scenario:  structured record for a patient that has inactive flag
	Given I configure the default "GpcGetStructuredRecord" request 
	And I add an NHS Number parameter for "patient21"
	And I add the medication parameter with includePrescriptionIssues set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"


#PG 6/9/2019 - #289
@1.2.4 @1.3.1
Scenario Outline: Structured Medications Patient Has multiple Warnings and Associated Notes
	Given I configure the default "GpcGetStructuredRecord" request 
		And I add an NHS Number parameter for "patient16"
		And I add the medication parameter with includePrescriptionIssues set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the Bundle should contain "1" lists
		And the List of MedicationStatements should be valid
		And Check the list contains the following warning "<Warning>"
		And Check the warning "<Warning>" has associated note "<Note>"
	Examples:
	| Warning             | Note                                                                                                                       |
	| data-in-transit      | Patient record transfer from previous GP practice not yet complete; information recorded before dd-Mmm-yyyy may be missing. |

	#SJD 20/09/2019
	@1.3.1
	Scenario: Retrieve the medication structured record startDate in future expected success with an operation outcome
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the medications parameter with a start date greater than current date
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And Check the operation outcome returns INVALID_PARAMETER for "includeMedication" and "medicationSearchFromDate"
		And Check the number of issues in the operation outcome "1"
	
	
	