@appointment
Feature: AppointmentCancel

Scenario Outline: I perform a successful cancel appointment
	Given I find or create an appointment with status Booked for patient "<PatientName>" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the JWT requested record NHS number to config patient "<PatientName>"
		And I set the JWT requested scope to "patient/*.write"
	When I cancel the appointment with the key "patientApp" and set the reason to "double booked"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
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

Scenario Outline: I perform cancel appointment and update an element which is invalid
	Given I find or create an appointment with status Booked for patient "<PatientName>" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the JWT requested record NHS number to config patient "<PatientName>"
		And I set the JWT requested scope to "patient/*.write"
		And I add an extension to "patientApp" with url "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-category-1" code "CLI" and display "Clinical"
		And I add an extension to "patientApp" with url "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-booking-method-1" code "ONL" and display "	Online"
		And I add an extension to "patientApp" with url "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-contact-method-1" code "ONL" and display "	Online"
		And I set the description to "RANDOM TEXT" for appointment "patientApp"
		And I set the priority to "3" for appointment "patientApp"
		And I set the minutes to "20" for appointment "patientApp"
		And I set the comment to "RANDOM COMMENT" for appointment "patientApp"
		And I set the type text to "RANDOM TYPE TEXT" for appointment "patientApp"
		And I set the identifier with system "http://fhir.nhs.net/Id/gpconnect-appointment-identifier" and value "898976578" for the appointment "patientApp"
		And I set the identifier with system "http://fhir.nhs.net/Id/gpconnect-appointment-identifier" and value "898955578" for the appointment "patientApp"
		And I add participant "location" with reference "lacation/2" to appointment "patientApp"
		And I add participant "patient" with reference "patient/2" to appointment "patientApp"
		And I add participant "practitioner" with reference "practitioner/2" to appointment "patientApp"
	When I cancel the appointment with the key "patientApp" and set the reason to "double booked"
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| PatientName |
		| patient1    |
		| patient2    |
		| patient3    |
		| patient10   |
		| patient11   |
		| patient12   |

Scenario Outline: Cancel appointment sending invalid URL
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.write"
	When I set the URL to "<url>" and cancel appointment with key "patientApp"
	Then the response status code should be "404"
	Examples:
		| url             |
		| /appointmentqq/!  |
		| /Appointments/# |

Scenario Outline: Cancel appointment failure due to missing header
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I do not send header "<Header>"
	When I cancel the appointment with the key "patientApp" and set the reason to "double booked"
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
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "<interactionId>" interaction
	When I cancel the appointment with the key "patientApp" and set the reason to "double booked"
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

Scenario Outline: Cancel appointment _format parameter only
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I add the parameter "_format" with the value "<Parameter>"
	When I cancel the appointment with the key "patientApp" and set the reason to "double booked"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
	Examples:
		| Parameter             | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Cancel appointment accept header only
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I set the Accept header to "<Header>"
	When I cancel the appointment with the key "patientApp" and set the reason to "double booked"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Cancel appointment accept header and _format parameter
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I cancel the appointment with the key "patientApp" and set the reason to "double booked"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/json+fhir | application/json+fhir | JSON       |
		| application/json+fhir | application/xml+fhir  | XML        |
		| application/xml+fhir  | application/json+fhir | JSON       |
		| application/xml+fhir  | application/xml+fhir  | XML        |
		
Scenario Outline: Cancel appointment checking that the format parameter and accept header works correctly
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I set the request content type to "<ContentType>"
		And I set the Accept header to "<AcceptHeader>"
		And I add the parameter "_format" with the value "<FormatParam>"
	When I cancel the appointment with the key "patientApp" and set the reason to "double booked"
	Then the response status code should indicate success
		And the response body should be FHIR <Format>
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
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
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.write"
	When I cancel the appointment and set the cancel extension to have url "<url>" and reason "<reason>" called "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the cancellation reason in the returned appointment response should be equal to "<reason>"
	Examples:
		| url                                                                                           | reason      |
		| http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1 | Too busy    |
		| http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1 | Car crashed |
		| http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1 | Too tired   |

Scenario Outline: Cancel appointment invalid cancellation extension url
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.write"
	When I cancel the appointment and set the cancel extension to have url "<url>" and reason "<reason>" called "patientApp"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"
	Examples:
		| url                                                                                                | reason   |
		|                                                                                                    | Too busy |
		| http://fhir.nhs.net/StructureDefinition/extension-gpINCORRECTect-appointment-cancellation-reason-1 | Too busy |
		| http://fhir.nhs.net/StructuraeDefinition/extension-gpconnect-appointment-cancellation-reason-1-010 | Too busy |

Scenario: Cancel appointment missing cancellation extension reason
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.write"
	When I cancel the appointment and set the cancel extension to have url "http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1" and missing reason called "patientApp"
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
	Given I find or create an appointment with status Booked for patient "patient4" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I set "If-Match" request header to "patientApp" version
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the JWT requested record NHS number to config patient "patient4"
		And I set the JWT requested scope to "patient/*.write"
	When I cancel the appointment with the key "patientApp" and set the reason to "double booked"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And I make a GET request for the appointment with key "patientApp" for patient "patient4" to ensure the status has been changed to cancelled
	
Scenario: Cancel appointment verify resource is not updated when an out of date ETag value is provided
	Given I find or create an appointment with status Booked for patient "patient3" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I set If-Match request header to "invalidEtag"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the JWT requested record NHS number to config patient "patient3"
		And I set the JWT requested scope to "patient/*.write"
	When I cancel the appointment with the key "patientApp" and set the reason to "double booked"
	Then the response status code should be "409"
		And the response body should be FHIR JSON
		And I make a GET request for the appointment with key "patientApp" for patient "patient3" to ensure the status has not been changed to cancelled

Scenario: Cancel appointment compare values send in request and returned in the response
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.write"
	When I cancel the appointment with the key "patientApp" and set the reason to "double booked"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the resource type of the appointment with key "patientApp" and the returned response should be equal
		And the id of the appointment with key "patientApp" and the returned response should be equal
		And the status of the appointment with key "patientApp" and the returned response should be equal
		And the extensions of the appointment with key "patientApp" and the returned response should be equal
		And the description of the appointment with key "patientApp" and the returned response should be equal
		And the start and end date of the appointment with key "patientApp" and the returned response should be equal
		And the slot display and reference of the appointment with key "patientApp" and the returned response should be equal
		And the reason of the appointment with key "patientApp" and the returned response should be equal
		And the participants of the appointment with key "patientApp" and the returned response should be equal

Scenario: Cancel appointment response body must contain valid slot reference
	Given I find or create an appointment with status Booked for patient "patient3" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.write"
	When I cancel the appointment with the key "patientApp" and set the reason to "double booked"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the appointment response resource contains a slot reference

Scenario: Cancel appointment response body must contain valid practitioner reference which conforms to the gp connect specification
	Given I find or create an appointment with status Booked for patient "patient2" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
		And I set the JWT requested scope to "patient/*.write"
	When I cancel the appointment with the key "patientApp" and set the reason to "double booked"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the appointment response resource contains atleast 2 participants a practitioner and a patient
		And the appointment participants of the appointment must conform to the gp connect specifications

Scenario Outline: Cancel appointment prefer header set to representation
	Given I find or create an appointment with status Booked for patient "<PatientName>" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I set the Prefer header to "return=representation"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the JWT requested record NHS number to config patient "<PatientName>"
		And I set the JWT requested scope to "patient/*.write"
	When I cancel the appointment with the key "patientApp" and set the reason to "double booked"
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
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I set the Prefer header to "return=minimal"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.write"
	When I cancel the appointment with the key "patientApp" and set the reason to "Double booked"
	Then the response status code should indicate success
		And the response body should be empty
		And the content-type should be equal to null

Scenario Outline: Cancel appointment check the version id of the cancelled resource is different
	Given I find or create an appointment with status Booked for patient "<PatientName>" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the JWT requested record NHS number to config patient "<PatientName>"
		And I set the JWT requested scope to "patient/*.write"
	When I cancel the appointment with the key "patientApp" and set the reason to "Double booked"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the cancellation reason in the returned appointment response should be equal to "Double booked"
		And the response version id should be different to the version id stored in "patientApp"
	Examples:
		| PatientName |
		| patient1    |
		| patient2    |
		| patient3    |
		| patient8    |
		| patient10   |
		| patient12   |