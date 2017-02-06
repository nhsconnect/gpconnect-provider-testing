@http
Feature: Html

Background:
	Given I have the following patient records
		| Id                      | NHSNumber  |
		| PWTP2                   | 9990049416 |
		| PWTP3                   | 9990049424 |
		| patientNotInSystem      | 9999999999 |
		| patientNoSharingConsent | 9476719958 |

Scenario Outline: HTML does not contain disallowed elements
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "PWTP2"
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
		| REF  |
		| SUM  |

Scenario Outline: html section headers present
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
		| PWTP2 | ADM  | Administrative Items |
		| PWTP2 | ALL  | Current Allergies and Adverse Reactions,Historical Allergies and Adverse Reactions |
		| PWTP2 | CLI  | Clinical Items |
		| PWTP2 | ENC  | Encounters |
		| PWTP2 | IMM  | Immunisations |
		#| PWTP2 | INV | Investigations |
		| PWTP2 | MED  | Current Medication Issues,Current Repeat Medications,Past Medications |
		| PWTP2 | OBS  | Observations |
		#| PWTP2 | PAT |  |
		| PWTP2 | PRB  | Active Problems and Issues,Inactive Problems and Issues |
		| PWTP2 | REF  | Referrals |
		| PWTP2 | SUM  | Active Problems and Issues,Current Medication Issues,Current Repeat Medications,Current Allergies and Adverse Reactions,Encounters |
	
	# NEED TO EXPAND TEST TO PATIENT WITH NO RETURNED DETAILS AND PATIENT WITH SOME SECTIONS AND ONLY CURRENT OR PAST MEDICATIONS, ONLY HISTORICAL ALLERGIES ETC

Scenario Outline: html table headers present and in order that is expected
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
		| PWTP2 | ADM      | Date,Entry,Details                                                                       | 1                |
		| PWTP2 | ALL      | Start Date,Details                                                                       | 1                |
		| PWTP2 | ALL      | Start Date,End Date,Details                                                              | 2                |
		| PWTP2 | CLI      | Date,Entry,Details                                                                       | 1                |
		| PWTP2 | ENC      | Date,Title,Details                                                                       | 1                |
		| PWTP2 | IMM      | Date,Vaccination,Part,Contents,Details                                                   | 1                |
#        | PWTP2 | INV                                                                                      |                  |
		| PWTP2 | MED      | Start Date,Medication Item,Type,Scheduled End Date,Days Duration,Details                 | 1                |
		| PWTP2 | MED      | Last Issued,Medication Item,Start Date,Review Date,Number Issued,Max Issues,Details      | 2                |
		| PWTP2 | MED      | Start Date,Medication Item,Type,Last Issued,Review Date,Number Issued,Max Issued,Details | 3                |
		| PWTP2 | OBS      | Date,Entry,Value,Details                                                                 | 1                |
#        | PWTP2 | PAT                                                                                      |                  |
		| PWTP2 | PRB      | Start Date,Entry,Significance,Details                                                    | 1                |
		| PWTP2 | PRB      | Start Date,End Date,Entry,Significance,Details                                           | 2                |
		| PWTP2 | REF      | Date,From,To,Priority,Details                                                            | 1                |
		| PWTP2 | SUM      | Start Date,Entry,Significance,Details                                                    | 1				|
		| PWTP2 | SUM      | Start Date,Medication Item,Type,Scheduled End Date,Days Duration,Details                 | 2				|
		| PWTP2 | SUM      | Last Issued,Medication Item,Start Date,Review Date,Number Issued,Max Issues,Details      | 3				|
		| PWTP2 | SUM      | Start Date,Details                                                                       | 4				|
		| PWTP2 | SUM      | Date,Title,Details                                                                       | 5				|

Scenario Outline: filtered sections should contain date range section banner
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "<Patient>"
		And I set a time period parameter start date to "<StartDateTime>" and end date to "<EndDateTime>"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the response html should contain the applied date range text "<TextStartDate>" to "<TextEndDate>"
	Examples:
		| Code | Patient  | StartDateTime | EndDateTime | TextStartDate | TextEndDate |
		| ADM  | PWTP2 | 2014-05-03    | 2016-09-14  | 03-May-2014   | 14-Sep-2016 |
		| CLI  | PWTP2 | 2014-02-03    | 2016-01-24  | 04-Feb-2014   | 24-Jan-2016 |
		| ENC  | PWTP2 | 1982-10-05    | 2016-09-01  | 05-Oct-1982   | 01-Sep-2016 |
		| SUM  | PWTP2 | 2014-03-21    | 2016-12-14  | 21-Mar-2014   | 14-Dec-2016 |
		| REF  | PWTP2 | 2014-03-21    | 2016-12-14  | 21-Mar-2014   | 14-Dec-2016 |
		| ADM  | PWTP2 | 2014-05       | 2016-09     | 01-May-2014   | 01-Sep-2016 |
		| CLI  | PWTP2 | 2014-02       | 2016-01     | 01-Feb-2014   | 01-Jan-2016 |
		| ENC  | PWTP2 | 2014-10       | 2016-09     | 01-Oct-2014   | 01-Sep-2016 |
		| SUM  | PWTP2 | 2014-03       | 2016-12     | 01-Mar-2014   | 01-Dec-2016 |
		| REF  | PWTP2 | 2014-03       | 2016-12     | 01-Mar-2014   | 01-Dec-2016 |
		| ADM  | PWTP2 | 1992          | 2016        | 01-Jan-1992   | 01-Jan-2016 |
		| CLI  | PWTP2 | 2014          | 2017        | 01-Jan-2014   | 01-Jan-2017 |
		| ENC  | PWTP2 | 2012          | 2014        | 01-Jan-2012   | 01-Jan-2014 |
		| SUM  | PWTP2 | 2015          | 2015        | 01-Jan-2015   | 01-Jan-2015 |
		| REF  | PWTP2 | 2016          | 2016        | 01-Jan-2016   | 01-Jan-2016 |
	#	| INV ||||||
	#	| PAT ||||||

Scenario Outline: sections should contain the all data items section banner
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "<Patient>"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the response html should contain the all data items text
	Examples:
		| Code | Patient  |
		| ADM  | PWTP2 |
		| CLI  | PWTP2 |
		| ENC  | PWTP2 |
		| SUM  | PWTP2 |
		| REF  | PWTP2 |
		| ALL  | PWTP2 |
		| IMM  | PWTP2 |
		| MED  | PWTP2 |
		| OBS  | PWTP2 |
		| PRB  | PWTP2 |
	#	| INV ||||||
	#	| PAT ||||||
	
@ignore
Scenario Outline: filtered sections should return no data available html banner
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "<Patient>"
		And I set a time period parameter start date to "<StartDateTime>" and end date to "<EndDateTime>"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the response html should contain the applied date range text "<TextStartDate>" to "<TextEndDate>"
		And the response html should contain the no data available html banner in section "<Section>"
	Examples:
		| Code | Patient  | StartDateTime | EndDateTime | TextStartDate | TextEndDate | Section                                 |
		| ADM  | PWTP2 | 2014-05-03    | 2016-09-14  | 03-May-2014   | 14-Sep-2016 | Administrative Items                    |
		| CLI  | PWTP2 | 2014-02-03    | 2016-01-24  | 04-Feb-2014   | 24-Jan-2016 | Clinical Items                          |
		| ENC  | PWTP2 | 1982-10-05    | 2016-09-01  | 05-Oct-1982   | 01-Sep-2016 | Encounters                              |
		| SUM  | PWTP2 | 2014-03-21    | 2016-12-14  | 21-Mar-2014   | 14-Dec-2016 | Active Problems and Issues              |
		| SUM  | PWTP2 | 2014-03-21    | 2016-12-14  | 21-Mar-2014   | 14-Dec-2016 | Current Medication Issues               |
		| SUM  | PWTP2 | 2014-03-21    | 2016-12-14  | 21-Mar-2014   | 14-Dec-2016 | Current Repeat Medications              |
		| SUM  | PWTP2 | 2014-03-21    | 2016-12-14  | 21-Mar-2014   | 14-Dec-2016 | Current Allergies and Adverse Reactions |
		| SUM  | PWTP2 | 2014-03-21    | 2016-12-14  | 21-Mar-2014   | 14-Dec-2016 | Encounters                              |
		| REF  | PWTP2 | 2014-03-21    | 2016-12-14  | 21-Mar-2014   | 14-Dec-2016 | Referrals                               |
	#	| INV ||||||
	#	| PAT ||||||

@ignore
Scenario Outline: sections should return no data available html banner
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "<Patient>"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the response html should contain the all data items text
		And the response html should contain the no data available html banner
	Examples:
		| Code | Patient  | Section                                    |
		| ADM  | PWTP2 | Administrative Items                       |
		| CLI  | PWTP2 | Clinical Items                             |
		| ENC  | PWTP2 | Encounters                                 |
		| SUM  | PWTP2 | Active Problems and Issues                 |
		| SUM  | PWTP2 | Current Medication Issues                  |
		| SUM  | PWTP2 | Current Repeat Medications                 |
		| SUM  | PWTP2 | Current Allergies and Adverse Reactions    |
		| SUM  | PWTP2 | Encounters                                 |
		| REF  | PWTP2 | Referrals                                  |
		| ALL  | PWTP2 | Current Allergies and Adverse Reactions    |
		| ALL  | PWTP2 | Historical Allergies and Adverse Reactions |
		| IMM  | PWTP2 | Immunisations                              |
		| MED  | PWTP2 | Current Medication Issues                  |
		| MED  | PWTP2 | Current Repeat Medications                 |
		| MED  | PWTP2 | Past Medications                           |
		| OBS  | PWTP2 | Observations                               |
		| PRB  | PWTP2 | Active Problems and Issues                 |
		| PRB  | PWTP2 | Inactive Problems and Issues               |
	#	| INV ||||||
	#	| PAT ||||||

@ignore
@Manual
Scenario: System does not support section html response where appropriate

@ignore
Scenario: Check html for non html formatting
	# new lines
	# tabs
	# coded characters

@ignore
@Manual
Scenario: Check dates are in decending order within the results tables

