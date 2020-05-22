@patient @1.2.6-Full-Pack

Feature: PatientRegister
Scenario Outline: Register patient send request to incorrect URL
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the request URL to "<url>"
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should indicate failure
		And the response should be a OperationOutcome resource
	Examples:
		| StartDate		| url                            |
		| 2017-05-05	| Patient/$gpc.registerpatien    |
		| 1999-01-22	| Patient/$gpc.registerpati#ent  |

Scenario: Register patient without sending identifier within patient
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I remove the Identifiers from the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "INVALID_NHS_NUMBER"

@1.2.3
Scenario: Register patient without gender element
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I remove the Gender from the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response meta profile should be for "searchset"
		And the response bundle should contain a single Patient resource
		And the Patient Metadata should be valid
		And the Patient Id should be valid
		And the Patient Nhs Number Identifer should be valid
		And the Patient Registration Details Extension should be valid
		And the Patient Demographics should match the Stored Patient

Scenario: Register patient without date of birth element
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I remove the Birth Date from the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Register patient with an invalid NHS number
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I remove the Identifiers from the Stored Patient
		And I add an Identifier with Value "<nhsNumber>" to the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "INVALID_NHS_NUMBER"
	Examples:
		| nhsNumber   |
		| 34555##4    |
		| hello       |
		| 999999999   |
		| 9000000008  |
		| 90000000090 |

Scenario Outline: Register Patient and use the Accept Header to request response format
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the request content type to "<ContentType>"
		And I set the Accept header to "<ContentType>"
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response meta profile should be for "searchset"
		And the response bundle should contain a single Patient resource
		And the Patient Id should be valid
		And the Patient Metadata should be valid
		And the Patient Nhs Number Identifer should be valid
		And the Patient Registration Details Extension should be valid
		And the Patient Demographics should match the Stored Patient
		And the required cacheing headers should be present in the response
	Examples:
		| ContentType           | ResponseFormat |
		| application/fhir+xml  | XML            |
		| application/fhir+json | JSON           |

Scenario Outline: Register Patient and use the _format parameter to request the response format
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the request content type to "<ContentType>"
		And I add a Format parameter with the Value "<ContentType>"
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response meta profile should be for "searchset"
		And the response bundle should contain a single Patient resource
		And the Patient Metadata should be valid
		And the Patient Nhs Number Identifer should be valid
		And the Patient Registration Details Extension should be valid
		And the Patient Demographics should match the Stored Patient
	Examples:
		| ContentType           | ResponseFormat |
		| application/fhir+xml  | XML            |
		| application/fhir+json | JSON           |

Scenario Outline: Register Patient and use both the Accept header and _format parameter to request the response format
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I add a generic Identifier to the Stored Patient
		And I set the request content type to "<ContentType>"
		And I set the Accept header to "<AcceptHeader>"
		And I add a Format parameter with the Value "<Format>"
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response should be the format FHIR <ResponseFormat>
		And the response should be a Bundle resource of type "searchset"
		And the response meta profile should be for "searchset"
		And the response bundle should contain a single Patient resource
		And the Patient Metadata should be valid
		And the Patient Nhs Number Identifer should be valid
		And the Patient Registration Details Extension should be valid
		And the Patient Demographics should match the Stored Patient
		And the Patient Optional Elements should be valid
		And the Patient Link should be valid and resolvable
	Examples:
		| ContentType           | AcceptHeader          | Format                | ResponseFormat |
		| application/fhir+xml  | application/fhir+xml  | application/fhir+xml  | XML            |
		| application/fhir+xml  | application/fhir+xml  | application/fhir+json | JSON           |
		| application/fhir+json | application/fhir+json | application/fhir+xml  | XML            |
		| application/fhir+xml  | application/fhir+json | application/fhir+json | JSON           |
		| application/fhir+json | application/fhir+xml  | application/fhir+xml  | XML            |
		| application/fhir+json | application/fhir+json | application/fhir+json | JSON           |
		| application/fhir+xml  | application/fhir+json | application/fhir+xml  | XML            |
		| application/fhir+json | application/fhir+xml  | application/fhir+json | JSON           |

Scenario: Register patient with invalid bundle resource type
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request with invalid Resource type
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario: Register patient with invalid patient resource type
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request with invalid parameter Resource type
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"
	
@1.2.3
Scenario: Register patient with invalid patient resource with additional element
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request with additional field in parameter Resource
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"
			
Scenario: Register patient with duplicate patient resource parameters
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
			
@1.2.3
Scenario: Register patient1 which already exists on the system as a normal patient
	Given I configure the default "PatientSearch" request
		And I add a Patient Identifier parameter with default System and Value "patient1"
		When I make the "PatientSearch" request
		Then the response should be a Bundle resource of type "searchset"
		And the response bundle should contain a single Patient resource
		Then  I store the patient in the register patient resource format
	Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
		When I make the "RegisterPatient" request
		Then the response status code should be "409"
		And the response should be a OperationOutcome resource with error code "DUPLICATE_REJECTED"
		
Scenario: Register patient which already exists on the system as a temporary patient
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response bundle should contain a single Patient resource
	Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "409"
		And the response should be a OperationOutcome resource with error code "DUPLICATE_REJECTED"

Scenario: Register patient which is not the Spine
	Given I create a Patient which does not exist on PDS and store it
	Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
# Changed from BAD_REQUEST to 'INVALID_PATIENT_DEMOGRAPHICS' git hub ref 80
# RMB 9/10/2018 
		And the response should be a OperationOutcome resource with error code "INVALID_PATIENT_DEMOGRAPHICS"

Scenario: Register patient with no official name
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I remove the Official Name from the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Register Patient with multiple given names
	Given I get the next Patient to register and store it
    Given I configure the default "RegisterPatient" request
        And I add "<ExtraGivenNames>" Given Names to the Stored Patient Official Name
        And I add the Stored Patient as a parameter
    When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response meta profile should be for "searchset"
		And the Patient Demographics should match the Stored Patient
	Examples: 
		| ExtraGivenNames |
		| 1               |
		| 2               |
		| 5               |

Scenario: Register patient no family names
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I remove the Family Name from the Active Given Name for the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario: Register patient containing identifier without mandatory system elements
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I add an Identifier with missing System to the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

Scenario Outline: Register patient with additional valid elements
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
		And I add a <ElementToAdd> element to the Stored Patient
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response meta profile should be for "searchset"
		And the response bundle should contain a single Patient resource
		And the Patient Metadata should be valid
		And the Patient Nhs Number Identifer should be valid
		And the Patient Registration Details Extension should be valid
		And the Patient Demographics should match the Stored Patient
	Examples:
		| ElementToAdd |
#		| Active       |
		| Address      |
		| Name         |
#		| Births        |
#		| CareProvider  |
#		| Contact       |
#		| ManagingOrg   |
#		| Marital       |
		| Telecom       |

# github ref 136
# RMB 1/11/2018
Scenario Outline: Register patient with invalid additional valid elements
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
	And I add the Stored Patient as a parameter
	And I add a <ElementToAdd> element to the Stored Patient
	When I make the "RegisterPatient" request
	Then the response status code should be "422"
	And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"
	Examples:
		| ElementToAdd |
		| Active       |
		| Births        |
		| CareProvider  |
		| Contact       |
		| ManagingOrg   |
		| Marital       |

Scenario: Register patient with Address and Telecom
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
		And I add a Address element to the Stored Patient
		And I add a Telecom element to the Stored Patient
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the Patient should has a correct Address
		And the Patient should has a correct Telecom

# github ref 138
# RMB 12/11/2018
# github ref 180
# RMB 6/2/19

Scenario: Register patient with Multiple Address not allowed and Telecom
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
		And I add a Address element to the Stored Patient
		And I add a Address element to the Stored Patient
		And I add a Telecom element to the Stored Patient
	When I make the "RegisterPatient" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

Scenario Outline: Register patient with Address and Valid Telecom elements
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
		And I add a Address element to the Stored Patient
		And I add a Telecom element use "<Use1>" to the Stored Patient
		And I add a Telecom element use "<Use2>" to the Stored Patient
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the Patient should has a correct Address
		And the Patient should has a correct Telecom
	Examples:		
		| Use1   | Use2   |
		| Home   | Work   |
		| Home   | Mobile |
		| Home   | Temp   |
		| Home   | Email  |
		| Work   | Home   |
		| Work   | Mobile |
		| Work   | Temp   |
		| Work   | Email  |
		| Mobile | Home   |
		| Mobile | Work   |
		| Mobile | Temp   |
		| Mobile | Email  |
		| Temp   | Home   |
		| Temp   | Work   |
		| Temp   | Mobile |
		| Temp   | Email  |
		| Email  | Home   |
		| Email  | Work   |
		| Email  | Mobile |
		| Email  | Temp   |

Scenario Outline: Register patient with Address and Invalid Telecom elements
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
		And I add a Address element to the Stored Patient
		And I add a Telecom element use "<Use1>" to the Stored Patient
		And I add a Telecom element use "<Use2>" to the Stored Patient
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:		
		| Use1   | Use2   |
		| Home   | Home   |
		| Work   | Work   |
		| Mobile | Mobile |
		| Temp   | Temp   |
		| Email  | Email  |
	
Scenario: Register patient with Address Telecom and nhsCommunication
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
		And I add a Address element to the Stored Patient
		And I add a Telecom element to the Stored Patient
		And I add nhsCommunication extension to the Stored Patient
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the Patient should has a correct Address
		And the Patient should has a correct Telecom
		And the Patient Optional Elements should be valid
# git hub ref 153
# RMB 8/1/19
		And the Patient NhsCommunicationExtension should be valid

Scenario: Register patient with Address Telecom and multiple nhsCommunication
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
		And I add a Address element to the Stored Patient
		And I add a Telecom element to the Stored Patient
		And I add nhsCommunication extension to the Stored Patient
		And I add nhsCommunication extension to the Stored Patient
	When I make the "RegisterPatient" request
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"

#Scenario Outline: Register patient without temp should error
#	Given I get the next Patient to register and store it
#	Given I configure the default "RegisterPatient" request
#		And I add the Stored Patient as a parameter
#		And I add a <ElementToAdd> element without temp to the Stored Patient
#	When I make the "RegisterPatient" request
#	Then the response status code should indicate failure
#		And the Patient should has a <ElementToAdd> error
#	Examples:
#		| ElementToAdd |
#		| Telecom      |
#		| Address      |

Scenario Outline: Register patient with additional not allowed elements
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
	And I add the Stored Patient as a parameter
	And I add a <ElementToAdd> element to the Stored Patient
	When I make the "RegisterPatient" request
	Then the response status code should be "422"
	And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"
	Examples:
		| ElementToAdd  |
		| Animal        |
		| Communication |
		| Photo         |
		| Deceased      |

@1.2.3
Scenario Outline: Register patient setting JWT request type to invalid type
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I set the JWT requested scope to "<JWTType>"
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
	Examples:
		| JWTType              |
		| patient/*.read       |
		| organization/*.read  |

Scenario:Register patient invalid response check caching headers exist
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I remove the Identifiers from the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "INVALID_NHS_NUMBER"
		And the required cacheing headers should be present in the response

@1.2.3
Scenario: Register patient and check preferred branch 
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the response should be a Bundle resource of type "searchset"
		And the response meta profile should be for "searchset"
		And the response bundle should contain a single Patient resource
		And the Patient Registration Details Extension should be valid

# github ref 111
# RMB 23/10/2018		
Scenario: Register patient with family name not matching PDS
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I change the Family Name from the Stored Patient
		And I add the Stored Patient as a parameter
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"

# github ref 111
# RMB 23/10/2018		
#Scenario: Register patient with gender not matching PDS
#	Given I get the next Patient to register and store it
#	Given I configure the default "RegisterPatient" request
#		And I change the Gender from the Stored Patient
#		And I add the Stored Patient as a parameter
#	When I make the "RegisterPatient" request
#	Then the response status code should be "400"
#		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
		
# github ref 111
# RMB 23/10/2018		
#Scenario: Register patient with birth date not matching PDS
#	Given I get the next Patient to register and store it
#	Given I configure the default "RegisterPatient" request
#		And I change the Birth Date from the Stored Patient
#		And I add the Stored Patient as a parameter
#	When I make the "RegisterPatient" request
#	Then the response status code should be "400"
#		And the response should be a OperationOutcome resource with error code "BAD_REQUEST"
#
# github demonstrator ref 128
# RMB 29/10/2018		
	Scenario: Register deceased patient
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
		And I change the NHSNo from the Stored Patient "patient18"
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "INVALID_PATIENT_DEMOGRAPHICS"

	Scenario: Register sensitive patient
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
		And I change the NHSNo from the Stored Patient "patient9"
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "INVALID_PATIENT_DEMOGRAPHICS"

	Scenario: Register superseded patient
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
		And I change the NHSNo from the Stored Patient "patient11"
	When I make the "RegisterPatient" request
	Then the response status code should be "400"
		And the response should be a OperationOutcome resource with error code "INVALID_NHS_NUMBER"

# git hub ref 154
# RMB 8/1/19
# git hub ref 180 removed test
# and replaced with multiple address Use Types
#Scenario: Register patient with Multiple Address and Telecom resources
#	Given I get the next Patient to register and store it
#	Given I configure the default "RegisterPatient" request
#		And I add the Stored Patient as a parameter
#		And I add a Address element to the Stored Patient
#		And I add a Address element without temp to the Stored Patient
#		And I add a Telecom element to the Stored Patient
#	When I make the "RegisterPatient" request
#	Then the response status code should indicate success
#		And the Patient should has a correct Address
#		And the Patient should has a correct Telecom

# git hub ref 180
# RMB 4/2/19
@1.2.3
Scenario Outline: Register patient with Multiple Address Use types
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
		And I add a Address element to the Stored Patient with Use "<Use1>"
		And I add a Address element to the Stored Patient with Use "<Use2>"
	When I make the "RegisterPatient" request
	Then the response status code should indicate success
		And the Patient should has a correct Address
	Examples:		
		| Use1 | Use2 |
		| Home | Temp |
		| Temp | Home |

# git hub ref 180
# RMB 4/2/19
Scenario Outline: Register patient with Invalid Multiple Address Use types
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
		And I add a Address element to the Stored Patient with Use "<Use1>"
		And I add a Address element to the Stored Patient with Use "<Use2>"
	When I make the "RegisterPatient" request
	Then the response status code should indicate failure
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"
	Examples:		
		| Use1 | Use2 |
		| Home | Work |
		| Home | Home |
		| Home | Old  |
		| Old  | Work |
		| Old  | Old  |
		| Old  | Temp |
		| Temp | Work |
		| Temp | Temp |
		| Temp | Old  |

# git hub ref 180
# RMB 4/2/19
Scenario Outline: Register patient with Invalid Address Use type
	Given I get the next Patient to register and store it
	Given I configure the default "RegisterPatient" request
		And I add the Stored Patient as a parameter
		And I add a Address element to the Stored Patient with Use "<Use1>"
	When I make the "RegisterPatient" request
	Then the response status code should indicate failure
	Then the response status code should be "422"
		And the response should be a OperationOutcome resource with error code "INVALID_RESOURCE"
	Examples:		
		| Use1 |
		| Work |
		| Old  |
