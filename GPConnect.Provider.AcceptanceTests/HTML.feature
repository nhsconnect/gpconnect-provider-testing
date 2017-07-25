@html
Feature: HTML

Scenario Outline: HTML should not contain disallowed elements
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "<Code>"
		And I set the JWT Requested Record to the NHS Number for "patient2"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "document"
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
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "<Patient>"		
		And I add a Record Section parameter for "<Code>"
		And I set the JWT Requested Record to the NHS Number for "<Patient>"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "document"
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
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "<Code>"
		And I set the JWT Requested Record to the NHS Number for "patient2"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "document"
		And the html should contain table headers in coma seperated list order "<Headers>" for the "<PageSectionIndex>"
	Examples:
		| Code     | Headers                                                                                  | PageSectionIndex |
		| ADM      | Date,Entry,Details                                                                       | 1                |
		| ALL      | Start Date,Details                                                                       | 1                |
		| ALL      | Start Date,End Date,Details                                                              | 2                |
		| CLI      | Date,Entry,Details                                                                       | 1                |
		| ENC      | Date,Title,Details                                                                       | 1                |
		| IMM      | Date,Vaccination,Part,Contents,Details                                                   | 1                |
#        | INV      |                                                                                          |                  |
		| MED      | Start Date,Medication Item,Type,Scheduled End,Days Duration,Details                      | 1                |
		| MED      | Last Issued,Medication Item,Start Date,Review Date,Number Issued,Max Issues,Details      | 2                |
		| MED      | Start Date,Medication Item,Type,Last Issued,Review Date,Number Issued,Max Issues,Details | 3                |
		| OBS      | Date,Entry,Value,Details                                                                 | 1                |
#        | PAT      |                                                                                          |                  |
		| PRB      | Start Date,Entry,Significance,Details                                                    | 1                |
		| PRB      | Start Date,End Date,Entry,Significance,Details                                           | 2                |
		| REF      | Date,From,To,Priority,Details                                                            | 1                |
		| SUM      | Start Date,Entry,Significance,Details                                                    | 1				|
		| SUM      | Start Date,Medication Item,Type,Scheduled End,Days Duration,Details                      | 2				|
		| SUM      | Last Issued,Medication Item,Start Date,Review Date,Number Issued,Max Issues,Details      | 3				|
		| SUM      | Start Date,Details                                                                       | 4				|
		| SUM      | Date,Title,Details                                                                       | 5				|

Scenario Outline: filtered sections should contain date range section banner
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "<Patient>"		
		And I add a Record Section parameter for "<Code>"
		And I add a Time Period parameter with "<StartDateTime>" and "<EndDateTime>"
		And I set the JWT Requested Record to the NHS Number for "<Patient>"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "document"
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
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "<Patient>"		
		And I add a Record Section parameter for "<Code>"
		And I set the JWT Requested Record to the NHS Number for "<Patient>"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "document"
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
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "SUM"
		And I set the JWT Requested Record to the NHS Number for "patient2"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "document"
		And the response html for "Last 3 Encounters" section should contain a table with "3" rows
	
Scenario: Encounters section should contain all encounters
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "ENC"
		And I set the JWT Requested Record to the NHS Number for "patient2"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "document"
		And the response html for "Encounters" section should contain a table with at least "4" rows

Scenario Outline: filtered sections should return no data available html banner
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "<Patient>"		
		And I add a Record Section parameter for "<Code>"
		And I add a Time Period parameter with "<StartDateTime>" and "<EndDateTime>"
		And I set the JWT Requested Record to the NHS Number for "<Patient>"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "document"
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
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "patient1"		
		And I add a Record Section parameter for "<Code>"
		And I set the JWT Requested Record to the NHS Number for "patient1"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "document"
		And the response html should contain the all data items text
		And the response html should contain the no data available html banner in section "<Section>"
	Examples:
		| Code | Section                                    |
		| ADM  | Administrative Items                       |
		| CLI  | Clinical Items                             |
		| ENC  | Encounters                                 |
		| SUM  | Active Problems and Issues                 |
		| SUM  | Current Medication Issues                  |
		| SUM  | Current Repeat Medications                 |
		| SUM  | Current Allergies and Adverse Reactions    |
		| SUM  | Last 3 Encounters                          |
		| REF  | Referrals                                  |
		| ALL  | Current Allergies and Adverse Reactions    |
		| ALL  | Historical Allergies and Adverse Reactions |
		| IMM  | Immunisations                              |
		| MED  | Current Medication Issues                  |
		| MED  | Current Repeat Medications                 |
		| MED  | Past Medications                           |
		| OBS  | Observations                               |
		| PRB  | Active Problems and Issues                 |
		| PRB  | Inactive Problems and Issues               |
	#	| INV ||||||
	#	| PAT ||||||

Scenario Outline: Check html for non html formatting
	Given I configure the default "GpcGetCareRecord" request
		And I add an NHS Number parameter for "patient2"		
		And I add a Record Section parameter for "<Code>"
		And I set the JWT Requested Record to the NHS Number for "patient2"
	When I make the "GpcGetCareRecord" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "document"
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