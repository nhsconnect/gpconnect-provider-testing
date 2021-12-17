@appointment @1.2.7-Full-Pack
Feature: AppointmentCancel

@1.2.3
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
		And the Appointment Status should be Cancelled
		And the Appointment Id should equal the Created Appointment Id
		And the Appointment Extensions should equal the Created Appointment Extensions
		And the Appointment Description should equal the Created Appointment Description
		And the Appointment Start and End Dates should equal the Created Appointment Start and End Dates
		And the Appointment Slots should equal the Created Appointment Slots
		And the Appointment Participants should be equal to the Created Appointment Participants
		And the Appointment Created should be equal to the Created Appointment Created
		And the Appointments returned must be in the future
		And the appointment reason must not be included
		And the Appointment DeliveryChannel must be valid
		And the Appointment PractitionerRole must be valid
# git hub ref 120
# RMB 25/10/2018		
		And the Appointment Not In Use should be valid
	Examples:
		| PatientName |
		| patient1    |
		| patient2    |
		| patient3    |

@1.2.3
Scenario Outline: I perform a successful cancel appointment with Extensions
	Given I create an Appointment for Patient "<PatientName>" 
		And I create an Appointment with org type "<OrgType>" with channel "<DeliveryChannel>" with prac role "<PracRole>"	
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointments returned must be in the future
		And the Appointment Id should be valid
		And the Appointment Metadata should be valid
		And the Appointment DeliveryChannel must be present
		And the Appointment PractitionerRole must be present
	Examples:
		| PatientName | OrgType | DeliveryChannel | PracRole |
		| patient1    | true    | true            | true     |

@1.2.3
Scenario: I perform a successful cancel appointment and amend the comment
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the Created Appointment Comment to "RANDOM COMMENT"
	When I make the "AppointmentCancel" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

@1.2.3
Scenario: I perform cancel appointment and update the description
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the Created Appointment Description to "RANDOM DESCRIPTION"
	When I make the "AppointmentCancel" request
	Then  the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"	 
		And the response status code should be "422"

Scenario: I perform cancel appointment and add participants
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I add a Participant with Reference "location/2" to the Created Appointment
		And I add a Participant with Reference "Patient/2" to the Created Appointment
		And I add a Participant with Reference "Practitioner/2" to the Created Appointment		
	When I make the "AppointmentCancel" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: I perform cancel appointment and update the type text
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the Created Appointment Type Text to "RANDOM TYPE TEXT"
	When I make the "AppointmentCancel" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

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
		| reason                     |
		| Test cancellation reason 1 |
		| test reason for cancel 2   |

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

# git hub ref 182
# RMB 14/2/19
Scenario: Amend a cancelled appointment 
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
	When I make the "AppointmentCancel" request
	Given I configure the default "AppointmentAmend" request
	When I make the "AppointmentAmend" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

# git hub ref 200 (demonstrator)
# RMB 20/2/19
Scenario: I cancel appointment for patient with absoulute reference
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I amend the cancel organization reference to absolute reference
	When I make the "AppointmentCancel" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: I perform cancel appointment with participants with absoulte references
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I add a Participant with Reference "https://test1.supplier.thirdparty.nhs.uk/A11111/STU3/1/GPConnect/Location/2" to the Created Appointment
		And I add a Participant with Reference "https://test1.supplier.thirdparty.nhs.uk/A11111/STU3/1/GPConnect/Practitioner/2" to the Created Appointment
		And I add a Participant with Reference "https://test1.supplier.thirdparty.nhs.uk/A11111/STU3/1/GPConnect/Patient/2" to the Created Appointment
	When I make the "AppointmentCancel" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

@1.2.7-IncrementalAndRegression
Scenario: Successfully Cancel an appointment that has serviceCategory populated
	Given I create an Appointment in "2" days time for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "Test-Cancel-Reason"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointment Status should be Cancelled
		And the Appointment Cancellation Reason Extension should be valid for "Test-Cancel-Reason"
		And the Appointment Metadata should be valid
		And the Appointment Status should be Cancelled
		And the Appointment Id should equal the Created Appointment Id
		And the Appointment Extensions should equal the Created Appointment Extensions
		And the Appointment Description should equal the Created Appointment Description
		And the Appointment Start and End Dates should equal the Created Appointment Start and End Dates
		And the Appointment Slots should equal the Created Appointment Slots
		And the Appointment Participants should be equal to the Created Appointment Participants
		And the Appointment Created should be equal to the Created Appointment Created
		And the Appointments returned must be in the future
		And the appointment reason must not be included
		And the Appointment DeliveryChannel must be valid
		And the Appointment PractitionerRole must be valid
		And the Appointment Not In Use should be valid
		And One Appointment contains serviceCategory element

@1.2.7-IncrementalAndRegression
Scenario: Successfully Cancel an appointment that has serviceType populated
	Given I create an Appointment in "2" days time for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "Test-Cancel-Reason"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointment Status should be Cancelled
		And the Appointment Cancellation Reason Extension should be valid for "Test-Cancel-Reason"
		And the Appointment Metadata should be valid
		And the Appointment Status should be Cancelled
		And the Appointment Id should equal the Created Appointment Id
		And the Appointment Extensions should equal the Created Appointment Extensions
		And the Appointment Description should equal the Created Appointment Description
		And the Appointment Start and End Dates should equal the Created Appointment Start and End Dates
		And the Appointment Slots should equal the Created Appointment Slots
		And the Appointment Participants should be equal to the Created Appointment Participants
		And the Appointment Created should be equal to the Created Appointment Created
		And the Appointments returned must be in the future
		And the appointment reason must not be included
		And the Appointment DeliveryChannel must be valid
		And the Appointment PractitionerRole must be valid
		And the Appointment Not In Use should be valid
		And One Appointment contains serviceType element

@1.2.7-IncrementalAndRegression
Scenario: Successfully Cancel an appointment ensuring backwards compatibility with consumers that do not send ServiceCategory And serviceType expect success
	Given I create an Appointment in "2" days time for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "Test-Cancel-Reason"
		And I Remove the serviceCategory and the serviceType from the appointment
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointment Status should be Cancelled
		And the Appointment Cancellation Reason Extension should be valid for "Test-Cancel-Reason"
		And the Appointment Metadata should be valid
		And the Appointment Status should be Cancelled
		And the Appointment Id should equal the Created Appointment Id
		And the Appointment Extensions should equal the Created Appointment Extensions
		And the Appointment Description should equal the Created Appointment Description
		And the Appointment Start and End Dates should equal the Created Appointment Start and End Dates
		And the Appointment Slots should equal the Created Appointment Slots
		And the Appointment Participants should be equal to the Created Appointment Participants
		And the Appointment Created should be equal to the Created Appointment Created
		And the Appointments returned must be in the future
		And the appointment reason must not be included
		And the Appointment DeliveryChannel must be valid
		And the Appointment PractitionerRole must be valid
		And the Appointment Not In Use should be valid
		And One Appointment contains serviceType element


@1.2.8-Only
Scenario Outline: Cancel appointment for patient where a HealthcareService was associated
	Given I set the Get Request Id to the Logical Identifier for Read Healthcare Service "<HealthCareService>"
		And I configure the default "HealthcareRead" request
		When I make the "HealthcareRead" request
		Then the response status code should indicate success
		And the Response Resource should be a Healthcare Service
		And the Healthcare Id should match the GET request Id
		And the Healthcare service should be valid
		And I Store the DOS id from the Healthcare service returned
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots With a DOS Id in request
		Then I Check that atleast One Slot is returned
	Given I store the Free Slots Bundle
		Then the Bundle Meta should be contain service filtering status set to "enabled"
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And the Response Resource should be an Appointment
	Given I store the Created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the Created Appointment to Cancelled with Reason "double booked"
		When I make the "AppointmentCancel" request
		Then the response status code should indicate success
		And the Appointment Participant Type and Actor should be valid
		And the Appointment Participant Actor should contains a HealthcareService Reference
	Examples:
		| HealthCareService |
		| HEALTHCARE2       |



