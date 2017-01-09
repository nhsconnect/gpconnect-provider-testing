@fhir @accessrecord
Feature: AccessRecord

Background:
	Given I have the following patient records
		| Id                      | NHSNumber  |
		| patient1                | 9000000033 |
		| patient2                | 9000000009 |
		| patientNotInSystem      | 9999999999 |
		| patientNoSharingConsent | 9000000041 |

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
	Then the response status code should be "400"
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

@ignore
Scenario: practitioner resource test agains specification
	# Check that an included practitioner has a name element and only one
	# No Photo element
	# No Qualification Element
	# if the praactitioner has an identifier it is either a sds-user-id or a role-profile-id and there is always a system and value
	# if practitionerRole exists and if it contains a managingOrganization the managing organization must haave a reference within response bundle
	# if practitionerRole has role element which contains a coding then the system, code and display must exist
	# If the practitioner has a communicaiton elemenets containing a coding then there must be a system, code and display element. There must only be one coding per communication element.

@ignore
Scenario: organization resource test agains specification
	# There should only be one ods-organization-code identifier element if included
	# There should only be one ods-site-code identifier element if included
	# If organization type includes a coding, there should only be one instance and that should include a system, code and display.
	# if partOf is included it should reference an Organization reference within the bundle
	# if an addresss is included there should only be one address

@ignore
Scenario: Device resource test agains specification


@ignore
Scenario: check all dateTime format variations are allowed
	// https://www.hl7.org/fhir/datatypes.html#dateTime

@ignore
Scenario: invalid request patientNHSNumber parameter names
	# Create Scenario with variation on parameter name

@ignore
Scenario: invalid request patientNHSNumber parameter case
	# Create Scenario with variation on parameter case

@ignore
Scenario: invalid request recordSection parameter names
	# Create Scenario with variation on parameter name

@ignore
Scenario: invalid request recordSection parameter case
	# Create Scenario with variation on parameter case

@ignore
Scenario: invalid request timePeriod parameter names
	# Create Scenario with variation on parameter name

@ignore
Scenario: invalid request timePeriod parameter case
	# Create Scenario with variation on parameter case

@ignore
Scenario: Request parameter patientNHSNumber values is empty
	# Send the parameter but leave the value empty

@ignore
Scenario: Request parameter patientNHSNumber system is empty
	# Send the parameter but leave the system empty

@ignore
Scenario: Request parameter recordSection values is empty
	# Send the parameter but leave the value empty

@ignore
Scenario: Request parameter recordSectoin system is empty
	# Send the parameter but leave the system empty

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
