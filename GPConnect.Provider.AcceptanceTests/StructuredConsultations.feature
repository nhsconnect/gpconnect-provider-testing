@structuredrecord
Feature: StructuredConsultations


@1.3.1
Scenario Outline: Verify Consultations structured record for a Patient with Immunizations
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the Consultations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		#And the response should be a Bundle resource of type "collection"
		#And the response meta profile should be for "structured"
		#And the patient resource in the bundle should contain meta data profile and version id
		#And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		#And if the response bundle contains an organization resource it should contain meta data profile and version id
		#And the Bundle should be valid for patient "<Patient>"
		#And the Patient Id should be valid
		#And the Practitioner Id should be valid
		#And the Organization Id should be valid 
		#And The Immunization Resources are Valid
		And The Consultations Resources are Valid
		#And The Immunization Resources Do Not Include Not In Use Fields
		#And the Bundle should contain "1" lists
		#And The Immunization List is Valid
		#And The Structured List Does Not Include Not In Use Fields
	Examples:
	| Patient  |
	| patient2 |
