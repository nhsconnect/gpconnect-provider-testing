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

		#PG 18/4/2019 #188 - Providers should now not reject JWT when creation time
		# is in the future. as such this test has been updated to expected a 200 response
Scenario: JWT - Creation Time - in the future
	Given I configure the default "MetadataRead" request
		And I set the JWT Creation Time to "200" seconds in the future
	When I make the "MetadataRead" request
	Then the response status code should be "200"

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

Scenario: JWT - Requested Scope - Metadata - incorrect
	Given I configure the default "MetadataRead" request
		And I set the JWT Requested Scope to be incorrect
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

Scenario: JWT - Requesting Organization - Backward Compatability for RC5
	Given I configure the default "MetadataRead" request
		And I set the JWT Requesting Organization Identifier system to match the rc5 specification
	When I make the "MetadataRead" request
	Then the response status code should indicate success

# git hub ref 188 
# PG 15/4/19
# #188 was updated so RMB tests updated /removed to meet new requirements
Scenario: JWT - Everything normal test
	Given I configure the default "MetadataRead" request
	And I set the JWT Creation Time to "0" seconds in the future
	And I set the JWT Expiry Time to "300" seconds after Creation Time
	When I make the "MetadataRead" request
	Then the response status code should be "200"

# PG 15/4/19 - #188 - Checking that Creation Time set in past is rejected.
Scenario: JWT - Consumer clock is slow 600s
	Given I configure the default "MetadataRead" request
		And I set the JWT Creation Time to "-600" seconds in the past
		And I set the JWT Expiry Time to "300" seconds after Creation Time
	When I make the "MetadataRead" request
	Then the response status code should be "400"