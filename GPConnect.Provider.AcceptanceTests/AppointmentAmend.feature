@appointment
Feature: AppointmentAmend

#Common
#Patient 1 used throughout, may want to think about using different patients
#Specification is unclear as to what can be updated

Scenario: I perform a successful amend appointment and change the comment to a custom message
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the created Appointment Comment to "customComment"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the response should be an Appointment resource
		And the appointment resource should contain a comment which equals "customComment"
		And the returned appointment resource should contain meta data profile and version id
	
Scenario: I perform a successful amend appointment and change the reason text to a custom message
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the created Appointment reason to "customComment"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the response should be an Appointment resource
		And the appointment resource should contain a reason text which equals "customComment"
		And the returned appointment resource should contain meta data profile and version id
@ignore
Scenario: I perform a successful amend appointment and change the description text to a custom message
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the created Appointment description to "customComment"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the response should be an Appointment resource
		And the appointment resource should contain description text which equals "customComment"
		And the returned appointment resource should contain meta data profile and version id

Scenario: Amend appointment and update element which cannot be updated
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the created Appointment description to "customComment"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	
Scenario Outline: Amend appointment making a request to an invalid URL
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the created Appointment reason to "customComment"
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
		And I store the created Appointment
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the created Appointment Comment to "customComment"
		And I do not send header "<Header>"
	When I make the "AppointmentAmend" request
	Then the response status code should be "400"
		And the response body should be FHIR JSON
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
		And I store the created Appointment
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the created Appointment Comment to "customComment"
		And I am performing the "<interactionId>" interaction
	When I make the "AppointmentAmend" request
	Then the response status code should be "400"
		And the response body should be FHIR JSON
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
		And I store the created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the created Appointment Comment to "customComment"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the appointment resource should contain a comment which equals "customComment"
	Examples:
		| Parameter             | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Amend appointment using the accept header to request response format
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the created Appointment Comment to "customComment"
		And I set the Accept header to "<Header>"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the appointment resource should contain a comment which equals "customComment"
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Amend appointment using the _format and accept parameter to request response format
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the created Appointment Comment to "customComment"
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		#Further validation on the appointment is probably sensible to ensure it is valid, ie identifier and ensure it belongs to patient 1 
		And the appointment resource should contain a comment which equals "customComment"
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/json+fhir | application/json+fhir | JSON       |
		| application/json+fhir | application/xml+fhir  | XML        |
		| application/xml+fhir  | application/json+fhir | JSON       |
		| application/xml+fhir  | application/xml+fhir  | XML        |

Scenario: Amend appointment and check the returned appointment resource conforms to the GPConnect specification
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the created Appointment Comment to "customComment"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the appointment resource should contain a comment which equals "customComment"
		And the appointment response resource contains a status with a valid value
		And the appointment response resource contains an start date
		And the appointment response resource contains an end date
		And the appointment response resource contains a slot reference
		And the appointment response resource contains atleast 2 participants a practitioner and a patient
		And if the appointment response resource contains a reason element and coding the codings must be one of the three allowed with system code and display elements
		And if the appointment contains a priority element it should be a valid value
		And the returned appointment participants must contain a type or actor element
		And if the appointment response resource contains any identifiers they must have a value
		

Scenario: Amend appointment prefer header set to representation
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the created Appointment Comment to "customComment"
		And I set the Prefer header to "return=representation"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the content-type should not be equal to null
		And the content-length should not be equal to zero

Scenario: Amend appointment prefer header set to minimal
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment	
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the created Appointment Comment to "customComment"
		And I set the Prefer header to "return=minimal"
	When I make the "AppointmentAmend" request
	Then the response status code should indicate success
		And the response body should be empty
		And the content-type should be equal to null

Scenario: Conformance profile supporAmend appointment send an update with an invalid if-match headerts the amend appointment operation
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the conformance profile should contain the "Appointment" resource with a "update" interaction

Scenario: Amend appointment send an update with an invalid if-match header
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I set "If-Match" request header to "INVALID"
	When I amend "patientApp" by changing the comment to "customComment"
	Then the response status code should be "409"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

#Name should be improved to be made clearer given the tests complexity
Scenario: Amend appointmentss
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I perform an appointment read for the appointment called "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response ETag is saved as "etagAmend"
		And the response should be an Appointment resource
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I set "If-Match" request header to resource stored "etagAmend"
	When I amend "patientApp" by changing the comment to "customComment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the appointment resource should contain a comment which equals "customComment"

Scenario: Amend appointment and send an invalid bundle resource
	Given I configure the default "AppointmentAmend" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I create a bundle resource and add it to the request
	When I make the "AppointmentAmend" request
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Amend appointment and send an invalid appointment resource
	Given I store the schedule for "ORG1" called "getScheduleResponseBundle" and create an appointment called "CustomAppointment1" for patient "patient1" using the interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I amend "CustomAppointment1" by changing the comment to "customComment" and send an empty appointment resource
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"