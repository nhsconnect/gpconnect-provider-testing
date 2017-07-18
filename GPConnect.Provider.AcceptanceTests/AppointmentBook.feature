@appointment
Feature: AppointmentBook

Scenario: Book single appointment for patient
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response should be an Appointment resource
	
Scenario: Book Appointment with invalid url for booking appointment
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
	When I make a POST request to ""/Appointmentz\"
	Then the response status code should be "404"

Scenario Outline: Book appointment failure due to missing header
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I do not send header "<Header>"
	When I make the "AppointmentCreate" request
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

Scenario Outline: Book appointment accept header and _format parameter to request response format
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the returned resource shall contains a logical id
		And the appointment resource should contain a status element
		And the appointment response resource contains a slot reference
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/json+fhir | application/json+fhir | JSON       |
		| application/json+fhir | application/xml+fhir  | XML        |
		| application/xml+fhir  | application/json+fhir | JSON       |
		| application/xml+fhir  | application/xml+fhir  | XML        |

Scenario Outline: Book appointment _format parameter only but varying request content types
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the request content type to "<ContentType>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the returned resource shall contains a logical id
		And the appointment resource should contain a status element
		And the appointment response resource contains a slot reference
	Examples:
		| ContentType           | Parameter             | BodyFormat |
		| application/json+fhir | application/json+fhir | JSON       |
		| application/json+fhir | application/xml+fhir  | XML        |
		| application/xml+fhir  | application/json+fhir | JSON       |
		| application/xml+fhir  | application/xml+fhir  | XML        |

#improve name to be more descriptive
Scenario Outline: Book appointment accept header to request response format
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Accept header to "<Header>"
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the returned resource shall contains a logical id
		And the appointment resource should contain a status element
		And the appointment response resource contains a slot reference
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario: Book appointment prefer header set to representation
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Prefer header to "return=representation"
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the content-type should not be equal to null
		And the content-length should not be equal to zero

Scenario: Book appointment prefer header set to minimal
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Prefer header to "return=minimal"
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be empty
		And the content-type should be equal to null

Scenario Outline: Book appointment with invalid interaction id
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I am performing the "<interactionId>" interaction
	When I make the "AppointmentCreate" request
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| interactionId                                                     |
		| urn:nhs:names:services:gpconnect:fhir:rest:search:organization    |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		|                                                                   |
		| null                                                              |

Scenario: Book Appointment and check response contains the manadatory elements
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned resource shall contains a logical id
		And the appointment resource should contain a status element
		And the appointment resource should contain a single start element
		And the appointment resource should contain a single end element
		And the appointment resource should contain at least one participant
		And the appointment resource should contain at least one slot reference
		And if the appointment resource contains a priority the value is valid

Scenario: Book Appointment and check returned appointment resource contains meta data
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource should contain meta data profile and version id

#improve name to be more descriptive
Scenario: Book Appointment and appointment participant is valid
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the appointment response resource contains atleast 2 participants a practitioner and a patient
		And the returned appointment participants must contain a type or actor element

#improve name to be more descriptive
Scenario: Book Appointment and check extensions are valid
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And if the returned appointment category element is present it is populated with the correct values
		And if the returned appointment booking element is present it is populated with the correct values
		And if the returned appointment contact element is present it is populated with the correct values
		And if the returned appointment cancellation reason element is present it is populated with the correct values

Scenario: Book Appointment without location participant
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the participant "Location" from the created Appointment
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource

Scenario Outline: Book Appointment and remove manadatory resources from the appointment booking
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the participant "<ParticipantToRemove>" from the created Appointment
	When I make the "AppointmentCreate" request
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"
	Examples:
		| ParticipantToRemove |
		| Patient             |
		| Practitioner        |

Scenario: Book Appointment and remove all participants
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the participant "Location" from the created Appointment
		And I remove the participant "Patient" from the created Appointment
		And I remove the participant "Practitioner" from the created Appointment
	When I make the "AppointmentCreate" request
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book appointment containing additional extension with only value populated
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I add an extra invalid extension to the created appointment only populating the value
	When I make the "AppointmentCreate" request
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book appointment containing additional extensions with only the system populated
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I add an extra invalid extension to the appointment only populating the url
	When I make the "AppointmentCreate" request
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book single appointment for patient and send additional extensions with url and value populated
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I add an extra invalid extension to the created appointment containing the url code and display
	When I make the "AppointmentCreate" request
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book appointment for patient with id
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I change the created appointment id to "1111222233334444"
	When I make the "AppointmentCreate" request
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

@ignore
Scenario: Book appointment for patient and send extra fields in the resource
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
	When I book the appointment called "Appointment" with an invalid field
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario Outline: Book appointment with invalid slot reference
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I change the created appointment slot reference to "<slotReference>"
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"
	Examples:
		| slotReference    |
		| Slot/44445555555 |
		| Slot/45555g55555 |
		| Slot/45555555##  |
		| Slot/hello       |

Scenario: Book single appointment for patient and check the location reference is valid
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And any location participant references included in returned appointment should be valid
	
Scenario: Book appointment with missing start element in appointment resource
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the created appointment start element
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book appointment with missing end element in appointment resource
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the created appointment end element
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book appointment with missing status element in appointment resource
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the created appointment status element
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book appointment with missing slot element in appointment resource
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the created appointment slot element
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book Appointment and remove identifier value from the appointment booking
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
	Then I set the created appointment identifier value element to null
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario Outline: Book Appointment and remove reason coding element from the appointment booking
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
	Then I set the created appointment reason coding system element to null
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"
	Examples:
		| CodingElement |
		| system        |
		| code          |
		| display       |

Scenario Outline: Book Appointment and remove participant status from the appointment booking
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
	Then I set the created appointment Patient participant status element to null
	When I make the "AppointmentCreate" request
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Participant  |
		| Patient      |
		| Practitioner |
		| Location     |

Scenario Outline: Book Appointment and remove participant type coding element from the appointment booking
		Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
	Then I set the created appointment "<Participant>" participant type coding "<CodingElement>" element to null
	When I make the "AppointmentCreate" request
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Participant  | CodingElement |
		| Patient      | system        |
		| Patient      | code          |
		| Patient      | display       |
		| Practitioner | system        |
		| Practitioner | code          |
		| Practitioner | display       |

Scenario: Book appointment and send an invalid bundle resource
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request with invalid Resource type
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book appointment and send an invalid appointment resource
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get the Schedule for Organization Code "ORG1"
		And I store the Schedule
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I set created appointment to a new appointment resource
		When I make the "AppointmentAmend" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

@ignore
Scenario: Book appointment for temporary patient

@ignore
@Manual
Scenario: Multi slot booking
	# Multiple adjacent slots success
	# Non adjacent slot failure
	# Slots from different schedules that are adjacent failure
	# Slots from different schedules which are not adjacent failure

@ignore
@Manual
Scenario: Extension supported
	# Is the data represented by the extensions such as booking method supported by the provider system? If so are the details saved when sent in and returned when resource is returned.