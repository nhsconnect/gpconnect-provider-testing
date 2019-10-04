@structuredrecord
Feature: StructuredConsultations


@1.3.1 @WIP
Scenario: Verify Consultations structured record for a Patient
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the Consultations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "patient2"
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid 
		And I Check the Consultations Resource linking
		#And The Immunization Resources Do Not Include Not In Use Fields
		And I Check The Problems List
		And The Structured List Does Not Include Not In Use Fields
