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

Scenario: JWT - Authorization Server Token URL - missing
	Given I configure the default "MetadataRead" request
		And I set the JWT with missing Authorization Server Token URL
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

Scenario: JWT - Requesting Identity - missing
	Given I configure the default "MetadataRead" request
		And I set the JWT with missing Requesting Identity
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requesting Identity - invalid
	Given I configure the default "MetadataRead" request
		And I set the JWT Requesting Identity as an invalid Identity
	When I make the "MetadataRead" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requesting Identity - Resource Type - invalid
	Given I configure the default "MetadataRead" request
		And I set the JWT Requesting Identity Resource Type as an invalid Resource Type
	When I make the "MetadataRead" request
	Then the response status code should be "422"
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
