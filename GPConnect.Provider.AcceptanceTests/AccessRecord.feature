@accessrecord @0.7.4-Full-Pack
Feature: AccessRecord

Background:
	Given I have the following patient records
		| Id                 | NHSNumber  |
		| patientNotInSystem | 9999999999 |
		| patient1           | 9000000001 |
		| patient2           | 9000000002 |
		| patient3           | 9000000003 |
		| patient4           | 9000000004 |
		| patient5           | 9000000005 |
		| patient6           | 9000000006 |
		| patient7           | 9000000007 |
		| patient8           | 9000000008 |
		| patient9           | 9000000009 |
		| patient10          | 9000000010 |
		| patient11          | 9000000011 |
		| patient12          | 9000000012 |
		| patient13          | 9000000013 |
		| patient14          | 9000000014 |
		| patient15          | 9000000015 |
		| patient16          | 9000000016 |
		| patient17          | 9000000017 |
		| patient18          | 9000000018 |

Scenario Outline: Retrieve the care record sections for a patient
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "<Code>" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
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
		And the JSON response should be a OperationOutcome resource

Scenario: No record section requested
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Invalid record section requested
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "ZZZ" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Multiple record sections requested
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "SUM" care record section
		And I am requesting the "ALL" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Multiple duplication record sections in request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "SUM" care record section
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Record section with invalid system for codable concept
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "SUM" care record section with system "http://GPConnectTest.nhs.net/ValueSet/record-section"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Request record sections with String type rather than CodableConcept
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "SUM" care record section with a string parameter
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: No patient NHS number supplied
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the "SUM" care record section
		And I set the JWT header for getcarerecord with config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Invalid NHS number supplied
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for patient with NHS Number "1234567891"
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource with error code "INVALID_NHS_NUMBER"

Scenario: Invalid identifier system for patient NHS number
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2" of system "http://GPConnectTest.nhs.net/Id/identifierSystem"
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource with error code "INVALID_IDENTIFIER_SYSTEM"

Scenario: Multiple different NHS number parameters in request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the record for config patient "patient3"
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Duplicate NHS number parameters in request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the record for config patient "patient2"
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: No patient found with NHS number
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "SUM" care record section for config patient "patientNotInSystem"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "404"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"

Scenario: Request care record section with patientNHSNumber using String type value
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2" using a fhir string parameter
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario Outline: Time period specified for a care record section that can be filtered
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "<Code>" care record section
		And I set a valid time period start and end date
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
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
		And the JSON response should be a OperationOutcome resource
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
		And the JSON response should be a OperationOutcome resource with error code "NO_PATIENT_CONSENT"
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
		And the JSON response should be a Bundle resource
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
		And the JSON response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Invalid start date parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "ENC" care record section
		And I set a time period parameter start date to "abcd" and end date to "2016"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Invalid end date parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "ENC" care record section
		And I set a time period parameter start date to "2014" and end date to "abcd"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Time period where start date parameter is after end date parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "ENC" care record section
		And I set a time period parameter start date to "2016" and end date to "2014"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Time period with only start date parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "ENC" care record section
		And I set a time period parameter with start date "2012"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource

Scenario: Time period with only end date parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient2"
		And I am requesting the "ENC" care record section
		And I set a time period parameter with end date "2016"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource

Scenario Outline: response should be bundle containing all mandatory elements
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the JSON response bundle should contain a single Composition resource
		And the JSON response bundle should contain a single Patient resource
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
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset
		And the JSON response bundle should contain the composition resource as the first entry
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
		And the JSON response should be a Bundle resource
		And the composition resource in the bundle should contain meta data profile
		And the patient resource in the bundle should contain meta data profile and version id
		And if the response bundle contains an organization resource it should contain meta data profile and version id
		And if the response bundle contains a practitioner resource it should contain meta data profile and version id
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

#25/07/2019 SJD Problems changed in 0.5.1 to Problems and Issues
Scenario Outline: composition contains generic mandatory fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "<Patient>"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And response bundle entry "Composition" should contain element "resource.date"
		And response bundle entry "Composition" should contain element "resource.title" with value "Patient Care Record"
		And response bundle entry "Composition" should contain element "resource.status" with value "final"
		And response bundle entry "Composition" should contain element "resource.section[0].title" with value "<Title>"
		And response bundle entry "Composition" should contain element "resource.section[0].code.coding[0].system" with value "http://fhir.nhs.net/ValueSet/gpconnect-record-section-1"
		And response bundle entry "Composition" should contain element "resource.section[0].code.coding[0].code" with value "<Code>"
		And response bundle entry "Composition" should contain element "resource.section[0].code.coding[0].display" with value "<Display>"
		And response bundle entry "Composition" should contain element "resource.section[0].code.text"
		And response bundle entry "Composition" should contain element "resource.section[0].text.status"
		And response bundle entry "Composition" should contain element "resource.section[0].text.div"
	Examples:
		| Patient  | Code | Title                           | Display                         |
		| patient1 | ADM  | Administrative Items            | Administrative Items            |
		| patient2 | ADM  | Administrative Items            | Administrative Items            |
		| patient1 | ALL  | Allergies and Adverse Reactions | Allergies and Adverse Reactions |
		| patient2 | ALL  | Allergies and Adverse Reactions | Allergies and Adverse Reactions |
		| patient1 | CLI  | Clinical Items                  | Clinical Items                  |
		| patient2 | CLI  | Clinical Items                  | Clinical Items                  |
		| patient1 | ENC  | Encounters                      | Encounters                      |
		| patient2 | ENC  | Encounters                      | Encounters                      |
		| patient1 | IMM  | Immunisations                   | Immunisations                   |
		| patient2 | IMM  | Immunisations                   | Immunisations                   |
		| patient1 | MED  | Medications                     | Medications                     |
		| patient2 | MED  | Medications                     | Medications                     |
		| patient1 | OBS  | Observations                    | Observations                    |
		| patient2 | OBS  | Observations                    | Observations                    |
		| patient1 | PRB  | Problems and Issues             | Problems and Issues             |
		| patient2 | PRB  | Problems and Issues             | Problems and Issues             |
		| patient1 | REF  | Referrals                       | Referrals                       |
		| patient2 | REF  | Referrals                       | Referrals                       |
		| patient1 | SUM  | Summary                         | Summary                         |
		| patient2 | SUM  | Summary                         | Summary                         |

Scenario Outline: if composition contains type mandatory field fixed values should be correct
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And if composition contains the resource type element the fields should match the fixed values of the specification
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
		And the JSON response should be a Bundle resource
		And if composition contains the resource class element the fields should match the fixed values of the specification
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
		And the JSON response should be a Bundle resource
		And response bundle entry "Composition" should contain element "resource.subject.reference"
		And response bundle entry "Composition" should contain element "resource.subject.reference" and that element should reference a resource in the bundle
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
		And the JSON response should be a Bundle resource
		And if response bundle entry "Composition" contains element "resource.author[0].reference"
		And response bundle entry "Composition" should contain element "resource.author[0].reference" and that element should reference a resource in the bundle
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

Scenario Outline: if composition contains custodian referenece
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And if response bundle entry "Composition" contains element "resource.custodian.reference"
		And response bundle entry "Composition" should contain element "resource.custodian.reference" and that element should reference a resource in the bundle
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
		And the JSON response should be a Bundle resource
		And response bundle entry "Patient" should contain element "resource.id"
		And response bundle Patient entry should contain a valid NHS number identifier
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
		And the JSON response should be a Bundle resource
		And if response bundle entry "Patient" contains element "resource.telecom"
		And response bundle Patient resource should contain valid telecom information
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
		And the JSON response should be a Bundle resource
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
		And the JSON response should be a Bundle resource
		And if composition contains the patient resource contact the mandatory fields should matching the specification
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
		And the JSON response should be a Bundle resource
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
		And the JSON response should be a Bundle resource
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
		And the JSON response should be a Bundle resource
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
		And the JSON response should be a Bundle resource
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
		And the JSON response should be a Bundle resource
		And if the response bundle contains a "Practitioner" resource
		And practitioner resources should contain a single name element
		And practitioner resources should not contain the disallowed elements
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
		And the JSON response should be a Bundle resource
		And if the response bundle contains a "Practitioner" resource
		And practitioner resources must only contain one user id and one profile id
		And if practitionerRole has role element which contains a coding then the system, code and display must exist
		And If the practitioner has communicaiton elemenets containing a coding then there must be a system, code and display element
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
		And the JSON response should be a Bundle resource
		And if the response bundle contains a "Practitioner" resource
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
		And the JSON response should be a Bundle resource
		And if the response bundle contains a "Organization" resource
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
		And the JSON response should be a Bundle resource
		And if the response bundle contains a "Organization" resource
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
		And the JSON response should be a Bundle resource
		And if the response bundle contains a "Organization" resource
		And if Organization includes partOf it should referene a resource in the response bundle
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
		And the JSON response should be a Bundle resource
		And if the response bundle contains a "Device" resource
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
		And the JSON response should be a Bundle resource
		And if the response bundle contains a "Device" resource
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
		And the JSON response should be a Bundle resource
		And the HTML in the response matches the Regex check "(.)*"
	Examples:
		| Code | StartDateTime             | EndDateTime               |
		| ADM  | 2014                      | 2016                      |
		| ADM  | 2014-02                   | 2016                      |
		| ADM  | 2014-10-05                | 2016-08                   |
		| ADM  | 2014-05                   | 2016-09-14                |
		| ADM  | 2015-10-23T11:08:32+00:00 | 2016-12-08T23:59:59+00:00 |
		| CLI  | 2013                      | 2017                      |
		| CLI  | 2014-02                   | 2016                      |
		| CLI  | 2014-02-03                | 2016-01-24                |
		| CLI  | 2014                      | 2016-06-01                |
		| CLI  | 2015-03-14T03:14:11+00:00 | 2016-08-03T18:32:43+00:00 |
		| ENC  | 2015                      | 2017-01                   |
		| ENC  | 2015-05                   | 2017-01-27                |
		| ENC  | 2014-10-05                | 2016                      |
		| ENC  | 2014-10-05                | 2016-08                   |
		| ENC  | 2014-10-05                | 2016-09-01                |
		| ENC  | 2014-04-03T22:03:25+00:00 | 2016-03-13T17:13:12+00:00 |
		| REF  | 2013                      | 2017                      |
		| REF  | 2014-02                   | 2016                      |
		| REF  | 2014-02-03                | 2016-01-24                |
		| REF  | 2014                      | 2016-06-01                |
		| REF  | 2015-03-14T03:14:11+00:00 | 2016-08-03T18:32:43+00:00 |
		| MED  | 2014                      | 2016                      |
		| MED  | 2014-02                   | 2016                      |
		| MED  | 2014-10-05                | 2016-08                   |
		| MED  | 2014-05                   | 2016-09-14                |
		| MED  | 2015-10-23T11:08:32+00:00 | 2016-12-08T23:59:59+00:00 |
		| OBS  | 2014                      | 2016                      |
		| OBS  | 2014-02                   | 2016                      |
		| OBS  | 2014-10-05                | 2016-08                   |
		| OBS  | 2014-05                   | 2016-09-14                |
		| OBS  | 2015-10-23T11:08:32+00:00 | 2016-12-08T23:59:59+00:00 |
		| PRB  | 2014                      | 2016                      |
		| PRB  | 2014-02                   | 2016                      |
		| PRB  | 2014-10-05                | 2016-08                   |
		| PRB  | 2014-05                   | 2016-09-14                |
		| PRB  | 2015-10-23T11:08:32+00:00 | 2016-12-08T23:59:59+00:00 |

Scenario Outline: check invalid dateTime format variations
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
		And I set a time period parameter start date to "<StartDateTime>" and end date to "<EndDateTime>"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource
	Examples:
		| Code | StartDateTime       | EndDateTime         |
		| ADM  | 2015-10-23T11:08:32 | 2016-12-08T23:59:59 |
		| CLI  | 2015-03-14T03:14:11 | 2016-08-03T18:32:43 |
		| ENC  | 2014-04-03T22:03:25 | 2016-03-13T17:13:12 |
		| REF  | 2015-03-14T03:14:11 | 2016-08-03T18:32:43 |
		| MED  | 2015-10-23T11:08:32 | 2016-12-08T23:59:59 |
		| OBS  | 2015-10-23T11:08:32 | 2016-12-08T23:59:59 |
		| PRB  | 2015-10-23T11:08:32 | 2016-12-08T23:59:59 |

Scenario Outline: invalid request parameter names and case
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "ENC" care record section for config patient "patient2"
		And I set a valid time period start and end date
		And I replace the parameter name "<ParamName>" with "<NewParamName>"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource
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
		And the JSON response should be a OperationOutcome resource with error code "INVALID_NHS_NUMBER"

Scenario: Request parameter patientNHSNumber system is empty
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "SUM" care record section for config patient "patient2"
		And I set the parameter patientNHSNumber with an empty system
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource with error code "INVALID_IDENTIFIER_SYSTEM"

Scenario: Request parameter recordSection values is empty
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario: Request parameter recordSection system is empty
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "SUM" care record section for config patient "patient2"
		And I set the parameter recordSection with an empty system
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource with error code "INVALID_PARAMETER"

Scenario Outline: Requested section code incorrect parameter case
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource with error code "INVALID_PARAMETER"
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
		And the JSON response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"
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


#Added By PG #192 20-3-2019

Scenario Outline: Ensure Retrieve the care record sections for senstive patients returns patient not found
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient9"
		And I am requesting the "<Code>" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate notfound
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"
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

@0.7.4-Only
	 #allowed access period is 28 days at the moment.
	Scenario Outline: Deceased Date is within  allowed access period post patient death returns patient record
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient18"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the response bundle contains a deceased date not older than allowed access period "28" days
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

@0.7.4-Only
	#Error checking when the request is received after allowed access period post patient death i.e 28 days
	Scenario Outline: When request is received after allowed access period post patient death returns patient not found
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient17"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "404"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"
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