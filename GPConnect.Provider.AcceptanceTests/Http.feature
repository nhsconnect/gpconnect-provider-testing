@http @1.5.0-Full-Pack
Feature: HTTP

Scenario: Http GET from invalid endpoint
	Given I configure the default "MetadataRead" request
		And I set the request URL to "metadatas"
	When I make the "MetadataRead" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Http POST to invalid endpoint
	Given I configure the default "MetadataRead" request
		And I set the request URL to "metadatas"
		And I set the request Http Method to "POST"
	When I make the "MetadataRead" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Http PUT to invalid endpoint
	Given I configure the default "MetadataRead" request
		And I set the request URL to "metadatas"
		And I set the request Http Method to "PUT"
	When I make the "MetadataRead" request
	Then the response status code should indicate failure
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource

Scenario: Http PATCH to valid endpoint
	Given I configure the default "MetadataRead" request
		And I set the request URL to "metadata"
		And I set the request Http Method to "PATCH"
	When I make the "MetadataRead" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Http DELETE to valid endpoint
	Given I configure the default "MetadataRead" request
		And I set the request URL to "metadata"
		And I set the request Http Method to "DELETE"
	When I make the "MetadataRead" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Http OPTIONS to valid endpoint
	Given I configure the default "MetadataRead" request
		And I set the request URL to "metadata"
		And I set the request Http Method to "OPTIONS"
	When I make the "MetadataRead" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Http incorrect case on url fhir resource
	Given I configure the default "MetadataRead" request
		And I set the request URL to "Metadata"
	When I make the "MetadataRead" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource
