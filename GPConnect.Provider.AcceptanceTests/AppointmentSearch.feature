@appointment
Feature: AppointmentSearch

Scenario: Appointment retrieve success valid id where appointment resource returned is not required
	Given I get the Patient for Patient Value "patient15"
		And I store the Patient
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And there are zero appointment resources

Scenario Outline: Appointment retrieve success valid id where single appointment resource should be returned
	Given I create an Appointment for Patient "<patient>" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain atleast "1" appointment
	Examples:
		| patient  |
		| patient1 |
		| patient2 |
		| patient3 |

Scenario Outline: Appointment retrieve multiple appointment retrived
	Given I create "<numberOfAppointments>" Appointments for Patient "<patient>" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain atleast "<numberOfAppointments>" appointment
	Examples:
		| patient  | numberOfAppointments |
		| patient4 | 2                    |
		| patient5 | 4                    |
		| patient6 | 3                    |

Scenario Outline: Appointment retrieve fail due to invalid patient logical id
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I make a GET request to "/Patient/<id>/Appointment"
	Then the response status code should be "404"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource
	Examples:
		| id   |
		| **   |
		| dd   |
		|      |
		| null |
	
Scenario Outline: Appointment retrieve send request with date variations which are invalid
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I search for "patient1" and make a get request for their appointments with the date "<startDate>" and prefix "<prefix>"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource
	Examples:
		| startDate                 | prefix |
		| 16-02-2016                | gt     |
		| 16/02/2016                | gt     |
		| 99-99-99999               | gt     |
		| 99999                     | gt     |
		| 201                       | gt     |
		| 2016-13                   | gt     |
		| 2016-13-14                | gt     |
		| 2016-13-08T09:22:16       | gt     |
		| 2016-13-08T23:59:59+00:00 | gt     |
		| 2016-08-05T08:16          | gt     |
		| 2016-08-                  | gt     |
		| 2016-08-05 08:16:07       | gt     |
		| 16-02-2016                | lt     |
		| 16/02/2016                | lt     |
		| 99-99-99999               | lt     |
		| 99999                     | lt     |
		| 201                       | lt     |
		| 2016-13                   | lt     |
		| 2016-13-14                | lt     |
		| 2016-13-08T09:22:16       | lt     |
		| 2016-13-08T23:59:59+00:00 | lt     |
		| 2016-08-05T08:16          | lt     |
		| 2016-08-                  | lt     |
		| 2016-08-05 08:16:07       | lt     |
		| 16-02-2016                | ge     |
		| 16/02/2016                | ge     |
		| 99-99-99999               | ge     |
		| 99999                     | ge     |
		| 201                       | ge     |
		| 2016-13                   | ge     |
		| 2016-13-14                | ge     |
		| 2016-13-08T09:22:16       | ge     |
		| 2016-13-08T23:59:59+00:00 | ge     |
		| 2016-08-05T08:16          | ge     |
		| 2016-08-                  | ge     |
		| 2016-08-05 08:16:07       | ge     |
		| 16-02-2016                | le     |
		| 16/02/2016                | le     |
		| 99-99-99999               | le     |
		| 99999                     | le     |
		| 201                       | le     |
		| 2016-13                   | le     |
		| 2016-13-14                | le     |
		| 2016-13-08T09:22:16       | le     |
		| 2016-13-08T23:59:59+00:00 | le     |
		| 2016-08-05T08:16          | le     |
		| 2016-08-                  | le     |
		| 2016-08-05 08:16:07       | le     |
		| 16-02-2016                | eq     |
		| 16/02/2016                | eq     |
		| 99-99-99999               | eq     |
		| 99999                     | eq     |
		| 201                       | eq     |
		| 2016-13                   | eq     |
		| 2016-13-14                | eq     |
		| 2016-13-08T09:22:16       | eq     |
		| 2016-13-08T23:59:59+00:00 | eq     |
		| 2016-08-05T08:16          | eq     |
		| 2016-08-                  | eq     |
		| 2016-08-05 08:16:07       | eq     |

Scenario Outline: Appointment retrieve send request and find request using equal to prefix
	Given I create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for "patient1" and make a get request for their appointments with the saved slot start date "slotStartDate" and prefix "<prefix>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain atleast "1" appointment
		And all appointments must have a start element which is populated with a date that equals "slotStartDate"
	Examples:
		| prefix |
		| eq     |
		|        |
		
Scenario Outline: Appointment retrieve send request with date variations and greater than and less than prefix
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for "patient1" and make a get request for their appointments with the date "<startDate>" and prefix "<prefix>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain atleast "1" appointment
	Examples:
		| startDate                 | prefix |
		| 2014                      | gt     |
		| 2014-02                   | gt     |
		| 2014-10-05                | gt     |
		| 2014-05                   | gt     |
		| 2014-05-01T11:08:32       | gt     |
		| 2015-10-23T11:08:32+00:00 | gt     |
		| 2044                      | lt     |
		| 2044-02                   | lt     |
		| 2044-10-05                | lt     |
		| 2044-05                   | lt     |
		| 2044-05-01T11:08:32       | lt     |
		| 2044-10-23T11:08:32+00:00 | lt     |
		| 2014                      | ge     |
		| 2014-02                   | ge     |
		| 2014-10-05                | ge     |
		| 2014-05                   | ge     |
		| 2014-05-01T11:08:32       | ge     |
		| 2015-10-23T11:08:32+00:00 | ge     |
		| 2044                      | le     |
		| 2044-02                   | le     |
		| 2044-10-05                | le     |
		| 2044-05                   | le     |
		| 2044-05-01T11:08:32       | le     |
		| 2044-10-23T11:08:32+00:00 | le     |

Scenario Outline: Appointment retrieve send request with lower start date boundry and start prefix and upper end date boundary and end prefix
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for "patient1" and make a get request for their appointments with lower start date boundry "<startDate>" with prefix "<prefix>" and upper end date boundary "<endDate>" with prefix "<prefix2>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain atleast "1" appointment
	Examples:
		| startDate                 | prefix | endDate                   | prefix2 |
		| 2015                      | gt     | 2018                      | lt      |
		| 2014-02                   | gt     | 2018-07                   | lt      |
		| 2014-10-05                | gt     | 2018-10-05                | lt      |
		| 2014-05                   | gt     | 2044-05-01T11:08:32       | lt      |
		| 2014-05-01T11:08:32       | gt     | 2018-05                   | lt      |
		| 2015-10-23T11:08:32+00:00 | gt     | 2018-10-23T11:08:32+00:00 | lt      |
		| 2014                      | ge     | 2044                      | le      |
		| 2014-02                   | ge     | 2044-02                   | le      |
		| 2014-10-05                | ge     | 2044-10-05                | le      |
		| 2014-05                   | ge     | 2044-05                   | le      |
		| 2014-05-01T11:08:32       | ge     | 2044-05-01T11:08:32       | le      |
		| 2015-10-23T11:08:32+00:00 | ge     | 2044-10-23T11:08:32+00:00 | le      |
		| 2014                      | gt     | 2044                      | le      |
		| 2014-02                   | gt     | 2044-02                   | le      |
		| 2014-10-05                | gt     | 2044-10-05                | le      |
		| 2014-05                   | gt     | 2044-05                   | le      |
		| 2014-05-01T11:08:32       | gt     | 2044-05-01T11:08:32       | le      |
		| 2015-10-23T11:08:32+00:00 | gt     | 2044-10-23T11:08:32+00:00 | le      |
		| 2014                      | ge     | 2044                      | lt      |
		| 2014-02                   | ge     | 2044-02                   | lt      |
		| 2014-10-05                | ge     | 2044-10-05                | lt      |
		| 2014-05                   | ge     | 2044-05                   | lt      |
		| 2014-05-01T11:08:32       | ge     | 2044-05-01T11:08:32       | lt      |
		| 2015-10-23T11:08:32+00:00 | ge     | 2044-10-23T11:08:32+00:00 | lt      |

Scenario Outline: Appointment retrieve send request with upper end date boundary and end prefix and lower start date boundry and start prefix
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for "patient1" and make a get request for their appointments with lower start date boundry "<startDate>" with prefix "<prefix>" and upper end date boundary "<endDate>" with prefix "<prefix2>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain atleast "1" appointment
	Examples:
		| startDate                 | prefix | endDate                   | prefix2 |
		| 2018                      | lt     | 2015                      | gt      |
		| 2018-07                   | lt     | 2014-02                   | gt      |
		| 2018-10-05                | lt     | 2014-10-05                | gt      |
		| 2044-05-01T11:08:32       | lt     | 2014-05                   | gt      |
		| 2018-05                   | lt     | 2014-05-01T11:08:32       | gt      |
		| 2018-10-23T11:08:32+00:00 | lt     | 2015-10-23T11:08:32+00:00 | gt      |
		| 2044                      | le     | 2014                      | ge      |
		| 2044-02                   | le     | 2014-02                   | ge      |
		| 2044-10-05                | le     | 2014-10-05                | ge      |
		| 2044-05                   | le     | 2014-05                   | ge      |
		| 2044-05-01T11:08:32       | le     | 2014-05-01T11:08:32       | ge      |
		| 2044-10-23T11:08:32+00:00 | le     | 2015-10-23T11:08:32+00:00 | ge      |
		| 2044                      | le     | 2014                      | gt      |
		| 2044-02                   | le     | 2014-02                   | gt      |
		| 2044-10-05                | le     | 2014-10-05                | gt      |
		| 2044-05                   | le     | 2014-05                   | gt      |
		| 2044-05-01T11:08:32       | le     | 2014-05-01T11:08:32       | gt      |
		| 2044-10-23T11:08:32+00:00 | le     | 2015-10-23T11:08:32+00:00 | gt      |
		| 2044                      | lt     | 2014                      | ge      |
		| 2044-02                   | lt     | 2014-02                   | ge      |
		| 2044-10-05                | lt     | 2014-10-05                | ge      |
		| 2044-05                   | lt     | 2014-05                   | ge      |
		| 2044-05-01T11:08:32       | lt     | 2014-05-01T11:08:32       | ge      |
		| 2044-10-23T11:08:32+00:00 | lt     | 2015-10-23T11:08:32+00:00 | ge      |

Scenario Outline: Appointment retrieve send request with different upper end date boundary formats and end prefix and different lower start date boundry formats and start prefix
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for "patient1" and make a get request for their appointments with lower start date boundry "<startDate>" with prefix "<prefix>" and upper end date boundary "<endDate>" with prefix "<prefix2>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain atleast "1" appointment
	Examples:
		| startDate                 | prefix | endDate                   | prefix2 |
		| 2015                      | gt     | 2018                      | lt      |
		| 2015                      | gt     | 2018-07                   | lt      |
		| 2015                      | gt     | 2018-10-05                | lt      |
		| 2015                      | gt     | 2044-05-01T11:08:32       | lt      |
		| 2015                      | gt     | 2018-05                   | lt      |
		| 2015                      | gt     | 2018-10-23T11:08:32+00:00 | lt      |
		| 2014-02                   | gt     | 2018                      | lt      |
		| 2014-02                   | gt     | 2018-07                   | lt      |
		| 2014-02                   | gt     | 2018-10-05                | lt      |
		| 2014-02                   | gt     | 2044-05-01T11:08:32       | lt      |
		| 2014-02                   | gt     | 2018-05                   | lt      |
		| 2014-02                   | gt     | 2018-10-23T11:08:32+00:00 | lt      |
		| 2014-10-05                | gt     | 2018                      | lt      |
		| 2014-10-05                | gt     | 2018-07                   | lt      |
		| 2014-10-05                | gt     | 2018-10-05                | lt      |
		| 2014-10-05                | gt     | 2044-05-01T11:08:32       | lt      |
		| 2014-10-05                | gt     | 2018-05                   | lt      |
		| 2014-10-05                | gt     | 2018-10-23T11:08:32+00:00 | lt      |
		| 2014-05-01T11:08:32       | gt     | 2018                      | lt      |
		| 2014-05-01T11:08:32       | gt     | 2018-07                   | lt      |
		| 2014-05-01T11:08:32       | gt     | 2018-10-05                | lt      |
		| 2014-05-01T11:08:32       | gt     | 2044-05-01T11:08:32       | lt      |
		| 2014-05-01T11:08:32       | gt     | 2018-05                   | lt      |
		| 2014-05-01T11:08:32       | gt     | 2018-10-23T11:08:32+00:00 | lt      |
		| 2015-10-23T11:08:32+00:00 | gt     | 2018                      | lt      |
		| 2015-10-23T11:08:32+00:00 | gt     | 2018-07                   | lt      |
		| 2015-10-23T11:08:32+00:00 | gt     | 2018-10-05                | lt      |
		| 2015-10-23T11:08:32+00:00 | gt     | 2044-05-01T11:08:32       | lt      |
		| 2015-10-23T11:08:32+00:00 | gt     | 2018-05                   | lt      |
	
Scenario Outline: Appointment retrieve send request with start date and invalid start prefix and end date and invalid end prefix
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I search for "patient1" and make a get request for their appointments with lower start date boundry "<startDate>" with prefix "<prefix>" and upper end date boundary "<endDate>" with prefix "<prefix2>"
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource
	Examples:
		| startDate                 | prefix | endDate                   | prefix2 |
		| 2015                      | lf     | 2018                      | lt      |
		| 2014-02                   | lt     | 2018-07                   | l2      |
		| 2014-10-05                | g1     | 2018-10-05                | gt      |
		| 2014-05                   | gt     | 2044-05-01T11:08:32       | g       |
		| 2014-05-01T11:08:32       | tt     | 2018-05                   | lu      |
		| 2015-10-23T11:08:32+00:00 | dd     | 2018-10-23T11:08:32+00:00 | zz      |
		| 2014                      | gt     | 2044                      | gt      |
		| 2014-02                   | gt     | 2044-02                   | gt      |
		| 2014-10-05                | gt     | 2044-10-05                | gt      |
		| 2014-05                   | gt     | 2044-05                   | gt      |
		| 2014-05-01T11:08:32       | gt     | 2044-05-01T11:08:32       | gt      |
		| 2015-10-23T11:08:32+00:00 | gt     | 2044-10-23T11:08:32+00:00 | gt      |
		| 2014                      | ge     | 2044                      | ge      |
		| 2014-02                   | ge     | 2044-02                   | ge      |
		| 2014-10-05                | ge     | 2044-10-05                | ge      |
		| 2014-05                   | ge     | 2044-05                   | ge      |
		| 2014-05-01T11:08:32       | ge     | 2044-05-01T11:08:32       | ge      |
		| 2014                      | lt     | 2044                      | lt      |
		| 2014-02                   | lt     | 2044-02                   | lt      |
		| 2014-10-05                | lt     | 2044-10-05                | lt      |
		| 2014-05                   | lt     | 2044-05                   | lt      |
		| 2014-05-01T11:08:32       | lt     | 2044-05-01T11:08:32       | lt      |
		| 2014                      | le     | 2044                      | le      |
		| 2014-02                   | le     | 2044-02                   | le      |
		| 2014-10-05                | le     | 2044-10-05                | le      |
		| 2014-05                   | le     | 2044-05                   | le      |
		| 2014-05-01T11:08:32       | le     | 2044-05-01T11:08:32       | le      |
		| 2015-10-23T11:08:32+00:00 | le     | 2044-10-23T11:08:32+00:00 | le      |

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
		| interactionId                                                     |
		| urn:nhs:names:services:gpconnect:fhir:rest:search:organization    |
		| urn:nh4s:names:se34rv4ices4:gpconnect3:fhir:re23st:seawwwwwwwww   |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		|                                                                   |
		| null                                                              |
	
Scenario Outline: Appointment retrieve accept header and _format parameter
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain atleast "1" appointment
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
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
		And I set the Accept header to "<Header>"
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain atleast "1" appointment
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Appointment retrieve _format parameter only
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
		And I add the parameter "_format" with the value "<Parameter>"
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain atleast "1" appointment
	Examples:
		| Parameter             | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario: Appointment retrieve appointment which contains all mandatory resources
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the bundle of appointments should all contain a single status element
		And the bundle of appointments should all contain a single start element
		And the bundle of appointments should all contain a single end element
		And the bundle of appointments should all contain at least one slot reference
		And the bundle of appointments should all contain one participant which is a patient and one which is a practitioner

Scenario: Appointment retrieve bundle resource must contain participant with type or actor present
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
	Then if appointment is present the single or multiple participant must contain a type or actor

Scenario: Appointment retrieve bundle valid resources returned in the response
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the practitioner resource returned in the appointments bundle is present
		And the patient resource returned in the appointments bundle is present

Scenario: Appointment retrieve bundle contains appointment with identifer with correct system and value
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And if the appointment resource contains an identifier it contains a valid system and value

Scenario: Appointment retrieve appointment response should contain meta data profile and version id
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the bundle of appointments should contain meta data profile and version id
	
Scenario: Appointment retrieve returned resources must contain coding with valid system code and display
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And if the bundle of appointment response resource contains a reason element and coding the codings must be one of the three allowed with system code and display elements
	
Scenario: Appointment retrieve bundle contains appointment contact method
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
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
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And all appointments must have an start element which is populated with a valid date
		And all appointments must have an end element which is populated vith a valid date

Scenario: Appointment retrieve JWT requesting scope claim should reflect the operation being performed
	Given I find or create "1" appointments for patient "patient1" at organization "ORG1" and save bundle of appintment resources to "Patient1AppointmentsInBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
	When I search for "patient1" and make a get request for their appointments
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Appointment retrieve book appointment and search for the appointment and compare the results
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "storedPatient1"
	Given I perform the getSchedule operation for organization "ORG1" and store the returned bundle resources against key "getScheduleResponseBundle"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:create:appointment" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I create an appointment for patient "storedPatient1" called "<appointment>" from schedule "getScheduleResponseBundle"
	When I book the appointment called "<appointment>"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for patient "storedPatient1" and search for the most recently booked appointment "<appointment>" using the stored startDate from the last booked appointment as a search parameter
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain atleast "1" appointment
		And the returned appointment start date should match "<appointment>" start Date
		And the returned appointment end date should match "<appointment>" end date
		And the returned appointment patient reference should match "<appointment>" patient reference
		And the returned appointment slot reference should match "<appointment>" slot reference
		And the returned appointment participant status should match "<appointment>" participant status
	Examples: 
		| appointment  |
		| Appointment1 |
		| Appointment2 |
		| Appointment3 |
		| Appointment4 |
		| Appointment5 |
		| Appointment6 |
		| Appointment7 |

Scenario Outline: Appointment retrieve JWT patient type request invalid
	Given I am using the default server
		And I perform a patient search for patient "patient1" and store the first returned resources against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "registerPatient"
		And I set the JWT requested scope to "<JWTType>"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I search for "registerPatient" from the list of patients and make a get request for their appointments
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| JWTType              |
		| organization/*.read  |
		| organization/*.write |
		| patient/*.write      |

Scenario: Appointment retrieve JWT patient type request valid
	Given I am using the default server
		And I perform a patient search for patient "patient1" and store the first returned resources against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "registerPatient"
		And I set the JWT requested scope to "patient/*.read"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for "registerPatient" from the list of patients and make a get request for their appointments
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"

Scenario: Appointment retrieve JWT patient reference must match payload patient nhs number
	Given I am using the default server
		And I perform a patient search for patient "patient1" and store the first returned resources against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to config patient "patient2"
		And I set the JWT requested scope to "patient/*.read"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments" interaction
	When I search for "registerPatient" from the list of patients and make a get request for their appointments
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Conformance profile supports the search appointment operation
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the conformance profile should contain the "Appointment" resource with a "read" interaction