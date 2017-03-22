@Practitioner
Feature: Practitioner
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario: PractitionerRequestValidPractitioner
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
	When I make a GET request to "/Practitioner?"
	Then the response status code should indicate success
		And the response body should be FHIR JSON


Scenario: PractitionerRequestInValidPractitioner
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
	And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G9999999"
	When I make a GET request to "/Practitioner?"
	Then the response status code should indicate success
	And the response body should be FHIR JSON


Scenario: PractitionerRequestWithCorrectSystem
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
			And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
	When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
		And the response body should be FHIR JSON


Scenario:  PractitionerRequestWithInCorrectSystem 
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/incorrect-id"|G11111116"
	When I make a GET request to "/Practitioner?"
		Then the response status code should be "422"
		And the response body should be FHIR JSON


Scenario:  PractitionerRequestWithBlankSystem 
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "|G11111116"
	When I make a GET request to "/Practitioner?"
		Then the response status code should be "422"
		And the response body should be FHIR JSON


Scenario:  PractitionerRequestWithoutASystem 
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
			And I add the parameter "identifier" with the value "G11111116"
		When I make a GET request to "/Practitioner?"
		Then the response status code should be "422"
		And the response body should be FHIR JSON

Scenario: PractitionerRequestWithoutTheIdentifierParameter
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
	When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
		And the response body should be FHIR JSON


Scenario:  PractitionerRequestWhereTheIdentifierIsCorrect
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
			And I add the parameter "identifier" with the value "G11111116"
	When I make a GET request to "/Practitioner?"
		Then the response status code should be "422"
		And the response body should be FHIR JSON


Scenario:  PractitionerRequestWhereTheIdentifierContainsTheIncorrectCase
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
			And I add the parameter "Identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
	When I make a GET request to "/Practitioner?"
		Then the response status code should be "400"
		And the response body should be FHIR JSON


Scenario:  PractitionerRequestWhereTheIdentifierIsSpeltIncorrectly
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "idenddstifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
	When I make a GET request to "/Practitioner?"
		Then the response status code should be "400"
		And the response body should be FHIR JSON

Scenario:  PractitionerRequestWhereThereIsaNullValueElement
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"| null"
	When I make a GET request to "/Practitioner?"
		Then the response status code should be "422"
		And the response body should be FHIR JSON

Scenario:  PractitionerRequestWhereThereIsNoValueElement
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"
	When I make a GET request to "/Practitioner?"
		Then the response status code should be "422"
		And the response body should be FHIR JSON


Scenario:  PractitionerRequestWhereFormatAddedAfterIdentifier 
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
		And I add the parameter "_format" with the value "application/json+fhir"
	When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
		And the response body should be FHIR JSON
		

Scenario:  PractitionerRequestWhereFormatAddedABeforeIdentifier
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "_format" with the value "application/json+fhir"
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
		When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: PractionerRequestUsingAcceptHeaderForJson
Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I set the Accept header to "application/json+fhir"
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
		When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
		And the response body should be FHIR JSON


Scenario: PractionerRequestUsingAcceptHeaderForXML
Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I set the Accept header to "application/xml+fhir"
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
		When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario: PractionerRequestUsingAcceptHeaderForText/xml
Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I set the Accept header to "text/xml"
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
		When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
		And the response body should be FHIR JSON

Scenario:  PractitionerRequestWhereFormatIsAddedToGetTheXML
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "_format" with the value "application/xml+fhir"
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
		When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario:  PractitionerRequestWhereAcceptAndFormatHeaderIsSentToRequestXML
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "_format" with the value "application/xml+fhir"
		And I set the Accept header to "application/xml+fhir"
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
		When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
		And the response body should be FHIR XML

Scenario:  PractitionerRequestWhereAcceptAndFormatHeaderIsSentToRequestJSON
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "_format" with the value "application/json+fhir"
		And I set the Accept header to "application/json+fhir"
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
		When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
		And the response body should be FHIR JSON