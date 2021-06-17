@Structured @StructuredMisc @1.5.0-Full-Pack @1.5.1
Feature: StructuredMisc

#Retrieve
Scenario: Structured Retrieve request for Patient FullRecord sent expect success
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the includeFullrecord parameter
    When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And check that the bundle does not contain any duplicate resources
		And the patient resource in the bundle should contain meta data profile and version id
		And check the response does not contain an operation outcome	

#Migrate
Scenario: Structured Migrate request for Patient Excluding Sensitive Data expect success
	Given I configure the default "MigrateStructuredRecordWithoutSensitive" request
		And I add an NHS Number parameter for "patient2"
		And I add the includeFullrecord parameter with includeSensitiveInformation set to "false"
    When I make the "MigrateStructuredRecordWithoutSensitive" request
	Then the response status code should indicate success
		And check that the bundle does not contain any duplicate resources
		And the patient resource in the bundle should contain meta data profile and version id
		And check the response does not contain an operation outcome


Scenario: Structured Migrate request for Patient including Sensitive Data expect success
	Given I configure the default "MigrateStructuredRecordWithSensitive" request
		And I add an NHS Number parameter for "patient2"
		And I add the includeFullrecord parameter with includeSensitiveInformation set to "true"
    When I make the "MigrateStructuredRecordWithSensitive" request
	Then the response status code should indicate success
		And check that the bundle does not contain any duplicate resources
		And the patient resource in the bundle should contain meta data profile and version id
		And check the response does not contain an operation outcome

		
		#TODO - Create Test where JWT Payload reason for request is not migration to generate error
