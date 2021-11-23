@HealthcareFind @1.2.8-Only @1.2.8-Full-Pack
Feature: HealthcareFind


#todo
# add check for validating health care resources

Scenario: Healthcare service find with no service id expect all services returned
	Given I configure the default "HealthcareFind" request
	When I make the "HealthcareRead" request
	Then the response status code should indicate success
	And the response should be a Bundle resource of type "searchset" 
	Then the response searchset contains atleast one HealthService
	And the response searchset contains valid Healthcare Service resources
	
	
	