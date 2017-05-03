Feature: BookAppointment

Background:
	Given I have the test patient codes
	Given I have the test ods codes

@Appointment

Scenario: Book single appointment for patient 
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource

Scenario: Book Appointment with invalid url for booking appointment
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	When I make a GET request to "/Appointment/5"
	Then the response status code should indicate failure

Scenario Outline: Book appointment failure due to missing header
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I do not send header "<Header>"
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle"
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
       	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
        And I set the Accept header to "<Header>"
        And I add the parameter "_format" with the value "<Parameter>"
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle"
    Then the response status code should indicate success
        And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
    Examples:
       | Header                | Parameter             | BodyFormat |
       | application/json+fhir | application/json+fhir | JSON       |
       | application/json+fhir | application/xml+fhir  | XML        |
       | application/xml+fhir  | application/json+fhir | JSON       |
       | application/xml+fhir  | application/xml+fhir  | XML        | 


Scenario Outline: Book appointment accept header variations
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I set the Accept header to "<Header>"
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
	Examples:
		 | Header                | BodyFormat |
		 | application/json+fhir | JSON       |
		 | application/xml+fhir  | XML        |

Scenario Outline: Book appointment prefer header variations
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I set the Prefer header to "<Header>"
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
	Examples:
		| Header         |
		| representation |
		| minimal        |

Scenario Outline: Book appointment interaction id incorrect fail
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "<interactionId>"
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
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource shall contains an id
		And the bundle appointment resource should contain a single status element
		And the bundle appointment resource should contain a single start element
		And the bundle appointment resource should contain a single end element
		And the bundle appointment resource should contain at least one participant
		And the bundle appointment resource should contain at least one slot reference
		And the appointment response contains a type with a valid system code and display
		And if the appointment resource contains a priority the value is valid
  
Scenario: Book Appointment and check response returns the relevent structured definition
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource should contain meta data profile and version id


Scenario: Book Appointment and check slot reference is valid
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the appointment response resource contains a slot reference
		And the slot reference is present and valid


Scenario: Book Appointment and appointment participant is valid
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the bundle appointment resource should contain at least one participant
		And if appointment is present the single or multiple participant must contain a type or actor
		And if the appointment participant contains a type is should have a valid system and code

Scenario: Book Appointment and check extension methods are valid
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And if the appointment category element is present it is populated with the correct values
		And if the appointment booking element is present it is populated with the correct values
		And if the appointment contact element is present it is populated with the correct values
		And if the appointment cancellation reason element is present it is populated with the correct values


Scenario Outline: Book Appointment and remove patient participant
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create an appointment for patient "patient1" unless 1 exists and save the appointment called "<Appointment>"
	Then I remove the patient participant from the appointment called "<Appointment>"
	Then I book the appointment called "<Appointment>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
	Examples:
		| Appointment  |
		| Appointment1 |

Scenario Outline: Book Appointment and remove location participant
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create an appointment for patient "patient1" unless 1 exists and save the appointment called "<Appointment>"
	Then I remove the location participant from the appointment called "<Appointment>"
	Then I book the appointment called "<Appointment>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		Examples:
		| Appointment  |
		| Appointment1 |

Scenario Outline: Book Appointment and remove practitioner participant
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create an appointment for patient "patient1" unless 1 exists and save the appointment called "<Appointment>"
	Then I remove the practitioner participant from the appointment called "<Appointment>"
	Then I book the appointment called "<Appointment>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
	And the response should be an Appointment resource
	Examples:
		| Appointment  |
		| Appointment1 |
