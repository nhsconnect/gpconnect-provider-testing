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

Scenario: Cancel appointment invalid cancel extension
	Given I find or create an appointment with status Booked for patient "CustomAppointment1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment called "patientApp" with an invalid extension
	Then the response status code should indicate failure
	
	


	
		 