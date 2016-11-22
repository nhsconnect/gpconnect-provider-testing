﻿Feature: Http

Scenario: Http Perform a successful GET request
	Given I am using server "fhirtest.uhn.ca" on port "80"
	And I am not using TLS Connection
	And I am not using the spine proxy server
	And I set base URL to "/baseDstu2"
	And I am using "application/json+fhir" to communicate with the server
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
	And the response body should be FHIR JSON
	And the JSON value "resourceType" should be "Conformance"

