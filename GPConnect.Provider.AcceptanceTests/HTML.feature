@http
Feature: Html

Background:
	Given I have the following patient records
		| Id                      | NHSNumber  |
		| patient1                | 9990049416 |
		| patient2                | 9990049424 |
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
		| patient1 | REF  | Referrals |
		| patient1 | SUM  | Active Problems and Issues,Current Medication Issues,Current Repeat Medications,Current Allergies and Adverse Reactions,Encounters |
	
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
		| patient1 | ADM      | Date,Entry,Details                                                                       | 1                |
		| patient1 | ALL      | Start Date,Details                                                                       | 1                |
		| patient1 | ALL      | Start Date,End Date,Details                                                              | 2                |
		| patient1 | CLI      | Date,Entry,Details                                                                       | 1                |
		| patient1 | ENC      | Date,Title,Details                                                                       | 1                |
		| patient1 | IMM      | Date,Vaccination,Part,Contents,Details                                                   | 1                |
#        | patient1 | INV                                                                                      |                  |
		| patient1 | MED      | Start Date,Medication Item,Type,Scheduled End Date,Days Duration,Details                 | 1                |
		| patient1 | MED      | Last Issued,Medication Item,Start Date,Review Date,Number Issued,Max Issues,Details      | 2                |
		| patient1 | MED      | Start Date,Medication Item,Type,Last Issued,Review Date,Number Issued,Max Issued,Details | 3                |
		| patient1 | OBS      | Date,Entry,Value,Details                                                                 | 1                |
#        | patient1 | PAT                                                                                      |                  |
		| patient1 | PRB      | Start Date,Entry,Significance,Details                                                    | 1                |
		| patient1 | PRB      | Start Date,End Date,Entry,Significance,Details                                           | 2                |
		| patient1 | REF      | Date,From,To,Priority,Details                                                            | 1                |
		| patient1 | SUM      | Start Date,Entry,Significance,Details                                                    | 1				|
		| patient1 | SUM      | Start Date,Medication Item,Type,Scheduled End Date,Days Duration,Details                 | 2				|
		| patient1 | SUM      | Last Issued,Medication Item,Start Date,Review Date,Number Issued,Max Issues,Details      | 3				|
		| patient1 | SUM      | Start Date,Details                                                                       | 4				|
		| patient1 | SUM      | Date,Title,Details                                                                       | 5				|

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
		| ADM  | patient1 | 2014-05-03    | 2016-09-14  | 03-May-2014   | 14-Sep-2016 |
		| CLI  | patient1 | 2014-02-03    | 2016-01-24  | 04-Feb-2014   | 24-Jan-2016 |
		| ENC  | patient1 | 1982-10-05    | 2016-09-01  | 05-Oct-1982   | 01-Sep-2016 |
		| SUM  | patient1 | 2014-03-21    | 2016-12-14  | 21-Mar-2014   | 14-Dec-2016 |
		| REF  | patient1 | 2014-03-21    | 2016-12-14  | 21-Mar-2014   | 14-Dec-2016 |
		| ADM  | patient1 | 2014-05       | 2016-09     | 01-May-2014   | 01-Sep-2016 |
		| CLI  | patient1 | 2014-02       | 2016-01     | 01-Feb-2014   | 01-Jan-2016 |
		| ENC  | patient1 | 2014-10       | 2016-09     | 01-Oct-2014   | 01-Sep-2016 |
		| SUM  | patient1 | 2014-03       | 2016-12     | 01-Mar-2014   | 01-Dec-2016 |
		| REF  | patient1 | 2014-03       | 2016-12     | 01-Mar-2014   | 01-Dec-2016 |
		| ADM  | patient1 | 1992          | 2016        | 01-Jan-1992   | 01-Jan-2016 |
		| CLI  | patient1 | 2014          | 2017        | 01-Jan-2014   | 01-Jan-2017 |
		| ENC  | patient1 | 2012          | 2014        | 01-Jan-2012   | 01-Jan-2014 |
		| SUM  | patient1 | 2015          | 2015        | 01-Jan-2015   | 01-Jan-2015 |
		| REF  | patient1 | 2016          | 2016        | 01-Jan-2016   | 01-Jan-2016 |
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
		| ADM  | patient1 |
		| CLI  | patient1 |
		| ENC  | patient1 |
		| SUM  | patient1 |
		| REF  | patient1 |
		| ALL  | patient1 |
		| IMM  | patient1 |
		| MED  | patient1 |
		| OBS  | patient1 |
		| PRB  | patient1 |
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
		| ADM  | patient1 | 2014-05-03    | 2016-09-14  | 03-May-2014   | 14-Sep-2016 | Administrative Items                    |
		| CLI  | patient1 | 2014-02-03    | 2016-01-24  | 04-Feb-2014   | 24-Jan-2016 | Clinical Items                          |
		| ENC  | patient1 | 1982-10-05    | 2016-09-01  | 05-Oct-1982   | 01-Sep-2016 | Encounters                              |
		| SUM  | patient1 | 2014-03-21    | 2016-12-14  | 21-Mar-2014   | 14-Dec-2016 | Active Problems and Issues              |
		| SUM  | patient1 | 2014-03-21    | 2016-12-14  | 21-Mar-2014   | 14-Dec-2016 | Current Medication Issues               |
		| SUM  | patient1 | 2014-03-21    | 2016-12-14  | 21-Mar-2014   | 14-Dec-2016 | Current Repeat Medications              |
		| SUM  | patient1 | 2014-03-21    | 2016-12-14  | 21-Mar-2014   | 14-Dec-2016 | Current Allergies and Adverse Reactions |
		| SUM  | patient1 | 2014-03-21    | 2016-12-14  | 21-Mar-2014   | 14-Dec-2016 | Encounters                              |
		| REF  | patient1 | 2014-03-21    | 2016-12-14  | 21-Mar-2014   | 14-Dec-2016 | Referrals                               |
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
		| ADM  | patient1 | Administrative Items                       |
		| CLI  | patient1 | Clinical Items                             |
		| ENC  | patient1 | Encounters                                 |
		| SUM  | patient1 | Active Problems and Issues                 |
		| SUM  | patient1 | Current Medication Issues                  |
		| SUM  | patient1 | Current Repeat Medications                 |
		| SUM  | patient1 | Current Allergies and Adverse Reactions    |
		| SUM  | patient1 | Encounters                                 |
		| REF  | patient1 | Referrals                                  |
		| ALL  | patient1 | Current Allergies and Adverse Reactions    |
		| ALL  | patient1 | Historical Allergies and Adverse Reactions |
		| IMM  | patient1 | Immunisations                              |
		| MED  | patient1 | Current Medication Issues                  |
		| MED  | patient1 | Current Repeat Medications                 |
		| MED  | patient1 | Past Medications                           |
		| OBS  | patient1 | Observations                               |
		| PRB  | patient1 | Active Problems and Issues                 |
		| PRB  | patient1 | Inactive Problems and Issues               |
	#	| INV ||||||
	#	| PAT ||||||

@ignore @Manual
Scenario: System does not support section html response where appropriate