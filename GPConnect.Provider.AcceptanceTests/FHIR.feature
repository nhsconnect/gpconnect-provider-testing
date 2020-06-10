@fhir @1.5.0-Full-Pack
Feature: FHIR

##########################################
#Foundations and Appointments Tests
##########################################

@1.3.2-IncrementalAndRegression
Scenario: Fhir Get Metadata and Check Version of Foundations And Appointments CapabilityStatement
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the Response Resource should be a CapabilityStatement
		And the FoundationsAndAppmts CapabilityStatement version should match the GP Connect specification release

Scenario: Foundations CapabilityStatement profile indicates acceptance of xml and json format
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement Format should contain XML and JSON

Scenario: Foundations CapabilityStatement profile suppliers software versions present
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement Software should be valid

Scenario: Foundations CapabilityStatement profile supported fhir version
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement FHIR Version should be "3.0.1"

Scenario: Foundations CapabilityStatement profile supports the RegisterPatient operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement REST Operations should contain "gpc.registerpatient"

Scenario: Foundations CapabilityStatement profile supports the GetStructuredRecord operation
	Given I configure the default "MetadataRead" request
	When I make the "MetaDataRead" request
	Then the response status code should indicate success
	And the CapabilityStatement REST Operations should contain "gpc.getstructuredrecord"
	And the CapabilityStatement Operation "gpc.getstructuredrecord" has url "https://fhir.nhs.uk/STU3/OperationDefinition/GPConnect-GetStructuredRecord-Operation-1/_history/1.15" 
	
Scenario: Foundations Fhir content type test where Accept header is JSON and request payload is XML
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+xml"
		And I set the Accept header to "application/fhir+json"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Foundations Fhir content type test where Accept header is XML and request payload is JSON
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+json"
		And I set the Accept header to "application/fhir+xml"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: Foundations Fhir content type test where _format parameter is JSON and request payload is JSON
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+json"
		And I do not send header "Accept"
		And I add a Format parameter with the Value "application/fhir+json"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Foundations Fhir content type test where _format parameter is JSON and request payload is XML
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+xml"
		And I do not send header "Accept"
		And I add a Format parameter with the Value "application/fhir+json"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Foundations Fhir content type test where _format parameter is XML and request payload is XML
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+xml"
		And I do not send header "Accept"
		And I add a Format parameter with the Value "application/fhir+xml"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: Foundations Fhir content type test where _format parameter is XML and request payload is JSON
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+json"
		And I do not send header "Accept"
		And I add a Format parameter with the Value "application/fhir+xml"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: Foundations Fhir content type test where Accept header is XML and _format parameter is XML
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+xml"
		And I set the Accept header to "application/fhir+xml"
		And I add a Format parameter with the Value "application/fhir+xml"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: Foundations Fhir content type test where Accept header is XML and _format parameter is JSON
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+xml"
		And I set the Accept header to "application/fhir+xml"
		And I add a Format parameter with the Value "application/fhir+json"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Foundations Fhir content type test where Accept header is JSON and _format parameter is JSON
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+json"
		And I set the Accept header to "application/fhir+json"
		And I add a Format parameter with the Value "application/fhir+json"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Foundations Fhir content type test where Accept header is JSON and _format parameter is XML
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+json"
		And I set the Accept header to "application/fhir+json"
		And I add a Format parameter with the Value "application/fhir+xml"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: Foundations endpoint should support gzip compression for metadata endpoint and contain the correct payload
	Given I configure the default "MetadataRead" request
		And I set the Accept-Encoding header to gzip
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response should be gzip encoded
		And the response body should be FHIR JSON
		And the Response Resource should be a CapabilityStatement
                  
Scenario Outline: Foundations CapabilityStatement returns correct profile versions
Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
	And the CapabilityStatement REST Operations should contain "gpc.getstructuredrecord"
    And the CapabilityStatement Profile should contain the correct reference and version history "<urlToCheck>" 
Examples: 
| urlToCheck                                                                              |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Patient-1                  |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Organization-1             |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Practitioner-1             |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-PractitionerRole-1         |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Location-1                 |
| https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-OperationOutcome-1               |
| https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-Appointment-1                    |
| https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-Schedule-1                       |
| https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-Slot-1                           |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-AllergyIntolerance-1       |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Medication-1               |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-MedicationStatement-1      |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-MedicationRequest-1        |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-List-1                     |
| https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-StructuredRecord-Bundle-1        |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Immunization-1             |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Condition-ProblemHeader-1  |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Encounter-1                |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Observation-1              |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-DiagnosticReport-1         |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Specimen-1			      |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-ProcedureRequest-1         |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-ReferralRequest-1          |

##########################################
#Structured Tests
##########################################

@Structured
Scenario: Fhir Get Metadata and Check Version of Structured CapabilityStatement
	Given I configure the default "StructuredMetaDataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the Response Resource should be a CapabilityStatement
		And the Structured CapabilityStatement version should match the GP Connect specification release

@Structured
Scenario: Structured CapabilityStatement profile indicates acceptance of xml and json format
	Given I configure the default "StructuredMetaDataRead" request
	When I make the "StructuredMetaDataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement Format should contain XML and JSON

@Structured
Scenario: Structured CapabilityStatement profile suppliers software versions present
	Given I configure the default "StructuredMetaDataRead" request
	When I make the "StructuredMetaDataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement Software should be valid

@Structured
Scenario: Structured CapabilityStatement profile supported fhir version
	Given I configure the default "StructuredMetaDataRead" request
	When I make the "StructuredMetaDataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement FHIR Version should be "3.0.1"

@Structured
Scenario: Structured CapabilityStatement profile supports the GetStructuredRecord operation
	Given I configure the default "StructuredMetaDataRead" request
	When I make the "StructuredMetaDataRead" request
	Then the response status code should indicate success
	And the CapabilityStatement REST Operations should contain "gpc.getstructuredrecord"
	And the CapabilityStatement Operation "gpc.getstructuredrecord" has url "https://fhir.nhs.uk/STU3/OperationDefinition/GPConnect-GetStructuredRecord-Operation-1/_history/1.15" 

@Structured	
Scenario: Structured Fhir content type test where Accept header is JSON and request payload is XML
	Given I configure the default "StructuredMetaDataRead" request
		And I set the request content type to "application/fhir+xml"
		And I set the Accept header to "application/fhir+json"
	When I make the "StructuredMetaDataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

@Structured
Scenario: Structured Fhir content type test where Accept header is XML and request payload is JSON
	Given I configure the default "StructuredMetaDataRead" request
		And I set the request content type to "application/fhir+json"
		And I set the Accept header to "application/fhir+xml"
	When I make the "StructuredMetaDataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

@Structured
Scenario: Structured Fhir content type test where _format parameter is JSON and request payload is JSON
	Given I configure the default "StructuredMetaDataRead" request
		And I set the request content type to "application/fhir+json"
		And I do not send header "Accept"
		And I add a Format parameter with the Value "application/fhir+json"
	When I make the "StructuredMetaDataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

@Structured
Scenario: Structured Fhir content type test where _format parameter is JSON and request payload is XML
	Given I configure the default "StructuredMetaDataRead" request
		And I set the request content type to "application/fhir+xml"
		And I do not send header "Accept"
		And I add a Format parameter with the Value "application/fhir+json"
	When I make the "StructuredMetaDataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

@Structured
Scenario: Structured Fhir content type test where _format parameter is XML and request payload is XML
	Given I configure the default "StructuredMetaDataRead" request
		And I set the request content type to "application/fhir+xml"
		And I do not send header "Accept"
		And I add a Format parameter with the Value "application/fhir+xml"
	When I make the "StructuredMetaDataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

@Structured
Scenario: Structured Fhir content type test where _format parameter is XML and request payload is JSON
	Given I configure the default "StructuredMetaDataRead" request
		And I set the request content type to "application/fhir+json"
		And I do not send header "Accept"
		And I add a Format parameter with the Value "application/fhir+xml"
	When I make the "StructuredMetaDataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

@Structured
Scenario: Structured Fhir content type test where Accept header is XML and _format parameter is XML
	Given I configure the default "StructuredMetaDataRead" request
		And I set the request content type to "application/fhir+xml"
		And I set the Accept header to "application/fhir+xml"
		And I add a Format parameter with the Value "application/fhir+xml"
	When I make the "StructuredMetaDataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

@Structured
Scenario: Structured Fhir content type test where Accept header is XML and _format parameter is JSON
	Given I configure the default "StructuredMetaDataRead" request
		And I set the request content type to "application/fhir+xml"
		And I set the Accept header to "application/fhir+xml"
		And I add a Format parameter with the Value "application/fhir+json"
	When I make the "StructuredMetaDataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

@Structured
Scenario: Structured Fhir content type test where Accept header is JSON and _format parameter is JSON
	Given I configure the default "StructuredMetaDataRead" request
		And I set the request content type to "application/fhir+json"
		And I set the Accept header to "application/fhir+json"
		And I add a Format parameter with the Value "application/fhir+json"
	When I make the "StructuredMetaDataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

@Structured
Scenario: Structured Fhir content type test where Accept header is JSON and _format parameter is XML
	Given I configure the default "StructuredMetaDataRead" request
		And I set the request content type to "application/fhir+json"
		And I set the Accept header to "application/fhir+json"
		And I add a Format parameter with the Value "application/fhir+xml"
	When I make the "StructuredMetaDataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

@Structured
Scenario Outline: Structured CapabilityStatement returns correct profile versions
Given I configure the default "StructuredMetaDataRead" request
	When I make the "StructuredMetaDataRead" request
	Then the response status code should indicate success
	And the CapabilityStatement REST Operations should contain "gpc.getstructuredrecord"
    And the CapabilityStatement Profile should contain the correct reference and version history "<urlToCheck>" 
Examples: 
| urlToCheck                                                                                          |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Patient-1/_history/1.8                 |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Organization-1/_history/1.4            |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Practitioner-1/_history/1.2            |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-PractitionerRole-1/_history/1.2        |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-AllergyIntolerance-1/_history/1.7      |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Medication-1/_history/1.2              |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-MedicationStatement-1/_history/1.6     |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-MedicationRequest-1/_history/1.7       |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-List-1/_history/1.2                    |
| https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-StructuredRecord-Bundle-1/_history/1.3       |
| https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-OperationOutcome-1/_history/1.2              |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Immunization-1/_history/1.5            |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Condition-ProblemHeader-1/_history/1.4 |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Encounter-1/_history/1.4               |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Observation-1/_history/1.4             |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-DiagnosticReport-1/_history/1.3        |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Specimen-1/_history/1.3                |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-ProcedureRequest-1/_history/1.3        |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-ReferralRequest-1/_history/1.2         |


##########################################
#Document Tests
##########################################

@1.5.0-IncrementalAndRegression @StructuredDocuments @Structured
Scenario: Fhir Get Metadata and Check Version of Documents CapabilityStatement
	Given I configure the default "DocumentsMetaDataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the Response Resource should be a CapabilityStatement
		And the Documents CapabilityStatement version should match the GP Connect specification release

@1.5.0-IncrementalAndRegression @StructuredDocuments @Structured
Scenario: Documents CapabilityStatement profile indicates acceptance of xml and json format
	Given I configure the default "DocumentsMetaDataRead" request
	When I make the "DocumentsMetaDataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement Format should contain XML and JSON

@1.5.0-IncrementalAndRegression @StructuredDocuments @Structured
Scenario: Documents CapabilityStatement profile suppliers software versions present
	Given I configure the default "DocumentsMetaDataRead" request
	When I make the "DocumentsMetaDataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement Software should be valid

@1.5.0-IncrementalAndRegression @StructuredDocuments @Structured
Scenario: Documents CapabilityStatement profile supported fhir version
	Given I configure the default "DocumentsMetaDataRead" request
	When I make the "DocumentsMetaDataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement FHIR Version should be "3.0.1"

@1.5.0-IncrementalAndRegression @StructuredDocuments @Structured
Scenario: Documents Fhir content type test where Accept header is JSON and request payload is XML
	Given I configure the default "DocumentsMetaDataRead" request
		And I set the request content type to "application/fhir+xml"
		And I set the Accept header to "application/fhir+json"
	When I make the "DocumentsMetaDataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

@1.5.0-IncrementalAndRegression @StructuredDocuments @Structured
Scenario: Documents Fhir content type test where Accept header is XML and request payload is JSON
	Given I configure the default "DocumentsMetaDataRead" request
		And I set the request content type to "application/fhir+json"
		And I set the Accept header to "application/fhir+xml"
	When I make the "DocumentsMetaDataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

@1.5.0-IncrementalAndRegression @StructuredDocuments @Structured
Scenario: Documents Fhir content type test where _format parameter is JSON and request payload is JSON
	Given I configure the default "DocumentsMetaDataRead" request
		And I set the request content type to "application/fhir+json"
		And I do not send header "Accept"
		And I add a Format parameter with the Value "application/fhir+json"
	When I make the "DocumentsMetaDataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

@1.5.0-IncrementalAndRegression @StructuredDocuments @Structured
Scenario: Documents Fhir content type test where _format parameter is JSON and request payload is XML
	Given I configure the default "DocumentsMetaDataRead" request
		And I set the request content type to "application/fhir+xml"
		And I do not send header "Accept"
		And I add a Format parameter with the Value "application/fhir+json"
	When I make the "DocumentsMetaDataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

@1.5.0-IncrementalAndRegression @StructuredDocuments @Structured
Scenario: Documents Fhir content type test where _format parameter is XML and request payload is XML
	Given I configure the default "DocumentsMetaDataRead" request
		And I set the request content type to "application/fhir+xml"
		And I do not send header "Accept"
		And I add a Format parameter with the Value "application/fhir+xml"
	When I make the "DocumentsMetaDataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

@1.5.0-IncrementalAndRegression @StructuredDocuments @Structured
Scenario: Documents Fhir content type test where _format parameter is XML and request payload is JSON
	Given I configure the default "DocumentsMetaDataRead" request
		And I set the request content type to "application/fhir+json"
		And I do not send header "Accept"
		And I add a Format parameter with the Value "application/fhir+xml"
	When I make the "DocumentsMetaDataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

@1.5.0-IncrementalAndRegression @StructuredDocuments @Structured
Scenario: Documents Fhir content type test where Accept header is XML and _format parameter is XML
	Given I configure the default "DocumentsMetaDataRead" request
		And I set the request content type to "application/fhir+xml"
		And I set the Accept header to "application/fhir+xml"
		And I add a Format parameter with the Value "application/fhir+xml"
	When I make the "DocumentsMetaDataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

@1.5.0-IncrementalAndRegression @StructuredDocuments @Structured
Scenario: Documents Fhir content type test where Accept header is XML and _format parameter is JSON
	Given I configure the default "DocumentsMetaDataRead" request
		And I set the request content type to "application/fhir+xml"
		And I set the Accept header to "application/fhir+xml"
		And I add a Format parameter with the Value "application/fhir+json"
	When I make the "DocumentsMetaDataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

@1.5.0-IncrementalAndRegression @StructuredDocuments @Structured
Scenario: Documents Fhir content type test where Accept header is JSON and _format parameter is JSON
	Given I configure the default "DocumentsMetaDataRead" request
		And I set the request content type to "application/fhir+json"
		And I set the Accept header to "application/fhir+json"
		And I add a Format parameter with the Value "application/fhir+json"
	When I make the "DocumentsMetaDataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

@1.5.0-IncrementalAndRegression @StructuredDocuments @Structured
Scenario: Documents Fhir content type test where Accept header is JSON and _format parameter is XML
	Given I configure the default "DocumentsMetaDataRead" request
		And I set the request content type to "application/fhir+json"
		And I set the Accept header to "application/fhir+json"
		And I add a Format parameter with the Value "application/fhir+xml"
	When I make the "DocumentsMetaDataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

@1.5.0-IncrementalAndRegression @StructuredDocuments @Structured
Scenario Outline: Documents CapabilityStatement returns correct profile versions
Given I configure the default "DocumentsMetaDataRead" request
	When I make the "DocumentsMetaDataRead" request
	Then the response status code should indicate success
    And the CapabilityStatement Profile should contain the correct reference and version history "<urlToCheck>" 
Examples: 
| urlToCheck                                                                                    |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Patient-1/_history/1.8           |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Organization-1/_history/1.4      |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Practitioner-1/_history/1.2      |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-PractitionerRole-1/_history/1.2  |
| https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-OperationOutcome-1/_history/1.2        |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-DocumentReference-1/_history/1.2 |
| https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-Searchset-Bundle-1/_history/1.3        |

@1.5.0-IncrementalAndRegression @StructuredDocuments @Structured
Scenario: Documents CapabilityStatement profile supports the Patient read operation
	Given I configure the default "DocumentsMetaDataRead" request
	When I make the "DocumentsMetaDataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement REST Resources should contain the "Patient" Resource with the "Read" Interaction

@1.5.0-IncrementalAndRegression @StructuredDocuments @Structured
Scenario: Documents CapabilityStatement profile supports the Patient search operation
	Given I configure the default "DocumentsMetaDataRead" request
	When I make the "DocumentsMetaDataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement REST Resources should contain the "Patient" Resource with the "SearchType" Interaction

@1.5.0-IncrementalAndRegression @StructuredDocuments @Structured
Scenario: Documents CapabilityStatement profile supports the Binary Read operation
	Given I configure the default "DocumentsMetaDataRead" request
	When I make the "DocumentsMetaDataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement REST Resources should contain the "Binary" Resource with the "Read" Interaction		

@1.5.0-IncrementalAndRegression @StructuredDocuments @Structured
Scenario: Documents CapabilityStatement profile supports the DocumentReference search operation
	Given I configure the default "DocumentsMetaDataRead" request
	When I make the "DocumentsMetaDataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement REST Resources should contain the "DocumentReference" Resource with the "SearchType" Interaction

@1.5.0-IncrementalAndRegression @StructuredDocuments @Structured
Scenario Outline: Check Documents CapabilityStatement includes specific searchInclude
	Given I configure the default "DocumentsMetaDataRead" request
	When I make the "DocumentsMetaDataRead" request
	Then the response status code should indicate success	
	And the Documents CapabilityStatement has a searchInclude called "<searchInclude>"
	Examples: 
	| searchInclude                            |
	| DocumentReference:subject:Patient        |
	| DocumentReference:custodian:Organization |
	| DocumentReference:author:Organization    |
	| DocumentReference:author:Practitioner    |

@1.5.0-IncrementalAndRegression @StructuredDocuments @Structured
Scenario Outline: Check Documents CapabilityStatement includes specific searchParams
	Given I configure the default "DocumentsMetaDataRead" request
	When I make the "DocumentsMetaDataRead" request
	Then the response status code should indicate success	
	And the Documents CapabilityStatement has a searchParam called "<searchParam>" of type "<searchParamType>"
	Examples: 
	| searchParam | searchParamType |
	| created     | date            |
	| facility    | token           |
	| author      | token           |
	| type        | token           |
	| custodian   | token           |
	| description | string          |
	
