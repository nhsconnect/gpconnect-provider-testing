@appointment
Feature: AppointmentRead

Scenario Outline: I perform a successful Read appointment
	Given I create an Appointment for Patient "<PatientName>" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
	Examples:
		| PatientName |
		| patient1    |
		| patient2    |
		| patient3    |
		| patient4    |
		| patient5    |
		| patient6    |

Scenario Outline: Read appointment invalid appointment id
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Read Operation logical identifier used in the request to "<id>"
	When I make the "AppointmentRead" request
	Then the response status code should be "404"
		And the response body should be FHIR JSON
	Examples:
		| id          |
		| Invalid4321 |
		| 8888888888  |
		|             |

Scenario Outline: Read appointment with missing mandatory header
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I do not send header "<Header>"
	When I make the "AppointmentRead" request
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
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Interaction Id header to "<interactionId>"
	When I make the "AppointmentRead" request
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| interactionId                                                     |
		| urn:nhs:names:services:gpconnect:fhir:rest:search:organization    |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		|                                                                   |
		| null                                                              |

Scenario Outline: Read appointment using the _format parameter to request response format
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be an Appointment
	Examples:
		| Parameter             | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Read appointment using the accept header to request response format
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Accept header to "<Header>"
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat> 
		And the Response Resource should be an Appointment
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |


Scenario Outline: Read appointment using the _format parameter and accept header to request response format
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be an Appointment
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/json+fhir | application/json+fhir | JSON       |
		| application/json+fhir | application/xml+fhir  | XML        |
		| application/xml+fhir  | application/json+fhir | JSON       |
		| application/xml+fhir  | application/xml+fhir  | XML        |

Scenario: Read appointment valid request shall include id and structure definition profile
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Appointment Id should be valid
		And the Appointment Metadata should be valid

Scenario Outline: Read appointment ensure response appointments contain the manadatory elements
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
		And I set the Created Appointment Status to "<AppointmentStatus>"
	Given I configure the default "AppointmentRead" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Accept header to "<Header>"
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be an Appointment
		And the Appointment Status should be valid
		And the Appointment Start should be valid
		And the Appointment End should be valid
		And the Appointment Slots should be valid
		And the Appointment Participants should be valid and resolvable
	Examples:
		| AppointmentStatus | Header                | BodyFormat |
		| Booked            | application/json+fhir | JSON       |
		| Booked            | application/xml+fhir  | XML        |
		| Cancelled         | application/json+fhir | JSON       |
		| Cancelled         | application/xml+fhir  | XML        |

Scenario: Read appointment if resource contains identifier then the value is mandatory
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Appointment Identifiers should be valid

Scenario: Read appointment if reason is included in response check that it conforms to one of the three valid types
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Response Resource should be an Appointment
		And the Appointment Reason should be valid

Scenario: Read appointment containing a priority element and check that the priority is valid
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
		And I set the Created Appointment Priority to "1"
	Given I configure the default "AppointmentRead" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Response Resource should be an Appointment
		And the Appointment Priority should be valid

Scenario: Read appointment and all participants must have a type or actor element
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Response Resource should be an Appointment
		And the Appointment Participant Type and Actor should be valid

Scenario Outline: Read appointment if extensions are included they should be valid
Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Accept header to "<Header>"
		And I set the Created Appointment Priority to "1"
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be an Appointment
		And the Appointment Category Extension should be valid
		And the Appointment Booking Method Extension should be valid
		And the Appointment Contact Method Extension should be valid
		And the Appointment Cancellation Reason Extension should be valid
	Examples:
		| AppointmentStatus | Header                | BodyFormat |
		| Booked            | application/json+fhir | JSON       |
		| Booked            | application/xml+fhir  | XML        |
		| Cancelled         | application/json+fhir | JSON       |
		| Cancelled         | application/xml+fhir  | XML        |

Scenario: Read appointment and response should contain an ETag header
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
	When I make the "AppointmentRead" request 
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Response Resource should be an Appointment
		And the Response should contain the ETag header matching the Resource Version Id

# Potentially out of scope, outstanding issue on github "https://github.com/nhsconnect/gpconnect/issues/189"
@ignore
Scenario: VRead an appointment for a valid version of the patient appointment resource
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
		And I store the Appointment Version Id
	Given I configure the default "AppointmentRead" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
	When I make the "AppointmentRead" request 
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Response Resource should be an Appointment

# Potentially out of scope, outstanding issue on github "https://github.com/nhsconnect/gpconnect/issues/189"
@ignore
Scenario: VRead an appointment for a invalid version of the patient appoint resource
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
		And I set the If-Match header to the Stored Appointment Version Id
	Given I configure the default "AppointmentRead" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
	When I make the "AppointmentRead" request 
	Then the response status code should be "404"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource