@appointment @1.2.7-Full-Pack
Feature: AppointmentAmend

@1.2.3
Scenario Outline: I perform a successful amend appointment and check the returned appointment resources are in the future
	Given I create an Appointment for Patient "<Patient>" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentAmend" request
		And I set the Created Appointment Description to "customDescription"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointments returned must be in the future
		And the Appointment Metadata should be valid
		And the Appointment Status should be valid
		And the Appointment Start should be valid
		And the Appointment End should be valid
		And the Appointment Slots should be valid
		And the Appointment Participants should be valid and resolvable
		And the Appointment Priority should be valid
		And the Appointment Participant Type and Actor should be valid
		And the Appointment Identifiers should be valid
		And the Appointment Description should be valid for "customDescription"
		And the Appointment Created must be valid
		And the Appointment DeliveryChannel must be valid
		And the Appointment PractitionerRole must be valid
# git hub ref 120
# RMB 25/10/2018		
		And the Appointment Not In Use should be valid
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |

Scenario Outline: I perform a successful amend appointment with Extensions
	Given I create an Appointment for Patient "<PatientName>" 
		And I create an Appointment with org type "<OrgType>" with channel "<DeliveryChannel>" with prac role "<PracRole>"	
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointments returned must be in the future
		And the Appointment Id should be valid
		And the Appointment Metadata should be valid
		And the Appointment DeliveryChannel must be present
		And the Appointment PractitionerRole must be present
# git hub ref 120
# RMB 25/10/2018		
		And the Appointment Not In Use should be valid
	Examples:
		| PatientName | OrgType | DeliveryChannel | PracRole |
		| patient1    | true    | true            | true     |

Scenario: Amend appointment and update element which cannot be updated
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentAmend" request
		And I set the Created Appointment Priority to "1"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario Outline: Amend appointment using the _format parameter to request response format
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I set the Created Appointment Description to "customDescription"
		And I add a Format parameter with the Value "<Format>"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be an Appointment
		And the Appointment Description should be valid for "customDescription"
	Examples:
		| Format                | BodyFormat |
		| application/fhir+json | JSON       |
		| application/fhir+xml  | XML        |

Scenario Outline: Amend appointment using the accept header to request response format
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I set the Created Appointment Description to "customDescription"
		And I set the Accept header to "<Header>"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be an Appointment
		And the Appointment Metadata should be valid
		And the Appointment Description should be valid for "customDescription"
	Examples:
		| Header                | BodyFormat |
		| application/fhir+json | JSON       |
		| application/fhir+xml  | XML        |

Scenario Outline: Amend appointment using the _format and accept parameter to request response format
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be an Appointment
		And the Appointment Metadata should be valid
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/fhir+json | application/fhir+json | JSON       |
		| application/fhir+json | application/fhir+xml  | XML        |
		| application/fhir+xml  | application/fhir+json | JSON       |
		| application/fhir+xml  | application/fhir+xml  | XML        |
		
Scenario: Amend appointment prefer header set to representation
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I set the Created Appointment Comment to "customComment"
		And I set the Prefer header to "return=representation"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Response Resource should be an Appointment
		And the content-type should not be equal to null
		And the content-length should not be equal to zero

Scenario: Amend appointment prefer header set to minimal
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I set the Prefer header to "return=minimal"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the response body should be empty

Scenario: Amend appointment send an update with an invalid if-match header
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I set the Created Appointment Comment to "customComment"
		And I set the If-Match header to "invalidEtag"
	When I make the "AppointmentAmend" request
	Then the response status code should be "409"

Scenario: Amend appointment set etag and check etag is the same in the returned amended appointment
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"		
		And I store the Created Appointment	
	Given I configure the default "AppointmentAmend" request
# Amended RMB from customComment to customDescription
		And I set the Created Appointment Description to "customDescription"
		And I set the If-Match header to the Stored Appointment Version Id
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
# Amended RMB from customComment to customDescription
		And the Appointment Description should be valid for "customDescription"

Scenario: Amend appointment and send an invalid bundle resource
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I set the Created Appointment Comment to "customComment"
	When I make the "AppointmentAmend" request with invalid Resource type
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"
#
# Test reeplaced with original tests from 1.2.0
# github ref 99
# RMB 17/10/2018
#
#Scenario: Amend appointment and send an invalid appointment resource
#	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
#		And I store the Created Appointment	
#	Given I configure the default "AppointmentAmend" request
#		And I set the Created Appointment to a new Appointment
#	When I make the "AppointmentAmend" request
#	Then the response status code should be "400"
#		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Amend appointment and send an invalid appointment resource
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I amend an invalid appointment field
	When I make the "AppointmentAmend" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"
		
Scenario: CapabilityStatement profile support the Amend appointment operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement REST Resources should contain the "Appointment" Resource with the "Update" Interaction

Scenario: Amend appointment valid response check caching headers exist
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentAmend" request
		And I set the Created Appointment Description to "customDescription"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointment Description should be valid for "customDescription"
		And the Appointment Metadata should be valid
		And the required cacheing headers should be present in the response
#
# Test replaced with original tests from 1.2.0
# github ref 99
# RMB 17/10/2018
#
#Scenario:Amend appointment invalid response check caching headers exist
#	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
#		And I store the Created Appointment	
#	Given I configure the default "AppointmentAmend" request
#		And I set the Created Appointment Comment to "customComment"
#		And I set the Created Appointment to a new Appointment
#	When I make the "AppointmentAmend" request
#	Then the response status code should be "400"
#		And the required cacheing headers should be present in the response

Scenario:Amend appointment invalid response check caching headers exist
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I amend an invalid appointment field														
	When I make the "AppointmentAmend" request
	Then the response status code should be "422"
		And the required cacheing headers should be present in the response
		
# git hub ref 145
# RMB 10/12/2018
Scenario: Amend appointment and update cancellation reason
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentAmend" request
		And I set the Created Appointment Cancellation Reason "double booked"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

# git hub ref 157
# RMB 8/1/19
@1.2.3
Scenario: Amend appointment with Comment and Description
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentAmend" request
		And I set the Created Appointment Description to "customDescription"
		And I set the Created Appointment Comment to "customComment"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointment Description should be valid for "customDescription"

# git hub ref 200 (demonstrator)
# RMB 21/2/19
Scenario: Amend appointment and update to absolute reference
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentAmend" request
		And I amend the cancel organization reference to absolute reference
	When I make the "AppointmentAmend" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: I perform amend appointment with participants with absoulte references
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I add a Participant with Reference "https://test1.supplier.thirdparty.nhs.uk/A11111/STU3/1/GPConnect/Location/2" to the Created Appointment
		And I add a Participant with Reference "https://test1.supplier.thirdparty.nhs.uk/A11111/STU3/1/GPConnect/Practitioner/2" to the Created Appointment
		And I add a Participant with Reference "https://test1.supplier.thirdparty.nhs.uk/A11111/STU3/1/GPConnect/Patient/2" to the Created Appointment
	When I make the "AppointmentAmend" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

@1.2.7-IncrementalAndRegression
Scenario: I perform a successful amend appointment on an appointment that has ServiceCategory and ServiceType elements set
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentAmend" request
		And I set the Created Appointment Description to "TestChangedDescription"
		And I set the Created Appointment Comment to "TestChangedComment"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointments returned must be in the future
		And the Appointment Metadata should be valid
		And the Appointment Status should be valid
		And the Appointment Start should be valid
		And the Appointment End should be valid
		And the Appointment Slots should be valid
		And the Appointment Participants should be valid and resolvable
		And the Appointment Priority should be valid
		And the Appointment Participant Type and Actor should be valid
		And the Appointment Identifiers should be valid
		And the Appointment Description should be valid for "TestChangedDescription"
		And the Appointment Comment should be valid for "TestChangedComment"
		And the Appointment Created must be valid
		And the Appointment DeliveryChannel must be valid
		And the Appointment PractitionerRole must be valid
		And the Appointment Not In Use should be valid
		And One Appointment contains serviceCategory and serviceType elements
	
@1.2.7-IncrementalAndRegression	
Scenario: I perform a successful amend appointment on an appointment that does not have ServiceCategory and ServiceType elements set
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentAmend" request
		And I set the Created Appointment Description to "TestChangedDescription"
		And I set the Created Appointment Comment to "TestChangedComment"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointments returned must be in the future
		And the Appointment Metadata should be valid
		And the Appointment Status should be valid
		And the Appointment Start should be valid
		And the Appointment End should be valid
		And the Appointment Slots should be valid
		And the Appointment Participants should be valid and resolvable
		And the Appointment Priority should be valid
		And the Appointment Participant Type and Actor should be valid
		And the Appointment Identifiers should be valid
		And the Appointment Description should be valid for "TestChangedDescription"
		And the Appointment Comment should be valid for "TestChangedComment"
		And the Appointment Created must be valid
		And the Appointment DeliveryChannel must be valid
		And the Appointment PractitionerRole must be valid
		And the Appointment Not In Use should be valid
		And Appointment Does not contains serviceCategory and serviceType elements
	