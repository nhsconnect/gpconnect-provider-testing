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
		And I Check the Consultations List is Valid
		And I Check the Consultation Lists are Valid
		And I Check All The Consultation Lists Do Not Include Not In Use Fields
		And I Check the Topic Lists are Valid
		#And I Check the Consultations Resource linking
		#And The Immunization Resources Do Not Include Not In Use Fields
		#And I Check The Problems List
		
		#TODO -add in check for problems list ( mandatory fields and NOT IN USE fields)


@1.3.1
Scenario: Retrieve consultations structured record for a patient that has no consultation data
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient4"
		And I add the Consultations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "collection"
		And the response meta profile should be for "structured"
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Bundle should be valid for patient "patient4"
		And the Patient Id should be valid
		And the Practitioner Id should be valid
		And the Organization Id should be valid 
		And check the response does not contain an operation outcome
		And check structured list contains a note and emptyReason when no data in section

@1.3.1
Scenario: Retrieve consultations structured record section for an invalid NHS number
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for an invalid NHS Number
		And I add the Consultations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

@1.3.1
Scenario: Retrieve consultations structured record section for an empty NHS number
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter with an empty NHS Number
		And I add the Consultations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

@1.3.1
Scenario: Retrieve consultations structured record section for an invalid Identifier System
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an invalid Identifier System
		And I add the Consultations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

@1.3.1
Scenario: Retrieve consultations structured record section for an empty Identifier System
Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient1" with an empty Identifier System
		And I add the Consultations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

@1.3.1
Scenario: Retrieve consultations structured record for a patient that has sensitive flag
	Given I configure the default "GpcGetStructuredRecord" request 
	And I add an NHS Number parameter for "patient9"
	And I add the Consultations parameter
	When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate failure
		And the response status code should be "404"
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

#SJD TODO

#Retrieve Consultations Structured response where consultationSearchPeriod part parameter is greater than the current date expected failure
#Retrieve Consultations Structured response where start date of the consultationSearchPeriod part parameter is greater than the end date expected failure
#Retrieve Consultations Structured response with both partparameters consultationSearchPeriod and includeNumberOfMostRecent applied expected failure
#Retrieve Consultations Structured response with consultationSearchPeriod and startDate value only expected success
#Retrieve Consultations Structured response with consultationSearchPeriod and endDate value only expected success

#SJD TODO need to understand how these actually works
#Retrieve a Draft Consultations Structure response expected success
#Retrieve Consultations Structured response where Confidential items flagged expected response with message to identify - is this success or failure? 
#Retrieve Consultations Structured response where Linked to single resource expected success 
#Retrieve Consultations Structured response where Linked to multile resources expected success
#Retrieve Consultations Structured response where Unsupported clinical items need to understand is this just Consultations - not understanding how this works
#Create a test around auto generated Topic - is this the same as DRAFT ???