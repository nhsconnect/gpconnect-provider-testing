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
	Examples:
		| HealthCareService |
		| HEALTH1     |
		| HEALTH2     |




