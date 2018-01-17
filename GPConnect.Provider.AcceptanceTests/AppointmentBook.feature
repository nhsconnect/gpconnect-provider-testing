@appointment
Feature: AppointmentBook

Scenario: Book single appointment for patient
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the Response Resource should be an Appointment
		
Scenario Outline: Book Appointment with invalid url for booking appointment
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the request URL to "<url>"
	When I make the "AppointmentCreate" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource
	Examples:
		| url             |
		| appointmentqq/! |

Scenario Outline: Book appointment accept header and _format parameter to request response format
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be an Appointment
		And the Appointment Id should be valid
		And the Appointment Status should be valid
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/fhir+json | application/fhir+json | JSON       |
		| application/fhir+json | application/fhir+xml  | XML        |
		| application/fhir+xml  | application/fhir+json | JSON       |
		| application/fhir+xml  | application/fhir+xml  | XML        |

Scenario Outline: Book appointment _format parameter only but varying request content types
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the request content type to "<ContentType>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be an Appointment
		And the Appointment Id should be valid
		And the Appointment Status should be valid
		And the Appointment Slots should be valid
	Examples:
		| ContentType           | Parameter             | BodyFormat |
		| application/fhir+json | application/fhir+json | JSON       |
		| application/fhir+json | application/fhir+xml  | XML        |
		| application/fhir+xml  | application/fhir+json | JSON       |
		| application/fhir+xml  | application/fhir+xml  | XML        |

Scenario Outline: Book appointment accept header to request response format
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Accept header to "<Header>"
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be an Appointment
		And the Appointment Id should be valid
		And the Appointment Status should be valid
		And the Appointment Slots should be valid
	Examples:
		| Header                | BodyFormat |
		| application/fhir+json | JSON       |
		| application/fhir+xml  | XML        |

Scenario: Book appointment prefer header set to representation
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Prefer header to "return=representation"
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the Response Resource should be an Appointment
		And the content-type should not be equal to null
		And the content-length should not be equal to zero

Scenario: Book appointment prefer header set to minimal
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Prefer header to "return=minimal"
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be empty
		And the content-type should be equal to null

Scenario: Book Appointment and check response contains the manadatory elements
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the Response Resource should be an Appointment
		And the Appointment Status should be valid
		And the Appointment Start should be valid
		And the Appointment End should be valid
		And the Appointment Participants should be valid and resolvable
		And the Appointment Slots should be valid
		And the Appointment Description must be valid
		And the booking organization extension must be valid
		And the Appointment Created must be valid

Scenario: Book Appointment and check returned appointment resource contains meta data
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the Response Resource should be an Appointment
		And the Appointment Metadata should be valid

#improve name to be more descriptive
Scenario: Book Appointment and appointment participant is valid
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the Response Resource should be an Appointment
		And the Appointment Participants should be valid and resolvable
		And the Appointment Participant Type and Actor should be valid

Scenario: Book Appointment without practitioner participant
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the "Practitioner" Participants from the Created Appointment
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the Response Resource should be an Appointment

Scenario Outline: Book Appointment and remove manadatory resources from the appointment booking
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the "<ParticipantToRemove>" Participants from the Created Appointment
	When I make the "AppointmentCreate" request
	Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"
	Examples:
		| ParticipantToRemove |
		| Patient             |
		| Location            |

Scenario: Book Appointment and remove all participants
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the "Location" Participants from the Created Appointment
		And I remove the "Patient" Participants from the Created Appointment
		And I remove the "Practitioner" Participants from the Created Appointment
	When I make the "AppointmentCreate" request
	Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book appointment and set an incorrect appointment id
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Created Appointment Id to "ZZ"
	When I make the "AppointmentCreate" request
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Book appointment for patient and send extra fields in the resource
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request with Invalid Additional Field in the Resource
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Book appointment with invalid slot reference
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Created Appointment Slot Reference to "<slotReference>"
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
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the Response Resource should be an Appointment
		And the Appointment Location Participant should be valid and resolvable
	
Scenario: Book appointment with missing start element in appointment resource
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the Start from the Created Appointment
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book appointment with missing end element in appointment resource
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the End from the Created Appointment
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book appointment with missing status element in appointment resource
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the Status from the Created Appointment
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book appointment with missing slot element in appointment resource
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the Slot from the Created Appointment
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book Appointment and remove identifier value from the appointment booking
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Created Appointment Identifier Value to null
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario Outline: Book Appointment and remove reason coding element from the appointment booking
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Created Appointment Reason Coding <CodingElement> to null
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"
	Examples:
		| CodingElement |
		| System        |
		| Code          |
		| Display       |

#Miss-leading test name - participant status is not removed but nullified
Scenario: Book Appointment and remove participant status from the appointment booking
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Created Appointment Patient Participant Status to null
		And I set the Created Appointment Practitioner Participant Status to null
		And I set the Created Appointment Location Participant Status to null
	When I make the "AppointmentCreate" request
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book appointment and send an invalid bundle resource
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
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
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Created Appointment to a new Appointment
		When I make the "AppointmentAmend" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: CapabilityStatement profile supports the book appointment operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement REST Resources should contain the "Appointment" Resource with the "Update" Interaction		

Scenario: Book appointment valid response check caching headers exist
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the Response Resource should be an Appointment
		And the required cacheing headers should be present in the response

Scenario: Book appointment invalid response check caching headers exist
Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the Slot from the Created Appointment
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"
		And the required cacheing headers should be present in the response

Scenario: Book appointment with name removed from booking organization
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the booking organization name element
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"


Scenario: Book appointment with telecom removed from booking organization
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the booking organization telecom element
	When I make the "AppointmentCreate" request
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