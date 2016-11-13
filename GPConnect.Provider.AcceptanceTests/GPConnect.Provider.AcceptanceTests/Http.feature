Feature: Http

Scenario: Perform a successful GET request
	Given I am using server "http://fhirtest.uhn.ca/"
	And I am not using a proxy server
	And I set base URL to "/baseDstu2"
	And I am using "application/json+fhir" to communicate with the server
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
	And the response body should be FHIR JSON
	And the JSON value "resourceType" should be "Conformance"
