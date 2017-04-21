@Appointment
Feature: SearchForFreeSlots
	
Background:
	Given I have the test ods codes
	
Scenario Outline: I successfully perform a gpc.getschedule operation
	Given I am using the default server
		And I search for the organization "<Organization>" on the providers system and save the first response to "<Organization>"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with a start date of todays and an end date "<DaysRange>" days later
	When I send a gpc.getschedule operation for the organization stored as "<Organization>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| Organization | DaysRange |
		| ORG1         | 14        |
		| ORG2         | 0         |
		| ORG3         | 8         |

Scenario Outline: I send an invalid date range to the getSchedule operation and should get an error
	Given I am using the default server
		And I search for the organization "ORG1" on the providers system and save the first response to "ORG1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with a start date of todays and an end date "<DaysRange>" days later
	When I send a gpc.getschedule operation for the organization stored as "ORG1"
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
	Examples:
		| DaysRange |
		| 15        |
		| 31        |
		| -1        |

Scenario Outline: I send a request to the getSchedule operation with invalid organization logic id
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with a start date of todays and an end date "6" days later
	When I send a gpc.getschedule operation for the organization with locical id "<LogicalIdentifier>"
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
	Examples: 
		| LogicalIdentifier         |
		|                           |
		| InvalidLogicalID123456789 |

Scenario Outline: getSchedule failure due to invalid interactionId
	Given I am using the default server
		And I search for the organization "ORG1" on the providers system and save the first response to "ORG1"
	Given I am using the default server
		And I am performing the "<InteractionId>" interaction
		And I add period request parameter with a start date of todays and an end date "8" days later
	When I send a gpc.getschedule operation for the organization stored as "ORG1"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| InteractionId                                                     |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		| InvalidInteractionId                                              |
		|                                                                   |
		
Scenario Outline: getSchedule failure due to missing header
	Given I am using the default server
		And I search for the organization "ORG2" on the providers system and save the first response to "ORG2"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with a start date of todays and an end date "7" days later
		And I do not send header "<Header>"
	When I send a gpc.getschedule operation for the organization stored as "ORG2"
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

Scenario: I try to getSchedule without a time period parameter
	Given I am using the default server
		And I search for the organization "ORG2" on the providers system and save the first response to "ORG2"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
	When I send a gpc.getschedule operation for the organization stored as "ORG2"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: I try to getSchedule with multiple time period parameter
	Given I am using the default server
		And I search for the organization "ORG2" on the providers system and save the first response to "ORG2"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with a start date of todays and an end date "7" days later
		And I add period request parameter with a start date of todays and an end date "9" days later
	When I send a gpc.getschedule operation for the organization stored as "ORG2"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"	

Scenario: I try to getSchedule with a time period containing only a start date
	Given I am using the default server
		And I search for the organization "ORG2" on the providers system and save the first response to "ORG2"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with only a start date
	When I send a gpc.getschedule operation for the organization stored as "ORG2"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"	

Scenario: I try to getSchedule with a time period containing only an end date
	Given I am using the default server
		And I search for the organization "ORG2" on the providers system and save the first response to "ORG2"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with only an end date
	When I send a gpc.getschedule operation for the organization stored as "ORG2"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"	

Scenario Outline: I perform a getSchedule with invalid end date and or start date parameters
	Given I am using the default server
		And I search for the organization "ORG1" on the providers system and save the first response to "ORG1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with start date "<StartDate>" and end date "<EndDate>"
	When I send a gpc.getschedule operation for the organization stored as "ORG1"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"	
	Examples: 
		| StartDate        | EndDate          |
		| invalidStartDate | 2017-02-28       |
		| 2017-02-26       | invalidEnddate   |
		| invalidStartDate | invalidEnddate   |
		| 2017-02-26       |                  |
		|                  | 2017-02-28       |
		| 2017-12-29T08:30 | 2017-12-29T18:22 |

Scenario Outline: I perform a getSchedule with valid partial dateTime strings
	Given I am using the default server
		And I search for the organization "ORG1" on the providers system and save the first response to "ORG1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with start date "<StartDate>" and end date "<EndDate>"
	When I send a gpc.getschedule operation for the organization stored as "ORG1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should include slot resources
	Examples: 
		| StartDate                 | EndDate                   |
		| 2017-02-26                | 2017-02-28                |
		| 2017                      | 2017-01-03                |
		| 2017-12-28                | 2017                      |
		| 2017-12-29T09:35:15+01:00 | 2017-12-29                |
		| 2017-12-29                | 2017-12-29T14:55:34+01:00 |

Scenario: I try to getSchedule with multiple parameters of which some are invalid
	Given I am using the default server
		And I search for the organization "ORG2" on the providers system and save the first response to "ORG2"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with a start date of todays and an end date "7" days later
		And I am requesting the "SUM" care record section
	When I send a gpc.getschedule operation for the organization stored as "ORG2"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"	