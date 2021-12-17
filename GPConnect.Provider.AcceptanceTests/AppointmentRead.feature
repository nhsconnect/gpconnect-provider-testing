﻿@appointment @1.2.7-Full-Pack
Feature: AppointmentRead

@1.2.3
Scenario Outline: I perform a successful Read appointment
	Given I create an Appointment for Patient "<PatientName>" 
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointments returned must be in the future
		And the Appointment Id should be valid
		And the Appointment Metadata should be valid
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

Scenario Outline: I perform a successful Read appointment with Extensions
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

Scenario: I perform a successful Read appointment with JWT Org Different
# git hub ref 177
# RMB 31/1/19
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG3"
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the Appointment Organisation Code should equal "ORG1"

Scenario Outline: Read appointment invalid appointment id
	Given I create an Appointment for Patient "patient1" 
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
	Given I create an Appointment for Patient "patient1" 
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


Scenario Outline: Read appointment using the _format parameter and accept header to request response format
	Given I create an Appointment for Patient "patient1" 
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

Scenario Outline: Read appointment ensure response appointments contain the manadatory elements
	Given I create an Appointment for Patient "patient1"
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
		And the Appointment Identifiers should be valid
		And the Appointment Priority should be valid
		And the Appointment Participant Type and Actor should be valid
		And the Response should contain the ETag header matching the Resource Version Id
		And the Appointment booking organization extension and contained resource must be valid
		And the appointment reason must not be included
	Examples:
		| Header                | BodyFormat |
		| application/fhir+json | JSON       |
		| application/fhir+xml  | XML        |
	
Scenario: Read appointment valid response check caching headers exist
	Given I create an Appointment for Patient "patient1" 
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
	When I make the "AppointmentRead" request
	Then the response status code should indicate success
		And the Response Resource should be an Appointment
		And the required cacheing headers should be present in the response

Scenario:Read appointment invalid response check caching headers exist
	Given I create an Appointment for Patient "patient1" 
		And I store the Created Appointment
	Given I configure the default "AppointmentRead" request
		And I set the Read Operation logical identifier used in the request to "555555"
	When I make the "AppointmentRead" request
	Then the response status code should be "404"
		And the response body should be FHIR JSON

# git hub ref 171
# RMB 22/1/19
Scenario: CapabilityStatement profile supports the read appointment operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement REST Resources should contain the "Appointment" Resource with the "Read" Interaction


@1.2.7-IncrementalAndRegression
Scenario Outline: Read a patient’s appointments expecting servicecategory is populated
	Given I create an Appointment in "2" days time for Patient "patient1" and Organization Code "ORG1"
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
		And One Appointment contains serviceCategory element
	Examples:
		| PatientName | OrgType | DeliveryChannel | PracRole |
		| patient1    | true    | true            | true     |

@1.2.7-IncrementalAndRegression
Scenario Outline: Read a patient’s appointments expecting serviceType is populated
	Given I create an Appointment in "2" days time for Patient "patient1" and Organization Code "ORG1"
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
		And One Appointment contains serviceType element
	Examples:
		| PatientName | OrgType | DeliveryChannel | PracRole |
		| patient1    | true    | true            | true     |


@1.2.8-Only
Scenario Outline: Read appointment for patient where a HealthcareService was associated
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
	Given I configure the default "AppointmentRead" request
		When I make the "AppointmentRead" request
		Then the response status code should indicate success
		And the Appointment Participant Type and Actor should be valid
		And the Appointment Participant Actor should contains a HealthcareService Reference
	Examples:
		| HealthCareService |
		| HEALTHCARE2       |

