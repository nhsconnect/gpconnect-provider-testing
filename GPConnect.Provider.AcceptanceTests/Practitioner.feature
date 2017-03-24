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
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset


Scenario: PractitionerRequestInValidPractitioner
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
	And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G9999999"
	When I make a GET request to "/Practitioner?"
	Then the response status code should indicate success
	And the response body should be FHIR JSON
	And the JSON response should be a Bundle resource
	And the JSON response bundle should be type searchset


Scenario: PractitionerRequestWithCorrectSystem
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
			And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
	When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset


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
		Then the response status code should be "400"


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
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset
		

Scenario:  PractitionerRequestWhereFormatAddedABeforeIdentifier
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "_format" with the value "application/json+fhir"
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
		When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset

Scenario: PractionerRequestUsingAcceptHeaderForJson
Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I set the Accept header to "application/json+fhir"
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
		When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset


Scenario: PractionerRequestUsingAcceptHeaderForXML
Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I set the Accept header to "application/xml+fhir"
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
		When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset

Scenario: PractionerRequestUsingAcceptHeaderForText/xml
Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I set the Accept header to "text/xml"
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
		When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset

Scenario:  PractitionerRequestWhereFormatIsAddedToGetTheXML
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "_format" with the value "application/xml+fhir"
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
		When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
		And the response body should be FHIR XML
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset

Scenario:  PractitionerRequestWhereAcceptAndFormatHeaderIsSentToRequestXML
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "_format" with the value "application/xml+fhir"
		And I set the Accept header to "application/xml+fhir"
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
		When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
		And the response body should be FHIR XML
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset

Scenario:  PractitionerRequestWhereAcceptAndFormatHeaderIsSentToRequestJSON
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "_format" with the value "application/json+fhir"
		And I set the Accept header to "application/json+fhir"
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
		When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset
@Ignore
#NEED TO ENCODE HERE, LOOK HOW TO DO IT 
Scenario:  PractitionerRequestSendingCodeTest
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "_format" with the value "application/json+fhir"
		And I set the Accept header to "application/json+fhir"
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
		When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response bundle should be type searchset

Scenario:  PractitionerRequestSendWithOrganizationOdsCode
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "_format" with the value "application/json+fhir"
		And I set the Accept header to "application/json+fhir"
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
		When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset

Scenario: SSP From header not included in Practitioner request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
	And I do not send header "Ssp-From"
	And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
	When I make a GET request to "/Practitioner?"
	Then the response status code should indicate failure
	And the response body should be FHIR JSON

Scenario: SSP To header not included in Practitioner request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
	And I do not send header "Ssp-To"
	And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
	When I make a GET request to "/Practitioner?"
	Then the response status code should indicate failure
	And the response body should be FHIR JSON

Scenario: SSP Trace Id header not included in Practitioner request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
	And I do not send header "Ssp-TraceID"
	And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
	When I make a GET request to "/Practitioner?"
	Then the response status code should indicate failure
	And the response body should be FHIR JSON

Scenario: SSP InteractionId not included in Practitioner request
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
	And I do not send header "Ssp-InteractionId"
	And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
	When I make a GET request to "/Practitioner?"
	Then the response status code should indicate failure
	And the response body should be FHIR JSON

Scenario: PractitionerRequestSendValidInteractionId
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
	And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
	Then the interactionId "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" should be valid
	When I make a GET request to "/Practitioner?"
	Then the response status code should indicate success
	And the response body should be FHIR JSON
	And the JSON response should be a Bundle resource
	And the JSON response bundle should be type searchset

Scenario: PractitionerRequestSendValidInteractionIdToIncorrectEndPoint
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:patient" interaction
	And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
	When I make a GET request to "/Practitioner?"
	Then the response status code should be "400"
	And the response body should be FHIR JSON
	And the JSON response should be a OperationOutcome resource

Scenario: PractitionerRequestSendInValidInteractionId
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
	And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
	Then the interactionId "urn:nhs:nasmes:servsices:gpconnects:fhir:rest:search:prassctitioner" should be Invalid
	When I make a GET request to "/Practitioner?"
	Then the response status code should indicate success
	And the response body should be FHIR JSON
	And the JSON response should be a Bundle resource
	And the JSON response bundle should be type searchset
	
Scenario: PractitionerResponseMultiplePractitionersContainsMetaDataAndPopulatedFields
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
	And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
	When I make a GET request to "/Practitioner?"
	Then the response status code should indicate success
	And the response body should be FHIR JSON
	And the JSON response should be a Bundle resource
	And if the response bundle contains a practitioner resource it should contain meta data profile and version id
	And the JSON response bundle should be type searchset

@ignore FIGURE OUT HOW TO READ ID
Scenario: PractitionerResponseSinglePractitionerContainsMetaDataAndPopulatedFields
	Given I am using the default server
	And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:read:practitioner" interaction
	When I make a GET request to "/Practitioner/1"
	Then the response status code should indicate success
	And the response body should be FHIR JSON
	And the JSON response should be a Bundle resource
	And if the response bundle contains a practitioner resource it should contain meta data profile and version id
	And the JSON response bundle should be type searchset


Scenario: PractitionerResponseContainsSDSUserIdentifier
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
	When I make a GET request to "/Practitioner?"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
	Then practitioner contains SDS identifier "http://fhir.nhs.net/Id/sds-user-id"
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset


Scenario: PractitionerResponseSendsBackUserIdWithSystemAndValue
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
	When I make a GET request to "/Practitioner?"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset
		Then practitioner resources must only contain one user id and one profile id

Scenario: PractitionerResponseSendsBackUserWithNameElement
		Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
		When I make a GET request to "/Practitioner?"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the JSON response bundle should be type searchset
		Then practitioner resources should contain a single name element


Scenario: PractitionerResponseSendsBackUserWithValidFamilyName
		Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
		When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
			And the response body should be FHIR JSON
		Then practitioner family name should equal "Parsons"
			And the JSON response should be a Bundle resource
			And the JSON response bundle should be type searchset
		


Scenario: PractitionerResponseSendsBackUserWithOnlyOneFamilyName
Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
		When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
			And the response body should be FHIR JSON
		Then practitioner should only have one family name
			And the JSON response should be a Bundle resource
			And the JSON response bundle should be type searchset


Scenario: PractitionerResponseReturnsPractitionerRoleElementWithValidParameters
Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:rest:search:practitioner" interaction
		And I add the parameter "identifier" with the value "http://fhir.nhs.net/Id/sds-user-id"|G11111116"
		When I make a GET request to "/Practitioner?"
		Then the response status code should indicate success
			And the response body should be FHIR JSON
			And the JSON response should be a Bundle resource
			And the JSON response bundle should be type searchset
			Given There is a practitionerRoleElement
			Then if practitionerRole has role element which contains a coding then the system, code and display must exist
			Then if practitionerRole has managingOrganization element then reference must exist

