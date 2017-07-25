@appointment
Feature: AppointmentCancel

Scenario Outline: I perform a successful cancel appointment
	Given I create an Appointment for Patient "<PatientName>" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment to Cancelled with Reason "double booked"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the response should be an Appointment resource
		And the Appointment Status should be Cancelled
		And the Appointment Cancellation Reason Extension should be valid for "double booked"
		And the Appointment Metadata should be valid
	Examples:
		| PatientName |
		| patient1    |
		| patient2    |
		| patient3    |
		| patient8    |
		| patient9    |

#Potentially split this up, only one of the steps could fair the test and the rest could result in a pass ?? May be false postive
Scenario Outline: I perform cancel appointment and update invalid elements
		Given I create an Appointment for Patient "<PatientName>" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I add a Category Extension with Code "CLI" and Display "Clinical" to the Created Appointment
		And I add a Booking Method Extension with Code "ONL" and Display "Online" to the Created Appointment
		And I add a Contact Method Extension with Code "ONL" and Display "Online" to the Created Appointment
		And I add a Participant with Reference "location/2" to the Created Appointment
		And I add a Participant with Reference "Patient/2" to the Created Appointment
		And I add a Participant with Reference "Practitioner/2" to the Created Appointment		
		And I add an Appointment Identifier with default System and Value "898976578" to the Created Appointment
		And I add an Appointment Identifier with default System and Value "898976578" to the Created Appointment
		And I set the Created Appointment Description to "RANDOM TEXT" 
		And I set the Created Appointment Priority to "3"
		And I set the Created Appointment Minutes Duration to "20"
		And I set the Created Appointment Comment to "RANDOM COMMENT"
		And I set the Created Appointment Type Text to "RANDOM TYPE TEXT"
	When I make the "AppointmentCancel" request
	Then the response status code should be "403"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| PatientName |
		| patient1    |
		| patient2    |
		| patient3    |
		| patient10   |
		| patient11   |
		| patient12   |

Scenario Outline: Cancel appointment making a request to an invalid URL
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the request URL to "<url>"
	When I make the "AppointmentCancel" request
	Then the response status code should be "404"
	Examples:
		| url              |
		| appointmentqq/!  |
		| Appointments/#   |

Scenario Outline: Cancel appointment failure due to missing header
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I do not send header "<Header>"
	When I make the "AppointmentCancel" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Header            |
		| Ssp-TraceID       |
		| Ssp-From          |
		| Ssp-To            |
		| Ssp-InteractionId |
		| Authorization     |

Scenario Outline: Cancel appointment failure with incorrect interaction id
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the Interaction Id header to "<InteractionId>`"
	When I make the "AppointmentCancel" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| InteractionId                                                     |
		| urn:nhs:names:services:gpconnect:fhir:rest:update:appointmentss   |
		| urn:nhs:names:services:gpconnect:fhir:rest:update:appointmenT     |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		|                                                                   |
		| null                                                              |

Scenario Outline: Cancel appointment using the _format parameter to request response format
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I add a Format parameter with the Value "<Parameter>"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the Appointment Status should be Cancelled
		And the Appointment Cancellation Reason Extension should be valid for "double booked"
		And the Appointment Metadata should be valid
	Examples:
		| Parameter             | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Cancel appointment using the accept header to request response format
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the Accept header to "<Header>"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the Appointment Status should be Cancelled
		And the Appointment Cancellation Reason Extension should be valid for "double booked"
		And the Appointment Metadata should be valid
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Cancel appointment using the accept header and _format parameter to request response format
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the Accept header to "<Header>"
		And I add a Format parameter with the Value "<Parameter>"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the Appointment Status should be Cancelled
		And the Appointment Cancellation Reason Extension should be valid for "double booked"
		And the Appointment Metadata should be valid
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/json+fhir | application/json+fhir | JSON       |
		| application/json+fhir | application/xml+fhir  | XML        |
		| application/xml+fhir  | application/json+fhir | JSON       |
		| application/xml+fhir  | application/xml+fhir  | XML        |
		
Scenario Outline: Cancel appointment using the accept header and _format parameter and content-type to request response format
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the request content type to "<ContentType>"
		And I set the Accept header to "<AcceptHeader>"
		And I add a Format parameter with the Value "<FormatParam>"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the response body should be FHIR <Format>
		And the response should be an Appointment resource
		And the Appointment Status should be Cancelled
		And the Appointment Cancellation Reason Extension should be valid for "double booked"
		And the Appointment Metadata should be valid
	Examples:
		| ContentType           | AcceptHeader          | FormatParam           | Format |
		| application/xml+fhir  | application/xml+fhir  | application/xml+fhir  | XML    |
		| application/json+fhir | application/json+fhir | application/json+fhir | JSON   |
		| application/xml+fhir  | application/xml+fhir  | application/json+fhir | JSON   |
		| application/json+fhir | application/json+fhir | application/xml+fhir  | XML    |
		| application/xml+fhir  | application/json+fhir | application/json+fhir | JSON   |
		| application/json+fhir | application/xml+fhir  | application/xml+fhir  | XML    |
		| application/xml+fhir  | application/json+fhir | application/xml+fhir  | XML    |
		| application/json+fhir | application/xml+fhir  | application/json+fhir | JSON   |

Scenario Outline: Cancel appointment check cancellation reason is equal to the request cancellation reason
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment to Cancelled with Url "<url>" and Reason "<reason>"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the response should be an Appointment resource
		And the Appointment Status should be Cancelled
		And the Appointment Cancellation Reason Extension should be valid for "<reason>"
		And the Appointment Metadata should be valid
	Examples:
		| url                                                                                           | reason      |
		| http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1 | Too busy    |
		| http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1 | Car crashed |
		| http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1 | Too tired   |

Scenario Outline: Cancel appointment invalid cancellation extension url
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment to Cancelled with Url "<url>" and Reason "<reason>"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"
	Examples:
		| url                                                                                                | reason   |
		|                                                                                                    | Too busy |
		| http://fhir.nhs.net/StructureDefinition/extension-gpINCORRECTect-appointment-cancellation-reason-1 | Too busy |
		| http://fhir.nhs.net/StructuraeDefinition/extension-gpconnect-appointment-cancellation-reason-1-010 | Too busy |

Scenario: Cancel appointment missing cancellation extension reason
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment to Cancelled with Reason ""
	When I make the "AppointmentCancel" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Cancel appointment verify resource is updated when an valid ETag value is provided
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the If-Match header to the Stored Appointment Version Id
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the response should be an Appointment resource
		And the Appointment Status should be Cancelled
		And the Appointment Cancellation Reason Extension should be valid for "double booked"	
		
Scenario: Cancel appointment verify resource is not updated when an out of date ETag value is provided
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set If-Match request header to "invalidEtag"
	When I make the "AppointmentCancel" request	
	Then the response status code should be "409"
		
Scenario: Cancel appointment compare request appointment to returned appointment
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment to Cancelled with Reason "double booked"
	When I make the "AppointmentCancel" request	
	Then the response status code should indicate success
		And the response should be an Appointment resource
		And the Appointment Status should be Cancelled
		And the Appointment Id should equal the Created Appointment Id
		And the Appointment Status should equal the Created Appointment Status
		And the Appointment Extensions should equal the Created Appointment Extensions
		And the Appointment Description should equal the Created Appointment Description
		And the Appointment Start and End Dates should equal the Created Appointment Start and End Dates
		And the Appointment Slots should equal the Created Appointment Slots
		And the Appointment Reason should equal the Created Appointment Reason
		And the Appointment Participants should be equal to the Created Appointment Participants		

Scenario: Cancel appointment response body must contain valid slot reference
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment to Cancelled with Reason "double booked"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the response should be an Appointment resource
		And the Appointment Cancellation Reason Extension should be valid for "double booked"
		And the Appointment Status should be Cancelled
		And the Appointment Slots should be valid
		And the Appointment Participants should be valid and resolvable

Scenario Outline: Cancel appointment prefer header set to representation
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the Prefer header to "return=representation"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the response should be an Appointment resource
		And the Appointment Status should be Cancelled
		And the content-type should not be equal to null
		And the content-length should not be equal to zero
		And the returned resource shall contains a logical id
		And the Appointment Start should be valid
		And the Appointment End should be valid
		And the Appointment Slots should be valid
		And the Appointment Participants should be valid and resolvable
		And the Appointment Reason should be valid
		And the Appointment Metadata should be valid
	Examples:
		| PatientName |
		| patient1    |
		| patient2    |
		| patient3    |
		| patient8    |
		| patient9    |

Scenario: Cancel appointment prefer header set to minimal
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment to Cancelled with Reason "double booked"
		And I set the Prefer header to "return=minimal"
	When I make the "AppointmentCancel" request	
	Then the response status code should indicate success
		And the response body should be empty
		And the content-type should be equal to null

Scenario Outline: Cancel appointment check the version id of the cancelled resource is different
		Given I create an Appointment for Patient "<PatientName>" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Created Appointment to Cancelled with Reason "double booked"
	When I make the "AppointmentCancel" request	
	Then the response status code should indicate success
		And the response should be an Appointment resource
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

Scenario: Conformance profile supports the cancel appointment operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the conformance profile should contain the "Appointment" resource with a "update" interaction
