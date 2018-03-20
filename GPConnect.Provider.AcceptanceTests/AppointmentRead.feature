@appointment
Feature: AppointmentRead

Scenario Outline: I perform a successful Read appointment
	Given I create an Appointment for Patient "<PatientName>" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
	Examples:
		| PatientName |
		| patient1    |
		| patient2    |
		| patient3    |
		| patient4    |
		| patient5    |
		| patient6    |

Scenario Outline: Read appointment responding appointment is in the future
	Given I create an Appointment for Patient "<PatientName>" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointments returned must be in the future
	Examples:
		| PatientName |
		| patient1    |
		| patient2    |
		| patient3    |
		| patient4    |
		| patient5    |
		| patient6    |

Scenario Outline: Read appointment invalid appointment id
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
		And I set the Read Operation logical identifier used in the request to "<id>"
	When I make the "AppointmentRead" request
	Then the response status code should be "404"
		And the response body should be FHIR JSON
	Examples:
		| id          |
		| Invalid4321 |
		| 8888888888  |
		|             |

Scenario Outline: Read appointment using the _format parameter to request response format
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be an Appointment
	Examples:
		| Parameter             | BodyFormat |
		| application/fhir+json | JSON       |
		| application/fhir+xml  | XML        |

Scenario Outline: Read appointment using the accept header to request response format
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
		And I set the Accept header to "<Header>"
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat> 
		And the Response Resource should be an Appointment
	Examples:
		| Header                | BodyFormat |
		| application/fhir+json | JSON       |
		| application/fhir+xml  | XML        |


Scenario Outline: Read appointment using the _format parameter and accept header to request response format
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be an Appointment
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/fhir+json | application/fhir+json | JSON       |
		| application/fhir+json | application/fhir+xml  | XML        |
		| application/fhir+xml  | application/fhir+json | JSON       |
		| application/fhir+xml  | application/fhir+xml  | XML        |

Scenario: Read appointment valid request shall include id and structure definition profile
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Appointment Id should be valid
		And the Appointment Metadata should be valid

Scenario Outline: Read appointment ensure response appointments contain the manadatory elements
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I set the Created Appointment Status to "<AppointmentStatus>"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
		And I set the Accept header to "<Header>"
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be an Appointment
		And the Appointment Status should be valid
		And the Appointment Start should be valid
		And the Appointment End should be valid
		And the Appointment Slots should be valid
		And the Appointment Participants should be valid and resolvable
		And the Appointment Description must be valid
		And the Appointment Created must be valid
	Examples:
		| AppointmentStatus | Header                | BodyFormat |
		| Booked            | application/fhir+json | JSON       |
		| Booked            | application/fhir+xml  | XML        |

Scenario: Read appointment if resource contains identifier then the value is mandatory
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Appointment Identifiers should be valid

Scenario: Read appointment containing a priority element and check that the priority is valid
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
		And I set the Created Appointment Priority to "1"
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And I store the Appointment
		And the Response Resource should be an Appointment
	Given I configure the default "AppointmentRead" request
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Response Resource should be an Appointment
		And the Appointment Priority should be valid
	
Scenario: Read appointment if all participants must have a actor element
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And I store the Appointment
		And the Response Resource should be an Appointment
	Given I configure the default "AppointmentRead" request
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointment Participant Type and Actor should be valid
		

Scenario: Read appointment and response should contain an ETag header
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
	When I make the "AppointmentRead" request 
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Response Resource should be an Appointment
		And the Response should contain the ETag header matching the Resource Version Id

Scenario: Read appointment valid response check caching headers exist
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the required cacheing headers should be present in the response

Scenario:Read appointment invalid response check caching headers exist
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
		And I set the Read Operation logical identifier used in the request to "555555"
	When I make the "AppointmentRead" request
	Then the response status code should be "404"
		And the response body should be FHIR JSON


Scenario: Read appointment and response should contain valid booking orgainzation
	Given I get an existing patients nshNumber
		And I store the Patient
	Given I get Available Free Slots
		And I store the Free Slots Bundle
	Given I configure the default "AppointmentCreate" request
		And I create an Appointment from the stored Patient and stored Schedule
	When I make the "AppointmentCreate" request
	Then the response status code should indicate created
		And I store the Appointment
		And the Response Resource should be an Appointment
	Given I configure the default "AppointmentRead" request
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointment booking organization extension and contained resource must be valid
