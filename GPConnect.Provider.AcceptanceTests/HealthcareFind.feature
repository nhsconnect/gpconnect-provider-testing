@HealthcareFind @1.2.8-Only @1.2.8-Full-Pack
Feature: HealthcareFind


Scenario: Healthcare service find with no service id expect all services returned
	Given I configure the default "HealthcareFind" request
	When I make the "HealthcareFind" request
	Then the response status code should indicate success
	And the response should be a Bundle resource of type "searchset" 
	Then the response searchset contains atleast one HealthService
	And the response searchset contains valid Healthcare Service resources

	#TDODO update data requirements to have atleast one healthcare service setuop with DOS ID.
	
	Scenario: Find a healthcare Service with a DOS ID Expect success
	Given I configure the default "HealthcareFind" request
	When I make the "HealthcareFind" request
	Then the response status code should indicate success
	And the response should be a Bundle resource of type "searchset" 
	Then the response searchset contains atleast one HealthService
	And the response searchset contains valid Healthcare Service resources
	And I Store the DOS id from the first Healthcare service returned
	And I set the Healthcare Find Operation to use the stored DOS ID from previous find all
	When I make the "HealthcareFind" request
	Then the response status code should indicate success


	#change      private static HttpRequestConfiguration HealthcareFindConfiguration() to check if ID passed and add on if present with correct parm
	#Given I set the Get Request Id to the Logical Identifer for Healthcare Service "<HealthCareService>"


	#TODO - Test find a service that doesnt exist

	
	
	