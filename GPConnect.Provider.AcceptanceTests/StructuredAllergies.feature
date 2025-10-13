@Structured @StructuredAllergies @1.5.0-Full-Pack
Feature: StructuredAllergies

	# These Tests are only Testing this Structured Area in isolation and Not with other Areas or Combinations of Include Parameters
	# Tests around Multiple Structured Areas in one Request are tested in the MultipleRequests Feature


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
		And I Check There is No Problems Secondary Problems List
		And I Check No Problem Resources are Included
		Examples:
			| Patient   |
			| patient3  |
			| patient4  |
			| patient6  |
			| patient7  |
			| patient8  |
			| patient12 |
			| patient13 |
			| patient24 |
			| patient35 |

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
		And I Check The Problems Secondary Problems List
		And I Check The Problems Secondary Problems List Does Not Include Not In Use Fields
		And I Check The Problems Resources are Valid
		And I check The Problem Resources Do Not Include Not In Use Fields
		And Check a Problem is linked to an "AllergyIntolerance" that is also included in the response with its list
		Examples:
			| Patient   |
			| patient2  |
			| patient35 |

	Scenario: Retrieve the allergy structured record section including resolved allergies for a patient without any allergies
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
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid
		And the Bundle should be valid for patient "<Patient>"
		And the Bundle should contain "2" lists
		And the Bundle should contain a list with the title "Allergies and adverse reactions"
		And the Bundle should contain a list with the title "Ended allergies"
		And the Lists are valid for a patient with no allergies
		And the List of AllergyIntolerances should be valid
		Examples:
			| Patient  |
			| patient1 |
			| patient5 |