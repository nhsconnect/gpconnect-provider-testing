@Structured @StructuredDiary @1.5.0-Full-Pack @1.5.0-IncrementalAndRegression
Feature: StructuredDiary


Scenario: Search for Documents on a Patient with Documents
	Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient2"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should indicate success
		Given I store the Patient
	Given I configure the default "DocumentsSearch" request
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters for a Documents Search call
	When I make the "DocumentsSearch" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And I Check the returned DocumentReference is Valid
		And I Check the returned DocumentReference Do Not Include Not In Use Fields


