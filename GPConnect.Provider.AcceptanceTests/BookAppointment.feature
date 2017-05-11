Feature: BookAppointment

Background:
	Given I have the test patient codes
	Given I have the test ods codes


Scenario Outline: Book single appointment for patient 
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "<interactionId>"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		 Examples:
		 | interactionId                                                 |
		 | urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |

Scenario: Book Appointment with invalid url for booking appointment
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	When I make a GET request to "/Appointment/5"
	Then the response status code should indicate failure

Scenario Outline: Book appointment failure due to missing header
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I do not send header "<Header>"
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "<interactionId>"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Header            |interactionId                                                 |
		| Ssp-TraceID       |urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |
		| Ssp-From          |urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |
		| Ssp-To            |urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |
		| Ssp-InteractionId |urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |
		| Authorization     |urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |

Scenario Outline: Book appointment accept header and _format parameter
    Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
        And I set the Accept header to "<Header>"
        And I add the parameter "_format" with the value "<Parameter>"
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "<interactionId>"
    Then the response status code should indicate created
        And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the returned appointment resource shall contains an id
		And the appointment resource should contain a single status element
		And the appointment response resource contains a slot reference
    Examples:
       | Header                | Parameter             | BodyFormat | interactionId                                                 |
       | application/json+fhir | application/json+fhir | JSON       | urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |
       | application/json+fhir | application/xml+fhir  | XML        | urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |
       | application/xml+fhir  | application/json+fhir | JSON       | urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |
       | application/xml+fhir  | application/xml+fhir  | XML        | urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |

Scenario Outline: Book appointment _format parameter only
	 Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
        And I set the Accept header to "<Header>"
        And I add the parameter "_format" with the value "<Parameter>"
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "<interactionId>"
	Then the response status code should indicate created
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the returned appointment resource shall contains an id
		And the appointment resource should contain a single status element
		And the appointment response resource contains a slot reference
    Examples:
        | Parameter             | BodyFormat | interactionId                                                 |
        | application/json+fhir | JSON       | urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |
        | application/xml+fhir  | XML        | urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |

Scenario Outline: Book appointment accept header variations
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the Accept header to "<Header>"
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "<interactionId>"
	Then the response status code should indicate created
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the returned appointment resource shall contains an id
		And the appointment resource should contain a single status element
		And the appointment response resource contains a slot reference
	Examples:
		 | Header                | BodyFormat |interactionId                                                 |
		 | application/json+fhir | JSON       |urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |
		 | application/xml+fhir  | XML        |urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |

##Come back to get working
Scenario Outline: Book appointment prefer header set to representation
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the Prefer header to "<Header>"
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "<interactionId>"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
	Examples:
		| Header                |interactionId                                                 |
		| return-representation |urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |

##Come back to get working
Scenario Outline: Book appointment prefer header set to minimal
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I set the Prefer header to "<Header>"
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "<interactionId>"
	Then the response status code should indicate created
	Examples:
		| Header                |interactionId                                                 |
		| return=minimal        |urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |


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
                                                  
Scenario Outline: Book Appointment and check response returns the correct values
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "<interactionId>"
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
		Examples:
		|interactionId                                                 |
		|urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |
  
Scenario Outline: Book Appointment and check response returns the relevent structured definition
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "<interactionId>"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource should contain meta data profile and version id
	Examples:
		|interactionId                                                 |
		|urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |

Scenario Outline: Book Appointment and appointment participant is valid
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "<interactionId>"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the appointment response resource contains atleast 2 participants a practitioner and a patient
		And the returned appointment participants must contain a type or actor element
	Examples:
		|interactionId                                                 |
		|urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |

Scenario Outline: Book Appointment and check extension methods are valid
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "<interactionId>"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And if the returned appointment category element is present it is populated with the correct values
		And if the returned appointment booking element is present it is populated with the correct values
		And if the returned appointment contact element is present it is populated with the correct values
		And if the returned appointment cancellation reason element is present it is populated with the correct values
	Examples:
		|interactionId                                                 |
		|urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |

Scenario Outline: Book Appointment and remove a participant from the appointment booking
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create an appointment for patient "patient1" called "<Appointment>" from schedule "getScheduleResponseBundle"
	Then I remove the participant from the appointment called "<Appointment>" which starts with reference "<participant>"
	Then I book the appointment called "<Appointment>"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		Examples:
		| Appointment  | interactionId                                                 | participant  |
		| Appointment1 | urn:nhs:names:services:gpconnect:fhir:rest:create:appointment | Patient      |
		| Appointment2 | urn:nhs:names:services:gpconnect:fhir:rest:create:appointment | Location     |
		| Appointment3 | urn:nhs:names:services:gpconnect:fhir:rest:create:appointment | Practitioner |



Scenario Outline: Book Appointment and remove all participants
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create an appointment for patient "patient1" called "<Appointment>" from schedule "getScheduleResponseBundle"
	Then I remove the participant from the appointment called "<Appointment>" which starts with reference "<participant>"
	Then I remove the participant from the appointment called "<Appointment>" which starts with reference "<participant1>"
	Then I remove the participant from the appointment called "<Appointment>" which starts with reference "<participant2>"
	Then I book the appointment called "<Appointment>"
	Then the response status code should indicate failure
		And the response status code should be "422"
	Examples:
		| Appointment  | interactionId                                                 | participant | participant1 | participant2 |
		| Appointment1 | urn:nhs:names:services:gpconnect:fhir:rest:create:appointment | Patient     | Location     | Practitioner |         

Scenario Outline: Book single appointment for patient and send additional extension with value populated
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create an appointment for patient "patient1" called "<Appointment>" from schedule "getScheduleResponseBundle"
	Then I add an extra invalid extension to the appointment called "<Appointment>" and populate the value
	Then I book the appointment called "<Appointment>"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		Examples:
		| Appointment  |interactionId                                                 |
		| Appointment3 |urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |

Scenario Outline: Book single appointment for patient and send additional extensions with system populated
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create an appointment for patient "patient1" called "<Appointment>" from schedule "getScheduleResponseBundle"
	Then I add an extra invalid extension to the appointment called "<Appointment>" and populate the system
	Then I book the appointment called "<Appointment>"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		Examples:
		| Appointment  |interactionId                                                 |
		| Appointment3 |urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |

Scenario Outline: Book single appointment for patient and send additional extensions with system and value populated
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
	Then I create an appointment for patient "patient1" called "<Appointment>" from schedule "getScheduleResponseBundle"
	Then I add an extra invalid extension to the appointment called "<Appointment>" and populate the system and value
	Then I book the appointment called "<Appointment>"
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		Examples:
		| Appointment  |interactionId                                                 |
		| Appointment3 |urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |

Scenario Outline: Book appointment with invalid slot reference
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
	Then I create an appointment with slot reference "<slotReference>" for patient "patient1" called "<Appointment>" from schedule "getScheduleResponseBundle"
	Then I book the appointment called "<Appointment>"
	Then the response status code should indicate failure
		Examples:
		| Appointment  | slotReference |interactionId                                                 |
		| Appointment3 | 45555555      |urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |
		| Appointment3 | 455g55555     |urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |
		| Appointment3 | 45555555##    |urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |
		| Appointment3 | hello         |urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |

Scenario Outline: Book single appointment for patient and check the location reference is valid
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
	When I book an appointment for patient "patient1" on the provider system with the schedule name "getScheduleResponseBundle" with interaction id "<interactionId>"
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
		Examples:
		| interactionId  |
		|urn:nhs:names:services:gpconnect:fhir:rest:create:appointment |






