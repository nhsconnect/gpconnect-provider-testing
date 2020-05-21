@Documents @1.5.0-Full-Pack
Feature: Documents

Scenario: Searching for Documents for a Patient with Documents
	#Given I get the Patient for Patient Value "patient2"
	#	And I store the Patient
	Given I add an NHS Number to GlobalContext for "patient2"
	Given I configure the default "DocumentsSearch" request
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters for a Documents Search call
	When I make the "DocumentsSearch" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"


Scenario Outline: Documents Patient search response conforms with the GPConnect specification
	Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "<Patient>"
	When I make the "DocumentsPatientSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Patient Name should be valid
		And the Patient Use should be valid
		And the Patient Communication should be valid
		And the Patient Contact should be valid
		And the Patient MultipleBirth should be valid
		And the Patient MaritalStatus should be valid
		And the Patient Deceased should be valid
		And the Patient Telecom should be valid
		And the Patient ManagingOrganization Organization should be valid and resolvable
		And the Patient GeneralPractitioner Practitioner should be valid and resolvable
		And the Patient should exclude disallowed fields
		And the Patient Link should be valid and resolvable
		And the Patient Contact Telecom use should be valid
		And the Patient Not In Use should be valid
	Examples:
		| Patient   |
		| patient1  |
		| patient2  |
		| patient3  |
		| patient4  |
		| patient5  |
		| patient6  |