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
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "patient2"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And all search response entities in bundle should contain a logical identifier

Scenario: Provider should return an error when an invalid system is supplied in the identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "patient1" with system "http://test.net/types/internalIdentifier"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_IDENTIFIER_SYSTEM"

Scenario: Provider should return an error when no system is supplied in the identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "patient1" without system in identifier parameter
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Provider should return an error when a blank system is supplied in the identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "patient1" with system ""
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: When a patient is not found on the provider system an empty bundle should be returned
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patientNotInSystem"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "patientNotInSystem"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "0" entries

Scenario: Patient search should fail if no identifier parameter is include
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
		And I set the JWT requested scope to "patient/*.read"
	When I make a GET request to "/Patient"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: The identifier parameter should be rejected if the case is incorrect
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "patient2" with parameter name "IdentIfier" and system "http://fhir.nhs.net/Id/nhs-number"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: The response should be an error if parameter is not identifier
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "patient2" with parameter name "nhsNumberParam" and system "http://fhir.nhs.net/Id/nhs-number"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: The response should be an error if no value is sent in the identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
		And I set the JWT requested scope to "patient/*.read"
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/nhs-number|"
	When I make a GET request to "/Patient"
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario Outline: The patient search endpoint should accept the format parameter after the identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
		And I set the JWT requested scope to "patient/*.read"
		And I set the Accept header to "<AcceptHeader>"
		And I add the parameter "identifier" with system "http://fhir.nhs.net/Id/nhs-number" for patient "patient2"
		And I add the parameter "_format" with the value "<FormatParam>"
	When I make a GET request to "/Patient"
	Then the response status code should indicate success
		And the response body should be FHIR <ResultFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
	Examples:
		| AcceptHeader          | FormatParam           | ResultFormat |
		| application/xml+fhir  | application/xml+fhir  | XML          |
		| application/json+fhir | application/xml+fhir  | XML          |
		| application/json+fhir | application/json+fhir | JSON         |
		| application/xml+fhir  | application/json+fhir | JSON         |
	
Scenario Outline: The patient search endpoint should accept the format parameter before the identifier parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
		And I set the JWT requested scope to "patient/*.read"
		And I set the Accept header to "<AcceptHeader>"
		And I add the parameter "_format" with the value "<FormatParam>"
		And I add the parameter "identifier" with system "http://fhir.nhs.net/Id/nhs-number" for patient "patient2"
	When I make a GET request to "/Patient"
	Then the response status code should indicate success
		And the response body should be FHIR <ResultFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
	Examples:
		| AcceptHeader          | FormatParam           | ResultFormat |
		| application/xml+fhir  | application/xml+fhir  | XML          |
		| application/json+fhir | application/xml+fhir  | XML          |
		| application/json+fhir | application/json+fhir | JSON         |
		| application/xml+fhir  | application/json+fhir | JSON         |

Scenario Outline: Patient search failure due to invalid interactionId
	Given I am using the default server
		And I am performing the "<InteractionId>" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
	When I search for Patient "patient2"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| InteractionId                                                     |
		| urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord |
		| InvalidInteractionId                                              |
		|                                                                   |

Scenario Outline: Patient search failure due to missing header
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
		And I set the JWT requested scope to "patient/*.read"
		And I do not send header "<Header>"
	When I search for Patient "patient2"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Header            |
		| Ssp-TraceID       |
		| Ssp-From          |
		| Ssp-To            |
		| Ssp-InteractionId |
		| Authorization     |

Scenario: Patient resource should contain meta data elements
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "patient1"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the patient resource in the bundle should contain meta data profile and version id

Scenario Outline: Patient resource should contain NHS number identifier
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "<Patient>"
		And I set the JWT requested scope to "patient/*.read"
		And I set the Accept header to "application/xml+fhir"
	When I search for Patient "<Patient>"
	Then the response status code should indicate success
		And the response body should be FHIR XML
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the response bundle Patient entries should contain a single NHS Number identifier for patient "<Patient>"
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |

Scenario Outline: Patient resource should not contain disallowed fields in resource
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "<Patient>"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "<Patient>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Patient should exclude fields
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |

Scenario Outline: Check that if a care provider exists in the Patient resource it is valid and there is only one
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "<Patient>"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "<Patient>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And if Patient resource contains careProvider the reference must be valid
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |

Scenario Outline: Check that if a managingOrganization exists in the Patient resource that it is a valid reference
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "<Patient>"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "<Patient>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And if Patient resource contains a managing organization the reference must be valid
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |

Scenario Outline: If patient contains telecom contacts the contact must contain mandatory fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "<Patient>"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "<Patient>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And if patient resource contains telecom element the system and value must be populated
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |

Scenario Outline: If patient deceased is present it should be a dateTime and not a boolean
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "<Patient>"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "<Patient>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And if patient resource contains deceased element it should be dateTime and not boolean
	Examples:
		| Patient   |
		| patient1  |
		| patient2  |
		| patient3  |
		| patient17 |

Scenario Outline: If patient contains marital status it must contain system code and display elements
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "<Patient>"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "<Patient>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Patient MaritalStatus should be valid
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |

Scenario Outline: If patient contains multiple birth field it must be a boolean type field
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "<Patient>"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "<Patient>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And if composition contains the patient resource and it contains the multiple birth field it should be a boolean value
	Examples:
		| Patient   |
		| patient1  |
		| patient2  |
		| patient17 |

Scenario Outline: If patient contains contact the relationship code display and system should be valid
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "<Patient>"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "<Patient>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And if composition contains the patient resource contact the mandatory fields should matching the specification
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |

Scenario Outline: If patient contains communication element the system and values should be valid
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "<Patient>"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "<Patient>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Patient Communication should be valid
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |

Scenario Outline: If patient contains care provider the reference should be valid
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "<Patient>"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "<Patient>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And if careProvider is present in patient resource the reference should be valid
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |

Scenario Outline: If patient contains managing organization the reference should be valid
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "<Patient>"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "<Patient>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And if managingOrganization is present in patient resource the reference should be valid
	Examples:
		| Patient  |
		| patient1 |
		| patient2 |
		| patient3 |

Scenario: Conformance profile supports the Patient search operation
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the conformance profile should contain the "Patient" resource with a "search-type" interaction

Scenario Outline: System should error if multiple parameters valid or invalid are sent
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient2"
		And I set the JWT requested scope to "patient/*.read"
		And I add the parameter "<Identifier1>" with system "http://fhir.nhs.net/Id/nhs-number" for patient "<PatientOne>"
		And I add the parameter "<Identifier2>" with system "http://fhir.nhs.net/Id/nhs-number" for patient "<PatientTwo>"
	When I make a GET request to "/Patient"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| Identifier1      | PatientOne | Identifier2       | PatientTwo |
		| identifier       | patient2   | identifier        | patient2   |
		| identifier       | patient1   | identifier        | patient2   |
		| identifier       | patient2   | identifier        | patient1   |
		| identifier       | patient2   | invalidIdentifier | patient2   |
		| randomIdentifier | patient2   | identifier        | patient2   |
	
Scenario: JWT requesting scope claim should reflect the operation being performed
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "organization/*.read"
	When I search for Patient "patient1"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: JWT patient claim should reflect the patient being searched for
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
		And I set the JWT requested record NHS number to config patient "patient1"
		And I set the JWT requested scope to "patient/*.read"
	When I search for Patient "patient2"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

@Manual
@ignore
Scenario: Test that if patient is part of a multiple birth that this is reflected in the patient resource with a boolean element only

@Manual
@ignore
Scenario: Check patients with contacts which contain multiple contacts and contacts with multiple names. There must be only one family name for contacts within the patient resource.