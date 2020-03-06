@fhir @1.3.2-Full-Pack
Feature: FHIR

@1.3.2-IncrementalAndRegression
Scenario: Fhir Get MetaData
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the Response Resource should be a CapabilityStatement
		And the CapabilityStatement version should match the GP Connect specification release

Scenario: CapabilityStatement profile indicates acceptance of xml and json format
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement Format should contain XML and JSON

Scenario: CapabilityStatement profile suppliers software versions present
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement Software should be valid

Scenario: CapabilityStatement profile supported fhir version
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement FHIR Version should be "3.0.1"

# github ref 132
# RMB 29/10/2018
Scenario: CapabilityStatement profile supports the RegisterPatient operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the CapabilityStatement REST Operations should contain "gpc.registerpatient"

#132 - RMB 29/10/2018
#292 - PG 30/8/2019 - added check for correct url on operation
Scenario: CapabilityStatement profile supports the GetStructuredRecord operation
	Given I configure the default "MetadataRead" request
	When I make the "MetaDataRead" request
	Then the response status code should indicate success
	And the CapabilityStatement REST Operations should contain "gpc.getstructuredrecord"
	And the CapabilityStatement Operation "gpc.getstructuredrecord" has url "https://fhir.nhs.uk/STU3/OperationDefinition/GPConnect-GetStructuredRecord-Operation-1/_history/1.15" 
	
Scenario: Fhir content type test where Accept header is JSON and request payload is XML
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+xml"
		And I set the Accept header to "application/fhir+json"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir content type test where Accept header is XML and request payload is JSON
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+json"
		And I set the Accept header to "application/fhir+xml"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: Fhir content type test where _format parameter is JSON and request payload is JSON
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+json"
		And I do not send header "Accept"
		And I add a Format parameter with the Value "application/fhir+json"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir content type test where _format parameter is JSON and request payload is XML
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+xml"
		And I do not send header "Accept"
		And I add a Format parameter with the Value "application/fhir+json"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir content type test where _format parameter is XML and request payload is XML
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+xml"
		And I do not send header "Accept"
		And I add a Format parameter with the Value "application/fhir+xml"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: Fhir content type test where _format parameter is XML and request payload is JSON
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+json"
		And I do not send header "Accept"
		And I add a Format parameter with the Value "application/fhir+xml"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: Fhir content type test where Accept header is XML and _format parameter is XML
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+xml"
		And I set the Accept header to "application/fhir+xml"
		And I add a Format parameter with the Value "application/fhir+xml"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: Fhir content type test where Accept header is XML and _format parameter is JSON
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+xml"
		And I set the Accept header to "application/fhir+xml"
		And I add a Format parameter with the Value "application/fhir+json"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir content type test where Accept header is JSON and _format parameter is JSON
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+json"
		And I set the Accept header to "application/fhir+json"
		And I add a Format parameter with the Value "application/fhir+json"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: Fhir content type test where Accept header is JSON and _format parameter is XML
	Given I configure the default "MetadataRead" request
		And I set the request content type to "application/fhir+json"
		And I set the Accept header to "application/fhir+json"
		And I add a Format parameter with the Value "application/fhir+xml"
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario: endpoint should support gzip compression for metadata endpoint and contain the correct payload
	Given I configure the default "MetadataRead" request
		And I set the Accept-Encoding header to gzip
	When I make the "MetadataRead" request
	Then the response status code should indicate success
		And the response should be gzip encoded
		And the response body should be FHIR JSON
		And the Response Resource should be a CapabilityStatement
                  
Scenario Outline: CapabilityStatement GetMetaDataRead operation returns correct profile versions
Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
	And the CapabilityStatement REST Operations should contain "gpc.getstructuredrecord"
    And the CapabilityStatement Profile should contain the correct reference and version history "<urlToCheck>" 
Examples: 
| urlToCheck                                                                         |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Patient-1             |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Organization-1        |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Practitioner-1        |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-PractitionerRole-1    |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Location-1            |
| https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-OperationOutcome-1          |
| https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-Appointment-1               |
| https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-Schedule-1                  |
| https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-Slot-1                      |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-AllergyIntolerance-1  |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Medication-1          |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-MedicationStatement-1 |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-MedicationRequest-1   |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-List-1                |
| https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-StructuredRecord-Bundle-1   |
          
Scenario Outline: CapabilityStatement GetStructuredMetaDataRead operation returns correct profile versions
Given I configure the default "StructuredMetaDataRead" request
	When I make the "StructuredMetaDataRead" request
	Then the response status code should indicate success
	And the CapabilityStatement REST Operations should contain "gpc.getstructuredrecord"
    And the CapabilityStatement Profile should contain the correct reference and version history "<urlToCheck>" 
Examples: 
| urlToCheck                                                                                      |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Patient-1/_history/1.8             |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Organization-1/_history/1.4        |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Practitioner-1/_history/1.2        |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-PractitionerRole-1/_history/1.2    |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-AllergyIntolerance-1/_history/1.6  |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-Medication-1/_history/1.2          |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-MedicationStatement-1/_history/1.6 |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-MedicationRequest-1/_history/1.5   |
| https://fhir.nhs.uk/STU3/StructureDefinition/CareConnect-GPC-List-1/_history/1.1                |
| https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-StructuredRecord-Bundle-1/_history/1.3   |
| https://fhir.nhs.uk/STU3/StructureDefinition/GPConnect-OperationOutcome-1/_history/1.2          |
          
Scenario: CapabilityStatement profile supports the GetStructuredMetaDataRead operation
Given I configure the default "StructuredMetaDataRead" request
	When I make the "StructuredMetaDataRead" request
	Then the response status code should indicate success
	And the CapabilityStatement REST Operations should contain "gpc.getstructuredrecord"
	And the CapabilityStatement Operation "gpc.getstructuredrecord" has url "https://fhir.nhs.uk/STU3/OperationDefinition/GPConnect-GetStructuredRecord-Operation-1/_history/1.15" 
	