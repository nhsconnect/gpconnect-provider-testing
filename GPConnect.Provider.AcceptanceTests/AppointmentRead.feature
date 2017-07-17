@appointment
Feature: AppointmentRead

#COMMON
#Refactor code using haydens test steps moving test setup behind the scenes
#Merge existing successful tests to reduce number of tests, also verifys the returned appointment is correct as has more params to satisfy
#Think about using more patients, only 1-3 used

Scenario Outline: I perform a successful Read appointment
	Given I create an Appointment for Patient "<PatientName>" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentRead" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
	#Think about adding further validation 
		And the response should be an Appointment resource
	Examples:
		| PatientName |
		| patient1    |
		| patient2    |
		| patient3    |

Scenario Outline: Read appointment invalid appointment id
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
        And I set the JWT requested record NHS number to config patient "patient1"
        And I set the JWT requested scope to "patient/*.read"
	When I make a GET request to "/Appointment/<id>"
	Then the response status code should be "404"
		And the response body should be FHIR JSON
	Examples:
		| id          |
		| Invalid4321 |
		| 8888888888  |
		|             |

Scenario Outline: Read appointment failure due to missing Ssp header
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
		And I do not send header "<Header>"
	When I perform an appointment read for the first appointment saved in the bundle of resources stored against key "Patient1AppointmentsInBundle" for patient "patient1"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	#Should auth not be included?
	Examples:
		| Header            |
		| Ssp-TraceID       |
		| Ssp-From          |
		| Ssp-To            |
		| Ssp-InteractionId |
#Why are these seperated?
Scenario: Read appointment failure due to missing Authoriztion header
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I perform an appointment read for the first appointment saved in the bundle of resources stored against key "Patient1AppointmentsInBundle" without the Authorization header
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Read appointment failure with incorrect interaction id
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "<interactionId>" interaction
	When I perform an appointment read for the first appointment saved in the bundle of resources stored against key "Patient1AppointmentsInBundle" for patient "patient1"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| interactionId                                                     |
		| urn:nhs:names:services:gpconnect:fhir:rest:search:organization    |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		|                                                                   |
		| null                                                              |

`#Make name more descriptive
Scenario Outline: Read appointment _format parameter only
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
		And I add the parameter "_format" with the value "<Parameter>"
	When I perform an appointment read for the first appointment saved in the bundle of resources stored against key "Patient1AppointmentsInBundle" for patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		#Think about adding further validation 
		And the response should be an Appointment resource
	Examples:
		| Parameter             | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

#Add for just accept header

#Make name more descriptive
Scenario Outline: Read appointment accept header and _format parameter
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I perform an appointment read for the first appointment saved in the bundle of resources stored against key "Patient1AppointmentsInBundle" for patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		#Think about adding further validation 
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
	When I perform an appointment read for the first appointment saved in the bundle of resources stored against key "Patient1AppointmentsInBundle" for patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the returned resource shall contains a logical id
		And the returned appointment resource should contain meta data profile and version id

#Name doesnt describe the test, link with test below which uses the same params
Scenario Outline: Read appointment check response contains required elements
	Given I find or create an appointment with status <AppointmentStatus> for patient "patient1" at organization "ORG1" and save the appointment resources to "<AppointmentStatus>Appointment<BodyFormat>"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
		And I set the Accept header to "<Header>"
	When I perform an appointment read appointment stored against key "<AppointmentStatus>Appointment<BodyFormat>" for patient "patient1"
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
	When I perform an appointment read for the first appointment saved in the bundle of resources stored against key "Patient1AppointmentsInBundle" for patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		#Think about adding further validation 
		And if the appointment response resource contains any identifiers they must have a value

Scenario: Read appointment if reason is included in response check that it conforms to one of the three valid types
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I perform an appointment read for the first appointment saved in the bundle of resources stored against key "Patient1AppointmentsInBundle" for patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		#Think about adding further validation 
		And if the appointment response resource contains a reason element and coding the codings must be one of the three allowed with system code and display elements

Scenario: Read appointment containing a priority element and check that the priority is valid
	Given I create an appointment for patient "patient1" at organization "ORG1" with priority "0" and save appintment resources to "Patient1PriorityAppointment"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I perform an appointment read appointment stored against key "Patient1PriorityAppointment" for patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		#Think about adding further validation 
		And if the appointment contains a priority element it should be a valid value

Scenario: Read appointment and all participants must have a type or actor element
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
	When I perform an appointment read for the first appointment saved in the bundle of resources stored against key "Patient1AppointmentsInBundle" for patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		#Think about adding further validation 
		And the returned appointment participants must contain a type or actor element

Scenario Outline: Read appointment if extensions are included they should be valid
	Given I find or create an appointment with status <AppointmentStatus> for patient "patient1" at organization "ORG1" and save the appointment resources to "<AppointmentStatus>Appointment<BodyFormat>"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
		And I set the Accept header to "<Header>"
	When I perform an appointment read appointment stored against key "<AppointmentStatus>Appointment<BodyFormat>" for patient "patient1"
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
	When I perform an appointment read for the first appointment saved in the bundle of resources stored against key "Patient1AppointmentsInBundle" for patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the response should contain the ETag header matching the resource version

Scenario: VRead an appointment for a valid version of the patient appointment resource
	Given I create an appointment for patient "patient1" at organization "ORG1" with priority "0" and save appintment resources to "Patient1Appointment"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
        And I set the JWT requested record NHS number to config patient "patient1"
        And I set the JWT requested scope to "patient/*.read"
	When I perform an appointment vread with history for appointment stored against key "Patient1Appointment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource

Scenario: VRead an appointment for a invalid version of the patient appoint resource
	Given I create an appointment for patient "patient1" at organization "ORG1" with priority "0" and save appintment resources to "Patient1Appointment"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:appointment" interaction
        And I set the JWT requested record NHS number to config patient "patient1"
        And I set the JWT requested scope to "patient/*.read"
	When I perform an appointment vread with version id "NotARealVersionId" for appointment stored against key "Patient1Appointment"
	Then the response status code should be "404"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource