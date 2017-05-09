Feature: ReadAppointment

Background:
	Given I have the test patient codes
	Given I have the test ods codes

Scenario Outline: I perform a successful Read appointment
	Given I find or create "1" appointments for patient "<PatientName>" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I perform an appointment read for the first appointment saved in the bundle of resources stored against key "Patient1AppointmentsInBundle"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
	Examples: 
		| PatientName |
		| patient1    |
		| patient2    |
		| patient3    |

Scenario Outline: Read appointment invalid appointment id
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I make a GET request to "/Appointment/<id>"
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "REFERENCE_NOT_FOUND"
	Examples:
		| id          |
		| Invalid4321 |
		| 8888888888  |
		|             |

Scenario Outline: Read appointment failure due to missing header
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
		And I do not send header "<Header>"
	When I perform an appointment read for the first appointment saved in the bundle of resources stored against key "Patient1AppointmentsInBundle"
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

Scenario Outline: Read appointment failure with incorrect interaction id
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "<interactionId>" interaction
		And I do not send header "<Header>"
	When I perform an appointment read for the first appointment saved in the bundle of resources stored against key "Patient1AppointmentsInBundle"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
    Examples:
      | interactionId                                                     |
      | urn:nhs:names:services:gpconnect:fhir:rest:search:organization    |
      | urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
      |                                                                   |
      | null                                                              |

Scenario Outline: Read appointment _format parameter only
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
        And I add the parameter "_format" with the value "<Parameter>"
	When I perform an appointment read for the first appointment saved in the bundle of resources stored against key "Patient1AppointmentsInBundle"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
    Examples:
        | Parameter             | BodyFormat |
        | application/json+fhir | JSON       |
        | application/xml+fhir  | XML        |
        
Scenario Outline: Read appointment accept header and _format parameter
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
		And I set the Accept header to "<Header>"
        And I add the parameter "_format" with the value "<Parameter>"
	When I perform an appointment read for the first appointment saved in the bundle of resources stored against key "Patient1AppointmentsInBundle"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
    Examples:
        | Header                | Parameter             | BodyFormat |
        | application/json+fhir | application/json+fhir | JSON       |
        | application/json+fhir | application/xml+fhir  | XML        |
        | application/xml+fhir  | application/json+fhir | JSON       |
        | application/xml+fhir  | application/xml+fhir  | XML        |   

Scenario: Read appointment valid request shall include id and structure definition profile
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I perform an appointment read for the first appointment saved in the bundle of resources stored against key "Patient1AppointmentsInBundle"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the returned appointment resource shall contains an id
		And the returned appointment resource should contain meta data profile and version id

Scenario Outline: Read appointment check response contains required elements
	Given I find or create an appointment with status <AppointmentStatus> for patient "patient1" at organization "ORG1" and save the appointment resources to "<AppointmentStatus>Appointment<BodyFormat>"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
		And I set the Accept header to "<Header>"
	When I perform an appointment read appointment stored against key "<AppointmentStatus>Appointment<BodyFormat>"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the appointment response resource contains a status with a valid value
		And the appointment response resource contains an start date
		And the appointment response resource contains an end date
		And the appointment response resource contains a slot reference
		And the appointment response resource contains atleast 2 participants a practitioner and a patient
    Examples:
        | AppointmentStatus | Header                | BodyFormat |
        | Booked            | application/json+fhir | JSON       |
        | Booked            | application/xml+fhir  | XML        |
        | Cancelled         | application/json+fhir | JSON       |
        | Cancelled         | application/xml+fhir  | XML        |
		
Scenario: Read appointment if resource contains identifier then the value is mandatory
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I perform an appointment read for the first appointment saved in the bundle of resources stored against key "Patient1AppointmentsInBundle"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And if the appointment response resource contains any identifiers they must have a value

Scenario: Read appointment if reason is included in response check that it conforms to one of the three valid types
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I perform an appointment read for the first appointment saved in the bundle of resources stored against key "Patient1AppointmentsInBundle"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And if the appointment response resource contains a reason element and coding the codings must be one of the three allowed with system code and display elements

Scenario: Read appointment containing a priority element and check that the priority is valid
	Given I create an appointment for patient "patient1" at organization "ORG1" with priority "0" and save appintment resources to "Patient1PriorityAppointment"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I perform an appointment read appointment stored against key "Patient1PriorityAppointment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And if the appointment contains a priority element it should be a valid value

Scenario: Read appointment and all participants must have a type or actor element
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I perform an appointment read for the first appointment saved in the bundle of resources stored against key "Patient1AppointmentsInBundle"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment participants must contain a type or actor element

Scenario Outline: Read appointment if extensions are included they should be valid
	Given I find or create an appointment with status <AppointmentStatus> for patient "patient1" at organization "ORG1" and save the appointment resources to "<AppointmentStatus>Appointment<BodyFormat>"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
		And I set the Accept header to "<Header>"
	When I perform an appointment read appointment stored against key "<AppointmentStatus>Appointment<BodyFormat>"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And if the returned appointment contains appointmentCategory extension the value should be valid
		And if the returned appointment contains appointmentBookingMethod extension the value should be valid
		And if the returned appointment contains appointmentContactMethod extension the value should be valid
		And if the returned appointment contains appointmentCancellationReason extension the value should be valid
	Examples:
        | AppointmentStatus | Header                | BodyFormat |
        | Booked            | application/json+fhir | JSON       |
        | Booked            | application/xml+fhir  | XML        |
        | Cancelled         | application/json+fhir | JSON       |
        | Cancelled         | application/xml+fhir  | XML        |

Scenario: Read appointment and response should contain an ETag header
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I perform an appointment read for the first appointment saved in the bundle of resources stored against key "Patient1AppointmentsInBundle"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the response should contain the ETag header matching the resource version

Scenario: VRead an appointment for a valid version of the patient appointment resource
	Given I create an appointment for patient "patient1" at organization "ORG1" with priority "0" and save appintment resources to "Patient1Appointment"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I perform an appointment vread with history for appointment stored against key "Patient1Appointment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource

Scenario: VRead an appointment for a invalid version of the patient appoint resource
	Given I create an appointment for patient "patient1" at organization "ORG1" with priority "0" and save appintment resources to "Patient1Appointment"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I perform an appointment vread with version id "NotARealVersionId" for appointment stored against key "Patient1Appointment"
	Then the response status code should be "404"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource