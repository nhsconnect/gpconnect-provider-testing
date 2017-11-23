@getschedule
Feature: SearchForFreeSlots
	
Scenario Outline: Searching for free slots with valid parameters should return success
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters with a time period of "<Days>" days
		When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| Days	|
		| 8		|
		| 0		|
		| 14	|

Scenario Outline: Searching for free slots with invalid date range should return error
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters with a time period of "<Days>" days
		When I make the "SearchForFreeSlots" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
	Examples:
		| Days |
		| 15   |
		| 31   |
		| -1   |

Scenario Outline: Searching for free slots should fail due to invalid interactionId
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I set the Interaction Id header to "<InteractionId>"
	When I make the "SearchForFreeSlots" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| InteractionId                                                     |
		| urn:nhs:names:zservices:gpconnect:fhir:operation:gpc.getcarerecord |
		| InvalidInteractionId                                              |
		|                                                                   |

Scenario Outline: Searching for free slots should fail due to missing header
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
	When I make the "SearchForFreeSlots" request with missing Header "<Header>"
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Header            |
		| Ssp-TraceID       |
		| Ssp-From          |
		| Ssp-To            |
		| Ssp-InteractionId |
		| Authorization     |

Scenario Outline: Searching for free slots should fail due to missing parameters
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters with a time period of "3" days
		And I remove the parameters "<Keys>"
	When I make the "SearchForFreeSlots" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples: 
	| Keys		|
	| start,end |
	| start		|
	| fb-type	|

Scenario: Searching for free slots with valid prefixes
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I add the time period parameters for "3" days starting today using the start date prefix "ge" and the end date prefix "le"
		And I add the parameter "fb-type" with the value "free"
		And I add the parameter "_include" with the value "Slot:schedule"
	When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
	
Scenario Outline: Searching for free slots with invalid prefixes
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I add the time period parameters for "3" days starting today using the start date prefix "<startDatePrefix>" and the end date prefix "<endDatePrefix>"
		And I add the parameter "fb-type" with the value "free"
		And I add the parameter "_include" with the value "Slot:schedule"
	When I make the "SearchForFreeSlots" request
	Then the response status code should indicate failure
		And the response status code should be "422"
	Examples:
		| startDatePrefix | endDatePrefix |
		| lt              | gt            |
		| gt              | gt            |
		| lt              | lt            |
		| le              | ge            |
		| eq              | eq            |
		| gt              | lt            |

Scenario Outline: Searching for free slots with unknown prefixes
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I add the time period parameters for "3" days starting today using the start date prefix "<startDatePrefix>" and the end date prefix "<endDatePrefix>"
		And I add the parameter "fb-type" with the value "free"
		And I add the parameter "_include" with the value "Slot:schedule"
	When I make the "SearchForFreeSlots" request
	Then the response status code should indicate failure
		And the response status code should be "422"
	Examples:
		| startDatePrefix | endDatePrefix |
		| ne              | ne            |
		| gt              | ne            |
		| ne              | lt            |
		| ge              | ne            |
		| ne              | le            |

Scenario Outline: Searching for free slots should fail due to invalid parameter values
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters with a time period of "3" days
		And I update the parameter "<Key>" with value "<Value>"
	When I make the "SearchForFreeSlots" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"	
	Examples:
		| Key			| Value				|
		| start			| invalidStartDate  |
		| start			|					|
		| end			| invalidEnddate	|
		| end			|					|
		| fb-type		| busy				|

Scenario Outline: Searching for free slots with valid partial dateTime strings
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I add the time period parameters for "3" days starting today using the start format "<StartDate>" and the end format "<EndDate>"
		And I add the parameter "fb-type" with the value "free"
		And I add the parameter "_include" with the value "Slot:schedule"
	When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| StartDate           | EndDate             |
		| yyyy-MM-dd          | yyyy-MM-dd          |
		| yyyy                | yyyy                |
		| yyyy-MM-ddTHH:mm:ss | yyyy-MM-ddTHH:mm:ss |
		| yyyy-MM-dd          | yyyy-MM-ddTHH:mm:ss |
		| yyyy-MM-ddTHH:mm:ss | yyyy-MM-dd          |

Scenario Outline: Searching for free slots with in-valid partial dateTime strings
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I add the time period parameters for "3" days starting today using the start format "<StartDate>" and the end format "<EndDate>"
		And I add the parameter "fb-type" with the value "free"
		And I add the parameter "_include" with the value "Slot:schedule"
	When I make the "SearchForFreeSlots" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"	
	Examples: 
		| StartDate | EndDate   |
		| yyyy      | yyyy-MM   |
		| yyyy-MM   | yyyy		|

Scenario: Successfully search for free slots and check the slot resources returned are valid
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters with a time period of "3" days	
	When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Slot FreeBusyType should be Free
		And the Slot Metadata should be valid
		And the Slot Identifiers should be valid

Scenario Outline: Successfully search for free slots using various content types XML and JSON in Accept header
	Given I configure the default "SearchForFreeSlots" request
		And I set the request content type to "<RequestContentType>"
		And I set the Accept header to "<AcceptHeaderValue>"
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters with a time period of "3" days
	When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response body should be FHIR <ResponseShouldBe>
		And the response should be a Bundle resource of type "searchset"
		And the Slot Schedule should be referenced in the Bundle
	Examples:
		| RequestContentType    | AcceptHeaderValue     | ResponseShouldBe |
		| application/xml+fhir  | application/xml+fhir  | XML              |
		| application/json+fhir | application/json+fhir | JSON             |
		| application/xml+fhir  | application/json+fhir | JSON             |
		| application/json+fhir | application/xml+fhir  | XML              |

Scenario Outline: Successfully search for free slots using various content types XML and JSON in format parameter
	Given I configure the default "SearchForFreeSlots" request
		And I set the request content type to "<RequestContentType>"
		And I add a Format parameter with the Value "<FormatParameterValue>"
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters with a time period of "3" days
	When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response body should be FHIR <ResponseShouldBe>
		And the response should be a Bundle resource of type "searchset"
		And the Slot Schedule should be referenced in the Bundle
	Examples:
		| RequestContentType    | FormatParameterValue  | ResponseShouldBe |
		| application/xml+fhir  | application/xml+fhir  | XML              |
		| application/json+fhir | application/json+fhir | JSON             |
		| application/xml+fhir  | application/json+fhir | JSON             |
		| application/json+fhir | application/xml+fhir  | XML              |

Scenario Outline: Successfully search for free slots using various content types XML and JSON in the Accept Header and format parameter
	Given I configure the default "SearchForFreeSlots" request
		And I set the request content type to "<RequestContentType>"
		And I set the Accept header to "<AcceptHeaderValue>"
		And I add a Format parameter with the Value "<FormatParameterValue>"
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters with a time period of "3" days
	When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response body should be FHIR <ResponseShouldBe>
		And the response should be a Bundle resource of type "searchset"
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

Scenario: Successfully search for free slots and check the included schedule resources returned are valid
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters with a time period of "3" days
		And I add the parameter "_include:recurse" with the value "Schedule:Practitioner"
		And I add the parameter "_include:recurse" with the value "Schedule:actor:Location"
	When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Slot Schedule should be referenced in the Bundle
		And the Schedule Metadata should be valid
		And the Schedule Location should be referenced in the Bundle
		And the Location should be valid
		And the Schedule Identifiers should be valid
		And the Schedule PlanningHorizon should be valid
		And the Schedule Type should be valid
		And the Schedule Practitioner Extensions should be valid and referenced in the Bundle
		And the Practitioner Entry should be valid
		And the Organization should be valid

Scenario Outline: Searching for free slots without actor parameter should return results without actor resource 
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters with a time period of "14" days
		And I add the parameter "_include:recurse" with the value "<IncludedActorValue>"
	When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain Slots
		And the Slot Schedule should be referenced in the Bundle
		And the excluded actor "<ExcludedActor>" should not be present in the Bundle
	Examples:
	| ExcludedActor | IncludedActorValue      |
	| Location      | Schedule:Practitioner   |
	| Practitioner  | Schedule:actor:Location |

Scenario: Searching in the future for no free slots should result in no resources returned
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I add the time period parameter that is "500" days in the future
		And I add the parameter "fb-type" with the value "free"
		And I add the parameter "_include" with the value "Slot:schedule"
	When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should not contain resources

@ignore
Scenario: Successfully search for free slots and check the included practitioner resources returned are valid
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters with a time period of "3" days
	When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Schedule Practitioner Extensions should be valid and referenced in the Bundle
		And the Practitioner Metadata should be valid
		And the Practitioner SDS User Identifier should be valid
		And the Practitioner Identifiers should be valid fixed values
		And the Practitioner Name should be valid
		And the Practitioner PractitionerRoles Roles should be valid
		And the Practitioner should exclude disallowed elements
		And the Practitioner nhsCommunication should be valid
		And the Practitioner PractitionerRoles ManagingOrganization should be referenced in the Bundle

@ignore
Scenario: Successfully search for free slots and check the included organization resources returned are valid
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters with a time period of "3" days
	When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Organization Metadata should be valid
		And the Organization Identifiers should be valid
		And the Organization Type should be valid
		And the Organization PartOf Organization should be valid

@ignore
Scenario: Successfully search for free slots and check the included location resources returned are valid
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters with a time period of "3" days
	When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Location Metadata should be valid
		And the Location Identifier should be valid
		And the Location Type should be valid
		And the Location Physical Type should be valid
		And the Location PartOf Location should be valid
		And the Location Managing Organization should be valid

Scenario: Conformance profile supports the Slot Search Resource
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the Conformance REST Resources should contain the "Slot" Resource with the "SearchType" Interaction

Scenario: SearchForFreeSlots valid response check caching headers exist
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters with a time period of "3" days
	When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the required cacheing headers should be present in the response

Scenario: SearchForFreeSlots invalid response check caching headers exist
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters with a time period of "3" days
		And I set the Interaction Id header to "InvalidInteractionId"
	When I make the "SearchForFreeSlots" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		And the required cacheing headers should be present in the response
