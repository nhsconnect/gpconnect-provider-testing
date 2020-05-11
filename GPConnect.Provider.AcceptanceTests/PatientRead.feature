@patient @1.2.7-Full-Pack
Feature: PatientRead

Scenario Outline: Read patient 404 if patient not found
	Given I configure the default "PatientRead" request
		And I set the Read Operation logical identifier used in the request to "<id>"
	When I make the "PatientRead" request
	Then the response status code should be "404"
		And the response should be a OperationOutcome resource with error code "PATIENT_NOT_FOUND"
	Examples:
		| id                                                         |
		| SomthingIncorrectWhichIsNotTheOnProviderSystem             |
		| 4543567638475665845564986758479086840564796854665748763454 |

Scenario Outline: Patient Read with valid identifier which does not exist on providers system
	Given I configure the default "PatientRead" request
		And I set the Read Operation logical identifier used in the request to "<LogicalId>"
	When I make the "PatientRead" request
	Then the response status code should be "404"
	Examples:
		| LogicalId      |
		| aaBA           |
		| 1ZEc2          |
		| z.as.dd        |
		| 1.1.22         |
		| 40-9           |
		| nd-skdm.mks--s |

Scenario: Read patient 400 or 404 if patient id not sent
	Given I configure the default "PatientRead" request
	When I make the "PatientRead" request
	Then the Response Status Code should be one of "400, 404"
		And the response should be a OperationOutcome resource

Scenario Outline: Read patient using the Accept header to request response format
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I configure the default "PatientRead" request
		And I set the Accept header to "<Header>"
	When I make the "PatientRead" request
	Then the response status code should indicate success
		And the response body should be FHIR <ResponseFormat>
		And the Response Resource should be a Patient
		And the Patient Id should equal the Request Id
	Examples:
		| Header                | ResponseFormat |
		| application/fhir+json | JSON           |
		| application/fhir+xml  | XML            |

Scenario Outline: Read patient using the _format parameter to request response format
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I configure the default "PatientRead" request
		And I add a Format parameter with the Value "<Format>"
	When I make the "PatientRead" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be a Patient
		And the Patient Id should equal the Request Id
		And the Patient Identifiers should be valid for Patient "patient1"
	Examples:
		| Format                | BodyFormat |
		| application/fhir+json | JSON       |
		| application/fhir+xml  | XML        |

Scenario Outline: Read patient sending the Accept header and _format parameter to request response format
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I configure the default "PatientRead" request
		And I set the Accept header to "<Header>"
		And I add a Format parameter with the Value "<Format>"
	When I make the "PatientRead" request
	Then the response status code should indicate success
		And the response body should be FHIR <BodyFormat>
		And the Response Resource should be a Patient
		And the Patient Id should equal the Request Id
		And the Patient Identifiers should be valid for Patient "patient1"
	Examples:
		| Header                | Format                | BodyFormat |
		| application/fhir+json | application/fhir+json | JSON       |
		| application/fhir+json | application/fhir+xml  | XML        |
		| application/fhir+xml  | application/fhir+json | JSON       |
		| application/fhir+xml  | application/fhir+xml  | XML        |

Scenario: Read patient should contain correct logical identifier
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I configure the default "PatientRead" request
	When I make the "PatientRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the Response Resource should be a Patient
		And the Patient Id should equal the Request Id

Scenario: Read patient response should contain an ETag header
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I configure the default "PatientRead" request
	When I make the "PatientRead" request
	Then the response status code should indicate success
		And the Response Resource should be a Patient
		And the Response should contain the ETag header matching the Resource Version Id
		And the Patient Identifiers should be valid for Patient "patient1"

@1.2.3
Scenario: Read patient returned should conform to the GPconnect specification
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I configure the default "PatientRead" request
	When I make the "PatientRead" request
	Then the response status code should indicate success
		And the Response Resource should be a Patient
		And the Patient Id should be valid
		And the Patient Metadata should be valid
		And the Patient Identifiers should be valid for Patient "patient1"
		And the Patient GeneralPractitioner Practitioner should be valid and resolvable
		And the Patient ManagingOrganization Organization should be valid and resolvable
		And the Patient Deceased should be valid
		And the Patient MultipleBirth should be valid
		And the Patient Telecom should be valid
		And the Patient Contact Relationship should be valid
		And the Patient Communication should be valid
		And the Patient Name should be valid
		And the Patient Contact Name should be valid
		And the Patient should exclude disallowed fields
		And the Patient Use should be valid
		And the Patient Gender should be valid
		And the Patient MaritalStatus should be valid
		And the Patient Link should be valid and resolvable
		And the Patient Registration Details should be valid
		# git hub ref 120
		# RMB 25/10/2018
		And the Patient Not In Use should be valid

Scenario: CapabilityStatement profile supports the Patient read operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement REST Resources should contain the "Patient" Resource with the "Read" Interaction

Scenario: Patient read valid response check caching headers exist
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I configure the default "PatientRead" request
	When I make the "PatientRead" request
	Then the response status code should indicate success
		And the Response Resource should be a Patient
		And the required cacheing headers should be present in the response
	
Scenario: Patient read invalid response check caching headers exist
	Given I configure the default "PatientRead" request
		And I set the Read Operation logical identifier used in the request to "AABa"
	When I make the "PatientRead" request
	Then the response status code should be "404"
		And the required cacheing headers should be present in the response

Scenario: Patient read valid response check preferred branch
	Given I get the Patient for Patient Value "patient1"
		And I store the Patient
	Given I configure the default "PatientRead" request
	When I make the "PatientRead" request
	Then the response status code should indicate success
		And the Response Resource should be a Patient
		And the Patient RegistrationDetails should include preferredBranchSurgery

#Scenario: When a patient on the provider system is deceased
# github ref 108
# RMB 22/10/2018
# removed as can't be done - needs a manual test
#	Given I configure the default "PatientRead" request
#		And I set the Read Operation logical identifier used in the request to "18"
#	Given I configure the default "PatientRead" request
#	When I make the "PatientRead" request
#	Then the response status code should be "400"
#		And the response should be a OperationOutcome resource with error code "INVALID_PATIENT_DEMOGRAAPHICS"

Scenario: Check read patient on a patient with no Consent returns a valid response
	Given I get the Patient for Patient Value "patient15"
		And I store the Patient
	Given I configure the default "PatientRead" request
	When I make the "PatientRead" request
	Then the response status code should indicate success
		And the Response Resource should be a Patient
		And the Patient Id should be valid
		And the Patient Metadata should be valid
		And the Patient Identifiers should be valid for Patient "patient15"
		And the Patient GeneralPractitioner Practitioner should be valid and resolvable
		And the Patient ManagingOrganization Organization should be valid and resolvable
		And the Patient Deceased should be valid
		And the Patient MultipleBirth should be valid
		And the Patient Telecom should be valid
		And the Patient Contact Relationship should be valid
		And the Patient Communication should be valid
		And the Patient Name should be valid
		And the Patient Contact Name should be valid
		And the Patient should exclude disallowed fields
		And the Patient Use should be valid
		And the Patient Gender should be valid
		And the Patient MaritalStatus should be valid
		And the Patient Link should be valid and resolvable
		And the Patient Registration Details should be valid
		# git hub ref 120
		# RMB 25/10/2018
		And the Patient Not In Use should be valid