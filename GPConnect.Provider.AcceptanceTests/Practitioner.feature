@Practitioner
Feature: Practitioner

Scenario Outline: Practitioner search valid practitioner
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the practitioner identifier parameter with system "<System>" and value "<Value>"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset
	Examples:
		| System                             | Value     | 
		| http://fhir.nhs.net/Id/sds-user-id | G11111116 |
		| http://fhir.nhs.net/Id/sds-user-id | G22345655 |
		| http://fhir.nhs.net/Id/sds-user-id | G13579135 |
		| http://fhir.nhs.net/Id/sds-role-profile-id | PT1234|
		| http://fhir.nhs.net/Id/sds-role-profile-id | PT1122|
		| http://fhir.nhs.net/Id/sds-role-profile-id  | PT1236|

Scenario Outline: Practitioner search invalid practitioner
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the practitioner identifier parameter with system "<System>" and value "<Value>"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset
	Examples:
		| System                             | Value     | 
		| http://fhir.nhs.net/Id/sds-user-id | G99999999 |
		| http://fhir.nhs.net/Id/sds-role-profile-id  | PT9999|

Scenario Outline: Practitioner search with variation of system id and value
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the practitioner identifier parameter with system "<System>" and value "<Value>"
	When I make a GET request to "/Practitioner"
	Then the response status code should be "422"
		And the response body should be FHIR JSON
	Examples:
		| System                                      | Value     |
		| http://fhir.nhs.net/Id/sds-user-id9         | G11111116 |
		| http://fhir.nhs.net/Id/sds-role-profile-id9 | PT1122    |
		|                                             | G11111116 |
		| null                                        | G11111116 |
		| http://fhir.nhs.net/Id/sds-user-id		  | null      |
		| http://fhir.nhs.net/Id/sds-user-id		  |			  |

Scenario: Practitioner search without the identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
	When I make a GET request to "/Practitioner"
	Then the response status code should be "400"

Scenario Outline:  Practitioner search where identifier contains the incorrect case or spelling
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the practitioner identifier with custom "<Identifier>"  parameter with system "<System>" and value "<Value>"
	When I make a GET request to "/Practitioner"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
	Examples:
		| Identifier | System | Value |
		|idenddstifier| http://fhir.nhs.net/Id/sds-user-id | G11111116 |
		|Idenddstifier| http://fhir.nhs.net/Id/sds-user-id | G11111116 |
		|Identifier| http://fhir.nhs.net/Id/sds-role-profile-id | PT1234|
		|idenddstifier| http://fhir.nhs.net/Id/sds-role-profile-id | PT1234|
		|Idenddstifier| http://fhir.nhs.net/Id/sds-role-profile-id | PT1234|

Scenario Outline:  Practitioner search where format added after identifier 
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id|G11111116"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset
	Examples:
		| Parameter             | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |
		
Scenario Outline:  Practitioner search where format added before identifier 
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "_format" with the value "<Parameter>"
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id|G11111116"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset
	Examples:
		| Parameter             | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Practitioner search accept header
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id|G11111116"
		And I set the Accept header to "<Header>"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
	Examples:
		| Header                | BodyFormat |
		| application/json+fhir | JSON       |
		| application/xml+fhir  | XML        |

Scenario Outline: Practitioner search accept header and _format parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id|G11111116"
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset
	Examples:
		| Header                | Parameter             | BodyFormat |
		| application/json+fhir | application/json+fhir | JSON       |
		| application/json+fhir | application/xml+fhir  | XML        |
		| application/xml+fhir  | application/json+fhir | JSON       |
		| application/xml+fhir  | application/xml+fhir  | XML        |

Scenario Outline: Practitioner search failure due to missing header
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id|G11111116"
		And I do not send header "<Header>"
	When I make a GET request to "/Practitioner"
	Then the response status code should be "500"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource
	Examples:
		| Header            |
		| Ssp-TraceID       |
		| Ssp-From          |
		| Ssp-To            |
		| Ssp-InteractionId |
		| Authorization     |

Scenario Outline: Practitioner search failure due to invalid interactionId
	Given I am using the default server
		And I am performing the "<InteractionId>" interaction
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id|G11111116"
    When I make a GET request to "/Practitioner"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource
	Examples:
		| InteractionId                                                     |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		| InvalidInteractionId                                              |
		|                                                                   |

Scenario: Practitioner search multiple practitioners contains metadata and populated fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id|G11111116"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
		And the JSON response bundle should be type searchset

Scenario: Practitioner search returns back user with name element
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id|G11111116"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset
	Then practitioner resources should contain a single name element

Scenario: Practitioner search returns user with only one family name
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id|G11111116"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
	Then practitioner should only have one family name
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset

Scenario: Practitioner search returns practitioner role element with valid parameters
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id|G11111116"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset
	Given There is a practitionerRoleElement
	Then if practitionerRole has role element which contains a coding then the system, code and display must exist
	Then if practitionerRole has managingOrganization element then reference must exist

Scenario: Practitioner search should not contain qualification or photo information
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id|G11111116"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset

Scenario: Practitioner search contains communication element
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id|G11111116"
	When I make a GET request to "/Practitioner"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset
	Given There is a communication element
	Then If the practitioner has communicaiton elemenets containing a coding then there must be a system, code and display element