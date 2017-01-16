@http
Feature: Html

Background:
	Given I have the following patient records
		| Id                      | NHSNumber  |
		| patient1                | 9476719931 |
		| patient2                | 9476719974 |
		| patientNotInSystem      | 9999999999 |
		| patientNoSharingConsent | 9476719958 |

@ignore
Scenario: HTML does not contain disallowed elements
	# script tags
	# style tags
	# inline styling
	#head
	#body
	#external references
	#form
	#links
	#frames
	#iframes
	#event attributes (onclick etc)
	# non xhtml attributes and tags

@ignore @Manual
Scenario: System does not support section html response where appropriate

@ignore
Scenario: section header present

@ignore
Scenario: content table headers present
