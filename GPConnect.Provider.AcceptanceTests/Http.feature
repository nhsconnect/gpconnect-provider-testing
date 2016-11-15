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

@stdHeaders
Scenario: GET_MetaData
	Given I am using server "http://gpconnect-uat.answerappcloud.com/"
	And I am not using a proxy server
	And I set base URL to "/fhir"
	And I am using "application/json+fhir" to communicate with the server
	And I set "Accept" request header to "application/json+fhir"
	And I am accredited system "200000000359"
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:metadata" interaction
	And I am connecting to accredited system "200000000360"
	And I am generating a random message trace identifier
	When I make a GET request to "/metadata"
	Then the response status code should indicate success
	And the response body should be FHIR JSON
	And the JSON value "resourceType" should be "Conformance"
