@fhir @accessrecord
Feature: AccessRecord

Scenario: Retrieve a care record section for a patient
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "SUM" care record section for patient with NHS Number "9000000033"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "Bundle"

Scenario Outline: Retrieve the care record sectons for a patient
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for patient with NHS Number "9000000033"
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
	| IMM |
	| INV |
	| MED |
	| OBS |
	| PAT |
	| PRB |
	| REF |
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
		And I am requesting the record for patient with NHS Number "9000000033"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: Invalid record section requested
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "ZZZ" care record section for patient with NHS Number "9000000033"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: Multiple record sections requested
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for patient with NHS Number "9000000033"
		And I am requesting the "SUM" care record section
		And I am requesting the "ALL" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: Multiple duplication record sections in request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for patient with NHS Number "9000000033"
		And I am requesting the "SUM" care record section
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: Record section with invalid system for codable concept
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for patient with NHS Number "9000000033"
		And I am requesting the "SUM" care record section with system "http://GPConnectTest.nhs.net/ValueSet/record-section"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: Request record sections with String type rather than CodableConcept
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for patient with NHS Number "9000000033"
		And I am requesting the "SUM" care record section with a string parameter
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: No patient NHS number supplied
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the "SUM" care record section
		And I set the JWT header for getcarerecord patient with nhs number "9000000033"
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
		And I am requesting the record for patient with identifier "9000000033" of system "http://GPConnectTest.nhs.net/Id/identifierSystem"
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: Multiple different NHS number parameters in request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for patient with NHS Number "9000000009"
		And I am requesting the record for patient with NHS Number "9000000033"
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

Scenario: Duplicate NHS number parameters in request
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I am requesting the record for patient with NHS Number "9000000033"
		And I am requesting the record for patient with NHS Number "9000000033"
		And I am requesting the "SUM" care record section
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the JSON value "resourceType" should be "OperationOutcome"

@ignore
Scenario: No patient found with NHS number

@ignore
Scenario: Request care record section with patientNHSNumber using String type value

@ignore
Scenario: Time period specified for a care record section that can be filtered
	# make sure that these sections accept the time period parameter

@ignore
Scenario: Access blocked to care record as no patient consent

@ignore
Scenario: Request patient summary with parameters in oposite order to other tests

@ignore
Scenario: Request care record where request resource type is something other than Parameters

@ignore
Scenario: Time period specified for a care record section that must not be filtered

@ignore
Scenario: Invalid start date parameter

@ignore
Scenario: Invalid end date parameter

@ignore
Scenario: Time period where start date parameter is after end date parameter

@ignore
Scenario: Time period with only start date parameter

@ignore
Scenario: Time period with only end date parameter

@ignore
Scenario: response should be bundle containing all mandatory elements
	# Composition
	# Patient
	
@ignore
Scenario: response bundle should contain composition as the first element
	# The first entry should be the composition

@ignore
Scenario: response conatin the structure definitions in the meta fields for all resources

@ignore
Scenario: composition contains generic mandatory fields
	# Date field 
	# Title (Patient Care Record)
	# Status 
		# System (http://hl7.org/fhir/ValueSet/composition-status)
		# value (Final)
	# Section 
		#Title 
		# Code - System, Code, Display and Text
		# Text - status and div elements

@ignore
Scenario: composition contains type element
	# System (http://fhir.nhs.net/ValueSet/document-type-codes-snct-1)
	# Contains System (http://snomed.info/sct)
	# Code (425173008)
	# Display (record extract (record artifact))
	# Text (record extract (record artifact))

@ignore
Scenario: composition contains subject referencing a patient resource in the bundle
	# Contains Subject
	# Contains Reference
	# Contains Display
	# Referenced patient is in bundle

@ignore
Scenario: if composition contains class element
	# System (http://fhir.nhs.net/ValueSet/care-setting-codes-snct-1)
	# Contains System (http://snomed.info/sct)
	# Code (700232004)
	# Display (general medical service (qualifier value))
	# Text (general medical service (qualifier value))

@ignore
Scenario: if composition contains author, the device reference can be found in the bundle
	# Contains Reference
	# Referenced device is in bundle
	# Contains Display

@ignore
Scenario: if composition contains custodian referenece
	# Contains Reference
	# Contains Display
	# Referenced organization is in bundle

@ignore
Scenario: patient contains a valid identifiers
	# Internal Id in resource
	# NHS Number exists with correct system and valid NHS number (http://fhir.nhs.net/Id/nhs-number)

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
