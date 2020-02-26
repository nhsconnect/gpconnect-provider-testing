@getschedule @1.3.2-Full_Pack
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

Scenario Outline: Searching for free slots should fail due to missing parameters
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters with a time period of "3" days
		And I remove the parameters "<Keys>"
	When I make the "SearchForFreeSlots" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples: 
	| Keys                      |
	| start,end                 |
	| start                     |
	| status                    |
# removed 1.2.1 RMB 1/10/2018	| searchFilter,searchFilter |

@1.2.3
Scenario: Searching for free slots with valid prefixes
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I add the time period parameters for "3" days starting today using the start date prefix "ge" and the end date prefix "le"
		And I add the parameter "status" with the value "free"
		And I add the parameter "_include" with the value "Slot:schedule"
		And I add two additional non GP Connect specific searchFilter paramaters
		And I add two valid searchFilter paramaters
		And I add one additional non GP Connect specific searchFilter paramaters
	When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
	
Scenario Outline: Searching for free slots with invalid prefixes
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I add the time period parameters for "3" days starting today using the start date prefix "<startDatePrefix>" and the end date prefix "<endDatePrefix>"
		And I add the parameter "status" with the value "free"
		And I add the parameter "_include" with the value "Slot:schedule"
		And I add two valid searchFilter paramaters
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
		And I add the parameter "status" with the value "free"
		And I add the parameter "_include" with the value "Slot:schedule"
		And I add two valid searchFilter paramaters
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
		| Key    | Value            |
		| start  | invalidStartDate |
		| start  |                  |
		| end    | invalidEnddate   |
		| end    |                  |
		| status | busy             |

Scenario: Searching for free slots with other searchFilter system
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters with a time period of "3" days
		And I add one additional non GP Connect specific searchFilter paramaters
		When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"

#PG 9-4-2018 #223 - Updated Test to use agreed date formats
@1.2.3
Scenario Outline: Searching for free slots with valid partial dateTime strings
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I add the time period parameters for "3" days starting today using the start format "<StartDate>" and the end format "<EndDate>"
		And I add the parameter "status" with the value "free"
		And I add the parameter "_include" with the value "Slot:schedule"
		And I add two valid searchFilter paramaters
		And I add one additional non GP Connect specific searchFilter paramaters
	When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
	Examples:
		| StartDate            | EndDate              |
		| yyyy-MM-dd           | yyyy-MM-dd           |
		| yyyy-MM-ddTHH:mm:sszzz  | yyyy-MM-ddTHH:mm:sszzz  |
		| yyyy-MM-ddTHH:mm:sszzz  | yyyy-MM-dd           |
		| yyyy-MM-dd  | yyyy-MM-ddTHH:mm:sszzz           |

@1.2.3
Scenario Outline: Searching for free slots with invalid partial dateTime strings
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I add the time period parameters for "3" days starting today using the start format "<StartDate>" and the end format "<EndDate>"
		And I add the parameter "status" with the value "free"
		And I add the parameter "_include" with the value "Slot:schedule"
		And I add two valid searchFilter paramaters
	When I make the "SearchForFreeSlots" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
	Examples:
		| StartDate            | EndDate              |
		| yyyy-MM-ddTHH:mm:ss  | yyyy-MM-ddTHH:mm:ss  |
		| yyyy-MM-ddTHH:mm:ssZ | yyyy-MM-ddTHH:mm:ss  |
		| yyyy-MM-ddTHH:mm:ss  | yyyy-MM-ddTHH:mm:ssZ |
		| yyyy                 | yyyy-MM              |
		| yyyy-MM              | yyyy	              |

Scenario: Successfully search for free slots and check the slot resources returned are valid
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters with a time period of "3" days
		And I add two additional non GP Connect specific searchFilter paramaters
	When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Slot Id should be valid
		And the Slot Status should be Free
		And the Slot Metadata should be valid
		And the Slot Identifiers should be valid
		And the Slot Extensions should be valid
# git hub ref 120
# RMB 25/10/2018
		And the Slot Not In Use should be valid

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
		| application/fhir+xml  | application/fhir+xml  | XML              |
		| application/fhir+json | application/fhir+json | JSON             |
		| application/fhir+xml  | application/fhir+json | JSON             |
		| application/fhir+json | application/fhir+xml  | XML              |

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
		| application/fhir+xml  | application/fhir+xml  | XML              |
		| application/fhir+json | application/fhir+json | JSON             |
		| application/fhir+xml  | application/fhir+json | JSON             |
		| application/fhir+json | application/fhir+xml  | XML              |

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
		| application/fhir+xml  | application/fhir+xml  | application/fhir+xml  | XML              |
		| application/fhir+json | application/fhir+json | application/fhir+json | JSON             |
		| application/fhir+xml  | application/fhir+json | application/fhir+json | JSON             |
		| application/fhir+json | application/fhir+xml  | application/fhir+xml  | XML              |
		| application/fhir+xml  | application/fhir+json | application/fhir+xml  | XML              |
		| application/fhir+json | application/fhir+xml  | application/fhir+json | JSON             |
		| application/fhir+xml  | application/fhir+xml  | application/fhir+json | JSON             |
		| application/fhir+json | application/fhir+json | application/fhir+xml  | XML              |

Scenario: Successfully search for free slots and check the included schedule resources returned are valid
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters with a time period of "3" days
		And I add the parameter "_include:recurse" with the value "Schedule:actor:Practitioner"
		And I add the parameter "_include:recurse" with the value "Schedule:actor:Location"
# github ref 124
# RMB 29/10/2018
		And I add the parameter "_include:recurse" with the value "Location:managingOrganization"
	When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Slot Schedule should be referenced in the Bundle
		And the Schedule Metadata should be valid
		And the Schedule Location should be referenced in the Bundle
		And the Location should be valid
		And the Location Id should be valid
		And the Schedule Id should be valid
		And the Schedule Identifiers should be valid
		And the Schedule PlanningHorizon should be valid
		And the Schedule ServiceType should be valid
		And the Schedule Extensions should be populated and valid
		And the Practitioner Entry should be valid
		And the Practitioner Id should be valid
		And the Organization should be valid
		And the Organization Id should be valid
# git hub ref 120
# RMB 25/10/2018
		And the Schedule Not In Use should be valid

Scenario Outline: Searching for free slots without actor parameter should return results without actor resource 
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters with a time period of "3" days
		And I add the parameter "_include:recurse" with the value "<IncludedActorValue>"
	When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Bundle should contain Slots
		And the Slot Schedule should be referenced in the Bundle
		And the excluded actor "<ExcludedActor>" should not be present in the Bundle
	Examples:
	| ExcludedActor | IncludedActorValue          |
	| Location      | Schedule:actor:Practitioner |
	| Practitioner  | Schedule:actor:Location     |

Scenario: Successfully search for free slots and check the included practitioner resources returned are valid
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters with a time period of "3" days
		And I add the parameter "_include:recurse" with the value "Schedule:actor:Practitioner"
	When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Practitioner Metadata should be valid
		And the Practitioner SDS User Identifier should be valid
		And the Practitioner Identifiers should be valid fixed values
		And the Practitioner Name should be valid
		And the Practitioner should exclude disallowed elements
		And the Practitioner nhsCommunication should be valid

@1.2.3
Scenario: Successfully search for free slots and check the included location resources returned are valid
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I set the required parameters with a time period of "3" days
		And I add the parameter "_include:recurse" with the value "Schedule:actor:Location"
	When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Location Metadata should be valid
		And the Location Id should be valid
		And the Location Identifier should be valid
		And the Location Type should be valid
		And the Location Physical Type should be valid
		And the Location PartOf Location should be valid
		And the Location Managing Organization should be valid
		And the Location Name should be valid
		And the Location Address should be valid

Scenario: CapabilityStatement profile supports the Slot Search Resource
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement REST Resources should contain the "Slot" Resource with the "SearchType" Interaction

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
# git hub ref 178 
# RMB 31/1/19
		And I set the required parameters with a time period of "30" days
#   And I set the Interaction Id header to "InvalidInteractionId"
	When I make the "SearchForFreeSlots" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
		And the required cacheing headers should be present in the response

# git hub ref 131
# RMB 25/10/2018
# git hub ref 175
# RMB 24/1/19
@1.2.3
Scenario Outline: Searching for free slots with org type searchFilter system
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I add the time period parameters for "14" days starting today using the start date prefix "ge" and the end date prefix "le"
		And I add the parameter "status" with the value "free"
		And I add the parameter "_include" with the value "Slot:schedule"
		And I add org type searchFilter paramaters "<orgType>"
		When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Organization should be valid
		And the Organization Id should be valid
		And the Slot Id should be valid
		And the Slot Count should be valid
		And the Slot Status should be Free
		And the Slot Metadata should be valid
		And the Slot Identifiers should be valid
		And the Slot Extensions should be valid
	Examples:
		| orgType     |
		| urgent-care |
		| gp-practice |
		| oncology    |

# git hub ref 131
# RMB 25/10/2018
# git hub ref 175
# RMB 24/1/19
@1.2.3
Scenario Outline: Searching for free slots with org code searchFilter system
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I add the time period parameters for "14" days starting today using the start date prefix "ge" and the end date prefix "le"
		And I add the parameter "status" with the value "free"
		And I add the parameter "_include" with the value "Slot:schedule"
		And I add org code searchFilter paramaters "<orgCode>"
		When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Organization should be valid
		And the Organization Id should be valid
		And the Slot Id should be valid
		And the Slot Count should be valid
		And the Slot Status should be Free
		And the Slot Metadata should be valid
		And the Slot Identifiers should be valid
		And the Slot Extensions should be valid
	Examples:
		| orgCode |
		| A20047  |
		| B81016  |
		| M85015  |
		| RNR01   |

# git hub ref 143 AG
# RMB 5/12/2018
# git hub ref 175
# RMB 24/1/19
Scenario: Searching for free slots with NO org code and NO org type searchFilter system
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I add the time period parameters for "14" days starting today using the start date prefix "ge" and the end date prefix "le"
		And I add the parameter "status" with the value "free"
		And I add the parameter "_include" with the value "Slot:schedule"		
		When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Organization should be valid
		And the Organization Id should be valid
		And the Slot Id should be valid
		And the Slot Count should be valid
		And the Slot Status should be Free
		And the Slot Metadata should be valid
		And the Slot Identifiers should be valid
		And the Slot Extensions should be valid

# git hub ref 175
# RMB 24/1/19
@1.2.3
Scenario Outline: Searching for free slots with org type and code searchFilter system
	Given I configure the default "SearchForFreeSlots" request
		And I set the JWT Requested Scope to Organization Read
		And I add the time period parameters for "14" days starting today using the start date prefix "ge" and the end date prefix "le"
		And I add the parameter "status" with the value "free"
		And I add the parameter "_include" with the value "Slot:schedule"
		And I add org type searchFilter paramaters "<orgType>"
		And I add org code searchFilter paramaters "<orgCode>"
		When I make the "SearchForFreeSlots" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the Organization should be valid
		And the Organization Id should be valid
		And the Slot Id should be valid
		And the Slot Count should be valid
		And the Slot Status should be Free
		And the Slot Metadata should be valid
		And the Slot Identifiers should be valid
		And the Slot Extensions should be valid
	Examples:
		| orgType     | orgCode |
		| urgent-care | A20047  |
		| urgent-care | B81016  |
		| urgent-care | M85015  |
		| urgent-care | RNR01   |
		| gp-practice | A20047  |
		| gp-practice | B81016  |
		| gp-practice | M85015  |
		| gp-practice | RNR01   |
		| oncology    | A20047  |
		| oncology    | B81016  |
		| oncology    | M85015  |
		| oncology    | RNR01   |

	#PG 12-4-2019 #225 - Check that CapabilityStatement includes Location:managingOrganization 
	Scenario: Check CapabilityStatement includes specific searchInclude
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success	
	And the CapabilityStatement has a searchInclude called "Location:managingOrganization"
	
