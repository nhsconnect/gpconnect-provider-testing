@structuredrecord
Feature: AccessStructuredRecordAllergies

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
		And the Bundle should be valid for patient "<Patient>"
		And the Bundle should contain "2" lists
		And the Bundle should contain a list with the title "Active Allergies"
		And the Bundle should contain a list with the title "Resolved Allergies"
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
		| patient9  |
		| patient10 |
		| patient11 |
		| patient12 |
		| patient13 |
		| patient15 |


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
		And the Bundle should be valid for patient "<Patient>"
		And the Bundle should contain "1" lists
		And the Bundle should contain a list with the title "Active Allergies"
		And the Bundle should not contain a list with the title "Resolved Allergies"
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
		| patient9  |
		| patient10 |
		| patient11 |
		| patient12 |
		| patient13 |
		| patient15 |

Scenario Outline: Retrieve the allergy structured record section for a patient without the resolved allergies parameter
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the allergies parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "<Patient>"
		And the Bundle should contain "1" lists
		And the Bundle should contain a list with the title "Active Allergies"
		And the Bundle should not contain a list with the title "Resolved Allergies"
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
		| patient9  |
		| patient10 |
		| patient11 |
		| patient12 |
		| patient13 |
		| patient15 |

Scenario Outline: Retrieve the allergy structured record section including resolved allergies for a patient without any allergies
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
		And the Bundle should be valid for patient "<Patient>"
		And the Bundle should contain "2" lists
		And the Bundle should contain a list with the title "Active Allergies"
		And the Bundle should contain a list with the title "Resolved Allergies"
		And the Lists are valid for a patient with no allergies
		And the List of AllergyIntolerances should be valid
	Examples:
		| Patient   |
		| patient1  |
		| patient5  |

Scenario Outline: Retrieve the allergy structured record section excluding resolved allergies for a patient without any allergies
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
		And the Bundle should be valid for patient "<Patient>"
		And the Bundle should contain "1" lists
		And the Bundle should contain a list with the title "Active Allergies"
		And the Lists are valid for a patient with no allergies
		And the List of AllergyIntolerances should be valid
	Examples:
		| Patient   |
		| patient1  |
		| patient5  |

Scenario Outline: Retrieve the allergy structured record section without the resolved allergies parameter for a patient without any allergies
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the allergies parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "<Patient>"
		And the Bundle should contain "1" lists
		And the Bundle should contain a list with the title "Active Allergies"
		And the Lists are valid for a patient with no allergies
		And the List of AllergyIntolerances should be valid
	Examples:
		| Patient   |
		| patient1  |
		| patient5  |

Scenario: Retrieve the allergy structured record section for a patient with an invalid include parameter
	Given I configure the default "GpcGetStructuredRecord" request
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
		And I add an NHS Number parameter for "patient1"
		And I add the allergies parameter with a timePeriod
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Retrieve the allergy structured record section for a patient with a start date
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the allergies parameter with a start date
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Retrieve the allergy structured record section for a patient with an end date
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1"
		And I add the allergies parameter with an end date
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Retrieve the allergy structured record section for a patient with recorder
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the allergies parameter with resolvedAllergies set to "true"
	When I make the "GpcGetStructuredRecord" request
	Then the response should contain the recorder reference



@Ignore @Manual
Scenario: Resolved allergy resources are assigned a clinicalStatus of resolved

@Ignore @Manual
Scenario: Unsupported allergy codes are encoded as text in the note element
	# Qualifiers and values should be rendered as suitably formatted name/value pairs

@Ignore @Manual
Scenario: Generalised allergy codes are assigned the correct category
	# In some cases, the type of allergy or intolerance may be more general - for example, a system designated type of ‘Other’ or 
	# equivalent. In such cases, if the allergy or intolerance entry interacts with prescribing decision support it SHOULD be assigned 
	# a category of medication. Otherwise, the category of environmental SHOULD be used
	
@Ignore @Manual
Scenario: Retrieve the allergy structured record section for a patient with 'No Known Allergies' recorded
	# Check that the emptyReason code is correct for 'NoKnownAllergies'
	# Where there is an explicit assertion of the ‘No Known Allergies’ concept in the record (equivalent to SNOMED CT concept 716186003 
	# and children) and there are otherwise no allergy or intolerance entries in the patient record, then systems may respond to queries 
	# for all allergy or intolerance resources for the patient with an empty List containing an emptyReason code of ‘nil-known’ with the 
	# term of the ‘No Known Allergies’ present expressed as text.

@Ignore @Manual
Scenario: Allergy data saved as non-allergies in the patient record is retreived as AllergyIntolerance resources
	#All allergy data must be brought back in the same format, regardless of how it is saved in the supplier system

@Ignore @Manual
Scenario: Allergy codes that are not fully understood by the consumer are degraded

@Ignore @Manual
Scenario: Consuming systems should prevent prescribing in the presence of degraded drug allergies

@Ignore @Manual
Scenario: Patient with an allergy that is saved as a problem
	# Allergy/Intolerance data is still retreived successfully

@Ignore @Manual
Scenario: Patient data only associated with MHRA Yellow Card dataset is ignored


