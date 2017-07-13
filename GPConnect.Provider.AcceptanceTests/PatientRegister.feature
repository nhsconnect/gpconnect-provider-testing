@patient
Feature: PatientRegister`

Scenario Outline: Register patient send request to incorrect URL
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the stored Patient Registration Period to "<StartDate>"
		And I set the stored Patient Registration Status to "A"
		And I set the stored Patient Registration Type to "T"
		And I set the request URL to "<url>"
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
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the stored Patient registration period with start date "2017-05-05" and end date "2018-11-12"
		And I set the stored Patient Registration Status to "A"
		And I set the stored Patient Registration Type to "T"
		And I am performing the "<interactionId>" interaction
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| interactionId                                                          |
		| urn:nhs:names:services:gpconnect:fhir:rest:search:patient_appointments |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatsssient |
		| urn:nhs:names:services:gpconnect:fhir:rest:create:appointment          |
		|                                                                        |
		| null                                                                   |

Scenario Outline: Register patient with missing header
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the stored Patient registration period with start date "2017-04-12" and end date "2018-12-24"
		And I set the stored Patient Registration Status to "A"
		And I set the stored Patient Registration Type to "T"
		And I do not send header "<Header>"
	When I make the "RegisterPatient" request
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
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the stored Patient registration period with start date "2017-04-12" and end date "2018-12-24"
		And I set the stored Patient Registration Status to "A"
		And I set the stored Patient Registration Type to "T"
		And I remove the patients identifiers from the stored patient
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "INVALID_NHS_NUMBER"

Scenario: Register patient without name element
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the stored Patient registration period with start date "2017-04-12" and end date "2018-12-24"
		And I set the stored Patient Registration Status to "A"
		And I set the stored Patient Registration Type to "T"
		And I remove the name element from the stored patient
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient without gender element
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the stored Patient registration period with start date "2017-04-12" and end date "2018-12-24"
		And I set the stored Patient Registration Status to "A"
		And I set the stored Patient Registration Type to "T"
		And I remove the gender element from the stored patient
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient without date of birth element
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the stored Patient registration period with start date "2017-04-12" and end date "2018-12-24"
		And I set the stored Patient Registration Status to "A"
		And I set the stored Patient Registration Type to "T"
		And I remove the DOB element from the stored patient
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Register patient with an invalid NHS number
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the stored Patient registration period with start date "2017-04-12" and end date "2018-12-24"
		And I set the stored Patient Registration Status to "A"
		And I set the stored Patient Registration Type to "T"
		And I remove the patients identifiers from the stored patient
		And I add the NHS Number identifier "<nhsNumber>" to the stored patient
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "INVALID_NHS_NUMBER"
	Examples:
		| nhsNumber   |
		| 34555##4    |
		|             |
		| hello       |
		| 999999999   |
		| 9999999990  |
		| 99999999999 |
		| 9000000008  |
		| 90000000090 |

Scenario Outline: Register Patient and use the Accept Header to request response format
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the stored Patient registration period with start date "2017-04-12" and end date "2018-12-24"
		And I set the stored Patient Registration Status to "A"
		And I set the stored Patient Registration Type to "T"
		And I set the request content type to "<ContentType>"
		And I set the Accept header to "<ContentType>"
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain a single Patient resource
		And the patient resource in the bundle should contain meta data profile and version id
		And the patient resources within the bundle should contain a registration period
		And the patient resources within the bundle should contain a registration status
		And the patient resources within the bundle should contain a registration type
		And the patient resources within the response bundle should contain the same demographic information as the stored patient
	Examples:
		| ContentType           | ResponseFormat |
		| application/xml+fhir  | XML            |
		| application/json+fhir | JSON           |

Scenario Outline: Register Patient and use the _format parameter to request the response format
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the stored Patient registration period with start date "2017-04-12" and end date "2018-12-24"
		And I set the stored Patient Registration Status to "A"
		And I set the stored Patient Registration Type to "T"
		And I set the request content type to "<ContentType>"
		And I add the parameter "_format" with the value "<ContentType>"
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain a single Patient resource
		And the patient resource in the bundle should contain meta data profile and version id
		And the patient resources within the bundle should contain a registration period
		And the patient resources within the bundle should contain a registration status
		And the patient resources within the bundle should contain a registration type
		And the patient resources within the response bundle should contain the same demographic information as the stored patient
	Examples:
		| ContentType           | ResponseFormat |
		| application/xml+fhir  | XML            |
		| application/json+fhir | JSON           |

Scenario Outline: Register Patient and use both the Accept header and _format parameter to request the response format
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the stored Patient registration period with start date "2017-04-12" and end date "2018-12-24"
		And I set the stored Patient Registration Status to "A"
		And I set the stored Patient Registration Type to "T"
		And I set the request content type to "<ContentType>"
		And I set the Accept header to "<AcceptHeader>"
		And I add the parameter "_format" with the value "<FormatParam>"
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain a single Patient resource
		And the patient resource in the bundle should contain meta data profile and version id
		And the patient resources within the bundle should contain a registration period
		And the patient resources within the bundle should contain a registration status
		And the patient resources within the bundle should contain a registration type
		And the patient resources within the response bundle should contain the same demographic information as the stored patient
	Examples:
		| ContentType           | AcceptHeader          | FormatParam           | ResponseFormat |
		| application/xml+fhir  | application/xml+fhir  | application/xml+fhir  | XML            |
		| application/json+fhir | application/json+fhir | application/json+fhir | JSON           |
		| application/xml+fhir  | application/xml+fhir  | application/json+fhir | JSON           |
		| application/json+fhir | application/json+fhir | application/xml+fhir  | XML            |
		| application/xml+fhir  | application/json+fhir | application/json+fhir | JSON           |
		| application/json+fhir | application/xml+fhir  | application/xml+fhir  | XML            |
		| application/xml+fhir  | application/xml+fhir  | application/xml+fhir  | XML            |
		| application/json+fhir | application/json+fhir | application/json+fhir | JSON           |
		| application/xml+fhir  | application/json+fhir | application/xml+fhir  | XML            |
		| application/json+fhir | application/xml+fhir  | application/json+fhir | JSON           |

Scenario: Register patient and check all elements conform to the gp connect profile with Extensions sent in a different order
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the stored Patient Registration Type to "T"
		And I set the stored Patient registration period with start date "2017-04-12" and end date "2018-12-24"
		And I set the stored Patient Registration Status to "A"
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain a single Patient resource
		And the patient resource in the bundle should contain meta data profile and version id
		And the patient resources within the bundle should contain a registration period
		And the patient resources within the bundle should contain a registration status
		And the patient resources within the bundle should contain a registration type
		And the patient resources within the response bundle should contain the same demographic information as the stored patient

Scenario: Register patient without registration period element
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the stored Patient Registration Type to "T"
		And I set the stored Patient Registration Status to "A"
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient without registration status code element
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the stored Patient Registration Type to "T"
		And I set the stored Patient registration period with start date "2017-04-12" and end date "2018-12-24"
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient without registration type element
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the stored Patient registration period with start date "2017-04-12" and end date "2018-12-24"
		And I set the stored Patient Registration Status to "A"
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient without registration period or type code elements
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the stored Patient Registration Status to "A"
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		
Scenario: Register patient without registration status code or registration type element
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the stored Patient registration period with start date "2017-04-12" and end date "2018-12-24"
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient without any extension elements
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient with duplicate extension
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the stored Patient registration period with start date "2017-04-12" and end date "2018-12-24"
		And I set the stored Patient Registration Status to "A"
		And I set the stored Patient Registration Type to "T"
		And I set the stored Patient Registration Status to "A"
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient with duplicate extension and missing extension
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the stored Patient registration period with start date "2017-04-12" and end date "2018-12-24"
		And I set the stored Patient registration period with start date "2017-04-12" and end date "2018-12-24"
		And I set the stored Patient Registration Type to "T"
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient with invalid bundle resource type
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the stored Patient registration period with start date "2017-04-12" and end date "2018-12-24"
		And I set the stored Patient Registration Status to "A"
		And I set the stored Patient Registration Type to "T"
	When I make the "RegisterPatient" request with invalid Resource type
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient with invalid patient resource type
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the stored Patient registration period with start date "2017-04-12" and end date "2018-12-24"
		And I set the stored Patient Registration Status to "A"
		And I set the stored Patient Registration Type to "T"
	When I make the "RegisterPatient" request with invalid parameter Resource type
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient with invalid patient resource with additional element
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT Requested Record to the NHS Number of the stored Patient
		And I set the stored Patient registration period with start date "2017-04-12" and end date "2018-12-24"
		And I set the stored Patient Registration Status to "A"
		And I set the stored Patient Registration Type to "T"
	When I make the "RegisterPatient" request with additional field in parameter Resource
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient with duplicate patient resource parameters
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "registerPatient"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I add the registration period with start date "2017-04-12" and end date "2018-12-24" to "registerPatient"
		And I add the resource stored against key "registerPatient" as a parameter named "registerPatient" to the request
		And I add the resource stored against key "registerPatient" as a parameter named "registerPatient" to the request
	When I send a gpc.registerpatient to create patient
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient with duplicate parameters valid first
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "registerPatient"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I add the registration period with start date "2017-04-12" and end date "2018-12-24" to "registerPatient"
		And I add the resource stored against key "registerPatient" as a parameter named "registerPatient" to the request
		And I am requesting the "SUM" care record section
	When I send a gpc.registerpatient to create patient
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient with duplicate parameters invalid first
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "registerPatient"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I add the registration period with start date "2017-04-12" and end date "2018-12-24" to "registerPatient"
		And I am requesting the "SUM" care record section
		And I add the resource stored against key "registerPatient" as a parameter named "registerPatient" to the request
	When I send a gpc.registerpatient to create patient
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Register patient with invalid parameters name
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "registerPatient"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I add the registration period with start date "2017-04-12" and end date "2018-12-24" to "registerPatient"
		And I add the resource stored against key "registerPatient" as a parameter named "<ParameterName>" to the request
	When I send a gpc.registerpatient to create patient
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
	Examples:
	| ParameterName        |
	| invalidName          |
	| registerPatient test |
	|                      |
	| null                 |

Scenario: Register patient which alread exists on the system as a normal patient
	Given I perform a patient search for patient "patient1" and store the first returned resources against key "registerPatient"
	Given I convert patient stored in "registerPatient" to a register temporary patient against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "registerPatient"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I add the registration period with start date "2017-04-12" and end date "2018-12-24" to "registerPatient"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient which alread exists on the system as a temporary patient
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "registerPatient"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration period with start date "2017-05-05" and end date "2018-09-12" to "registerPatient"
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain a single Patient resource
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"		
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient which is not on PDS
	Given I create a patient to register which does not exist on PDS and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "registerPatient"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I add the registration period with start date "2017-04-12" and end date "2018-12-24" to "registerPatient"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient with Prefer header representation response
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "registerPatient"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration period with start date "2017-05-05" and end date "2018-09-12" to "registerPatient"
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I set the Prefer header to "return=representation"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain a single Patient resource
		And the content-type should not be equal to null
		And the content-length should not be equal to zero
		And the response location header should resolve to a patient resource with matching details to stored patient "registerPatient"

Scenario: Register patient with Prefer header minimal response
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "registerPatient"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration period with start date "2017-05-05" and end date "2018-09-12" to "registerPatient"
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I set the Prefer header to "return=minimal"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should indicate success
		And the response body should be empty
		And the content-type should be equal to null
		And the response location header should resolve to a patient resource with matching details to stored patient "registerPatient"

Scenario: Multiple family names
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "registerPatient"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I add the registration period with start date "2017-04-12" and end date "2018-12-24" to "registerPatient"
		And I add the family name "AddFamilyName" to the patient stored against key "registerPatient"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Multiple given names
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "registerPatient"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I add the registration period with start date "2017-04-12" and end date "2018-12-24" to "registerPatient"
		And I add the given name "AddGivenName" to the patient stored against key "registerPatient"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Multiple Names
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "registerPatient"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I add the registration period with start date "2017-04-12" and end date "2018-12-24" to "registerPatient"
		And I add a name with given name "NewGivenName" and family name "NewFamilyName" to the patient stored against key "registerPatient"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Identifier without mandatory system elements
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "registerPatient"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I add the registration period with start date "2017-04-12" and end date "2018-12-24" to "registerPatient"
		And I add an identifier with no system element to stored patient "registerPatient"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Invalid registration period
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "registerPatient"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I add the registration period with start date "<StartDate>" and end date "<EndDate>" to "registerPatient"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
	Examples:
	| StartDate  | EndDate    |
	| abc        | 2018-12-24 |
	| 2017-08-24 | invalid    |
	| noEnd      |            |
	|            | noStart    |

Scenario: Registration period with only end date
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "registerPatient"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I add the registration period with start date "" and end date "2018-12-24" to "registerPatient"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain a single Patient resource
		And the bundle should contain a registration period

Scenario: Registration period with only start date
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "registerPatient"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I add the registration period with start date "2016-09-24" and end date "" to "registerPatient"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain a single Patient resource
		And the bundle should contain a registration period

Scenario Outline: Invalid registration status
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "registerPatient"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration status with code "<Code>" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I add the registration period with start date "2017-04-12" and end date "2018-12-24" to "registerPatient"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
	| Code     |
	| Z        |
	| Active   |
	| Inactive |
	| AA       |
	| OK       |
	|          |

Scenario Outline: Invalid registration type
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "registerPatient"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "<Code>" to "registerPatient"
		And I add the registration period with start date "2017-04-12" and end date "2018-12-24" to "registerPatient"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Code             |
		| Fully Registered |
		| Private          |
		| Temp             |
		| Private          |
		|                  |

Scenario Outline: Additional not allowed elements
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "registerPatient"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I add the registration period with start date "2017-04-12" and end date "2018-12-24" to "registerPatient"
		And I add a <ElementToAdd> element to patient stored against "registerPatient"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| ElementToAdd  |
		| active        |
		| telecom       |
		| deceased      |
		| address       |
		| marital       |
		| births        |
		| photo         |
		| contact       |
		| animal        |
		| communication |
		| careprovider  |
		| managingorg   |
		| link          |

Scenario Outline: JWT matches patient patient type request
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to the NHS number of patient stored against key "registerPatient"
		And I set the JWT requested scope to "<JWTType>"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I add the registration period with start date "2017-04-12" and end date "2018-12-24" to "registerPatient"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| JWTType              |
		| patient/*.read       |
		| organization/*.read  |
		| organization/*.write |

Scenario: JWT patient reference match payload patients nhs number
	Given I find the next patient to register and store the Patient Resource against key "registerPatient"
	Given I am using the default server
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.write"
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.registerpatient" interaction
		And I add the registration status with code "A" to "registerPatient"
		And I add the registration type with code "T" to "registerPatient"
		And I add the registration period with start date "2017-04-12" and end date "2018-12-24" to "registerPatient"
	When I send a gpc.registerpatient to create patient stored against key "registerPatient"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"