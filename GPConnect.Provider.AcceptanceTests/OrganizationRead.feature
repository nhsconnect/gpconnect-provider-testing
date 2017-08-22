@organization
Feature: OrganizationRead

Scenario Outline: Organization Read successful request validate all of response
	Given I get the Organization for Organization Code "<Organization>"
		And I store the Organization
	Given I configure the default "OrganizationRead" request
	When I make the "OrganizationRead" request
	Then the response status code should indicate success
		And the Response should contain the ETag header matching the Resource Version Id
		And the Response Resource should be an Organization
		And the Organization Identifiers should be valid for Organization "<Organization>"
		And the Organization Id should equal the Request Id
		And the Organization Metadata should be valid
		And the Organization PartOf Organization should be resolvable
		And the Organization Type should be valid
		And the Organization Name should be valid
		And the Organization Telecom should be valid
		And the Organization Address should be valid
		And the Organization Contact should be valid
		And the Organization Extensions should be valid
	Examples:
		| Organization |
		| ORG1         |
		| ORG2         |
		| ORG3         |

Scenario Outline: Organization Read successful request validate site codes returned are as expected
	Given I get the Organization for Organization Code "<Organization>"
		And I store the Organization
	Given I configure the default "OrganizationRead" request
	When I make the "OrganizationRead" request
	Then the response status code should indicate success
		And the returned organization contains an identifier of type "https://fhir.nhs.uk/Id/ods-site-code" with a value of "<ExpectedSiteCode>"
	Examples:
		| Organization | ExpectedSiteCode |
		| ORG1         | SIT1             |
		| ORG2         | SIT2             |
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
		And I store the Organization
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
		And I store the Organization
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
		And I store the Organization
	Given I configure the default "OrganizationRead" request
		And I set the Interaction Id header to "<interactionId>"
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
		And I store the Organization
	Given I configure the default "OrganizationRead" request
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "OrganizationRead" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the Response Resource should be an Organization
		And the Organization Identifiers should be valid
		And the Organization Id should equal the Request Id
		And the Organization Identifiers should be valid for Organization "ORG1"
	Examples:
		| Parameter             | ResponseFormat |
		| application/json+fhir | JSON           |
		| application/xml+fhir  | XML            |

Scenario Outline: Organization Read using the Accept header to request response format
	Given I get the Organization for Organization Code "ORG1"
		And I store the Organization
	Given I configure the default "OrganizationRead" request
		And I set the Accept header to "<Header>"
	When I make the "OrganizationRead" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the Response Resource should be an Organization
		And the Organization Id should equal the Request Id
		And the Organization Identifiers should be valid for Organization "ORG1"
	Examples:
		| Header                | ResponseFormat |
		| application/json+fhir | JSON           |
		| application/xml+fhir  | XML            |

Scenario Outline: Organization Read sending the Accept header and _format parameter to request response format
	Given I get the Organization for Organization Code "ORG1"
		And I store the Organization
	Given I configure the default "OrganizationRead" request
		And I set the Accept header to "<Header>"
		And I add the parameter "_format" with the value "<Parameter>"
	When I make the "OrganizationRead" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the Response Resource should be an Organization
		And the Organization Id should equal the Request Id
		And the Organization Identifiers should be valid for Organization "ORG1"
	Examples:
		| Header                | Parameter             | ResponseFormat |
		| application/json+fhir | application/json+fhir | JSON       |
		| application/json+fhir | application/xml+fhir  | XML        |
		| application/xml+fhir  | application/json+fhir | JSON       |
		| application/xml+fhir  | application/xml+fhir  | XML        |

Scenario: Conformance profile supports the Organization read operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the Conformance REST Resources should contain the "Organization" Resource with the "Read" Interaction

Scenario: Organization read valid response check caching headers exist
	Given I get the Organization for Organization Code "ORG1"
		And I store the Organization
	Given I configure the default "OrganizationRead" request
	When I make the "OrganizationRead" request
	Then the response status code should indicate success
		And the Response should contain the ETag header matching the Resource Version Id
		And the Response Resource should be an Organization
		And the required cacheing headers should be present in the response

Scenario: Organization read invalid response check caching headers exist
	Given I get the Organization for Organization Code "ORG1"
		And I store the Organization
	Given I configure the default "OrganizationRead" request
		And I set the Interaction Id header to "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner3"
	When I make the "OrganizationRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		And the required cacheing headers should be present in the response
