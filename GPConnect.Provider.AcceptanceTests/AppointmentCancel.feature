@appointment
Feature: AppointmentCancel

Scenario Outline: I perform a successful cancel appointment
	Given I create an Appointment for Patient "<PatientName>" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the created Appointment to Cancelled with Reason "double booked"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the cancellation reason in the returned appointment response should be equal to "double booked"
		And the returned appointment resource should contain meta data profile and version id
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
		And I set the created Appointment to Cancelled with Reason "double booked"
		And I add an extension to created appointment with url "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1" code "CLI" and display "Clinical"
		And I add an extension to created appointment with url "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1" code "ONL" and display "Online"
		And I add an extension to created appointment with url "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1" code "ONL" and display "Online"
		And I set the description to "RANDOM TEXT" for created appointment
		And I set the priority to "3" for created appointment
		And I set the minutes to "20" for created appointment
		And I set the comment to "RANDOM COMMENT" for created appointment
		And I set the type text to "RANDOM TYPE TEXT" for created appointment
		And I set the identifier with system "http://fhir.nhs.net/Id/gpconnect-appointment-identifier" and value "898976578" for the created appointment
		And I set the identifier with system "http://fhir.nhs.net/Id/gpconnect-appointment-identifier" and value "898955578" for the created appointment
		And I add participant "location" with reference "lacation/2" for the created appointment
		And I add participant "patient" with reference "patient/2" for the created appointment
		And I add participant "practitioner" with reference "practitioner/2" for the created appointment
	When I make the "AppointmentCancel" request
	Then the response status code should be "403"
		And the response body should be FHIR JSON
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
		And I set the created Appointment to Cancelled with Reason "double booked"
	When I make a PUT request to "<url>"
	Then the response status code should be "404"
	Examples:
		| url              |
		| /appointmentqq/! |
		| /Appointments/#  |

Scenario Outline: Cancel appointment failure due to missing header
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the created Appointment to Cancelled with Reason "double booked"
		And I do not send header "<Header>"
	When I make the "AppointmentCancel" request
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

Scenario Outline: Cancel appointment failure with incorrect interaction id
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the created Appointment to Cancelled with Reason "double booked"
		And I am performing the "<interactionId>" interaction
	When I make the "AppointmentCancel" request
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| interactionId                                                     |
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
		And I set the created Appointment to Cancelled with Reason "double booked"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the cancellation reason in the returned appointment response should be equal to "double booked"
		And the returned appointment resource should contain meta data profile and version id
	Examples:
		| Parameter             | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Cancel appointment using the accept header to request response format
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the created Appointment to Cancelled with Reason "double booked"
		And I set the Accept header to "<Header>"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the cancellation reason in the returned appointment response should be equal to "double booked"
		And the returned appointment resource should contain meta data profile and version id
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Cancel appointment using the accept header and _format parameter to request response format
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the created Appointment to Cancelled with Reason "double booked"
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the cancellation reason in the returned appointment response should be equal to "double booked"
		And the returned appointment resource should contain meta data profile and version id
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
		And I set the created Appointment to Cancelled with Reason "double booked"
		And I set the request content type to "<ContentType>"
		And I set the Accept header to "<AcceptHeader>"
		And I add the parameter "_format" with the value "<FormatParam>"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the response body should be FHIR <Format>
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the cancellation reason in the returned appointment response should be equal to "double booked"
		And the returned appointment resource should contain meta data profile and version id
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
		And I set the created Appointment to Cancelled with URL "<url>" and Reason "<reason>"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the cancellation reason in the returned appointment response should be equal to "<reason>"
		And the returned appointment resource should contain meta data profile and version id
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
		And I set the created Appointment to Cancelled with URL "<url>" and Reason "<reason>"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
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
		And I set the created Appointment to Cancelled with URL "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1" and Reason ""
	When I make the "AppointmentCancel" request
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Conformance profile supports the cancel appointment operation
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the conformance profile should contain the "Appointment" resource with a "update" interaction

Scenario: Cancel appointment verify resource is updated when an valid ETag value is provided
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the created Appointment to Cancelled with Reason "double booked"
		And I set "If-Match" request header to created appointment version
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the cancellation reason in the returned appointment response should be equal to "double booked"	
		
Scenario: Cancel appointment verify resource is not updated when an out of date ETag value is provided
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the created Appointment to Cancelled with Reason "double booked"
		And I set If-Match request header to "invalidEtag"
	When I make the "AppointmentCancel" request	
	Then the response status code should be "409"
		And the response body should be FHIR JSON
		
Scenario: Cancel appointment compare request appointment to returned appointment
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the created Appointment to Cancelled with Reason "double booked"
	When I make the "AppointmentCancel" request	
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the resource type of the request appointment and the returned response should be equal
		And the id of the request appointment and the returned response should be equal
		And the status of the request appointment and the returned response should be equal
		And the extensions of the request appointment and the returned response should be equal
		And the description of the request appointment and the returned response should be equal
		And the start and end date of the request appointment and the returned response should be equal
		And the slot display and reference of the request appointment and the returned response should be equal
		And the reason of the request appointment and the returned response should be equal
		And the participants of the request appointment and the returned response should be equal
		

Scenario: Cancel appointment response body must contain valid slot reference
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the created Appointment to Cancelled with Reason "double booked"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the cancellation reason in the returned appointment response should be equal to "double booked"
		And the returned appointment resource status should be set to cancelled
		And the appointment response resource contains a slot reference
		And the appointment response resource contains atleast 2 participants a practitioner and a patient
		And the appointment participants of the appointment must conform to the gp connect specifications

Scenario Outline: Cancel appointment prefer header set to representation
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the created Appointment
	Given I configure the default "AppointmentCancel" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the created Appointment to Cancelled with Reason "double booked"
		And I set the Prefer header to "return=representation"
	When I make the "AppointmentCancel" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the content-type should not be equal to null
		And the content-length should not be equal to zero
		And the returned resource shall contains a logical id
		And the appointment resource should contain a single start element
		And the appointment resource should contain a single end element
		And the appointment resource should contain at least one slot reference
		And the appointment resource should contain at least one participant
		And if the appointment response resource contains a reason element and coding the codings must be one of the three allowed with system code and display elements
		And the returned appointment resource should contain meta data profile and version id
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
		And I set the created Appointment to Cancelled with Reason "double booked"
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
		And I set the created Appointment to Cancelled with Reason "Double booked"
	When I make the "AppointmentCancel" request	
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the cancellation reason in the returned appointment response should be equal to "Double booked"
		And the response version id should be different to the version id stored in the requesting appointment
	Examples:
		| PatientName |
		| patient1    |
		| patient2    |
		| patient3    |
		| patient8    |
		| patient10   |
		| patient12   |