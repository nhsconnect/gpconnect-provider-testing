@Structured @StructuredMisc @1.5.0-Full-Pack @1.5.1
Feature: StructuredMisc

#Retrieve Tests
Scenario: Structured Retrieve request for Patient FullRecord sent expect success
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the includeFullrecord parameter
    When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And check that the bundle does not contain any duplicate resources
		And the patient resource in the bundle should contain meta data profile and version id
		And check the response does not contain an operation outcome	

#Migrate Tests
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


Scenario: Structured Migrate request for a Deceased Patient Excluding Sensitive Data expect fail
	Given I configure the default "MigrateStructuredRecordWithoutSensitive" request
		And I add an NHS Number parameter for "patient18"
		And I add the includeFullrecord parameter with includeSensitiveInformation set to "false"
    When I make the "MigrateStructuredRecordWithoutSensitive" request
	Then the response status code should indicate failure
		And the response status code should be "404"
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"


Scenario: Structured Migrate request for a Deceased Patient including Sensitive Data expect fail
	Given I configure the default "MigrateStructuredRecordWithSensitive" request
		And I add an NHS Number parameter for "patient18"
		And I add the includeFullrecord parameter with includeSensitiveInformation set to "true"
    When I make the "MigrateStructuredRecordWithSensitive" request
	Then the response status code should indicate failure
		And the response status code should be "404"
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"


Scenario: Structured Migrate request for Patient Excluding Sensitive Data With Extra Param expect Fail
	Given I configure the default "MigrateStructuredRecordWithoutSensitive" request
		And I add an NHS Number parameter for "patient2"
		And I add the includeFullrecord parameter with includeSensitiveInformation set to "false"
		And I add the immunizations parameter
    When I make the "MigrateStructuredRecordWithoutSensitive" request
	Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"


Scenario: Structured Migrate request for Patient including Sensitive Data With Extra Param expect Fail
	Given I configure the default "MigrateStructuredRecordWithSensitive" request
		And I add an NHS Number parameter for "patient2"
		And I add the includeFullrecord parameter with includeSensitiveInformation set to "true"
		And I add the medication parameter with includePrescriptionIssues set to "false"
    When I make the "MigrateStructuredRecordWithSensitive" request
	Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"


		#TODO - Create Test where JWT Payload reason for request is not migration to generate error
		#TODO Tests that have extra params for both migrate and retrieve