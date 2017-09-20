@patient
Feature: PatientRegister

Scenario Outline: Register patient send request to incorrect URL
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the request URL to "<url>"
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "404"
		And the response should be a OperationOutcome resource with error code "REFERENCE_NOT_FOUND"
	Examples:
		| StartDate		| url                            |
		| 2017-05-05	| Patient/$gpc.registerpatien    |
		| 1999-01-22	| Patient/$gpc.registerpati#ent  |

Scenario Outline: Register patient with invalid interactionIds
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Interaction Id header to "<InteractionId>"
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| InteractionId                                                          |
		| urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments |
		| urn:nhs:nxames:services:gpconnect:fhir:operation:gpc.registerpatsssient |
		| urn:nhs:names:sservices:gpconnect:fhir:rest:create:appointment          |
		|                                                                        |
		| null                                                                   |

Scenario Outline: Register patient with missing header
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request with missing Header "<Header>"
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Header            |
		| Ssp-TraceID       |
		| Ssp-From          |
		| Ssp-To            |
		| Ssp-InteractionId |
		| Authorization     |

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
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
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
		And the response bundle should contain a single Patient resource
		And the Patient Metadata should be valid
		And the Patient Nhs Number Identifer should be valid
		And the Patient Registration Details Extension should be valid
		And the Patient Demographics should match the Stored Patient
	Examples:
		| ContentType           | ResponseFormat |
		| application/xml+fhir  | XML            |
		| application/json+fhir | JSON           |

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
		And the response bundle should contain a single Patient resource
		And the Patient Metadata should be valid
		And the Patient Nhs Number Identifer should be valid
		And the Patient Registration Details Extension should be valid
		And the Patient Demographics should match the Stored Patient
	Examples:
		| ContentType           | ResponseFormat |
		| application/xml+fhir  | XML            |
		| application/json+fhir | JSON           |

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
		And the response bundle should contain a single Patient resource
		And the Patient Metadata should be valid
		And the Patient Nhs Number Identifer should be valid
		And the Patient Registration Details Extension should be valid
		And the Patient Demographics should match the Stored Patient
	Examples:
		| ContentType           | AcceptHeader          | Format                | ResponseFormat |
		| application/xml+fhir  | application/xml+fhir  | application/xml+fhir  | XML            |
		| application/xml+fhir  | application/xml+fhir  | application/json+fhir | JSON           |
		| application/json+fhir | application/json+fhir | application/xml+fhir  | XML            |
		| application/xml+fhir  | application/json+fhir | application/json+fhir | JSON           |
		| application/json+fhir | application/xml+fhir  | application/xml+fhir  | XML            |
		| application/json+fhir | application/json+fhir | application/json+fhir | JSON           |
		| application/xml+fhir  | application/json+fhir | application/xml+fhir  | XML            |
		| application/json+fhir | application/xml+fhir  | application/json+fhir | JSON           |


Scenario: Register patient and check all elements conform to the gp connect profile
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain a single Patient resource
		And the Patient Metadata should be valid
		And the Patient Nhs Number Identifer should be valid
		And the Patient Registration Details Extension should be valid
		And the Patient Demographics should match the Stored Patient
		And the Patient Optional Elements should be valid

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

Scenario: Register patient with additional parameters but the valid patient parameter first
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add the Stored Patient as a parameter
		And I add the Stored Appointment as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient with duplicate parameters invalid first
	Given I create an Appointment for Patient "patient1" and Organization Code "ORG1"
		And I store the Created Appointment
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add the Stored Appointment as a parameter
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Register patient with invalid parameters name
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add the Stored Patient as a parameter with name "<ParameterName>"
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
	Examples:
	| ParameterName        |
	| invalidName          |
	| registerPatient test |
	| null                 |

Scenario: Register patient with missing parameters name
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add the Stored Patient as a parameter with name ""
	When I make the "RegisterPatient" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Register patient which alread exists on the system as a normal patient
	Given I get the Patient for Patient Value "patient1"
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

Scenario: Register patient with demographics which do not match spine PDS trace
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I set the Stored Patient Demographics to not match the NHS number
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient with no usual name
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I remove the Usual Name from the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient with an additional usual name
    Given I get the next Patient to register and store it
    Given I configure the default "RegisterPatient" request
        And I set the JWT Requested Record to the NHS Number of the Stored Patient
        And I add a Usual Name to the Stored Patient
        And I add the Stored Patient as a parameter
    When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain a single Patient resource
		And the Patient Metadata should be valid
		And the Patient Nhs Number Identifer should be valid
		And the Patient Registration Details Extension should be valid
		And the Patient Demographics should match the Stored Patient

Scenario: Register patient no family names
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I remove the Family Name from the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient with multiple family names
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the Stored Patient
		And I add the Family Name "AddedFamilyName" to the Stored Patient
		And I add the Family Name "AddedFSecondamilyName" to the Stored Patient
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
		| Deceased      |
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
		| Link          |
		| Photo         |

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
