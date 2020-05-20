@practitioner
Feature: PractitionerSearch

# Common
# JWT is hard coded value and it should probably be considered what JWT requested resource should be, organization but which?
# Compress successful tests into one possibly // For clarity it may be better to keep successful seperated as if one failes it is easier to see where the problem is
@1.2.3
Scenario Outline: Practitioner search success and validate the practitioner identifiers
	Given I configure the default "PractitionerSearch" request		
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "<Value>"
	When I make the "PractitionerSearch" request
	Then the response status code should indicate success
		And the response bundle should contain "<EntrySize>" entries
		And the response should be a Bundle resource of type "searchset" 
		And the Practitioner Id should be valid
		And the Practitioner Identifiers should be valid fixed values
		And the Practitioner SDS User Identifier should be valid for Value "<Value>"
		And the Practitioner SDS Role Profile Identifier should be valid for "<RoleSize>" Role Profile Identifiers
	Examples:
		| Value         | EntrySize | RoleSize |
		| practitioner1 | 1         | 0        |
		| practitioner2 | 1         | 1        |
		| practitioner3 | 1         | 2        |

Scenario Outline: Practitioner search with failure due to invalid identifier
	Given I configure the default "PractitionerSearch" request
		And I add a Practitioner Identifier parameter with System "<System>" and Value "<Value>"
	When I make the "PractitionerSearch" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
	Examples:
		| System                                     | Value         |
		| https://fhir.nhs.uk/Id/sds-user-id         |               |
		|                                            | practitioner2 |

Scenario: Practitioner search without the identifier parameter
	Given I configure the default "PractitionerSearch" request
	When I make the "PractitionerSearch" request
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario Outline: Practitioner search where identifier contains the incorrect case or spelling
	Given I configure the default "PractitionerSearch" request
		And I add a Practitioner "<ParameterName>" parameter with System "https://fhir.nhs.uk/Id/sds-user-id" and Value "practitioner2"
	When I make the "PractitionerSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| ParameterName |
		| Identifier    |

Scenario Outline: Practitioner search testing paramater validity before adding identifier
	Given I configure the default "PractitionerSearch" request
		And I add the parameter "<Param1Name>" with the value "<Param1Value>"
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner2"
	When I make the "PractitionerSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And the Practitioner Identifiers should be valid fixed values
		And the Practitioner Name should be valid
		And the Practitioner should exclude disallowed elements
		And the Practitioner nhsCommunication should be valid
	Examples:
		| Param1Name | Param1Value           | BodyFormat |
		| _format    | application/fhir+json | JSON       |
		| _format    | application/fhir+xml  | XML        |

Scenario Outline: Practitioner search testing paramater validity after adding identifier
	Given I configure the default "PractitionerSearch" request
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner2"
		And I add the parameter "<Param1Name>" with the value "<Param1Value>"
	When I make the "PractitionerSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And the Practitioner Identifiers should be valid fixed values
		And the Practitioner Name should be valid
		And the Practitioner should exclude disallowed elements
		And the Practitioner nhsCommunication should be valid
	Examples:
		| Param1Name | Param1Value           | BodyFormat |
		| _format    | application/fhir+json | JSON       |
		| _format    | application/fhir+xml  | XML        |


Scenario Outline: Practitioner search add accept header to request and check for correct response format
	Given I configure the default "PractitionerSearch" request
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner2"
		And I set the Accept header to "<Header>"
	When I make the "PractitionerSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And the Practitioner Identifiers should be valid fixed values
		And the Practitioner Name should be valid
		And the Practitioner should exclude disallowed elements
		And the Practitioner nhsCommunication should be valid
	Examples:
		| Header                | BodyFormat |
		| application/fhir+json | JSON       |
		| application/fhir+xml  | XML        |

Scenario Outline: Practitioner search add accept header and _format parameter to the request and check for correct response format
	Given I configure the default "PractitionerSearch" request
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner2"
		And I set the Accept header to "<Header>"
		And I add a Format parameter with the Value "<Parameter>"
	When I make the "PractitionerSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the response should be a Bundle resource of type "searchset"
		And the Practitioner Metadata should be valid
		And the Practitioner Identifiers should be valid fixed values
		And the Practitioner Name should be valid
		And the Practitioner should exclude disallowed elements
		And the Practitioner nhsCommunication should be valid
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/fhir+json | application/fhir+json | JSON       |
		| application/fhir+json | application/fhir+xml  | XML        |
		| application/fhir+xml  | application/fhir+json | JSON       |
		| application/fhir+xml  | application/fhir+xml  | XML        |

@1.2.3
Scenario: Practitioner search multiple identifier parameter failure
	Given I configure the default "PractitionerSearch" request
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner2"
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner2"
	When I make the "PractitionerSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Practitioner search multiple multiple identifiers for different practitioner parameter failure
	Given I configure the default "PractitionerSearch" request
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner1"
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner2"
	When I make the "PractitionerSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Practitioner search include count and sort parameters
	Given I configure the default "PractitionerSearch" request		
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner1"
		And I add the parameter "_sort" with the value "practitioner.coding"
		And I add the parameter "_count" with the value "1"
	When I make the "PractitionerSearch" request
	Then the response status code should indicate success
		And the response bundle should contain "1" entries
		And the response should be a Bundle resource of type "searchset" 

Scenario: CapabilityStatement profile supports the Practitioner search operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement REST Resources should contain the "Practitioner" Resource with the "SearchType" Interaction

Scenario:Practitioner search valid response check caching headers exist
	Given I configure the default "PractitionerSearch" request		
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner1"
	When I make the "PractitionerSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset" 
		And the required cacheing headers should be present in the response

Scenario:Practitioner search invalid response check caching headers exist
	Given I configure the default "PractitionerSearch" request
		And I set the Interaction Id header to "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner-1"
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner2"
	When I make the "PractitionerSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		And the required cacheing headers should be present in the response
	
# git hub ref 120
# RMB 25/10/2018
Scenario: Practitioner search returned should conform to the GPconnect specification
	Given I configure the default "PractitionerSearch" request		
		And I add a Practitioner Identifier parameter with SDS User Id System and Value "practitioner1"
	When I make the "PractitionerSearch" request
	Then the response status code should indicate success
		And the response bundle should contain "1" entries
		And the response should be a Bundle resource of type "searchset"
		And the Practitioner Not In Use should be valid

# git hub ref 320 SJD 05/12/2019
Scenario: Practitioner search with unknown identifier
       Given I configure the default "PractitionerSearch" request
              And I add a Practitioner Identifier parameter with unknown value
       When I make the "PractitionerSearch" request
       Then the response status code should indicate success
              And the response should be a Bundle resource of type "searchset"
              And the response bundle should contain "0" entries
