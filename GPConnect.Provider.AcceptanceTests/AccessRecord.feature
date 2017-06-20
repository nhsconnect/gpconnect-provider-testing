@accessrecord
Feature: AccessRecord

@ignore
Scenario: patient is a valid fhir resource
# There is no need to check that the patient resource and included value sets are correct if included as this is done by the parse of the response within scenario above.
# The Fhir Patient object checks the values passed in are within the standard value sets as the values are mapped to an enum and throw an exception if the value does not map to a allowed value.

@ignore
Scenario: if patient contains name elements
# There is no need to check that the patient resource name element value sets are correct if included as this is done by the parse of the response within scenario above.
# The Fhir Patient object checks the values passed in are within the standard value sets as the values are mapped to an enum and throw an exception if the value does not map to a allowed value.

@ignore
Scenario: if patient contains gender
# There is no need to check that the patient gender value set is valid as this is done by the parse of the response within scenario above.
# The Fhir Patient object checks the values passed in are within the standard value sets as the values are mapped to an enum and throw an exception if the value does not map to a allowed value.

@ignore
Scenario: if patient contains address
# There is no need to check that the patient address value sets are valid as this is done by the parse of the response within scenario above.
# The Fhir Patient object checks the values passed in are within the standard value sets as the values are mapped to an enum and throw an exception if the value does not map to a allowed value.

Scenario Outline: Retrieve the care record sections for a patient
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "<Code>" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
	Examples:
		| Code |
		| ADM |
		| ALL |
		| CLI |
		| ENC |
		| IMM |
		#| INV |
		| MED |
		| OBS |
		#| PAT |
		| PRB |
		| REF |
		| SUM |

Scenario: Empty request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
	When I make a POST request to "/Patient/$gpc.getcarerecord"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: No record section requested
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: Invalid record section requested
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "ZZZ" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Multiple record sections requested
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "SUM" care record section
		And I am requesting the "ALL" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: Multiple duplication record sections in request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "SUM" care record section
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: Record section with invalid system for codable concept
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "SUM" care record section with system "http://GPConnectTest.nhs.net/ValueSet/record-section"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: Request record sections with String type rather than CodableConcept
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "SUM" care record section with a string parameter
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: No patient NHS number supplied
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the "SUM" care record section
		And I set the JWT header for getcarerecord with config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: Invalid NHS number supplied
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for patient with NHS Number "1234567891"
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_NHS_NUMBER"

Scenario: Invalid identifier system for patient NHS number
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2" of system "http://GPConnectTest.nhs.net/Id/identifierSystem"
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_IDENTIFIER_SYSTEM"

Scenario: Multiple different NHS number parameters in request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the record for config patient "patient3"
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: Duplicate NHS number parameters in request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the record for config patient "patient2"
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: No patient found with NHS number
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "SUM" care record section for config patient "patientNotInSystem"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "404"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

Scenario: Request care record section with patientNHSNumber using String type value
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2" using a fhir string parameter
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario Outline: Time period specified for a care record section that can be filtered
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "<Code>" care record section
		And I set a valid time period start and end date
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
	Examples:
		| Code |
		| ADM |
		| CLI |
		| ENC |
		| MED |
		| OBS |
		| PRB |
		#| INV |
		#| PAT |
		| REF |

Scenario Outline: Time period specified for a care record section that must not be filtered
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "<Code>" care record section
		And I set a valid time period start and end date
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource
	Examples:
		| Code |
		| ALL |
		| IMM |
		| SUM |

Scenario Outline: Access blocked to care record as no patient consent
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient15"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "403"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "NO_PATIENT_CONSENT"
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		| REF  |
		| SUM  |

Scenario Outline: Request patient summary with parameters in oposite order to other tests
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I set a valid time period start and end date
		And I am requesting the "<Code>" care record section
		And I am requesting the record for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
	Examples:
		| Code |
		| ADM  |
		| CLI  |
		| ENC  |
		#| INV  |
		#| PAT  |
		| REF  |

Scenario: Request care record where request resource type is something other than Parameters
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "SUM" care record section for config patient "patient2"
	When I send a gpc.getcarerecord operation request with invalid resource type payload
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Invalid start date parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "ENC" care record section
		And I set a time period parameter start date to "abcd" and end date to "2016"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Invalid end date parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "ENC" care record section
		And I set a time period parameter start date to "2014" and end date to "abcd"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Time period where start date parameter is after end date parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "ENC" care record section
		And I set a time period parameter start date to "2016" and end date to "2014"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Time period with only start date parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "ENC" care record section
		And I set a time period parameter with start date "2012"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"

Scenario: Time period with only end date parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "ENC" care record section
		And I set a time period parameter with end date "2016"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"

Scenario Outline: response should be bundle containing all mandatory elements
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And the response bundle should contain a single Composition resource
		And the response bundle should contain a single Patient resource
	Examples:
		| Code |
		| ADM |
		| ALL |
		| CLI |
		| ENC |
		| IMM |
		#| INV |
		| MED |
		| OBS |
		#| PAT |
		| PRB |
		| REF |
		| SUM |

Scenario Outline: response bundle should contain composition as the first entry
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And the response bundle should contain the composition resource as the first entry
	Examples:
		| Code |
		| ADM |
		| ALL |
		| CLI |
		| ENC |
		| IMM |
		#| INV |
		| MED |
		| OBS |
		#| PAT |
		| PRB |
		| REF |
		| SUM |

Scenario Outline: request contain the structure definition in the meta fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And the Composition Metadata should be valid
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And the Practitioner Metadata should be valid
		And if the response bundle contains a device resource it should contain meta data profile and version id
		And if the response bundle contains a location resource it should contain meta data profile and version id
	Examples:
		| Code |
		| ADM |
		| ALL |
		| CLI |
		| ENC |
		| IMM |
		#| INV |
		| MED |
		| OBS |
		#| PAT |
		| PRB |
		| REF |
		| SUM |

Scenario Outline: composition contains generic mandatory fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "<Patient>"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And the Composition should be valid
		And the Composition Section should be valid for "<Title>", "<Code>", "<Display>"
	Examples:
		| Patient  | Code     | Title                           | Display                         |
		| patient1 | ADM      | Administrative Items            | Administrative Items            |
		| patient2 | ADM      | Administrative Items            | Administrative Items            |
		| patient1 | ALL      | Allergies and Adverse Reactions | Allergies and Adverse Reactions |
		| patient2 | ALL      | Allergies and Adverse Reactions | Allergies and Adverse Reactions |
		| patient1 | CLI      | Clinical Items                  | Clinical Items                  |
		| patient2 | CLI      | Clinical Items                  | Clinical Items                  |
		| patient1 | ENC      | Encounters                      | Encounters                      |
		| patient2 | ENC      | Encounters                      | Encounters                      |
		| patient1 | IMM      | Immunisations                   | Immunisations                   |
		| patient2 | IMM      | Immunisations                   | Immunisations                   |
	#    | patient1 | INV      | Investigations                  | Investigations                  |
	#    | patient2 | INV      | Investigations                  | Investigations                  |
		| patient1 | MED      | Medications                     | Medications                     |
		| patient2 | MED      | Medications                     | Medications                     |
		| patient1 | OBS      | Observations                    | Observations                    |
		| patient2 | OBS      | Observations                    | Observations                    |
	#    | patient1 | PAT      | Patient Details                 | Patient Details                 |
	#    | patient2 | PAT      | Patient Details                 | Patient Details                 |
		| patient1 | PRB      | Problems                        | Problems                        |
		| patient2 | PRB      | Problems                        | Problems                        |
		| patient1 | REF      | Referrals                       | Referrals                       |
		| patient2 | REF      | Referrals                       | Referrals                       |
		| patient1 | SUM      | Summary                         | Summary                         |
		| patient2 | SUM      | Summary                         | Summary                         |

Scenario Outline: if composition contains type mandatory field fixed values should be correct
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And the Composition Type should be valid
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		| REF  |
		| SUM  |

Scenario Outline: if composition contains class coding
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And the Composition Class should be valid
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		| REF  |
		| SUM  |

Scenario Outline: composition contains subject referencing a patient resource in the bundle
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And the response bundle entry "Composition" should optionally contain element "resource.subject.reference" and that element should reference a resource in the bundle
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		| REF  |
		| SUM  |

Scenario Outline: if composition contains author, the device reference can be found in the bundle
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And the response bundle entry "Composition" should optionally contain element "resource.author[0].reference" and that element should reference a resource in the bundle
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		| REF  |
		| SUM  |

Scenario Outline: if composition contains custodian reference
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And the response bundle entry "Composition" should optionally contain element "resource.custodian.reference" and that element should reference a resource in the bundle
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		| REF  |
		| SUM  |

Scenario Outline: patient contains a valid identifiers
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And the response bundle entry "Patient" should contain element "resource.id"
		And the response bundle Patient entry should contain a valid NHS number identifier
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		| REF  |
		| SUM  |

Scenario Outline: if patient contains telecom information
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And the response bundle Patient resource should optionally contain valid telecom information
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		| REF  |
		| SUM  |

Scenario Outline: if patient contains maritalStatus
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And if composition contains the patient resource maritalStatus fields matching the specification
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		| REF  |
		| SUM  |

Scenario Outline: if patient contains contact
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And if care record composition contains the patient resource contact the mandatory fields should matching the specification
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		| REF  |
		| SUM  |

Scenario Outline: if patient contins communicaiton
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And if composition contains the patient resource communication the mandatory fields should matching the specification
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		| REF  |
		| SUM  |

Scenario Outline: if patient contains practitioner as care provider
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And if Patient careProvider is included in the response the reference should reference a Practitioner within the bundle
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		| REF  |
		| SUM  |

Scenario Outline: if patient contains managingOrganizaiton
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And if Patient managingOrganization is included in the response the reference should reference an Organization within the bundle
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		| REF  |
		| SUM  |

Scenario Outline: patient does not contain disallowed fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And patient resource should not contain the fhir fields photo animal or link
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		| REF  |
		| SUM  |

Scenario Outline: practitioner resource contains mandatory fields and does not include dissallowed fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And the Practitioner Name should be valid
		And the Practitioner Photo and Qualification should be excluded
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		| REF  |
		| SUM  |

Scenario Outline: practitioner resource contains mandatory fields within optional elements
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And the Practitioner Identifiers should be valid
		And the Practitioner PractitionerRoles Roles should be valid
		And if the practitioner has communicaiton elemenets containing a coding then there must be a system, code and display element
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		| REF  |
		| SUM  |

Scenario Outline: if practitioner resource contains a managing organization it must reference an organization within the response bundle
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And if practitioner contains a managingOrganization the reference relates to an Organization within the response bundle
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		| REF  |
		| SUM  |

Scenario Outline: organization resource identifiers
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And Organization resources identifiers must comply with specification identifier restricitions
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		| REF  |
		| SUM  |

Scenario Outline: organization resource element cardinality
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And if Organization includes type coding the elements are mandatory
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		| REF  |
		| SUM  |

Scenario Outline: organization resource internal reference
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And if Organization includes partOf it should reference a resource in the response bundle
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		| REF  |
		| SUM  |

Scenario Outline: device resource element cardinality conformance
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And the Device resource should conform to cardinality set out in specificaiton
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		| REF  |
		| SUM  |

Scenario Outline: device resource type element values match specification
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
		And the Device resource type should match the fixed values from the specfication
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		| REF  |
		| SUM  |

Scenario Outline: check all dateTime format variations are allowed
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
		And I set a time period parameter start date to "<StartDateTime>" and end date to "<EndDateTime>"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "document"
	Examples:
		| Code | StartDateTime             | EndDateTime               |
		| ADM  | 2014                      | 2016                      |
		| ADM  | 2014-02                   | 2016                      |
		| ADM  | 2014-10-05                | 2016-08                   |
		| ADM  | 2014-05                   | 2016-09-14                |
		| ADM  | 2014-05-01T11:08:32       | 2016-12-08T09:22:16       |
		| ADM  | 2015-10-23T11:08:32+00:00 | 2016-12-08T23:59:59+00:00 |
		| CLI  | 2013                      | 2017                      |
		| CLI  | 2014-02                   | 2016                      |
		| CLI  | 2014-02-03                | 2016-01-24                |
		| CLI  | 2014                      | 2016-06-01                |
		| CLI  | 2015-11-28T22:53:01       | 2017-01-08T14:02:43       |
		| CLI  | 2015-03-14T03:14:11+00:00 | 2016-08-03T18:32:43+00:00 |
		| ENC  | 2015                      | 2017-01                   |
		| ENC  | 2015-05                   | 2017-01-27                |
		| ENC  | 2014-10-05                | 2016                      |
		| ENC  | 2014-10-05                | 2016-08                   |
		| ENC  | 2014-10-05                | 2016-09-01                |
		| ENC  | 2015-11-28T18:22:01       | 2017-01-04T01:01:22       |
		| ENC  | 2014-04-03T22:03:25+00:00 | 2016-03-13T17:13:12+00:00 |
		| REF  | 2013                      | 2017                      |
		| REF  | 2014-02                   | 2016                      |
		| REF  | 2014-02-03                | 2016-01-24                |
		| REF  | 2014                      | 2016-06-01                |
		| REF  | 2015-11-28T22:53:01       | 2017-01-08T14:02:43       |
		| REF  | 2015-03-14T03:14:11+00:00 | 2016-08-03T18:32:43+00:00 |
		| MED  | 2014                      | 2016                      |
		| MED  | 2014-02                   | 2016                      |
		| MED  | 2014-10-05                | 2016-08                   |
		| MED  | 2014-05                   | 2016-09-14                |
		| MED  | 2014-05-01T11:08:32       | 2016-12-08T09:22:16       |
		| MED  | 2015-10-23T11:08:32+00:00 | 2016-12-08T23:59:59+00:00 |
		| OBS  | 2014                      | 2016                      |
		| OBS  | 2014-02                   | 2016                      |
		| OBS  | 2014-10-05                | 2016-08                   |
		| OBS  | 2014-05                   | 2016-09-14                |
		| OBS  | 2014-05-01T11:08:32       | 2016-12-08T09:22:16       |
		| OBS  | 2015-10-23T11:08:32+00:00 | 2016-12-08T23:59:59+00:00 |
		| PRB  | 2014                      | 2016                      |
		| PRB  | 2014-02                   | 2016                      |
		| PRB  | 2014-10-05                | 2016-08                   |
		| PRB  | 2014-05                   | 2016-09-14                |
		| PRB  | 2014-05-01T11:08:32       | 2016-12-08T09:22:16       |
		| PRB  | 2015-10-23T11:08:32+00:00 | 2016-12-08T23:59:59+00:00 |
#		 | INV  |                           |                           |
#		 | PAT  |                           |                           |

Scenario Outline: invalid request parameter names and case
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "ENC" care record section for config patient "patient2"
		And I set a valid time period start and end date
		And I replace the parameter name "<ParamName>" with "<NewParamName>"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource
	Examples:
		| ParamName        | NewParamName      |
		| patientNHSNumber | patientsNHSNumber |
		| patientNHSNumber | patientnhsnumber  |
		| patientNHSNumber | PATIENTNHSNUMBER  |
		| recordSection    | recordSections    |
		| recordSection    | RecordSection     |
		| recordSection    | RECORDSECTION     |
		| timePeriod       | time              |
		| timePeriod       | TimePeriod        |
		| timePeriod       | TIMEPERIOD        |

Scenario: Request parameter patientNHSNumber values is empty
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "SUM" care record section for config patient "patient2"
		And I set the parameter patientNHSNumber with an empty value
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_NHS_NUMBER"

Scenario: Request parameter patientNHSNumber system is empty
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "SUM" care record section for config patient "patient2"
		And I set the parameter patientNHSNumber with an empty system
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_IDENTIFIER_SYSTEM"

Scenario: Request parameter recordSection values is empty
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Request parameter recordSection system is empty
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "SUM" care record section for config patient "patient2"
		And I set the parameter recordSection with an empty system
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario Outline: Requested section code incorrect parameter case
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
	Examples:
		| Code |
		| adm |
		| Adm |
		| aDm |
		| all |
		| All |
		| AlL |
		| cli |
		| Cli |
		| enc |
		| Enc |
		| ENc |
		| imm |
		| Imm |
		| iMM |
		#| inv |
		#| Inv |
		| med |
		| Med |
		| mEd |
		| obs |
		| Obs |
		#| pat |
		#| Pat |
		| prb |
		| Prb |
		| ref |
		| Ref |
		| sum |
		| Sum |
		| sUm |

Scenario Outline: A patient is requested which is not on Spine but is on provider system
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient14"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "404"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"
	Examples:
		| Code |
		| ADM |
		| ALL |
		| CLI |
		| ENC |
		| IMM |
		#| INV |
		| MED |
		| OBS |
		#| PAT |
		| PRB |
		| REF |
		| SUM |

@ignore
Scenario: Identifier order in response resources
# The identifiers within the response resources have to appear in the correct order as per the specfication.

@ignore
@Manual
Scenario: Patients flag as sensitive should return a minimal patient resource which does not contain sensitive fields

@ignore
@Manual
Scenario: Patient with inactive nhs number in system should not return that NHS Number

@ignore
@Manual
Scenario: Request records for patients with genders which do not match the valueset so must addear to gender mapping
	# Check that the gender returned matches the expected mapping

@ignore
@Manual
Scenario: Check that all the genders supported by provider are in the GP Connect value set, if not check mapping is covered in documentation and system maps correctly
	# Run tests with patients with non value set genders if possible and check mapping in response is acceptable

@ignore
@Manual
Scenario: Request records for patients contact with relationship which do not match the valueset so must addear to relationship mapping
	# Check that the relationship returned matches the expected mapping

@ignore
@Manual
Scenario: Check that all the relationship supported for contacts by the provider are in the GP Connect value set, if not check mapping is covered in documentation and system maps correctly
	# Run tests with patients with non value set relationships for contacts and check mapping in response is acceptable

@ignore
@Manual
Scenario: Patient whos records are currently in transit