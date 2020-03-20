@appointment @1.5.0-Full-Pack
Feature: AppointmentRetrieve

Scenario: Appointment retrieve success valid id where appointment resource returned is not required
	Given I get the Patient for Patient Value "patientNoAppointments"
		And I store the Patient
	Given I configure the default "AppointmentSearch" request
		And I add start query parameters to the Request URL for Period starting today for "1" days
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain no Appointments

Scenario Outline: I perform a successful retrieve appointment with Extensions
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
# git hub ref 120
# RMB 25/10/2018		
		And the Appointment Not In Use should be valid
	Examples:
		| PatientName | OrgType | DeliveryChannel | PracRole |
		| patient1    | true    | true            | true     |

Scenario Outline: Appointment retrieve multiple appointment retrived
	Given I create "<numberOfAppointments>" Appointments for Patient "<patient>" and Organization Code "ORG1"
	Given I get the Patient for Patient Value "<patient>"
		And I store the Patient
	Given I configure the default "AppointmentSearch" request
		And I add start query parameters to the Request URL for Period starting today for "14" days
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain a minimum of "<numberOfAppointments>" Appointments
		And the Appointment Id should be valid
		And the Appointments returned must be in the future
		And the Appointment Status should be valid
		And the Appointment Start should be valid
		And the Appointment End should be valid
		And the Appointment Slots should be valid
		And the Appointment Participants should be valid and resolvable
		And the Appointment Description must be valid
		And the Appointment Created must be valid
		And the Appointment Participant Type and Actor should be valid
		And the Appointment Metadata should be valid
		And the Appointment Start should be valid
		And the Appointment End should be valid
		And the appointment reason must not be included
		And the Appointment DeliveryChannel must be valid
		And the Appointment PractitionerRole must be valid
		# git hub ref 120
# RMB 25/10/2018		
		And the Appointment Not In Use should be valid
	Examples:
		| patient  | numberOfAppointments |
		| patient4 | 1                    |
		| patient5 | 3                    |
		| patient6 | 2                    |

Scenario Outline: Appointment retrieve fail due to invalid patient logical id
	Given I get the Patient for Patient Value "patient4"
		And I store the Patient
	Given I configure the default "AppointmentSearch" request
		And I set the request URL to "Patient/<id>/Appointment?start=ge2030-03-30&start=le2030-04-30"
	When I make the "AppointmentSearch" request
	Then the response status code should be "404"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource
	Examples:
		| id   |
		| null |
		| dd   |
	
Scenario Outline: Appointment retrieve accept header and _format parameter to request response format
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the Accept header to "<Header>"
		And I add the start time period parameters for "14" days starting today using the prefixes "ge" and "le" and formats "yyyy-MM-dd" and "yyyy-MM-dd"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain a minimum of "1" Appointments
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/fhir+json | application/fhir+json | JSON       |
		| application/fhir+json | application/fhir+xml  | XML        |
		| application/fhir+xml  | application/fhir+json | JSON       |
		| application/fhir+xml  | application/fhir+xml  | XML        |

Scenario Outline: Appointment retrieve accept header to request response format
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I add start query parameters to the Request URL for Period starting today for "14" days
		And I set the Accept header to "<Header>"
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain a minimum of "1" Appointments
	Examples:
		| Header                | BodyFormat |
		| application/fhir+json | JSON       |
		| application/fhir+xml  | XML        |

Scenario Outline: Appointment retrieve _format parameter only to request response format
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I add the start time period parameters for "14" days starting today using the prefixes "ge" and "le" and formats "yyyy-MM-dd" and "yyyy-MM-dd"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain a minimum of "1" Appointments
	Examples:
		| Parameter             | BodyFormat |
		| application/fhir+json | JSON       |
		| application/fhir+xml  | XML        |

Scenario Outline: Appointment retrieve invalid date format
	Given I get the Patient for Patient Value "patient4"
		And I store the Patient
	Given I configure the default "AppointmentSearch" request
		And I add the start time period parameters for "14" days starting today using the prefixes "ge" and "le" and formats "<StartFormat>" and "<EndFormat>"
	When I make the "AppointmentSearch" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
	Examples:
		| StartFormat          | EndFormat            |
		| yyyy-MM-dd           | yyyy                 |
		| yyyy-MM-dd           | yyyy-MM              |
		| yyyy-MM-dd           | yyyy-MM-ddTHH:mm:ss  |
		| yyyy-MM-dd           | yyyy-MM-ddTHH:mm:ssZ |
		| yyyy                 | yyyy-MM-dd           |
		| yyyy-MM              | yyyy-MM-dd           |
		| yyyy-MM-ddTHH:mm:ss  | yyyy-MM-dd           |
		| yyyy-MM-ddTHH:mm:ssZ | yyyy-MM-dd           |

Scenario Outline: Appointment retrieve invalid date prefix
	Given I get the Patient for Patient Value "patient5"
		And I store the Patient
	Given I configure the default "AppointmentSearch" request
		And I add the start time period parameters for "14" days starting today using the prefixes "<StartPrefix>" and "<EndPrefix>" and formats "yyyy-MM-dd" and "yyyy-MM-dd"
	When I make the "AppointmentSearch" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
	Examples:
		| StartPrefix | EndPrefix |
		| ge          | lt        |
		| ge          | gt        |
		| ge          | eq        |
		| ge          | ge        |
		| lt          | le        |
		| gt          | le        |
		| eqs         | le        |
		| le          | le        |
		| random123   | random123 |

Scenario Outline: Appointment retrieve with only one start parameter
	Given I get the Patient for Patient Value "patient5"
		And I store the Patient
	Given I configure the default "AppointmentSearch" request
		And I add the parameter "start" with the value "<Parameter>"
	When I make the "AppointmentSearch" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
	Examples:
		| Parameter    |
		| le2030-03-03 |
		| ge2030-03-03 |

Scenario Outline: Appointment retreive with date parameters in the past
	Given I get the Patient for Patient Value "patient3"
		And I store the Patient
	Given I configure the default "AppointmentSearch" request
		And I add the parameter "start" with the value "<StartDate>"
		And I add the parameter "start" with the value "<EndDate>"
	When I make the "AppointmentSearch" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
	Examples:
		| StartDate  | EndDate    |
		| 2010-02-03 | 2010-03-03 |
		| 2010-03-03 | 2030-03-30 |

Scenario: Appointment retreive with end date before start date
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I add the parameter "start" with the value "ge2030-03-03"
		And I add the parameter "start" with the value "le2030-01-01"
	When I make the "AppointmentSearch" request
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Appointment retrieve bundle valid resources returned in the response
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I add start query parameters to the Request URL for Period starting today for "14" days
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Appointment Participants should be valid and resolvable

Scenario Outline: Appointment retrieve JWT patient type request invalid
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I add start query parameters to the Request URL for Period starting today for "14" days
		And I set the JWT requested scope to "<JWTType>"
	When I make the "AppointmentSearch" request
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| JWTType              |
		| organization/*.write |
		| patient/*.write      |

Scenario: Appointment retrieve sending additional valid parameters in the request
	Given I get the Patient for Patient Value "patient5"
		And I store the Patient
	Given I configure the default "AppointmentSearch" request
		And I add the parameter "start" with the value "ge2030-03-03"
		And I add the parameter "start" with the value "le2030-03-30"
		And I add the parameter "_sort" with the value "date"
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"

Scenario: CapabilityStatement profile supports the search appointment operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement REST Resources should contain the "Appointment" Resource with the "SearchType" Interaction

Scenario: Appointment retrieve valid response check caching headers exist
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I configure the default "AppointmentSearch" request
		And I add start query parameters to the Request URL for Period starting today for "1" days
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the required cacheing headers should be present in the response

Scenario: Appointment retrieve invalid response check caching headers exist
	Given I get the Patient for Patient Value "patient4"
		And I store the Patient
	Given I configure the default "AppointmentSearch" request
		And I add start query parameters to the Request URL for Period starting today for "14" days
		And I set the Interaction Id header to "urn:nhs:names:services:gpconnect:fhir:rest:search:organization-1 "
	When I make the "AppointmentSearch" request
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		And the required cacheing headers should be present in the response

Scenario: Appointment retrieve and response should contain valid booking orgainzation
	Given I create an Appointment for an existing Patient and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I add start query parameters to the Request URL for Period starting today for "14" days
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Appointment booking organization extension and contained resource must be valid
