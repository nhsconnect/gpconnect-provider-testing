﻿@structuredrecord @1.2.8-Full-Pack
Feature: AccessStructuredRecordMedications

#SJD 06/02/20 changed test to reflect parameter.part cardinality change to 0..1
Scenario Outline: Retrieve the medication structured record section for a patient without the parameter part
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the medications parameter
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
	Examples:
		| Patient   |
		| patient2  |
		| patient3  |
		| patient5  |
		| patient12 |
		| patient16 |

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
	Examples:
		| Patient  |
		| patient2 |
		| patient3 |
		| patient5 |
		| patient12 |

Scenario: Retrieve the medication structured record section for a patient excludes the parameter part value as now optional
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient4"
		And I add the medications parameter
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
Scenario: Retrieve the medication structured record section for a patient with an invalid include parameter
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add an invalid medications parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And Check the operation outcome returns the correct text and diagnotics "includeInvalidMedications"

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
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

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

Scenario Outline: Retrieve the medication structured record expected failure with invalid date used operation outcome 
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I set a medications period parameter start date to "<StartDate>" 
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource
	
Examples:
		| StartDate                  |
		| 2014                       |
		| 2014-02                    |
		| 2015-10-23T11:08:32        |
		| 2015-10-23T11:08:32+00:00  |
		| X2014                      |
		| X2014-02                   |
		| X2015-10-23T11:08:32       |
		| X2015-10-23T11:08:32+00:00 |
		|                            |

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

Scenario:  structured record for a patient that is not in the database 
	Given I configure the default "GpcGetStructuredRecord" request 
	And I add an NHS Number parameter for "patientNotInSystem"
	And I add the medication parameter with includePrescriptionIssues set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

Scenario:  structured record for a patient that is deceased
	Given I configure the default "GpcGetStructuredRecord" request 
	And I add an NHS Number parameter for "patient18"
	And I add the medication parameter with includePrescriptionIssues set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

Scenario:  structured record for a patient that has sensitive flag
	Given I configure the default "GpcGetStructuredRecord" request 
	And I add an NHS Number parameter for "patient9"
	And I add the medication parameter with includePrescriptionIssues set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

Scenario:  structured record for a patient that has inactive flag
	Given I configure the default "GpcGetStructuredRecord" request 
	And I add an NHS Number parameter for "patient21"
	And I add the medication parameter with includePrescriptionIssues set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

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
	| data-awaiting-filing | Patient data may be incomplete as there is data supplied by a third party awaiting review before becoming available.        |

Scenario: Retrieve the medication structured record from request with empty values for the parameter parts expect Failure
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I set a medications period parameter start date to "" 
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"