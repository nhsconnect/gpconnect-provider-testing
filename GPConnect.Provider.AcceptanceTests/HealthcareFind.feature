@Healthcare @1.2.8-IncrementalAndRegression @1.2.8-Full-Pack
Feature: HealthcareFind

#NOTE Service Filtering Needs to be enabled for these Tests

Scenario: Healthcare service find with no service id expect all services returned
	Given I configure the default "HealthcareFind" request
	When I make the "HealthcareFind" request
	Then the response status code should indicate success
	And the response should be a Bundle resource of type "searchset" 
	Then the response searchset contains atleast one HealthService
	And the response searchset contains valid Healthcare Service resources

	
	
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
	And the response searchset has only One Healthcare Service resource
	And the response searchset contains valid Healthcare Service resources

	
Scenario: Healthcare service find with a DOS ID that does not exist
	And I configure the default "HealthcareFind" request
	Given I set the Find Operation DOS ID for the request to "zzzzzz"
	When I make the "HealthcareFind" request
	Then the response status code should indicate success
	And the response should be a Bundle resource of type "searchset" 
	And the response searchset has NO Healthcare Service resources

	
Scenario: CapabilityStatement profile supports the HealthcareService search operation
	Given I configure the default "MetadataRead" request
	When I make the "MetadataRead" request
	Then the response status code should indicate success
	And the CapabilityStatement REST Resources should contain the "HealthcareService" Resource with the "SearchType" Interaction
	
	
	