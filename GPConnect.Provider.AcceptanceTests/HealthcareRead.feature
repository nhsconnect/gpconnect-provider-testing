@HealthcareRead @1.2.8-Only @1.2.8-Full-Pack
Feature: HealthcareRead

#WIP
#Perms
#1 - read a specific id
#2 - read a id that doesnt exist
#3 - read - no id passed - error?

Scenario Outline: Healthcare service read successful request validate the response contains logical identifier
	Given I set the Get Request Id to the Logical Identifer for Healthcare Service "<HealthCareService>"
	Given I configure the default "HealthcareRead" request
	When I make the "HealthcareRead" request
	Then the response status code should indicate success
		And the Response Resource should be a Healthcare Service
		And the Healthcare Id should match the GET request Id
		And the Healthcare service should be valid
	Examples:
		| HealthCareService |
		| HEALTH1     |
		| HEALTH2     |


Scenario Outline: Healthcare service Read with valid identifier which does not exist on providers system
	Given I configure the default "HealthcareRead" request
	And I set the Read Operation logical identifier used in the request to "<LogicalId>"
	When I make the "HealthcareRead" request
	Then the response status code should be "404"
	Examples:
		| LogicalId   |
		| mfpBm       |
		| 231Zcr64    |
		| th.as.e     |
		| 11dd4.45-23 |
		| 40-95-3     |
		| a-tm.mss..s |


Scenario: Healthcare service Read with NO identifier expect fail
	Given I configure the default "HealthcareRead" request
	And I set the Read Operation logical identifier used in the request to ""
	When I make the "HealthcareRead" request
	Then the response status code should be "400"
	

