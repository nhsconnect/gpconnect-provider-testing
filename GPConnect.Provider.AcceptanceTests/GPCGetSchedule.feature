@getschedule
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
	Given I get the Organization for Organization Code "ORG1"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Time Period parameter with Start Date today and End Date in "<Days>" days
	When I make the "GpcGetSchedule" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
	Examples:
		| Days |
		| 15   |
		| 31   |
		| -1   |

Scenario Outline: I send a request to the getSchedule operation with invalid organization logic id
	Given I get the Organization for Organization Code "ORG1"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Time Period parameter with Start Date today and End Date in "6" days	
		And I set the Read Operation logical identifier used in the request to "<LogicalIdentifier>"
	When I make the "GpcGetSchedule" request
	Then the response status code should be "404"
		And the response should be a OperationOutcome resource with error code "ORGANISATION_NOT_FOUND"
	Examples:
		| LogicalIdentifier         |
		|                           |
		| InvalidLogicalID123456789 |

Scenario Outline: getSchedule failure due to invalid interactionId
	Given I get the Organization for Organization Code "ORG1"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Time Period parameter with Start Date today and End Date in "6" days	
		And I set the Interaction Id header to "<InteractionId>"
	When I make the "GpcGetSchedule" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| InteractionId                                                     |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		| InvalidInteractionId                                              |
		|                                                                   |

Scenario Outline: getSchedule failure due to missing header
	Given I get the Organization for Organization Code "ORG2"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Time Period parameter with Start Date today and End Date in "6" days	
		And I do not send header "<Header>"
	When I make the "GpcGetSchedule" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Header            |
		| Ssp-TraceID       |
		| Ssp-From          |
		| Ssp-To            |
		| Ssp-InteractionId |
		| Authorization     |

Scenario: I try to getSchedule without a time period parameter
	Given I get the Organization for Organization Code "ORG2"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
	When I make the "GpcGetSchedule" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: I try to getSchedule with multiple time period parameter
	Given I get the Organization for Organization Code "ORG2"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Time Period parameter with Start Date today and End Date in "6" days	
		And I add a Time Period parameter with Start Date today and End Date in "7" days	
	When I make the "GpcGetSchedule" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"	

Scenario: I try to getSchedule with a time period containing only a start date
	Given I get the Organization for Organization Code "ORG2"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Time Period parameter with Start Date only
	When I make the "GpcGetSchedule" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"	

Scenario: I try to getSchedule with a time period containing only an end date
	Given I get the Organization for Organization Code "ORG2"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Time Period parameter with End Date only
	When I make the "GpcGetSchedule" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"	

Scenario Outline: I perform a getSchedule with invalid end date and or start date parameters
	Given I get the Organization for Organization Code "ORG1"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Time Period parameter with "<StartDate>" and "<EndDate>"
	When I make the "GpcGetSchedule" request
	Then the response status code should be "422"
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
	Given I get the Organization for Organization Code "ORG1"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Time Period parameter with Start Date format "<StartDate>" and End Date format "<EndDate>" 
	When I make the "GpcGetSchedule" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| StartDate           | EndDate             |
		| yyyy-MM-dd          | yyyy-MM-dd          |
		| yyyy                | yyyy                |
		| yyyy-MM-ddTHH:mm:ss | yyyy-MM-ddTHH:mm:ss |
		| yyyy-MM-dd          | yyyy-MM-ddTHH:mm:ss |
		| yyyy-MM-ddTHH:mm:ss | yyyy-MM-dd          |


Scenario Outline: I perform a getSchedule with in-valid partial dateTime strings
	Given I get the Organization for Organization Code "ORG1"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Time Period parameter with Start Date format "<StartDate>" and End Date format "<EndDate>" 
	When I make the "GpcGetSchedule" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"	
		Examples: 
		| StartDate           | EndDate             |
		| yyyy                | yyyy-MM-dd          |
		| yyyy-MM-dd          | yyyy                |
		| yyyy                | yyyy-MM             |
		| yyyy-MM             | yyyy-MM-dd          |
	
Scenario: I try to getSchedule with multiple parameters of which some are invalid
	Given I get the Organization for Organization Code "ORG2"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Time Period parameter with Start Date today and End Date in "6" days	
		And I add a Record Section parameter for "SUM"
	When I make the "GpcGetSchedule" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"	

Scenario: I try to getSchedule with multiple parameters of which some are invalid different order
	Given I get the Organization for Organization Code "ORG2"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Record Section parameter for "SUM"
		And I add a Time Period parameter with Start Date today and End Date in "6" days	
	When I make the "GpcGetSchedule" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: I successfully perform a gpc.getschedule operation and check the response bundle resources returned contains required meta data
	Given I get the Organization for Organization Code "<Organization>"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Time Period parameter with Start Date today and End Date in "<Days>" days	
	When I make the "GpcGetSchedule" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Schedule Bundle Metadata should be valid
	Examples:
		| Organization | Days |
		| ORG1         | 14   |
		| ORG2         | 8    |
		| ORG3         | 8    |

Scenario Outline: I successfully perform a gpc.getschedule operation and check the slot resources returned are valid
	Given I get the Organization for Organization Code "<Organization>"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Time Period parameter with Start Date today and End Date in "<Days>" days	
	When I make the "GpcGetSchedule" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain Slots
		And the Slot FreeBusyType should be Free
		And the Slot Metadata should be valid
		And the Slot Identifiers should be valid
	Examples:
		| Organization | Days |
		| ORG1         | 14   |
		| ORG2         | 8    |

Scenario Outline: I successfully perform a gpc.getschedule operation and check the slot references to schedule resource can be resolved in bundle
	Given I get the Organization for Organization Code "<Organization>"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Time Period parameter with Start Date today and End Date in "<DaysRange>" days	
	When I make the "GpcGetSchedule" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain Slots
		And the Slot Schedule should be referenced in the Bundle
	Examples:
		| Organization | DaysRange |
		| ORG1         | 14        |
		| ORG2         | 13        |

Scenario Outline: I successfully perform a gpc.getschedule operation using various content types XML and JSON in Accept header
	Given I get the Organization for Organization Code "ORG1"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I set the request content type to "<RequestContentType>"
		And I set the Accept header to "<AcceptHeaderValue>"
		And I add a Time Period parameter with Start Date today and End Date in "8" days	
	When I make the "GpcGetSchedule" request
	Then the response status code should indicate success
		And the response body should be FHIR <ResponseShouldBe>
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain Slots
		And the Slot Schedule should be referenced in the Bundle
	Examples:
		| RequestContentType    | AcceptHeaderValue     | ResponseShouldBe |
		| application/xml+fhir  | application/xml+fhir  | XML              |
		| application/json+fhir | application/json+fhir | JSON             |
		| application/xml+fhir  | application/json+fhir | JSON             |
		| application/json+fhir | application/xml+fhir  | XML              |

Scenario Outline: I successfully perform a gpc.getschedule operation using various content types XML and JSON in format parameter
	Given I get the Organization for Organization Code "ORG1"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I set the request content type to "<RequestContentType>"
		And I add a Format parameter with the Value "<FormatParameterValue>"
		And I add a Time Period parameter with Start Date today and End Date in "8" days	
	When I make the "GpcGetSchedule" request
	Then the response status code should indicate success
		And the response body should be FHIR <ResponseShouldBe>
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain Slots
		And the Slot Schedule should be referenced in the Bundle
	Examples:
		| RequestContentType    | FormatParameterValue  | ResponseShouldBe |
		| application/xml+fhir  | application/xml+fhir  | XML              |
		| application/json+fhir | application/json+fhir | JSON             |
		| application/xml+fhir  | application/json+fhir | JSON             |
		| application/json+fhir | application/xml+fhir  | XML              |

Scenario Outline: I successfully perform a gpc.getschedule operation using various content types XML and JSON in the Accept Header and format parameter
	Given I get the Organization for Organization Code "ORG1"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I set the request content type to "<RequestContentType>"
		And I set the Accept header to "<AcceptHeaderValue>"
		And I add a Format parameter with the Value "<FormatParameterValue>"
		And I add a Time Period parameter with Start Date today and End Date in "8" days	
	When I make the "GpcGetSchedule" request
	Then the response status code should indicate success
		And the response body should be FHIR <ResponseShouldBe>
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain Slots
		And the Slot Schedule should be referenced in the Bundle
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
	Given I get the Organization for Organization Code "ORG1"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Time Period parameter with Start Date today and End Date in "8" days
		And I set the request URL to ""
	When I make the "GpcGetSchedule" request
	Then the response status code should be "404"
		And the response should be a OperationOutcome resource with error code "REFERENCE_NOT_FOUND"


Scenario Outline: I successfully perform a gpc.getschedule operation and check the included schedule resources returned are valid
	Given I get the Organization for Organization Code "<Organization>"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Time Period parameter with Start Date today and End Date in "<Days>" days
	When I make the "GpcGetSchedule" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain Slots
		And the Schedule Metadata should be valid
		And the Schedule Location should be referenced in the Bundle
		And the Schedule Identifiers should be valid
		And the Schedule PlanningHorizon should be valid
		And the Schedule Type should be valid
		And the Schedule Practitioner Extensions should be valid and referenced in the Bundle
	Examples:
		| Organization | Days |
		| ORG1         | 13   |
		| ORG2         | 11   |

Scenario Outline: I successfully perform a gpc.getschedule operation and check the included practitioner resources returned are valid
	Given I get the Organization for Organization Code "<Organization>"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Time Period parameter with Start Date today and End Date in "<Days>" days
	When I make the "GpcGetSchedule" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain Slots
		And the Practitioner Metadata should be valid
		And the Practitioner SDS User Identifier should be valid
		And the Practitioner Identifiers should be valid fixed values
		And the Practitioner Name should be valid
		And the Practitioner PractitionerRoles Roles should be valid
		And the Practitioner should exclude disallowed elements
		And the Practitioner Communication should be valid
		And the Practitioner PractitionerRoles ManagingOrganization should be valid and resolvable
	Examples:
		| Organization | Days |
		| ORG1         | 10   |
		| ORG2         | 13   |

Scenario Outline: I successfully perform a gpc.getschedule operation and check the included organization resources returned are valid
	Given I get the Organization for Organization Code "<Organization>"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Time Period parameter with Start Date today and End Date in "<Days>" days
	When I make the "GpcGetSchedule" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain Slots
		And the Organization Metadata should be valid
		And the Organization Identifiers should be valid
		And the Organization Type should be valid
		And the Organization PartOf Organization should be referenced in the Bundle
	Examples:
		| Organization | Days |
		| ORG1         | 13   |
		| ORG2         | 12   |

Scenario Outline: I successfully perform a gpc.getschedule operation and check the included location resources returned are valid
	Given I get the Organization for Organization Code "<Organization>"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Time Period parameter with Start Date today and End Date in "<Days>" days
	When I make the "GpcGetSchedule" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain Slots
		And the Location Metadata should be valid
		And the Location Identifier should be valid
		And the Location Name should be valid
		And the Location Type should be valid
		And the Location Physical Type should be valid
		And the Location PartOf Location should be valid
		And the Location Managing Organization should be valid
	Examples:
		| Organization | Days |
		| ORG1         | 13   |
		| ORG2         | 13   |

Scenario: Conformance profile supports the gpc.getSchedule operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the Conformance REST Operations should contain "gpc.getschedule"

Scenario:GPCGetSchedule valid response check caching headers exist
	Given I get the Organization for Organization Code "ORG1"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Time Period parameter with Start Date today and End Date in "14" days
	When I make the "GpcGetSchedule" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the required cacheing headers should be present in the response

Scenario:GPCGetSchedule invalid response check caching headers exist
	Given I get the Organization for Organization Code "ORG1"
		And I store the Organization
	Given I configure the default "GpcGetSchedule" request
		And I add a Time Period parameter with Start Date today and End Date in "15" days
	When I make the "GpcGetSchedule" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
		And the required cacheing headers should be present in the response

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