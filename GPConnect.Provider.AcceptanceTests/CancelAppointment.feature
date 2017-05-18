Feature: CancelAppointment

Background:
	Given I have the test patient codes
	Given I have the test ods codes

Scenario: I perform a successful cancel appointment
	Given I find or create an appointment with status Booked for patient "CustomAppointment1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment called "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
	
		 