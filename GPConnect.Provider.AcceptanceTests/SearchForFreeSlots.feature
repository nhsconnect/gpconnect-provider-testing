@Appointment
Feature: SearchForFreeSlots
	
Background:
	Given I have the test ods codes
	
Scenario Outline: I successfully perform a gpc.getschedule operation
	Given I am using the default server
		And I search for the organization "<Organization>" on the providers system and save the first response to "<Organization>"
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getschedule" interaction
		And I add period request parameter with a start date of todays and an end date "8" days later
	When I send a gpc.getschedule operation for the organization stored as "<Organization>"
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the response should be a Bundle resource of type "searchset"
		#And the response bundle should contain an Organization resource matching the organization stored as "<Organization>"
	Examples:
		| Organization |
		| ORG1         |
		| ORG2         |