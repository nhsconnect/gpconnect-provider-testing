@Structured @StructuredDocuments @1.5.0-Full-Pack @1.5.0-IncrementalAndRegression
Feature: Documents

	# These Tests are only Testing this Structured Area in isolation and Not with other Areas or Combinations of Include Parameters
	# Tests around Multiple Structured Areas in one Request are tested in the MultipleRequests Feature

	##########################################
	#Search For Documents Tests
	##########################################

	Scenario Outline: Scenario Outline name: Search for Documents on a Patient with Documents
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "<Patient>"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should indicate success
		Given I store the Patient
		Given I configure the default "DocumentsSearch" request
		And I set the required parameters for a Documents Search call
		When I make the "DocumentsSearch" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And The Bundle id should match the SSPTraceID
		And I Check the returned DocumentReference is Valid
		And I Check the returned DocumentReference Do Not Include Not In Use Fields
		Examples:
			| Patient   |
			| patient2  |
			| patient40 |


	#This test will fail against the demonstrator. Till the demonstraor is updates
	@1.5.0-IncrementalAndRegression
	Scenario: Search for Documents on a Patient with Documents Over 5MB
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient4"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should indicate success
		Given I store the Patient
		Given I configure the default "DocumentsSearch" request
		And I set the required parameters for a Documents Search call
		When I make the "DocumentsSearch" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And The Bundle id should match the SSPTraceID
		And I Check the returned DocumentReference is Valid
		And I Check the returned DocumentReference Content Doesnot Contain URL for over 5MB Attachment

	Scenario: Search for Documents on a Patient with NO Documents
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient3"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should indicate success
		Given I store the Patient
		Given I configure the default "DocumentsSearch" request
		And I set the required parameters for a Documents Search call
		When I make the "DocumentsSearch" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And The Bundle id should match the SSPTraceID
		And The Bundle should contain NO Documents

	Scenario: Search for Documents without Mandatory include Params expect fail
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient2"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should indicate success
		Given I store the Patient
		Given I configure the default "DocumentsSearch" request
		When I make the "DocumentsSearch" request
		Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

	Scenario Outline: Search for Documents using author parameter
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "<Patient>"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should indicate success
		Given I store the Patient
		Given I configure the default "DocumentsSearch" request
		And I set the required parameters for a Documents Search call
		And I set the author parameters for a Documents Search call to "ORG1"
		When I make the "DocumentsSearch" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And The Bundle id should match the SSPTraceID
		And I Check the returned DocumentReference is Valid
		And I Check the returned DocumentReference Do Not Include Not In Use Fields
		Examples:
			| Patient   |
			| patient2  |
			| patient40 |

	Scenario: Search for Documents using author parameter but with invalid identifier
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient2"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should indicate success
		Given I store the Patient
		Given I configure the default "DocumentsSearch" request
		And I set the required parameters for a Documents Search call
		And I set the author parameters with an invalid identifier for a Documents Search call to "ORG1"
		When I make the "DocumentsSearch" request
		Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

	Scenario: Search for Patient Documents created within a last 365 days
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient2"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should indicate success
		Given I store the Patient
		Given I configure the default "DocumentsSearch" request
		And I set the required parameters for a Documents Search call
		Then I set the documents search parameters le to today and ge to 365 days ago
		When I make the "DocumentsSearch" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And The Bundle id should match the SSPTraceID

	Scenario Outline: Search for Patient Documents created less than a date
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "<Patient>"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should indicate success
		Given I store the Patient
		Given I configure the default "DocumentsSearch" request
		And I set the required parameters for a Documents Search call
		Then I set the created search parameter to less than "<Days>" days ago
		When I make the "DocumentsSearch" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And The Bundle id should match the SSPTraceID
		Examples:
			| Days | Patient   |
			| 2    | patient2  |
			| 2    | patient40 |

	Scenario Outline: Search for Patient Documents created greater than a date
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "<Patient>"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should indicate success
		Given I store the Patient
		Given I configure the default "DocumentsSearch" request
		And I set the required parameters for a Documents Search call
		Then I set the created search parameter to greater than "<Days>" days ago
		When I make the "DocumentsSearch" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And The Bundle id should match the SSPTraceID
		Examples:
			| Days | Patient   |
			| 365  | patient2  |
			| 365  | patient40 |

	Scenario Outline: Scenario Outline name: Search for Documents with an invalid parameter
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "<Patient>"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should indicate success
		Given I store the Patient
		Given I configure the default "DocumentsSearch" request
		And I set the required parameters for a Documents Search call
		And I set an invalid parameter for a Documents Search call
		When I make the "DocumentsSearch" request
		Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
		Examples:
			| Patient   |
			| patient2  |
			| patient40 |

	Scenario: Search for Documents on a Patient that doesnt exist
		Given I configure the default "DocumentsSearch" request
		And I set the required parameters for a Documents Search call
		And I change the patient logical id to a non existent id
		When I make the "DocumentsSearch" request
		Then the response status code should be "404"
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

	Scenario: Search for Documents on a patient which exists on the system as a temporary patient
		Given I get the next Patient to register and store it
		Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
		When I make the "RegisterPatient" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain a single Patient resource
		Given I store the Patient
		Given I configure the default "DocumentsSearch" request
		And I set the required parameters for a Documents Search call
		When I make the "DocumentsSearch" request
		Then the response status code should be "404"
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

	##########################################
	#Retrieve  Documents Tests
	##########################################

	Scenario Outline: Retrieve a Document for Patient2
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "<Patient>"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should indicate success
		Given I store the Patient
		Given I configure the default "DocumentsSearch" request
		And I set the required parameters for a Documents Search call
		When I make the "DocumentsSearch" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And I save a document url for retrieving later
		Given I configure the default "DocumentsRetrieve" request
		When I make the "DocumentsRetrieve" request
		Then the response status code should indicate success
		And I save the binary document from the retrieve
		And I Check the returned Binary Document is Valid
		And I Check the returned Binary Document Do Not Include Not In Use Fields
		Examples:
			| Patient   |
			| patient2  |
			| patient40 |

	##########################################
	#Documents Search/Find Patients Tests
	##########################################

	Scenario Outline: Documents Patient Search and check response conforms with the GPConnect specification
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "<Patient>"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And The Bundle id should match the SSPTraceID
		And the Patient Name should be valid
		And the Patient Gender should be valid
		And the Patient DOB should be valid
		And the Patient Use should be valid
		And the Patient Communication should be valid
		And the Patient Contact should be valid
		And the Patient MultipleBirth should be valid
		And the Patient MaritalStatus should be valid
		And the Patient Deceased should be valid
		And the Patient Telecom should be valid
		And the Patient ManagingOrganization Should be Valid
		And the Patient GeneralPractitioner Practitioner should be valid and resolvable
		And the Patient should exclude disallowed fields
		And the Patient Link should be valid and resolvable
		And the Patient Contact Telecom use should be valid
		And the Patient Not In Use should be valid
		Examples:
			| Patient   |
			| patient1  |
			| patient2  |
			| patient3  |
			| patient4  |
			| patient5  |
			| patient6  |
			| patient40 |

	Scenario Outline: Documents Patient Search with an invalid NHS number
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "<nhsNumber>"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "INVALID_NHS_NUMBER"
		Examples:
			| nhsNumber   |
			| 34555##4    |
			| hello       |
			| 999999999   |
			| 9000000008  |
			| 90000000090 |


	Scenario: Documents Patient Search results should contain a logical identifier
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient2"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Patient Id should be valid
		And the patient resource in the bundle should contain meta data profile and version id

	Scenario: Documents Patient Search should return an error when no system is supplied in the identifier parameter
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with no System and Value "patient1"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

	Scenario: Documents Patient Search should return an error when a blank system is supplied in the identifier parameter
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with System "" and Value "patient2"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

	Scenario: Documents Patient Search When a patient is not found on the provider system an empty bundle should be returned
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patientNotInSystem"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "0" entries
		And The Bundle id should match the SSPTraceID

	Scenario: Documents Patient Search should fail if no identifier parameter is include
		Given I configure the default "DocumentsPatientSearch" request
		When I make the "DocumentsPatientSearch" request
		Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

	Scenario: Documents Patient Search The identifier parameter should be rejected if the case is incorrect
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with identifier name "Identifier" default System and Value "patient2"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

	Scenario: Documents Patient Search response should be an error if no value is sent in the identifier parameter
		Given I configure the default "DocumentsPatientSearch" request
		And I add the parameter "identifier" with the value "https://fhir.nhs.uk/Id/nhs-number|"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

	Scenario Outline: The Documents Patient Search endpoint should accept the accept header
		Given I configure the default "DocumentsPatientSearch" request
		And I set the Accept header to "<AcceptHeader>"
		And I add a Patient Identifier parameter with default System and Value "patient2"
		When I make the "DocumentsPatientSearch" request
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

	Scenario Outline: The Documents patient search endpoint should accept the format parameter
		Given I configure the default "DocumentsPatientSearch" request
		And I add the parameter "_format" with the value "<FormatParam>"
		And I add a Patient Identifier parameter with default System and Value "patient2"
		When I make the "DocumentsPatientSearch" request
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

	Scenario Outline: The Documents patient search endpoint should accept the format parameter after the identifier parameter
		Given I configure the default "DocumentsPatientSearch" request
		And I set the Accept header to "<AcceptHeader>"
		And I add a Patient Identifier parameter with default System and Value "patient2"
		And I add the parameter "_format" with the value "<FormatParam>"
		When I make the "DocumentsPatientSearch" request
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

	Scenario Outline: The Documents Patient search endpoint should accept the format parameter before the identifier parameter
		Given I configure the default "DocumentsPatientSearch" request
		And I set the Accept header to "<AcceptHeader>"
		And I add the parameter "_format" with the value "<FormatParam>"
		And I add a Patient Identifier parameter with default System and Value "patient2"
		When I make the "DocumentsPatientSearch" request
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

	Scenario Outline: Documents Patient Search resource should contain NHS number identifier returned as XML
		Given I configure the default "DocumentsPatientSearch" request
		And I set the Accept header to "application/fhir+xml"
		And I add a Patient Identifier parameter with default System and Value "<Patient>"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should indicate success
		And the response body should be FHIR XML
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Patient Identifiers should be valid for Patient "<Patient>"
		Examples:
			| Patient   |
			| patient1  |
			| patient2  |
			| patient3  |
			| patient40 |

	Scenario: Documents Patient search response does not return deceased patient
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient18"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "0" entries
		And The Bundle id should match the SSPTraceID

	Scenario Outline: Documents Patient search should error if multiple parameters valid or invalid are sent
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with identifier name "<Identifier1>" default System and Value "<PatientOne>"
		And I add a Patient Identifier parameter with identifier name "<Identifier2>" default System and Value "<PatientTwo>"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		Examples:
			| Identifier1 | PatientOne | Identifier2 | PatientTwo |
			| identifier  | patient2   | identifier  | patient2   |
			| identifier  | patient1   | identifier  | patient2   |
			| identifier  | patient2   | identifier  | patient1   |

	Scenario: Documents Patient search valid response check caching headers exist
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient2"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Patient Id should be valid
		And the required cacheing headers should be present in the response

	Scenario: Documents Patient search invalid response check caching headers exist
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with identifier name "Identifier" default System and Value "patient2"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		And the required cacheing headers should be present in the response

	Scenario: Documents Patient search should contain a preferred branch
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient2"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Patient RegistrationDetails should include preferredBranchSurgery

	Scenario: Documents Patient search for a patient with a sensitive flag should return no results
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient9"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "0" entries

	Scenario: Documents Patient search for a patient with a inactive flag should return no results
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient21"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "0" entries

	Scenario: Documents Patient search for a No Consent Patient search gets a valid response
		Given I configure the default "DocumentsPatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient15"
		When I make the "DocumentsPatientSearch" request
		Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain "1" entries
		And the Patient Id should be valid
		And the required cacheing headers should be present in the response

