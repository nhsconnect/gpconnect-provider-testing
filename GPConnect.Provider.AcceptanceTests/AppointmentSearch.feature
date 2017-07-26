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
		And the Bundle should contain no Appointments

Scenario Outline: Appointment retrieve success valid id where single appointment resource should be returned
	Given I create an Appointment for Patient "<patient>" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain a minimum of "1" Appointments
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
		And the Bundle should contain a minimum of "<numberOfAppointments>" Appointments
	Examples:
		| patient  | numberOfAppointments |
		| patient4 | 2                    |
		| patient5 | 4                    |
		| patient6 | 3                    |

Scenario Outline: Appointment retrieve fail due to invalid patient logical id
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the request URL to "/Patient/<id>/Appointment"
	When I make the "AppointmentSearch" request
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
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add a query parameter to the Request URL with Prefix "<prefix>" for Start "<startDate>"
	When I make the "AppointmentSearch" request
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
	Given I create "1" Appointments for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add a query parameter to the Request URL with Prefix "<prefix>" for the Created Appointment Start
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain a minimum of "1" Appointments
		And the Appointment Start should equal the Created Appointment Start
		And the Appointment Start and End Dates should equal the Created Appointment Start and End Dates
		And the Appointment Participants should be equal to the Created Appointment Participants
	Examples:
		| prefix |
		| eq     |
		|        |

Scenario Outline: Appointment retrieve send request with date variations and greater than and less than prefix
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add a query parameter to the Request URL with Prefix "<prefix>" for Start "<startDate>"
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain a minimum of "1" Appointments
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
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add a query parameter to the Request URL with Prefix "<prefix>" for Start "<startDate>" and Prefix "<prefix2>" for End "<endDate>"
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain a minimum of "1" Appointments
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
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add a query parameter to the Request URL with Prefix "<prefix>" for Start "<startDate>" and Prefix "<prefix2>" for End "<endDate>"
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain a minimum of "1" Appointments
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
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add a query parameter to the Request URL with Prefix "<prefix>" for Start "<startDate>" and Prefix "<prefix2>" for End "<endDate>"
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain a minimum of "1" Appointments
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
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add a query parameter to the Request URL with Prefix "<prefix>" for Start "<startDate>" and Prefix "<prefix2>" for End "<endDate>"
	When I make the "AppointmentSearch" request
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
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I do not send header "<Header>"
	When I make the "AppointmentSearch" request
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
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I am performing the "<interactionId>" interaction
	When I make the "AppointmentSearch" request
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
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain a minimum of "1" Appointments
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/json+fhir | application/json+fhir | JSON       |
		| application/json+fhir | application/xml+fhir  | XML        |
		| application/xml+fhir  | application/json+fhir | JSON       |
		| application/xml+fhir  | application/xml+fhir  | XML        |

Scenario Outline: Appointment retrieve accept header
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Accept header to "<Header>"
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain a minimum of "1" Appointments
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Appointment retrieve _format parameter only
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain a minimum of "1" Appointments
	Examples:
		| Parameter             | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario: Appointment retrieve appointment which contains all mandatory resources
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the Appointment Status should be valid
		And the Appointment Start should be valid
		And the Appointment End should be valid
		And the Appointment Slots should be valid
		And the Appointment Participants should be valid and resolvable

Scenario: Appointment retrieve bundle resource must contain participant with type or actor present
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the Appointment Participant Type and Actor should be valid

Scenario: Appointment retrieve bundle valid resources returned in the response
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Appointment Participants should be valid and resolvable

Scenario: Appointment retrieve bundle contains appointment with identifer with correct system and value
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Appointment Identifiers should be valid

Scenario: Appointment retrieve appointment response should contain meta data profile and version id
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Appointment Metadata should be valid
	
Scenario: Appointment retrieve returned resources must contain coding with valid system code and display
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Appointment Reason should be valid
	
Scenario: Appointment retrieve bundle contains appointment contact method
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Appointment Category Extension should be valid
		And the Appointment Booking Method Extension should be valid
		And the Appointment Contact Method Extension should be valid
		And the Appointment Cancellation Reason Extension should be valid

Scenario: Appointment retrieve bundle contains valid start and end dates
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
	When I make the "AppointmentSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Appointment Start should be valid
		And the Appointment End should be valid

Scenario: Appointment retrieve JWT requesting scope claim should reflect the operation being performed
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the JWT requested record NHS number to config patient "patient2"
	When I make the "AppointmentSearch" request
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Appointment retrieve JWT patient type request invalid
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the JWT requested scope to "<JWTType>"
	When I make the "AppointmentSearch" request
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| JWTType              |
		| organization/*.read  |
		| organization/*.write |
		| patient/*.write      |

Scenario: Appointment retrieve JWT patient reference must match payload patient nhs number
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
	Given I configure the default "AppointmentSearch" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the JWT requested record NHS number to config patient "patient2"
	When I make the "AppointmentSearch" request
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Conformance profile supports the search appointment operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the conformance profile should contain the "Appointment" resource with a "search-type" interaction
