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
		And I author a request for the "<Code>" care record section for config patient "patient1"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the html should not contain headers in coma seperated list "<Headers>"
	Examples:
		| Code | Headers |
		| ADM  | Administrative Items |
		| ALL  | Current Allergies and Adverse Reactions,Historical Allergies and Adverse Reactions |
		| CLI  | Clinical Items |
		| ENC  | Encounters |
		| IMM  | Immunisations |
		#| INV | Investigations |
		| MED  | Current Medication Issues,Current Repeat Medications,Past Medications |
		| OBS  | Observations |
		#| PAT |  |
		| PRB  | Active Problems and Issues,Inactive Problems and Issues |
		#| REF | Referrals |
		| SUM  | Active Problems and Issues,Current Medication Issues,Current Repeat Medications,Current Allergies and Adverse Reactions,Encounters |

@ignore
Scenario: content table headers present

@ignore
Scenario: filtered sections should contain date range section banner

@ignore @Manual
Scenario: System does not support section html response where appropriate