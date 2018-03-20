@appointment
Feature: AppointmentCancel

Scenario Outline: I perform a successful cancel appointment
	Given I create an Appointment for Patient "<PatientName>" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointment Status should be Cancelled
		And the Appointment Cancellation Reason Extension should be valid for "double booked"
		And the Appointment Metadata should be valid
		And the Appointment Description must be valid
	Examples:
		| PatientName |
		| patient1    |
		| patient2    |
		| patient3    |
		| patient8    |
		| patient9    |

Scenario Outline: I perform a successful cancel appointment and all returned appointments must be in the future	
	Given I create an Appointment for Patient "<PatientName>" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointment Status should be Cancelled
		And the Appointments returned must be in the future
	Examples:
		| PatientName |
		| patient1    |
		| patient2    |
		| patient3    |
		| patient8    |
		| patient9    |

Scenario: I perform a successful cancel appointment and amend the comment
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the Created Appointment Comment to "RANDOM COMMENT"
	When I make the "AppointmentCancel" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: I perform cancel appointment and update the description
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the Created Appointment Description to "RANDOM DESCRIPTION"
	When I make the "AppointmentCancel" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"	

Scenario: I perform cancel appointment and update the reason
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the Created Appointment Reason to "RANDOM REASON"
	When I make the "AppointmentCancel" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: I perform cancel appointment and add participants
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I add a Participant with Reference "location/2" to the Created Appointment
		And I add a Participant with Reference "Patient/2" to the Created Appointment
		And I add a Participant with Reference "Practitioner/2" to the Created Appointment		
	When I make the "AppointmentCancel" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: I perform cancel appointment and update the priority
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the Created Appointment Priority to "3"
	When I make the "AppointmentCancel" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: I perform cancel appointment and update the minutes duration
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the Created Appointment Minutes Duration to "20"
	When I make the "AppointmentCancel" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"


Scenario: I perform cancel appointment and update the type text
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the Created Appointment Type Text to "RANDOM TYPE TEXT"
	When I make the "AppointmentCancel" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Cancel appointment making a request to an invalid URL
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the request URL to "<url>"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource
	Examples:
		| url              |
		| appointmentqq/!  |
		| Appointments/#   |

Scenario Outline: Cancel appointment using the _format parameter to request response format
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I add a Format parameter with the Value "<Parameter>"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be an Appointment
		And the Appointment Status should be Cancelled
		And the Appointment Cancellation Reason Extension should be valid for "double booked"
		And the Appointment Metadata should be valid
	Examples:
		| Parameter             | BodyFormat |
		| application/fhir+json | JSON       |
		| application/fhir+xml  | XML        |

Scenario Outline: Cancel appointment using the accept header to request response format
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the Accept header to "<Header>"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be an Appointment
		And the Appointment Status should be Cancelled
		And the Appointment Cancellation Reason Extension should be valid for "double booked"
		And the Appointment Metadata should be valid
	Examples:
		| Header                | BodyFormat |
		| application/fhir+json | JSON       |
		| application/fhir+xml  | XML        |

Scenario Outline: Cancel appointment using the accept header and _format parameter to request response format
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the Accept header to "<Header>"
		And I add a Format parameter with the Value "<Parameter>"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be an Appointment
		And the Appointment Status should be Cancelled
		And the Appointment Cancellation Reason Extension should be valid for "double booked"
		And the Appointment Metadata should be valid
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/fhir+json | application/fhir+json | JSON       |
		| application/fhir+json | application/fhir+xml  | XML        |
		| application/fhir+xml  | application/fhir+json | JSON       |
		| application/fhir+xml  | application/fhir+xml  | XML        |
		
Scenario Outline: Cancel appointment using the accept header and _format parameter and content-type to request response format
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the request content type to "<ContentType>"
		And I set the Accept header to "<AcceptHeader>"
		And I add a Format parameter with the Value "<FormatParam>"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the response body should be FHIR <Format>
		And the Response Resource should be an Appointment
		And the Appointment Status should be Cancelled
		And the Appointment Cancellation Reason Extension should be valid for "double booked"
		And the Appointment Metadata should be valid
	Examples:
		| ContentType           | AcceptHeader          | FormatParam           | Format |
		| application/fhir+xml  | application/fhir+xml  | application/fhir+xml  | XML    |
		| application/fhir+json | application/fhir+json | application/fhir+json | JSON   |
		| application/fhir+xml  | application/fhir+xml  | application/fhir+json | JSON   |
		| application/fhir+json | application/fhir+json | application/fhir+xml  | XML    |
		| application/fhir+xml  | application/fhir+json | application/fhir+json | JSON   |
		| application/fhir+json | application/fhir+xml  | application/fhir+xml  | XML    |
		| application/fhir+xml  | application/fhir+json | application/fhir+xml  | XML    |
		| application/fhir+json | application/fhir+xml  | application/fhir+json | JSON   |

Scenario Outline: Cancel appointment check cancellation reason is equal to the request cancellation reason
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "<reason>"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointment Status should be Cancelled
		And the Appointment Cancellation Reason Extension should be valid for "<reason>"
		And the Appointment Metadata should be valid
	Examples:
		| reason      |
		| Too busy    |
		| Car crashed |
		| Too tired   |

Scenario Outline: Cancel appointment invalid cancellation extension url
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Url "<url>" and Reason "<reason>"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate failure
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"
	Examples:
		| url                                                                                               | reason   |
		|                                                                                                   | Too busy |
		| http://fhir.nhs.uk/StructureDefinition/extension-gpINCORRECTect-appointment-cancellation-reason-1 | Too busy |
		| http://fhir.nhs.uk/StructuraeDefinition/extension-gpconnect-appointment-cancellation-reason-1-010 | Too busy |

Scenario: Cancel appointment missing cancellation extension reason
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason ""
	When I make the "AppointmentCancel" request
	Then the response status code should indicate failure
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Cancel appointment verify resource is updated when an valid ETag value is provided
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the If-Match header to the Stored Appointment Version Id
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointment Status should be Cancelled
		And the Appointment Cancellation Reason Extension should be valid for "double booked"	
		
Scenario: Cancel appointment verify resource is not updated when an out of date ETag value is provided
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the If-Match header to "invalidEtag"
	When I make the "AppointmentCancel" request	
	Then the response status code should be "409"
		And the response should be a OperationOutcome resource with error code "FHIR_CONSTRAINT_VIOLATION"

Scenario: Cancel appointment compare request appointment to returned appointment
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
	When I make the "AppointmentCancel" request	
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointment Status should be Cancelled
		And the Appointment Id should equal the Created Appointment Id
		And the Appointment Status should equal the Created Appointment Status
		And the Appointment Extensions should equal the Created Appointment Extensions
		And the Appointment Description should equal the Created Appointment Description
		And the Appointment Start and End Dates should equal the Created Appointment Start and End Dates
		And the Appointment Slots should equal the Created Appointment Slots
		And the Appointment Participants should be equal to the Created Appointment Participants
		And the Appointment Created should be equal to the Created Appointment Created

Scenario: Cancel appointment response body must contain valid slot reference
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointment Cancellation Reason Extension should be valid for "double booked"
		And the Appointment Status should be Cancelled
		And the Appointment Slots should be valid
		And the Appointment Participants should be valid and resolvable

Scenario: Cancel appointment prefer header set to representation
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the Prefer header to "return=representation"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointment Status should be Cancelled
		And the content-type should not be equal to null
		And the content-length should not be equal to zero
		And the Appointment Id should be valid
		And the Appointment Start should be valid
		And the Appointment End should be valid
		And the Appointment Slots should be valid
		And the Appointment Participants should be valid and resolvable
		And the Appointment Metadata should be valid

Scenario: Cancel appointment prefer header set to minimal
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the Prefer header to "return=minimal"
	When I make the "AppointmentCancel" request	
	Then the response status code should indicate success
		And the response body should be empty

Scenario Outline: Cancel appointment check the version id of the cancelled resource is different
		Given I create an Appointment for Patient "<PatientName>" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
	When I make the "AppointmentCancel" request	
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointment Status should be Cancelled
		And the Appointment Cancellation Reason Extension should be valid for "double booked"
		And the Appointment Version Id should not equal the Created Appointment Version Id
	Examples:
		| PatientName |
		| patient1    |
		| patient2    |
		| patient3    |
		| patient8    |
		| patient10   |
		| patient12   |

Scenario: CapabilityStatement profile supports the cancel appointment operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement REST Resources should contain the "Appointment" Resource with the "Update" Interaction		

Scenario: Cancel appointment valid response check caching headers exist
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointment Status should be Cancelled
		And the Appointment Cancellation Reason Extension should be valid for "double booked"
		And the Appointment Metadata should be valid
		And the required cacheing headers should be present in the response

Scenario:Cancel appointment invalid response check caching headers exist
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason ""
	When I make the "AppointmentCancel" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
		And the required cacheing headers should be present in the response
