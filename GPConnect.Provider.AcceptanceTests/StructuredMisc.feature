@Structured @StructuredMisc @1.5.0-Full-Pack @1.5.1
Feature: StructuredMisc

Scenario: Structured Retrieve request for Patient FullRecord sent expect success
	Given I configure the default "GpcGetStructuredRecord" request
		And I add an NHS Number parameter for "patient2"
		And I add the allergies parameter with resolvedAllergies set to "true"
		And I add the medication parameter with includePrescriptionIssues set to "true"
		And I add the includeConsultations parameter only
		And I add the Problems parameter
		And I add the immunizations parameter
		And I add the uncategorised data parameter
		And I add the Investigations parameter
		And I add the Referrals parameter
    When I make the "GpcGetStructuredRecord" request
	Then the response status code should indicate success
		And check that the bundle does not contain any duplicate resources
		And the patient resource in the bundle should contain meta data profile and version id
		And check the response does not contain an operation outcome	


