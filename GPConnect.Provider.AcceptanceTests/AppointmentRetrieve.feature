Feature: SpecFlowFeature1

Background:
	Given I have the test patient codes
	Given I have the test ods codes

@Appointment
Scenario Outline: Appointment retrieve success valid id where appointment resource returned is not required
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I make a GET request to "/Patient/<id>/Appointment"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| id     |
		| 1		 |
		| 2		 |
		| 400000 |

Scenario: Appointment retrieve success valid id where single appointment resource is required resource
Given I find or create "6" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
	
	
Scenario Outline: Appointment retrieve fail invalid id
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I make a GET request to "/Patient/<id>/Appointment"
	Then the response status code should be "404"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource
	Examples:
		| id  |
		| **  |
		| dd  |
		|     |
		| null|

Scenario Outline: Appointment retrieve send request with date variations with valid start date
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I search for "patient1" and make a get request for their appointments with the date "<startDate>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
	Examples:
		 | startDate                 |
		 | 2014                      |
		 | 2014-02                   |
		 | 2014-10-05                |
		 | 2014-05                   |
		 | 2014-05-01T11:08:32       |
		 | 2015-10-23T11:08:32+00:00 |
		 | 2014                      |
		 | 2014-02                   |
		 | 2014-10-05                |
		 | 2014-05                   |
		 | 2014-05-01T11:08:32       |
		 | 2015-10-23T11:08:32+00:00 |

##Creates an appointment every time run to check the date exists
Scenario: Appointment retrieve book appointment then request appointment and check it is returned
	Given I create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I search for "patient1" and make a get request for their appointments with the date "startDate"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response total should be at least 1

	
Scenario Outline: Appointment retrieve send request with date variations which are invalid
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I search for "patient1" and make a get request for their appointments searching with the date "<startDate>"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource
	Examples:
		| startDate                 |
		| 16-02-2016                |
		| 16/02/2016                |
		| 99-99-99999               |
		| 99999                     |
		| 201                       |
		| 2016-13                   |
		| 2016-13-14                |
		| 2016-13-08T09:22:16       |
		| 2016-13-08T23:59:59+00:00 |
		| 2014                      |      

@ignore
Scenario Outline: Appointment retrieve send request with date variations which are valid with prefix
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I save to current time called "timeNow"
	When I search for "patient1" and make a get request for their appointments with the date "timeNow" and prefix "<prefix>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response total should be at least 1
	Examples:
		| startDate                 | prefix |
		| 2014                      | eq     |
		| 2014-02                   | eq     |
		| 2014-10-05                | eq     |
		| 2014-05                   | eq     |
		| 2014-05-01T11:08:32       | eq     |
		| 2015-10-23T11:08:32+00:00 | eq     |
		| 2014                      | ne     |
		| 2014-02                   | ne     |
		| 2014-10-05                | ne     |
		| 2014-05                   | ne     |
		| 2014-05-01T11:08:32       | ne     |
		| 2015-10-23T11:08:32+00:00 | ne     |
		| 2014                      | gt     |
		| 2014-02                   | gt     |
		| 2014-10-05                | gt     |
		| 2014-05                   | gt     |
		| 2014-05-01T11:08:32       | gt     |
		| 2015-10-23T11:08:32+00:00 | gt     |
		| 2014                      | lt     |
		| 2014-02                   | lt     |
		| 2014-10-05                | lt     |
		| 2014-05                   | lt     |
		| 2014-05-01T11:08:32       | lt     |
		| 2015-10-23T11:08:32+00:00 | lt     |
		| 2014                      | sa     |
		| 2014-02                   | sa     |
		| 2014-10-05                | sa     |
		| 2014-05                   | sa     |
		| 2014-05-01T11:08:32       | sa     |
		| 2015-10-23T11:08:32+00:00 | sa     |
		| 2014                      | sb     |
		| 2014-02                   | sb     |
		| 2014-10-05                | sb     |
		| 2014-05                   | sb     |
		| 2014-05-01T11:08:32       | sb     |
		| 2015-10-23T11:08:32+00:00 | sb     |
		| 2014                      | ap     |
		| 2014-02                   | ap     |
		| 2014-10-05                | ap     |
		| 2014-05                   | ap     |
		| 2014-05-01T11:08:32       | ap     |
		| 2015-10-23T11:08:32+00:00 | ap     |
		


Scenario Outline: Appointment retrieve failure due to missing header
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I do not send header "<Header>"
	When I search for "patient1" and make a get request for their appointments
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

Scenario Outline: Appointment retrieve interaction id incorrect fail
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
    Given I am using the default server
        And I am performing the "<interactionId>" interaction
    When I search for "patient1" and make a get request for their appointments
    Then the response status code should be "400"
        And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
    Examples:
       | id | interactionId                                                     |
       | 1  | urn:nhs:names:services:gpconnect:fhir:rest:search:organization    |
       | 1  | urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
       | 1  |                                                                   |
       | 1  | null                                                              |
	
Scenario Outline: Appointment retrieve accept header and _format parameter
    Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
        And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
        And I set the Accept header to "<Header>"
        And I add the parameter "_format" with the value "<Parameter>"
    When I search for "patient1" and make a get request for their appointments
    Then the response status code should indicate success
        And the response body should be FHIR <BodyFormat>
        And the response should be a Bundle resource of type "searchset"
    Examples:
       | Header                | Parameter             | BodyFormat |
       | application/json+fhir | application/json+fhir | JSON       |
       | application/json+fhir | application/xml+fhir  | XML        |
       | application/xml+fhir  | application/json+fhir | JSON       |
       | application/xml+fhir  | application/xml+fhir  | XML        |  

Scenario Outline: Appointment retrieve accept header
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I set the Accept header to "<Header>"
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Appointment retrieve bundle resource with empty appointment resource
	Given I find or create "0" appointments for patient "<patient>" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I search for "<patient>" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
        And the response should be a Bundle resource of type "searchset"
		And there are zero appointment resources
	 Examples:
        | patient  |
        | patient2 |

Scenario: Appointment retrieve appointment which contains all mandatory resources
		Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
        And the response should be a Bundle resource of type "searchset"
	Then the bundle of appointments should all contain a single status element
	Then the bundle of appointments should all contain a single start element
	Then the bundle of appointments should all contain a single end element
	Then the bundle of appointments should all contain at least one slot reference
	Then the bundle of appointments should all contain at least one participant
	
Scenario: Appointment retrieve bundle resource must contain status with valid value
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
        And the response should be a Bundle resource of type "searchset"
	Then the bundle appointment resource should contain a single status element
		And the appointment status element should be valid


Scenario: Appointment retrieve bundle resource must contain participant with type or actor present
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
	Then if appointment is present the single or multiple participant must contain a type or actor


Scenario: Appointment retrieve bundle participant actor contains valid references
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And if actor returns a practitioner resource the resource is valid
		And if actor returns a location resource the resource is valid
		And if actor returns a location resource the resource is valid
		And if actor returns a patient resource the resource is valid

Scenario: Appointment retrieve bundle participant type contains valid references
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And if the appointment participant contains a type is should have a valid system and code

Scenario: Appointment retrieve bundle contains appointment with identifer with correct system and value
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And if the appointment resource contains an identifier it contains a valid system and value

Scenario: Appointment retrieve appointment response should contain meta data profile and version id
		Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the bundle of appointments should contain meta data profile and version id
	
Scenario: Appointment retrieve bundle of coding type SNOMED resource must contain coding with valid system and code and display
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And if appointment contains the resource coding SNOMED CT element the fields should match the fixed values of the specification

	
Scenario: Appointment retrieve bundle of coding type READ V2 resource must contain coding with valid system and code and display
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And if appointment contains the resource coding READ V2 element the fields should match the fixed values of the specification


Scenario: Appointment retrieve bundle of coding type SREAD CTV3 resource must contain coding with valid system and code and display
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And if appointment contains the resource coding SREAD CTV3 element the fields should match the fixed values of the specification


Scenario: Appointment retrieve bundle contains appointment with slot 
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the slot reference is present and valid

	
Scenario: Appointment retrieve bundle contains appointment contact method
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And if the appointment category element is present it is populated with the correct values
		And if the appointment booking element is present it is populated with the correct values
		And if the appointment contact element is present it is populated with the correct values
		And if the appointment cancellation reason element is present it is populated with the correct values

Scenario: Appointment retrieve bundle contains valid start and end dates
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And all appointments must have an start element which is populated with a valid date
		And all appointments must have an end element which is populated vith a valid date

@ignore
Scenario Outline: Appointment retrieve JWT requesting scope claim should reflect the operation being performed
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
	When I search for Patient "/Patient/<id>/Appointment"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		Examples:
        | id | 
		| 5  |
