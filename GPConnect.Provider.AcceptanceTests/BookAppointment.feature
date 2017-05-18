Feature: BookAppointment

Background:
	Given I have the test patient codes
	Given I have the test ods codes

Scenario: Book single appointment for patient 
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
	
Scenario: Book Appointment with invalid url for booking appointment
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" via url "/Appointments"
	Then the response status code should indicate failure

Scenario Outline: Book appointment failure due to missing header
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I do not send header "<Header>"
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" without status check
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

Scenario Outline: Book appointment accept header and _format parameter
    Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
        And I set the Accept header to "<Header>"
        And I add the parameter "_format" with the value "<Parameter>"
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
    Then the response status code should indicate created
        And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the returned appointment resource shall contains an id
		And the appointment resource should contain a single status element
		And the appointment response resource contains a slot reference
    Examples:
       | Header                | Parameter             | BodyFormat | 
       | application/json+fhir | application/json+fhir | JSON       |
       | application/json+fhir | application/xml+fhir  | XML        |
       | application/xml+fhir  | application/json+fhir | JSON       |
       | application/xml+fhir  | application/xml+fhir  | XML        |

Scenario Outline: Book appointment _format parameter only
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
        And I add the parameter "_format" with the value "<Parameter>"
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Then the response status code should indicate created
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the returned appointment resource shall contains an id
		And the appointment resource should contain a single status element
		And the appointment response resource contains a slot reference
    Examples:
        | Parameter             | BodyFormat |
        | application/json+fhir | JSON       |
        | application/xml+fhir  | XML        |

Scenario Outline: Book appointment accept header variations
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the Accept header to "<Header>"
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Then the response status code should indicate created
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the returned appointment resource shall contains an id
		And the appointment resource should contain a single status element
		And the appointment response resource contains a slot reference
	Examples:
		 | Header                | BodyFormat |
		 | application/json+fhir | JSON       |
		 | application/xml+fhir  | XML        |

Scenario: Book appointment prefer header set to representation
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the Prefer header to "return=representation"
	When I book an appointment for patient "CustomAppointment1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" without header clean up
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the content-type should not be equal to null
		And the content-length should not be equal to zero

Scenario: Book appointment prefer header set to minimal
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the Prefer header to "return=minimal"
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" without header clean up
	Then the response status code should indicate created
		And the response body should be empty
		And the content-type should be equal to null
		And the content-length should be equal to zero

Scenario Outline: Book appointment interaction id incorrect fail
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "<interactionId>" without status check
    Then the response status code should be "400"
        And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
    Examples:
       | interactionId                                                     |
       | urn:nhs:names:services:gpconnect:fhir:rest:search:organization    |
       | urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
       |                                                                   |
       | null                                                              |
                                                  
Scenario: Book Appointment and check response returns the correct values
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource shall contains an id
		And the appointment resource should contain a single status element
		And the appointment resource should contain a single start element
		And the appointment resource should contain a single end element
		And the appointment resource should contain at least one participant
		And the appointment resource should contain at least one slot reference
		And if the appointment resource contains a priority the value is valid
  
Scenario: Book Appointment and check response returns the relevent structured definition
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource should contain meta data profile and version id

Scenario: Book Appointment and appointment participant is valid
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the appointment response resource contains atleast 2 participants a practitioner and a patient
		And the returned appointment participants must contain a type or actor element

Scenario: Book Appointment and check extension methods are valid
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And if the returned appointment category element is present it is populated with the correct values
		And if the returned appointment booking element is present it is populated with the correct values
		And if the returned appointment contact element is present it is populated with the correct values
		And if the returned appointment cancellation reason element is present it is populated with the correct values

Scenario: Book Appointment and remove location from the appointment booking
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create an appointment for patient "patient1" called "Appointment2" from schedule "getScheduleResponseBundle"
	Then I remove the participant from the appointment called "Appointment2" which starts with reference "Location"
	Then I book the appointment called "Appointment2"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource

Scenario Outline: Book Appointment and remove manadatory resources from the appointment booking
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create an appointment for patient "patient1" called "<Appointment>" from schedule "getScheduleResponseBundle"
	Then I remove the participant from the appointment called "<Appointment>" which starts with reference "<participant>"
	Then I book the appointment called "<Appointment>" without status check
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		And the response status code should be "400"
	Examples:
		| Appointment  | participant  |
		| Appointment1 | Patient      |
		| Appointment3 | Practitioner |

Scenario: Book Appointment and remove all participants
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create an appointment for patient "patient1" called "Appointment1" from schedule "getScheduleResponseBundle"
	Then I remove the participant from the appointment called "Appointment1" which starts with reference "Patient"
	Then I remove the participant from the appointment called "Appointment1" which starts with reference "Location"
	Then I remove the participant from the appointment called "Appointment1" which starts with reference "Practitioner"
	Then I book the appointment called "Appointment1" without status check
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		And the response status code should be "400"
    
Scenario: Book single appointment for patient and send additional extension with value populated
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create an appointment for patient "patient1" called "Appointment3" from schedule "getScheduleResponseBundle"
	Then I add an extra invalid extension to the appointment called "Appointment3" and populate the value
	Then I book the appointment called "Appointment3"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource

Scenario: Book single appointment for patient and send additional extensions with system populated
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create an appointment for patient "patient1" called "Appointment3" from schedule "getScheduleResponseBundle"
	Then I add an extra invalid extension to the appointment called "Appointment3" and populate the system
	Then I book the appointment called "Appointment3"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource

Scenario: Book single appointment for patient and send additional extensions with system and value populated
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create an appointment for patient "patient1" called "Appointment3" from schedule "getScheduleResponseBundle"
	Then I add an extra invalid extension to the appointment called "Appointment3" and populate the system and value
	Then I book the appointment called "Appointment3"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource

Scenario: Book single appointment for patient with random id
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create an appointment for patient "patient1" called "Appointment3" from schedule "getScheduleResponseBundle"
	Then I change the appointment id to "random" to the appointment called "Appointment3"
	Then I book the appointment called "Appointment3"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Book single appointment for patient and send extra fields in the resource
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create an appointment for patient "patient1" called "Appointment3" from schedule "getScheduleResponseBundle"
	Then I book the appointment called "Appointment3" with an invalid field
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Book appointment with invalid slot reference
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create an appointment with slot reference "<slotReference>" for patient "patient1" called "Appointment3" from schedule "getScheduleResponseBundle"
	Then I book the appointment called "Appointment3" without status check
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		And the response status code should be "422"
	Examples:
		| slotReference |
		| 45555555      |
		| 455g55555     |
		| 45555555##    |
		| hello         |

Scenario: Book single appointment for patient and check the location reference is valid
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the appointment location reference is present and is saved as "responseLocation"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:location" interaction
	When I make a GET request to saved location resource "responseLocation"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a valid Location resource
		And if the location response resource contains an identifier it is valid

Scenario: Book appointment with invalid start element in appointment resource
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create an appointment for patient "patient1" called ""Appointment3" from schedule "getScheduleResponseBundle"
	Then I set the appointment start element to null for "Appointment3"
	Then I book the appointment called "Appointment3" without status check
	Then the response status code should indicate failure
		And the response body should be FHIR JSON

Scenario: Book appointment with invalid end element in appointment resource
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create an appointment for patient "patient1" called "Appointment3" from schedule "getScheduleResponseBundle"
	Then I set the appointment end element to null for "Appointment3"
	Then I book the appointment called "Appointment3" without status check
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Book appointment and send patient resource in the request
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "AppointmentReadPatientResource"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create an appointment for patient "patient1" called "Appointment3" using a patient resource
	Then I book the appointment called "Appointment3" without status check
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Book appointment and send bundle resource in the request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create a new bundle to contain an appointment for patient "patient1" called "Appointment3"
	Then I book the appointment called "Appointment3" without status check
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"