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
# RMB 28/2/19
# test 1
Scenario: JWT - Everything normal test
	Given I configure the default "MetadataRead" request
		And I set the JWT Creation Time to "0" seconds in the future
	When I make the "MetadataRead" request
	Then the response status code should be "200"

# test 2
Scenario: JWT - Ensure token expiration check
	Given I configure the default "MetadataRead" request
		And I set the JWT Creation Time to "-600" seconds in the past
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

# test 3
Scenario: JWT - Ensure token future date check
	Given I configure the default "MetadataRead" request
		And I set the JWT Creation Time to "600" seconds in the future
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

# clock skew allowance
# test 4
Scenario: JWT - Consumer clock is fast 90s
	Given I configure the default "MetadataRead" request
		And I set the JWT Creation Time to "90" seconds in the future
	When I make the "MetadataRead" request
	Then the response status code should be "200"
	
# clock skew allowance
# test 5
Scenario: JWT - Consumer clock is fast 180s
	Given I configure the default "MetadataRead" request
		And I set the JWT Creation Time to "180" seconds in the future
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource

# clock skew allowance
# test 6
Scenario: JWT - Consumer clock is slow 90s
	Given I configure the default "MetadataRead" request
		And I set the JWT Creation Time to "-90" seconds in the past
	When I make the "MetadataRead" request
	Then the response status code should be "200"
		
# clock skew allowance
# test 7
Scenario: JWT - Consumer clock is slow 390s 
	Given I configure the default "MetadataRead" request
		And I set the JWT Creation Time to "-390" seconds in the past
	When I make the "MetadataRead" request
	Then the response status code should be "200"

# clock skew allowance
# test 8
Scenario: JWT - Consumer clock is slow 480s 
	Given I configure the default "MetadataRead" request
		And I set the JWT Creation Time to "-480" seconds in the past
	When I make the "MetadataRead" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource