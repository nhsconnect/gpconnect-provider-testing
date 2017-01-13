@fhir @accessrecord
Feature: AccessRecord

Background:
	Given I have the following patient records
		| Id                      | NHSNumber  |
		| patient1                | 9476719931 |
		| patient2                | 9476719974 |
		| patientNotInSystem      | 9999999999 |
		| patientNoSharingConsent | 9476719958 |

Scenario Outline: Retrieve the care record sections for a patient
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
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
	#| REF |
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
		And I am requesting the record for config patient "patient1"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Invalid record section requested
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "ZZZ" care record section for config patient "patient1"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Multiple record sections requested
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the "SUM" care record section
		And I am requesting the "ALL" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Multiple duplication record sections in request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the "SUM" care record section
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Record section with invalid system for codable concept
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the "SUM" care record section with system "http://GPConnectTest.nhs.net/ValueSet/record-section"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Request record sections with String type rather than CodableConcept
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the "SUM" care record section with a string parameter
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: No patient NHS number supplied
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the "SUM" care record section
		And I set the JWT header for getcarerecord with config patient "patient1"
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
		And the JSON response should be a OperationOutcome resource

Scenario: Invalid identifier system for patient NHS number
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1" of system "http://GPConnectTest.nhs.net/Id/identifierSystem"
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Multiple different NHS number parameters in request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the record for config patient "patient2"
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Duplicate NHS number parameters in request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the record for config patient "patient1"
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
		And the JSON response should be a OperationOutcome resource

Scenario: Request care record section with patientNHSNumber using String type value
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1" using a fhir string parameter
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario Outline: Time period specified for a care record section that can be filtered
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
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
	#| INV |
	#| PAT |
	#| REF |
	| SUM |

Scenario Outline: Time period specified for a care record section that must not be filtered
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
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
	| MED |
	| OBS |
	| PRB |
	
Scenario: Access blocked to care record as no patient consent
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "SUM" care record section for config patient "patientNoSharingConsent"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "403"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Request patient summary with parameters in oposite order to other tests
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I set a valid time period start and end date
		And I am requesting the "SUM" care record section
		And I am requesting the record for config patient "patient1"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource

Scenario: Request care record where request resource type is something other than Parameters
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "SUM" care record section for config patient "patient1"
	When I send a gpc.getcarerecord operation request with invalid resource type payload
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Invalid start date parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the "SUM" care record section
		And I set a time period parameter start date to "abcd" and end date to "2016"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Invalid end date parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the "SUM" care record section
		And I set a time period parameter start date to "2014" and end date to "abcd"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Time period where start date parameter is after end date parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the "SUM" care record section
		And I set a time period parameter start date to "2016" and end date to "2014"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Time period with only start date parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the "SUM" care record section
		And I set a time period parameter with start date "2012"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource

Scenario: Time period with only end date parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the "SUM" care record section
		And I set a time period parameter with end date "2016"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource

Scenario: Time period format start and end date only contain year and month
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the "SUM" care record section
		And I set a time period parameter start date to "2015-02" and end date to "2016-07"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource

Scenario Outline: response should be bundle containing all mandatory elements
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
	#| REF |
	| SUM |
	
Scenario Outline: response bundle should contain composition as the first entry
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
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
	#| REF |
	| SUM |

@ignore
Scenario: request contain the structure definition in the meta fields for the operation

@ignore
Scenario: response contains the structure definitions in the meta fields for all resources

Scenario Outline: composition contains generic mandatory fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
	| Code | Title | Display |
	| ADM  | Administrative Items | Administrative Items |
	| ALL  | Allergies and Sensitivities | Allergies and Sensitivities |
	| CLI  | Clinical Items | Clinical Items |
	| ENC  | Encounters | Encounters |
	| IMM  | Immunisations | Immunisations |
	#| INV  | Investigations | Investigations |
	| MED  | Medications | Medications |
	| OBS  | Observations | Observations |
	#| PAT  | Patient Details | Patient Details |
	| PRB  | Problems | Problems |
	#| REF  | Referrals | Referrals |
	| SUM  | Summary | Summary |


Scenario Outline: if composition contains type mandatory field fixed values should be correct
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
		#| REF  |
		| SUM  |

Scenario Outline: if composition contains class coding
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
		#| REF  |
		| SUM  |

Scenario Outline: composition contains subject referencing a patient resource in the bundle
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
		#| REF  |
		| SUM  |

Scenario Outline: if composition contains author, the device reference can be found in the bundle
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
		#| REF  |
		| SUM  |

Scenario Outline: if composition contains custodian referenece
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
		#| REF  |
		| SUM  |

@ignore
Scenario: patient is a valid fhir resource
# There is no need to check that the patient resource and included value sets are correct if included as this is done by the parse of the response within scenario above.
# The Fhir Patient object checks the values passed in are within the standard value sets as the values are mapped to an enum and throw an exception if the value does not map to a allowed value.
	
Scenario Outline: patient contains a valid identifiers
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
		#| REF  |
		| SUM  |

@ignore
Scenario: if patient contains name elements
# There is no need to check that the patient resource name element value sets are correct if included as this is done by the parse of the response within scenario above.
# The Fhir Patient object checks the values passed in are within the standard value sets as the values are mapped to an enum and throw an exception if the value does not map to a allowed value.

Scenario Outline: if patient contains telecom information
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
		#| REF  |
		| SUM  |

@ignore
Scenario: if patient contains gender
# There is no need to check that the patient gender value set is valid as this is done by the parse of the response within scenario above.
# The Fhir Patient object checks the values passed in are within the standard value sets as the values are mapped to an enum and throw an exception if the value does not map to a allowed value.

@ignore
Scenario: if patient contains address
# There is no need to check that the patient address value sets are valid as this is done by the parse of the response within scenario above.
# The Fhir Patient object checks the values passed in are within the standard value sets as the values are mapped to an enum and throw an exception if the value does not map to a allowed value.

Scenario Outline: if patient contains maritalStatus
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
		#| REF  |
		| SUM  |

Scenario Outline: if patient contains contact
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
		#| REF  |
		| SUM  |

Scenario Outline: if patient contins communicaiton
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
		#| REF  |
		| SUM  |

Scenario Outline: if patient contains practitioner as care provider
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
		#| REF  |
		| SUM  |

Scenario Outline: if patient contains managingOrganizaiton
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
		#| REF  |
		| SUM  |

Scenario Outline: patient does not contain disallowed fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
		#| REF  |
		| SUM  |

Scenario Outline: practitioner resource contains mandatory fields and does not include dissallowed fields
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
		#| REF  |
		| SUM  |

Scenario Outline: practitioner resource contains mandatory fields within optional elements
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
		#| REF  |
		| SUM  |

Scenario Outline: if practitioner resource contains a managing organization it must reference an organization within the response bundle
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
		#| REF  |
		| SUM  |

Scenario Outline: organization resource identifiers
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
		#| REF  |
		| SUM  |

Scenario Outline: organization resource element cardinality
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
		#| REF  |
		| SUM  |

Scenario Outline: organization resource internal reference
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
		#| REF  |
		| SUM  |

Scenario Outline: device resource element cardinality conformance
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
		#| REF  |
		| SUM  |

Scenario Outline: device resource type element values match specification
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
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
		#| REF  |
		| SUM  |

Scenario Outline: check all dateTime format variations are allowed
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "<Patient>"
		And I set a time period parameter start date to "<StartDateTime>" and end date to "<EndDateTime>"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the HTML in the response matches the Regex check "<RegexToCheck>"
	Examples:
		| Code | Patient  | StartDateTime             | EndDateTime               | RegexToCheck |
		| ADM  | patient1 | 2014                      | 2016                      | (.)*         |
		| ADM  | patient1 | 2014-02                   | 2016                      | (.)*         |
		| ADM  | patient1 | 2014-10-05                | 2016-08                   | (.)*         |
		| ADM  | patient1 | 2014-05                   | 2016-09-14                | (.)*         |
		| ADM  | patient1 | 2014-05-01T11:08:32       | 2016-12-08T09:22:16       | (.)*         |
		| ADM  | patient1 | 2015-10-23T11:08:32+00:00 | 2016-12-08T23:59:59+00:00 | (.)*         |
		| CLI  | patient1 | 2013                      | 2017                      | (.)*         |
		| CLI  | patient1 | 2014-02                   | 2016                      | (.)*         |
		| CLI  | patient1 | 2014-02-03                | 2016-01-24                | (.)*         |
		| CLI  | patient1 | 2014                      | 2016-06-01                | (.)*         |
		| CLI  | patient1 | 2015-11-28T22:53:01       | 2017-01-08T14:02:43       | (.)*         |
		| CLI  | patient1 | 2015-03-14T03:14:11+00:00 | 2016-08-03T18:32:43+00:00 | (.)*         |
		| ENC  | patient1 | 2015                      | 2017-01                   | (.)*         |
		| ENC  | patient1 | 2015-05                   | 2017-01-27                | (.)*         |
		| ENC  | patient1 | 2014-10-05                | 2016                      | (.)*         |
		| ENC  | patient1 | 2014-10-05                | 2016-08                   | (.)*         |
		| ENC  | patient1 | 2014-10-05                | 2016-09-01                | (.)*         |
		| ENC  | patient1 | 2015-11-28T18:22:01       | 2017-01-04T01:01:22       | (.)*         |
		| ENC  | patient1 | 2014-04-03T22:03:25+00:00 | 2016-03-13T17:13:12+00:00 | (.)*         |
		| SUM  | patient1 | 2012                      | 2017                      | (.)*         |
		| SUM  | patient1 | 2014-05                   | 2016-12-18                | (.)*         |
		| SUM  | patient1 | 2014-05-03                | 2016-12                   | (.)*         |
		| SUM  | patient1 | 2014-03-21                | 2016-12-14                | (.)*         |
		| SUM  | patient1 | 2015-02-28T09:20:14       | 2017-01-01T03:05:08       | (.)*         |
		| SUM  | patient1 | 2014-12-22T22:22:22+00:00 | 2016-06-06T06:08:06+00:00 | (.)*         |
	#	| INV ||||||
	#	| PAT ||||||
	#	| REF ||||||
	
Scenario Outline: invalid request parameter names and case
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "SUM" care record section for config patient "patient1"
		And I set a valid time period start and end date
		And I replace the parameter name "<ParamName>" with "<NewParamName>"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "<ExpectedResponseCode>"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource
	Examples: 
	| ParamName        | NewParamName      | ExpectedResponseCode |
	| patientNHSNumber | patientsNHSNumber | 422                  |
	| patientNHSNumber | patientnhsnumber  | 422                  |
	| patientNHSNumber | PATIENTNHSNUMBER  | 422                  |
	| recordSection    | recordSections    | 422                  |
	| recordSection    | RecordSection     | 422                  |
	| recordSection    | RECORDSECTION     | 422                  |
	| timePeriod       | time              | 422                  |
	| timePeriod       | TimePeriod        | 422                  |
	| timePeriod       | TIMEPERIOD        | 422                  |


Scenario: Request parameter patientNHSNumber values is empty
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "SUM" care record section for config patient "patient1"
		And I set the parameter patientNHSNumber with an empty value
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Request parameter patientNHSNumber system is empty
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "SUM" care record section for config patient "patient1"
		And I set the parameter patientNHSNumber with an empty system
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Request parameter recordSection values is empty
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "" care record section for config patient "patient1"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario: Request parameter recordSection system is empty
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "SUM" care record section for config patient "patient1"
		And I set the parameter recordSection with an empty system
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource

Scenario Outline: Requested section code incorrect parameter case
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON response should be a OperationOutcome resource
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
	#| ref |
	#| Ref |
	| sum |
	| Sum |
	| sUm |

@ignore
Scenario: Request records for patients with genders which do not match the valueset so must addear to gender mapping
	# Check that the gender returned matches the expected mapping

@ignore
@Manual
Scenario: Check that all the genders supported by provider are in the GP Connect value set, if not check mapping is covered in documentation and system maps correctly
	# Run tests with patients with non value set genders if possible and check mapping in response is acceptable

@ignore
Scenario: Request records for patients contact with relationship which do not match the valueset so must addear to relationship mapping
	# Check that the relationship returned matches the expected mapping

@ignore
@Manual
Scenario: Check that all the relationship supported for contacts by the provider are in the GP Connect value set, if not check mapping is covered in documentation and system maps correctly
	# Run tests with patients with non value set relationships for contacts and check mapping in response is acceptable

@ignore
Scenario: Identifier order in response resources
# The identifiers within the response resources have to appear in the correct order as per the specfication.

@ignore
Scenario: Patient with inactive nhs number in system should not return that NHS Number