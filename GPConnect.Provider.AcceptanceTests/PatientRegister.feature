@patient
Feature: PatientRegister

Scenario Outline: Register patient send request to incorrect URL
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the request URL to "<url>"
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource
	Examples:
		| StartDate		| url                            |
		| 2017-05-05	| Patient/$gpc.registerpatien    |
		| 1999-01-22	| Patient/$gpc.registerpati#ent  |

Scenario: Register patient without sending identifier within patient
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I remove the Identifiers from the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "INVALID_NHS_NUMBER"

Scenario: Register patient without gender element
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I remove the Gender from the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response meta profile should be for "searchset"
		And the response bundle should contain a single Patient resource
		And the Patient Metadata should be valid
		And the Patient Nhs Number Identifer should be valid
		And the Patient Registration Details Extension should be valid
		And the Patient Demographics should match the Stored Patient

Scenario: Register patient without date of birth element
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I remove the Birth Date from the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Register patient with an invalid NHS number
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number "<nhsNumber>"
		And I remove the Identifiers from the Stored Patient
		And I add an Identifier with Value "<nhsNumber>" to the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "INVALID_NHS_NUMBER"
	Examples:
		| nhsNumber   |
		| 34555##4    |
		| hello       |
		| 999999999   |
		| 9000000008  |
		| 90000000090 |

Scenario Outline: Register Patient and use the Accept Header to request response format
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the request content type to "<ContentType>"
		And I set the Accept header to "<ContentType>"
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response meta profile should be for "searchset"
		And the response bundle should contain a single Patient resource
		And the Patient Metadata should be valid
		And the Patient Nhs Number Identifer should be valid
		And the Patient Registration Details Extension should be valid
		And the Patient Demographics should match the Stored Patient
	Examples:
		| ContentType           | ResponseFormat |
		| application/fhir+xml  | XML            |
		| application/fhir+json | JSON           |

Scenario Outline: Register Patient and use the _format parameter to request the response format
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the request content type to "<ContentType>"
		And I add a Format parameter with the Value "<ContentType>"
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response meta profile should be for "searchset"
		And the response bundle should contain a single Patient resource
		And the Patient Metadata should be valid
		And the Patient Nhs Number Identifer should be valid
		And the Patient Registration Details Extension should be valid
		And the Patient Demographics should match the Stored Patient
	Examples:
		| ContentType           | ResponseFormat |
		| application/fhir+xml  | XML            |
		| application/fhir+json | JSON           |

Scenario Outline: Register Patient and use both the Accept header and _format parameter to request the response format
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add a generic Identifier to the Stored Patient
		And I set the request content type to "<ContentType>"
		And I set the Accept header to "<AcceptHeader>"
		And I add a Format parameter with the Value "<Format>"
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response meta profile should be for "searchset"
		And the response bundle should contain a single Patient resource
		And the Patient Metadata should be valid
		And the Patient Nhs Number Identifer should be valid
		And the Patient Registration Details Extension should be valid
		And the Patient Demographics should match the Stored Patient
	Examples:
		| ContentType           | AcceptHeader          | Format                | ResponseFormat |
		| application/fhir+xml  | application/fhir+xml  | application/fhir+xml  | XML            |
		| application/fhir+xml  | application/fhir+xml  | application/fhir+json | JSON           |
		| application/fhir+json | application/fhir+json | application/fhir+xml  | XML            |
		| application/fhir+xml  | application/fhir+json | application/fhir+json | JSON           |
		| application/fhir+json | application/fhir+xml  | application/fhir+xml  | XML            |
		| application/fhir+json | application/fhir+json | application/fhir+json | JSON           |
		| application/fhir+xml  | application/fhir+json | application/fhir+xml  | XML            |
		| application/fhir+json | application/fhir+xml  | application/fhir+json | JSON           |


Scenario: Register patient and check all elements conform to the gp connect profile
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response meta profile should be for "searchset"
		And the response bundle should contain a single Patient resource
		And the Patient Metadata should be valid
		And the Patient Nhs Number Identifer should be valid
		And the Patient Registration Details Extension should be valid
		And the Patient Demographics should match the Stored Patient
		And the Patient Optional Elements should be valid
		And the Patient Link should be valid and resolvable

Scenario: Register patient with registration details extension
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I Set the Stored Patient Registration Details Extension 
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient with invalid bundle resource type
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request with invalid Resource type
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Register patient with invalid patient resource type
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request with invalid parameter Resource type
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Register patient with invalid patient resource with additional element
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request with additional field in parameter Resource
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient with duplicate patient resource parameters
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add the Stored Patient as a parameter
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient which alread exists on the system as a normal patient
	Given I get an existing patients nshNumber
		And I store the patient in the register patient resource format
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient which alread exists on the system as a temporary patient
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain a single Patient resource
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient which is not the Spine
	Given I create a Patient which does not exist on PDS and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient with no official name
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I remove the Official Name from the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Register Patient with multiple given names
	Given I get the next Patient to register and store it
    Given I configure the default "RegisterPatient" request
        And I set the JWT Requested Record to the NHS Number of the Stored Patient
        And I add "<ExtraGivenNames>" Given Names to the Stored Patient Official Name
        And I add the Stored Patient as a parameter
    When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response meta profile should be for "searchset"
		And the Patient Demographics should match the Stored Patient
	Examples: 
		| ExtraGivenNames |
		| 1               |
		| 2               |
		| 5               |

Scenario: Register patient no family names
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I remove the Family Name from the Active Given Name for the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient containing identifier without mandatory system elements
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add an Identifier with missing System to the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Register patient with additional valid elements
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add the Stored Patient as a parameter
		And I add a <ElementToAdd> element to the Stored Patient
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response meta profile should be for "searchset"
		And the response bundle should contain a single Patient resource
		And the Patient Metadata should be valid
		And the Patient Nhs Number Identifer should be valid
		And the Patient Registration Details Extension should be valid
		And the Patient Demographics should match the Stored Patient
	Examples:
		| ElementToAdd |
		| Active       |
		| Address      |
		| Name         |
		| Births        |
		| CareProvider  |
		| Contact       |
		| ManagingOrg   |
		| Marital       |
		| Telecom       |

Scenario Outline: Register patient with additional not allowed elements
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add the Stored Patient as a parameter
		And I add a <ElementToAdd> element to the Stored Patient
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| ElementToAdd  |
		| Animal        |
		| Communication |
		| Photo         |
		| Deceased      |

Scenario Outline: Register patient setting JWT request type to invalid type
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the JWT requested scope to "<JWTType>"
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| JWTType              |
		| patient/*.read       |
		| organization/*.read  |
		| organization/*.write |

Scenario: Register patient setting JWT patient reference so it does not match payload patient
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number "9999999999"
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register pateient valid response check caching headers exist
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response meta profile should be for "searchset"
		And the required cacheing headers should be present in the response

Scenario:Register pateient invalid response check caching headers exist
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I remove the Identifiers from the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "INVALID_NHS_NUMBER"
		And the required cacheing headers should be present in the response
