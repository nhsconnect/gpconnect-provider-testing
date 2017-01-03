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
		And the JSON value "resourceType" should be "Bundle"
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
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: No record section requested
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: Invalid record section requested
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "ZZZ" care record section for config patient "patient1"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: Multiple record sections requested
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the "SUM" care record section
		And I am requesting the "ALL" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: Multiple duplication record sections in request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the "SUM" care record section
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: Record section with invalid system for codable concept
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the "SUM" care record section with system "http://GPConnectTest.nhs.net/ValueSet/record-section"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: Request record sections with String type rather than CodableConcept
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the "SUM" care record section with a string parameter
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: No patient NHS number supplied
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the "SUM" care record section
		And I set the JWT header for getcarerecord with config patient "patient1"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: Invalid NHS number supplied
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for patient with NHS Number "1234567891"
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: Invalid identifier system for patient NHS number
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1" of system "http://GPConnectTest.nhs.net/Id/identifierSystem"
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: Multiple different NHS number parameters in request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the record for config patient "patient2"
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: Duplicate NHS number parameters in request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the record for config patient "patient1"
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: No patient found with NHS number
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "SUM" care record section for config patient "patientNotInSystem"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "404"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: Request care record section with patientNHSNumber using String type value
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1" using a fhir string parameter
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario Outline: Time period specified for a care record section that can be filtered
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the "<Code>" care record section
		And I set a valid time period start and end date
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Bundle"
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
		And the JSON value "resourceType" should be "OperationOutcome"
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
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: Request patient summary with parameters in oposite order to other tests
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I set a valid time period start and end date
		And I am requesting the "SUM" care record section
		And I am requesting the record for config patient "patient1"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Bundle"

Scenario: Request care record where request resource type is something other than Parameters
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "SUM" care record section for config patient "patient1"
	When I send a gpc.getcarerecord operation request with invalid resource type payload
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: Invalid start date parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the "SUM" care record section
		And I set a time period parameter start date to "abcd" and end date to "2016"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: Invalid end date parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the "SUM" care record section
		And I set a time period parameter start date to "2014" and end date to "abcd"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: Time period where start date parameter is after end date parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the "SUM" care record section
		And I set a time period parameter start date to "2016" and end date to "2014"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "422"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: Time period with only start date parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the "SUM" care record section
		And I set a time period parameter with start date "2012"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Bundle"

Scenario: Time period with only end date parameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the "SUM" care record section
		And I set a time period parameter with end date "2016"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Bundle"

Scenario: Time period format start and end date only contain year and month
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for config patient "patient1"
		And I am requesting the "SUM" care record section
		And I set a time period parameter start date to "2015-02" and end date to "2016-07"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Bundle"

Scenario Outline: response should be bundle containing all mandatory elements
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Bundle"
		And the JSON response bundle should contain the "Composition" resource
		And the JSON response bundle should contain the "Patient" resource
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
		And the JSON value "resourceType" should be "Bundle"
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
		And the JSON value "resourceType" should be "Bundle"
		And response bundle entry "Composition" should contain element "resource.date"
		And response bundle entry "Composition" should contain element "resource.title" with value "Patient Care Record"
		And response bundle entry "Composition" should contain element "resource.status" with value "final"
		And response bundle entry "Composition" should contain element "resource.type.coding[0].system" with value "http://snomed.info/sct"
		And response bundle entry "Composition" should contain element "resource.type.coding[0].code" with value "425173008"
		And response bundle entry "Composition" should contain element "resource.type.coding[0].display" with value "record extract (record artifact)"
		And response bundle entry "Composition" should contain element "resource.type.text" with value "record extract (record artifact)"
		And response bundle entry "Composition" should contain element "resource.class.coding[0].system" with value "http://snomed.info/sct"
		And response bundle entry "Composition" should contain element "resource.class.coding[0].code" with value "700232004"
		And response bundle entry "Composition" should contain element "resource.class.coding[0].display" with value "general medical service (qualifier value)"
		And response bundle entry "Composition" should contain element "resource.class.text" with value "general medical service (qualifier value)"
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


Scenario Outline: composition contains subject referencing a patient resource in the bundle
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Bundle"
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
		And the JSON value "resourceType" should be "Bundle"
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
		And the JSON value "resourceType" should be "Bundle"
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

Scenario Outline: patient contains a valid identifiers
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Bundle"
		And response bundle entry "Patient" should contain element "resource.id"
		And response bundle entry "Patient" should contain a valid NHS number identifier
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
	# Family
	# Given
	# text?

@ignore
Scenario: if patient contains telecom information
	# system
	# value
	#use

@ignore
Scenario: if patient contains gender
	# value should be text not numeric, one of the values

@ignore
Scenario: if patient contains address
	# Use
	# Type

@ignore
Scenario: if patient contains maritalStatus
	# System (http://hl7.org/fhir/ValueSet/marital-status)
	# Code is valid from valueset
	# Display from value set

@ignore
Scenario: if patient contains contect
	# if relationship - System, code, display
	# if name - Use, family, given, prefix?
	# if telecom - system, value, use
	# if address - etc

@ignore
Scenario: if patient contins communicaiton
	# System
	# Code
	# display

@ignore
Scenario: if patient contains practitioner as care provider
	# Practitioner Resource
	# Reference practitioner in bundle
	# Contains Display

@ignore
Scenario: if patient contains managingOrganizaiton
	# Organization Resource
	# Reference organization in bundle
	# Contains Display

@ignore
Scenario: patient does not contain disallowed fields
	# photo
	# animal
	# link

# Practitioner Resource Validation Scenarios

# Organization Resource Validation Scenarios

# Device Resource Validation

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
