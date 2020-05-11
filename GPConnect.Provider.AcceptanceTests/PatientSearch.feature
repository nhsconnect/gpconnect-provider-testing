@patient @1.2.7-Full-Pack
Feature: PatientSearch

Scenario: Returned patients should contain a logical identifier
	Given I configure the default "PatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient2"
	When I make the "PatientSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Patient Id should be valid
		And the patient resource in the bundle should contain meta data profile and version id

Scenario: Provider should return an error when no system is supplied in the identifier parameter
	Given I configure the default "PatientSearch" request
		And I add a Patient Identifier parameter with no System and Value "patient1"
	When I make the "PatientSearch" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Provider should return an error when a blank system is supplied in the identifier parameter
	Given I configure the default "PatientSearch" request
		And I add a Patient Identifier parameter with System "" and Value "patient2"
	When I make the "PatientSearch" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: When a patient is not found on the provider system an empty bundle should be returned
	Given I configure the default "PatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patientNotInSystem"
	When I make the "PatientSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "0" entries

Scenario: Patient search should fail if no identifier parameter is include
	Given I configure the default "PatientSearch" request
	When I make the "PatientSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: The identifier parameter should be rejected if the case is incorrect
	Given I configure the default "PatientSearch" request
		And I add a Patient Identifier parameter with identifier name "Identifier" default System and Value "patient2"
	When I make the "PatientSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: The response should be an error if no value is sent in the identifier parameter
	Given I configure the default "PatientSearch" request
		And I add the parameter "identifier" with the value "https://fhir.nhs.uk/Id/nhs-number|"
	When I make the "PatientSearch" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario Outline: The patient search endpoint should accept the accept header
	Given I configure the default "PatientSearch" request
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
		| application/fhir+xml  | XML          |
		| application/fhir+json | JSON         |

Scenario Outline: The patient search endpoint should accept the format parameter
	 Given I configure the default "PatientSearch" request
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
		| application/fhir+xml  | XML          |
		| application/fhir+json | JSON         |

Scenario Outline: The patient search endpoint should accept the format parameter after the identifier parameter
	 Given I configure the default "PatientSearch" request
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
		| application/fhir+xml  | application/fhir+xml  | XML          |
		| application/fhir+json | application/fhir+xml  | XML          |
		| application/fhir+json | application/fhir+json | JSON         |
		| application/fhir+xml  | application/fhir+json | JSON         |

Scenario Outline: The patient search endpoint should accept the format parameter before the identifier parameter
	Given I configure the default "PatientSearch" request
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
		| application/fhir+xml  | application/fhir+xml  | XML          |
		| application/fhir+json | application/fhir+xml  | XML          |
		| application/fhir+json | application/fhir+json | JSON         |
		| application/fhir+xml  | application/fhir+json | JSON         |

Scenario Outline: Patient resource should contain NHS number identifier returned as XML
	Given I configure the default "PatientSearch" request
		And I set the Accept header to "application/fhir+xml"
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

@1.2.3
Scenario Outline: Patient search response conforms with the GPConnect specification
	Given I configure the default "PatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "<Patient>"
	When I make the "PatientSearch" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Patient Name should be valid
		And the Patient Use should be valid
		And the Patient Communication should be valid
		And the Patient Contact should be valid
		And the Patient MultipleBirth should be valid
		And the Patient MaritalStatus should be valid
		And the Patient Deceased should be valid
		And the Patient Telecom should be valid
		And the Patient ManagingOrganization Organization should be valid and resolvable
		And the Patient GeneralPractitioner Practitioner should be valid and resolvable
		And the Patient should exclude disallowed fields
		And the Patient Link should be valid and resolvable
		# git hub ref 121
		# RMB 25/10/2018
		And the Patient Contact Telecom use should be valid
		# git hub ref 120
		# RMB 25/10/2018
		And the Patient Not In Use should be valid

	Examples:
		| Patient   |
		| patient1  |
		| patient2  |
		| patient3  |
		| patient4  |
		| patient5  |
		| patient6  |

Scenario: Patient search response does not return deceased patient
	Given I configure the default "PatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient18"
	When I make the "PatientSearch" request
	Then the response status code should indicate success
	And the response body should be FHIR JSON
	And the response should be a Bundle resource of type "searchset"
	And the response bundle should contain "0" entries
	

Scenario: CapabilityStatement profile supports the Patient search operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement REST Resources should contain the "Patient" Resource with the "SearchType" Interaction

Scenario Outline: System should error if multiple parameters valid or invalid are sent
	 Given I configure the default "PatientSearch" request
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

Scenario: Patient Search include count and sort parameters
	Given I configure the default "PatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient2"
		And I add the parameter "_count" with the value "1"
		And I add the parameter "_sort" with the value "status"
	When I make the "PatientSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries

Scenario: Patient search valid response check caching headers exist
	Given I configure the default "PatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient2"
	When I make the "PatientSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Patient Id should be valid
		And the required cacheing headers should be present in the response

Scenario:Patient search invalid response check caching headers exist
Given I configure the default "PatientSearch" request
		And I add a Patient Identifier parameter with identifier name "Identifier" default System and Value "patient2"
	When I make the "PatientSearch" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		And the required cacheing headers should be present in the response

@1.2.3
Scenario: Returned patients should contain a preferred branch
	Given I configure the default "PatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient2"
	When I make the "PatientSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Patient RegistrationDetails should include preferredBranchSurgery

Scenario: When a patient on the provider system has sensitive flag
# github ref 103
# RMB 22/10/2018
	Given I configure the default "PatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient9"
	When I make the "PatientSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "0" entries

Scenario: When a patient on the provider system has inactive flag
# github ref 107 demonstrator 115
# RMB 22/10/2018
	Given I configure the default "PatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient21"
	When I make the "PatientSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "0" entries


	Scenario: No Consent Patient search gets a valid response
	Given I configure the default "PatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient15"
	When I make the "PatientSearch" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Patient Id should be valid
		And the required cacheing headers should be present in the response