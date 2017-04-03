@http
Feature: Html

Background:
	Given I have the following patient records
		| Id                 | NHSNumber  |
		| patientNotInSystem | 9999999999 |
		| patient1           | 9000000001 |
		| patient2           | 9000000002 |
		| patient3           | 9000000003 |
		| patient4           | 9000000004 |
		| patient5           | 9000000005 |
		| patient6           | 9000000006 |
		| patient7           | 9000000007 |
		| patient8           | 9000000008 |
		| patient9           | 9000000009 |
		| patient10          | 9000000010 |
		| patient11          | 9000000011 |
		| patient12          | 9000000012 |
		| patient13          | 9000000013 |
		| patient14          | 9000000014 |
		| patient15          | 9000000015 |

Scenario Outline: HTML should not contain disallowed elements
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
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
		| Patient  | Code | Headers |
		| patient1 | ADM  | Administrative Items |
		| patient2 | ADM  | Administrative Items |
		| patient1 | ALL  | Current Allergies and Adverse Reactions,Historical Allergies and Adverse Reactions |
		| patient2 | ALL  | Current Allergies and Adverse Reactions,Historical Allergies and Adverse Reactions |
		| patient3 | ALL  | Current Allergies and Adverse Reactions,Historical Allergies and Adverse Reactions |
		| patient4 | ALL  | Current Allergies and Adverse Reactions,Historical Allergies and Adverse Reactions |
		| patient1 | CLI  | Clinical Items |
		| patient2 | CLI  | Clinical Items |
		| patient1 | ENC  | Encounters |
		| patient2 | ENC  | Encounters |
		| patient1 | IMM  | Immunisations |
		| patient2 | IMM  | Immunisations |
		#| patient1 | INV | Investigations |
		#| patient2 | INV | Investigations |
		| patient1 | MED  | Current Medication Issues,Current Repeat Medications,Past Medications |
		| patient2 | MED  | Current Medication Issues,Current Repeat Medications,Past Medications |
		| patient3 | MED  | Current Medication Issues,Current Repeat Medications,Past Medications |
		| patient4 | MED  | Current Medication Issues,Current Repeat Medications,Past Medications |
		| patient5 | MED  | Current Medication Issues,Current Repeat Medications,Past Medications |
		| patient6 | MED  | Current Medication Issues,Current Repeat Medications,Past Medications |
		| patient7 | MED  | Current Medication Issues,Current Repeat Medications,Past Medications |
		| patient8 | MED  | Current Medication Issues,Current Repeat Medications,Past Medications |
		| patient1 | OBS  | Observations |
		| patient2 | OBS  | Observations |
		#| patient1 | PAT |  |
		#| patient2 | PAT |  |
		| patient1 | PRB  | Active Problems and Issues,Inactive Problems and Issues |
		| patient2 | PRB  | Active Problems and Issues,Inactive Problems and Issues |
		| patient3 | PRB  | Active Problems and Issues,Inactive Problems and Issues |
		| patient4 | PRB  | Active Problems and Issues,Inactive Problems and Issues |
		| patient1 | REF  | Referrals |
		| patient2 | REF  | Referrals |
		| patient1 | SUM  | Active Problems and Issues,Current Medication Issues,Current Repeat Medications,Current Allergies and Adverse Reactions,Last 3 Encounters |
		| patient2 | SUM  | Active Problems and Issues,Current Medication Issues,Current Repeat Medications,Current Allergies and Adverse Reactions,Last 3 Encounters |
		| patient3 | SUM  | Active Problems and Issues,Current Medication Issues,Current Repeat Medications,Current Allergies and Adverse Reactions,Last 3 Encounters |
		| patient4 | SUM  | Active Problems and Issues,Current Medication Issues,Current Repeat Medications,Current Allergies and Adverse Reactions,Last 3 Encounters |
		| patient5 | SUM  | Active Problems and Issues,Current Medication Issues,Current Repeat Medications,Current Allergies and Adverse Reactions,Last 3 Encounters |
		| patient6 | SUM  | Active Problems and Issues,Current Medication Issues,Current Repeat Medications,Current Allergies and Adverse Reactions,Last 3 Encounters |
		| patient7 | SUM  | Active Problems and Issues,Current Medication Issues,Current Repeat Medications,Current Allergies and Adverse Reactions,Last 3 Encounters |
		| patient8 | SUM  | Active Problems and Issues,Current Medication Issues,Current Repeat Medications,Current Allergies and Adverse Reactions,Last 3 Encounters |

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
		| patient2 | ADM      | Date,Entry,Details                                                                       | 1                |
		| patient2 | ALL      | Start Date,Details                                                                       | 1                |
		| patient2 | ALL      | Start Date,End Date,Details                                                              | 2                |
		| patient2 | CLI      | Date,Entry,Details                                                                       | 1                |
		| patient2 | ENC      | Date,Title,Details                                                                       | 1                |
		| patient2 | IMM      | Date,Vaccination,Part,Contents,Details                                                   | 1                |
#        | patient2 | INV      |                                                                                          |                  |
		| patient2 | MED      | Start Date,Medication Item,Type,Scheduled End,Days Duration,Details                      | 1                |
		| patient2 | MED      | Last Issued,Medication Item,Start Date,Review Date,Number Issued,Max Issues,Details      | 2                |
		| patient2 | MED      | Start Date,Medication Item,Type,Last Issued,Review Date,Number Issued,Max Issues,Details | 3                |
		| patient2 | OBS      | Date,Entry,Value,Details                                                                 | 1                |
#        | patient2 | PAT      |                                                                                          |                  |
		| patient2 | PRB      | Start Date,Entry,Significance,Details                                                    | 1                |
		| patient2 | PRB      | Start Date,End Date,Entry,Significance,Details                                           | 2                |
		| patient2 | REF      | Date,From,To,Priority,Details                                                            | 1                |
		| patient2 | SUM      | Start Date,Entry,Significance,Details                                                    | 1				|
		| patient2 | SUM      | Start Date,Medication Item,Type,Scheduled End,Days Duration,Details                      | 2				|
		| patient2 | SUM      | Last Issued,Medication Item,Start Date,Review Date,Number Issued,Max Issues,Details      | 3				|
		| patient2 | SUM      | Start Date,Details                                                                       | 4				|
		| patient2 | SUM      | Date,Title,Details                                                                       | 5				|

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
		| ADM  | patient2 | 2014-05-03    | 2016-09-14  | 03-May-2014   | 14-Sep-2016 |
		| CLI  | patient1 | 2014-02-03    | 2016-01-24  | 03-Feb-2014   | 24-Jan-2016 |
		| CLI  | patient2 | 2014-02-03    | 2016-01-24  | 03-Feb-2014   | 24-Jan-2016 |
		| ENC  | patient1 | 1982-10-05    | 2016-09-01  | 05-Oct-1982   | 01-Sep-2016 |
		| ENC  | patient2 | 1982-10-05    | 2016-09-01  | 05-Oct-1982   | 01-Sep-2016 |
		| REF  | patient1 | 2014-03-21    | 2016-12-14  | 21-Mar-2014   | 14-Dec-2016 |
		| REF  | patient2 | 2014-03-21    | 2016-12-14  | 21-Mar-2014   | 14-Dec-2016 |
		| ADM  | patient1 | 2014-05       | 2016-09     | 01-May-2014   | 30-Sep-2016 |
		| ADM  | patient2 | 2014-05       | 2016-09     | 01-May-2014   | 30-Sep-2016 |
		| CLI  | patient1 | 2014-02       | 2016-01     | 01-Feb-2014   | 31-Jan-2016 |
		| CLI  | patient2 | 2014-02       | 2016-01     | 01-Feb-2014   | 31-Jan-2016 |
		| ENC  | patient1 | 2014-10       | 2016-09     | 01-Oct-2014   | 30-Sep-2016 |
		| ENC  | patient2 | 2014-10       | 2016-09     | 01-Oct-2014   | 30-Sep-2016 |
		| REF  | patient1 | 2014-03       | 2016-12     | 01-Mar-2014   | 31-Dec-2016 |
		| REF  | patient2 | 2014-03       | 2016-12     | 01-Mar-2014   | 31-Dec-2016 |
		| ADM  | patient1 | 1992          | 2016        | 01-Jan-1992   | 31-Dec-2016 |
		| ADM  | patient2 | 1992          | 2016        | 01-Jan-1992   | 31-Dec-2016 |
		| CLI  | patient1 | 2014          | 2017        | 01-Jan-2014   | 31-Dec-2017 |
		| CLI  | patient2 | 2014          | 2017        | 01-Jan-2014   | 31-Dec-2017 |
		| ENC  | patient1 | 2012          | 2014        | 01-Jan-2012   | 31-Dec-2014 |
		| ENC  | patient2 | 2012          | 2014        | 01-Jan-2012   | 31-Dec-2014 |
		| REF  | patient1 | 2016          | 2016        | 01-Jan-2016   | 31-Dec-2016 |
		| REF  | patient2 | 2016          | 2016        | 01-Jan-2016   | 31-Dec-2016 |
		| MED  | patient1 | 2014-03       | 2016-12     | 01-Mar-2014   | 31-Dec-2016 |
		| MED  | patient2 | 2014-02-03    | 2016-01-24  | 03-Feb-2014   | 24-Jan-2016 |
		| OBS  | patient1 | 2014-03       | 2016-12     | 01-Mar-2014   | 31-Dec-2016 |
		| OBS  | patient2 | 2014-02-03    | 2016-01-24  | 03-Feb-2014   | 24-Jan-2016 |
		| PRB  | patient1 | 2014-03       | 2016-12     | 01-Mar-2014   | 31-Dec-2016 |
		| PRB  | patient2 | 2014-02-03    | 2016-01-24  | 03-Feb-2014   | 24-Jan-2016 |
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
		| ADM  | patient2 |
		| CLI  | patient1 |
		| CLI  | patient2 |
		| ENC  | patient1 |
		| ENC  | patient2 |
		| SUM  | patient1 |
		| SUM  | patient2 |
		| REF  | patient1 |
		| REF  | patient2 |
		| ALL  | patient1 |
		| ALL  | patient2 |
		| IMM  | patient1 |
		| IMM  | patient2 |
		| MED  | patient1 |
		| MED  | patient2 |
		| OBS  | patient1 |
		| OBS  | patient2 |
		| PRB  | patient1 |
		| PRB  | patient2 |
	#	| INV ||||||
	#	| PAT ||||||
	
Scenario: Summary should contain a max of 3 encounters
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "SUM" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the response html for "Last 3 Encounters" section should contain a table with "3" rows
	
Scenario: Encounters section should contain all encounters
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "ENC" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the response html for "Encounters" section should contain a table with at least "4" rows

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
		| Code | Patient  | StartDateTime | EndDateTime | TextStartDate | TextEndDate | Section              |
		| ADM  | patient2 | 2014-05-03    | 2015-04-30  | 03-May-2014   | 30-Apr-2015 | Administrative Items |
		| CLI  | patient2 | 2014-02-03    | 2015-01-24  | 03-Feb-2014   | 24-Jan-2015 | Clinical Items       |
		| ENC  | patient2 | 1982-10-05    | 2015-04-30  | 05-Oct-1982   | 30-Apr-2015 | Encounters           |
		| REF  | patient2 | 2014-03-21    | 2015-03-14  | 21-Mar-2014   | 14-Mar-2015 | Referrals            |
	#	| INV ||||||
	#	| PAT ||||||

Scenario Outline: sections should return no data available html banner
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "<Patient>"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the response html should contain the all data items text
		And the response html should contain the no data available html banner in section "<Section>"
	Examples:
		| Code | Patient  | Section                                    |
		| ADM  | patient1 | Administrative Items                       |
		| CLI  | patient1 | Clinical Items                             |
		| ENC  | patient1 | Encounters                                 |
		| SUM  | patient1 | Active Problems and Issues                 |
		| SUM  | patient1 | Current Medication Issues                  |
		| SUM  | patient1 | Current Repeat Medications                 |
		| SUM  | patient1 | Current Allergies and Adverse Reactions    |
		| SUM  | patient1 | Last 3 Encounters                          |
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

Scenario Outline: Check html for non html formatting
	Given I am using the default server
		And I am performing the "urn:nhs:names:services:gpconnect:fhir:operation:gpc.getcarerecord" interaction
		And I author a request for the "<Code>" care record section for config patient "patient2"
	When I request the FHIR "gpc.getcarerecord" Patient Type operation
	Then the response status code should indicate success
		And the response body should be FHIR JSON
		And the JSON response should be a Bundle resource
		And the html should not contain "\n"
		And the html should not contain "\r"
		And the html should not contain "\t"
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

@ignore
@Manual
Scenario: Patients flag as sensitive should return any information within the HTML which may allow for identification of contact information or address

@ignore
@Manual
Scenario: Check dates are in decending order within the results tables

@ignore
@Manual
Scenario: System does not support section html response where appropriate