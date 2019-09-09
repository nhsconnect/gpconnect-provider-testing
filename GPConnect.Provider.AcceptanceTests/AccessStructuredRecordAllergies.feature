@structuredrecord
Feature: AccessStructuredRecordAllergies

@1.2.4
Scenario Outline: Retrieve the allergy structured record section for a patient including resolved allergies
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
		And the Bundle should contain "2" lists
		And the Bundle should contain a list with the title "Allergies and adverse reactions"
		And the Bundle should contain a list with the title "Ended allergies"
		And the Bundle should contain the correct number of allergies
		And the Lists are valid for a patient with allergies
	Examples:
		| Patient   |
		| patient2  |
		| patient3  |
		| patient4  |
		| patient6  |
		| patient7  |
		| patient8  |
# git hub ref 144		| patient11 |
		| patient12 |
		| patient13 |
## removed github ref 91 		| patient15 |

@1.2.4
Scenario Outline: Retrieve the allergy structured record section for a patient excluding resolved allergies
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
	Examples:
		| Patient   |
		| patient2  |
		| patient3  |
		| patient4  |
		| patient6  |
		| patient7  |
		| patient8  |
# git hub ref 144		| patient11 |
		| patient12 |
		| patient13 |
## removed github ref 91 		| patient15 |

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

#SJD 06/09/2019 #295 this is now accepted under forward compatability for 1.3.0
@1.2.4
Scenario: Retrieve the allergy structured record section for a patient without the resolved allergies parameter expected success
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the allergies parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And Check the operation outcome returns PARAMETER_NOT_FOUND "includeAllergies" and "includeResolvedAllergies"

#SJD 06/09/2019 #295 this is now accepted under forward compatability for 1.3.0
@1.2.4
Scenario: Retrieve the allergy structured record to include prescription issues parameter expected success
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the allergies parameter with includePrescriptionIssues
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And Check the operation outcome returns the correct text and diagnotics "includePrescriptionIssues"

#SJD 06/09/2019 #295 this is now accepted under forward compatability for 1.3.0
@1.2.4
Scenario: Retrieve the allergy structured record section for a patient with an uknown parameter expected success
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add an unknown allergies parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And Check the operation outcome returns the correct text and diagnotics "inlcudeUnknownAllergies"

@1.2.4
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

@1.2.4
Scenario: Retrieve the allergy structured record section for an empty Identifier System
Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an empty Identifier System
		And I add the allergies parameter with resolvedAllergies set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource
		
Scenario: Retrieve the allergy structured record section for an invalid parameter type
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" using an invalid parameter type
		And I add the allergies parameter with resolvedAllergies set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

#SJD 06/09/2019 #295 this is now accepted under forward compatability for 1.3.0
@1.2.4
Scenario: Retrieve the allergy structured record section for a patient with a timePeriod expected success
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the allergies parameter with a timePeriod
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And Check the operation outcome returns the correct text and diagnotics "TimePeriod"

#SJD 06/09/2019 #295 this is now accepted under forward compatability for 1.3.0
@1.2.4
Scenario: Retrieve the allergy structured record section for a patient with a start date only expected success
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the allergies parameter with a start date
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And Check the operation outcome returns the correct text and diagnotics "TimePeriod"

#SJD 06/09/2019 #295 this is now accepted under forward compatability for 1.3.0
@1.2.4
Scenario: Retrieve the allergy structured record section for a patient with an end date only expected success
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the allergies parameter with an end date
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And Check the operation outcome returns the correct text and diagnotics "TimePeriod"

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
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

	