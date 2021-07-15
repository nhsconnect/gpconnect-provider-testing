@Structured @StructuredMedications @1.5.0-Full-Pack @1.6.0-Full-Pack
Feature: StructuredMedications

# These Tests are only Testing this Structured Area in isolation and Not with other Areas or Combinations of Include Parameters
# Tests around Multiple Structured Areas in one Request are tested in the MultipleRequests Feature

@1.3.1-IncrementalAndRegression @1.3.2-IncrementalAndRegression
Scenario Outline: Retrieve the medication structured record section for a patient with no problems and including prescription issues
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
		And check that the bundle does not contain any duplicate resources
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
		And I Check There is No Problems Secondary Problems List
		And I Check No Problem Resources are Included

	Examples:
		| Patient   |
		| patient3  |
		| patient5  |
		| patient12 |
		| patient16 |

@1.3.2-IncrementalAndRegression
Scenario Outline: Retrieve the medication structured record section for a patient with problems linked and including prescription issues
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
		And check that the bundle does not contain any duplicate resources
		And the Bundle should contain "2" lists
		And the Medications should be valid
		And the Medication Statements should be valid
		And the Medication Requests should be valid
		And the List of MedicationStatements should be valid
		And there should only be one order request for acute prescriptions
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid
		And check the response does not contain an operation outcome
		And I Check The Problems Secondary Problems List
		And I Check The Problems Secondary Problems List Does Not Include Not In Use Fields
		And I Check The Problems Resources are Valid
		And I check The Problem Resources Do Not Include Not In Use Fields
		And Check a Problem is Linked to a MedicationRequest resource that has been included in the response
		And Check the MedicationRequests have a link to a medication that has been included in response
		And Check there is a MedicationStatement resource that is linked to the MedicationRequest and Medication
		And Check the Medications List resource has been included in response
	Examples:
		| Patient   |
		| patient2  |

@1.3.1-IncrementalAndRegression @1.3.2-IncrementalAndRegression
Scenario Outline: Retrieve the medication structured record for a patient with no problems and excluding prescription issues
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
		And check that the bundle does not contain any duplicate resources
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
		And I Check There is No Problems Secondary Problems List
		And I Check No Problem Resources are Included
	Examples:
		| Patient  |
		| patient3 |
		| patient5 |
		| patient12 |

@1.3.2-IncrementalAndRegression
Scenario Outline: Retrieve the medication structured record for a patient with problems linked and excluding prescription issues
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
		And check that the bundle does not contain any duplicate resources
		And the Bundle should contain "2" lists
		And the Medications should be valid
		And the Medication Statements should be valid
		And the Medication Requests should be valid
		And the List of MedicationStatements should be valid
		And the Medication Requests should not contain any issues
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid
		And check the response does not contain an operation outcome
		And I Check The Problems Secondary Problems List
		And I Check The Problems Secondary Problems List Does Not Include Not In Use Fields
		And I Check The Problems Resources are Valid
		And I check The Problem Resources Do Not Include Not In Use Fields
		And Check a Problem is Linked to a MedicationRequest resource that has been included in the response
		And Check the MedicationRequests have a link to a medication that has been included in response
		And Check there is a MedicationStatement resource that is linked to the MedicationRequest and Medication
		And Check the Medications List resource has been included in response
	Examples:
		| Patient  |
		| patient2 |

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
		
@1.3.1-IncrementalAndRegression @1.3.2-IncrementalAndRegression
#PG 19-2-2020 - 1.3.2 - param no longer mandatory, so checking call works
Scenario: Retrieve the medication structured record section for a patient without the includePrescriptionIssue parameter
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the medications parameter without mandatory partParameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response meta profile should be for "structured"
		

#SJD 06/09/2019 #295 this is now accepted under forward compatability for 1.3.0
@1.3.1-IncrementalAndRegression
Scenario: Verify that when the medication parameter is labelled incorrectly with correct mandatory partParameter returns success
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add an incorrectly named medication parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And Check the operation outcome returns the correct text and diagnotics "includeInvalidMedications"
		And Check the number of issues in the operation outcome "1"

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
		
@1.3.1-IncrementalAndRegression
#SJD 04/10/2019 changed the response code as per specification for invalid parameter	
Scenario: Retrieve the medication structured record section for an invalid parameter type
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" using an invalid parameter type
		And I add the medication parameter with includePrescriptionIssues set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Retrieve the medication structured record section for a patient with a timePeriod
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the medications parameter to search from "3" years back
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "patient2"		
		And the Medications should be valid
		And the Medication Statements should be valid
		And the Medication Requests should be valid
		And the List of MedicationStatements should be valid
		And the MedicationStatement EffectiveDate is Greater Than Search Date of "3" years ago	

@1.3.1-IncrementalAndRegression
Scenario Outline: Retrieve the medication structured record section for a patient with invalid date values
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I set a medications period parameter start date to "<StartDate>" 
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
Examples:
		| StartDate                  |
		| X2014                      |
		| X2014-02                   |
		| X2015-10-23T11:08:32       |
		| X2015-10-23T11:08:32+00:00 |
		|                            |
		| 2014                       |
		| 2014-02                    |
		| null                       |

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
		And the MedicationStatement for prescriptions prescribed elsewhere should be valid

Scenario: Check warning code is populated for a patient
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the medication parameter with includePrescriptionIssues set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		#PG 1.3.2 - Updated to check for extra problems list for this patient.
		And the Bundle should contain "2" lists
# RMB 13/9/2018 github ref 84 Replaced 'Medication List' with 'Medications and medical devices'
		And the Bundle should contain a list with the title "Medications and medical devices"
		And the Lists are valid for a patient without allergies

# Added for github ref 110 (Demonstrator)
# 1.2.1 RMB 15/10/2018

Scenario: Structured record for a patient that is not in the database 
	Given I configure the default "GpcGetStructuredRecord" request 
		And I add an NHS Number parameter for "patientNotInSystem"
		And I add the medication parameter with includePrescriptionIssues set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response status code should be "404"
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"
#
# Added for github ref 129
# RMB 5/11/2018
#
Scenario: Structured record for a patient that is deceased
	Given I configure the default "GpcGetStructuredRecord" request 
		And I add an NHS Number parameter for "patient18"
		And I add the medication parameter with includePrescriptionIssues set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response status code should be "404"
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

Scenario:  structured record for a patient that has sensitive flag
	Given I configure the default "GpcGetStructuredRecord" request 
		And I add an NHS Number parameter for "patient9"
		And I add the medication parameter with includePrescriptionIssues set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response status code should be "404"
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

Scenario:  structured record for a patient that has inactive flag
	Given I configure the default "GpcGetStructuredRecord" request 
		And I add an NHS Number parameter for "patient21"
		And I add the medication parameter with includePrescriptionIssues set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		Then the response status code should be "404"
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"


#PG 6/9/2019 - #289
@1.3.1-IncrementalAndRegression
Scenario Outline: Structured Medications Patient Has multiple Warnings and Associated Notes
	Given I configure the default "GpcGetStructuredRecord" request 
		And I add an NHS Number parameter for "patient13"
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

	@1.3.1-IncrementalAndRegression
	Scenario: Retrieve the medication structured record with startDate in the future - expected failure
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the medications parameter with a start date greater than current date
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
	
	#Duplicate Part Parm check - Martin Hillyand advised to check INVALID_RESOURCE returned
	@1.3.2-IncrementalAndRegression
	Scenario: Attempt to Retrieve medications using duplicate part param expect failure
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient3"
		And I add a duplicate medication part parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should be "422"
	And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"
