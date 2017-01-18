@http
Feature: Html

Background:
	Given I have the following patient records
		| Id                      | NHSNumber  |
		| patient1                | 9476719931 |
		| patient2                | 9476719974 |
		| patientNotInSystem      | 9999999999 |
		| patientNoSharingConsent | 9476719958 |

Scenario Outline: HTML does not contain disallowed elements
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient1"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the html should be valid xhtml
		And the html should not contain "head" tags
		And the html should not contain "body" tags
		And the html should not contain "script" tags
		And the html should not contain "style" tags
		And the html should not contain "iframe" tags
		And the html should not contain "form" tags
		And the html should not contain "a" tags
		And the html should not contain any attributes
	Examples:
		| Code |
		| ADM  |
		| ALL  |
		| CLI  |
		| ENC  |
		| IMM  |
		#| INV  |
		| MED  |
		| OBS  |
		#| PAT  |
		| PRB  |
		#| REF  |
		| SUM  |

Scenario Outline: section headers present
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "<Patient>"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the html should contain headers in coma seperated list "<Headers>"
	Examples:
		| Patient | Code | Headers |
		| patient1 | ADM  | Administrative Items |
		| patient1 | ALL  | Current Allergies and Adverse Reactions,Historical Allergies and Adverse Reactions |
		| patient1 | CLI  | Clinical Items |
		| patient1 | ENC  | Encounters |
		| patient1 | IMM  | Immunisations |
		#| patient1 | INV | Investigations |
		| patient1 | MED  | Current Medication Issues,Current Repeat Medications,Past Medications |
		| patient1 | OBS  | Observations |
		#| patient1 | PAT |  |
		| patient1 | PRB  | Active Problems and Issues,Inactive Problems and Issues |
		#| patient1 | REF | Referrals |
		| patient1 | SUM  | Active Problems and Issues,Current Medication Issues,Current Repeat Medications,Current Allergies and Adverse Reactions,Encounters |
	
	# NEED TO EXPAND TEST TO PATIENT WITH NO RETURNED DETAILS AND PATIENT WITH SOME SECTIONS AND ONLY CURRENT OR PAST MEDICATIONS, ONLY HISTORICAL ALLERGIES ETC


@ignore
Scenario Outline: content table headers present
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "<Patient>"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the html should contain table headers in coma seperated list order "<Headers>" for the "<PageSectionIndex>"
	Examples:
		| Patient  | Code     | Headers                                                                                  | PageSectionIndex |
		| patient1 | ADM      | Date,Entry,Details                                                                       | 1          |
		| patient1 | ALL      | Start Date,Details                                                                       | 1          |
		| patient1 | ALL      | Start Date,End Date,Details                                                              | 2          |
		| patient1 | CLI      | Date,Entry,Details                                                                       | 1          |
		| patient1 | ENC      | Date,Title,Details                                                                       | 1          |
		| patient1 | IMM      | Date,Vaccination,Part,Contents,Details                                                   | 1          |
#        | patient1 | INV                                                                                      |            |
		| patient1 | MED      | Start Date,Medication Item,Type,Scheduled End Date,Days Duration,Details                 | 1          |
		| patient1 | MED      | Last Issued,Medication Item,Start Date,Review Date,Number Issued,Max Issues,Details      | 2          |
		| patient1 | MED      | Start Date,Medication Item,Type,Last Issued,Review Date,Number Issued,Max Issued,Details | 3          |
		| patient1 | OBS      | Date,Entry,Value,Details                                                                 | 1          |
#        | patient1 | PAT                                                                                      |            |
		| patient1 | PRB      | Start Date,Entry,Significance,Details                                                    | 1          |
		| patient1 | PRB      | Start Date,End Date,Entry,Significance,Details                                           | 2          |
#        | patient1 | REF                                                                                      |            |
		| patient1 | SUM      | Start Date,Entry,Significance,Details                                                    | 1          |
		| patient1 | SUM      | Start Date,Medication Item,Type,Scheduled End Date,Days Duration,Details                 | 2          |
		| patient1 | SUM      | Last Issued,Medication Item,Start Date,Review Date,Number Issued,Max Issues,Details      | 3          |
		| patient1 | SUM      | Start Date,Details                                                                       | 4          |
		| patient1 | SUM      | Date,Title,Details                                                                       | 5          |

@ignore
Scenario: filtered sections should contain date range section banner

@ignore @Manual
Scenario: System does not support section html response where appropriate