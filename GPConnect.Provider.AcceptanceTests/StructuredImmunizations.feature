@structuredrecord
Feature: StructuredImmunizations
	
	@1.3.0
	Scenario Outline: Verify Immunizations structured record for a Patient with Immunizations
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "<Patient>"
		And I add the immunisations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "<Patient>"
		And the Bundle should contain "1" lists
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid 
		And The Immunization Resource is Valid
		And The Immunization Resource Does Not Include Not In Use Fields
		And the Bundle should contain "1" lists
		And The Immunization List is Valid
		And The Immunization List Does Not Include Not In Use Fields
	Examples:
	| Patient  |
	| patient2 |
			