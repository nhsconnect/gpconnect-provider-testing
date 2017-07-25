@appointment
Feature: AppointmentAmend
#Specification is unclear as to what can be updated
Scenario Outline: I perform a successful amend appointment and change the comment to a custom message
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment Comment to "customComment"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointment Comment should be valid for "customComment"
		And the Appointment Metadata should be valid
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |
		| patient4 |
		| patient5 |
		| patient6 |
		| patient7 |
		| patient8 |
		| patient8 |

Scenario: I perform a successful amend appointment and change the reason text to a custom message
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment Reason to "customComment"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointment Reason Text should be valid for "customComment"
		And the Appointment Metadata should be valid
@ignore
Scenario: I perform a successful amend appointment and change the description text to a custom message
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment Description to "customComment"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointment Description should be valid for "customComment"
		And the Appointment Metadata should be valid

Scenario: Amend appointment and update element which cannot be updated
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment Description to "customComment"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	
Scenario Outline: Amend appointment making a request to an invalid URL
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment Reason to "customComment"
	When I make a GET request to "<url>"
	Then the response status code should indicate failure
	Examples:
		| url                 |
		| /Appointment/!      |
		| /APPointment/23     |
		| /Appointment/#      |
		| /Appointment/update |

Scenario Outline: Amend appointment failure due to missing header
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment Comment to "customComment"
		And I do not send header "<Header>"
	When I make the "AppointmentAmend" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Header            |
		| Ssp-TraceID       |
		| Ssp-From          |
		| Ssp-To            |
		| Ssp-InteractionId |
		| Authorization     |

Scenario Outline: Amend appointment failure with incorrect interaction id
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment Comment to "customComment"
		And I am performing the "<interactionId>" interaction
	When I make the "AppointmentAmend" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| interactionId                                                     |
		| urn:nhs:names:services:gpconnect:fhir:rest:update:appointmentt    |
		| urn:nSs:namEs:servIces:gpconnect:fhir:rest:update:appointmenT     |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		|                                                                   |
		| null                                                              |

Scenario Outline: Amend appointment using the _format parameter to request response format
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment Comment to "customComment"
		And I add a Format parameter with the Value "<Format>"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be an Appointment
		And the Appointment Comment should be valid for "customComment"
	Examples:
		| Format                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Amend appointment using the accept header to request response format
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment Comment to "customComment"
		And I set the Accept header to "<Header>"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be an Appointment
		And the Appointment Metadata should be valid
		And the Appointment Comment should be valid for "customComment"
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Amend appointment using the _format and accept parameter to request response format
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment Comment to "customComment"
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be an Appointment
		And the Appointment Metadata should be valid
		And the Appointment Comment should be valid for "customComment"
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/json+fhir | application/json+fhir | JSON       |
		| application/json+fhir | application/xml+fhir  | XML        |
		| application/xml+fhir  | application/json+fhir | JSON       |
		| application/xml+fhir  | application/xml+fhir  | XML        |

Scenario: Amend appointment and check the returned appointment resource conforms to the GPConnect specification
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment Comment to "customComment"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Response Resource should be an Appointment
		And the Appointment Comment should be valid for "customComment"
		And the Appointment Status should be valid
		And the Appointment Start should be valid
		And the Appointment End should be valid
		And the Appointment Slots should be valid
		And the Appointment Participants should be valid and resolvable
		And the Appointment Reason should be valid
		And the Appointment Priority should be valid
		And the Appointment Participant Type and Actor should be valid
		And the Appointment Identifiers should be valid
		
Scenario: Amend appointment prefer header set to representation
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment Comment to "customComment"
		And I set the Prefer header to "return=representation"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Response Resource should be an Appointment
		And the content-type should not be equal to null
		And the content-length should not be equal to zero

Scenario: Amend appointment prefer header set to minimal
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment Comment to "customComment"
		And I set the Prefer header to "return=minimal"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the response body should be empty
		And the content-type should be equal to null

Scenario: Amend appointment send an update with an invalid if-match header
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment Comment to "customComment"
		And I set If-Match request header to "invalidEtag"
	When I make the "AppointmentAmend" request
	Then the response status code should be "409"

Scenario: Amend appointment set etag and check etag is the same in the returned amended appointment
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"		
		And I store the Created Appointment			
	Given I read the Stored Appointment
		And I store the Appointment 
		And I store the Appointment Id
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment Comment to "customComment"
		And I set the If-Match header to the Stored Appointment Version Id
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointment Comment should be valid for "customComment"

Scenario: Amend appointment and send an invalid bundle resource
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment Comment to "customComment"
	When I make the "AppointmentAmend" request with invalid Resource type
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Amend appointment and send an invalid appointment resource
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment Comment to "customComment"
		And I set the Created Appointment to a new Appointment
	When I make the "AppointmentAmend" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
				
Scenario: Conformance profile support the Amend appointment operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the conformance profile should contain the "Appointment" resource with a "update" interaction