@http
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

Scenario: Http operation incorrect case
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I set the request URL to "Patient/$gpc.GETcarerecord"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource

Scenario: Allow and audit additional http headers
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
		And I set the JWT Requested Record to the NHS Number for "patient2"
		And I set "AdditionalHeader" request header to "NotStandardHeader"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "document"