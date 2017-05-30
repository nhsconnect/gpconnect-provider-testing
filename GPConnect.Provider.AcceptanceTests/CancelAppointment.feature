Feature: CancelAppointment

Background:
	Given I have the test patient codes
	Given I have the test ods codes

Scenario Outline: I perform a successful cancel appointment
	Given I find or create an appointment with status Booked for patient "<PatientName>" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment with the key "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
	Examples: 
		| PatientName        |
		| patient1           |
		| patient2           |
		| patient3           |
		| CustomAppointment1 |

Scenario Outline: I perform a successful cancel appointment and update an element which is invalid
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment with the key "patientApp" and change the "Element" element
	Then the response status code should indicate failure
	Examples: 
		| Element         |
		| description     |
		| priority        |
		| minutesDuration |
		| comment         |
		| typeText        |
		| identifier      |
		| reason          |
		| participant     |                
		        	
Scenario Outline: Cancel appointment sending invalid URL
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I set the URL to "<url>" and cancel appointment with key "patientApp"
	Then the response status code should indicate failure
	Examples: 
		| url                 |
		| /Appointment/!      |
		| /APPointment/23     |
		| /Appointment/#      |
		| /Appointment/cancel |

Scenario Outline: Cancel appointment failure due to missing header
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I do not send header "<Header>"
	When I cancel the appointment with the key "patientApp"
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
	When I cancel the appointment with the key "patientApp"
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
		And I add the parameter "_format" with the value "<Parameter>"
	When I cancel the appointment with the key "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
	Examples:
        | Parameter             | BodyFormat |
        | application/json+fhir | JSON       |
        | application/xml+fhir  | XML        |

Scenario Outline: Cancel appointment accept header and _format parameter
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
		And I set the Accept header to "<Header>"
        And I add the parameter "_format" with the value "<Parameter>"
	When I cancel the appointment with the key "patientApp"
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


Scenario Outline: Cancel appointment check cancellation reason is equal to the request cancellation reason
	Given I find or create an appointment with status Booked for patient "CustomAppointment1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment and set the cancel extension to have url "<url>" code "<code>" and display "<display>" called "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the cancellation reason in the returned appointment response should be equal to "<display>"
	Examples: 
		| url                                                                                             | code | display     |
		| http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1-0 | aa   | Too busy    |
		| http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1-0 | aa   | Car crashed |
		| http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1-0 | aa   | Too tired   |


Scenario Outline: Cancel appointment invalid cancellation extension
	Given I find or create an appointment with status Booked for patient "CustomAppointment1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment and set the cancel extension to have url "<url>" code "<code>" and display "<display>" called "patientApp"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "500"
	Examples: 
		| url                                                                                                  | code | display |
		|                                                                                                      |      |         |
		|                                                                                                      | ff   |         |
		|                                                                                                      |      | ee      |
		| http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1-0      |      |         |
		| http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1-0      | ff   |         |
		| http://fhir.nhs.net/StructureDefinition/extension-gpconnect-appointment-cancellation-reason-1-0      |      | ee      |
		| http://fhir.nhs.net/StructureDefinition/extension-gpINCORRECTect-appointment-cancellation-reason-1-0 |      | ee      |


Scenario: Cancel appointment and check the returned appointment resource is a valid resource
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment with the key "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the returned appointment resource should contain meta data profile and version id

Scenario: Conformance profile supports the cancel appointment operation
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the conformance profile should contain the "Appointment" resource with a "update" interaction

Scenario: Cancel appointment verify resource is updated when an valid ETag value is provided
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I perform an appointment read on appointment saved with key "patientApp" and read the etag and save it as "etagCancel"
	Given I am using the default server
		And I set "If-Match" request header to "etagCancel"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment with the key "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And I make a GET request for the appointment with key "patientApp" to ensure the status has been changed to cancelled
	
Scenario: Cancel appointment verify resource is not updated when an out of date ETag value is provided
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I perform an appointment read on appointment saved with key "patientApp" and read the etag and save it as "etagCancel"
	Given I am using the default server
		And I set If-Match request header to "invalidEtag"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment with the key "patientApp"
	Then the response status code should be "409"
		And the response body should be FHIR JSON
		And I make a GET request for the appointment with key "patientApp" to ensure the status has not been changed to cancelled

Scenario: Cancel appointment compare values send in request and returned in the response
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment with the key "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the resource type of the appointment with key "patientApp" and the returned response should be equal
		And the id of the appointment with key "patientApp" and the returned response should be equal
		And the status of the appointment with key "patientApp" and the returned response should be equal
		And the extension of the appointment with key "patientApp" and the returned response should be equal
		And the description of the appointment with key "patientApp" and the returned response should be equal
		And the start and end date of the appointment with key "patientApp" and the returned response should be equal
		And the slot display and reference of the appointment with key "patientApp" and the returned response should be equal
		And the reason of the appointment with key "patientApp" and the returned response should be equal
		And the "Patient" participant of the appointment with key "patientApp" and the returned response should be equal
		And the "Location" participant of the appointment with key "patientApp" and the returned response should be equal
		And the "Practitioner" participant of the appointment with key "patientApp" and the returned response should be equal

Scenario: Cancel appointment response body must contain valid slot reference
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment with the key "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the appointment response resource contains a slot reference

Scenario: Cancel appointment response body must contain valid practitioner reference which conforms to the gp connect specification
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment with the key "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the appointment response resource contains atleast 2 participants a practitioner and a patient
		And the appointment participant "Practitioner" reference must be a valid reference and is saved as "practitioner1"
		And the practitoner resource saved with name "practitioner1" must contain name with a valid subset of elements
		And if the practitioner resource saved with name "practitioner1" contains an identifier then it is valid
		And if the practitioner resource saved with name "practitioner1" contains a practitioner role it is valid 
		And if the practitioner resource saved with name "practitioner1" has communicaiton elemenets containing a coding then there must be a system, code and display element

Scenario: Cancel appointment response body must contain valid patient reference which conforms to the gp connect specification
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment with the key "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the appointment response resource contains atleast 2 participants a practitioner and a patient
		And the appointment participant "Patient" reference must be a valid reference and is saved as "patient1"
		And the patient resource saved with name "patient1" must contain identifier with valid system and value
		And the patient resource saved with name "patient1" should contain meta data profile and version id

Scenario: Cancel appointment if response body contains location reference it conforms to the gp connect specification
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment with the key "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the appointment participant "Location" reference must be a valid reference and is saved as "location1"
		And if the location resource saved with the name "location1" should contain a maximum of one ODS Site Code and one other identifier
		And if the location resource saved with the name "location1" should contain a name element
		And if the location resource saved with the name "location1" should contain system code and display if the Type coding is included in the resource
		And if the location resource saved with the name "location1" should contain valid system code and display if the PhysicalType coding is included in the resource
		And if the location resource saved with the name "location1" contains partOf element the reference should reference a resource in the response bundle
		And if the location resource saved with the name "location1" contains managingOrganization element the reference should reference a resource in the response bundle

Scenario: Cancel appointment prefer header set to representation
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I set the Prefer header to "return=representation"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment with the key "patientApp"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Appointment resource
		And the returned appointment resource status should be set to cancelled
		And the content-type should not be equal to null
		And the content-length should not be equal to zero

Scenario: Cancel appointment prefer header set to minimal
	Given I find or create an appointment with status Booked for patient "patient1" at organization "ORG1" and save the appointment resources to "patientApp"
	Given I am using the default server
		And I set the Prefer header to "return=minimal"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:update:appointment" interaction
	When I cancel the appointment with the key "patientApp"
	Then the response status code should indicate success
		And the response body should be empty
		And the content-type should be equal to null
		And the content-length should be equal to zero
