@jwt
Feature: JWT

Scenario: JWT - Bearer Token - not base 64 encoded
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request with an unencoded JWT Bearer Token
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requesting System URL - missing
	Given I configure the default "MetadataRead" request
		And I set the JWT with missing Requesting System URL
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Expiry Time - missing
	Given I configure the default "MetadataRead" request
		And I set the JWT with missing Expiry Time
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Expiry Time - greater than 300 seconds
	Given I configure the default "MetadataRead" request
		And I set the JWT Expiry Time to "301" seconds after Creation Time
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Expiry Time - less than 300 seconds
	Given I configure the default "MetadataRead" request
		And I set the JWT Expiry Time to "299" seconds after Creation Time
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Expiry Time - before Creation Time
	Given I configure the default "MetadataRead" request
		And I set the JWT Expiry Time to "-1" seconds after Creation Time
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Creation Time - missing
	Given I configure the default "MetadataRead" request
		And I set the JWT with missing Creation Time
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Creation Time - in the future
	Given I configure the default "MetadataRead" request
		And I set the JWT Creation Time to "200" seconds in the future
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Reason For Request - missing
	Given I configure the default "MetadataRead" request
		And I set the JWT with missing Reason For Request
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Reason For Request - incorrect
	Given I configure the default "MetadataRead" request
		And I set the JWT Reason For Request to "notdirectcare"
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Authorization Server Token URL - missing
	Given I configure the default "MetadataRead" request
		And I set the JWT with missing Authorization Server Token URL
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Authorization Server Token URL - incorrect
	Given I configure the default "MetadataRead" request
		And I set the JWT Authorization Server Token URL to "https://notValid.fhir.nhs.net/tokenEndpoint"
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requesting Device - missing
	Given I configure the default "MetadataRead" request
		And I set the JWT with missing Requesting Device
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requesting Device - invalid
	Given I configure the default "MetadataRead" request
		And I set the JWT Requesting Device as an invalid Device
	When I make the "MetadataRead" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requesting Device - Resource Type - invalid
	Given I configure the default "MetadataRead" request
		And I set the JWT Requesting Device Resource Type as an invalid Resource Type
	When I make the "MetadataRead" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requesting Organization - missing
	Given I configure the default "MetadataRead" request
		And I set the JWT with missing Requesting Organization
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requesting Organization - invalid
	Given I configure the default "MetadataRead" request
		And I set the JWT Requesting Organization as an invalid Organization
	When I make the "MetadataRead" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requesting Organization - Resource Type - invalid
	Given I configure the default "MetadataRead" request
		And I set the JWT Requesting Organization Resource Type as an invalid Resource Type
	When I make the "MetadataRead" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requesting Organization - Identifier - ODS Code - missing
	Given I configure the default "MetadataRead" request
		And I set the JWT Requesting Organization Identifier with missing ODS Code
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requesting Organization - Identifier - missing
	Given I configure the default "MetadataRead" request
		And I set the JWT Requesting Organization with missing Identifier
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requesting Practitioner - missing
	Given I configure the default "MetadataRead" request
		And I set the JWT with missing Requesting Practitioner
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requesting Practitioner - invalid
	Given I configure the default "MetadataRead" request
		And I set the JWT Requesting Practitioner as an invalid Practitioner
	When I make the "MetadataRead" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requesting Practitioner - Resource Type - invalid
	Given I configure the default "MetadataRead" request
		And I set the JWT Requesting Practitioner Resource Type as an invalid Resource Type
	When I make the "MetadataRead" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requesting Practitioner - Identifier - SDS Id - missing
	Given I configure the default "MetadataRead" request
		And I set the JWT Requesting Practitioner with missing SDS Id
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requesting Practitioner - Identifier - missing
	Given I configure the default "MetadataRead" request
		And I set the JWT Requesting Practitioner with missing Identifier
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requesting Practitioner - User Id - not matching
	Given I configure the default "MetadataRead" request
		And I set the JWT Requesting Practitioner with User Id not matching
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requesting Practitioner - Name - missing
	Given I configure the default "MetadataRead" request
		And I set the JWT Requesting Practitioner with missing Name
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requesting Practitioner - Practitioner Role - missing
	Given I configure the default "MetadataRead" request
		And I set the JWT Requesting Practitioner with missing Practitioner Role
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requesting Practitioner - Practitioner Role - SDS Job Role - missing
	Given I configure the default "MetadataRead" request
		And I set the JWT Requesting Practitioner Pratitioner Role with missing SDS Job Role
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requesting Practitioner Id - missing
	Given I configure the default "MetadataRead" request
		And I set the JWT with missing Requesting Practitioner Id
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: JWT - Requested Record - missing
	Given I configure the default "MetadataRead" request
		And I set the JWT with missing Requested Record
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requested Record - GetCareRecord Patient - not matching
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
		And I set the JWT Requested Record to the NHS Number for "patient15"
	When I make the "GpcGetCareRecord" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requested Scope - missing
	Given I configure the default "MetadataRead" request
		And I set the JWT with missing Requested Scope
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requested Scope - GetCareRecord - incorrect
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
		And I set the JWT Requested Scope to Organization Read
	When I make the "GpcGetCareRecord" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requested Scope - Metadata - incorrect
	Given I configure the default "MetadataRead" request
		And I set the JWT Requested Scope to Patient Read
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource
