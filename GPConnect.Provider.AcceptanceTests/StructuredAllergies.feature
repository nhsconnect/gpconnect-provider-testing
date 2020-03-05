@structured @structuredallergies @1.3.2-Full-Pack
Feature: StructuredAllergies

@1.3.1-IncrementalAndRegression @1.3.2-IncrementalAndRegression
Scenario Outline: Retrieve the allergy structured record section for a patient including resolved allergies no problems associated
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the allergies parameter with resolvedAllergies set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the AllergyIntolerance should be valid
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid
		And the Bundle should be valid for patient "<Patient>"
		And check that the bundle does not contain any duplicate resources
		And the Bundle should contain "2" lists
		And the Bundle should contain a list with the title "Allergies and adverse reactions"
		And the Bundle should contain a list with the title "Ended allergies"
		And the Bundle should contain the correct number of allergies
		And the Lists are valid for a patient with allergies
		And check the response does not contain an operation outcome
	Examples:
		| Patient   |
		| patient3  |
		| patient4  |
		| patient6  |
		| patient7  |
		| patient8  |
# git hub ref 144		| patient11 |
		| patient12 |
		| patient13 |
## removed github ref 91 		| patient15 |

@1.3.2-IncrementalAndRegression
Scenario Outline: Retrieve the allergy structured record section for a patient including resolved allergies with linked Problems
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the allergies parameter with resolvedAllergies set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the AllergyIntolerance should be valid
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid
		And the Bundle should be valid for patient "<Patient>"
		And check that the bundle does not contain any duplicate resources
		And the Bundle should contain "3" lists
		And the Bundle should contain a list with the title "Allergies and adverse reactions"
		And the Bundle should contain a list with the title "Ended allergies"
		And the Bundle should contain the correct number of allergies
		And the Lists are valid for a patient with allergies
		And check the response does not contain an operation outcome
		And I Check The Problems List
		And I Check The Problems List Does Not Include Not In Use Fields
		And I Check The Problems Resources are Valid
		And I check The Problem Resources Do Not Include Not In Use Fields
		And Check a Problem is linked to an "AllergyIntolerance" that is also included in the response with its list
	Examples:
		| Patient   |
		| patient2  |

@1.3.1-IncrementalAndRegression @1.3.2-IncrementalAndRegression
Scenario Outline: Retrieve the allergy structured record for a patient with no problems and excluding resolved allergies
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the allergies parameter with resolvedAllergies set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid
		And the Bundle should be valid for patient "<Patient>"
		And the Bundle should contain "1" lists
		And the Bundle should contain a list with the title "Allergies and adverse reactions"
		And the Bundle should not contain a list with the title "Ended allergies"
		And the AllergyIntolerance should be valid
		And the Bundle should contain the correct number of allergies
		And the Lists are valid for a patient with allergies
		And check the response does not contain an operation outcome
	Examples:
		| Patient   |
		| patient3  |
		| patient4  |
		| patient6  |
		| patient7  |
		| patient8  |
# git hub ref 144		| patient11 |
		| patient12 |
		| patient13 |
## removed github ref 91 		| patient15 |

@1.3.2-IncrementalAndRegression
Scenario Outline: Retrieve the allergy structured record for a patient with problems linked but excluding resolved allergies
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the allergies parameter with resolvedAllergies set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid
		And the Bundle should be valid for patient "<Patient>"
		And check that the bundle does not contain any duplicate resources
		And the Bundle should contain "2" lists
		And the Bundle should contain a list with the title "Allergies and adverse reactions"
		And the Bundle should not contain a list with the title "Ended allergies"
		And the AllergyIntolerance should be valid
		And the Bundle should contain the correct number of allergies
		And the Lists are valid for a patient with allergies
		And check the response does not contain an operation outcome
		And I Check The Problems List
		And I Check The Problems List Does Not Include Not In Use Fields
		And I Check The Problems Resources are Valid
		And I check The Problem Resources Do Not Include Not In Use Fields
		And Check a Problem is linked to an "AllergyIntolerance" that is also included in the response with its list

	Examples:
		| Patient   |
		| patient2  |

Scenario: Retrieve the allergy structured record section including resolved allergies for a patient without any allergies
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the allergies parameter with resolvedAllergies set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid
		And the Bundle should be valid for patient "patient1"
		And the Bundle should contain "2" lists
		And the Bundle should contain a list with the title "Allergies and adverse reactions"
		And the Bundle should contain a list with the title "Ended allergies"
		And the Lists are valid for a patient with no allergies
		And the List of AllergyIntolerances should be valid

Scenario: Retrieve the allergy structured record section excluding resolved allergies for a patient without any allergies
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the allergies parameter with resolvedAllergies set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid
		And the Bundle should be valid for patient "patient1"
		And the Bundle should contain "1" lists
		And the Bundle should contain a list with the title "Allergies and adverse reactions"
		And the Lists are valid for a patient with no allergies
		And the List of AllergyIntolerances should be valid

Scenario: Retrieve the allergy structured record section including resolved allergies for a patient with no allergies coding
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient5"
		And I add the allergies parameter with resolvedAllergies set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid
		And the Bundle should be valid for patient "patient5"
		And the Bundle should contain "2" lists
		And the Bundle should contain a list with the title "Allergies and adverse reactions"
		And the Bundle should contain a list with the title "Ended allergies"
		And the Lists are valid for a patient with explicit no allergies coding
		And the List of AllergyIntolerances should be valid

Scenario: Retrieve the allergy structured record section excluding resolved allergies for a patient with no allergies coding
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient5"
		And I add the allergies parameter with resolvedAllergies set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid
		And the Bundle should be valid for patient "patient5"
		And the Bundle should contain "1" lists
		And the Bundle should contain a list with the title "Allergies and adverse reactions"
		And the Lists are valid for a patient with explicit no allergies coding
		And the List of AllergyIntolerances should be valid

@1.3.1-IncrementalAndRegression
Scenario: Retrieve the allergy structured record section for a patient without the resolved allergies parameter expected failure
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the allergies parameter without mandatory part parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

#SJD 06/09/2019 #295 this is now accepted under forward compatability for 1.3.0
@1.3.1-IncrementalAndRegression
Scenario: Retrieve the allergy structured record with additional include unknown prescription issues parameter expected success with operational outcome
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the allergies parameter with mandatory part parameter and includePrescriptionIssues
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And Check the operation outcome returns the correct text and diagnotics "includePrescriptionIssues"
		And Check the number of issues in the operation outcome "1"

#SJD 06/09/2019 #295 this is now accepted under forward compatability for 1.3.0
Scenario: Retrieve the allergy structured record section for a patient with an uknown parameter expected success
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add an unknown allergies parameter name
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And Check the operation outcome returns the correct text and diagnotics "inlcudeUnknownAllergies"
		And Check the number of issues in the operation outcome "1"

Scenario: Retrieve the allergy structured record section for an invalid NHS number
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for an invalid NHS Number
		And I add the allergies parameter with resolvedAllergies set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Retrieve the allergy structured record section for an empty NHS number
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter with an empty NHS Number
		And I add the allergies parameter with resolvedAllergies set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Retrieve the allergy structured record section for an invalid Identifier System
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an invalid Identifier System
		And I add the allergies parameter with resolvedAllergies set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Retrieve the allergy structured record section for an empty Identifier System
Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an empty Identifier System
		And I add the allergies parameter with resolvedAllergies set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

@1.3.1-IncrementalAndRegression
#SJD 04/10/2019 changed the response code as per specification for invalid parameter		
Scenario: Retrieve the allergy structured record section for an invalid parameter type
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" using an invalid parameter type
		And I add the allergies parameter with resolvedAllergies set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

#SJD 06/09/2019 #295 this is now accepted under forward compatability for 1.3.0
@1.3.1-IncrementalAndRegression
Scenario: Retrieve the allergy structured record section for a patient with additional timePeriod parameter expected success
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the allergies parameter with mandatory parameter and additional parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And Check the operation outcome returns the correct text and diagnotics "TimePeriod"
		And Check the number of issues in the operation outcome "1"

#SJD 06/09/2019 #295 this is now accepted under forward compatability for 1.3.0
@1.3.1-IncrementalAndRegression
Scenario: Retrieve the allergy structured record section for a patient with additional start date parameter expected success
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the allergies parameter with mandatory part parameter start date
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And Check the operation outcome returns the correct text and diagnotics "TimePeriod"
		And Check the number of issues in the operation outcome "1"

Scenario: Retrieve the allergy structured record section for a patient with recorder
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the allergies parameter with resolvedAllergies set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response should contain the recorder reference

Scenario: Check allergy warning code is populated for a patient
	Given I configure the default "GpcGetStructuredRecord" request 
		And I add an NHS Number parameter for "patient17"
		And I add the allergies parameter with resolvedAllergies set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the Bundle should contain "2" lists
		And the Bundle should contain a list with the title "Allergies and adverse reactions"
		And the Bundle should contain a list with the title "Ended allergies"
		And the Lists are valid for a patient without allergies

	# Added 1.2.0 RMB 15/8/2018

Scenario: Check allergy legacy endReason
	Given I configure the default "GpcGetStructuredRecord" request 
		And I add an NHS Number parameter for "patient16"
		And I add the allergies parameter with resolvedAllergies set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the Bundle should contain "2" lists
		And the Bundle should contain a list with the title "Allergies and adverse reactions"
		And the Bundle should contain a list with the title "Ended allergies"
		And the List of AllergyIntolerances should be valid
		And the Lists are valid for a patient with legacy endReason

# Added for github ref 110 (Demonstrator)
# 1.2.1 RMB 15/10/2018

Scenario:  structured record for a patient that is not in the database 
	Given I configure the default "GpcGetStructuredRecord" request 
		And I add an NHS Number parameter for "patientNotInSystem"
		And I add the allergies parameter with resolvedAllergies set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response status code should be "404"		
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

#PG 30/8/2019 - #289
@1.3.1-IncrementalAndRegression
Scenario Outline: Structured Allergies Patient Has multiple Warnings and Associated Notes
	Given I configure the default "GpcGetStructuredRecord" request 
		And I add an NHS Number parameter for "patient16"
		And I add the allergies parameter with resolvedAllergies set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the Bundle should contain "1" lists
		And Check the list contains the following warning "<Warning>"
		And Check the warning "<Warning>" has associated note "<Note>"
	Examples:
	| Warning             | Note                                                                                                                       |
	| confidential-items   | Items excluded due to confidentiality and/or patient preferences.                                                           |
	| data-in-transit      | Patient record transfer from previous GP practice not yet complete; information recorded before dd-Mmm-yyyy may be missing. |
