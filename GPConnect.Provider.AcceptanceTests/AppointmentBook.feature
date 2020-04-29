@appointment @1.2.7-Full-Pack
Feature: AppointmentBook

@1.2.3
Scenario: Book single appointment for patient
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the Response Resource should be an Appointment

Scenario Outline: Book single appointment for patient with optional elements
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment with org type "<OrgType>" with channel "<DeliveryChannel>" with prac role "<PracRole>"
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the Response Resource should be an Appointment
		And the Appointment DeliveryChannel must be valid
		And the Appointment PractitionerRole must be valid
# git hub ref 120
# RMB 25/10/2018		
		And the Appointment Not In Use should be valid
	Examples:
		| OrgType | DeliveryChannel | PracRole |
		| false   | false           | false    |
		| false   | false           | true     |
		| false   | true            | false    |
		| false   | true            | true     |
		| true    | false           | false    |
		| true    | false           | true     |
		| true    | true            | false    |
		| true    | true            | true     |

@1.2.3
Scenario Outline: Book single appointment for patient with Extensions
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment with org type "<OrgType>" with channel "<DeliveryChannel>" with prac role "<PracRole>"
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the Response Resource should be an Appointment
		And the Appointment DeliveryChannel must be present
		And the Appointment PractitionerRole must be present
	Examples:
		| OrgType | DeliveryChannel | PracRole |
		| true    | true            | true     |

@1.2.3
Scenario: Book single appointment for patient without organisation type
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots without organisation type
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment without organisationType from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the Response Resource should be an Appointment
#
# Provider systems unable to keep state between interactions so test removed
# RMB 16/10/2018
#
#Scenario: Search without organisation type and book with organisation type 
#	Given I get an existing patients nshNumber
#		And I store the Patient
#	Given I get Available Free Slots without organisation type
#		And I store the Free Slots Bundle
#	Given I configure the default "AppointmentCreate" request
#		And I create an Appointment from the stored Patient and stored Schedule
#	When I make the "AppointmentCreate" request
#	Then the response status code should indicate failure
#		And the response status code should be "422"
#		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"
		
Scenario Outline: Book Appointment with invalid url for booking appointment
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the request URL to "<url>"
	When I make the "AppointmentCreate" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource
	Examples:
		| url             |
		| appointmentqq/! |

Scenario Outline: Book appointment accept header and _format parameter to request response format
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be an Appointment
		And the Appointment Metadata should be valid
		And the Appointment Id should be valid
		And the Appointment Status should be valid
		And the Appointment Start should be valid
		And the Appointment End should be valid
		And the Appointment Participants should be valid and resolvable
		And the Appointment Slots should be valid
		And the Appointment Description must be valid
		And the booking organization extension must be valid
		And the Appointment Created must be valid
		And the appointment reason must not be included
		And the Appointment DeliveryChannel must be valid
		And the Appointment PractitionerRole must be valid
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/fhir+json | application/fhir+json | JSON       |
		| application/fhir+json | application/fhir+xml  | XML        |
		| application/fhir+xml  | application/fhir+json | JSON       |
		| application/fhir+xml  | application/fhir+xml  | XML        |

Scenario Outline: Book appointment _format parameter only but varying request content types
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
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
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
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
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Prefer header to "return=representation"
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the Response Resource should be an Appointment
		And the content-type should not be equal to null
		And the content-length should not be equal to zero

Scenario: Book appointment prefer header set to minimal
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Prefer header to "return=minimal"
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be empty

	@1.2.7-IncrementalAndRegression
Scenario: Book Appointment and appointment participant is valid
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the Response Resource should be an Appointment
		And the Appointment Participants should be valid and resolvable
		And the Appointment Participant Type and Actor should be valid

Scenario: Book Appointment without practitioner participant
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the "Practitioner" Participants from the Created Appointment
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the Response Resource should be an Appointment

Scenario Outline: Book Appointment and remove manadatory resources from the appointment booking
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
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
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the "Location" Participants from the Created Appointment
		And I remove the "Patient" Participants from the Created Appointment
		And I remove the "Practitioner" Participants from the Created Appointment
	When I make the "AppointmentCreate" request
	Then the response status code should indicate failure
		And the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book appointment for patient and send extra fields in the resource
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request with Invalid Additional Field in the Resource
	Then the response status code should be "422"
	And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario Outline: Book appointment with invalid slot reference
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Created Appointment Slot Reference to "<slotReference>"
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"
	Examples:
		| slotReference                                                               |
		| Slot/45555g55555                                                            |
		| Slot/45555555##                                                             |
		| Slot/hello                                                                  |
		| https://test1.supplier.thirdparty.nhs.uk/A11111/STU3/1/GPConnect/Slot/#12345 |


Scenario: Book single appointment for patient and check the location reference is valid
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the response body should be FHIR JSON
		And the Response Resource should be an Appointment
		And the Appointment Location Participant should be valid and resolvable
	
Scenario: Book appointment with missing start element in appointment resource
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the Start from the Created Appointment
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book appointment with missing end element in appointment resource
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the End from the Created Appointment
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book appointment with missing status element in appointment resource
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the Status from the Created Appointment
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book appointment with missing slot element in appointment resource
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the Slot from the Created Appointment
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book Appointment and remove identifier value from the appointment booking
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Created Appointment Identifier Value to null
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book Appointment and remove participant status from the appointment booking
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Created Appointment Patient Participant Status to null
		And I set the Created Appointment Practitioner Participant Status to null
		And I set the Created Appointment Location Participant Status to null
	When I make the "AppointmentCreate" request
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book appointment and send an invalid bundle resource
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request with invalid Resource type
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book appointment and send an invalid appointment resource
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
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
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the Response Resource should be an Appointment
		And the required cacheing headers should be present in the response

Scenario: Book appointment invalid response check caching headers exist
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the Slot from the Created Appointment
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"
		And the required cacheing headers should be present in the response

@1.2.3
Scenario: Book appointment with name removed from booking organization
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the booking organization name element
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Book appointment with telecom removed from booking organization
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I remove the booking organization telecom element
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

@1.2.3
Scenario: Book appointment with a description
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Created Appointment Description to "customDescription"
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the Response Resource should be an Appointment
		And the Appointment Description should be valid for "customDescription"
		And the Appointment Comment should be null

Scenario: Book appointment re-using an already booked slot
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the Response Resource should be an Appointment
	Given I get the Patient for Patient Value "patient2"
		And I store the Patient
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request
	Then the response status code should be "409"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "DUPLICATE_REJECTED"

Scenario: Book appointment and send a reason in the appointment
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I add an Appointment Reason to the appointment
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

# git hub ref 141
# 6/12/2018 RMB
Scenario: Book appointment with typeCode in appointment resource
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I add a typeCode to the Created Appointment
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

# git hub ref 155
# RMB 8/1/19
Scenario: Book appointment with a description and comment
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Created Appointment Description to "customDescription"
		And I set the Created Appointment Comment
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the Response Resource should be an Appointment
		And the Appointment Description should be valid for "customDescription"
		And the Appointment Comment should be valid for "CustomComment"

# git hub ref 190 (demonstrator)
# RMB 6/2/19
Scenario: Book appointment with a comment
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Created Appointment Comment
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the Response Resource should be an Appointment
		And the Appointment Comment should be valid for "CustomComment"

# git hub ref 190 (demonstrator)
# RMB 6/2/19
Scenario: Book appointment without a comment
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the Response Resource should be an Appointment
		And the Appointment Comment should be null

# git hub ref 200 (demonstrator)
# RMB 20/2/19
	Scenario: Book single appointment for patient with invalid reference
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I amend the Organization reference to absolute reference
	When I make the "AppointmentCreate" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

@1.2.7-IncrementalAndRegression
Scenario: Book single appointment for Patient expecting servicecategory is populated
	Given I create an Appointment in "2" days time for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Then the response status code should indicate created
		And the Response Resource should be an Appointment
		And the Appointment Metadata should be valid
		And the Appointment Id should be valid
		And the Appointment Status should be valid
		And the Appointment Start should be valid
		And the Appointment End should be valid
		And the Appointment Participants should be valid and resolvable
		And the Appointment Slots should be valid
		And the Appointment Description must be valid
		And the booking organization extension must be valid
		And the Appointment Created must be valid
		And the appointment reason must not be included
		And the Appointment DeliveryChannel must be valid
		And the Appointment PractitionerRole must be valid
		And One Appointment contains serviceCategory element

@1.2.7-IncrementalAndRegression
Scenario: Book single appointment for Patient expecting serviceType is populated
	Given I create an Appointment in "2" days time for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Then the response status code should indicate created
		And the Response Resource should be an Appointment
		And the Appointment Metadata should be valid
		And the Appointment Id should be valid
		And the Appointment Status should be valid
		And the Appointment Start should be valid
		And the Appointment End should be valid
		And the Appointment Participants should be valid and resolvable
		And the Appointment Slots should be valid
		And the Appointment Description must be valid
		And the booking organization extension must be valid
		And the Appointment Created must be valid
		And the appointment reason must not be included
		And the Appointment DeliveryChannel must be valid
		And the Appointment PractitionerRole must be valid
		And One Appointment contains serviceType element

@1.2.7-IncrementalAndRegression
Scenario: Book single appointment expecting servicecategory in response when ServiceCategory and serviceType are in request but ignored
	Given I create an Appointment in "2" days time for Patient "patient1" and Organization Code "ORG1" With serviceCategory and serviceType in Request
		And I store the Created Appointment
	Then the response status code should indicate created
		And the Response Resource should be an Appointment
		And the Appointment Metadata should be valid
		And the Appointment Id should be valid
		And the Appointment Status should be valid
		And the Appointment Start should be valid
		And the Appointment End should be valid
		And the Appointment Participants should be valid and resolvable
		And the Appointment Slots should be valid
		And the Appointment Description must be valid
		And the booking organization extension must be valid
		And the Appointment Created must be valid
		And the appointment reason must not be included
		And the Appointment DeliveryChannel must be valid
		And the Appointment PractitionerRole must be valid
		And One Appointment contains serviceCategory element

@1.2.7-IncrementalAndRegression
Scenario: Book single appointment expecting serviceType in response when ServiceCategory and serviceType are in request but ignored
	Given I create an Appointment in "2" days time for Patient "patient1" and Organization Code "ORG1" With serviceCategory and serviceType in Request
		And I store the Created Appointment
	Then the response status code should indicate created
		And the Response Resource should be an Appointment
		And the Appointment Metadata should be valid
		And the Appointment Id should be valid
		And the Appointment Status should be valid
		And the Appointment Start should be valid
		And the Appointment End should be valid
		And the Appointment Participants should be valid and resolvable
		And the Appointment Slots should be valid
		And the Appointment Description must be valid
		And the booking organization extension must be valid
		And the Appointment Created must be valid
		And the appointment reason must not be included
		And the Appointment DeliveryChannel must be valid
		And the Appointment PractitionerRole must be valid
		And One Appointment contains serviceType element