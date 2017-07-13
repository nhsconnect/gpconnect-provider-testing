﻿@getschedule
Feature: GPCGetSchedule
	
Scenario Outline: I successfully perform a gpc.getschedule operation
	Given I get the Organization for Organization Code "<Organization>"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Time Period parameter with Start Date today and End Date in "<Days>" days
	When I make the "GpcGetSchedule" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| Organization | Days |
		| ORG1         | 14   |
		| ORG2         | 0    |
		| ORG3         | 8    |

Scenario Outline: I send an invalid date range to the getSchedule operation and should get an error
	Given I am using the default server
		And I search for the organization "ORG1" on the providers system and save the first response to "ORG1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with a start date of today and an end date "<DaysRange>" days later
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
		And I add period request parameter with a start date of today and an end date "6" days later
	When I send a gpc.getschedule operation for the organization with locical id "<LogicalIdentifier>"
	Then the response status code should be "404"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "ORGANISATION_NOT_FOUND"
	Examples:
		| LogicalIdentifier         |
		|                           |
		| InvalidLogicalID123456789 |

Scenario Outline: getSchedule failure due to invalid interactionId
	Given I am using the default server
		And I search for the organization "ORG1" on the providers system and save the first response to "ORG1"
	Given I am using the default server
		And I am performing the "<InteractionId>" interaction
		And I add period request parameter with a start date of today and an end date "8" days later
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
		And I add period request parameter with a start date of today and an end date "7" days later
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
		And I add period request parameter with a start date of today and an end date "7" days later
		And I add period request parameter with a start date of today and an end date "9" days later
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
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"	

Scenario: I try to getSchedule with a time period containing only an end date
	Given I am using the default server
		And I search for the organization "ORG2" on the providers system and save the first response to "ORG2"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with only an end date
	When I send a gpc.getschedule operation for the organization stored as "ORG2"
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"	

Scenario Outline: I perform a getSchedule with invalid end date and or start date parameters
	Given I am using the default server
		And I search for the organization "ORG1" on the providers system and save the first response to "ORG1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with start date "<StartDate>" and end date "<EndDate>"
	When I send a gpc.getschedule operation for the organization stored as "ORG1"
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"	
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
		And I add period request parameter with start date format "<StartDate>" and end date format "<EndDate>"
	When I send a gpc.getschedule operation for the organization stored as "ORG1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| StartDate           | EndDate             |
		| yyyy-MM-dd          | yyyy-MM-dd          |
		| yyyy                | yyyy                |
		| yyyy-MM-ddTHH:mm:ss | yyyy-MM-ddTHH:mm:ss |
		| yyyy-MM-dd          | yyyy-MM-ddTHH:mm:ss |
		| yyyy-MM-ddTHH:mm:ss | yyyy-MM-dd          |
		| yyyy-MM             | yyyy-MM             |

Scenario Outline: I perform a getSchedule with in-valid partial dateTime strings
	Given I am using the default server
		And I search for the organization "ORG1" on the providers system and save the first response to "ORG1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with start date format "<StartDate>" and end date format "<EndDate>"
	When I send a gpc.getschedule operation for the organization stored as "ORG1"
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"	
		Examples: 
		| StartDate           | EndDate             |
		| yyyy                | yyyy-MM-dd          |
		| yyyy-MM-dd          | yyyy                |
		| yyyy                | yyyy-MM             |
		| yyyy-MM             | yyyy-MM-dd          |
	
Scenario: I try to getSchedule with multiple parameters of which some are invalid
	Given I am using the default server
		And I search for the organization "ORG2" on the providers system and save the first response to "ORG2"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with a start date of today and an end date "7" days later
		And I am requesting the "SUM" care record section
	When I send a gpc.getschedule operation for the organization stored as "ORG2"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"	

Scenario: I try to getSchedule with multiple parameters of which some are invalid different order
	Given I am using the default server
		And I search for the organization "ORG2" on the providers system and save the first response to "ORG2"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I am requesting the "SUM" care record section
		And I add period request parameter with a start date of today and an end date "7" days later
	When I send a gpc.getschedule operation for the organization stored as "ORG2"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"	

Scenario Outline: I successfully perform a gpc.getschedule operation and check the response bundle resources returned contains required meta data
	Given I am using the default server
		And I search for the organization "<Organization>" on the providers system and save the first response to "<Organization>"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with a start date of today and an end date "<DaysRange>" days later
	When I send a gpc.getschedule operation for the organization stored as "<Organization>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the bundle resources should contain required meta data elements
	Examples:
		| Organization | DaysRange |
		| ORG1         | 14        |
		| ORG2         | 8         |
		| ORG3         | 8         |

Scenario Outline: I successfully perform a gpc.getschedule operation and check the slot resources returned are valid
	Given I am using the default server
		And I search for the organization "<Organization>" on the providers system and save the first response to "<Organization>"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with a start date of today and an end date "<DaysRange>" days later
	When I send a gpc.getschedule operation for the organization stored as "<Organization>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should include slot resources
		And the slots resources within the response bundle should be free
		And the slot resources in the response bundle should contain meta data information
		And the slot resources can contain a maximum of one identifier with a populated value
	Examples:
		| Organization | DaysRange |
		| ORG1         | 14        |
		| ORG2         | 8         |

Scenario Outline: I successfully perform a gpc.getschedule operation and check the slot references to schedule resource can be resolved in bundle
	Given I am using the default server
		And I search for the organization "<Organization>" on the providers system and save the first response to "<Organization>"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with a start date of today and an end date "<DaysRange>" days later
	When I send a gpc.getschedule operation for the organization stored as "<Organization>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should include slot resources
		And the schedule reference within the slots resources should be resolvable in the response bundle
	Examples:
		| Organization | DaysRange |
		| ORG1         | 14        |
		| ORG2         | 13        |

Scenario Outline: I successfully perform a gpc.getschedule operation using various content types XML and JSON in Accept header
	Given I am using the default server
		And I search for the organization "ORG1" on the providers system and save the first response to "ORG1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I set the request content type to "<RequestContentType>"
		And I set the Accept header to "<AcceptHeaderValue>"
		And I add period request parameter with a start date of today and an end date "8" days later
	When I send a gpc.getschedule operation for the organization stored as "ORG1"
	Then the response status code should indicate success
		And the response body should be FHIR <ResponseShouldBe>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should include slot resources
		And the schedule reference within the slots resources should be resolvable in the response bundle
	Examples:
		| RequestContentType    | AcceptHeaderValue     | ResponseShouldBe |
		| application/xml+fhir  | application/xml+fhir  | XML              |
		| application/json+fhir | application/json+fhir | JSON             |
		| application/xml+fhir  | application/json+fhir | JSON             |
		| application/json+fhir | application/xml+fhir  | XML              |

Scenario Outline: I successfully perform a gpc.getschedule operation using various content types XML and JSON in format parameter
	Given I am using the default server
		And I search for the organization "ORG1" on the providers system and save the first response to "ORG1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I set the request content type to "<RequestContentType>"
		And I add the parameter "_format" with the value "<FormatParameterValue>"
		And I add period request parameter with a start date of today and an end date "8" days later
	When I send a gpc.getschedule operation for the organization stored as "ORG1"
	Then the response status code should indicate success
		And the response body should be FHIR <ResponseShouldBe>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should include slot resources
		And the schedule reference within the slots resources should be resolvable in the response bundle
	Examples:
		| RequestContentType    | FormatParameterValue  | ResponseShouldBe |
		| application/xml+fhir  | application/xml+fhir  | XML              |
		| application/json+fhir | application/json+fhir | JSON             |
		| application/xml+fhir  | application/json+fhir | JSON             |
		| application/json+fhir | application/xml+fhir  | XML              |

Scenario Outline: I successfully perform a gpc.getschedule operation using various content types XML and JSON in the Accept Header and format parameter
	Given I am using the default server
		And I search for the organization "ORG1" on the providers system and save the first response to "ORG1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I set the request content type to "<RequestContentType>"
		And I set the Accept header to "<AcceptHeaderValue>"
		And I add the parameter "_format" with the value "<FormatParameterValue>"
		And I add period request parameter with a start date of today and an end date "8" days later
	When I send a gpc.getschedule operation for the organization stored as "ORG1"
	Then the response status code should indicate success
		And the response body should be FHIR <ResponseShouldBe>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should include slot resources
		And the schedule reference within the slots resources should be resolvable in the response bundle
	Examples:
		| RequestContentType    | AcceptHeaderValue     | FormatParameterValue  | ResponseShouldBe |
		| application/xml+fhir  | application/xml+fhir  | application/xml+fhir  | XML              |
		| application/json+fhir | application/json+fhir | application/json+fhir | JSON             |
		| application/xml+fhir  | application/json+fhir | application/json+fhir | JSON             |
		| application/json+fhir | application/xml+fhir  | application/xml+fhir  | XML              |
		| application/xml+fhir  | application/json+fhir | application/xml+fhir  | XML              |
		| application/json+fhir | application/xml+fhir  | application/json+fhir | JSON             |
		| application/xml+fhir  | application/xml+fhir  | application/json+fhir | JSON             |
		| application/json+fhir | application/json+fhir | application/xml+fhir  | XML              |

Scenario: Send a request to an invalid endpoint for the gpc.getschedule operation
	Given I am using the default server
		And I search for the organization "ORG1" on the providers system and save the first response to "ORG1"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with a start date of today and an end date "8" days later
	When I send a gpc.getschedule operation for the organization stored as "ORG1" to the wrong endpoint
	Then the response status code should be "404"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "REFERENCE_NOT_FOUND"

Scenario Outline: I successfully perform a gpc.getschedule operation and check the included schedule resources returned are valid
	Given I am using the default server
		And I search for the organization "<Organization>" on the providers system and save the first response to "<Organization>"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with a start date of today and an end date "<DaysRange>" days later
	When I send a gpc.getschedule operation for the organization stored as "<Organization>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should include slot resources
		And the schedule resources in the response bundle should contain meta data information
		And the schedule resources in the response bundle should contain an actor which references a location within the response bundle
		And the schedule resources can contain a single identifier but must have a value if present
		And the schedule resources can contain a planningHorizon but it must contain a valid start date
		And the schedule resources can contain a single type element
		And the schedule resources can contain extensions which references practitioner resources within the bundle
	Examples:
		| Organization | DaysRange |
		| ORG1         | 13        |
		| ORG2         | 11        |

Scenario Outline: I successfully perform a gpc.getschedule operation and check the included practitioner resources returned are valid
	Given I am using the default server
		And I search for the organization "<Organization>" on the providers system and save the first response to "<Organization>"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with a start date of today and an end date "<DaysRange>" days later
	When I send a gpc.getschedule operation for the organization stored as "<Organization>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should include slot resources
		And the Practitioner Metadata should be valid
		And the Practitioner SDS User Identifier should be valid
		And the Practitioner Identifiers should be valid fixed values
		And the Practitioner Name should be valid
		And the Practitioner PractitionerRoles Roles should be valid
		And the Practitioner PractitionerRoles ManagingOrganization should be valid and resolvable
		And the Practitioner should exclude disallowed elements
		And the Practitioner Communication should be valid
	Examples:
		| Organization | DaysRange |
		| ORG1         | 10        |
		| ORG2         | 13        |

Scenario Outline: I successfully perform a gpc.getschedule operation and check the included organization resources returned are valid
	Given I am using the default server
		And I search for the organization "<Organization>" on the providers system and save the first response to "<Organization>"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with a start date of today and an end date "<DaysRange>" days later
	When I send a gpc.getschedule operation for the organization stored as "<Organization>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should include slot resources
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the response bundle Organization entries should contain a maximum of 1 http://fhir.nhs.net/Id/ods-organization-code system identifier
		And the Organization Identifiers should be valid
		And the Organization Type should be valid
		And the Organization PartOf Organization should be referenced in the Bundle
	Examples:
		| Organization | DaysRange |
		| ORG1         | 13        |
		| ORG2         | 12        |

Scenario Outline: I successfully perform a gpc.getschedule operation and check the included location resources returned are valid
	Given I am using the default server
		And I search for the organization "<Organization>" on the providers system and save the first response to "<Organization>"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with a start date of today and an end date "<DaysRange>" days later
	When I send a gpc.getschedule operation for the organization stored as "<Organization>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should include slot resources
		And the Location Metadata should be valid
		And the Location Identifier should be valid
		And the Location Name should be valid
		And the Location Type should be valid
		And the Location Physical Type should be valid
		And the Location PartOf Location should be valid
		And the Location Managing Organization should be valid
	Examples:
		| Organization | DaysRange |
		| ORG1         | 13        |
		| ORG2         | 13        |

Scenario: Conformance profile supports the gpc.getSchedule operation
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the conformance profile should contain the "gpc.getschedule" operation

@Manual
@ignore
# This is tested by "I perform a getSchedule with valid partial dateTime strings" but would benefit from additional manual testing
Scenario: I successfully perform a gpc.getschedule operation where the start date in the request is after the start date of a slot but before the end date of the slot
	Given I search for free slots at organization "ORG1"
		And I search for slots using a date range I know there is a free slot which has a start date before the request parameter start date but the slot end date is after the requeste parameter start date
	When I perform the gpc.getSchedule operation
	Then the response should be successful and return a FHIR Bundle
	Then the slot which straddles the request parameter start date should appear in the response bundle.

@Manual
@ignore
# This is tested by "I perform a getSchedule with valid partial dateTime strings" but would benefit from additional manual testing
Scenario: I successfully perform a gpc.getschedule operation where the end date parameter in the request is after the start date of a slot but before the end date of the slot
	Given I search for free slots at organization "ORG1"
		And I search for slots using a date range I know there is a free slot which has a start date before the request parameter end date but the slot end date is after the requeste parameter end date
	When I perform the gpc.getSchedule operation
	Then the response should be successful and return a FHIR Bundle
	Then the slot which straddles the request parameter start date should appear in the response bundle.