@Structured @StructuredProblems @1.5.0-Full-Pack
Feature: StructuredProblems

	# These Tests are only Testing this Structured Area in isolation and Not with other Areas or Combinations of Include Parameters
	# Tests around Multiple Structured Areas in one Request are tested in the MultipleRequests Feature


	Scenario Outline: Verify response for a Patient with Problems linked to some clinical items
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the Problems parameter
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
		And check that the bundle does not contain any duplicate resources
		And I Check The Primary Problems List
		And I Check The Primary Problems List Does Not Include Not In Use Fields
		And I Check The Problems Resources are Valid
		And I check The Problem Resources Do Not Include Not In Use Fields
		And check the response does not contain an operation outcome
		And Check a Problem is Linked to a MedicationRequest resource that has been included in the response
		And Check the MedicationRequests have a link to a medication that has been included in response
		And Check there is a MedicationStatement resource that is linked to the MedicationRequest and Medication
		And Check a Problem is linked to an "Observation" that is also included in the response with its list
		And Check that a Problem is linked via context to a consultation but only a reference is sent in response
		And I Check the Problems Uncategorised Secondary List is Valid
		And I Check the Problems Medications Secondary List is Valid
		Examples:
			| Patient   |
			| patient2  |
			| patient32 |

	#Expect this test to fail for TPP as they do not support Problems linked to Problems

	Scenario Outline: Verify response for a Patient with Problems linked to other Problems
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the Problems parameter
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
		And check that the bundle does not contain any duplicate resources
		And I Check that a problem is linked to another problem
		And I Check The Primary Problems List
		And I Check The Primary Problems List Does Not Include Not In Use Fields
		And I Check The Problems Resources are Valid
		And I check The Problem Resources Do Not Include Not In Use Fields
		And check the response does not contain an operation outcome
		Examples:
			| Patient   |
			| patient2  |
			| patient32 |


	Scenario Outline: Retrieve problems structured record with status partParameter expected success
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the problems parameter with filterStatus "<value>"
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
		And check the response does not contain an operation outcome
		Examples:
			| value    | Patient   |
			| active   | patient2  |
			| inactive | patient32 |
			| active   | patient2  |
			| inactive | patient32 |

	Scenario Outline: Retrieve problems structured record with significance partParameter expected success
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the problems parameter with filterSignificance "<value>"
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
		And I Check The Primary Problems List
		And I Check The Primary Problems List Does Not Include Not In Use Fields
		And I Check The Problems Resources are Valid
		And I check The Problem Resources Do Not Include Not In Use Fields
		And check the response does not contain an operation outcome
		Examples:
			| value | Patient   |
			| major | patient2  |
			| minor | patient32 |

	Scenario Outline: Retrieve problems structured record with status and significance partParameter expected success
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the problems parameter including status and significance value "<statusValue>" "<sigValue>"
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
		And I Check The Primary Problems List
		And I Check The Primary Problems List Does Not Include Not In Use Fields
		And I Check The Problems Resources are Valid
		And I check The Problem Resources Do Not Include Not In Use Fields
		And check the response does not contain an operation outcome
		Examples:
			| statusValue | sigValue | Patient   |
			| active      | major    | patient2  |
			| active      | minor    | patient32 |
			| inactive    | major    | patient2  |
			| inactive    | minor    | patient32 |

	Scenario: Retrieve problems structured record for a patient that has no problems data
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient4"
		And I add the Problems parameter
		And I add the medication parameter with includePrescriptionIssues set to "true"
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
		And I Check The Primary Problems List
		And check the response does not contain an operation outcome
		And check structured list contains a note and emptyReason when no data in section

	Scenario Outline: Retrieve problems structured record for a patient that has repeating pair values expected success
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the problems parameter including repeating filter pairs
		And I add the medication parameter with includePrescriptionIssues set to "true"
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
		And check the response does not contain an operation outcome
		Examples:
			| Patient   |
			| patient2  |
			| patient32 |

	Scenario Outline: Retrieve problems structured record with invalid status partParameter expected failure
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the problems parameter with filterStatus "<value>"
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
		Examples:
			| value      |
			| resolved   |
			| recurrence |
			| remission  |
			| null       |
			| off        |

	Scenario Outline: Retrieve problems structured record with invalid significance partParameter expected failure
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the problems parameter with filterSignificance "<value>"
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
		Examples:
			| value |
			| HIGH  |
			| null  |
			| off   |

	Scenario Outline: Retrieve problems structured record with invalid status and significance partParameter expected failure
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the problems parameter including status and significance value "<statusValue>" "<sigValue>"
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
		Examples:
			| statusValue | sigValue |
			| invalid     | invalid  |
			| active      | low      |
			| resolved    | major    |

	Scenario: Retrieve Problems structured record with made up partParameter expected success
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add a madeUpProblems part parameter
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate success
		And Check the operation outcome returns the correct text and diagnotics "madeUpProblems"
		And Check the number of issues in the operation outcome "1"

	Scenario: Retrieve Problems structured record section for an invalid NHS number
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for an invalid NHS Number
		And I add the Problems parameter
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

	Scenario: Retrieve Problems structured record section for an empty NHS number
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter with an empty NHS Number
		And I add the Problems parameter
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

	Scenario: Retrieve Problems structured record section for an invalid Identifier System
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an invalid Identifier System
		And I add the Problems parameter
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

	Scenario: Retrieve Problems structured record section for an empty Identifier System
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an empty Identifier System
		And I add the Problems parameter
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

	Scenario Outline: Retrieve Problems structured record for a patient that has sensitive flag
		Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the Problems parameter
		When I make the "GpcGetStructuredRecord" request
		Then the response status code should indicate failure
		And the response status code should be "404"
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"
		Examples:
			| Patient   |
			| patient9  |
			| patient32 |

