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
		And the AllergyIntolerance should be valid
		And the Bundle should contain "1" lists
		And the Bundle should contain a list with the title "Active Allergies"
		And the Bundle should not contain a list with the title "Resolved Allergies"
		And the Bundle should contain the correct number of allergies
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |
		| patient4 |
		| patient5 |
		| patient6 |

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
		And the Bundle should contain "2" lists
		And the Bundle should contain a list with the title "Active Allergies"
		And the Bundle should contain a list with the title "Resolved Allergies"
		And the AllergyIntolerance should be valid
		And the Bundle should contain the correct number of allergies
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |
		| patient4 |
		| patient5 |
		| patient6 |

Scenario Outline: Retrieve the allergy structured record section for a patient without the resolved allergies parameter
	Given I configure the default "GpcGetStructuredRecord" request
		And I set the JWT Requested Record to the NHS Number for "<Patient>"
		And I add an NHS Number parameter for "<Patient>"
		And I add the allergies parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should contain "<NumberOfAllergies>" allergies
		And the Bundle should contain "1" lists
		And the Bundle should contain a list with the title "Active Allergies"
		And the Bundle should not contain a list with the title "Resolved Allergies"
		And the AllergyIntolerance should be valid
		And the Bundle should contain the correct number of allergies
	Examples:
		| Patient  | 
		| patient1 |
		| patient2 |
		| patient3 |
		| patient4 |
		| patient5 |
		| patient6 |

@Ignore
Scenario: Retrieve the allergy structured record section for a patient with 'No Known Allergies' recorded
	Given I configure the default "GpcGetStructuredRecord" request
		And I set the JWT Requested Record to the NHS Number for "patient7"
		And I add an NHS Number parameter for "patient7"
		And I add the allergies parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the Bundle should contain "1" lists
		And the emptyReason code is correct for 'NoKnownAllergies'
		And the AllergyIntolerance should be valid

@Ignore
Scenario: Retrieve the allergy structured record section for a patient with no allergies
	Given I configure the default "GpcGetStructuredRecord" request
		And I set the JWT Requested Record to the NHS Number for "patient7"
		And I add an NHS Number parameter for "patient7"
		And I add the allergies parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the Bundle should contain "1" lists
		And the emptyReason code is correct for no allergies recorded
		And the AllergyIntolerance should be valid

Scenario: Retrieve the allergy structured record section for a patient with an invalid include parameter
	Given I configure the default "GpcGetStructuredRecord" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
		And I add an NHS Number parameter for "patient1"
		And I add an invalid allergies parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Retrieve the allergy structured record section for an invalid NHS number
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for an invalid NHS Number
		And I add the allergies parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Retrieve the allergy structured record section for an empty NHS number
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter with an empty NHS Number
		And I add the allergies parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Retrieve the allergy structured record section for an invalid Identifier System
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an invalid Identifier System
		And I add the allergies parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Retrieve the allergy structured record section for an empty Identifier System
Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an empty Identifier System
		And I add the allergies parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource
		
Scenario: Retrieve the allergy structured record section for an invalid parameter type
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" using an invalid parameter type
		And I add the allergies parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Retrieve the allergy structured record section for a patient with a timePeriod
	Given I configure the default "GpcGetStructuredRecord" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
		And I add an NHS Number parameter for "patient1"
		And I add the allergies parameter with a timePeriod
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Retrieve the allergy structured record section for a patient with a start date
	Given I configure the default "GpcGetStructuredRecord" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
		And I add an NHS Number parameter for "patient1"
		And I add the allergies parameter with a start date
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Retrieve the allergy structured record section for a patient with an end date
	Given I configure the default "GpcGetStructuredRecord" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
		And I add an NHS Number parameter for "patient1"
		And I add the allergies parameter with an end date
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

