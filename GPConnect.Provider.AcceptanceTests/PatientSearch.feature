@patient
Feature: PatientSearch

@ignore
Scenario: The provider system should accept the search parameter URL encoded
	# The API being used in the test suite encodes the parameter string by default so no additional test needs to be performed.
	# The FHIR and HTTP standards require the request to be URL encoded so it is mandated that clents encode their requests.

@ignore
Scenario: The response resources must be valid FHIR JSON or XML
	# This validation is done impliciitly by the parsing of the response XML or JSON into the FHIR resource used in most of the
	# test scenarios so no specific test needs to be implemented.

Scenario: Returned patients should contain a logical identifier
	Given I configure the default "PatientSearch" request
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add a Patient Identifier parameter with default System and Value "patient2"
	When I make the "PatientSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Patient Id should be valid

Scenario: Provider should return an error when no system is supplied in the identifier parameter
	Given I configure the default "PatientSearch" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
		And I add a Patient Identifier parameter with no System and Value "patient1"
	When I make the "PatientSearch" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Provider should return an error when a blank system is supplied in the identifier parameter
	Given I configure the default "PatientSearch" request
		And I set the JWT Requested Record to the NHS Number for "patient1"
		And I add a Patient Identifier parameter with System "" and Value "patient2"
	When I make the "PatientSearch" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: When a patient is not found on the provider system an empty bundle should be returned
	Given I configure the default "PatientSearch" request
		And I set the JWT Requested Record to the NHS Number for "patientNotInSystem"
		And I add a Patient Identifier parameter with default System and Value "patientNotInSystem"
	When I make the "PatientSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "0" entries

Scenario: Patient search should fail if no identifier parameter is include
	Given I configure the default "PatientSearch" request
		And I set the JWT Requested Record to the NHS Number for "patient2"
	When I make the "PatientSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: The identifier parameter should be rejected if the case is incorrect
	Given I configure the default "PatientSearch" request
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add a Patient Identifier parameter with identifier name "<ParameterName>" default System and Value "patient2"
	When I make the "PatientSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		Examples:
		| ParameterName |
		| idenddstifier |
		| Idenddstifier |
		| Identifier    |
		| identifiers   |

Scenario: The response should be an error if parameter is not identifier
	Given I configure the default "PatientSearch" request
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add a Patient Identifier parameter with identifier name "nhsNumberParam" default System and Value "patient2"
	When I make the "PatientSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: The response should be an error if no value is sent in the identifier parameter
	Given I configure the default "PatientSearch" request
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add the parameter "identifier" with the value "https://fhir.nhs.uk/Id/nhs-number|"
	When I make the "PatientSearch" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario Outline: The patient search endpoint should accept the accept header
	Given I configure the default "PatientSearch" request
        And I set the JWT Requested Record to the NHS Number for "patient2"
		And I set the Accept header to "<AcceptHeader>"
		And I add a Patient Identifier parameter with default System and Value "patient2"
	When I make the "PatientSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <ResultFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Patient Id should be valid
		And the Patient Identifiers should be valid for Patient "patient2"
	Examples:
		| AcceptHeader          | ResultFormat |
		| application/xml+fhir  | XML          |
		| application/json+fhir | JSON         |

Scenario Outline: The patient search endpoint should accept the format parameter
	 Given I configure the default "PatientSearch" request
        And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add the parameter "_format" with the value "<FormatParam>"
		And I add a Patient Identifier parameter with default System and Value "patient2"
	When I make the "PatientSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <ResultFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Patient Id should be valid
		And the Patient Identifiers should be valid for Patient "patient2"
	Examples:
		| FormatParam           | ResultFormat |
		| application/xml+fhir  | XML          |
		| application/json+fhir | JSON         |

Scenario Outline: The patient search endpoint should accept the format parameter after the identifier parameter
	 Given I configure the default "PatientSearch" request
        And I set the JWT Requested Record to the NHS Number for "patient2"
		And I set the Accept header to "<AcceptHeader>"
		And I add a Patient Identifier parameter with default System and Value "patient2"
		And I add the parameter "_format" with the value "<FormatParam>"
	When I make the "PatientSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <ResultFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Patient Id should be valid
		And the Patient Identifiers should be valid for Patient "patient2"
	Examples:
		| AcceptHeader          | FormatParam           | ResultFormat |
		| application/xml+fhir  | application/xml+fhir  | XML          |
		| application/json+fhir | application/xml+fhir  | XML          |
		| application/json+fhir | application/json+fhir | JSON         |
		| application/xml+fhir  | application/json+fhir | JSON         |

Scenario Outline: The patient search endpoint should accept the format parameter before the identifier parameter
	Given I configure the default "PatientSearch" request
        And I set the JWT Requested Record to the NHS Number for "patient2"
		And I set the Accept header to "<AcceptHeader>"
		And I add the parameter "_format" with the value "<FormatParam>"
		And I add a Patient Identifier parameter with default System and Value "patient2"
	When I make the "PatientSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR <ResultFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Patient Id should be valid
		And the Patient Identifiers should be valid for Patient "patient2"
	Examples:
		| AcceptHeader          | FormatParam           | ResultFormat |
		| application/xml+fhir  | application/xml+fhir  | XML          |
		| application/json+fhir | application/xml+fhir  | XML          |
		| application/json+fhir | application/json+fhir | JSON         |
		| application/xml+fhir  | application/json+fhir | JSON         |

Scenario Outline: Patient search failure due to invalid interactionId
	Given I configure the default "PatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient2"
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I set the Interaction Id header to "<InteractionId>"
	When I make the "PatientSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| InteractionId                                                     |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		| InvalidInteractionId                                              |
		|                                                                   |

Scenario Outline: Patient search failure due to missing header
	Given I configure the default "PatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient2"
		And I set the JWT Requested Record to the NHS Number for "patient2"
	When I make the "PatientSearch" request with missing Header "<Header>"
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Header            |
		| Ssp-TraceID       |
		| Ssp-From          |
		| Ssp-To            |
		| Ssp-InteractionId |
		| Authorization     |

Scenario: Patient resource should contain meta data elements
	Given I configure the default "PatientSearch" request
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add a Patient Identifier parameter with default System and Value "patient2"
	When I make the "PatientSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the patient resource in the bundle should contain meta data profile and version id

Scenario Outline: Patient resource should contain NHS number identifier returned as XML
	Given I configure the default "PatientSearch" request
		And I set the JWT Requested Record to the NHS Number for "<Patient>"
		And I set the Accept header to "application/xml+fhir"
		And I add a Patient Identifier parameter with default System and Value "<Patient>"
	When I make the "PatientSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR XML
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Patient Identifiers should be valid for Patient "<Patient>"
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |

Scenario Outline: Patient search response conforms with the GPConnect specification
	Given I configure the default "PatientSearch" request
		And I set the JWT Requested Record to the NHS Number for "<Patient>"
		And I add a Patient Identifier parameter with default System and Value "<Patient>"
	When I make the "PatientSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Patient CareProvider Practitioner should be valid and resolvable
		And the Patient Name should be valid
		And the Patient Use should be valid
		And the Patient Communication should be valid
		And the Patient Contact should be valid
		And the Patient MultipleBirth should be valid
		And the Patient MaritalStatus should be valid
		And the Patient Deceased should be valid
		And the Patient Telecom should be valid
		And the Patient ManagingOrganization Organization should be valid and resolvable
		And the Patient CareProvider Practitioner should be valid and resolvable
		And the Patient should exclude disallowed fields
	Examples:
		| Patient   |
		| patient1  |
		| patient2  |
		| patient3  |
		| patient4  |
		| patient5  |
		| patient6  |
		| patient18 |

Scenario Outline: Patient search decesed patient not returned
	Given I configure the default "PatientSearch" request
		And I set the JWT Requested Record to the NHS Number for "patient18"
		And I add a Patient Identifier parameter with default System and Value "patient18"
	When I make the "PatientSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "0" entries

Scenario: Conformance profile supports the Patient search operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the Conformance REST Resources should contain the "Patient" Resource with the "SearchType" Interaction

Scenario Outline: System should error if multiple parameters valid or invalid are sent
	 Given I configure the default "PatientSearch" request
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add a Patient Identifier parameter with identifier name "<Identifier1>" default System and Value "<PatientOne>"
		And I add a Patient Identifier parameter with identifier name "<Identifier2>" default System and Value "<PatientTwo>"
	When I make the "PatientSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Identifier1      | PatientOne | Identifier2       | PatientTwo |
		| identifier       | patient2   | identifier        | patient2   |
		| identifier       | patient1   | identifier        | patient2   |
		| identifier       | patient2   | identifier        | patient1   |
		| identifier       | patient2   | invalidIdentifier | patient2   |
		| randomIdentifier | patient2   | identifier        | patient2   |
		| identifier       | patient1   | identifiers       | patient2   |

Scenario: JWT requesting scope claim should reflect the operation being performed
	 Given I configure the default "PatientSearch" request
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add a Patient Identifier parameter with default System and Value "patient2"
		And I set the JWT requested scope to "organization/*.read"
	When I make the "PatientSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: JWT patient claim should reflect the patient being searched for
	Given I configure the default "PatientSearch" request
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add a Patient Identifier parameter with default System and Value "patient1"
	When I make the "PatientSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Patient Search include count and sort parameters
	Given I configure the default "PatientSearch" request
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add a Patient Identifier parameter with default System and Value "patient2"
		And I add the parameter "_count" with the value "1"
		And I add the parameter "_sort" with the value "status"
	When I make the "PatientSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
	

Scenario: Patient search valid response check caching headers exist
	Given I configure the default "PatientSearch" request
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add a Patient Identifier parameter with default System and Value "patient2"
	When I make the "PatientSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Patient Id should be valid
		And the required cacheing headers should be present in the response

Scenario:Patient search invalid response check caching headers exist
Given I configure the default "PatientSearch" request
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I add a Patient Identifier parameter with default System and Value "patient1"
	When I make the "PatientSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		And the required cacheing headers should be present in the response


@Manual
@ignore
Scenario: Test that if patient is part of a multiple birth that this is reflected in the patient resource with a boolean element only

@Manual
@ignore
Scenario: Check patients with contacts which contain multiple contacts and contacts with multiple names. There must be only one family name for contacts within the patient resource.