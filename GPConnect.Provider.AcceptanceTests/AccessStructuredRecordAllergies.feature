@structuredrecord
Feature: AccessStructuredRecordAllergies

Scenario Outline: Retrieve the allergy structured record section for a patient including resolved allergies
	Given I configure the default "GpcGetStructuredRecord" request
		And I set the JWT Requested Record to the NHS Number for "<Patient>"
		And I add an NHS Number parameter for "<Patient>"
		And I add the allergies parameter with resolvedAllergies set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the AllergyIntolerance Metadata should be valid
		And the Bundle should contain "<NumberOfAllergies>" allergies
		And the AllergyIntolerance Id should be valid
		And the AllergyIntolerance clinicalStatus should be valid
		And the AllergyIntolerance verificationStatus should be valid
		And the AllergyIntolerance category should be valid
		And the AllergyIntolerance Code should be valid
		And the AllergyIntolerance assertedDate should be valid
	Examples:
		| Patient  | NumberOfAllergies |
		| patient1 | 1                 |
		| patient2 | 1                 |
		| patient3 | 1                 |
		| patient4 | 1                 |
		| patient5 | 1                 |
		| patient6 | 1                 |

Scenario Outline: Retrieve the allergy structured record section for a patient excluding resolved allergies
	Given I configure the default "GpcGetStructuredRecord" request
		And I set the JWT Requested Record to the NHS Number for "<Patient>"
		And I add an NHS Number parameter for "<Patient>"
		And I add the allergies parameter with resolvedAllergies set to "false"
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
	Examples:
		| Patient  | NumberOfAllergies |
		| patient1 | 1                 |
		| patient2 | 1                 |
		| patient3 | 1                 |
		| patient4 | 1                 |
		| patient5 | 1                 |
		| patient6 | 1                 |
