Feature: CancelAppointment

Background:
	Given I have the test patient codes
	Given I have the test ods codes

Scenario Outline: I perform a successful cancel appointment
	Given I find or create an appointment with status Booked for patient "<PatientName>" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment called "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource should be cancelled
			Examples: 
		| PatientName        |
		| patient1           |
		| patient2           |
		| patient3           |
		| CustomAppointment1 |

Scenario Outline: Cancel appointment sending invalid URL
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I set the URL to "<url>" and cancel "patientApp"
	Then the response status code should indicate failure
	Examples: 
		| url                 |
		| /Appointment/!      |
		| /APPointment/23     |
		| /Appointment/#      |
		| /Appointment/cancel |

Scenario Outline: Cancel appointment failure due to missing header
		Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I do not send header "<Header>"
	When I cancel the appointment called "patientApp"
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

Scenario Outline: Cancel appointment failure with incorrect interaction id
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "<interactionId>" interaction
	When I cancel the appointment called "patientApp"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
	  | interactionId                                                     |
	  | urn:nhs:names:services:gpconnect:fhir:rest:update:appointmentss   |
	  | urn:nhs:names:services:gpconnect:fhir:rest:update:appointmenT     |
	  | urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
	  |                                                                   |
	  | null                                                              |

Scenario Outline: Cancel appointment _format parameter only
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I add the parameter "_format" with the value "<Parameter>"
	When I cancel the appointment called "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the returned appointment resource should be cancelled
	 Examples:
        | Parameter             | BodyFormat |
        | application/json+fhir | JSON       |
        | application/xml+fhir  | XML        |

Scenario Outline: Cancel appointment accept header and _format parameter
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the Accept header to "<Header>"
        And I add the parameter "_format" with the value "<Parameter>"
		When I cancel the appointment called "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the returned appointment resource should be cancelled
	 Examples:
	    | Header                | Parameter             | BodyFormat |
        | application/json+fhir | application/json+fhir | JSON       |
        | application/json+fhir | application/xml+fhir  | XML        |
        | application/xml+fhir  | application/json+fhir | JSON       |
        | application/xml+fhir  | application/xml+fhir  | XML        |   


Scenario Outline: Cancel appointment check cancellation reason is equal to the request cancellation reason
	Given I find or create an appointment with status Booked for patient "CustomAppointment1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment with cancel extension with url "<url>" code "<code>" and display "<display>" called "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource should be cancelled
		And the cancellation reason in the returned response should be equal to "<display>"
		Examples: 
		| url                                                                                             | code | display     |
		| http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1-0 | aa   | Too busy    |
		| http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1-0 | aa   | Car crashed |
		| http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1-0 | aa   | Too tired   |


Scenario Outline: Cancel appointment invalid cancellation extension
	Given I find or create an appointment with status Booked for patient "CustomAppointment1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment with cancel extension with url "<url>" code "<code>" and display "<display>" called "patientApp"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "500"
		Examples: 
		| url                                                                                             | code | display |
		|                                                                                                 |      |         |
		|                                                                                                 | ff   |         |
		|                                                                                                 |      | ee      |
		| http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1-0 |      |         |
		| http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1-0 | ff   |         |
		| http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1-0 |      | ee      |

Scenario: Cancel appointment and check the returned appointment resource is a valid resource
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment called "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource should be cancelled
		And the returned appointment resource should contain meta data profile and version id

Scenario: Conformance profile supports the cancel appointment operation
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the conformance profile should contain the "Appointment" resource with a "update" interaction

Scenario: Cancel appointment verify resource is updated when an valid ETag value is provided
Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I perform an appointment read for the appointment called "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response ETag is saved as "etagCancel"
		And the response should be an Appointment resource
	Given I am using the default server
	And I set "If-Match" request header to "etagCancel"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment called "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource should be cancelled
	
Scenario: Cancel appointment verify resource is not updated when an out of date ETag value is provided
Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I perform an appointment read for the appointment called "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response ETag is saved as "etagCancel"
		And the response should be an Appointment resource
	Given I am using the default server
	And I set If-Match request header to "hello"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment called "patientApp"
	Then the response status code should be "409"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: Cancel appointment compare values send in request and returned in the response
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment called "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource should be cancelled
		And the resource type of "patientApp" and the returned response should be equal
		And the id of "patientApp" and the returned response should be equal
		And the status of "patientApp" and the returned response should be equal
		And the extension of "patientApp" and the returned response should be equal
		And the description of "patientApp" and the returned response should be equal
		And the start and end date of "patientApp" and the returned response should be equal
		And the slot display and reference of "patientApp" and the returned response should be equal
		And the type and reference of "patientApp" and the returned response should be equal
		And the reason of "patientApp" and the returned response should be equal
		And the patient participant of "patientApp" and the returned response should be equal
		And the location participant of "patientApp" and the returned response should be equal
		And the practitioner participant of "patientApp" and the returned response should be equal




 