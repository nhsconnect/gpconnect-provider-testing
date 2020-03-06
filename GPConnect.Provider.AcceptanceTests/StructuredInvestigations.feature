@structured @structuredinvestigations @1.3.2-Full-Pack
Feature: StructuredInvestigations
	
@1.3.1-IncrementalAndRegression @1.3.2-IncrementalAndRegression
Scenario: Verify Investigations structured record for a Patient with Investigations
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the Investigations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "patient2"
		And check the response does not contain an operation outcome
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid 
		
