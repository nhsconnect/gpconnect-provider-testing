@practitioner
Feature: PractitionerRead

Scenario Outline: Practitioner read successful request validate all of response
	Given I get the Practitioner for Practitioner Code "<practitioner>"
		And I store the Practitioner
	Given I configure the default "PractitionerRead" request
	When I make the "PractitionerRead" request
	Then the response status code should indicate success
		And the Response Resource should be a Practitioner
		And the Practitioner Id should equal the Request Id
		And the Practitioner Metadata should be valid
		And the Practitioner Identifiers should be valid
		And the Practitioner Name should be valid
		And the Practitioner nhsCommunication should be valid
		And the Practitioner should exclude disallowed elements
		And the practitioner Telecom should be valid
		And the practitioner Address should be valid
		And the practitioner Gender should be valid
		And the Practitioner SDS Role Profile Identifier should be valid for "<numberOfRoleIdentifiers>" Role Profile Identifiers
		And the Practitioner SDS User Identifier should be valid for Value "<practitioner>"
	Examples: 
		| practitioner  | numberOfRoleIdentifiers |
		| practitioner1 | 0                       |
		| practitioner2 | 1                       |
		| practitioner3 | 2                       |

Scenario Outline: Practitioner Read with valid identifier which does not exist on providers system
	Given I configure the default "PractitionerRead" request
		And I set the Read Operation logical identifier used in the request to "<LogicalId>"
	When I make the "PractitionerRead" request
	Then the response status code should be "404"
		And the response should be a OperationOutcome resource
	Examples:
		| LogicalId      |
		| aaBA           |
		| 1ZEc2          |
		| z.as.dd        |
		| 1.1.22         |
		| 40-9           |
		| nd-skdm.mks--s |

Scenario Outline: Practitioner Read with invalid resource path in URL
	Given I get the Practitioner for Practitioner Code "practitioner1"
		And I store the Practitioner
	Given I configure the default "PractitionerRead" request
		And I set the Read Operation relative path to "<RelativePath>" and append the resource logical identifier
	When I make the "PractitionerRead" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource
	Examples:
		| RelativePath  |
		| Practitioners |
		| Practitioner! |
		| Practitioner2 |
		| practitioners |

Scenario Outline: Practitioner Read using the _format parameter to request response format
	Given I get the Practitioner for Practitioner Code "practitioner1"
		And I store the Practitioner
	Given I configure the default "PractitionerRead" request
		And I add a Format parameter with the Value "<Parameter>"
	When I make the "PractitionerRead" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the Response Resource should be a Practitioner
		And the Practitioner Id should equal the Request Id
		And the Practitioner SDS User Identifier should be valid for Value "practitioner1"
	Examples:
		| Parameter             | ResponseFormat |
		| application/fhir+json | JSON           |
		| application/fhir+xml  | XML            |

Scenario Outline: Practitioner Read using the Accept header to request response format
	Given I get the Practitioner for Practitioner Code "practitioner1"
		And I store the Practitioner
	Given I configure the default "PractitionerRead" request
		And I set the Accept header to "<Header>"
	When I make the "PractitionerRead" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the Response Resource should be a Practitioner
		And the Practitioner Id should equal the Request Id
		And the Practitioner SDS User Identifier should be valid for Value "practitioner1"
	Examples:
		| Header                | ResponseFormat |
		| application/fhir+json | JSON           |
		| application/fhir+xml  | XML            |

Scenario Outline: Practitioner Read sending the Accept header and _format parameter to request response format
	Given I get the Practitioner for Practitioner Code "practitioner2"
		And I store the Practitioner
	Given I configure the default "PractitionerRead" request
		And I set the Accept header to "<Header>"
		And I add a Format parameter with the Value "<Parameter>"
	When I make the "PractitionerRead" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the Response Resource should be a Practitioner
		And the Practitioner Id should equal the Request Id
		And the Practitioner SDS User Identifier should be valid for Value "practitioner2"
	Examples:
		| Header                | Parameter             | ResponseFormat |
		| application/fhir+json | application/fhir+json | JSON           |
		| application/fhir+json | application/fhir+xml  | XML            |
		| application/fhir+xml  | application/fhir+json | JSON           |
		| application/fhir+xml  | application/fhir+xml  | XML            |

Scenario: CapabilityStatement profile supports the Practitioner read operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement REST Resources should contain the "Practitioner" Resource with the "Read" Interaction

Scenario: Practitioner read response should contain an ETag header
	Given I get the Practitioner for Practitioner Code "practitioner1"
		And I store the Practitioner
	Given I configure the default "PractitionerRead" request
	When I make the "PractitionerRead" request
	Then the response status code should indicate success
		And the Response Resource should be a Practitioner
		And the Response should contain the ETag header matching the Resource Version Id

Scenario: Practitioner read valid response check caching headers exist
	Given I get the Practitioner for Practitioner Code "practitioner1"
		And I store the Practitioner
	Given I configure the default "PractitionerRead" request
	When I make the "PractitionerRead" request
	Then the response status code should indicate success
		And the required cacheing headers should be present in the response

Scenario: Practitioner read invalid response check caching headers exist
	Given I get the Practitioner for Practitioner Code "practitioner1"
		And I store the Practitioner
	Given I configure the default "PractitionerRead" request
		And I set the Interaction Id header to "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner3>"
	When I make the "PractitionerRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		And the required cacheing headers should be present in the response
	



@Manual
@ignore
Scenario: If provider sysstems allow the practitioner to be associated with multiple languages shown by communication element manual testing is required
		#communication - A language the practitioner is able to use in patient communication
@Manual
@ignore
Scenario: If provider supports versioning test that once a resource is updated that the old version can be retrieved

@Manual
@ignore
Scenario: If the provider supports active and inactive practitioners is this information visible within the returned practitioner resource

@Manual
@ignore
Scenario: Check that the optional fields are populated in the practitioner resource if they are available in the provider system
	# telecom - Telecom information for the practitioner, can be multiple instances for different types.
	# address - Address(s) for the Practitioner.
	# gender - Gender of the practitioner
	# practitionerRole - The list of Roles/Organizations that the Practitioner is associated with