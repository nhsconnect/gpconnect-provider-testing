@structuredrecord
Feature: AccessStructuredRecordMedications

Scenario Outline: Retrieve the medication structured record section for a patient including prescirption issues
	Given I configure the default "GpcGetStructuredRecord" request
		And I set the JWT Requested Record to the NHS Number for "<Patient>"
		And I add an NHS Number parameter for "<Patient>"
		And I add the medication parameter with includePrescriptionIssues set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Medications should be valid
		And the Medication Statements should be valid
		And the Medication Requests should be valid
		And the List of MedicationStatements should be valid
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |
		| patient4 |
		| patient5 |
		| patient6 |
		
Scenario Outline: Retrieve the medication structured record section for a patient including prescription issues
	Given I configure the default "GpcGetStructuredRecord" request
		And I set the JWT Requested Record to the NHS Number for "<Patient>"
		And I add an NHS Number parameter for "<Patient>"
		And I add the medication parameter with includePrescriptionIssues set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Medications should be valid
		And the Medication Statements should be valid
		And the Medication Requests should be valid
		And the List of MedicationStatements should be valid
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |
		| patient4 |
		| patient5 |
		| patient6 |

Scenario Outline: Retrieve the medication structured record section for a patient excluding prescirption issues
	Given I configure the default "GpcGetStructuredRecord" request
		And I set the JWT Requested Record to the NHS Number for "<Patient>"
		And I add an NHS Number parameter for "<Patient>"
		And I add the medications parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Medications should be valid
		And the Medication Statements should be valid
		And the Medication Requests should be valid
		And the List of MedicationStatements should be valid
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |
		| patient4 |
		| patient5 |
		| patient6 |


