@organization
Feature: OrganizationRead

Scenario Outline: Organization Read successful request validate all of response
	Given I get the Organization for Organization Code "<Organization>"
		And I store the Organization Id
	Given I configure the default "OrganizationRead" request
	When I make the "OrganizationRead" request
	Then the response status code should indicate success
		And the response should be the format FHIR JSON
		And the response should be an Organization resource
		And the returned resource shall contain a logical id matching the requested read logical identifier
		And the returned organization resource identifiers should be of a valid type and cardinality
		And the returned resource shall contain the business identifier for Organization "<Organization>"
		And the organization resource shall contain meta data profile and version id
		And if the organization resource contains a partOf reference it is valid and resolvable
		And if the organization resource contains type element it shall contain the system code and display elements
		And the response should contain the ETag header matching the resource version
	Examples:
		| Organization |
		| ORG1         |
		| ORG2         |
		| ORG3         |

Scenario Outline: Organization Read successful request validate site codes returned are as expected
	Given I get the Organization for Organization Code "<Organization>"
		And I store the Organization Id
	Given I configure the default "OrganizationRead" request
	When I make the "OrganizationRead" request
	Then the response status code should indicate success
		And the returned organization contains identifiers of type "http://fhir.nhs.net/Id/ods-site-code" with values "<ExpectedSiteCode>"
	Examples:
		| Organization | ExpectedSiteCode |
		| ORG1         | SIT1             |
		| ORG2         | SIT2,SIT3        |
		| ORG3         | SIT3             |

Scenario Outline: Organization Read with valid identifier which does not exist on providers system
	Given I configure the default "OrganizationRead" request
		And I set the Read Operation logical identifier used in the request to "<LogicalId>"
	When I make the "OrganizationRead" request
	Then the response status code should be "404"
	Examples:
		| LogicalId      |
		| ddBm           |
		| 1Zcr4          |
		| z.as.e         |
		| 1.1445.23      |
		| 40-9223        |
		| nc-sfem.mks--s |

Scenario Outline: Organization Read with invalid resource path in URL
	Given I get the Organization for Organization Code "ORG1"
		And I store the Organization Id
	Given I configure the default "OrganizationRead" request
		And I set the Read Operation relative path to "<RelativePath>" and append the resource logical identifier
	When I make the "OrganizationRead" request
	Then the response status code should be "404"
	Examples:
		| RelativePath  |
		| Organizations |
		| Organization! |
		| Organization2 |
		| organizations |

Scenario Outline: Organization Read with missing mandatory header
	Given I get the Organization for Organization Code "ORG1"
		And I store the Organization Id
	Given I configure the default "OrganizationRead" request
		And I do not send header "<Header>"
	When I make the "OrganizationRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Header            |
		| Ssp-TraceID       |
		| Ssp-From          |
		| Ssp-To            |
		| Ssp-InteractionId |
		| Authorization     |

Scenario Outline: Organization Read with incorrect interaction id
	Given I get the Organization for Organization Code "ORG1"
		And I store the Organization Id
	Given I configure the default "OrganizationRead" request
		And I am performing the "<interactionId>" interaction
	When I make the "OrganizationRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| interactionId                                                     |
		| urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner3     |
		| urn:nhs:names:services:gpconnect:fhir:rest:read:practitioners     |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		|                                                                   |
		| null                                                              |

Scenario Outline: Organization Read using the _format parameter to request response format
	Given I get the Organization for Organization Code "ORG1"
		And I store the Organization Id
	Given I configure the default "OrganizationRead" request
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "OrganizationRead" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be an Organization resource
		And the returned resource shall contain a logical id matching the requested read logical identifier
		And the returned resource shall contain the business identifier for Organization "ORG1"
	Examples:
		| Parameter             | ResponseFormat |
		| application/json+fhir | JSON           |
		| application/xml+fhir  | XML            |

Scenario Outline: Organization Read using the Accept header to request response format
	Given I get the Organization for Organization Code "ORG1"
		And I store the Organization Id
	Given I configure the default "OrganizationRead" request
		And I set the Accept header to "<Header>"
	When I make the "OrganizationRead" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be an Organization resource
		And the returned resource shall contain a logical id matching the requested read logical identifier
		And the returned resource shall contain the business identifier for Organization "ORG1"
	Examples:
		| Header                | ResponseFormat |
		| application/json+fhir | JSON           |
		| application/xml+fhir  | XML            |

Scenario Outline: Organization Read sending the Accept header and _format parameter to request response format
	Given I get the Organization for Organization Code "ORG1"
		And I store the Organization Id
	Given I configure the default "OrganizationRead" request
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "OrganizationRead" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be an Organization resource
		And the returned resource shall contain a logical id matching the requested read logical identifier
		And the returned resource shall contain the business identifier for Organization "ORG1"
	Examples:
		| Header                | Parameter             | ResponseFormat |
		| application/json+fhir | application/json+fhir | JSON       |
		| application/json+fhir | application/xml+fhir  | XML        |
		| application/xml+fhir  | application/json+fhir | JSON       |
		| application/xml+fhir  | application/xml+fhir  | XML        |

Scenario: Conformance profile supports the Organization read operation
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the conformance profile should contain the "Organization" resource with a "read" interaction

#Potentially out of scope, outstanding issue on github "https://github.com/nhsconnect/gpconnect/issues/189"
Scenario: VRead of current resource should return resource
	Given I get organization "ORG3" id and save it as "ORGID"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:organization" interaction
	When I perform a vread for organizaiton "ORGID"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Organization resource

#Potentially out of scope, outstanding issue on github "https://github.com/nhsconnect/gpconnect/issues/189"
Scenario: VRead of non existant version should return an error
	Given I get organization "ORG2" id and save it as "storedOrganization"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:organization" interaction
	When I perform an organization vread with version "NotARealVersionId" for organization stored against key "storedOrganization"
	Then the response status code should be "404"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

@ignore
Scenario: If-None-Match read organization on a matching version
	#Potentially out of scope, outstanding issue on github "https://github.com/nhsconnect/gpconnect/issues/189"

@ignore
Scenario: If-None-Match read organization on a non matching version
	#Potentially out of scope, outstanding issue on github "https://github.com/nhsconnect/gpconnect/issues/189"

