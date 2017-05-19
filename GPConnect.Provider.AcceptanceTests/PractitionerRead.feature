Feature: PractitionerRead

Background:
	Given I have the test patient codes
	Given I have the test practitioner codes
	Given I have the test ods codes

Scenario: Practitioner read successful request
	Given I get "practitioner1" id and save it as "practitioner1Id"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I get "practitioner1Id" id then make a GET request to "/Practitioner/""
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be an Practitioner resource

Scenario Outline: Practitioner read invalid request invalid id
	Given I get "practitioner1" id and save it as "practitioner1Id"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I make a GET request for a practitioner using an invalid id of "<InvalidId>"
	Then the response status code should be "400"
		And the response body should be FHIR JSON
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		Examples: 
		| InvalidId |
		| ##        |
		| 1@        |
		| 9i        |
		| 40-9      |

Scenario Outline: Practitioner read invalid request invalid URL
	Given I get "practitioner1" id and save it as "practitioner1Id"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I get "practitioner1Id" id then make a GET request to "<InvalidURL>""
	Then the response status code should be "404"
		Examples: 
		| InvalidURL      |
		| /Practitioners/ |
		| /Practitioner!/ |
		| /Practitioner2/ |
		



