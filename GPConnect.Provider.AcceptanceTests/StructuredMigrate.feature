@Structured @StructuredMigrate @1.6.0-Full-Pack @1.6.0-Only
Feature: StructuredMigrate

#Migrate Tests -- WIP --

#WIP - Unable to complete as the GPConnect Structured Resources Do not currently have
#	   any element that indicates that a resource is sensitive.
#	   Discussions are underway to possibly implement a change that will enable this to be tested

#Scenario: Structured Migrate request for Patient2 including Sensitive Data expect success
#	Given I configure the default "MigrateStructuredRecordWithSensitive" request
#		And I add an NHS Number parameter for "patient2"
#		And I add the includeFullrecord parameter with includeSensitiveInformation set to "true"
#    When I make the "MigrateStructuredRecordWithSensitive" request
#	Then the response status code should indicate success
#		And check that the bundle does not contain any duplicate resources
#		And the patient resource in the bundle should contain meta data profile and version id
#		And check the response does not contain an operation outcome
#		And the Patient Id should be valid
#		And the Practitioner Id should be valid
#		And the Organization Id should be valid
#		And the Medications should be valid
#		And the Medication Statements should be valid
#		And the Medication Requests should be valid
#		And the List of MedicationStatements should be valid
#		And there should only be one order request for acute prescriptions
#		And I Check The Problems Resources are Valid
#		And I check The Problem Resources Do Not Include Not In Use Fields
#		And Check a Problem is Linked to a MedicationRequest resource that has been included in the response
#		And Check the MedicationRequests have a link to a medication that has been included in response
#		And Check there is a MedicationStatement resource that is linked to the MedicationRequest and Medication
#		And Check the Medications List resource has been included in response
#		And the AllergyIntolerance should be valid
#		And the Bundle should contain a list with the title "Allergies and adverse reactions"
#		And the Bundle should contain a list with the title "Ended allergies"
#		And the Bundle should contain the correct number of allergies
#		And the Lists are valid for a patient with allergies
#		And Check a Problem is linked to an "AllergyIntolerance" that is also included in the response with its list
#		And I Check the Consultations List is Valid
#		And The Consultations List Does Not Include Not In Use Fields
#		And I Check the Encounters are Valid
#		And I Check the Encounters Do Not Include Not in Use Fields
#		And I Check the Consultation Lists are Valid
#		And I Check All The Consultation Lists Do Not Include Not In Use Fields
#		And I Check the Topic Lists are Valid
#		And I Check one Topic is linked to a problem
#		And I Check the Heading Lists are Valid	
#		And check the response does not contain an operation outcome
#		And I Check The Problems Resources are Valid
#		And I check The Problem Resources Do Not Include Not In Use Fields
#		And I Check that a Topic or Heading is linked to an "Observation" and that is included in response with a list
#		And I Check that a Topic or Heading is linked to an "MedicationRequest" and that is included in response with a list
#		And Check the MedicationRequests have a link to a medication that has been included in response
#		And Check there is a MedicationStatement resource that is linked to the MedicationRequest and Medication
#		And I Check the Consultation Medications Secondary List is Valid
#		And I Check the Consultation Uncategorised Secondary List is Valid
#		And The Immunization Resources are Valid
#		And The Immunization Resources Do Not Include Not In Use Fields
#		And The Immunization List is Valid
#		And Check a Problem is linked to an "Immunization" that is also included in the response with its list
#		And I Check the Investigations List is Valid
#		And I Check the DiagnosticReports are Valid
#		And I Check the DiagnosticReports Do Not Include Not in Use Fields
#		And I Check the Specimens are Valid		
#		And I Check the Specimens Do Not Include Not in Use Fields
#		And Check a Problem is linked to DiagnosticReport and that it is also included
		#TODO Checks for Sensitive INFO being included


#This test requires that the patient has sensitive Medication and uncategorised data assocaited to patient as well as a sensitive medication assocaited to a problem
Scenario: Structured Migrate request for Patient2 Excluding Sensitive Data expect success
	Given I configure the default "MigrateStructuredRecordWithoutSensitive" request
		And I add an NHS Number parameter for "patient2"
		And I add the includeFullrecord parameter with includeSensitiveInformation set to "false"
    When I make the "MigrateStructuredRecordWithoutSensitive" request
	Then the response status code should indicate success
		And check that the bundle does not contain any duplicate resources
		And the patient resource in the bundle should contain meta data profile and version id
		And check the response does not contain an operation outcome
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid
		And the Medications should be valid
		And the Medication Statements should be valid
		And the Medication Requests should be valid
		And the List of MedicationStatements should be valid
		And there should only be one order request for acute prescriptions
		And I Check The Problems Resources are Valid
		And I check The Problem Resources Do Not Include Not In Use Fields
		And Check a Problem is Linked to a MedicationRequest resource that has been included in the response
		And Check the MedicationRequests have a link to a medication that has been included in response
		And Check there is a MedicationStatement resource that is linked to the MedicationRequest and Medication
		And Check the Medications List resource has been included in response
		And the AllergyIntolerance should be valid
		And the Bundle should contain a list with the title "Allergies and adverse reactions"
		And the Bundle should contain a list with the title "Ended allergies"
		And the Bundle should contain the correct number of allergies
		And the Lists are valid for a patient with allergies
		And Check a Problem is linked to an "AllergyIntolerance" that is also included in the response with its list
		And I Check the Consultations List is Valid
		And The Consultations List Does Not Include Not In Use Fields
		And I Check the Encounters are Valid
		And I Check the Encounters Do Not Include Not in Use Fields
		And I Check the Consultation Lists are Valid
		And I Check All The Consultation Lists Do Not Include Not In Use Fields
		And I Check the Topic Lists are Valid
		And I Check one Topic is linked to a problem
		And I Check the Heading Lists are Valid	
		And check the response does not contain an operation outcome
		And I Check The Problems Resources are Valid
		And I check The Problem Resources Do Not Include Not In Use Fields
		And I Check that a Topic or Heading is linked to an "Observation" and that is included in response with a list
		And I Check that a Topic or Heading is linked to an "MedicationRequest" and that is included in response with a list
		And Check the MedicationRequests have a link to a medication that has been included in response
		And Check there is a MedicationStatement resource that is linked to the MedicationRequest and Medication
		And I Check the Consultation Medications Secondary List is Valid
		And I Check the Consultation Uncategorised Secondary List is Valid
		And The Immunization Resources are Valid
		And The Immunization Resources Do Not Include Not In Use Fields
		And The Immunization List is Valid
		And Check a Problem is linked to an "Immunization" that is also included in the response with its list
		And I Check the Investigations List is Valid
		And I Check the DiagnosticReports are Valid
		And I Check the DiagnosticReports Do Not Include Not in Use Fields
		And I Check the Specimens are Valid		
		And I Check the Specimens Do Not Include Not in Use Fields
		And Check a Problem is linked to DiagnosticReport and that it is also included
		And Check the list "Medications and medical devices" contains confidential marking
		And Check the list "Uncategorised data" contains confidential marking
		And Check the list "Problems - medications related to problems" contains confidential marking
		
		

#WIP - Unable to complete as the GPConnect Structured Resources Do not currently have
#	   any element that indicates that a resource is sensitive.
#	   Discussions are underway to possibly implement a change that will enable this to be tested

#Scenario: Structured Migrate request for Patient3 including Sensitive Data expect No Docs and success
#	Given I configure the default "MigrateStructuredRecordWithSensitive" request
#		And I add an NHS Number parameter for "patient3"
#		And I add the includeFullrecord parameter with includeSensitiveInformation set to "true"
#    When I make the "MigrateStructuredRecordWithSensitive" request
#	Then the response status code should indicate success
#		And check that the bundle does not contain any duplicate resources
#		And the patient resource in the bundle should contain meta data profile and version id
#		And check the response does not contain an operation outcome
#		And the Patient Id should be valid
#		And the Practitioner Id should be valid
#		And the Organization Id should be valid
#		And the Medications should be valid
#		And the Medication Statements should be valid
#		And the Medication Requests should be valid
#		And the List of MedicationStatements should be valid
#		And there should only be one order request for acute prescriptions
#		And I Check The Problems Resources are Valid
#		And I check The Problem Resources Do Not Include Not In Use Fields
#		And Check a Problem is Linked to a MedicationRequest resource that has been included in the response
#		And Check the MedicationRequests have a link to a medication that has been included in response
#		And Check there is a MedicationStatement resource that is linked to the MedicationRequest and Medication
#		And Check the Medications List resource has been included in response
#		And the AllergyIntolerance should be valid
#		And the Bundle should contain a list with the title "Allergies and adverse reactions"
#		And the Bundle should contain a list with the title "Ended allergies"
#		And the Bundle should contain the correct number of allergies
#		And the Lists are valid for a patient with allergies
#		And Check a Problem is linked to an "AllergyIntolerance" that is also included in the response with its list
#		And I Check the Consultations List is Valid
#		And The Consultations List Does Not Include Not In Use Fields
#		And I Check the Encounters are Valid
#		And I Check the Encounters Do Not Include Not in Use Fields
#		And I Check the Consultation Lists are Valid
#		And I Check All The Consultation Lists Do Not Include Not In Use Fields
#		And I Check the Topic Lists are Valid
#		And I Check one Topic is linked to a problem
#		And I Check the Heading Lists are Valid	
#		And check the response does not contain an operation outcome
#		And I Check The Problems Resources are Valid
#		And I check The Problem Resources Do Not Include Not In Use Fields
#		And I Check that a Topic or Heading is linked to an "Observation" and that is included in response with a list
#		And I Check that a Topic or Heading is linked to an "MedicationRequest" and that is included in response with a list
#		And Check the MedicationRequests have a link to a medication that has been included in response
#		And Check there is a MedicationStatement resource that is linked to the MedicationRequest and Medication
#		And I Check the Consultation Medications Secondary List is Valid
#		And I Check the Consultation Uncategorised Secondary List is Valid
#		And The Immunization Resources are Valid
#		And The Immunization Resources Do Not Include Not In Use Fields
#		And The Immunization List is Valid
#		And Check a Problem is linked to an "Immunization" that is also included in the response with its list
#		And I Check the Investigations List is Valid
#		And I Check the DiagnosticReports are Valid
#		And I Check the DiagnosticReports Do Not Include Not in Use Fields
#		And I Check the Specimens are Valid		
#		And I Check the Specimens Do Not Include Not in Use Fields
#		And Check a Problem is linked to DiagnosticReport and that it is also included
#       And The Bundle should contain NO Documents
		#TODO Checks for Sensitive INFO being included

Scenario: Structured Migrate request for Patient8 excluding Sensitive Data expect No Docs and success
	Given I configure the default "MigrateStructuredRecordWithoutSensitive" request
		And I add an NHS Number parameter for "patient8"
		And I add the includeFullrecord parameter with includeSensitiveInformation set to "false"
    When I make the "MigrateStructuredRecordWithoutSensitive" request
	Then the response status code should indicate success
		And check that the bundle does not contain any duplicate resources
		And the patient resource in the bundle should contain meta data profile and version id
		And check the response does not contain an operation outcome
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid
		And the Medications should be valid
		And the Medication Statements should be valid
		And the Medication Requests should be valid
		And the List of MedicationStatements should be valid
		And there should only be one order request for acute prescriptions
		And I Check The Problems Resources are Valid
		And I check The Problem Resources Do Not Include Not In Use Fields
		And Check a Problem is Linked to a MedicationRequest resource that has been included in the response
		And Check the MedicationRequests have a link to a medication that has been included in response
		And Check there is a MedicationStatement resource that is linked to the MedicationRequest and Medication
		And Check the Medications List resource has been included in response
		And the AllergyIntolerance should be valid
		And the Bundle should contain a list with the title "Allergies and adverse reactions"
		And the Bundle should contain a list with the title "Ended allergies"
		And the Bundle should contain the correct number of allergies
		And the Lists are valid for a patient with allergies
		And Check a Problem is linked to an "AllergyIntolerance" that is also included in the response with its list
		And I Check the Consultations List is Valid
		And The Consultations List Does Not Include Not In Use Fields
		And I Check the Encounters are Valid
		And I Check the Encounters Do Not Include Not in Use Fields
		And I Check the Consultation Lists are Valid
		And I Check All The Consultation Lists Do Not Include Not In Use Fields
		And I Check the Topic Lists are Valid
		And I Check one Topic is linked to a problem
		And I Check the Heading Lists are Valid	
		And check the response does not contain an operation outcome
		And I Check The Problems Resources are Valid
		And I check The Problem Resources Do Not Include Not In Use Fields
		And I Check that a Topic or Heading is linked to an "Observation" and that is included in response with a list
		And I Check that a Topic or Heading is linked to an "MedicationRequest" and that is included in response with a list
		And Check the MedicationRequests have a link to a medication that has been included in response
		And Check there is a MedicationStatement resource that is linked to the MedicationRequest and Medication
		And I Check the Consultation Medications Secondary List is Valid
		And I Check the Consultation Uncategorised Secondary List is Valid
		And The Immunization Resources are Valid
		And The Immunization Resources Do Not Include Not In Use Fields
		And The Immunization List is Valid
		And Check a Problem is linked to an "Immunization" that is also included in the response with its list
		And I Check the Investigations List is Valid
		And I Check the DiagnosticReports are Valid
		And I Check the DiagnosticReports Do Not Include Not in Use Fields
		And I Check the Specimens are Valid		
		And I Check the Specimens Do Not Include Not in Use Fields
		And Check a Problem is linked to DiagnosticReport and that it is also included
		And The Bundle should contain NO Documents

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

Scenario: Structured Migrate request for Patient2 Excluding Sensitive Data With Extra Param expect Fail
	Given I configure the default "MigrateStructuredRecordWithoutSensitive" request
		And I add an NHS Number parameter for "patient2"
		And I add the includeFullrecord parameter with includeSensitiveInformation set to "false"
		And I add the immunizations parameter
    When I make the "MigrateStructuredRecordWithoutSensitive" request
	Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Structured Migrate request for Patient2 including Sensitive Data With Extra Param expect Fail
	Given I configure the default "MigrateStructuredRecordWithSensitive" request
		And I add an NHS Number parameter for "patient2"
		And I add the includeFullrecord parameter with includeSensitiveInformation set to "true"
		And I add the medication parameter with includePrescriptionIssues set to "false"
    When I make the "MigrateStructuredRecordWithSensitive" request
	Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Structured Migrate request for Patient2 inc Sensitive Data where JWT reason is not set expect Fail
	Given I configure the default "MigrateStructuredRecordWithSensitive" request
		And I add an NHS Number parameter for "patient2"
		And I add the includeFullrecord parameter with includeSensitiveInformation set to "true"
		And I set the JWT with missing Reason For Request
    When I make the "MigrateStructuredRecordWithSensitive" request
		Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: Structured Migrate request for Patient2 exc Sensitive Data where JWT reason is not set expect Fail
	Given I configure the default "MigrateStructuredRecordWithoutSensitive" request
		And I add an NHS Number parameter for "patient2"
		And I add the includeFullrecord parameter with includeSensitiveInformation set to "false"
		And I set the JWT with missing Reason For Request
    When I make the "MigrateStructuredRecordWithSensitive" request
		Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: Structured Migrate request for Patient2 inc Sensitive Data where JWT reason is not migrate expect Fail
	Given I configure the default "MigrateStructuredRecordWithSensitive" request
		And I add an NHS Number parameter for "patient2"
		And I add the includeFullrecord parameter with includeSensitiveInformation set to "true"
		And I set the JWT Reason For Request to "badReason"
    When I make the "MigrateStructuredRecordWithSensitive" request
			Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: Structured Migrate request for Patient2 exc Sensitive Data where JWT reason is not migrate expect Fail
	Given I configure the default "MigrateStructuredRecordWithoutSensitive" request
		And I add an NHS Number parameter for "patient2"
		And I add the includeFullrecord parameter with includeSensitiveInformation set to "false"
		And I set the JWT Reason For Request to "badReason"
    When I make the "MigrateStructuredRecordWithSensitive" request
			Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: Structured Migrate request for Patient2 inc Sensitive Data where JWT scope is incorrect expect Fail
	Given I configure the default "MigrateStructuredRecordWithSensitive" request
		And I add an NHS Number parameter for "patient2"
		And I add the includeFullrecord parameter with includeSensitiveInformation set to "true"
		And I set the JWT Request Scope to Normal confidentiality
    When I make the "MigrateStructuredRecordWithSensitive" request
			Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: Structured Migrate request for Patient2 exc Sensitive Data where JWT scope is incorrect expect Fail
	Given I configure the default "MigrateStructuredRecordWithoutSensitive" request
		And I add an NHS Number parameter for "patient2"
		And I add the includeFullrecord parameter with includeSensitiveInformation set to "false"
		And I set the JWT Request Scope to Sensitive confidentiality
    When I make the "MigrateStructuredRecordWithoutSensitive" request
			Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: Structured Migrate request for Patient2 exc Sensitive Data where JWT scope is not sent expect Fail
	Given I configure the default "MigrateStructuredRecordWithoutSensitive" request
		And I add an NHS Number parameter for "patient2"
		And I add the includeFullrecord parameter with includeSensitiveInformation set to "false"
		And I set the JWT Request Scope to nothing
    When I make the "MigrateStructuredRecordWithoutSensitive" request
			Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: Structured Migrate request for Patient2 inc Sensitive Data where JWT scope is not sent expect Fail
	Given I configure the default "MigrateStructuredRecordWithSensitive" request
		And I add an NHS Number parameter for "patient2"
		And I add the includeFullrecord parameter with includeSensitiveInformation set to "true"
		And I set the JWT Request Scope to nothing
    When I make the "MigrateStructuredRecordWithSensitive" request
			Then the response status code should be "400"
		And the response should be a OperationOutcome resource		

Scenario: Structured Migrate request for Patient2 inc Sensitive Data where JWT Requesting Practitioner is Not sent expect Success
	Given I configure the default "MigrateStructuredRecordWithSensitive" request
		And I add an NHS Number parameter for "patient2"
		And I add the includeFullrecord parameter with includeSensitiveInformation set to "true"
		And I set the JWT Practitioner to nothing
    When I make the "MigrateStructuredRecordWithSensitive" request
		Then the response status code should indicate success

Scenario: Structured Migrate request for Patient2 exc Sensitive Data where JWT Requesting Practitioner is Not sent expect Success
	Given I configure the default "MigrateStructuredRecordWithoutSensitive" request
		And I add an NHS Number parameter for "patient2"
		And I add the includeFullrecord parameter with includeSensitiveInformation set to "false"
		And I set the JWT Practitioner to nothing
    When I make the "MigrateStructuredRecordWithoutSensitive" request
		Then the response status code should indicate success
		
#Migrate Document Tests

Scenario: Migrate Patient2 With Sensitive and then migrate first document
	Given I configure the default "MigrateStructuredRecordWithSensitive" request
	And I add an NHS Number parameter for "patient2"
		And I add the includeFullrecord parameter with includeSensitiveInformation set to "true"
    When I make the "MigrateStructuredRecordWithSensitive" request
	Then the response status code should indicate success
		And check that the bundle does not contain any duplicate resources
		And the patient resource in the bundle should contain meta data profile and version id
		And check the response does not contain an operation outcome
		And I Check Documents have been Returned and save the first documents url for retrieving later
		And I Check the returned DocumentReference is Valid
		And I Check the returned DocumentReference Do Not Include Not In Use Fields
	Given I configure the default "MigrateDocument" request
		When I make the "MigrateDocument" request
		Then the response status code should indicate success
		And I Check the returned Binary Document is Valid
		And I Check the returned Binary Document Do Not Include Not In Use Fields
		And I save the binary document from the retrieve

Scenario: Migrate Patient2 Without Sensitive and then migrate first document
	Given I configure the default "MigrateStructuredRecordWithoutSensitive" request
		And I add an NHS Number parameter for "patient2"
		And I add the includeFullrecord parameter with includeSensitiveInformation set to "false"
    When I make the "MigrateStructuredRecordWithoutSensitive" request
	Then the response status code should indicate success
		And check that the bundle does not contain any duplicate resources
		And the patient resource in the bundle should contain meta data profile and version id
		And check the response does not contain an operation outcome
		And I Check Documents have been Returned and save the first documents url for retrieving later
		And I Check the returned DocumentReference is Valid
		And I Check the returned DocumentReference Do Not Include Not In Use Fields
	Given I configure the default "MigrateDocument" request
		When I make the "MigrateDocument" request
		Then the response status code should indicate success
		And I Check the returned Binary Document is Valid
		And I Check the returned Binary Document Do Not Include Not In Use Fields
		And I save the binary document from the retrieve

